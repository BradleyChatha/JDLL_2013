using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JDLL.Content
{
    public class Int32ArrayProcessor : IContentProcessor
    {
        public override string TypeName()
        {
            return "int32Array";
        }

        public override void Export(System.IO.BinaryWriter bw, object data)
        {
            int[] Data = (int[])data;

            bw.Write(Data.Length);

            foreach (int i in Data)
            {
                bw.Write(i);
            }
        }

        public override object Import(System.IO.BinaryReader br)
        {
            List<int> Ints = new List<int>();
            int Length = br.ReadInt32();

            for (int i = 0; i < Length; i++)
            {
                Ints.Add(br.ReadInt32());
            }

            return Ints.ToArray();
        }
    }
}
