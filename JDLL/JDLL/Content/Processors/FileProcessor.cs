using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace JDLL.Content
{
    public class FileProcessor : IContentProcessor
    {
        public override string TypeName()
        {
            return "file";
        }

        public override void Export(BinaryWriter bw, object data)
        {
            byte[] Data = File.ReadAllBytes((String)data);

            bw.Write(Data.Length);
            bw.Write(Data);
        }

        public override object Import(BinaryReader br)
        {
            byte[] Data =  new byte[br.ReadInt32()];
            Data = br.ReadBytes(Data.Length);

            return Data;
        }
    }
}
