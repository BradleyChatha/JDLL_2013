using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Numerics;

namespace JDLL.JMaths
{
    class JMathsCompiler
    {
        public String FilePath { get; private set; }

        private String[] Contents;

        public JMathsCompiler(String filePath)
        {
            this.FilePath = filePath;
            this.Contents = File.ReadAllLines(this.FilePath);
        }

        public void Compile()
        {
            using (FileStream fs = new FileStream(Path.ChangeExtension(this.FilePath, ".j"), FileMode.Create))
            {
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    bw.Write('%');
                    bw.Write('J');
                    bw.Write('M');

                    foreach (String s in Contents)
                    {
                        if (s.StartsWith("var"))
                        {
                            ParseVariable(s, bw);
                        }
                        else if (s.Contains("("))
                        {
                            ParseMethod(s, bw);
                        }
                    }
                }
            }
        }

        private void ParseMethod(String methodLine, BinaryWriter bin)
        {
            // add(20, 50, $Value1)
            String[] Parse1 = methodLine.Trim(new char[] { ';' }).Split(new char[] { '(', ')' });
            List<String> Parse2 = new List<String>();
            String[] Parse3;

            for (int i = 0; i < Parse1.Length; i++)
            {
                Parse1[i] = Parse1[i].Trim(new char[] { ' ', ',' });
            }

            bool Meh = false;

            foreach (String s in Parse1)
            {
                if (!Meh)
                {
                    Meh = true;
                    continue;
                }

                Parse2.Add(s.Trim(new char[] { ' ', ',' }));
            }

            Parse3 = Parse2[0].Split(',');

            for (int i = 0; i < Parse3.Length; i++)
            {
                Parse3[i] = Parse3[i].Trim(new char[] { ' ', ',' });
            }

            if (Parse1[0].Equals("add"))
            {
                bin.Write(Opcodes.Add);

                WriteType(Parse3[0], bin);
                bin.Write(Parse3[0]);

                WriteType(Parse3[1], bin);
                bin.Write(Parse3[1]);

                bin.Write(Parse3[2]);
            }
        }

        private void WriteType(String toFind, BinaryWriter bin)
        {
            if (toFind.StartsWith("$"))
            {
                bin.Write(Opcodes.Var);
            }
            else if (toFind.Contains("."))
            {
                bin.Write(Opcodes.VarDec);
            }
            else
            {
                bin.Write(Opcodes.VarInt);
            }
        }

        private void ParseVariable(String varLine, BinaryWriter bin)
        {
            // var Test1 20, int;
            // var - Test1 - 20 - int
            String[] Parse1 = varLine.Trim().Trim(new char[] { ';' }).Split(' ');
            long Value = Convert.ToInt32(Convert.ToDecimal(Parse1[2].Trim(new char[] { ',' })));

            if (Parse1[3].ToLower().Equals("int"))
            {
                bin.Write(Opcodes.Var);
                bin.Write(Parse1[1]);
                bin.Write(Opcodes.VarInt);
                bin.Write(Value);
            }
            if (Parse1[3].ToLower().Equals("decimal"))
            {
                bin.Write(Opcodes.Var);
                bin.Write(Parse1[1]);
                bin.Write(Opcodes.VarDec);
                bin.Write(Convert.ToDecimal(Parse1[2].Trim(new char[] { ',' })));
            }
        }
    }
}
