using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace SystemMenuShell {

    static class HookMethod {

        [DllImport("SystemMenuShellHook.dll", EntryPoint = "InitShellHook")]
        public static extern bool InitShellHook(int threadId, IntPtr destWindow);

        [DllImport("SystemMenuShellHook.dll", EntryPoint = "UnInitShellHook")]
        public static extern void UnInitShellHook();

        [DllImport("SystemMenuShellHook.dll", EntryPoint = "InitCbtHook")]
        public static extern bool InitCbtHook(int threadId, IntPtr destWindow);

        [DllImport("SystemMenuShellHook.dll", EntryPoint = "UnInitCbtHook")]
        public static extern void UnInitCbtHook();

        [DllImport("SystemMenuShellHook.dll", EntryPoint = "InitGetMsgHook")]
        public static extern bool InitGetMsgHook(int threadId, IntPtr destWindow);

        [DllImport("SystemMenuShellHook.dll", EntryPoint = "UnInitGetMsgHook")]
        public static extern void UnInitGetMsgHook();

        [DllImport("SystemMenuShellHook.dll", EntryPoint = "InitCallWndProcHook")]
        public static extern bool InitCallWndProcHook(int threadId, IntPtr destWindow);

        [DllImport("SystemMenuShellHook.dll", EntryPoint = "UnInitCallWndProcHook")]
        public static extern void UnInitCallWndProcHook();
        
    }
}
