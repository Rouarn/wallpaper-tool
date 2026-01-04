# Wallpaper Tool (壁纸自动更新工具)

这是一个简单而强大的 Windows 壁纸下载工具，专为配合 Windows Terminal 背景更新而设计，但也适用于通用的壁纸下载需求。它提供了一个原生 Windows .exe 应用程序，支持图形界面配置和开机静默自动运行。

## ✨ 主要功能

- **原生 Windows 应用**: 基于 .NET Framework (C# WinForms) 开发，无需安装额外运行时（Windows 自带支持）。
- **图形化配置界面**: 方便地设置保存目录、下载 URL 和日志文件名。
- **开机静默更新**:
  - 当作为开机启动项运行时，程序会自动在后台静默下载壁纸并退出，不干扰用户。
  - 支持通过命令行参数 `-silent` 手动触发静默模式。
- **高 DPI 适配**: 完美支持高分辨率屏幕，界面清晰不模糊。
- **动态界面调整**: 窗口大小和布局会根据屏幕分辨率自动调整，提供最佳视觉体验。
- **中文支持**: 全中文界面和日志记录，解决了常见的乱码问题。
- **日志记录**: 详细记录每次运行的时间和结果，方便排查问题。

## 📂 项目结构

```
wallpaper-tool/
├── bin/                # 编译后的可执行文件 (WallpaperTool.exe) 和日志
├── src/                # 源代码目录
│   ├── WallpaperApp.cs # 主程序代码
│   ├── app.ico         # 应用程序图标
│   └── app.manifest    # 应用程序清单 (DPI 适配)
├── scripts/            # 辅助脚本
│   ├── download_wallpaper.ps1 # PowerShell 下载逻辑 (旧版核心)
│   ├── install_startup.ps1    # 安装开机启动项脚本
│   └── run_now.bat            # 快速运行脚本
├── build_app.bat       # 编译脚本
└── README.md           # 项目文档
```

## 🚀 快速开始

### 1. 编译项目 (首次使用)

如果你下载的是源码，请先运行根目录下的编译脚本生成可执行文件：

双击运行 `build_app.bat`

编译成功后，可执行文件将生成在 `bin\WallpaperTool.exe`。

### 2. 正常运行

双击 `bin\WallpaperTool.exe` 即可打开图形界面。

- **保存目录**: 默认为 "Saved Pictures"，你可以点击“浏览”修改。
- **下载地址**: 默认为 `https://picsum.photos/1920/1080`，你可以修改为任何你喜欢的图片 URL。
- **立即下载**: 点击按钮即可测试下载。

### 3. 设置开机自动更新

为了实现每天开机自动更新壁纸（例如用于 Windows Terminal 背景），请运行安装脚本：

1.  进入 `scripts` 目录。
2.  右键点击 `install_startup.ps1`。
3.  选择 **"使用 PowerShell 运行"**。

脚本会在你的启动文件夹中创建一个快捷方式，指向 `WallpaperTool.exe` 并带有 `-silent` 参数。下次开机时，它将自动在后台更新壁纸。

### 4. 手动脚本运行

你也可以直接运行 `scripts\run_now.bat` 来快速触发更新（主要用于测试旧版脚本逻辑）。

## 🛠️ 技术细节

- **语言**: C# (WinForms), PowerShell, Batch
- **编译器**: csc.exe (Windows 内置 .NET Framework 编译器)
- **依赖**: 无需额外安装，Windows 7/10/11 均可直接运行。

## 📝 常见问题

**Q: 运行时出现中文乱码？**
A: 我们已经在代码中强制使用了 UTF-8 编码，并配置了控制台输出编码，确保在中文 Windows 环境下正常显示。

**Q: 如何修改默认下载源？**
A: 在图形界面中修改 URL 即可。程序启动时会加载默认值，你也可以在源码 `src\WallpaperApp.cs` 中修改 `defaultUrl` 变量并重新编译。

**Q: 开机启动没有反应？**
A: 开机启动模式下程序是**静默运行**的，不会显示窗口。你可以查看 `bin\wallpaper_log.txt` 日志文件确认运行情况。
