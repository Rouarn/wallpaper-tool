@echo off
chcp 65001 >nul
echo Updating wallpaper...
powershell -ExecutionPolicy Bypass -File "%~dp0download_wallpaper.ps1"
if %ERRORLEVEL% EQU 0 (
    echo Success! Window will close in 3 seconds.
    timeout /t 3 >nul
) else (
    echo Failed! Press any key to exit.
    pause
)
