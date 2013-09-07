using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JDLL.Data.Logging
{
    public abstract class ISeverety
    {
        public String LogInfo = "[Default]";
        public bool ThrowError = false;
        public Exception Error { get; private set; }

        public void SetError(Exception ex)
        {
            Error = ex;
        }
    }
}
