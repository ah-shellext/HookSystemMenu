using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Windows.Forms;

namespace SystemMenuImpl {

    class SystemMenu {

        public const uint MENUID_START = 0xA5F8;
        public const uint MENUID_SPLITTER = MENUID_START + 1;
        public const uint MENUID_TOPMOST = MENUID_START + 2;
        public const uint MENUID_SENDTOBACK = MENUID_START + 3;
        public const uint MENUID_COPYSCREENSHOT = MENUID_START + 4;
        public const uint MENUID_OPENPROCESSPATH = MENUID_START + 5;
        public const uint MENUID_WINDOWINFORMATION = MENUID_START + 6;

        public static bool InsertSystmMenu(IntPtr hwnd) {
            IntPtr sysMenu = NativeMethods.GetSystemMenu(hwnd, false);
            if (sysMenu == IntPtr.Zero) {
                return false;
            }
            uint index = 5; // (uint) NativeMethods.GetMenuItemCount(sysMenu);

            var splitter = new NativeMethods.MENUITEMINFO {
                cbSize = NativeMethods.MENUITEMINFO.SizeOf,
                fMask = NativeConstants.MIIM_ID | NativeConstants.MIIM_FTYPE,
                wID = MENUID_SPLITTER,
                fType = NativeConstants.MFT_SEPARATOR
            };

            var topMostItem = new NativeMethods.MENUITEMINFO {
                cbSize = NativeMethods.MENUITEMINFO.SizeOf,
                fMask = NativeConstants.MIIM_ID | NativeConstants.MIIM_STRING,
                wID = MENUID_TOPMOST,
                dwTypeData = "常に手前に表示(&P)",
            };

            var copyScreenshotItem = new NativeMethods.MENUITEMINFO {
                cbSize = NativeMethods.MENUITEMINFO.SizeOf,
                fMask = NativeConstants.MIIM_ID | NativeConstants.MIIM_STRING,
                wID = MENUID_COPYSCREENSHOT,
                dwTypeData = "スクリーンショットをコピー(&E)"
            };

            var openProcessItem = new NativeMethods.MENUITEMINFO {
                cbSize = NativeMethods.MENUITEMINFO.SizeOf,
                fMask = NativeConstants.MIIM_ID | NativeConstants.MIIM_STRING,
                wID = MENUID_OPENPROCESSPATH,
                dwTypeData = "プロセスの場所を開く(&O)"
            };

            var windowInformationItem = new NativeMethods.MENUITEMINFO {
                cbSize = NativeMethods.MENUITEMINFO.SizeOf,
                fMask = NativeConstants.MIIM_ID | NativeConstants.MIIM_STRING,
                wID = MENUID_WINDOWINFORMATION,
                dwTypeData = "ウィンドウの情報(&I)"
            };

            NativeMethods.InsertMenuItem(sysMenu, index, true, ref splitter);
            NativeMethods.InsertMenuItem(sysMenu, ++index, true, ref topMostItem);
            NativeMethods.InsertMenuItem(sysMenu, ++index, true, ref copyScreenshotItem);
            NativeMethods.InsertMenuItem(sysMenu, ++index, true, ref openProcessItem);
            NativeMethods.InsertMenuItem(sysMenu, ++index, true, ref windowInformationItem);

            return true;
        }

        public static bool RemoveSystemMenu(IntPtr hwnd) {
            IntPtr sysMenu = NativeMethods.GetSystemMenu(hwnd, false);
            if (sysMenu == IntPtr.Zero) {
                return false;
            }

            NativeMethods.DeleteMenu(sysMenu, MENUID_SPLITTER, NativeConstants.MF_BYCOMMAND);
            NativeMethods.DeleteMenu(sysMenu, MENUID_TOPMOST, NativeConstants.MF_BYCOMMAND);
            NativeMethods.DeleteMenu(sysMenu, MENUID_COPYSCREENSHOT, NativeConstants.MF_BYCOMMAND);
            NativeMethods.DeleteMenu(sysMenu, MENUID_OPENPROCESSPATH, NativeConstants.MF_BYCOMMAND);
            NativeMethods.DeleteMenu(sysMenu, MENUID_WINDOWINFORMATION, NativeConstants.MF_BYCOMMAND);

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

        public static void ClickCopyScreenshotMenuItem(IntPtr hwnd) {
            NativeMethods.GetWindowRect(hwnd, out NativeMethods.Rect rect);
            var bitmap = new Bitmap(rect.Width, rect.Height, PixelFormat.Format32bppArgb);
            using (var graphics = Graphics.FromImage(bitmap)) {
                var hdc = graphics.GetHdc();
                NativeMethods.PrintWindow(hwnd, hdc, 0);
                graphics.ReleaseHdc(hdc);
            }
            Clipboard.Clear();
            Clipboard.SetImage(bitmap);
        }

        public static void ClickOpenProcessPath(IntPtr hwnd) {
            Process process = Utils.GetProcessFromHwnd(hwnd);
            if (process == null) {
                MessageBox.Show("プロセスは見つかりません。", "場所を開く", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string path;
            try {
                path = process.MainModule.FileName;
            } catch {
                var fileNameBuilder = new StringBuilder(1024);
                var bufferLength = (uint) fileNameBuilder.Capacity + 1;
                path = NativeMethods.QueryFullProcessImageName(process.Handle, 0, fileNameBuilder, ref bufferLength) ? fileNameBuilder.ToString() : null;
            }
            Process.Start("explorer.exe", "/select," + path);
        }

        public static void ClickWindowInformationPath(IntPtr hwnd) {
            var title = Utils.GetWindowTitle(hwnd);
            var form = new InfoForm() {
                Text = string.Format("\"{0}\" の情報", title)
            };
            form.Show(Utils.GetIWin32WindowFromHwnd(hwnd));
        }
    }
}
