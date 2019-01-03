

Imports WinMonitorBLIB_1_0

Public Class FrmWinMonitorServerList : Inherits System.Windows.Forms.Form

    Private m_ServerConfigFile As String
    Private m_XmlUty As New CXmlUtility()

#Region " Windows Form Designer generated code "

    Public Sub New(ByVal ServerConfigFile As String)
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        m_ServerConfigFile = ServerConfigFile
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
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(FrmWinMonitorServerList))
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
        Me.lblNoOfServersAvailable.Text = "Number of detected servers"
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
        'FrmWinMonitorServerList
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(546, 197)
        Me.Controls.AddRange(New System.Windows.Forms.Control() {Me.pnlServersList, Me.txtNoOfServersAvailable, Me.lblNoOfServersAvailable})
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "FrmWinMonitorServerList"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "WinMonitor.1.0 Servers List"
        Me.pnlServersList.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Function LoadServerConfigXML(ByVal ConfigFile As String) As Boolean

        If (Not m_XmlUty.OpenXmlDocument(ConfigFile)) Then
            MessageBox.Show(vbCrLf & ConfigFile & ":" & IDS_NON_EXIST_XML, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
            Return False
        End If
        Me.txtNoOfServersAvailable.Text = "0"
        Try
            With Me.lstwServersList
                .Clear()
                .Columns.Add("Server Name", .Width * 1 / 6, HorizontalAlignment.Left)
                .Columns.Add("Host Name", .Width * 1 / 6, HorizontalAlignment.Left)
                .Columns.Add("Listen Port", .Width * 1 / 6, HorizontalAlignment.Left)
                .Columns.Add("Reverse Port", .Width * 1 / 6, HorizontalAlignment.Left)
                .Columns.Add("Password", .Width * 1 / 6, HorizontalAlignment.Left)
                .Columns.Add("E-mail", .Width * 1 / 6, HorizontalAlignment.Left)
                .Refresh()
            End With
            If (Not m_XmlUty.MoveToChildByIndex("WinMonitorServers", 0, True)) Then
                MessageBox.Show(vbCrLf & IDS_INVALID_XML, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
                Return False
            End If

            Dim strvalue(6) As String
            Dim intIndx As Integer
            Dim boolOk As Boolean = False
            Do
                With m_XmlUty
                    .GetNodeTextFromHeirarchy("WinMonitorServer/WIN-MONITOR-SERVER/INTERNAL-NAME", strvalue(0), False)
                    .GetNodeTextFromHeirarchy("WinMonitorServer/NETWORK-MONITOR/CONNECTION/HOST", strvalue(1), False)
                    .GetNodeTextFromHeirarchy("WinMonitorServer/NETWORK-MONITOR/CONNECTION/PORT", strvalue(2), False)
                    .GetNodeTextFromHeirarchy("WinMonitorServer/NETWORK-MONITOR/REVERSE-CONNECTION/PORT", strvalue(3), False)
                    .GetNodeTextFromHeirarchy("WinMonitorServer/NETWORK-MONITOR/CONNECTION/PASSWORD", strvalue(4), False)
                    .GetNodeTextFromHeirarchy("WinMonitorServer/NETWORK-MONITOR/CONNECTION/EMAIL", strvalue(5), False)
                End With

                For intIndx = 0 To 5 Step 1
                    If strvalue(intIndx) <> IDS_NULL_STRING Then Exit For
                Next intIndx

                If (intIndx <> 6) Then
                    Dim lstwItem As ListViewItem
                    With Me.lstwServersList
                        lstwItem = .Items.Add(IIf(strvalue(0) = IDS_NULL_STRING, "N/A", strvalue(0)))
                        For intIndx = 1 To 5 Step 1
                            lstwItem.SubItems.Add(IIf(strvalue(intIndx) = IDS_NULL_STRING, "N/A", strvalue(intIndx)))
                        Next intIndx
                    End With
                    boolOk = True
                    Me.txtNoOfServersAvailable.Text = Str((Val(Me.txtNoOfServersAvailable.Text) + 1))
                End If

                For intIndx = 0 To 5 Step 1
                    strvalue(intIndx) = Nothing
                Next intIndx

            Loop While (m_XmlUty.MoveToBrother(False))

            Return boolOk
        Catch e As Exception
            Return False
        End Try
    End Function

    Protected Overrides Sub OnClosing(ByVal e As System.ComponentModel.CancelEventArgs)
        m_XmlUty = Nothing
    End Sub

    Private Sub FrmWinMonitorServerList_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Activated
        If (Not LoadServerConfigXML(m_ServerConfigFile)) Then
        End If

    End Sub

    Private Sub FrmWinMonitorServerList_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If (Not LoadServerConfigXML(m_ServerConfigFile)) Then
        End If
    End Sub
End Class
