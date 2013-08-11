using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using JDLL.Util;
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
            this.Path = Path;
        }

        public String[] GetEntryByPrefix(String Prefix)
        {
            List<String> PrefixData = new List<String>();

            foreach (String s in Contents)
                if (s.StartsWith(Prefix + ";"))
                    PrefixData.Add(s);


            return PrefixData.ToArray();
        }

        public String GetName()
        {
            return Contents[0].Split(';')[1];
        }

        public String[] ReadValueFromEntry(String Prefix, String Name, bool withPrefix)
        {
            foreach (String s in Contents)
                if (Seal_Protocol.GetEntryName(s).Equals(Name) && s.StartsWith(Prefix))
                    return Seal_Protocol.GetEntryData(s, withPrefix);

            return new String[1] { Seal_Protocol.NO_VALUE };
        }

        public void DeleteEntry(String Prefix, String Name)
        {
            List<String> NewContent = new List<String>();

            foreach (String s in Contents)
                if (s.StartsWith(Prefix) && Seal_Protocol.GetEntryName(s).Equals(Name))
                    continue;
                else
                    NewContent.Add(s);

            Contents = NewContent.ToArray();
        }

        public void AddEntry(String Prefix, String Name, String Value)
        {
            List<String> NewContent = new List<String>();
            NewContent.AddRange(Contents);
            NewContent.Add(Prefix + ";" + Value + ";NAME=" + Name);

            Contents = NewContent.ToArray();
        }

        public void AddEntry(String Prefix, String Name, IEnumerable<String> Values)
        {
            String Value = "";

            foreach (String s in Values)
                Value += s + ";";

            List<String> NewContent = new List<String>();
            NewContent.AddRange(Contents);
            NewContent.Add(Prefix + ";" + Value + "NAME=" + Name);

            Contents = NewContent.ToArray();
        }

        public void Save()
        {
            FileIO.ReplaceAll(Path, Contents);
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
