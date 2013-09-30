using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Permissions;
using System.IO;

namespace JDLL.Data.Logging
{
    public class Log
    {
        const String LogSender = "log";
        const String Start = "Start()";
        const String End = "End()";

        public String FilePath { get; private set; }

        List<String> Contents = new List<String>();

        public bool WriteWhenSave = true;
        public bool ThrowErrors = true;

        public Log(String FilePath, bool DeleteOld = false, bool WriteWhenSave = true, bool ThrowErrors = true)
        {
            this.FilePath = FilePath;

            if (!File.Exists(FilePath) || DeleteOld)
                File.WriteAllText(FilePath, "");
            else
                Contents.AddRange(File.ReadAllLines(FilePath));

            this.WriteWhenSave = false;
            Write(LogSender, Start, new SEV_Info(), true);
            this.WriteWhenSave = WriteWhenSave;
            this.ThrowErrors = ThrowErrors;
        }

        public void ChangePath(String FilePath, bool Save = true, bool DeleteOld = false)
        {
            Write(LogSender, End, new SEV_Info(), false);

            if (Save)
                EndSave();

            Contents.Clear();

            if (!File.Exists(FilePath) || DeleteOld)
                File.WriteAllText(FilePath, "");
            else
                Contents.AddRange(File.ReadAllLines(FilePath));

            this.FilePath = FilePath;
            Write(LogSender, Start, new SEV_Info(), true);
        }

        public void Write(String Sender, String Message, bool Save = false)
        {
            try
            {
                String DDMM = DateTime.Now.ToString("dd/MM") + "\t";
                String HHMMSS = DateTime.Now.ToString("HH:mm:ss" + "\t");

                Contents.Add(DDMM + HHMMSS + Sender + ":\t" + Message);

                if (Save)
                    this.Save();
            }
            catch
            { 
            }
        }

        public void Write(String Sender, String Message, ISeverety Severety, bool Save = false)
        {
            Write(Sender, Severety.LogInfo + " " + Message, Save);

            if (Severety.ThrowError && ThrowErrors)
            {
                File.WriteAllLines(FilePath + ".stacktrace", new String[] { Severety.Error.Message, Severety.Error.StackTrace });

                Dispose(true);
                throw Severety.Error;
            }
        }

        public void Save()
        {
            if(WriteWhenSave)
                Write(LogSender, "Save()", new SEV_Info());

            File.WriteAllLines(FilePath, Contents.ToArray());
        }

        public void ForceSave()
        {
            Write(LogSender, "Save()", new SEV_Info());

            File.WriteAllLines(FilePath, Contents.ToArray());
        }

        private void EndSave()
        {
            Write(LogSender, "EndSave()", new SEV_Info(), false);
            File.WriteAllLines(FilePath, Contents.ToArray());
        }

        public void Dispose(bool Save = false)
        {
            Write(LogSender, End, new SEV_Info(), false);

            if (Save)
                EndSave();

            FilePath = null;
            Contents = null;
        }

        ~Log()
        {
            Dispose(true);
        }
    }
}
