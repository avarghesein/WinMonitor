

Imports WinMonitorBLIB_1_0


Public Class frmWinMonitorMdi : Inherits System.Windows.Forms.Form

    Private m_frmServerListing As FrmWinMonitorServerList = Nothing
    Private m_frmConnectedServerList As FrmWinMonitorConnectedServerList = Nothing
    Private m_frmReverseConnectionWzd As FrmListenForServers = Nothing
    Private WithEvents m_frmConnectWzrd As FrmStartUpConnection = Nothing

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        m_frmConnectedServerList = New FrmWinMonitorConnectedServerList(False)
        m_frmConnectedServerList.MdiParent = Me
        m_frmConnectedServerList.StartPosition = FormStartPosition.CenterParent
        m_frmConnectedServerList.m_IsClose = False
        m_frmConnectedServerList.Visible = False

        m_frmReverseConnectionWzd = New FrmListenForServers()
        AddHandler m_frmReverseConnectionWzd.eventGotConnection, AddressOf RegisterReverseConnection
        m_frmReverseConnectionWzd.MdiParent = Me
        m_frmReverseConnectionWzd.StartPosition = FormStartPosition.CenterParent
        m_frmReverseConnectionWzd.m_IsClose = False
        m_frmReverseConnectionWzd.Visible = False
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
    Friend WithEvents mnuWinMonitor As System.Windows.Forms.MainMenu
    Friend WithEvents mnuWinMonitorFile As System.Windows.Forms.MenuItem
    Friend WithEvents mnuWinMonitorFileOpenServers As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem1 As System.Windows.Forms.MenuItem
    Friend WithEvents frmWinMonitorFileNewServer As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem3 As System.Windows.Forms.MenuItem
    Friend WithEvents frmWinMonitorFileOpenSettingsFile As System.Windows.Forms.MenuItem
    Friend WithEvents mnuWinMonitorTools As System.Windows.Forms.MenuItem
    Friend WithEvents mnuWinMonitorToolsConnectToServer As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem2 As System.Windows.Forms.MenuItem
    Friend WithEvents mnuWinMonitorToolsOpenServer As System.Windows.Forms.MenuItem
    Friend WithEvents mnuWinMonitorNetwork As System.Windows.Forms.MenuItem
    Friend WithEvents mnuWinMonitorNetworkListenForServers As System.Windows.Forms.MenuItem
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmWinMonitorMdi))
        Me.mnuWinMonitor = New System.Windows.Forms.MainMenu()
        Me.mnuWinMonitorFile = New System.Windows.Forms.MenuItem()
        Me.frmWinMonitorFileNewServer = New System.Windows.Forms.MenuItem()
        Me.MenuItem3 = New System.Windows.Forms.MenuItem()
        Me.mnuWinMonitorFileOpenServers = New System.Windows.Forms.MenuItem()
        Me.MenuItem1 = New System.Windows.Forms.MenuItem()
        Me.frmWinMonitorFileOpenSettingsFile = New System.Windows.Forms.MenuItem()
        Me.mnuWinMonitorTools = New System.Windows.Forms.MenuItem()
        Me.mnuWinMonitorToolsConnectToServer = New System.Windows.Forms.MenuItem()
        Me.MenuItem2 = New System.Windows.Forms.MenuItem()
        Me.mnuWinMonitorToolsOpenServer = New System.Windows.Forms.MenuItem()
        Me.mnuWinMonitorNetwork = New System.Windows.Forms.MenuItem()
        Me.mnuWinMonitorNetworkListenForServers = New System.Windows.Forms.MenuItem()
        '
        'mnuWinMonitor
        '
        Me.mnuWinMonitor.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuWinMonitorFile, Me.mnuWinMonitorTools, Me.mnuWinMonitorNetwork})
        '
        'mnuWinMonitorFile
        '
        Me.mnuWinMonitorFile.Index = 0
        Me.mnuWinMonitorFile.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.frmWinMonitorFileNewServer, Me.MenuItem3, Me.mnuWinMonitorFileOpenServers, Me.MenuItem1, Me.frmWinMonitorFileOpenSettingsFile})
        Me.mnuWinMonitorFile.Text = "&File"
        '
        'frmWinMonitorFileNewServer
        '
        Me.frmWinMonitorFileNewServer.Index = 0
        Me.frmWinMonitorFileNewServer.Shortcut = System.Windows.Forms.Shortcut.CtrlN
        Me.frmWinMonitorFileNewServer.Text = "New WinMonitor Server"
        '
        'MenuItem3
        '
        Me.MenuItem3.Index = 1
        Me.MenuItem3.Text = "-"
        '
        'mnuWinMonitorFileOpenServers
        '
        Me.mnuWinMonitorFileOpenServers.Index = 2
        Me.mnuWinMonitorFileOpenServers.Shortcut = System.Windows.Forms.Shortcut.CtrlO
        Me.mnuWinMonitorFileOpenServers.Text = "Open Created Server List"
        '
        'MenuItem1
        '
        Me.MenuItem1.Index = 3
        Me.MenuItem1.Text = "-"
        '
        'frmWinMonitorFileOpenSettingsFile
        '
        Me.frmWinMonitorFileOpenSettingsFile.Index = 4
        Me.frmWinMonitorFileOpenSettingsFile.Shortcut = System.Windows.Forms.Shortcut.CtrlS
        Me.frmWinMonitorFileOpenSettingsFile.Text = "Open Contacted Server List"
        '
        'mnuWinMonitorTools
        '
        Me.mnuWinMonitorTools.Index = 1
        Me.mnuWinMonitorTools.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuWinMonitorToolsConnectToServer, Me.MenuItem2, Me.mnuWinMonitorToolsOpenServer})
        Me.mnuWinMonitorTools.Text = "&Tools"
        '
        'mnuWinMonitorToolsConnectToServer
        '
        Me.mnuWinMonitorToolsConnectToServer.Index = 0
        Me.mnuWinMonitorToolsConnectToServer.Shortcut = System.Windows.Forms.Shortcut.CtrlS
        Me.mnuWinMonitorToolsConnectToServer.Text = "Connect to Server"
        '
        'MenuItem2
        '
        Me.MenuItem2.Index = 1
        Me.MenuItem2.Text = "-"
        '
        'mnuWinMonitorToolsOpenServer
        '
        Me.mnuWinMonitorToolsOpenServer.Index = 2
        Me.mnuWinMonitorToolsOpenServer.Shortcut = System.Windows.Forms.Shortcut.CtrlE
        Me.mnuWinMonitorToolsOpenServer.Text = "Open Server"
        '
        'mnuWinMonitorNetwork
        '
        Me.mnuWinMonitorNetwork.Index = 2
        Me.mnuWinMonitorNetwork.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuWinMonitorNetworkListenForServers})
        Me.mnuWinMonitorNetwork.Text = "&Network"
        '
        'mnuWinMonitorNetworkListenForServers
        '
        Me.mnuWinMonitorNetworkListenForServers.Index = 0
        Me.mnuWinMonitorNetworkListenForServers.Text = "Listen For Servers"
        '
        'frmWinMonitorMdi
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(328, 253)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.IsMdiContainer = True
        Me.Menu = Me.mnuWinMonitor
        Me.Name = "frmWinMonitorMdi"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "WinMonitor.1.0"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized

    End Sub

#End Region

    Private Sub mnuWinMonitorFileOpenServers_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuWinMonitorFileOpenServers.Click
        Try
            If (Not IsNothing(m_frmServerListing)) Then m_frmServerListing.Close()
        Catch Ex As Exception : End Try
        Try
            m_frmServerListing = Nothing
            m_frmServerListing = New FrmWinMonitorServerList(IDS_XML_FILE_PATH & IDS_DEFAULT_XML_FILE)
            m_frmServerListing.MdiParent = Me
            m_frmServerListing.StartPosition = FormStartPosition.CenterParent
            m_frmServerListing.Show()
        Catch Ex As Exception : End Try
    End Sub


    Private Sub mnuWinMonitorToolsConnectToServer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuWinMonitorToolsConnectToServer.Click
        If Not (m_frmConnectWzrd Is Nothing) Then
            Try
                m_frmConnectWzrd.Close()
                m_frmConnectWzrd = Nothing
            Catch Ex As Exception : End Try
        End If
        m_frmConnectWzrd = New FrmStartUpConnection()
        AddHandler m_frmConnectWzrd.eventGotConnection, AddressOf RegisterConnection
        m_frmConnectWzrd.StartPosition = FormStartPosition.CenterScreen : m_frmConnectWzrd.MdiParent = Me
        m_frmConnectWzrd.Show() : m_frmConnectWzrd.txtConnectionServerIP.Focus()
    End Sub

    Public Sub RegisterConnection(ByRef SeverConnection As CTcpNetworkMonitorConnection)
        Static i As Integer = 0
        i += 1
        If (i > 1) Then
            i = 0
            Exit Sub
        End If
        If (SeverConnection Is Nothing) Then
            Exit Sub
        End If
        Cursor.Current = Cursors.Default
        m_frmConnectedServerList.Visible = True
        m_frmConnectedServerList.AddServer(SeverConnection, m_frmConnectWzrd.txtConnectionServerIP.Text, m_frmConnectWzrd.txtConnectionServerPort.Text, m_frmConnectWzrd.txtConnectionServerPassword.Text)
        m_frmConnectedServerList.Visible = False
    End Sub

    Public Sub RegisterReverseConnection(ByRef SeverConnection As CTcpNetworkMonitorConnection, ByVal strIP As String, ByVal intListenPort As Integer, ByVal strPwd As String)
        Static i As Integer = 0
        i += 1
        If (i > 1) Then
            i = 0
            Exit Sub
        End If
        If (SeverConnection Is Nothing) Then
            Exit Sub
        End If
        Cursor.Current = Cursors.Default
        m_frmConnectedServerList.Visible = True
        m_frmConnectedServerList.AddServer(SeverConnection, strIP, Str(intListenPort).Trim, strPwd)
        m_frmConnectedServerList.Visible = False
    End Sub

    Protected Overrides Sub OnClosing(ByVal e As System.ComponentModel.CancelEventArgs)
        Try
            If Not (m_frmConnectWzrd Is Nothing) Then
                m_frmConnectWzrd.Close()
                m_frmConnectWzrd = Nothing
            End If
            m_frmConnectedServerList.m_IsClose = True
            m_frmConnectedServerList.Close()
            m_frmReverseConnectionWzd.StopThreadIfRunning()
            m_frmReverseConnectionWzd.m_IsClose = True
            m_frmReverseConnectionWzd.Close()
            Application.Exit()
        Catch Ex As Exception : End Try
    End Sub

    Private Sub mnuWinMonitorToolsOpenServer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuWinMonitorToolsOpenServer.Click
        m_frmConnectedServerList.Visible = True
    End Sub

    Private Sub frmWinMonitorFileNewServer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles frmWinMonitorFileNewServer.Click
        Dim frmWinMonitorServerBuilder As frmWinMonitorServerConfig
        Try
            frmWinMonitorServerBuilder = New frmWinMonitorServerConfig()
            frmWinMonitorServerBuilder.DefaultXML = IDS_XML_FILE_PATH & IDS_DEFAULT_XML_FILE
            frmWinMonitorServerBuilder.ServerTemplate = IDS_SERVER_TEMPLATE
            frmWinMonitorServerBuilder.ServerDLLTemplate = IDS_SERVER_DLL_TEMPLATE
            frmWinMonitorServerBuilder.StartPosition = FormStartPosition.CenterScreen
            frmWinMonitorServerBuilder.MdiParent = Me
            frmWinMonitorServerBuilder.Show()
        Catch Ex As Exception
            MessageBox.Show(Ex.Message(), IDS_PROGRAM_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
        End Try
    End Sub


    Private Sub mnuWinMonitorNetworkListenForServers_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuWinMonitorNetworkListenForServers.Click
        m_frmReverseConnectionWzd.StartPosition = FormStartPosition.CenterScreen
        m_frmReverseConnectionWzd.Visible = True
    End Sub

End Class
