using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using JDLL.Exceptions;

namespace JDLL.Data.Structures
{
    public class Seal_Protocol_File
    {
        String Path;
        public String[] Contents;

        public Seal_Protocol_File(String Path)
        {
            Seal_Protocol.IsSealFile(Path);

            Contents = Seal_Protocol.GetData(Path);
            Contents = FileIO.EncryptString(Contents);
            this.Path = Path;
        }

        public String[] GetDataByPrefix(String Prefix)
        {
            List<String> PrefixData = new List<String>();

            foreach (String s in FileIO.DecryptString(Contents))
                if (s.StartsWith(Prefix + ";"))
                    PrefixData.Add(s);


            return PrefixData.ToArray();
        }

        public String GetName()
        {
            return FileIO.DecryptString(Contents)[0].Split(';')[1];
        }

        public String[] ReadValueFromEntry(String Prefix, String Name, bool withPrefix)
        {
            foreach (String s in FileIO.DecryptString(Contents))
                if (Seal_Protocol.GetEntryName(s).Equals(Name) && s.StartsWith(Prefix))
                    return Seal_Protocol.GetEntryData(s, withPrefix);

            return new String[1] { Seal_Protocol.NO_VALUE };
        }

        public Seal_Protocol_File Clone()
        {
            return this;
        }

        public bool DeleteFile()
        {
            try
            {
                File.Delete(Path);

                Contents = null;
                Path = null;

                return true;
            }
            catch
            {
                return false;
            }
        }

        public void DeleteEntry(String Prefix, String Name)
        {
            List<String> NewContent = new List<String>();

            foreach (String s in FileIO.DecryptString(Contents))
                if (s.StartsWith(Prefix) && Seal_Protocol.GetEntryName(s).Equals(Name))
                    continue;
                else
                    NewContent.Add(s);

            Contents = FileIO.EncryptString(NewContent.ToArray());
        }

        public void AddEntry(String Prefix, String Name, String Value)
        {
            List<String> NewContent = new List<String>();
            NewContent.AddRange(FileIO.DecryptString(Contents));
            NewContent.Add(Prefix + ";" + Value + ";NAME=" + Name);

            Contents = FileIO.EncryptString(NewContent.ToArray());
        }

        public void AddEntry(String Prefix, String Name, IEnumerable<String> Values)
        {
            String Value = "";

            foreach (String s in Values)
                Value += s + ";";

            List<String> NewContent = new List<String>();
            NewContent.AddRange(FileIO.DecryptString(Contents));
            NewContent.Add(Prefix + ";" + Value + "NAME=" + Name);

            Contents = FileIO.EncryptString(NewContent.ToArray());
        }

        public void ChangeEntryData(String Prefix, String Name, String Value)
        {
            DeleteEntry(Prefix, Name);

            AddEntry(Prefix, Name, Value);
        }

        public void ChangeEntryData(String Prefix, String Name, IEnumerable<String> Values)
        {
            DeleteEntry(Prefix, Name);

            AddEntry(Prefix, Name, Values);
        }

        public void WipeContents(bool KeepName)
        {
            String Name = null;

            if (KeepName)
                Name = GetName();

            if (Name != null)
                Contents = new String[] { Name };
            else
                Contents = new String[] { "" };
        }

        public void CloneFile(String Path)
        {
            Seal_Protocol.CreateFile(Path, GetName());
        }

        public void Save()
        {
            FileIO.ReplaceAll(Path, FileIO.DecryptString(Contents));
        }

        public void Close(bool Save)
        {
            if (Save)
                this.Save();

            Contents = null;
            Path = null;
        }
    }
}
