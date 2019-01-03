# Microsoft Developer Studio Generated NMAKE File, Based on WinMonitorServer.dsp
!IF "$(CFG)" == ""
CFG=WinMonitorServer - Win32 Debug
!MESSAGE No configuration specified. Defaulting to WinMonitorServer - Win32 Debug.
!ENDIF 

!IF "$(CFG)" != "WinMonitorServer - Win32 Release" && "$(CFG)" != "WinMonitorServer - Win32 Debug"
!MESSAGE Invalid configuration "$(CFG)" specified.
!MESSAGE You can specify a configuration when running NMAKE
!MESSAGE by defining the macro CFG on the command line. For example:
!MESSAGE 
!MESSAGE NMAKE /f "WinMonitorServer.mak" CFG="WinMonitorServer - Win32 Debug"
!MESSAGE 
!MESSAGE Possible choices for configuration are:
!MESSAGE 
!MESSAGE "WinMonitorServer - Win32 Release" (based on "Win32 (x86) Application")
!MESSAGE "WinMonitorServer - Win32 Debug" (based on "Win32 (x86) Application")
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

!IF  "$(CFG)" == "WinMonitorServer - Win32 Release"

OUTDIR=.\Release
INTDIR=.\Release
# Begin Custom Macros
OutDir=.\Release
# End Custom Macros

ALL : "$(OUTDIR)\WinMonitorServer.exe"


CLEAN :
	-@erase "$(INTDIR)\StdAfx.obj"
	-@erase "$(INTDIR)\vc60.idb"
	-@erase "$(INTDIR)\WinMonitorServer.obj"
	-@erase "$(INTDIR)\WinMonitorServer.pch"
	-@erase "$(OUTDIR)\WinMonitorServer.exe"

"$(OUTDIR)" :
    if not exist "$(OUTDIR)/$(NULL)" mkdir "$(OUTDIR)"

CPP_PROJ=/nologo /ML /W3 /GX /O2 /D "WIN32" /D "NDEBUG" /D "_WINDOWS" /D "_MBCS" /Fp"$(INTDIR)\WinMonitorServer.pch" /Yu"stdafx.h" /Fo"$(INTDIR)\\" /Fd"$(INTDIR)\\" /FD /c 
MTL_PROJ=/nologo /D "NDEBUG" /mktyplib203 /win32 
BSC32=bscmake.exe
BSC32_FLAGS=/nologo /o"$(OUTDIR)\WinMonitorServer.bsc" 
BSC32_SBRS= \
	
LINK32=link.exe
LINK32_FLAGS=kernel32.lib user32.lib gdi32.lib winspool.lib comdlg32.lib advapi32.lib shell32.lib ole32.lib oleaut32.lib uuid.lib odbc32.lib odbccp32.lib ws2_32.lib comctl32.lib Kernel32.lib /nologo /subsystem:windows /incremental:no /pdb:"$(OUTDIR)\WinMonitorServer.pdb" /machine:I386 /out:"$(OUTDIR)\WinMonitorServer.exe" 
LINK32_OBJS= \
	"$(INTDIR)\StdAfx.obj" \
	"$(INTDIR)\WinMonitorServer.obj"

"$(OUTDIR)\WinMonitorServer.exe" : "$(OUTDIR)" $(DEF_FILE) $(LINK32_OBJS)
    $(LINK32) @<<
  $(LINK32_FLAGS) $(LINK32_OBJS)
<<

!ELSEIF  "$(CFG)" == "WinMonitorServer - Win32 Debug"

OUTDIR=.\Debug
INTDIR=.\Debug
# Begin Custom Macros
OutDir=.\Debug
# End Custom Macros

ALL : "$(OUTDIR)\WinMonitorServer.exe" "$(OUTDIR)\WinMonitorServer.bsc"


CLEAN :
	-@erase "$(INTDIR)\StdAfx.obj"
	-@erase "$(INTDIR)\StdAfx.sbr"
	-@erase "$(INTDIR)\vc60.idb"
	-@erase "$(INTDIR)\vc60.pdb"
	-@erase "$(INTDIR)\WinMonitorServer.obj"
	-@erase "$(INTDIR)\WinMonitorServer.pch"
	-@erase "$(INTDIR)\WinMonitorServer.sbr"
	-@erase "$(OUTDIR)\WinMonitorServer.bsc"
	-@erase "$(OUTDIR)\WinMonitorServer.exe"
	-@erase "$(OUTDIR)\WinMonitorServer.ilk"
	-@erase "$(OUTDIR)\WinMonitorServer.pdb"

"$(OUTDIR)" :
    if not exist "$(OUTDIR)/$(NULL)" mkdir "$(OUTDIR)"

CPP_PROJ=/nologo /MLd /W3 /Gm /GX /ZI /Od /D "WIN32" /D "_DEBUG" /D "_WINDOWS" /D "_MBCS" /FR"$(INTDIR)\\" /Fp"$(INTDIR)\WinMonitorServer.pch" /Yu"stdafx.h" /Fo"$(INTDIR)\\" /Fd"$(INTDIR)\\" /FD /GZ /c 
MTL_PROJ=/nologo /D "_DEBUG" /mktyplib203 /win32 
BSC32=bscmake.exe
BSC32_FLAGS=/nologo /o"$(OUTDIR)\WinMonitorServer.bsc" 
BSC32_SBRS= \
	"$(INTDIR)\StdAfx.sbr" \
	"$(INTDIR)\WinMonitorServer.sbr"

"$(OUTDIR)\WinMonitorServer.bsc" : "$(OUTDIR)" $(BSC32_SBRS)
    $(BSC32) @<<
  $(BSC32_FLAGS) $(BSC32_SBRS)
<<

LINK32=link.exe
LINK32_FLAGS=kernel32.lib user32.lib gdi32.lib winspool.lib comdlg32.lib advapi32.lib shell32.lib ole32.lib oleaut32.lib uuid.lib odbc32.lib odbccp32.lib ws2_32.lib comctl32.lib Kernel32.lib /nologo /subsystem:windows /incremental:yes /pdb:"$(OUTDIR)\WinMonitorServer.pdb" /debug /machine:I386 /out:"$(OUTDIR)\WinMonitorServer.exe" /pdbtype:sept 
LINK32_OBJS= \
	"$(INTDIR)\StdAfx.obj" \
	"$(INTDIR)\WinMonitorServer.obj"

"$(OUTDIR)\WinMonitorServer.exe" : "$(OUTDIR)" $(DEF_FILE) $(LINK32_OBJS)
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
!IF EXISTS("WinMonitorServer.dep")
!INCLUDE "WinMonitorServer.dep"
!ELSE 
!MESSAGE Warning: cannot find "WinMonitorServer.dep"
!ENDIF 
!ENDIF 


!IF "$(CFG)" == "WinMonitorServer - Win32 Release" || "$(CFG)" == "WinMonitorServer - Win32 Debug"
SOURCE=.\StdAfx.cpp

!IF  "$(CFG)" == "WinMonitorServer - Win32 Release"

CPP_SWITCHES=/nologo /ML /W3 /GX /O2 /D "WIN32" /D "NDEBUG" /D "_WINDOWS" /D "_MBCS" /Fp"$(INTDIR)\WinMonitorServer.pch" /Yc"stdafx.h" /Fo"$(INTDIR)\\" /Fd"$(INTDIR)\\" /FD /c 

"$(INTDIR)\StdAfx.obj"	"$(INTDIR)\WinMonitorServer.pch" : $(SOURCE) "$(INTDIR)"
	$(CPP) @<<
  $(CPP_SWITCHES) $(SOURCE)
<<


!ELSEIF  "$(CFG)" == "WinMonitorServer - Win32 Debug"

CPP_SWITCHES=/nologo /MLd /W3 /Gm /GX /ZI /Od /D "WIN32" /D "_DEBUG" /D "_WINDOWS" /D "_MBCS" /FR"$(INTDIR)\\" /Fp"$(INTDIR)\WinMonitorServer.pch" /Yc"stdafx.h" /Fo"$(INTDIR)\\" /Fd"$(INTDIR)\\" /FD /GZ /c 

"$(INTDIR)\StdAfx.obj"	"$(INTDIR)\StdAfx.sbr"	"$(INTDIR)\WinMonitorServer.pch" : $(SOURCE) "$(INTDIR)"
	$(CPP) @<<
  $(CPP_SWITCHES) $(SOURCE)
<<


!ENDIF 

SOURCE=.\WinMonitorServer.cpp

!IF  "$(CFG)" == "WinMonitorServer - Win32 Release"


"$(INTDIR)\WinMonitorServer.obj" : $(SOURCE) "$(INTDIR)" "$(INTDIR)\WinMonitorServer.pch"


!ELSEIF  "$(CFG)" == "WinMonitorServer - Win32 Debug"


"$(INTDIR)\WinMonitorServer.obj"	"$(INTDIR)\WinMonitorServer.sbr" : $(SOURCE) "$(INTDIR)" "$(INTDIR)\WinMonitorServer.pch"


!ENDIF 


!ENDIF 

