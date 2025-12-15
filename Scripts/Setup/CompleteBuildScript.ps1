# ==============================================================================
# 🚀 Build & Packaging Script for GradeMaster.DesktopClient
# ==============================================================================

# --- Helper Configuration Variables ---

$win_x64 = "win-x64"
$win_arm64 = "win-arm64"
$AppVersion = "v3.3.1"
$ArchitectureParam = $win_x64 # Default architecture
$ContainmentParam = "self_contained" # Default containment
$OutputSubDir = "bin" # New subdirectory introduced by the output path change

# --- Path Variables ---

$ProjectDir = "$(Get-Location)\..\..\Src\Client\GradeMaster.DesktopClient"
$OutputDir = "$(Get-Location)\..\..\Src\Client\GradeMaster.DesktopClient\bin\Publish"

# Inno Setup Paths (Only needed for final packaging step)
$InnoSetupMachine = "C:\Program Files (x86)\Inno Setup 6\ISCC.exe"
$InnoSetupUser = "$($env:USERPROFILE)\AppData\Local\Programs\Inno Setup 6\ISCC.exe"
$InnoSetupPath = $InnoSetupMachine
if (-not ([System.IO.File]::Exists($InnoSetupMachine))) {
    $InnoSetupPath = $InnoSetupUser
}

# Inno Setup Script Files (ISS paths remain relative to the script location)
$ISS_x64_Lean = "$(Get-Location)\..\InnoSetup\GradeMaster_win-x64_setup_Lean.iss"
$ISS_x64 = "$(Get-Location)\..\InnoSetup\GradeMaster_win-x64_setup.iss"
$ISS_arm64_Lean ="$(Get-Location)\..\InnoSetup\GradeMaster_win-arm64_setup_Lean.iss"
$ISS_arm64 = "$(Get-Location)\..\InnoSetup\GradeMaster_win-arm64_setup.iss"

# NOTE: THE SOURCE FOLDER PATHS ARE UPDATED TO REFLECT THE NEW DEEPER NESTING
# The new structure is: .../bin/Publish/{runtime}/GradeMaster/bin/{runtime}

$SourceFolderX64 = "$($OutputDir)\$($win_x64)\GradeMaster\$($OutputSubDir)\$($win_x64)"
$SourceFolderArm64 = "$($OutputDir)\$($win_arm64)\GradeMaster\$($OutputSubDir)\$($win_arm64)"


# --- Dotnet Argument Definitions (Updated) ---
# The last argument (-o) now ends with /bin/{runtime}

$DotnetArgs_x64_sc = @("publish", "-c", "Release", "-f", "net10.0-windows10.0.19041.0", "-r", "win-x64", "--self-contained", "true", "-o", "bin/Publish/win-x64/GradeMaster/bin/win-x64")
$DotnetArgs_x64_fd = @("publish", "-c", "Release", "-f", "net10.0-windows10.0.19041.0", "-r", "win-x64", "--self-contained", "false", "-o", "bin/Publish/win-x64/GradeMaster/bin/win-x64")
$DotnetArgs_arm64_sc = @("publish", "-c", "Release", "-f", "net10.0-windows10.0.19041.0", "-r", "win-arm64", "--self-contained", "true", "-o", "bin/Publish/win-arm64/GradeMaster/bin/win-arm64")
$DotnetArgs_arm64_fd = @("publish", "-c", "Release", "-f", "net10.0-windows10.0.19041.0", "-r", "win-arm64", "--self-contained", "false", "-o", "bin/Publish/win-arm64/GradeMaster/bin/win-arm64")


# ==============================================================================
# 🛠️ Helper Functions (Remain the same)
# ==============================================================================

function Invoke-DotnetPublish {
    param(
        [Parameter(Mandatory=$true)][string]$PathToProject,
        [Parameter(Mandatory=$true)][string[]]$Arguments
    )

    Write-Host "--- Starting dotnet publish ---" -ForegroundColor Yellow
    Push-Location -Path $PathToProject

    try {
        Write-Host "Architecture: '$($Arguments[6])', Containment: '$($Arguments[8])'"
        dotnet @Arguments
        
        if ($LASTEXITCODE -ne 0) {
            throw "dotnet publish failed with exit code $LASTEXITCODE."
        }
        Write-Host "✅ Publish successful." -ForegroundColor Green
    }
    catch {
        Write-Error "❌ Error: $($_.Exception.Message)"
        exit 1
    }
    finally {
        Pop-Location
    }
}

function Invoke-Packager {
    param(
        [Parameter(Mandatory=$true)][string]$SourceDirectory,
        [Parameter(Mandatory=$true)][string]$Architecture,
        [Parameter(Mandatory=$true)][string]$Containment,
        [string[]]$Args # $Args here contains the sanitized/CleanArgs
    )

    # 1. ZIP PACKAGING
    if ("zip" -in $Args) {
        Write-Host "--- Creating ZIP Package ---" -ForegroundColor Cyan
        
        # Construct the final ZIP path and name
        $ZipFileName = "GradeMaster_$($AppVersion)_$($Architecture)_$($Containment).zip"
        $DestinationZipPath = Join-Path -Path $OutputDir -ChildPath $Architecture
        $DestinationZip = Join-Path -Path $DestinationZipPath -ChildPath $ZipFileName
        
        try {
            Compress-Archive -Path "$SourceDirectory\*" -DestinationPath $DestinationZip -Force
            Write-Host "✅ Created ZIP: $DestinationZip" -ForegroundColor Green
        }
        catch {
             Write-Error "❌ Error creating ZIP file: $($_.Exception.Message)"
        }
    }
    
    # 2. INNOSETUP PACKAGING
    if ("isi" -in $Args -or (-not ("zip" -in $Args) -and -not ("isi" -in $Args))) {
        Write-Host "--- Building InnoSetup Installer ---" -ForegroundColor Cyan

        $ISS_File = $null
        
        # Determine the Base ISS file based on Architecture
        if ($Architecture -eq $win_x64) {
            #$ISS_File = $ISS_x64

            if ("slim" -in $Args) {
                $ISS_File = $ISS_x64_Lean
            } else {
                $ISS_File = $ISS_x64
            }

        } else { # arm64
            #$ISS_File = $ISS_arm64
            
            if ("slim" -in $Args) {
                $ISS_File = $ISS_arm64_Lean
            } else {
                $ISS_File = $ISS_arm64
            }
        }
        
        try {
            Write-Host "Compiling Inno Setup Script: $ISS_File" -ForegroundColor DarkGray
            & $InnoSetupPath $ISS_File
            Write-Host "✅ InnoSetup Installer build successful." -ForegroundColor Green
        }
        catch {
             Write-Error "❌ Error building InnoSetup installer: $($_.Exception.Message)"
        }
    }
}

# ==============================================================================
# ⚙️ Pre-Processing and Argument Parsing (Remain the same)
# ==============================================================================

# Sanitize arguments: remove leading hyphens for simpler matching
$CleanArgs = @()
for ($i = 0; $i -lt $args.length; $i++)
{
    $CleanArgs += $args[$i].Replace('-', '')
}

# Help check
if ("h" -in $CleanArgs -or "help" -in $CleanArgs)
{
    Write-Host ""
    Write-Host "CompleteBuildScript Help: "
    Write-Host "---"
    Write-Host "Valid Parameters:"
    Write-Host "    - Architecture:                           --win-x64 (default) --win-arm64"
    Write-Host "    - .NET runtime containment:               --self_contained (default) --framework_dependant"
    Write-Host "    - Final output type:                      --isi (default (InnoSetup installer)) --zip"
    Write-Host "    - (If InnoSetup selected) Installer type: --slim (default) [for a slim-installer (recommended only for local usage)]"
    return
}

# Determine Architecture (Default is x64)
if ($win_arm64 -in $CleanArgs) {
    $ArchitectureParam = $win_arm64
}

# Determine Containment (Default is self-contained)
if ("framework_dependant" -in $CleanArgs) {
    $ContainmentParam = "framework_dependant"
}


# --- Main Configuration Selection ---

$SourceFolder = $null
$DotnetArgs = $null

if ($ArchitectureParam -eq $win_x64) {
    # UPDATED $SourceFolderX64 is used here
    $SourceFolder = $SourceFolderX64
    if ($ContainmentParam -eq "self_contained") {
        $DotnetArgs = $DotnetArgs_x64_sc
    } else {
        $DotnetArgs = $DotnetArgs_x64_fd
    }
} else { # win-arm64
    # UPDATED $SourceFolderArm64 is used here
    $SourceFolder = $SourceFolderArm64
    if ($ContainmentParam -eq "self_contained") {
        $DotnetArgs = $DotnetArgs_arm64_sc
    } else {
        $DotnetArgs = $DotnetArgs_arm64_fd
    }
}


# ==============================================================================
# 🎯 MAIN EXECUTION FLOW (Remain the same)
# ==============================================================================

Write-Host "`n=================================================="
Write-Host "  Starting Build Script for $($ArchitectureParam) ($($ContainmentParam))"
Write-Host "==================================================`n"

# 1. CLEAN
Write-Host "--- Cleaning Output Directory ---" -ForegroundColor Yellow
if ([System.IO.Directory]::Exists($SourceFolder)) {
    [System.IO.Directory]::Delete($SourceFolder, $true)
    Write-Host "🗑️ Deleted existing directory: $SourceFolder" -ForegroundColor DarkGray
} else {
    Write-Host "Directory does not exist, proceeding." -ForegroundColor DarkGray
}

# 2. PUBLISH
Invoke-DotnetPublish -PathToProject $ProjectDir -Arguments $DotnetArgs

# 3. PACKAGE
Invoke-Packager -SourceDirectory $SourceFolder -Architecture $ArchitectureParam -Containment $ContainmentParam -Args $CleanArgs

Write-Host "`n=================================================="
Write-Host "  ✅ Build and Packaging Complete"
Write-Host "=================================================="