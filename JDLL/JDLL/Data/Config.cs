using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;

using JDLL.Exceptions;
using JDLL.Data.Logging;

namespace JDLL.Data
{
    public class Config
    {
        String FilePath;
        Log Log;

        public Config(String FileName)
        {
            this.FilePath = FileName += ".cfg";
        }

        public Config(String FileName, Log Log)
        {
            this.FilePath = FileName += ".cfg";
            this.Log = Log;
        }

        public void CreateFile()
        {
            if(!File.Exists(FilePath))
                File.WriteAllText(FilePath, "[Config]"); 
        }

        public void isValid()
        {
            CreateFile();

            if (!File.ReadAllText(FilePath).Contains("[Config]"))
                if (Log != null)
                    new MalformedConfigException().WriteToLog(ref Log, FilePath, false);
                else
                    throw new MalformedConfigException("Missing [Config] Tag");
        }

        public void WriteValue(String Key, Object Value)
        {
            isValid();

            foreach (String s in File.ReadAllLines(FilePath))
                if (s.Split('=')[0].Equals("[" + Key + "]"))
                    if (Log != null)
                    {
                        new ValueAlreadyExistsException().WriteToLog(ref Log, Key, false);

                        if (!Log.ThrowErrors)
                            return;
                    }
                    else
                        throw new ValueAlreadyExistsException("[" + Key + "]" + " Already exists, Call ChangeValue() if you need to edit a value");

            List<String> List1 = new List<String>();
            List1.AddRange(File.ReadAllLines(FilePath));
            List1.Add("[" + Key + "]" + "=" + Value);

            if (Log != null)
                Log.Write("cfg", "Entry: " + "[" + Key + "]" + "=" + Value + " :Was Added to: " + FilePath, new SEV_Info(), false);

            File.WriteAllLines(FilePath, List1.ToArray());
        }

        public void WriteValue(String Key, IEnumerable<String> Enumerator)
        {
            isValid();

            foreach (String s in File.ReadAllLines(FilePath))
                if (s.Split('=')[0].Equals("[" + Key + "]"))
                    if (Log != null)
                    {
                        new ValueAlreadyExistsException().WriteToLog(ref Log, Key, false);

                        if (!Log.ThrowErrors)
                            return;
                    }
                    else
                        throw new ValueAlreadyExistsException("[" + Key + "]" + " Already exists, Call ChangeValue() if you need to edit a value");

            List<String> List1 = new List<String>();
            
            String Str1 = "";

            List1.AddRange(File.ReadAllLines(FilePath));

            foreach (String V in Enumerator)
                Str1 += V + ";";

            List1.Add("[" + Key + "]" + "=" + Str1);

            if (Log != null)
                Log.Write("cfg", "Entry: " + "[" + Key + "]" + "=" + Str1 + " :Was Added to: " + FilePath, new SEV_Info(), false);

            File.WriteAllLines(FilePath, List1.ToArray());
        }

        public void WriteValue(String Key, IEnumerable<int> Enumerator)
        {
            isValid();

            foreach (String s in File.ReadAllLines(FilePath))
                if (s.Split('=')[0].Equals("[" + Key + "]"))
                    if (Log != null)
                    {
                        new ValueAlreadyExistsException().WriteToLog(ref Log, Key, false);

                        if (!Log.ThrowErrors)
                            return;
                    }
                    else
                        throw new ValueAlreadyExistsException("[" + Key + "]" + " Already exists, Call ChangeValue() if you need to edit a value");

            List<String> List1 = new List<String>();

            String Str1 = "";

            List1.AddRange(File.ReadAllLines(FilePath));

            foreach (int V in Enumerator)
                Str1 += V.ToString() + ";";

            List1.Add("[" + Key + "]" + "=" + Str1);

            if (Log != null)
                Log.Write("cfg", "Entry: " + "[" + Key + "]" + "=" + Str1 + " :Was Added to: " + FilePath, new SEV_Info(), false);

            File.WriteAllLines(FilePath, List1.ToArray());
        }

        public void WriteIfNoneExistant(String Key, Object Value)
        {
            if (!DoesKeyExist(Key))
                WriteValue(Key, Value);
        }

        public void WriteIfNoneExistant(String Key, IEnumerable<String> Enumerator)
        {
            if (!DoesKeyExist(Key))
                WriteValue(Key, Enumerator);
        }

        public void WriteIfNoneExistant(String Key, IEnumerable<int> Enumerator)
        {
            if (!DoesKeyExist(Key))
                WriteValue(Key, Enumerator);
        }

        public void ChangeValue(String Key, Object Value)
        {
            isValid();

            List<String> List1 = new List<String>();

            foreach (String s in File.ReadAllLines(FilePath))
                if (s.StartsWith("[" + Key + "]"))
                {
                    if (Log != null)
                        Log.Write("cfg", "Entry: " + s + " :Was Changed to: " + "[" + Key + "]" + "=" + Value + " :In: " + FilePath, new SEV_Info(), false);

                    List1.Add("[" + Key + "]" + "=" + Value);
                }
                else
                    List1.Add(s);

            File.WriteAllLines(FilePath, List1);
         }

        public void ChangeValue(String Key, IEnumerable<String> Values)
        {
            isValid();

            List<String> List1 = new List<String>();
            String S = "";

            foreach (String S1 in Values)
                S += S1 + ";";

            foreach (String s in File.ReadAllLines(FilePath))
                if (s.StartsWith("[" + Key + "]"))
                {
                    if (Log != null)
                        Log.Write("cfg", "Entry: " + s + " :Was Changed to: " + "[" + Key + "]" + "=" + S + " :In: " + FilePath, new SEV_Info(), false);

                    List1.Add("[" + Key + "]" + "=" + S);
                }
                else
                    List1.Add(s);

            File.WriteAllLines(FilePath, List1);
        }

        public void ChangeValue(String Key, IEnumerable<int> Values)
        {
            isValid();

            List<String> List1 = new List<String>();
            String S = "";

            foreach (int S1 in Values)
                S += S1.ToString() + ";";

            foreach (String s in File.ReadAllLines(FilePath))
                if (s.StartsWith("[" + Key + "]"))
                {
                    if (Log != null)
                        Log.Write("cfg", "Entry: " + s + " :Was Changed to: " + "[" + Key + "]" + "=" + S + " :In: " + FilePath, new SEV_Info(), false);

                    List1.Add("[" + Key + "]" + "=" + S);
                }
                else
                    List1.Add(s);

            File.WriteAllLines(FilePath, List1);
        }

        public void DeleteValue(String Key)
        {
            String[] Contents = File.ReadAllLines(FilePath);
            List<String> Contents1 = new List<String>();

            foreach (String s in Contents)
            {
                if (!s.StartsWith("[" + Key + "]"))
                    Contents1.Add(s);
            }

            File.WriteAllLines(FilePath, Contents1);
        }

        public void ClearFile()
        {
            File.Delete(FilePath);

            if (Log != null)
                Log.Write("cfg", FilePath + " Cleared", new SEV_Info(), true);

            CreateFile();
        }

        public bool DoesKeyExist(String Key)
        {
            bool Return = false;

            foreach (String s in File.ReadAllLines(FilePath))
                if (s.StartsWith("[" + Key + "]"))
                    Return = true;

            return Return;
        }

        public Object Parse(String Key)
        {
            isValid();

            try
            {
                String[] Array1 = File.ReadAllLines(FilePath);

                Object Obj1 = null;
                int Null;

                foreach (String s in Array1)
                {
                    String[] Array2 = s.Split('=');

                    if (Array2[0].Equals("[" + Key + "]"))
                    {
                        if (Array2[1].ToLower().Equals("true"))
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
            catch(Exception ex)
            {
                if (Log != null)
                {
                    Log.Write("cfg", "An Error has Occured " + ex.Message, new SEV_Severe(ex), false);
                }

                throw ex;
            }
        }

        public T Read<T>(String Key)
        {
            try
            {
                String[] Array1 = File.ReadAllLines(FilePath);

                String Str1 = "";

                foreach (String s in Array1)
                    if (s.StartsWith("[" + Key + "]"))
                        Str1 = s.Split('=')[1];

                if (typeof(T) == typeof(int))
                    return (T)((object)Convert.ToInt32(Str1));


                return (T)((object)Str1);
            }
            catch (Exception ex)
            {
                if (Log != null)
                {
                    Log.Write("cfg", "An Error has Occured ", new SEV_Severe(ex), false);
                }

                throw ex;
            }
        }
    }
}