## GradeMaster Publish commands for msix:
win-x64: <code>dotnet publish -c Release -f net9.0-windows10.0.19041.0 -r win-x64 --self-contained --output ./publish_win-x64 -p:PublishProfile=msix-publish</code> 

win-arm64: <code>dotnet publish -c Release -f net9.0-windows10.0.19041.0 -r win-x64 --self-contained --output ./publish_win-x64 -p:PublishProfile=msix-publish</code> 

## GradeMaster Publish commands normal:
win-x64: <code>dotnet publish -c Release -f net9.0-windows10.0.19041.0 -r win-x64 --self-contained -o bin/Publish/win-x64/GradeMaster/bin/win-x64</code> 

win-arm64: <code>dotnet publish -c Release -f net9.0-windows10.0.19041.0 -r win-arm64 --self-contained -o bin/Publish/win-arm64/GradeMaster/bin/win-arm64</code>  