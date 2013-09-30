using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JDLL.InternalSecurity
{
    abstract class AccessLevel
    {
        public String Name;
        int _AccessID = 0;

        protected virtual void WriteDebugInfo()
        {
            Console.WriteLine("AccessLevel with ID of " + _AccessID + " was called");
        }
    }
}
