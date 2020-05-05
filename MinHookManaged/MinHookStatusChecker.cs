namespace MinHookManaged
{
    internal static class MinHookStatusChecker
    {
        public static void EnsureOk(MinHookApi.MHStatus status)
        {
            if (status != MinHookApi.MHStatus.Ok)
            {
                throw new MinHookException(status);
            }
        }
    }
}
