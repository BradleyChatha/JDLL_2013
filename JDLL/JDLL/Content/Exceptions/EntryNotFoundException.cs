using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JDLL.Content
{
    public class EntryNotFoundException : Exception
    {
        public EntryNotFoundException(String entry) : base("Entry with the name '" + entry + "' does not exist")
        {
        }
    }
}
