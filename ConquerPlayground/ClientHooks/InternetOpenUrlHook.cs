namespace ConquerPlayground.ClientHooks
{
    using System;
    using System.Runtime.InteropServices;
    using MinHookManaged;

    [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
    internal delegate IntPtr InternetOpenUrlDelegate(IntPtr internet, string url, string headers, int headersLength, int flags, IntPtr context);

    internal class InternetOpenUrlHook : Hook<InternetOpenUrlDelegate>
    {
        public InternetOpenUrlHook()
        {
            this.Create("wininet.dll", "InternetOpenUrlA", new InternetOpenUrlDelegate(this.DetouredInternetOpenUrl));
        }

        private IntPtr DetouredInternetOpenUrl(IntPtr internet, string url, string headers, int headersLength, int flags, IntPtr context)
        {
            // The old Conquer client tries to load this URL which no longer exists,
            // and this causes the client to "hang" for a noticeable amount of time at startup,
            // so we instantly return from the function to prevent this
            if (url == "http://account.conqueronline.com/status.php")
            {
                return IntPtr.Zero;
            }
            else
            {
                return this.Original(internet, url, headers, headersLength, flags, context);
            }
        }
    }
}
