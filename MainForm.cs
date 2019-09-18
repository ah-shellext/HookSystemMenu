using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace test
{
    class MainForm : Form
    {
        // https://www.ipentec.com/document/csharp-add-menu-item-in-system-menu

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

        private const uint MFT_BITMAP = 0x00000004;
        private const uint MFT_MENUBARBREAK = 0x00000020;
        private const uint MFT_MENUBREAK = 0x00000040;
        private const uint MFT_OWNERDRAW = 0x00000100;
        private const uint MFT_RADIOCHECK = 0x00000200;
        private const uint MFT_RIGHTJUSTIFY = 0x00004000;
        private const uint MFT_RIGHTORDER = 0x000002000;

        private const uint MFT_SEPARATOR = 0x00000800;
        private const uint MFT_STRING = 0x00000000;

        private const uint MIIM_FTYPE = 0x00000100;
        private const uint MIIM_STRING = 0x00000040;
        private const uint MIIM_ID = 0x00000002;

        private const uint WM_SYSCOMMAND = 0x0112;


        private const uint MENU_ID_01 = 0x0001;
        private const uint MENU_ID_02 = 0x0002;
        
        public MainForm()
        {
            IntPtr hSysMenu = GetSystemMenu(this.Handle, false);

            MENUITEMINFO splititem = new MENUITEMINFO();
            splititem.cbSize = (uint)Marshal.SizeOf(splititem);
            splititem.fMask = MIIM_FTYPE;
            splititem.fType = MFT_SEPARATOR;

            MENUITEMINFO testitem1 = new MENUITEMINFO();
            testitem1.cbSize = (uint)Marshal.SizeOf(testitem1);
            testitem1.fMask = MIIM_STRING | MIIM_ID;
            testitem1.wID = MENU_ID_01;
            testitem1.dwTypeData = "テスト1";

            MENUITEMINFO testitem2 = new MENUITEMINFO();
            testitem2.cbSize = (uint)Marshal.SizeOf(testitem2);
            testitem2.fMask = MIIM_STRING | MIIM_ID;
            testitem2.wID = MENU_ID_02;
            testitem2.dwTypeData = "テスト2";

            // Down to top
            InsertMenuItem(hSysMenu, 5, true, ref splititem);
            InsertMenuItem(hSysMenu, 6, true, ref testitem2);
            InsertMenuItem(hSysMenu, 6, true, ref testitem1);
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == WM_SYSCOMMAND)
            {
                uint menuid = (uint)(m.WParam.ToInt32() & 0xffff);

                switch (menuid)
                {
                    case MENU_ID_01:
                        MessageBox.Show("テスト1が選択されました。");
                        break;
                    case MENU_ID_02:
                        MessageBox.Show("テスト2が選択されました。");
                        break;

                }
            }
        }
    }
}