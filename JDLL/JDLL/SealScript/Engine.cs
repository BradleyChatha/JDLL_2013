using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

using JDLL.Data.Logging;
using JDLL.Data;

namespace JDLL.SealScript
{
    public class Engine : IDisposable
    {
        public Log Log = null;
        public Config Config = null;
        public Dictionary<String, ScriptMethod> EngineMethods = new Dictionary<String, ScriptMethod>();

        internal Dictionary<String, String> Variables = new Dictionary<String, String>();

        public bool Debug = true;

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
        void EngineDebug(String[] Parameters)
        {
            if (Parameters[0].Equals(Variables["FAL"]))
            {
                Debug = false;
            }

            if (Parameters[0].Equals(Variables["TRU"]))
            {
                Debug = true;
            }
        }

        void RunScript(String[] Parameters)
        {
            if (!Parameters[0].EndsWith(".seal"))
            {
                Parameters[0] += ".seal";
            }

            Run(Parameters[0]);
        }

        void WriteToConfig(String[] Parameters)
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

        void ClearConsole(String[] Parameters)
        {
            Console.Clear();
        }

        void OutputLine(String[] Parameters)
        {
            Console.Write("\r\n");
        }

        void Output(String[] Parameters)
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

        void MsgBox(String[] Parameters)
        {
            Win32.MessageBox(new IntPtr(0), Parameters[0], Parameters[1], 0);
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
