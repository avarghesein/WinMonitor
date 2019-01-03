
Imports WinMonitorBLIB_1_0
Imports System.IO

Public Class usrctrlFolderBrowser : Inherits System.Windows.Forms.UserControl

#Region "Variables and Properties"

    Private m_ServerPwd As String
    Private m_ServerIp As String
    Private m_ServerListenPort As Integer

    Private m_WinMessenger As CWinMonitorMessenger
    Private m_ServerConnection As CTcpNetworkMonitorConnection
    Private m_CmdMonitorClient As CCommandMonitorClient
    Private m_strUty As CStringUtility
    Private m_ImgList As CExtendedImageList
    Private m_ImgLstIconSize As Size
    Private m_objFileMonitor As New CFileMonitor()

    Private m_Thread As System.Threading.Thread
    Private m_ThreadDownLoad As System.Threading.Thread
    Private m_frmResponse As Form
    Private m_blnRefresh As Boolean
    Private m_blnContinueDownLoad As Boolean
    Private m_blnDownLoadError As Boolean
    Private m_strTargetDir As String
    Private m_blnCreateFolderFlg As Boolean
    Private m_UploadThread As System.Threading.Thread
    Private m_blnUpLoadOk As Boolean
    Private m_strFolderArray() As String
    Private m_blnUpLoadingFolder As Boolean

    Public ReadOnly Property SelectedFolder()
        Get
            Return txtFolderName.Text
        End Get
    End Property

    Public WriteOnly Property IconSize_32x32() As Boolean
        Set(ByVal Type_32x32_Icon As Boolean)
            If (Type_32x32_Icon) Then
                m_ImgLstIconSize.Height = 32
                m_ImgLstIconSize.Width = 32
            Else
                m_ImgLstIconSize.Height = 16
                m_ImgLstIconSize.Width = 16
            End If
        End Set
    End Property

#End Region

#Region " Windows Form Designer generated code "

    'UserControl overrides dispose to clean up the component list.
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
    Friend WithEvents txtSelectedFolder As System.Windows.Forms.TextBox
    Friend WithEvents txtFolderName As System.Windows.Forms.TextBox
    Friend WithEvents trewFolders As System.Windows.Forms.TreeView
    Friend WithEvents spltrWindows As System.Windows.Forms.Splitter
    Friend WithEvents lstwFolders As System.Windows.Forms.ListView
    Friend WithEvents tlbarFolders As System.Windows.Forms.ToolBar
    Friend WithEvents tlbarbtnNewFolder As System.Windows.Forms.ToolBarButton
    Friend WithEvents tlbarbtnGoFolderUp As System.Windows.Forms.ToolBarButton
    Friend WithEvents tlbarbtnDeleteFolder As System.Windows.Forms.ToolBarButton
    Friend WithEvents tlbarbtnSeperator1 As System.Windows.Forms.ToolBarButton
    Friend WithEvents tlbarbtnSeperator2 As System.Windows.Forms.ToolBarButton
    Friend WithEvents tlbarbtnSeperator3 As System.Windows.Forms.ToolBarButton
    Friend WithEvents cntxtmnuServerSideFileBrowser As System.Windows.Forms.ContextMenu
    Friend WithEvents cntxtmnuServerSideFileBrowserDownload As System.Windows.Forms.MenuItem
    Friend WithEvents cntxtmnuServerSideFileBrowserExecute As System.Windows.Forms.MenuItem
    Friend WithEvents cntxtmnuSeperator4 As System.Windows.Forms.MenuItem
    Friend WithEvents cntxtmnuSeperator5 As System.Windows.Forms.MenuItem
    Friend WithEvents cntxtmnuServerSideFileBrowserProperties As System.Windows.Forms.MenuItem
    Friend WithEvents imgLstIcons As System.Windows.Forms.ImageList
    Friend WithEvents cntxtmnuServerSideFileBrowserRefresh As System.Windows.Forms.MenuItem
    Friend WithEvents cntxtmnuSeperator6 As System.Windows.Forms.MenuItem
    Friend WithEvents cntxtmnuSeperator7 As System.Windows.Forms.MenuItem
    Friend WithEvents cntxtmnuServerSideFileBrowserDelete As System.Windows.Forms.MenuItem
    Friend WithEvents cntxtmnuSeperator8 As System.Windows.Forms.MenuItem
    Friend WithEvents cntxtmnuServerSideFileBrowserUpload As System.Windows.Forms.MenuItem
    Friend WithEvents cntxtmnuSeperator9 As System.Windows.Forms.MenuItem
    Friend WithEvents cntxtmnuServerSideFileBrowserNewFolder As System.Windows.Forms.MenuItem
    Friend WithEvents opnfiledlgForUploading As System.Windows.Forms.OpenFileDialog
    Friend WithEvents MenuItem1 As System.Windows.Forms.MenuItem
    Friend WithEvents cntxtmnuServerSideFileBrowserUploadFolder As System.Windows.Forms.MenuItem
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(usrctrlFolderBrowser))
        Me.txtSelectedFolder = New System.Windows.Forms.TextBox()
        Me.txtFolderName = New System.Windows.Forms.TextBox()
        Me.tlbarFolders = New System.Windows.Forms.ToolBar()
        Me.tlbarbtnNewFolder = New System.Windows.Forms.ToolBarButton()
        Me.tlbarbtnSeperator1 = New System.Windows.Forms.ToolBarButton()
        Me.tlbarbtnGoFolderUp = New System.Windows.Forms.ToolBarButton()
        Me.tlbarbtnSeperator2 = New System.Windows.Forms.ToolBarButton()
        Me.tlbarbtnDeleteFolder = New System.Windows.Forms.ToolBarButton()
        Me.tlbarbtnSeperator3 = New System.Windows.Forms.ToolBarButton()
        Me.imgLstIcons = New System.Windows.Forms.ImageList(Me.components)
        Me.trewFolders = New System.Windows.Forms.TreeView()
        Me.spltrWindows = New System.Windows.Forms.Splitter()
        Me.lstwFolders = New System.Windows.Forms.ListView()
        Me.cntxtmnuServerSideFileBrowser = New System.Windows.Forms.ContextMenu()
        Me.cntxtmnuServerSideFileBrowserDownload = New System.Windows.Forms.MenuItem()
        Me.cntxtmnuSeperator7 = New System.Windows.Forms.MenuItem()
        Me.cntxtmnuServerSideFileBrowserNewFolder = New System.Windows.Forms.MenuItem()
        Me.cntxtmnuSeperator8 = New System.Windows.Forms.MenuItem()
        Me.cntxtmnuServerSideFileBrowserUpload = New System.Windows.Forms.MenuItem()
        Me.cntxtmnuSeperator5 = New System.Windows.Forms.MenuItem()
        Me.cntxtmnuServerSideFileBrowserUploadFolder = New System.Windows.Forms.MenuItem()
        Me.MenuItem1 = New System.Windows.Forms.MenuItem()
        Me.cntxtmnuServerSideFileBrowserDelete = New System.Windows.Forms.MenuItem()
        Me.cntxtmnuSeperator4 = New System.Windows.Forms.MenuItem()
        Me.cntxtmnuServerSideFileBrowserExecute = New System.Windows.Forms.MenuItem()
        Me.cntxtmnuSeperator6 = New System.Windows.Forms.MenuItem()
        Me.cntxtmnuServerSideFileBrowserRefresh = New System.Windows.Forms.MenuItem()
        Me.cntxtmnuSeperator9 = New System.Windows.Forms.MenuItem()
        Me.cntxtmnuServerSideFileBrowserProperties = New System.Windows.Forms.MenuItem()
        Me.opnfiledlgForUploading = New System.Windows.Forms.OpenFileDialog()
        Me.SuspendLayout()
        '
        'txtSelectedFolder
        '
        Me.txtSelectedFolder.BackColor = System.Drawing.Color.White
        Me.txtSelectedFolder.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.txtSelectedFolder.ForeColor = System.Drawing.Color.Black
        Me.txtSelectedFolder.Location = New System.Drawing.Point(0, 228)
        Me.txtSelectedFolder.Name = "txtSelectedFolder"
        Me.txtSelectedFolder.ReadOnly = True
        Me.txtSelectedFolder.Size = New System.Drawing.Size(400, 20)
        Me.txtSelectedFolder.TabIndex = 1
        Me.txtSelectedFolder.Text = ""
        '
        'txtFolderName
        '
        Me.txtFolderName.BackColor = System.Drawing.Color.White
        Me.txtFolderName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtFolderName.ForeColor = System.Drawing.Color.Black
        Me.txtFolderName.Location = New System.Drawing.Point(178, 136)
        Me.txtFolderName.Multiline = True
        Me.txtFolderName.Name = "txtFolderName"
        Me.txtFolderName.Size = New System.Drawing.Size(112, 32)
        Me.txtFolderName.TabIndex = 5
        Me.txtFolderName.Text = ""
        Me.txtFolderName.Visible = False
        '
        'tlbarFolders
        '
        Me.tlbarFolders.Appearance = System.Windows.Forms.ToolBarAppearance.Flat
        Me.tlbarFolders.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tlbarFolders.Buttons.AddRange(New System.Windows.Forms.ToolBarButton() {Me.tlbarbtnNewFolder, Me.tlbarbtnSeperator1, Me.tlbarbtnGoFolderUp, Me.tlbarbtnSeperator2, Me.tlbarbtnDeleteFolder, Me.tlbarbtnSeperator3})
        Me.tlbarFolders.ButtonSize = New System.Drawing.Size(25, 22)
        Me.tlbarFolders.DropDownArrows = True
        Me.tlbarFolders.ImageList = Me.imgLstIcons
        Me.tlbarFolders.Name = "tlbarFolders"
        Me.tlbarFolders.ShowToolTips = True
        Me.tlbarFolders.Size = New System.Drawing.Size(400, 26)
        Me.tlbarFolders.TabIndex = 6
        Me.tlbarFolders.TextAlign = System.Windows.Forms.ToolBarTextAlign.Right
        '
        'tlbarbtnNewFolder
        '
        Me.tlbarbtnNewFolder.ImageIndex = 0
        Me.tlbarbtnNewFolder.Text = "New"
        '
        'tlbarbtnSeperator1
        '
        Me.tlbarbtnSeperator1.Style = System.Windows.Forms.ToolBarButtonStyle.Separator
        '
        'tlbarbtnGoFolderUp
        '
        Me.tlbarbtnGoFolderUp.ImageIndex = 1
        Me.tlbarbtnGoFolderUp.Text = "Up"
        '
        'tlbarbtnSeperator2
        '
        Me.tlbarbtnSeperator2.Style = System.Windows.Forms.ToolBarButtonStyle.Separator
        '
        'tlbarbtnDeleteFolder
        '
        Me.tlbarbtnDeleteFolder.ImageIndex = 2
        Me.tlbarbtnDeleteFolder.Text = "Delete"
        '
        'tlbarbtnSeperator3
        '
        Me.tlbarbtnSeperator3.Style = System.Windows.Forms.ToolBarButtonStyle.Separator
        '
        'imgLstIcons
        '
        Me.imgLstIcons.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit
        Me.imgLstIcons.ImageSize = New System.Drawing.Size(16, 16)
        Me.imgLstIcons.ImageStream = CType(resources.GetObject("imgLstIcons.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imgLstIcons.TransparentColor = System.Drawing.Color.Transparent
        '
        'trewFolders
        '
        Me.trewFolders.BackColor = System.Drawing.Color.White
        Me.trewFolders.Dock = System.Windows.Forms.DockStyle.Left
        Me.trewFolders.ForeColor = System.Drawing.Color.Black
        Me.trewFolders.ImageIndex = -1
        Me.trewFolders.Location = New System.Drawing.Point(0, 26)
        Me.trewFolders.Name = "trewFolders"
        Me.trewFolders.SelectedImageIndex = -1
        Me.trewFolders.Size = New System.Drawing.Size(118, 202)
        Me.trewFolders.TabIndex = 7
        '
        'spltrWindows
        '
        Me.spltrWindows.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.spltrWindows.Location = New System.Drawing.Point(118, 26)
        Me.spltrWindows.Name = "spltrWindows"
        Me.spltrWindows.Size = New System.Drawing.Size(2, 202)
        Me.spltrWindows.TabIndex = 8
        Me.spltrWindows.TabStop = False
        '
        'lstwFolders
        '
        Me.lstwFolders.BackColor = System.Drawing.Color.White
        Me.lstwFolders.ContextMenu = Me.cntxtmnuServerSideFileBrowser
        Me.lstwFolders.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstwFolders.ForeColor = System.Drawing.Color.Black
        Me.lstwFolders.Location = New System.Drawing.Point(120, 26)
        Me.lstwFolders.Name = "lstwFolders"
        Me.lstwFolders.Size = New System.Drawing.Size(280, 202)
        Me.lstwFolders.TabIndex = 9
        '
        'cntxtmnuServerSideFileBrowser
        '
        Me.cntxtmnuServerSideFileBrowser.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.cntxtmnuServerSideFileBrowserDownload, Me.cntxtmnuSeperator7, Me.cntxtmnuServerSideFileBrowserNewFolder, Me.cntxtmnuSeperator8, Me.cntxtmnuServerSideFileBrowserUpload, Me.cntxtmnuSeperator5, Me.cntxtmnuServerSideFileBrowserUploadFolder, Me.MenuItem1, Me.cntxtmnuServerSideFileBrowserDelete, Me.cntxtmnuSeperator4, Me.cntxtmnuServerSideFileBrowserExecute, Me.cntxtmnuSeperator6, Me.cntxtmnuServerSideFileBrowserRefresh, Me.cntxtmnuSeperator9, Me.cntxtmnuServerSideFileBrowserProperties})
        '
        'cntxtmnuServerSideFileBrowserDownload
        '
        Me.cntxtmnuServerSideFileBrowserDownload.Index = 0
        Me.cntxtmnuServerSideFileBrowserDownload.Text = "Download Files"
        '
        'cntxtmnuSeperator7
        '
        Me.cntxtmnuSeperator7.Index = 1
        Me.cntxtmnuSeperator7.Text = "-"
        '
        'cntxtmnuServerSideFileBrowserNewFolder
        '
        Me.cntxtmnuServerSideFileBrowserNewFolder.Index = 2
        Me.cntxtmnuServerSideFileBrowserNewFolder.Text = "New Folder"
        '
        'cntxtmnuSeperator8
        '
        Me.cntxtmnuSeperator8.Index = 3
        Me.cntxtmnuSeperator8.Text = "-"
        '
        'cntxtmnuServerSideFileBrowserUpload
        '
        Me.cntxtmnuServerSideFileBrowserUpload.Index = 4
        Me.cntxtmnuServerSideFileBrowserUpload.Text = "Upload Files"
        '
        'cntxtmnuSeperator5
        '
        Me.cntxtmnuSeperator5.Index = 5
        Me.cntxtmnuSeperator5.Text = "-"
        '
        'cntxtmnuServerSideFileBrowserUploadFolder
        '
        Me.cntxtmnuServerSideFileBrowserUploadFolder.Index = 6
        Me.cntxtmnuServerSideFileBrowserUploadFolder.Text = "Upload Folder"
        '
        'MenuItem1
        '
        Me.MenuItem1.Index = 7
        Me.MenuItem1.Text = "-"
        '
        'cntxtmnuServerSideFileBrowserDelete
        '
        Me.cntxtmnuServerSideFileBrowserDelete.Index = 8
        Me.cntxtmnuServerSideFileBrowserDelete.Text = "Delete"
        '
        'cntxtmnuSeperator4
        '
        Me.cntxtmnuSeperator4.Index = 9
        Me.cntxtmnuSeperator4.Text = "-"
        '
        'cntxtmnuServerSideFileBrowserExecute
        '
        Me.cntxtmnuServerSideFileBrowserExecute.Index = 10
        Me.cntxtmnuServerSideFileBrowserExecute.Text = "Execute Application"
        '
        'cntxtmnuSeperator6
        '
        Me.cntxtmnuSeperator6.Index = 11
        Me.cntxtmnuSeperator6.Text = "-"
        '
        'cntxtmnuServerSideFileBrowserRefresh
        '
        Me.cntxtmnuServerSideFileBrowserRefresh.Index = 12
        Me.cntxtmnuServerSideFileBrowserRefresh.Text = "Refresh"
        '
        'cntxtmnuSeperator9
        '
        Me.cntxtmnuSeperator9.Index = 13
        Me.cntxtmnuSeperator9.Text = "-"
        '
        'cntxtmnuServerSideFileBrowserProperties
        '
        Me.cntxtmnuServerSideFileBrowserProperties.Index = 14
        Me.cntxtmnuServerSideFileBrowserProperties.Text = "Properties"
        '
        'usrctrlFolderBrowser
        '
        Me.AutoScroll = True
        Me.Controls.AddRange(New System.Windows.Forms.Control() {Me.txtFolderName, Me.lstwFolders, Me.spltrWindows, Me.trewFolders, Me.tlbarFolders, Me.txtSelectedFolder})
        Me.Name = "usrctrlFolderBrowser"
        Me.Size = New System.Drawing.Size(400, 248)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Dim reslen As Long

    Public Sub New()
        '-----Default Operations--------'   
        MyBase.New() : InitializeComponent()
        '-----End Default Operations----'
    End Sub


    Private Function GetPath(ByVal e As TreeNode) As String
        Dim AbsolutePath As String = e.Text + "\"
        While Not (e.Parent Is Nothing)
            e = e.Parent
            AbsolutePath = e.Text + "\" + AbsolutePath
        End While
        AbsolutePath = AbsolutePath.Substring(trewFolders.Nodes(0).Text.Length + 1)
        Return AbsolutePath
    End Function

    Private Sub Responser()
        While (m_blnRefresh)
            Dim cntrl As Control
            For Each cntrl In m_frmResponse.Controls
                cntrl.Refresh()
            Next cntrl
            m_frmResponse.Refresh()
            Threading.Thread.Sleep(100)
        End While
    End Sub

    Private Sub CloseForTreeview(Optional ByRef frmResponse As FrmResponse = Nothing)
        m_blnRefresh = False
        m_Thread.Join()
        m_Thread = Nothing
        If (frmResponse Is Nothing) Then Return
        frmResponse.Close()
        frmResponse = Nothing
    End Sub

    Private Sub trewFolders_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles trewFolders.AfterSelect

        If (e.Node Is Nothing) Then Exit Sub

        Try
            If Not (e.Node.FirstNode Is Nothing) Then
                If (e.Node.FirstNode.Tag Is Nothing) Then
                    e.Node.Nodes.Clear()
                End If
            End If
        Catch : End Try

        'm_ImgList.CurrentImageList.ImageSize = m_ImgLstIconSize

        If (e.Node.FirstNode Is Nothing) Then
            Dim Folders As String()
            Dim CurrentPath As String
            Dim Folder As String
            Dim SlashIndex As Long
            Dim Treenode As Treenode
            With trewFolders
                Dim frmResponse As New frmResponse()
                Try
                    CurrentPath = GetPath(e.Node)
                    If CType(e.Node.Tag, String) <> "ROOT-NODES" Then
                        If CType(e.Node.Tag, Integer) <> 0 Then Return
                    End If

                    With frmResponse
                        .lblRespondCaption.Text = "Collecting information..."
                        .lblProgressBar.Left = .lblProgressBarBase.Left + 2
                        .lblProgressBar.Width = 1
                        .Show()
                    End With
                    m_blnRefresh = True
                    m_frmResponse = frmResponse
                    m_Thread = New Threading.Thread(AddressOf Responser)
                    m_Thread.Start()

                    If (Not m_CmdMonitorClient.SetCommand(COMMAND_MONITOR_GET_SUB_DIRS, CurrentPath & "*")) Then
                        Call CloseForTreeview(frmResponse)
                        Return
                    End If

                    If (Not m_WinMessenger.ExecuteCommandMonitorClient(m_CmdMonitorClient)) Then
                        Call CloseForTreeview(frmResponse)
                        Return
                    End If

                    If (Not Uint32Equals(m_CmdMonitorClient.GetResultantType(), COMMAND_MONITOR_GOT_SUB_DIRS)) Then
                        Call CloseForTreeview(frmResponse)
                        txtSelectedFolder.Text = CurrentPath
                        txtSelectedFolder.Tag = e
                        lstwFolders.Clear()
                        lstwFolders.Refresh()
                        If (e.Node.Parent Is Nothing) Then
                            tlbarbtnGoFolderUp.Enabled = False
                        Else
                            tlbarbtnGoFolderUp.Enabled = True
                        End If
                        Call cntxtmnuServerSideFileBrowser_Popup(sender, New EventArgs())
                        Return
                    End If

                    If (Not m_objFileMonitor.SetFileDataStream(m_CmdMonitorClient)) Then
                        Call CloseForTreeview(frmResponse)
                        Return
                    End If
                    Dim intNumFiles As Integer = m_objFileMonitor.GetNumberOfFiles()
                    Dim intCountedFiles As Integer = 0

                    txtSelectedFolder.Text = CurrentPath
                    txtSelectedFolder.Tag = e

                    Call CloseForTreeview()

                    lstwFolders.Clear()
                    Dim FileOrFolder As String
                    m_objFileMonitor.GetFirstFile(FileOrFolder, CFileMonitor_FILENAME_ONLY)
                    Dim FileType As Integer
                    Do
                        intCountedFiles += 1
                        Try

                            FileOrFolder = FileOrFolder.Trim()
                            If Not (FileOrFolder = IDS_DOT Or FileOrFolder = ".." Or FileOrFolder.Length >= 50) Then
                                m_objFileMonitor.GetCurrentFileType(FileType)
                                SlashIndex = FileOrFolder.LastIndexOf("\")
                                SlashIndex = IIf(SlashIndex = 0, 1, SlashIndex)
                                If (m_objFileMonitor.IsCurrentFileIsDirectory() = True) Then
                                    Treenode = New Treenode(FileOrFolder.Substring(SlashIndex + 1), 6, 6)
                                    lstwFolders.Items.Add(FileOrFolder.Substring(SlashIndex + 1), 6)
                                    Treenode.Tag = 0
                                Else
                                    Treenode = New Treenode(FileOrFolder.Substring(SlashIndex + 1), m_ImgList.GetImageIndexByFIleExtension(FileOrFolder), m_ImgList.GetImageIndexByFIleExtension(FileOrFolder))
                                    lstwFolders.Items.Add(FileOrFolder.Substring(SlashIndex + 1), m_ImgList.GetImageIndexByFIleExtension(FileOrFolder))
                                    Treenode.Tag = 1
                                End If
                                e.Node.Nodes.Add(Treenode)
                                lstwFolders.Items(lstwFolders.Items.Count - 1).Tag = Treenode

                                Dim strProp As String : Dim lngSze As Long
                                If (m_objFileMonitor.GetCurrentFileSize(lngSze) = True And lngSze > 0) Then
                                    m_objFileMonitor.GetCurrentFile(strProp, CFileMonitor_FILENAME_AND_SIZE)
                                    lstwFolders.Items(lstwFolders.Items.Count - 1).SubItems.Add(strProp)
                                    m_objFileMonitor.GetCurrentFile(strProp, CFileMonitor_FILEINFO_DETAILED)
                                    lstwFolders.Items(lstwFolders.Items.Count - 1).SubItems.Add(strProp)
                                    lstwFolders.Items(lstwFolders.Items.Count - 1).SubItems.Add(Str(lngSze))
                                Else
                                    strProp = "UnKnown File Size!"
                                    lstwFolders.Items(lstwFolders.Items.Count - 1).SubItems.Add(strProp)
                                    lstwFolders.Items(lstwFolders.Items.Count - 1).SubItems.Add(strProp)
                                    lstwFolders.Items(lstwFolders.Items.Count - 1).SubItems.Add(Str(0))
                                End If
                                e.Node.Expand()
                            End If
                        Catch Ex As Exception
                            Exit Do
                        End Try
                        With frmResponse
                            .lblRespondCaption.Text = "Collecting information..." & Str(Math.Round((intCountedFiles / intNumFiles) * 100, 0)) & "%"
                            .lblProgressBar.Width = ((frmResponse.lblProgressBarBase.Width - 4) / intNumFiles) * intCountedFiles
                            .lblProgressBar.Refresh()
                            .Refresh()
                        End With
                    Loop While (m_objFileMonitor.GetNextFile(FileOrFolder, CFileMonitor_FILENAME_ONLY) = True)
                    frmResponse.Close()
                    frmResponse = Nothing
                    lstwFolders.Refresh()
                    .Refresh()
                Catch err As Exception
                    'e.Node.Nodes.Clear()
                    MessageBox.Show(err.Message().ToString, "Folder Browser Error", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1)
                    frmResponse.Close()
                    frmResponse = Nothing
                    lstwFolders.Refresh()
                    .Refresh()
                End Try
            End With
        Else
            Dim FolderNode As Treenode
            txtSelectedFolder.Text = GetPath(e.Node)
            txtSelectedFolder.Tag = e
            lstwFolders.Clear()
            lstwFolders.BeginUpdate()
            For Each FolderNode In e.Node.Nodes
                lstwFolders.Items.Add(FolderNode.Text, FolderNode.ImageIndex)
                lstwFolders.Items(lstwFolders.Items.Count - 1).Tag = FolderNode
            Next FolderNode
            e.Node.Expand()
            lstwFolders.EndUpdate()
        End If
        If (e.Node.Parent Is Nothing) Then
            tlbarbtnGoFolderUp.Enabled = False
        Else
            tlbarbtnGoFolderUp.Enabled = True
        End If
        Call cntxtmnuServerSideFileBrowser_Popup(sender, New EventArgs())
    End Sub



    Private Sub lstwFolders_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstwFolders.DoubleClick
        If (lstwFolders.SelectedItems.Count <= 0) Then Exit Sub
        trewFolders_AfterSelect(sender, New TreeViewEventArgs(CType(lstwFolders.SelectedItems(0).Tag, TreeNode)))
    End Sub


    Private Function CreateDir(ByVal Filename As String) As Boolean
        Dim path As String = txtSelectedFolder.Text
        Try
            If (Not path.Substring(path.Length - 1, 1).Equals("\")) Then
                path += "\"
            End If
            path += IIf(Filename.Trim.Equals(IDS_NULL_STRING), "NonNamedFolder", Filename)
        Catch Ex As Exception
            Return False
        End Try
        If Not m_CmdMonitorClient.SetCommand(COMMAND_MONITOR_CREATE_FOLDER, path) Then Return False
        If Not m_WinMessenger.ExecuteCommandMonitorClient(m_CmdMonitorClient) Then Return False
        If Not Uint32Equals(m_CmdMonitorClient.GetResultantType(), COMMAND_MONITOR_CREATE_FOLDER_OK) Then Return False
        Return True
    End Function

    Private Sub lstwFolders_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lstwFolders.MouseDown
        cntxtmnuServerSideFileBrowser_Popup(sender, e)
    End Sub

    Private Sub lstwFolders_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstwFolders.Click
        cntxtmnuServerSideFileBrowser_Popup(sender, e)
    End Sub

    Private Sub lstwFolders_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstwFolders.SelectedIndexChanged
        cntxtmnuServerSideFileBrowser_Popup(sender, e)
    End Sub


    Private Sub tlbarFolders_ButtonClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles tlbarFolders.ButtonClick

        If (e.Button Is tlbarbtnNewFolder) Then
            Call cntxtmnuServerSideFileBrowserNewFolder_Click(sender, New EventArgs())
        ElseIf (e.Button Is tlbarbtnDeleteFolder) Then
            Call cntxtmnuServerSideFileBrowserDelete_Click(sender, New EventArgs())
        ElseIf (e.Button Is tlbarbtnGoFolderUp) Then
            trewFolders_AfterSelect(sender, New TreeViewEventArgs(CType(txtSelectedFolder.Tag, TreeViewEventArgs).Node.Parent))
        End If
    End Sub


    Private Sub lstwFolders_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles lstwFolders.KeyDown
        If (lstwFolders.SelectedItems.Count <= 0) Then Exit Sub
        If (e.KeyCode = Keys.Delete) Then
            cntxtmnuServerSideFileBrowserDelete_Click(sender, New EventArgs())
        ElseIf (e.KeyCode = Keys.Enter) Then
            lstwFolders_DoubleClick(sender, New EventArgs())
        End If
    End Sub

    Public Sub Initialize(ByRef TcpConnection As CTcpNetworkMonitorConnection, ByRef WinMsgr As CWinMonitorMessenger, ByRef CmdMonitor As CCommandMonitorClient, ByRef StrUty As CStringUtility, ByRef ImgList As ImageList, ByRef LstView As ListView, ByVal ServerIp As String, ByVal ServerPwd As String, ByVal ServerListenPort As Integer)
        m_ServerIp = ServerIp
        m_ServerPwd = ServerPwd
        m_ServerListenPort = ServerListenPort

        m_ServerConnection = TcpConnection
        m_WinMessenger = WinMsgr
        m_CmdMonitorClient = CmdMonitor
        m_strUty = StrUty

        m_ImgList = New CExtendedImageList(ImgList)

        trewFolders.ImageList = ImgList
        lstwFolders.LargeImageList = ImgList
        trewFolders.BeginUpdate()
        trewFolders.Nodes.Add(New TreeNode("My Computer", 0, 0))
        trewFolders.Nodes(0).Tag = CType("ROOT-NODES", String)

        Dim LstItem As ListViewItem
        Dim NodeIndex As Integer
        Dim Drive As String

        For Each LstItem In LstView.Items
            With trewFolders
                Try
                    Drive = LstItem.Text
                    NodeIndex = .Nodes(0).Nodes.Add(New TreeNode(Drive, LstItem.ImageIndex, LstItem.ImageIndex))
                    .Nodes(0).LastNode.Tag = CType("ROOT-NODES", String)
                Catch err As Exception
                    MessageBox.Show(err.Message().ToString, "Folder Browser Error", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1)
                    Exit Sub
                End Try
            End With
        Next LstItem

        trewFolders.EndUpdate()

        tlbarbtnGoFolderUp.Enabled = False
        Call cntxtmnuServerSideFileBrowser_Popup(New Object(), New EventArgs())
    End Sub


    Private Sub FileDownLoadingCancel(ByRef Frm As FrmFileDownLoadResponse)
        m_blnContinueDownLoad = False
        m_blnRefresh = False
        If Not (m_Thread Is Nothing) Then If (m_Thread.IsAlive()) Then m_Thread.Join()
        If ((Not (Frm Is Nothing)) And Frm.Visible) Then Frm.Close()
        Frm = Nothing
    End Sub

    Private Sub cntxtmnuServerSideFileBrowser_Popup(ByVal sender As Object, ByVal e As System.EventArgs) Handles cntxtmnuServerSideFileBrowser.Popup
        If (txtSelectedFolder.Text.Trim().Equals("") Or txtFolderName.Visible) Then
            cntxtmnuServerSideFileBrowserDownload.Enabled = False
            cntxtmnuServerSideFileBrowserExecute.Enabled = False
            cntxtmnuServerSideFileBrowserRefresh.Enabled = False
            cntxtmnuServerSideFileBrowserProperties.Enabled = False
            cntxtmnuServerSideFileBrowserDelete.Enabled = False
            cntxtmnuServerSideFileBrowserNewFolder.Enabled = False
            cntxtmnuServerSideFileBrowserUpload.Enabled = False
            cntxtmnuServerSideFileBrowserUploadFolder.Enabled = False
            tlbarbtnDeleteFolder.Enabled = False
            tlbarbtnNewFolder.Enabled = False
            Return
        Else
            cntxtmnuServerSideFileBrowserUpload.Enabled = True
            cntxtmnuServerSideFileBrowserUploadFolder.Enabled = True
            cntxtmnuServerSideFileBrowserNewFolder.Enabled = True
            tlbarbtnNewFolder.Enabled = True
            cntxtmnuServerSideFileBrowserRefresh.Enabled = True
        End If

        If (lstwFolders.SelectedItems.Count <= 0) Then
            cntxtmnuServerSideFileBrowserDelete.Enabled = False
            tlbarbtnDeleteFolder.Enabled = False
            cntxtmnuServerSideFileBrowserDownload.Enabled = False
            cntxtmnuServerSideFileBrowserExecute.Enabled = False
            cntxtmnuServerSideFileBrowserProperties.Enabled = False
        Else
            If (lstwFolders.SelectedItems.Count = 1) Then
                If (CType(txtSelectedFolder.Tag, TreeViewEventArgs).Node.Parent Is Nothing) Then
                    cntxtmnuServerSideFileBrowserProperties.Enabled = False
                Else
                    cntxtmnuServerSideFileBrowserProperties.Enabled = True
                End If
                'If lstwFolders.SelectedItems(0).SubItems(0).Text.Trim.ToUpper.EndsWith(".EXE") Then
                cntxtmnuServerSideFileBrowserExecute.Enabled = True
                'Else
                'cntxtmnuServerSideFileBrowserExecute.Enabled = False
                'End If
            Else
                cntxtmnuServerSideFileBrowserProperties.Enabled = False
            End If
            cntxtmnuServerSideFileBrowserDelete.Enabled = True
            tlbarbtnDeleteFolder.Enabled = True
            cntxtmnuServerSideFileBrowserDownload.Enabled = True
        End If
    End Sub

    Private Sub cntxtmnuServerSideFileBrowserDownload_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cntxtmnuServerSideFileBrowserDownload.Click
        Dim strTarDir As String
        If (m_strUty.BrowseFolder("Select destination directory", strTarDir, Me.Handle) = False) Then
            MessageBox.Show(IDS_FILEORPATH_INVALID, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
            Return
        ElseIf (System.IO.Directory.Exists(strTarDir) = False) Then
            MessageBox.Show(IDS_FILEORPATH_INVALID, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
            Return
        End If

        m_ThreadDownLoad = New System.Threading.Thread(AddressOf StartCopyOperation)
        m_blnDownLoadError = False
        m_strTargetDir = strTarDir
        m_ThreadDownLoad.Start()
        m_ThreadDownLoad.Join()
        m_ThreadDownLoad = Nothing

        m_blnDownLoadError = False
        If m_blnDownLoadError Then
            MessageBox.Show(IDS_FILE_COPY_NOTOK, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
            Return
        Else
            MessageBox.Show(IDS_FILE_COPY_OK, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
            Return
        End If

    End Sub

    Private Sub StartCopyOperation()
        Dim lstwItem As ListViewItem
        Dim FrmResponse As New FrmFileDownLoadResponse()

        AddHandler FrmResponse.EventUserCancelled, AddressOf FileDownLoadingCancel

        m_frmResponse = FrmResponse
        m_Thread = Nothing
        m_blnContinueDownLoad = True

        Dim intCount As Integer = 0
        For Each lstwItem In Me.lstwFolders.SelectedItems
            If (m_blnContinueDownLoad = False) Then
                m_blnDownLoadError = True
                Exit For
            End If
            intCount += 1
            With FrmResponse
                .lblRespondCaption.Text = "Downloading... " & Me.txtSelectedFolder.Text & lstwItem.Text & IDS_SPACE & Str(Math.Round((intCount / lstwFolders.SelectedItems.Count) * 100, 0)) & "%"
                .lblProgressBar.Width = ((FrmResponse.lblProgressBarBase.Width - 4) / lstwFolders.SelectedItems.Count) * intCount
                .lblProgressBar.Refresh()
            End With
            If (CopyAllFiles(Me.txtSelectedFolder.Text, Me.txtSelectedFolder.Text, lstwItem.Text & IIf(CType(CType(lstwItem.Tag, TreeNode).Tag, String).Trim().Equals("0"), "1", "0"), m_strTargetDir & IDS_FILE_SLASH, 1) = False) Then
                m_blnDownLoadError = True
            End If
        Next lstwItem

        Call FileDownLoadingCancel(FrmResponse) : m_frmResponse = Nothing

    End Sub

    Private Function CopyAllFiles(ByVal SrcRootDir As String, ByVal SrcDir As String, ByVal FileName As String, ByVal DstDir As String, ByVal DepthOfRecurssion As Long) As Boolean

        If m_blnContinueDownLoad = False Then Return False

        Dim FrmResponse As FrmFileDownLoadResponse = CType(m_frmResponse, FrmFileDownLoadResponse)

        Call InitializeDownLoadResponser(True, FrmResponse)

        If (FileName.EndsWith("0")) Then
            FileName = FileName.Substring(0, FileName.Length - 1)
            Dim Path As String = DstDir & SrcDir.Substring(SrcRootDir.Length())
            If m_blnContinueDownLoad = False Then Return False
            If (m_CmdMonitorClient.SetCommand(COMMAND_MONITOR_DOWNLOAD_FILE, SrcDir & FileName) = True) Then
                If (m_WinMessenger.ExecuteCommandMonitorClient(m_CmdMonitorClient) = True) Then
                    If (Uint32Equals(m_CmdMonitorClient.GetResultantType(), COMMAND_MONITOR_DOWNLOADED_FILE)) Then
                        If m_blnContinueDownLoad = False Then Return False
                        Call StopInitializeDownLoadResponser(FrmResponse)
                        FrmResponse.lblProgressBarSub.Width = 0
                        FrmResponse.statBarPanelStatus.Text = "Copying..." & SrcDir & FileName & " 0%"
                        FrmResponse.Refresh()
                        If (m_CmdMonitorClient.SaveRemoteFile(FileName, Path) = True) Then
                            FrmResponse.lblProgressBarSub.Width = (FrmResponse.lblProgressBarBaseSub.Width - 4)
                            FrmResponse.statBarPanelStatus.Text = "Copying..." & SrcDir & FileName & " 100%"
                            FrmResponse.Refresh()
                            Return True
                        Else
                            Return False
                        End If
                    Else
                        Return False
                    End If
                Else
                    Return False
                End If
            Else
                Return False
            End If
        Else
            FileName = FileName.Substring(0, FileName.Length - 1)
            If (m_CmdMonitorClient.SetCommand(COMMAND_MONITOR_GET_SUB_DIR_NAMES, SrcDir & FileName & "\*") = True) Then
                If m_blnContinueDownLoad = False Then Return False
                If (m_WinMessenger.ExecuteCommandMonitorClient(m_CmdMonitorClient) = True) Then
                    If m_blnContinueDownLoad = False Then Return False
                    If (Uint32Equals(m_CmdMonitorClient.GetResultantType(), COMMAND_MONITOR_GOT_SUB_DIR_NAMES) = True) Then
                        Dim strMaySubDir As String
                        Dim strDirArray() As String = m_strUty.ParseString(m_CmdMonitorClient.GetResult(), Chr(10))
                        Dim intTotFiles As Long = strDirArray.Length(), lngNumFilesCopied As Long = 0
                        Call StopInitializeDownLoadResponser(FrmResponse)
                        If (DepthOfRecurssion = 1) Then
                            FrmResponse.lblProgressBar.Left = (FrmResponse.lblProgressBarBase.Left + 2)
                            FrmResponse.lblProgressBar.Width = 0
                        Else
                            FrmResponse.lblProgressBarSub.Left = (FrmResponse.lblProgressBarBaseSub.Left + 2)
                            FrmResponse.lblProgressBarSub.Width = 0
                        End If
                        FrmResponse.Refresh()
                        Dim blnOk As Boolean = True
                        Dim strRealName As String
                        For Each strMaySubDir In strDirArray
                            If m_blnContinueDownLoad = False Then Return False
                            lngNumFilesCopied += 1 : strMaySubDir = strMaySubDir.Trim
                            strRealName = strMaySubDir.Substring(0, strMaySubDir.Length - 1)
                            If Not (strRealName = IDS_DOT Or strRealName = ".." Or strRealName.Length >= 50) Then
                                If (DepthOfRecurssion = 1) Then
                                    With FrmResponse
                                        .lblRespondCaption.Text = "Downloading... " & SrcDir & FileName & IDS_SPACE & Str(Math.Round((lngNumFilesCopied / intTotFiles) * 100, 0)) & "%"
                                        .lblProgressBar.Width = ((FrmResponse.lblProgressBarBase.Width - 4) / intTotFiles) * lngNumFilesCopied
                                        .lblProgressBar.Refresh()
                                    End With
                                Else
                                    With FrmResponse
                                        .lblProgressBarSub.Width = ((FrmResponse.lblProgressBarBaseSub.Width - 4) / intTotFiles) * lngNumFilesCopied
                                        .lblProgressBarSub.Refresh()
                                    End With
                                End If
                                FrmResponse.statBarPanelStatus.Text = "Downloading..." & SrcDir & FileName & IDS_FILE_SLASH & strRealName
                                FrmResponse.Refresh()
                                If Not (CopyAllFiles(SrcRootDir, SrcDir & FileName & IDS_FILE_SLASH, strMaySubDir, DstDir, DepthOfRecurssion + 1) = True) Then
                                    blnOk = False
                                End If
                            End If
                        Next strMaySubDir
                        Return blnOk
                    Else
                        Return False
                    End If
                Else
                    Return False
                End If
            Else
                Return False
            End If
        End If

    End Function

    Private Sub InitializeDownLoadResponser(ByVal IsDownLoad As Boolean, ByRef Frm As FrmFileDownLoadResponse)
        If IsDownLoad Then
            Frm.statBarPanelStatus.Text = "waiting for remote WinMonitorServer to respond..."
            m_blnRefresh = True
            Frm.Visible = True
            Call StopInitializeDownLoadResponser(Frm)
            m_Thread = New Threading.Thread(AddressOf Responser)
            m_Thread.Start()
        End If
    End Sub

    Private Sub StopInitializeDownLoadResponser(ByRef Frm As FrmFileDownLoadResponse)
        m_blnRefresh = False
        Try
            If Not (m_Thread Is Nothing) Then If (Not m_Thread.IsAlive()) Then m_Thread.Join()
        Catch Ex As Exception : End Try
    End Sub

    Private Sub cntxtmnuServerSideFileBrowserProperties_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cntxtmnuServerSideFileBrowserProperties.Click
        Try
            MessageBox.Show(lstwFolders.SelectedItems(0).SubItems(2).Text, lstwFolders.SelectedItems(0).Text, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
        Catch Ex As Exception : End Try
    End Sub

    Private Sub cntxtmnuServerSideFileBrowserExecute_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cntxtmnuServerSideFileBrowserExecute.Click
        Dim strCmdLine As String = InputBox("Enter command line, if any...", IDS_NULL_STRING)
        If (m_CmdMonitorClient.SetCommand(COMMAND_MONITOR_EXEC_PROGRAM, _
        txtSelectedFolder.Text & lstwFolders.SelectedItems(0).SubItems(0).Text.Trim & Chr(10) & strCmdLine) = False) Then

            m_strUty.MsgErr("Setting command failed", "Command failed...")
            Return
        End If
        If (m_WinMessenger.ExecuteCommandMonitorClient(m_CmdMonitorClient) = False) Then
            m_strUty.MsgErr("Execution of command failed", "Command failed...")
            Return
        End If
        If Uint32Equals(m_CmdMonitorClient.GetResultantType(), COMMAND_MONITOR_EXECD_PROGRAM) = False Then
            m_strUty.MsgErr("Execution of command failed", "Command failed...")
            Return
        End If
        MessageBox.Show("Program " & lstwFolders.SelectedItems(0).SubItems(0).Text.Trim & IDS_SPACE & "properly executing on server ...", "Command succeeded...", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
    End Sub

    Private Sub cntxtmnuServerSideFileBrowserRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cntxtmnuServerSideFileBrowserRefresh.Click
        Dim TreEvntArgs As TreeViewEventArgs = CType(txtSelectedFolder.Tag, TreeViewEventArgs)
        If Not (TreEvntArgs.Node Is Nothing) Then
            TreEvntArgs.Node.Nodes.Clear()
            Call trewFolders_AfterSelect(sender, TreEvntArgs)
        End If
    End Sub

    Private Sub cntxtmnuServerSideFileBrowserDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cntxtmnuServerSideFileBrowserDelete.Click
        If (MessageBox.Show("Are You Sure To Delete?", "Confirm Folder Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = DialogResult.No) Then
            Exit Sub
        End If
        Dim strFileName As String
        Dim intSelectedIndx As Integer
        Dim frmResp As New FrmResponse()
        Dim lngMax As Long = frmResp.lblProgressBarBase.Width
        Dim lngCurrent As Long = 0
        Dim lngMaxCnt As Long = lstwFolders.SelectedItems.Count
        frmResp.lblProgressBar.Left = frmResp.lblProgressBarBase.Left
        frmResp.lblRespondCaption.Text = "Deleting...0%"
        frmResp.Visible = True
        Try
            For intSelectedIndx = 0 To lstwFolders.SelectedItems.Count - 1 Step 1
                lngCurrent = (intSelectedIndx / lngMaxCnt)
                frmResp.lblProgressBar.Width = lngCurrent * lngMax
                lngCurrent *= 100
                frmResp.lblRespondCaption.Text = "Deleting " & GetPath(CType(lstwFolders.SelectedItems(intSelectedIndx).Tag, TreeNode)) & "..." & Str(lngCurrent) & "%"
                If m_CmdMonitorClient.SetCommand(COMMAND_MONITOR_DELETE_FILE, GetPath(CType(lstwFolders.SelectedItems(intSelectedIndx).Tag, TreeNode))) Then
                    If m_WinMessenger.ExecuteCommandMonitorClient(m_CmdMonitorClient) Then
                        If Not Uint32Equals(COMMAND_MONITOR_DELETE_FILE_OK, m_CmdMonitorClient.GetResultantType()) Then
                            frmResp.lblRespondCaption.Text = "Deleted " & GetPath(CType(lstwFolders.SelectedItems(intSelectedIndx).Tag, TreeNode)) & "..." & Str(lngCurrent) & "%"
                        End If
                    End If
                End If
            Next intSelectedIndx
        Catch err As Exception
            MessageBox.Show(err.Message, "Deletion of failed...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        Call cntxtmnuServerSideFileBrowserRefresh_Click(sender, New System.EventArgs())

        If Not frmResp Is Nothing Then
            If frmResp.Visible Then
                frmResp.Close()
                frmResp = Nothing
            End If
        End If
    End Sub

    Private Sub cntxtmnuServerSideFileBrowserNewFolder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cntxtmnuServerSideFileBrowserNewFolder.Click
        Dim j As Rectangle
        lstwFolders.Items.Add("NewFolder_Unnamed", 6)
        j = lstwFolders.Items(lstwFolders.Items.Count - 1).Bounds()
        txtFolderName.Location = New Point(j.Left + lstwFolders.Left + 6, j.Top + lstwFolders.Top + j.Height - 12)
        txtFolderName.Text() = "NewFolder_Unnamed"
        txtFolderName.Visible = True
        m_blnCreateFolderFlg = False
        txtFolderName.Focus()
        SendKeys.Send("{HOME}+{END}")
    End Sub

    Private Sub txtFolderName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtFolderName.KeyDown
        If (e.KeyCode = 13) And (Not m_blnCreateFolderFlg) Then
            Try
                If Not CreateDir(txtFolderName.Text) Then Throw New Exception("Directory creation failed...")
                txtFolderName.Visible = False
                CType(txtSelectedFolder.Tag, TreeViewEventArgs).Node.Nodes.Clear()
                trewFolders_AfterSelect(sender, CType(txtSelectedFolder.Tag, TreeViewEventArgs))
            Catch err As Exception
                MessageBox.Show(err.Message, "Creation of Directory failed!", MessageBoxButtons.OK, MessageBoxIcon.Error)
                txtFolderName.Visible = False
                lstwFolders.Items.RemoveAt(lstwFolders.Items.Count - 1)
                lstwFolders.Update()
                'cntxtmnuLstwFolderFolderNew_Click(sender, New EventArgs())
                Return
            End Try
            m_blnCreateFolderFlg = True
        End If
    End Sub


    Private Sub txtFolderName_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtFolderName.Leave
        Call txtFolderName_KeyDown(sender, New KeyEventArgs(13))
    End Sub

    Private Sub cntxtmnuServerSideFileBrowserUpload_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cntxtmnuServerSideFileBrowserUpload.Click
        opnfiledlgForUploading.CheckFileExists = True
        opnfiledlgForUploading.CheckPathExists = True
        opnfiledlgForUploading.Multiselect = True
        opnfiledlgForUploading.Title = "Select Folders and Files to upload..."
        opnfiledlgForUploading.Filter = "All files (*.*)|*.*"
        If opnfiledlgForUploading.ShowDialog = DialogResult.Cancel Or opnfiledlgForUploading.FileName = IDS_NULL_STRING Then
            Exit Sub
        End If
        m_blnUpLoadingFolder = False
        m_UploadThread = New System.Threading.Thread(AddressOf UploadSelectedFilesThreadProc)
        m_UploadThread.Start()
        Return
    End Sub

    Private Sub cntxtmnuServerSideFileBrowserUploadFolder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cntxtmnuServerSideFileBrowserUploadFolder.Click
        Dim FolderName As String
        Dim Path As String
        Dim File As String

        If Not (m_UploadThread Is Nothing) Then
            If MessageBox.Show("Previous uploading not completed..." & vbCrLf & "Abort previous uploading...?", "Confirm previous upload cancel...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = DialogResult.No Then
                Return
            Else
                Call UpLoadCancelEvent(CType(Me.Tag, FrmFileDownLoadResponse))
                Me.Tag = Nothing
            End If
        End If

        If (m_strUty.BrowseFolder("Select folder to upload...", FolderName, Me.Handle) = False) Or FolderName = IDS_NULL_STRING Then
            Return
        End If
        If FolderName.IndexOf(IDS_FILE_SLASH) = -1 Or FolderName.Substring(FolderName.Length() - 1).Equals(":") Or FolderName.Substring(FolderName.Length() - 2, 1).Equals(":") Then
            m_strUty.MsgErr("Select a folder to upload...", "Selection is not a folder...")
            Return
        End If
        ReDim m_strFolderArray(0)
        m_strFolderArray(0) = FolderName
        m_blnUpLoadingFolder = True
        m_UploadThread = New System.Threading.Thread(AddressOf UploadSelectedFilesThreadProc)
        m_UploadThread.Start()
        Return
    End Sub

    Private Sub UploadSelectedFilesThreadProc()
        m_blnUpLoadOk = False
        Dim TcpCon As CTcpNetworkMonitorConnection = New CTcpNetworkMonitorConnection()
        If TcpCon Is Nothing Then Return
        If (TcpCon Is Nothing) Or (Not TcpCon.ConnectTo(m_ServerIp, m_ServerListenPort)) Then Return
        Dim WinMesgr As CWinMonitorMessenger = New CWinMonitorMessenger(TcpCon)
        If Not WinMesgr.SynchronizeServer() Then Return
        If Not WinMesgr.Authenticate(m_ServerPwd) Then Return

        Dim strFileName As String
        Dim blnOk As Boolean = True
        Dim Fname As String
        Dim PathName As String
        Dim strUty As CStringUtility = New CStringUtility()
        Dim CmdMntr As CCommandMonitorClient = New CCommandMonitorClient(COMMAND_MONITOR_NULL_COMMAND, "", COMMAND_MONITOR_COMPRESS_LZSS)

        Dim frmResp As New FrmFileDownLoadResponse()
        frmResp.Tag = WinMesgr
        Me.Tag = frmResp
        AddHandler frmResp.EventUserCancelled, AddressOf UpLoadCancelEvent
        frmResp.lblRespondCaption.Text = "Uploading...0%" : frmResp.lblProgressBar.Left = frmResp.lblProgressBarBase.Left
        frmResp.lblProgressBar.Top = frmResp.lblProgressBarBase.Top
        frmResp.lblProgressBar.Height = frmResp.lblProgressBarBase.Height
        frmResp.lblProgressBar.Width = frmResp.lblProgressBarBase.Width / 10
        frmResp.Show()
        Dim lngTotFiles As Long = IIf(m_blnUpLoadingFolder, m_strFolderArray, opnfiledlgForUploading.FileNames).Length
        Dim lngCurFile As Long = 0
        For Each strFileName In IIf(m_blnUpLoadingFolder, m_strFolderArray, opnfiledlgForUploading.FileNames)
            If (strUty.SplitFileNameAndPath(strFileName, PathName, Fname) = True) Then
                If (UpLoadFilesProc(PathName, Fname, txtSelectedFolder.Text.Substring(0, txtSelectedFolder.Text.Length() - 1), WinMesgr, CmdMntr, frmResp) = False) Then blnOk = False
            End If
            lngCurFile += 1
            frmResp.lblProgressBar.Width = (lngCurFile / lngTotFiles) * frmResp.lblProgressBarBase.Width
            frmResp.lblRespondCaption.Text = "Uploading..." & Str(System.Math.Round((lngCurFile / lngTotFiles) * 100, 0)).Trim() & " % " & strFileName
            frmResp.Refresh()
        Next strFileName

        m_blnUpLoadOk = blnOk
        frmResp.lblRespondCaption.Text = "Uploading...100%"
        WinMesgr.ShutDown()
        frmResp.Close()
        frmResp = Nothing
        m_UploadThread = Nothing
        'Call cntxtmnuServerSideFileBrowserRefresh_Click(New Object(), New System.EventArgs())
        Return
    End Sub

    Public Sub UpLoadCancelEvent(ByRef frmResp As FrmFileDownLoadResponse)
        If Not (m_UploadThread Is Nothing) Then
            If (m_UploadThread.IsAlive()) Then
                m_UploadThread.Abort()
            End If
        End If
        If Not frmResp Is Nothing Then
            Dim WMsgr As CWinMonitorMessenger = CType(frmResp.Tag, CWinMonitorMessenger)
            WMsgr.ShutDown()
            frmResp.Close()
            frmResp = Nothing
        End If
        m_UploadThread = Nothing
    End Sub

    Private Function UpLoadFilesProc(ByVal SrcRoot As String, ByVal FileName As String, ByRef TarRoot As String, ByRef WinMesgr As CWinMonitorMessenger, ByVal CmdMntr As CCommandMonitorClient, ByRef frmResponse As FrmFileDownLoadResponse) As Boolean
        Dim blnOk As Boolean = True
        If Directory.Exists(SrcRoot & IDS_FILE_SLASH & FileName) Then
            Dim strSubFiles As String
            Dim strSubDirs As String
            Dim strArrSubDirs() As String = Directory.GetDirectories(SrcRoot & IDS_FILE_SLASH & FileName)
            Dim strArrSubFiles() As String = Directory.GetFiles(SrcRoot & IDS_FILE_SLASH & FileName)

            Dim lngTotCount As Long = strArrSubDirs.Length() + strArrSubFiles.Length()
            Dim lngCurCount As Long = 0
            frmResponse.lblProgressBarSub.Left = frmResponse.lblProgressBarBaseSub.Left
            frmResponse.lblProgressBarSub.Height = frmResponse.lblProgressBarBaseSub.Height
            frmResponse.lblProgressBarSub.Top = frmResponse.lblProgressBarBaseSub.Top
            frmResponse.lblProgressBarSub.Width = 0

            For Each strSubDirs In strArrSubDirs
                frmResponse.statBarPanelStatus.Text = "Uploading " & strSubDirs & "... 0%"
                frmResponse.Refresh()
                If Not (UpLoadFilesProc(SrcRoot, (FileName & IDS_FILE_SLASH & strSubDirs).Substring((SrcRoot & IDS_FILE_SLASH & FileName).Length() + 1), TarRoot, WinMesgr, CmdMntr, frmResponse)) Then blnOk = False
                lngCurCount += 1
                frmResponse.lblProgressBarSub.Width = (lngCurCount / lngTotCount) * frmResponse.lblProgressBarBaseSub.Width
                frmResponse.lblRespondCaption.Text = "Uploading " & "..." & IDS_FILE_SLASH & FileName & IDS_SPACE & Str(System.Math.Round((lngCurCount / lngTotCount) * 100)).Trim() & "%"
                frmResponse.statBarPanelStatus.Text = "Uploading " & strSubDirs & "... 100%"
                frmResponse.Refresh()
            Next strSubDirs

            For Each strSubFiles In strArrSubFiles
                frmResponse.statBarPanelStatus.Text = "Uploading " & strSubFiles & "... 0%"
                frmResponse.Refresh()
                If CmdMntr.SetFileUpLoad(SrcRoot, (FileName & IDS_FILE_SLASH & strSubFiles).Substring((SrcRoot & IDS_FILE_SLASH & FileName).Length() + 1), TarRoot) Then
                    If WinMesgr.ExecuteCommandMonitorClient(CmdMntr) Then
                        If Not Uint32Equals(CmdMntr.GetResultantType(), COMMAND_MONITOR_FILE_UPLOADED) Then
                            blnOk = False
                        End If
                    Else
                        blnOk = False
                    End If
                Else
                    blnOk = False
                End If
                lngCurCount += 1
                frmResponse.lblProgressBarSub.Width = (lngCurCount / lngTotCount) * frmResponse.lblProgressBarBaseSub.Width
                frmResponse.lblRespondCaption.Text = "Uploading " & "..." & IDS_FILE_SLASH & FileName & IDS_SPACE & Str(System.Math.Round((lngCurCount / lngTotCount) * 100)).Trim() & "%"
                frmResponse.statBarPanelStatus.Text = "Uploading " & strSubFiles & "... 100%"
                frmResponse.Refresh()
            Next strSubFiles
            Return blnOk
        Else
            frmResponse.lblProgressBarSub.Left = frmResponse.lblProgressBarBaseSub.Left
            frmResponse.lblProgressBarSub.Width = 0
            frmResponse.statBarPanelStatus.Text = "Uploading " & FileName & "... 0%"
            frmResponse.lblRespondCaption.Text = "Uploading " & "..." & IDS_FILE_SLASH & FileName & IDS_SPACE & "0%"
            frmResponse.Refresh()
            If CmdMntr.SetFileUpLoad(SrcRoot, FileName, TarRoot) Then
                If WinMesgr.ExecuteCommandMonitorClient(CmdMntr) Then
                    If Not Uint32Equals(CmdMntr.GetResultantType(), COMMAND_MONITOR_FILE_UPLOADED) Then
                        blnOk = False
                    End If
                Else
                    blnOk = False
                End If
            Else
                blnOk = False
            End If
            frmResponse.statBarPanelStatus.Text = "Uploading " & FileName & "... 100%"
            frmResponse.lblRespondCaption.Text = "Uploading " & "..." & IDS_FILE_SLASH & FileName & IDS_SPACE & "100%"
            frmResponse.lblProgressBarSub.Width = frmResponse.lblProgressBarBaseSub.Width
            frmResponse.Refresh()
            Return blnOk
        End If
    End Function

End Class
