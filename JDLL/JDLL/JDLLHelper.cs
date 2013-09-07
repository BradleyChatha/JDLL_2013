using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace JDLL
{
    public class JDLLHelper
    {
        public const String Version = "1";

        public bool CheckForUpdate()
        {
            using (WebClient wc = new WebClient())
            {
                wc.DownloadFile("https://dl.dropbox.com/s/4nm5dmtuukiwc60/Version.txt", "Version.txt");
            }

            if (File.ReadAllLines("Version.txt")[0].Equals(Version))
            {
                File.Delete("Version.txt");
                return false;
            }
            else
            {
                File.Delete("Version.txt");
                return true;
            }
        }
    }
}
