using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;

using JDLL.InternalSecurity;
using JDLL.Data;
using JDLL.Components;
using JDLL.Data.Logging;
using JDLL.Exceptions;
using JDLL;
using JDLL.SealScript;

// ^ The Namespaces of the DLL

namespace Testerr
{
    class Program
    {
        static void Main(string[] args)
        {
            Resource Resources = new Resource("Data");
            Resources.Organise();

            Console.ReadKey();
        }
    }
}
