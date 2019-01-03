

class CDesktopPlugin:public INetworkByteStream
{
	protected:
		UINT64 m_uint64Address;

	protected:
		void *GetFunctionAddress(LPCSTR FunctionName);
		bool SetCompressionOrType(BYTE TypeOrCompression,bool IsType=true);
		bool CaptureOrReplaceDesktopOrWindowImage(bool IsCapture,bool IsDesktop,HWND hWnd=0);
		bool LoadOrSaveImage(bool IsLoad,bool IsCompress,TCHAR *FileName);
			
	public:
		CDesktopPlugin(BYTE Compression=SCREEN_MONITOR_COMPRESS_NIL);
	   ~CDesktopPlugin();
		bool IsValid(void);	    
		bool SetCompression(BYTE Compression);
		bool SetType(BYTE TransferItemType);
		bool CaptureDesktop(void);
		bool CaptureWindow(HWND hWnd=0);
		bool ReplaceDesktop(void);
		bool ReplaceWindow(HWND hWnd=0);
		bool LoadImageFromFile(TCHAR *FileName,bool IsCompressed=false);
		bool SaveImageToFile(TCHAR *FileName,bool AsCompressed=true);
	    UINT32 WriteToNetworkByteStream(BYTE* &OutByteStream);
		UINT32 ReadFromNetworkByteStream(BYTE *InByteStream);
};


bool CDesktopPlugin::IsValid(void) { return m_uint64Address?true:false; }

void *CDesktopPlugin::GetFunctionAddress(LPCSTR FunctionName)
{
	if(!GblStat_HModulePlugins_1_0) return 0;

	try
	{	
		return (void*)GetProcAddress(GblStat_HModulePlugins_1_0,FunctionName); 
	}
	catch(...) { return 0; }
}

CDesktopPlugin::CDesktopPlugin(BYTE Compression)
{
	MethodRetUint64ArgByte RegisterProc=(MethodRetUint64ArgByte)GetFunctionAddress("RegisterAsScreenMonitorUser");
	if(m_uint64Address=0,RegisterProc) 
		try { m_uint64Address=(UINT64)RegisterProc(Compression); } catch(...) {}
}

CDesktopPlugin::~CDesktopPlugin(void)
{
	MethodRetBoolArgUint64 UnRegisterProc=(MethodRetBoolArgUint64)GetFunctionAddress("RemoveScreenMonitorUser");
	if(UnRegisterProc) 
		try {  UnRegisterProc(m_uint64Address); } catch(...) {}
}

bool CDesktopPlugin::SetCompressionOrType(BYTE TypeOrCompression,bool IsType)
{
	MethodRetBoolArgUint64ByteBoolDef ProcID=(MethodRetBoolArgUint64ByteBoolDef)GetFunctionAddress("SetCompressionOrType");
	if(!ProcID) return false;
	try {  return ProcID(m_uint64Address,TypeOrCompression,IsType); } catch(...) { return false;}
}

bool CDesktopPlugin::SetCompression(BYTE Compression)
{
	return SetCompressionOrType(Compression,false);
}

bool  CDesktopPlugin::SetType(BYTE TransferItemType)
{
	return SetCompressionOrType(TransferItemType,true);
}

bool CDesktopPlugin::LoadOrSaveImage(bool IsLoad,bool IsCompress,TCHAR *FileName)
{
	MethodRetBoolArgUint64Bool2Tcharptr  ProcID=(MethodRetBoolArgUint64Bool2Tcharptr)GetFunctionAddress("LoadOrSaveImage");
	if(!ProcID) return false;
	try {  return ProcID(m_uint64Address,IsLoad,IsCompress,FileName); } catch(...) { return false;}
}

bool CDesktopPlugin::LoadImageFromFile(TCHAR *FileName,bool IsCompressed)
{
	return LoadOrSaveImage(true,IsCompressed,FileName);
}

bool CDesktopPlugin::SaveImageToFile(TCHAR *FileName,bool AsCompressed)
{
	return LoadOrSaveImage(false,AsCompressed,FileName);
}

bool CDesktopPlugin::CaptureOrReplaceDesktopOrWindowImage(bool IsCapture,bool IsDesktop,HWND hWnd)
{
	MethodRetBoolArgUint64Bool2HwndDef ProcID=(MethodRetBoolArgUint64Bool2HwndDef)GetFunctionAddress("CaptureOrReplaceDesktopOrWindowImage");
	if(!ProcID) return false;
	try {  return ProcID(m_uint64Address,IsCapture,IsDesktop,hWnd); } catch(...) { return false;}
}

bool CDesktopPlugin::CaptureDesktop(void)
{
	return CaptureOrReplaceDesktopOrWindowImage(true,true,0);
}

bool CDesktopPlugin::CaptureWindow(HWND hWnd)
{
	return CaptureOrReplaceDesktopOrWindowImage(true,false,hWnd);
}

bool CDesktopPlugin::ReplaceDesktop(void)
{
	return CaptureOrReplaceDesktopOrWindowImage(false,true,0);
}

bool CDesktopPlugin::ReplaceWindow(HWND hWnd)
{
	return CaptureOrReplaceDesktopOrWindowImage(false,false,hWnd);
}

UINT32 CDesktopPlugin::WriteToNetworkByteStream(BYTE* &OutByteStream)
{
	MethodRetUint32ArgUint64ByteptrRef ProcID=(MethodRetUint32ArgUint64ByteptrRef)GetFunctionAddress("ScreenMonitorWriteToNetworkByteStream");
	if(!ProcID) return false;
	try {  return ProcID(m_uint64Address,OutByteStream); } catch(...) { return false;}
}

UINT32 CDesktopPlugin::ReadFromNetworkByteStream(BYTE *InByteStream)
{
	MethodRetUint32ArgUint64Byteptr ProcID=(MethodRetUint32ArgUint64Byteptr)GetFunctionAddress("ScreenMonitorReadFromNetworkByteStream");
	if(!ProcID) return false;
	try {  return ProcID(m_uint64Address,InByteStream); } catch(...) { return false;}
}
