@echo off
cd
set srcdir=Bookmarket.UI
set pubdir=..\pub\Release
dotnet cake --target="Version"

type Directory.Build.props
cd Bookmarket.UI
echo publishing %srcdir% to %pubdir%...
pause

dotnet publish -o ..\pub\Release

pause

cd ..
