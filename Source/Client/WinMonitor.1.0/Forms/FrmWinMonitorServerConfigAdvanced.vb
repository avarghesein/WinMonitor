
Imports WinMonitorBLIB_1_0

Public Class FrmWinMonitorServerConfigurationsAdvanced : Inherits System.Windows.Forms.Form


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
   Friend WithEvents lblWinMonitorFileName As System.Windows.Forms.Label
   Friend WithEvents txtWinMonitorFileName As System.Windows.Forms.TextBox
   Friend WithEvents btnWinMonitorAdvancedOk As System.Windows.Forms.Button
   Friend WithEvents btnWinMonitorAdvancedCancel As System.Windows.Forms.Button
   Friend WithEvents gbxWinMonitorAdvancedOptionsInstallLocation As System.Windows.Forms.GroupBox
   Friend WithEvents rdobtnWinMonitorAdvancedOptionsInstallLocationProgramFiles As System.Windows.Forms.RadioButton
   Friend WithEvents rdobtnWinMonitorAdvancedOptionsInstallLocationSystem As System.Windows.Forms.RadioButton
   Friend WithEvents rdobtnWinMonitorAdvancedOptionsInstallLocationWindows As System.Windows.Forms.RadioButton
   Friend WithEvents Label1 As System.Windows.Forms.Label
   Friend WithEvents Panel1 As System.Windows.Forms.Panel
   Friend WithEvents lblPicture As System.Windows.Forms.Label
   Friend WithEvents rdobtnWinMonitorAdvancedOptionsIncludeConfigFile As System.Windows.Forms.RadioButton
   Friend WithEvents rdobtnWinMonitorAdvancedOptionsExcludeConfigFile As System.Windows.Forms.RadioButton
   Friend WithEvents rdobtnWinMonitorAdvancedOptionsInstallLocationCommonFiles As System.Windows.Forms.RadioButton
   <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(FrmWinMonitorServerConfigurationsAdvanced))
        Me.lblWinMonitorFileName = New System.Windows.Forms.Label()
        Me.txtWinMonitorFileName = New System.Windows.Forms.TextBox()
        Me.btnWinMonitorAdvancedOk = New System.Windows.Forms.Button()
        Me.btnWinMonitorAdvancedCancel = New System.Windows.Forms.Button()
        Me.gbxWinMonitorAdvancedOptionsInstallLocation = New System.Windows.Forms.GroupBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.lblPicture = New System.Windows.Forms.Label()
        Me.rdobtnWinMonitorAdvancedOptionsIncludeConfigFile = New System.Windows.Forms.RadioButton()
        Me.rdobtnWinMonitorAdvancedOptionsExcludeConfigFile = New System.Windows.Forms.RadioButton()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.rdobtnWinMonitorAdvancedOptionsInstallLocationProgramFiles = New System.Windows.Forms.RadioButton()
        Me.rdobtnWinMonitorAdvancedOptionsInstallLocationSystem = New System.Windows.Forms.RadioButton()
        Me.rdobtnWinMonitorAdvancedOptionsInstallLocationWindows = New System.Windows.Forms.RadioButton()
        Me.rdobtnWinMonitorAdvancedOptionsInstallLocationCommonFiles = New System.Windows.Forms.RadioButton()
        Me.gbxWinMonitorAdvancedOptionsInstallLocation.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblWinMonitorFileName
        '
        Me.lblWinMonitorFileName.Location = New System.Drawing.Point(8, 12)
        Me.lblWinMonitorFileName.Name = "lblWinMonitorFileName"
        Me.lblWinMonitorFileName.Size = New System.Drawing.Size(148, 22)
        Me.lblWinMonitorFileName.TabIndex = 0
        Me.lblWinMonitorFileName.Text = "WinMonitor Server file name"
        Me.lblWinMonitorFileName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtWinMonitorFileName
        '
        Me.txtWinMonitorFileName.Font = New System.Drawing.Font("Courier New", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtWinMonitorFileName.Location = New System.Drawing.Point(146, 12)
        Me.txtWinMonitorFileName.Name = "txtWinMonitorFileName"
        Me.txtWinMonitorFileName.Size = New System.Drawing.Size(238, 21)
        Me.txtWinMonitorFileName.TabIndex = 1
        Me.txtWinMonitorFileName.Text = ""
        '
        'btnWinMonitorAdvancedOk
        '
        Me.btnWinMonitorAdvancedOk.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnWinMonitorAdvancedOk.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnWinMonitorAdvancedOk.Location = New System.Drawing.Point(222, 218)
        Me.btnWinMonitorAdvancedOk.Name = "btnWinMonitorAdvancedOk"
        Me.btnWinMonitorAdvancedOk.Size = New System.Drawing.Size(78, 20)
        Me.btnWinMonitorAdvancedOk.TabIndex = 12
        Me.btnWinMonitorAdvancedOk.Text = "&Ok"
        '
        'btnWinMonitorAdvancedCancel
        '
        Me.btnWinMonitorAdvancedCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnWinMonitorAdvancedCancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnWinMonitorAdvancedCancel.Location = New System.Drawing.Point(302, 218)
        Me.btnWinMonitorAdvancedCancel.Name = "btnWinMonitorAdvancedCancel"
        Me.btnWinMonitorAdvancedCancel.Size = New System.Drawing.Size(78, 20)
        Me.btnWinMonitorAdvancedCancel.TabIndex = 13
        Me.btnWinMonitorAdvancedCancel.Text = "&Cancel"
        '
        'gbxWinMonitorAdvancedOptionsInstallLocation
        '
        Me.gbxWinMonitorAdvancedOptionsInstallLocation.Controls.AddRange(New System.Windows.Forms.Control() {Me.Panel1, Me.Label1, Me.rdobtnWinMonitorAdvancedOptionsInstallLocationProgramFiles, Me.rdobtnWinMonitorAdvancedOptionsInstallLocationSystem, Me.rdobtnWinMonitorAdvancedOptionsInstallLocationWindows, Me.rdobtnWinMonitorAdvancedOptionsInstallLocationCommonFiles})
        Me.gbxWinMonitorAdvancedOptionsInstallLocation.Location = New System.Drawing.Point(6, 46)
        Me.gbxWinMonitorAdvancedOptionsInstallLocation.Name = "gbxWinMonitorAdvancedOptionsInstallLocation"
        Me.gbxWinMonitorAdvancedOptionsInstallLocation.Size = New System.Drawing.Size(378, 164)
        Me.gbxWinMonitorAdvancedOptionsInstallLocation.TabIndex = 2
        Me.gbxWinMonitorAdvancedOptionsInstallLocation.TabStop = False
        Me.gbxWinMonitorAdvancedOptionsInstallLocation.Text = " [ WinMonitor Server Install Directory ]"
        '
        'Panel1
        '
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel1.Controls.AddRange(New System.Windows.Forms.Control() {Me.lblPicture, Me.rdobtnWinMonitorAdvancedOptionsIncludeConfigFile, Me.rdobtnWinMonitorAdvancedOptionsExcludeConfigFile})
        Me.Panel1.Location = New System.Drawing.Point(12, 76)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(354, 76)
        Me.Panel1.TabIndex = 8
        '
        'lblPicture
        '
        Me.lblPicture.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblPicture.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.lblPicture.Image = CType(resources.GetObject("lblPicture.Image"), System.Drawing.Bitmap)
        Me.lblPicture.Location = New System.Drawing.Point(6, 6)
        Me.lblPicture.Name = "lblPicture"
        Me.lblPicture.Size = New System.Drawing.Size(72, 60)
        Me.lblPicture.TabIndex = 11
        '
        'rdobtnWinMonitorAdvancedOptionsIncludeConfigFile
        '
        Me.rdobtnWinMonitorAdvancedOptionsIncludeConfigFile.Checked = True
        Me.rdobtnWinMonitorAdvancedOptionsIncludeConfigFile.Location = New System.Drawing.Point(86, 12)
        Me.rdobtnWinMonitorAdvancedOptionsIncludeConfigFile.Name = "rdobtnWinMonitorAdvancedOptionsIncludeConfigFile"
        Me.rdobtnWinMonitorAdvancedOptionsIncludeConfigFile.Size = New System.Drawing.Size(250, 16)
        Me.rdobtnWinMonitorAdvancedOptionsIncludeConfigFile.TabIndex = 9
        Me.rdobtnWinMonitorAdvancedOptionsIncludeConfigFile.TabStop = True
        Me.rdobtnWinMonitorAdvancedOptionsIncludeConfigFile.Text = "&Include Configuration file during installation"
        '
        'rdobtnWinMonitorAdvancedOptionsExcludeConfigFile
        '
        Me.rdobtnWinMonitorAdvancedOptionsExcludeConfigFile.Location = New System.Drawing.Point(86, 42)
        Me.rdobtnWinMonitorAdvancedOptionsExcludeConfigFile.Name = "rdobtnWinMonitorAdvancedOptionsExcludeConfigFile"
        Me.rdobtnWinMonitorAdvancedOptionsExcludeConfigFile.Size = New System.Drawing.Size(250, 16)
        Me.rdobtnWinMonitorAdvancedOptionsExcludeConfigFile.TabIndex = 10
        Me.rdobtnWinMonitorAdvancedOptionsExcludeConfigFile.Text = "&Exclude Configuration file during installation"
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.Gray
        Me.Label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label1.Location = New System.Drawing.Point(12, 60)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(354, 3)
        Me.Label1.TabIndex = 7
        '
        'rdobtnWinMonitorAdvancedOptionsInstallLocationProgramFiles
        '
        Me.rdobtnWinMonitorAdvancedOptionsInstallLocationProgramFiles.Location = New System.Drawing.Point(281, 34)
        Me.rdobtnWinMonitorAdvancedOptionsInstallLocationProgramFiles.Name = "rdobtnWinMonitorAdvancedOptionsInstallLocationProgramFiles"
        Me.rdobtnWinMonitorAdvancedOptionsInstallLocationProgramFiles.Size = New System.Drawing.Size(86, 16)
        Me.rdobtnWinMonitorAdvancedOptionsInstallLocationProgramFiles.TabIndex = 6
        Me.rdobtnWinMonitorAdvancedOptionsInstallLocationProgramFiles.Text = "&Program Files"
        '
        'rdobtnWinMonitorAdvancedOptionsInstallLocationSystem
        '
        Me.rdobtnWinMonitorAdvancedOptionsInstallLocationSystem.Location = New System.Drawing.Point(205, 34)
        Me.rdobtnWinMonitorAdvancedOptionsInstallLocationSystem.Name = "rdobtnWinMonitorAdvancedOptionsInstallLocationSystem"
        Me.rdobtnWinMonitorAdvancedOptionsInstallLocationSystem.Size = New System.Drawing.Size(94, 16)
        Me.rdobtnWinMonitorAdvancedOptionsInstallLocationSystem.TabIndex = 5
        Me.rdobtnWinMonitorAdvancedOptionsInstallLocationSystem.Text = "&System"
        '
        'rdobtnWinMonitorAdvancedOptionsInstallLocationWindows
        '
        Me.rdobtnWinMonitorAdvancedOptionsInstallLocationWindows.Checked = True
        Me.rdobtnWinMonitorAdvancedOptionsInstallLocationWindows.Location = New System.Drawing.Point(11, 34)
        Me.rdobtnWinMonitorAdvancedOptionsInstallLocationWindows.Name = "rdobtnWinMonitorAdvancedOptionsInstallLocationWindows"
        Me.rdobtnWinMonitorAdvancedOptionsInstallLocationWindows.Size = New System.Drawing.Size(94, 16)
        Me.rdobtnWinMonitorAdvancedOptionsInstallLocationWindows.TabIndex = 3
        Me.rdobtnWinMonitorAdvancedOptionsInstallLocationWindows.TabStop = True
        Me.rdobtnWinMonitorAdvancedOptionsInstallLocationWindows.Text = "&Windows"
        '
        'rdobtnWinMonitorAdvancedOptionsInstallLocationCommonFiles
        '
        Me.rdobtnWinMonitorAdvancedOptionsInstallLocationCommonFiles.Location = New System.Drawing.Point(105, 34)
        Me.rdobtnWinMonitorAdvancedOptionsInstallLocationCommonFiles.Name = "rdobtnWinMonitorAdvancedOptionsInstallLocationCommonFiles"
        Me.rdobtnWinMonitorAdvancedOptionsInstallLocationCommonFiles.Size = New System.Drawing.Size(94, 16)
        Me.rdobtnWinMonitorAdvancedOptionsInstallLocationCommonFiles.TabIndex = 4
        Me.rdobtnWinMonitorAdvancedOptionsInstallLocationCommonFiles.Text = "Co&mmon Files"
        '
        'FrmWinMonitorServerConfigurationsAdvanced
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(390, 245)
        Me.Controls.AddRange(New System.Windows.Forms.Control() {Me.gbxWinMonitorAdvancedOptionsInstallLocation, Me.btnWinMonitorAdvancedCancel, Me.btnWinMonitorAdvancedOk, Me.txtWinMonitorFileName, Me.lblWinMonitorFileName})
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FrmWinMonitorServerConfigurationsAdvanced"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "WinMonitor Server Advanced Building Options"
        Me.gbxWinMonitorAdvancedOptionsInstallLocation.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region


   Private Sub txtWinMonitorFileName_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtWinMonitorFileName.Validated
        Dim FrmWinMonitorServer As New frmWinMonitorServerConfig()
      FrmWinMonitorServer.CurrentTextBox = FrmWinMonitorServer.TEXT_IDS.ID_EXE_FILE_NAME
      FrmWinMonitorServer.txtLogFileName_Validated(sender, e)
      FrmWinMonitorServer.CurrentTextBox = FrmWinMonitorServer.TEXT_IDS.ID_KEYLOGGER_FILENAME
      FrmWinMonitorServer = Nothing
   End Sub

   Private Sub txtWinMonitorFileName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtWinMonitorFileName.KeyUp
        Dim FrmWinMonitorServer As New frmWinMonitorServerConfig()
      FrmWinMonitorServer.CurrentTextBox = FrmWinMonitorServer.TEXT_IDS.ID_EXE_FILE_NAME
      FrmWinMonitorServer.txtLogFileName_KeyUp(sender, e)
      FrmWinMonitorServer.CurrentTextBox = FrmWinMonitorServer.TEXT_IDS.ID_KEYLOGGER_FILENAME
      FrmWinMonitorServer = Nothing
   End Sub

  
End Class
