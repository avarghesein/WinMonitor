


Public Class FrmResponse : Inherits System.Windows.Forms.Form

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
    Friend WithEvents lblProgressBarBase As System.Windows.Forms.Label
    Friend WithEvents lblProgressBar As System.Windows.Forms.Label
    Friend WithEvents lblRespondCaption As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(FrmResponse))
        Me.lblProgressBarBase = New System.Windows.Forms.Label()
        Me.lblProgressBar = New System.Windows.Forms.Label()
        Me.lblRespondCaption = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'lblProgressBarBase
        '
        Me.lblProgressBarBase.BackColor = System.Drawing.Color.White
        Me.lblProgressBarBase.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblProgressBarBase.Location = New System.Drawing.Point(2, 18)
        Me.lblProgressBarBase.Name = "lblProgressBarBase"
        Me.lblProgressBarBase.Size = New System.Drawing.Size(422, 18)
        Me.lblProgressBarBase.TabIndex = 0
        '
        'lblProgressBar
        '
        Me.lblProgressBar.BackColor = System.Drawing.Color.Navy
        Me.lblProgressBar.Location = New System.Drawing.Point(2, 18)
        Me.lblProgressBar.Name = "lblProgressBar"
        Me.lblProgressBar.Size = New System.Drawing.Size(4, 18)
        Me.lblProgressBar.TabIndex = 1
        Me.lblProgressBar.Text = "/"
        '
        'lblRespondCaption
        '
        Me.lblRespondCaption.BackColor = System.Drawing.SystemColors.Control
        Me.lblRespondCaption.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblRespondCaption.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblRespondCaption.ForeColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(0, Byte), CType(64, Byte))
        Me.lblRespondCaption.Name = "lblRespondCaption"
        Me.lblRespondCaption.Size = New System.Drawing.Size(426, 18)
        Me.lblRespondCaption.TabIndex = 2
        Me.lblRespondCaption.Text = "fgdfg"
        Me.lblRespondCaption.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'FrmResponse
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(426, 38)
        Me.Controls.AddRange(New System.Windows.Forms.Control() {Me.lblRespondCaption, Me.lblProgressBar, Me.lblProgressBarBase})
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "FrmResponse"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Please wait...."
        Me.ResumeLayout(False)

    End Sub

#End Region

End Class
