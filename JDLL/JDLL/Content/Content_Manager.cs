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

        public static ushort op_Start = 0x01;
        public static ushort op_End = 0x02;

        public Content_Manager(String filename)
        {
            this.Filename = filename;
            this.FillNames();

            this.RegisterProcessor(new StringProcessor());
        }

        public void RegisterProcessor(IContentProcessor processor)
        {
            this.Processors.Add(processor.TypeName(), processor);
        }

        public object Read(String name)
        {
            using (FileStream fs = new FileStream(this.Filename, FileMode.OpenOrCreate))
            {
                using (BinaryReader br = new BinaryReader(fs))
                {
                    while (br.PeekChar() != -1 && br.PeekChar() != 0)
                    {
                        if (br.ReadUInt16() == Content_Manager.op_Start)
                        {
                            if (Helper.ReadString(br).Equals(name))
                            {
                                String Process = Helper.ReadString(br);

                                return this.Processors[Process].Import(br);
                            }
                        }
                    }
                }
            }

            // TODO: Create a custom exception
            throw new Exception("Entry '" + name + "' not found!");
        }

        public void Write(object data, String name, String processorTypeName)
        {
            if (this.Names.Contains(name))
            {
                // TODO: Add exception to throw
                return;
            }

            if (!this.Processors.ContainsKey(processorTypeName))
            {
                // TODO: Add exception to throw
                return;
            }

            this.Names.Add(name);

            using (FileStream fs = new FileStream(this.Filename, FileMode.Append))
            {
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    bw.Write(Content_Manager.op_Start);
                    Helper.WriteString(name, bw);
                    Helper.WriteString((this.Processors[processorTypeName].TypeName()), bw);

                    this.Processors[processorTypeName].Export(bw, data);

                    bw.Write(Content_Manager.op_End);
                }
            }
        }

        private void FillNames()
        {
            using (FileStream fs = new FileStream(this.Filename, FileMode.OpenOrCreate))
            {
                using (BinaryReader br = new BinaryReader(fs))
                {
                    while (br.PeekChar() != -1 && br.PeekChar() != 0)
                    {
                        ushort Num = br.ReadUInt16();

                        if (Num == Content_Manager.op_Start)
                        {
                            this.Names.Add(Helper.ReadString(br));
                        }
                    }
                }
            }
        }

        ~Content_Manager()
        {
            File.WriteAllLines("Debug.txt", Names.ToArray());

            this.Filename = "";
            this.Names = null;
            this.Processors = null;
        }
    }
}
