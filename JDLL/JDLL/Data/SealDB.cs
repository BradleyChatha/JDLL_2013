using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JDLL.Data
{
    public struct Group
    {
        public String Name;

        public Dictionary<String, Member> Members;

        public Group(String name)
        {
            Name = name;
            Members = new Dictionary<String, Member>();
        }

        public void Add(Member member)
        {
            try
            {
                Members.Add(member.Name, member);
            }
            catch (ArgumentException ex)
            {
                File.WriteAllLines("ERROR.log", new string[] { ex.Message, ex.StackTrace, member.Name });
                return;
            }
        }
    }

    public struct Member
    {
        public String Name;

        public Dictionary<String, Value> Values;

        public Member(String name)
        {
            Name = name;
            Values = new Dictionary<String, Value>();
        }

        public void Add(Value value)
        {
            Values.Add(value.Name, value);
        }
    }

    public struct Value
    {
        public String Name;
        public String sValue;

        public Value(String name, String value)
        {
            sValue = value;
            Name = name;
        }

        public void Set(String value)
        {
            sValue = value;
        }
    }

    public class SealDB
    {
        String _Extention = ".sealdb";

        String _Filename;
        String[] _Contents;

        public readonly String NotFound = "SEALDB::NOTFOUND::ERROR";

        Dictionary<String, Group> _Groups = new Dictionary<String, Group>();

        public SealDB(String filename)
        {
            if (!filename.EndsWith(_Extention))
            {
                filename += _Extention;
            }

            this._Filename = filename;

            if (File.Exists(this._Filename))
            {
                this._Contents = File.ReadAllLines(this._Filename);
            }

            GetGroups();
        }

        public void WriteGroup(String name)
        {
            if (this._Groups.ContainsKey(name))
            {
                return;
            }

            List<String> New = new List<String>();

            if (this._Contents != null && this._Contents.Length > 0)
            {
                New.AddRange(this._Contents);
            }

            New.Add("$" + name);
            New.Add("{");
            New.Add("}");

            this._Contents = New.ToArray();
            New = null;

            File.WriteAllLines(this._Filename, this._Contents);

            _Groups[name] = new Group(name);

            return;
        }

        public void WriteMember(String group, String name)
        {
            if(_Groups[group].Members.ContainsKey(name))
            {
                return;
            }

            List<String> New = new List<String>();

            bool Found = false;
            bool Found2 = false;

            foreach (String s in this._Contents)
            {
                if (s.Equals("$" + group))
                {
                    Found = true;
                }
                else if (s.Equals("{") && Found)
                {
                    Found2 = true;
                }

                if (Found && Found2 && !s.Equals("{"))
                {
                    New.Add("\t£" + name);
                    New.Add("\t{");
                    New.Add("\t}");

                    Found = false;
                    Found2 = false;
                }

                New.Add(s);
            }

            this._Contents = New.ToArray();
            New = null;

            File.WriteAllLines(this._Filename, this._Contents);
            _Groups[group].Members[name] = new Member(name);

            return;
        }

        public void DeleteValue(String group, String member, String valueName)
        {
            String Name = "";
            String Name2 = "";

            List<String> New = new List<String>();

            foreach (String s in this._Contents)
            {
                if (s.StartsWith("$"))
                {
                    Name = s.Split('$').Last();
                }

                if(s.Trim('\t').StartsWith("£"))
                {
                    Name2 = s.Trim('\t').Split('£').Last();
                }

                if (Name.Equals(group) && Name2.Equals(member) && s.Trim('\t').StartsWith(valueName))
                {
                    _Groups[group].Members[member].Values.Remove(valueName);
                    continue;
                }

                New.Add(s);
            }

            this._Contents = New.ToArray();
            New = null;

            File.WriteAllLines(this._Filename, this._Contents);

            return;
        }

        public void ChangeValue(String group, String member, String valueName, String value)
        {
            DeleteValue(group, member, valueName);
            WriteValue(group, member, valueName, value);
        }

        public void DeleteMember(String group, String member)
        {
            String Name1 = "";
            String Name2 = "";

            bool Found = false;
            bool Stop = false;

            List<String> New = new List<String>();

            foreach (String s in this._Contents)
            {
                if (!Stop)
                {
                    if (s.StartsWith("$" + group))
                    {
                        Name1 = s.Split('$').Last();
                    }

                    if (s.Trim('\t').StartsWith("£" + member))
                    {
                        Name2 = s.Trim('\t').Split('£').Last();
                        continue;
                    }

                    if (s.Trim('\t').Equals("{") && Name1.Equals(group) && Name2.Equals(member))
                    {
                        Found = true;
                        continue;
                    }

                    if (s.Trim('\t').Equals("}") && Name1.Equals(group) && Name2.Equals(member))
                    {
                        Found = false;
                        Name2 = "";
                        Stop = true;
                        continue;
                    }

                    if (Found)
                    {
                        continue;
                    }
                }

                New.Add(s);
            }

            this._Contents = New.ToArray();
            New = null;

            File.WriteAllLines(this._Filename, this._Contents);
            _Groups[group].Members.Remove(member);

            return;
        }

        public void WriteValue(String group, String member, String name, String value)
        {
            if (_Groups[group].Members[member].Values.ContainsKey(name))
            {
                return;
            }

            List<String> New = new List<String>();

            bool Found = false;
            bool Found2 = false;
            bool Found3 = false;
            bool Found4 = false;

            foreach (String s in this._Contents)
            {
                if (s.Equals("$" + group))
                {
                    Found = true;
                }

                if (s.Equals("{") && Found)
                {
                    Found2 = true;
                }

                if (s.Trim('\t').Equals("£" + member) && Found2)
                {
                    Found3 = true;
                }

                if (Found3 && !s.Trim('\t').Equals("{") && !s.Trim('\t').StartsWith("£") && !Found4)
                {
                    New.Add("\t\t" + name + " = " + value);
                    Found4 = true;
                }

                New.Add(s);
            }

            this._Contents = New.ToArray();
            New = null;

            File.WriteAllLines(this._Filename, this._Contents);
            this._Groups[group].Members[member].Values[name] = new Value(name, value);

            return;
        }

        public void DeleteGroup(String group)
        {
            if (!_Groups.ContainsKey(group))
            {
                return;
            }

            List<String> New = new List<String>();

            bool Found1 = false;

            foreach (String s in this._Contents)
            {
                if (s.Equals("$" + group))
                {
                    Found1 = true;
                    continue;
                }

                if (s.Equals("}") && Found1)
                {
                    Found1 = false;
                    continue;
                }

                if (Found1)
                {
                    continue;
                }

                New.Add(s);
            }

            this._Contents = New.ToArray();
            New = null;

            File.WriteAllLines(this._Filename, this._Contents);

            return;
        }

        public String GetValue(String group, String member, String valueName)
        {
            if (_Groups.ContainsKey(group))
            {
                if (_Groups[group].Members.ContainsKey(member))
                {
                    if (_Groups[group].Members[member].Values.ContainsKey(valueName))
                    {
                        return _Groups[group].Members[member].Values[valueName].sValue;
                    }
                }
            }

            return NotFound;
        }

        public bool DoesGroupExist(String group)
        {
            return _Groups.ContainsKey(group);
        }

        public bool DoesMemberExist(String group, String member)
        {
            if (_Groups.ContainsKey(group))
            {
                return _Groups[group].Members.ContainsKey(member);
            }

            return false;
        }

        public bool DoesValueExist(String group, String member, String valueName)
        {
            if (_Groups.ContainsKey(group))
            {
                if (_Groups[group].Members.ContainsKey(member))
                {
                    return _Groups[group].Members[member].Values.ContainsKey(valueName);
                }
            }

            return false;
        }

        private void GetGroups()
        {
            if (this._Contents == null)
            {
                return;
            }

            String Name = "";
            Member Memb = new Member();
            Value Val;

            bool Found = false;
            bool Found2 = false;
            bool Close1 = false;

            bool Found3 = false;
            bool Found4 = false;
            bool Close2 = false;

            foreach (String s in this._Contents)
            {
                if (s.StartsWith("$"))
                {
                    Name = s.Split('$').Last();
                    Found = true;

                    _Groups[Name] = new Group(Name);
                    continue;
                }

                if (Found && s.Equals("{"))
                {
                    Found2 = true;
                    continue;
                }

                if (Found && Found2) // Within the members
                {
                    if (s.Trim('\t').StartsWith("£"))
                    {
                        Memb = new Member(s.Split('£').Last());
                        Found3 = true;
                        continue;
                    }

                    if (Found3 && s.Trim('\t').Equals("{"))
                    {
                        Found4 = true;
                        continue;
                    }

                    if (Found3 && Found4 && !s.Trim('\t').Equals("}")) // Within the values
                    {
                        String Name2 = s.Split(new char[] { '=' }, 2)[0].Trim('\t', ' ');
                        String Value2 = s.Split(new char[] { '=' }, 2)[1].Trim(' ');

                        Memb.Add(new Value(Name2, Value2));
                        continue;
                    }

                    if (s.Trim('\t').Equals("}") && s.Contains('\t')) // End of a member
                    {
                        Found3 = false;
                        Found4 = false;

                        if (_Groups.ContainsKey(Name))
                        {
                            _Groups[Name].Add(Memb);
                        }

                        continue;
                    }
                }

                if (!s.StartsWith("\t") && s.Equals("}")) // End of a group
                {
                    Found = false;
                    Found2 = false;
                    continue;
                }
            }
        }
    }
}
