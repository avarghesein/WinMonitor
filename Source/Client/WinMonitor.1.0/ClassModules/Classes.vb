
Imports WinMonitorBLIB_1_0

Public Class CExtendedImageList : Implements IDisposable

    Private m_ImageList As ImageList
    Private m_ImageTag() As String
    Private m_strUty As CStringUtility

    Public Sub Delete() Implements IDisposable.Dispose
        m_ImageList = Nothing
        m_ImageTag = Nothing
        m_strUty = Nothing
    End Sub

    Public ReadOnly Property CurrentImageList() As ImageList
        Get
            Return m_ImageList
        End Get
    End Property

    Public Sub New(ByRef ImgList As ImageList)
        m_ImageList = ImgList
        m_strUty = New CStringUtility()
        ReDim m_ImageTag(m_ImageList.Images.Count())
        Dim intPopCount As Integer
        For intPopCount = 0 To m_ImageList.Images.Count - 1 Step 1
            m_ImageTag(intPopCount) = IDS_NULL_STRING
        Next intPopCount
    End Sub

    Public Function GetImageIndexByFIleExtension(ByVal FileName As String) As Integer

        Dim strExtn As String
        Try
            strExtn = FileName.Substring(FileName.LastIndexOf(IDS_DOT)).Trim()
        Catch Ex As Exception
            strExtn = ".FILE"
        End Try

        Dim intPopCount As Integer
        For intPopCount = 0 To m_ImageList.Images.Count - 1 Step 1
            If (m_ImageTag(intPopCount).Trim.CompareTo(strExtn) = 0) Then Return intPopCount
        Next intPopCount
        Try
            m_ImageList.Images.Add(m_strUty.GetIconByExtension(strExtn, 32, 32))
        Catch ex As Exception
            MsgBox(" dddd" & ex.Message())
        End Try
        ReDim Preserve m_ImageTag(m_ImageList.Images.Count())
        m_ImageTag(m_ImageList.Images.Count() - 1) = strExtn
        Return m_ImageList.Images.Count() - 1
    End Function

End Class
