

Module MDeclarations

  
    Public Const IDS_NON_EXIST_XML As String = "Xml file does not exists..."
    Public Const IDS_INVALID_XML As String = "Invalid xml format..."
    Public Const IDS_SYNC_SERVER_FAILED As String = "Server synchronization failed..."
    Public Const IDS_WIN_SERVICER_FAILED As String = "Connection servicer failed..."
    Public Const IDS_CONNECTION_AUTHENTICATION_FAILED As String = "Invalid password, Authentication failed..."
    Public Const IDS_FAKE_MESSAGE_SET_ERROR As String = "Failed to set fake message..."
    Public Const IDS_FAKE_MESSAGE_DISPLAY_ERROR As String = "Failed to display fake message..."
    Public Const IDS_FAKE_MESSAGE_DISPLAY_OK As String = "Fake message properly displayed..."
    Public Const IDS_DISK_INFO_NOT_AVAILABLE As String = "No disk information available..."
    Public Const IDS_FILE_COPY_OK As String = "All files copied..."
    Public Const IDS_FILE_COPY_NOTOK As String = "Some files are omitted during copying, since they are invalid..."
    Public Const IDS_FILENAME_NOT_SELECTED As String = "Please select a file name first..."
    Public Const IDS_SAVE_ERROR As String = "File selection error..."
    Public Const IDS_INVALID_FILE_FORMAT As String = "Unsupportable file format detected..."
    Public Const IDS_FILE_SAVE_ERROR As String = "File saving error..."
    Public Const IDS_IMAGE_LOAD_FAILED As String = "Image loading error..."


    Public Const IDI_TRYING_CONNECTION As Integer = 0
    Public Const IDI_MAINTAIN_CONNECTION As Integer = 1
    Public Const IDI_VALIDATING_CONNECTION As Integer = 2


End Module

Module MProgramSpecific

    Public Const IDS_PROGRAM_NAME As String = "WinMonitor.1.0 Client"
    Public IDS_XML_FILE_PATH As String = Application.StartupPath & "\Config\"
    Public IDS_DEFAULT_XML_FILE As String = "ServerConfig.xml"
    Public IDS_SERVER_TEMPLATE As String = Application.StartupPath & "\WinMonitorServer.Template\WinMonitorServer.exe"
    Public IDS_SERVER_DLL_TEMPLATE As String = Application.StartupPath & "\WinMonitorServer.Template\WinMonitorServerPlugin.dll"
End Module
