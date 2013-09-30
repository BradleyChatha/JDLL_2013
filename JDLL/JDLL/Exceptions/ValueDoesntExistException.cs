using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JDLL.Data.Logging;

namespace JDLL.Exceptions
{
    public class ValueDoesntExistException : Exception
    {
        public ValueDoesntExistException()
        {

        }

        public ValueDoesntExistException(String Message) : base(Message)
        {

        }

        public void WriteToLog(ref Log Log, String Value, bool Save = false)
        {
            Log.Write("JDLL", "(ValueDoesntExistsException) Value doesn't exist and Caller wants to throw this error : Error - Value = " + "[" + Value + "]", new SEV_Severe(this), Save);
        }
    }
}
