using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

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
    }
}
