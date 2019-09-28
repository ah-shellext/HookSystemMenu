using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Drawing;

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
            return
                //  NativeMethod.IsWindow(Hwnd)
                // && owner == IntPtr.Zero
                // && (parent.Equals(owner) || parent.Equals(dsk))
                 ((NativeMethod.GetWindowLong(Hwnd, NativeConstant.GWL_STYLE).ToInt64() & NativeConstant.WS_VISIBLE) != 0);
                // && ((NativeMethod.GetWindowLong(Hwnd, NativeConstant.GWL_EXSTYLE).ToInt64() & NativeConstant.WS_EX_TOOLWINDOW) == 0);
        }

        public static List<IntPtr> GetAllWindows() {
            var Hwnds = new List<IntPtr>();
            IntPtr dsk = NativeMethod.GetDesktopWindow();
            IntPtr hwnd = NativeMethod.GetWindow(dsk, NativeConstant.GW_CHILD);

            while (hwnd != IntPtr.Zero) {
                if (IsWindow(hwnd) && Hwnds.IndexOf(hwnd) == -1)
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

        // 防止 ID 被占用：0xAF0X Prefix
        public const uint MENU_ID_SPLIT = 0xAF01;
        public const uint MENU_ID_TOPMOST = 0xAF02;
        public const uint MENU_ID_PRTSC = 0xAF03;

        public static void InsertSystemMenu(IntPtr Hwnd) {

            IntPtr hSysMenu = NativeMethod.GetSystemMenu(Hwnd, false);

            if (hSysMenu == IntPtr.Zero) 
                return;

            // Split
            var split_menuitem = new NativeMethod.MENUITEMINFO();
            split_menuitem.cbSize = (uint)Marshal.SizeOf(split_menuitem);
            split_menuitem.fMask = NativeConstant.MIIM_FTYPE | NativeConstant.MIIM_ID;
            split_menuitem.fType = NativeConstant.MFT_SEPARATOR;
            split_menuitem.wID = MENU_ID_SPLIT;

            // TopMost
            var topmost_menuitem = new NativeMethod.MENUITEMINFO();
            topmost_menuitem.cbSize = (uint)Marshal.SizeOf(topmost_menuitem);
            topmost_menuitem.fMask = NativeConstant.MIIM_STRING | NativeConstant.MIIM_ID | NativeConstant.MIIM_STATE;
            topmost_menuitem.dwTypeData = "トップにピン(&P)";
            topmost_menuitem.wID = MENU_ID_TOPMOST;
            topmost_menuitem.fState = NativeConstant.MFS_UNCHECKED;

            // Screenshot
            var prtSc_menuitem = new NativeMethod.MENUITEMINFO();
            prtSc_menuitem.cbSize = (uint)Marshal.SizeOf(prtSc_menuitem);
            prtSc_menuitem.fMask = NativeConstant.MIIM_STRING | NativeConstant.MIIM_ID | NativeConstant.MIIM_STATE;
            prtSc_menuitem.dwTypeData = "スクリーンショット(&C)";
            prtSc_menuitem.wID = MENU_ID_PRTSC;
            prtSc_menuitem.fState = NativeConstant.MFS_UNCHECKED;

            NativeMethod.InsertMenuItem(hSysMenu, 5, true, ref split_menuitem);
            NativeMethod.InsertMenuItem(hSysMenu, 6, true, ref topmost_menuitem);
            NativeMethod.InsertMenuItem(hSysMenu, 7, true, ref prtSc_menuitem);
        }

        public static void RemoveSystemMenu(IntPtr Hwnd) {
            IntPtr hSysMenu = NativeMethod.GetSystemMenu(Hwnd, false);
            NativeMethod.DeleteMenu(hSysMenu, MENU_ID_SPLIT, NativeConstant.MF_BYCOMMAND);
            NativeMethod.DeleteMenu(hSysMenu, MENU_ID_TOPMOST, NativeConstant.MF_BYCOMMAND);
            NativeMethod.DeleteMenu(hSysMenu, MENU_ID_PRTSC, NativeConstant.MF_BYCOMMAND);
        }

        public static void InitMenuItemState(IntPtr Hwnd) {
            IntPtr hSysMenu = NativeMethod.GetSystemMenu(Hwnd, false);

            uint topMost =
                (NativeMethod.GetWindowLong(Hwnd, NativeConstant.GWL_EXSTYLE).ToInt64() & NativeConstant.WS_EX_TOPMOST) != 0 ?
                NativeConstant.MFS_CHECKED : NativeConstant.MFS_UNCHECKED;

            NativeMethod.CheckMenuItem(hSysMenu, MENU_ID_TOPMOST, NativeConstant.MF_BYCOMMAND | topMost);
        }

        // トップにピン(&P)
        public static void OnTopMostMenuItemClick(IntPtr Hwnd) {
            IntPtr hSysMenu = NativeMethod.GetSystemMenu(Hwnd, false);

            bool isTop = (NativeMethod.GetWindowLong(Hwnd, NativeConstant.GWL_EXSTYLE).ToInt64() & NativeConstant.WS_EX_TOPMOST) != 0;
            isTop = !isTop;

            IntPtr handleTopMost = (IntPtr)(-1);
            IntPtr handleNotTopMost = (IntPtr)(-2);

            IntPtr hWndInsertAfter = isTop ? handleTopMost : handleNotTopMost;
            NativeMethod.SetWindowPos(Hwnd, hWndInsertAfter, 0, 0, 0, 0, NativeConstant.TOPMOST_FLAGS);

            InitMenuItemState(Hwnd);
        }

        // スクリーンショット(&C)
        public static void OnPrtScMenuItemClick(IntPtr Hwnd) {
            NativeMethod.Rect rect;
            NativeMethod.GetWindowRect(Hwnd, out rect);
            Bitmap bitmap = new Bitmap(rect.Width, rect.Height, PixelFormat.Format32bppArgb);
            using (var graphics = Graphics.FromImage(bitmap)) {
                IntPtr hdc = graphics.GetHdc();
                NativeMethod.PrintWindow(Hwnd, hdc, 0);
                graphics.ReleaseHdc(hdc);
            }
            Clipboard.Clear();
            Clipboard.SetImage(bitmap);
        }
    }
}
