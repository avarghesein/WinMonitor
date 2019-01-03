
Imports WinMonitorBLIB_1_0

Public Class FrmStartUpConnection : Inherits System.Windows.Forms.Form

    Private m_struty As New CStringUtility()
    Private m_boolConnected As Boolean
    Private m_ConnectionAcceptThrd As System.Threading.Thread
    Private m_ConnectionAcceptThrdSub As System.Threading.Thread
    Private m_TcpConnection As CTcpNetworkMonitorConnection
    Private m_intStatus As Integer
    Private m_WinMessenger As CWinMonitorMessenger
    Public Event eventGotConnection(ByRef TcpConnection As CTcpNetworkMonitorConnection)

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
    Friend WithEvents lblConnectionServerIP As System.Windows.Forms.Label
    Friend WithEvents txtConnectionServerIP As System.Windows.Forms.TextBox
    Friend WithEvents lblConnectionServerPort As System.Windows.Forms.Label
    Friend WithEvents txtConnectionServerPort As System.Windows.Forms.TextBox
    Friend WithEvents lblConnectionLine1 As System.Windows.Forms.Label
    Friend WithEvents picboxConnection As System.Windows.Forms.PictureBox
    Friend WithEvents btnConnect As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lblConnectionServerPassword As System.Windows.Forms.Label
    Friend WithEvents txtConnectionServerPassword As System.Windows.Forms.TextBox
    Friend WithEvents btnServerPasswordOk As System.Windows.Forms.Button
    Friend WithEvents lblConnectionStatus As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(FrmStartUpConnection))
        Me.lblConnectionServerIP = New System.Windows.Forms.Label()
        Me.txtConnectionServerIP = New System.Windows.Forms.TextBox()
        Me.lblConnectionServerPort = New System.Windows.Forms.Label()
        Me.txtConnectionServerPort = New System.Windows.Forms.TextBox()
        Me.lblConnectionLine1 = New System.Windows.Forms.Label()
        Me.btnConnect = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.picboxConnection = New System.Windows.Forms.PictureBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lblConnectionServerPassword = New System.Windows.Forms.Label()
        Me.txtConnectionServerPassword = New System.Windows.Forms.TextBox()
        Me.btnServerPasswordOk = New System.Windows.Forms.Button()
        Me.lblConnectionStatus = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'lblConnectionServerIP
        '
        Me.lblConnectionServerIP.Location = New System.Drawing.Point(60, 3)
        Me.lblConnectionServerIP.Name = "lblConnectionServerIP"
        Me.lblConnectionServerIP.Size = New System.Drawing.Size(63, 33)
        Me.lblConnectionServerIP.TabIndex = 0
        Me.lblConnectionServerIP.Text = "Server IP"
        Me.lblConnectionServerIP.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtConnectionServerIP
        '
        Me.txtConnectionServerIP.Font = New System.Drawing.Font("Courier New", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtConnectionServerIP.Location = New System.Drawing.Point(126, 6)
        Me.txtConnectionServerIP.Name = "txtConnectionServerIP"
        Me.txtConnectionServerIP.Size = New System.Drawing.Size(216, 21)
        Me.txtConnectionServerIP.TabIndex = 1
        Me.txtConnectionServerIP.Text = ""
        '
        'lblConnectionServerPort
        '
        Me.lblConnectionServerPort.Location = New System.Drawing.Point(60, 24)
        Me.lblConnectionServerPort.Name = "lblConnectionServerPort"
        Me.lblConnectionServerPort.Size = New System.Drawing.Size(63, 33)
        Me.lblConnectionServerPort.TabIndex = 2
        Me.lblConnectionServerPort.Text = "Server Port"
        Me.lblConnectionServerPort.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtConnectionServerPort
        '
        Me.txtConnectionServerPort.Font = New System.Drawing.Font("Courier New", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtConnectionServerPort.Location = New System.Drawing.Point(126, 33)
        Me.txtConnectionServerPort.Name = "txtConnectionServerPort"
        Me.txtConnectionServerPort.Size = New System.Drawing.Size(216, 21)
        Me.txtConnectionServerPort.TabIndex = 3
        Me.txtConnectionServerPort.Text = ""
        '
        'lblConnectionLine1
        '
        Me.lblConnectionLine1.BackColor = System.Drawing.Color.Silver
        Me.lblConnectionLine1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblConnectionLine1.Location = New System.Drawing.Point(3, 63)
        Me.lblConnectionLine1.Name = "lblConnectionLine1"
        Me.lblConnectionLine1.Size = New System.Drawing.Size(339, 3)
        Me.lblConnectionLine1.TabIndex = 4
        '
        'btnConnect
        '
        Me.btnConnect.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnConnect.Location = New System.Drawing.Point(177, 72)
        Me.btnConnect.Name = "btnConnect"
        Me.btnConnect.Size = New System.Drawing.Size(80, 20)
        Me.btnConnect.TabIndex = 5
        Me.btnConnect.Text = "Connect"
        '
        'btnCancel
        '
        Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnCancel.Location = New System.Drawing.Point(262, 72)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(80, 20)
        Me.btnCancel.TabIndex = 6
        Me.btnCancel.Text = "Cancel"
        '
        'picboxConnection
        '
        Me.picboxConnection.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.picboxConnection.Image = CType(resources.GetObject("picboxConnection.Image"), System.Drawing.Bitmap)
        Me.picboxConnection.Location = New System.Drawing.Point(6, 6)
        Me.picboxConnection.Name = "picboxConnection"
        Me.picboxConnection.Size = New System.Drawing.Size(51, 48)
        Me.picboxConnection.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.picboxConnection.TabIndex = 7
        Me.picboxConnection.TabStop = False
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.Silver
        Me.Label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label1.Location = New System.Drawing.Point(5, 99)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(339, 3)
        Me.Label1.TabIndex = 8
        '
        'lblConnectionServerPassword
        '
        Me.lblConnectionServerPassword.Location = New System.Drawing.Point(15, 102)
        Me.lblConnectionServerPassword.Name = "lblConnectionServerPassword"
        Me.lblConnectionServerPassword.Size = New System.Drawing.Size(63, 33)
        Me.lblConnectionServerPassword.TabIndex = 9
        Me.lblConnectionServerPassword.Text = "Password"
        Me.lblConnectionServerPassword.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtConnectionServerPassword
        '
        Me.txtConnectionServerPassword.Font = New System.Drawing.Font("Courier New", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtConnectionServerPassword.Location = New System.Drawing.Point(81, 108)
        Me.txtConnectionServerPassword.Name = "txtConnectionServerPassword"
        Me.txtConnectionServerPassword.Size = New System.Drawing.Size(177, 21)
        Me.txtConnectionServerPassword.TabIndex = 10
        Me.txtConnectionServerPassword.Text = ""
        '
        'btnServerPasswordOk
        '
        Me.btnServerPasswordOk.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnServerPasswordOk.Location = New System.Drawing.Point(262, 108)
        Me.btnServerPasswordOk.Name = "btnServerPasswordOk"
        Me.btnServerPasswordOk.Size = New System.Drawing.Size(80, 20)
        Me.btnServerPasswordOk.TabIndex = 11
        Me.btnServerPasswordOk.Text = "Ok"
        '
        'lblConnectionStatus
        '
        Me.lblConnectionStatus.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblConnectionStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblConnectionStatus.ForeColor = System.Drawing.Color.Red
        Me.lblConnectionStatus.Location = New System.Drawing.Point(4, 72)
        Me.lblConnectionStatus.Name = "lblConnectionStatus"
        Me.lblConnectionStatus.Size = New System.Drawing.Size(168, 21)
        Me.lblConnectionStatus.TabIndex = 12
        Me.lblConnectionStatus.Text = "Status:"
        Me.lblConnectionStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'FrmStartUpConnection
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(348, 134)
        Me.Controls.AddRange(New System.Windows.Forms.Control() {Me.lblConnectionStatus, Me.btnServerPasswordOk, Me.txtConnectionServerPassword, Me.lblConnectionServerPassword, Me.Label1, Me.picboxConnection, Me.btnCancel, Me.btnConnect, Me.lblConnectionLine1, Me.txtConnectionServerPort, Me.lblConnectionServerPort, Me.txtConnectionServerIP, Me.lblConnectionServerIP})
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "FrmStartUpConnection"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "WinMonitor.1.0 Connection Wizard"
        Me.ResumeLayout(False)

    End Sub

#End Region


    Private Sub FrmStartUpConnection_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.txtConnectionServerPassword.Enabled = False
        Me.btnServerPasswordOk.Enabled = False
        Me.btnCancel.Enabled = False
        Me.btnConnect.Enabled = True
        Me.lblConnectionStatus.Text = "Status:ready..."
        m_ConnectionAcceptThrd = Nothing
        m_boolConnected = False
    End Sub


    Private Sub btnConnect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConnect.Click
        If (Not m_struty.ValidIP(txtConnectionServerIP.Text)) Then
            MessageBox.Show(IDS_IPADDRESS_INVALID, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
            txtConnectionServerIP.Focus()
            SendKeys.Send("{END}")
            Return
        End If
        Dim intPort As Integer = Val(txtConnectionServerPort.Text)
        If (intPort < IDI_LOWER_PORT Or intPort > IDI_UPPER_PORT) Then
            MessageBox.Show(IDS_PORTNUMBER_INVALID, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
            txtConnectionServerPort.Focus()
            SendKeys.Send("{END}")
            Return
        End If
        Me.btnConnect.Enabled = False
        Me.btnCancel.Enabled = True
        m_ConnectionAcceptThrd = New Threading.Thread(AddressOf AcceptConnection)
        m_ConnectionAcceptThrd.Start()
    End Sub

    Private Sub AcceptConnection()
        m_boolConnected = False
        m_TcpConnection = New CTcpNetworkMonitorConnection()
        m_intStatus = IDI_TRYING_CONNECTION
        m_ConnectionAcceptThrdSub = New Threading.Thread(AddressOf Respond)
        m_ConnectionAcceptThrdSub.Start()

        While (Not m_TcpConnection.ConnectTo(txtConnectionServerIP.Text, Val(txtConnectionServerPort.Text))) : End While
        m_intStatus = IDI_MAINTAIN_CONNECTION
        m_WinMessenger = New CWinMonitorMessenger(m_TcpConnection)

        If Not m_WinMessenger.SetConnection(m_TcpConnection) Then
            MessageBox.Show(IDS_WIN_SERVICER_FAILED, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
            Call btnCancel_Click(Nothing, Nothing)
            Call FrmStartUpConnection_Load(Nothing, Nothing)
            Return
        End If

        If Not m_WinMessenger.SynchronizeServer() Then
            MessageBox.Show(IDS_SYNC_SERVER_FAILED, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
            Call btnCancel_Click(Nothing, Nothing)
            Call FrmStartUpConnection_Load(Nothing, Nothing)
            Return
        End If

        Try
            If Not (m_ConnectionAcceptThrdSub Is Nothing) Then
                If (m_ConnectionAcceptThrdSub.IsAlive()) Then
                    m_ConnectionAcceptThrdSub.Abort()
                End If
            End If
            m_ConnectionAcceptThrdSub = Nothing
        Catch ex As Exception : End Try

        Me.lblConnectionStatus.Text = "Status:connected ok..."
        Me.btnConnect.Enabled = Me.btnCancel.Enabled = False
        Me.btnServerPasswordOk.Enabled = True
        Me.txtConnectionServerPassword.Enabled = True
        Me.txtConnectionServerPassword.Focus()
        Me.txtConnectionServerPassword.Focus()
    End Sub

    Private Sub Respond()
        Dim intCount As Integer = 3
        Me.lblConnectionStatus.Text = IIf(m_intStatus = IDI_TRYING_CONNECTION, "Status:connecting...", IIf(m_intStatus = IDI_VALIDATING_CONNECTION, "Status:validating user", "Status:maintaining"))
        While (True)
            intCount += 1
            If (intCount > 15) Then intCount = 0
            Me.lblConnectionStatus.Text = IIf(m_intStatus = IDI_TRYING_CONNECTION, "Status:connecting...", IIf(m_intStatus = IDI_VALIDATING_CONNECTION, "Status:validating user", "Status:maintaining"))
            Dim intIndx As Integer
            For intIndx = 0 To intCount Step 1
                Me.lblConnectionStatus.Text &= IDS_DOT
            Next intIndx
            System.Threading.Thread.Sleep(100)
        End While
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Try
            If Not (m_ConnectionAcceptThrdSub Is Nothing) Then
                If (m_ConnectionAcceptThrdSub.IsAlive()) Then
                    m_ConnectionAcceptThrdSub.Abort()
                End If
            End If
            m_ConnectionAcceptThrdSub = Nothing
        Catch ex As Exception : End Try

        Try
            If Not (m_ConnectionAcceptThrd Is Nothing) Then
                If (m_ConnectionAcceptThrd.IsAlive()) Then
                    m_ConnectionAcceptThrd.Abort()
                End If
            End If
            m_ConnectionAcceptThrd = Nothing
        Catch ex As Exception : End Try
        Me.btnCancel.Enabled = False
        Me.btnConnect.Enabled = True
        Me.lblConnectionStatus.Text = "Status:aborted by user..."
    End Sub

    Protected Overrides Sub OnClosing(ByVal e As System.ComponentModel.CancelEventArgs)
        Try
            Call btnCancel_Click(Nothing, e)
        Catch ex As Exception : End Try
    End Sub

    Private Sub btnServerPasswordOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnServerPasswordOk.Click
        m_ConnectionAcceptThrd = New Threading.Thread(AddressOf AuthenticateConnection)
        m_ConnectionAcceptThrd.Start()
        btnServerPasswordOk.Enabled = False
        m_ConnectionAcceptThrd.Join()
        Cursor.Current = Cursors.WaitCursor
        RaiseEvent eventGotConnection(m_TcpConnection)
        Me.Close()
    End Sub

    Private Sub AuthenticateConnection()
        m_boolConnected = True
        m_intStatus = IDI_VALIDATING_CONNECTION
        m_ConnectionAcceptThrdSub = New Threading.Thread(AddressOf Respond)
        m_ConnectionAcceptThrdSub.Start()

        If Not m_WinMessenger.Authenticate(Me.txtConnectionServerPassword.Text) Then
            MessageBox.Show(IDS_CONNECTION_AUTHENTICATION_FAILED, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
            Me.lblConnectionStatus.Text = "Status:validation failed..."
            m_TcpConnection = Nothing
            Return
        End If

        Try
            If Not (m_ConnectionAcceptThrdSub Is Nothing) Then
                If (m_ConnectionAcceptThrdSub.IsAlive()) Then
                    m_ConnectionAcceptThrdSub.Abort()
                End If
            End If
            m_ConnectionAcceptThrdSub = Nothing
        Catch ex As Exception : End Try
        Me.lblConnectionStatus.Text = "Status:validation success..."
        Return
    End Sub


    Private Sub txtConnectionServerIP_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtConnectionServerIP.KeyDown
        If (e.KeyCode = Keys.Enter) Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub txtConnectionServerPort_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtConnectionServerPort.KeyDown
        If (e.KeyCode = Keys.Enter) Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub txtConnectionServerPassword_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtConnectionServerPassword.KeyDown
        If (e.KeyCode = Keys.Enter) Then SendKeys.Send("{TAB}")
    End Sub

End Class
