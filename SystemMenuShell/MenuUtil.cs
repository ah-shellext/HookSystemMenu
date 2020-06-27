using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Diagnostics;

namespace SystemMenuShell {

    // 菜单相关：创建，移出，状态，事件
    static class MenuUtil {

        // 防止 ID 被占用：0xAF0X Prefix
        public const uint MENU_ID_SPLIT = 0xAF01;
        public const uint MENU_ID_TOPMOST = 0xAF02;
        public const uint MENU_ID_PRTSC = 0xAF03;
        public const uint MENU_ID_PATH = 0xAF04;

        public static bool InsertSystemMenu(IntPtr hwnd) {
            NativeMethod.GetSystemMenu(hwnd, true);
            IntPtr hSysMenu = NativeMethod.GetSystemMenu(hwnd, false);
            if (hSysMenu == IntPtr.Zero) {
                return false;
            }

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

            // Screenshots
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

        public static void RemoveSystemMenu(IntPtr hwnd) {
            IntPtr hSysMenu = NativeMethod.GetSystemMenu(hwnd, false);
            if (hSysMenu == IntPtr.Zero) {
                return;
            }

            NativeMethod.DeleteMenu(hSysMenu, MENU_ID_SPLIT, NativeConstant.MF_BYCOMMAND);
            NativeMethod.DeleteMenu(hSysMenu, MENU_ID_TOPMOST, NativeConstant.MF_BYCOMMAND);
            NativeMethod.DeleteMenu(hSysMenu, MENU_ID_PRTSC, NativeConstant.MF_BYCOMMAND);
            NativeMethod.DeleteMenu(hSysMenu, MENU_ID_PATH, NativeConstant.MF_BYCOMMAND);

            NativeMethod.GetSystemMenu(hwnd, true);
        }

        public static void InitMenuItemState(IntPtr hwnd) {
            bool isTop = WinUtil.IsWindowTopMost(hwnd);
            uint topMost = isTop ? NativeConstant.MFS_CHECKED : NativeConstant.MFS_UNCHECKED;

            IntPtr hSysMenu = NativeMethod.GetSystemMenu(hwnd, false);
            NativeMethod.CheckMenuItem(hSysMenu, MENU_ID_TOPMOST, NativeConstant.MF_BYCOMMAND | topMost);
        }

        // トップにピン(&P)
        public static void OnTopMostMenuItemClick(IntPtr hwnd) {
            bool isTop = (NativeMethod.GetWindowLong(hwnd, NativeConstant.GWL_EXSTYLE).ToInt64() & NativeConstant.WS_EX_TOPMOST) != 0;
            isTop = !isTop;
            WinUtil.SetWindowTopMost(hwnd, isTop);
            InitMenuItemState(hwnd);
        }

        // スクリーンショット(&C)
        public static void OnPrtScMenuItemClick(IntPtr hwnd) {
            WinUtil.CaptureWindow(hwnd);
        }

        // 場所を開く(&O)
        public static void OnPathMenuItemClick(IntPtr hwnd) {
            String hstr = "0x" + hwnd.ToInt64().ToString("X6");
            uint pid;
            NativeMethod.GetWindowThreadProcessId(hwnd, out pid);
            if (pid == 0L) {
                MessageBox.Show("プロセス (Win: " + hstr + ") は見つかりません。", "場所を開く", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Process targetProcess = Process.GetProcessById((int) pid);
            if (targetProcess != null) {
                Process.Start("explorer.exe", "/select," + targetProcess.MainModule.FileName);
            } else {
                MessageBox.Show("プロセス (pid: " + pid + ") は見つかりません。", "場所を開く", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
