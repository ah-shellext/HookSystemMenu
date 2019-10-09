using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace SystemMenuShell {

    static class NativeMethod {

        // https://www.pinvoke.net/default.aspx/user32/GetWindowRect.html

        // http://chokuto.ifdef.jp/urawaza/struct/MENUITEMINFO.html

        [StructLayout(LayoutKind.Sequential)]
        public struct MENUITEMINFO {
            public uint cbSize;
            public uint fMask;
            public uint fType;
            public uint fState;
            public uint wID;
            public IntPtr hSubMenu;
            public IntPtr hbmpChecked;
            public IntPtr hbmpUnchecked;
            public IntPtr dwItemData;
            public string dwTypeData;
            public uint cch;
            public IntPtr hbmpItem;

            public static uint sizeOf {
                get { return (uint)Marshal.SizeOf(typeof(MENUITEMINFO)); }
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Rect {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;

            public int Width { get { return Right - Left; } }
            public int Height { get { return Bottom - Top; } }
        }

        // Menu

        [DllImport("user32.dll")]
        public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("user32.dll")]
        public static extern bool InsertMenuItem(IntPtr hMenu, uint uItem, bool fByPosition, [In] ref MENUITEMINFO lpmii);

        [DllImport("user32.dll")]
        public static extern IntPtr CreateMenu();

        [DllImport("user32.dll")]
        public static extern uint GetMenuState(IntPtr hmenu, uint uItem, uint uFlags);

        [DllImport("user32.dll")]
        public static extern uint CheckMenuItem(IntPtr hmenu, uint uItem, uint uCheck);

        [DllImport("user32.dll")]
        public static extern bool CheckMenuRadioItem(IntPtr hMenu, uint idFirst, uint idLast, uint idCheck, uint uFlags);

        [DllImport("user32.dll")]
        public static extern bool EnableMenuItem(IntPtr hMenu, uint uItem, uint uEnable);

        [DllImport("user32.dll")]
        public static extern bool DeleteMenu(IntPtr hMenu, uint uPosition, uint uFlags);

        [DllImport("user32.dll")]
        public static extern bool IsMenu(IntPtr hMenu);

        // Hook

        [DllImport("user32.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int RegisterShellHookWindow(IntPtr hWnd);

        // Message

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern uint RegisterWindowMessage(string lpString);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool SendNotifyMessage(int hWnd, uint Msg, int wParam, int lParam);

        // Window

        [DllImport("user32.dll")]
        public static extern IntPtr GetThreadDesktop(int threadId);

        [DllImport("User32.Dll")]
        public static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindow(IntPtr hWnd, uint uCmd);

        public static IntPtr GetNextWindow(IntPtr hWnd) {
            return GetWindow(hWnd, NativeConstant.GW_HWNDNEXT);
        }

        [DllImport("user32.dll")]
        public static extern IntPtr GetParent(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int nMaxCount);

        [DllImport("user32.dll")]
        public static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool GetWindowRect(IntPtr hwnd, out Rect lpRect);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool PrintWindow(IntPtr hwnd, IntPtr hDC, uint nFlags);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
    }
}
