

Imports WinMonitorBLIB_1_0

Public Class FrmPriorities : Inherits System.Windows.Forms.Form

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
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents gbxProcessPriorities As System.Windows.Forms.GroupBox
    Friend WithEvents rdobtnPriorityIdle As System.Windows.Forms.RadioButton
    Friend WithEvents rdobtnPriorityNormal As System.Windows.Forms.RadioButton
    Friend WithEvents rdobtnPriorityBelowNormal As System.Windows.Forms.RadioButton
    Friend WithEvents rdobtnPriorityAboveNormal As System.Windows.Forms.RadioButton
    Friend WithEvents rdobtnPriorityHigh As System.Windows.Forms.RadioButton
    Friend WithEvents rdobtnPriorityRealTime As System.Windows.Forms.RadioButton
    Friend WithEvents btnOk As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(FrmPriorities))
        Me.gbxProcessPriorities = New System.Windows.Forms.GroupBox()
        Me.rdobtnPriorityRealTime = New System.Windows.Forms.RadioButton()
        Me.rdobtnPriorityHigh = New System.Windows.Forms.RadioButton()
        Me.rdobtnPriorityAboveNormal = New System.Windows.Forms.RadioButton()
        Me.rdobtnPriorityBelowNormal = New System.Windows.Forms.RadioButton()
        Me.rdobtnPriorityNormal = New System.Windows.Forms.RadioButton()
        Me.rdobtnPriorityIdle = New System.Windows.Forms.RadioButton()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnOk = New System.Windows.Forms.Button()
        Me.gbxProcessPriorities.SuspendLayout()
        Me.SuspendLayout()
        '
        'gbxProcessPriorities
        '
        Me.gbxProcessPriorities.Controls.AddRange(New System.Windows.Forms.Control() {Me.rdobtnPriorityRealTime, Me.rdobtnPriorityHigh, Me.rdobtnPriorityAboveNormal, Me.rdobtnPriorityBelowNormal, Me.rdobtnPriorityNormal, Me.rdobtnPriorityIdle})
        Me.gbxProcessPriorities.Location = New System.Drawing.Point(15, 18)
        Me.gbxProcessPriorities.Name = "gbxProcessPriorities"
        Me.gbxProcessPriorities.Size = New System.Drawing.Size(434, 135)
        Me.gbxProcessPriorities.TabIndex = 0
        Me.gbxProcessPriorities.TabStop = False
        Me.gbxProcessPriorities.Text = "Select priority level"
        '
        'rdobtnPriorityRealTime
        '
        Me.rdobtnPriorityRealTime.Location = New System.Drawing.Point(227, 96)
        Me.rdobtnPriorityRealTime.Name = "rdobtnPriorityRealTime"
        Me.rdobtnPriorityRealTime.Size = New System.Drawing.Size(190, 22)
        Me.rdobtnPriorityRealTime.TabIndex = 5
        Me.rdobtnPriorityRealTime.Text = "Run with real time priority..."
        '
        'rdobtnPriorityHigh
        '
        Me.rdobtnPriorityHigh.Location = New System.Drawing.Point(227, 60)
        Me.rdobtnPriorityHigh.Name = "rdobtnPriorityHigh"
        Me.rdobtnPriorityHigh.Size = New System.Drawing.Size(190, 22)
        Me.rdobtnPriorityHigh.TabIndex = 4
        Me.rdobtnPriorityHigh.Text = "Run with high priority..."
        '
        'rdobtnPriorityAboveNormal
        '
        Me.rdobtnPriorityAboveNormal.Location = New System.Drawing.Point(227, 24)
        Me.rdobtnPriorityAboveNormal.Name = "rdobtnPriorityAboveNormal"
        Me.rdobtnPriorityAboveNormal.Size = New System.Drawing.Size(190, 22)
        Me.rdobtnPriorityAboveNormal.TabIndex = 3
        Me.rdobtnPriorityAboveNormal.Text = "Run with above normal priority..."
        '
        'rdobtnPriorityBelowNormal
        '
        Me.rdobtnPriorityBelowNormal.Location = New System.Drawing.Point(17, 96)
        Me.rdobtnPriorityBelowNormal.Name = "rdobtnPriorityBelowNormal"
        Me.rdobtnPriorityBelowNormal.Size = New System.Drawing.Size(190, 22)
        Me.rdobtnPriorityBelowNormal.TabIndex = 2
        Me.rdobtnPriorityBelowNormal.Text = "Run with below normal priority..."
        '
        'rdobtnPriorityNormal
        '
        Me.rdobtnPriorityNormal.Checked = True
        Me.rdobtnPriorityNormal.Location = New System.Drawing.Point(17, 60)
        Me.rdobtnPriorityNormal.Name = "rdobtnPriorityNormal"
        Me.rdobtnPriorityNormal.Size = New System.Drawing.Size(190, 22)
        Me.rdobtnPriorityNormal.TabIndex = 1
        Me.rdobtnPriorityNormal.TabStop = True
        Me.rdobtnPriorityNormal.Text = "Run with normal priority mode..."
        '
        'rdobtnPriorityIdle
        '
        Me.rdobtnPriorityIdle.Location = New System.Drawing.Point(17, 24)
        Me.rdobtnPriorityIdle.Name = "rdobtnPriorityIdle"
        Me.rdobtnPriorityIdle.Size = New System.Drawing.Size(190, 22)
        Me.rdobtnPriorityIdle.TabIndex = 0
        Me.rdobtnPriorityIdle.Text = "Run when system become idle..."
        '
        'btnCancel
        '
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnCancel.Location = New System.Drawing.Point(369, 162)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(80, 20)
        Me.btnCancel.TabIndex = 9
        Me.btnCancel.Text = "&Cancel"
        '
        'btnOk
        '
        Me.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnOk.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnOk.Location = New System.Drawing.Point(286, 162)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(80, 20)
        Me.btnOk.TabIndex = 10
        Me.btnOk.Text = "&Ok"
        '
        'FrmPriorities
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(464, 191)
        Me.Controls.AddRange(New System.Windows.Forms.Control() {Me.btnOk, Me.btnCancel, Me.gbxProcessPriorities})
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "FrmPriorities"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Process priority level selector"
        Me.gbxProcessPriorities.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region


    Public ReadOnly Property PriorityLevel() As Byte
        Get
            Select Case True
                Case rdobtnPriorityAboveNormal.Checked
                    Return COMMAND_MONITOR_PROCESS_PRIORITY_ABOVE_NORMAL
                Case rdobtnPriorityBelowNormal.Checked
                    Return COMMAND_MONITOR_PROCESS_PRIORITY_BELOW_NORMAL
                Case rdobtnPriorityHigh.Checked
                    Return COMMAND_MONITOR_PROCESS_PRIORITY_HIGH_PRIORITY
                Case rdobtnPriorityIdle.Checked
                    Return COMMAND_MONITOR_PROCESS_PRIORITY_IDLE_PRIORITY
                Case rdobtnPriorityNormal.Checked
                    Return COMMAND_MONITOR_PROCESS_PRIORITY_NORMAL_PRIORITY
                Case rdobtnPriorityRealTime.Checked
                    Return COMMAND_MONITOR_PROCESS_PRIORITY_REALTIME_PRIORITY
                Case Else
                    Return COMMAND_MONITOR_PROCESS_PRIORITY_NO_CHANGE
            End Select
        End Get
    End Property
End Class
