# GradeMaster
![GradeMaster Logo](Images/logo.png)


## About
GradeMaster is a Desktop based grade management tool. It allows you to manage your educations, subjects and grades.


## Technologies Used

### Desktop Client
- [.NET MAUI](https://dotnet.microsoft.com/en-us/apps/maui)
- [Blazor](https://dotnet.microsoft.com/en-us/apps/aspnet/web-apps/blazor)
- [Blazor Bootstrap](https://demos.blazorbootstrap.com/) 
- [Bootstrap](https://getbootstrap.com/) <br>


### Backend & Database
- [Entity Framework Core](https://learn.microsoft.com/de-de/ef/core/)
- [SQLite](https://sqlite.org/) <br>

<!-- ### Win Client (discontinued and removed)
- [WinUi 3](https://learn.microsoft.com/de-de/windows/apps/winui/winui3/) <br> -->

### .NET Version: 9.0


## Installation and Setup Guide

### For personal usage (Windows)
1. Install the Zip-File (GradeMaster_[win-x64/win-arm64] depending on your System) from the Release Section. Make sure you install the newest available version.
2. Unpack the Zip-File in the `C:\Program Files\` directory.
3. After the installation you will see the folder containing the app. There you can copy the shortcut and paste it to the Desktop.
4. You Can now launch the app from the shortcut.
5. Before you launch the application make shure you trust the binary, by checking the mark in the Properties tap when right clicking the file. (File location: `C:\Program Files\GradeMaster\bin\win-x64\GradeMaster.DesktopClient.exe`) (or `C:\Program Files\GradeMaster\bin\win-arm64\GradeMaster.DesktopClient.exe` for arm based systems)
6. Optional: If you like you can also copy the shortcut to the Start Menu. You can do this by copying the shortcut to this directory: `Win + R` then `%AppData%\Microsoft\Windows\Start Menu\Programs`.

### Updating Software (Windows)
1. Make shure you delete the `C:\Program Files\GradeMaster` directory. (It is recommended you make a backup of your Data by copying your GradeMaster.db file located in `C:\Users\YourUser\AppData\Local\GradeMaster\Data`. The database should remain compatible with the newer versions of the app and no data should be lost)
2. Then Follow the same steps from the Instalation Guide for personal use. (You do not have to copy the shortcut again. You can skip this step. Only do this step if the shortcut doesnt work anymore.)

### Uninstalling the App for Users (Windows)
1. Delete the directory `C:\Program Files\GradeMaster`.
2. You can also delete the application data directory if you want to delete your data (not recommended). To do this delete the directory `C:\Users\YourUser\AppData\Local\GradeMaster`.
3. You can also delete all of the shortcuts. Delete them, where you copied them.

### For Developement
1. Clone this repository to your PC.
2. Make shure you have the necessary stuff installed in Visual Studio for .NET MAUI Blazor Hybrid applications.
3. Open the Solution located in: `..YourPath\GradeMaster\Src\GradeMaster.sln` in Visual Studio.
4. Let Visual Studio start up.
5. Then make shure all the packages and dependancies are loaded.
6. Next you can build the solution.
7. After you can start the application from the start button in Visual Studio. Make shure you have the Developer-Mode activated on your device.


## Info

### Current Release Version
[![Release Version Badge](https://img.shields.io/github/v/release/devt06/GradeMaster)](https://github.com/DevT06/GradeMaster/releases)
[![Downloads@latest](https://img.shields.io/github/downloads/devt06/GradeMaster/latest/total)](https://github.com/DevT06/GradeMaster/releases/latest)
<!-- [![Downloads@v1.0.0](https://img.shields.io/github/downloads/DevT06/GradeMaster/v1.0.0/total)](https://github.com/DevT06/GradeMaster/releases/latest) -->

### Price
GradeMaster is free to use, as of this moment.

### Development State
GradeMaster is in active development. <br> 

### Supported Platforms
| OS      | Architecture | Supported |
|---------|--------------|-----------|
| Windows | x64          | Yes       |
| Windows | arm64        | Yes       |
| macOS   | x64          | No        |
| macOS   | arm64        | No        |
| iOS     | arm64        | No        |
| Android | arm64        | No        |

#### Info
- You also have to make shure you have the WebView2 installed on your System. [WebView2](https://developer.microsoft.com/en-us/microsoft-edge/webview2)
- The WebView2 should already be pre installed on all Windows 11 and most Windows 10 devices.

#### The Desktop GUI Client is currently limited to Windows only.
- Windows versions that are supported and work (tested): Windows 11
- Windows versions that are supported and should work (not tested): Windows 10

Other platforms might get Support in the future.

#### Disk Space Requirement
- It is recommended to have at least 1gb of free disk space for the application.

### Supported Languages
- English

### Supported Grading Systems
- Swiss

### Console Client
The Console Client is currently only for testing purposes.

<!-- ### Win Client
The WinClient (based on WinUi 3) is discontinued and removed from the main branch, in favor of the .Net MAUI Blazor hybrid DesktopClient. -->


<br>


## License
This project is licensed under the Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International (CC BY-NC-SA 4.0) License. For commercial use, please contact **[timothygmaurer@outlook.com](mailto:timothygmaurer@outlook.com)**.

[License: CC BY-NC-SA 4.0](https://creativecommons.org/licenses/by-nc-sa/4.0/)


## Disclaimer
THIS SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE, AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES, OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT, OR OTHERWISE, ARISING FROM, OUT OF, OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

<!--Maybe add more disclaimers (TBD)-->
