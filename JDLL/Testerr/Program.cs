using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Threading;
using System.Drawing.Imaging;
using System.Drawing;

using JDLL;
using JDLL.Components;
using JDLL.Data;
using JDLL.Data.Logging;
using JDLL.Content;
using JDLL.Exceptions;
using JDLL.SealScript;
using JDLL.Experiments;
using JDLL.Windows;
using JDLL.JMaths;

// ^ The Namespaces of the DLL

namespace Testerr
{
    class Program
    {
        static void Main(string[] args)
        {
            Content_Manager manager = new Content_Manager("Data.dat");

            Console.ReadKey();
        }
    }
}
