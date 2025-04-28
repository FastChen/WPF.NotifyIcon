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

        var icon = _notifyIcon.GetIconHandleFromFile(Environment.ProcessPath);
        _notifyIcon.Create(this, icon, "HELLO!\nI'm here!");

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

            default:
                toolTipIcon = ToolTipIcon.None;
                break;
        }

        _notifyIcon.ShowBalloonTip(5000, title, message, toolTipIcon);
    }

}