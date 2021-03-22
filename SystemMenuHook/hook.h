#ifndef HOOK_H
#define HOOK_H

#include "pch.h"

#define DLLEXPORT extern "C" __declspec(dllexport)

DLLEXPORT bool __stdcall InitializeShellHook(int threadId, HWND destination);
DLLEXPORT void __stdcall UninitializeShellHook();
DLLEXPORT bool __stdcall InitializeCbtHook(int threadId, HWND destination);
DLLEXPORT void __stdcall UninitializeCbtHook();
DLLEXPORT bool __stdcall InitializeGetMessageHook(int threadId, HWND destination);
DLLEXPORT void __stdcall UninitializeGetMessageHook();
DLLEXPORT bool __stdcall InitializeCallWndProcHook(int threadId, HWND destination);
DLLEXPORT void __stdcall UninitializeCallWndProcHook();

const wchar_t *WcsShellReplaced = L"AH_SYSTEM_MENU_HOOK_SHELL_REPLACED";
const wchar_t *WcsHShellWindowCreated = L"AH_SYSTEM_MENU_HOOK_HSHELL_WINDOWCREATED";
const wchar_t *WcsHShellWindowDestroyed = L"AH_SYSTEM_MENU_HOOK_HSHELL_WINDOWDESTROYED";
const wchar_t *WcsHShellActivateShellWindow = L"AH_SYSTEM_MENU_HOOK_HSHELL_ACTIVATESHELLWINDOW";
const wchar_t *WcsHShellWindowActivated = L"AH_SYSTEM_MENU_HOOK_HSHELL_WINDOWACTIVATED";
const wchar_t *WcsHShellGetMinRect = L"AH_SYSTEM_MENU_HOOK_HSHELL_GETMINRECT";
const wchar_t *WcsHShellRedraw = L"AH_SYSTEM_MENU_HOOK_HSHELL_REDRAW";
const wchar_t *WcsHShellTaskman = L"AH_SYSTEM_MENU_HOOK_HSHELL_TASKMAN";
const wchar_t *WcsHShellLanguage = L"AH_SYSTEM_MENU_HOOK_HSHELL_LANGUAGE";
const wchar_t *WcsHShellAccessibilityState = L"AH_SYSTEM_MENU_HOOK_HSHELL_ACCESSIBILITYSTATE";
const wchar_t *WcsHShellAppCommand = L"AH_SYSTEM_MENU_HOOK_HSHELL_APPCOMMAND";
const wchar_t *WcsHShellWindowReplaced = L"AH_SYSTEM_MENU_HOOK_HSHELL_WINDOWREPLACED";

const wchar_t *WcsCbtReplaced = L"AH_SYSTEM_MENU_HOOK_CBT_REPLACED";
const wchar_t *WcsHCbtMoveSize = L"AH_SYSTEM_MENU_HOOK_HCBT_MOVESIZE";
const wchar_t *WcsHCbtMinMax = L"AH_SYSTEM_MENU_HOOK_HCBT_MINMAX";
const wchar_t *WcsHCbtQs = L"AH_SYSTEM_MENU_HOOK_HCBT_QS";
const wchar_t *WcsHCbtCreateWnd = L"AH_SYSTEM_MENU_HOOK_HCBT_CREATEWND";
const wchar_t *WcsHCbtDestroyWnd = L"AH_SYSTEM_MENU_HOOK_HCBT_DESTROYWND";
const wchar_t *WcsHCbtActivate = L"AH_SYSTEM_MENU_HOOK_HCBT_ACTIVATE";
const wchar_t *WcsHCbtClickSkipped = L"AH_SYSTEM_MENU_HOOK_HCBT_CLICKSKIPPED";
const wchar_t *WcsHCbtKeySkipped = L"AH_SYSTEM_MENU_HOOK_HCBT_KEYSKIPPED";
const wchar_t *WcsHCbtSysCommand = L"AH_SYSTEM_MENU_HOOK_HCBT_SYSCOMMAND";
const wchar_t *WcsHCbtSetFocus = L"AH_SYSTEM_MENU_HOOK_HCBT_SETFOCUS";

const wchar_t *WcsGetMessageReplaced = L"AH_SYSTEM_MENU_HOOK_GETMESSAGE_REPLACED";
const wchar_t *WcsGetMessage = L"AH_SYSTEM_MENU_HOOK_GETMESSAGE";
const wchar_t *WcsGetMessageParams = L"AH_SYSTEM_MENU_HOOK_GETMESSAGE_PARAMS";

const wchar_t *WcsCallWndProcReplaced = L"AH_SYSTEM_MENU_HOOK_CALLWNDPROC_REPLACED";
const wchar_t *WcsCallWndProc = L"AH_SYSTEM_MENU_HOOK_CALLWNDPROC";
const wchar_t *WcsCallWndProcParams = L"AH_SYSTEM_MENU_HOOK_CALLWNDPROC_PARAMS";

inline void ShowNullModuleInstanceMessage() {
    MessageBox(NULL, L"You do not register the dll yet!", L"HookSystemMenu", MB_OK | MB_ICONERROR);
}

#endif // HOOK_H
