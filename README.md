
# C# Tools

## Development Environment

* Windows 10 Pro
* .NET Core SDK 3.1.101
* Visual Studio Code with pwsh
* Git for Windows + TortoiseGit

## Development Cycle

* dotnet build
* dotnet test
* dotnet pack -c Release
* dotnet nuget push SharpTools/bin/Release/SharpTools.1.0.11.nupkg -s https://api.nuget.org/v3/index.json -k $KEY
* dotnet nuget push SharpToolsUI/bin/Release/SharpToolsUI.1.0.11.nupkg -s https://api.nuget.org/v3/index.json -k $KEY
* https://www.nuget.org/account/apikeys
* get-childitem -include bin -recurse -force | remove-item -force -recurse
* get-childitem -include obj -recurse -force | remove-item -force -recurse

## Improvement Ideas

* Unhandled exception catcher for console apps
* Fluent API for constructors with many arguments
* Atomic action execution
