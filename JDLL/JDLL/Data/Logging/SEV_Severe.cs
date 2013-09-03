using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JDLL.Data.Logging
{
    public class SEV_Severe : ISeverety
    {
        public SEV_Severe(Exception Error)
        {
            this.LogInfo = "[SEVERE]";
            this.ThrowError = true;
            SetError(Error);
        }
    }
}
