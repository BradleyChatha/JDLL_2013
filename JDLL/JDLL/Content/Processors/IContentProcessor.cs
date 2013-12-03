using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace JDLL.Content
{
    public abstract class IContentProcessor
    {
        public abstract String TypeName();

        public abstract object Import(BinaryReader br);
        public abstract void Export(BinaryWriter bw, object data);
    }
}
