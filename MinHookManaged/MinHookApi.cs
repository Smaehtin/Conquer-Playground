/*
 *  MinHook - The Minimalistic API Hooking Library for x64/x86
 *  Copyright (C) 2009-2015 Tsuda Kageyu.
 *  All rights reserved.
 *
 *  Redistribution and use in source and binary forms, with or without
 *  modification, are permitted provided that the following conditions
 *  are met:
 *
 *   1. Redistributions of source code must retain the above copyright
 *      notice, this list of conditions and the following disclaimer.
 *   2. Redistributions in binary form must reproduce the above copyright
 *      notice, this list of conditions and the following disclaimer in the
 *      documentation and/or other materials provided with the distribution.
 *
 *  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
 *  "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED
 *  TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A
 *  PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER
 *  OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
 *  EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
 *  PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
 *  PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
 *  LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
 *  NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
 *  SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

namespace MinHookManaged
{
    using System;
    using System.Runtime.InteropServices;

    internal static class MinHookApi
    {
        public static readonly IntPtr MHAllHooks = IntPtr.Zero;

#if WIN64
        private const string MinHookLibrary = "MinHook.x64.dll";
#else
        private const string MinHookLibrary = "MinHook.x86.dll";
#endif

        public enum MHStatus : int
        {
            Unknown = -1,
            Ok = 0,
            ErrorAlreadyInitialized,
            ErrorNotInitialized,
            ErrorAlreadyCreated,
            ErrorNotCreated,
            ErrorEnabled,
            ErrorDisabled,
            ErrorNotExecutable,
            ErrorUnsupportedFunction,
            ErrorMemoryAlloc,
            ErrorMemoryProtect,
            ErrorModuleNotFound,
            ErrorFunctionNotFound
        }

        /// <summary>
        /// Initialize the MinHook library. You must call this function EXACTLY ONCE at the beginning of your program.
        /// </summary>
        [DllImport(MinHookLibrary, CallingConvention = CallingConvention.StdCall, EntryPoint = "MH_Initialize")]
        public static extern void MHInitialize();

        /// <summary>
        /// Uninitialize the MinHook library. You must call this function EXACTLY ONCE at the end of your program.
        /// </summary>
        [DllImport(MinHookLibrary, CallingConvention = CallingConvention.StdCall, EntryPoint = "MH_Uninitialize")]
        public static extern void MHUninitialize();

        /// <summary>
        /// Creates a Hook for the specified target function, in disabled state.
        /// </summary>
        /// <param name="target">A pointer to the target function, which will be overridden by the detour function.</param>
        /// <param name="detour">A pointer to the detour function, which will override the target function.</param>
        /// <param name="original">A pointer to the trampoline function, which will be used to call the original target function. This parameter can be NULL.</param>
        /// <returns>If the operation completed successfully, <see cref="MHStatus.Ok"/> is returned.</returns>
        [DllImport(MinHookLibrary, CallingConvention = CallingConvention.StdCall, EntryPoint = "MH_CreateHook")]
        public static extern MHStatus MHCreateHook(IntPtr target, IntPtr detour, out IntPtr original);

        /// <summary>
        /// Creates a Hook for the specified API function, in disabled state.
        /// </summary>
        /// <param name="module">A pointer to the loaded module name which contains the target function.</param>
        /// <param name="procedureName">A pointer to the target function name, which will be overridden by the detour function.</param>
        /// <param name="detour">A pointer to the detour function, which will override the target function.</param>
        /// <param name="original">A pointer to the trampoline function, which will be used to call the original target function. This parameter can be NULL.</param>
        /// <returns>If the operation completed successfully, <see cref="MHStatus.Ok"/> is returned.</returns>
        [DllImport(MinHookLibrary, CallingConvention = CallingConvention.StdCall, EntryPoint = "MH_CreateHookApi")]
        public static extern MHStatus MHCreateHookApi([MarshalAs(UnmanagedType.LPWStr)]string module, [MarshalAs(UnmanagedType.LPStr)]string procedureName, IntPtr detour, out IntPtr original);

        /// <summary>
        /// Removes an already created hook.
        /// </summary>
        /// <param name="target">A pointer to the target function.</param>
        /// <returns>If the operation completed successfully, <see cref="MHStatus.Ok"/> is returned.</returns>
        [DllImport(MinHookLibrary, CallingConvention = CallingConvention.StdCall, EntryPoint = "MH_RemoveHook")]
        public static extern MHStatus MHRemoveHook(IntPtr target);

        /// <summary>
        /// Enables an already created hook.
        /// </summary>
        /// <param name="target">A pointer to the target function. If this parameter is <see cref="MHAllHooks"/>, all created hooks are enabled in one go.</param>
        /// <returns>If the operation completed successfully, <see cref="MHStatus.Ok"/> is returned.</returns>
        [DllImport(MinHookLibrary, CallingConvention = CallingConvention.StdCall, EntryPoint = "MH_EnableHook")]
        public static extern MHStatus MHEnableHook(IntPtr target);

        /// <summary>
        /// Disables an already created hook.
        /// </summary>
        /// <param name="target">A pointer to the target function. If this parameter is <see cref="MHAllHooks"/>, all created hooks are disabled in one go.</param>
        /// <returns>If the operation completed successfully, <see cref="MHStatus.Ok"/> is returned.</returns>
        [DllImport(MinHookLibrary, CallingConvention = CallingConvention.StdCall, EntryPoint = "MH_DisableHook")]
        public static extern MHStatus MHDisableHook(IntPtr target);

        /// <summary>
        /// Queues to enable an already created hook.
        /// </summary>
        /// <param name="target">A pointer to the target function. If this parameter is <see cref="MHAllHooks"/>, all created hooks are queued to be enabled.</param>
        /// <returns>If the operation completed successfully, <see cref="MHStatus.Ok"/> is returned.</returns>
        [DllImport(MinHookLibrary, CallingConvention = CallingConvention.StdCall, EntryPoint = "MH_QueueEnableHook")]
        public static extern MHStatus MHQueueEnableHook(IntPtr target);

        /// <summary>
        /// Queues to disable an already created hook.
        /// </summary>
        /// <param name="target">A pointer to the target function. If this parameter is <see cref="MHAllHooks"/>, all created hooks are queued to be disabled.</param>
        /// <returns>If the operation completed successfully, <see cref="MHStatus.Ok"/> is returned.</returns>
        [DllImport(MinHookLibrary, CallingConvention = CallingConvention.StdCall, EntryPoint = "MH_QueueDisableHook")]
        public static extern MHStatus MHQueueDisableHook(IntPtr target);

        /// <summary>
        /// Applies all queued changes in one go.
        /// </summary>
        /// <returns>If the operation completed successfully, <see cref="MHStatus.Ok"/> is returned.</returns>
        [DllImport(MinHookLibrary, CallingConvention = CallingConvention.StdCall, EntryPoint = "MH_ApplyQueued")]
        public static extern MHStatus MHApplyQueued();
    }
}
