using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace JDLL.SealScript
{
    class Win32
    {
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern int MessageBox(IntPtr hWnd, String text, String caption, uint type);
        //Win32.MessageBox(new IntPtr(0), "Box Text", "Box Title", 0);
    }
}
