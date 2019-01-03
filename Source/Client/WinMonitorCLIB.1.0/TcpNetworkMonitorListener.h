

#define SELECT_CLIENT 0
#define DELETE_CLIENT 1

UINT ThreadForAcceptClients(LPVOID ptrToListenerObject);

class CTcpNwMonitorListener
{
protected:
	 static const LPCSTR m_strLOCAL_HOST;
	 	 
protected:
	 volatile bool m_boolListenFlag;
	 bool m_boolErrorFlag;
	 enum NW_ERROR m_enumErrCode;
	 	 	 	 
	 struct sockaddr_in m_sokadrinetListenAddress;
	 SOCKET m_socketTcpSocket;

	 CExtendedLinkedList<void> *m_xtndlnkdlstptrConnectionSockQueue;
	 
	 HANDLE m_handletoAcceptClientThread;   
	 HANDLE m_muxListener;

protected:
	 bool Initialize(UINT uintIP,UINT Port=0);
	 bool SetError(enum NW_ERROR ErrCode); 
	 void ClearError(void)  { m_enumErrCode=NO_ERR; m_boolErrorFlag=0;  }  
	 bool ReFresh(void);
	 void Reset(void);
	 bool ExtractClient(CTcpNwMonitorConnection &Client,int How);
	 bool ShutDownAllClients(void);

public:
	 bool RegisterClient(CTcpNwMonitorConnection Client);
	 SOCKET GetListenerSocket(void);
	 ~CTcpNwMonitorListener();

public:

   CTcpNwMonitorListener(bool LocalHost,UINT Port=0);
	 CTcpNwMonitorListener(LPCSTR IpAddress,UINT Port=0);
	 CTcpNwMonitorListener(void);
	 
	 bool ListenAt(bool LocalHost,UINT Port=0);
	 bool ListenAt(LPCSTR IpAddress,UINT Port=0);

	 enum NW_ERROR LastError(void) {  return m_enumErrCode; }
	 bool SetListenState(bool On=true);
	 bool RetrieveClient(CTcpNwMonitorConnection &Client);
	 bool RemoveClient(CTcpNwMonitorConnection &Client);
	 bool GetListenFlag(void) {	return m_boolListenFlag; } 
	 bool DisconnectListenConnection(void);
};

const LPCSTR CTcpNwMonitorListener::m_strLOCAL_HOST="127.0.0.1";

bool CTcpNwMonitorListener::ShutDownAllClients(void)
{
	if(!m_xtndlnkdlstptrConnectionSockQueue) return true;
	if(m_handletoAcceptClientThread) SetListenState(false);

	CTcpNwMonitorConnection *oClientSocket;
	//WaitForSingleObject(m_muxListener,INFINITE);
	while(m_xtndlnkdlstptrConnectionSockQueue->DeleteFromFront((void**)&oClientSocket))
	{
		oClientSocket->Disconnect();
		delete oClientSocket;
	}
	delete m_xtndlnkdlstptrConnectionSockQueue;
	m_xtndlnkdlstptrConnectionSockQueue = 0;
	try { shutdown(m_socketTcpSocket,SD_BOTH); closesocket(m_socketTcpSocket); } catch(...){}
	m_socketTcpSocket=0;
	//ReleaseMutex(m_muxListener); 
	return true;
}

bool CTcpNwMonitorListener::SetError(enum NW_ERROR ErrCode) 
{
	 this->m_enumErrCode=ErrCode;
	 return false;
}

SOCKET CTcpNwMonitorListener::GetListenerSocket(void)
{
	 if(this->m_boolErrorFlag) return NON_VALID_SOCKET;
	 return m_socketTcpSocket;
}
	 

bool CTcpNwMonitorListener::RegisterClient(CTcpNwMonitorConnection Client)
{
	if(this->m_boolErrorFlag) return false; 

	WaitForSingleObject(m_muxListener,INFINITE);
	 try
	 {
		 this->m_xtndlnkdlstptrConnectionSockQueue->InsertAtBack((void*)new CTcpNwMonitorConnection(Client));
	ReleaseMutex(m_muxListener); 
		 return true;
	 }
	 catch(...) 
	 { 
	ReleaseMutex(m_muxListener); 
	 return SetError(CLIENT_OVERFLOW);
	 }	
}

bool CTcpNwMonitorListener::ExtractClient(CTcpNwMonitorConnection &Client,int How)
{
	 	 
	 if(this->m_boolErrorFlag) return false;
   WaitForSingleObject(m_muxListener,INFINITE); 
	 try
	 {
			CTcpNwMonitorConnection *FirstItem;
			bool boolFlag;
			if(How==SELECT_CLIENT) boolFlag=m_xtndlnkdlstptrConnectionSockQueue->FirstItem((void**)&FirstItem);
			else boolFlag=m_xtndlnkdlstptrConnectionSockQueue->DeleteFromFront((void**)&FirstItem);
			if(boolFlag==false)
			{		
				 ReleaseMutex(m_muxListener); 
				 return false;
			}
			Client=*FirstItem;
			if(How==DELETE_CLIENT) delete FirstItem;
			ReleaseMutex(m_muxListener); 
			return true;
	 }
	 catch(...)
	 {
			ReleaseMutex(m_muxListener); 
			return false;
	 }
}
bool CTcpNwMonitorListener::RetrieveClient(CTcpNwMonitorConnection &Client)
{
	 return this->ExtractClient(Client,SELECT_CLIENT);
}
bool CTcpNwMonitorListener::RemoveClient(CTcpNwMonitorConnection &Client)
{
	 return this->ExtractClient(Client,DELETE_CLIENT);
}

CTcpNwMonitorListener::CTcpNwMonitorListener(void)
{
	 Reset();	
}

bool CTcpNwMonitorListener::ListenAt(bool LocalHost,UINT Port)
{
	 ReFresh();
	 m_boolErrorFlag=!Initialize(LocalHost?inet_addr(m_strLOCAL_HOST):INADDR_ANY,Port); 
	 return !m_boolErrorFlag;
}

bool CTcpNwMonitorListener::ListenAt(LPCSTR IpAddress,UINT Port)
{
	 ReFresh();
	 m_boolErrorFlag=!Initialize(inet_addr(IpAddress),Port); 
	 return !m_boolErrorFlag;
}

bool CTcpNwMonitorListener::Initialize(UINT uintIP,UINT Port)
{
	try
	{
  			Reset();		
	  		this->ClearError(); 
			m_muxListener=CreateMutex(0,0,0); 
		
			m_socketTcpSocket=socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);
			if(m_socketTcpSocket==INVALID_SOCKET) return SetError(NON_VALID_SOCKET) ;

			m_sokadrinetListenAddress.sin_family=AF_INET;
			m_sokadrinetListenAddress.sin_port=htons(Port);
			m_sokadrinetListenAddress.sin_addr.S_un.S_addr=uintIP;
			if(bind(m_socketTcpSocket,(struct sockaddr*)&m_sokadrinetListenAddress,sizeof(struct sockaddr))!=0) return SetError(SOCKET_BIND_ERROR);
			if(listen(m_socketTcpSocket,SOMAXCONN)!=0) return SetError(LISTEN_ERROR);
			this->m_xtndlnkdlstptrConnectionSockQueue=new CExtendedLinkedList<void>();

		  return true;
	}
	catch(...)	 { return SetError(OTHER);	 }
}

CTcpNwMonitorListener::CTcpNwMonitorListener(bool LocalHost,UINT Port)
{
 	 m_boolErrorFlag=!Initialize(LocalHost?inet_addr(m_strLOCAL_HOST):INADDR_ANY,Port); 
}

void CTcpNwMonitorListener::Reset(void)
{
	 m_socketTcpSocket=NULL;
	 m_xtndlnkdlstptrConnectionSockQueue=0; 
	 m_boolListenFlag=false; 
	 m_handletoAcceptClientThread=NULL;
	 m_muxListener=NULL;
	 this->SetError(SOCKET_BIND_ERROR);   
	 return;
}

bool CTcpNwMonitorListener::DisconnectListenConnection(void)
{
	 return ReFresh();
}

bool CTcpNwMonitorListener::ReFresh(void) 
{
	 try
	 {
			if(m_handletoAcceptClientThread!=NULL) this->SetListenState(false);
			if(m_muxListener) CloseHandle(m_muxListener);  
			if(!this->m_xtndlnkdlstptrConnectionSockQueue) return true;

			ShutDownAllClients();
			
			Reset();
			return true;
	 }
	 catch(...){ Reset(); return false; }
}

CTcpNwMonitorListener::~CTcpNwMonitorListener()
{
	 ReFresh();
}


CTcpNwMonitorListener::CTcpNwMonitorListener(LPCSTR IpAddress,UINT Port)
{
	 m_boolErrorFlag=!Initialize(inet_addr(IpAddress),Port); 
}


bool CTcpNwMonitorListener::SetListenState(bool On)
{
	 if(this->m_boolErrorFlag) return false; 

	 if(On==true) 
	 {
			if(this->m_boolListenFlag==true) return true; 
			if(m_handletoAcceptClientThread!=NULL) return SetError(THREAD_ERROR);
			m_handletoAcceptClientThread=CreateThread(NULL,NULL,(LPTHREAD_START_ROUTINE)&ThreadForAcceptClients,(LPVOID)this,CREATE_SUSPENDED,NULL);
			if(m_handletoAcceptClientThread==NULL) return SetError(THREAD_ERROR);  
			this->m_boolListenFlag=true;
			ResumeThread(m_handletoAcceptClientThread); 	 
	 }
	 else 
	 {
			if(this->m_boolListenFlag==false) return true; 
			if(m_handletoAcceptClientThread==NULL) return SetError(THREAD_ERROR); 
			this->m_boolListenFlag=false; 
			WaitForSingleObject(m_muxListener,INFINITE); 
				 TerminateThread(m_handletoAcceptClientThread,NO_ERR);  
			ReleaseMutex(m_muxListener); 
			DWORD dwrdExitCode=NO_ERR;
			GetExitCodeThread(m_handletoAcceptClientThread,&dwrdExitCode);
			this->m_enumErrCode=(enum NW_ERROR)dwrdExitCode;  
			m_handletoAcceptClientThread=NULL;
	 }
	 return true;
}


UINT ThreadForAcceptClients(LPVOID ptrToListenerObject)
{
	 try
	 {
			CTcpNwMonitorListener *ptrToListener=(CTcpNwMonitorListener*)ptrToListenerObject;
			SOCKET ClientAttached,ListenSocket=NON_VALID_SOCKET;
			struct sockaddr_in ClientSockAddr;
			int ulngSockAddrSize=sizeof(struct sockaddr_in);
			CTcpNwMonitorConnection Client;
	 
			while(Sleep(600),ptrToListener->GetListenFlag())
				 if((ListenSocket=ptrToListener->GetListenerSocket())==NON_VALID_SOCKET) return NON_VALID_SOCKET;
				 else
						if((ClientAttached=accept(ListenSocket,(sockaddr*)&ClientSockAddr,&ulngSockAddrSize))==-1) return ACCEPT_ERROR;  
				 else
				 {
					if(Client.SetAddress(ClientSockAddr),Client.SetSocket(ClientAttached),!ptrToListener->RegisterClient(Client)) 
						return CLIENT_OVERFLOW;  
				 }
	 }
	 catch(...) { return OTHER;	}

	 return NO_ERR;
}

				 
/*---Use Of socket options---------*/

//int intOption=1;
//if(setsockopt(ClientAttached,IPPROTO_TCP,SO_REUSEADDR,(const char*) &intOption,sizeof(int))==SOCKET_ERROR) return OPTION_BIND_ERROR;
//if(intOption=SO_MAX_MSG_SIZE,setsockopt(ClientAttached,IPPROTO_TCP,SO_RCVBUF,(const char*) &intOption,sizeof(int))==SOCKET_ERROR) return OPTION_BIND_ERROR;
//if(intOption=SO_MAX_MSG_SIZE,setsockopt(ClientAttached,IPPROTO_TCP,SO_SNDBUF,(const char*) &intOption,sizeof(int))==SOCKET_ERROR) return OPTION_BIND_ERROR;
	