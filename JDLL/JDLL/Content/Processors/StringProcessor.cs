using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JDLL.Content
{
    public class StringProcessor : IContentProcessor
    {
        public override string TypeName()
        {
            return "string";
        }

        public override object Import(System.IO.BinaryReader br)
        {
            return br.ReadString();
        }

        public override void Export(System.IO.BinaryWriter bw, object data)
        {
            bw.Write(Convert.ToString(data)); 
        }
    }
}
