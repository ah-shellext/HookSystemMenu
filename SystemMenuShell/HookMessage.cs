using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SystemMenuShell {

    static class HookMessage {

        // Replace

        public static uint MSG_HOOKSHELL_REPLACE = 0;
        public static uint MSG_HOOKCBT_REPLACE = 0;
        public static uint MSG_HOOKGETMSG_REPLACE = 0;
        public static uint MSG_HOOKCALLWNDPROC_REPLACE = 0;

        // Shell

        public static uint MSG_HSHELL_ACTIVATESHELLWINDOW = 0;
        public static uint MSG_HSHELL_GETMINRECT = 0;
        public static uint MSG_HSHELL_LANGUAGE = 0;
        public static uint MSG_HSHELL_REDRAW = 0;
        public static uint MSG_HSHELL_TASKMAN = 0;
        public static uint MSG_HSHELL_WINDOWACTIVATED = 0;
        public static uint MSG_HSHELL_WINDOWCREATED = 0;
        public static uint MSG_HSHELL_WINDOWDESTROYED = 0;

        // Cbt

        public static uint MSG_HCBT_ACTIVATE = 0;
        public static uint MSG_HCBT_CREATEWND = 0;
        public static uint MSG_HCBT_DESTROYWND = 0;
        public static uint MSG_HCBT_MINMAX = 0;
        public static uint MSG_HCBT_MOVESIZE = 0;
        public static uint MSG_HCBT_SETFOCUS = 0;
        public static uint MSG_HCBT_SYSCOMMAND = 0;

        // GetMsg

        public static uint MSG_HGETMSG_GETMSG = 0;
        public static uint MSG_HGETMSG_GETMSG_PARAMS = 0;

        // CallWndProc

        public static uint MSG_HCALLWNDPROC_CALLWNDPROC = 0;
        public static uint MSG_HCALLWNDPROC_CALLWNDPROC_PARAMS = 0;


        public static void RegisterMsg() {
            // Replace
            MSG_HOOKSHELL_REPLACE = NativeMethod.RegisterWindowMessage("AH_SYSTEM_MENU_HOOK_SHELL_REPLACE");
            MSG_HOOKCBT_REPLACE = NativeMethod.RegisterWindowMessage("AH_SYSTEM_MENU_HOOK_CBT_REPLACE");
            MSG_HOOKGETMSG_REPLACE = NativeMethod.RegisterWindowMessage("AH_SYSTEM_MENU_HOOK_GETMSG_REPLACE");
            MSG_HOOKCALLWNDPROC_REPLACE = NativeMethod.RegisterWindowMessage("AH_SYSTEM_MENU_HOOK_CALLWNDPROC_REPLACE");
            
            // Shell
            MSG_HSHELL_ACTIVATESHELLWINDOW = NativeMethod.RegisterWindowMessage("AH_SYSTEM_MENU_HOOK_HSHELL_ACTIVATESHELLWINDOW");
            MSG_HSHELL_GETMINRECT = NativeMethod.RegisterWindowMessage("AH_SYSTEM_MENU_HOOK_HSHELL_GETMINRECT");
            MSG_HSHELL_LANGUAGE = NativeMethod.RegisterWindowMessage("AH_SYSTEM_MENU_HOOK_HSHELL_LANGUAGE");
            MSG_HSHELL_REDRAW = NativeMethod.RegisterWindowMessage("AH_SYSTEM_MENU_HOOK_HSHELL_REDRAW");
            MSG_HSHELL_TASKMAN = NativeMethod.RegisterWindowMessage("AH_SYSTEM_MENU_HOOK_HSHELL_TASKMAN");
            MSG_HSHELL_WINDOWACTIVATED = NativeMethod.RegisterWindowMessage("AH_SYSTEM_MENU_HOOK_HSHELL_WINDOWACTIVATED");
            MSG_HSHELL_WINDOWCREATED = NativeMethod.RegisterWindowMessage("AH_SYSTEM_MENU_HOOK_HSHELL_WINDOWCREATED");
            MSG_HSHELL_WINDOWDESTROYED = NativeMethod.RegisterWindowMessage("AH_SYSTEM_MENU_HOOK_HSHELL_WINDOWDESTROYED");

            // Cbt
            MSG_HCBT_ACTIVATE = NativeMethod.RegisterWindowMessage("AH_SYSTEM_MENU_HOOK_HCBT_ACTIVATE");
            MSG_HCBT_CREATEWND = NativeMethod.RegisterWindowMessage("AH_SYSTEM_MENU_HOOK_HCBT_CREATEWND");
            MSG_HCBT_DESTROYWND = NativeMethod.RegisterWindowMessage("AH_SYSTEM_MENU_HOOK_HCBT_DESTROYWND");
            MSG_HCBT_MINMAX = NativeMethod.RegisterWindowMessage("AH_SYSTEM_MENU_HOOK_HCBT_MINMAX");
            MSG_HCBT_MOVESIZE = NativeMethod.RegisterWindowMessage("AH_SYSTEM_MENU_HOOK_HCBT_MOVESIZE");
            MSG_HCBT_SETFOCUS = NativeMethod.RegisterWindowMessage("AH_SYSTEM_MENU_HOOK_HCBT_SETFOCUS");
            MSG_HCBT_SYSCOMMAND = NativeMethod.RegisterWindowMessage("AH_SYSTEM_MENU_HOOK_HCBT_SYSCOMMAND");

            // GetMsg
            MSG_HGETMSG_GETMSG = NativeMethod.RegisterWindowMessage("AH_SYSTEM_MENU_HOOK_GETMSG");
            MSG_HGETMSG_GETMSG_PARAMS = NativeMethod.RegisterWindowMessage("AH_SYSTEM_MENU_HOOK_GETMSG_PARAMS");

            // CallWndProc
            MSG_HCALLWNDPROC_CALLWNDPROC = NativeMethod.RegisterWindowMessage("AH_SYSTEM_MENU_HOOK_CALLWNDPROC");
            MSG_HCALLWNDPROC_CALLWNDPROC_PARAMS = NativeMethod.RegisterWindowMessage("AH_SYSTEM_MENU_HOOK_CALLWNDPROC_PARAMS");
        }
    }
}
