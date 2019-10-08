using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SystemMenuShell {

    static class NativeConstant {

        // Mask
        public const uint MIIM_STATE = 0x0001;
        public const uint MIIM_ID = 0x0002;
        public const uint MIIM_SUBMENU = 0x0004;
        public const uint MIIM_CHECKMARKS = 0x0008;
        public const uint MIIM_STRING = 0x0040;
        public const uint MIIM_FTYPE = 0x0100;

        // Type
        public const uint MFT_STRING = 0x0000;
        public const uint MFT_RADIOCHECK = 0x0200;
        public const uint MFT_SEPARATOR = 0x0800;

        // State
        public const uint MFS_ENABLED = 0x0000;
        public const uint MFS_UNCHECKED = 0x0000;
        public const uint MFS_UNHILITE = 0x0000;
        public const uint MFS_GLAYED = 0x0003;
        public const uint MFS_DISABLED = 0x0003;
        public const uint MFS_CHECKED = 0x0008;
        public const uint MFS_HILITE = 0x0080;
        public const uint MFS_DEFAULT = 0x1000;

        // Message
        public const uint WM_SYSCOMMAND = 0x0112;
        public const uint WM_CREATE = 0x0001;
        public const uint WM_DESTROY = 0x0002;
        public const uint WM_SHOWWINDOW = 0x0018;

        public const uint MF_BYCOMMAND = 0x0000;

        // UCMD

        public const uint GW_HWNDFIRST = 0;
        public const uint GW_HWNDLAST = 1;
        public const uint GW_HWNDNEXT = 2;
        public const uint GW_HWNDPREV = 3;
        public const uint GW_OWNER = 4;
        public const uint GW_CHILD = 5;
        public const uint GW_ENABLEDPOPUP = 6;

        // Window Long Param

        public const int GWL_WNDPROC = -4;
        public const int GWL_HINSTANCE = -6;
        public const int GWL_HWNDPARENT = -8;
        public const int GWL_STYLE = -16;
        public const int GWL_EXSTYLE = -20;
        public const int GWL_USERDATA = -21;
        public const int GWL_ID = -12;

        // Window styles

        public const uint WS_VISIBLE = 0x10000000;
        public const uint WS_EX_TOOLWINDOW = 0x00000080;
        public const uint WS_EX_TOPMOST = 0x00000008;

        // SetWindowPos Flags

        public const uint SWP_NOSIZE = 0x0001;
        public const uint SWP_NOMOVE = 0x0002;
        public const uint TOPMOST_FLAGS = SWP_NOMOVE | SWP_NOSIZE;

    }
}
