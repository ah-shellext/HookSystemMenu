using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SystemMenuShell {

    static class NativeConstant {

        // Mask
        public const uint MIIM_STATE = 0x00000001;
        public const uint MIIM_ID = 0x00000002;
        public const uint MIIM_SUBMENU = 0x00000004;
        public const uint MIIM_CHECKMARKS = 0x00000008;
        public const uint MIIM_STRING = 0x00000040;
        public const uint MIIM_FTYPE = 0x00000100;

        // Type
        public const uint MFT_STRING = 0x00000000;
        public const uint MFT_RADIOCHECK = 0x00000200;
        public const uint MFT_SEPARATOR = 0x00000800;

        // State
        public const uint MFS_ENABLED = 0x00000000;
        public const uint MFS_UNCHECKED = 0x00000000;
        public const uint MFS_UNHILITE = 0x00000000;
        public const uint MFS_GLAYED = 0x00000003;
        public const uint MFS_DISABLED = 0x00000003;
        public const uint MFS_CHECKED = 0x00000008;
        public const uint MFS_HILITE = 0x00000080;
        public const uint MFS_DEFAULT = 0x00001000;

        // Message
        public const uint WM_SYSCOMMAND = 0x0112;
        public const uint MF_BYCOMMAND = 0x0;
    }
}
