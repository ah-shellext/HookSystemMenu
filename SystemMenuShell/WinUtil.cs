using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Drawing;
using System.Diagnostics;

namespace SystemMenuShell {

    // 窗口相关：判断窗口，获取窗口，获取标题
    static class WinUtil {

        // WParam
        public static IntPtr cacheHandle = IntPtr.Zero;
        // LParam
        public static IntPtr cacheMessage = IntPtr.Zero;

        public static bool IsWindow(IntPtr Hwnd) {
            IntPtr dsk = NativeMethod.GetDesktopWindow();
            IntPtr owner = NativeMethod.GetWindow(Hwnd, NativeConstant.GW_OWNER);
            IntPtr parent = NativeMethod.GetParent(Hwnd);
            string title = GetWindowTitle(Hwnd);
            return 
                // !title.Equals("Program Manager");
                //  NativeMethod.IsWindow(Hwnd)
                // && owner == IntPtr.Zero
                // && (parent.Equals(owner) || parent.Equals(dsk))
                ((NativeMethod.GetWindowLong(Hwnd, NativeConstant.GWL_STYLE).ToInt64() & NativeConstant.WS_VISIBLE) != 0);
                // && ((NativeMethod.GetWindowLong(Hwnd, NativeConstant.GWL_EXSTYLE).ToInt64() & NativeConstant.WS_EX_TOOLWINDOW) == 0);
        }

        public static List<IntPtr> GetAllWindows() {
            var hwnds = new List<IntPtr>();
            IntPtr dsk = NativeMethod.GetDesktopWindow();
            IntPtr hwnd = NativeMethod.GetWindow(dsk, NativeConstant.GW_CHILD);

            while (hwnd != IntPtr.Zero) {
                if (IsWindow(hwnd) && hwnds.IndexOf(hwnd) == -1) {
                    hwnds.Add(hwnd);
                }
                hwnd = NativeMethod.GetNextWindow(hwnd);
            }
            return hwnds;
        }

        public static string GetWindowTitle(IntPtr hwnd) {
            int length = NativeMethod.GetWindowTextLength(hwnd);
            StringBuilder windowName = new StringBuilder(length + 1);
            NativeMethod.GetWindowText(hwnd, windowName, windowName.Capacity);
            return windowName.ToString();
        }

        public static bool IsWindowTopMost(IntPtr hwnd) {
            var flag = NativeMethod.GetWindowLong(hwnd, NativeConstant.GWL_EXSTYLE).ToInt64() & NativeConstant.WS_EX_TOPMOST;
            return flag != 0;
        }

        public static void SetWindowTopMost(IntPtr hwnd, Boolean topMost) {
            var handleTopMost = (IntPtr) (-1);
            var handleNotTopMost = (IntPtr) (-2);
            var after = topMost ? handleTopMost : handleNotTopMost;

            var flag = NativeConstant.SWP_NOOWNERZORDER | NativeConstant.SWP_NOACTIVATE | NativeConstant.SWP_NOMOVE | NativeConstant.SWP_NOSIZE;
            NativeMethod.SetWindowPos(hwnd, after, 0, 0, 0, 0, flag);
        }

        public static void CaptureWindow(IntPtr hwnd) {
            NativeMethod.Rect rect;
            NativeMethod.GetWindowRect(hwnd, out rect);
            Bitmap bitmap = new Bitmap(rect.Width, rect.Height, PixelFormat.Format32bppArgb);
            using (var graphics = Graphics.FromImage(bitmap)) {
                IntPtr hdc = graphics.GetHdc();
                NativeMethod.PrintWindow(hwnd, hdc, 0);
                graphics.ReleaseHdc(hdc);
            }
            Clipboard.Clear();
            Clipboard.SetImage(bitmap);
        }
    }
}
