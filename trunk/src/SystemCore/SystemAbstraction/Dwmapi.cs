using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace SystemCore.SystemAbstraction
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MARGINS
    {
        public int cxLeftWidth;
        public int cxRightWidth;
        public int cyTopHeight;
        public int cyBottomHeight;
    } 

    public static class Dwmapi
    {
        [System.Runtime.InteropServices.DllImport("dwmapi.dll")]
        public extern static int DwmIsCompositionEnabled(ref int en);

        [System.Runtime.InteropServices.DllImport("dwmapi.dll")]
        public extern static int DwmExtendFrameIntoClientArea(IntPtr hwnd,
                                 ref MARGINS margin);

    }
}
