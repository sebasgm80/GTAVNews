@echo off
cd /d "%~dp0"
dotnet build -c Release -f uap10.0
echo Build completado. El .msix esta en: bin\x64\Release\Appx\
pause