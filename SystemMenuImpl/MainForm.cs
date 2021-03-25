using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace SystemMenuImpl {

    public partial class MainForm : Form {

        private readonly bool _isSlave;
        private readonly IntPtr _masterHwnd;
        private readonly string _anotherExePath;
        private Process _x86Process;

        public MainForm(bool slave, IntPtr masterHwnd, string anotherExePath) {
            InitializeComponent();
            _isSlave = slave;
            _masterHwnd = masterHwnd;
            _anotherExePath = anotherExePath;
        }

        protected override void OnShown(EventArgs e) {
            if (!_isSlave) {
                base.OnShown(e);
                ntfIcon.Visible = true;
            } else {
                Hide();
            }
        }

        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);

            if (!_isSlave) {
                var psi = new ProcessStartInfo(_anotherExePath) {
                    Arguments = string.Format("SLAVEOF {0:X}", Handle.ToInt64()),
                    Verb = "runas"
                };
                _x86Process = new Process { StartInfo = psi };
                _x86Process.Start();

                Utils.SetWindowTopMost(Handle, true);
                Utils.CurrentWindowsList = Utils.GetAllWindows();
                foreach (var hwnd in Utils.CurrentWindowsList) {
                    AddToList("Exist", hwnd);
                    if (SystemMenu.InsertSystmMenu(hwnd)) {
                        SystemMenu.InitializeSystemMenu(hwnd);
                    }
                }
            }

            Text += IntPtr.Size == 4 ? " (x86)" : " (x64)";

            HookMessages.RegisterMessages();
            HookMethods.StartHook(Handle);
        }

        protected override void OnFormClosing(FormClosingEventArgs e) {
            HookMethods.StopHook();
            HookMessages.UnregisterMessages();

            if (!_isSlave) {
                foreach (var hwnd in Utils.CurrentWindowsList) {
                    SystemMenu.RemoveSystemMenu(hwnd);
                }
                if (_x86Process != null && !_x86Process.HasExited) {
                    Utils.ExitProcess(_x86Process);
                }
            }

            base.OnFormClosing(e);
        }

        protected override void WndProc(ref Message m) {
            base.WndProc(ref m);

            if (m.Msg == HookMessages.MSG_HSHELL_WINDOWCREATED) {
                OnWindowCreated(m);
            } else if (m.Msg == HookMessages.MSG_HSHELL_WINDOWDESTROYED) {
                OnWindowDestroyed(m);
            } else if (m.Msg == HookMessages.MSG_HSHELL_WINDOWACTIVATED) {
                OnWindowActivated(m);
            } else if (m.Msg == HookMessages.MSG_HGETMESSAGE) {
                OnWindowGetMessage(m);
            } else if (m.Msg == HookMessages.MSG_HGETMESSAGE_PARAMS) {
                OnWindowGetMessageParams(m);
            }
        }

        private void OnWindowCreated(Message m) {
            if (_isSlave) {
                NativeMethods.SendNotifyMessage(_masterHwnd, HookMessages.MSG_HSHELL_WINDOWCREATED, m.WParam, m.LParam);
                return;
            }

            var hwnd = m.WParam;
            if (Utils.CurrentWindowsList.Contains(hwnd)) {
                return;
            }
            Utils.CurrentWindowsList.Add(hwnd);
            AddToList("Create", hwnd);
            if (SystemMenu.InsertSystmMenu(hwnd)) {
                SystemMenu.InitializeSystemMenu(hwnd);
            }
        }

        private void OnWindowDestroyed(Message m) {
            if (_isSlave) {
                NativeMethods.SendNotifyMessage(_masterHwnd, HookMessages.MSG_HSHELL_WINDOWDESTROYED, m.WParam, m.LParam);
                return;
            }

            var hwnd = m.WParam;
            if (!Utils.CurrentWindowsList.Contains(hwnd)) {
                return;
            }
            Utils.CurrentWindowsList.Remove(hwnd);
            AddToList("Destroy", hwnd, true);
            SystemMenu.RemoveSystemMenu(hwnd);
        }

        private void OnWindowActivated(Message m) {
            if (_isSlave) {
                NativeMethods.SendNotifyMessage(_masterHwnd, HookMessages.MSG_HSHELL_WINDOWACTIVATED, m.WParam, m.LParam);
                return;
            }

            var hwnd = m.WParam;
            AddToList("Activate", hwnd);
            SystemMenu.InitializeSystemMenu(hwnd);
        }

        private void OnWindowGetMessage(Message m) {
            if (_isSlave) {
                NativeMethods.SendNotifyMessage(_masterHwnd, HookMessages.MSG_HGETMESSAGE, m.WParam, m.LParam);
                return;
            }

            var hwnd = m.WParam;
            AddToList("GetMsg", hwnd);
            Utils.CachedHandle = m.WParam;
            Utils.CachedMessage = m.LParam;
        }

        private void OnWindowGetMessageParams(Message m) {
            if (_isSlave) {
                NativeMethods.SendNotifyMessage(_masterHwnd, HookMessages.MSG_HGETMESSAGE_PARAMS, m.WParam, m.LParam);
                return;
            }

            if (Utils.CachedHandle == IntPtr.Zero || Utils.CachedMessage == IntPtr.Zero) {
                return;
            }
            var hwnd = Utils.CachedHandle;
            var msg = Utils.CachedMessage;
            AddToList("GetMsgParams", Utils.CachedHandle);
            if (msg.ToInt64() == NativeConstants.WM_SYSCOMMAND) {
                uint menuId = (uint) (m.WParam.ToInt64() & 0x0000FFFF);
                switch (menuId) {
                case SystemMenu.MENUID_TOPMOST:
                    SystemMenu.ClickTopMostMenuItem(hwnd);
                    break;
                case SystemMenu.MENUID_SENDTOBACK:
                    SystemMenu.ClickSendToBackMenuItem(hwnd);
                    break;
                case SystemMenu.MENUID_COPYSCREENSHOT:
                    SystemMenu.ClickCopyScreenshotMenuItem(hwnd);
                    break;
                case SystemMenu.MENUID_OPENPROCESSPATH:
                    SystemMenu.ClickOpenProcessPath(hwnd);
                    break;
                case SystemMenu.MENUID_WINDOWINFORMATION:
                    SystemMenu.ClickWindowInformationPath(hwnd);
                    break;
                }
            }

            Utils.CachedHandle = IntPtr.Zero;
            Utils.CachedMessage = IntPtr.Zero;
        }

        private void AddToList(string message, IntPtr hwnd, bool more = false) {
            if (!Utils.IsWindow(hwnd, more)) {
                return;
            }
            lbMessages.Items.Add(string.Format("{0} {1} 0x{2}", message, Utils.GetWindowTitle(hwnd), hwnd.ToInt64().ToString("X6")));
            lbMessages.ClearSelected();
            if (lbMessages.Items.Count != 0) {
                lbMessages.SetSelected(lbMessages.Items.Count - 1, true);
            }
        }

        private void NtfIcon_MouseDoubleClick(object sender, MouseEventArgs e) {
            Show();
        }
    }
}
