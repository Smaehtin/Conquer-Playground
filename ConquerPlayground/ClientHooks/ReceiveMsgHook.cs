namespace ConquerPlayground.ClientHooks
{
    using System;
    using System.Runtime.InteropServices;
    using MinHookManaged;
    using Network;

    [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
    internal delegate bool ReceiveMsgDelegate(IntPtr gameSocket, IntPtr buffer, out int size);

    internal class ReceiveMsgHook : Hook<ReceiveMsgDelegate>
    {
        private static readonly IntPtr ReceiveMsgAddress = new IntPtr(0x48866E);

        private GameNetwork gameNetwork;

        public ReceiveMsgHook(GameNetwork gameNetwork)
        {
            this.gameNetwork = gameNetwork;
            this.Create(ReceiveMsgAddress, new ReceiveMsgDelegate(this.DetouredReceiveMsg));
        }

        private bool DetouredReceiveMsg(IntPtr gameSocket, IntPtr buffer, out int size)
        {
            this.gameNetwork.ProcessClientMessages();

            NetMsg message = null;

            if (this.gameNetwork.ReceiveMsg(out message))
            {
                size = message.MsgSize;
                Marshal.Copy(message.Buffer, 0, buffer, size);
                return true;
            }
            else
            {
                size = 0;
                return false;
            }
        }
    }
}
