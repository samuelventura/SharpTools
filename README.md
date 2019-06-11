
# C# Tools

## Development Environment

* Windows 10 Pro
* Visual Studio Code
* .NET Core SDK 2.1.500
* .NET Core SDK 3.0.100-preview5-011568

## Development Cycle

* dotnet build
* dotnet test
* dotnet pack -c Release
* dotnet nuget push SharpTools/bin/Release/SharpTools.1.0.10.nupkg -s https://api.nuget.org/v3/index.json -k $KEY
* dotnet nuget push SharpToolsUI/bin/Release/SharpToolsUI.1.0.10.nupkg -s https://api.nuget.org/v3/index.json -k $KEY
* https://www.nuget.org/account/apikeys
* ```find . -name bin -exec rm -fr {} \;```
* ```find . -name obj -exec rm -fr {} \;```
