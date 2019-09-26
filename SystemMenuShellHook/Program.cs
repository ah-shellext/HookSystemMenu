using System;
using System.Runtime.InteropServices;

namespace SystemMenuShellHook {

    public class ShellHook {

        // https://blog.csdn.net/slimboy123/article/details/5689831

        private delegate int HookProc(int code, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hMod, int dwThreadId);

        [DllImport("user32.dll")]
        static extern int CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool UnhookWindowsHookEx(IntPtr idHook);

        public static int WH_SHELL = 10;

        ///

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int RegisterWindowMessage(string lpString);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern bool SendNotifyMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        ///

        public static int HSHELL_ACTIVATESHELLWINDOW = 3;
        public static int HSHELL_GETMINRECT = 5;
        public static int HSHELL_LANGUAGE = 8;
        public static int HSHELL_REDRAW = 6;
        public static int HSHELL_TASKMAN = 7;
        public static int HSHELL_WINDOWACTIVATED = 4;
        public static int HSHELL_WINDOWCREATED = 1;
        public static int HSHELL_WINDOWDESTROYED = 2;

        ///

        private static IntPtr hwndMain;
        private static IntPtr shellHook;

        public static bool initShellHook(int threadID, IntPtr destination) {
            hwndMain = destination;
            shellHook = SetWindowsHookEx(WH_SHELL, new HookProc(shellCb), IntPtr.Zero, threadID);
            return shellHook != IntPtr.Zero;
        }

        public static void unInitShellHook() {
            if (shellHook != IntPtr.Zero)
                UnhookWindowsHookEx(shellHook);
            shellHook = IntPtr.Zero;
        }

        public static int shellCb(int code, IntPtr wParam, IntPtr lParam) {
            if (code >= 0) {
                int msg = 0;

                if (code == HSHELL_ACTIVATESHELLWINDOW)
                    msg = RegisterWindowMessage("SYSTEMMENUSHELL_HSHELL_ACTIVATESHELLWINDOW");
                else if (code == HSHELL_GETMINRECT)
                    msg = RegisterWindowMessage("SYSTEMMENUSHELL_HSHELL_GETMINRECT");
                else if (code == HSHELL_LANGUAGE)
                    msg = RegisterWindowMessage("SYSTEMMENUSHELL_HSHELL_LANGUAGE");
                else if (code == HSHELL_REDRAW)
                    msg = RegisterWindowMessage("SYSTEMMENUSHELL_HSHELL_REDRAW");
                else if (code == HSHELL_TASKMAN)
                    msg = RegisterWindowMessage("SYSTEMMENUSHELL_HSHELL_TASKMAN");
                else if (code == HSHELL_WINDOWACTIVATED)
                    msg = RegisterWindowMessage("SYSTEMMENUSHELL_HSHELL_WINDOWACTIVATED");
                else if (code == HSHELL_WINDOWCREATED)
                    msg = RegisterWindowMessage("SYSTEMMENUSHELL_HSHELL_WINDOWCREATED");
                else if (code == HSHELL_WINDOWDESTROYED)
                    msg = RegisterWindowMessage("SYSTEMMENUSHELL_HSHELL_WINDOWDESTROYED");

                if (msg != 0)
                    SendNotifyMessage(hwndMain, msg, wParam, lParam);
            }

            return CallNextHookEx(shellHook, code, wParam, lParam);
        }
    }
}
