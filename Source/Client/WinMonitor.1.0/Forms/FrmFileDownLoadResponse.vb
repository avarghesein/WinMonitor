

Public Class FrmFileDownLoadResponse : Inherits System.Windows.Forms.Form

    Public Event EventUserCancelled(ByRef Frm As FrmFileDownLoadResponse)

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
    Friend WithEvents lblRespondCaption As System.Windows.Forms.Label
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents statBarPanelStatus As System.Windows.Forms.StatusBarPanel
    Friend WithEvents statBarStatus As System.Windows.Forms.StatusBar
    Friend WithEvents lblProgressBarBase As System.Windows.Forms.Label
    Friend WithEvents lblProgressBar As System.Windows.Forms.Label
    Friend WithEvents lblProgressBarSub As System.Windows.Forms.Label
    Friend WithEvents lblProgressBarBaseSub As System.Windows.Forms.Label
    Friend WithEvents pnlProgressBarBase As System.Windows.Forms.Panel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(FrmFileDownLoadResponse))
        Me.lblRespondCaption = New System.Windows.Forms.Label()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.statBarStatus = New System.Windows.Forms.StatusBar()
        Me.statBarPanelStatus = New System.Windows.Forms.StatusBarPanel()
        Me.pnlProgressBarBase = New System.Windows.Forms.Panel()
        Me.lblProgressBarSub = New System.Windows.Forms.Label()
        Me.lblProgressBarBaseSub = New System.Windows.Forms.Label()
        Me.lblProgressBar = New System.Windows.Forms.Label()
        Me.lblProgressBarBase = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        CType(Me.statBarPanelStatus, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlProgressBarBase.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblRespondCaption
        '
        Me.lblRespondCaption.BackColor = System.Drawing.SystemColors.Control
        Me.lblRespondCaption.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblRespondCaption.ForeColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(0, Byte), CType(64, Byte))
        Me.lblRespondCaption.Image = CType(resources.GetObject("lblRespondCaption.Image"), System.Drawing.Bitmap)
        Me.lblRespondCaption.ImageAlign = System.Drawing.ContentAlignment.BottomLeft
        Me.lblRespondCaption.Location = New System.Drawing.Point(2, 2)
        Me.lblRespondCaption.Name = "lblRespondCaption"
        Me.lblRespondCaption.Size = New System.Drawing.Size(452, 36)
        Me.lblRespondCaption.TabIndex = 4
        Me.lblRespondCaption.Text = "fgdfg"
        Me.lblRespondCaption.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnCancel
        '
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnCancel.Location = New System.Drawing.Point(370, 110)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(80, 20)
        Me.btnCancel.TabIndex = 9
        Me.btnCancel.Text = "&Cancel"
        '
        'statBarStatus
        '
        Me.statBarStatus.Location = New System.Drawing.Point(0, 136)
        Me.statBarStatus.Name = "statBarStatus"
        Me.statBarStatus.Panels.AddRange(New System.Windows.Forms.StatusBarPanel() {Me.statBarPanelStatus})
        Me.statBarStatus.ShowPanels = True
        Me.statBarStatus.Size = New System.Drawing.Size(456, 18)
        Me.statBarStatus.TabIndex = 10
        '
        'statBarPanelStatus
        '
        Me.statBarPanelStatus.Width = 400
        '
        'pnlProgressBarBase
        '
        Me.pnlProgressBarBase.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlProgressBarBase.Controls.AddRange(New System.Windows.Forms.Control() {Me.lblProgressBarSub, Me.lblProgressBarBaseSub, Me.lblProgressBar, Me.lblProgressBarBase})
        Me.pnlProgressBarBase.Location = New System.Drawing.Point(6, 48)
        Me.pnlProgressBarBase.Name = "pnlProgressBarBase"
        Me.pnlProgressBarBase.Size = New System.Drawing.Size(443, 58)
        Me.pnlProgressBarBase.TabIndex = 12
        '
        'lblProgressBarSub
        '
        Me.lblProgressBarSub.BackColor = System.Drawing.Color.Navy
        Me.lblProgressBarSub.Location = New System.Drawing.Point(8, 32)
        Me.lblProgressBarSub.Name = "lblProgressBarSub"
        Me.lblProgressBarSub.Size = New System.Drawing.Size(4, 14)
        Me.lblProgressBarSub.TabIndex = 16
        Me.lblProgressBarSub.Text = "/"
        '
        'lblProgressBarBaseSub
        '
        Me.lblProgressBarBaseSub.BackColor = System.Drawing.Color.White
        Me.lblProgressBarBaseSub.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblProgressBarBaseSub.Location = New System.Drawing.Point(6, 30)
        Me.lblProgressBarBaseSub.Name = "lblProgressBarBaseSub"
        Me.lblProgressBarBaseSub.Size = New System.Drawing.Size(428, 18)
        Me.lblProgressBarBaseSub.TabIndex = 15
        '
        'lblProgressBar
        '
        Me.lblProgressBar.BackColor = System.Drawing.Color.Navy
        Me.lblProgressBar.Location = New System.Drawing.Point(8, 8)
        Me.lblProgressBar.Name = "lblProgressBar"
        Me.lblProgressBar.Size = New System.Drawing.Size(4, 14)
        Me.lblProgressBar.TabIndex = 14
        Me.lblProgressBar.Text = "/"
        '
        'lblProgressBarBase
        '
        Me.lblProgressBarBase.BackColor = System.Drawing.Color.White
        Me.lblProgressBarBase.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblProgressBarBase.Location = New System.Drawing.Point(6, 6)
        Me.lblProgressBarBase.Name = "lblProgressBarBase"
        Me.lblProgressBarBase.Size = New System.Drawing.Size(428, 18)
        Me.lblProgressBarBase.TabIndex = 12
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.Silver
        Me.Label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label1.Location = New System.Drawing.Point(2, 38)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(452, 3)
        Me.Label1.TabIndex = 13
        '
        'FrmFileDownLoadResponse
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(456, 154)
        Me.Controls.AddRange(New System.Windows.Forms.Control() {Me.Label1, Me.pnlProgressBarBase, Me.statBarStatus, Me.btnCancel, Me.lblRespondCaption})
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "FrmFileDownLoadResponse"
        Me.Text = "Please wait..."
        CType(Me.statBarPanelStatus, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlProgressBarBase.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        RaiseEvent EventUserCancelled(Me)
    End Sub
End Class
