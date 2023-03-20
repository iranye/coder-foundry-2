@echo off
cd
set srcdir=MediaManager.WPF
set pubdir=..\..\mediamanager-pub\Release
dotnet cake --target="Version"

type Directory.Build.props
cd %srcdir%
echo publishing %srcdir% to %pubdir%...
pause

:: dotnet publish -o C:\src\NewStuff\Pluralsight\Thomas\WPF6-Fundamentals\WPF6-Fundamentals\WorkTool.WPF\pub\Release
dotnet publish -o %pubdir%

pause

dotnet publish -o %pubdir%

cd ..
