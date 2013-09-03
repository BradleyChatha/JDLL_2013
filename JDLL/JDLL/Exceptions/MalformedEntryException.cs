using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using JDLL.Data.Logging;

namespace JDLL.Exceptions
{
    public class MalformedEntryException : Exception
    {
        public MalformedEntryException()
        {
        }

        public MalformedEntryException(String message, String Entry) : base(message)
        {
            File.WriteAllLines("Error.txt", new String[]{ message, "Malformed Entry = " + Entry });
        }

        public void WriteToLog(ref Log Log, String Malformity, String Entry, bool Save = false)
        {
            Log.Write("JDLL", Malformity + " : Error - Entry = " + Entry, new SEV_Severe(this), Save);
        }
    }
}
