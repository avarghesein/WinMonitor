

class CChatServerPlugin
{
	protected:
		UINT64 m_uint64Address;

	protected:
		void *GetFunctionAddress(LPCSTR FunctionName);

	public:
		CChatServerPlugin(void);
	   ~CChatServerPlugin();
		bool IsValid(void);
		bool ServiceChatClient(CTcpNwMonitorConnection TcpConnection);
};

void *CChatServerPlugin::GetFunctionAddress(LPCSTR FunctionName)
{
	if(!GblStat_HModulePlugins_1_0) return 0;

	try
	{	
		return (void*)GetProcAddress(GblStat_HModulePlugins_1_0,FunctionName); 
	}
	catch(...) { return 0; }
}

bool CChatServerPlugin::IsValid(void) { return m_uint64Address?true:false; }

CChatServerPlugin::CChatServerPlugin(void)
{
	MethodRetUint64ArgVoid RegisterProc=(MethodRetUint64ArgVoid) GetFunctionAddress("RegisterAsChatServer");
	if(m_uint64Address=0,RegisterProc) 
		try { m_uint64Address=(UINT64)RegisterProc(); } catch(...) {}
}

CChatServerPlugin::~CChatServerPlugin()
{
	MethodRetBoolArgUint64 UnRegisterProc=(MethodRetBoolArgUint64)GetFunctionAddress("RemoveChatServer");
	if(UnRegisterProc) 
		try {  UnRegisterProc(m_uint64Address); } catch(...) {}
}

bool CChatServerPlugin::ServiceChatClient(CTcpNwMonitorConnection TcpConnection)
{
	MethodRetBoolArgUint64Ctcpconnection ProcID=(MethodRetBoolArgUint64Ctcpconnection)GetFunctionAddress("ServiceChatClient");
	if(!ProcID) return false;
	try {  return ProcID(m_uint64Address,TcpConnection); } catch(...) { return false;}
}
