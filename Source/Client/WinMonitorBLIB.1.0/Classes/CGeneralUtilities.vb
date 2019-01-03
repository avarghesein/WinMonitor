
Imports System.Windows.Forms

'Class(Public):String Utilities

Public Class CStringUtility

    Private Declare Function GetFolderName Lib "WinMonitorCLIB.1.0.dll" Alias "GetFolderName" (ByVal Caption As String, ByRef strFolderName As String, ByVal hWnd As IntPtr) As Boolean
    Private Declare Function GetIconByExtn Lib "WinMonitorCLIB.1.0.dll" Alias "GetIconHandle" (ByVal strExtn As String) As System.IntPtr

#Region "Public Functions"

    Public Declare Function WritePrivateProfileString Lib "kernel32" Alias "WritePrivateProfileStringA" (ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal lpString As String, ByVal lpFileName As String) As Long
    Public Declare Function GetPrivateProfileString Lib "kernel32" Alias "GetPrivateProfileStringA" (ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal lpDefault As String, ByVal lpReturnedString As String, ByVal nSize As Long, ByVal lpFileName As String) As Long

    Public Function BrowseFolder(ByVal Caption As String, ByRef FolderName As String, ByVal MyHwnd As IntPtr) As Boolean
        If (IsNothing(FolderName)) Then FolderName = FreshString(IDL_FILENAME_MAX_LEN)
        If (GetFolderName(Caption, FolderName, MyHwnd)) Then
            FolderName = CStyleStringToVbString(FolderName)
            Return True
        Else
            Return False
        End If
    End Function

    Public Function GetIconByExtension(ByVal strExtension As String, Optional ByVal Width As Integer = 16, Optional ByVal Height As Integer = 16) As System.Drawing.Bitmap
        Try
            Dim IconHandle As System.IntPtr = GetIconByExtn(strExtension)
            Dim HIcon As New System.Drawing.Icon(System.Drawing.Icon.FromHandle(IconHandle), Width, Height)
            Return HIcon.ToBitmap()
        Catch Ex As Exception
            Return Nothing
        End Try
    End Function


#Region "Fun(Private):PreviousFileSlash"

    '---Remarks: IDS_FILE_SLASH='\' and IDS_NONFILE_SLASH='/', You can change
    '---Slahes by redefining the above constant, we used '\' below assuming
    '---that IDS_FILE_SLASH='\' and IDS_NONFILE_SLASH='/', you can reverse 
    '---that by redefining the constant.
    '---Finding Index of immediately previous '\' from index intIndex in the string strText
    '---If no such a slash exists it returns 0, if found it will return the next charcter's
    '---Index(character which immediately succeeding the previous '\' just found.

    Private Function PreviousFileSlash(ByVal strText As String, ByVal intIndex As Integer) As Integer
        If (IsNothing(strText)) Then Return -1
        If (strText.Trim() = IDS_NULL_STRING) Then Return -1
        If (intIndex >= strText.Length) Then Return 0
        Dim strSubStr As String
        strSubStr = strText.Substring(0, intIndex)
        Dim intSlashIndex As Integer = strSubStr.LastIndexOf(IDS_FILE_SLASH)
        If (intSlashIndex >= 0) Then
            Return intSlashIndex + 1
        Else
            Return 0
        End If
    End Function
#End Region

#Region "Fun(Public):ValidPathAndFileNameChar"

    '---Remarks: IDS_FILE_SLASH='\' and IDS_NONFILE_SLASH='/', You can change
    '---Slahes by redefining the above constant, we used '\' below assuming
    '---that IDS_FILE_SLASH='\' and IDS_NONFILE_SLASH='/', you can reverse 
    '---that by redefining the constant.
    '---This function will find whether a string is a probable valid path to
    '---a filename,if found chars such as *,|,/,:,<,>,? with in the string
    '---it will return false,It allows \ and ., but returns false if it will
    '---find a directory name containing all dots(.) or one which will end 
    '---with dots, such as \...\ or \..as.\ etc, But it allows dots and \'s
    '---at the very end of string and accepts an empty string with the assumption 
    '---that the string may be edited to a valid one in futre.So this function can
    '--- use to validate a string which is beging updated.It allows length of names
    '--- about IDI_DIRECTORYORFILE_NAME_MAXLENGTH declared as const int.

    Public Function ValidPathAndFileNameChar(ByVal strText As String) As Boolean
        If (strText Is Nothing) Then Return False
        If (strText.Trim() = IDS_NULL_STRING Or strText.Trim().Equals(IDS_FILE_SLASH)) Then Return True
        Dim intI As Integer
        Dim strSubStr As String
        For intI = 0 To strText.Length - 1 Step 1
            strSubStr = strText.Substring(intI, 1)
            If (strSubStr.Equals("*") Or strSubStr.Equals(IDS_NONFILE_SLASH) Or _
                strSubStr.Equals("<") Or strSubStr.Equals(">") Or _
                strSubStr.Equals("?") Or strSubStr.Equals(":") Or _
                strSubStr.Equals("|")) Then
                Return False
            End If
            If (strSubStr = IDS_FILE_SLASH) Then
                If (intI < strText.Length - 1) Then
                    If (strText.Substring(intI + 1, 1) = IDS_FILE_SLASH) Then Return False
                End If
                Dim intPrevSlashIndex As Integer = PreviousFileSlash(strText, intI)
                If (intPrevSlashIndex <> -1) Then
                    Dim strTmpStr As String = strText.Substring(intPrevSlashIndex, intI - intPrevSlashIndex).Trim()
                    If (strTmpStr.EndsWith(IDS_DOT)) Then Return False
                    If (strTmpStr.Length() > IDI_DIRECTORYORFILE_NAME_MAXLENGTH) Then Return False
                    If (Not strTmpStr.Equals(IDS_NULL_STRING)) Then
                        If (strTmpStr.Replace(IDS_DOT, IDS_NULL_STRING).Trim().Equals(IDS_NULL_STRING)) Then Return False
                    End If
                End If
            End If
        Next intI
        Return True
    End Function
#End Region

#Region "Fun(Public):ValidPathAndFileName"

    '---Remarks: IDS_FILE_SLASH='\' and IDS_NONFILE_SLASH='/', You can change
    '---Slahes by redefining the above constant, we used '\' below assuming
    '---that IDS_FILE_SLASH='\' and IDS_NONFILE_SLASH='/', you can reverse 
    '---that by redefining the constant.
    '---This function will find whether a string is a probable valid path to
    '---a filename or directory ,if found chars such as *,|,/,:,<,>,? with in
    '--- the string it will return false,It allows \ and ., but returns false 
    '---if it will find a directory name containing all dots(.) or one which will end 
    '---with dots, such as \...\ or \..as.\ etc,It will not allow a \ or .
    '---at the end of the string:So it can use to validate a path Finally.It
    '---allows \ at the end if we are considering a directory name by specifying
    '---boolIsDir as true.

    Public Function ValidPathAndFileName(ByRef strText As String, ByVal boolIsDir As Boolean) As Boolean
        If (Not ValidPathAndFileNameChar(strText)) Then Return False
        Dim strTmpStr As String = strText.Trim()
        If (strTmpStr.EndsWith(IDS_DOT) Or (Not boolIsDir And strTmpStr.EndsWith(IDS_FILE_SLASH)) Or strTmpStr.Equals(IDS_NULL_STRING)) Then Return False
        If (Not strTmpStr.StartsWith(IDS_FILE_SLASH)) Then
            strText = IDS_FILE_SLASH + strText.Trim()
        End If
        Return True
    End Function
#End Region

#Region "Fun(Public):IsNumeralString"

    '---Finds whether all characters in the string are digits b/w(0-9)

    Public Function IsNumeralString(ByVal strNumeralString As String) As Boolean
        If (IsNothing(strNumeralString)) Then Return False
        If (strNumeralString.Trim().Equals(IDS_NULL_STRING)) Then Return False
        Dim intI As Integer
        Dim chrDigit As Char
        For intI = 0 To strNumeralString.Length - 1 Step 1
            chrDigit = strNumeralString.Chars(intI)
            If (chrDigit < IDC_CHAR0 Or chrDigit > IDC_CHAR9) Then Return False
        Next intI
        Return True
    End Function
#End Region

    Public Function IsValidIdentifierChar(ByVal strIdentifierString As String) As Boolean
        If (IsNothing(strIdentifierString)) Then Return False
        If (strIdentifierString.Trim().Equals(IDS_NULL_STRING)) Then Return True
        'If (IsNumeralString(strIdentifierString.Trim().Substring(0, 1))) = True Then Return False
        Return True
    End Function

    Public Function IsValidIdentifier(ByVal strIdentifierString As String) As Boolean
        If (strIdentifierString.Trim().Equals(IDS_NULL_STRING)) Then Return False
        Return IsValidIdentifierChar(strIdentifierString)
    End Function

#Region "Fun(Public):ValidIPChar"

    '--This function validates an Ip, which is being edited
    '--So it will check whether the Ip beging edited have any 
    '--chance to be a valid Ip-Address after editing; So it 
    '--allows '.' at the end of string. It also allows 'Empty
    '--String' as well.

    Public Function ValidIPChar(ByVal Ip As String) As Boolean
        If (IsNothing(Ip) = True) Then Return False
        If (Ip.Equals(IDS_NULL_STRING)) Then Return True
        If (Ip.StartsWith(IDS_DOT)) Then Return False
        Dim intI As Integer = 0
        Dim strTmp As String = Ip
        Dim intDotCount As Integer = 0
        While (Not Ip.Equals(IDS_NULL_STRING))
            intI = Ip.IndexOf(IDS_DOT, 0)
            If (intI < Ip.Length - 1) Then
                If (Ip.Substring(intI + 1, 1).Equals(IDS_DOT)) Then Return False
            End If
            If (intI <> -1) Then intDotCount += 1
            If (intDotCount > 3) Then Return False
            If (intI <> -1) Then
                strTmp = IIf(intI = 0, IDS_NULL_STRING, Ip.Substring(0, intI))
                Ip = Ip.Substring(intI + 1, Ip.Length - (intI + 1))
            Else
                strTmp = Ip.Substring(0)
                Ip = IDS_NULL_STRING
            End If
            If (strTmp.Equals(IDS_NULL_STRING)) Then Return True
            If ((Not IsNumeralString(strTmp)) Or strTmp.Length() > 3 Or strTmp.Length() < 0 _
                 Or Val(strTmp) < 0 Or Val(strTmp) > 255) Then Return False
        End While
        Return True
    End Function

#End Region

#Region "Fun(Public):ValidIP"

    '--Checks whether the string represents a valid IP-Address

    Public Function ValidIP(ByVal Ip As String) As Boolean
        If (Not ValidIPChar(Ip)) Then Return False
        If (Ip.EndsWith(IDS_DOT)) Then Return False

        Dim intDotCount As Integer = 0
        Dim intIndex As Integer
        While (Not Ip.Equals(IDS_NULL_STRING))
            intIndex = Ip.IndexOf(IDS_DOT)
            If (intIndex <> -1) Then
                intDotCount += 1
                Ip = Ip.Substring(intIndex + 1, Ip.Length - (intIndex + 1))
            Else
                Ip = IDS_NULL_STRING
            End If
        End While
        If (intDotCount <> 3) Then Return False
        Return True
    End Function

#End Region

#Region "Fun(Public):SelectText"

    '--Selects text in as text box 
    Public Function SelectText(ByRef Sender As TextBox) As Boolean
        Sender.Focus()
        If (Not Sender.Text.Equals(IDS_NULL_STRING)) Then Return False
        SendKeys.Send("{HOME}+{END}")
        Return True
    End Function

#End Region

    Public Sub MsgErr(ByRef Msg As String, ByRef caption As String)
        Call MessageBox.Show(Msg, caption, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
        Return
    End Sub

    Public Function UniCodeArrayToByteArray(ByVal UniCodeArray As String, ByVal UnicodeArrayLen As Long) As Byte()
        Dim bytArray(UnicodeArrayLen) As Byte
        Dim intCounter As Long
        Try
            For intCounter = 0 To UnicodeArrayLen - 1 Step 1
                bytArray(intCounter) = Convert.ToByte(UniCodeArray.Chars(intCounter))
            Next intCounter
            Return bytArray
        Catch Ex As Exception
            Return Nothing
        End Try
    End Function


    Public Function StringToByteArray(ByVal StringToConvert As String) As Byte()
        Try
            If (IsNothing(StringToConvert)) Then StringToConvert = IDS_NULL_STRING
            Dim intCounter As Integer
            Dim bytArray(StringToConvert.Length()) As Byte
            For intCounter = 0 To StringToConvert.Length - 1 Step 1
                bytArray(intCounter) = Convert.ToByte(Asc(StringToConvert.Substring(intCounter, 1)))
            Next intCounter
            Return bytArray
        Catch Ex As Exception
            Return Nothing
        End Try
    End Function

    Public Function ByteArrayToString(ByRef ByteArrayToConvert() As Byte, ByVal NumberOfBytes As Long) As String
        Try
            If (IsNothing(ByteArrayToConvert)) Then Return IDS_NULL_STRING
            Dim intCounter As Integer
            Dim stgString As String = IDS_NULL_STRING
            For intCounter = 0 To NumberOfBytes - 1 Step 1
                stgString += Chr(Convert.ToInt32(ByteArrayToConvert(intCounter)))
            Next intCounter
            Return stgString
        Catch Ex As Exception
            Return IDS_NULL_STRING
        End Try
    End Function

    Public Function CStyleStringToVbString(ByRef CStyleString As String) As String
        If (IsNothing(CStyleString)) Then Return IDS_NULL_STRING
        Dim intNullCharIndex As Integer = InStr(CStyleString, Chr(0))
        If (intNullCharIndex = 0) Then Return CStyleString.Substring(0, CStyleString.Length())
        If (intNullCharIndex = 1) Then Return IDS_NULL_STRING
        Return CStyleString.Substring(0, intNullCharIndex - 1)
    End Function

    Public Function ParseString(ByVal StringToBeParsed As String, ByVal TokenSeperator As Char) As String()
        Dim ParsedStrings() As String
        Dim lngCurrentIndxInParsingString As Long = 0
        Dim lngNextParsedStringStartIndx As Long = 0
        Dim lngIndxOfNewParsedString As Long = 0
        While (lngCurrentIndxInParsingString < StringToBeParsed.Length)
            If (StringToBeParsed.Chars(lngCurrentIndxInParsingString) = TokenSeperator) Then
                ReDim Preserve ParsedStrings(lngIndxOfNewParsedString)
                ParsedStrings(lngIndxOfNewParsedString) = StringToBeParsed.Substring(lngNextParsedStringStartIndx, lngCurrentIndxInParsingString - lngNextParsedStringStartIndx)
                lngIndxOfNewParsedString += 1
                lngCurrentIndxInParsingString += 1
                lngNextParsedStringStartIndx = lngCurrentIndxInParsingString
            Else
                lngCurrentIndxInParsingString += 1
            End If
        End While

        If (lngNextParsedStringStartIndx < StringToBeParsed.Length) Then
            ReDim Preserve ParsedStrings(lngIndxOfNewParsedString)
            ParsedStrings(lngIndxOfNewParsedString) = StringToBeParsed.Substring(lngNextParsedStringStartIndx, StringToBeParsed.Length - lngNextParsedStringStartIndx)
        End If

        Return ParsedStrings
    End Function


    Public Function FreshString(ByVal Size As Long) As String
        Return New String(" "c, Size)
    End Function

    Public Function SplitFileNameAndPath(ByRef EntireName As String, ByRef Path As String, ByRef FileName As String) As Boolean
        Dim intIndx As Integer
        intIndx = EntireName.LastIndexOf(IDS_FILE_SLASH)
        If intIndx = -1 Then
            Path = EntireName
            FileName = IDS_NULL_STRING
            Return True
        End If
        Try
            Path = EntireName.Substring(0, intIndx)
            FileName = EntireName.Substring(intIndx + 1)
            Return True
        Catch Ex As Exception
            Return False
        End Try
    End Function

#End Region

End Class


Public Module MUint32Utility

    Public Function Uint32Equals(ByVal Arg1 As UInt32, ByVal Arg2 As UInt32) As Boolean
        If (Arg1.CompareTo(Arg2) = 0) Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function Uint32NotEquals(ByVal Arg1 As UInt32, ByVal Arg2 As UInt32) As Boolean
        Return (Not Uint32Equals(Arg1, Arg2))
    End Function

    Public Function Uint32Greater(ByVal Arg1 As UInt32, ByVal Arg2 As UInt32) As Boolean
        If (Arg1.CompareTo(Arg2) > 0) Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function Uint32Lower(ByVal Arg1 As UInt32, ByVal Arg2 As UInt32) As Boolean
        If (Arg1.CompareTo(Arg2) < 0) Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function Uint32GreaterOrEqual(ByVal Arg1 As UInt32, ByVal Arg2 As UInt32) As Boolean
        If (Uint32Greater(Arg1, Arg2) Or Uint32Equals(Arg1, Arg2)) Then Return True
        Return False
    End Function

    Public Function Uint32LessOrEqual(ByVal Arg1 As UInt32, ByVal Arg2 As UInt32) As Boolean
        If (Uint32Lower(Arg1, Arg2) Or Uint32Equals(Arg1, Arg2)) Then Return True
        Return False
    End Function

    Public Function ToUint(ByVal Any As Object) As UInt32
        Return Convert.ToUInt32(Any)
    End Function

End Module

