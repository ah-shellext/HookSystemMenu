using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SystemMenuShell {

    static class HookMessage {

        // Replace

        public static uint MSG_HOOKSHELL_REPLACE = NativeMethod.RegisterWindowMessage("AH_SYSTEM_MENU_HOOK_SHELL_REPLACE");
        public static uint MSG_HOOKGETMSG_REPLACE = NativeMethod.RegisterWindowMessage("AH_SYSTEM_MENU_HOOK_GETMSG_REPLACE");

        // Shell

        public static uint MSG_HSHELL_ACTIVATESHELLWINDOW = NativeMethod.RegisterWindowMessage("AH_SYSTEM_MENU_HOOK_HSHELL_ACTIVATESHELLWINDOW");
        public static uint MSG_HSHELL_GETMINRECT = NativeMethod.RegisterWindowMessage("AH_SYSTEM_MENU_HOOK_HSHELL_GETMINRECT");
        public static uint MSG_HSHELL_LANGUAGE = NativeMethod.RegisterWindowMessage("AH_SYSTEM_MENU_HOOK_HSHELL_LANGUAGE");
        public static uint MSG_HSHELL_REDRAW = NativeMethod.RegisterWindowMessage("AH_SYSTEM_MENU_HOOK_HSHELL_REDRAW");
        public static uint MSG_HSHELL_TASKMAN = NativeMethod.RegisterWindowMessage("AH_SYSTEM_MENU_HOOK_HSHELL_TASKMAN");
        public static uint MSG_HSHELL_WINDOWACTIVATED = NativeMethod.RegisterWindowMessage("AH_SYSTEM_MENU_HOOK_HSHELL_WINDOWACTIVATED");
        public static uint MSG_HSHELL_WINDOWCREATED = NativeMethod.RegisterWindowMessage("AH_SYSTEM_MENU_HOOK_HSHELL_WINDOWCREATED");
        public static uint MSG_HSHELL_WINDOWDESTROYED = NativeMethod.RegisterWindowMessage("AH_SYSTEM_MENU_HOOK_HSHELL_WINDOWDESTROYED");

        // GetMsg

        public static uint MSG_HOOK_GETMSG = NativeMethod.RegisterWindowMessage("AH_SYSTEM_MENU_HOOK_GETMSG");
        public static uint MSG_HOOK_GETMSG_PARAMS = NativeMethod.RegisterWindowMessage("AH_SYSTEM_MENU_HOOK_GETMSG_PARAMS");

    }
}
