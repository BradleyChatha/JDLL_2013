using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security;

namespace JDLL.Util
{
    public class Variables
    {
        #region String
        public static String RandomString(int Length)
        {
            String S = "";
            Random Rand = new Random();

            char[] Letters = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

            for (int i = 0; i < Length; i++)
            {
                S += Letters[Rand.Next(Letters.Length - 1)];
                Console.WriteLine(i);
            }

            return S;
        }

        public static String CensorString(String S, String Censor)
        {
            String Censored = "";

            foreach (String s in S.Split(' '))
            {
                if (s.ToLower().Equals(Censor.ToLower()))
                    Censored += "****";
                else
                    Censored += s;

                Censored += " ";
            }

            Censored.Trim();
            return Censored;
        }
        
        public static String ReplaceTextInString(String S, String Replace, String NewString)
        {
            String Replaced = "";

            foreach (String s in S.Split(' '))
            {
                if (s.Equals(Replace))
                    Replaced += NewString;
                else
                    Replaced += s;

                Replaced += " ";
            }

            Replaced.Trim();
            return Replaced;
        }

        public static String BytetoString(byte[] bytes)
        {
            ASCIIEncoding Encoder = new ASCIIEncoding();

            return Encoder.GetString(bytes);
        }
        #endregion

        #region Byte
        public static byte[] StringtoByte(String S)
        {
            ASCIIEncoding Encoder = new ASCIIEncoding();

            return Encoder.GetBytes(S);
        }
        #endregion

        #region int
        public static int CharacterCount(String ToRead)
        {
            return ToRead.Length;
        }

        public static int CharacterCount(char Character, String ToRead)
        {
            int Count = 0;

            foreach (char c in ToRead)
                if (c.Equals(Character))
                    Count++;

            return Count;
        }

        public static int WordCount(String Sentance)
        {
            return Sentance.Split(' ').Length;
        }

        public static int WordCount(String Sentance, String Word)
        {
            int Count = 0;

            foreach (String s in Sentance.Split(' '))
                if (s.Equals(Word))
                    Count++;

            return Count;
        }
        #endregion
    }
}
