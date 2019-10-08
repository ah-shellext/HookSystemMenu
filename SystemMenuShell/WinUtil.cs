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
        public const uint MENU_ID_PATH = 0xAF04;

        public static bool InsertSystemMenu(IntPtr Hwnd) {

            IntPtr hSysMenu = NativeMethod.GetSystemMenu(Hwnd, false);

            if (hSysMenu == IntPtr.Zero) 
                return false;

            // NativeMethod.GetMenuItemCount(hSysMenu);

            // Split
            var split_menuitem = new NativeMethod.MENUITEMINFO();
            split_menuitem.cbSize = (uint)Marshal.SizeOf(split_menuitem);
            split_menuitem.fMask = NativeConstant.MIIM_FTYPE | NativeConstant.MIIM_ID;
            split_menuitem.fType = NativeConstant.MFT_SEPARATOR;
            split_menuitem.wID = MENU_ID_SPLIT;

            // TopMost
            var topMost_menuitem = new NativeMethod.MENUITEMINFO();
            topMost_menuitem.cbSize = (uint)Marshal.SizeOf(topMost_menuitem);
            topMost_menuitem.fMask = NativeConstant.MIIM_STRING | NativeConstant.MIIM_ID | NativeConstant.MIIM_STATE;
            topMost_menuitem.dwTypeData = "トップにピン(&P)";
            topMost_menuitem.wID = MENU_ID_TOPMOST;
            topMost_menuitem.fState = NativeConstant.MFS_UNCHECKED;

            // Screenshot
            var prtSc_menuitem = new NativeMethod.MENUITEMINFO();
            prtSc_menuitem.cbSize = (uint)Marshal.SizeOf(prtSc_menuitem);
            prtSc_menuitem.fMask = NativeConstant.MIIM_STRING | NativeConstant.MIIM_ID;
            prtSc_menuitem.dwTypeData = "スクリーンショット(&C)";
            prtSc_menuitem.wID = MENU_ID_PRTSC;

            // ProcessPath
            var path_menuitem = new NativeMethod.MENUITEMINFO();
            path_menuitem.cbSize = (uint)Marshal.SizeOf(path_menuitem);
            path_menuitem.fMask = NativeConstant.MIIM_STRING | NativeConstant.MIIM_ID;
            path_menuitem.dwTypeData = "場所を開く(&O)";
            path_menuitem.wID = MENU_ID_PATH;

            NativeMethod.InsertMenuItem(hSysMenu, 5, true, ref split_menuitem);
            NativeMethod.InsertMenuItem(hSysMenu, 6, true, ref topMost_menuitem);
            NativeMethod.InsertMenuItem(hSysMenu, 7, true, ref prtSc_menuitem);
            NativeMethod.InsertMenuItem(hSysMenu, 8, true, ref path_menuitem);

            return true;
        }

        public static void RemoveSystemMenu(IntPtr Hwnd) {
            IntPtr hSysMenu = NativeMethod.GetSystemMenu(Hwnd, false);
            if (hSysMenu == IntPtr.Zero)
                return;

            NativeMethod.DeleteMenu(hSysMenu, MENU_ID_SPLIT, NativeConstant.MF_BYCOMMAND);
            NativeMethod.DeleteMenu(hSysMenu, MENU_ID_TOPMOST, NativeConstant.MF_BYCOMMAND);
            NativeMethod.DeleteMenu(hSysMenu, MENU_ID_PRTSC, NativeConstant.MF_BYCOMMAND);
            
            NativeMethod.GetSystemMenu(Hwnd, true);
        }

        public static void InitMenuItemState(IntPtr Hwnd) {
            IntPtr hSysMenu = NativeMethod.GetSystemMenu(Hwnd, false);
            
            bool isTop = (NativeMethod.GetWindowLong(Hwnd, NativeConstant.GWL_EXSTYLE).ToInt64() & NativeConstant.WS_EX_TOPMOST) != 0;
            uint topMost = isTop ? NativeConstant.MFS_CHECKED : NativeConstant.MFS_UNCHECKED;

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

        // 場所を開く(&O)
        public static void OnPathMenuItemClick(IntPtr Hwnd) {
            try {
                Process[] allProcess = Process.GetProcesses();
                Process targetProcess = null;
                foreach (Process process in allProcess) {
                    // アクセスが拒否されました。
                    // if (process.MainWindowHandle == Hwnd || process.Handle == Hwnd) {
                    if (process.MainWindowHandle == Hwnd) {
                        targetProcess = process;
                        break;
                    }
                }
                if (targetProcess != null) 
                    Process.Start("explorer.exe", "/select," + targetProcess.MainModule.FileName);
                else 
                    MessageBox.Show("プロセスの場所は見つかりません。", "場所を開く", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                MessageBox.Show("プロセスの場所は見つかりません。", "場所を開く", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
