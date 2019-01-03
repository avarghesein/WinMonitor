
Imports WinMonitorBLIB_1_0

Public Class FrmListenForServers : Inherits System.Windows.Forms.Form

    Friend m_IsClose As Boolean
    Private m_NwListener As CTcpNetworkMonitorListener = New CTcpNetworkMonitorListener()
    Private m_ListenThread As System.Threading.Thread = Nothing
    Private m_blnContinueListen As Boolean = False

    Public Event eventGotConnection(ByRef TcpConnection As CTcpNetworkMonitorConnection, ByVal strIP As String, ByVal intListenPort As Integer, ByVal strPwd As String)

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
    Friend WithEvents pnlServersList As System.Windows.Forms.Panel
    Friend WithEvents lstwServersList As System.Windows.Forms.ListView
    Friend WithEvents lblListenPort As System.Windows.Forms.Label
    Friend WithEvents chkboxEnableListening As System.Windows.Forms.CheckBox
    Friend WithEvents txtListenPort As System.Windows.Forms.TextBox
    Friend WithEvents cntxtmnuAcceptServer As System.Windows.Forms.ContextMenu
    Friend WithEvents cntxtmnuAcceptServerAcceptConnection As System.Windows.Forms.MenuItem
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(FrmListenForServers))
        Me.pnlServersList = New System.Windows.Forms.Panel()
        Me.lstwServersList = New System.Windows.Forms.ListView()
        Me.txtListenPort = New System.Windows.Forms.TextBox()
        Me.lblListenPort = New System.Windows.Forms.Label()
        Me.chkboxEnableListening = New System.Windows.Forms.CheckBox()
        Me.cntxtmnuAcceptServer = New System.Windows.Forms.ContextMenu()
        Me.cntxtmnuAcceptServerAcceptConnection = New System.Windows.Forms.MenuItem()
        Me.pnlServersList.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlServersList
        '
        Me.pnlServersList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlServersList.Controls.AddRange(New System.Windows.Forms.Control() {Me.lstwServersList})
        Me.pnlServersList.Location = New System.Drawing.Point(9, 50)
        Me.pnlServersList.Name = "pnlServersList"
        Me.pnlServersList.Size = New System.Drawing.Size(531, 150)
        Me.pnlServersList.TabIndex = 5
        '
        'lstwServersList
        '
        Me.lstwServersList.AllowColumnReorder = True
        Me.lstwServersList.ContextMenu = Me.cntxtmnuAcceptServer
        Me.lstwServersList.FullRowSelect = True
        Me.lstwServersList.GridLines = True
        Me.lstwServersList.Location = New System.Drawing.Point(8, 10)
        Me.lstwServersList.MultiSelect = False
        Me.lstwServersList.Name = "lstwServersList"
        Me.lstwServersList.Size = New System.Drawing.Size(510, 128)
        Me.lstwServersList.TabIndex = 0
        Me.lstwServersList.View = System.Windows.Forms.View.Details
        '
        'txtListenPort
        '
        Me.txtListenPort.Location = New System.Drawing.Point(194, 16)
        Me.txtListenPort.Name = "txtListenPort"
        Me.txtListenPort.Size = New System.Drawing.Size(87, 20)
        Me.txtListenPort.TabIndex = 4
        Me.txtListenPort.Text = ""
        Me.txtListenPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblListenPort
        '
        Me.lblListenPort.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblListenPort.Image = CType(resources.GetObject("lblListenPort.Image"), System.Drawing.Bitmap)
        Me.lblListenPort.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lblListenPort.Location = New System.Drawing.Point(9, 8)
        Me.lblListenPort.Name = "lblListenPort"
        Me.lblListenPort.Size = New System.Drawing.Size(184, 33)
        Me.lblListenPort.TabIndex = 3
        Me.lblListenPort.Text = "Listen at port..."
        Me.lblListenPort.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'chkboxEnableListening
        '
        Me.chkboxEnableListening.Location = New System.Drawing.Point(410, 16)
        Me.chkboxEnableListening.Name = "chkboxEnableListening"
        Me.chkboxEnableListening.Size = New System.Drawing.Size(130, 20)
        Me.chkboxEnableListening.TabIndex = 6
        Me.chkboxEnableListening.Text = "Enable Listening"
        '
        'cntxtmnuAcceptServer
        '
        Me.cntxtmnuAcceptServer.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.cntxtmnuAcceptServerAcceptConnection})
        '
        'cntxtmnuAcceptServerAcceptConnection
        '
        Me.cntxtmnuAcceptServerAcceptConnection.Index = 0
        Me.cntxtmnuAcceptServerAcceptConnection.Text = "Accept Connection"
        '
        'FrmListenForServers
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(548, 207)
        Me.Controls.AddRange(New System.Windows.Forms.Control() {Me.chkboxEnableListening, Me.pnlServersList, Me.txtListenPort, Me.lblListenPort})
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "FrmListenForServers"
        Me.Text = "Reverse Connection Wizard"
        Me.pnlServersList.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Protected Overrides Sub OnClosing(ByVal e As System.ComponentModel.CancelEventArgs)
        Me.Visible = False
        If (Not m_IsClose) Then
            e.Cancel = True
        Else
            Call StopThreadIfRunning()
        End If
    End Sub

    Friend Sub StopThreadIfRunning()
        If Not (m_ListenThread) Is Nothing Then
            m_blnContinueListen = False
            System.Threading.Thread.Sleep(500)
            If m_ListenThread.IsAlive Then
                m_ListenThread.Abort()
            End If
        End If
        m_ListenThread = Nothing
    End Sub

    Private Sub chkboxEnableListening_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkboxEnableListening.CheckedChanged
        If (chkboxEnableListening.Checked = True) Then
            If (Val(txtListenPort.Text) >= IDI_LOWER_PORT And Val(txtListenPort.Text) <= IDI_UPPER_PORT) Then
                If m_NwListener.ListenAt(False, CType(Val(txtListenPort.Text), Integer)) Then
                    If m_NwListener.SetListenState(True) Then
                        Call StopThreadIfRunning()
                        m_ListenThread = New System.Threading.Thread(AddressOf ThrdListenerThread)
                        m_blnContinueListen = True
                        m_ListenThread.Start()
                        Return
                    End If
                End If
            End If
        Else
            Call StopThreadIfRunning()
            Call m_NwListener.SetListenState(False)
            Return
        End If
        Dim strUty As New CStringUtility()
        strUty.MsgErr(IDS_LISTENING_FAILED, IDS_LISTENING_FAILED)
        strUty = Nothing
    End Sub

    Private Sub ThrdListenerThread()
        Dim Conn As CTcpNetworkMonitorConnection
        Dim lstwItem As ListViewItem
        Dim intPort As Integer
        Dim strIp As String

        While m_blnContinueListen = True
            If (m_NwListener.RemoveClient(Conn)) Then
                With Me.lstwServersList
                    Conn.GetIPandPort(True, strIp, intPort)
                    lstwItem = .Items.Add(IDS_NULL_STRING)
                    lstwItem.SubItems(0).Text = Str(lstwItem.Index + 1).Trim()
                    lstwItem.SubItems.Add(strIp)
                    lstwItem.SubItems.Add(Str(intPort).Trim)
                    lstwItem.Tag = Conn
                    .Refresh()
                End With
            End If
            System.Threading.Thread.Sleep(2000)
        End While

    End Sub


    Private Sub FrmListenForServers_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            With Me.lstwServersList
                .Clear()
                .Columns.Add("Slno", .Width * 1 / 7, HorizontalAlignment.Left)
                .Columns.Add("Server IP", .Width * 3 / 7, HorizontalAlignment.Left)
                .Columns.Add("Server Port", .Width * 3 / 7, HorizontalAlignment.Left)
                .Refresh()
            End With
        Catch ex As Exception : End Try
    End Sub

    Private Sub cntxtmnuAcceptServer_Popup(ByVal sender As Object, ByVal e As System.EventArgs) Handles cntxtmnuAcceptServer.Popup
        If lstwServersList.SelectedIndices.Count <= 0 Then
            cntxtmnuAcceptServerAcceptConnection.Enabled = False
        Else
            cntxtmnuAcceptServerAcceptConnection.Enabled = True
        End If
    End Sub

    Private Sub cntxtmnuAcceptServerAcceptConnection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cntxtmnuAcceptServerAcceptConnection.Click
        Dim WinMsgr As New CWinMonitorMessenger()
        Dim Connection As CTcpNetworkMonitorConnection = CType(lstwServersList.SelectedItems(0).Tag, CTcpNetworkMonitorConnection)
        Dim strIp As String
        Dim intPort As Integer
        Dim strPwd As String

        If WinMsgr.SetConnection(Connection) Then
            If WinMsgr.SynchronizeServer(False) Then
                strPwd = InputBox("Enter Server Password...", "Authenticating...", IDS_NULL_STRING)
                If WinMsgr.Authenticate(strPwd) Then
                    strIp = lstwServersList.SelectedItems(0).SubItems(1).Text
                    Dim CmdMntr As CCommandMonitorClient = New CCommandMonitorClient(COMMAND_MONITOR_NULL_COMMAND, IDS_NULL_STRING, COMMAND_MONITOR_COMPRESS_NIL)
                    CmdMntr.SetRemoteServerCompression(COMMAND_MONITOR_COMPRESS_NIL)
                    WinMsgr.ExecuteCommandMonitorClient(CmdMntr)
                    CmdMntr.SetCommand(COMMAND_MONITOR_LISTEN_PORT, IDS_NULL_STRING)
                    If WinMsgr.ExecuteCommandMonitorClient(CmdMntr) Then
                        If Uint32Equals(CmdMntr.GetResultantType(), COMMAND_MONITOR_LISTEN_PORT) Then
                            intPort = CInt(CmdMntr.GetResult())
                            CmdMntr.SetRemoteServerCompression(COMMAND_MONITOR_COMPRESS_LZSS)
                            WinMsgr.ExecuteCommandMonitorClient(CmdMntr)
                            lstwServersList.SelectedItems(0).Remove()
                            WinMsgr = Nothing
                            RaiseEvent eventGotConnection(Connection, strIp, intPort, strPwd)
                            Me.Close()
                            Return
                        End If
                    End If
                End If
            End If
        End If
        WinMsgr.ShutDown()
        Connection.Disconnect()
        WinMsgr = Nothing
        lstwServersList.SelectedItems(0).Remove()
        Dim struty As New CStringUtility()
        struty.MsgErr("Authentication failed...", "Connection error...") : struty = Nothing
        Me.Close()
        Return
    End Sub

    Private Sub FrmListenForServers_VisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.VisibleChanged
        If Me.Visible = True Then
            txtListenPort.Focus()
        End If
    End Sub

End Class
