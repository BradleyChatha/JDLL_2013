#define Debug

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Numerics;

namespace JDLL.JMaths
{
    enum TYPE
    {
        Int,
        Dec,
        Var
    }

    class Variable
    {
        public Decimal DecValue = 0.0m;
        public BigInteger IntValue = 0;
        public TYPE Type = TYPE.Int;
        public String Name = "";

        public void Set(Decimal value)
        {
            if (Type == TYPE.Int)
            {
                IntValue = Convert.ToInt64(value);
            }
            else
            {
                DecValue = value;
            }
        }
    }

    class JMathsRunner
    {
        public String FilePath { get; private set; }

        private Dictionary<String, Variable> Variables = new Dictionary<String, Variable>();

        public JMathsRunner(String filePath)
        {
            this.FilePath = filePath;
        }

        public void Execute()
        {
            using (FileStream fs = new FileStream(this.FilePath, FileMode.Open))
            {
                using (BinaryReader br = new BinaryReader(fs))
                {
                    String Magics = "";
                    Magics += br.ReadChar();
                    Magics += br.ReadChar();
                    Magics += br.ReadChar();

                    if (!Magics.Equals("%JM"))
                    {
                        throw new Exception("Not a valid JMaths file, missing magic header!");
                    }

                    ushort Op = 0x000000; // Current OP code

                    while (br.PeekChar() != -1) // Keep reading
                    {
                        Op = br.ReadUInt16(); // Get the current Opcode

                        #region Variable Stuff
                        if (Op == Opcodes.Var) // If we're making a Variable
                        {
                            String Name = "$" + br.ReadString(); // Get it's name first
                            TYPE Type = GetByValue(br.ReadUInt16()); // Then it's type
                            decimal Dec = 0.0m;
                            long Int = 0; // Dummy values

                            // Set the appropriate value to the value of the type
                            if (Type == TYPE.Dec) 
                            {
                                Dec = br.ReadDecimal();
                            }
                            else if (Type == TYPE.Int)
                            {
                                Int = br.ReadInt64();
                            }

                            Variables[Name] = new Variable() { Type = Type, DecValue = Dec, IntValue = Int, Name = Name }; // Add the variable
                        }
                        #endregion

                        #region Method Stuff

                        #region Add
                        if (Op == Opcodes.Add)
                        {
                            TYPE Num1Type = GetByValue(br.ReadUInt16());
                            Decimal Dec1 = 0.0m;
                            long Int1 = 0;
                            Variable Var1 = null;

                            if (Num1Type == TYPE.Int)
                            {
                                Int1 = Convert.ToInt64(br.ReadString());
                            }
                            else if (Num1Type == TYPE.Dec)
                            {
                                Dec1 = Convert.ToDecimal(br.ReadString());
                            }
                            else
                            {
                                Var1 = Variables[br.ReadString()];
                            }

                            TYPE Num2Type = GetByValue(br.ReadUInt16());
                            Decimal Dec2 = 0.0m;
                            long Int2 = 0;
                            Variable Var2 = null;

                            if (Num2Type == TYPE.Int)
                            {
                                Int2 = Convert.ToInt64(br.ReadString());
                            }
                            else if (Num1Type == TYPE.Dec)
                            {
                                Dec2 = Convert.ToDecimal(br.ReadString());
                            }
                            else
                            {
                                Var2 = Variables[br.ReadString()];
                            }

                            Variable ToStore = Variables[br.ReadString()];
                            Decimal Temp = 0.0m;

                            if (Num1Type == TYPE.Dec)
                            {
                                Temp += Dec1;
                            }
                            else if (Num1Type == TYPE.Int)
                            {
                                Temp += Int1;
                            }
                            else
                            {
                                if (Var1.Type == TYPE.Int)
                                {
                                    Temp += Convert.ToInt64(Var1.IntValue.ToString());
                                }
                                else
                                {
                                    Temp += Var1.DecValue;
                                }
                            }

                            if (Num2Type == TYPE.Dec)
                            {
                                Temp += Dec2;
                            }
                            else if (Num2Type == TYPE.Int)
                            {
                                Temp += Int2;
                            }
                            else
                            {
                                if (Var2.Type == TYPE.Int)
                                {
                                    Temp += Convert.ToInt64(Var2.IntValue.ToString());
                                }
                                else
                                {
                                    Temp += Var2.DecValue;
                                }
                            }

                            ToStore.Set(Temp);
                        }
                        #endregion

                        #endregion
                    }
                }
            }

#if Debug
            List<String> Contents = new List<String>();

            foreach (Variable Vari in Variables.Values)
            {
                Contents.Add("N:" + Vari.Name + " D:" + Vari.DecValue + " I:" + Vari.IntValue + " T:" + Vari.Type);
            }

            File.WriteAllLines("Debug.txt", Contents.ToArray());
#endif
        }

        private TYPE GetByValue(ushort value)
        {
            if (value == Opcodes.VarDec)
            {
                return TYPE.Dec;
            }
            else if (value == Opcodes.VarInt)
            {
                return TYPE.Int;
            }
            else if (value == Opcodes.Var)
            {
                return TYPE.Var;
            }

            return TYPE.Int;
        }
    }
}
