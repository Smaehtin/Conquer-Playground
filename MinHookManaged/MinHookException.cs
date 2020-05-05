namespace MinHookManaged
{
    using System;

    public class MinHookException : Exception
    {
        internal MinHookException(MinHookApi.MHStatus status)
            : base(GetErrorMessage(status))
        {
        }

        internal MinHookException(string message)
            : base(message)
        {
        }

        private static string GetErrorMessage(MinHookApi.MHStatus status)
        {
            string description = string.Empty;

            switch (status)
            {
                case MinHookApi.MHStatus.Unknown:
                    description = "Unknown error.";
                    break;

                case MinHookApi.MHStatus.Ok:
                    description = "Successful.";
                    break;

                case MinHookApi.MHStatus.ErrorAlreadyInitialized:
                    description = "MinHook is already initialized.";
                    break;

                case MinHookApi.MHStatus.ErrorNotInitialized:
                    description = "MinHook is not initialized yet, or already uninitialized.";
                    break;

                case MinHookApi.MHStatus.ErrorAlreadyCreated:
                    description = "The hook for the specified target function is already created.";
                    break;

                case MinHookApi.MHStatus.ErrorNotCreated:
                    description = "The hook for the specified target function is not created yet.";
                    break;

                case MinHookApi.MHStatus.ErrorEnabled:
                    description = "The hook for the specified target function is already enabled.";
                    break;

                case MinHookApi.MHStatus.ErrorDisabled:
                    description = "The hook for the specified target function is not enabled yet, or already disabled.";
                    break;

                case MinHookApi.MHStatus.ErrorNotExecutable:
                    description = "The specified pointer is invalid. It points the address of non-allocated and/or non-executable region.";
                    break;

                case MinHookApi.MHStatus.ErrorUnsupportedFunction:
                    description = "The specified target function cannot be hooked.";
                    break;

                case MinHookApi.MHStatus.ErrorMemoryAlloc:
                    description = "Failed to allocate memory.";
                    break;

                case MinHookApi.MHStatus.ErrorMemoryProtect:
                    description = "Failed to change the memory protection.";
                    break;

                case MinHookApi.MHStatus.ErrorModuleNotFound:
                    description = "The specified module is not loaded.";
                    break;

                case MinHookApi.MHStatus.ErrorFunctionNotFound:
                    description = "The specified function is not found.";
                    break;
            }

            return string.Format("Error code: {0}, description: {1}", (int)status, description);
        }
    }
}
