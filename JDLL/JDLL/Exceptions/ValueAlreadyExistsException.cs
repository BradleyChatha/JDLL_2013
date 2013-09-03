using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using JDLL.Data.Logging;

namespace JDLL.Exceptions
{
    public class ValueAlreadyExistsException : Exception
    {
        public ValueAlreadyExistsException()
        {
        }

        public ValueAlreadyExistsException(String message) : base(message)
        {
        }

        public void WriteToLog(ref Log Log, String Value, bool Save = false)
        {
            Log.Write("JDLL", "(ValueAlreadyExistsException) Value already exists : Error - Value = " + "[" + Value + "]", new SEV_Severe(this), Save);
        }
    }
}
