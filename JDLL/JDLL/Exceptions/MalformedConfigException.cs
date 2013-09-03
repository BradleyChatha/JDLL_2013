using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using JDLL.Data.Logging;

namespace JDLL.Exceptions
{
    public class MalformedConfigException : Exception
    {
        public MalformedConfigException()
        {
        }

        public MalformedConfigException(String message) : base(message)
        {
        }

        public void WriteToLog(ref Log Log, String FilePath, bool Save = false)
        {
            Log.Write("JDLL", "File is missing NAME; Entry : Error - File = " + FilePath, new SEV_Severe(this), Save);
        }
    }
}
