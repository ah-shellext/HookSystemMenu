using System;

namespace SystemMenuImpl {

    class HookMessages {

        public static uint MSG_HSHELL_WINDOWCREATED = 0;
        public static uint MSG_HSHELL_WINDOWDESTROYED = 0;
        public static uint MSG_HSHELL_WINDOWACTIVATED = 0;
        public static uint MSG_HGETMESSAGE = 0;
        public static uint MSG_HGETMESSAGE_PARAMS = 0;

        public static void RegisterMessages() {
            MSG_HSHELL_WINDOWCREATED = NativeMethods.RegisterWindowMessage("AH_SYSTEM_MENU_HOOK_HSHELL_WINDOWCREATED");
            MSG_HSHELL_WINDOWDESTROYED = NativeMethods.RegisterWindowMessage("AH_SYSTEM_MENU_HOOK_HSHELL_WINDOWDESTROYED");
            MSG_HSHELL_WINDOWACTIVATED = NativeMethods.RegisterWindowMessage("AH_SYSTEM_MENU_HOOK_HSHELL_WINDOWACTIVATED");
            MSG_HGETMESSAGE = NativeMethods.RegisterWindowMessage("AH_SYSTEM_MENU_HOOK_GETMESSAGE");
            MSG_HGETMESSAGE_PARAMS = NativeMethods.RegisterWindowMessage("AH_SYSTEM_MENU_HOOK_GETMESSAGE_PARAMS");

            if (Environment.OSVersion.Version.Major >= 6) {
                NativeMethods.ChangeWindowMessageFilter(MSG_HSHELL_WINDOWCREATED, NativeConstants.MSGFLT_ADD);
                NativeMethods.ChangeWindowMessageFilter(MSG_HSHELL_WINDOWDESTROYED, NativeConstants.MSGFLT_ADD);
                NativeMethods.ChangeWindowMessageFilter(MSG_HSHELL_WINDOWACTIVATED, NativeConstants.MSGFLT_ADD);
                NativeMethods.ChangeWindowMessageFilter(MSG_HGETMESSAGE, NativeConstants.MSGFLT_ADD);
                NativeMethods.ChangeWindowMessageFilter(MSG_HGETMESSAGE_PARAMS, NativeConstants.MSGFLT_ADD);
            }
        }

        public static void UnregisterMessages() {
            // Do nothing
        }
