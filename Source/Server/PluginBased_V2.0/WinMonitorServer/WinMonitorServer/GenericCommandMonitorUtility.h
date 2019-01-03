

#include<io.h>
#include<stdio.h>
#include<sys/stat.h>
#include<FCNTL.H>

#define MADE_ERROR -1

class CCommandMonitorUtility:public CGeneralMemoryUtility
{
	protected:
		UINT32 m_uint32TypeAndCompression;
		BYTE  *m_tchrCommandResult;
		BYTE  *m_tchrCommandArguments;
		BYTE   m_tchrTokenSeperator;
		UINT32 m_ulngResultLength;
		UINT32 m_ulngCommandArgLength;

	protected:
		void DeleteAll(void);
		
		bool SaveFileToDisk(TCHAR *FileName,BYTE *DataStream,UINT32 StreamLength);
		bool SaveFileToDisk(TCHAR *FileName,TCHAR *DstRootDirPath,BYTE *DataStream,UINT32 StreamLength);
		bool LoadFileFromDisk(TCHAR *FileName,void **DataStream,UINT32 &StreamLength,UINT32 DataOffset=0);
		
	public:
	  ~CCommandMonitorUtility();
	   CCommandMonitorUtility(UINT32 CommandType=COMMAND_MONITOR_NULL_COMMAND,TCHAR *CommandArgs=0,UINT32 Compression=COMMAND_MONITOR_COMPRESS_NIL,TCHAR ResultSeperator=_T('\n'));
	   bool SetTokenSeperator(TCHAR ResultSeperator=_T('\n'));
	   bool SetCommand(UINT32 CommandType,TCHAR *CommandArgs=0);
	   bool SetCompression(UINT32 Compression);
};

//-------------------------------------------------------------------//

CCommandMonitorUtility::CCommandMonitorUtility(UINT32 CommandType,TCHAR *CommandArgs,UINT32 Compression,TCHAR ResultSeperator)
{
	COMMAND_MONITOR_SetType(CommandType,m_uint32TypeAndCompression);
	COMMAND_MONITOR_SetCompression(Compression,m_uint32TypeAndCompression);   
	if(m_ulngCommandArgLength=0,m_tchrCommandResult=m_tchrCommandArguments=0,!CommandArgs || !AllocateMem((TCHAR**)&m_tchrCommandArguments,CommandArgs)) m_tchrCommandArguments=0;
	if(m_tchrCommandArguments && _tcslen((TCHAR*)m_tchrCommandArguments)>0) m_ulngCommandArgLength=sizeof(TCHAR)*(_tcslen((TCHAR*)m_tchrCommandArguments)+1);
	m_tchrTokenSeperator=ResultSeperator;
	m_ulngResultLength=0;
}

bool CCommandMonitorUtility::SetTokenSeperator(TCHAR ResultSeperator)
{
	m_tchrTokenSeperator=ResultSeperator;
	return true;
}

CCommandMonitorUtility::~CCommandMonitorUtility()
{
	DeleteAll(); 
}

bool CCommandMonitorUtility::SetCommand(UINT32 CommandType,TCHAR *CommandArgs)
{
	m_ulngCommandArgLength=0;
	COMMAND_MONITOR_SetType(CommandType,m_uint32TypeAndCompression);
	CGeneralMemoryUtility::DeleteAll((void**)&m_tchrCommandArguments); 
	if(!CommandArgs) return true;
	if(!AllocateMem((TCHAR**)&m_tchrCommandArguments,(TCHAR*)CommandArgs)) 
	{ m_tchrCommandArguments=0; return false; }
	if(m_tchrCommandArguments && _tcslen((TCHAR*)m_tchrCommandArguments)>0)
		m_ulngCommandArgLength=sizeof(TCHAR)*(_tcslen((TCHAR*)m_tchrCommandArguments)+1);

	return true;
}	

bool CCommandMonitorUtility::SetCompression(UINT32 Compression)
{
	COMMAND_MONITOR_SetCompression(Compression,m_uint32TypeAndCompression);   
	return true;
}


void CCommandMonitorUtility::DeleteAll(void)
{
	try
	{ 
		if(m_tchrCommandResult) 
		{
			try { CGeneralMemoryUtility::DeleteAll((void**)&m_tchrCommandResult); }
			catch(...){}
			m_tchrCommandResult=0;
		}
		if(m_tchrCommandArguments) 
		{
			try{ CGeneralMemoryUtility::DeleteAll((void**)&m_tchrCommandArguments); }
			catch(...){}
			m_tchrCommandArguments=0;
		}
	} catch(...) {}
	return;
}

bool CCommandMonitorUtility::SaveFileToDisk(TCHAR *FileName,BYTE *DataStream,UINT32 StreamLength)
{
	TCHAR *Path=0,*RelFileName=0;
	if(!SplitToPathAndFileName(FileName,&Path,&RelFileName)) return false;
	bool blnOk=SaveFileToDisk(RelFileName,Path,DataStream,StreamLength);
	CGeneralMemoryUtility::DeleteAll((void**)&Path),CGeneralMemoryUtility::DeleteAll((void**)&RelFileName);
	return blnOk;
}

/*

bool CCommandMonitorUtility::SaveFileToDisk(TCHAR *FileName,TCHAR *DstRootDirPath,BYTE *DataStream,UINT32 StreamLength)
{
	TCHAR *Path=0; 
	try
	{
		Path=new TCHAR[sizeof(TCHAR)*(_tcslen(FileName)+_tcslen(DstRootDirPath)+5)];
		if(!Path) return false;
		if(!_tcscpy(Path,DstRootDirPath)) return false;
	}
	catch(...) { return false; }

	try
	{
		if(!CreatePathFromRoot(Path))
		{
			DeleteNew((void**)&Path);
			return false;
		}
	}
	catch(...) { return false; }
	
	try 
	{
		FILE *File=fopen((char*)_tcscat(Path,FileName),"wb");
		if(!File) return false;
		if(fwrite(DataStream,StreamLength,1,File)!=1)
		{
			fclose(File);
			DeleteNew((void**)&Path);
			return false;
		}
		fclose(File);
	}
	catch(...) { return false; }

	DeleteNew((void**)&Path);
	return true;
}

bool CCommandMonitorUtility::LoadFileFromDisk(TCHAR *FileName,void **DataStream,UINT32 &StreamLength,UINT32 DataOffset)
{
	TCHAR *ch=0;
	FILE *hFile=0;
	try
	{
		StreamLength=0,CGeneralMemoryUtility::DeleteAll(DataStream);
		if(!FileName ) return false;
		hFile=fopen((char*)FileName,"rb");
		if(!hFile || fseek(hFile,0,SEEK_END)!=0) return false;
		UINT32 FileLen=ftell(hFile);
		if(rewind(hFile),!AllocateMem((void**)&ch,(sizeof(TCHAR)*(FileLen+1))+DataOffset)) throw MADE_ERROR;
		if(fread((void*)((BYTE*)ch+DataOffset),FileLen,1,hFile)!=1) throw MADE_ERROR;
		fclose(hFile);
		*DataStream=(void*)ch,StreamLength=DataOffset+FileLen;
		return true;
	}
	catch(...) 
	{
		fclose(hFile);
		if(ch) CGeneralMemoryUtility::DeleteAll((void**)&ch);
		return false;
	}
}
*/

bool CCommandMonitorUtility::SaveFileToDisk(TCHAR *FileName,TCHAR *DstRootDirPath,BYTE *DataStream,UINT32 StreamLength)
{
	TCHAR *Path=0; 
	try
	{
		Path=new TCHAR[sizeof(TCHAR)*(_tcslen(FileName)+_tcslen(DstRootDirPath)+5)];
		if(!Path) return false;
		if(!_tcscpy(Path,DstRootDirPath)) return false;
	}
	catch(...) { return false; }

	try
	{
		if(!CreatePathFromRoot(Path))
		{
			DeleteNew((void**)&Path);
			return false;
		}
	}
	catch(...) { return false; }
	
	try 
	{
		HFILE File=_open((char*)_tcscat(Path,FileName),_O_CREAT | _O_TRUNC | _O_WRONLY | _O_BINARY ,_S_IREAD | _S_IWRITE );
		if(File==-1) return false;
		if(_write(File,DataStream,StreamLength)!=(int)StreamLength)
		{
			_close(File);
			DeleteNew((void**)&Path);
			return false;
		}
		_commit(File),_close(File);
	}
	catch(...) { return false; }

	DeleteNew((void**)&Path);
	return true;
}

bool CCommandMonitorUtility::LoadFileFromDisk(TCHAR *FileName,void **DataStream,UINT32 &StreamLength,UINT32 DataOffset)
{
	TCHAR *ch=0;
	HFILE hFile=-1;
	try
	{
		StreamLength=0,CGeneralMemoryUtility::DeleteAll(DataStream);
		if(!FileName ) return false;
		hFile=_open((char*)FileName,_O_RDONLY|_O_BINARY); 
		if(hFile==-1) return false;
		__int64 FileLen=-1L;
		if(hFile==-1 || (FileLen=_lseeki64(hFile,0L,SEEK_END))==-1L) return false;
		if(_lseek(hFile,0L,SEEK_SET),!AllocateMem((void**)&ch,(sizeof(TCHAR)*(FileLen+1))+DataOffset)) throw MADE_ERROR;
		if(_read(hFile,(void*)((BYTE*)ch+DataOffset),(UINT64)FileLen)!=FileLen) throw MADE_ERROR;
		_close(hFile);
		*DataStream=(void*)ch,StreamLength=DataOffset+FileLen;
		return true;
	}
	catch(...) 
	{
		_close(hFile);
		if(ch) CGeneralMemoryUtility::DeleteAll((void**)&ch);
		return false;
	}
}
//--------------------------------------------------------------------------//



