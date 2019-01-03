
#include<math.h>
#include<malloc.h>

class CGeneralMemoryUtility
{
	private:
		static HANDLE m_hPrivateHeap;

	public:
		static bool SetLocalHeap(HANDLE &HndToHeap);
		static HANDLE &GetLocalHeap(void);

   public:
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
		bool ShowLastError(void);
};


HANDLE CGeneralMemoryUtility::m_hPrivateHeap = (HANDLE)HeapCreate((DWORD)0,100,(DWORD)0) ; 

bool CGeneralMemoryUtility::ShowLastError(void)
{
	LPVOID lpMsgBuf;

	FormatMessage( 
		FORMAT_MESSAGE_ALLOCATE_BUFFER | 
		FORMAT_MESSAGE_FROM_SYSTEM | 
		FORMAT_MESSAGE_IGNORE_INSERTS,
		NULL,
		GetLastError(),
		MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT), // Default language
		(LPTSTR) &lpMsgBuf,
		0,
		NULL 
				);
	// Process any inserts in lpMsgBuf.
	// ...
	// Display the string.
	MessageBox( NULL, (LPCTSTR)lpMsgBuf, "Error", MB_OK | MB_ICONINFORMATION );
	// Free the buffer.
	LocalFree( lpMsgBuf );
	return true;
}

bool CGeneralMemoryUtility::SetLocalHeap(HANDLE &HndToHeap)
{
	m_hPrivateHeap = HndToHeap;
	return true;
}

HANDLE &CGeneralMemoryUtility::GetLocalHeap(void)
{
	return (HANDLE&)m_hPrivateHeap; 
}

bool CGeneralMemoryUtility::ReAllocateString(TCHAR **StringToReallocate,TCHAR *StringToAppend)
{
	if(!(*StringToReallocate)) return AllocateMem(StringToReallocate,StringToAppend);
	try
	{
		UINT32 uintLen=(_tcslen(*StringToReallocate)+_tcslen(StringToAppend)+1)*sizeof(TCHAR);
		_tcscat(*StringToReallocate=(TCHAR*)ReAllocateMemory(*StringToReallocate,uintLen),StringToAppend);
		return true;
	}
	catch(...)
	{
		return false;
	}
}

bool CGeneralMemoryUtility::AllocateMem(void **TarObj,UINT32 NumberOfBytes)
{
	if(*TarObj) try { DeleteAll((void**)TarObj); } catch(...) {}
	*TarObj=(void*)HeapAlloc(m_hPrivateHeap,HEAP_ZERO_MEMORY,NumberOfBytes);
	if(!(*TarObj)) return false; else return true;
}

void *CGeneralMemoryUtility::ReAllocateMemory(void *MemoryBlock,size_t size_tNewSize)
{
	if(MemoryBlock)
		return (void*)HeapReAlloc(m_hPrivateHeap,HEAP_ZERO_MEMORY,MemoryBlock,size_tNewSize);
	else 
		return (void*)HeapAlloc(m_hPrivateHeap,HEAP_ZERO_MEMORY,size_tNewSize);
}

bool CGeneralMemoryUtility::AllocateNew(TCHAR **TarObj,TCHAR *SrcObj,UINT32 NumChars)
{
	try
	{
		//ULONG i;

		if(!SrcObj) return false;
		UINT32 Length=(NumChars?NumChars:_tcslen(SrcObj))+1;
		DeleteNew((void**)TarObj);
		*TarObj=new TCHAR[Length];
		memcpy(*TarObj,SrcObj,Length);
		(*TarObj)[Length-1]=_T('\0');
		return true;
	}
	catch(...) { return false; }

}

bool CGeneralMemoryUtility::CopyBytes(BYTE **Dst,BYTE *Src,UINT32 BytesToCopy)
{
	if(*Dst) try { DeleteAll((void**)Dst); } catch(...) {}
	*Dst=(BYTE*)HeapAlloc(m_hPrivateHeap,HEAP_ZERO_MEMORY,(BytesToCopy));
	if(!(*Dst)) return false;
	else return CopyBytes(*Dst,Src,BytesToCopy);
}

bool CGeneralMemoryUtility::CopyBytes(BYTE *Dst,BYTE *Src,UINT32 BytesToCopy)
{
	try
	{
		memcpy(Dst,Src,BytesToCopy);
		return true;
	}
	catch(...) { return false; }
}

void CGeneralMemoryUtility::DeleteAll(void **ArrayObject)
{
	try
	{	
		if(*ArrayObject && HeapSize(m_hPrivateHeap,0,*ArrayObject) > 0) //!=((sizeof(SIZE_T)) -1)) 
		{
			try 
			{ 
				HeapFree(m_hPrivateHeap,0,*ArrayObject); 
			} 
			catch(...) {}
		}

	}catch(...) {}
	*ArrayObject=0;
	return;
}

void CGeneralMemoryUtility::DeleteNew(void **ArrayObject)
{
	try
	{	if(*ArrayObject && (_msize(*ArrayObject)>0)) 
		{
			try { delete[] (*ArrayObject); } catch(...) {}
		}
	}catch(...) {}
	*ArrayObject=0;
	return;
}

bool CGeneralMemoryUtility::ReplaceChar(TCHAR *TarString,UINT32 NumOfCharsToConsider,TCHAR CharToReplace,TCHAR NewChar)
{
	try
	{
		if(!TarString) return false;

		for(ULONG i=0;i<NumOfCharsToConsider;i++)
			if(TarString[i]==CharToReplace) TarString[i]=NewChar;
		return true;
	}
	catch(...) { return false; }
}

bool CGeneralMemoryUtility::AllocateMem(TCHAR **TarObj,TCHAR *SrcObj,UINT32 NumOfChrsToCopy,bool IsAllocate)
{
	try
	{
		if(!SrcObj) return false;
		if(IsAllocate) 
		{	
			DeleteAll((void**)TarObj);
			*TarObj=(TCHAR*)HeapAlloc(m_hPrivateHeap,HEAP_ZERO_MEMORY,(sizeof(TCHAR)*(NumOfChrsToCopy+1)));
		}
		memcpy(*TarObj,SrcObj,NumOfChrsToCopy);
		(*TarObj)[NumOfChrsToCopy]=_T('\0');
		return true;
	}
	catch(...) { return false; }

}

bool CGeneralMemoryUtility::AllocateMem(TCHAR **TarObj,TCHAR *SrcObj)
{
	try
	{
		if(!SrcObj) return false;
		DeleteAll((void**)TarObj);
		*TarObj=(TCHAR*)HeapAlloc(m_hPrivateHeap,HEAP_ZERO_MEMORY,(sizeof(TCHAR)*(_tcslen(SrcObj)+1)));
		 _tcscpy(*TarObj,SrcObj);
		return true;
	}
	catch(...) { return false; }
}


//----------------------------------------------------------------------//

typedef UINT  (*GetGenericDirectory) (LPTSTR lpBuffer,UINT uiBufferSize);
typedef DWORD (*GetFixedDirectory) (DWORD dwBufferSize,LPTSTR lpBuffer);

class CTString : private CGeneralMemoryUtility 
{
	protected: 
		TCHAR *pvt_tchrString;

	public:
		static CTString GetDirectory(GetGenericDirectory DirectoryRetrievalAPIdelegate);
		static CTString GetDirectory(GetFixedDirectory DirectoryRetrievalAPIdelegate);
		static CTString CreateFolderLocation(CTString VirtualRootFolder,CTString RemainPath);

		CTString(void);
		CTString(unsigned long vulBufferSize);
		CTString(TCHAR* vptrCharPointer);
		CTString(CTString& robjStringToBeCloned);
	   ~CTString();
		
		CTString GetDirectoryPart(void);
		CTString GetFileNamePart(void);
		CTString Append(CTString &robjStringToBeAppend);
		CTString Append(TCHAR* vptrCharPointerToBeAppend);
		long LastIndexOf(TCHAR vtchrCharToFound);
		long IndexOf(TCHAR vtchrCharToFound);
		long IndexOf(TCHAR *vtchrSubStringToFound);
		long IndexOf(CTString &robjSubStringToFound);
		CTString Cut(unsigned long vulStartIndex);
		CTString Cut(unsigned long vulStartIndex,unsigned long vulNumberOfCharactersToCut);
		long Length(void);
		void Clear(void);
		bool SplitByToken(TCHAR *vtchrToken,CTString &robjFirstPart,CTString &robjSecondPart);
		bool SplitByToken(CTString robjToken,CTString &robjFirstPart,CTString &robjSecondPart);
		CTString AttachAnotherFileName(CTString vobjAnotherFileName);
		CTString AttachAnotherFileName(TCHAR *vptrAnotherFileName);
		bool CreatePath(void);
		CTString ToLower(void);
		CTString ToUpper(void);
		CTString LTrim(void);
		CTString RTrim(void);
		CTString Trim(void);
		CTString Replace(TCHAR tcCharInString,TCHAR tcCharNew);
		bool StartsWith(TCHAR *vtchrSubString);
		bool StartsWith(CTString &robjSubString);
		bool EndsWith(TCHAR *vtchrSubString);
		bool EndsWith(CTString &robjSubString);

		CTString operator+=(CTString &robjStringToBeAppend);
		CTString operator+=(TCHAR* vptrCharPointerToBeAppend);
		operator const TCHAR*(void);
		operator TCHAR*(void);
		CTString& operator=(CTString &robjStringToBeAssigned);
		CTString& operator=(TCHAR* vptrCharPointer);
		friend CTString operator+(CTString &robjFirstString,CTString &robjSecondString);
		friend CTString operator+(CTString &robjFirstString,TCHAR* vptrCharPointer);
		friend CTString operator+(TCHAR* vptrCharPointer,CTString &robjSecondString);
		TCHAR& operator[](unsigned long vulIndex);
		int operator==(UINT64 uiAddress);
		int operator!=(UINT64 uiAddress);
		int operator==(CTString vobjStringToBeCompared);
		int operator!=(TCHAR* vptrStringToBeCompared);
		int operator==(TCHAR* vptrStringToBeCompared);
		int operator!=(CTString vobjStringToBeCompared);
};

bool CTString::StartsWith(TCHAR *vtchrSubString)
{
	return IndexOf(vtchrSubString) == 0;
}

bool CTString::StartsWith(CTString &robjSubString)
{
	return StartsWith((TCHAR*)robjSubString);
}

bool CTString::EndsWith(TCHAR *vtchrSubString)
{
	TCHAR *tchrReversedStr = 0; 
	TCHAR *tchrReversedSubStr = 0; 

	tchrReversedStr = _tcsrev((TCHAR*)*this);
	tchrReversedSubStr = _tcsrev((TCHAR*)vtchrSubString);

	return CTString(tchrReversedStr).StartsWith(tchrReversedSubStr); 
}

bool CTString::EndsWith(CTString &robjSubString)
{
	return EndsWith((TCHAR*)robjSubString);
}

CTString CTString::Replace(TCHAR tcCharInString,TCHAR tcCharNew)
{
	if(!pvt_tchrString) return *this;

	CTString oNewStr(*this);
	if(ReplaceChar(oNewStr.pvt_tchrString,oNewStr.Length(),tcCharInString,tcCharNew))
		return oNewStr;

	return CTString(); 
}

CTString CTString::ToLower(void)
{
	if(pvt_tchrString)
	{
		return CTString((TCHAR*)CharLower((TCHAR*)*this));
	}
	return *this;
}

CTString CTString::ToUpper(void)
{
	if(pvt_tchrString)
	{
		return CTString((TCHAR*)CharUpper((TCHAR*)*this));
	}
	return *this;
}

CTString CTString::AttachAnotherFileName(TCHAR *vptrAnotherFileName)
{
	long lLastIndx = LastIndexOf(_T('\\'));
	CTString oAnotherFile;

	if(lLastIndx != -1)
	{
		oAnotherFile = Cut(0,lLastIndx+1);
		return oAnotherFile + vptrAnotherFileName;
	}
	return oAnotherFile;
}

bool CTString::SplitByToken(CTString robjToken,CTString &robjFirstPart,CTString &robjSecondPart)
{
	return SplitByToken((TCHAR*)robjToken,robjFirstPart,robjSecondPart);
}

bool CTString::SplitByToken(TCHAR *vtchrToken,CTString &robjFirstPart,CTString &robjSecondPart)
{
	if(!pvt_tchrString || !vtchrToken) return false;
	long lTokenIndex = IndexOf(vtchrToken);
	if(lTokenIndex == -1)
	{
		robjFirstPart = *this;
		robjSecondPart = (TCHAR*)0;
	}
	else 
	{
		CTString oTmp(vtchrToken);
		robjFirstPart = Cut(0,lTokenIndex); 
		robjSecondPart = Cut(lTokenIndex+oTmp.Length());
	}
	return true;
}

long CTString::IndexOf(TCHAR *vtchrSubStringToFound)
{
	if(!pvt_tchrString || !vtchrSubStringToFound) return -1;
	TCHAR *tchrTokenSeperator= _tcsstr((TCHAR*)(*this),vtchrSubStringToFound);
	if(!tchrTokenSeperator) return -1;
	return abs((long)(tchrTokenSeperator-pvt_tchrString));
}

long CTString::IndexOf(CTString &robjSubStringToFound)
{
	return IndexOf((TCHAR*)robjSubStringToFound.pvt_tchrString);
}

int CTString::operator==(UINT64 uiAddress)
{
	return pvt_tchrString==(TCHAR*)uiAddress;
}

int CTString::operator!=(UINT64 uiAddress)
{
	return pvt_tchrString!=(TCHAR*)uiAddress;
}

int CTString::operator==(CTString vobjStringToBeCompared)
{
	return (*this)==((TCHAR*)vobjStringToBeCompared);
}

int CTString::operator!=(CTString vobjStringToBeCompared)
{
	return (*this)!=((TCHAR*)vobjStringToBeCompared);
}

int CTString::operator!=(TCHAR* vptrStringToBeCompared)
{
	if(!pvt_tchrString || !(vptrStringToBeCompared)) 
	{
		return (*this) != (UINT64)vptrStringToBeCompared;
	}
	return _tcscmp(pvt_tchrString,vptrStringToBeCompared) != 0;
}

int CTString::operator==(TCHAR* vptrStringToBeCompared)
{
	if(!pvt_tchrString || !(vptrStringToBeCompared)) 
	{
		return (*this) == (UINT64)vptrStringToBeCompared;
	}
	return _tcscmp(pvt_tchrString,vptrStringToBeCompared) == 0;
}

void CTString::Clear(void)
{
	DeleteAll((void**)&pvt_tchrString); 
}

long CTString::Length(void)
{
	if(pvt_tchrString)
	{
		try { return _tcslen(pvt_tchrString); } catch(...) {}
	}
	return -1;
}

CTString CTString::Cut(unsigned long vulStartIndex)
{
	if(pvt_tchrString && ((unsigned long)Length())>=vulStartIndex && vulStartIndex>=0)
	{
		try { return CTString((TCHAR*)(pvt_tchrString+vulStartIndex)); }
		catch(...){}
	}
	return CTString();
}

CTString CTString::Cut(unsigned long vulStartIndex,unsigned long vulNumberOfCharactersToCopy)
{
	 long lStrLen = 0;
	 CTString oNewString;

	 try 
	 {
		lStrLen = Length();
		if(pvt_tchrString && (ULONG)lStrLen>=vulStartIndex && vulStartIndex>=0 && vulNumberOfCharactersToCopy <= (lStrLen - vulStartIndex))
		{
			AllocateMem((TCHAR**)&(oNewString.pvt_tchrString),(TCHAR*)(pvt_tchrString+vulStartIndex),vulNumberOfCharactersToCopy,true);  
		}
	 }
	 catch(...) {}
	 return oNewString;
}

long CTString::LastIndexOf(TCHAR vtchrCharToFound)
{
	UINT uintLen;
	if(pvt_tchrString)
	{
		for(uintLen=_tcslen(pvt_tchrString)-1;uintLen>=0;uintLen--)
			if(pvt_tchrString[uintLen] == vtchrCharToFound)
				return (long)uintLen;
	}
	return (long)-1;
}

long CTString::IndexOf(TCHAR vtchrCharToFound)
{
	UINT uintLen,uintMaxLen;
	if(pvt_tchrString)
	{
		for(uintMaxLen =_tcslen(pvt_tchrString)-1,uintLen = 0;uintLen<=uintMaxLen;uintLen++)
			if(pvt_tchrString[uintLen] == vtchrCharToFound)
				return (long)uintLen;
	}
	return (long)-1;
}

TCHAR& CTString::operator[](unsigned long vulIndex)
{
	TCHAR tchrErr = _T('\0');
	try {	return (TCHAR) pvt_tchrString[vulIndex];	}
	catch(...) { return tchrErr; }
}

CTString CTString::operator+=(CTString &robjStringToBeAppend)
{
	return (*this).Append(robjStringToBeAppend); 
}

CTString CTString::operator+=(TCHAR* vptrCharPointerToBeAppend)
{
	return (*this).Append(vptrCharPointerToBeAppend); 
}

CTString CTString::Append(CTString &robjStringToBeAppend)
{
	return Append((TCHAR*)robjStringToBeAppend); 
}

CTString CTString::Append(TCHAR* vptrCharPointerToBeAppend)
{
	if(vptrCharPointerToBeAppend)
	{
		TCHAR *mtchrNewString = 0;
		AllocateMem((TCHAR**)&mtchrNewString,(TCHAR*)pvt_tchrString);  
		ReAllocateString((TCHAR**)&(mtchrNewString),(TCHAR*)vptrCharPointerToBeAppend);  
		DeleteAll((void**)&pvt_tchrString);
		pvt_tchrString = mtchrNewString;
	}
	return *this;
}

CTString operator+(CTString &robjFirstString,CTString &robjSecondString)
{	
	return robjFirstString + (TCHAR*)robjSecondString.pvt_tchrString; 
}

CTString operator+(TCHAR* vptrCharPointer,CTString &robjSecondString)
{
	return CTString(vptrCharPointer) + (TCHAR*)robjSecondString.pvt_tchrString;   
}

CTString operator+(CTString &robjFirstString,TCHAR* vptrCharPointer)
{
	CTString oTmpStr(robjFirstString);
	return oTmpStr.Append((TCHAR*)vptrCharPointer); 
}

CTString::operator TCHAR*(void)
{
	return (TCHAR*)pvt_tchrString;  
}

CTString::operator const TCHAR*(void)
{
	return (TCHAR*)pvt_tchrString;  
}

CTString& CTString::operator=(CTString &robjStringToBeAssigned)
{
	return (*this)=((TCHAR*)robjStringToBeAssigned);
}

CTString& CTString::operator=(TCHAR* vptrCharPointer)
{
	if(vptrCharPointer)
	{
		if(AllocateMem((TCHAR**)&pvt_tchrString,(TCHAR*)vptrCharPointer)) return *this;
	}
	else
	{
		Clear(); 
	}
	return *this;
}

CTString::CTString(void):CGeneralMemoryUtility()
{
	pvt_tchrString = 0;
}

CTString::~CTString(void)
{
	DeleteAll((void**)&pvt_tchrString);
}

CTString::CTString(TCHAR* vptrCharPointer):CGeneralMemoryUtility()
{
	pvt_tchrString = 0;
	AllocateMem((TCHAR**)&pvt_tchrString,(TCHAR*)vptrCharPointer);
}

CTString::CTString(unsigned long vulBufferSize)
{
	pvt_tchrString = 0;
	AllocateMem((void**)&pvt_tchrString,sizeof(TCHAR)*vulBufferSize);
	memset(pvt_tchrString,' ',sizeof(TCHAR)*vulBufferSize);
	pvt_tchrString[vulBufferSize - 1] = _T('\0');
}

CTString::CTString(CTString& robjStringToBeCloned):CGeneralMemoryUtility()
{
	pvt_tchrString = 0;
	AllocateMem((TCHAR**)&pvt_tchrString,(TCHAR*)robjStringToBeCloned);
}

bool CTString::CreatePath(void)
{
	CTString CurPath,PendingPath,CreatePath,tchrTmp;
	CTString thrFILE_SLASH(_T("\\"));

	if(!SplitByToken(thrFILE_SLASH,CurPath,PendingPath)) return false; 
	CreatePath = CurPath + thrFILE_SLASH;

	UINT uintPrevMode=SetErrorMode(SEM_FAILCRITICALERRORS);
	try
	{
		while(PendingPath.SplitByToken(thrFILE_SLASH,CurPath,tchrTmp))
		{
			CreatePath+=CurPath;
			CreateDirectory(CreatePath,0);
			CreatePath += thrFILE_SLASH;
			PendingPath=tchrTmp;
			tchrTmp=0;
		}
	}
	catch(...) {}	
	SetErrorMode(uintPrevMode);
	if(PendingPath == (TCHAR*)0) return true; else return false;
}

CTString CTString::LTrim(void)
{
	TCHAR *tchrPtr = (TCHAR*)(*this);
	while( (*tchrPtr) == _T(' ') ) ++tchrPtr;
	return CTString(tchrPtr); 
}

CTString CTString::RTrim(void)
{
	CTString oNewStr(*this);
	TCHAR *tchrPtr = ((TCHAR*)oNewStr) + oNewStr.Length() - 1;
	while( (*tchrPtr) == _T(' ') ) --tchrPtr;
	return CTString(oNewStr);
}

CTString CTString::Trim(void)
{
	return (*this).LTrim().RTrim();   
}

CTString CTString::GetDirectoryPart(void)
{
	long lIndx = LastIndexOf(_T('\\')); 

	if(lIndx <= 0) return CTString();
	return Cut(0,lIndx);  
}

CTString CTString::GetFileNamePart(void)
{
	long lIndx = LastIndexOf(_T('\\')); 

	if(lIndx <= 0) return *this;
	else 
		if(lIndx >= (Length() - 1)) return CTString();

	return Cut(lIndx + 1);  
}

CTString CTString::GetDirectory(GetFixedDirectory DirectoryRetrievalAPIdelegate)
{
	UINT uintLen=DirectoryRetrievalAPIdelegate(0,0);
	CTString oBuffer(uintLen + 1);	
	DirectoryRetrievalAPIdelegate(uintLen+1,(LPTSTR)oBuffer.pvt_tchrString);
	return oBuffer;
}

CTString CTString::GetDirectory(GetGenericDirectory DirectoryRetrievalAPIdelegate)
{
	UINT uintLen=DirectoryRetrievalAPIdelegate(0,0);
	CTString oBuffer(uintLen + 1);	
	DirectoryRetrievalAPIdelegate((LPTSTR)oBuffer.pvt_tchrString,uintLen+1);
	return oBuffer;
}

CTString CTString::CreateFolderLocation(CTString VirtualRootFolder,CTString RemainPath)
{
	UINT uintLen=0;
	CTString tchrLowerRootDir,tchrLowerRoot,tchrPrevCurDir;

	tchrLowerRootDir = VirtualRootFolder.ToLower();
	if(tchrLowerRootDir == "system")
	{
		tchrLowerRoot = CTString::GetDirectory(&GetSystemDirectory); 
		tchrLowerRoot += _T("\\");
	}
	else
	{
		tchrPrevCurDir  = CTString::GetDirectory(&GetCurrentDirectory); 
		tchrLowerRoot = CTString::GetDirectory(&GetWindowsDirectory); 
		tchrLowerRoot += _T("\\..\\");

		SetCurrentDirectory(tchrLowerRoot);
		tchrLowerRoot  = CTString::GetDirectory(&GetCurrentDirectory); 
		SetCurrentDirectory(tchrPrevCurDir);

		if(tchrLowerRootDir == "program files")
			tchrLowerRoot += _T("program files\\");
		else
			if(tchrLowerRootDir == "common files")
				tchrLowerRoot += _T("program files\\common files\\");
	}
	return tchrLowerRoot + RemainPath;
}
