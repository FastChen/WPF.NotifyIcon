// This Source Code Form is subject to the terms of the MIT License.
// Copyright (C) FastChen and Contributors.

namespace WPF.NotifyIcon
{
    class NativeIconExtractor
    {
        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr ExtractAssociatedIcon(IntPtr hInst, string lpIconPath, out ushort lpiIcon);

        public static IntPtr ExtractAssociatedIcon(string filePath)
        {
            ushort index = 0;
            return ExtractAssociatedIcon(Process.GetCurrentProcess().Handle, filePath, out index);
        }

    }
}
