

class CFunPlugin
{
	protected:
		UINT64 m_uint64Address;

	protected:
		void *GetFunctionAddress(LPCSTR FunctionName);
		bool StartOrStopKeyBoardFunPlugin(bool IsStart=true);
		bool CDRomOpenOrClose(bool IsOpen=true);
	
	public:
		CFunPlugin(void);
		bool StartKeyBoardFunPlugin(void); 
		bool StopKeyBoardFunPlugin(void);  
		bool OpenCDRom(void);
		bool CloseCDRom(void);
	   ~CFunPlugin(void);
};


bool CFunPlugin::CDRomOpenOrClose(bool IsOpen)
{
	MethodRetBoolArgUint64BoolDef OpenOrCloseCDRomProc=(MethodRetBoolArgUint64BoolDef)GetFunctionAddress("OpenOrCloseCDRom");
	if(OpenOrCloseCDRomProc) 
		try {  return (bool)OpenOrCloseCDRomProc(m_uint64Address,IsOpen); } catch(...) {}

	return false;
}

bool CFunPlugin::OpenCDRom(void)
{
	return CDRomOpenOrClose(true);
}

bool CFunPlugin::CloseCDRom(void)
{
	return CDRomOpenOrClose(false);
}

void *CFunPlugin::GetFunctionAddress(LPCSTR FunctionName)
{
	if(!GblStat_HModulePlugins_1_0) return 0;

	try
	{	
		return (void*)GetProcAddress(GblStat_HModulePlugins_1_0,FunctionName); 
	}
	catch(...) { return 0; }
}

CFunPlugin::CFunPlugin(void)
{
	MethodRetUint64ArgVoid RegisterProc=(MethodRetUint64ArgVoid) GetFunctionAddress("RegisterAsFunPluginClient");
	if(m_uint64Address=0,RegisterProc) 
		try { m_uint64Address=(UINT64)RegisterProc(); } catch(...) {}
}

CFunPlugin::~CFunPlugin(void)
{
	MethodRetBoolArgUint64 UnRegisterProc=(MethodRetBoolArgUint64)GetFunctionAddress("UnRegisterAsFunPluginClient");
	if(UnRegisterProc) 
		try {  UnRegisterProc(m_uint64Address); } catch(...) {}
}

bool CFunPlugin::StartOrStopKeyBoardFunPlugin(bool IsStart)
{
	MethodRetBoolArgUint64BoolDef StartOrStopKBrdPluginProc=(MethodRetBoolArgUint64BoolDef)GetFunctionAddress("StartOrStopKeyBoardFunPlugin");
	if(StartOrStopKBrdPluginProc) 
		try {  return (bool)StartOrStopKBrdPluginProc(m_uint64Address,IsStart); } catch(...) {}

	return false;
}

bool CFunPlugin::StartKeyBoardFunPlugin(void)
{
	return StartOrStopKeyBoardFunPlugin(true);
}

bool CFunPlugin::StopKeyBoardFunPlugin(void)
{
	return StartOrStopKeyBoardFunPlugin(false);
}