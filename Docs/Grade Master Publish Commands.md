# GradeMaster Publish commands

## GradeMaster Publish commands for MSIX

**win-x64:**

```console
dotnet publish -c Release -f net9.0-windows10.0.19041.0 -r win-x64 --self-contained --output ./publish_win-x64 -p:PublishProfile=msix-publish
```

**win-arm64:**

```console
dotnet publish -c Release -f net9.0-windows10.0.19041.0 -r win-x64 --self-contained --output ./publish_win-x64 -p:PublishProfile=msix-publish
```

## GradeMaster Publish commands standard .NET 10

**win-x64:**

```console
dotnet publish -c Release -f net10.0-windows10.0.19041.0 -r win-x64 --self-contained -o bin/Publish/win-x64/GradeMaster/bin/win-x64
```

**win-arm64:**

```console
dotnet publish -c Release -f net10.0-windows10.0.19041.0 -r win-arm64 --self-contained -o bin/Publish/win-arm64/GradeMaster/bin/win-arm64
```

## GradeMaster Publish commands standard .NET 9

**win-x64:**

```console
dotnet publish -c Release -f net9.0-windows10.0.19041.0 -r win-x64 --self-contained -o bin/Publish/win-x64/GradeMaster/bin/win-x64
```

**win-arm64:**

```console
dotnet publish -c Release -f net9.0-windows10.0.19041.0 -r win-arm64 --self-contained -o bin/Publish/win-arm64/GradeMaster/bin/win-arm64
```

## GradeMaster Publish commands with Ready2Run

**win-x64:**

```console
dotnet publish -c Release -f net9.0-windows10.0.19041.0 -r win-x64 --self-contained -p:PublishReadyToRun=true -o bin/Publish/win-x64/GradeMaster/bin/win-x64
```

**win-arm64:**

```console
dotnet publish -c Release -f net9.0-windows10.0.19041.0 -r win-arm64 --self-contained -p:PublishReadyToRun=true -o bin/Publish/win-arm64/GradeMaster/bin/win-arm64
```

## GradeMaster Publish commands with Ready2Run and with trimming

> [!CAUTION]
> Do not use Trimming currently, until incompatibilites are resolved!
> Compiling the app with trimming will not work and produce faulty binaries!

**win-x64:**

```console
dotnet publish -c Release -f net9.0-windows10.0.19041.0 -r win-x64 --self-contained -p:PublishReadyToRun=true -p:PublishTrimmed=true -o bin/Publish/win-x64/GradeMaster/bin/win-x64
```

**win-arm64:**

```console
dotnet publish -c Release -f net9.0-windows10.0.19041.0 -r win-arm64 --self-contained -p:PublishReadyToRun=true -p:PublishTrimmed=true -o bin/Publish/win-arm64/GradeMaster/bin/win-arm64
```
