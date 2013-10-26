using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace JDLL.Data
{
    public class BinaryResource
    {
        String _FileName;

        public BinaryResource(String file)
        {
            this._FileName = file;
        }

        public void Write(String content, bool oldContents = false)
        {
            String[] Contents = null;

            if (oldContents)
            {
                Contents = Read();
            }

            using (FileStream fs = new FileStream(this._FileName, FileMode.Create))
            {
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    if (oldContents && Contents != null)
                    {
                        foreach (String s in Contents)
                        {
                            bw.Write(s);
                        }
                    }

                    bw.Write(content);
                }
            }
        }

        public void Write(String[] contents, bool oldContents = false)
        {
            foreach (String s in contents)
            {
                this.Write(s, oldContents);
            }
        }

        public String[] Read()
        {
            List<String> Contents = new List<String>();

            using (FileStream fs = new FileStream(this._FileName, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader br = new BinaryReader(fs))
                {
                    while (br.BaseStream.Position != br.BaseStream.Length)
                    {
                        if (br.PeekChar() != 0)
                        {
                            Contents.Add(br.ReadString());
                        }
                    }
                }
            }

            return Contents.ToArray();
        }
    }
}
