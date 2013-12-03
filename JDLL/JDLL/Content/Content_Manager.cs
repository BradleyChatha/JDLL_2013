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

        public static ushort op_Start = 20;
        public static ushort op_End = 21;

        public Content_Manager(String filename)
        {
            this.Filename = filename;
            this.FillNames();

            this.RegisterProcessor(new StringProcessor());
            this.RegisterProcessor(new StringArrayProcessor());
            this.RegisterProcessor(new Int32Processor());
            this.RegisterProcessor(new Int32ArrayProcessor());
        }

        public void RegisterProcessor(IContentProcessor processor)
        {
            this.Processors.Add(processor.TypeName(), processor);
        }

        public void DeleteFile()
        {
            File.Delete(this.Filename);
        }

        public object Read(String name)
        {
            using (FileStream fs = new FileStream(this.Filename, FileMode.OpenOrCreate))
            {
                using (BinaryReader br = new BinaryReader(fs))
                {
                    while (br.PeekChar() != -1)
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

        public T Read<T>(String name)
        {
            return (T)this.Read(name);
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
                throw new ProcessorNotRegisteredException("Processor '" + processorTypeName + "' hasn't been registered!");
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

                    // TODO: Fix Char Buffer error related to this opcode writing out null values
                    // bw.Write(Content_Manager.op_End);
                }
            }
        }

        private void FillNames()
        {
            using (FileStream fs = new FileStream(this.Filename, FileMode.OpenOrCreate))
            {
                using (BinaryReader br = new BinaryReader(fs))
                {
                    ushort Num = 0;

                    try
                    {
                        while (br.PeekChar() != -1)
                        {
                            try
                            {
                                Num = br.ReadUInt16();
                            }
                            catch
                            {
                                return;
                            }

                            if (Num == Content_Manager.op_Start)
                            {
                                this.Names.Add(Helper.ReadString(br));
                            }
                        }
                    }
                    catch
                    {
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
