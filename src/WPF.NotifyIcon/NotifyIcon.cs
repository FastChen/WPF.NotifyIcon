// This Source Code Form is subject to the terms of the MIT License.
// Copyright (C) FastChen and Contributors.

using static WPF.NotifyIcon.NativeNotifyIcon;

namespace WPF.NotifyIcon
{
    public class NotifyIcon
    {
        private IntPtr _iconHandle;
        private IntPtr _windowHandle;
        private NOTIFYICONDATA _data;

        private const int WM_TRAYICON = WM_USER + 1;
        public event Action LeftClick;
        public event Action RightClick;
        public event Action DoubleClick;

        // Icon 属性
        public IntPtr Icon
        {
            get => _iconHandle;
            set => SetIcon(value);
        }

        /// <summary>
        /// 裁剪提示文本，szTip 最大 128 字符（含结尾 null）
        /// </summary>
        /// <param name="tip"></param>
        /// <returns></returns>
        private string TrimTip(string tip) => tip.Length > 127 ? tip.Substring(0, 127) : tip;


        /// <summary>
        /// 从可执行文件中获取图标句柄
        /// Get Icon Handle from Executable
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public IntPtr GetIconHandleFromFile(string path)
        {
            return NativeIconExtractor.ExtractAssociatedIcon(path);
        }

        /// <summary>
        /// 设置提示内容
        /// Set ToolTip
        /// </summary>
        /// <param name="toolTip"></param>
        /// <returns></returns>
        public bool SetToolTip(string toolTip)
        {
            _data.uFlags |= NIF_TIP;
            _data.szTip = TrimTip(toolTip);

            return Shell_NotifyIcon(NIM_MODIFY, ref _data);
        }

        /// <summary>
        /// 创建托盘图标
        /// Create Tray Icon
        /// </summary>
        /// <param name="window"></param>
        /// <param name="iconHandle"></param>
        /// <param name="toolTip"></param>
        public void Create(Window window, IntPtr iconHandle, string toolTip)
        {
            Create(new WindowInteropHelper(window).EnsureHandle(), iconHandle, toolTip);
        }

        /// <summary>
        /// 创建托盘图标
        /// Create Tray Icon
        /// </summary>
        /// <param name="windowHandle"></param>
        /// <param name="iconHandle"></param>
        /// <param name="toolTip"></param>
        public void Create(IntPtr windowHandle, IntPtr iconHandle, string toolTip)
        {
            _windowHandle = windowHandle;
            _iconHandle = iconHandle;

            _data = new NOTIFYICONDATA
            {
                cbSize = (uint)Marshal.SizeOf(typeof(NOTIFYICONDATA)),
                hWnd = _windowHandle,
                uID = 100247,
                uFlags = NIF_MESSAGE | NIF_ICON | NIF_TIP,
                uCallbackMessage = WM_TRAYMESSAGE,
                hIcon = _iconHandle,
                szTip = TrimTip(toolTip),
                dwInfoFlags = 0,
                guidItem = Guid.NewGuid()
            };

            Shell_NotifyIcon(NIM_ADD, ref _data);

            // 添加消息钩子
            var source = HwndSource.FromHwnd(_data.hWnd);
            source?.AddHook(WndProc);
        }

        // 设置图标的方法
        private void SetIcon(IntPtr iconHandle)
        {
            _iconHandle = iconHandle;
            _data.uFlags |= NIF_ICON;
            _data.hIcon = _iconHandle;
            Shell_NotifyIcon(NIM_MODIFY, ref _data);
        }

        /// <summary>
        /// 显示气泡提示
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="type">0无图标、1警告、2错误、3显示设置的图标</param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public void ShowBalloonTip(int timeout, string tipTitle, string tipText, ToolTipIcon tipIcon)
        {
            if (timeout < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(timeout));
            }

            if (string.IsNullOrEmpty(tipText))
            {
                throw new ArgumentException(nameof(tipText));
            }

            _data.uFlags |= NIF_INFO;
            _data.szInfoTitle = tipTitle.Length > 63 ? tipTitle.Substring(0, 63) : tipTitle;
            _data.szInfo = tipText.Length > 255 ? tipText.Substring(0, 255) : tipText;
            _data.uTimeoutOrVersion = (uint)timeout; // 超时时间（毫秒）

            _data.dwInfoFlags = tipIcon switch
            {
                ToolTipIcon.Info => NIIF_INFO,
                ToolTipIcon.Warning => NIIF_WARNING,
                ToolTipIcon.Error => NIIF_ERROR,
                ToolTipIcon.Custom => NIIF_USER,
                _ => NIIF_NONE
            };

            Shell_NotifyIcon(NIM_MODIFY, ref _data);
        }

        public void Dispose()
        {
            Shell_NotifyIcon(NIM_DELETE, ref _data);
        }


        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == WM_TRAYICON)
            {
                switch ((int)lParam)
                {
                    case 0x0200: // WM_MOUSEMOVE
                        break;
                    case 0x0203: // WM_LBUTTONDBLCLK
                        DoubleClick?.Invoke();
                        handled = true;
                        break;
                    case 0x0201: // WM_LBUTTONDOWN
                        LeftClick?.Invoke();
                        handled = true;
                        break;
                    case 0x0204: // WM_RBUTTONDOWN
                        RightClick?.Invoke();
                        handled = true;
                        break;
                }
            }
            return IntPtr.Zero;
        }
    }

}
