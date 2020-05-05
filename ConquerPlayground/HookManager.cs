namespace ConquerPlayground
{
    using ClientHooks;
    using MinHookManaged;
    using Network;

    internal class HookManager
    {
        private ConnectHook connectHook;
        private GameNetwork gameNetwork;
        private InternetOpenUrlHook internetOpenUrlHook;
        private ReceiveMsgHook receiveMsgHook;
        private SendMsgHook sendMsgHook;
        private WsaGetLastErrorHook wsaGetLastErrorHook;

        public HookManager(GameNetwork gameNetwork)
        {
            this.gameNetwork = gameNetwork;
        }

        public void InstallHooks()
        {
            // Initialize the MinHook library
            Hooks.Initialize();

            // Since we might be installing hooks in libraries that are dynamically loaded by Conquer (which means delayed loading),
            // we need to make sure that the libraries are actually loaded before trying to install the hooks
            LoadLibraries();

            // Create all our hooks
            this.connectHook = new ConnectHook(this.gameNetwork);
            this.internetOpenUrlHook = new InternetOpenUrlHook();
            this.receiveMsgHook = new ReceiveMsgHook(this.gameNetwork);
            this.sendMsgHook = new SendMsgHook(this.gameNetwork);
            this.wsaGetLastErrorHook = new WsaGetLastErrorHook();

            // Enable all the created hooks
            Hooks.QueueEnableAllHooks();
            Hooks.ApplyQueued();
        }

        private static void LoadLibraries()
        {
            Winapi.LoadLibrary("wininet.dll");
        }
    }
}
