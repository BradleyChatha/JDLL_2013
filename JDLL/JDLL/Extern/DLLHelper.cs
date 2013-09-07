using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace JDLL.Extern
{
    public class DLLHelper
    {
        Dictionary<String, String> URLS = new Dictionary<String, String>();

        Dictionary<String, bool> Usuable = new Dictionary<String, bool>();

        public DLLHelper()
        {
            Usuable["SoupDLL"] = false;
        }

        public void DownloadDLL(String Name)
        {
            using (WebClient wc = new WebClient())
            {
                wc.DownloadFile(URLS[Name], Name + ".dll");
            }
        }
    }
}
