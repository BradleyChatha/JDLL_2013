using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JDLL.InternalSecurity
{
    public abstract class IJClass
    {
        protected static String AccessLevel;
        protected static Exception InvalidAccess = null;

        protected static bool hasAccess = false;

        public static bool Verify(Security Sec)
        {
            if (AccessLevel == null)
                AccessLevel = Security.VIP;

            hasAccess = Sec.hasPermission(AccessLevel);
            return hasAccess;
        }
    }
}
