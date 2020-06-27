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

        public MainForm() {
            InitializeComponent();
        }

        protected override void WndProc(ref Message m) {
            base.WndProc(ref m);

            // Init Hook:
            // 間違ったフォーマットのプログラムを読み込もうとしました。
            if (m.Msg == NativeConstant.WM_CREATE) {
                onStartHook();
            } else if (m.Msg == NativeConstant.WM_DESTROY) {
                onStopHook();
            } else if (m.Msg == NativeConstant.WM_SHOWWINDOW) {
                this.Hide();
            }
            
            // Proc Shell Hook Msg:
            if (m.Msg == HookMessage.MSG_HSHELL_WINDOWCREATED || m.Msg == HookMessage.MSG_HCBT_CREATEWND) {
                onWindowCreated(m.WParam); 
            } else if (m.Msg == HookMessage.MSG_HSHELL_WINDOWDESTROYED || m.Msg == HookMessage.MSG_HCBT_DESTROYWND) { 
                onWindowDestroyed(m.WParam);
            } else if (m.Msg == HookMessage.MSG_HSHELL_WINDOWACTIVATED || m.Msg == HookMessage.MSG_HCBT_ACTIVATE || m.Msg == HookMessage.MSG_HCBT_SETFOCUS) { 
                onWindowActivated(m.WParam); 
            }

            // Proc GetMsg Hook Msg:
            if (m.Msg == HookMessage.MSG_HGETMSG_GETMSG) {
                addToList(m.WParam, "GetMsg");
                WinUtil.cacheHandle = m.WParam;
                WinUtil.cacheMessage = m.LParam;
            } else if (m.Msg == HookMessage.MSG_HGETMSG_GETMSG_PARAMS) {
                if (WinUtil.cacheHandle != IntPtr.Zero && WinUtil.cacheMessage != IntPtr.Zero) {
                    addToList(WinUtil.cacheHandle, "GetMsgParam");
                    onWinProcMsg(WinUtil.cacheHandle, WinUtil.cacheMessage, m.WParam, m.LParam);
                    WinUtil.cacheHandle = WinUtil.cacheMessage = IntPtr.Zero;
                }
            }
        }

        // ui
        private void addToList(IntPtr hwnd, string token) {
            string title = WinUtil.GetWindowTitle(hwnd);
            if (title != "") {
                listBox1.Items.Add(token + ": 0x" + hwnd.ToInt64().ToString("X6") + " " + title);
            }
            listBox1.ClearSelected();
            if (listBox1.Items.Count != 0) {
                listBox1.SetSelected(listBox1.Items.Count - 1, true);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Handle events

        // current windows
        List<IntPtr> currentWinList;

        private void onStartHook() {
            HookMessage.RegisterMsg();
            HookMethod.InitGetMsgHook(0, Handle);
            HookMethod.InitCallWndProcHook(0, Handle);
            HookMethod.InitShellHook(0, Handle);
            HookMethod.InitCbtHook(0, Handle);

            currentWinList = WinUtil.GetAllWindows();
            foreach (var hwnd in currentWinList) {
                addToList(hwnd, "Exist");
                if (MenuUtil.InsertSystemMenu(hwnd)) {
                    MenuUtil.InitMenuItemState(hwnd);
                }
            }
        }

        private void onStopHook() {
            foreach (var hwnd in currentWinList) {
                MenuUtil.RemoveSystemMenu(hwnd);
            }

            HookMethod.UnInitCbtHook();
            HookMethod.UnInitShellHook();
            HookMethod.UnInitCallWndProcHook();
            HookMethod.UnInitGetMsgHook();
            HookMessage.UnregisterMsg();
        }

        private void onWindowCreated(IntPtr hwnd) {
            var newList = WinUtil.GetAllWindows();
            button1.Text = "Cre " + newList.Count.ToString();
            foreach (var newHwnd in newList.Except(currentWinList)) {
                addToList(newHwnd, "Create");
                if (MenuUtil.InsertSystemMenu(hwnd)) {
                    MenuUtil.InitMenuItemState(hwnd);
                 }
            }
            currentWinList = newList;
        }

        private void onWindowDestroyed(IntPtr hwnd) {
            var newList = WinUtil.GetAllWindows();
            button1.Text = "Des " + newList.Count.ToString();
            foreach (var oldHwnd in currentWinList.Except(newList)) {
                addToList(oldHwnd, "Delete");
                MenuUtil.RemoveSystemMenu(hwnd);
            }
            currentWinList = newList;
        }

        private void onWindowActivated(IntPtr hwnd) {
            if (WinUtil.IsWindow(hwnd)) {
                MenuUtil.InitMenuItemState(hwnd);
            }

            var newList = WinUtil.GetAllWindows();
            button1.Text = "Act " + newList.Count.ToString();

            foreach (var newHwnd in newList.Except(currentWinList)) {
                addToList(newHwnd, "Create(Act)");
                if (MenuUtil.InsertSystemMenu(hwnd)) {
                    MenuUtil.InitMenuItemState(hwnd);
                }
            }
            foreach (var oldHwnd in currentWinList.Except(newList)) {
                addToList(oldHwnd, "Delete(Act)");
                MenuUtil.RemoveSystemMenu(oldHwnd);
            }
            currentWinList = newList;
        }

        private void onWinProcMsg(IntPtr hwnd, IntPtr message, IntPtr WParam, IntPtr LParam) {
            if (message.ToInt64() == NativeConstant.WM_SYSCOMMAND) {
                uint menuid = (uint) (WParam.ToInt64() & 0x0000FFFF);

                switch (menuid) {
                    case MenuUtil.MENU_ID_TOPMOST:
                        MenuUtil.OnTopMostMenuItemClick(hwnd);
                        break;
                    case MenuUtil.MENU_ID_PRTSC:
                        MenuUtil.OnPrtScMenuItemClick(hwnd);
                        break;
                    case MenuUtil.MENU_ID_PATH:
                        MenuUtil.OnPathMenuItemClick(hwnd);
                        break;
                }
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Others

        private bool hookStoped = false;

        private void MainForm_Load(object sender, EventArgs e) {
            ContextMenu ctxMenu = new ContextMenu();
            ctxMenu.MenuItems.Add(new MenuItem("モニターを表示(&S)", new EventHandler((sender2, e2) => {
                Show();
            })));
            ctxMenu.MenuItems.Add(new MenuItem("ホックを終了(&E)", new EventHandler((sender2, e2) => {
                hookStoped = true;
                Close();
            })));

            ctxMenu.MenuItems[0].DefaultItem = true;
            notifyIcon.ContextMenu = ctxMenu;

            this.TopMost = true;
            new Thread(new ThreadStart(() => {
                Thread.Sleep(100);
                // this.Invoke(new Action(() => Hide() ));
            })).Start();
        }

        private void notifyIcon_MouseClick(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                notifyIcon.ContextMenu.MenuItems[0].PerformClick();
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
            if (hookStoped) {
                e.Cancel = false;
            } else {
                e.Cancel = true;
                Hide();
            }
        }

        private void button1_Click(object sender, EventArgs e) {
            Form form = new Form();
            form.Text = "Test";
            form.TopMost = true;
            form.Show();
        }
    }
}
