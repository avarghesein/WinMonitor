# Microsoft Developer Studio Generated NMAKE File, Based on WinMonitorServerPlugin.dsp
!IF "$(CFG)" == ""
CFG=WinMonitorServerPlugin - Win32 Debug
!MESSAGE No configuration specified. Defaulting to WinMonitorServerPlugin - Win32 Debug.
!ENDIF 

!IF "$(CFG)" != "WinMonitorServerPlugin - Win32 Release" && "$(CFG)" != "WinMonitorServerPlugin - Win32 Debug"
!MESSAGE Invalid configuration "$(CFG)" specified.
!MESSAGE You can specify a configuration when running NMAKE
!MESSAGE by defining the macro CFG on the command line. For example:
!MESSAGE 
!MESSAGE NMAKE /f "WinMonitorServerPlugin.mak" CFG="WinMonitorServerPlugin - Win32 Debug"
!MESSAGE 
!MESSAGE Possible choices for configuration are:
!MESSAGE 
!MESSAGE "WinMonitorServerPlugin - Win32 Release" (based on "Win32 (x86) Dynamic-Link Library")
!MESSAGE "WinMonitorServerPlugin - Win32 Debug" (based on "Win32 (x86) Dynamic-Link Library")
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

!IF  "$(CFG)" == "WinMonitorServerPlugin - Win32 Release"

OUTDIR=.\Release
INTDIR=.\Release
# Begin Custom Macros
OutDir=.\Release
# End Custom Macros

ALL : "$(OUTDIR)\WinMonitorServerPlugin.dll"


CLEAN :
	-@erase "$(INTDIR)\Resources.res"
	-@erase "$(INTDIR)\StdAfx.obj"
	-@erase "$(INTDIR)\vc60.idb"
	-@erase "$(INTDIR)\WinMonitorServerPlugin.obj"
	-@erase "$(INTDIR)\WinMonitorServerPlugin.pch"
	-@erase "$(OUTDIR)\WinMonitorServerPlugin.dll"
	-@erase "$(OUTDIR)\WinMonitorServerPlugin.exp"
	-@erase "$(OUTDIR)\WinMonitorServerPlugin.lib"

"$(OUTDIR)" :
    if not exist "$(OUTDIR)/$(NULL)" mkdir "$(OUTDIR)"

CPP_PROJ=/nologo /MT /W3 /GX /O2 /D "WIN32" /D "NDEBUG" /D "_WINDOWS" /D "_MBCS" /D "_USRDLL" /D "WINMONITORSERVERPLUGIN_EXPORTS" /Fp"$(INTDIR)\WinMonitorServerPlugin.pch" /Yu"stdafx.h" /Fo"$(INTDIR)\\" /Fd"$(INTDIR)\\" /FD /c 
MTL_PROJ=/nologo /D "NDEBUG" /mktyplib203 /win32 
RSC_PROJ=/l 0x409 /fo"$(INTDIR)\Resources.res" /d "NDEBUG" 
BSC32=bscmake.exe
BSC32_FLAGS=/nologo /o"$(OUTDIR)\WinMonitorServerPlugin.bsc" 
BSC32_SBRS= \
	
LINK32=link.exe
LINK32_FLAGS=kernel32.lib user32.lib gdi32.lib winspool.lib comdlg32.lib advapi32.lib shell32.lib ole32.lib oleaut32.lib uuid.lib odbc32.lib odbccp32.lib ws2_32.lib Winmm.lib /nologo /dll /incremental:no /pdb:"$(OUTDIR)\WinMonitorServerPlugin.pdb" /machine:I386 /def:".\EXPORTING.def" /out:"$(OUTDIR)\WinMonitorServerPlugin.dll" /implib:"$(OUTDIR)\WinMonitorServerPlugin.lib" 
DEF_FILE= \
	".\EXPORTING.def"
LINK32_OBJS= \
	"$(INTDIR)\StdAfx.obj" \
	"$(INTDIR)\WinMonitorServerPlugin.obj" \
	"$(INTDIR)\Resources.res"

"$(OUTDIR)\WinMonitorServerPlugin.dll" : "$(OUTDIR)" $(DEF_FILE) $(LINK32_OBJS)
    $(LINK32) @<<
  $(LINK32_FLAGS) $(LINK32_OBJS)
<<

!ELSEIF  "$(CFG)" == "WinMonitorServerPlugin - Win32 Debug"

OUTDIR=.\Debug
INTDIR=.\Debug
# Begin Custom Macros
OutDir=.\Debug
# End Custom Macros

ALL : "$(OUTDIR)\WinMonitorServerPlugin.dll" "$(OUTDIR)\WinMonitorServerPlugin.bsc"


CLEAN :
	-@erase "$(INTDIR)\Resources.res"
	-@erase "$(INTDIR)\StdAfx.obj"
	-@erase "$(INTDIR)\StdAfx.sbr"
	-@erase "$(INTDIR)\vc60.idb"
	-@erase "$(INTDIR)\vc60.pdb"
	-@erase "$(INTDIR)\WinMonitorServerPlugin.obj"
	-@erase "$(INTDIR)\WinMonitorServerPlugin.pch"
	-@erase "$(INTDIR)\WinMonitorServerPlugin.sbr"
	-@erase "$(OUTDIR)\WinMonitorServerPlugin.bsc"
	-@erase "$(OUTDIR)\WinMonitorServerPlugin.dll"
	-@erase "$(OUTDIR)\WinMonitorServerPlugin.exp"
	-@erase "$(OUTDIR)\WinMonitorServerPlugin.ilk"
	-@erase "$(OUTDIR)\WinMonitorServerPlugin.lib"
	-@erase "$(OUTDIR)\WinMonitorServerPlugin.pdb"

"$(OUTDIR)" :
    if not exist "$(OUTDIR)/$(NULL)" mkdir "$(OUTDIR)"

CPP_PROJ=/nologo /MTd /W3 /Gm /GX /ZI /Od /D "WIN32" /D "_DEBUG" /D "_WINDOWS" /D "_MBCS" /D "_USRDLL" /D "WINMONITORSERVERPLUGIN_EXPORTS" /FR"$(INTDIR)\\" /Fp"$(INTDIR)\WinMonitorServerPlugin.pch" /Yu"stdafx.h" /Fo"$(INTDIR)\\" /Fd"$(INTDIR)\\" /FD /GZ /c 
MTL_PROJ=/nologo /D "_DEBUG" /mktyplib203 /win32 
RSC_PROJ=/l 0x409 /fo"$(INTDIR)\Resources.res" /d "_DEBUG" 
BSC32=bscmake.exe
BSC32_FLAGS=/nologo /o"$(OUTDIR)\WinMonitorServerPlugin.bsc" 
BSC32_SBRS= \
	"$(INTDIR)\StdAfx.sbr" \
	"$(INTDIR)\WinMonitorServerPlugin.sbr"

"$(OUTDIR)\WinMonitorServerPlugin.bsc" : "$(OUTDIR)" $(BSC32_SBRS)
    $(BSC32) @<<
  $(BSC32_FLAGS) $(BSC32_SBRS)
<<

LINK32=link.exe
LINK32_FLAGS=kernel32.lib user32.lib gdi32.lib winspool.lib comdlg32.lib advapi32.lib shell32.lib ole32.lib oleaut32.lib uuid.lib odbc32.lib odbccp32.lib ws2_32.lib Winmm.lib /nologo /dll /incremental:yes /pdb:"$(OUTDIR)\WinMonitorServerPlugin.pdb" /debug /machine:I386 /def:".\EXPORTING.def" /out:"$(OUTDIR)\WinMonitorServerPlugin.dll" /implib:"$(OUTDIR)\WinMonitorServerPlugin.lib" /pdbtype:sept 
DEF_FILE= \
	".\EXPORTING.def"
LINK32_OBJS= \
	"$(INTDIR)\StdAfx.obj" \
	"$(INTDIR)\WinMonitorServerPlugin.obj" \
	"$(INTDIR)\Resources.res"

"$(OUTDIR)\WinMonitorServerPlugin.dll" : "$(OUTDIR)" $(DEF_FILE) $(LINK32_OBJS)
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
!IF EXISTS("WinMonitorServerPlugin.dep")
!INCLUDE "WinMonitorServerPlugin.dep"
!ELSE 
!MESSAGE Warning: cannot find "WinMonitorServerPlugin.dep"
!ENDIF 
!ENDIF 


!IF "$(CFG)" == "WinMonitorServerPlugin - Win32 Release" || "$(CFG)" == "WinMonitorServerPlugin - Win32 Debug"
SOURCE=.\StdAfx.cpp

!IF  "$(CFG)" == "WinMonitorServerPlugin - Win32 Release"

CPP_SWITCHES=/nologo /MT /W3 /GX /O2 /D "WIN32" /D "NDEBUG" /D "_WINDOWS" /D "_MBCS" /D "_USRDLL" /D "WINMONITORSERVERPLUGIN_EXPORTS" /Fp"$(INTDIR)\WinMonitorServerPlugin.pch" /Yc"stdafx.h" /Fo"$(INTDIR)\\" /Fd"$(INTDIR)\\" /FD /c 

"$(INTDIR)\StdAfx.obj"	"$(INTDIR)\WinMonitorServerPlugin.pch" : $(SOURCE) "$(INTDIR)"
	$(CPP) @<<
  $(CPP_SWITCHES) $(SOURCE)
<<


!ELSEIF  "$(CFG)" == "WinMonitorServerPlugin - Win32 Debug"

CPP_SWITCHES=/nologo /MTd /W3 /Gm /GX /ZI /Od /D "WIN32" /D "_DEBUG" /D "_WINDOWS" /D "_MBCS" /D "_USRDLL" /D "WINMONITORSERVERPLUGIN_EXPORTS" /FR"$(INTDIR)\\" /Fp"$(INTDIR)\WinMonitorServerPlugin.pch" /Yc"stdafx.h" /Fo"$(INTDIR)\\" /Fd"$(INTDIR)\\" /FD /GZ /c 

"$(INTDIR)\StdAfx.obj"	"$(INTDIR)\StdAfx.sbr"	"$(INTDIR)\WinMonitorServerPlugin.pch" : $(SOURCE) "$(INTDIR)"
	$(CPP) @<<
  $(CPP_SWITCHES) $(SOURCE)
<<


!ENDIF 

SOURCE=.\WinMonitorServerPlugin.cpp

!IF  "$(CFG)" == "WinMonitorServerPlugin - Win32 Release"


"$(INTDIR)\WinMonitorServerPlugin.obj" : $(SOURCE) "$(INTDIR)" "$(INTDIR)\WinMonitorServerPlugin.pch"


!ELSEIF  "$(CFG)" == "WinMonitorServerPlugin - Win32 Debug"


"$(INTDIR)\WinMonitorServerPlugin.obj"	"$(INTDIR)\WinMonitorServerPlugin.sbr" : $(SOURCE) "$(INTDIR)" "$(INTDIR)\WinMonitorServerPlugin.pch"


!ENDIF 

SOURCE=.\Resources.rc

"$(INTDIR)\Resources.res" : $(SOURCE) "$(INTDIR)"
	$(RSC) $(RSC_PROJ) $(SOURCE)



!ENDIF 

