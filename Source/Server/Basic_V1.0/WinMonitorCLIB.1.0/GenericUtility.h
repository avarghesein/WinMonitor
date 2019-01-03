
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
		bool GetToken(TCHAR *Heirarchy,TCHAR *TokenSeperator,TCHAR **tchrToken,TCHAR **tchrSubHeirarchy);
		bool CreatePathFromRoot(TCHAR *Heirarchy);
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
		bool SplitToPathAndFileName(TCHAR *FullFileName,TCHAR **Path,TCHAR **FileName);
		int GetLastIndexOfFileSlash(TCHAR *FullFileName);
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
		//for(i=0;i<Length-1;i++) (*TarObj)[i]=SrcObj[i];
		//(*TarObj)[i]=_T('\0');
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
		/*for(UINT32 i=0;i<BytesToCopy;i++)
			Dst[i]=Src[i];*/
		return true;
	}
	catch(...) { return false; }
}

void CGeneralMemoryUtility::DeleteAll(void **ArrayObject)
{
	try
	{	if(*ArrayObject && HeapSize(m_hPrivateHeap,0,*ArrayObject) !=((sizeof(SIZE_T)) -1)) 
		{
			try { HeapFree(m_hPrivateHeap,0,*ArrayObject); } catch(...) {}
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
		ULONG i;
		if(!SrcObj) return false;
		if(IsAllocate) 
		{	
			DeleteAll((void**)TarObj);
			*TarObj=(TCHAR*)HeapAlloc(m_hPrivateHeap,HEAP_ZERO_MEMORY,(sizeof(TCHAR)*(NumOfChrsToCopy+1)));
		}
		memcpy(*TarObj,SrcObj,NumOfChrsToCopy);
		(*TarObj)[NumOfChrsToCopy]=_T('\0');
		/*for(i=0;i<NumOfChrsToCopy;i++) (*TarObj)[i]=SrcObj[i];
		(*TarObj)[i]=_T('\0');*/
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

bool CGeneralMemoryUtility::CreatePathFromRoot(TCHAR *Heirarchy)
{
	TCHAR *CurPath=0,*PendingPath=0,*CreatePath=0,*tchrTmp=0;
	TCHAR thrFILE_SLASH[3]=_T("\\");

	CreatePath=new TCHAR[sizeof(TCHAR)*(_tcslen(Heirarchy)+5)];
	
	if(!CreatePath) return false;

	if(!GetToken(Heirarchy,thrFILE_SLASH,&CurPath,&PendingPath)) return false;
	_tcscat(_tcscpy(CreatePath,CurPath),thrFILE_SLASH);
	
	UINT uintPrevMode=SetErrorMode(SEM_FAILCRITICALERRORS);
	
	try
	{
		while(GetToken(PendingPath,thrFILE_SLASH,&CurPath,&tchrTmp))
		{
			_tcscat(CreatePath,CurPath);
			CreateDirectory(CreatePath,0);
			_tcscat(CreatePath,thrFILE_SLASH);
			PendingPath=tchrTmp;
			tchrTmp=0;
		}
	}
	catch(...) {}
	
	SetErrorMode(uintPrevMode);
		
	CGeneralMemoryUtility::DeleteAll((void**)&PendingPath);
	DeleteNew((void**)&CreatePath);
	if(!PendingPath) return true; else return false;
}

bool CGeneralMemoryUtility::GetToken(TCHAR *Heirarchy,TCHAR *TokenSeperator,TCHAR **tchrToken,TCHAR **tchrSubHeirarchy)
{
	 if(!Heirarchy) return false;
	 try
	 {
			TCHAR *tchrTokenSeperator= _tcsstr(Heirarchy,TokenSeperator);

			CGeneralMemoryUtility::DeleteAll((void**)tchrToken);
			CGeneralMemoryUtility::DeleteAll((void**)tchrSubHeirarchy);

			if(tchrTokenSeperator==NULL) 
			{
				 *tchrSubHeirarchy=NULL;
				 *tchrToken=(TCHAR*)HeapAlloc(m_hPrivateHeap,HEAP_ZERO_MEMORY,(sizeof(TCHAR)*(_tcslen(Heirarchy)+1))); 
				 if(!(*tchrToken)) return false;
				 if(!_tcscpy(*tchrToken,Heirarchy)) return false;
			}
			else
			{
				 int lngIndexOfTokenSeperator=abs((long)(tchrTokenSeperator-Heirarchy));
				 *tchrSubHeirarchy=(TCHAR*)HeapAlloc(m_hPrivateHeap,HEAP_ZERO_MEMORY,(sizeof(TCHAR)*(1+ _tcslen((TCHAR*)(tchrTokenSeperator+_tcslen(TokenSeperator))))));
				 if(!(*tchrSubHeirarchy)) return false;
				 if(!_tcscpy(*tchrSubHeirarchy,(TCHAR*)(tchrTokenSeperator+_tcslen(TokenSeperator)))) return false;
				 if(lngIndexOfTokenSeperator==0)
				 {
						*tchrToken=(TCHAR*)HeapAlloc(m_hPrivateHeap,HEAP_ZERO_MEMORY,(sizeof(TCHAR)*2));
						if(!(*tchrToken)) return false;
						if(!_tcscpy(*tchrToken,_T(""))) return false;

				 }
				 else
				 {
						*tchrToken=(TCHAR*)HeapAlloc(m_hPrivateHeap,HEAP_ZERO_MEMORY,(sizeof(TCHAR)*(lngIndexOfTokenSeperator+1)));
						//for(int i=0;i<lngIndexOfTokenSeperator;i++) (*tchrToken)[i]=Heirarchy[i];
						memcpy(*tchrToken,Heirarchy,sizeof(TCHAR)*lngIndexOfTokenSeperator);
						(*tchrToken)[lngIndexOfTokenSeperator]=(TCHAR)'\0';
				 }
			 }
			 return true;
	 }
	 catch(...)
	 {
			return false;
	 }
}

int CGeneralMemoryUtility::GetLastIndexOfFileSlash(TCHAR *FullFileName)
{
	if(!FullFileName) return -1;

	TCHAR prevChr=0; UINT uintLen;

	try
	{
		for(uintLen=_tcslen(FullFileName)-1;uintLen>=0;uintLen--)
			if(FullFileName[uintLen]==_T('\\')) return (int)uintLen;
		
		return -1;
	}
	catch(...) { return -1; }
}

bool CGeneralMemoryUtility::SplitToPathAndFileName(TCHAR *FullFileName,TCHAR **Path,TCHAR **FileName)
{
	try
	{
		int indxOfSlash=GetLastIndexOfFileSlash(FullFileName);
		if(indxOfSlash==-1) return false;
		CGeneralMemoryUtility memUty;
		memUty.AllocateMem(Path,FullFileName,indxOfSlash+1,true);
		memUty.AllocateMem(FileName,&FullFileName[indxOfSlash+1]);
		return true;
	}
	catch(...) { return false; }
}