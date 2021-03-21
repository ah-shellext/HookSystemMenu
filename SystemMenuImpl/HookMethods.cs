﻿using System;
using System.Runtime.InteropServices;

namespace SystemMenuImpl {

    class HookMethods {

        [DllImport("SystemMenuHook.dll", EntryPoint = "InitializeShellHook")]
        public static extern bool InitializeShellHook(int threadId, IntPtr destination);

        [DllImport("SystemMenuHook.dll", EntryPoint = "UninitializeShellHook")]
        public static extern void UninitializeShellHook();

        [DllImport("SystemMenuHook.dll", EntryPoint = "InitializeCbtHook")]
        public static extern void InitializeCbtHook(int threadId, IntPtr destination);

        [DllImport("SystemMenuHook.dll", EntryPoint = "UninitializeCbtHook")]
        public static extern void UninitializeCbtHook();

        [DllImport("SystemMenuHook.dll", EntryPoint = "InitializeGetMessageHook")]
        public static extern bool InitializeGetMessageHook(int threadId, IntPtr destination);

        [DllImport("SystemMenuHook.dll", EntryPoint = "UninitializeGetMessageHook")]
        public static extern void UninitializeGetMessageHook();

        [DllImport("SystemMenuHook.dll", EntryPoint = "InitializeCallWndProcHook")]
        public static extern void InitializeCallWndProcHook(int threadId, IntPtr destination);

        [DllImport("SystemMenuHook.dll", EntryPoint = "UninitializeCallWndProcHook")]
        public static extern void UninitializeCallWndProcHook();

        public static void StartHook(IntPtr hwnd) {
            InitializeShellHook(0, hwnd);
            InitializeCbtHook(0, hwnd);
            InitializeGetMessageHook(0, hwnd);
            InitializeCallWndProcHook(0, hwnd);
        }

        public static void StopHook() {
            UninitializeShellHook();
            UninitializeCbtHook();
            UninitializeGetMessageHook();
            UninitializeCallWndProcHook();
        }
    }
}
