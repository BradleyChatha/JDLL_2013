using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JDLL.Content
{
    public class ProcessorNotRegisteredException : Exception
    {
        public ProcessorNotRegisteredException(String message) : base(message)
        {

        }
    }
}
