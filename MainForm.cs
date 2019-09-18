using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace test
{
    class MainForm : Form
    {

        // https://blog.csdn.net/leehong2005/article/details/8607578

        // protected override void WndProc(ref Message m) {
        //     switch (m.Msg) {
        //         case WM_TASKBARRCLICK:
        //             POINT pt;
        //             GetCursorPos(out pt);
        //             int hMenu = LoadMenu(this.Handle.ToInt32(), 
        //                     MAKEINTRESOURCE(IDR_CONTEXT_MENU));

        //         break;
        //     }
        // }

        // [DllImport("user32.dll")]
        // static extern bool EnableMenuItem(IntPtr hMenu, uint uIDEnableItem, uint uEnable);

        // internal const int MF_DISABLED = 0x00000002;

        // internal const int SC_MOVE = 0xF010;

        private const int WM_TASKBARRCLICK = 0x0313;
        private const int WM_NCRBUTTONDOWN = 0x00A4;
        private const int WM_INITMENUPOPUP = 0x0117;

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_INITMENUPOPUP:
                    MessageBox.Show("WM_INITMENUPOPUP");
                    // if ((((int)m.LParam & 0x10000) != 0))
                    //     EnableMenuItem(m.WParam, SC_MOVE, MF_DISABLED);
                    break;
                case WM_TASKBARRCLICK:
                case WM_NCRBUTTONDOWN:
                default:
                    base.WndProc(ref m);
                    break;
            }
        }
    }
}