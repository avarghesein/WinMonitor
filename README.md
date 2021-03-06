# WinMonitor
Demo Project On Remote Workstation Administration through Visual C++ - Win32 Socket Programming

# Note: Build Visual C++ Projects In Visual Studio 6 and Visual Basic Projects In VS2003+ IDEs. Upgrades are welcome!

[Architecture In a NutShell](https://github.com/avarghesein/WinMonitor/blob/master/Docs/Architecture%20In%20a%20NutShell.txt)

[Detailed Design Document](https://github.com/avarghesein/WinMonitor/blob/master/Docs/Detailed%20Architecture.docx)

The project demos the aspects of controlling remote workstations from a central administration server using Client-Server model. It comprises of two components:

# WinMonitor-Client or System-Administrator:
Will be installed on a Central Administration Server. 
The main program is a Visual Basic GUI application, named [WinMonitor](https://github.com/avarghesein/WinMonitor/tree/master/Source/Client/WinMonitor.1.0),
using which you could register remote workstations to be controlled and then monitor them.
Each workstation has WinMonitor-Server component installed on them (More below).

The main program depends on a Visual  Basic Class Library Named [WinMonitorBLIB](https://github.com/avarghesein/WinMonitor/tree/master/Source/Client/WinMonitorBLIB.1.0),
which in turn depends on a Win32 C++ Library named [WinMonitorCLIB](https://github.com/avarghesein/WinMonitor/tree/master/Source/Client/WinMonitorCLIB.1.0).

WinMonitorCLIB is a shared C++ Library shared between WinMonitor Client and WinMonitor Server (below)

# WinMonitor-Server or Remote-User-Workstation:
Will be installed on every single Remote Workstations to be controlled.
They are configured to run on boot up and listen for incoming connections (From WInMonitor Client application) to be serviced.

The main program is a Win32 EXE Application named [WinMonitorServer](https://github.com/avarghesein/WinMonitor/tree/master/Source/Server/Basic_V1.0/WinMonitorServer/WinMonitorServer)
The main program depends on a Win32 C++ Library named [WinMonitorCLIB](https://github.com/avarghesein/WinMonitor/tree/master/Source/Server/Basic_V1.0/WinMonitorCLIB.1.0) and a Win32 C++ Plugin Library named [WinMonitorServerPlugin](https://github.com/avarghesein/WinMonitor/tree/master/Source/Server/Basic_V1.0/WinMonitorServer/WinMonitorServerPlugin) for added features

**A more fine grained plugin model of WinMonitor-Server (though not fully tested) is also available under Version to [here](https://github.com/avarghesein/WinMonitor/tree/master/Source/Server/PluginBased_V2.0).


The server support Administration Tasks (based on the commands initiated from the WinMonitor Client) to be done
on the Remote workstation in which it is running, includes-

* Enable Key Loggers to capture User Typed Keys for monitoring from the Remote Workstation
* Live or Offline Screen Capturing from Remote Workstation, on a periodic basis 
* Browsing File System and Drives on the Remote Workstation
* Copying Files/Folders between WinMonitor Client and Remote Workstation
* Live Chat messaging between WinMonitor Client and REmote Workstation user
* Running WinMonitor client issueed commands on the Remote Workstation
* Open/Close Workstations CD Rom Drive remotely
* View or manage Remote workstation running processes (change priority, Kill Process)
* Log Off, Restart, Shutdown the Remote workstation

By default The client will make connections to the Remote Servers.
On the contrarary The Remote server can also configured to reverse connect back to a WinMonitor-Client, so the The Client hasn't need to lookup for such Remote Servers.
This is called Reverse Connections.

Features In Pictures:

* Registering a New Remote Workstation In Client

![alt Pic1](https://github.com/avarghesein/WinMonitor/blob/master/Docs/Screenshots/WinMonitor-RegisterWorkStation.jpg)

![alt Pic2](https://github.com/avarghesein/WinMonitor/blob/master/Docs/Screenshots/WinMonitor-RegisterWorkStation2.jpg)

* Client Connecting To Remote Workstation Or Server

![alt Pic3](https://github.com/avarghesein/WinMonitor/blob/master/Docs/Screenshots/WinMonitor-ConnectToServer.jpg)

* Remote Workstation Reverse Connecting To Client

![alt Pic4](https://github.com/avarghesein/WinMonitor/blob/master/Docs/Screenshots/WinMonitor-LoginToReverseConnectedServers.jpg)

* Client showsing Remote Workstation's Dashboard

![alt Pic5](https://github.com/avarghesein/WinMonitor/blob/master/Docs/Screenshots/WinMonitor-RemoteServerDashboard.jpg)

* Client Capturing Remote Server Desktop Offline

![alt Pic6](https://github.com/avarghesein/WinMonitor/blob/master/Docs/Screenshots/WinMonitor-RemoteServerOfflineDesktopCapture.jpg)

![alt Pic7](https://github.com/avarghesein/WinMonitor/blob/master/Docs/Screenshots/WinMonitor-RemoteServeScreenCapture.jpg)

* Client shows Remote Workstation's Drive Details

![alt Pic8](https://github.com/avarghesein/WinMonitor/blob/master/Docs/Screenshots/WinMonitor-RemoteServeDriveDetails.jpg)

* Client exploring and managing Remote Workstation's File System

![alt Pic9](https://github.com/avarghesein/WinMonitor/blob/master/Docs/Screenshots/WinMonitor-RemoteServerFileExplorer.jpg)

* Client LogOff/Shutdown Remote Workstation 

![alt Pic10](https://github.com/avarghesein/WinMonitor/blob/master/Docs/Screenshots/WinMonitor-RemoteServeShutdownLogOff.jpg)

* Client showing and managing Remote Workstation's Running Application Processes

![alt Pic11](https://github.com/avarghesein/WinMonitor/blob/master/Docs/Screenshots/WinMonitor-RemoteServeProcessManagement.jpg)

* Client messages, Live Chats with Remote Workstation Logged-In User

![alt Pic12](https://github.com/avarghesein/WinMonitor/blob/master/Docs/Screenshots/WinMonitor-RemoteServeMessaging.jpg)

![alt Pic13](https://github.com/avarghesein/WinMonitor/blob/master/Docs/Screenshots/WinMonitor-RemoteServeLiveChat.jpg)

* Client Captures, Remote Workstation, User Keyboard Activities

![alt Pic14](https://github.com/avarghesein/WinMonitor/blob/master/Docs/Screenshots/WinMonitor-RemoteServeCaptureUserTypedKeys.jpg)

* Client opening/closing Remote Workstation CD Rom Drive

![alt Pic15](https://github.com/avarghesein/WinMonitor/blob/master/Docs/Screenshots/WinMonitor-RemoteServeCDDriveManagement.jpg)

