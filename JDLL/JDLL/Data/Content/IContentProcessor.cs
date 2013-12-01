using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace JDLL.Data.Content
{
    public abstract class IContentProcessor<T>
    {
         public abstract T Import(BinaryReader br);
         public abstract void Export(BinaryWriter bw, T data);
    }
}
