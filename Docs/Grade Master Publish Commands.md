# GradeMaster Publish commands

## GradeMaster Publish commands for MSIX

**win-x64:**

```ps1
dotnet publish -c Release -f net9.0-windows10.0.19041.0 -r win-x64 --self-contained --output ./publish_win-x64 -p:PublishProfile=msix-publish
```

**win-arm64:**

```ps1
dotnet publish -c Release -f net9.0-windows10.0.19041.0 -r win-x64 --self-contained --output ./publish_win-x64 -p:PublishProfile=msix-publish
```

## GradeMaster Publish commands standard

**win-x64:**

```ps1
dotnet publish -c Release -f net9.0-windows10.0.19041.0 -r win-x64 --self-contained -o bin/Publish/win-x64/GradeMaster/bin/win-x64
```

**win-arm64:**

```ps1
dotnet publish -c Release -f net9.0-windows10.0.19041.0 -r win-arm64 --self-contained -o bin/Publish/win-arm64/GradeMaster/bin/win-arm64
```
