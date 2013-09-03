using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Permissions;
using System.IO;

namespace JDLL.Data
{
    public class Log
    {
        public String FilePath { get; private set; }

        List<String> Contents = new List<String>();

        public Log(String FilePath, bool DeleteOld = false)
        {
            this.FilePath = FilePath;

            if (!File.Exists(FilePath) || DeleteOld)
                File.WriteAllText(FilePath, "");
            else
                Contents.AddRange(File.ReadAllLines(FilePath));

            Write("cfg", "Start()", true);
        }

        public void ChangePath(String FilePath, bool Save = false, bool DeleteOld = false)
        {
            Write("cfg", "End()", false);

            if (Save)
                this.Save();

            Contents.Clear();

            if (!File.Exists(FilePath) || DeleteOld)
                File.WriteAllText(FilePath, "");
            else
                Contents.AddRange(File.ReadAllLines(FilePath));

            this.FilePath = FilePath;
            Write("cfg", "Start()", true);
        }

        public void Write(String Sender, String Message, bool Save = false)
        {
            String DDMM = DateTime.Now.ToString("dd/MM") + "\t";
            String HHMMSS = DateTime.Now.ToString("HH:mm:ss" + "\t");

            Contents.Add(DDMM + HHMMSS + Sender + ":\t" + Message);

            if (Save)
                this.Save();
        }

        public void Save()
        {
            Write("cfg", "Save()");

            File.WriteAllLines(FilePath, Contents.ToArray());
        }

        public void Dispose(bool Save = false)
        {
            Write("cfg", "End()", Save);

            FilePath = null;
            Contents = null;
        }
    }
}
