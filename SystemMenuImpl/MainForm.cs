using System;
using System.Security.Permissions;
using System.Windows.Forms;

namespace SystemMenuImpl {

    public partial class MainForm : Form {

        public MainForm() {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);
            ClearList();
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

            if (m.Msg == HookMessages.MSG_HSHELL_WINDOWCREATED || m.Msg == HookMessages.MSG_HCBT_CREATEWND) {
                AddToList("Create", m.WParam);
            } else if (m.Msg == HookMessages.MSG_HSHELL_WINDOWDESTROYED || m.Msg == HookMessages.MSG_HCNT_DESTROYWND) {
                AddToList("Destroy", m.WParam);
            } else if (m.Msg == HookMessages.MSG_HSHELL_WINDOWACTIVATED || m.Msg == HookMessages.MSG_HCBT_ACTIVATE) {
                AddToList("Activate", m.WParam);
            }
        }

        private void ClearList() {
            lbMessages.Items.Clear();
        }

        private void AddToList(string message, IntPtr hWnd) {
            if (Utils.IsWindow(hWnd)) {
                lbMessages.Items.Add(message + " " + Utils.GetWindowTitle(hWnd));
            }
        }
    }
}
