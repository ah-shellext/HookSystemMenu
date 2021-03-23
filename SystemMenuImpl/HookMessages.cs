using System;
using System.Windows.Forms;

namespace SystemMenuImpl {

    class HookMessages {

        public static uint MSG_HSHELL_WINDOWCREATED = 0;
        public static uint MSG_HSHELL_WINDOWDESTROYED = 0;
        public static uint MSG_HSHELL_WINDOWACTIVATED = 0;

        public static uint MSG_HCBT_CREATEWND = 0;
        public static uint MSG_HCBT_DESTROYWND = 0;
        public static uint MSG_HCBT_ACTIVATE = 0;

        public static void RegisterMessages() {
            MSG_HSHELL_WINDOWCREATED = NativeMethods.RegisterWindowMessage("AH_SYSTEM_MENU_HOOK_HSHELL_WINDOWCREATED");
            MSG_HSHELL_WINDOWDESTROYED = NativeMethods.RegisterWindowMessage("AH_SYSTEM_MENU_HOOK_HSHELL_WINDOWDESTROYED");
            MSG_HSHELL_WINDOWACTIVATED = NativeMethods.RegisterWindowMessage("AH_SYSTEM_MENU_HOOK_HSHELL_WINDOWACTIVATED");

            MSG_HCBT_CREATEWND = NativeMethods.RegisterWindowMessage("AH_SYSTEM_MENU_HOOK_HCBT_CREATEWND");
            MSG_HCBT_DESTROYWND = NativeMethods.RegisterWindowMessage("AH_SYSTEM_MENU_HOOK_HCBT_DESTROYWND");
            MSG_HCBT_ACTIVATE = NativeMethods.RegisterWindowMessage("AH_SYSTEM_MENU_HOOK_HCBT_ACTIVATE");

            if (Environment.OSVersion.Version.Major >= 6) {
                NativeMethods.ChangeWindowMessageFilter(MSG_HSHELL_WINDOWCREATED, NativeConstants.MSGFLT_ADD);
                NativeMethods.ChangeWindowMessageFilter(MSG_HSHELL_WINDOWDESTROYED, NativeConstants.MSGFLT_ADD);
                NativeMethods.ChangeWindowMessageFilter(MSG_HSHELL_WINDOWACTIVATED, NativeConstants.MSGFLT_ADD);

                NativeMethods.ChangeWindowMessageFilter(MSG_HCBT_CREATEWND, NativeConstants.MSGFLT_ADD);
                NativeMethods.ChangeWindowMessageFilter(MSG_HCBT_DESTROYWND, NativeConstants.MSGFLT_ADD);
                NativeMethods.ChangeWindowMessageFilter(MSG_HCBT_ACTIVATE, NativeConstants.MSGFLT_ADD);
            }
        }

        public static void UnregisterMessages() {
            // Do nothing
        }
    }
}
