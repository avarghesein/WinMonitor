
#define MADE_ERROR -1

class CWinMonitorClientBase:public CGeneralMemoryUtility 
{
	protected:
		CTcpNwMonitorConnection m_TcpConnection;
		bool m_blnConnectionOk;
		HANDLE m_muxSync;

	public:
		CWinMonitorClientBase(CTcpNwMonitorConnection *RefConnection=0);
		~CWinMonitorClientBase();
		bool SetConnection(CTcpNwMonitorConnection Connection);
		bool SynchronizeServer(bool FromServer=true);
		bool Authenticate(TCHAR *PassWord=0);
		bool ShutDown(void);
		bool RegisterType(BYTE ServiceType);
};


class CChatClient:public CWinMonitorClientBase
{
	protected:
		 bool SendStringGeneric(TCHAR *srcString,BYTE ServiceType);
		 bool ReceieveStringGeneric(TCHAR *dstString,UINT32 &strLen,BYTE ServiceType);

	public:
		 CChatClient(CTcpNwMonitorConnection *RefConnection=0);
		 ~CChatClient(){};
		 bool SendChatString(TCHAR *srcString);
		 bool ReceieveChatString(TCHAR *dstString,UINT32 &strLen);
		 bool ReceieveOnLineKeyLogString(TCHAR *dstString,UINT32 &strLen);
};

class CWinMonitorMessenger:public CWinMonitorClientBase
{	
	public:
		CWinMonitorMessenger(CTcpNwMonitorConnection *RefConnection=0);
		~CWinMonitorMessenger() {}
		bool ExecuteCommandMonitorClient(CCommandMonitorClient *CommandMonitorClient);
		bool ExecuteScreenMonitorMessenger(CScreenMonitorMessenger *Messenger,CScreenMonitorBase *ScreenMonitor); 
};

//------------------------------------------------------------------------//

bool CWinMonitorClientBase::ShutDown(void)
{
	WinMonitorNetworkServiceHeader Header;
	UINT32 uint32BytesSend=0;
	try
	{
		//WaitForSingleObject(m_muxSync,INFINITE); 
		Header.m_uint32Length=0;Header.m_bytMsgType=WIN_MONITOR_SHUTDOWN_CONNECTION;
		m_TcpConnection.SendByteStream((char*)&Header,sizeof(WinMonitorNetworkServiceHeader),uint32BytesSend);
		m_TcpConnection.Disconnect(); 
		//ReleaseMutex(m_muxSync); 
		return true;
	}catch(...) { /*ReleaseMutex(m_muxSync);*/ return false; }
}


bool CWinMonitorClientBase::SynchronizeServer(bool FromServer)
{
	m_blnConnectionOk=false;
	try
	{
		WinMonitorNetworkServiceHeader Header;
		UINT32 uint32BytesReceivedSend;

		WaitForSingleObject(m_muxSync,INFINITE); 

		if(FromServer)
		{
			while(!m_TcpConnection.ReceiveByteStream((char*)&Header,sizeof(WinMonitorNetworkServiceHeader),uint32BytesReceivedSend));
			if(Header.m_bytMsgType!=WIN_MONITOR_KEEP_ALIVE) throw MADE_ERROR;
		}
		else
		{
			Header.m_uint32Length=0, Header.m_bytMsgType=WIN_MONITOR_KEEP_ALIVE;
			while(!m_TcpConnection.SendByteStream((char*)&Header,sizeof(WinMonitorNetworkServiceHeader),uint32BytesReceivedSend));
		}
		ReleaseMutex(m_muxSync);

		return m_blnConnectionOk=true;
	}
	catch(...) {	m_TcpConnection.Disconnect(); ReleaseMutex(m_muxSync); return false;  }
}

bool CWinMonitorClientBase::RegisterType(BYTE ServiceType)
{
		WinMonitorNetworkServiceHeader Header;
		UINT32 uint32BytesSend, uint32BytesReceived;

		m_blnConnectionOk=false;

		try
		{
			WaitForSingleObject(m_muxSync,INFINITE); 
			while(!m_TcpConnection.ReceiveByteStream((char*)&Header,sizeof(WinMonitorNetworkServiceHeader),uint32BytesReceived));
			if(Header.m_bytMsgType!=WIN_MONITOR_GET_CLIENTTYPE) throw MADE_ERROR;
			Header.m_bytMsgType=ServiceType;
			Header.m_uint32Length=0;
			while(!m_TcpConnection.SendByteStream((char*)&Header,sizeof(WinMonitorNetworkServiceHeader),uint32BytesSend));
			while(!m_TcpConnection.ReceiveByteStream((char*)&Header,sizeof(WinMonitorNetworkServiceHeader),uint32BytesReceived));
			if(Header.m_bytMsgType!=WIN_MONITOR_CLIENT_ACCEPTED) throw MADE_ERROR;
	
			ReleaseMutex(m_muxSync);
			return m_blnConnectionOk=true;
		}
		catch(...) { m_TcpConnection.Disconnect(); ReleaseMutex(m_muxSync); return false;  }

}

bool CWinMonitorClientBase::Authenticate(TCHAR *PassWord)
{
	m_blnConnectionOk=false;
	try
	{
		WinMonitorNetworkServiceHeader Header;
		UINT32 uint32BytesSend, uint32BytesReceived;
		WaitForSingleObject(m_muxSync,INFINITE); 
		while(!m_TcpConnection.ReceiveByteStream((char*)&Header,sizeof(WinMonitorNetworkServiceHeader),uint32BytesReceived));
		if(Header.m_bytMsgType!=WIN_MONITOR_GET_PWD) throw MADE_ERROR;

		Header.m_bytMsgType=WIN_MONITOR_GOT_PWD;
		Header.m_uint32Length=(UINT32)_tcslen(PassWord);
		while(!m_TcpConnection.SendByteStream((char*)&Header,sizeof(WinMonitorNetworkServiceHeader),uint32BytesSend));
		while(!m_TcpConnection.SendByteStream((char*)PassWord,Header.m_uint32Length*sizeof(TCHAR),uint32BytesSend)); 

		while(!m_TcpConnection.ReceiveByteStream((char*)&Header,sizeof(WinMonitorNetworkServiceHeader),uint32BytesReceived));
		if(Header.m_bytMsgType!=WIN_MONITOR_PWD_OK) throw MADE_ERROR;

		ReleaseMutex(m_muxSync);
		return m_blnConnectionOk=true;
	}
	catch(...) { m_TcpConnection.Disconnect(); ReleaseMutex(m_muxSync); return false;  }

}

CWinMonitorClientBase::CWinMonitorClientBase(CTcpNwMonitorConnection *RefConnection)
{
	if(RefConnection) SetConnection(*RefConnection);
	else m_blnConnectionOk=false;
	m_muxSync=CreateMutex(0,0,0);
}

CWinMonitorClientBase::~CWinMonitorClientBase()
{
	CloseHandle(m_muxSync);m_muxSync=0;
}

bool CWinMonitorClientBase::SetConnection(CTcpNwMonitorConnection Connection)
{
	WaitForSingleObject(m_muxSync,INFINITE); 
	try { m_TcpConnection=Connection; m_blnConnectionOk=true; }
	catch(...) { m_blnConnectionOk=false; }
	ReleaseMutex(m_muxSync);
	return m_blnConnectionOk;
}


//-------------------------------------------------------------------------------//

CWinMonitorMessenger::CWinMonitorMessenger(CTcpNwMonitorConnection *RefConnection):CWinMonitorClientBase(RefConnection) {}

bool CWinMonitorMessenger::ExecuteScreenMonitorMessenger(CScreenMonitorMessenger *Messenger,CScreenMonitorBase *ScreenMonitor)
{
	if(!m_blnConnectionOk) return false;
	BYTE *Stream=0;
	try
	{
		WinMonitorNetworkServiceHeader Header;
		UINT32 uint32BytesSend, uint32BytesReceived;
		Header.m_bytMsgType=WIN_MONITOR_SCREEN_MONITOR;
		Header.m_uint32Length=Messenger->WriteToNetworkByteStream(Stream);
		WaitForSingleObject(m_muxSync,INFINITE); 
		*((WinMonitorNetworkServiceHeader*)Stream)=Header;
		if(!m_TcpConnection.SendByteStream((char*)Stream,sizeof(WinMonitorNetworkServiceHeader)+Header.m_uint32Length,uint32BytesSend)) throw MADE_ERROR;
		DeleteAll((void**)&Stream); 		
		if(!m_TcpConnection.ReceiveByteStream((char*)&Header,sizeof(WinMonitorNetworkServiceHeader),uint32BytesReceived)) throw MADE_ERROR;
		if(Header.m_bytMsgType!=WIN_MONITOR_SCREEN_MONITOR) throw MADE_ERROR;
		if(!AllocateMem((void**)&Stream,sizeof(BYTE)*Header.m_uint32Length)) throw MADE_ERROR;
		if(!m_TcpConnection.ReceiveByteStream((char*)Stream,Header.m_uint32Length,uint32BytesReceived)) throw MADE_ERROR;
		if(uint32BytesReceived!=Header.m_uint32Length) throw MADE_ERROR;
		if(ScreenMonitor->ReadFromNetworkByteStream(Stream)!=Header.m_uint32Length) throw MADE_ERROR; 
		ReleaseMutex(m_muxSync); 
		DeleteAll((void**)&Stream); 		
		return true;
	 }
	catch(...) 
	{ 
		DeleteAll((void**)&Stream); 
		m_TcpConnection.FlushAll();  
		ReleaseMutex(m_muxSync); 
		return false;
	} 		
}

bool CWinMonitorMessenger::ExecuteCommandMonitorClient(CCommandMonitorClient *CommandMonitorClient)
{
	if(!m_blnConnectionOk) return false;
	BYTE *Stream=0;
	try
	{
		WinMonitorNetworkServiceHeader Header;
		UINT32 uint32BytesSend=0, uint32BytesReceived=0;
		Header.m_bytMsgType=WIN_MONITOR_COMMAND_MONITOR; 
		Header.m_uint32Length=CommandMonitorClient->WriteToNetworkByteStream(Stream); 
		*((WinMonitorNetworkServiceHeader*)Stream)=Header;
		WaitForSingleObject(m_muxSync,INFINITE); 
		if(!m_TcpConnection.SendByteStream((char*)Stream,sizeof(WinMonitorNetworkServiceHeader)+Header.m_uint32Length,uint32BytesSend)) throw MADE_ERROR;
		if(sizeof(WinMonitorNetworkServiceHeader)+Header.m_uint32Length!=uint32BytesSend) throw MADE_ERROR;
		DeleteAll((void**)&Stream); 		
		if(!m_TcpConnection.ReceiveByteStream((char*)&Header,sizeof(WinMonitorNetworkServiceHeader),uint32BytesReceived)) throw MADE_ERROR;
		if(sizeof(WinMonitorNetworkServiceHeader)!=uint32BytesReceived)  throw MADE_ERROR;
		if(Header.m_bytMsgType!=WIN_MONITOR_COMMAND_MONITOR) throw MADE_ERROR;
		if(!AllocateMem((void**)&Stream,sizeof(BYTE)*Header.m_uint32Length)) throw MADE_ERROR;
		if(!m_TcpConnection.ReceiveByteStream((char*)Stream,Header.m_uint32Length,uint32BytesReceived)) throw MADE_ERROR;
		if(uint32BytesReceived!=Header.m_uint32Length) throw MADE_ERROR;
		if(CommandMonitorClient->ReadFromNetworkByteStream(Stream)!=Header.m_uint32Length) throw MADE_ERROR; 
		ReleaseMutex(m_muxSync); 
		DeleteAll((void**)&Stream); 	
		return true;
	}
	catch(...) { DeleteAll((void**)&Stream); m_TcpConnection.FlushAll(); ReleaseMutex(m_muxSync); return false; } 		
}

//-----------------------------------------------------------------------------------//

CChatClient::CChatClient(CTcpNwMonitorConnection *RefConnection):CWinMonitorClientBase(RefConnection) {}

bool CChatClient::SendStringGeneric(TCHAR *srcString,BYTE ServiceType)
{
	WinMonitorNetworkServiceHeader Header;
	UINT32 uint32BytesSend=0,uint32BytesSended=0;
	CTcpNwMonitorConnection *RefCon=new CTcpNwMonitorConnection(m_TcpConnection); 
	Header.m_bytMsgType=ServiceType; 
	Header.m_uint32Length=(_tcslen(srcString)+1)*sizeof(TCHAR);
	BYTE *strData=0;
	if(!CopyBytes(&strData,(BYTE*)srcString,Header.m_uint32Length)) return false; 
	try
	{
		//WaitForSingleObject(m_muxSync,INFINITE); 
		if(!(RefCon->SendByteStream((char*)&Header,sizeof(WinMonitorNetworkServiceHeader),uint32BytesSended))) throw MADE_ERROR;
		if(!(RefCon->SendByteStream((char*)strData,Header.m_uint32Length,uint32BytesSended))) throw MADE_ERROR;
		DeleteAll((void**)&strData);
		if(RefCon) { delete RefCon; RefCon=0; }
		//ReleaseMutex(m_muxSync);
		return true;
	}
	catch(...) 
	{ /*ReleaseMutex(m_muxSync);*/ 
		DeleteAll((void**)&strData);
		if(RefCon) delete RefCon; 
		return false;
	}
}

bool CChatClient::SendChatString(TCHAR *srcString)
{
	return SendStringGeneric(srcString,WIN_MONITOR_CHAT_CLIENTSERVER);
}

bool CChatClient::ReceieveStringGeneric(TCHAR *dstString,UINT32 &strLen,BYTE ServiceType)
{
	 try
	{
		static TCHAR *tchrPtrDstStr;
		bool blnOk=true;
		if(!strLen) 
		{
			strLen=0;
			DeleteAll((void**)&tchrPtrDstStr);
			WinMonitorNetworkServiceHeader Header;
			UINT32 uint32BytesRecv=0,uint32BytesRecvd=0;
			bool blnSockError;
			//WaitForSingleObject(m_muxSync,INFINITE); 
			try
			{
				if(m_TcpConnection.IsEmptyReceiveBuffer(blnSockError)) throw -1; 
				if(!m_TcpConnection.ReceiveByteStream((char*)&Header,sizeof(WinMonitorNetworkServiceHeader),uint32BytesRecvd)) throw -1;
				if(Header.m_bytMsgType!=ServiceType) throw -1;  
				TCHAR strRec[1001];
				if(!m_TcpConnection.ReceiveByteStream((char*)strRec,Header.m_uint32Length,uint32BytesRecvd)) throw -1;
				if(!CopyBytes((BYTE**)&tchrPtrDstStr,(BYTE*)strRec,Header.m_uint32Length)) throw -1;
				strLen=Header.m_uint32Length; 
				//ReleaseMutex(m_muxSync);
				return true;
			}
			catch(...)
			{  /*ReleaseMutex(m_muxSync); */DeleteAll((void**)&tchrPtrDstStr); return false;	}
		}
		else
		{
			if(!CopyBytes((BYTE*)dstString,(BYTE*)tchrPtrDstStr,strLen)) return false;
			DeleteAll((void**)&tchrPtrDstStr); 
			return true;
		}
	}
	catch(...)
	{
		return false;
	}
}

 bool CChatClient::ReceieveChatString(TCHAR *dstString,UINT32 &strLen)
 {
	 return ReceieveStringGeneric(dstString,strLen,WIN_MONITOR_CHAT_CLIENTSERVER);
 }

 bool CChatClient::ReceieveOnLineKeyLogString(TCHAR *dstString,UINT32 &strLen)
 {
	 return ReceieveStringGeneric(dstString,strLen,WIN_MONITOR_KEYLOG_CLIENTSERVER);
 }