using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace SystemMenuShell {

    static class HookMethod {

        [DllImport("SystemMenuShellHook.dll")]
        public static extern bool InitShellHook(int threadId, IntPtr destWindow);

        [DllImport("SystemMenuShellHook.dll")]
        public static extern bool UnInitShellHook();

        [DllImport("SystemMenuShellHook.dll")]
        public static extern bool InitGetMsgHook(int threadId, IntPtr destWindow);

        [DllImport("SystemMenuShellHook.dll")]
        public static extern bool UnInitGetMsgHook();
        
    }
}
