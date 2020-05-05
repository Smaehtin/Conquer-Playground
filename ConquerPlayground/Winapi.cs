namespace ConquerPlayground
{
    using System;
    using System.Runtime.InteropServices;

    internal static class Winapi
    {
        [Flags]
        public enum MessageBoxType
        {
            Ok = 0x0,
            OkCancel = 0x1,
            AbortRetryIgnore = 0x2,
            YesNoCancel = 0x3,
            YesNo = 0x4,
            RetryCancel = 0x5,
            CancelTryContinue = 0x6,
            IconError = 0x10,
            IconQuestion = 0x20,
            IconWarning = 0x30,
            IconInformation = 0x40,
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool AllocConsole();

        [DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int GetCurrentThreadId();

        [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern int MessageBox(IntPtr window, string text, string caption, MessageBoxType type);

        [DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr LoadLibrary(string fileName);
    }
}
