using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
