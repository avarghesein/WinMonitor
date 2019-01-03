

Public Class FrmRemoteExplorer : Inherits System.Windows.Forms.Form


    Private m_IsClose As Boolean
    Friend m_usrcntrlFolderBrowser As usrctrlFolderBrowser = Nothing

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        '---------User Control Creation---------------'

        m_usrcntrlFolderBrowser = New usrctrlFolderBrowser()
        Me.Controls.Add(m_usrcntrlFolderBrowser)
        m_usrcntrlFolderBrowser.Dock = DockStyle.Fill
        m_usrcntrlFolderBrowser.Visible = True
        '---------------------------------------------'

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
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(FrmRemoteExplorer))
        '
        'FrmRemoteExplorer
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(458, 313)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "FrmRemoteExplorer"
        Me.Text = "Remote Explorer"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized

    End Sub

#End Region

    Public WriteOnly Property CloseOnExit() As Boolean
        Set(ByVal Value As Boolean)
            m_IsClose = Value
        End Set
    End Property

    Protected Overrides Sub OnClosing(ByVal e As System.ComponentModel.CancelEventArgs)
        e.Cancel = Not m_IsClose
        If (Not m_IsClose) Then
            Me.Visible = False
        Else
            m_usrcntrlFolderBrowser = Nothing
        End If
    End Sub

    Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
        'm_usrcntrlFolderBrowser.txtFolderName.BackColor = System.Drawing.Color.Wheat
        'm_usrcntrlFolderBrowser.txtFolderName.ForeColor = System.Drawing.Color.Black
        'm_usrcntrlFolderBrowser.txtSelectedFolder.BackColor = System.Drawing.Color.Wheat
        'm_usrcntrlFolderBrowser.txtSelectedFolder.ForeColor = System.Drawing.Color.Black
        'm_usrcntrlFolderBrowser.lstwFolders.BackColor = System.Drawing.Color.Wheat
        'm_usrcntrlFolderBrowser.trewFolders.BackColor = System.Drawing.Color.Wheat
    End Sub

End Class
