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

            bw.Write(data.Length);
            bw.Write(data);
        }

        internal static String ReadString(BinaryReader br)
        {
            return UTF8Encoding.UTF8.GetString(br.ReadBytes(br.ReadInt32()));
        }
    }
}
