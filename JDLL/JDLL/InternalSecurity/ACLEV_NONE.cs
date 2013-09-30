using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JDLL.InternalSecurity
{
    class ACLEV_NONE : AccessLevel
    {
        public ACLEV_NONE()
        {
            Name = "None";
        }

        protected override void WriteDebugInfo()
        {
            Console.WriteLine("Standard Developer Method Called");
        }
    }
}
