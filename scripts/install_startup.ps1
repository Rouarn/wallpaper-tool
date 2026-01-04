$WshShell = New-Object -comObject WScript.Shell
$StartupDir = $WshShell.SpecialFolders.Item("Startup")
$ShortcutPath = "$StartupDir\UpdateTerminalWallpaper.lnk"
$TargetScript = Join-Path $PSScriptRoot "download_wallpaper.ps1"

try {
    $Shortcut = $WshShell.CreateShortcut($ShortcutPath)
    # 使用 PowerShell 运行脚本，并设置为 Hidden 窗口模式（静默运行）
    $Shortcut.TargetPath = "powershell.exe"
    $Shortcut.Arguments = "-ExecutionPolicy Bypass -WindowStyle Hidden -File `"$TargetScript`""
    $Shortcut.WorkingDirectory = $PSScriptRoot
    $Shortcut.Description = "Auto-update Terminal wallpaper on login"
    $Shortcut.Save()

    Write-Host "Startup shortcut successfully created at:"
    Write-Host $ShortcutPath
    Write-Host "`nThe wallpaper tool will now run automatically when you log in."
} catch {
    Write-Error "Failed to create startup shortcut: $_"
}
