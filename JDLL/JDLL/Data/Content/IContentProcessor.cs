using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace JDLL.Data.Content
{
    public interface IContentProcessor<T>
    {
        public T Import(BinaryReader br);
        public void Export(BinaryWriter bw);
    }
}
