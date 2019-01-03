

Imports WinMonitorBLIB_1_0

Module Main

    Public Sub main()
        Dim m_objNwIface As New CTcpNetworkMonitorInterface()
        Dim frmMdi As New frmWinMonitorMdi()
        m_objNwIface.Initialize()
        frmMdi.ShowDialog(Nothing)
        CleanUp(frmMdi)
        m_objNwIface.CleanUp()
    End Sub

    Public Sub CleanUp(ByRef PFrm As Form)
        Dim Cntrl As Form
        For Each Cntrl In PFrm.MdiChildren
            If (TypeOf (Cntrl) Is Form) Then
                Cntrl.Close()
            End If
        Next Cntrl
    End Sub

End Module
