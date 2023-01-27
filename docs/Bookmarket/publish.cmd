@echo off
cd
set srcdir=Bookmarket.UI
set pubdir=..\..\bookmarket-pub\Release
dotnet cake --target="Version"

type Directory.Build.props
cd Bookmarket.UI
echo publishing %srcdir% to %pubdir%...
pause

dotnet publish -o %pubdir%

pause

cd ..
