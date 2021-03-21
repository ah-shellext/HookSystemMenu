#ifndef HOOK_H
#define HOOK_H

#include "pch.h"

#define DLLEXPORT extern "C" __declspec(dllexport)

typedef void (CALLBACK *HookProc)(int code, WPARAM w, LPARAM l);

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

const wchar_t *WcsGetMessageReplaced = L"AH_SYSTEM_MENU_HOOK_GETMESSAGE_REPLACED";

const wchar_t *WcsCallWndProcReplaced = L"AH_SYSTEM_MENU_HOOK_CALLWNDPROC_REPLACED";

#endif // HOOK_H
