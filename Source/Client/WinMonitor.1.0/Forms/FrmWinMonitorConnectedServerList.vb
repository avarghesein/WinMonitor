

Imports WinMonitorBLIB_1_0

Public Class FrmWinMonitorConnectedServerList : Inherits System.Windows.Forms.Form

    Friend m_IsClose As Boolean

#Region " Windows Form Designer generated code "

    Public WriteOnly Property CloseState() As Boolean
        Set(ByVal CLOSE As Boolean)
            m_IsClose = CLOSE
        End Set
    End Property


    Public Sub New(ByVal IsClose As Boolean)
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()
        'Add any initialization after the InitializeComponent() call
        m_IsClose = IsClose
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
    Friend WithEvents lblNoOfServersAvailable As System.Windows.Forms.Label
    Friend WithEvents txtNoOfServersAvailable As System.Windows.Forms.TextBox
    Friend WithEvents pnlServersList As System.Windows.Forms.Panel
    Friend WithEvents lstwServersList As System.Windows.Forms.ListView
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(FrmWinMonitorConnectedServerList))
        Me.lblNoOfServersAvailable = New System.Windows.Forms.Label()
        Me.txtNoOfServersAvailable = New System.Windows.Forms.TextBox()
        Me.pnlServersList = New System.Windows.Forms.Panel()
        Me.lstwServersList = New System.Windows.Forms.ListView()
        Me.pnlServersList.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblNoOfServersAvailable
        '
        Me.lblNoOfServersAvailable.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNoOfServersAvailable.Image = CType(resources.GetObject("lblNoOfServersAvailable.Image"), System.Drawing.Bitmap)
        Me.lblNoOfServersAvailable.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lblNoOfServersAvailable.Location = New System.Drawing.Point(6, 3)
        Me.lblNoOfServersAvailable.Name = "lblNoOfServersAvailable"
        Me.lblNoOfServersAvailable.Size = New System.Drawing.Size(184, 33)
        Me.lblNoOfServersAvailable.TabIndex = 0
        Me.lblNoOfServersAvailable.Text = "Number of Connected servers"
        Me.lblNoOfServersAvailable.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtNoOfServersAvailable
        '
        Me.txtNoOfServersAvailable.Enabled = False
        Me.txtNoOfServersAvailable.Location = New System.Drawing.Point(195, 9)
        Me.txtNoOfServersAvailable.Name = "txtNoOfServersAvailable"
        Me.txtNoOfServersAvailable.Size = New System.Drawing.Size(87, 20)
        Me.txtNoOfServersAvailable.TabIndex = 1
        Me.txtNoOfServersAvailable.Text = ""
        Me.txtNoOfServersAvailable.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'pnlServersList
        '
        Me.pnlServersList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlServersList.Controls.AddRange(New System.Windows.Forms.Control() {Me.lstwServersList})
        Me.pnlServersList.Location = New System.Drawing.Point(6, 36)
        Me.pnlServersList.Name = "pnlServersList"
        Me.pnlServersList.Size = New System.Drawing.Size(531, 150)
        Me.pnlServersList.TabIndex = 2
        '
        'lstwServersList
        '
        Me.lstwServersList.AllowColumnReorder = True
        Me.lstwServersList.FullRowSelect = True
        Me.lstwServersList.GridLines = True
        Me.lstwServersList.Location = New System.Drawing.Point(8, 10)
        Me.lstwServersList.MultiSelect = False
        Me.lstwServersList.Name = "lstwServersList"
        Me.lstwServersList.Size = New System.Drawing.Size(510, 128)
        Me.lstwServersList.TabIndex = 0
        Me.lstwServersList.View = System.Windows.Forms.View.Details
        '
        'FrmWinMonitorConnectedServerList
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(546, 197)
        Me.Controls.AddRange(New System.Windows.Forms.Control() {Me.pnlServersList, Me.txtNoOfServersAvailable, Me.lblNoOfServersAvailable})
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "FrmWinMonitorConnectedServerList"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "WinMonitor.1.0 Connected Servers List"
        Me.pnlServersList.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Protected Overrides Sub OnClosing(ByVal e As System.ComponentModel.CancelEventArgs)
        Me.Visible = False
        If (Not m_IsClose) Then e.Cancel = True
    End Sub

    Private Sub FrmWinMonitorServerList_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.txtNoOfServersAvailable.Text = "0"
        Try
            With Me.lstwServersList
                .Clear()
                .Columns.Add("Server Name", .Width * 1 / 4, HorizontalAlignment.Left)
                .Columns.Add("Server IP", .Width * 1 / 4, HorizontalAlignment.Left)
                .Columns.Add("Listen Port", .Width * 1 / 4, HorizontalAlignment.Left)
                .Columns.Add("Password", .Width * 1 / 4, HorizontalAlignment.Left)
                .Refresh()
            End With
        Catch ex As Exception : End Try
    End Sub

    Public Function AddServer(ByRef ServerConnection As CTcpNetworkMonitorConnection, ByVal ServerIp As String, ByVal ListenPort As String, ByVal PassWord As String) As Boolean
        Dim objWinMonMsgr As New CWinMonitorMessenger(ServerConnection)
        Dim objCmdMonitorClient As New CCommandMonitorClient(COMMAND_MONITOR_GET_WINSERVER_NAME, Nothing, COMMAND_MONITOR_COMPRESS_NIL)
        If (Not objWinMonMsgr.ExecuteCommandMonitorClient(objCmdMonitorClient)) Then
            objWinMonMsgr = Nothing
            objCmdMonitorClient = Nothing
            Return False
        End If
        If (Not Uint32Equals(objCmdMonitorClient.GetResultantType(), COMMAND_MONITOR_GOT_WINSERVER_NAME)) Then
            objWinMonMsgr = Nothing
            objCmdMonitorClient = Nothing
            Return False
        End If
        Dim strServerName As String : Dim NameLen As Long
        objCmdMonitorClient.GetResult(strServerName, NameLen)

        Dim lstwItem As ListViewItem
        With Me.lstwServersList
            lstwItem = .Items.Add(strServerName)
            lstwItem.SubItems.Add(ServerIp)
            lstwItem.SubItems.Add(ListenPort)
            lstwItem.SubItems.Add(PassWord)
            Dim frmCntrlr As New FrmWinMonitorControler(ServerConnection, PassWord, ServerIp, Val(ListenPort))
            frmCntrlr.MdiParent = Me.MdiParent
            frmCntrlr.StartPosition = FormStartPosition.CenterParent
            frmCntrlr.lblWelcome.Text = "Welcome To Server " & strServerName
            frmCntrlr.Visible = True
            lstwItem.Tag = frmCntrlr
            .Refresh()
            frmCntrlr.Tag = 1
        End With
        Me.txtNoOfServersAvailable.Text = Str(Val(Me.txtNoOfServersAvailable.Text) + 1)
        objWinMonMsgr = Nothing
        objCmdMonitorClient = Nothing
        Return True
    End Function

End Class
