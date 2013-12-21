using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace JDLL.Content
{
    public static class Helper
    {
        public static short DefaultObfuscationKey = 20; 

        #region Internal
        internal static void WriteString(String input, BinaryWriter bw)
        {
            byte[] data = UTF8Encoding.UTF8.GetBytes(input);
            int length = data.Length;

            bw.Write(length);
            bw.Write(data);
        }

        internal static String ReadString(BinaryReader br)
        {
            String Data = UTF8Encoding.UTF8.GetString(br.ReadBytes(br.ReadInt32()));

            foreach (char c in Data)
            {
                if (c == '\0')
                {
                    Data += UTF8Encoding.UTF8.GetString(new byte[] { br.ReadByte() });
                }
            }

            Data.Trim('\0');
            return Data;
        }
        #endregion

        #region Obfuscation
        /// <summary>
        /// Obfuscates the string (Insecurely) to make it unreadable to end-users
        /// </summary>
        /// <param name="toObf">String to Obfuscate</param>
        /// <param name="key">Short to shift the string by, aka a Key</param>
        /// <returns>Obfuscated String</returns>
        public static String Obfuscate(this String toObf, Int16 key)
        {
            int Min = Convert.ToInt32(char.MinValue);
            int Max = Convert.ToInt32(char.MaxValue);

            char[] Buffer = toObf.ToCharArray();

            for (int i = 0; i < Buffer.Length; i++)
            {
                int CharNum = (Convert.ToInt16(Buffer[i]) + key);

                if (CharNum > Max)
                {
                    CharNum -= Max;
                }

                if (CharNum < Min)
                {
                    CharNum += Min;
                }

                Buffer[i] = Convert.ToChar(CharNum);
            }

            return new String(Buffer);
        }


        /// <summary>
        /// Deobfuscates an obfuscated String, note that using the incorrect key can result in the loss of the string
        /// </summary>
        /// <param name="toDeObf">String to Deobfuscate</param>
        /// <param name="key">Key to shift the string by, aka a key</param>
        /// <returns>Deobfuscate String</returns>
        public static String Deobfuscate(this String toDeObf, Int16 key)
        {
            return Helper.Obfuscate(toDeObf, (short)(key * -1));
        }
        #endregion

        #region Base64
        /// <summary> 
        /// Convert a String into a Base64 String, primarily for insecure Obfuscation
        /// </summary>
        /// <param name="toEncode">String to Encode</param>
        /// <returns>Base64 Encoded string</returns>
        public static String Base64Encode(String toEncode)
        {
            return Convert.ToBase64String(Encoding.Unicode.GetBytes(toEncode));
        }

        /// <summary>
        /// Converts a byte array into a Base64 String, Primarily used for storage of binary data that can be sent easily over a network
        /// </summary>
        /// <param name="toEncode">Byte array to encode</param>
        /// <returns>Base64 Encoded string</returns>
        public static String Base64Encode(byte[] toEncode)
        {
            return Convert.ToBase64String(toEncode);
        }

        /// <summary>
        /// Decodes a Base64 String and returns it's bytes
        /// </summary>
        /// <param name="toDecode">String to decode</param>
        /// <returns>Bytes of the Base64 String</returns>
        public static byte[] Base64Decode(String toDecode)
        {
            return Convert.FromBase64String(toDecode);
        }


        /// <summary>
        /// Decodes a Base64 String and returns it as a string, only use for obfuscated Strings, avoid binary data
        /// </summary>
        /// <param name="toDecode">String to decode</param>
        /// <returns>Unobfuscated version of a Base64 String, unusual results may occur if the string was made from binary data</returns>
        public static String Base64DecodeString(String toDecode)
        {
            return Encoding.Unicode.GetString(Convert.FromBase64String(toDecode));
        }
        #endregion

        #region MemoryStream
        public static MemoryStream ByteArrayToMemoryStream(byte[] array)
        {
            return new MemoryStream(array);
        }

        public static MemoryStream FileEntryToMemoryStream(String entryName, ref Content_Manager manager)
        {
            return new MemoryStream(manager.Read<byte[]>(entryName));
        }
        #endregion

        #region Write Methods
        /// <summary>
        /// Takes the data from a file entry/byte array entry and puts it into a file
        /// </summary>
        /// <param name="entryName">Name of the Byte Array/File entry</param>
        /// <param name="filePath">Path to extract the file to</param>
        /// <param name="manager">Content Manager used to read the data file</param>
        public static void ExtractFile(String entryName, String filePath, ref Content_Manager manager)
        {
            File.WriteAllBytes(filePath, manager.Read<byte[]>(entryName));
        }

        /// <summary>
        /// Stores a file into the Content Managers file, Throws FileNotFoundException if the file doesn't exist. Same effect from using "manager.Write(filePath, entryName, "file")"
        /// </summary>
        /// <param name="entryName">Name to give the entry</param>
        /// <param name="filePath">Path to the file to store</param>
        /// <param name="manager">Content Manager to store the file with</param>
        /// <exception cref="FileNotFoundException">Thrown if "filePath" doesn't point to a file</exception>
        public static void StoreFile(String entryName, String filePath, ref Content_Manager manager)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException(filePath + " not found!");
            }

            manager.Write(filePath, entryName, "file");
        }

        public static void WriteString(String entryName, String data, ref Content_Manager manager)
        {
            manager.Write(data, entryName, "string");
        }

        public static void WriteStringArray(String entryName, String[] data, ref Content_Manager manager)
        {
            manager.Write(data, entryName, "stringArray");
        }

        public static void WriteInt32(String entryName, int data, ref Content_Manager manager)
        {
            manager.Write(data, entryName, "int32");
        }

        public static void WriteInt32Array(String entryName, int[] data, ref Content_Manager manager)
        {
            manager.Write(data, entryName, "int32Array");
        }

        public static void WriteBool(String entryName, bool data, ref Content_Manager manager)
        {
            manager.Write(data, entryName, "bool");
        }

        public static void WriteByteArray(String entryName, byte[] data, ref Content_Manager manager)
        {
            manager.Write(data, entryName, "byteArray");
        }
        #endregion
    }
}
