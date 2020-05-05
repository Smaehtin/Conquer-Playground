namespace ConquerPlayground.ClientHooks
{
    using System;
    using System.Net;
    using System.Runtime.InteropServices;
    using MinHookManaged;
    using Network;

    [UnmanagedFunctionPointer(CallingConvention.StdCall, SetLastError = true)]
    internal delegate int ConnectDelegate(IntPtr socket, IntPtr name, int namelen);

    internal class ConnectHook : Hook<ConnectDelegate>
    {
        private GameNetwork gameNetwork;

        public ConnectHook(GameNetwork gameNetwork)
        {
            this.gameNetwork = gameNetwork;
            this.Create("ws2_32.dll", "connect", new ConnectDelegate(this.DetouredConnect));
        }

        private int DetouredConnect(IntPtr socket, IntPtr name, int namelen)
        {
            /*
                struct sockaddr_in {
                        short   sin_family;
                        u_short sin_port;
                        struct  in_addr sin_addr;
                        char    sin_zero[8];
                };
            */

            if (name != IntPtr.Zero)
            {
                var port = Marshal.ReadInt16(name, 2);
                port = IPAddress.HostToNetworkOrder(port);

                if (port == 9958 || port == 5816)
                {
                    return 0;
                }
            }

            return this.Original(socket, name, namelen);
        }
    }
}
