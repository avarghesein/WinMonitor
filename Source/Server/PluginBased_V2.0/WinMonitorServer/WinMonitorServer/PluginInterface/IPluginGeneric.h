

	//-------------Loaded Plugin Modules---------//

	static HMODULE GblStat_HModulePlugins_1_0 = 0;

	//-------------Function Prototypes------------//
	
	typedef UINT64 (*MethodRetUint64ArgVoid)                (void);
	typedef UINT64 (*MethodRetUint64ArgByte)                (BYTE);
	typedef bool   (*MethodRetBoolArgUint64)                (UINT64);
	typedef bool   (*MethodRetBoolArgUint64BoolDef)         (UINT64,bool = false);
	typedef bool   (*MethodRetBoolArgUint64ByteBoolDef)     (UINT64,BYTE,bool = false); 
	typedef UINT32 (*MethodRetUint32ArgUint64ByteptrRef)    (UINT64,BYTE*&);
	typedef UINT32 (*MethodRetUint32ArgUint64Byteptr)       (UINT64,BYTE*);
	typedef bool   (*MethodRetBoolArgUint64Ctcpconnection)  (UINT64,CTcpNwMonitorConnection); 
	typedef bool   (*MethodRetBoolArgCtcpconnection)        (CTcpNwMonitorConnection); 
	typedef bool   (*MethodRetBoolArgUint64Bool2HwndDef)    (UINT64,bool,bool,HWND = 0);
	typedef bool   (*MethodRetBoolArgUint64Bool2Tcharptr)   (UINT64,bool,bool,TCHAR*);
	typedef bool   (*MethodRetBoolArgInt) (int);
	typedef bool   (*MethodRetBoolArgVoid) (void);
	typedef bool   (*MethodRetBoolArgHndRef)(HANDLE&);
			



