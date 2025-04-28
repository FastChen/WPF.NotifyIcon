// This Source Code Form is subject to the terms of the MIT License.
// Copyright (C) FastChen and Contributors.

namespace WPF.NotifyIcon
{
    class NativeNotifyIcon
    {

        public const int WM_USER = 0x0400;
        public const int WM_TRAYMESSAGE = WM_USER + 1;

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct NOTIFYICONDATA
        {
            public uint cbSize;
            public IntPtr hWnd;
            public uint uID;
            public uint uFlags;
            public uint uCallbackMessage;
            public IntPtr hIcon;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string szTip;

            public uint dwState;
            public uint dwStateMask;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string szInfo;

            public uint uTimeoutOrVersion;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string szInfoTitle;

            public uint dwInfoFlags;

            public Guid guidItem;

            public IntPtr hBalloonIcon;
        }

        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        public static extern bool Shell_NotifyIcon(uint dwMessage, [In] ref NOTIFYICONDATA lpData);

        public const uint NIM_ADD = 0x00000000;
        public const uint NIM_MODIFY = 0x00000001;
        public const uint NIM_DELETE = 0x00000002;

        public const uint NIF_MESSAGE = 0x00000001;
        public const uint NIF_ICON = 0x00000002;
        public const uint NIF_TIP = 0x00000004;
        public const uint NIF_INFO = 0x00000010;

        public const int NIIF_NONE = 0x00000000; // 无图标
        public const int NIIF_INFO = 0x00000001; // 信息图标（蓝色 i）
        public const int NIIF_WARNING = 0x00000002; // 警告图标（黄色感叹号）
        public const int NIIF_ERROR = 0x00000003; // 错误图标（红色叉）
        public const int NIIF_USER = 0x00000004; // 使用自定义图标（来自 hIcon）

        // 可选标志位
        public const int NIIF_NOSOUND = 0x00000010; // 不播放提示音
        public const int NIIF_LARGE_ICON = 0x00000020; // 使用大图标（Win Vista+）
        public const int NIIF_RESPECT_QUIET_TIME = 0x00000080; // 尊重“安静时间”（不打扰用户）
    }


}