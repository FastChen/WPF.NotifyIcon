<div align="center">
  <img src="./images/logo_256x.png" alt="WPF.NotifyIcon" height="160" width="160"/>
  <h1>WPF.NotifyIcon</h1>
  <h3>WPF Native NotifyIcon</h3>
  <img src="https://img.shields.io/github/stars/fastchen/WPF.NotifyIcon?label=Star&logo=github"/>
  <a href="https://github.com/FastChen/WPF.NotifyIcon/issues"><img src="https://img.shields.io/github/issues/fastchen/WPF.NotifyIcon?label=Issues"/></a>
  <h4><a href="./README.md">中文</a> / English</h4>
</div>

## 特点

- Only 10Kb Size
- Use WinAPI `Shell_NotifyIcon`
- Support .NET4.62 Framework to .NET9
- Windows BalloonTip
- Like WinForm NotifyIcon Control

## Usage

**Step 1:** Install Package `WPF.NotifyIcon` on Nuget

CommandLine: `Install-Package WPF.NotifyIcon`

Visual Studio Nuget: 
![](images\nuget.png)

**Step 2:** NotifyIcon = new();

> [!TIP]
> you can check `samples\WPF.NotifyIcon.Demo` get more info.


```c#
// Create new NotifyIcon
private NotifyIcon _notifyIcon = new();

public MainWindow()
{
    InitializeComponent();

    // Get Icon.Handle from self
    var icon = _notifyIcon.GetIconHandleFromFile(Environment.ProcessPath);

    // Create Tray in taskbar
    _notifyIcon.Create(this, icon, "HELLO!\nI'm here!");

    // Action Event
    _notifyIcon.LeftClick += ShowWindow;
    _notifyIcon.RightClick += ShowContextMenu;

    // Popup ShowBalloonTip
    _notifyIcon.ShowBalloonTip(5000, "Hello!", "WPF.NotifyIcon!", ToolTipIcon.None);

    // Update ToolTip
     _notifyIcon.SetToolTip("Wow!");
}
```

the `ToolTipIcon` Support icon list:

```c#
public enum ToolTipIcon
{
    /// <summary>
    /// No icon.
    /// </summary>
    None = 0,

    /// <summary>
    /// Info icon.
    /// </summary>
    Info = 1,

    /// <summary>
    /// Warning icon.
    /// </summary>
    Warning = 2,

    /// <summary>
    /// Error icon.
    /// </summary>
    Error = 3,

    /// <summary>
    /// Use Application icon.
    /// </summary>
    Custom = 4,
}
```

## Images

### ToolTip:

![ToolTip](images\ToolTip.png)

### TrayMenu:

![ToolTip](images\TrayMenu.png)

### BalloonTip:

**Light:**

![ToolTip](images\None.png)
![ToolTip](images\Info.png)
![ToolTip](images\Warning.png)
![ToolTip](images\Error.png)
![ToolTip](images\Custom.png)

**Dark:**

![ToolTip](images\Dark_None.png)
![ToolTip](images\Dark_Info.png)
![ToolTip](images\Dark_Warning.png)
![ToolTip](images\Dark_Error.png)
![ToolTip](images\Dark_Custom.png)