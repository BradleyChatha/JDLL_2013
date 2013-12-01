using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Speech.Synthesis;
using System.IO;
using System.Runtime.InteropServices;
namespace JDLL
{
    public class Util
    {
        [DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

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

        public static void LeftClickAtPosition(int X, int Y)
        {
            mouse_event(0x02, X, Y, 0, 0);
        }

        public static void RightClickAtPosition(int X, int Y)
        {
            mouse_event(0x08, X, Y, 0, 0);
        }
    }
}
