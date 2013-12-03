using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace JDLL.Content
{
    public class Content_Manager
    {
        Dictionary<String, IContentProcessor> Processors = new Dictionary<String, IContentProcessor>();
        List<String> Names = new List<String>();

        public String Filename { get; private set; }

        public static ushort op_Start = 0x00F2F1;
        public static ushort op_End = 0x00F2F2;

        public Content_Manager(String filename)
        {
            this.Filename = filename;
        }

        public void RegisterProcessor(IContentProcessor processor)
        {
            this.Processors.Add(processor.TypeName(), processor);
        }

        public void Write(object data, String name, String processorTypeName)
        {
            if (this.Names.Contains(name))
            {
                // TODO: Add exception to throw
                return;
            }

            if (!this.Processors.ContainsKey(name))
            {
                // TODO: Add exception to throw
                return;
            }

            using (FileStream fs = new FileStream(this.Filename, FileMode.Append))
            {
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    bw.Write(Content_Manager.op_Start);
                    bw.Write(name);
                    bw.Write(this.Processors[processorTypeName].TypeName());

                    this.Processors[processorTypeName].Export(bw, data);

                    bw.Write(Content_Manager.op_End);
                }
            }
        }
    }
}
