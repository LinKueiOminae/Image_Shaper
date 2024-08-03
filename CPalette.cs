using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;

namespace ImageShaper
{
    public class PaletteColor
    {
        public Color Color;
        public bool IsUsed = true;
        public bool MakeTransparent = false;

        public PaletteColor(Color c)
        {
            this.Color = c;
            this.IsUsed = true;
            this.MakeTransparent = false;
        }

        public PaletteColor(Color c, bool IsUsed, bool MakeTransparent)
        {
            this.Color = c;
            this.IsUsed = IsUsed;
            this.MakeTransparent = MakeTransparent;
        }

        public static bool operator ==(PaletteColor col1, PaletteColor col2)
        {
            if (object.ReferenceEquals(col1, null))
            {
                return object.ReferenceEquals(col2, null);
            }
            if (col2 == null) return false;

            return (col1.Color == col2.Color) && (col1.IsUsed == col2.IsUsed) && (col1.MakeTransparent == col2.MakeTransparent);
        }

        // this is second one '!='
        public static bool operator !=(PaletteColor col1, PaletteColor col2)
        {
            return !(col1 == col2);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
    }

    public static class PaletteManager
    {
        public static CPalette[] Palettes
        {
            get { return UsedPalettes.ToArray(); }
        }
        private static List<CPalette> UsedPalettes = new List<CPalette>();
        /// <summary>
        /// returns the palette if it exists already, otherwise creates a new palette
        /// </summary>
        /// <param name="pal"></param>
        /// <returns></returns>
        internal static CPalette GetPalette(CPalette pal)
        {
            return GetPalette(pal, false);
        }

        /// <summary>
        /// returns the palette if it exists already, otherwise creates a new palette
        /// </summary>
        public static CPalette GetPalette(int index)
        {
            if ((index != -1) && (index < Palettes.Length)) return Palettes[index];
            return null;
        }

        /// <summary>
        /// returns the palette if it exists already, otherwise creates a new palette
        /// </summary>
        internal static CPalette GetPalette(CPalette pal, bool addDuplicate)
        {
            int index = 0;
            for (int i = 0; i < UsedPalettes.Count; i++)
            {
                if ((UsedPalettes[i] != null) && (pal != null))
                {
                    if (UsedPalettes[i].PaletteFile == pal.PaletteFile) index++;
                    if ((UsedPalettes[i] == pal) && (!addDuplicate))
                        return UsedPalettes[i];
                }
                else break;
            }
            if (pal != null)
            {
                if (addDuplicate)
                    pal = pal.GetClone();

                if (index > 0)
                    pal.PaletteName = pal.PaletteName + "_" + index.ToString();
                UsedPalettes.Add(pal);
            }
            return pal;
        }

        internal static int GetPaletteIndex(CPalette pal, bool compareFileNameOnly)
        {
            int index = -1;
            if (pal!=null)
                for (int i = 0; i < UsedPalettes.Count; i++)
                {
                    if ((UsedPalettes[i] != null) && (pal != null))
                    {
                        if ((compareFileNameOnly) && (UsedPalettes[i].PaletteFile == pal.PaletteFile)) return index;
                        if (!compareFileNameOnly && (UsedPalettes[i] == pal)) return i;
                    }
                }
            return index;
        }

        internal static void Clear(CPalette pal)
        {
            for (int i = UsedPalettes.Count - 1; i >= 0; i--)
                if (UsedPalettes[i] == pal)
                    UsedPalettes.RemoveAt(i);
        }
        internal static void ClearAll()
        {
            UsedPalettes.Clear();
        }
    }

    public class CPalette
    {
        public ColorConversionMethod ConversionMethod;

        public CPalette GetClone()
        {
            CPalette clone = new CPalette();
            clone.palette = new PaletteColor[this.palette.Length];
            for (int i = 0; i < this.palette.Length; i++)
                clone.palette[i] = new PaletteColor(this.palette[i].Color, this.palette[i].IsUsed, this.palette[i].MakeTransparent);

            clone.ConversionMethod = this.ConversionMethod;
            clone.palfilename = this.palfilename;
            clone.PaletteName = Path.GetFileNameWithoutExtension(clone.PaletteFile);
            return clone;
        }

        public CPalette()
        {
            palette = new PaletteColor[256];
            for (int i = 0; i < 256; i++) palette[i] = new PaletteColor(Color.FromArgb(255, i, i, i));
            PaletteName = "Null";
        }

        public CPalette(string filename)
        {
            Load(filename);
        }
        
        public static Color GetColor(CPalette p, int index)
        {
            if ((p != null) && (p.palette != null) && (index >= 0) && (index <= p.palette.Length))
                return p.palette[index].Color;
            return Color.FromArgb(255, 0, 0, 0);
        }

        /// <summary>
        /// returns the index of the Palette p color, that is matching closest the input Color c
        /// </summary>
        public static int NearestColorDeltaE2000(CPalette p, Color c)
        {
            int nearestcolor = 0;
            double nearestcolorDeltaC = double.MaxValue;
            if ((p != null)
             && (p.palette != null))
            {
                for (int i = 0; i < p.palette.Length; i++)
                {
                    if (p.palette[i].IsUsed)
                    {
                        if (c.A == 0)
                        {
                            nearestcolor = 0;
                            break;
                        }

                        Color col2 = p.palette[i].Color;
                        double dC = Tools.getDeltaE2000_viaCIELab(c, col2);
                        if (dC < nearestcolorDeltaC)
                        {
                            nearestcolor = i;
                            nearestcolorDeltaC = dC;
                        }
                    }
                }
            }
            if (p.palette[nearestcolor].MakeTransparent) nearestcolor = 0;
            return nearestcolor;
        }

        /// <summary>
        /// produces complete crap
        /// </summary>
        public static int NearestColorHSB(CPalette p, Color c)
        {
            int nearestcolor = 0;
            double distance = double.MaxValue;

            float h,s,v;
            h = c.GetHue();
            s = c.GetSaturation();
            v = c.GetBrightness();

            if ((p != null)
             && (p.palette != null))
            {
                for (int i = 0; i < p.palette.Length; i++)
                {
                    if (!p.palette[i].IsUsed) continue;
                    if (c.A == 0)
                    {
                        nearestcolor = 0;
                        break;
                    }
                    float ph, ps, pv;
                    ph = p.palette[i].Color.GetHue();
                    ps = p.palette[i].Color.GetSaturation();
                    pv = p.palette[i].Color.GetBrightness();

                    double dH = (ph - h) / 360;
                    if (dH > 0.5) dH = 1.0 - dH;

                    double dS = ps - s;
                    double dV = pv - v;

                    double cur_dist = Math.Sqrt(0.8 * Math.Pow(dH, 2) + 0.1 * Math.Pow(dS, 2) + 0.1 * Math.Pow(dV, 2));
                    if (cur_dist == 0)
                    {
                        nearestcolor = i;
                        break;
                    }
                    else if (cur_dist < distance)
                    {
                        distance = cur_dist;
                        nearestcolor = i;
                    }
                }
            }
            return nearestcolor;
        }

        public static int NearestColorEuclidean(CPalette p, Color c)
        {
            int nearestcolor = 0;
            double distance = double.MaxValue;

            if ((p != null)
             && (p.palette != null))
            {
                for (int i = 0; i < p.palette.Length; i++)
                {
                    if (p.palette[i].IsUsed)
                    {
                        if (c.A == 0)
                        {
                            nearestcolor = 0;
                            break;
                        }
                        double dr = (p.palette[i].Color.R + c.R) / 2;
                        int test_red = (int)Math.Pow(p.palette[i].Color.R - c.R, 2.0);
                        int test_green = (int)Math.Pow(p.palette[i].Color.G - c.G, 2.0);
                        int test_blue = (int)Math.Pow(p.palette[i].Color.B - c.B, 2.0);
                        int test_alpha = (int)Math.Pow(Math.Abs(p.palette[i].Color.A - c.A), 2.5);//alpha channel difference is more prominent than others

                        double cur_dist = (2 + dr / 256) * test_red + 4 * test_green + (2 + (255 - dr) / 256) * test_blue + test_alpha;

                        //int cur_dist = 2 * test_red + 4 * test_green + 3 * test_blue;
                        if (cur_dist == 0)
                        {
                            nearestcolor = i;
                            break;
                        }
                        else if (cur_dist < distance)
                        {
                            distance = cur_dist;
                            nearestcolor = i;
                        }
                    }
                }
            }
            if (p.palette[nearestcolor].MakeTransparent) nearestcolor = 0;
            return nearestcolor;
        }

        public PaletteColor[] palette;
        public string PaletteFile { get { return palfilename; } }
        private string palfilename;

        public string PaletteName { get; set; }

        public override string ToString()
        {
            return PaletteName;
        }

        public System.Drawing.Imaging.ColorPalette GetAsColorPalette()
        {
            Bitmap bmp = new Bitmap(1, 1, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
            System.Drawing.Imaging.ColorPalette pal = bmp.Palette;

            for (int i = 0; i < 256; i++)
                if (palette != null)
                    pal.Entries[i] = palette[i].Color;
                else
                    pal.Entries[i] = Color.FromArgb(255, i, i, i);

            return pal;
        }

        public void Save(string filename, int PalFormat)
        {
            switch (PalFormat)
            {
                case 1: SaveTSPal(filename); break;
                case 2: SaveJASCPal(filename); break;
                case 3: Cinimanager.SavePaletteSetup(filename, new CPalette[] { this }); break;
            }
        }

        private void SaveTSPal(string filename)
        {
            System.IO.File.Delete(filename);
            using (FileStream fs = new FileStream(filename, FileMode.OpenOrCreate))
            {
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    for (int i = 0; i < 256; i++)
                    {
                        byte r = (byte)(palette[i].Color.R / 4);
                        byte g = (byte)(palette[i].Color.G / 4);
                        byte b = (byte)(palette[i].Color.B / 4);
                        bw.Write(r);
                        bw.Write(g);
                        bw.Write(b);
                    }
                }
            }
        }

        private void SaveJASCPal(string filename)
        {
            System.IO.File.Delete(filename);
            using (FileStream fs = new FileStream(filename, FileMode.OpenOrCreate))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine("JASC-PAL");
                    sw.WriteLine(palette.Length.ToString("X4"));
                    sw.WriteLine(palette.Length.ToString());

                    for (int i = 0; i < palette.Length; i++)
                        sw.WriteLine(palette[i].Color.R + " " + palette[i].Color.G + " " + palette[i].Color.B);  
                }
            }
        }


        private bool IsTSPal(string filename)
        {
            bool result = false;
            if (System.IO.File.Exists(filename))
            {
                FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
                result = fs.Length == 768;
                fs.Close();
            }
            return result;
        }

        private bool IsJASCPal(string filename)
        {
            bool result = false;
            if (System.IO.File.Exists(filename))
            {
                FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
                using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
                {
                    string line = sr.ReadLine();
                    if (line.ToUpper() == "JASC-PAL")
                        result = true;
                }
                fs.Close();
            }
            return result;
        }

        private bool IsPaletteSetup(string filename)
        {
            if ((System.IO.File.Exists(filename)) && (System.IO.Path.GetExtension(filename) == ".isps"))
                return true;
            return false;
        }

        public void Load(string filename)
        {
            this.palfilename = filename;
            this.PaletteName = Path.GetFileNameWithoutExtension(filename);
            palette = new PaletteColor[256];
            for (int i = 0; i < 256; i++) palette[i] = new PaletteColor(Color.FromArgb(255, i, i, i));

            if (IsTSPal(filename))
                LoadTSPal(filename);

            if (IsJASCPal(filename))
                LoadJASCPal(filename);

            if (IsPaletteSetup(filename))
            {
                CPalette tmp = Cinimanager.LoadPaletteSetup(filename)[0];
                this.palfilename = tmp.palfilename;
                this.PaletteName = tmp.PaletteName;
                this.palette = tmp.palette;
            }
        }

        private string removeDoubleSpace(string s)
        {
            s = s.Replace('\t', ' ');
            while (s.Contains("  "))
                s = s.Replace("  ", " ");

            return s;
        }

        private void LoadJASCPal(string filename)
        {
            if (System.IO.File.Exists(filename))
            {
                FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
                using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
                {
                    string line = "";
                    int index = 0;
                    while ((line = sr.ReadLine()) != null)
                    {
                        line = removeDoubleSpace(line);
                        string[] values = line.Split(' ');
                        if (values.Length >= 3) //maybe add later alpha value support
                        {
                            int a, r, g, b;
                            a = 255;
                            r = 0;
                            g = 0;
                            b = 0;
                            int.TryParse(values[0], out r);
                            int.TryParse(values[1], out g);
                            int.TryParse(values[2], out b);
                            if (values.Length == 4) int.TryParse(values[3], out a);
                            if (r > 255) r = 255;
                            if (g > 255) g = 255;
                            if (b > 255) b = 255;
                            if (a > 255) a = 255;
                            if (index < palette.Length)
                                palette[index].Color = Color.FromArgb(Math.Abs(a), Math.Abs(r), Math.Abs(g), Math.Abs(b));
                            else break;
                            index++;
                        }
                    }
                }
                fs.Close();
            }
        }

        private void LoadTSPal(string filename)
        {
            if (System.IO.File.Exists(filename))
            {
                FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                for (int i = 0; i < 256; i++)
                {
                    byte a = 0;
                    byte r = 0;
                    byte g = 0;
                    byte b = 0;
                    if (br.BaseStream.Position < br.BaseStream.Length)
                    {
                        a = 255;
                        r = br.ReadByte();
                        g = br.ReadByte();
                        b = br.ReadByte();
                        if ((r * 4) > 255) r = 255;
                        else r = (byte)(r * 4);
                        if ((g * 4) > 255) g = 255;
                        else g = (byte)(g * 4);
                        if ((b * 4) > 255) b = 255;
                        else b = (byte)(b * 4);
                    }
                    palette[i].Color = Color.FromArgb(a, r, g, b);
                }
                br.Close();
                fs.Close();
            }
        }

        public static bool operator ==(CPalette pal1, CPalette pal2)
        {
            if (object.ReferenceEquals(pal1, null))
            {
                return object.ReferenceEquals(pal2, null);
            }
            if (pal2 == null) return false;

            bool IsSame = (pal1.PaletteFile == pal2.PaletteFile)
                       && (pal1.palette.Length == pal2.palette.Length)
                       && (pal1.PaletteName == pal2.PaletteName);
            if (IsSame)
                for (int i = 0; i < pal1.palette.Length; i++)
                    if (pal1.palette[i] != pal2.palette[i]) return false;

            return IsSame;
        }

        // this is second one '!='
        public static bool operator !=(CPalette pal1, CPalette pal2)
        {
            return !(pal1 == pal2);
        }


        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
    }
}
