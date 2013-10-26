using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security;
using System.Security.Cryptography;
using System.IO;

namespace JDLL.Data
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
            char[] Chars = new char[bytes.Length / sizeof(char)];

            System.Buffer.BlockCopy(bytes, 0, Chars, 0, bytes.Length);

            return new String(Chars);
        }

        public static String StringtoBinaryString(String Input)
        {
            String toReturn = "";

            foreach (Char c in Input)
            {
                toReturn += Convert.ToString(c, 2).PadLeft(8, '0');
            }

            return toReturn;
        }

        public static String ToBase64String(String Input)
        {
            return Convert.ToBase64String(StringtoByte(Input));
        }

        public static String Base64ToString(String Input)
        {
            return BytetoString(Convert.FromBase64String(Input));
        }
        #endregion

        #region Byte
        public static byte[] StringtoByte(String S)
        {
            byte[] Bytes = new byte[S.Length * sizeof(char)];

            System.Buffer.BlockCopy(S.ToCharArray(), 0, Bytes, 0, Bytes.Length);

            return Bytes;
        }

        public static byte[] MD5Hash(String ToHash)
        {
            UTF8Encoding utf8 = new UTF8Encoding();
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            return md5.ComputeHash(utf8.GetBytes(ToHash));
        }

        public static byte[] MD5Hash(byte[] ToHash)
        {
            MD5CryptoServiceProvider crypt = new MD5CryptoServiceProvider();

            return crypt.ComputeHash(ToHash);
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

        #region T

        public static T Random<T>(IEnumerable<T> Enumerator)
        {
            Random Rand = new Random();

            List<T> List1 = new List<T>();

            foreach (T Value in Enumerator)
                List1.Add(Value);

            return List1[Rand.Next(List1.ToArray().Length)];
        }

        #endregion

        #region Encryption
        public static String GetHash(String ToHash, byte[] Salt)
        {
            byte[] Input = StringtoByte(ToHash);

            List<byte> Hash = new List<byte>();

            foreach (byte B in Input)
            {
                Hash.Add(B);
            }

            foreach (byte B in Salt)
            {
                Hash.Add(B);
            }

            return BytetoString(MD5Hash(Convert.ToBase64String(Hash.ToArray())));
        }

        public static void CreatePasswordFile(String FilePath, String Password, String Salt)
        {
            File.WriteAllText(FilePath, GetHash(Password, StringtoByte(Salt)));
        }

        public static bool ComparePasswords(String FilePath, String Password, String Salt)
        {
            String InputHash = GetHash(Password, StringtoByte(Salt));
            String FileHash = File.ReadAllText(FilePath);

            return (InputHash.Equals(FileHash));
        }
        #endregion
    }
}
