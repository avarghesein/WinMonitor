


Public Class CXmlUtility : Implements IDisposable

#Region "Private And Protected Fields"

   Private m_UInt64Address As UInt64
   Private m_MaxStringLength As Long = 100

#End Region

#Region "Private API's"
    Private Declare Function RegisterAsXmlUtilityUser Lib "WinMonitorCLIB.1.0.dll" Alias "RegisterAsXmlUtilityUser" () As UInt64
    Private Declare Function RemoveXmlUtilityUser Lib "WinMonitorCLIB.1.0.dll" Alias "RemoveXmlUtilityUser" (ByVal Address As UInt64) As Boolean
    Private Declare Function GetError Lib "WinMonitorCLIB.1.0.dll" Alias "GetErrorX" (ByVal Address As UInt64) As String
    Private Declare Function SetTokenSeperator Lib "WinMonitorCLIB.1.0.dll" Alias "SetTokenSeperatorX" (ByVal Address As UInt64, Optional ByVal TokenSeperator As String = "/") As Boolean
    Private Declare Function CreateNewXmlDocument Lib "WinMonitorCLIB.1.0.dll" Alias "CreateNewXmlDocumentX" (ByVal Address As UInt64, ByVal NewXML_FullFileName As String, ByVal RootNodeName As String) As Boolean
    Private Declare Function OpenXmlDocument Lib "WinMonitorCLIB.1.0.dll" Alias "OpenXmlDocumentX" (ByVal Address As UInt64, ByVal bstrXmlFullFileName As String) As Boolean
    Private Declare Function GetNodeName Lib "WinMonitorCLIB.1.0.dll" Alias "GetNodeNameX" (ByVal Address As UInt64, ByVal NodeName As String, Optional ByVal OfRootNode As Boolean = 1) As Boolean
    Private Declare Function GetNodeTextFromHeirarchy Lib "WinMonitorCLIB.1.0.dll" Alias "GetNodeTextFromHeirarchyX" (ByVal Address As UInt64, ByVal Heirarchy As String, ByVal NodeText As String, Optional ByVal BeginAtRoot As Boolean = 1) As Boolean
    Private Declare Function GetNodeAttributeFromHeirarchy Lib "WinMonitorCLIB.1.0.dll" Alias "GetNodeAttributeFromHeirarchyX" (ByVal Address As UInt64, ByVal Heirarchy As String, ByVal AttributeName As String, ByVal AttributeValue As String, Optional ByVal BeginAtRoot As Boolean = 1) As Boolean
    Private Declare Function SetTextIntoHeirarchy Lib "WinMonitorCLIB.1.0.dll" Alias "SetTextIntoHeirarchyX" (ByVal Address As UInt64, ByVal Heirarchy As String, ByVal Text As String, Optional ByVal BeginAtRoot As Boolean = 1) As Boolean
    Private Declare Function SetNodeTextIntoHeirarchy Lib "WinMonitorCLIB.1.0.dll" Alias "SetNodeTextIntoHeirarchyX" (ByVal Address As UInt64, ByVal Heirarchy As String, ByVal NodeToBeCreated As String, ByVal NodeValue As String, Optional ByVal BeginAtRoot As Boolean = 1) As Boolean
    Private Declare Function SetNodeAttributeIntoHeirarchy Lib "WinMonitorCLIB.1.0.dll" Alias "SetNodeAttributeIntoHeirarchyX" (ByVal Address As UInt64, ByVal Heirarchy As String, ByVal AttributeToBeCreated As String, ByVal AttributeValue As String, Optional ByVal BeginAtRoot As Boolean = 1) As Boolean
    Private Declare Function InsertAllNodesFrom Lib "WinMonitorCLIB.1.0.dll" Alias "InsertAllNodesFromX" (ByVal Address As UInt64, ByVal XML_FileToBeMerged As String, Optional ByVal InsertAtRoot As Boolean = 1) As Boolean
    Private Declare Function InsertSelectedNodesFrom Lib "WinMonitorCLIB.1.0.dll" Alias "InsertSelectedNodesFromX" (ByVal Address As UInt64, Optional ByVal InsertAtRoot As Boolean = 1) As Boolean
    Private Declare Function MoveToChildByIndex Lib "WinMonitorCLIB.1.0.dll" Alias "MoveToChildByIndexX" (ByVal Address As UInt64, ByVal ParentNodeHeirarchy As String, Optional ByVal ChildIndex As Long = 0, Optional ByVal BeginAtRoot As Boolean = 1) As Boolean
    Private Declare Function MoveToChildByName Lib "WinMonitorCLIB.1.0.dll" Alias "MoveToChildByNameX" (ByVal Address As UInt64, ByVal ParentNodeHeirarchy As String, ByVal ChildName As String, Optional ByVal BeginAtRoot As Boolean = 1) As Boolean
    Private Declare Function MoveToBrother Lib "WinMonitorCLIB.1.0.dll" Alias "MoveToBrotherX" (ByVal Address As UInt64, Optional ByVal Older As Boolean = 0) As Boolean
    Private Declare Function MoveToParent Lib "WinMonitorCLIB.1.0.dll" Alias "MoveToParentX" (ByVal Address As UInt64) As Boolean
    Private Declare Function ResetSearchPointerToRoot Lib "WinMonitorCLIB.1.0.dll" Alias "ResetSearchPointerToRootX" (ByVal Address As UInt64) As Boolean
    Private Declare Function NumberOfBrothers Lib "WinMonitorCLIB.1.0.dll" Alias "NumberOfBrothersX" (ByVal Address As UInt64) As Integer
    Private Declare Function NumberOfChildren Lib "WinMonitorCLIB.1.0.dll" Alias "NumberOfChildrenX" (ByVal Address As UInt64, Optional ByVal OfRootNode As Boolean = 1) As Integer
    Private Declare Function SetForceCreate Lib "WinMonitorCLIB.1.0.dll" Alias "SetForceCreateX" (ByVal Address As UInt64, Optional ByVal MakeOnState As Boolean = False) As Boolean
#End Region

#Region "Constructor/Destructor"

    Public Sub New()
        m_UInt64Address = RegisterAsXmlUtilityUser()
    End Sub

    Public Sub dispose() Implements IDisposable.Dispose
        RemoveXmlUtilityUser(m_UInt64Address)
    End Sub

#End Region

#Region "Public Properties"

    Public WriteOnly Property MaximumStringLength() As Long
        Set(ByVal Value As Long)
            m_MaxStringLength = Value
        End Set
    End Property

#End Region

#Region "Public Methods"

    Public Function GetError() As String
        Dim objNwUtility As New CStringUtility()
        Return objNwUtility.CStyleStringToVbString(GetError(m_UInt64Address))
    End Function

    Public Function SetTokenSeperator(Optional ByVal TokenSeperator As String = "/") As Boolean
        Return SetTokenSeperator(m_UInt64Address, TokenSeperator)
    End Function

    Public Function CreateNewXmlDocument(ByVal NewXML_FullFileName As String, ByVal RootNodeName As String) As Boolean
        Return CreateNewXmlDocument(m_UInt64Address, NewXML_FullFileName, RootNodeName)
    End Function

    Public Function OpenXmlDocument(ByVal XmlFullFileName As String) As Boolean
        Return OpenXmlDocument(m_UInt64Address, XmlFullFileName)
    End Function

    Public Function GetNodeName(ByRef NodeName As String, Optional ByVal OfRootNode As Boolean = 1) As Boolean
        Dim objNwUtility As New CStringUtility()
        Dim strNodeName As String = objNwUtility.FreshString(m_MaxStringLength)
        Dim boolOk As Boolean = GetNodeName(m_UInt64Address, strNodeName, OfRootNode)
        NodeName = objNwUtility.CStyleStringToVbString(strNodeName)
        Return boolOk
    End Function

    Public Function GetNodeTextFromHeirarchy(ByVal Heirarchy As String, ByRef NodeText As String, Optional ByVal BeginAtRoot As Boolean = 1) As Boolean
        Dim objNwUtility As New CStringUtility()
        Dim strNodeText As String = objNwUtility.FreshString(m_MaxStringLength)
        Dim boolOk As Boolean = GetNodeTextFromHeirarchy(m_UInt64Address, Heirarchy, strNodeText, BeginAtRoot)
        NodeText = objNwUtility.CStyleStringToVbString(strNodeText)
        Return boolOk
    End Function

    Public Function GetNodeAttributeFromHeirarchy(ByVal Heirarchy As String, ByVal AttributeName As String, ByRef AttributeValue As String, Optional ByVal BeginAtRoot As Boolean = 1) As Boolean
        Dim objNwUtility As New CStringUtility()
        Dim strAttributeValue As String = objNwUtility.FreshString(m_MaxStringLength)
        Dim boolOk As Boolean = GetNodeAttributeFromHeirarchy(m_UInt64Address, Heirarchy, AttributeName, strAttributeValue, BeginAtRoot)
        AttributeValue = objNwUtility.CStyleStringToVbString(strAttributeValue)
        Return boolOk
    End Function

    Public Function SetTextIntoHeirarchy(ByVal Heirarchy As String, ByVal Text As String, Optional ByVal BeginAtRoot As Boolean = 1) As Boolean
        Return SetTextIntoHeirarchy(m_UInt64Address, Heirarchy, Text, BeginAtRoot)
    End Function

    Public Function SetNodeTextIntoHeirarchy(ByVal Heirarchy As String, ByVal NodeToBeCreated As String, ByVal NodeValue As String, Optional ByVal BeginAtRoot As Boolean = 1) As Boolean
        Return SetNodeTextIntoHeirarchy(m_UInt64Address, Heirarchy, NodeToBeCreated, NodeValue, BeginAtRoot)
    End Function

    Public Function SetNodeAttributeIntoHeirarchy(ByVal Heirarchy As String, ByVal AttributeToBeCreated As String, ByVal AttributeValue As String, Optional ByVal BeginAtRoot As Boolean = 1) As Boolean
        Return SetNodeAttributeIntoHeirarchy(m_UInt64Address, Heirarchy, AttributeToBeCreated, AttributeValue, BeginAtRoot)
    End Function

    Public Function InsertAllNodesFrom(ByVal XML_FileToBeMerged As String, Optional ByVal InsertAtRoot As Boolean = 1) As Boolean
        Return InsertAllNodesFrom(m_UInt64Address, XML_FileToBeMerged, InsertAtRoot)
    End Function

    Public Function InsertSelectedNodesFrom(Optional ByVal InsertAtRoot As Boolean = 1) As Boolean
        Return InsertSelectedNodesFrom(m_UInt64Address, InsertAtRoot)
    End Function

    Public Function MoveToChildByIndex(ByVal ParentNodeHeirarchy As String, Optional ByVal ChildIndex As Long = 0, Optional ByVal BeginAtRoot As Boolean = 1) As Boolean
        Return MoveToChildByIndex(m_UInt64Address, ParentNodeHeirarchy, ChildIndex, BeginAtRoot)
    End Function

    Public Function MoveToChildByName(ByVal ParentNodeHeirarchy As String, ByVal ChildName As String, Optional ByVal BeginAtRoot As Boolean = 1) As Boolean
        Return MoveToChildByName(m_UInt64Address, ParentNodeHeirarchy, ChildName, BeginAtRoot)
    End Function

    Public Function MoveToBrother(Optional ByVal Older As Boolean = 0) As Boolean
        Return MoveToBrother(m_UInt64Address, Older)
    End Function

    Public Function MoveToParent() As Boolean
        Return MoveToParent(m_UInt64Address)
    End Function

    Public Function ResetSearchPointerToRoot() As Boolean
        Return ResetSearchPointerToRoot(m_UInt64Address)
    End Function

    Public Function NumberOfBrothers() As Integer
        Return NumberOfBrothers(m_UInt64Address)
    End Function

    Public Function NumberOfChildren(Optional ByVal OfRootNode As Boolean = 1) As Integer
        Return NumberOfChildren(m_UInt64Address, OfRootNode)
    End Function

    Public Function SetForceCreate(Optional ByVal MakeOnState As Boolean = False) As Boolean
        Return SetForceCreate(m_UInt64Address, MakeOnState)
    End Function

#End Region

End Class


Public Class CTcpNetworkMonitorInterface

#Region "Private API's"

    Private Declare Function GetLastError Lib "WinMonitorCLIB.1.0.dll" Alias "GenericNetworkLastError" () As Integer
    Private Declare Function InitializeNetworkInterface Lib "WinMonitorCLIB.1.0.dll" Alias "LoadNetworkLibraryAndVersion" () As Boolean
    Private Declare Function CleanUpNetworkInterface Lib "WinMonitorCLIB.1.0.dll" Alias "CleanUpNetworkLibrary" () As Boolean

#End Region

#Region "Public Methods"

    Public Function LastError() As Integer
        Return GetLastError()
    End Function

    Public Function Initialize() As Boolean
        Return InitializeNetworkInterface()
    End Function

    Public Function CleanUp() As Boolean
        Return CleanUpNetworkInterface()
    End Function

#End Region

End Class


Public Class CTcpNetworkMonitorConnection : Implements IDisposable

#Region "Private And Protected Fields"

    Private m_UInt64Address As UInt64
    Private m_bytMaxNumberOfBytesToReceive As Long = 5000

    Protected Friend Property Address() As UInt64
        Get
            Return m_UInt64Address
        End Get

        Set(ByVal AddressCopy As UInt64)
            m_UInt64Address = AddressCopy
        End Set
    End Property

#End Region

#Region "Private API's"

    Private Declare Function RegisterAsNwConnectionUser Lib "WinMonitorCLIB.1.0.dll" Alias "RegisterAsNwConnectionUser" () As UInt64
    Private Declare Function RemoveNwConnectionUser Lib "WinMonitorCLIB.1.0.dll" Alias "RemoveNwConnectionUser" (ByVal Address As UInt64) As Boolean
    Private Declare Function LastError Lib "WinMonitorCLIB.1.0.dll" Alias "ConnectionLastError" (ByVal Address As UInt64) As Integer
    Private Declare Function ConnectTo Lib "WinMonitorCLIB.1.0.dll" Alias "ConnectTo" (ByVal Address As UInt64, ByVal ServerIP As String, ByVal ServerPort As UInt32) As Boolean
    Private Declare Function GetIPandPort Lib "WinMonitorCLIB.1.0.dll" Alias "GetIPandPort" (ByVal Address As UInt64, ByVal OfRemoteHost As Boolean, ByVal IP As String, ByRef Port As UInt32) As Boolean
    Private Declare Function SendByteStream Lib "WinMonitorCLIB.1.0.dll" Alias "SendByteStream" (ByVal Address As UInt64, ByVal MessageBuffer() As Byte, ByVal NumberOfBytesToSend As UInt32, ByRef NumberOfBytesSended As UInt32) As Boolean
    Private Declare Function ReceiveByteStream Lib "WinMonitorCLIB.1.0.dll" Alias "ReceiveByteStream" (ByVal Address As UInt64, ByVal MessageBuffer() As Byte, ByVal NumberOfBytesToReceive As UInt32, ByRef NumberOfBytesReceived As UInt32) As Boolean
    Private Declare Function Disconnect Lib "WinMonitorCLIB.1.0.dll" Alias "Disconnect" (ByVal Address As UInt64) As Boolean

#End Region

#Region "Constructor/Destructor"

    Protected Friend Sub New(ByVal AddressCopy As UInt64)
        m_UInt64Address = AddressCopy
    End Sub

    Public Sub New()
        m_UInt64Address = RegisterAsNwConnectionUser()
    End Sub

    Public Sub dispose() Implements IDisposable.Dispose
        RemoveNwConnectionUser(m_UInt64Address)
    End Sub

#End Region

#Region "Private Methods"

    Private Function prvtReceiveString(ByVal NumberOfCharsToReceive As Long) As String
        Dim NwUtility As New CStringUtility()
        Dim lngBytReceived As Long
        Dim bytReceive(NumberOfCharsToReceive) As Byte
        If (ReceiveByteStream(bytReceive, NumberOfCharsToReceive, lngBytReceived) = False) Then
            Return IDS_NULL_STRING
        Else
            Return NwUtility.ByteArrayToString(bytReceive, lngBytReceived)
        End If
    End Function

#End Region

#Region "Public Properties"

    Public WriteOnly Property MaximumStringLength() As Long
        Set(ByVal Value As Long)
            m_bytMaxNumberOfBytesToReceive = Value
        End Set
    End Property

#End Region

#Region "Public Methods"


    Public Function LastError() As Integer
        Return LastError(m_UInt64Address)
    End Function

    Public Function ConnectTo(ByVal ServerIP As String, ByVal ServerPort As Integer) As Boolean
        Dim uint32ServerPort As UInt32 = System.Convert.ToUInt32(ServerPort)
        Return ConnectTo(m_UInt64Address, ServerIP, uint32ServerPort)
    End Function

    Public Function GetIPandPort(ByVal OfRemoteHost As Boolean, ByRef IP As String, ByRef Port As Integer) As Boolean
        Dim objNwUtility As New CStringUtility()
        Dim uint32Port As UInt32
        Dim strIp As String = objNwUtility.FreshString(20)
        Dim boolOk As Boolean = GetIPandPort(m_UInt64Address, OfRemoteHost, strIp, uint32Port)
        IP = objNwUtility.CStyleStringToVbString(strIp)
        Port = System.Convert.ToInt32(uint32Port)
        Return boolOk
    End Function

    Public Function SendByteStream(ByRef MessageBuffer() As Byte, ByVal NumberOfBytesToSend As Long, ByRef NumberOfBytesSended As Long) As Boolean
        Dim uint32LenToSend As UInt32 = System.Convert.ToUInt32(NumberOfBytesToSend)
        Dim uint32LenSended As UInt32
        Dim boolOk As Boolean = SendByteStream(m_UInt64Address, MessageBuffer, uint32LenToSend, uint32LenSended)
        NumberOfBytesSended = System.Convert.ToInt64(uint32LenSended)
        Return boolOk
    End Function

    Public Function SendString(ByVal Message As String) As Boolean
        Dim NwUtility As New CStringUtility()
        Dim stgForNothing As Long
        Return SendByteStream(NwUtility.StringToByteArray(Message), Message.Length(), stgForNothing)
    End Function

    Public Function ReceiveString() As String
        Return prvtReceiveString(m_bytMaxNumberOfBytesToReceive)
    End Function

    Public Function ReceiveString(ByVal NumberOfCharsToReceive As Long) As String
        Return prvtReceiveString(NumberOfCharsToReceive)
    End Function

    Public Function ReceiveByteStream(ByRef MessageBuffer() As Byte, ByVal NumberOfBytesToReceive As Long, ByRef NumberOfBytesReceived As Long) As Boolean
        Dim uint32LenToReceive As UInt32 = System.Convert.ToUInt32(NumberOfBytesToReceive)
        Dim uint32LenReceived As UInt32
        Dim boolOk As Boolean = ReceiveByteStream(m_UInt64Address, MessageBuffer, uint32LenToReceive, uint32LenReceived)
        NumberOfBytesReceived = System.Convert.ToInt64(uint32LenReceived)
        Return boolOk
    End Function

    Public Function Disconnect() As Boolean
        Return Disconnect(m_UInt64Address)
    End Function

#End Region

End Class


Public Class CTcpNetworkMonitorListener : Implements IDisposable

#Region "Private And Protected Fields"

    Private m_UInt64Address As UInt64
#End Region

#Region "Private API's"

    Private Declare Function RegisterAsNwListenerUser Lib "WinMonitorCLIB.1.0.dll" Alias "RegisterAsNwListenerUser" () As UInt64
    Private Declare Function RegisterAsNwListenerUser Lib "WinMonitorCLIB.1.0.dll" Alias "RegisterAsNwListenerUserEx" (ByVal IP As String, ByVal Port As UInt32) As UInt64
    Private Declare Function RegisterAsNwListenerUser Lib "WinMonitorCLIB.1.0.dll" Alias "RegisterAsNwListenerUserExx" (ByVal LocalHost As Boolean, ByVal Port As UInt32) As UInt64
    Private Declare Function RemoveNwListenerUser Lib "WinMonitorCLIB.1.0.dll" Alias "RemoveNwListenerUser" (ByVal Address As UInt64) As Boolean
    Private Declare Function RemoveClient Lib "WinMonitorCLIB.1.0.dll" Alias "RemoveClient" (ByVal Address As UInt64, ByRef ClientConnectionAddress As UInt64) As Boolean

    Private Declare Function ListenAt Lib "WinMonitorCLIB.1.0.dll" Alias "ListenAt" (ByVal Address As UInt64, ByVal IpAddress As String, ByVal Port As UInt32) As Boolean
    Private Declare Function ListenAt Lib "WinMonitorCLIB.1.0.dll" Alias "ListenAtEx" (ByVal Address As UInt64, ByVal LocalHost As Boolean, ByVal Port As UInt32) As Boolean
    Private Declare Function LastError Lib "WinMonitorCLIB.1.0.dll" Alias "ListenLastError" (ByVal Address As UInt64) As Integer
    Private Declare Function SetListenState Lib "WinMonitorCLIB.1.0.dll" Alias "SetListenState" (ByVal Address As UInt64, Optional ByVal OnState As Boolean = True) As Boolean
    Private Declare Function GetListenFlag Lib "WinMonitorCLIB.1.0.dll" Alias "GetListenFlag" (ByVal Address As UInt64) As Boolean
    Private Declare Function RetrieveClient Lib "WinMonitorCLIB.1.0.dll" Alias "RetrieveClient" (ByVal Address As UInt64, ByRef ClientConnectionAddress As UInt64) As Boolean
    Private Declare Function Disconnect Lib "WinMonitorCLIB.1.0.dll" Alias "DisconnectListenConnection" (ByVal Address As UInt64) As Boolean

#End Region

#Region "Constructors/Destructor"

    Public Sub New()
        m_UInt64Address = RegisterAsNwListenerUser()
    End Sub

    Public Sub New(ByVal IP As String, ByVal Port As Integer)
        Dim uint32Port As UInt32 = System.Convert.ToUInt32(Port)
        m_UInt64Address = RegisterAsNwListenerUser(IP, uint32Port)
    End Sub

    Public Sub New(ByVal LocalHost As Boolean, ByVal Port As Integer)
        Dim uint32Port As UInt32 = System.Convert.ToUInt32(Port)
        m_UInt64Address = RegisterAsNwListenerUser(LocalHost, uint32Port)
    End Sub

    Public Sub dispose() Implements IDisposable.Dispose
        RemoveNwListenerUser(m_UInt64Address)
    End Sub

#End Region

#Region "Public Methods"

    Public Function ListenAt(ByVal IpAddress As String, ByVal Port As Integer) As Boolean
        Dim uint32Port As UInt32 = System.Convert.ToUInt32(Port)
        Return ListenAt(m_UInt64Address, IpAddress, uint32Port)
    End Function


    Public Function ListenAt(ByVal LocalHost As Boolean, ByVal Port As Integer) As Boolean
        Dim uint32Port As UInt32 = System.Convert.ToUInt32(Port)
        Return ListenAt(m_UInt64Address, LocalHost, uint32Port)
    End Function

    Public Function LastError() As Integer
        Return LastError(m_UInt64Address)
    End Function

    Public Function SetListenState(Optional ByVal OnState As Boolean = True) As Boolean
        Return SetListenState(m_UInt64Address, OnState)
    End Function

    Public Function GetListenFlag() As Boolean
        Return GetListenFlag(m_UInt64Address)
    End Function

    Public Function RetrieveClient(ByRef ClientConnection As CTcpNetworkMonitorConnection) As Boolean
        Dim uint64ClientId As UInt64
        Dim boolIsRetrieved As Boolean = RetrieveClient(m_UInt64Address, uint64ClientId)
        ClientConnection = New CTcpNetworkMonitorConnection(uint64ClientId)
        Return boolIsRetrieved
    End Function

    Public Function RemoveClient(ByRef ClientConnection As CTcpNetworkMonitorConnection) As Boolean
        Dim uint64ClientId As UInt64
        Dim boolIsRetrieved As Boolean = RemoveClient(m_UInt64Address, uint64ClientId)
        ClientConnection = New CTcpNetworkMonitorConnection(uint64ClientId)
        Return boolIsRetrieved
    End Function

    Public Function Disconnect() As Boolean
        Return Disconnect(m_UInt64Address)
    End Function

#End Region

End Class


Public Class CScreenMonitorBase : Implements IDisposable

#Region "Private And Protected Fields"

    Private m_UInt64Address As UInt64

    Protected Friend Property Address() As UInt64
        Get
            Return m_UInt64Address
        End Get

        Set(ByVal AddressCopy As UInt64)
            m_UInt64Address = AddressCopy
        End Set
    End Property

#End Region

#Region "Private API's"

    Private Declare Function RegisterAsScreenMonitorUser Lib "WinMonitorCLIB.1.0.dll" Alias "RegisterAsScreenMonitorUser" (Optional ByVal Compression As Byte = MScreenMonitorDeclarations.SCREEN_MONITOR_COMPRESS_NIL) As UInt64
    Private Declare Function RemoveScreenMonitorUser Lib "WinMonitorCLIB.1.0.dll" Alias "RemoveScreenMonitorUser" (ByVal Address As UInt64) As Boolean
    Private Declare Function SetCompression Lib "WinMonitorCLIB.1.0.dll" Alias "SetCompression" (ByVal Address As UInt64, ByVal Compression As Byte) As Boolean
    Private Declare Function SetType Lib "WinMonitorCLIB.1.0.dll" Alias "SetType" (ByVal Address As UInt64, ByVal TransferItemType As Byte) As Boolean
    Private Declare Function CaptureDesktopImage Lib "WinMonitorCLIB.1.0.dll" Alias "CaptureDesktopImage" (ByVal Address As UInt64) As Boolean
    Private Declare Function CaptureWindowImage Lib "WinMonitorCLIB.1.0.dll" Alias "CaptureWindowImage" (ByVal Address As UInt64, ByVal hWnd As System.IntPtr) As Boolean
    Private Declare Function SaveBitmapToFile Lib "WinMonitorCLIB.1.0.dll" Alias "SaveBitmapToFile" (ByVal Address As UInt64, ByVal FileName As String, Optional ByVal IsCompress As Boolean = False) As Boolean
    Private Declare Function ReplaceDesktopImage Lib "WinMonitorCLIB.1.0.dll" Alias "ReplaceDesktopImage" (ByVal Address As UInt64) As Boolean
    Private Declare Function ReplaceWindowImage Lib "WinMonitorCLIB.1.0.dll" Alias "ReplaceWindowImage" (ByVal Address As UInt64, ByVal hWnd As System.IntPtr) As Boolean
    Private Declare Function LoadBitmapFromFile Lib "WinMonitorCLIB.1.0.dll" Alias "LoadBitmapFromFile" (ByVal Address As UInt64, ByVal FileName As String, Optional ByVal IsCompressed As Boolean = False) As Boolean

#End Region

#Region "Constructors/Destructor"

    Public Sub New(Optional ByVal Compression As Byte = MScreenMonitorDeclarations.SCREEN_MONITOR_COMPRESS_NIL)
        m_UInt64Address = RegisterAsScreenMonitorUser(Compression)
    End Sub

    Public Sub dispose() Implements IDisposable.Dispose
        RemoveScreenMonitorUser(m_UInt64Address)
    End Sub

#End Region

#Region "Public functions"

    Public Function SetCompression(ByVal Compression As Byte) As Boolean
        Return SetCompression(m_UInt64Address, Compression)
    End Function

    Public Function SetType(ByVal TransferItemType As Byte) As Boolean
        Return SetType(m_UInt64Address, TransferItemType)
    End Function

    Public Function CaptureDesktopImage() As Boolean
        Return CaptureDesktopImage(m_UInt64Address)
    End Function

    Public Function CaptureWindowImage(ByVal hWnd As System.IntPtr) As Boolean
        Return CaptureWindowImage(m_UInt64Address, hWnd)
    End Function

    Public Function SaveBitmapToFile(ByVal FileName As String, Optional ByVal IsCompress As Boolean = False) As Boolean
        Return SaveBitmapToFile(m_UInt64Address, FileName, IsCompress)
    End Function

    Public Function ReplaceDesktopImage() As Boolean
        Return ReplaceDesktopImage(m_UInt64Address)
    End Function

    Public Function ReplaceWindowImage(ByVal hWnd As System.IntPtr) As Boolean
        Return ReplaceWindowImage(m_UInt64Address, hWnd)
    End Function

    Public Function LoadBitmapFromFile(ByVal FileName As String, Optional ByVal IsCompressed As Boolean = False) As Boolean
        Return LoadBitmapFromFile(m_UInt64Address, FileName, IsCompressed)
    End Function

#End Region

End Class


Public Class CScreenMonitorMessenger : Implements IDisposable

#Region "Private And Protected Fields"

    Private m_UInt64Address As UInt64

    Protected Friend Property Address() As UInt64
        Get
            Return m_UInt64Address
        End Get

        Set(ByVal AddressCopy As UInt64)
            m_UInt64Address = AddressCopy
        End Set
    End Property


#End Region

#Region "Private API's"

    Private Declare Function RegisterAsCScreenMonitorMessenger Lib "WinMonitorCLIB.1.0.dll" Alias "RegisterAsCScreenMonitorMessenger" () As UInt64
    Private Declare Function RemoveScreenMonitorMessenger Lib "WinMonitorCLIB.1.0.dll" Alias "RemoveScreenMonitorMessenger" (ByVal Address As UInt64) As Boolean
    Private Declare Function ScreenMonitorMessengerSetMessageType Lib "WinMonitorCLIB.1.0.dll" Alias "ScreenMonitorMessengerSetMessageType" (ByVal Address As UInt64, ByVal MsgType As Byte, Optional ByVal MsgArgs() As Byte = Nothing, Optional ByVal ArgLen As Integer = 0) As Boolean

#End Region

#Region "Constructors/Destructor"

    Public Sub New()
        m_UInt64Address = RegisterAsCScreenMonitorMessenger()
    End Sub

    Public Sub dispose() Implements IDisposable.Dispose
        RemoveScreenMonitorMessenger(m_UInt64Address)
    End Sub

#End Region

#Region "Public Methods"

    Public Function SetMessageType(ByVal MsgType As Byte, Optional ByVal MsgArgs() As Byte = Nothing, Optional ByVal ArgLen As Integer = 0) As Boolean
        Return ScreenMonitorMessengerSetMessageType(m_UInt64Address, MsgType, MsgArgs, ArgLen)
    End Function

#End Region

End Class


Public Class CExeEditor : Implements IDisposable

#Region "Private And Protected Fields"
    Private m_UInt64Address As UInt64
#End Region

#Region "Private API's"

    Private Declare Function RegisterAsExeEditor Lib "WinMonitorCLIB.1.0.dll" Alias "RegisterAsExeEditor" () As UInt64
    Private Declare Function RegisterAsExeEditorEx Lib "WinMonitorCLIB.1.0.dll" Alias "RegisterAsExeEditorEx" (ByVal exeNameWithFullPath As String) As UInt64
    Private Declare Function RegisterAsExeEditorExx Lib "WinMonitorCLIB.1.0.dll" Alias "RegisterAsExeEditorExx" (ByVal exeNameWithFullPath As String, ByVal ResourceName As String, ByVal ResourceType As String) As UInt64
    Private Declare Function RemoveExeEditor Lib "WinMonitorCLIB.1.0.dll" Alias "RemoveExeEditor" (ByVal Address As UInt64) As Boolean

    Private Declare Function ErrorFlag Lib "WinMonitorCLIB.1.0.dll" Alias "ErrorFlag" (ByVal Address As UInt64) As Integer
    Private Declare Function SetExeFileName Lib "WinMonitorCLIB.1.0.dll" Alias "SetExeFileName" (ByVal Address As UInt64, ByVal exeNameWithFullPath As String) As Boolean
    Private Declare Function SetResourceInfo Lib "WinMonitorCLIB.1.0.dll" Alias "SetResourceInfo" (ByVal Address As UInt64, ByVal ResourceName As String, ByVal ResourceType As String) As Boolean
    Private Declare Function EmbedFileToexe Lib "WinMonitorCLIB.1.0.dll" Alias "EmbedFileToexe" (ByVal Address As UInt64, ByVal FileNamewithFullPath As String) As Boolean
    Private Declare Function ExtractFileFromExe Lib "WinMonitorCLIB.1.0.dll" Alias "ExtractFileFromExe" (ByVal Address As UInt64, ByVal FileNameWithExtnToSave As String) As Boolean

#End Region

#Region "Constructors/Destructor"

    Public Sub New()
        m_UInt64Address = RegisterAsExeEditor()
    End Sub

    Public Sub New(ByVal exeNameWithFullPath As String)
        m_UInt64Address = RegisterAsExeEditorEx(exeNameWithFullPath)
    End Sub

    Public Sub New(ByVal exeNameWithFullPath As String, ByVal ResourceName As String, ByVal ResourceType As String)
        m_UInt64Address = RegisterAsExeEditorExx(exeNameWithFullPath, ResourceName, ResourceType)
    End Sub

    Public Sub dispose() Implements IDisposable.Dispose
        RemoveExeEditor(m_UInt64Address)
    End Sub

#End Region

#Region "Public functions"

    Public Function ErrorFlag() As Integer
        Return ErrorFlag(m_UInt64Address)
    End Function

    Public Function SetExeFileName(ByVal exeNameWithFullPath As String) As Boolean
        Return SetExeFileName(m_UInt64Address, exeNameWithFullPath)
    End Function

    Public Function SetResourceInfo(ByVal ResourceName As String, ByVal ResourceType As String) As Boolean
        Return SetResourceInfo(m_UInt64Address, ResourceName, ResourceType)
    End Function

    Public Function EmbedFileToexe(ByVal FileNamewithFullPath As String) As Boolean
        Return EmbedFileToexe(m_UInt64Address, FileNamewithFullPath)
    End Function

    Public Function ExtractFileFromExe(ByVal FileNameWithExtnToSave As String) As Boolean
        Return ExtractFileFromExe(m_UInt64Address, FileNameWithExtnToSave)
    End Function

#End Region

End Class


Public Class CCommandMonitorClient : Implements IDisposable

#Region "Private And Protected Fields"

    Private m_UInt64Address As UInt64

    Protected Friend Property Address() As UInt64
        Get
            Return m_UInt64Address
        End Get

        Set(ByVal AddressCopy As UInt64)
            m_UInt64Address = AddressCopy
        End Set
    End Property

#End Region

#Region "Private API's"
    Private Declare Function RegisterAsCommandMonitorClient Lib "WinMonitorCLIB.1.0.dll" Alias "RegisterAsCommandMonitorClient" (ByVal CommandType As UInt32, ByVal CommandArgs As String, ByVal Compression As UInt32, Optional ByVal ResultSeperator As Char = Chr(10)) As UInt64
    Private Declare Function RemoveCommandMonitorClient Lib "WinMonitorCLIB.1.0.dll" Alias "RemoveCommandMonitorClient" (ByVal Address As UInt64) As Boolean
    Private Declare Function GetResultantType Lib "WinMonitorCLIB.1.0.dll" Alias "CMCGetResultantType" (ByVal Address As UInt64) As UInt32
    Private Declare Function SetCommand Lib "WinMonitorCLIB.1.0.dll" Alias "CMCSetCommand" (ByVal Address As UInt64, ByVal CommandType As UInt32, ByVal CommandArgs As String) As Boolean
    Private Declare Function SetFakeMessage Lib "WinMonitorCLIB.1.0.dll" Alias "CMCSetFakeMessage" (ByVal Address As UInt64, ByVal MsgTxt As String, ByVal MsgCap As String, ByVal MsgType As Byte) As Boolean
    Private Declare Function SetCompression Lib "WinMonitorCLIB.1.0.dll" Alias "CMCSetCompression" (ByVal Address As UInt64, ByVal Compression As UInt32) As Boolean
    Private Declare Function SetTokenSeperator Lib "WinMonitorCLIB.1.0.dll" Alias "CMCSetTokenSeperator" (ByVal Address As UInt64, Optional ByVal ResultSeperator As Char = Chr(10)) As Boolean
    Private Declare Function GetResult Lib "WinMonitorCLIB.1.0.dll" Alias "CMCGetResult" (ByVal Address As UInt64, ByVal bytptrResult() As Byte, ByRef ResultLength As UInt32) As Boolean
    Private Declare Function SaveRemoteFile Lib "WinMonitorCLIB.1.0.dll" Alias "CMCSaveRemoteFile" (ByVal Address As UInt64, ByVal FileName As String, ByVal DstRootDirPath As String) As Boolean
    Private Declare Function SetRemoteServerCompression Lib "WinMonitorCLIB.1.0.dll" Alias "CMCSetRemoteServerCompression" (ByVal Address As UInt64, ByVal uint32Compression As UInt32) As Boolean
    Private Declare Function SetProcessPriority Lib "WinMonitorCLIB.1.0.dll" Alias "CMCSetProcessPriority" (ByVal Address As UInt64, ByVal ProcessID As UInt32, ByVal PriorityLevel As Byte) As Boolean
    Private Declare Function SetKillProcess Lib "WinMonitorCLIB.1.0.dll" Alias "CMCSetKillProcess" (ByVal Address As UInt64, ByVal ProcessID As UInt32) As Boolean
    Private Declare Function SetSystemDownMode Lib "WinMonitorCLIB.1.0.dll" Alias "CMCSetSystemDownMode" (ByVal Address As UInt64, ByVal ForceDown As Boolean) As Boolean
    Private Declare Function SetFileUpLoad Lib "WinMonitorCLIB.1.0.dll" Alias "CMCSetFileUpLoad" (ByVal Address As UInt64, ByVal SrcRoot As String, ByVal FileName As String, ByVal TarRoot As String) As Boolean
#End Region

#Region "Constructors/Destructor"

    Public Sub New(ByVal CommandType As UInt32, ByVal CommandArgs As String, ByVal Compression As UInt32, Optional ByVal ResultSeperator As Char = Chr(10))
        m_UInt64Address = RegisterAsCommandMonitorClient(CommandType, CommandArgs, Compression, ResultSeperator)
    End Sub

    Public Sub dispose() Implements IDisposable.Dispose
        RemoveCommandMonitorClient(m_UInt64Address)
    End Sub

#End Region

#Region "Public functions"

    Public Function SetFileUpLoad(ByVal SrcRoot As String, ByVal FileName As String, ByVal TarRoot As String) As Boolean
        Return SetFileUpLoad(m_UInt64Address, SrcRoot, FileName, TarRoot)
    End Function

    Public Function SetSystemDownMode(Optional ByVal ForceDown As Boolean = False) As Boolean
        Return SetSystemDownMode(m_UInt64Address, ForceDown)
    End Function

    Public Function SetProcessPriority(ByVal ProcessID As Long, ByVal PriorityLevel As Byte) As Boolean
        Return SetProcessPriority(m_UInt64Address, Convert.ToUInt32(ProcessID), PriorityLevel)
    End Function

    Public Function SetKillProcess(ByVal ProcessID As Long) As Boolean
        Return SetKillProcess(m_UInt64Address, Convert.ToUInt32(ProcessID))
    End Function

    Public Function SetRemoteServerCompression(ByVal uint32Compression As UInt32) As Boolean
        Return SetRemoteServerCompression(m_UInt64Address, uint32Compression)
    End Function

    Public Function SaveRemoteFile(ByVal FileName As String, ByVal DestinationPath As String) As Boolean
        Return SaveRemoteFile(m_UInt64Address, FileName, DestinationPath)
    End Function

    Public Function GetResultantType() As UInt32
        Return GetResultantType(m_UInt64Address)
    End Function

    Public Function SetCommand(ByVal CommandType As UInt32, ByVal CommandArgs As String) As Boolean
        Return SetCommand(m_UInt64Address, CommandType, CommandArgs)
    End Function

    Public Function SetFakeMessage(ByVal MsgTxt As String, ByVal MsgCap As String, ByVal MsgType As Byte) As Boolean
        Return SetFakeMessage(m_UInt64Address, MsgTxt, MsgCap, MsgType)
    End Function

    Public Function SetCompression(ByVal Compression As UInt32) As Boolean
        Return SetCompression(m_UInt64Address, Compression)
    End Function

    Public Function SetTokenSeperator(Optional ByVal ResultSeperator As Char = Chr(10)) As Boolean
        Return SetTokenSeperator(m_UInt64Address, ResultSeperator)
    End Function

    Public Function GetResult(ByRef bytmappedstrResult() As Byte, ByRef ResultLength As Long) As Boolean
        Dim ResultTmp(1) As Byte
        Dim ResLen As UInt32 = Convert.ToUInt32(0)
        Dim boolOk As Boolean = GetResult(m_UInt64Address, ResultTmp, ResLen)
        If Not boolOk Then Return False
        ResultTmp = Nothing
        ResultLength = Convert.ToDecimal(ResLen)
        If (ResultLength <= 0) Then Return False
        Try
            Dim Result() As Byte
            'System.GC.Collect()
            ReDim Result(ResultLength) '+ IIf(ResultLength < 4, 4 Mod ResultLength, ResultLength Mod 4))
            ResLen = Convert.ToUInt32(1)
            boolOk = GetResult(m_UInt64Address, Result, ResLen)
            bytmappedstrResult = Nothing
            bytmappedstrResult = Result
            Return boolOk
        Catch ex As Exception
            Return False
        End Try

    End Function

    Public Function GetResult(ByRef ResultLength As Long) As Byte()
        Dim bytRes() As Byte
        GetResult(bytRes, ResultLength)
        Return bytRes
    End Function

    Public Function GetResult() As String
        Dim Res As String : Dim leng As Long
        If (GetResult(Res, leng)) Then Return Res
        Return IDS_NULL_STRING
    End Function

    Public Function GetResult(ByRef StringResult As String, ByRef ResultLength As Long) As Boolean
        Dim Result() As Byte
        Dim ResLen As Long
        If (Not GetResult(Result, ResLen)) Then Return False
        ResultLength = ResLen
        Dim uty As New CStringUtility()
        StringResult = uty.ByteArrayToString(Result, ResultLength)
        uty = Nothing : Result = Nothing
        Return True
    End Function

    Public Function GetDriveType(ByRef DriveType As Byte) As Boolean
        If (Not (GetResultantType().CompareTo(COMMAND_MONITOR_GOT_DRIVE_TYPE) = 0)) Then Return False
        Dim Res() As Byte : Dim ResLen As Long
        Dim blnOk As Boolean = GetResult(Res, ResLen)
        If (blnOk) Then DriveType = Res(0) : Res = Nothing
        Return blnOk
    End Function

    Public Function GetDriveTypeAsString(ByRef DriveTypeStr As String) As Boolean
        Dim DriveType As Byte
        If (Not GetDriveType(DriveType)) Then Return False
        Dim strDrv As String = IDS_NULL_STRING
        Select Case DriveType
            Case COMMAND_MONITOR_DRIVE_FIXED : strDrv = "Hard Drive"
            Case COMMAND_MONITOR_DRIVE_REMOVABLE : strDrv = "Floppy Drive"
            Case COMMAND_MONITOR_DRIVE_CDROM : strDrv = "CD Drive"
            Case COMMAND_MONITOR_DRIVE_RAMDISK : strDrv = "RAM Drive"
            Case COMMAND_MONITOR_DRIVE_REMOTE : strDrv = "Network Mapped Drive"
            Case COMMAND_MONITOR_DRIVE_NOROOT : strDrv = "UnMounted Drive"
            Case Else 'COMMAND_MONITOR_DRIVE_UNKNOWN
                strDrv = "UnKnown Drive"
        End Select
        DriveTypeStr = strDrv
        Return True
    End Function


    Public Function GetExeType(ByRef ExeType As Byte) As Boolean
        If (Not (GetResultantType().CompareTo(COMMAND_MONITOR_GOT_FILE_ASEXE) = 0)) Then Return False
        Dim Res() As Byte : Dim ResLen As Long
        Dim blnOk As Boolean = GetResult(Res, ResLen)
        If (blnOk) Then ExeType = Res(0) : Res = Nothing
        Return blnOk
    End Function

    Public Function GetExeTypeAsString(ByRef ExeTypeStr As String) As Boolean
        Dim ExeType As Byte
        If (Not GetExeType(ExeType)) Then Return False
        Dim strExe As String = IDS_NULL_STRING
        Select Case ExeType
            Case COMMAND_MONITOR_EXE_32BIT_BINARY : strExe = "32-Bit Windows EXE"
            Case COMMAND_MONITOR_EXE_64BIT_BINARY : strExe = "64-Bit Windows EXE"
            Case COMMAND_MONITOR_EXE_DOS_BINARY : strExe = "16-Bit DOS EXE"
            Case COMMAND_MONITOR_EXE_OS216_BINARY : strExe = "16-Bit OS/2 EXE"
            Case COMMAND_MONITOR_EXE_PIF_BINARY : strExe = "PIF DOS EXE"
            Case COMMAND_MONITOR_EXE_POSIX_BINARY : strExe = "POSIX EXE"
            Case COMMAND_MONITOR_EXE_WOW_BINARY : strExe = "16-Bit Windows EXE"
            Case Else 'COMMAND_MONITOR_EXE_UNKNOWN
                strExe = "UnKnown Type Exe"
        End Select
        ExeTypeStr = strExe
        Return True
    End Function

#End Region

End Class


Public Class CFileMonitor : Implements IDisposable

#Region "Private And Protected Fields"

    Private m_UInt64Address As UInt64

#End Region

#Region "Private API's"

    Private Declare Function RegisterAsFileMonitor Lib "WinMonitorCLIB.1.0.dll" Alias "RegisterAsFileMonitor" () As UInt64
    Private Declare Function RemoveFileMonitor Lib "WinMonitorCLIB.1.0.dll" Alias "RemoveFileMonitor" (ByVal Address As UInt64) As Boolean
    Private Declare Function SetFileDataStream Lib "WinMonitorCLIB.1.0.dll" Alias "CFMSetFileDataStream" (ByVal Address As UInt64, ByVal FileDataStream() As Byte, ByVal LengthOfBytes As UInt32) As Boolean
    Private Declare Function SetFileDataStream Lib "WinMonitorCLIB.1.0.dll" Alias "CFMSetFileDataStreamEx" (ByVal Address As UInt64, ByVal CmdMonitorAddress As UInt64) As Boolean
    Private Declare Function GetFirstFile Lib "WinMonitorCLIB.1.0.dll" Alias "CFMGetFirstFile" (ByVal Address As UInt64, ByVal FileInfo As String, ByVal InfoLevel As Byte, ByRef ResultLength As UInt32) As Boolean
    Private Declare Function GetNextFile Lib "WinMonitorCLIB.1.0.dll" Alias "CFMGetNextFile" (ByVal Address As UInt64, ByVal FileInfo As String, ByVal InfoLevel As Byte, ByRef ResultLength As UInt32) As Boolean
    Private Declare Function GetCurrentFile Lib "WinMonitorCLIB.1.0.dll" Alias "CFMGetCurrentFile" (ByVal Address As UInt64, ByVal FileInfo As String, ByVal InfoLevel As Byte, ByRef ResultLength As UInt32) As Boolean
    Private Declare Function GetCurrentFileType Lib "WinMonitorCLIB.1.0.dll" Alias "CFMGetCurrentFileType" (ByVal Address As UInt64, ByRef FileType As Byte) As Boolean
    Private Declare Function GetNumberOfFiles Lib "WinMonitorCLIB.1.0.dll" Alias "CFMGetNumberOfFiles" (ByVal Address As UInt64) As Integer
    Private Declare Function GetCurrentFileSize Lib "WinMonitorCLIB.1.0.dll" Alias "CFMGetCurrentFileSize" (ByVal Address As UInt64, ByRef FileSize As UInt32) As Integer
    Private Declare Function IsCurrentFileIsDir Lib "WinMonitorCLIB.1.0.dll" Alias "CFMIsCurrentFileIsDir" (ByVal Address As UInt64) As Boolean

#End Region

#Region "Constructors/Destructor"

    Public Sub New()
        m_UInt64Address = RegisterAsFileMonitor()
    End Sub

    Public Sub dispose() Implements IDisposable.Dispose
        RemoveFileMonitor(m_UInt64Address)
    End Sub

#End Region

#Region "Public Functions"

    Public Function IsCurrentFileIsDirectory() As Boolean
        Return IsCurrentFileIsDir(m_UInt64Address)
    End Function

    Public Function SetFileDataStream(ByRef CmdMonitorClient As CCommandMonitorClient) As Boolean
        Return SetFileDataStream(m_UInt64Address, CmdMonitorClient.Address)
    End Function

    Public Function GetNumberOfFiles() As Integer
        Return GetNumberOfFiles(m_UInt64Address)
    End Function

    Public Function GetCurrentFileSize(ByRef FileSize As Long) As Boolean
        Dim uint32FileSize As UInt32
        Dim blnOk As Boolean = GetCurrentFileSize(m_UInt64Address, uint32FileSize)
        If (blnOk = True) Then FileSize = Convert.ToDecimal(uint32FileSize)
        Return blnOk
    End Function

    Public Function SetFileDataStream(ByVal FileDataStream() As Byte, ByVal LengthOfBytes As Long) As Boolean
        Return SetFileDataStream(m_UInt64Address, FileDataStream, Convert.ToUInt32(LengthOfBytes))
    End Function

    Public Function GetFirstFile(ByRef FileInfo As String, ByVal InfoLevel As Integer) As Boolean
        Dim ResLen As UInt32 = Convert.ToUInt32(0)
        Dim strTmp As String = New String(" "c, 1)
        Dim boolOk As Boolean = GetFirstFile(m_UInt64Address, strTmp, InfoLevel, ResLen)
        If Not boolOk Then Return False
        strTmp = Nothing
        Dim ResultLength As Long = Convert.ToDecimal(ResLen)
        If (ResultLength <= 0) Then Return False
        Try
            'System.GC.Collect()
            Dim strUty As New CStringUtility()
            Dim strRes As String = strUty.FreshString(ResultLength + 1)
            boolOk = GetFirstFile(m_UInt64Address, strRes, InfoLevel, ResLen)
            FileInfo = Nothing : FileInfo = strUty.CStyleStringToVbString(strRes)
            strUty = Nothing : strRes = Nothing
            Return boolOk
        Catch Ex As Exception
            Return False
        End Try
    End Function

    Public Function GetNextFile(ByRef FileInfo As String, ByVal InfoLevel As Integer) As Boolean
        Dim ResLen As UInt32 = Convert.ToUInt32(0)
        Dim strTmp As String = New String(" "c, 1)
        Dim boolOk As Boolean = GetNextFile(m_UInt64Address, strTmp, InfoLevel, ResLen)
        If Not boolOk Then Return False
        strTmp = Nothing
        Dim ResultLength As Long = Convert.ToDecimal(ResLen)
        If (ResultLength <= 0) Then Return False
        Try
            'System.GC.Collect()
            Dim strUty As New CStringUtility()
            Dim strRes As String = strUty.FreshString(ResultLength + 1)
            boolOk = GetNextFile(m_UInt64Address, strRes, InfoLevel, ResLen)
            FileInfo = Nothing : FileInfo = strUty.CStyleStringToVbString(strRes)
            strUty = Nothing : strRes = Nothing
            Return boolOk
        Catch Ex As Exception
            Return False
        End Try
    End Function

    Public Function GetCurrentFile(ByRef FileInfo As String, ByVal InfoLevel As Integer) As Boolean
        Dim ResLen As UInt32 = Convert.ToUInt32(0)
        Dim strTmp As String = New String(" "c, 1)
        Dim boolOk As Boolean = GetCurrentFile(m_UInt64Address, strTmp, InfoLevel, ResLen)
        If Not boolOk Then Return False
        strTmp = Nothing
        Dim ResultLength As Long = Convert.ToDecimal(ResLen)
        If (ResultLength <= 0) Then Return False
        Try
            'System.GC.Collect()
            Dim strUty As New CStringUtility()
            Dim strRes As String = strUty.FreshString(ResultLength + 1)
            boolOk = GetCurrentFile(m_UInt64Address, strRes, InfoLevel, ResLen)
            FileInfo = Nothing : FileInfo = strUty.CStyleStringToVbString(strRes)
            strUty = Nothing : strRes = Nothing
            Return boolOk
        Catch Ex As Exception
            Return False
        End Try
    End Function

    Public Function GetCurrentFileType(ByRef FileType As Byte) As Boolean
        GetCurrentFileType(m_UInt64Address, FileType)
    End Function

#End Region


End Class


Public Class CWinMonitorMessenger : Implements IDisposable

#Region "Private And Protected Fields And functions"

    Private m_UInt64Address As UInt64

    Private Function RegisterType(ByVal ServiceType As Byte) As Boolean
        Return WinMonitorMessengerRegisterType(m_UInt64Address, ServiceType)
    End Function

#End Region

#Region "Private API's"

    Private Declare Function RegisterAsWinMonitorMessenger Lib "WinMonitorCLIB.1.0.dll" Alias "RegisterAsWinMonitorMessenger" (ByVal ConnectionAddress As UInt64) As UInt64
    Private Declare Function RemoveWinMonitorMessenger Lib "WinMonitorCLIB.1.0.dll" Alias "RemoveWinMonitorMessenger" (ByVal Address As UInt64) As Boolean
    Private Declare Function WinMonitorMessengerSetConnection Lib "WinMonitorCLIB.1.0.dll" Alias "WinMonitorMessengerSetConnection" (ByVal Address As UInt64, ByVal ConnectionAddress As UInt64) As Boolean
    Private Declare Function WinMonitorMessengerSynchronizeServer Lib "WinMonitorCLIB.1.0.dll" Alias "WinMonitorMessengerSynchronizeServer" (ByVal Address As UInt64, Optional ByVal FromServer As Boolean = True) As Boolean
    Private Declare Function WinMonitorMessengerAuthenticate Lib "WinMonitorCLIB.1.0.dll" Alias "WinMonitorMessengerAuthenticate" (ByVal Address As UInt64, Optional ByVal PassWord As String = Nothing) As Boolean
    Private Declare Function WinMonitorMessengerExecuteCommandMonitorClient Lib "WinMonitorCLIB.1.0.dll" Alias "WinMonitorMessengerExecuteCommandMonitorClient" (ByVal Address As UInt64, ByVal CommandMonitorClientAddress As UInt64) As Boolean
    Private Declare Function WinMonitorMessengerExecuteScreenMonitorMessenger Lib "WinMonitorCLIB.1.0.dll" Alias "WinMonitorMessengerExecuteScreenMonitorMessenger" (ByVal Address As UInt64, ByVal SMessengerAddress As UInt64, ByVal ScreenMonitorAddress As UInt64) As Boolean
    Private Declare Function WinMonitorMessengerShutDown Lib "WinMonitorCLIB.1.0.dll" Alias "WinMonitorMessengerShutDown" (ByVal Address As UInt64) As Boolean
    Private Declare Function WinMonitorMessengerRegisterType Lib "WinMonitorCLIB.1.0.dll" Alias "WinMonitorMessengerRegisterType" (ByVal Address As UInt64, ByVal ServiceType As Byte) As Boolean

#End Region

#Region "Constructors/Destructor"

    Public Sub New(ByRef RefConnection As CTcpNetworkMonitorConnection)
        m_UInt64Address = RegisterAsWinMonitorMessenger(RefConnection.Address)
    End Sub

    Public Sub New()
        m_UInt64Address = RegisterAsWinMonitorMessenger(Convert.ToUInt64(0))
    End Sub

    Public Sub dispose() Implements IDisposable.Dispose
        RemoveWinMonitorMessenger(m_UInt64Address)
    End Sub

#End Region

#Region "Public Methods"

    Public Function SetConnection(ByRef RefConnection As CTcpNetworkMonitorConnection) As Boolean
        Return WinMonitorMessengerSetConnection(m_UInt64Address, RefConnection.Address)
    End Function

    Public Function SynchronizeServer(Optional ByVal FromServer As Boolean = True) As Boolean
        Return WinMonitorMessengerSynchronizeServer(m_UInt64Address, FromServer)
    End Function

    Public Function Authenticate(Optional ByVal PassWord As String = Nothing) As Boolean
        If (WinMonitorMessengerAuthenticate(m_UInt64Address, PassWord) = False) Then Return False
        Return RegisterType(WIN_MONITOR_NORMAL_CLIENTSERVER)
    End Function

    Public Function ExecuteCommandMonitorClient(ByRef RefCommandMonitorClient As CCommandMonitorClient) As Boolean
        Return WinMonitorMessengerExecuteCommandMonitorClient(m_UInt64Address, RefCommandMonitorClient.Address)
    End Function

    Public Function ExecuteScreenMonitorMessenger(ByRef ScreenMonitorMessenger As CScreenMonitorMessenger, ByRef ScreenMonitor As CScreenMonitorBase) As Boolean
        Return WinMonitorMessengerExecuteScreenMonitorMessenger(m_UInt64Address, ScreenMonitorMessenger.Address, ScreenMonitor.Address)
    End Function

    Public Function ShutDown() As Boolean
        Return WinMonitorMessengerShutDown(m_UInt64Address)
    End Function

#End Region

End Class


Public Class CMainMemoryMonitor : Implements IDisposable

#Region "Private And Protected Fields"
    Private m_UInt64Address As UInt64
#End Region

#Region "Private API's"

    Private Declare Function RegisterAsMainMemoryMonitor Lib "WinMonitorCLIB.1.0.dll" Alias "RegisterAsMainMemoryMonitor" (Optional ByVal MainMemoryInfoStream() As Byte = Nothing, Optional ByVal TokenSeperator As String = Chr(10)) As UInt64
    Private Declare Function RemoveMainMemoryMonitor Lib "WinMonitorCLIB.1.0.dll" Alias "RemoveMainMemoryMonitor" (ByVal Address As UInt64) As Boolean
    Private Declare Function SetMemoryInfoStream Lib "WinMonitorCLIB.1.0.dll" Alias "SetMemoryInfoStream" (ByVal Address As UInt64, ByVal MainMemoryInfoStream() As Byte, Optional ByVal TokenSeperator As String = Chr(10)) As Boolean
    Private Declare Function GetMainMemoryStatus Lib "WinMonitorCLIB.1.0.dll" Alias "GetMainMemoryStatus" (ByVal Address As UInt64, ByVal MainMemoryStatus As String, ByRef ResultLength As UInt32) As Boolean
    Private Declare Function GetVirtualMemoryStatus Lib "WinMonitorCLIB.1.0.dll" Alias "GetVirtualMemoryStatus" (ByVal Address As UInt64, ByVal VirtualMemoryStatus As String, ByRef ResultLength As UInt32) As Boolean

#End Region

#Region "Constructors/Destructor"

    Public Sub New(Optional ByVal MainMemoryInfoStream() As Byte = Nothing, Optional ByVal TokenSeperator As String = Chr(10))
        m_UInt64Address = RegisterAsMainMemoryMonitor(MainMemoryInfoStream, TokenSeperator)
    End Sub

    Public Sub dispose() Implements IDisposable.Dispose
        RemoveMainMemoryMonitor(m_UInt64Address)
    End Sub

#End Region

#Region "Public Methods"

    Public Function SetMemoryInfoStream(ByVal MainMemoryInfoStream() As Byte, Optional ByVal TokenSeperator As String = Chr(10)) As Boolean
        Return SetMemoryInfoStream(m_UInt64Address, MainMemoryInfoStream, TokenSeperator)
    End Function

    Public Function GetMainMemoryStatus(ByRef MainMemoryStatus As String) As Boolean
        Dim ResLen As UInt32 = Convert.ToUInt32(0)
        Dim strTmp As String = New String(" "c, 1)
        Dim boolOk As Boolean = GetMainMemoryStatus(m_UInt64Address, strTmp, ResLen)
        If Not boolOk Then Return False
        strTmp = Nothing
        Dim ResultLength As Long = Convert.ToDecimal(ResLen)
        If (ResultLength <= 0) Then Return False
        Try
            'System.GC.Collect()
            Dim strUty As New CStringUtility()
            Dim strRes As String = strUty.FreshString(ResultLength + 1)
            boolOk = GetMainMemoryStatus(m_UInt64Address, strRes, ResLen)
            MainMemoryStatus = Nothing : MainMemoryStatus = strUty.CStyleStringToVbString(strRes)
            strUty = Nothing : strRes = Nothing
            Return boolOk
        Catch Ex As Exception
            Return False
        End Try
    End Function

    Public Function GetVirtualMemoryStatus(ByRef VirtualMemoryStatus As String) As Boolean
        Dim ResLen As UInt32 = Convert.ToUInt32(0)
        Dim strTmp As String = New String(" "c, 1)
        Dim boolOk As Boolean = GetVirtualMemoryStatus(m_UInt64Address, strTmp, ResLen)
        strTmp = Nothing
        Dim ResultLength As Long = Convert.ToDecimal(ResLen)
        If (ResultLength <= 0) Then Return False
        Try
            System.GC.Collect()
            Dim strUty As New CStringUtility()
            Dim strRes As String = strUty.FreshString(ResultLength + 1)
            boolOk = GetVirtualMemoryStatus(m_UInt64Address, strRes, ResLen)
            VirtualMemoryStatus = Nothing : VirtualMemoryStatus = strUty.CStyleStringToVbString(strRes)
            strUty = Nothing : strRes = Nothing
            Return boolOk
        Catch Ex As Exception
            Return False
        End Try
    End Function

#End Region

End Class


Public Class COSMonitor : Implements IDisposable

#Region "Private And Protected Fields"
    Private m_UInt64Address As UInt64
#End Region

#Region "Private API's"

    Private Declare Function RegisterAsOSMonitor Lib "WinMonitorCLIB.1.0.dll" Alias "RegisterAsOSMonitor" (Optional ByVal OSInfoStream() As Byte = Nothing, Optional ByVal TokenSeperator As String = Chr(10)) As UInt64
    Private Declare Function RemoveOSMonitor Lib "WinMonitorCLIB.1.0.dll" Alias "RemoveOSMonitor" (ByVal Address As UInt64) As Boolean
    Private Declare Function SetOSInfoStream Lib "WinMonitorCLIB.1.0.dll" Alias "SetOSInfoStream" (ByVal Address As UInt64, ByVal OSInfoStream() As Byte, Optional ByVal TokenSeperator As String = Chr(10)) As Boolean
    Private Declare Function GetOSInformation Lib "WinMonitorCLIB.1.0.dll" Alias "GetOSInformation" (ByVal Address As UInt64, ByVal OSInfo As String, ByRef ResultLength As UInt32) As Boolean

#End Region

#Region "Constructors/Destructor"

    Public Sub New(Optional ByVal OSInfoStream() As Byte = Nothing, Optional ByVal TokenSeperator As String = Chr(10))
        m_UInt64Address = RegisterAsOSMonitor(OSInfoStream, TokenSeperator)
    End Sub

    Public Sub dispose() Implements IDisposable.Dispose
        RemoveOSMonitor(m_UInt64Address)
    End Sub

#End Region

#Region "Public Methods"

    Public Function SetOSInfoStream(ByVal OSInfoStream() As Byte, Optional ByVal TokenSeperator As String = Chr(10)) As Boolean
        Return SetOSInfoStream(m_UInt64Address, OSInfoStream, TokenSeperator)
    End Function

    Public Function GetOSInformation(ByRef OSInfo As String) As Boolean
        Dim ResLen As UInt32 = Convert.ToUInt32(0)
        Dim strTmp As String = New String(" "c, 1)
        Dim boolOk As Boolean = GetOSInformation(m_UInt64Address, strTmp, ResLen)
        If Not boolOk Then Return False
        strTmp = Nothing
        Dim ResultLength As Long = Convert.ToDecimal(ResLen)
        If (ResultLength <= 0) Then Return False
        Try
            'System.GC.Collect()
            Dim strUty As New CStringUtility()
            Dim strRes As String = strUty.FreshString(ResultLength + 1)
            boolOk = GetOSInformation(m_UInt64Address, strRes, ResLen)
            OSInfo = Nothing : OSInfo = strUty.CStyleStringToVbString(strRes)
            strUty = Nothing : strRes = Nothing
            Return boolOk
        Catch Ex As Exception
            Return False
        End Try
    End Function

#End Region

End Class


Public Class CChatClient : Implements IDisposable

#Region "Private,Protected Fields and Functions"

    Private m_UInt64Address As UInt64

    Protected Friend Property Address() As UInt64
        Get
            Return m_UInt64Address
        End Get

        Set(ByVal AddressCopy As UInt64)
            m_UInt64Address = AddressCopy
        End Set
    End Property

    Protected Function RegisterType(ByVal ServiceType As Byte) As Boolean
        Return ChatClientRegisterType(m_UInt64Address, ServiceType)
    End Function

#End Region

#Region "Private API's"

    Private Declare Function RegisterAsChatClient Lib "WinMonitorCLIB.1.0.dll" Alias "RegisterAsChatClient" (ByVal ConnectionAddress As UInt64) As UInt64
    Private Declare Function RemoveChatClient Lib "WinMonitorCLIB.1.0.dll" Alias " RemoveChatClient" (ByVal Address As UInt64) As Boolean
    Private Declare Function ChatClientSetConnection Lib "WinMonitorCLIB.1.0.dll" Alias "ChatClientSetConnection" (ByVal Address As UInt64, ByVal ConnectionAddress As UInt64) As Boolean
    Private Declare Function ChatClientSynchronizeServer Lib "WinMonitorCLIB.1.0.dll" Alias "ChatClientSynchronizeServer" (ByVal Address As UInt64) As Boolean
    Private Declare Function ChatClientShutDown Lib "WinMonitorCLIB.1.0.dll" Alias "ChatClientShutDown" (ByVal Address As UInt64) As Boolean
    Private Declare Function ChatClientSendChatString Lib "WinMonitorCLIB.1.0.dll" Alias "ChatClientSendChatString" (ByVal Address As UInt64, ByVal srcString As String) As Boolean
    Private Declare Function ChatClientReceieveChatString Lib "WinMonitorCLIB.1.0.dll" Alias "ChatClientReceieveChatString" (ByVal Address As UInt64, ByVal dstString As String, ByRef strLen As UInt32) As Boolean
    Private Declare Function ChatClientRegisterType Lib "WinMonitorCLIB.1.0.dll" Alias "ChatClientRegisterType" (ByVal Address As UInt64, ByVal ServiceType As Byte) As Boolean
    Protected Declare Function ChatClientAuthenticate Lib "WinMonitorCLIB.1.0.dll" Alias "ChatClientAuthenticate" (ByVal Address As UInt64, ByVal PassWord As String) As Boolean

#End Region

#Region "Constructors/Destructor"

    Public Sub New(ByRef RefConnection As CTcpNetworkMonitorConnection)
        m_UInt64Address = RegisterAsChatClient(RefConnection.Address)
    End Sub

    Public Sub dispose() Implements IDisposable.Dispose
        RemoveChatClient(m_UInt64Address)
    End Sub

#End Region

#Region "Public Methods"


    Public Function SetConnection(ByRef Connection As CTcpNetworkMonitorConnection) As Boolean
        Return ChatClientSetConnection(m_UInt64Address, Connection.Address)
    End Function

    Public Function SynchronizeServer() As Boolean
        Return ChatClientSynchronizeServer(m_UInt64Address)
    End Function

    Public Overridable Function Authenticate(ByVal PassWord As String) As Boolean
        If (ChatClientAuthenticate(m_UInt64Address, PassWord) = False) Then Return False
        Return RegisterType(WIN_MONITOR_CHAT_CLIENTSERVER)
    End Function

    Public Function ShutDown() As Boolean
        Return ChatClientShutDown(m_UInt64Address)
    End Function

    Public Overridable Function SendString(ByVal srcString As String) As Boolean
        Return ChatClientSendChatString(m_UInt64Address, srcString)
    End Function

    Public Overridable Function ReceieveString(ByRef dstString As String) As Boolean
        Dim ResLen As UInt32 = Convert.ToUInt32(0)
        Dim strTmp As String = New String(" "c, 1)
        Dim boolOk As Boolean = ChatClientReceieveChatString(m_UInt64Address, strTmp, ResLen)
        If Not boolOk Then Return False
        strTmp = Nothing
        Dim ResultLength As Long = Convert.ToDecimal(ResLen)
        If (ResultLength <= 0) Then Return False
        Try
            Dim strUty As New CStringUtility()
            Dim strRes As String = strUty.FreshString(ResultLength + 1)
            boolOk = ChatClientReceieveChatString(m_UInt64Address, strRes, ResLen)
            dstString = Nothing : dstString = strUty.CStyleStringToVbString(strRes)
            strUty = Nothing : strRes = Nothing
            Return boolOk
        Catch Ex As Exception
            Return False
        End Try
    End Function

#End Region

End Class


Public Class COnLineKeyLoggerClient : Inherits CChatClient

#Region "Private API's"

    Private Declare Function ChatClientReceieveOnLineKeyLogString Lib "WinMonitorCLIB.1.0.dll" Alias "ChatClientReceieveOnLineKeyLogString" (ByVal Address As UInt64, ByVal dstString As String, ByRef strLen As UInt32) As Boolean

#End Region

#Region "Constructors/Destructor"

    Public Sub New(ByRef RefConnection As CTcpNetworkMonitorConnection)
        MyBase.New(RefConnection)
    End Sub

#End Region

#Region "Public Methods"

    Public Overrides Function Authenticate(ByVal PassWord As String) As Boolean
        If (ChatClientAuthenticate(MyBase.Address, PassWord) = False) Then Return False
        Return RegisterType(WIN_MONITOR_KEYLOG_CLIENTSERVER)
    End Function

    Public Overrides Function SendString(ByVal srcString As String) As Boolean
        Return False
    End Function

    Public Overrides Function ReceieveString(ByRef dstString As String) As Boolean
        Dim ResLen As UInt32 = Convert.ToUInt32(0)
        Dim strTmp As String = New String(" "c, 1)
        Dim boolOk As Boolean = ChatClientReceieveOnLineKeyLogString(MyBase.Address, strTmp, ResLen)
        If Not boolOk Then Return False
        strTmp = Nothing
        Dim ResultLength As Long = Convert.ToDecimal(ResLen)
        If (ResultLength <= 0) Then Return False
        Try
            Dim strUty As New CStringUtility()
            Dim strRes As String = strUty.FreshString(ResultLength + 1)
            boolOk = ChatClientReceieveOnLineKeyLogString(MyBase.Address, strRes, ResLen)
            dstString = Nothing : dstString = strUty.CStyleStringToVbString(strRes)
            strUty = Nothing : strRes = Nothing
            Return boolOk
        Catch Ex As Exception
            Return False
        End Try
    End Function

#End Region

End Class


