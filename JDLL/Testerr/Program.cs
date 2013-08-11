using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

using JDLL.Data;
using JDLL.Util;
using JDLL.JDLLCPP;

namespace Testerr
{
    class Program
    {
        static void Main(string[] args)
        {
            int x = 2;
            int y = 6;

            Console.WriteLine(SoupDLL.Add(x, y));
            Console.WriteLine(SoupDLL.Subtract(x, y));
            Console.WriteLine(SoupDLL.Multiply(x, y));
            Console.WriteLine(SoupDLL.Divide(y, x));

            SoupDLL.ShowMessagebox("Message", "Box");

            Console.Read();
        }
    }
}
