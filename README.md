# GradeMaster
![GradeMaster Logo](Images/logo.png)

## About
GradeMaster is a Desktop based grade management tool. It allows you to manage your educations, subjects and grades.

## Technologies Used
### Desktop Client
- [.Net MAUI](https://dotnet.microsoft.com/en-us/apps/maui)
- [Blazor](https://dotnet.microsoft.com/en-us/apps/aspnet/web-apps/blazor)
- [Blazor Bootstrap](https://demos.blazorbootstrap.com/) 
- [Bootstrap](https://getbootstrap.com/) <br>


### Backend & Database
- [Entity Framework Core](https://learn.microsoft.com/de-de/ef/core/)
- [SQLite](https://sqlite.org/) <br>

### Win Client (discontinued and removed)
- [WinUi 3](https://learn.microsoft.com/de-de/windows/apps/winui/winui3/) <br>

### .NET Version: 9.0

## Installation and Setup
### For personal usage
1. Install the Zip-File (GradeMaster_[win-x64/win-arm64] depending on your System) from the Release Section. Make sure you install the newest available version.
2. Unpack the Zip-File in the `C:\` directory.
3. After the installation you will see the folder containing the app. There you can copy the shortcut and paste it to the Desktop.
4. You Can now launch the app from the shortcut.
5. Before you launch the application make shure you trust the binary, by checking the mark in the Properties tap when right clicking the file. (File location: `C:\GradeMaster\bin\win-x64\GradeMaster.DesktopClient.exe`)


### For Developement
1. Clone this repository to your PC.
2. Make shure you have the necessary stuff installed in Visual Studio for .NET MAUI Blazor Hybrid applications.
3. Open the Solution located in: `..YourPath\GradeMaster\Src\GradeMaster.sln` in Visual Studio.
4. Let Visual Studio start up.
5. Then make shure all the packages and dependancies are loaded.
6. Next you can build the solution.
7. After you can start the application from the start button in Visual Studio. Make shure you have the Developer-Mode activated on your device.

## Info

### Releases and Versions
[![Release Version Badge](https://img.shields.io/github/v/release/devt06/GradeMaster)](https://github.com/DevT06/GradeMaster/releases)
[![Downloads@latest](https://img.shields.io/github/downloads/devt06/GradeMaster/1.0.0/total)](https://github.com/DevT06/GradeMaster/releases/latest)

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

The Desktop GUI Client is currently limited to Windows only. <br>
Support for other platforms might change in the future.

### Supported Languages
- English

### Supported Grading Systems
- Swiss

### Console Client
The Console Client is currently only for testing purposes.

### Win Client
The WinClient (based on WinUi 3) is discontinued and removed from the main branch, in favor of the .Net MAUI Blazor hybrid DesktopClient.


<br>


## License
This project is licensed under the Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International (CC BY-NC-SA 4.0) License. For commercial use, please contact **[timothygmaurer@outlook.com](mailto:timothygmaurer@outlook.com)**.

[License: CC BY-NC-SA 4.0](https://creativecommons.org/licenses/by-nc-sa/4.0/)
