using System.Windows.Forms;

namespace SystemMenuImpl {

    public partial class MainForm : Form {

        public MainForm() {
            InitializeComponent();
        }

        protected override void WndProc(ref Message m) {
            base.WndProc(ref m);

            if (m.Msg == NativeConstants.WM_CREATE) {
                HookMessages.RegisterMessages();
                HookMethods.StartHook(Handle);
            } else if (m.Msg == NativeConstants.WM_DESTROY) {
                HookMethods.StopHook();
                HookMessages.UnregisterMessages();
            }
        }
    }
}
