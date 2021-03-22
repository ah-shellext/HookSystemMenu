using System;
using System.Text;

namespace SystemMenuImpl {

    class Utils {

        public static string GetWindowTitle(IntPtr hwnd) {
            int length = NativeMethods.GetWindowTextLength(hwnd);
            StringBuilder windowName = new StringBuilder(length + 1);
            NativeMethods.GetWindowText(hwnd, windowName, windowName.Capacity);
            return windowName.ToString();
        }

        public static bool IsWindow(IntPtr hWnd) {
            IntPtr dsk = NativeMethods.GetDesktopWindow();
            IntPtr owner = NativeMethods.GetWindow(hWnd, NativeConstants.GW_OWNER);
            IntPtr parent = NativeMethods.GetParent(hWnd);
            string title = GetWindowTitle(hWnd);
            return (NativeMethods.GetWindowLong(hWnd, NativeConstants.GWL_STYLE).ToInt64() & NativeConstants.WS_VISIBLE) != 0;
        }
    }
}
