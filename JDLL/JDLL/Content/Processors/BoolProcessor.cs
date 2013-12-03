using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JDLL.Content
{
    public class BoolProcessor : IContentProcessor
    {
        public override string TypeName()
        {
            return "bool";
        }

        public override void Export(System.IO.BinaryWriter bw, object data)
        {
            bw.Write((bool)data);
        }
    }
}
