namespace SystemMenuImpl {

    class NativeConstants {

        public const uint MSGFLT_ADD = 0x1;

        public const uint WM_CLOSE = 0x0010;
        public const uint WM_SYSCOMMAND = 0x0112;

        public const uint SWP_NOOWNERZORDER = 0x0200;
        public const uint SWP_NOACTIVATE = 0x0010;
        public const uint SWP_NOSIZE = 0x0001;
        public const uint SWP_NOMOVE = 0x0002;

        public const uint GW_HWNDNEXT = 2;
        public const uint GW_OWNER = 4;
        public const uint GW_CHILD = 5;
        public const int GWL_STYLE = -16;
        public const int GWL_EXSTYLE = -20;

        public const uint WS_VISIBLE = 0x10000000;
        public const uint WS_EX_TOOLWINDOW = 0x00000080;
        public const uint WS_EX_TOPMOST = 0x00000008;

        public const uint MIIM_STATE = 0x0001;
        public const uint MIIM_ID = 0x0002;
        public const uint MIIM_STRING = 0x0040;
        public const uint MIIM_FTYPE = 0x0100;

        public const uint MFT_STRING = 0x0000;
        public const uint MFT_SEPARATOR = 0x0800;

        public const uint MF_BYCOMMAND = 0x0000;
        public const uint MFS_ENABLED = 0x0000;
        public const uint MFS_UNCHECKED = 0x0000;
        public const uint MFS_DISABLED = 0x0003;
        public const uint MFS_CHECKED = 0x0008;
    }
}
