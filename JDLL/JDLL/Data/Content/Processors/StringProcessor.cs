using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace JDLL.Data.Content
{
    public class StringProcessor : IContentProcessor<String>
    {
        public override void Export(BinaryWriter bw, string data)
        {
            bw.Write(data);
        }

        public override String Import(BinaryReader br)
        {
            return br.ReadString();
        }
    }
}
