namespace ConquerPlayground.ClientHooks
{
    using System.Net.Sockets;
    using MinHookManaged;

    internal delegate int WsaGetLastErrorDelegate();

    internal class WsaGetLastErrorHook : Hook<WsaGetLastErrorDelegate>
    {
        public WsaGetLastErrorHook()
        {
            this.Create("ws2_32.dll", "WSAGetLastError", new WsaGetLastErrorDelegate(this.DetouredWsaGetLastError));
        }

        private int DetouredWsaGetLastError()
        {
            return (int)SocketError.WouldBlock;
        }
    }
}
