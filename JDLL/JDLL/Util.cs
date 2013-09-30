using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Synthesis;

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
    }
}
