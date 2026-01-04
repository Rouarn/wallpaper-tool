@echo off
set CSC_PATH=C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe

if not exist "%CSC_PATH%" (
    echo "Error: csc.exe not found at %CSC_PATH%"
    pause
    exit /b 1
)

echo Found csc.exe at %CSC_PATH%
echo Compiling WallpaperApp.cs...

if not exist bin mkdir bin

"%CSC_PATH%" /target:winexe /out:bin\WallpaperTool.exe /win32icon:src\app.ico /win32manifest:src\app.manifest src\WallpaperApp.cs

if %ERRORLEVEL% EQU 0 (
    echo.
    echo Build SUCCESS!
    echo Created: bin\WallpaperTool.exe
    echo.
) else (
    echo.
    echo Build FAILED!
)
pause
