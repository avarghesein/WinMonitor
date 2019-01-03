

DWORD   WINAPI    ChatThread(LPVOID PtrToCChatServer);
LRESULT CALLBACK  ChatWindowProc(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam);

class CChatServer:public CGeneralMemoryUtility,public CNetworkBasicUtility 
{
	protected:
		bool m_ValidServer;
		HWND m_ChatWnd;
		HANDLE m_ChatThread;
		static WORD m_wQuitMessage;

	public:
		static CExtendedLinkedList gbl_lnkdlst_ChatClients;
	
	protected:
		bool CreateChatWindow(void);
		HINSTANCE GetCurrentInstance(void) { return gbl_hinstCurrentInstance; }
		int GetShowType(void) { return gbl_intWndShowType; }  
		void Init(void);
			
	public:
		static bool ClearChatClients(void);
		bool ResetServiceThread(void);
		bool EndChatSession(void) { Init(); return true; }
		bool CloseChatWindow(void);
		HWND GetChatWindow(void) { return m_ChatWnd; }
		bool ServiceChatClient(CTcpNwMonitorConnection TcpConnection);
		static WORD GetExitMessage(void) { return m_wQuitMessage; }
        CChatServer(void);
	   ~CChatServer();
};

CExtendedLinkedList CChatServer::gbl_lnkdlst_ChatClients;

WORD CChatServer::m_wQuitMessage=(WORD)RegisterWindowMessage("Quit Chat Server Window..."); 

bool CChatServer::ClearChatClients(void)
{
	CChatServer *ChatSer;
	while(gbl_lnkdlst_ChatClients.DeleteFromFront((void**)&ChatSer))
	{
		ChatSer->EndChatSession();
		delete ChatSer;
	}
	return true;
}

bool CChatServer::ResetServiceThread(void) 
{ 
	WaitForMux(); 
		m_ChatThread=0;
	GiveUpMux();
	return true;
}

void CChatServer::Init(void)
{
	if(m_ChatThread)
	{
		try
		{
			DWORD ExitStatus;
			if(GetExitCodeThread(m_ChatThread,(LPDWORD)&ExitStatus))
				if(ExitStatus==STILL_ACTIVE) TerminateThread(m_ChatThread,0); 
		}
		catch(...) {}
		SendShutDownMessage();
		CreateNetworkMutex(); 
	}
	m_TcpConnection.Invalidate(); 
	m_ValidServer=false;
	m_ChatThread=0;
	if(m_ChatWnd) SendMessage(m_ChatWnd,m_wQuitMessage,0,0L); 
	m_ChatWnd=0;
	return;
}

bool CChatServer::CloseChatWindow(void)
{
	if(m_ChatWnd) SendMessage(m_ChatWnd,m_wQuitMessage,0,0L); 
	return true;
}

CChatServer::CChatServer(void):CGeneralMemoryUtility(),CNetworkBasicUtility()
{
	m_ChatThread=0;
	m_ChatWnd=0;
	Init();
}

CChatServer::~CChatServer()
{
	Init();
}

bool CChatServer::CreateChatWindow(void)
{
	if(m_ChatWnd) SendMessage(m_ChatWnd,m_wQuitMessage,0,0L); 
	m_ChatWnd= CreateDialog(GetCurrentInstance(),(LPCTSTR)IDD_DLGCHAT,0, (DLGPROC)ChatWindowProc);
	if(!m_ChatWnd) return false;
	if(!SetProp(m_ChatWnd,"ChatServerAddress",this)) return false; 
	ShowWindow(m_ChatWnd,GetShowType());
	return true;
}

bool CChatServer::ServiceChatClient(CTcpNwMonitorConnection TcpConnection)
{
	if(m_wQuitMessage==NULL) return false;
	Init();
	try { m_TcpConnection=TcpConnection; } catch(...){}
	if(!CreateChatWindow()) return false;
	m_ChatThread=CreateThread(NULL,NULL,(LPTHREAD_START_ROUTINE)&ChatThread,(LPVOID)this,CREATE_SUSPENDED,NULL);
	if(!m_ChatThread) return false;
	ResumeThread(m_ChatThread);
	gbl_lnkdlst_ChatClients.InsertAtBack((void*)this); 
	return true;
}


LRESULT CALLBACK ChatWindowProc(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam)
{
	static bool isClose=false;
	
	if(message==(UINT)CChatServer::GetExitMessage()) 
	{
		isClose=true;
		SendMessage(hDlg,WM_CLOSE,0,0L); 
		return 0;
	}

	CChatServer *ptrToChatServer=(CChatServer*)GetProp(hDlg,"ChatServerAddress");

	switch (message)
	{
	case WM_CLOSE:
		if(!isClose) return 0;
		else
		{
			DestroyWindow(hDlg); 
			return 0;
		}

	case WM_INITDIALOG:
		isClose=false;
	    SetWindowText(hDlg,"Hai from remote WinMonitorClient.1.0"); 
		return 0;

	case WM_KEYDOWN: return 0;

	case WM_COMMAND:
		 if (LOWORD(wParam) == IDC_EditServer)
		 {
			if(HIWORD(wParam)==EN_CHANGE)
			{	 	
				int NumLines=(int)SendMessage(GetDlgItem(hDlg,IDC_EditServer),EM_GETLINECOUNT,0,0L);
				if(NumLines>1)
				{
					TCHAR tchrLine[701];
					WinMonitorNetworkServiceHeader Header;
					UINT32 uint32BytesSended;
					try
					{
						_tcscpy(tchrLine,"User:>");
						Header.m_uint32Length=GetDlgItemText(hDlg,IDC_EditServer,(LPTSTR)(tchrLine+13),700)+1+13;
						tchrLine[Header.m_uint32Length-1]='\0';
						Header.m_bytMsgType=WIN_MONITOR_CHAT_CLIENTSERVER; 
						ptrToChatServer->GetConnection().SendByteStream((char*)&Header,(UINT32)sizeof(WinMonitorNetworkServiceHeader),uint32BytesSended); 
						ptrToChatServer->GetConnection().SendByteStream((char*)tchrLine,Header.m_uint32Length,uint32BytesSended); 
						SetDlgItemText(hDlg,IDC_EditServer,(LPTSTR)"");
					}
					catch(...) {}
					TCHAR tchrPrevLine[1001];
					try
					{
						ptrToChatServer->WaitForMux(); 
							if(GetDlgItemText(hDlg,IDC_EditClient,(LPTSTR)tchrPrevLine,1000)>=1000) 
								tchrPrevLine[0]=_T('\0');
							_tcscat(tchrPrevLine,tchrLine);
							SetDlgItemText(hDlg,IDC_EditClient,tchrPrevLine);
						ptrToChatServer->GiveUpMux(); 
					}
					catch(...) { ptrToChatServer->GiveUpMux(); }
				}
			}
		 }
		return 0;

	case WM_PAINT:
	default:
		return DefWindowProc(hDlg,message,wParam,lParam); 
	}
	
}

DWORD WINAPI ChatThread(LPVOID PtrToCChatServer)
{
	CChatServer *ChatServerRef=(CChatServer*)PtrToCChatServer;
	CTcpNwMonitorConnection ClientConnection=ChatServerRef->GetConnection();
	WinMonitorNetworkServiceHeader Header;
	UINT32 uint32BytesReceived;
	BYTE *bytDataStream=0;
	bool blnSockError;

	while(true)
	{
		Sleep(10);
		while(ClientConnection.IsEmptyReceiveBuffer( blnSockError)) 
			if(!blnSockError) Sleep(100); 
			else break;

		ClientConnection.ReceiveByteStream((char*)&Header,(UINT32)sizeof(WinMonitorNetworkServiceHeader),uint32BytesReceived);
		switch(Header.m_bytMsgType)
		{
			case WIN_MONITOR_SHUTDOWN_CONNECTION: 
				ChatServerRef->CloseChatWindow(); 
				ChatServerRef->SendShutDownMessage(); 
				ChatServerRef->ResetServiceThread(); 
				CChatServer::gbl_lnkdlst_ChatClients.DeleteItemByAddress((UINT64)ChatServerRef);
				delete ChatServerRef;
				return 0;
			
			case WIN_MONITOR_CHAT_CLIENTSERVER:
				if(Header.m_uint32Length)
				{
					try
					{	 
						while(!ChatServerRef->AllocateMem((void**)&bytDataStream,sizeof(BYTE)*Header.m_uint32Length));
						ClientConnection.ReceiveByteStream((char*)bytDataStream,Header.m_uint32Length,uint32BytesReceived); 
						TCHAR *strTran=new TCHAR[sizeof(TCHAR)*(Header.m_uint32Length+2)];
						ChatServerRef->CopyBytes((BYTE*)(strTran),bytDataStream,Header.m_uint32Length); 
						strTran[Header.m_uint32Length+1]='\0';
						TCHAR tchrPrevText[701];
						try
						{
							ChatServerRef->WaitForMux(); 
								if(GetDlgItemText(ChatServerRef->GetChatWindow(),IDC_EditClient,(LPTSTR)tchrPrevText,700)>=700)
								tchrPrevText[0]='\0';
								_tcscat(tchrPrevText,strTran);
								SetDlgItemText(ChatServerRef->GetChatWindow(),IDC_EditClient,(LPTSTR)tchrPrevText);
							ChatServerRef->GiveUpMux(); 
						}
						catch(...) { ChatServerRef->GiveUpMux(); }
    					ChatServerRef->DeleteAll((void**)&bytDataStream); 
						if(strTran) delete []strTran;
					}
					catch(...) {}
				}
				break;

			default:
				break;
		}
	}
}
		

		

