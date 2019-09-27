using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;

namespace SystemMenuShell {
    static class Program {

        public static int GetMainThreadId(this Process currentProcess) {
            int mainThreadId = -1;
            DateTime startTime = DateTime.MaxValue;
            foreach (ProcessThread thread in currentProcess.Threads) {
                if (thread.StartTime < startTime) {
                    startTime = thread.StartTime;
                    mainThreadId = thread.Id;
                }
            }
            return mainThreadId;
        }

        public static bool ExistProcessWithSameNameAndDesktop(Process currentProcess) {
            foreach (Process process in Process.GetProcessesByName(currentProcess.ProcessName)) {
                if (currentProcess.Id != process.Id) {
                    int processThreadId = process.GetMainThreadId();
                    int currentProcessThreadId = currentProcess.GetMainThreadId();
                    IntPtr processDesktop = NativeMethod.GetThreadDesktop(processThreadId);
                    IntPtr currentProcessDesktop = NativeMethod.GetThreadDesktop(currentProcessThreadId);
                    if (currentProcessDesktop == processDesktop)
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main() {
            if (ExistProcessWithSameNameAndDesktop(Process.GetCurrentProcess()))
                return;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
