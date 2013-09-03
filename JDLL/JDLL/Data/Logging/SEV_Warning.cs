using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JDLL.Data.Logging
{
    public class SEV_Warning : ISeverety
    {
        public SEV_Warning()
        {
            this.LogInfo = "[WARNING]";
        }
    }
}
