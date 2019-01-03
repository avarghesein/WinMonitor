# Microsoft Developer Studio Generated NMAKE File, Based on WinMonitorGenericLib.dsp
!IF "$(CFG)" == ""
CFG=WinMonitorGenericLib - Win32 Debug
!MESSAGE No configuration specified. Defaulting to WinMonitorGenericLib - Win32 Debug.
!ENDIF 

!IF "$(CFG)" != "WinMonitorGenericLib - Win32 Release" && "$(CFG)" != "WinMonitorGenericLib - Win32 Debug"
!MESSAGE Invalid configuration "$(CFG)" specified.
!MESSAGE You can specify a configuration when running NMAKE
!MESSAGE by defining the macro CFG on the command line. For example:
!MESSAGE 
!MESSAGE NMAKE /f "WinMonitorGenericLib.mak" CFG="WinMonitorGenericLib - Win32 Debug"
!MESSAGE 
!MESSAGE Possible choices for configuration are:
!MESSAGE 
!MESSAGE "WinMonitorGenericLib - Win32 Release" (based on "Win32 (x86) Dynamic-Link Library")
!MESSAGE "WinMonitorGenericLib - Win32 Debug" (based on "Win32 (x86) Dynamic-Link Library")
!MESSAGE 
!ERROR An invalid configuration is specified.
!ENDIF 

!IF "$(OS)" == "Windows_NT"
NULL=
!ELSE 
NULL=nul
!ENDIF 

CPP=cl.exe
MTL=midl.exe
RSC=rc.exe

!IF  "$(CFG)" == "WinMonitorGenericLib - Win32 Release"

OUTDIR=.\Release
INTDIR=.\Release
# Begin Custom Macros
OutDir=.\Release
# End Custom Macros

ALL : "$(OUTDIR)\WinMonitorGenericLib.dll"


CLEAN :
	-@erase "$(INTDIR)\StdAfx.obj"
	-@erase "$(INTDIR)\vc60.idb"
	-@erase "$(INTDIR)\WinMonitorGenericLib.obj"
	-@erase "$(INTDIR)\WinMonitorGenericLib.pch"
	-@erase "$(OUTDIR)\WinMonitorGenericLib.dll"
	-@erase "$(OUTDIR)\WinMonitorGenericLib.exp"
	-@erase "$(OUTDIR)\WinMonitorGenericLib.lib"

"$(OUTDIR)" :
    if not exist "$(OUTDIR)/$(NULL)" mkdir "$(OUTDIR)"

CPP_PROJ=/nologo /MT /W3 /GX /O2 /D "WIN32" /D "NDEBUG" /D "_WINDOWS" /D "_MBCS" /D "_USRDLL" /D "WINMONITORGENERICLIB_EXPORTS" /Fp"$(INTDIR)\WinMonitorGenericLib.pch" /Yu"stdafx.h" /Fo"$(INTDIR)\\" /Fd"$(INTDIR)\\" /FD /c 
MTL_PROJ=/nologo /D "NDEBUG" /mktyplib203 /win32 
BSC32=bscmake.exe
BSC32_FLAGS=/nologo /o"$(OUTDIR)\WinMonitorGenericLib.bsc" 
BSC32_SBRS= \
	
LINK32=link.exe
LINK32_FLAGS=kernel32.lib user32.lib gdi32.lib winspool.lib comdlg32.lib advapi32.lib shell32.lib ole32.lib oleaut32.lib uuid.lib odbc32.lib odbccp32.lib Ws2_32.lib /nologo /dll /incremental:no /pdb:"$(OUTDIR)\WinMonitorGenericLib.pdb" /machine:I386 /out:"$(OUTDIR)\WinMonitorGenericLib.dll" /implib:"$(OUTDIR)\WinMonitorGenericLib.lib" 
LINK32_OBJS= \
	"$(INTDIR)\StdAfx.obj" \
	"$(INTDIR)\WinMonitorGenericLib.obj"

"$(OUTDIR)\WinMonitorGenericLib.dll" : "$(OUTDIR)" $(DEF_FILE) $(LINK32_OBJS)
    $(LINK32) @<<
  $(LINK32_FLAGS) $(LINK32_OBJS)
<<

!ELSEIF  "$(CFG)" == "WinMonitorGenericLib - Win32 Debug"

OUTDIR=.\Debug
INTDIR=.\Debug
# Begin Custom Macros
OutDir=.\Debug
# End Custom Macros

ALL : "$(OUTDIR)\WinMonitorGenericLib.dll"


CLEAN :
	-@erase "$(INTDIR)\StdAfx.obj"
	-@erase "$(INTDIR)\vc60.idb"
	-@erase "$(INTDIR)\vc60.pdb"
	-@erase "$(INTDIR)\WinMonitorGenericLib.obj"
	-@erase "$(INTDIR)\WinMonitorGenericLib.pch"
	-@erase "$(OUTDIR)\WinMonitorGenericLib.dll"
	-@erase "$(OUTDIR)\WinMonitorGenericLib.exp"
	-@erase "$(OUTDIR)\WinMonitorGenericLib.ilk"
	-@erase "$(OUTDIR)\WinMonitorGenericLib.lib"
	-@erase "$(OUTDIR)\WinMonitorGenericLib.pdb"

"$(OUTDIR)" :
    if not exist "$(OUTDIR)/$(NULL)" mkdir "$(OUTDIR)"

CPP_PROJ=/nologo /MTd /W3 /Gm /GX /ZI /Od /D "WIN32" /D "_DEBUG" /D "_WINDOWS" /D "_MBCS" /D "_USRDLL" /D "WINMONITORGENERICLIB_EXPORTS" /Fp"$(INTDIR)\WinMonitorGenericLib.pch" /Yu"stdafx.h" /Fo"$(INTDIR)\\" /Fd"$(INTDIR)\\" /FD /GZ /c 
MTL_PROJ=/nologo /D "_DEBUG" /mktyplib203 /win32 
BSC32=bscmake.exe
BSC32_FLAGS=/nologo /o"$(OUTDIR)\WinMonitorGenericLib.bsc" 
BSC32_SBRS= \
	
LINK32=link.exe
LINK32_FLAGS=kernel32.lib user32.lib gdi32.lib winspool.lib comdlg32.lib advapi32.lib shell32.lib ole32.lib oleaut32.lib uuid.lib odbc32.lib odbccp32.lib Ws2_32.lib /nologo /dll /incremental:yes /pdb:"$(OUTDIR)\WinMonitorGenericLib.pdb" /debug /machine:I386 /out:"$(OUTDIR)\WinMonitorGenericLib.dll" /implib:"$(OUTDIR)\WinMonitorGenericLib.lib" /pdbtype:sept 
LINK32_OBJS= \
	"$(INTDIR)\StdAfx.obj" \
	"$(INTDIR)\WinMonitorGenericLib.obj"

"$(OUTDIR)\WinMonitorGenericLib.dll" : "$(OUTDIR)" $(DEF_FILE) $(LINK32_OBJS)
    $(LINK32) @<<
  $(LINK32_FLAGS) $(LINK32_OBJS)
<<

!ENDIF 

.c{$(INTDIR)}.obj::
   $(CPP) @<<
   $(CPP_PROJ) $< 
<<

.cpp{$(INTDIR)}.obj::
   $(CPP) @<<
   $(CPP_PROJ) $< 
<<

.cxx{$(INTDIR)}.obj::
   $(CPP) @<<
   $(CPP_PROJ) $< 
<<

.c{$(INTDIR)}.sbr::
   $(CPP) @<<
   $(CPP_PROJ) $< 
<<

.cpp{$(INTDIR)}.sbr::
   $(CPP) @<<
   $(CPP_PROJ) $< 
<<

.cxx{$(INTDIR)}.sbr::
   $(CPP) @<<
   $(CPP_PROJ) $< 
<<


!IF "$(NO_EXTERNAL_DEPS)" != "1"
!IF EXISTS("WinMonitorGenericLib.dep")
!INCLUDE "WinMonitorGenericLib.dep"
!ELSE 
!MESSAGE Warning: cannot find "WinMonitorGenericLib.dep"
!ENDIF 
!ENDIF 


!IF "$(CFG)" == "WinMonitorGenericLib - Win32 Release" || "$(CFG)" == "WinMonitorGenericLib - Win32 Debug"
SOURCE=.\StdAfx.cpp

!IF  "$(CFG)" == "WinMonitorGenericLib - Win32 Release"

CPP_SWITCHES=/nologo /MT /W3 /GX /O2 /D "WIN32" /D "NDEBUG" /D "_WINDOWS" /D "_MBCS" /D "_USRDLL" /D "WINMONITORGENERICLIB_EXPORTS" /Fp"$(INTDIR)\WinMonitorGenericLib.pch" /Yc"stdafx.h" /Fo"$(INTDIR)\\" /Fd"$(INTDIR)\\" /FD /c 

"$(INTDIR)\StdAfx.obj"	"$(INTDIR)\WinMonitorGenericLib.pch" : $(SOURCE) "$(INTDIR)"
	$(CPP) @<<
  $(CPP_SWITCHES) $(SOURCE)
<<


!ELSEIF  "$(CFG)" == "WinMonitorGenericLib - Win32 Debug"

CPP_SWITCHES=/nologo /MTd /W3 /Gm /GX /ZI /Od /D "WIN32" /D "_DEBUG" /D "_WINDOWS" /D "_MBCS" /D "_USRDLL" /D "WINMONITORGENERICLIB_EXPORTS" /Fp"$(INTDIR)\WinMonitorGenericLib.pch" /Yc"stdafx.h" /Fo"$(INTDIR)\\" /Fd"$(INTDIR)\\" /FD /GZ /c 

"$(INTDIR)\StdAfx.obj"	"$(INTDIR)\WinMonitorGenericLib.pch" : $(SOURCE) "$(INTDIR)"
	$(CPP) @<<
  $(CPP_SWITCHES) $(SOURCE)
<<


!ENDIF 

SOURCE=.\WinMonitorGenericLib.cpp

"$(INTDIR)\WinMonitorGenericLib.obj" : $(SOURCE) "$(INTDIR)" "$(INTDIR)\WinMonitorGenericLib.pch"



!ENDIF 

