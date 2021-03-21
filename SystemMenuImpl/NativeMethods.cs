using System.Runtime.InteropServices;

namespace SystemMenuImpl {

    class NativeMethods {

        [DllImport("user32.dll")]
        public static extern uint RegisterWindowMessage(string lpString);
    }
}
