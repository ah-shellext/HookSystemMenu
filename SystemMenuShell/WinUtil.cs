using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SystemMenuShell {

    static class WinUtil {

        // WParam
        public static IntPtr cacheHandle = IntPtr.Zero;
        // LParam
        public static IntPtr cacheMessage = IntPtr.Zero;

        public static bool IsWindow(IntPtr Hwnd) {
            IntPtr dsk = NativeMethod.GetDesktopWindow();
            IntPtr owner = NativeMethod.GetWindow(Hwnd, NativeConstant.GW_OWNER);
            IntPtr parent = NativeMethod.GetParent(Hwnd);
            return NativeMethod.IsWindow(Hwnd)
                && owner == IntPtr.Zero
                && (parent.Equals(owner) || parent.Equals(dsk))
                && ((NativeMethod.GetWindowLong(Hwnd, NativeConstant.GWL_STYLE).ToInt64() & NativeConstant.WS_VISIBLE) != 0)
                && ((NativeMethod.GetWindowLong(Hwnd, NativeConstant.GWL_EXSTYLE).ToInt64() & NativeConstant.WS_EX_TOOLWINDOW) == 0);
        }

        public static List<IntPtr> GetAllWindows() {
            var Hwnds = new List<IntPtr>();
            IntPtr dsk = NativeMethod.GetDesktopWindow();
            IntPtr hwnd = NativeMethod.GetWindow(dsk, NativeConstant.GW_CHILD);

            while (hwnd != IntPtr.Zero) {
                if (IsWindow(hwnd))
                    Hwnds.Add(hwnd);

                hwnd = NativeMethod.GetNextWindow(hwnd);
            }
            return Hwnds;
        }

        public static string GetWindowTitle(IntPtr Hwnd) {
            int length = NativeMethod.GetWindowTextLength(Hwnd);
            StringBuilder windowName = new StringBuilder(length + 1);
            NativeMethod.GetWindowText(Hwnd, windowName, windowName.Capacity);
            return windowName.ToString();
        }

        public static void InsertSystemMenu(IntPtr Hwnd) {

        }

        public static void RemoveSystemMenu(IntPtr Hwnd) {

        }
    }
}
