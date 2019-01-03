

//-------------Class Declaration---------//

class CWinMonitorFunPlugin 
{
	protected:
		static HANDLE         m_KeyBrdFunPluginThreadHandle;
		static volatile bool  m_blnContinueFunPlugin;

	protected:
		bool CDRomOpenOrClose(bool IsOpen=true);

	public:
		bool CanContinue(void);
		bool StartKeyBoardPlugin(void);
		bool StopKeyBoardPlugin(void);
		bool OpenCDRom(void);
		bool CloseCDRom(void);
};

//-------------End Class Declaration---------//

DWORD  WINAPI KeyBoardFunPluginThread(LPVOID RootName);
HANDLE CWinMonitorFunPlugin::m_KeyBrdFunPluginThreadHandle=0;
volatile bool CWinMonitorFunPlugin::m_blnContinueFunPlugin=false;

bool CWinMonitorFunPlugin::OpenCDRom(void)
{
	return CDRomOpenOrClose(true);
}

bool CWinMonitorFunPlugin::CloseCDRom(void)
{
	return CDRomOpenOrClose(false);
}

bool CWinMonitorFunPlugin::CDRomOpenOrClose(bool IsOpen)
{ 
	MCI_OPEN_PARMS open;
	DWORD flags;

	ZeroMemory(&open, sizeof(MCI_OPEN_PARMS));

	open.lpstrDeviceType = (LPCSTR) MCI_DEVTYPE_CD_AUDIO;
	open.lpstrElementName = 0;//--need name G:\

	//flags = MCI_OPEN_TYPE | MCI_OPEN_TYPE_ID | MCI_OPEN_SHAREABLE;
	flags = MCI_OPEN_TYPE | MCI_OPEN_TYPE_ID;

	if (!mciSendCommand(0, MCI_OPEN, flags, (DWORD) &open)) 
	{
		if(mciSendCommand(open.wDeviceID, MCI_SET, (IsOpen)? MCI_SET_DOOR_OPEN : MCI_SET_DOOR_CLOSED, 0)) return false;
		if(mciSendCommand(open.wDeviceID, MCI_CLOSE, MCI_WAIT, 0)) return false;
	}
	else 
		return false;

	return true;
}



bool CWinMonitorFunPlugin::StartKeyBoardPlugin(void)
{
	StopKeyBoardPlugin();
	m_blnContinueFunPlugin=true;
	if((m_KeyBrdFunPluginThreadHandle=CreateThread(NULL,NULL,(LPTHREAD_START_ROUTINE)&KeyBoardFunPluginThread,(LPVOID)this,NULL,NULL))==0)
		return false;
	else
		return true;
}

bool CWinMonitorFunPlugin::StopKeyBoardPlugin(void)
{
	if(m_KeyBrdFunPluginThreadHandle)
	{
		m_blnContinueFunPlugin=false;
		DWORD ExitCode;
		if(GetExitCodeThread(m_KeyBrdFunPluginThreadHandle,&ExitCode))
			if(ExitCode==STILL_ACTIVE)
				if(WaitForSingleObject(m_KeyBrdFunPluginThreadHandle,2000)!=WAIT_OBJECT_0)
					TerminateThread(m_KeyBrdFunPluginThreadHandle,0);
				
	}
	m_KeyBrdFunPluginThreadHandle=0;
	return true;		
}

bool CWinMonitorFunPlugin::CanContinue(void)
{
	return m_blnContinueFunPlugin;
}

DWORD WINAPI KeyBoardFunPluginThread(LPVOID RootName)
{
	
	CWinMonitorFunPlugin *KbdPlugin=(CWinMonitorFunPlugin*)RootName;
//  int intCnt;
//	INPUT inputKBrd[3];

	while(KbdPlugin->CanContinue())
	{
		keybd_event(VK_NUMLOCK,0x45,KEYEVENTF_EXTENDEDKEY | 0,0);
		keybd_event(VK_NUMLOCK,0x45,KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP,0);
		Sleep(100);
		keybd_event(VK_CAPITAL,0x45,KEYEVENTF_EXTENDEDKEY | 0,0);
		keybd_event(VK_CAPITAL,0x45,KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP,0);
		Sleep(100);
		keybd_event(VK_SCROLL,0x45,KEYEVENTF_EXTENDEDKEY | 0,0);
		keybd_event(VK_SCROLL,0x45,KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP,0);
		Sleep(100);

/*		for(intCnt=0;intCnt<3;intCnt++)
		{
			inputKBrd[intCnt].type=INPUT_KEYBOARD;
			switch(intCnt)
			{
				case 0:
					inputKBrd[intCnt].ki.wVk=VK_NUMLOCK;  
					break;

				case 1:
					inputKBrd[intCnt].ki.wVk=VK_CAPITAL;  
					break;

				default:
					inputKBrd[intCnt].ki.wVk=VK_SCROLL;  
			}
			inputKBrd[intCnt].ki.wScan=0x45;
			inputKBrd[intCnt].ki.dwFlags=KEYEVENTF_EXTENDEDKEY;	
			inputKBrd[intCnt].ki.time=0;
			inputKBrd[intCnt].ki.dwExtraInfo=GetMessageExtraInfo();
		}

		for(SendInput(3,inputKBrd,sizeof(INPUT)),intCnt=0;intCnt<3;intCnt++)
			inputKBrd[intCnt].ki.dwFlags=KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP;

		SendInput(3,inputKBrd,sizeof(INPUT));   
*/
	}

	return 0;
}


