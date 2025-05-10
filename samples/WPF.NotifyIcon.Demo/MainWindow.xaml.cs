using System.Windows;
using System.Windows.Controls;

namespace WPF.NotifyIcon.Demo;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private NotifyIcon _notifyIcon = new();
    private bool canExit = false;
    private ContextMenu trayContextMenu;

    public MainWindow()
    {
        InitializeComponent();

        TextBox_ToolTip.Text = $"Hello!{Environment.NewLine}I'm here!";

        // 通过文件获取图标句柄
        // Get icon handle from executable
        var icon = _notifyIcon.GetIconHandleFromFile(Environment.ProcessPath);

        // 向闪烁图标列表添加图标句柄
        // Add icon handle to blink icon list
        _blinkIcons = new ()
        {
            icon,
            IntPtr.Zero
        };

        // 创建托盘图标
        // Create NotifyIcon
        _notifyIcon.Create(this, icon, "HELLO!\nI'm here!");

        // 设置托盘左右键单击事件
        // Set left and right click events
        _notifyIcon.LeftClick += ShowWindow;
        _notifyIcon.RightClick += ShowContextMenu;

    }

    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        if (!canExit)
        {
            e.Cancel = true;
            this.Hide();

            _notifyIcon.ShowBalloonTip(5000, "提示 | INFO", "程序已最小化到系统托盘\nTray in taskbar", ToolTipIcon.Info);
        }
        else
        {
            _notifyIcon.ShowBalloonTip(5000, "", "ByeBye~", ToolTipIcon.None);
        }
    }

    private void ShowWindow()
    {
        this.Show();
        this.WindowState = WindowState.Normal;
        this.Activate();
    }

    private void ShowContextMenu()
    {
        trayContextMenu = new ContextMenu();

        var showMenuItem = new MenuItem();
        showMenuItem.Header = "显示 | Show";
        showMenuItem.Click += (s, e) => ShowWindow();
        trayContextMenu.Items.Add(showMenuItem);

        var exitMenuItem = new MenuItem();
        exitMenuItem.Header = "退出 | Close";
        exitMenuItem.Click += (s, e) => { canExit = true; this.Close(); };
        trayContextMenu.Items.Add(exitMenuItem);

        trayContextMenu.IsOpen = true;
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        Button button = sender as Button;
        string tag = button.Tag.ToString();

        var toolTipIcon = ToolTipIcon.None;
        string title = TextBox_Title.Text;
        string message = TextBox_Message.Text;

        switch (tag)
        {
            case "INFO":
                toolTipIcon = ToolTipIcon.Info;
                break;

            case "WARNING":
                toolTipIcon = ToolTipIcon.Warning;
                break;

            case "ERROR":
                toolTipIcon = ToolTipIcon.Error;
                break;

            case "CUSTOM":
                toolTipIcon = ToolTipIcon.Custom;
                break;

            case "CLOSE":
                canExit = true;
                this.Close();
                return;

            case "SetToolTip":
                _notifyIcon.SetToolTip(TextBox_ToolTip.Text);
                return;

            default:
                toolTipIcon = ToolTipIcon.None;
                break;
        }

        _notifyIcon.ShowBalloonTip(5000, title, message, toolTipIcon);
    }

    private void Button_Blink_Click(object sender, RoutedEventArgs e)
    {
        Button button = sender as Button;
        string tag = button.Tag.ToString();

        switch (tag)
        {
            case "StartBlink":
                StartBlink();
                break;
            case "StopBlink":
                StopBlink();
                break;
        }
    }

    // new NotifyIcon.Icon 属性的 get; set; 使用示例：图标闪烁
    // Example of using the `new NotifyIcon.Icon property get; set;` Icon blinking

    private List<IntPtr> _blinkIcons;

    private System.Timers.Timer _blinkTimer;
    private readonly double _blinkInterval = 300;
    private int _blinkIconCurrentIndex;
    private bool _ascending = true;

    public void StartBlink()
    {
        if (_blinkTimer != null) return;

        _blinkIconCurrentIndex = 0;
        _blinkTimer = new System.Timers.Timer { Interval = _blinkInterval };

        _blinkTimer.Elapsed += OnTimerTick;
        _blinkTimer.Start();
    }

    public void StopBlink()
    {
        if (_blinkTimer == null) return;

        _blinkTimer.Stop();
        _blinkTimer = null;

        // 恢复到第一个图标
        _notifyIcon.Icon = _blinkIcons[0];
    }


    private void OnTimerTick(object sender, EventArgs e)
    {
        // 循环列表中的图标
        // Simple loop through the icons in the list
        //_blinkIconCurrentIndex = (_blinkIconCurrentIndex + 1) % _blinkIcons.Count;
        //_notifyIcon.Icon = _blinkIcons[_blinkIconCurrentIndex];

        // 往返模式（更平滑的过渡效果）
        // Round trip mode (smoother transition effect)

        // set the icon
        _notifyIcon.Icon = _blinkIcons[_blinkIconCurrentIndex];

        if (_ascending)
        {
            if (_blinkIconCurrentIndex < _blinkIcons.Count - 1)
                _blinkIconCurrentIndex++;
            else
                _ascending = false;
        }
        else
        {
            if (_blinkIconCurrentIndex > 0)
                _blinkIconCurrentIndex--;
            else
                _ascending = true;
        }
    }

}