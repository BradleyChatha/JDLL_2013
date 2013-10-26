using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace JDLL.SealScript
{
    public delegate void ScriptMethod(String[] Parameters, ScriptInstance Sender = null);

    public class ScriptInstance : IDisposable
    {
        private const char Invoke = '~';
        private const char Set = '=';
        private const char TypeSet = '&';
        private const char StringDef = '"';
        private const char ParamSeperator = '>';

        private const String StringPrefix = "str";
        private const String DoublePrefix = "double";
        private const String IntPrefix = "int";
        private const String MethodVariable = "->";
        private const String DontFilter = "^";
        private const String Parameter = "%";
        private const String VariableValue = "*";

        internal Dictionary<String, object> Variables = new Dictionary<String, object>();
        internal Dictionary<String, int[]> Methods = new Dictionary<String, int[]>();
        internal Dictionary<String, ScriptMethod> EngineMethods = new Dictionary<String, ScriptMethod>();
        private Engine Engine;
        private String FilePath;
        internal String[] Contents;

        private bool _Run = true;

        public ScriptInstance(String FilePath, String[] Parameters, Engine Engine)
        {
            Contents = File.ReadAllLines(FilePath);

            if (Parameters != null)
            {
                for (int i = 0; i < Parameters.Length; i++)
                {
                    String S = Parameters[i];
                    String[] Data = S.Split('¬');
                    Variables[Data[0]] = Data[1];
                }
            }

            this.Engine = Engine;
            this.FilePath = FilePath;
            this.EngineMethods = Engine.EngineMethods;
        }

        public void SetVariable(String Key, String Value)
        {
            Variables[Key] = ParseInfo(Value, true);
        }

        public void Execute(bool Disposes = true)
        {
            if (Engine.Debug("START"))
            {
                Console.WriteLine("START " + FilePath);
            }

            DiscoverMethods();
            ExecuteMethod(".DATA");

            if (Engine.Debug("VARIABLE"))
            {
                foreach (KeyValuePair<String, object> Entry in Variables)
                {
                    Console.WriteLine("VARIABLE " + Entry.Key + " " + Entry.Value);
                }
            }

            foreach (KeyValuePair<String, String> s in Engine.Variables)
            {
                Variables[s.Key] = s.Value;
            }

            ExecuteMethod(".Main");

            if (Disposes)
            {
                Dispose();
            }
        }

        private void ExecuteMethod(String Name)
        {
            List<String> CurrentMethod = new List<String>();

            if (!_Run)
            {
                return;
            }

            if (Engine.Debug("EXECUTE"))
            {
                Console.WriteLine("EXECUTE " + Name);
            }

            int Line = -1;

            for (int i = Methods[Name][0] + 1; i < Methods[Name][1]; i++)
            {
                CurrentMethod.Add(Contents[i]);
            }

            foreach (String s in CurrentMethod)
            {
                List<String> Instructions = new List<String>();
                String Type = null;
                bool isSet = false;

                Line++;

                if (s.Contains(Invoke))
                {
                    if (Instructions != null)
                    {
                        Instructions.Clear();
                    }
                    Instructions.AddRange(s.Split(Invoke));
                }
                if (s.Contains(TypeSet))
                {
                    List<String> s2 = new List<String>();

                    foreach (String s3 in Instructions)
                    {
                        s2.Add(s3);
                    }
                    if (Instructions != null)
                    {
                        Instructions.Clear();
                    }
                    if (s2.ToArray().Length == 0)
                    {
                        s2.Add(s);
                    }

                    foreach (String s4 in s2)
                    {
                        Instructions.AddRange(s4.Split(TypeSet));
                    }

                    foreach (String s5 in s2)
                    {
                        if (s5.Split(TypeSet)[0].Equals(StringPrefix))
                        {
                            Type = StringPrefix;
                        }
                    }
                }
                if (s.Contains(Set))
                {
                    List<String> s6 = new List<String>();

                    foreach (String s7 in Instructions)
                    {
                        s6.Add(s7);
                    }
                    if (Instructions != null)
                    {
                        Instructions.Clear();
                    }
                    if (s6.ToArray().Length == 0)
                    {
                        s6.Add(s);
                    }

                    foreach (String s8 in s6)
                    {
                        Instructions.AddRange(s8.Split(Set));
                    }

                    isSet = true;
                }

                int Word = -1;

                foreach (String s9 in Instructions)
                {
                    Word++;

                    if (!_Run)
                    {
                        if (Engine.Debug("RETURN"))
                        {
                            Console.WriteLine("RETURN " + Name);
                        }

                        break;
                    }

                    if (s9.Equals("call"))
                    {
                        ParseCall(Instructions[1]);
                    }

                    if (isSet && Type != null && Type.Equals(StringPrefix) && Instructions[Word].StartsWith("\""))
                    {
                        Variables[Instructions[Word - 1]] = ParseInfo(Instructions[Word].Split(StringDef)[1], false);

                        if (Engine.Debug("VARIABLE"))
                        {
                            Console.WriteLine("VARIABLE " + Instructions[Word - 1]);
                        }
                    }
                }
            }

            if (Engine.Debug("RESOLVE"))
            {
                Console.WriteLine("RESOLVE " + Name);
            }

            String[] Info = Name.Split('(', ')');
            List<String> Keys = new List<String>();

            foreach (KeyValuePair<String, object> Key in Variables)
            {
                if (Key.Key.StartsWith(Info[0] + Parameter))
                {
                    Keys.Add(Key.Key);
                }
            }

            foreach (String s10 in Keys)
            {
                Variables.Remove(s10);

                if (Engine.Debug("CLEARED"))
                {
                    Console.WriteLine("CLEARED " + s10);
                }
            }
        }

        private void DiscoverMethods()
        {
            int Count = -1;
            int num = 0;
            String Name = "";
            String[] contents = Contents;

            for (int i = 0; i < contents.Length; i++)
            {
                String s = contents[i];
                Count++;
                if (s.StartsWith(".") && s.EndsWith(":"))
                {
                    num = Count;
                    Name = s.Split(new char[]
					{
						':'
					})[0];
                }

                if (s.Equals(":" + Name))
                {
                    Methods[Name] = new int[]
					{
						num,
						Count
					};
                }
            }

            if (Engine.Debug("DISCOVERED"))
            {
                foreach (KeyValuePair<String, int[]> Entry in Methods)
                {
                    Console.WriteLine("DISCOVERED " + Entry.Key);
                }
            }
        }

        public void ParseCall(String Method)
        {
            if (Method.StartsWith("**"))
            {
                return;
            }

            if (!_Run)
            {
                return;
            }

            String[] Info = Method.Split('(', ')');
            String[] Data = Info[1].Split(ParamSeperator);

            if (Engine.Debug("CALL"))
            {
                Console.WriteLine("CALL " + Method);
            }

            int Counter = 0;

            if (Methods.ContainsKey(Method.Split('(', ')')[0]))
            {
                String[] array = Data;

                for (int i = 0; i < array.Length; i++)
                {
                    String s = array[i];
                    Counter++;
                    Variables[Info[0] + "%Param" + Counter] = ParseInfo(s, true);

                    if (Engine.Debug("PARAMETER"))
                    {
                        Console.WriteLine(String.Concat("PARAMETER " + Info[0] + "%Param" + Counter));
                    }
                }

                ExecuteMethod(Info[0]);
                return;
            }

            if (EngineMethods.ContainsKey(Info[0]))
            {
                List<String> Params = new List<String>();

                foreach (String s in Data)
                {
                    Params.Add(ParseInfo(s, true));
                }

                EngineMethods[Info[0]].Invoke(Params.ToArray(), this);
                return;
            }

            #region Methods
            if (Method.StartsWith(".if("))
            {
                int Count = -1;
                String Obj = null;
                String Operator = null;
                String Obj2 = null;
                String CallTrue = null;
                String CallFalse = null;
                String[] array3 = Data;

                for (int k = 0; k < array3.Length; k++)
                {
                    String s3 = array3[k];
                    Count++;

                    if (Count == 0)
                    {
                        Obj = ParseInfo(s3, true);
                    }
                    if (Count == 1)
                    {
                        Operator = ParseInfo(s3, false);
                    }
                    if (Count == 2)
                    {
                        Obj2 = ParseInfo(s3, true);
                    }
                    if (Count == 3)
                    {
                        CallTrue = ParseInfo(s3, false);
                    }
                    if (Count == 4)
                    {
                        CallFalse = ParseInfo(s3, false);
                    }
                }

                if (Operator.Equals(Variables["EQ"]))
                {
                    if (Obj.Equals(Obj2))
                    {
                        ExecuteMethod(CallTrue);
                    }
                    else
                    {
                        ExecuteMethod(CallFalse);
                    }
                }

                if (Operator.Equals(Variables["GT"]))
                {
                    if (Convert.ToInt32(Obj) > Convert.ToInt32(Obj2))
                    {
                        ExecuteMethod(CallTrue);
                    }
                    else
                    {
                        ExecuteMethod(CallFalse);
                    }
                }

                if (Operator.Equals(Variables["LT"]))
                {
                    if (Convert.ToInt32(Obj) < Convert.ToInt32(Obj2))
                    {
                        ExecuteMethod(CallTrue);
                    }
                    else
                    {
                        ExecuteMethod(CallFalse);
                    }
                }

                if (Operator.Equals(Variables["NE"]))
                {
                    if (Convert.ToInt32(Obj) != Convert.ToInt32(Obj2))
                    {
                        ExecuteMethod(CallTrue);
                    }
                    else
                    {
                        ExecuteMethod(CallFalse);
                    }
                }
            }

            if (Method.StartsWith(".input("))
            {
                String Value = ParseInfo(Data[0], false);
                if (Variables.ContainsKey(Value))
                {
                    Variables[Value] = Console.ReadLine();
                }
            }

            if (Method.StartsWith(".maths("))
            {
                bool isDouble = false;

                if (Data.Length == 5 && Data[4].Equals("double"))
                {
                    isDouble = true;
                }

                double Num = Convert.ToDouble(ParseInfo(Data[0], true));
                double Num2 = Convert.ToDouble(ParseInfo(Data[2], true));

                String Operator2 = ParseInfo(Data[1], false);
                String Variable = ParseInfo(Data[3], false);

                if (Operator2.Equals(Variables["ADD"]))
                {
                    if (isDouble)
                    {
                        Variables[Variable] = (Num + Num2).ToString();
                    }
                    else
                    {
                        Variables[Variable] = ((int)(Num + Num2)).ToString();
                    }
                }

                if (Operator2.Equals(Variables["SUB"]))
                {
                    if (isDouble)
                    {
                        Variables[Variable] = (Num - Num2).ToString();
                    }
                    else
                    {
                        Variables[Variable] = ((int)(Num - Num2)).ToString();
                    }
                }

                if (Operator2.Equals(Variables["MUL"]))
                {
                    if (isDouble)
                    {
                        Variables[Variable] = (Num * Num2).ToString();
                    }
                    else
                    {
                        Variables[Variable] = ((int)(Num * Num2)).ToString();
                    }
                }

                if (Operator2.Equals(Variables["DIV"]))
                {
                    if (isDouble)
                    {
                        Variables[Variable] = (Num / Num2).ToString();
                    }
                    else
                    {
                        Variables[Variable] = ((int)(Num / Num2)).ToString();
                    }
                }
            }

            if (Method.StartsWith(".readf("))
            {
                String FileName = ParseInfo(Data[0], true);
                String Variable2 = ParseInfo(Data[1], false);

                Variables[Variable2] = File.ReadAllText(FileName);
            }

            if (Method.StartsWith(".writef("))
            {
                String FileName2 = ParseInfo(Data[0], true);
                String Contents = ParseInfo(Data[1], true);

                File.WriteAllText(FileName2, Contents);
            }

            if (Method.StartsWith(".set("))
            {
                Variables[ParseInfo(Data[0], true)] = ParseInfo(Data[1], true);
            }

            if (Method.StartsWith(".while("))
            {
                if (ParseInfo(Data[1], true).Equals(Variables["LT"]))
                {
                    while (Convert.ToInt32(ParseInfo(Data[0], true)) < Convert.ToInt32(ParseInfo(Data[2], true)))
                    {
                        ExecuteMethod(ParseInfo(Data[3], true));
                    }
                }

                if (ParseInfo(Data[1], false).Equals(Variables["GT"]))
                {
                    while (Convert.ToInt32(ParseInfo(Data[0], true)) > Convert.ToInt32(ParseInfo(Data[2], true)))
                    {
                        ExecuteMethod(ParseInfo(Data[3], true));
                    }
                }

                if (ParseInfo(Data[1], false).Equals(Variables["NE"]))
                {
                    while (!ParseInfo(Data[0], true).Equals(ParseInfo(Data[2], true)))
                    {
                        ExecuteMethod(ParseInfo(Data[3], true));
                    }
                }
            }

            if (Method.StartsWith(".toUpper("))
            {
                Variables[ParseInfo(Data[1], true)] = ParseInfo(Data[0], true).ToUpper();
            }

            if (Method.StartsWith(".toLower("))
            {
                Variables[ParseInfo(Data[1], true)] = ParseInfo(Data[0], true).ToLower();
            }

            if (Method.StartsWith(".combine("))
            {
                StringBuilder Builder = new StringBuilder();

                if (Data.Length == 4)
                {
                    if (ParseInfo(Data[3], false).Equals(Variables["TRU"]))
                    {
                        Builder.Append(ParseInfo(Data[0], true));
                        Builder.Append(' ');
                        Builder.Append(ParseInfo(Data[1], true));
                        Variables[ParseInfo(Data[2], true)] = Builder.ToString();
                    }

                    return;
                }

                Builder.Append(ParseInfo(Data[0], true));
                Builder.Append(ParseInfo(Data[1], true));

                Variables[Data[2]] = Builder.ToString();
            }

            if (Method.StartsWith(".log(") && Engine.Log != null)
            {
                Engine.Log.Write(FilePath, ParseInfo(Data[0], true), true);
            }

            if (Method.StartsWith(".confRead(") && Engine.Config != null)
            {
                Variables[ParseInfo(Data[1], false)] = Engine.Config.Read<String>(Data[0]);
            }

            if (Method.StartsWith(".wait("))
            {
                Thread.Sleep(Convert.ToInt32(ParseInfo(Data[0], true)));
            }

            if (Method.StartsWith(".getDebugStatus("))
            {
                if (Engine.DebugInfo[ParseInfo(Data[0], true)])
                {
                    Variables[ParseInfo(Data[1], true)] = Variables["TRU"];
                }
                else
                {
                    Variables[ParseInfo(Data[1], true)] = Variables["FAL"];
                }
            }

            if (Method.StartsWith(".rand("))
            {
                Random Rand = new Random();

                if (Data.Length == 1)
                {
                    Variables[ParseInfo(Data[0], true)] = Convert.ToString(Rand.Next());
                    return;
                }

                if (Data.Length == 2)
                {
                    Variables[ParseInfo(Data[0], true)] = Convert.ToString(Rand.Next(Convert.ToInt32(ParseInfo(Data[1], true))));
                    return;
                }

                if (Data.Length == 3)
                {
                    Variables[ParseInfo(Data[0], true)] = Convert.ToString(Rand.Next(Convert.ToInt32(ParseInfo(Data[1], true)), Convert.ToInt32(ParseInfo(Data[2], true))));
                    return;
                }
            }

            if (Method.StartsWith(".return("))
            {
                _Run = false;
                return;
            }
            #endregion
        }

        private String ParseInfo(String Info, bool StripSpeech = false)
        {
            if (Info.StartsWith(VariableValue))
            {
                Info = Convert.ToString(Variables[Info.Split(VariableValue.ToCharArray())[1]]);
                return Info;
            }

            if (Info.StartsWith(DontFilter))
            {
                return Info.Split(DontFilter.ToCharArray())[1];
            }

            if (Variables.ContainsKey(Info))
            {
                return Convert.ToString(Variables[Info]);
            }

            if (!StripSpeech)
            {
                return Info;
            }

            String[] Data = Info.Split(StringDef);

            if (Data.Length > 1)
            {
                return Data[1];
            }

            return Info;
        }

        public void Dispose()
        {
            if (Engine.Debug("END"))
            {
                Console.WriteLine("END " + FilePath);
            }

            Contents = null;
            Variables = null;
            Methods = null;
            Engine = null;
            FilePath = null;
        }
    }
}