using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;

namespace SystemMenuShell {

    public partial class MainForm : Form {

        // wID
        private const uint MENU_ID_03 = 0x0003;
        
        // MSG
        private uint MY_MESSAGE = 0;
        private const int HWND_BROADCAST = 0xffff;

        private IntPtr hSysMenu;

        public MainForm() {
            InitializeComponent();

            hSysMenu = NativeMethod.GetSystemMenu(this.Handle, false);

            // Split
            var splititem = new NativeMethod.MENUITEMINFO();
            splititem.cbSize = (uint) Marshal.SizeOf(splititem);
            splititem.fMask = NativeConstant.MIIM_FTYPE;
            splititem.fType = NativeConstant.MFT_SEPARATOR;

            // Disable
            var testitem3 = new NativeMethod.MENUITEMINFO();
            testitem3.cbSize = (uint) Marshal.SizeOf(testitem3);
            testitem3.fMask = NativeConstant.MIIM_STRING | NativeConstant.MIIM_ID | NativeConstant.MIIM_STATE;
            testitem3.dwTypeData = "MFS_GLAYED";
            testitem3.wID = MENU_ID_03;
            testitem3.fState = NativeConstant.MFS_DEFAULT;
            // testitem3.fState = NativeConstant.MFS_GLAYED;

            NativeMethod.InsertMenuItem(hSysMenu, 5, true, ref splititem);
            NativeMethod.InsertMenuItem(hSysMenu, 6, true, ref testitem3);
        }

        protected override void WndProc(ref Message m) {
            base.WndProc(ref m);

            // System menu event test:

            if (m.Msg == NativeConstant.WM_SYSCOMMAND) {
                uint menuid = (uint)(m.WParam.ToInt32() & 0xffff);

                switch (menuid) {
                    case MENU_ID_03:
                        // MessageBox.Show("MFS_GLAYED が選択されました。");
                        MY_MESSAGE = NativeMethod.RegisterWindowMessage("MY_MESSAGE");
                        if (MY_MESSAGE != 0)
                            NativeMethod.SendNotifyMessage(HWND_BROADCAST, MY_MESSAGE, 0, 0);
                        break;
                }
            } else if (m.Msg == MY_MESSAGE) {
                MessageBox.Show(m.Msg.ToString() + " From WndProc MY_MESSAGE");
            }

            // Init Hook:
            // 間違ったフォーマットのプログラムを読み込もうとしました。

            if (m.Msg == NativeConstant.WM_CREATE)
                onStartHook();
            else if (m.Msg == NativeConstant.WM_DESTROY)
                onStopHook();
            
            // Proc Shell Hook Msg:

            if (m.Msg == HookMessage.MSG_HSHELL_WINDOWCREATED || m.Msg == HookMessage.MSG_HCBT_CREATEWND)
                onWindowCreated(m.WParam);
            else if (m.Msg == HookMessage.MSG_HSHELL_WINDOWDESTROYED || m.Msg == HookMessage.MSG_HCBT_DESTROYWND)
                onWindowDestroyed(m.WParam);
            else if (m.Msg == HookMessage.MSG_HSHELL_WINDOWACTIVATED || m.Msg == HookMessage.MSG_HCBT_ACTIVATE || m.Msg == HookMessage.MSG_HCBT_SETFOCUS)
                onWindowActivated(m.WParam);

            // Proc GetMsg Hook Msg:

            if (m.Msg == HookMessage.MSG_HGETMSG_GETMSG) {
                addToList(m.WParam, "GetMsg");
                WinUtil.cacheHandle = m.WParam;
                WinUtil.cacheMessage = m.LParam;
            } else if (m.Msg == HookMessage.MSG_HGETMSG_GETMSG_PARAMS) {
                if (WinUtil.cacheHandle != IntPtr.Zero && WinUtil.cacheMessage != IntPtr.Zero) {

                    addToList(WinUtil.cacheHandle, "GetMsgParam"); 
                    onWinProcMsg(WinUtil.cacheHandle, WinUtil.cacheMessage, m.WParam, m.LParam);
                    WinUtil.cacheHandle = IntPtr.Zero;
                    WinUtil.cacheMessage = IntPtr.Zero;
                }
            }
        }

        private void addToList(IntPtr hwnd, string token) {
            string title = WinUtil.GetWindowTitle(hwnd);
            if (title != "") listBox1.Items.Add(token + ": 0x" + hwnd.ToInt64().ToString("X6") + " " + title);
            listBox1.SelectedIndex = listBox1.Items.Count - 1;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Handle events
        List<IntPtr> WinList;

        private void onStartHook() {
            WinList = WinUtil.GetAllWindows();
            foreach (var hwnd in WinList) {
                addToList(hwnd, "Exist");
                WinUtil.InsertSystemMenu(hwnd);
                WinUtil.InitMenuItemState(hwnd);
            }

            HookMessage.RegisterMsg();

            HookMethod.InitGetMsgHook(0, Handle);
            HookMethod.InitShellHook(0, Handle);
            HookMethod.InitCbtHook(0, Handle);
        }

        private void onStopHook() {
            foreach (var hwnd in WinList) {
                WinUtil.RemoveSystemMenu(hwnd);
            }

            HookMethod.UnInitGetMsgHook();
            HookMethod.UnInitShellHook();
            HookMethod.UnInitCbtHook();
        }

        private void onWindowCreated(IntPtr hwnd) {
            var newList = WinUtil.GetAllWindows();
            button1.Text = "Cre " + newList.Count.ToString();
            foreach (var newHwnd in newList.Except(WinList)) {
                addToList(newHwnd, "Create");
                WinUtil.InsertSystemMenu(hwnd);
                WinUtil.InitMenuItemState(hwnd);
            }
            WinList = newList;
        }

        private void onWindowDestroyed(IntPtr hwnd) {
            var newList = WinUtil.GetAllWindows();
            button1.Text = "Des " + newList.Count.ToString();
            foreach (var oldHwnd in WinList.Except(newList)) {
                addToList(oldHwnd, "Delete");
                WinUtil.RemoveSystemMenu(hwnd);
            }
            WinList = newList;
        }

        private void onWindowActivated(IntPtr hwnd) {
            if (WinUtil.IsWindow(hwnd))
                WinUtil.InitMenuItemState(hwnd);

            var newList = WinUtil.GetAllWindows();
            button1.Text = "Act " + newList.Count.ToString();

            foreach (var newHwnd in newList.Except(WinList)) {
                addToList(newHwnd, "Create(Act)");
                WinUtil.InsertSystemMenu(newHwnd);
                WinUtil.InitMenuItemState(newHwnd);
            }

            foreach (var oldHwnd in WinList.Except(newList)) {
                addToList(oldHwnd, "Delete(Act)");
                WinUtil.RemoveSystemMenu(oldHwnd);
            }

            WinList = newList;
        }

        private void onWinProcMsg(IntPtr hwnd, IntPtr message, IntPtr WParam, IntPtr LParam) {
            // TODO
            if (message.ToInt64() == NativeConstant.WM_SYSCOMMAND) {
                uint menuid = (uint)(WParam.ToInt32() & 0xffff);
                if (menuid == WinUtil.MENU_ID_TOPMOST)
                    WinUtil.OnTopMostMenuItemClick(hwnd);
                else if (menuid == WinUtil.MENU_ID_PRTSC)
                    WinUtil.OnPrtScMenuItemClick(hwnd);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Others

        private void MainForm_Load(object sender, EventArgs e) {
            this.TopMost = true;
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e) {
            
        }

        private void button1_Click(object sender, EventArgs e) {
            Form form = new Form();
            form.Text = "Test";
            form.TopMost = true;
            form.Show();
        }
    }
}
