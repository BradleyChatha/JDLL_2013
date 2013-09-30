using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using JDLL;

namespace JDLL.InternalSecurity
{
    public class Security
    {
        public bool AlreadyVerified = false;

        internal bool _Allowed = false;
        internal Dictionary<String, String> _Exes = new Dictionary<String, String>();

        public static String None = new ACLEV_NONE().Name;
        public static String Priviledged = new ACLEV_PRIV().Name;
        public static String VIP = new ACLEV_VIP().Name;

        private AccessLevel Current = new ACLEV_NONE();

        public Security()
        {
            _Exes.Add("Testerr.exe", "dcc97b1d-7e2c-411c-99ef-54949d5b0295");
            _Exes.Add("Testerr.vshost.exe", "dcc97b1d-7e2c-411c-99ef-54949d5b0295");
        }

        public String GetLevel()
        {
            return Current.Name;
        }

        public bool Verify(String Pass1, String[] ExeCheck = null)
        {
            if (this.AlreadyVerified)
            {
                return true;
            }

            if (Pass1.Equals(Access.Default.Pass1))
            {
                this.AlreadyVerified = true;
                this.Current = new ACLEV_PRIV();

                if (ExeCheck != null && ExeCheck[0].Equals(Access.Default.Pass2) && this._Exes[ExeCheck[1]] == ExeCheck[2])
                {
                    this.Current = new ACLEV_VIP();
                }

                return true;
            }

            return false;
        }

        internal bool hasPermission(String MinPerm)
        {
            if (MinPerm.Equals(Security.None))
                return true;

            if (Current.Name == MinPerm)
                return true;

            if (MinPerm == Security.Priviledged && Current.Name == Security.VIP)
                return true;

            return false;
        }

        internal void OverrideVerify()
        {
            this.Current = new ACLEV_VIP();
        }
    }
}
