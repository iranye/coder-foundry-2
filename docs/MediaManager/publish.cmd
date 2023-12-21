@echo off
cd
set srcdir=MediaManager.WPF
set pubdir=..\..\mediamanager-pub\Release
set zipdir=..\..\mediamanager-pub\MediaManager.pub
dotnet cake --target="Version"

type Directory.Build.props
cd %srcdir%
echo publishing %srcdir% to %pubdir%...
pause

:: dotnet publish -o C:\src\NewStuff\Pluralsight\Thomas\WPF6-Fundamentals\WPF6-Fundamentals\WorkTool.WPF\pub\Release
dotnet publish -o %pubdir%

pause

echo robocopy %pubdir% %zipdir% /mir /tee /v
pause
robocopy %pubdir% %zipdir% /mir /tee /v

echo 7zproject %zipdir% ..
7zproject %zipdir% %zipdir%\..

cd ..
