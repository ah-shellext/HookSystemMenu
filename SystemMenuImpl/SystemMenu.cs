using System;

namespace SystemMenuImpl {

    class SystemMenu {

        public const uint MENUID_SPLITTER = 0xAF01;
        public const uint MENUID_TOPMOST = 0xAF02;

        public static bool InsertSystmMenu(IntPtr hwnd) {
            NativeMethods.GetSystemMenu(hwnd, true);
            IntPtr sysMenu = NativeMethods.GetSystemMenu(hwnd, false);
            if (sysMenu == IntPtr.Zero) {
                return false;
            }

            var splitter = new NativeMethods.MENUITEMINFO {
                cbSize = NativeMethods.MENUITEMINFO.sizeOf,
                fMask = NativeConstants.MIIM_ID | NativeConstants.MIIM_FTYPE,
                wID = MENUID_SPLITTER,
                fType = NativeConstants.MFT_SEPARATOR
            };

            var topMostItem = new NativeMethods.MENUITEMINFO {
                cbSize = NativeMethods.MENUITEMINFO.sizeOf,
                fMask = NativeConstants.MIIM_ID | NativeConstants.MIIM_STRING | NativeConstants.MIIM_STATE,
                wID = MENUID_TOPMOST,
                dwTypeData = "トップにピン(&P)",
                fState = NativeConstants.MFS_UNCHECKED
            };

            NativeMethods.InsertMenuItem(sysMenu, 5, true, ref splitter);
            NativeMethods.InsertMenuItem(sysMenu, 6, true, ref topMostItem);

            return true;
        }

        public static bool RemoveSystemMenu(IntPtr hwnd) {
            IntPtr sysMenu = NativeMethods.GetSystemMenu(hwnd, false);
            if (sysMenu == IntPtr.Zero) {
                return false;
            }

            NativeMethods.DeleteMenu(sysMenu, MENUID_SPLITTER, NativeConstants.MF_BYCOMMAND);
            NativeMethods.DeleteMenu(sysMenu, MENUID_TOPMOST, NativeConstants.MF_BYCOMMAND);

            NativeMethods.GetSystemMenu(hwnd, true);
            return true;
        }

        public static void InitializeSystemMenu(IntPtr hwnd) {
            bool isTopMost = Utils.IsWindowTopMost(hwnd);
            uint flag = isTopMost ? NativeConstants.MFS_CHECKED : NativeConstants.MFS_UNCHECKED;

            IntPtr sysMenu = NativeMethods.GetSystemMenu(hwnd, false);
            NativeMethods.CheckMenuItem(sysMenu, MENUID_TOPMOST, flag);
        }

        public static void ClickTopMostMenuItem(IntPtr hwnd) {
            bool isTopMost = Utils.IsWindowTopMost(hwnd);
            isTopMost = !isTopMost;
            Utils.SetWindowTopMost(hwnd, isTopMost);
            InitializeSystemMenu(hwnd);
        }
    }
}
