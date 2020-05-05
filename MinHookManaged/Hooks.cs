namespace MinHookManaged
{
    public static class Hooks
    {
        /// <summary>
        /// Applies all queued changes in one go.
        /// </summary>
        public static void ApplyQueued()
        {
            MinHookStatusChecker.EnsureOk(MinHookApi.MHApplyQueued());
        }

        /// <summary>
        /// Initialize the MinHook library. You must call this function EXACTLY ONCE at the beginning of your program.
        /// </summary>
        public static void Initialize()
        {
            MinHookApi.MHInitialize();
        }

        /// <summary>
        /// Queues all created hooks to be disabled.
        /// </summary>
        public static void QueueDisableAllHooks()
        {
            MinHookStatusChecker.EnsureOk(MinHookApi.MHQueueDisableHook(MinHookApi.MHAllHooks));
        }

        /// <summary>
        /// Queues all created hooks to be enabled.
        /// </summary>
        public static void QueueEnableAllHooks()
        {
            MinHookStatusChecker.EnsureOk(MinHookApi.MHQueueEnableHook(MinHookApi.MHAllHooks));
        }

        /// <summary>
        /// Uninitialize the MinHook library. You must call this function EXACTLY ONCE at the end of your program.
        /// </summary>
        public static void Uninitialize()
        {
            MinHookApi.MHUninitialize();
        }
    }
}
