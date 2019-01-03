

class CTString : private CGeneralMemoryUtility 
{
	protected: 
		TCHAR *pvt_tchrString;

	public:
		CTString(void);
		CTString(TCHAR* vptrCharPointer);
		CTString(CTString& robjStringToBeCloned);
	   ~CTString();
		
		CTString& Append(CTString &robjStringToBeAppend);
		CTString& Append(TCHAR* vptrCharPointerToBeAppend);
		long LastIndexOf(TCHAR vtchrCharToFound);
		long IndexOf(TCHAR vtchrCharToFound);
		CTString& Cut(unsigned long vulStartIndex);
		CTString& Cut(unsigned long vulStartIndex,unsigned long vulNumberOfCharactersToCut);
		long Length(void);

		CTString& operator+=(CTString &robjStringToBeAppend);
		CTString& operator+=(TCHAR* vptrCharPointerToBeAppend);
		operator const TCHAR*(void);
		CTString& operator=(CTString &robjStringToBeAssigned);
		CTString& operator=(TCHAR* vptrCharPointer);
		friend CTString& operator+(CTString &robjFirstString,CTString &robjSecondString);
		friend CTString& operator+(CTString &robjFirstString,TCHAR* vptrCharPointer);
		friend CTString& operator+(TCHAR* vptrCharPointer,CTString &robjSecondString);
		TCHAR& operator[](unsigned long vulIndex);
		long operator[](TCHAR vtchrChar);
};

long CTString::Length(void)
{
	if(pvt_tchrString)
	{
		try { return _tcslen(pvt_tchrString); } catch(...) {}
	}
	return -1;
}

CTString& CTString::Cut(unsigned long vulStartIndex)
{
	if(pvt_tchrString && Length()>=vulStartIndex && vulStartIndex>=0)
	{
		try { return CTString((TCHAR*)(pvt_tchrString+vulStartIndex)); }
		catch(...){}
	}
	return CTString();
}

CTString& CTString::Cut(unsigned long vulStartIndex,unsigned long vulNumberOfCharactersToCopy)
{
	 long lStrLen = 0;
	 CTString oNewString;

	 try 
	 {
		lStrLen = Length();
		if(pvt_tchrString && lStrLen>=vulStartIndex && vulStartIndex>=0 && vulNumberOfCharactersToCopy <= (lStrLen - vulStartIndex))
		{
			AllocateMem((TCHAR**)&(oNewString.pvt_tchrString),(TCHAR*)(pvt_tchrString+vulStartIndex),vulNumberOfCharactersToCopy,true);  
		}
	 }
	 catch(...) { return oNewString; }
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

long CTString::operator[](TCHAR vtchrChar)
{
	return IndexOf(vtchrChar);
}

TCHAR& CTString::operator[](unsigned long vulIndex)
{
	TCHAR tchrErr = _T('\0');
	try {	return (TCHAR) pvt_tchrString[vulIndex];	}
	catch(...) { return tchrErr; }
}

CTString& CTString::operator+=(CTString &robjStringToBeAppend)
{
	return (*this).Append(robjStringToBeAppend); 
}

CTString& CTString::operator+=(TCHAR* vptrCharPointerToBeAppend)
{
	return (*this).Append(vptrCharPointerToBeAppend); 
}

CTString& CTString::Append(CTString &robjStringToBeAppend)
{
	ReAllocateString((TCHAR**)&(pvt_tchrString),(TCHAR*)robjStringToBeAppend.pvt_tchrString);  
	return *this;
}

CTString& CTString::Append(TCHAR* vptrCharPointerToBeAppend)
{
	ReAllocateString((TCHAR**)&(pvt_tchrString),(TCHAR*)vptrCharPointerToBeAppend);  
	return *this;
}

CTString& operator+(CTString &robjFirstString,CTString &robjSecondString)
{
	CTString oNewString(robjFirstString);
	return oNewString.Append(robjSecondString); 
}

CTString& operator+(TCHAR* vptrCharPointer,CTString &robjSecondString)
{
	CTString oNewString((TCHAR*)vptrCharPointer);
	return oNewString.Append(robjSecondString); 
}

CTString& operator+(CTString &robjFirstString,TCHAR* vptrCharPointer)
{
	CTString oNewString(robjFirstString);
	return oNewString.Append((TCHAR*)vptrCharPointer); 
}

CTString::operator const TCHAR*(void)
{
	return (TCHAR*)pvt_tchrString;  
}

CTString &CTString::operator=(CTString &robjStringToBeAssigned)
{
	if(AllocateMem((TCHAR**)&pvt_tchrString,(TCHAR*)(robjStringToBeAssigned.pvt_tchrString)))
		return *this;
	else 
		return CTString();
}

CTString &CTString::operator=(TCHAR* vptrCharPointer)
{
	if(AllocateMem((TCHAR**)&pvt_tchrString,(TCHAR*)vptrCharPointer))
		return *this;
	else 
		return CTString();
}

CTString::CTString(void):CGeneralMemoryUtility()
{
	pvt_tchrString = 0;
}

CTString::~CTString(void)
{
	this->DeleteAll((void**)&pvt_tchrString);
}

CTString::CTString(TCHAR* vptrCharPointer):CGeneralMemoryUtility()
{
	pvt_tchrString = 0;
	(*this) = vptrCharPointer;
}

CTString::CTString(CTString& robjStringToBeCloned):CGeneralMemoryUtility()
{
	pvt_tchrString = 0;
	(*this) = robjStringToBeCloned;
}