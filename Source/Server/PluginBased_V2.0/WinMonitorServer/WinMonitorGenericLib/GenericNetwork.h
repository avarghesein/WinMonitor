

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

	