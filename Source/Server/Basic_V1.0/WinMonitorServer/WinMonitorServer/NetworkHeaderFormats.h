

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
	UINT16 uint16PathLength;
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