using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Diagnostics;

using JDLL.Data.Logging;
using JDLL.Data;

namespace JDLL.SealScript
{
    public class Engine : IDisposable
    {
        public Log Log = null;
        public Config Config = null;
        public Dictionary<String, ScriptMethod> EngineMethods = new Dictionary<String, ScriptMethod>();
        public Dictionary<String, ScriptInstance> EngineScripts = new Dictionary<String, ScriptInstance>();
        public Dictionary<String, bool> DebugInfo = new Dictionary<String, bool>();

        internal Dictionary<String, String> Variables = new Dictionary<String, String>();

        public bool Debugs = true;

        public Engine()
        {
            #region Built in Variables
            Variables["EQ"] = "@EQUALSOPERATOR@";
            Variables["GT"] = "@GREATERTHENOPERATOR@";
            Variables["LT"] = "@LESSTHANOPERATOR@";
            Variables["NE"] = "@NOTEQUALTOOPERATOR@";
            Variables["ADD"] = "@ADDITIONOPERATOR@";
            Variables["MUL"] = "@MULTIPLYOPERATOR@";
            Variables["SUB"] = "@SUBTRACTOPERATOR@";
            Variables["DIV"] = "@DIVIDEROPERATOR@";
            Variables["TRU"] = "@TRUEBOOLOPERATOR@";
            Variables["FAL"] = "@FALSEBOOLOPERATOR@";
            #endregion

            #region Built in Methods
            AddMethod(new ScriptMethod(EngineDebug), "engineDebug");
            AddMethod(new ScriptMethod(RunScript), "runScript");
            AddMethod(new ScriptMethod(WriteToConfig), "confWrite");
            AddMethod(new ScriptMethod(ClearConsole), "outputclr");
            AddMethod(new ScriptMethod(OutputLine), "outputln");
            AddMethod(new ScriptMethod(Output), "output");
            AddMethod(new ScriptMethod(MsgBox), "msgbox");
            AddMethod(new ScriptMethod(RunExe), "runApp");
            AddMethod(new ScriptMethod(Speak), "speak");
            AddMethod(new ScriptMethod(EngineInternal), "engine");
            AddMethod(new ScriptMethod(DeleteFile), "deletef");
            AddMethod(new ScriptMethod(MoveFile), "movef");
            AddMethod(new ScriptMethod(SetDebug), "setDebugStatus");
            #endregion

            #region Debug
            DebugInfo["START"] = true;
            DebugInfo["VARIABLE"] = true;
            DebugInfo["EXECUTE"] = true;
            DebugInfo["RESOLVE"] = true;
            DebugInfo["CLEARED"] = true;
            DebugInfo["DISCOVERED"] = true;
            DebugInfo["CALL"] = true;
            DebugInfo["PARAMETER"] = true;
            DebugInfo["END"] = true;
            DebugInfo["RETURN"] = true;
            #endregion
        }

        public void Run(String FilePath, String[] Parameters = null)
        {
            if (!File.Exists(FilePath))
                return;

            ScriptInstance Instance = new ScriptInstance(FilePath, Parameters, this);
            Instance.Execute();
        }
        
        public void RunThreaded(String FilePath, String[] Parameters = null)
        {
            if (!File.Exists(FilePath))
                return;

            Thread s = new Thread(new ThreadStart(() =>
            {
                ScriptInstance Instance = new ScriptInstance((String)FilePath, (String[])Parameters, this);
                Instance.Execute();
            }));

            s.Start();
        }

        public void AddMethod(ScriptMethod Method, String Name)
        {
            if (!Name.StartsWith("."))
            {
                Name = "." + Name;
            }

            EngineMethods.Add(Name, Method);
        }

        #region Methods that can be outside the instance class
        void EngineDebug(String[] Parameters, ScriptInstance Sender = null)
        {
            if (Parameters[0].Equals(Variables["FAL"]))
            {
                Debugs = false;
            }

            if (Parameters[0].Equals(Variables["TRU"]))
            {
                Debugs = true;
            }
        }

        void RunScript(String[] Parameters, ScriptInstance Sender = null)
        {
            if (!Parameters[0].EndsWith(".seal"))
            {
                Parameters[0] += ".seal";
            }

            Run(Parameters[0]);
        }

        void WriteToConfig(String[] Parameters, ScriptInstance Sender = null)
        {
            if (Config != null)
            {
                if (!Config.DoesKeyExist(Parameters[0]))
                {
                    Config.WriteValue(Parameters[0], Parameters[1]);
                }
                else
                {
                    Config.ChangeValue(Parameters[0], Parameters[1]);
                }
            }
        }

        void ClearConsole(String[] Parameters, ScriptInstance Sender = null)
        {
            Console.Clear();
        }

        void OutputLine(String[] Parameters, ScriptInstance Sender = null)
        {
            Console.Write("\r\n");
        }

        void Output(String[] Parameters, ScriptInstance Sender = null)
        {
            if (Parameters.Length == 2)
            {
                if (Parameters[1].Equals(Variables["TRU"]))
                {
                    Console.Write(Parameters[0], true);
                    return;
                }
            }
            Console.WriteLine(Parameters[0], true);
        }

        void MsgBox(String[] Parameters, ScriptInstance Sender = null)
        {
            Win32.MessageBox(new IntPtr(0), Parameters[0], Parameters[1], 0);
        }

        void RunExe(String[] Parameters, ScriptInstance Sender = null)
        {
            Process.Start(Parameters[0]);
        }

        void Speak(String[] Parameters, ScriptInstance Sender = null)
        {
            if (Parameters.Length > 1)
            {
                Util.Volume = Convert.ToInt32(Parameters[1]);
            }

            Util.Speak(Parameters[0]);
        }

        // Loads/Unloads Scripts Also runs scripts, checks if script exists
        void EngineInternal(String[] Parameters, ScriptInstance Sender = null)
        {
            if (Parameters[0].ToLower().Equals("load"))
            {
                EngineScripts.Add(Parameters[2], new ScriptInstance(Parameters[1], null, this));
                EngineScripts[Parameters[2]].Execute(false);
            }

            if (Parameters[0].ToLower().Equals("unload"))
            {
                EngineScripts[Parameters[1]].Dispose();
                EngineScripts.Remove(Parameters[1]);
            }

            if (Parameters[0].ToLower().Equals("run"))
            {
                if (!Parameters[2].StartsWith("."))
                {
                    EngineScripts[Parameters[1]].ParseCall("." + Parameters[2] + "()");
                }
                else
                {
                    EngineScripts[Parameters[1]].ParseCall(Parameters[2] + "()");
                }
            }

            if (Parameters[0].ToLower().Equals("scriptexist"))
            {
                if (EngineScripts.ContainsKey(Parameters[1]))
                {
                    Sender.SetVariable(Parameters[2], Variables["TRU"]);
                }
                else
                {
                    Sender.SetVariable(Parameters[2], Variables["FAL"]);
                }
            }
        }

        void DeleteFile(String[] Parameters, ScriptInstance Sender = null)
        {
            File.Delete(Parameters[0]);
        }

        void MoveFile(String[] Parameters, ScriptInstance Sender = null)
        {
            File.Move(Parameters[0], Parameters[1]);
        }

        void SetDebug(String[] Parameters, ScriptInstance Sender = null)
        {
            if (DebugInfo.ContainsKey(Parameters[0]))
            {
                if (Parameters[1].Equals("TRU") || Parameters[1].Equals(Variables["TRU"]))
                {
                    DebugInfo[Parameters[0]] = true;
                }
                else
                {
                    DebugInfo[Parameters[0]] = false;
                }
            }
        }
        #endregion

        #region Debug
        public bool Debug(String Key)
        {
            return (DebugInfo[Key] && this.Debugs);
        }

        public void SetDebug(String Key, bool Value)
        {
            DebugInfo[Key.ToUpper()] = Value;
        }

        public void AddDebug(String Key, bool Value)
        {
            DebugInfo[Key] = Value;
        }

        public void RemoveDebug(String Key)
        {
            DebugInfo.Remove(Key);
        }
        #endregion

        public void Dispose()
        {
            if (Log != null)
            {
                Log = null;
            }

            if (Config != null)
            {
                Config = null;
            }
        }
    }
}
