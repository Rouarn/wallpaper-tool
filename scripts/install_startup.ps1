$WshShell = New-Object -comObject WScript.Shell
$StartupDir = $WshShell.SpecialFolders.Item("Startup")
$ShortcutPath = "$StartupDir\UpdateTerminalWallpaper.lnk"

# 目标指向新生成的 EXE 文件
# 假设 install_startup.ps1 在 scripts 目录，EXE 在 bin 目录
$TargetExe = Join-Path $PSScriptRoot "..\bin\WallpaperTool.exe"
$TargetExe = [System.IO.Path]::GetFullPath($TargetExe)

try {
    if (-not (Test-Path $TargetExe)) {
        throw "在 $TargetExe 未找到 WallpaperTool.exe。请先构建项目。"
    }

    $Shortcut = $WshShell.CreateShortcut($ShortcutPath)
    # 直接指向 EXE 文件
    $Shortcut.TargetPath = $TargetExe
    # 添加 -silent 参数以触发后台自动下载逻辑
    $Shortcut.Arguments = "-silent"
    $Shortcut.WorkingDirectory = [System.IO.Path]::GetDirectoryName($TargetExe)
    $Shortcut.Description = "登录时自动更新终端壁纸"
    $Shortcut.Save()

    Write-Host "启动快捷方式已成功创建于："
    Write-Host $ShortcutPath
    Write-Host "`n壁纸工具现在将在您登录时自动（静默）运行。"
} catch {
    Write-Error "创建启动快捷方式失败: $_"
}
