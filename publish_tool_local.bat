dotnet pack ExtCopy -c Release -o ../DotnetPack

dotnet tool uninstall -g ExtCopy

dotnet tool install -g ExtCopy --add-source ../DotnetPack

pause
