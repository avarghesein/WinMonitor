


class CTcpNwMonitorConnection:public CNetworkBase 
{
protected:
	 SOCKET m_socketDataSocket;
	 struct sockaddr_in m_sokadrinetRemoteAddr;
	 enum NW_ERROR m_enumErrCode;
	 bool m_boolErrorFlag;
	 HANDLE m_muxConnection;

protected:
	 void ReFresh(void);
	 bool SetError(enum NW_ERROR ErrCode); 
	 void ClearError(void)  { m_enumErrCode=NO_ERR;m_boolErrorFlag=0;  }
	 bool Connect(void);
	 bool SetUnBlocking(void);

public:
	 CTcpNwMonitorConnection();
	 CTcpNwMonitorConnection(SOCKET *m_socketDataSocket,struct sockaddr_in *m_sokadrinetRemoteAddr);
	 CTcpNwMonitorConnection(CTcpNwMonitorConnection &ClientToBeCloned);	
	 ~CTcpNwMonitorConnection();
	 CTcpNwMonitorConnection &operator=(CTcpNwMonitorConnection &ClientToBeCloned);
	 void SetSocket(SOCKET SocketCopy) { m_socketDataSocket=SocketCopy; ClearError(); }
	 void SetAddress(struct sockaddr_in SockAddress) { m_sokadrinetRemoteAddr=SockAddress; ClearError(); }
	 bool FlushAll(void);	
	 enum NW_ERROR LastError(void) {  return m_enumErrCode; }
	 bool ConnectTo(TCHAR *ServerIP,UINT ServerPort);
	 bool GetIPandPort(bool OfRemoteHost,TCHAR *IP,UINT32 &Port);
	 bool IsEmptyReceiveBuffer(bool &blnIsSockError);
	 bool SendByteStream(char *MessageBuffer,UINT32 NumberOfBytesToSend,UINT32 &NumberOfBytesSended);
	 bool ReceiveByteStream(char *MessageBuffer,UINT32 NumberOfBytesToReceive,UINT32 &NumberOfBytesReceived);
	 bool Disconnect(void);
	 bool Invalidate(void);
};

bool CTcpNwMonitorConnection::SetUnBlocking(void)
{
	/*
	int intOption=1;
	if(setsockopt(m_socketDataSocket,IPPROTO_TCP,SO_DONTLINGER,(const char*) &intOption,sizeof(int))==SOCKET_ERROR) 
	{
		return SetError(SOCKET_BIND_ERROR);
	}
	*/
	return true;
}

bool CTcpNwMonitorConnection::Invalidate(void)
{
	ReFresh();
	return true;
}

bool CTcpNwMonitorConnection::FlushAll(void)
{
	if(this->m_boolErrorFlag) return false; 
	return CNetworkBase::FlushAll(m_socketDataSocket);  
}

bool CTcpNwMonitorConnection::GetIPandPort(bool OfRemoteHost,TCHAR *IP,UINT32 &Port)
{
	 if(this->m_boolErrorFlag) return false; 
	 try
	 {
			struct sockaddr_in Host; int HostLen=sizeof(struct sockaddr_in); int SuccFlg;
			WaitForSingleObject(m_muxConnection,INFINITE); 
				 if(OfRemoteHost) SuccFlg=getpeername(m_socketDataSocket,(sockaddr*)&Host,&HostLen);
				 else SuccFlg=getsockname(m_socketDataSocket,(sockaddr*)&Host,&HostLen);
			ReleaseMutex(m_muxConnection); 
			if(SuccFlg==SOCKET_ERROR) return false;
			_tcscpy(IP,inet_ntoa(Host.sin_addr));    
			Port=Host.sin_port;
			return true;
	 }
	 catch(...)
	 {	 return false;  }
}


CTcpNwMonitorConnection::~CTcpNwMonitorConnection()
{
	 try{  CloseHandle(m_muxConnection);m_muxConnection=0;  }
	 catch(...) { m_muxConnection=0; }
}

bool CTcpNwMonitorConnection::Disconnect(void)
{
	 try
	 { 
			CloseHandle(m_muxConnection); m_muxConnection=0;
			if(!m_boolErrorFlag)
			{
				shutdown(m_socketDataSocket,SD_BOTH);
				closesocket(m_socketDataSocket);
			}
	 }
	 catch(...){	return SetError(CONNECTION_CLOSE_ERROR); }
	 return !SetError(CONNECTION_CLOSED); 
}

bool CTcpNwMonitorConnection::Connect(void)
{
	 try
	 {
			m_socketDataSocket=socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);
			if(m_socketDataSocket==INVALID_SOCKET) return SetError(NON_VALID_SOCKET) ;
			if(connect(m_socketDataSocket,(struct sockaddr*)&m_sokadrinetRemoteAddr,sizeof(struct sockaddr))==-1) return SetError(SERVER_INVALID);		
			SetUnBlocking();
			ClearError();
			return true;
	 }
	 catch(...) { return SetError(OTHER); }
}



bool  CTcpNwMonitorConnection::SetError(enum NW_ERROR ErrCode) 
{
	 this->m_enumErrCode=ErrCode;
	 m_boolErrorFlag=true;
	 return false;
}

bool CTcpNwMonitorConnection::ConnectTo(TCHAR *ServerIP,UINT ServerPort)
{
	 try
	 {
			this->ReFresh(); 
			m_sokadrinetRemoteAddr.sin_family=AF_INET;
			m_sokadrinetRemoteAddr.sin_port=htons(ServerPort);
 			m_sokadrinetRemoteAddr.sin_addr.S_un.S_addr=inet_addr(ServerIP);
			return Connect();
	 }
	 catch(...) { return SetError(OTHER);	 }
}

bool CTcpNwMonitorConnection::SendByteStream(char *MessageBuffer,UINT32 NumberOfBytesToSend,UINT32 &NumberOfBytesSended)
{
	 if(this->m_boolErrorFlag) return false; 
	 if(NumberOfBytesToSend==0) return true;
	 WaitForSingleObject(m_muxConnection,INFINITE); 
	 fd_set objWriteChk; 
	 objWriteChk.fd_count=1;
	 objWriteChk.fd_array[0]=m_socketDataSocket;
	 while(select(0,0,&objWriteChk,0,NULL)!=1);
	 UINT32 uint32PendingBytesToSend=NumberOfBytesToSend;
	 while(uint32PendingBytesToSend>0)
	 {
		if((NumberOfBytesSended=send(m_socketDataSocket,(char*)(MessageBuffer+(NumberOfBytesToSend-uint32PendingBytesToSend)),uint32PendingBytesToSend,0))==SOCKET_ERROR) 
		{
			NumberOfBytesSended=NumberOfBytesToSend-uint32PendingBytesToSend;
			ReleaseMutex(m_muxConnection); 
			return false;
		}
		uint32PendingBytesToSend-=NumberOfBytesSended;
		Sleep(500); 
	 }
	 NumberOfBytesSended=NumberOfBytesToSend-uint32PendingBytesToSend;
	 ReleaseMutex(m_muxConnection); 
	 return true;
}

bool CTcpNwMonitorConnection::IsEmptyReceiveBuffer(bool &blnIsSockError)
{
	blnIsSockError=false;
	char chrCh;
	int intPendingBytes=recv(m_socketDataSocket,&chrCh,1,MSG_PEEK);
	if(intPendingBytes==SOCKET_ERROR)
	{
		blnIsSockError=true;
		return true;
	}
    if(intPendingBytes<=0) return true;
	else return false;
}

bool CTcpNwMonitorConnection::ReceiveByteStream(char *MessageBuffer,UINT32 NumberOfBytesToReceive,UINT32 &NumberOfBytesReceived)
{
	 if(this->m_boolErrorFlag) return false; 
	 if(NumberOfBytesToReceive==0) return true;
	 WaitForSingleObject(m_muxConnection,INFINITE); 
	 fd_set objReadChk; 
	 objReadChk.fd_count=1;
	 objReadChk.fd_array[0]=m_socketDataSocket;
	 while(select(0,&objReadChk,0,0,NULL)!=1);

	 UINT32 uint32PendingBytesToReceive=NumberOfBytesToReceive;
	 while(uint32PendingBytesToReceive>0)
	 {
		if((NumberOfBytesReceived=recv(m_socketDataSocket,(char*)(MessageBuffer+(NumberOfBytesToReceive-uint32PendingBytesToReceive)),uint32PendingBytesToReceive,0))==SOCKET_ERROR) 
		{
				NumberOfBytesReceived=NumberOfBytesToReceive-uint32PendingBytesToReceive;
				ReleaseMutex(m_muxConnection); 
				return false;
		}
		uint32PendingBytesToReceive-=NumberOfBytesReceived;
		Sleep(500); 
	 }
	 ReleaseMutex(m_muxConnection); 
	 NumberOfBytesReceived=NumberOfBytesToReceive-uint32PendingBytesToReceive;
	 return true;
}


CTcpNwMonitorConnection &CTcpNwMonitorConnection::operator=(CTcpNwMonitorConnection &ClientToBeCloned)
{
	 this->ReFresh(); 
	 this->m_sokadrinetRemoteAddr=ClientToBeCloned.m_sokadrinetRemoteAddr;  
	 this->m_socketDataSocket=ClientToBeCloned.m_socketDataSocket;
	 this->m_boolErrorFlag=ClientToBeCloned.m_boolErrorFlag;
	 this->m_enumErrCode=ClientToBeCloned.m_enumErrCode;  
	 SetUnBlocking();
	 return *this;
}

CTcpNwMonitorConnection::CTcpNwMonitorConnection(SOCKET *m_socketDataSocket,struct sockaddr_in *m_sokadrinetRemoteAddr)
{	 
	 m_muxConnection=0;
	 this->ReFresh();
	 this->m_socketDataSocket=*m_socketDataSocket;	
	 this->m_sokadrinetRemoteAddr=*m_sokadrinetRemoteAddr;
	 SetUnBlocking();
	 ClearError();
}

CTcpNwMonitorConnection::CTcpNwMonitorConnection(CTcpNwMonitorConnection &ClientToBeCloned)
{
	 m_muxConnection=0; this->ReFresh(); 
	 this->m_socketDataSocket=ClientToBeCloned.m_socketDataSocket;
	 this->m_sokadrinetRemoteAddr=ClientToBeCloned.m_sokadrinetRemoteAddr; 
	 this->m_boolErrorFlag=ClientToBeCloned.m_boolErrorFlag;
	 this->m_enumErrCode=ClientToBeCloned.m_enumErrCode;  
 	 SetUnBlocking();
}

CTcpNwMonitorConnection::CTcpNwMonitorConnection()
{
	 m_muxConnection=0;
	 this->ReFresh();
}


void  CTcpNwMonitorConnection::ReFresh(void) 
{
	 try
	 {
			if(m_muxConnection==0) 
				 m_muxConnection=CreateMutex(0,0,0); 
	 
			this->m_socketDataSocket=-1;
			this->m_sokadrinetRemoteAddr.sin_addr.S_un.S_addr=-1;
			SetError(SERVER_INVALID);
			return;
	 }catch(...) { return; }
}


//-----------------------------------------------------------//


class CNetworkBasicUtility
{
	
   protected:
		HANDLE m_MuxNetworkBasicUtility;
		CTcpNwMonitorConnection m_TcpConnection; 

   protected:
    	bool SendAbnormalMessage(BYTE CmdType);
		bool FlushAll(void);
		bool CreateNetworkMutex(void);

	public:
		CNetworkBasicUtility(CTcpNwMonitorConnection TcpConnection);
		CNetworkBasicUtility(void);
	   ~CNetworkBasicUtility(void);
		bool SendShutDownMessage(void);
		bool SendNullCommand(void);
		bool WaitForMux(void);
		bool GiveUpMux(void);
		CTcpNwMonitorConnection GetConnection(void);
};

CTcpNwMonitorConnection CNetworkBasicUtility::GetConnection(void)
{
	return this->m_TcpConnection;
}

bool CNetworkBasicUtility::CreateNetworkMutex(void)
{
	try
	{ 
		if(m_MuxNetworkBasicUtility) CloseHandle(m_MuxNetworkBasicUtility);
	}
	catch(...) {}
	m_MuxNetworkBasicUtility=CreateMutex(0,false,0);
	if(m_MuxNetworkBasicUtility) return true; else return false;
}


bool CNetworkBasicUtility::GiveUpMux(void)
{
	if(!m_MuxNetworkBasicUtility) return false;
	ReleaseMutex(m_MuxNetworkBasicUtility); 
	return true;
}

bool CNetworkBasicUtility::WaitForMux(void)
{
	if(!m_MuxNetworkBasicUtility) return false;
	WaitForSingleObject(m_MuxNetworkBasicUtility,INFINITE);
	return true;
}

CNetworkBasicUtility::CNetworkBasicUtility(CTcpNwMonitorConnection TcpConnection)
{
	m_MuxNetworkBasicUtility=0; 
	m_TcpConnection=TcpConnection;
	CreateNetworkMutex();
}

CNetworkBasicUtility::CNetworkBasicUtility(void)
{
	m_MuxNetworkBasicUtility=0;  
	m_TcpConnection.Invalidate(); 
	CreateNetworkMutex();
}

CNetworkBasicUtility::~CNetworkBasicUtility()
{
	try
	{
		if(m_MuxNetworkBasicUtility) CloseHandle(m_MuxNetworkBasicUtility);
	}
	catch(...) 
	{}
}

bool CNetworkBasicUtility::FlushAll(void)
{
	return  m_TcpConnection.FlushAll(); 
}
bool CNetworkBasicUtility::SendNullCommand(void)
{
	return SendAbnormalMessage(WIN_MONITOR_NULL_COMMAND);
}

bool CNetworkBasicUtility::SendShutDownMessage(void)
{
	bool blnOk=SendAbnormalMessage(WIN_MONITOR_SHUTDOWN_CONNECTION);
	m_TcpConnection.Disconnect(); 
	return blnOk;
}

bool  CNetworkBasicUtility::SendAbnormalMessage(BYTE CmdType)
{
		try
		{
			WinMonitorNetworkServiceHeader Header;
			UINT32 uint32BytesSend;
			Header.m_uint32Length=0,Header.m_bytMsgType=CmdType;  
			if(!m_TcpConnection.SendByteStream((char*)&Header,(UINT32)sizeof(WinMonitorNetworkServiceHeader),uint32BytesSend)) return false; 
			return true;
		}
		catch(...) { return false; }
}
