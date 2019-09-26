using System;
using System.Windows.Forms;
using EasyHook;

namespace SystemMenuShell
{
    static class Program
    {
        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
            
            // 遍历所有 wnd
            // 在目前所有 wnd insertmenu
            // GetMsgHook 监听 wndproc
            // ShellHook 监听 窗口创建
        }
    }
}
