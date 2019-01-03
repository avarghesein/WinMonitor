
Imports WinMonitorBLIB_1_0

Public Class frmDiskProperties : Inherits System.Windows.Forms.Form

    Private m_strDiskProperties As String

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
    Friend WithEvents tabcntrlDiskProperties As System.Windows.Forms.TabControl
    Friend WithEvents tabcntrlDiskPropertiesMainMemory As System.Windows.Forms.TabPage
    Friend WithEvents tabcntrlDiskPropertiesDiskProperties As System.Windows.Forms.TabPage
    Friend WithEvents picMainMemoryIcon As System.Windows.Forms.PictureBox
    Friend WithEvents lblLine1 As System.Windows.Forms.Label
    Friend WithEvents picMainMemoryUsage As System.Windows.Forms.PictureBox
    Friend WithEvents lblCapacity As System.Windows.Forms.Label
    Friend WithEvents lblFree As System.Windows.Forms.Label
    Friend WithEvents lblCapacityTxt As System.Windows.Forms.Label
    Friend WithEvents lblFreeTxt As System.Windows.Forms.Label
    Friend WithEvents lblLine2 As System.Windows.Forms.Label
    Friend WithEvents btnServerPasswordOk As System.Windows.Forms.Button
    Friend WithEvents lblFreeSpaceColor As System.Windows.Forms.Label
    Friend WithEvents lblTotalSpaceColor As System.Windows.Forms.Label
    Friend WithEvents lblUsedTxt As System.Windows.Forms.Label
    Friend WithEvents lblUsed As System.Windows.Forms.Label
    Friend WithEvents lblUsedSpaceColor As System.Windows.Forms.Label
    Friend WithEvents txtMainMemory As System.Windows.Forms.TextBox
    Friend WithEvents lblStorageUsed As System.Windows.Forms.Label
    Friend WithEvents lblStorageUsedSpaceColor As System.Windows.Forms.Label
    Friend WithEvents txtStorageDisk As System.Windows.Forms.TextBox
    Friend WithEvents lblLine4 As System.Windows.Forms.Label
    Friend WithEvents lblStorageCapacityTxt As System.Windows.Forms.Label
    Friend WithEvents lblStorageFree As System.Windows.Forms.Label
    Friend WithEvents lbStoragelCapacity As System.Windows.Forms.Label
    Friend WithEvents lbStoragelFreeSpaceColor As System.Windows.Forms.Label
    Friend WithEvents lblTotalStorageSpaceColor As System.Windows.Forms.Label
    Friend WithEvents lblLine3 As System.Windows.Forms.Label
    Friend WithEvents picDriveIcon As System.Windows.Forms.PictureBox
    Friend WithEvents PicStorageDiskUsage As System.Windows.Forms.PictureBox
    Friend WithEvents tabcntrlDiskPropertiesDiskPropertiesExtra As System.Windows.Forms.TabPage
    Friend WithEvents txtStorageDiskExtra As System.Windows.Forms.TextBox
    Friend WithEvents lblLine5 As System.Windows.Forms.Label
    Friend WithEvents picDriveExtraIcon As System.Windows.Forms.PictureBox
    Friend WithEvents txtStorageDiskExtraInfo As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtStorageDiskExtraType As System.Windows.Forms.TextBox
    Friend WithEvents lblStorageUsedTxt As System.Windows.Forms.Label
    Friend WithEvents lblStorageFreeTxt As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmDiskProperties))
        Me.tabcntrlDiskProperties = New System.Windows.Forms.TabControl()
        Me.tabcntrlDiskPropertiesMainMemory = New System.Windows.Forms.TabPage()
        Me.lblUsedTxt = New System.Windows.Forms.Label()
        Me.lblUsed = New System.Windows.Forms.Label()
        Me.lblUsedSpaceColor = New System.Windows.Forms.Label()
        Me.txtMainMemory = New System.Windows.Forms.TextBox()
        Me.picMainMemoryUsage = New System.Windows.Forms.PictureBox()
        Me.lblLine2 = New System.Windows.Forms.Label()
        Me.lblFreeTxt = New System.Windows.Forms.Label()
        Me.lblCapacityTxt = New System.Windows.Forms.Label()
        Me.lblFree = New System.Windows.Forms.Label()
        Me.lblCapacity = New System.Windows.Forms.Label()
        Me.lblFreeSpaceColor = New System.Windows.Forms.Label()
        Me.lblTotalSpaceColor = New System.Windows.Forms.Label()
        Me.lblLine1 = New System.Windows.Forms.Label()
        Me.picMainMemoryIcon = New System.Windows.Forms.PictureBox()
        Me.tabcntrlDiskPropertiesDiskProperties = New System.Windows.Forms.TabPage()
        Me.lblStorageUsedTxt = New System.Windows.Forms.Label()
        Me.lblStorageUsed = New System.Windows.Forms.Label()
        Me.lblStorageUsedSpaceColor = New System.Windows.Forms.Label()
        Me.txtStorageDisk = New System.Windows.Forms.TextBox()
        Me.PicStorageDiskUsage = New System.Windows.Forms.PictureBox()
        Me.lblLine4 = New System.Windows.Forms.Label()
        Me.lblStorageFreeTxt = New System.Windows.Forms.Label()
        Me.lblStorageCapacityTxt = New System.Windows.Forms.Label()
        Me.lblStorageFree = New System.Windows.Forms.Label()
        Me.lbStoragelCapacity = New System.Windows.Forms.Label()
        Me.lbStoragelFreeSpaceColor = New System.Windows.Forms.Label()
        Me.lblTotalStorageSpaceColor = New System.Windows.Forms.Label()
        Me.lblLine3 = New System.Windows.Forms.Label()
        Me.picDriveIcon = New System.Windows.Forms.PictureBox()
        Me.tabcntrlDiskPropertiesDiskPropertiesExtra = New System.Windows.Forms.TabPage()
        Me.txtStorageDiskExtraType = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtStorageDiskExtraInfo = New System.Windows.Forms.TextBox()
        Me.txtStorageDiskExtra = New System.Windows.Forms.TextBox()
        Me.lblLine5 = New System.Windows.Forms.Label()
        Me.picDriveExtraIcon = New System.Windows.Forms.PictureBox()
        Me.btnServerPasswordOk = New System.Windows.Forms.Button()
        Me.tabcntrlDiskProperties.SuspendLayout()
        Me.tabcntrlDiskPropertiesMainMemory.SuspendLayout()
        Me.tabcntrlDiskPropertiesDiskProperties.SuspendLayout()
        Me.tabcntrlDiskPropertiesDiskPropertiesExtra.SuspendLayout()
        Me.SuspendLayout()
        '
        'tabcntrlDiskProperties
        '
        Me.tabcntrlDiskProperties.Controls.AddRange(New System.Windows.Forms.Control() {Me.tabcntrlDiskPropertiesMainMemory, Me.tabcntrlDiskPropertiesDiskProperties, Me.tabcntrlDiskPropertiesDiskPropertiesExtra})
        Me.tabcntrlDiskProperties.Location = New System.Drawing.Point(11, 12)
        Me.tabcntrlDiskProperties.Name = "tabcntrlDiskProperties"
        Me.tabcntrlDiskProperties.SelectedIndex = 0
        Me.tabcntrlDiskProperties.Size = New System.Drawing.Size(378, 332)
        Me.tabcntrlDiskProperties.TabIndex = 0
        '
        'tabcntrlDiskPropertiesMainMemory
        '
        Me.tabcntrlDiskPropertiesMainMemory.Controls.AddRange(New System.Windows.Forms.Control() {Me.lblUsedTxt, Me.lblUsed, Me.lblUsedSpaceColor, Me.txtMainMemory, Me.picMainMemoryUsage, Me.lblLine2, Me.lblFreeTxt, Me.lblCapacityTxt, Me.lblFree, Me.lblCapacity, Me.lblFreeSpaceColor, Me.lblTotalSpaceColor, Me.lblLine1, Me.picMainMemoryIcon})
        Me.tabcntrlDiskPropertiesMainMemory.Location = New System.Drawing.Point(4, 22)
        Me.tabcntrlDiskPropertiesMainMemory.Name = "tabcntrlDiskPropertiesMainMemory"
        Me.tabcntrlDiskPropertiesMainMemory.Size = New System.Drawing.Size(370, 306)
        Me.tabcntrlDiskPropertiesMainMemory.TabIndex = 0
        Me.tabcntrlDiskPropertiesMainMemory.Text = "Main memory"
        '
        'lblUsedTxt
        '
        Me.lblUsedTxt.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblUsedTxt.Location = New System.Drawing.Point(134, 118)
        Me.lblUsedTxt.Name = "lblUsedTxt"
        Me.lblUsedTxt.Size = New System.Drawing.Size(218, 18)
        Me.lblUsedTxt.TabIndex = 13
        Me.lblUsedTxt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblUsed
        '
        Me.lblUsed.Location = New System.Drawing.Point(36, 118)
        Me.lblUsed.Name = "lblUsed"
        Me.lblUsed.Size = New System.Drawing.Size(72, 18)
        Me.lblUsed.TabIndex = 12
        Me.lblUsed.Text = "Used KBytes"
        Me.lblUsed.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblUsedSpaceColor
        '
        Me.lblUsedSpaceColor.BackColor = System.Drawing.Color.Purple
        Me.lblUsedSpaceColor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblUsedSpaceColor.Location = New System.Drawing.Point(14, 118)
        Me.lblUsedSpaceColor.Name = "lblUsedSpaceColor"
        Me.lblUsedSpaceColor.Size = New System.Drawing.Size(16, 18)
        Me.lblUsedSpaceColor.TabIndex = 11
        '
        'txtMainMemory
        '
        Me.txtMainMemory.Location = New System.Drawing.Point(94, 22)
        Me.txtMainMemory.Name = "txtMainMemory"
        Me.txtMainMemory.ReadOnly = True
        Me.txtMainMemory.Size = New System.Drawing.Size(258, 20)
        Me.txtMainMemory.TabIndex = 10
        Me.txtMainMemory.Text = "Main memory status"
        '
        'picMainMemoryUsage
        '
        Me.picMainMemoryUsage.BackColor = System.Drawing.SystemColors.Control
        Me.picMainMemoryUsage.Location = New System.Drawing.Point(15, 158)
        Me.picMainMemoryUsage.Name = "picMainMemoryUsage"
        Me.picMainMemoryUsage.Size = New System.Drawing.Size(340, 138)
        Me.picMainMemoryUsage.TabIndex = 9
        Me.picMainMemoryUsage.TabStop = False
        '
        'lblLine2
        '
        Me.lblLine2.BackColor = System.Drawing.Color.FromArgb(CType(224, Byte), CType(224, Byte), CType(224, Byte))
        Me.lblLine2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblLine2.Location = New System.Drawing.Point(15, 146)
        Me.lblLine2.Name = "lblLine2"
        Me.lblLine2.Size = New System.Drawing.Size(340, 3)
        Me.lblLine2.TabIndex = 8
        '
        'lblFreeTxt
        '
        Me.lblFreeTxt.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblFreeTxt.Location = New System.Drawing.Point(134, 96)
        Me.lblFreeTxt.Name = "lblFreeTxt"
        Me.lblFreeTxt.Size = New System.Drawing.Size(218, 18)
        Me.lblFreeTxt.TabIndex = 7
        Me.lblFreeTxt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblCapacityTxt
        '
        Me.lblCapacityTxt.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblCapacityTxt.Location = New System.Drawing.Point(134, 74)
        Me.lblCapacityTxt.Name = "lblCapacityTxt"
        Me.lblCapacityTxt.Size = New System.Drawing.Size(218, 18)
        Me.lblCapacityTxt.TabIndex = 6
        Me.lblCapacityTxt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblFree
        '
        Me.lblFree.Location = New System.Drawing.Point(36, 96)
        Me.lblFree.Name = "lblFree"
        Me.lblFree.Size = New System.Drawing.Size(72, 18)
        Me.lblFree.TabIndex = 5
        Me.lblFree.Text = "Free KBytes"
        Me.lblFree.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblCapacity
        '
        Me.lblCapacity.Location = New System.Drawing.Point(36, 74)
        Me.lblCapacity.Name = "lblCapacity"
        Me.lblCapacity.Size = New System.Drawing.Size(94, 18)
        Me.lblCapacity.TabIndex = 4
        Me.lblCapacity.Text = "Capcity in KBytes"
        Me.lblCapacity.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblFreeSpaceColor
        '
        Me.lblFreeSpaceColor.BackColor = System.Drawing.Color.Navy
        Me.lblFreeSpaceColor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblFreeSpaceColor.Location = New System.Drawing.Point(14, 96)
        Me.lblFreeSpaceColor.Name = "lblFreeSpaceColor"
        Me.lblFreeSpaceColor.Size = New System.Drawing.Size(16, 18)
        Me.lblFreeSpaceColor.TabIndex = 3
        '
        'lblTotalSpaceColor
        '
        Me.lblTotalSpaceColor.BackColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(192, Byte), CType(0, Byte))
        Me.lblTotalSpaceColor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblTotalSpaceColor.Location = New System.Drawing.Point(14, 74)
        Me.lblTotalSpaceColor.Name = "lblTotalSpaceColor"
        Me.lblTotalSpaceColor.Size = New System.Drawing.Size(16, 18)
        Me.lblTotalSpaceColor.TabIndex = 2
        '
        'lblLine1
        '
        Me.lblLine1.BackColor = System.Drawing.Color.FromArgb(CType(224, Byte), CType(224, Byte), CType(224, Byte))
        Me.lblLine1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblLine1.Location = New System.Drawing.Point(12, 60)
        Me.lblLine1.Name = "lblLine1"
        Me.lblLine1.Size = New System.Drawing.Size(340, 3)
        Me.lblLine1.TabIndex = 1
        '
        'picMainMemoryIcon
        '
        Me.picMainMemoryIcon.Image = CType(resources.GetObject("picMainMemoryIcon.Image"), System.Drawing.Bitmap)
        Me.picMainMemoryIcon.Location = New System.Drawing.Point(12, 12)
        Me.picMainMemoryIcon.Name = "picMainMemoryIcon"
        Me.picMainMemoryIcon.Size = New System.Drawing.Size(56, 38)
        Me.picMainMemoryIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.picMainMemoryIcon.TabIndex = 0
        Me.picMainMemoryIcon.TabStop = False
        '
        'tabcntrlDiskPropertiesDiskProperties
        '
        Me.tabcntrlDiskPropertiesDiskProperties.Controls.AddRange(New System.Windows.Forms.Control() {Me.lblStorageUsedTxt, Me.lblStorageUsed, Me.lblStorageUsedSpaceColor, Me.txtStorageDisk, Me.PicStorageDiskUsage, Me.lblLine4, Me.lblStorageFreeTxt, Me.lblStorageCapacityTxt, Me.lblStorageFree, Me.lbStoragelCapacity, Me.lbStoragelFreeSpaceColor, Me.lblTotalStorageSpaceColor, Me.lblLine3, Me.picDriveIcon})
        Me.tabcntrlDiskPropertiesDiskProperties.Location = New System.Drawing.Point(4, 22)
        Me.tabcntrlDiskPropertiesDiskProperties.Name = "tabcntrlDiskPropertiesDiskProperties"
        Me.tabcntrlDiskPropertiesDiskProperties.Size = New System.Drawing.Size(370, 306)
        Me.tabcntrlDiskPropertiesDiskProperties.TabIndex = 1
        Me.tabcntrlDiskPropertiesDiskProperties.Text = "Storage disk"
        '
        'lblStorageUsedTxt
        '
        Me.lblStorageUsedTxt.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblStorageUsedTxt.Location = New System.Drawing.Point(134, 118)
        Me.lblStorageUsedTxt.Name = "lblStorageUsedTxt"
        Me.lblStorageUsedTxt.Size = New System.Drawing.Size(218, 18)
        Me.lblStorageUsedTxt.TabIndex = 27
        Me.lblStorageUsedTxt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblStorageUsed
        '
        Me.lblStorageUsed.Location = New System.Drawing.Point(36, 118)
        Me.lblStorageUsed.Name = "lblStorageUsed"
        Me.lblStorageUsed.Size = New System.Drawing.Size(72, 18)
        Me.lblStorageUsed.TabIndex = 26
        Me.lblStorageUsed.Text = "Used MBytes"
        Me.lblStorageUsed.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblStorageUsedSpaceColor
        '
        Me.lblStorageUsedSpaceColor.BackColor = System.Drawing.Color.Purple
        Me.lblStorageUsedSpaceColor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblStorageUsedSpaceColor.Location = New System.Drawing.Point(14, 118)
        Me.lblStorageUsedSpaceColor.Name = "lblStorageUsedSpaceColor"
        Me.lblStorageUsedSpaceColor.Size = New System.Drawing.Size(16, 18)
        Me.lblStorageUsedSpaceColor.TabIndex = 25
        '
        'txtStorageDisk
        '
        Me.txtStorageDisk.Location = New System.Drawing.Point(94, 22)
        Me.txtStorageDisk.Name = "txtStorageDisk"
        Me.txtStorageDisk.ReadOnly = True
        Me.txtStorageDisk.Size = New System.Drawing.Size(258, 20)
        Me.txtStorageDisk.TabIndex = 24
        Me.txtStorageDisk.Text = ""
        '
        'PicStorageDiskUsage
        '
        Me.PicStorageDiskUsage.BackColor = System.Drawing.SystemColors.Control
        Me.PicStorageDiskUsage.Location = New System.Drawing.Point(16, 158)
        Me.PicStorageDiskUsage.Name = "PicStorageDiskUsage"
        Me.PicStorageDiskUsage.Size = New System.Drawing.Size(340, 138)
        Me.PicStorageDiskUsage.TabIndex = 23
        Me.PicStorageDiskUsage.TabStop = False
        '
        'lblLine4
        '
        Me.lblLine4.BackColor = System.Drawing.Color.FromArgb(CType(224, Byte), CType(224, Byte), CType(224, Byte))
        Me.lblLine4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblLine4.Location = New System.Drawing.Point(15, 146)
        Me.lblLine4.Name = "lblLine4"
        Me.lblLine4.Size = New System.Drawing.Size(340, 3)
        Me.lblLine4.TabIndex = 22
        '
        'lblStorageFreeTxt
        '
        Me.lblStorageFreeTxt.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblStorageFreeTxt.Location = New System.Drawing.Point(134, 96)
        Me.lblStorageFreeTxt.Name = "lblStorageFreeTxt"
        Me.lblStorageFreeTxt.Size = New System.Drawing.Size(218, 18)
        Me.lblStorageFreeTxt.TabIndex = 21
        Me.lblStorageFreeTxt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblStorageCapacityTxt
        '
        Me.lblStorageCapacityTxt.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblStorageCapacityTxt.Location = New System.Drawing.Point(134, 74)
        Me.lblStorageCapacityTxt.Name = "lblStorageCapacityTxt"
        Me.lblStorageCapacityTxt.Size = New System.Drawing.Size(218, 18)
        Me.lblStorageCapacityTxt.TabIndex = 20
        Me.lblStorageCapacityTxt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblStorageFree
        '
        Me.lblStorageFree.Location = New System.Drawing.Point(36, 96)
        Me.lblStorageFree.Name = "lblStorageFree"
        Me.lblStorageFree.Size = New System.Drawing.Size(72, 18)
        Me.lblStorageFree.TabIndex = 19
        Me.lblStorageFree.Text = "Free MBytes"
        Me.lblStorageFree.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lbStoragelCapacity
        '
        Me.lbStoragelCapacity.Location = New System.Drawing.Point(36, 74)
        Me.lbStoragelCapacity.Name = "lbStoragelCapacity"
        Me.lbStoragelCapacity.Size = New System.Drawing.Size(96, 18)
        Me.lbStoragelCapacity.TabIndex = 18
        Me.lbStoragelCapacity.Text = "Capcity in MBytes"
        Me.lbStoragelCapacity.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lbStoragelFreeSpaceColor
        '
        Me.lbStoragelFreeSpaceColor.BackColor = System.Drawing.Color.Navy
        Me.lbStoragelFreeSpaceColor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbStoragelFreeSpaceColor.Location = New System.Drawing.Point(14, 96)
        Me.lbStoragelFreeSpaceColor.Name = "lbStoragelFreeSpaceColor"
        Me.lbStoragelFreeSpaceColor.Size = New System.Drawing.Size(16, 18)
        Me.lbStoragelFreeSpaceColor.TabIndex = 17
        '
        'lblTotalStorageSpaceColor
        '
        Me.lblTotalStorageSpaceColor.BackColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(192, Byte), CType(0, Byte))
        Me.lblTotalStorageSpaceColor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblTotalStorageSpaceColor.Location = New System.Drawing.Point(14, 74)
        Me.lblTotalStorageSpaceColor.Name = "lblTotalStorageSpaceColor"
        Me.lblTotalStorageSpaceColor.Size = New System.Drawing.Size(16, 18)
        Me.lblTotalStorageSpaceColor.TabIndex = 16
        '
        'lblLine3
        '
        Me.lblLine3.BackColor = System.Drawing.Color.FromArgb(CType(224, Byte), CType(224, Byte), CType(224, Byte))
        Me.lblLine3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblLine3.Location = New System.Drawing.Point(12, 60)
        Me.lblLine3.Name = "lblLine3"
        Me.lblLine3.Size = New System.Drawing.Size(340, 3)
        Me.lblLine3.TabIndex = 15
        '
        'picDriveIcon
        '
        Me.picDriveIcon.Location = New System.Drawing.Point(12, 12)
        Me.picDriveIcon.Name = "picDriveIcon"
        Me.picDriveIcon.Size = New System.Drawing.Size(56, 38)
        Me.picDriveIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.picDriveIcon.TabIndex = 14
        Me.picDriveIcon.TabStop = False
        '
        'tabcntrlDiskPropertiesDiskPropertiesExtra
        '
        Me.tabcntrlDiskPropertiesDiskPropertiesExtra.Controls.AddRange(New System.Windows.Forms.Control() {Me.txtStorageDiskExtraType, Me.Label1, Me.txtStorageDiskExtraInfo, Me.txtStorageDiskExtra, Me.lblLine5, Me.picDriveExtraIcon})
        Me.tabcntrlDiskPropertiesDiskPropertiesExtra.Location = New System.Drawing.Point(4, 22)
        Me.tabcntrlDiskPropertiesDiskPropertiesExtra.Name = "tabcntrlDiskPropertiesDiskPropertiesExtra"
        Me.tabcntrlDiskPropertiesDiskPropertiesExtra.Size = New System.Drawing.Size(370, 306)
        Me.tabcntrlDiskPropertiesDiskPropertiesExtra.TabIndex = 2
        Me.tabcntrlDiskPropertiesDiskPropertiesExtra.Text = "Disk details"
        '
        'txtStorageDiskExtraType
        '
        Me.txtStorageDiskExtraType.Location = New System.Drawing.Point(15, 78)
        Me.txtStorageDiskExtraType.Multiline = True
        Me.txtStorageDiskExtraType.Name = "txtStorageDiskExtraType"
        Me.txtStorageDiskExtraType.ReadOnly = True
        Me.txtStorageDiskExtraType.Size = New System.Drawing.Size(340, 52)
        Me.txtStorageDiskExtraType.TabIndex = 16
        Me.txtStorageDiskExtraType.Text = ""
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.FromArgb(CType(224, Byte), CType(224, Byte), CType(224, Byte))
        Me.Label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label1.Location = New System.Drawing.Point(15, 146)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(340, 3)
        Me.Label1.TabIndex = 15
        '
        'txtStorageDiskExtraInfo
        '
        Me.txtStorageDiskExtraInfo.Location = New System.Drawing.Point(15, 158)
        Me.txtStorageDiskExtraInfo.Multiline = True
        Me.txtStorageDiskExtraInfo.Name = "txtStorageDiskExtraInfo"
        Me.txtStorageDiskExtraInfo.ReadOnly = True
        Me.txtStorageDiskExtraInfo.Size = New System.Drawing.Size(340, 138)
        Me.txtStorageDiskExtraInfo.TabIndex = 14
        Me.txtStorageDiskExtraInfo.Text = ""
        '
        'txtStorageDiskExtra
        '
        Me.txtStorageDiskExtra.Location = New System.Drawing.Point(94, 22)
        Me.txtStorageDiskExtra.Name = "txtStorageDiskExtra"
        Me.txtStorageDiskExtra.ReadOnly = True
        Me.txtStorageDiskExtra.Size = New System.Drawing.Size(258, 20)
        Me.txtStorageDiskExtra.TabIndex = 13
        Me.txtStorageDiskExtra.Text = ""
        '
        'lblLine5
        '
        Me.lblLine5.BackColor = System.Drawing.Color.FromArgb(CType(224, Byte), CType(224, Byte), CType(224, Byte))
        Me.lblLine5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblLine5.Location = New System.Drawing.Point(12, 60)
        Me.lblLine5.Name = "lblLine5"
        Me.lblLine5.Size = New System.Drawing.Size(340, 3)
        Me.lblLine5.TabIndex = 12
        '
        'picDriveExtraIcon
        '
        Me.picDriveExtraIcon.Location = New System.Drawing.Point(12, 12)
        Me.picDriveExtraIcon.Name = "picDriveExtraIcon"
        Me.picDriveExtraIcon.Size = New System.Drawing.Size(56, 38)
        Me.picDriveExtraIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.picDriveExtraIcon.TabIndex = 11
        Me.picDriveExtraIcon.TabStop = False
        '
        'btnServerPasswordOk
        '
        Me.btnServerPasswordOk.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnServerPasswordOk.Location = New System.Drawing.Point(309, 354)
        Me.btnServerPasswordOk.Name = "btnServerPasswordOk"
        Me.btnServerPasswordOk.Size = New System.Drawing.Size(80, 20)
        Me.btnServerPasswordOk.TabIndex = 12
        Me.btnServerPasswordOk.Text = "Ok"
        '
        'frmDiskProperties
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(400, 381)
        Me.Controls.AddRange(New System.Windows.Forms.Control() {Me.btnServerPasswordOk, Me.tabcntrlDiskProperties})
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "frmDiskProperties"
        Me.Text = "Properties"
        Me.tabcntrlDiskProperties.ResumeLayout(False)
        Me.tabcntrlDiskPropertiesMainMemory.ResumeLayout(False)
        Me.tabcntrlDiskPropertiesDiskProperties.ResumeLayout(False)
        Me.tabcntrlDiskPropertiesDiskPropertiesExtra.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Public WriteOnly Property DiskProperty() As String
        Set(ByVal Value As String)
            m_strDiskProperties = Value
        End Set
    End Property

    Private Sub picMainMemoryUsage_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles picMainMemoryUsage.Paint
        Call PaintPie(e.Graphics())
    End Sub

    Private Sub picMainMemoryUsage_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles picMainMemoryUsage.MouseEnter
        Dim i As System.Drawing.Graphics = picMainMemoryUsage.CreateGraphics()
        Call PaintPie(i)
    End Sub

    Private Sub frmDiskProperties_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.lblUsedTxt.Text = Trim(Str(Val(lblCapacityTxt.Text) - Val(lblFreeTxt.Text)))
        Call PrintDiskProp()
    End Sub

    Private Function PaintPie(ByRef i As System.Drawing.Graphics, Optional ByVal IsDisk As Boolean = False)

        Dim freeAng As Double

        If (IsDisk) Then
            freeAng = 360 * (CDbl(lblStorageFreeTxt.Text) / CDbl(lblStorageCapacityTxt.Text))
        Else
            freeAng = 360 * (CDbl(lblFreeTxt.Text) / CDbl(lblCapacityTxt.Text))
        End If

        Dim j As Integer
        For j = 0 To 15 Step 1
            i.DrawPie(New System.Drawing.Pen(lblFreeSpaceColor.BackColor), New Rectangle(5, 5, picMainMemoryUsage.Width - 5, picMainMemoryUsage.Height - 25 + j), 0, freeAng)
            i.DrawPie(New System.Drawing.Pen(lblUsedSpaceColor.BackColor), New Rectangle(5, 5, picMainMemoryUsage.Width - 5, picMainMemoryUsage.Height - 25 + j), freeAng, 180 - (freeAng))
        Next j
        i.DrawPie(New System.Drawing.Pen(System.Drawing.Color.Black), New Rectangle(5, 5, picMainMemoryUsage.Width - 5, picMainMemoryUsage.Height - 25 + 1), 0, freeAng)
        i.DrawPie(New System.Drawing.Pen(System.Drawing.Color.Black), New Rectangle(5, 5, picMainMemoryUsage.Width - 5, picMainMemoryUsage.Height - 25 + 1), freeAng, 180 - (freeAng))
        i.DrawPie(New System.Drawing.Pen(System.Drawing.Color.Black), New Rectangle(5, 5, picMainMemoryUsage.Width - 5, picMainMemoryUsage.Height - 25 + 15), 0, freeAng)
        i.DrawPie(New System.Drawing.Pen(System.Drawing.Color.Black), New Rectangle(5, 5, picMainMemoryUsage.Width - 5, picMainMemoryUsage.Height - 25 + 15), freeAng, 180 - (freeAng))

        i.FillPie(New System.Drawing.SolidBrush(lblFreeSpaceColor.BackColor), New Rectangle(5, 5, picMainMemoryUsage.Width - 5, picMainMemoryUsage.Height - 25), 0, freeAng)
        i.FillPie(New System.Drawing.SolidBrush(lblUsedSpaceColor.BackColor), New Rectangle(5, 5, picMainMemoryUsage.Width - 5, picMainMemoryUsage.Height - 25), freeAng, 360 - (freeAng))
    End Function

    Private Function PrintDiskProp() As Boolean
        lblStorageCapacityTxt.Text = IDS_DISK_INFO_NOT_AVAILABLE
        lblStorageFreeTxt.Text = IDS_DISK_INFO_NOT_AVAILABLE
        lblStorageUsedTxt.Text = IDS_DISK_INFO_NOT_AVAILABLE
        txtStorageDiskExtraInfo.Text = IDS_DISK_INFO_NOT_AVAILABLE
        If (m_strDiskProperties.Equals(IDS_DISK_INFO_NOT_AVAILABLE) = True) Then
            Return False
        End If
        Dim strUty As New CStringUtility()
        Try
            Dim strPropCollection() As String = strUty.ParseString(m_strDiskProperties, Chr(10))
            If (strPropCollection(1).Trim.StartsWith("Total Mega Bytes On Drive:") = True) Then
                lblStorageCapacityTxt.Text = Trim(Str(System.Math.Round(Val(strPropCollection(1).Substring(26)), 3)))
                lblStorageFreeTxt.Text = Trim(Str(System.Math.Round(Val(strPropCollection(2).Substring(22)), 3)))
                lblStorageUsedTxt.Text = Trim(Str(System.Math.Round(Val(lblStorageCapacityTxt.Text) - Val(lblStorageFreeTxt.Text), 4)))
            End If
            txtStorageDiskExtraInfo.Text = IDS_NULL_STRING
            Dim strg As String
            For Each strg In strPropCollection
                txtStorageDiskExtraInfo.Text &= (strg & vbCrLf)
            Next strg
            strPropCollection = Nothing
        Catch Ex As Exception : End Try
        strUty = Nothing
    End Function

    Private Sub btnServerPasswordOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnServerPasswordOk.Click
        Me.Close()
    End Sub


    Private Sub PicStorageDiskUsage_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles PicStorageDiskUsage.Paint
        If Not (lblStorageCapacityTxt.Text.Trim.Equals(IDS_DISK_INFO_NOT_AVAILABLE) = True) Then
            Call PaintPie(e.Graphics(), True)
        End If
    End Sub
End Class
