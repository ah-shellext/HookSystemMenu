using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using EasyHook;

namespace SystemMenuShell
{
    class MainForm : Form
    {
        // https://www.ipentec.com/document/csharp-add-menu-item-in-system-menu
        // http://www-higashi.ist.osaka-u.ac.jp/~k-maeda/vcpp/sec6-3menuapp.html
        // http://chokuto.ifdef.jp/urawaza/struct/MENUITEMINFO.html
        // https://www.pinvoke.net/default.aspx/user32.CreateMenu

        [StructLayout(LayoutKind.Sequential)]
        struct MENUITEMINFO
        {
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

            // return the size of the structure
            public static uint sizeOf
            {
                get { return (uint)Marshal.SizeOf(typeof(MENUITEMINFO)); }
            }
        }

        [DllImport("user32.dll")]
        static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("user32.dll")]
        static extern bool InsertMenuItem(IntPtr hMenu, uint uItem, bool fByPosition, [In] ref MENUITEMINFO lpmii);

        [DllImport("user32.dll")]
        static extern IntPtr CreateMenu();

        [DllImport("user32.dll")]
        static extern uint GetMenuState(IntPtr hmenu, uint uItem, uint uFlags);

        [DllImport("user32.dll")]
        static extern uint CheckMenuItem(IntPtr hmenu, uint uItem, uint uCheck);

        [DllImport("user32.dll")]
        static extern bool CheckMenuRadioItem(IntPtr hMenu, uint idFirst, uint idLast, uint idCheck, uint uFlags);

        [DllImport("user32.dll")]
        static extern bool EnableMenuItem(IntPtr hMenu, uint uItem, uint uEnable);

        // Mask
        private const uint MIIM_STATE = 0x00000001;
        private const uint MIIM_ID = 0x00000002;
        private const uint MIIM_SUBMENU = 0x00000004;
        private const uint MIIM_CHECKMARKS = 0x00000008;
        private const uint MIIM_STRING = 0x00000040;
        private const uint MIIM_FTYPE = 0x00000100;

        // Type
        private const uint MFT_STRING = 0x00000000;
        private const uint MFT_RADIOCHECK = 0x00000200;
        private const uint MFT_SEPARATOR = 0x00000800;

        // State
        private const uint MFS_ENABLED = 0x00000000;
        private const uint MFS_UNCHECKED = 0x00000000;
        private const uint MFS_UNHILITE = 0x00000000;
        private const uint MFS_GLAYED = 0x00000003;
        private const uint MFS_DISABLED = 0x00000003;
        private const uint MFS_CHECKED = 0x00000008;
        private const uint MFS_HILITE = 0x00000080;
        private const uint MFS_DEFAULT = 0x00001000;

        ////////////////////////////////////////////////////////////////////////////////////
        // Message
        private const uint WM_SYSCOMMAND = 0x0112;
        private const uint MF_BYCOMMAND = 0x0;

        // wID
        private const uint MENU_ID_01 = 0x0001;
        private const uint MENU_ID_02 = 0x0002;
        private const uint MENU_ID_03 = 0x0003;
        private const uint MENU_ID_04 = 0x0004;
        private const uint MENU_ID_05 = 0x0005;
        private const uint MENU_ID_06 = 0x0006;

        private IntPtr hSysMenu;

        public MainForm()
        {
            hSysMenu = GetSystemMenu(this.Handle, false);

            MENUITEMINFO splititem = new MENUITEMINFO();
            splititem.cbSize = (uint)Marshal.SizeOf(splititem);
            splititem.fMask = MIIM_FTYPE;
            splititem.fType = MFT_SEPARATOR;

            // Check
            MENUITEMINFO testitem1 = new MENUITEMINFO();
            testitem1.cbSize = (uint)Marshal.SizeOf(testitem1);
            testitem1.fMask = MIIM_STRING | MIIM_ID | MIIM_STATE;
            testitem1.dwTypeData = "MFS_CHECKED";
            testitem1.wID = MENU_ID_01;
            testitem1.fState = MFS_CHECKED;

            // String
            MENUITEMINFO testitem2 = new MENUITEMINFO();
            testitem2.cbSize = (uint)Marshal.SizeOf(testitem2);
            testitem2.fMask = MIIM_STRING | MIIM_ID;
            testitem2.dwTypeData = "MIIM_STRING";
            testitem2.wID = MENU_ID_02;

            // Disable
            MENUITEMINFO testitem3 = new MENUITEMINFO();
            testitem3.cbSize = (uint)Marshal.SizeOf(testitem3);
            testitem3.fMask = MIIM_STRING | MIIM_ID | MIIM_STATE;
            testitem3.dwTypeData = "MFS_GLAYED";
            testitem3.wID = MENU_ID_03;
            testitem3.fState = MFS_GLAYED;

            // Radio
            MENUITEMINFO testitem4 = new MENUITEMINFO();
            testitem4.cbSize = (uint)Marshal.SizeOf(testitem4);
            testitem4.fMask = MIIM_STRING | MIIM_ID | MIIM_FTYPE | MIIM_CHECKMARKS | MIIM_STATE;
            testitem4.dwTypeData = "MFT_RADIOCHECK_1";
            testitem4.wID = MENU_ID_04;
            testitem4.fType = MFT_RADIOCHECK;
            testitem4.hbmpChecked = IntPtr.Zero;
            testitem4.fState = MFS_CHECKED;

            MENUITEMINFO testitem5 = new MENUITEMINFO();
            testitem5.cbSize = (uint)Marshal.SizeOf(testitem5);
            testitem5.fMask = MIIM_STRING | MIIM_ID | MIIM_FTYPE | MIIM_CHECKMARKS | MIIM_STATE;
            testitem5.dwTypeData = "MFT_RADIOCHECK_2";
            testitem5.wID = MENU_ID_05;
            testitem5.fType = MFT_RADIOCHECK;
            testitem5.hbmpChecked = IntPtr.Zero;
            testitem5.fState = MFS_UNCHECKED;

            MENUITEMINFO testitem6 = new MENUITEMINFO();
            testitem6.cbSize = (uint)Marshal.SizeOf(testitem6);
            testitem6.fMask = MIIM_STRING | MIIM_ID | MIIM_FTYPE | MIIM_CHECKMARKS | MIIM_STATE;
            testitem6.dwTypeData = "MFT_RADIOCHECK_3";
            testitem6.wID = MENU_ID_06;
            testitem6.fType = MFT_RADIOCHECK;
            testitem6.hbmpChecked = IntPtr.Zero;
            testitem6.fState = MFS_UNCHECKED;

            // SubMenu
            IntPtr hSubMenu = CreateMenu();

            MENUITEMINFO testitem7 = new MENUITEMINFO();
            testitem7.cbSize = (uint)Marshal.SizeOf(testitem7);
            testitem7.fMask = MIIM_STRING | MIIM_SUBMENU;
            testitem7.dwTypeData = "MIIM_SUBMENU";
            testitem7.hSubMenu = hSubMenu;

            InsertMenuItem(hSysMenu, 5, true, ref splititem);
            InsertMenuItem(hSysMenu, 6, true, ref testitem1);
            InsertMenuItem(hSysMenu, 7, true, ref testitem2);
            InsertMenuItem(hSysMenu, 8, true, ref testitem3);
            InsertMenuItem(hSysMenu, 9, true, ref testitem7);

            InsertMenuItem(hSubMenu, 0, true, ref testitem4);
            InsertMenuItem(hSubMenu, 1, true, ref testitem5);
            InsertMenuItem(hSubMenu, 2, true, ref testitem6);

            // HookProc proc = new HookProc(Proc);

            // using (Process process = Process.GetCurrentProcess()) {
            //     using (ProcessModule module = process.MainModule) {
            //         IntPtr hModule = GetModuleHandle(module.ModuleName);
            //         hHook = SetWindowsHookEx(WH_SHELL, proc, hModule, 0);
            //     }
            // }

            // RegisterShellHookWindow(this.Handle);

        }

        int MY_MESSAGE = RegisterWindowMessage("MY_MESSAGE");
        int HWND_BROADCAST = 0xffff;

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == WM_SYSCOMMAND)
            {
                uint menuid = (uint)(m.WParam.ToInt32() & 0xffff);
                uint state;

                switch (menuid)
                {
                    case MENU_ID_01: // 6
                        state = GetMenuState(hSysMenu, MENU_ID_01, MF_BYCOMMAND);
                        if ((state & MFS_CHECKED) != 0x0)
                            CheckMenuItem(hSysMenu, MENU_ID_01, MF_BYCOMMAND | MFS_UNCHECKED);
                        else
                            CheckMenuItem(hSysMenu, MENU_ID_01, MF_BYCOMMAND | MFS_CHECKED);
                        break;
                    case MENU_ID_02: // 7
                        state = GetMenuState(hSysMenu, MENU_ID_03, MF_BYCOMMAND);
                        if ((state & MFS_DISABLED) != 0x0)
                            EnableMenuItem(hSysMenu, MENU_ID_03, MF_BYCOMMAND | MFS_ENABLED);
                        else
                            EnableMenuItem(hSysMenu, MENU_ID_03, MF_BYCOMMAND | MFS_DISABLED);
                        break;
                    case MENU_ID_03: // 8
                        MessageBox.Show("MFS_GLAYED が選択されました。");
                        SendNotifyMessage(HWND_BROADCAST, MY_MESSAGE, 0, 0);
                        break;
                    case MENU_ID_04: // 9
                    case MENU_ID_05: // 10
                    case MENU_ID_06: // 11
                        CheckMenuRadioItem(hSysMenu, MENU_ID_04, MENU_ID_06, menuid, MF_BYCOMMAND);
                        break;
                }
            }
            else if (m.Msg == MY_MESSAGE) {
                MessageBox.Show(m.Msg.ToString() + " = from wndproc");
            }
        }

        static IntPtr hHook;

        private delegate int HookProc(int code, IntPtr wParam, IntPtr lParam);

        private int Proc(int code, IntPtr wParam, IntPtr lParam) {
            MessageBox.Show(code.ToString());
            return CallNextHookEx(hHook, code, wParam, lParam);
        }

        private int WH_SHELL = 10;

        [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "SetWindowsHookEx", SetLastError = true)]
        static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hMod, uint dwThreadId);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern int CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("user32.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern int RegisterShellHookWindow(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int RegisterWindowMessage(string lpString);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern bool SendNotifyMessage(int hWnd, int Msg, int wParam, int lParam);

    }
}