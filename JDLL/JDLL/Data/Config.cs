using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;

using JDLL.Exceptions;

namespace JDLL.Data
{
    public class Config
    {
        String FilePath;

        public Config(String FilePath)
        {
            this.FilePath = FilePath;
        }

        public void CreateFile()
        {
            if(!File.Exists(FilePath))
                File.WriteAllText(FilePath, "[Config]"); 
        }

        public void isValid()
        {
            CreateFile();

            if (File.ReadAllLines(FilePath)[0].Equals("[Config]"))
                throw new MalformedConfigException("Missing [Config] Tag");
        }

        public void WriteValue(String Option, Object Value)
        {
            isValid();

            foreach (String s in File.ReadAllLines(FilePath))
                if (s.Split('=')[0].Equals("[" + Option + "]"))
                    throw new ValueAlreadyExistsException("[" + Option + "]" + " Already exists, Call ChangeValue() if you need to edit a value");

            List<String> List1 = new List<String>();
            List1.AddRange(File.ReadAllLines(FilePath));
            List1.Add("[" + Option + "]" + "=" + Value);

            File.WriteAllLines(FilePath, List1.ToArray());
        }

        public void WriteValue(String Option, IEnumerable<String> Enumerator)
        {
            isValid();

            foreach (String s in File.ReadAllLines(FilePath))
                if (s.Split('=')[0].Equals("[" + Option + "]"))
                    throw new ValueAlreadyExistsException("[" + Option + "]" + " Already exists, Call ChangeValue() if you need to edit a value");

            List<String> List1 = new List<String>();
            
            String Str1 = "";

            List1.AddRange(File.ReadAllLines(FilePath));

            foreach (String V in Enumerator)
                Str1 += V + ";";

            List1.Add("[" + Option + "]" + "=" + Str1);

            File.WriteAllLines(FilePath, List1.ToArray());
        }

        public void WriteValue(String Option, IEnumerable<int> Enumerator)
        {
            isValid();

            foreach (String s in File.ReadAllLines(FilePath))
                if (s.Split('=')[0].Equals("[" + Option + "]"))
                    throw new ValueAlreadyExistsException("[" + Option + "]" + " Already exists, Call ChangeValue() if you need to edit a value");

            List<String> List1 = new List<String>();

            String Str1 = "";

            List1.AddRange(File.ReadAllLines(FilePath));

            foreach (int V in Enumerator)
                Str1 += V.ToString() + ";";

            List1.Add("[" + Option + "]" + "=" + Str1);

            File.WriteAllLines(FilePath, List1.ToArray());
        }

        public void ChangeValue(String Option, Object Value)
        {
            isValid();

            List<String> List1 = new List<String>();

            foreach (String s in File.ReadAllLines(FilePath))
                if (s.StartsWith("[" + Option + "]"))
                    List1.Add("[" + Option + "]" + "=" + Value);
                else
                    List1.Add(s);

            File.WriteAllLines(FilePath, List1);
         }

        public void ClearFile()
        {
            File.Delete(FilePath);

            CreateFile();
        }

        public Object Parse(String Option)
        {
            isValid();

            String[] Array1 = File.ReadAllLines(FilePath);

            Object Obj1 = null;
            int Null;

            foreach (String s in Array1)
            {
                String[] Array2 = s.Split('=');

                if (Array2[0].Equals("[" + Option + "]"))
                {
                    if (Array2[1].ToLower().Equals("true"))
                    {
                        if (Array2[1].Contains(';'))
                        {
                            List<Boolean> List1 = new List<Boolean>();

                            foreach (String s2 in Array2[1].Split(';'))
                            {
                                if(!String.IsNullOrEmpty(s2))
                                List1.Add(Convert.ToBoolean(s2));
                            }

                            Obj1 = List1;
                        }
                        else
                            Obj1 = true;

                        break;
                    }

                    else if (Array2[1].ToLower().Equals("false"))
                    {
                        if (Array2[1].Contains(';'))
                        {
                            List<Boolean> List1 = new List<Boolean>();

                            foreach (String s2 in Array2[1].Split(';'))
                            {
                                if (!String.IsNullOrEmpty(s2))
                                List1.Add(Convert.ToBoolean(s2));
                            }

                            Obj1 = List1;
                        }
                        else
                            Obj1 = false;

                        break;
                    }

                    else if (int.TryParse(Array2[1], out Null))
                    {
                        if (Array2[1].Contains(';'))
                        {
                            List<int> List1 = new List<int>();

                            foreach (String s2 in Array2[1].Split(';'))
                            {
                                if (!String.IsNullOrEmpty(s2))
                                List1.Add(Convert.ToInt32(s2));
                            }

                            Obj1 = List1;
                        }
                        else
                            Obj1 = Convert.ToInt32(Array2[1]);

                        break;
                    }

                    else
                    {
                        if (Array2[1].Contains(';'))
                        {
                            List<String> List1 = new List<String>();

                            foreach (String s2 in Array2[1].Split(';'))
                            {
                                if (!String.IsNullOrEmpty(s2))
                                List1.Add(s2);
                            }

                            Obj1 = List1;
                        }
                        else
                            Obj1 = Array2[1];

                        break;
                    }
                }
            }

            return Obj1;
        }
    }
}