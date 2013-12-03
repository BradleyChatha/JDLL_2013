using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JDLL.Content
{
    public class Int32Processor : IContentProcessor
    {
        public override string TypeName()
        {
            return "int32";
        }

        public override void Export(System.IO.BinaryWriter bw, object data)
        {
            bw.Write((int)data);
        }

        public override object Import(System.IO.BinaryReader br)
        {
            return br.ReadInt32();
        }
    }
}
