#include "pch.h"
#include "hook.h"

#pragma data_seg(".Shared")
HWND hwndDestnation = NULL;
HHOOK hookShell = NULL;
HHOOK hookCbt = NULL;
HHOOK hookGetMessage = NULL;
HHOOK hookCallWndProc = NULL;
#pragma data_seg()
#pragma comment(linker, "/section:.Shared,rws")

HINSTANCE hModuleInstance = NULL;

static LRESULT __stdcall ShellHookCallback(int, WPARAM, LPARAM);
static LRESULT __stdcall CbtHookCallback(int, WPARAM, LPARAM);
static LRESULT __stdcall GetMessageHookCallback(int, WPARAM, LPARAM);
static LRESULT __stdcall CallWndProcHookCallback(int, WPARAM, LPARAM);

// WH_SHELL

DLLEXPORT bool __stdcall InitializeShellHook(int threadId, HWND destination) {
    if (hModuleInstance == NULL) {
        return false;
    }

    if (hwndDestnation != NULL) {
        UINT msg = RegisterWindowMessage(WcsShellReplaced);
        if (msg != 0) {
            SendNotifyMessage(hwndDestnation, msg, 0, 0);
        }
    }

    hwndDestnation = destination;
    hookShell = SetWindowsHookEx(WH_SHELL, (HOOKPROC) ShellHookCallback, hModuleInstance, threadId);
    return hookShell != NULL;
}

DLLEXPORT void __stdcall UninitializeShellHook() {
    if (hookShell != NULL) {
        UnhookWindowsHookEx(hookShell);
    }

    hookShell = NULL;
}

static LRESULT __stdcall ShellHookCallback(int code, WPARAM wparam, LPARAM lparam) {
    if (hwndDestnation != NULL && code >= 0) {
        LPCWSTR wcs;
        switch (code) {
        case HSHELL_WINDOWCREATED:
            wcs = WcsHShellWindowCreated;
            break;
        case HSHELL_WINDOWDESTROYED:
            wcs = WcsHShellWindowDestroyed;
            break;
        case HSHELL_ACTIVATESHELLWINDOW:
            wcs = WcsHShellActivateShellWindow;
            break;
        case HSHELL_WINDOWACTIVATED:
            wcs = WcsHShellWindowActivated;
            break;
        case HSHELL_GETMINRECT:
            wcs = WcsHShellGetMinRect;
            break;
        case HSHELL_REDRAW:
            wcs = WcsHShellRedraw;
            break;
        case HSHELL_TASKMAN:
            wcs = WcsHShellTaskman;
            break;
        case HSHELL_LANGUAGE:
            wcs = WcsHShellLanguage;
            break;
        case HSHELL_ACCESSIBILITYSTATE:
            wcs = WcsHShellAccessibilityState;
            break;
        case HSHELL_APPCOMMAND:
            wcs = WcsHShellAppCommand;
            break;
        case HSHELL_WINDOWREPLACED:
            wcs = WcsHShellWindowReplaced;
            break;
        default:
            wcs = NULL;
        }

        if (wcs != NULL) {
            UINT msg = RegisterWindowMessage(wcs);
            if (msg != 0) {
                SendNotifyMessage(hwndDestnation, msg, wparam, lparam);
            }
        }
    }

    return CallNextHookEx(hookShell, code, wparam, lparam);
}

// WH_CBT

DLLEXPORT bool __stdcall InitializeCbtHook(int threadId, HWND destination) {
    if (hModuleInstance == NULL) {
        return false;
    }

    if (hwndDestnation != NULL) {
        UINT msg = RegisterWindowMessage(WcsCbtReplaced);
        if (msg != 0) {
            SendNotifyMessage(hwndDestnation, msg, 0, 0);
        }
    }

    hwndDestnation = destination;
    hookCbt = SetWindowsHookEx(WH_CBT, (HOOKPROC) CbtHookCallback, hModuleInstance, threadId);
    return hookCbt != NULL;
}

DLLEXPORT void __stdcall UninitializeCbtHook() {
    if (hookCbt != NULL) {
        UnhookWindowsHookEx(hookCbt);
    }

    hookCbt = NULL;
}

static LRESULT __stdcall CbtHookCallback(int code, WPARAM wparam, LPARAM lparam) {
    if (hwndDestnation != NULL && code >= 0) {
        LPCWSTR wcs;
        switch (code) {
        case HCBT_MOVESIZE:
            wcs = WcsHCbtMoveSize;
            break;
        case HCBT_MINMAX:
            wcs = WcsHCbtMinMax;
            break;
        case HCBT_QS:
            wcs = WcsHCbtQs;
            break;
        case HCBT_CREATEWND:
            wcs = WcsHCbtCreateWnd;
            break;
        case HCBT_DESTROYWND:
            wcs = WcsHCbtDestroyWnd;
            break;
        case HCBT_ACTIVATE:
            wcs = WcsHCbtActivate;
            break;
        case HCBT_CLICKSKIPPED:
            wcs = WcsHCbtClickSkipped;
            break;
        case HCBT_KEYSKIPPED:
            wcs = WcsHCbtKeySkipped;
            break;
        case HCBT_SYSCOMMAND:
            wcs = WcsHCbtSysCommand;
            break;
        case HCBT_SETFOCUS:
            wcs = WcsHCbtSetFocus;
            break;
        default:
            wcs = NULL;
        }

        if (wcs != NULL) {
            UINT msg = RegisterWindowMessage(wcs);
            if (msg != 0) {
                SendNotifyMessage(hwndDestnation, msg, wparam, lparam);
            }
        }
    }

    return CallNextHookEx(hookCbt, code, wparam, lparam);
}

// WH_GETMESSAGE

DLLEXPORT bool __stdcall InitializeGetMessageHook(int threadId, HWND destination) {
    if (hModuleInstance == NULL) {
        return false;
    }

    if (hwndDestnation != NULL) {
        UINT msg = RegisterWindowMessage(WcsGetMessageReplaced);
        if (msg != 0) {
            SendNotifyMessage(hwndDestnation, msg, 0, 0);
        }
    }

    hwndDestnation = destination;
    hookGetMessage = SetWindowsHookEx(WH_GETMESSAGE, (HOOKPROC) GetMessageHookCallback, hModuleInstance, threadId);
    return hookGetMessage != NULL;
}

DLLEXPORT void __stdcall UninitializeGetMessageHook() {
    if (hookGetMessage != NULL) {
        UnhookWindowsHookEx(hookGetMessage);
    }

    hookGetMessage = NULL;
}

static LRESULT __stdcall GetMessageHookCallback(int code, WPARAM wparam, LPARAM lparam) {
    if (hwndDestnation != NULL && code >= 0) {
        UINT msg = RegisterWindowMessage(WcsGetMessage);
        UINT msgParams = RegisterWindowMessage(WcsGetMessageParams);

        if (msg != 0 && msgParams != 0) {
            MSG *pMsg = (MSG *) lparam;
            if (pMsg->message != msg && pMsg->message != msgParams) {
                if (wparam == PM_REMOVE && pMsg->message == WM_SYSCOMMAND) {
                    SendNotifyMessage(hwndDestnation, msg, (WPARAM) pMsg->hwnd, pMsg->message);
                    SendNotifyMessage(hwndDestnation, msgParams, pMsg->wParam, pMsg->lParam);
                }
            }
        }
    }

    return CallNextHookEx(hookGetMessage, code, wparam, lparam);
}

// WH_CALLWNDPROC

DLLEXPORT bool __stdcall InitializeCallWndProcHook(int threadId, HWND destination) {
    if (hModuleInstance == NULL) {
        return false;
    }

    if (hwndDestnation != NULL) {
        UINT msg = RegisterWindowMessage(WcsCallWndProcReplaced);
        if (msg != 0) {
            SendNotifyMessage(hwndDestnation, msg, 0, 0);
        }
    }

    hwndDestnation = destination;
    hookCallWndProc = SetWindowsHookEx(WH_CALLWNDPROC, (HOOKPROC) CallWndProcHookCallback, hModuleInstance, threadId);
    return hookCallWndProc != NULL;
}

DLLEXPORT void __stdcall UninitializeCallWndProcHook() {
    if (hookCallWndProc != NULL) {
        UnhookWindowsHookEx(hookCallWndProc);
    }

    hookCallWndProc = NULL;
}

static LRESULT __stdcall CallWndProcHookCallback(int code, WPARAM wparam, LPARAM lparam) {
    if (hwndDestnation != NULL && code >= 0) {
        UINT msg = RegisterWindowMessage(WcsCallWndProc);
        UINT msgParams = RegisterWindowMessage(WcsCallWndProcParams);

        if (msg != 0 && msgParams != 0) {
            CWPSTRUCT *pCwp = (CWPSTRUCT *) lparam;
            if (pCwp->message != msg && pCwp->message != msgParams) {
                if (wparam == PM_REMOVE) {
                    SendNotifyMessage(hwndDestnation, msg, (WPARAM) pCwp->hwnd, pCwp->message);
                    SendNotifyMessage(hwndDestnation, msgParams, pCwp->wParam, pCwp->lParam);
                }
            }
        }
    }

    return CallNextHookEx(hookCallWndProc, code, wparam, lparam);
}
