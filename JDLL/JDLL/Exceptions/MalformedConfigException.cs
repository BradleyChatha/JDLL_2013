using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

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
    }
}
