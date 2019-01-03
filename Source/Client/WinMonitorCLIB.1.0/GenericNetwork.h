


#include <WinSock2.h>
#include<TCHAR.H>

#define MADE_NW_ERROR -1

enum NW_ERROR 
{	
	 NON_VALID_SOCKET=-1,
	 NO_ERR=0,
	 LISTEN_ERROR=1,
	 ACCEPT_ERROR=2,
	 CLIENT_OVERFLOW=3,
	 CLIENT_INVALID=4,
	 OTHER=5,
	 WINSOCK_DLL_LOAD_ERROR=6,
	 WINSOCK_DLL_RESET_ERROR=7,
	 WINSOCK_VERSION_NOT_FOUND=8,
	 SOCKET_BIND_ERROR=9,
	 THREAD_ERROR=10,
	 SERVER_INVALID=11,
	 CONNECTION_CLOSED=12,
	 CONNECTION_CLOSE_ERROR=13,
	 OPTION_BIND_ERROR=14
};


class INetworkByteStream
{
	public:
		virtual UINT32 WriteToNetworkByteStream(BYTE* &OutByteStream)=0;
		virtual UINT32 ReadFromNetworkByteStream(BYTE *InByteStream)=0;
};


class CInitializeNetworkLibrary
{
protected:
	 static const int m_statconstintWinSockVersion;
	 static enum NW_ERROR m_statenumErrCode;
	 static bool SetError(enum NW_ERROR ErrCode){ m_statenumErrCode=ErrCode;return false; } 

public:
	 static enum NW_ERROR LastError(void) {  return m_statenumErrCode; }
	 static bool LoadLibraryAndVersion(void);
	 static bool CleanUpLibrary(void);
};

const int CInitializeNetworkLibrary::m_statconstintWinSockVersion=2; 
enum NW_ERROR CInitializeNetworkLibrary::m_statenumErrCode=NO_ERR;

bool CInitializeNetworkLibrary::LoadLibraryAndVersion(void)
{
	 const int iReqWinsockVer = m_statconstintWinSockVersion;   
	 WSADATA wsaData;
	 SetError(NO_ERR);  		
	 if (WSAStartup(MAKEWORD(iReqWinsockVer,0), &wsaData)!=0) return SetError(WINSOCK_DLL_LOAD_ERROR); 
	 if (LOBYTE(wsaData.wVersion) < iReqWinsockVer) return SetError(WINSOCK_VERSION_NOT_FOUND); 
	 
	 return true;
}

bool CInitializeNetworkLibrary::CleanUpLibrary(void)
{
	 if(WSACleanup()!=0) return SetError(WINSOCK_DLL_RESET_ERROR);  
	 return true;
}


class CNetworkBase
{
   protected:
		bool   FlushAll(SOCKET Socket);
	
	public:
		TCHAR *GetHostIP(const char *addr);
};


bool CNetworkBase::FlushAll(SOCKET Socket) 
{
	char *Buff=0;
	try
	{
		char chrCh;
		int intPendingBytes=recv(Socket,&chrCh,1,MSG_PEEK),intRecvd;

		Buff=new char[intPendingBytes];

		while(intPendingBytes>0)
			if((intRecvd=recv(Socket,Buff,intPendingBytes,NULL))==SOCKET_ERROR) throw MADE_NW_ERROR;
			else intPendingBytes-=intRecvd;

		delete[] Buff;Buff=0;
		return true;
	}
	catch(...) 
	{ 
		if(Buff) delete[] Buff;
		return false;
	}
}


TCHAR *CNetworkBase::GetHostIP(const char *addr)
{
	/* performs host resolution, pass NULL to get local ip */

   struct hostent *he = NULL;
   char address[64];

   if (addr == NULL) //strcpy(address, "");
   {
	   if(gethostname(address,64)!=0) return 0; 
   }
   else strcpy(address, addr);

   if(he = gethostbyname(address),he == NULL) return (TCHAR*)NULL;
   TCHAR *HostAddr=new TCHAR[18];
   return _tcscpy(HostAddr,(TCHAR*)(inet_ntoa(*(struct in_addr *) he->h_addr_list[0])));
}

	