
/*---EXE Editor
     
	 Purpose:     To embed any type of files in to an EXE and
	              later extract from the exe. Useful for storing
			      configuration files for the exe.

	 Requirement: The program must contain atleast one .rc file.
				  Include the class	'CGeneralMemoryUtility'.

	 Usage      : You must specify resource name and type as you like when embedding,
	              and must use the same when extracting the same resource. You
				  can even do the embedding while the exe is running by giving
				  the module handle for the running exe.

---*/

#include<TCHAR.H>
#include<stdio.H>

class CexeExtraEditor:public CGeneralMemoryUtility
{
protected:
	 bool   m_boolErrorFlag;
	 TCHAR *m_tcharExeName;
	 TCHAR *m_tcharResourceType;
	 TCHAR *m_tcharResourceName;

protected:
	 bool CopyString(TCHAR **Target,TCHAR *Source);
	 void ClearAll(void);
	 bool SetError(void) {  m_boolErrorFlag=true; return false;  }
	 bool prvtExtractFile(HMODULE LibLoaded,TCHAR *ResourceName,TCHAR *ResourceType,TCHAR *FileNameWithExtnToSave);

public:
	 CexeExtraEditor(TCHAR *exeNameWithFullPath);
	 CexeExtraEditor(TCHAR *exeNameWithFullPath,TCHAR *ResourceName,TCHAR *ResourceType);
	 CexeExtraEditor(void);
	 ~CexeExtraEditor();
	 bool ErrorFlag(void) { return this->m_boolErrorFlag; } 
	 bool SetExeFileName(TCHAR *exeNameWithFullPath);
	 bool SetResourceInfo(TCHAR *ResourceName,TCHAR *ResourceType);
	 bool EmbedFileToexe(TCHAR *FileNamewithFullPath);
	 bool ExtractFileFromExe(TCHAR *FileNameWithExtnToSave);
	 bool ExtractFileFromHandle(HMODULE LibLoaded,TCHAR *ResourceName,TCHAR *ResourceType,TCHAR *FileNameWithExtnToSave);
};

void CexeExtraEditor::ClearAll(void)
{
	 DeleteAll((void**)&m_tcharExeName);
	 DeleteAll((void**)&m_tcharResourceName);
	 DeleteAll((void**)&m_tcharResourceType);
	 m_boolErrorFlag=false;
	 return;
}

CexeExtraEditor::~CexeExtraEditor()
{	 
	 ClearAll();
}

bool CexeExtraEditor::CopyString(TCHAR **Target,TCHAR *Source)
{
	 try
	 {
	
			DeleteAll((void**)Target);
			if(!AllocateMem(Target,Source)) return false;
			return true;
	 }
	 catch(...) { Target=0; return false; }
}

CexeExtraEditor::CexeExtraEditor()
{
	 m_tcharExeName=m_tcharResourceType=m_tcharResourceName=0;  
	 m_boolErrorFlag=true;
}

CexeExtraEditor::CexeExtraEditor(TCHAR *exeNameWithFullPath)
{
	m_tcharExeName=m_tcharResourceType=m_tcharResourceName=0;   
	if(!CopyString(&m_tcharExeName,exeNameWithFullPath)) 
			m_boolErrorFlag=true;
}

bool CexeExtraEditor::SetExeFileName(TCHAR *exeNameWithFullPath)
{
	if(ClearAll(),!CopyString(&m_tcharExeName,exeNameWithFullPath)) 
			m_boolErrorFlag=true;
	 return !m_boolErrorFlag;
}

CexeExtraEditor::CexeExtraEditor(TCHAR *exeNameWithFullPath,TCHAR *ResourceName,TCHAR *ResourceType)
{
	    m_tcharExeName=m_tcharResourceType=m_tcharResourceName=0; 
		if(ClearAll(),!CopyString(&m_tcharExeName,exeNameWithFullPath)) 
		 {
				m_boolErrorFlag=true;
		 }
		 else
		 {
				CopyString(&m_tcharResourceName,ResourceName);
				CopyString(&m_tcharResourceType,ResourceType); 
		 }
}

bool CexeExtraEditor::SetResourceInfo(TCHAR *ResourceName,TCHAR *ResourceType)
{
  if(this->m_boolErrorFlag) return false; 
	if(!CopyString(&m_tcharResourceName,ResourceName)) return false;
	if(!CopyString(&m_tcharResourceType,ResourceType)) return false;
	return true;
}

bool CexeExtraEditor::EmbedFileToexe(TCHAR *FileNamewithFullPath)
{
	 void *ch=0;
	 try
	 {
			if(this->m_boolErrorFlag) return SetError(); 
			FILE *f=fopen(FileNamewithFullPath,"rb");
			if(!f) return SetError();
			HANDLE hUpdateRes = BeginUpdateResource(m_tcharExeName,false);
			if(!hUpdateRes) return SetError(); 
			if(fseek(f,0,SEEK_END)!=0) return SetError(); 
			long lngDataLen=ftell(f);
			if(rewind(f),!AllocateMem((void**)&ch,lngDataLen*sizeof(BYTE))) throw -1;
			int intOk=fread((void*)ch,lngDataLen,1,f);
			if(fclose(f),intOk!=1) throw -1;

			BOOL result= UpdateResource(hUpdateRes,m_tcharResourceType,m_tcharResourceName,MAKELANGID(LANG_NEUTRAL, SUBLANG_NEUTRAL), 
						 (void*)ch,(DWORD)lngDataLen);
	 
			if (result == FALSE)  throw -1;
			if (!EndUpdateResource(hUpdateRes,false)) throw -1;
			DeleteAll((void**)&ch);
			return true;
	 }
	 catch(...) { DeleteAll((void**)&ch); return SetError(); } 
}


bool CexeExtraEditor::prvtExtractFile(HMODULE LibLoaded,TCHAR *ResourceName,TCHAR *ResourceType,TCHAR *FileNameWithExtnToSave)
{
	try
	{
	  if(!LibLoaded) return false;
	  HRSRC ResInfo=FindResource(LibLoaded,ResourceName,ResourceType);
	  if(!ResInfo) return false;
	  DWORD SizeOfRes=SizeofResource(LibLoaded,ResInfo);
	  if(SizeOfRes<=0) return false;
	  HGLOBAL Hglobal=LoadResource(LibLoaded,ResInfo);
	  if(!Hglobal) return false;
	  LPVOID lpResdata=LockResource(Hglobal);
	  if(!lpResdata) return false;
	  FILE *filePtr=fopen(FileNameWithExtnToSave,"wb");
	  if(!filePtr) return false;
	  fwrite(lpResdata,SizeOfRes-1,1,filePtr);
	  fclose(filePtr);
		return true;
	}
	catch(...) { return false; }
}

bool  CexeExtraEditor::ExtractFileFromExe(TCHAR *FileNameWithExtnToSave)
{
	if(this->m_boolErrorFlag) return false; 
	HMODULE LibLoaded=LoadLibrary(this->m_tcharExeName); 
	if(!LibLoaded) return SetError();
	if(!(this->prvtExtractFile(LibLoaded,this->m_tcharResourceName,this->m_tcharResourceType,FileNameWithExtnToSave))) return SetError();
	return true;
}

bool CexeExtraEditor::ExtractFileFromHandle(HMODULE LibLoaded,TCHAR *ResourceName,TCHAR *ResourceType,TCHAR *FileNameWithExtnToSave)
{
	 if(!(this->prvtExtractFile(LibLoaded,ResourceName,ResourceType,FileNameWithExtnToSave))) return false;
	 return true;
}