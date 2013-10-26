using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Speech.Synthesis;
using System.IO;

namespace JDLL
{
    public class Util
    {
        public static int Volume = 100;

        public static void Speak(String Phrase)
        {
            SpeechSynthesizer Talk = new SpeechSynthesizer();

            Talk.Volume = Volume;
            Talk.Speak(Phrase);

            Talk.Dispose();
        }

        public static void DownloadFile(String url, String file)
        {
            using (WebClient wc = new WebClient())
            {
                wc.DownloadFile(url, file);
            }
        }
    }
}
