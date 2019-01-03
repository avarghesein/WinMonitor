# WinMonitor
Demo Project On Remote Workstation Administration through Visual C++ - Win32 Socket Programming

# Note: Build Visual C++ Projects In Visual Studio 6 and Visual Basic Projects In VS2003+ IDEs. Upgrades are welcome!

[Architecture In a NutShell](https://github.com/avarghesein/WinMonitor/blob/master/Docs/Architecture%20In%20a%20NutShell.txt)
[Detailed Design Document](https://github.com/avarghesein/WinMonitor/blob/master/Docs/Detailed%20Design.docx)

The project demos the aspects of controlling remote workstations from a central administration server using Client-Server model. It comprises of two components:

# Client :
Will be installed on a Central Administration Server. 
The main program is a Visual Basic GUI application, named [WinMonitor](https://github.com/avarghesein/WinMonitor/tree/master/Source/Client/WinMonitor.1.0),
using which you could register remote workstations to be controlled and then monitor them.
Each workstation has WinMonitorServer component installed on them (More below).

The main program depends on Visual  Basic Class Library Named [WinMonitorBLIB](https://github.com/avarghesein/WinMonitor/tree/master/Source/Client/WinMonitorBLIB.1.0),
which in turn depends on a Win32 C++ Library named [WinMonitorCLIB](https://github.com/avarghesein/WinMonitor/tree/master/Source/Client/WinMonitorCLIB.1.0)
WinMonitorCLIB is a shared C++ Library shared between WinMonitor Client and WinMonitor Server (below)

# Server:
Will be installed on every single Remote Workstations to be controlled.
They are configured to run on boot up and listen for incoming connections (From WInMonitor Client application) to be serviced.

The server support Administration Tasks (based on the commands initiated from the WinMonitor Client)
enabled at the workstation in which it is running includes;

* Enable Key Loggers to capture User Typed Keys for monitoring
* Live or Offline Screen Capturing on a periodic basis 
* Browsing File System and Drives
* Copying Files/Folders between WinMonitor Client and Workstation
* Live Chat messaging between WinMonitor Client and Workstation user
* Running WinMonitor client issues commands on the Workstation
* Open/Close Workstations CD Rom Drive remotely
* Allows view or manage workstation process remotely (change priority, Kill Process)
* Log Off, Restart, Shutdown the workstation

By default The client will make connections to the Server.
On the contrarary The server can also configured to connect to a Remote Client, so the The Client hasn't need to lookup for such Servers.
This is called Reverse Connections.

Features In Pictures:

* Registering a New Remote Server In Client
(https://github.com/avarghesein/WinMonitor/blob/master/Docs/Screenshots/WinMonitor-RegisterWorkStation.jpg)
(https://github.com/avarghesein/WinMonitor/blob/master/Docs/Screenshots/WinMonitor-RegisterWorkStation2.jpg)

* Client Connecting To Remote Server
(https://github.com/avarghesein/WinMonitor/blob/master/Docs/Screenshots/WinMonitor-ConnectToServer.jpg)

* Remote Server Reverse Connecting To Client
(https://github.com/avarghesein/WinMonitor/blob/master/Docs/Screenshots/WinMonitor-LoginToReverseConnectedServers.jpg)

* Client showsing Remote Server's Dashboard
(https://github.com/avarghesein/WinMonitor/blob/master/Docs/Screenshots/WinMonitor-RemoteServerDashboard.jpg)

* Client Capturing Remote Server Desktop Offline
(https://github.com/avarghesein/WinMonitor/blob/master/Docs/Screenshots/WinMonitor-RemoteServerOfflineDesktopCapture.jpg)
(https://github.com/avarghesein/WinMonitor/blob/master/Docs/Screenshots/WinMonitor-RemoteServeScreenCapture.jpg)

* Client shows Remote Server's Drive Details
(https://github.com/avarghesein/WinMonitor/blob/master/Docs/Screenshots/WinMonitor-RemoteServeDriveDetails.jpg)

* Client exploring and managing Remote Server's File System
(https://github.com/avarghesein/WinMonitor/blob/master/Docs/Screenshots/WinMonitor-RemoteServerFileExplorer.jpg)

* Client LogOff/Shutdown Remote Server 
(https://github.com/avarghesein/WinMonitor/blob/master/Docs/Screenshots/WinMonitor-RemoteServeShutdownLogOff.jpg)

* Client showing and managing Remote Server Processes
(https://github.com/avarghesein/WinMonitor/blob/master/Docs/Screenshots/WinMonitor-RemoteServeProcessManagement.jpg)

* Client messages, Live Chats with Remote Server Use
(https://github.com/avarghesein/WinMonitor/blob/master/Docs/Screenshots/WinMonitor-RemoteServeMessaging.jpg)
(https://github.com/avarghesein/WinMonitor/blob/master/Docs/Screenshots/WinMonitor-RemoteServeLiveChat.jpg)

* Client Captures, Remote Server, User Keyboard Activities
(https://github.com/avarghesein/WinMonitor/blob/master/Docs/Screenshots/WinMonitor-RemoteServeCaptureUserTypedKeys.jpg)

* Client opening/closing Remote Server CD Rom Drive
(https://github.com/avarghesein/WinMonitor/blob/master/Docs/Screenshots/WinMonitor-RemoteServeCDDriveManagement.jpg)

