# WinMonitor
Demo Project On Remote Workstation Administration through Visual C++ - Win32 Socket Programming

# Note: Build Visual C++ Projects In Visual Studio 6 and Visual Basic Projects In VS2003+ IDEs. Upgrades are welcome!

[Architecture In a NutShell](https://github.com/avarghesein/WinMonitor/blob/master/Docs/Architecture%20In%20a%20NutShell.txt)

[Detailed Design Document](https://github.com/avarghesein/WinMonitor/blob/master/Docs/Detailed%20Design.docx)

The project demos the aspects of controlling remote workstations from a central administration server using Client-Server model. It comprises of two components:

# WinMonitor-Client :
Will be installed on a Central Administration Server. 
The main program is a Visual Basic GUI application, named [WinMonitor](https://github.com/avarghesein/WinMonitor/tree/master/Source/Client/WinMonitor.1.0),
using which you could register remote workstations to be controlled and then monitor them.
Each workstation has WinMonitor-Server component installed on them (More below).

The main program depends on a Visual  Basic Class Library Named [WinMonitorBLIB](https://github.com/avarghesein/WinMonitor/tree/master/Source/Client/WinMonitorBLIB.1.0),
which in turn depends on a Win32 C++ Library named [WinMonitorCLIB](https://github.com/avarghesein/WinMonitor/tree/master/Source/Client/WinMonitorCLIB.1.0).

WinMonitorCLIB is a shared C++ Library shared between WinMonitor Client and WinMonitor Server (below)

# WinMonitor-Server:
Will be installed on every single Remote Workstations to be controlled.
They are configured to run on boot up and listen for incoming connections (From WInMonitor Client application) to be serviced.

The main program is a Win32 EXE Application named [WinMonitorServer](https://github.com/avarghesein/WinMonitor/tree/master/Source/Server/Basic_V1.0/WinMonitorServer/WinMonitorServer)
The main program depends on a Win32 C++ Library named [WinMonitorCLIB](https://github.com/avarghesein/WinMonitor/tree/master/Source/Server/Basic_V1.0/WinMonitorCLIB.1.0) and a Win32 C++ Plugin Library named [WinMonitorServerPlugin](https://github.com/avarghesein/WinMonitor/tree/master/Source/Server/Basic_V1.0/WinMonitorServer/WinMonitorServerPlugin) for added features

**A more fine grained plugin model of WinMonitor-Server (though not fully tested) is also available under Version to [here](https://github.com/avarghesein/WinMonitor/tree/master/Source/Server/PluginBased_V2.0).


The server support Administration Tasks (based on the commands initiated from the WinMonitor Client) to be done
on the workstation in which it is running, includes-

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

![alt Pic1](https://github.com/avarghesein/WinMonitor/blob/master/Docs/Screenshots/WinMonitor-RegisterWorkStation.jpg)

![alt Pic2](https://github.com/avarghesein/WinMonitor/blob/master/Docs/Screenshots/WinMonitor-RegisterWorkStation2.jpg)

* Client Connecting To Remote Server

![alt Pic3](https://github.com/avarghesein/WinMonitor/blob/master/Docs/Screenshots/WinMonitor-ConnectToServer.jpg)

* Remote Server Reverse Connecting To Client

![alt Pic4](https://github.com/avarghesein/WinMonitor/blob/master/Docs/Screenshots/WinMonitor-LoginToReverseConnectedServers.jpg)

* Client showsing Remote Server's Dashboard

![alt Pic5](https://github.com/avarghesein/WinMonitor/blob/master/Docs/Screenshots/WinMonitor-RemoteServerDashboard.jpg)

* Client Capturing Remote Server Desktop Offline

![alt Pic6](https://github.com/avarghesein/WinMonitor/blob/master/Docs/Screenshots/WinMonitor-RemoteServerOfflineDesktopCapture.jpg)

![alt Pic7](https://github.com/avarghesein/WinMonitor/blob/master/Docs/Screenshots/WinMonitor-RemoteServeScreenCapture.jpg)

* Client shows Remote Server's Drive Details

![alt Pic8](https://github.com/avarghesein/WinMonitor/blob/master/Docs/Screenshots/WinMonitor-RemoteServeDriveDetails.jpg)

* Client exploring and managing Remote Server's File System

![alt Pic9](https://github.com/avarghesein/WinMonitor/blob/master/Docs/Screenshots/WinMonitor-RemoteServerFileExplorer.jpg)

* Client LogOff/Shutdown Remote Server 

![alt Pic10](https://github.com/avarghesein/WinMonitor/blob/master/Docs/Screenshots/WinMonitor-RemoteServeShutdownLogOff.jpg)

* Client showing and managing Remote Server Processes

![alt Pic11](https://github.com/avarghesein/WinMonitor/blob/master/Docs/Screenshots/WinMonitor-RemoteServeProcessManagement.jpg)

* Client messages, Live Chats with Remote Server Use

![alt Pic12](https://github.com/avarghesein/WinMonitor/blob/master/Docs/Screenshots/WinMonitor-RemoteServeMessaging.jpg)

![alt Pic13](https://github.com/avarghesein/WinMonitor/blob/master/Docs/Screenshots/WinMonitor-RemoteServeLiveChat.jpg)

* Client Captures, Remote Server, User Keyboard Activities

![alt Pic14](https://github.com/avarghesein/WinMonitor/blob/master/Docs/Screenshots/WinMonitor-RemoteServeCaptureUserTypedKeys.jpg)

* Client opening/closing Remote Server CD Rom Drive

![alt Pic15](https://github.com/avarghesein/WinMonitor/blob/master/Docs/Screenshots/WinMonitor-RemoteServeCDDriveManagement.jpg)

