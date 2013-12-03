using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JDLL.Content
{
    public class StringArrayProcessor : IContentProcessor
    {
        public override string TypeName()
        {
            return "arrString";
        }

        public override void Export(System.IO.BinaryWriter bw, object data)
        {
            String[] Data = (String[])data;

            bw.Write(Data.Length);

            foreach (String s in Data)
            {
                Helper.WriteString(s, bw);
            }
        }

        public override object Import(System.IO.BinaryReader br)
        {
            List<String> Data = new List<String>();
            int Length = br.ReadInt32();

            for (int i = 0; i < Length; i++)
            {
                Data.Add(Helper.ReadString(br));
            }

            return Data.ToArray();
        }
    }
}
