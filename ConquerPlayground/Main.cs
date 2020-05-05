namespace ConquerPlayground
{
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using Network;
    using RGiesecke.DllExport;

    public class Main
    {
        [DllExport("Initialize", CallingConvention = CallingConvention.StdCall)]
        public static void Initialize()
        {
            try
            {
#if DEBUG
                Winapi.AllocConsole();
                Debug.Listeners.Add(new TextWriterTraceListener(Console.Out));
#endif

                GameNetwork gameNetwork = new GameNetwork();
                HookManager hookManager = new HookManager(gameNetwork);

                hookManager.InstallHooks();
            }
            catch (Exception e)
            {
                ShowException(e);
                Environment.Exit(0);
            }
        }

        private static void ShowException(Exception e)
        {
            Winapi.MessageBox(IntPtr.Zero, e.ToString(), "Error", Winapi.MessageBoxType.Ok | Winapi.MessageBoxType.IconError);
        }
    }
}