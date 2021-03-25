using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security.Permissions;
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
            }

            Text += IntPtr.Size == 4 ? " (x86)" : " (x64)";
            HookMessages.RegisterMessages();
            HookMethods.StartHook(Handle);
        }

        protected override void OnFormClosing(FormClosingEventArgs e) {
            if (!_isSlave && _x86Process != null && !_x86Process.HasExited) {
                Utils.ExitProcess(_x86Process);
            }

            HookMethods.StopHook();
            HookMessages.UnregisterMessages();

            base.OnFormClosing(e);
        }

        protected override void WndProc(ref Message m) {
            base.WndProc(ref m);

            if (m.Msg == HookMessages.MSG_HSHELL_WINDOWCREATED /* || m.Msg == HookMessages.MSG_HCBT_CREATEWND */) {
                if (_isSlave) {
                    NativeMethods.SendNotifyMessage(_masterHwnd, HookMessages.MSG_HSHELL_WINDOWCREATED, m.WParam, m.LParam);
                }
                AddToList("Create", m.WParam);
            } else if (m.Msg == HookMessages.MSG_HSHELL_WINDOWDESTROYED /* || m.Msg == HookMessages.MSG_HCBT_DESTROYWND */) {
                if (_isSlave) {
                    NativeMethods.SendNotifyMessage(_masterHwnd, HookMessages.MSG_HSHELL_WINDOWDESTROYED, m.WParam, m.LParam);
                }
                AddToList("Destroy", m.WParam, true);
            } else if (m.Msg == HookMessages.MSG_HSHELL_WINDOWACTIVATED /* || m.Msg == HookMessages.MSG_HCBT_ACTIVATE */) {
                if (_isSlave) {
                    NativeMethods.SendNotifyMessage(_masterHwnd, HookMessages.MSG_HSHELL_WINDOWACTIVATED, m.WParam, m.LParam);
                }
                AddToList("Activate", m.WParam);
            }
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
    }
}
