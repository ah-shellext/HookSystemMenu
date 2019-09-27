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

        public const uint MF_BYCOMMAND = 0x0000;
    }
}
