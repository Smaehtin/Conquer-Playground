namespace ConquerPlayground.ClientHooks
{
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using MinHookManaged;
    using Network;

    [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
    internal delegate bool SendMsgDelegate(IntPtr gameSocket, IntPtr buffer, int size);

    internal class SendMsgHook : Hook<SendMsgDelegate>
    {
        private static readonly IntPtr SendMsgAddress = new IntPtr(0x4877B5);

        private GameNetwork gameNetwork;

        public SendMsgHook(GameNetwork gameNetwork)
        {
            this.gameNetwork = gameNetwork;
            this.Create(SendMsgAddress, new SendMsgDelegate(this.DetouredSendMsg));
        }

        private bool DetouredSendMsg(IntPtr gameSocket, IntPtr buffer, int size)
        {
            var message = NetMsg.Create(this.gameNetwork, buffer, size);
            this.gameNetwork.AddClientMessage(message);

            Debug.WriteLine(message.ToString() + Environment.NewLine, "[SENT]");

            return true;
        }
    }
}
