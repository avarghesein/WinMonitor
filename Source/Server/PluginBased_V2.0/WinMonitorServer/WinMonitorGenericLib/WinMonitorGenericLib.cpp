// WinMonitorGenericLib.cpp : Defines the entry point for the DLL application.
//

#define WINMONITORGENERICLIB_EXPORTS 1

#include "stdafx.h"

#include<WINDOWS.H>
#include<math.h>
#include<malloc.h>
#include<stdio.h>
#include <WinSock2.h>
#include<TCHAR.H>
#include<limits.h>

#include "WinMonitorGenericLib.h"
#include "GenericUtility.h"
#include "GenericExtendedQueue.h"
#include "GenericNetwork.h"
#include "LzssCompression.h"
#include "TcpNetworkMonitorDataSocket.h"
#include "TcpNetworkMonitorListener.h"

BOOL APIENTRY DllMain( HANDLE hModule, 
                       DWORD  ul_reason_for_call, 
                       LPVOID lpReserved
					 )
{
    switch (ul_reason_for_call)
	{
		case DLL_PROCESS_ATTACH:
		case DLL_THREAD_ATTACH:
		case DLL_THREAD_DETACH:
		case DLL_PROCESS_DETACH:
			break;
    }
    return TRUE;
}



