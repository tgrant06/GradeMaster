# GradeMaster

![GradeMaster Logo](Images/logo.png)

## About

GradeMaster is a Desktop based grade management tool. It allows you to manage your educations, subjects and grades.

## Technologies Used

### Desktop Client

- [.NET MAUI](https://dotnet.microsoft.com/en-us/apps/maui)
- [Blazor](https://dotnet.microsoft.com/en-us/apps/aspnet/web-apps/blazor)
- [Blazor Bootstrap](https://demos.blazorbootstrap.com/)
- [Bootstrap](https://getbootstrap.com/)
- [Inno Setup](https://jrsoftware.org/isinfo.php) (for creating the installer)<br>

### Backend & Database

- [Entity Framework Core](https://learn.microsoft.com/de-de/ef/core/)
- [SQLite](https://sqlite.org/) <br>

### .NET Version: 10.0

## Installation Guide (Installer)

Install, uninstall and update GradeMaster like any other program, with the dedicated installer found in the Releases section, of this repository.

This is also the recommended approach of installing GradeMaster.

**Note**: If you currently run GradeMaster installed form the official .zip files, make sure to have uninstalled the previous version of GradeMaster manually. This includes removing the GradeMaster directory in the `C:\Program Files` directory, desktop shortcuts, taskbar pin and the start menu shortcut.

Also important GradeMaster creates specific files and directories, that are not deleted when you delete the app.

**Prominent file/directory locations:**

- `C:\Users\[YourUser]\AppData\Local\GradeMaster`
- `C:\Users\[YourUser]\OneDrive\Apps\GradeMaster`

## Installation (ZIP file) and Setup Guide

### For personal usage (Windows)

1. Install the Zip-File GradeMaster_[win-x64/win-arm64].zip (depending on your System architecture) from the Release Section. Make sure you install the newest available version.
2. Unpack the Zip-File in the `C:\Program Files\` directory.
3. After the installation you will see the folder containing the app. There you can copy the shortcut and paste it to the Desktop.
4. You can now launch the app from the shortcut.
5. Before you launch the application make sure you trust the binary, by checking the mark in the Properties tab when right clicking the file. <br/>
(File location: `C:\Program Files\GradeMaster\bin\win-x64\GradeMaster.DesktopClient.exe` <br/>
or `C:\Program Files\GradeMaster\bin\win-arm64\GradeMaster.DesktopClient.exe` for arm based systems)
6. Optional: If you like you can also copy the shortcut to the Start Menu. You can do this by copying the shortcut to this directory: `Win + R` then `%AppData%\Microsoft\Windows\Start Menu\Programs`.

### Updating Software (Windows)

1. Make sure you delete the `C:\Program Files\GradeMaster` directory. (It is recommended you make a backup of your Data by copying your GradeMaster.db file located in `C:\Users\YourUser\AppData\Local\GradeMaster\Data`. The database should remain compatible with the newer versions of the app and no data should be lost)
2. Then follow the same steps from the Installation Guide for personal use. (You do not have to copy the shortcut again. You can skip this step. Only do this step if the shortcut doesn't work anymore.)

#### Disclaimer: [Updating from Version v1.x.x to v2.x.x or higher](Docs/Update_from_version_v1.x.x_to_v2.x.x.md)

### Uninstalling the App for Users (Windows)

1. Delete the directory `C:\Program Files\GradeMaster`.
2. You can also delete the application data directory if you want to delete your data (not recommended). To do this delete the directory `C:\Users\YourUser\AppData\Local\GradeMaster`.
3. You can also delete all of the shortcuts. Delete them, where you copied them.

### For Development

1. Clone this repository to your PC.
2. Make sure you have the necessary stuff installed in Visual Studio for .NET MAUI Blazor Hybrid applications.
3. Open the Solution located in: `..YourPath\GradeMaster\Src\GradeMaster.sln` in Visual Studio.
4. Let Visual Studio start up.
5. Then make sure all the packages and dependencies are loaded.
6. Next you can build the solution.
7. After you can start the application from the start button in Visual Studio. Make sure you have the Developer-Mode activated on your device.

## Info

### Current Release Version

[![Release Version Badge](https://img.shields.io/github/v/release/tgrant06/GradeMaster)](https://github.com/tgrant06/GradeMaster/releases)
[![Downloads@latest](https://img.shields.io/github/downloads/tgrant06/GradeMaster/latest/total)](https://github.com/tgrant06/GradeMaster/releases/latest)

### Total Downloads

[![Total Downloads](https://img.shields.io/github/downloads/tgrant06/GradeMaster/total)](https://github.com/tgrant06/GradeMaster/releases)

### Price

GradeMaster is free to use, as of this moment.

### Versioning of the Releases

[Semantic Versioning](https://semver.org/) is used for GradeMaster releases.

### Development State

GradeMaster is actively maintained. <br>
New releases are generally released on a monthly basis. <br>

### Future Plans and Roadmap [all complete ✅]

- Planned Features:
  - Hot-Keys ✅
  - Notes ✅
  - Backup of Database ✅
  - Database tools ✅
  - OneDrive integration ✅

#### Future Releases

As of now GradeMaster is in active maintenance, currently on major version 3 (3.x.x). <br>
Major version 3 (3.x.x) will presumably the last major version this application will receive. After Major version 3 there will only be necessary feature updates, for missing features. After that GradeMaster will be considered as feature complete and as a result only maintenance updates are provided. <br>
What does maintenance mean? For GradeMaster maintenance means patch updates, for example: 3.0.1 to 3.0.2 containing bug fixes, updates to packages (dependencies) and other miscellaneous stuff. <br>
There might be more major and minor versions of GradeMaster, for example updating the .NET version from 10 (currently) to .NET 11 in November 2026. There might be another major version in the future, when Windows 10 will inevitably be dropped from GradeMaster support in the future, to signify incompatibility.

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

- You also have to make sure you have the [WebView2](https://developer.microsoft.com/en-us/microsoft-edge/webview2) installed on your System.
- The [WebView2](https://developer.microsoft.com/en-us/microsoft-edge/webview2) should already be pre installed on all Windows 11 and most Windows 10 devices.

#### The Desktop GUI Client is currently limited to Windows only

- Windows versions that are supported and work (tested): Windows 11, 10

Other platforms might get Support in the future.

#### Disk Space Requirement

- It is recommended to have at least 1gb of free disk space for the application.

### Supported Languages

- English

### Supported Grading Systems

- Swiss

<!--### Console Client

The Console Client is only for testing purposes.-->

## License

This project is licensed under the Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International (CC BY-NC-SA 4.0) License. For commercial use or more information, please create an Issue.

[License: CC BY-NC-SA 4.0](https://creativecommons.org/licenses/by-nc-sa/4.0/)

## Disclaimer

THIS SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE, AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES, OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT, OR OTHERWISE, ARISING FROM, OUT OF, OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

<!--Maybe add more disclaimers TBD-->
