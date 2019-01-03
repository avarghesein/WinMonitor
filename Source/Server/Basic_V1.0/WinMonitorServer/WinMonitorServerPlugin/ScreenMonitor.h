

class CScreenMonitorBase:public INetworkByteStream,public CGeneralMemoryUtility   
{
protected:
	 bool m_boolIsLoaded;
	 BYTE m_bytTypeCompression;
	 
	 static HPALETTE m_hPalette;
	 HDC m_hdcDc;
	 HBITMAP m_hbitmapImage;
	 BITMAPFILEHEADER m_bitmapFileHeader; 
	 BITMAPINFO *m_bitmapInfoHeader;
	 DWORD m_dwInfoHeaderLen;
	 BYTE *m_bytptrDIBdata;

protected:
	 void Init(void);
	 void DestroyInfoHeader(void);
	 bool CreateInfoHeader(unsigned long InfoSize);
	 bool CaptureImage(HDC HandleToDeviceContext,bool IsWnd=false,HWND hWnd=0);
	 bool ReplaceImage(HDC HandleToDeviceContext,bool IsWnd=false,HWND hWnd=0);
	 bool SetHeaders(void);
	 bool LoadPalette(HDC hdcHdc);
	 bool LoadBitmapFromCompressdByteStream(BYTE TypeAndCompress,void *StreamCompressed,UINT32 StreamLength,void **StremUnCompressed,UINT32 *UnCompressedLength,BYTE CompressionType);
	 bool LoadBitmapFromByteStream(void *ByteStream);
	 BITMAPFILEHEADER BmpFileHeaderFromBitmapHeader(BITMAPINFO *ptrToBmpInfo,DWORD &BmpInfoHdrLength); 
     
public:
	  CScreenMonitorBase(BYTE Compression=SCREEN_MONITOR_COMPRESS_LZSS);
	 ~CScreenMonitorBase();
	 
	 bool SetCompression(BYTE Compression);
	 bool SetType(BYTE TransferItemType);

	 bool CaptureDesktopImage(void);
	 bool CaptureWindowImage(HWND hWnd=0);
	 bool SaveBitmapToFile(TCHAR *FileName,bool IsCompress=false);
     
	 bool ReplaceDesktopImage(void);
	 bool ReplaceWindowImage(HWND hWnd=0);
	 bool LoadBitmapFromFile(TCHAR *FileName,bool IsCompressed=false);

	 UINT32 WriteToNetworkByteStream(BYTE* &OutByteStream);
	 UINT32 ReadFromNetworkByteStream(BYTE *InByteStream);
};

HPALETTE CScreenMonitorBase::m_hPalette=0;   

bool CScreenMonitorBase::SetCompression(BYTE Compresion)
{
	m_bytTypeCompression = SCREEN_MONITOR_SETCOMPRESSION(Compresion,m_bytTypeCompression);
	return true;
}

bool CScreenMonitorBase::SetType(BYTE TransferItemType)
{
	m_bytTypeCompression = SCREEN_MONITOR_SETTYPE(TransferItemType,m_bytTypeCompression);   
	return true;
}

bool CScreenMonitorBase::LoadBitmapFromCompressdByteStream(BYTE TypeAndCompress,void *StreamCompressed,UINT32 StreamLength,void **StreamUnCompressed,UINT32 *UnCompressedLength,BYTE CompressionType)
{
	if(SCREEN_MONITOR_GETTYPE(TypeAndCompress)==SCREEN_MONITOR_TYPE_UNDEF) return false; 
	
	switch(CompressionType)
	{
		case SCREEN_MONITOR_COMPRESS_LZSS:
			CLzhCompress CPress;
			CPress.lzh_melt_memory(StreamCompressed,StreamLength,StreamUnCompressed,(int*)(UnCompressedLength));
			break;
	
		case SCREEN_MONITOR_COMPRESS_NIL:
		default:
			return false;
	}

	if((*UnCompressedLength)==0 || !(*StreamUnCompressed)) 
	{
		*StreamUnCompressed=0;
		return false;
	}
	if(SCREEN_MONITOR_GETTYPE(TypeAndCompress)==SCREEN_MONITOR_TYPE_DIBITMAPFILE)  
		if(LoadBitmapFromByteStream((void*)((BYTE*)((BYTE*)(*StreamUnCompressed))+sizeof(BITMAPFILEHEADER)))) return true;   
		else return false;
	else if(SCREEN_MONITOR_GETTYPE(TypeAndCompress)==SCREEN_MONITOR_TYPE_DIBITMAP)
		if(!LoadBitmapFromByteStream((void*)((BYTE*)((BYTE*)(*StreamUnCompressed))))) return false;   
		else return true;
	else return false;
}

UINT32 CScreenMonitorBase::ReadFromNetworkByteStream(BYTE *InByteStream)
{
	try
	{
		BYTE *InStream=InByteStream+sizeof(ScreenMonitorNetworkeader);
		ScreenMonitorNetworkeader Header;
		Header=*((ScreenMonitorNetworkeader*)InByteStream);
		if(SCREEN_MONITOR_GETTYPE(Header.TypeAndCompression)==SCREEN_MONITOR_TYPE_UNDEF) return 0;
		if((SCREEN_MONITOR_GETTYPE(Header.TypeAndCompression)==SCREEN_MONITOR_TYPE_TAKE_DESKTOP && CaptureDesktopImage()) ||
		   (SCREEN_MONITOR_GETTYPE(Header.TypeAndCompression)==SCREEN_MONITOR_TYPE_TAKE_WINDOW &&  CaptureWindowImage ()))
				return sizeof(ScreenMonitorNetworkeader);
		if(SCREEN_MONITOR_GETTYPE(Header.TypeAndCompression)==SCREEN_MONITOR_TYPE_SET_COMPRESSION)
		{
			BYTE Pbyt=*((BYTE*)(InByteStream+sizeof(ScreenMonitorNetworkeader)));
			SetCompression(Pbyt);
			return (sizeof(ScreenMonitorNetworkeader)+sizeof(BYTE));
		}

		BYTE *bytUnCompressed; int intUnCompressLen;
		switch(SCREEN_MONITOR_GETCOMPRESSION(Header.TypeAndCompression))
		{
			case SCREEN_MONITOR_COMPRESS_LZSS:
				m_boolIsLoaded=LoadBitmapFromCompressdByteStream(Header.TypeAndCompression,InStream,(int)Header.Length,(void**)(&bytUnCompressed),(UINT32*)&intUnCompressLen,SCREEN_MONITOR_GETCOMPRESSION(Header.TypeAndCompression));
				DeleteAll((void**)&bytUnCompressed);
				return (m_boolIsLoaded?sizeof(ScreenMonitorNetworkeader)+Header.Length:0);

			case SCREEN_MONITOR_COMPRESS_NIL:
			default:
				break;

		}
			
		if(SCREEN_MONITOR_GETTYPE(Header.TypeAndCompression)==SCREEN_MONITOR_TYPE_DIBITMAPFILE)  
			m_boolIsLoaded=LoadBitmapFromByteStream((void*)((BYTE*)((BYTE*)InStream)+sizeof(BITMAPFILEHEADER)));   
		else  if(SCREEN_MONITOR_GETTYPE(Header.TypeAndCompression)==SCREEN_MONITOR_TYPE_DIBITMAP)
			m_boolIsLoaded=LoadBitmapFromByteStream((void*)((BYTE*)((BYTE*)InStream)));   
		else m_boolIsLoaded=false; 
	
		return m_boolIsLoaded?sizeof(ScreenMonitorNetworkeader)+Header.Length:0; 
	}
	catch(...) { m_boolIsLoaded=false; return 0; }
}

UINT32 CScreenMonitorBase::WriteToNetworkByteStream(BYTE* &OutByteStream)
{
	try
	{
		if(!m_boolIsLoaded || SCREEN_MONITOR_GETTYPE(m_bytTypeCompression)==SCREEN_MONITOR_TYPE_UNDEF) return 0; 

		ScreenMonitorNetworkeader Header;
		Header.TypeAndCompression =m_bytTypeCompression;
		BYTE *DataToTransfer=0;
		if(!AllocateMem((void**)&DataToTransfer,sizeof(BYTE)*m_bitmapFileHeader.bfSize)) return 0;
		BYTE *OStream=DataToTransfer;
		UINT32 uintByteCopied=0;

		if(SCREEN_MONITOR_GETTYPE(Header.TypeAndCompression) ==SCREEN_MONITOR_TYPE_DIBITMAPFILE)  
			*((BITMAPFILEHEADER*)OStream)=m_bitmapFileHeader,OStream+=(uintByteCopied+=sizeof(BITMAPFILEHEADER));
		//unsigned int Count;

		memcpy(OStream,(BYTE*)m_bitmapInfoHeader,m_dwInfoHeaderLen); OStream += m_dwInfoHeaderLen;
		////for(Count=0;Count<m_dwInfoHeaderLen;Count++) *OStream++=((BYTE*)m_bitmapInfoHeader)[Count];
		memcpy(OStream,m_bytptrDIBdata,m_bitmapFileHeader.bfSize-m_dwInfoHeaderLen); uintByteCopied+=m_bitmapFileHeader.bfSize-m_dwInfoHeaderLen;
		//OStream+=m_bitmapFileHeader.bfSize-m_dwInfoHeaderLen;
		////for(uintByteCopied+=Count,Count=0;Count<m_bitmapFileHeader.bfSize-m_dwInfoHeaderLen ;Count++) *OStream++=m_bytptrDIBdata[Count];
		////uintByteCopied+=Count;

		BYTE *DataCompressed=0;
		int intBytesCompressed;
		bool CompressFlg=false;

		switch(SCREEN_MONITOR_GETCOMPRESSION(Header.TypeAndCompression))
		{
			case SCREEN_MONITOR_COMPRESS_LZSS:
				CLzhCompress CPress;
				CPress.lzh_freeze_memory((void*)DataToTransfer,uintByteCopied,(void**) &DataCompressed,(int*)&intBytesCompressed);
				if(intBytesCompressed==0) return 0;
				OStream=DataCompressed;
				uintByteCopied=intBytesCompressed;
				CompressFlg=true;
				break;
	
			case  SCREEN_MONITOR_COMPRESS_NIL:
			default: 
				//uintByteCopied=Count; 
				break;
		}

		Header.Length=uintByteCopied;  

		DeleteAll((void**)&OutByteStream);
		if(!AllocateMem((void**)&OutByteStream,sizeof(BYTE)*(sizeof(WinMonitorNetworkServiceHeader)+sizeof(ScreenMonitorNetworkeader)+Header.Length))) return 0;
	
		BYTE *OutStream=(BYTE*)(OutByteStream+sizeof(WinMonitorNetworkServiceHeader));
		*((ScreenMonitorNetworkeader*)OutStream)=Header;
		OutStream+=sizeof(ScreenMonitorNetworkeader);

		memcpy(OutStream,OStream,uintByteCopied);
		//for(Count=0;Count<uintByteCopied;Count++) *OutStream++=OStream[Count];

		if(CompressFlg) DeleteAll((void**)&DataCompressed);
		DeleteAll((void**)&DataToTransfer); 

		try
		{
			Init();	 
			try
			{
				this->DestroyInfoHeader(); 
				if(m_hPalette) DeleteObject(m_hPalette),m_hPalette=0;   
			}
			catch(...) {}   
		}
		catch(...) {}

		return uintByteCopied+sizeof(ScreenMonitorNetworkeader);  

	}
	catch(...) { return 0; }
}


bool CScreenMonitorBase::CaptureImage(HDC HandleToDeviceContext,bool IsWnd,HWND hWnd)
{
	HDC Mem;
	HBITMAP old;
	RECT wind;
	try
	{
		Mem=CreateCompatibleDC(HandleToDeviceContext);
			
		if(IsWnd) GetClientRect(hWnd,&wind);
		else
            wind.left=wind.top=0,
			wind.right=GetDeviceCaps(HandleToDeviceContext,HORZRES),
			wind.bottom= GetDeviceCaps(HandleToDeviceContext,VERTRES);
		try
		{
			if(m_hbitmapImage != 0) DeleteObject(m_hbitmapImage),m_hbitmapImage=0;   
		}
		catch(...) {}

		m_hbitmapImage=CreateCompatibleBitmap(HandleToDeviceContext,wind.right-wind.left,wind.bottom-wind.top);
		old=(HBITMAP)SelectObject(Mem,m_hbitmapImage);
		StretchBlt(Mem,0,0,wind.right-wind.left,wind.bottom-wind.top,HandleToDeviceContext,wind.left,wind.top,wind.right-wind.left,wind.bottom-wind.top,SRCCOPY);
		m_hbitmapImage=(HBITMAP)SelectObject(Mem,old);

		DeleteDC(Mem);
		if(SetHeaders()) return true;
		else return false;
	}
	catch(...)
	{
		return false;
	}
}

bool CScreenMonitorBase::CreateInfoHeader(unsigned long InfoSize)
{
	 DestroyInfoHeader();
	 try
	 {		if(!AllocateMem((void**)&m_bitmapInfoHeader,sizeof(unsigned char)*InfoSize)) return false;
			return true;
	 }
	 catch(...) { return false; } 
}

void CScreenMonitorBase::DestroyInfoHeader(void)
{	DeleteAll((void**)&m_bitmapInfoHeader);	   }

bool CScreenMonitorBase::LoadPalette(HDC hdcHdc)
{
	if(!m_boolIsLoaded) return false; 
	int intClrUsed=0;
	try
	{
		if((intClrUsed=m_bitmapInfoHeader->bmiHeader.biClrUsed)==0)    
		switch(m_bitmapInfoHeader->bmiHeader.biBitCount)
		{
				case 1: intClrUsed=2;break;
				case 4: intClrUsed=16;break;
				case 8: intClrUsed=256;break;
				case 24: default:intClrUsed=0;
		}

		HANDLE hPal=GlobalAlloc(GMEM_MOVEABLE,sizeof(LOGPALETTE)+intClrUsed*sizeof(PALETTEENTRY));
		LPLOGPALETTE hpPal=(LPLOGPALETTE) GlobalLock(hPal);
		hpPal->palVersion=0x300;
		hpPal->palNumEntries=intClrUsed;
		for(int Counter=0;Counter<intClrUsed;Counter++)
			hpPal->palPalEntry[Counter].peRed=m_bitmapInfoHeader->bmiColors[Counter].rgbRed,    
			hpPal->palPalEntry[Counter].peGreen=m_bitmapInfoHeader->bmiColors[Counter].rgbGreen ,    
			hpPal->palPalEntry[Counter].peBlue =m_bitmapInfoHeader->bmiColors[Counter].rgbBlue;

		if(m_hPalette) DeleteObject(m_hPalette),m_hPalette=0;   
		m_hPalette=CreatePalette(hpPal);
		GlobalUnlock(hPal),GlobalFree(hPal);
		
		SelectPalette(hdcHdc,m_hPalette,false);
		RealizePalette(hdcHdc);
		return true;
	}
	catch(...) { return false; }
}

CScreenMonitorBase::~CScreenMonitorBase()
{ 
	 Init();	 
}

void CScreenMonitorBase::Init(void)
{
	 m_boolIsLoaded=false;
	 DeleteAll((void**)&m_bytptrDIBdata) ; 
	 if(m_hbitmapImage)  
	 	try { DeleteObject(m_hbitmapImage); } catch(...) {} 
	 if(m_hdcDc)
	 	try { DeleteDC(m_hdcDc); } catch(...) {}
	 
	 try
	 {
		this->DestroyInfoHeader(); 
		if(m_hPalette) DeleteObject(m_hPalette),m_hPalette=0;   
	 }
	 catch(...) {}
	 
	 m_hbitmapImage=0; 
	 m_hdcDc=0;
	 m_bytptrDIBdata=0;
	 m_dwInfoHeaderLen=0;
	 m_bytTypeCompression=SCREEN_MONITOR_SETTYPE(SCREEN_MONITOR_TYPE_DIBITMAPFILE, m_bytTypeCompression);  
	 m_bytTypeCompression=SCREEN_MONITOR_SETCOMPRESSION(SCREEN_MONITOR_COMPRESS_LZSS, m_bytTypeCompression);   
	 return;
}

CScreenMonitorBase::CScreenMonitorBase(BYTE Compression)
{
	 m_bytptrDIBdata=0,m_hbitmapImage=0,m_hdcDc=0,m_bitmapInfoHeader=0,Init();
	 AllocateMem((void**)&m_bitmapInfoHeader,sizeof(unsigned char)*(sizeof(BITMAPINFO)+1024));
	 m_bytTypeCompression=SCREEN_MONITOR_SETCOMPRESSION(Compression,m_bytTypeCompression);  
}


bool CScreenMonitorBase::CaptureDesktopImage(void)
{
	  if(!m_hdcDc) try { DeleteDC(m_hdcDc); m_hdcDc=0; } catch(...) {}

		try
		{
	 		m_hdcDc=CreateDC("DISPLAY",NULL,NULL,NULL);

			if(CaptureImage(m_hdcDc))  m_boolIsLoaded=true;
			else m_boolIsLoaded=false;
		}
		catch(...) { m_boolIsLoaded=false; }
		
		return m_boolIsLoaded;
}

bool CScreenMonitorBase::ReplaceDesktopImage(void)
{
	 if(!this->m_boolIsLoaded) return false;
	 try
	 {		
				m_hdcDc=CreateDC("DISPLAY",NULL,NULL,NULL);
				if(ReplaceImage(m_hdcDc,false,false)) return true;
				else return false;
	 }
	 catch(...) { return false; }
}

bool CScreenMonitorBase::ReplaceImage(HDC HandleToDeviceContext,bool IsWnd,HWND hWnd)
{

	RECT wind;
	try
	{
			
		if(IsWnd) GetClientRect(hWnd,&wind);
		else
      wind.left=wind.top=0,
	  wind.right=GetDeviceCaps(HandleToDeviceContext,HORZRES),
	  wind.bottom= GetDeviceCaps(HandleToDeviceContext,VERTRES);
	  LoadPalette(HandleToDeviceContext); 	
	  StretchDIBits(HandleToDeviceContext,wind.left,wind.top,wind.right-wind.left,wind.bottom-wind.top,
			        0,0,m_bitmapInfoHeader->bmiHeader.biWidth,m_bitmapInfoHeader->bmiHeader.biHeight,m_bytptrDIBdata,
					(BITMAPINFO*)m_bitmapInfoHeader,BI_RGB,SRCCOPY);  
		
		return true;
	}
	catch(...)
	{
		return false;
	}
}

bool CScreenMonitorBase::ReplaceWindowImage(HWND hWnd)
{
	 if(!this->m_boolIsLoaded) return false;
	 try
	{		
		 if(!hWnd) hWnd=GetActiveWindow();
		 if(!hWnd) hWnd=GetForegroundWindow();
		 m_hdcDc=GetDC(hWnd); 
		 if(ReplaceImage(m_hdcDc,true,hWnd)) return true;
		 else return false;
	}
	catch(...) { return false; }
}


bool CScreenMonitorBase::CaptureWindowImage(HWND hWnd)
{
	  
	if(!m_hdcDc) try { DeleteDC(m_hdcDc); m_hdcDc=0; } catch(...) {}
	try
	{		
		 if(!hWnd) hWnd=GetActiveWindow();
		 if(!hWnd) hWnd=GetForegroundWindow();
		 m_hdcDc=GetDC/*WindowDC*/(hWnd); 
  		 if(CaptureImage(m_hdcDc,true,hWnd)) m_boolIsLoaded=true;
	 	 else m_boolIsLoaded=false;
	}
	catch(...) { m_boolIsLoaded=false; }
		
	return m_boolIsLoaded;
}

bool CScreenMonitorBase::SaveBitmapToFile(TCHAR *FileName,bool IsCompress)
{
	if(!this->m_boolIsLoaded) return false;

	FILE *hFile; 

	try
	{
		if(!IsCompress)
		{
			hFile =fopen(FileName,"wb"); 
			fwrite((LPCSTR)&m_bitmapFileHeader,sizeof(BITMAPFILEHEADER),1,hFile);
			fwrite((LPSTR)m_bitmapInfoHeader,(UINT)m_dwInfoHeaderLen,1,hFile);
			fwrite((LPSTR)m_bytptrDIBdata,m_bitmapInfoHeader->bmiHeader.biSizeImage,1,hFile);	
			fclose(hFile);
			return true;
		}
		else if(SCREEN_MONITOR_GETCOMPRESSION(m_bytTypeCompression)==SCREEN_MONITOR_COMPRESS_LZSS)
		{	
			BYTE *Buff=0;
			if(!AllocateMem((void**)&Buff,sizeof(BYTE)*m_bitmapFileHeader.bfSize)) return false;
			UINT32 uintWritten=WriteToNetworkByteStream(Buff);
			if(uintWritten==0) return false;
			hFile =fopen(FileName,"wb"); 
			bool boolOk=fwrite((const void*)Buff,uintWritten,1,hFile)<= 0 ? false:true;
			fclose(hFile);
			DeleteAll((void**)&Buff);
			if(boolOk) return true; else return false;
    	}
		return false;
	}
	catch(...)
	{		
		try { fclose(hFile); } catch(...) {}
		return false;
	}
}

bool CScreenMonitorBase::LoadBitmapFromFile(TCHAR *FileName,bool IsCompressed)
{

	FILE *hFile; 
	BYTE *pDib=0;
	long FileLength=0;
	ULONG DibDataAndHeaderLength;
	int intLoadOk=false;
	
	try
	{
		if(!IsCompressed)
		{
			hFile = fopen(FileName,"rb");
			if(!hFile) return false;
			
			fseek(hFile,0,SEEK_END);FileLength=ftell(hFile);rewind(hFile);
			DibDataAndHeaderLength=FileLength-sizeof(BITMAPFILEHEADER);
			if(!AllocateMem((void**)&pDib,sizeof(BYTE)*DibDataAndHeaderLength)) return false;
	
			if(pDib)
			  if(fread((void*)&m_bitmapFileHeader,sizeof(BITMAPFILEHEADER),1,hFile)==1)
				if(m_bitmapFileHeader.bfType=='MB') 
				   if(fread((void*)pDib,DibDataAndHeaderLength,1,hFile)==1)
				   {
						if(fclose(hFile),LoadBitmapFromByteStream(pDib)) m_boolIsLoaded=true;
						else m_boolIsLoaded=false;
						DeleteAll((void**)&pDib);
						return m_boolIsLoaded; 
				   }
		   DeleteAll((void**)&pDib);fclose(hFile);
		   return intLoadOk? true : false;
		}
		else 
		{
			hFile = fopen(FileName,"rb");
			fseek(hFile,0,SEEK_END);
			long lngEndPos=ftell(hFile); rewind(hFile);
			BYTE *Buff=0;
			if(!AllocateMem((void**)&Buff,sizeof(BYTE)*(lngEndPos+1))) return false;
			int intOk=(int)fread((void*)Buff,lngEndPos,1,hFile);
			fclose(hFile);
			if(intOk==1)
				if(ReadFromNetworkByteStream((BYTE*)(Buff+sizeof(WinMonitorNetworkServiceHeader)))!=0) intOk=true;
				else intOk=(int)false;
			else intOk=(int)false;
			DeleteAll((void**)&Buff);
		   return intLoadOk? true : false;
		}
	}
	catch(...)
	{		
		 fclose(hFile);
		 return false;
	}
}

bool CScreenMonitorBase::LoadBitmapFromByteStream(void *ByteStream)
{
	try
	{
		m_bitmapFileHeader=BmpFileHeaderFromBitmapHeader((BITMAPINFO*)ByteStream,m_dwInfoHeaderLen); 
		BITMAPINFO *bmi=0;
		if(!AllocateMem((void**)&bmi,m_dwInfoHeaderLen)) return false;
		if(!CopyBytes((BYTE*)bmi,(BYTE*)ByteStream,m_dwInfoHeaderLen))
		{
			DeleteAll((void**)&bmi);  
			return false;
		}
		DestroyInfoHeader();
		m_bitmapInfoHeader=bmi;
		DeleteAll((void**)&m_bytptrDIBdata);
		if(!AllocateMem((void**)&m_bytptrDIBdata,sizeof(BYTE)*(bmi->bmiHeader.biSizeImage))) return false;
		if(!CopyBytes((BYTE*)m_bytptrDIBdata,(BYTE*) &(((BYTE*)ByteStream)[m_dwInfoHeaderLen]),bmi->bmiHeader.biSizeImage))
		{
			DeleteAll((void**)&m_bytptrDIBdata);
			return false;
		} //-------changed  
		return true;
	}
	catch(...) { return false; }
}

bool CScreenMonitorBase::SetHeaders(void)
{
	    BITMAPINFO *pbmi;

		try
		{	
			if(!AllocateMem((void**)&pbmi,sizeof(unsigned char)*(sizeof(BITMAPINFO)+1024))) return false;
			pbmi->bmiHeader.biSize = sizeof(BITMAPINFOHEADER);
			pbmi->bmiHeader.biBitCount = 0;
			GetDIBits(m_hdcDc, m_hbitmapImage, 0, 0, (LPSTR)NULL, pbmi, DIB_RGB_COLORS);
					
			DeleteAll((void**)&m_bytptrDIBdata);
			if(!AllocateMem((void**)&m_bytptrDIBdata,pbmi->bmiHeader.biSizeImage)) return false;
			
			GetDIBits(m_hdcDc, m_hbitmapImage, 0, pbmi->bmiHeader.biHeight, (LPSTR)m_bytptrDIBdata, pbmi, DIB_RGB_COLORS);

			m_bitmapFileHeader=BmpFileHeaderFromBitmapHeader(pbmi,m_dwInfoHeaderLen); 
			CreateInfoHeader(m_dwInfoHeaderLen); 
			m_bitmapInfoHeader=pbmi;
							 
			return true;
		}
		catch(...)
		{
			 DeleteAll((void**)&m_bytptrDIBdata);
			 return false;
		}
}

BITMAPFILEHEADER CScreenMonitorBase::BmpFileHeaderFromBitmapHeader(BITMAPINFO *ptrToBmpInfo,DWORD &BmpInfoHdrLength) 
{
	BITMAPFILEHEADER bfh; 
	
	WORD clrdpth=ptrToBmpInfo->bmiHeader.biBitCount;

	DWORD InfHdrLen=(clrdpth==24?sizeof(BITMAPINFOHEADER):
	               (clrdpth==16||clrdpth==32)?
					sizeof(BITMAPINFOHEADER)+sizeof(DWORD)*3:
					sizeof(BITMAPINFOHEADER)+sizeof(RGBQUAD)*(1<<ptrToBmpInfo->bmiHeader.biBitCount));

	bfh.bfType ='MB'; 
	bfh.bfSize = sizeof(BITMAPFILEHEADER)+InfHdrLen+ ptrToBmpInfo->bmiHeader.biSizeImage;
	bfh.bfOffBits = sizeof(BITMAPFILEHEADER)+InfHdrLen; 
	bfh.bfReserved1 = bfh.bfReserved2 = 0; 
	BmpInfoHdrLength=InfHdrLen;
	return bfh;
}



			
