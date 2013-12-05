using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JDLL.Content
{
    public class ByteArrayProcessor : IContentProcessor
    {
        public override string TypeName()
        {
            return "byteArray";
        }

        public override void Export(System.IO.BinaryWriter bw, object data)
        {
            byte[] Data = (byte[])data;

            bw.Write(Data.Length);
            bw.Write(Data);
        }

        public override object Import(System.IO.BinaryReader br)
        {
            Byte[] Data = br.ReadBytes(br.ReadInt32());

            return Data;
        }
    }
}
