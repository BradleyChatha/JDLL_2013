using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace JDLL.Content
{
    class Helper
    {
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
    }
}
