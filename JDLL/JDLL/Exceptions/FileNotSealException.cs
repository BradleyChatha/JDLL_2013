using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using JDLL.Data;

namespace JDLL.Exceptions
{
    public class FileNotSealException : Exception
    {
        public FileNotSealException()
        {
        }

        public FileNotSealException(String message) : base(message)
        {
        }

        public void WriteToLog(ref Log Log, String FilePath, bool Save = false)
        {
            Log.Write("JDLL", "File is not part of the data structure : Error - File = " + FilePath, Save);
        }
    }
}
