using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace SystemMenuImpl {

    static class Program {

        static readonly Mutex mutex = new Mutex(false, "{E6868BB1-D280-42AE-9439-1371E6304D5E}");

        static void ShowError(string message) {
            var title = string.Format("HookSystemMenu ({0})", IntPtr.Size == 4 ? "x86" : "x64");
            MessageBox.Show(new Form { TopMost = true }, message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        [STAThread]
        static void Main(string[] args) {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var slave = false;
            var masterHwnd = IntPtr.Zero;

            if (IntPtr.Size == 8) {
                slave = false; // master
                if (args.Length != 0) {
                    ShowError("Wrong arguments, x64 exe is only allowed to be a master, that takes no argument.");
                    return;
                }
            } else if (IntPtr.Size == 4) {
                slave = true; // slave
                if (args.Length != 2) {
                    ShowError("Wrong arguments, x86 exe is only allowed to be a slave, that takes two arguments.");
                    return;
                }
                if (args[0] != "SLAVEOF") {
                    ShowError("Wrong arguments, x86 exe is only allowed to be a slave, and the first argument must be SLAVEOF.");
                    return;
                }
                try {
                    masterHwnd = new IntPtr(int.Parse(args[1], NumberStyles.HexNumber));
                } catch {
                    ShowError("Wrong arguments, master hwnd should be a valid hex number value.");
                    return;
                }
            } else {
                ShowError("This exe is neither x86 nor x64 version, is not supported yet.");
                return;
            }

            if (!slave && !mutex.WaitOne(TimeSpan.FromSeconds(1), false)) {
                ShowError("Application has already started!");
                return;
            }

            var key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\AoiHosizora\HookSystemMenu");
            if (key == null) {
                ShowError(@"You have not set HookSystemMenu's registry setting, please check the HKEY_CURRENT_USER\SOFTWARE\AoiHosizora\HookSystemMenu key.");
                return;
            }
            var executablePath = (key.GetValue(IntPtr.Size == 4 ? "x64" : "x86") as string).Trim('"').Replace(@"\\", @"\");
            if (string.IsNullOrWhiteSpace(executablePath) || !File.Exists(executablePath)) {
                ShowError(@"HookSystemMenu's application file is not found, please check the HKEY_CURRENT_USER\SOFTWARE\AoiHosizora\HookSystemMenu key.");
                return;
            }

            try {
                Application.Run(new MainForm(slave, masterHwnd, executablePath));
            } finally {
                if (!slave) {
                    mutex.ReleaseMutex();
                }
            }
        }
    }
}
