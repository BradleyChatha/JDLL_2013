using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace JDLL.Data
{
    public class FileIO
    {
        #region Encryption
        internal static char[] Letters = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', ',', '.', '"', '!', ' ', '(', ')', ';', '=' };
        internal static String[] Encryption = new String[] { "M7S0P", "O2P4X", "F0M5F", "P3A7S", "Z7C9N", "N9K4D", "X2G8O", "P5D1B", "Q7A2G", "N9H4W", "J4L8I", "M2P1U", "F4W0R", "L4Z0R", "V8T4M", "L0L0L", "Q7D6F", "B0B0X", "E6G6Y", "L0V3Y", "N4T7Q", "X0N7L", "L6Z3B", "G4Y3R", "H4X0R", "K9P8F", "P2P2P", "M0M0M", "D4D3Y", "H8U6V", "T0O0I", "M4N8P", "S0P9Y", "N5A2D", "U5S8N", "S5G2G", "M0P8L", "M1P2H", "K1Z3L", "F9L3Y", "Z0X0R", "A5P9N", "S7G7H", "Z4P3R", "M2A8K", "K4P4P", "N1P3X", "A0X3Y", "C8S0S", "C0C7Y", "M3M3M", "I0N7X", "I9Q3H", "U0I8U", "A8I2F", "D5I9K", "S7D2J", "I0P2L", "C7O2F", "C9I1G", "S5I2G", "U0M4D", "U4P2K", "U1X3C", "U9O0X", "A0W3E", "N0T5M", "M3H2S", "E3D1E", "P0L0R" };
        
        public static void EncryptFile(String Path, String Output, bool DeleteOrigin)
        {
            List<String> Contents = new List<String>();

            foreach(String S in File.ReadAllLines(Path))
            {
                String Encrypt = "";

                foreach (Char C in S)
                    for (int i = 0; i < Letters.Length; i++)
                        if (C.Equals(Letters[i]))
                            Encrypt += Encryption[i] + "*";

                Contents.Add(Encrypt);
            }

            File.WriteAllLines(Output, Contents.ToArray());

            if (DeleteOrigin)
                File.Delete(Path);
        }

        public static void DecryptFile(String Path, String Output, bool DeleteEncryptedFile)
        {
            List<String> Contents = new List<String>();

            foreach (String S in File.ReadAllLines(Path))
            {
                String Decrypt = "";

                foreach (String S2 in S.Split('*'))
                    for (int i = 0; i < Encryption.Length; i++)
                        if (S2.Equals(Encryption[i]))
                            Decrypt += Letters[i];

                Contents.Add(Decrypt);
            }

            File.WriteAllLines(Output, Contents.ToArray());

            if (DeleteEncryptedFile)
                File.Delete(Path);
        }

        public static void MultiEncrypt(String Path, String Output, bool DeleteOrigin, int Number)
        {
            File.Copy(Path, Path + ".bak");

            for (int i = 0; i < Number - 1; i++)
                EncryptFile(Path, Path, false);

            EncryptFile(Path, Output, DeleteOrigin);

            if(!DeleteOrigin)
                File.Copy(Path + ".bak", Path);

            File.Delete(Path + ".bak");
        }

        public static void MultDecrypt(String Path, String Output, bool DeleteEncryptedFile, int Number)
        {
            File.Copy(Path, Path + ".bak");

            for (int i = 0; i < Number - 1; i++)
                DecryptFile(Path, Path, false);

            DecryptFile(Path, Output, DeleteEncryptedFile);

            if(!DeleteEncryptedFile)
                File.Copy(Path + ".bak", Path);

            File.Delete(Path + ".bak");
        }

        public static String[] ReadFile(String Path)
        {
            String FileName = Variables.RandomString(5);

            DecryptFile(Path, FileName, false);

            String[] Array = File.ReadAllLines(FileName);

            File.Delete(FileName);

            return Array;
        }

        public static String ReadLine(String Path, int Line)
        {
            String[] S = ReadFile(Path);

            return S[Line - 1];
        }

        public static void WriteToFile(String Path, String Write)
        {
            String[] OldContents = ReadFile(Path);
            String TempFile = Variables.RandomString(6);

            List<String> NewContetnts = new List<String>();
            NewContetnts.AddRange(OldContents);
            NewContetnts.Add(Write);

            File.WriteAllLines(TempFile, NewContetnts);

            EncryptFile(TempFile, Path, true);
        }

        public static void WriteToFile(String Path, IEnumerable<String> Write)
        {
            String[] OldContents = ReadFile(Path);
            String TempFile = Variables.RandomString(6);

            List<String> NewContetnts = new List<String>();
            NewContetnts.AddRange(OldContents);
            NewContetnts.AddRange(Write);

            File.WriteAllLines(TempFile, NewContetnts);

            EncryptFile(TempFile, Path, true);
        }

        public static void CreateFile(String Path, String Contents)
        {
            String TempFile = Variables.RandomString(4);

            File.WriteAllText(TempFile, Contents);
            EncryptFile(TempFile, Path, true);
        }

        public static void CreateFile(String Path, IEnumerable<String> Contents)
        {
            String TempFile = Variables.RandomString(10);

            File.WriteAllLines(TempFile, Contents);
            EncryptFile(TempFile, Path, true);
        }

        public static void ReplaceAll(String Path, IEnumerable<String> Contents)
        {
            String TempFile = Variables.RandomString(3);

            File.WriteAllLines(TempFile, Contents);
            EncryptFile(TempFile, Path, true);
        }

        public static String EncryptString(String Input)
        {
            String Encrypt = "";

            foreach (Char C in Input)
                for (int i = 0; i < Letters.Length; i++)
                    if (C.Equals(Letters[i]))
                        Encrypt += Encryption[i] + "*";

            return Encrypt;
        }

        public static String DecryptString(String Input)
        {
            String Decrypt = "";

            foreach (String S2 in Input.Split('*'))
                for (int i = 0; i < Encryption.Length; i++)
                    if (S2.Equals(Encryption[i]))
                        Decrypt += Letters[i];

            return Decrypt;
        }

        public static String[] EncryptString(IEnumerable<String> Input)
        {
            List<String> Enc = new List<String>();


            foreach (String s in Input)
            {
                String Encrypt = "";

                foreach (Char C in s)
                    for (int i = 0; i < Letters.Length; i++)
                        if (C.Equals(Letters[i]))
                            Encrypt += Encryption[i] + "*";

                Enc.Add(Encrypt);
            }

            return Enc.ToArray();
        }

        public static String[] DecryptString(IEnumerable<String> Input)
        {
            List<String> Dec = new List<String>();

            foreach (String s in Input)
            {
                String Decrypt = "";

                foreach (String S2 in s.Split('*'))
                    for (int i = 0; i < Encryption.Length; i++)
                        if (S2.Equals(Encryption[i]))
                            Decrypt += Letters[i];

                Dec.Add(Decrypt);
            }

            return Dec.ToArray();
        }
        #endregion
    }
}