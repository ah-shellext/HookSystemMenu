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

        [DllImport("SystemMenuShellHook.dll", EntryPoint = "InitGetMsgHook")]
        public static extern bool InitGetMsgHook(int threadId, IntPtr destWindow);

        [DllImport("SystemMenuShellHook.dll", EntryPoint = "UnInitGetMsgHook")]
        public static extern void UnInitGetMsgHook();
        
    }
}
