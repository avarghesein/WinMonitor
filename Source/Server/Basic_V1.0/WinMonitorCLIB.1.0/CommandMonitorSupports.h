

class CFileMonitor:public CGeneralMemoryUtility
{
	protected:
			WIN32_FIND_DATA *m_dblptrFileData;
			bool   m_boolIsLoaded;
			UINT32 m_lngNoOfFiles;
			UINT32 m_CurrentIndex;

	protected:
			bool ReFresh(BYTE *FileDataStream,UINT32 LengthOfBytes);
			bool GetFileData(int Index,TCHAR **FileInfo,BYTE InfoLevel);
	public:
			CFileMonitor(void);
		   ~CFileMonitor(void);
		    long GetNumberOfFiles(void);
			bool SetFileDataStream(BYTE *FileDataStream,UINT32 LengthOfBytes);
			bool SetFileDataStream(CCommandMonitorClient *CmdMonitor);
			bool GetFirstFile(TCHAR **FileInfo,BYTE InfoLevel);
			bool GetFirstFile(TCHAR *FileInfo,BYTE InfoLevel,UINT32 &ResultLength);
			bool GetNextFile (TCHAR **FileInfo,BYTE InfoLevel);
			bool GetNextFile (TCHAR *FileInfo,BYTE InfoLevel,UINT32 &ResultLength);
			bool GetCurrentFileType(BYTE &FileType);
			bool GetCurrentFile(TCHAR **FileInfo,BYTE InfoLevel);
			bool GetCurrentFile(TCHAR *FileInfo,BYTE InfoLevel,UINT32 &ResultLength);
			bool GetCurrentFileSize(UINT32 &FileSize);
			bool IsCurrentFileIsDir(void);
};


class CMainMemoryMonitor:public CGeneralMemoryUtility
{
	protected:
		MEMORYSTATUSEX MainMemStat;
		bool   m_boolIsLoaded;
		TCHAR  *m_TokenSeperator;
	
	protected:
		bool ReadMemoryInfoFromStream(BYTE *Stream,TCHAR *TokenSeperator);

	public:
		CMainMemoryMonitor(BYTE *MainMemoryInfoStream=0,TCHAR *TokenSeperator=_T("\n"));
	   ~CMainMemoryMonitor();
	   bool SetMemoryInfoStream(BYTE *MainMemoryInfoStream,TCHAR *TokenSeperator=_T("\n"));
	   bool GetMainMemoryStatus(TCHAR **MainMemoryStatus);
	   bool GetMainMemoryStatus(TCHAR *MainMemoryStatus,UINT32 &ResultLength);
	   bool GetVirtualMemoryStatus(TCHAR **VirtualMemoryStatus);	
	   bool GetVirtualMemoryStatus(TCHAR *VirtualMemoryStatus,UINT32 &ResultLength);	
};

class COSMonitor:public CGeneralMemoryUtility
{
	protected:
		OSVERSIONINFOEX OsInfo;
		bool   m_boolIsLoaded;
		TCHAR  *m_TokenSeperator;

	protected:
		bool ReadOSInfoFromStream(BYTE *Stream,TCHAR *TokenSeperator);

	public:
		COSMonitor(BYTE *OSInfoStream=0,TCHAR *TokenSeperator=_T("\n"));
	   ~COSMonitor();
	   bool SetOSInfoStream(BYTE *OSInfoStream,TCHAR *TokenSeperator=_T("\n"));
	   bool GetOSInformation(TCHAR **OSInfo);
	   bool GetOSInformation(TCHAR *OSInfo,UINT32 &ResultLength);
};

//----------------------------------------------------------------//

bool COSMonitor::ReadOSInfoFromStream(BYTE *Stream,TCHAR *TokenSeperator)
{
	if(!Stream) return false;
	if(!TokenSeperator) if(!AllocateMem(&TokenSeperator,_T("\n"))) return false;  
	if(!AllocateMem(&m_TokenSeperator,TokenSeperator)) return false; 
	try
	{	OsInfo=*((OSVERSIONINFOEX*)Stream); }
	catch(...) { return false; }
	return true;
}

COSMonitor::COSMonitor(BYTE *OSInfoStream,TCHAR *TokenSeperator)
{
	if(ReadOSInfoFromStream(OSInfoStream,TokenSeperator)) m_boolIsLoaded=true;
	else m_boolIsLoaded=false;
}

COSMonitor::~COSMonitor() { DeleteAll((void**)&m_TokenSeperator); }

bool COSMonitor::SetOSInfoStream(BYTE *OSInfoStream,TCHAR *TokenSeperator)
{
	if(ReadOSInfoFromStream(OSInfoStream,TokenSeperator)) return m_boolIsLoaded=true;
	else return m_boolIsLoaded=false;
}


bool COSMonitor::GetOSInformation(TCHAR *OSInfo,UINT32 &ResultLength)
{
	try
	{ 
		static TCHAR *tchrPtrResult;
		bool blnOk=true;
		if(!ResultLength) 
		{
			tchrPtrResult=0;
			blnOk=GetOSInformation(&tchrPtrResult);
			ResultLength=_tcslen(tchrPtrResult); 
		}
		else
		{
			try
			{	CopyBytes((BYTE*)OSInfo,(BYTE*)tchrPtrResult,(ResultLength+1)*sizeof(TCHAR) );	}
            catch(...) { blnOk=false; }
			DeleteAll((void**)&tchrPtrResult);
		}
		return blnOk;
	}
	catch(...) { return false; }
}

bool COSMonitor::GetOSInformation(TCHAR **OSInfo)
{
	if(!m_boolIsLoaded) return false;
	TCHAR Tmp[500]; _tcscpy(Tmp,"");

	switch (OsInfo.dwPlatformId)
	{
		case VER_PLATFORM_WIN32_NT:
			if ( OsInfo.dwMajorVersion <= 4 )
				_tcscat(Tmp,"Windows NT family");
			else if ( OsInfo.dwMajorVersion == 5 && OsInfo.dwMinorVersion == 0)
				_tcscat(Tmp,"Windows 2000");
			else if (OsInfo.dwMajorVersion == 5 && OsInfo.dwMinorVersion == 1)
				_tcscat(Tmp,"Windows XP");
			else if (OsInfo.dwMajorVersion == 5 && OsInfo.dwMinorVersion == 2)
				_tcscat(Tmp,"Windows 2003");
			else _tcscat(Tmp,"UnKnown");
			break;
		case VER_PLATFORM_WIN32_WINDOWS:
			if (OsInfo.dwMajorVersion == 4 && OsInfo.dwMinorVersion == 0)
				_tcscat(Tmp,"Windows 95");
			else if (OsInfo.dwMajorVersion == 4 && OsInfo.dwMinorVersion == 10)
				_tcscat(Tmp,"Windows 98");
			else if (OsInfo.dwMajorVersion == 4 && OsInfo.dwMinorVersion == 90)
				_tcscat(Tmp,"Windows ME");
			else _tcscat(Tmp,"UnKnown");
			break;
		case VER_PLATFORM_WIN32s:
			_tcscat(Tmp,"Windows (Win32) family");
			break;
	}

	_tcscat(Tmp,m_TokenSeperator);
	if(!wcscmp((const wchar_t*)OsInfo.szCSDVersion,(const wchar_t*)" A"))
		_tcscat(Tmp,"Windows 98 Second Edition");
	else if(!wcscmp((const wchar_t*)OsInfo.szCSDVersion,(const wchar_t*)" C"))
		_tcscat(Tmp,"Windows 95 OSR2");
	else
		_tcscat(Tmp,(TCHAR*)OsInfo.szCSDVersion);

if(!AllocateMem(OSInfo,Tmp)) return false;
return true;
}

//----------------------------------------------------------------//

bool CMainMemoryMonitor::ReadMemoryInfoFromStream(BYTE *Stream,TCHAR *TokenSeperator)
{
	if(!Stream) return false;
	if(!TokenSeperator) if(!AllocateMem(&TokenSeperator,_T("\n"))) return false;  
	if(!AllocateMem(&m_TokenSeperator,TokenSeperator)) return false; 
	try
	{	MainMemStat=*((MEMORYSTATUSEX *)Stream); }
	catch(...) { return false; }
	return true;
}

bool CMainMemoryMonitor::SetMemoryInfoStream(BYTE *MainMemoryInfoStream,TCHAR *TokenSeperator)
{
	if(ReadMemoryInfoFromStream(MainMemoryInfoStream,TokenSeperator)) return m_boolIsLoaded=true;
	else return m_boolIsLoaded=false;
}

CMainMemoryMonitor::CMainMemoryMonitor(BYTE *MainMemoryInfoStream,TCHAR *TokenSeperator)
{
	if(ReadMemoryInfoFromStream(MainMemoryInfoStream,TokenSeperator)) m_boolIsLoaded=true;
	else m_boolIsLoaded=false;
}

CMainMemoryMonitor::~CMainMemoryMonitor() { DeleteAll((void**)&m_TokenSeperator); }

bool CMainMemoryMonitor::GetVirtualMemoryStatus(TCHAR *VirtualMemoryStatus,UINT32 &ResultLength)
{
	try
	{
		static TCHAR *tchrPtrResult;
		bool blnOk=true;
		if(!ResultLength) 
		{
			tchrPtrResult=0;
			blnOk=GetVirtualMemoryStatus(&tchrPtrResult);
			ResultLength=_tcslen(tchrPtrResult);
		}
		else
		{		
			try
			{	CopyBytes((BYTE*)VirtualMemoryStatus,(BYTE*)tchrPtrResult,(ResultLength+1)*sizeof(TCHAR) );	}
            catch(...) { blnOk=false; }			
			DeleteNew((void**)&tchrPtrResult);
		}
		return blnOk;
	}
	catch(...) { return false; }
}

bool CMainMemoryMonitor::GetVirtualMemoryStatus(TCHAR **VirtualMemoryStatus)
{
	if(!m_boolIsLoaded) return false;

	TCHAR Tmp[500];
	double DIV=1024.0;
	TCHAR *SZE=" KB";

    unsigned long  dbl1=(unsigned long)MainMemStat.ullTotalVirtual/DIV,
		           dbl2=(unsigned long)MainMemStat.ullAvailVirtual/DIV;
	_stprintf(Tmp,"%ld%s%s%ld%s%s",
	     dbl1,SZE,m_TokenSeperator,
	     dbl2,SZE,m_TokenSeperator); 

	if(AllocateNew(VirtualMemoryStatus,Tmp)) return true;
	else return false;
}

bool CMainMemoryMonitor::GetMainMemoryStatus(TCHAR *MainMemoryStatus,UINT32 &ResultLength)
{
	try
	{
		static TCHAR *tchrPtrResult;
		bool blnOk=true;
		if(!ResultLength) 
		{
			tchrPtrResult=0;
			blnOk=GetMainMemoryStatus(&tchrPtrResult);
			ResultLength=_tcslen(tchrPtrResult);
		}
		else
		{		
			try
			{	CopyBytes((BYTE*)MainMemoryStatus,(BYTE*)tchrPtrResult,(ResultLength+1)*sizeof(TCHAR) );	}
            catch(...) { blnOk=false; }			
			DeleteNew((void**)&tchrPtrResult);
		}
		return blnOk;
	}
	catch(...) { return false; }
}

bool CMainMemoryMonitor::GetMainMemoryStatus(TCHAR **MainMemoryStatus)
{
	if(!m_boolIsLoaded) return false;

	TCHAR Tmp[500];
	double DIV=1024.0;
	TCHAR SZE[4]=" KB";

	unsigned long  dbl1=(unsigned long)MainMemStat.ullTotalPhys/DIV,
           dbl2=(unsigned long)MainMemStat.dwMemoryLoad,
		   dbl3=(unsigned long)((double)dbl1*((double)100.0-(double)dbl2)/(double)100.00),
		   dbl4=(unsigned long)MainMemStat.ullTotalPageFile/DIV,
		   dbl5=(unsigned long)MainMemStat.ullAvailPageFile/DIV;

	_stprintf(Tmp,"%ld%s%s%ld%%%s%ld%s%s%ld%s%s%ld%s%s",
    	dbl1,SZE,m_TokenSeperator,
		dbl2,m_TokenSeperator,
        dbl3,SZE,m_TokenSeperator,
		dbl4,SZE,m_TokenSeperator, 	
        dbl5,SZE,m_TokenSeperator);
	if(AllocateNew(MainMemoryStatus,Tmp)) return true;
	else return false;
 }

//----------------------------------------------------------------//

bool CFileMonitor::ReFresh(BYTE *FileDataStream,UINT32 LengthOfBytes)
{
	if(LengthOfBytes<=0) return false;

	if(m_dblptrFileData) try { DeleteAll((void**)&m_dblptrFileData); }
	catch(...) {}
	try
	{
		m_lngNoOfFiles=LengthOfBytes/sizeof(WIN32_FIND_DATA);
		if(!AllocateMem((void**)&m_dblptrFileData,sizeof(WIN32_FIND_DATA)*m_lngNoOfFiles)) return false;
		CopyBytes((BYTE*)m_dblptrFileData,FileDataStream,LengthOfBytes);
		m_CurrentIndex=0;
		return true;
	}
	catch(...) { return false; }
}

long CFileMonitor::GetNumberOfFiles(void)
{
	if(m_boolIsLoaded) return m_lngNoOfFiles; 
	else return 0;
}

CFileMonitor::CFileMonitor(void)
{
	m_boolIsLoaded=false;
	m_lngNoOfFiles=0;
	m_dblptrFileData=0;
	m_CurrentIndex=0;
}

CFileMonitor::~CFileMonitor(void)
{
	if(m_boolIsLoaded) DeleteAll((void**)&m_dblptrFileData);
}

bool CFileMonitor::SetFileDataStream(CCommandMonitorClient *CmdMonitor)
{
	if(CmdMonitor->GetResultantType()!=COMMAND_MONITOR_GOT_SUB_DIRS) return false;  
	BYTE *bytptrResult=0;UINT32 uint32Length=0;
	if(!CmdMonitor->GetResult(bytptrResult,uint32Length) || !uint32Length) return false;
	if(!AllocateMem((void**)&bytptrResult,uint32Length*sizeof(BYTE))) return false;
	if(!CmdMonitor->GetResult(bytptrResult,uint32Length)) return false; 
	return SetFileDataStream(bytptrResult,uint32Length);
}

bool CFileMonitor::SetFileDataStream(BYTE *FileDataStream,UINT32 LengthOfBytes)
{
	if(!ReFresh(FileDataStream,LengthOfBytes)) m_boolIsLoaded=false;
	else m_boolIsLoaded=true;
	return m_boolIsLoaded;
}

bool CFileMonitor::GetFirstFile(TCHAR *FileInfo,BYTE InfoLevel,UINT32 &ResultLength)
{
	try
	{
		static TCHAR *tchrPtrResult;
		bool blnOk=true;
		if(!ResultLength) 
		{	
			tchrPtrResult=0;
			blnOk=GetFirstFile(&tchrPtrResult,InfoLevel);
			if(!blnOk) ResultLength=0; else ResultLength=_tcslen(tchrPtrResult); 
		}
		else
		{
			if(!tchrPtrResult) return false;
			try
			{	CopyBytes((BYTE*)FileInfo,(BYTE*)tchrPtrResult,(ResultLength+1)*sizeof(TCHAR) );	}
            catch(...) { blnOk=false; }
			DeleteAll((void**)&tchrPtrResult);
		}
		return blnOk;
	}
	catch(...) { return false; }
}

bool CFileMonitor::GetFirstFile(TCHAR **FileInfo,BYTE InfoLevel)
{
	m_CurrentIndex=0;
	return GetFileData(0,FileInfo,InfoLevel);
}

bool CFileMonitor::GetCurrentFile(TCHAR **FileInfo,BYTE InfoLevel)
{
	if(m_CurrentIndex>=m_lngNoOfFiles) return false;
	else return GetFileData(m_CurrentIndex,FileInfo,InfoLevel);
}

bool CFileMonitor::IsCurrentFileIsDir(void)
{
	if(m_CurrentIndex>=m_lngNoOfFiles) return false;
	WIN32_FIND_DATA FileInf=m_dblptrFileData[m_CurrentIndex];
	DWORD DwdAttribute=FileInf.dwFileAttributes;
	return (FILE_ATTRIBUTE_DIRECTORY & DwdAttribute)?true:false;
}

bool CFileMonitor::GetCurrentFile(TCHAR *FileInfo,BYTE InfoLevel,UINT32 &ResultLength)
{
	try
	{
		static TCHAR *tchrPtrResult;
		bool blnOk=true;
		if(!ResultLength) 
		{	
			tchrPtrResult=0;
			blnOk=GetCurrentFile(&tchrPtrResult,InfoLevel);
			if(!blnOk) ResultLength=0; else ResultLength=_tcslen(tchrPtrResult); 
		}
		else
		{
			if(!tchrPtrResult) return false;
			try
			{	CopyBytes((BYTE*)FileInfo,(BYTE*)tchrPtrResult,(ResultLength+1)*sizeof(TCHAR) );	}
            catch(...) { blnOk=false; }
			DeleteAll((void**)&tchrPtrResult);
		}
		return blnOk;
	}
	catch(...) { return false; }
}

bool CFileMonitor::GetNextFile (TCHAR *FileInfo,BYTE InfoLevel,UINT32 &ResultLength)
{
	try
	{
		static TCHAR *tchrPtrResult;
		bool blnOk=true;
		if(!ResultLength) 
		{	
			tchrPtrResult=0;
			blnOk=GetNextFile(&tchrPtrResult,InfoLevel);
			if(!blnOk) ResultLength=0; else ResultLength=_tcslen(tchrPtrResult); 
		}
		else
		{
			if(!tchrPtrResult) return false;
            try
			{	CopyBytes((BYTE*)FileInfo,(BYTE*)tchrPtrResult,(ResultLength+1)*sizeof(TCHAR) );	}
            catch(...) { blnOk=false; }
			DeleteAll((void**)&tchrPtrResult);
		}
		return blnOk;
	}
	catch(...) { return false; }
}


bool CFileMonitor::GetNextFile(TCHAR **FileInfo,BYTE InfoLevel)
{
	if(m_CurrentIndex>=m_lngNoOfFiles-1) return false;
	else return GetFileData(++m_CurrentIndex,FileInfo,InfoLevel);
}

bool CFileMonitor::GetCurrentFileSize(UINT32 &FileSize)
{
	if(FileSize=0,!m_boolIsLoaded || m_CurrentIndex>=m_lngNoOfFiles-1) return false;
	WIN32_FIND_DATA FileInf=m_dblptrFileData[m_CurrentIndex];
	FileSize=((UINT64)(FileInf.nFileSizeHigh)) * ((UINT64)MAXDWORD+1) + (UINT64)FileInf.nFileSizeLow;
	return true;
}

bool CFileMonitor::GetFileData(int Index,TCHAR **FileInfo,BYTE InfoLevel)
{
	if(!m_boolIsLoaded) return false;
	try
	{
		WIN32_FIND_DATA FileInf=m_dblptrFileData[m_CurrentIndex];
		TCHAR *strFileData=0;
		switch(InfoLevel)
		{
			case CFileMonitor_FILENAME_ONLY:
					AllocateMem(&strFileData,(TCHAR*)FileInf.cFileName);
					*FileInfo=strFileData;
					return true;

			case CFileMonitor_FILENAME_AND_SIZE:
				TCHAR Tmp[1000];
				_stprintf(Tmp,"File Name:%s\nFile Size in bytes:%I64d",FileInf.cFileName,((UINT64)(FileInf.nFileSizeHigh)) * ((UINT64)MAXDWORD+1) + (UINT64)FileInf.nFileSizeLow);
				AllocateMem(&strFileData,(TCHAR*)Tmp);
				*FileInfo=strFileData;
				return true;

			case CFileMonitor_FILEINFO_DETAILED:
			default:
				TCHAR Tmp1[5000];
				_stprintf(Tmp1,"File Name:%s\nFile Size in bytes:%I64d",FileInf.cFileName,((UINT64)(FileInf.nFileSizeHigh) * ((UINT64)MAXDWORD+1)) +(UINT64) FileInf.nFileSizeLow);
				TCHAR Tmp2[100];
				SYSTEMTIME s; FILETIME s1;
				FileTimeToLocalFileTime(&FileInf.ftCreationTime,&s1),FileTimeToSystemTime(&s1,&s);
    			_stprintf(Tmp2,"\nCreation Date %u-%u-%u\nCreation Time %u:%u:%u",s.wDay,s.wMonth,s.wYear,s.wSecond,s.wMinute,s.wHour);      
				FileTimeToLocalFileTime(&FileInf.ftLastAccessTime,&s1),FileTimeToSystemTime(&s1,&s);
				TCHAR Tmp3[100];
				_stprintf(Tmp3,"\nLast Access Date %u-%u-%u\nLast Access Time %u:%u:%u",s.wDay,s.wMonth,s.wYear,s.wSecond,s.wMinute,s.wHour);      
				TCHAR Tmp4[2000];
				_tcscpy(Tmp4,_T("\n\nFile Attributes"));
				bool blnFlag=false;
				DWORD DwdAttribute=FileInf.dwFileAttributes;

				if(FILE_ATTRIBUTE_DIRECTORY & DwdAttribute) _tcscat(Tmp4,_T("\n Directory:Yes")),blnFlag=true;
				if(FILE_ATTRIBUTE_TEMPORARY & DwdAttribute) _tcscat(Tmp4,_T("\n Temporary:Yes")),blnFlag=true; 
				if(FILE_ATTRIBUTE_ARCHIVE & DwdAttribute) _tcscat(Tmp4,_T("\n Archive:Yes")),blnFlag=true; 
				if(FILE_ATTRIBUTE_ENCRYPTED & DwdAttribute) _tcscat(Tmp4,_T("\n Encrypted:Yes")),blnFlag=true; 
				if(FILE_ATTRIBUTE_SYSTEM & DwdAttribute) _tcscat(Tmp4,_T("\n System File:Yes")),blnFlag=true; 
				if(FILE_ATTRIBUTE_READONLY & DwdAttribute) _tcscat(Tmp4,_T("\n Read Only:Yes")),blnFlag=true; 
				if(FILE_ATTRIBUTE_OFFLINE & DwdAttribute) _tcscat(Tmp4,_T("\n Offline:Yes")),blnFlag=true;
				if(FILE_ATTRIBUTE_COMPRESSED & DwdAttribute) _tcscat(Tmp4,_T("\n Compressed:Yes")),blnFlag=true; 
				if(FILE_ATTRIBUTE_HIDDEN & DwdAttribute) _tcscat(Tmp4,_T("\n Hidden:Yes")),blnFlag=true;
				if(FILE_ATTRIBUTE_NORMAL & DwdAttribute) _tcscat(Tmp4,_T("\n Normal:Yes")),blnFlag=true; 

				_tcscat(_tcscat(_tcscat(Tmp1,Tmp2),Tmp3),Tmp4);
				if(!blnFlag) _tcscat(Tmp1,_T(":No additional information"));
				AllocateMem(&strFileData,(TCHAR*)Tmp1);
				*FileInfo=strFileData;
		}
	    return true;
	}
	catch(...) { return false; }
}

bool CFileMonitor::GetCurrentFileType(BYTE &FileType)
{
	if(!m_boolIsLoaded) return false;
	DWORD DwdAttribute=m_dblptrFileData[m_CurrentIndex].dwFileAttributes;
	if(FILE_ATTRIBUTE_DIRECTORY & DwdAttribute) FileType=CFileMonitor_FILETYPE_DIR; else
	if(FILE_ATTRIBUTE_TEMPORARY & DwdAttribute) FileType=CFileMonitor_FILETYPE_TEMP; else
	if(FILE_ATTRIBUTE_ARCHIVE & DwdAttribute) FileType=CFileMonitor_FILETYPE_ARCHIVE; else
	if(FILE_ATTRIBUTE_ENCRYPTED & DwdAttribute) FileType=CFileMonitor_FILETYPE_ENCRYPT; else
	if(FILE_ATTRIBUTE_SYSTEM & DwdAttribute) FileType=CFileMonitor_FILETYPE_SYSTEM; else
	if(FILE_ATTRIBUTE_READONLY & DwdAttribute) FileType=CFileMonitor_FILETYPE_READONLY; else
	if(FILE_ATTRIBUTE_OFFLINE & DwdAttribute) FileType=CFileMonitor_FILETYPE_OFFLINE; else
	if(FILE_ATTRIBUTE_COMPRESSED & DwdAttribute) FileType=CFileMonitor_FILETYPE_COMPRESSED; else
    if(FILE_ATTRIBUTE_HIDDEN & DwdAttribute) FileType=CFileMonitor_FILETYPE_HIDDEN;else
	if(FILE_ATTRIBUTE_NORMAL & DwdAttribute) FileType=CFileMonitor_FILETYPE_NORMAL; 
	else
		FileType=CFileMonitor_FILETYPE_UNDEF;

	return true;
}


//-----------------------------------------------------//