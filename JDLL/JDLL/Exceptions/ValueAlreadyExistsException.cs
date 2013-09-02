using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
