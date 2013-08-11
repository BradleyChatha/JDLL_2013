using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace JDLL.JDLLCPP
{
    public class SoupDLL
    {
        [DllImport("SoupDLL.dll")]
        public static extern int Add(int x, int y);

        [DllImport("SoupDLL.dll")]
        public static extern int Subtract(int x, int y);

        [DllImport("SoupDLL.dll")]
        public static extern int Multiply(int x, int y);

        [DllImport("SoupDLL.dll")]
        public static extern int Divide(int x, int y);

        [DllImport("SoupDLL.dll")]
        public static extern void ShowMessagebox(String Message, String Title);
    }
}
