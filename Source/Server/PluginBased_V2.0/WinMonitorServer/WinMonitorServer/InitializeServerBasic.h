
extern DWORD WINAPI ReverseConnectionThread(LPVOID Fname);

bool UnInstallReverseConnection(void)
{
	if(!gbl_struct_WinMonitorServerConfig.SpReverseConnection) return true;
	if(gbl_hReverseConnectionThread)
	{
		gbl_blnContinueReverseConnection=false;
		DWORD ExitCode;
		if(GetExitCodeThread(gbl_hReverseConnectionThread,&ExitCode))
			if(ExitCode==STILL_ACTIVE)
				if(WaitForSingleObject(gbl_hReverseConnectionThread,5000)!=WAIT_OBJECT_0)
					TerminateThread(gbl_hReverseConnectionThread,0);
				
	}
	gbl_hReverseConnectionThread=0;
	return true;		
}


bool InstallReverseConnection(void)
{
	if(!gbl_struct_WinMonitorServerConfig.SpReverseConnection) return false;
	if(UnInstallReverseConnection(),gbl_blnContinueReverseConnection=true,(gbl_hReverseConnectionThread=CreateThread(NULL,NULL,(LPTHREAD_START_ROUTINE)&ReverseConnectionThread,NULL,NULL,NULL))==0) 
		return false;

	return true;
}


