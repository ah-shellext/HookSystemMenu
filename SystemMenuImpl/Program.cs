using System;
using System.Threading;
using System.Windows.Forms;

namespace SystemMenuImpl {

    static class Program {

        // Static mutex, GC will not recycle this object.
        static readonly Mutex mutex = new Mutex(false, "{E6868BB1-D280-42AE-9439-1371E6304D5E}");

        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (!mutex.WaitOne(TimeSpan.FromSeconds(1), false)) {
                MessageBox.Show("Application already started!", "HookSystemMenu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try {
                Application.Run(new MainForm());
            } finally {
                mutex.ReleaseMutex();
            }
        }
    }
}
