# 配置参数
$destDir = "C:\Users\29373\Pictures\Saved Pictures"
$destFile = "$destDir\wallpaper_Terminal.jpg"
$url = "https://picsum.photos/1920/1080"
$logFile = "$PSScriptRoot\..\bin\wallpaper_log.txt"

# 获取当前时间用于日志
$date = Get-Date -Format "yyyy-MM-dd HH:mm:ss"

try {
    # 确保目录存在
    if (-not (Test-Path $destDir)) {
        New-Item -ItemType Directory -Path $destDir -Force | Out-Null
    }

    Write-Host "[$date] 正在从 $url 下载壁纸..."
    
    # 下载图片
    Invoke-WebRequest -Uri $url -OutFile $destFile -UseBasicParsing
    
    $msg = "[$date] 成功: 壁纸已更新至 $destFile"
    Write-Host $msg
    Add-Content -Path $logFile -Value $msg

} catch {
    $errorMsg = "[$date] 错误: 下载壁纸失败。详情: $_"
    Write-Error $errorMsg
    Add-Content -Path $logFile -Value $errorMsg
}
