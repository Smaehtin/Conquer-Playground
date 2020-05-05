namespace MinHookManaged
{
    using System;
    using System.Runtime.InteropServices;

    public abstract class Hook<T> where T : class
    {
        private T detour;
        private T original;
        private IntPtr target;

        static Hook()
        {
            if (!typeof(T).IsSubclassOf(typeof(Delegate)))
            {
                throw new ArgumentException(typeof(T).Name + " is not a delegate type.");
            }
        }

        protected Hook()
        {
            this.detour = null;
            this.original = null;
            this.target = IntPtr.Zero;
        }

        /// <summary>
        /// The trampoline function, which is used to call the original target function.
        /// </summary>
        public T Original
        {
            get
            {
                this.EnsureCreated();
                return this.original;
            }
        }

        /// <summary>
        /// Disables the hook.
        /// </summary>
        public void Disable()
        {
            this.EnsureCreated();
            MinHookStatusChecker.EnsureOk(MinHookApi.MHDisableHook(this.target));
        }

        /// <summary>
        /// Enables the hook.
        /// </summary>
        public void Enable()
        {
            this.EnsureCreated();
            System.Diagnostics.Debug.WriteLine(MinHookApi.MHEnableHook(this.target));
            System.Diagnostics.Debug.WriteLine(MinHookApi.MHEnableHook(this.target));
            MinHookStatusChecker.EnsureOk(MinHookApi.MHEnableHook(this.target));
        }

        /// <summary>
        /// Queues the hook to be disabled.
        /// </summary>
        public void QueueDisable()
        {
            this.EnsureCreated();
            MinHookStatusChecker.EnsureOk(MinHookApi.MHQueueDisableHook(this.target));
        }

        /// <summary>
        /// Queues the hook to be enabled.
        /// </summary>
        public void QueueEnable()
        {
            this.EnsureCreated();
            MinHookStatusChecker.EnsureOk(MinHookApi.MHQueueEnableHook(this.target));
        }

        /// <summary>
        /// Creates the hook for the specified target function, in disabled state.
        /// </summary>
        /// <param name="target">The address of the target function, which will be overridden by the detour function.</param>
        /// <param name="detour">The detour function, which will override the target function.</param>
        protected void Create(IntPtr target, T detour)
        {
            var detourAddress = Marshal.GetFunctionPointerForDelegate(detour as Delegate);
            IntPtr original;

            MinHookStatusChecker.EnsureOk(MinHookApi.MHCreateHook(target, detourAddress, out original));

            this.Init(target, detour, original);
        }

        /// <summary>
        /// Creates the hook for the specified API function, in disabled state.
        /// </summary>
        /// <param name="moduleName">The name of the loaded module which contains the target function.</param>
        /// <param name="procedureName">The name of the target function, which will be overridden by the detour function.</param>
        /// <param name="detour">The detour function, which will override the target function.</param>
        protected void Create(string moduleName, string procedureName, T detour)
        {
            var detourAddress = Marshal.GetFunctionPointerForDelegate(detour as Delegate);
            IntPtr original;

            MinHookStatusChecker.EnsureOk(MinHookApi.MHCreateHookApi(moduleName, procedureName, detourAddress, out original));

            IntPtr target = Winapi.GetProcAddress(Winapi.GetModuleHandle(moduleName), procedureName);
            this.Init(target, detour, original);
        }

        private void EnsureCreated()
        {
            if (this.target == IntPtr.Zero)
            {
                throw new MinHookException("The hook is not created yet.");
            }
        }

        private void Init(IntPtr target, T detour, IntPtr original)
        {
            this.target = target;
            this.detour = detour;
            this.original = Marshal.GetDelegateForFunctionPointer(original, typeof(T)) as T;
        }
    }
}
