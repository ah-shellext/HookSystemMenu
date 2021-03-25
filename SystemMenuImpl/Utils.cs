using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace SystemMenuImpl {

    class Utils {
        public static void SetWindowTopMost(IntPtr hwnd, Boolean topMost) {
            var handleTopMost = (IntPtr) (-1);
            var handleNotTopMost = (IntPtr) (-2);
            var after = topMost ? handleTopMost : handleNotTopMost;

            var flag = NativeConstants.SWP_NOOWNERZORDER | NativeConstants.SWP_NOACTIVATE | NativeConstants.SWP_NOMOVE | NativeConstants.SWP_NOSIZE;
            NativeMethods.SetWindowPos(hwnd, after, 0, 0, 0, 0, flag);
        }

        public static void ExitProcess(Process process) {
            var handles = new List<IntPtr>();
            foreach (ProcessThread thread in process.Threads) {
                NativeMethods.EnumThreadWindows((uint) thread.Id, (hwnd, lParam) => { handles.Add(hwnd); return true; }, (IntPtr) 0);
            }
            foreach (var handle in handles) {
                NativeMethods.PostMessage(handle, NativeConstants.WM_CLOSE, (IntPtr) 0, (IntPtr) 0);
            }
            try {
                if (process.WaitForExit(5000)) {
                    process.Kill();
                }
            } catch { }
        }

        public static string GetWindowTitle(IntPtr hwnd) {
            int length = NativeMethods.GetWindowTextLength(hwnd);
            var windowName = new StringBuilder(length + 1);
            NativeMethods.GetWindowText(hwnd, windowName, windowName.Capacity);
            return windowName.ToString();
        }

        public static bool IsWindow(IntPtr hwnd, bool more = false) {
            string title = GetWindowTitle(hwnd);
            IntPtr dsk = NativeMethods.GetDesktopWindow();
            IntPtr owner = NativeMethods.GetWindow(hwnd, NativeConstants.GW_OWNER);
            IntPtr parent = NativeMethods.GetParent(hwnd);
            return !string.IsNullOrEmpty(title) && title != "Program Manager"
                && owner == IntPtr.Zero && (parent != owner || parent != dsk)
                && NativeMethods.IsWindow(hwnd) && (more || NativeMethods.IsWindowVisible(hwnd))
                && (more || (NativeMethods.GetWindowLong(hwnd, NativeConstants.GWL_STYLE).ToInt64() & NativeConstants.WS_VISIBLE) != 0)
                && (NativeMethods.GetWindowLong(hwnd, NativeConstants.GWL_EXSTYLE).ToInt64() & NativeConstants.WS_EX_TOOLWINDOW) == 0;
        }
    }
}
