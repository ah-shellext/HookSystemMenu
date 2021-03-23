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

        public MainForm() {
            InitializeComponent();
        }

        public MainForm(bool slave, IntPtr masterHwnd, string anotherExePath) {
            InitializeComponent();
            _isSlave = slave;
            _masterHwnd = masterHwnd;
            _anotherExePath = anotherExePath;
        }

        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);

            if (!_isSlave) {
                var psi = new ProcessStartInfo(_anotherExePath) {
                    Arguments = string.Format("SLAVEOF {0:X}", _masterHwnd.ToInt64()),
                    Verb = "runas"
                };
                var process = new Process { StartInfo = psi };
                process.Start();
                Utils.SetWindowTopMost(Handle, true);
            } else {
                // Hide();
            }
            Text += IntPtr.Size == 4 ? " (x86)" : " (x64)";
            HookMessages.RegisterMessages();
            HookMethods.StartHook(Handle);
        }

        protected override void OnFormClosing(FormClosingEventArgs e) {
            HookMethods.StopHook();
            HookMessages.UnregisterMessages();

            base.OnFormClosing(e);
        }

        protected override void WndProc(ref Message m) {
            base.WndProc(ref m);

            if (_isSlave) {
                // Send
            }
            if (m.Msg == HookMessages.MSG_HSHELL_WINDOWCREATED /*|| m.Msg == HookMessages.MSG_HCBT_CREATEWND*/) {
                AddToList("Create", m.WParam);
            } else if (m.Msg == HookMessages.MSG_HSHELL_WINDOWDESTROYED /*|| m.Msg == HookMessages.MSG_HCBT_DESTROYWND*/) {
                AddToList("Destroy", m.WParam, true);
            } else if (m.Msg == HookMessages.MSG_HSHELL_WINDOWACTIVATED /*|| m.Msg == HookMessages.MSG_HCBT_ACTIVATE*/) {
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
