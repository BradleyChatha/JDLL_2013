using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

using JDLL.Data.Structures;
using JDLL.Data;
using JDLL.Components;

namespace Testerr
{
    class Program
    {
        static void Main(string[] args)
        {
            Config Config = new Config("Config.txt");
            Config.ClearFile();

            Config.WriteValue("String", "String");
            Config.WriteValue("Int", 20);
            Config.WriteValue("IntArray", new int[] { 10, 20, 30 });
            Config.WriteValue("Bool", true);

            Console.WriteLine("{0} {1}", "String", (String)Config.Parse("String"));
            Console.WriteLine("{0} {1}", "Int", (int)Config.Parse("Int"));
            Console.WriteLine("{0} {1}", "Bool", (bool)Config.Parse("Bool"));

            List<String> List1 = Config.Parse("IntArray") as List<String>;

            foreach (String s in List1)
                Console.WriteLine("{0} {1}", "IntArray", s);

            Console.ReadKey();
        }
    }
}
