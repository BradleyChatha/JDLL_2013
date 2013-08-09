using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JDLL.Exceptions
{
    public class MalformedEntryException : Exception
    {
        public MalformedEntryException()
        {
        }

        public MalformedEntryException(String message) : base(message)
        {
        }
    }
}
