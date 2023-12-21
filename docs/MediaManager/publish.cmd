@echo off
cd
set srcdir=MediaManager.WPF
set pubdir=..\..\mediamanager-pub\MediaManager.pub
dotnet cake --target="Version"

type Directory.Build.props
cd %srcdir%
echo publishing %srcdir% to %pubdir%...
pause

dotnet publish -o %pubdir%

pause

echo 7zproject %pubdir% %pubdir%\..
7zproject %pubdir% %pubdir%\..

cd ..
