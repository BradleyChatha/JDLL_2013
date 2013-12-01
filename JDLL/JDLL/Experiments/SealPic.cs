using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing.Imaging;
using System.Drawing;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JDLL.Experiments
{
    public class SealPic
    {
        public void Compile(String Source, String Output)
        {
            String[] Contents = File.ReadAllLines(Source);

            bool Setting = false;

            using (FileStream fs = new FileStream(Output, FileMode.Create))
            {
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    bw.Write('S');
                    bw.Write('P');
                    bw.Write('C');

                    foreach (String s in Contents)
                    {
                        String[] Data = s.Split(new char[] { ' ' }, 2);

                        if (!Setting)
                        {
                            if (Data[0].Equals("0x01")) // Width
                            {
                                bw.Write((ushort)0x01);
                                bw.Write(Convert.ToInt32(Data[1]));
                            }

                            if (Data[0].Equals("0x02")) // Height
                            {
                                bw.Write((ushort)0x02);
                                bw.Write(Convert.ToInt32(Data[1]));
                            }

                            if (Data[0].Equals("0x03")) // Name
                            {
                                bw.Write((ushort)0x03);
                                bw.Write(Data[1]);
                            }
                        }
                        else
                        {
                            String[] Parse1 = s.Split(':');

                            foreach (String s2 in Parse1)
                            {
                                String[] Parse2 = s2.Trim().Split(' ');

                                if (String.IsNullOrEmpty(s2) || String.IsNullOrWhiteSpace(s2))
                                {
                                    continue;
                                }

                                bw.Write((ushort)0x05); // Write that we're adding in a pixel
                                if (!Parse2[0].Equals("}"))
                                {
                                    bw.Write(Convert.ToInt32(Parse2[0]));
                                    bw.Write(Convert.ToInt32(Parse2[1]));
                                    bw.Write(Convert.ToInt32(Parse2[2]));
                                }
                            }
                        }

                        if (Data[0].Equals("{"))
                        {
                            Setting = true;
                        }
                        else if (Data[0].Equals("}"))
                        {
                            Setting = false;
                            return;
                        }
                    }
                }
            }
        }

        public void ToSealPic(String source, String output, String name)
        {
            Bitmap bm = new Bitmap(source);

            List<String> Contents = new List<String>();

            Contents.Add("0x01 " + bm.Width);
            Contents.Add("0x02 " + bm.Height);
            Contents.Add("0x03 " + name);
            Contents.Add("{");

            String toAppend = "";

            for (int y = 0; y < bm.Height; y++)
            {
                for (int x = 0; x < bm.Width; x++)
                {
                    Color Current = bm.GetPixel(x, y);

                    toAppend += Current.R + " " + Current.G + " " + Current.B + " : ";
                }

                toAppend.Trim(new char[] { ' ', ':' });
                Contents.Add(toAppend);

                toAppend = "";
            }

            Contents.Add("}");

            File.WriteAllLines(output, Contents.ToArray());

            bm = null;
            Contents = null;
        }

        public Image GetImage(String file)
        {
            return GetBitmap(file) as Image;
        }

        public Bitmap GetBitmap(String file)
        {
            int Width = 0;
            int Height = 0;
            String Name = "";

            Bitmap toReturn;

            using (FileStream fs = new FileStream(file, FileMode.Open))
            {
                using (BinaryReader br = new BinaryReader(fs))
                {
                    bool Clear = false;

                    String Magic = "";

                    Magic += br.ReadChar();                    
                    Magic += br.ReadChar();                    
                    Magic += br.ReadChar();

                    if(!Magic.Equals("SPC"))
                    {
                        throw new Exception("Missing Magic Header");
                    }

                    while (!Clear)
                    {
                        ushort HeaderData = br.ReadUInt16();

                        if (HeaderData == 0x01)
                        {
                            Width = br.ReadInt32();
                        }

                        if (HeaderData == 0x02)
                        {
                            Height = br.ReadInt32();
                        }

                        if (HeaderData == 0x03)
                        {
                            Name = br.ReadString();
                            Clear = true;
                        }
                    }
                    toReturn = new Bitmap(Width, Height);


                    int X = 0;
                    int Y = 0;

                    int R = 0;
                    int B = 0;
                    int G = 0;

                    ushort Current = 0;

                    while (br.PeekChar() != -1)
                    {
                        if (Y == Height)
                        {
                            break;
                        }

                        Current = br.ReadUInt16();

                        if (Current == 0x05)
                        {
                            R = br.ReadInt32();
                            G = br.ReadInt32();
                            B = br.ReadInt32();

                            toReturn.SetPixel(X, Y, Color.FromArgb(R, G, B));

                            if (X == Width - 1)
                            {
                                X = 0;
                                Y++;
                            }
                            else
                            {
                                X++;
                            }
                        }
                    }

                    return toReturn;
                }
            }
        }
    }
}
