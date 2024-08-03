using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using ImageShaper;
using System.Drawing;

public static class Cinimanager
{
    [DllImport("KERNEL32.DLL", EntryPoint = "GetPrivateProfileStringW",
      SetLastError = true,
      CharSet = CharSet.Unicode, ExactSpelling = true,
      CallingConvention = CallingConvention.StdCall)]
    private static extern int GetPrivateProfileString(
        string lpAppName,
        string lpKeyName,
        string lpDefault,
        string lpReturnString,
        int nSize,
        string lpFilename);

    [DllImport("KERNEL32.DLL", EntryPoint = "WritePrivateProfileStringW",
      SetLastError = true,
      CharSet = CharSet.Unicode, ExactSpelling = true,
      CallingConvention = CallingConvention.StdCall)]
    private static extern int WritePrivateProfileString(
        string lpAppName,
        string lpKeyName,
        string lpString,
        string lpFilename);

    public struct TSETTINGS
    {
        public SHP_TS_EncodingFormat DefaultCompression;
        public Point StartPosition;
        public Size StartSize;

        public bool ShowPreview;
        public string LastPalette;

        public bool PreventTSWobbleBug;
        public bool CreateImages;
        public string CreateImages_FileName;
        public string CreateImages_Format;

        public bool OptimizeCanvas;
        public bool KeepCentered;

        public Color RadarColor;
        public bool AverageRadarColor;

        public bool UseCustomBackgroundColor;
        public Color CustomBackgroundColor;
        public bool CombineTransparentPixel;

        public string OutputFolder;
        public string PreviewBackgroundImage;

        public string LastFireFLHFinderDirectory;
    }

    public static string inifilename = "ImageShaper.ini";

    public static TSETTINGS inisettings;

    private static void IniWriteValue(string Section, string Key, string Value)
    {
        WritePrivateProfileString(Section, Key, Value, inifilename);
    }

    private static string GetIniFileString(string inifilename, string category, string key, string defaultValue)
    {
        string returnString = new string(' ', 1024);
        GetPrivateProfileString(category, key, defaultValue, returnString, 1024, inifilename);
        return returnString.Split('\0')[0];
    }

    private static List<string> GetSections(string inifilename)
    {
        string returnString = new string(' ', 65536);
        GetPrivateProfileString(null, null, null, returnString, 65536, inifilename);
        char[] sep = { '\0' };
        List<string> result = new List<string>(returnString.Split(sep));
        result.RemoveRange(result.Count - 2, 2);
        return result;
    }

    private static void RemoveIniSection(string sectionname, string filename)
    {
        WritePrivateProfileString(sectionname, null, null, filename);
    }

    private static List<string> GetKeys(string inifilename, string category)
    {
        string returnString = new string(' ', 32768);
        GetPrivateProfileString(category, null, null, returnString, 32768, inifilename);
        List<string> result = new List<string>(returnString.Split('\0'));
        result.RemoveRange(result.Count() - 2, 2);
        return result;
    }

    public static int ClearIniFile(string filename)
    {
        if (System.IO.File.Exists(filename))
        {
            System.IO.File.Delete(filename);
            return 0;
        }
        else return 1;
    }

    public static int SaveIniSettings()
    {
        WritePrivateProfileString("General", "X_StartPosition", inisettings.StartPosition.X.ToString(), inifilename);
        WritePrivateProfileString("General", "Y_StartPosition", inisettings.StartPosition.Y.ToString(), inifilename);
        WritePrivateProfileString("General", "StartWidth", inisettings.StartSize.Width.ToString(), inifilename);
        WritePrivateProfileString("General", "StartHeight", inisettings.StartSize.Height.ToString(), inifilename);
        WritePrivateProfileString("General", "DefaultCompression", inisettings.DefaultCompression.ToString(), inifilename);
        WritePrivateProfileString("General", "ShowPreview", inisettings.ShowPreview.ToString(), inifilename);
        WritePrivateProfileString("General", "LastPalette", inisettings.LastPalette, inifilename);

        WritePrivateProfileString("General", "CreateImages", inisettings.CreateImages.ToString(), inifilename);
        WritePrivateProfileString("General", "CreateImages_FileName", inisettings.CreateImages_FileName, inifilename);
        WritePrivateProfileString("General", "CreateImages_Format", inisettings.CreateImages_Format, inifilename);
        WritePrivateProfileString("General", "PreventTSWobbleBug", inisettings.PreventTSWobbleBug.ToString(), inifilename);

        WritePrivateProfileString("General", "OptimizeCanvas", inisettings.OptimizeCanvas.ToString(), inifilename);
        WritePrivateProfileString("General", "KeepCentered", inisettings.KeepCentered.ToString(), inifilename);

        WritePrivateProfileString("General", "RadarColor", ColorToStr(inisettings.RadarColor, true), inifilename);
        WritePrivateProfileString("General", "AverageRadarColor", inisettings.AverageRadarColor.ToString(), inifilename);

        WritePrivateProfileString("General", "UseCustomBackgroundColor", inisettings.UseCustomBackgroundColor.ToString(), inifilename);
        WritePrivateProfileString("General", "CustomBackgroundColor", ColorToStr(inisettings.CustomBackgroundColor, true), inifilename);
        WritePrivateProfileString("General", "CombineTransparentPixel", inisettings.CombineTransparentPixel.ToString(), inifilename);

        WritePrivateProfileString("General", "OutputFolder", inisettings.OutputFolder, inifilename);
        WritePrivateProfileString("General", "PreviewBackgroundImage", inisettings.PreviewBackgroundImage, inifilename);

        WritePrivateProfileString("General", "LastFireFLHFinderDirectory", inisettings.LastFireFLHFinderDirectory, inifilename);
        
        if (System.IO.File.Exists(inifilename))
        {
            if ((System.IO.File.GetAttributes(inifilename) & System.IO.FileAttributes.ReadOnly) != System.IO.FileAttributes.ReadOnly)
            {
                return 0;//no problems
            }
            else return 2;//write protected
        }
        else return 1;//file exists
    }

    public static void LoadIniSettings()
    {
        inisettings = new TSETTINGS();
        inisettings.LastPalette = "";
        inisettings.ShowPreview = true;
        inisettings.DefaultCompression = SHP_TS_EncodingFormat.Detect_best_size;

        string returnString = new string(' ', 1024);

        int X = 0, Y = 0;
        int.TryParse(GetIniFileString(inifilename, "General", "X_StartPosition", "0"), out X);
        int.TryParse(GetIniFileString(inifilename, "General", "Y_StartPosition", "0"), out Y);
        inisettings.StartPosition = new Point(X, Y);
        int W = 0, H = 0;
        int.TryParse(GetIniFileString(inifilename, "General", "StartWidth", "0"), out W);
        int.TryParse(GetIniFileString(inifilename, "General", "StartHeight", "0"), out H);
        inisettings.StartSize = new Size(W, H);
        bool.TryParse(GetIniFileString(inifilename, "General", "ShowPreview", "True"), out inisettings.ShowPreview);
        inisettings.LastPalette = GetIniFileString(inifilename, "General", "LastPalette", "");
        try
        {
            inisettings.DefaultCompression = (SHP_TS_EncodingFormat)Enum.Parse(typeof(SHP_TS_EncodingFormat), GetIniFileString(inifilename, "General", "DefaultCompression", "True"));
        }
        catch 
        { 
        }

        bool.TryParse(GetIniFileString(inifilename, "General", "CreateImages", "False"), out inisettings.CreateImages);
        inisettings.CreateImages_FileName = GetIniFileString(inifilename, "General", "CreateImages_FileName", "tmpimg");
        inisettings.CreateImages_Format = GetIniFileString(inifilename, "General", "CreateImages_Format", "Png");
        bool.TryParse(GetIniFileString(inifilename, "General", "PreventTSWobbleBug", "False"), out inisettings.PreventTSWobbleBug);

        bool.TryParse(GetIniFileString(inifilename, "General", "OptimizeCanvas", "True"), out inisettings.OptimizeCanvas);
        bool.TryParse(GetIniFileString(inifilename, "General", "KeepCentered", "True"), out inisettings.KeepCentered);

        inisettings.RadarColor = GetColorFromHexString(GetIniFileString(inifilename, "General", "RadarColor", "#FFFFFF"));
        bool.TryParse(GetIniFileString(inifilename, "General", "AverageRadarColor", "True"), out inisettings.AverageRadarColor);

        bool.TryParse(GetIniFileString(inifilename, "General", "UseCustomBackgroundColor", "False"), out inisettings.UseCustomBackgroundColor);
        inisettings.CustomBackgroundColor = GetColorFromHexString(GetIniFileString(inifilename, "General", "CustomBackgroundColor", "#FFFFFF"));
        bool.TryParse(GetIniFileString(inifilename, "General", "CombineTransparentPixel", "False"), out inisettings.CombineTransparentPixel);


        inisettings.OutputFolder = GetIniFileString(inifilename, "General", "OutputFolder", "");
        inisettings.PreviewBackgroundImage = GetIniFileString(inifilename, "General", "PreviewBackgroundImage", "");

        inisettings.LastFireFLHFinderDirectory = GetIniFileString(inifilename, "General", "LastFireFLHFinderDirectory", "");
    }


    public static System.Drawing.Color GetSystemDrawingColorFromHexString(string hexString)
    {
        if ((hexString == null) || (hexString == ""))
            hexString = "#FF000000";
        if (!System.Text.RegularExpressions.Regex.IsMatch(hexString, @"[#]([0-9]|[a-f]|[A-F]){8}\b"))
            throw new ArgumentException();

        int alpha = int.Parse(hexString.Substring(1, 2), System.Globalization.NumberStyles.HexNumber);
        int red = int.Parse(hexString.Substring(3, 2), System.Globalization.NumberStyles.HexNumber);
        int green = int.Parse(hexString.Substring(5, 2), System.Globalization.NumberStyles.HexNumber);
        int blue = int.Parse(hexString.Substring(7, 2), System.Globalization.NumberStyles.HexNumber);
        return System.Drawing.Color.FromArgb(alpha, red, green, blue);
    }

    public static string ColorToStr(Color c, bool AsHexString)
    {
        if (AsHexString)
            return "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
        else
            return c.R.ToString() + "," + c.G.ToString() + "," + c.B.ToString();
    }

    private static Color GetColorFromHexString(string hex)
    {
        try
        {
            return ColorTranslator.FromHtml(hex);
        }
        catch { return Color.FromArgb(0, 0, 0); }
    }

    private static string Color2String(Color c)
    {
        return "[" + c.A.ToString("D3") + "," + c.R.ToString("D3") + "," + c.G.ToString("D3") + "," + c.B.ToString("D3") + "]";
    }

    private static Color String2Color(string s)
    {
        if ((s.StartsWith("[")) && (s.EndsWith("]")) && (s.Split(',').Length == 4))
        {
            s = s.Replace("[", "").Replace("]", "");
            string[] v = s.Split(',');
            byte a = byte.Parse(v[0]);
            byte r = byte.Parse(v[1]);
            byte g = byte.Parse(v[2]);
            byte b = byte.Parse(v[3]);
            return Color.FromArgb(a, r, g, b);
        }
        else return Color.FromArgb(255, 0, 0, 0);
    }

    internal static void SavePaletteSetup(string filename, CPalette[] Palettes)
    {
        for (int i = 0; i < Palettes.Length; i++)
        {
            WritePrivateProfileString("Palette" + i.ToString("D5"), "PaletteFile", Palettes[i].PaletteFile, filename);
            WritePrivateProfileString("Palette" + i.ToString("D5"), "ConversionMethod", ((int)Palettes[i].ConversionMethod).ToString(), filename);
            WritePrivateProfileString("Palette" + i.ToString("D5"), "PaletteName", Palettes[i].PaletteName.ToString(), filename);
            for (int c = 0; c < Palettes[i].palette.Length; c++)
                WritePrivateProfileString("Palette" + i.ToString("D5"), "Color" + c.ToString("D3"), Palettes[i].palette[c].IsUsed.ToString() + "|" + Palettes[i].palette[c].MakeTransparent.ToString(), filename);
        }
    }
    internal static CPalette[] LoadPaletteSetup(string filename)
    {
        CPalette[] pals = new CPalette[0];
        List<string> paletteSections = GetSections(filename);
        paletteSections.RemoveAll(u => !u.Contains("Palette"));
        pals = new CPalette[paletteSections.Count()];
        for (int i = 0; i < paletteSections.Count(); i++)
        {
            CPalette p = new CPalette();
            string palfile = GetIniFileString(filename, paletteSections[i], "PaletteFile", "");
            if (System.IO.File.Exists(palfile))
                p.Load(palfile);
            int cm = 0;
            int.TryParse(GetIniFileString(filename, paletteSections[i], "ConversionMethod", ""), out cm);
            p.ConversionMethod = (ColorConversionMethod)cm;
            p.PaletteName = GetIniFileString(filename, paletteSections[i], "PaletteName", "");

            List<string> keys = GetKeys(filename, paletteSections[i]);
            keys.RemoveAll(u => !u.Contains("Color"));

            for (int c = 0; c < keys.Count(); c++)
            {
                int keynr = 0;
                int.TryParse(keys[c].Replace("Color", ""), out keynr);

                string val = GetIniFileString(filename, paletteSections[i], keys[c], "");

                bool IsUsed = true;
                bool.TryParse(val.Split('|')[0], out IsUsed);
                bool MakeTransparent = true;
                bool.TryParse(val.Split('|')[1], out MakeTransparent);
                if ((keynr >= 0) && (keynr < p.palette.Length))
                {
                    p.palette[keynr].IsUsed = IsUsed;
                    p.palette[keynr].MakeTransparent = MakeTransparent;
                }
            }
            pals[i] = p;
        }

        return pals;
    }

    internal static void SaveProject(string filename, Form_ImageShaper.DGVCell[] Cells, CPalette[] Palettes)
    {
        SavePaletteSetup(filename, Palettes);

        for (int i=0;i<Cells.Length;i++)
        {
            CImageFile imf = (CImageFile)Cells[i].Value;
            WritePrivateProfileString("Cell" + i.ToString("D5"), "ColumnIndex", Cells[i].ColumnIndex.ToString(), filename);
            WritePrivateProfileString("Cell" + i.ToString("D5"), "RowIndex", Cells[i].RowIndex.ToString(), filename);
            WritePrivateProfileString("Cell" + i.ToString("D5"), "FileName", imf.FileName, filename);
            WritePrivateProfileString("Cell" + i.ToString("D5"), "BitFlags", ((int)imf.BitFlags).ToString(), filename);
            WritePrivateProfileString("Cell" + i.ToString("D5"), "CompressionFormat", ((int)imf.CompressionFormat).ToString(), filename);
            WritePrivateProfileString("Cell" + i.ToString("D5"), "UseCustomBackgroundColor", imf.UseCustomBackgroundColor.ToString(), filename);
            WritePrivateProfileString("Cell" + i.ToString("D5"), "CustomBackgroundColor", Color2String(imf.CustomBackgroundColor), filename);
            WritePrivateProfileString("Cell" + i.ToString("D5"), "PaletteIndex", imf.PaletteIndex.ToString(), filename);
            //WritePrivateProfileString("Cell" + i.ToString("D5"), "PaletteName", imf.PaletteName.ToString(), filename);
            WritePrivateProfileString("Cell" + i.ToString("D5"), "IsSHP", imf.IsSHP.ToString(), filename);
            WritePrivateProfileString("Cell" + i.ToString("D5"), "SHPFrameNr", imf.SHPFrameNr.ToString(), filename);
            WritePrivateProfileString("Cell" + i.ToString("D5"), "RadarColor", Color2String(imf.RadarColor), filename);
            WritePrivateProfileString("Cell" + i.ToString("D5"), "RadarColorAverage", imf.RadarColorAverage.ToString(), filename);
            WritePrivateProfileString("Cell" + i.ToString("D5"), "CombineTransparentPixel", imf.CombineTransparentPixel.ToString(), filename);
        }
    }

    internal static Form_ImageShaper.DGVCell[] LoadProject(string filename)
    {
        List<string> cellSections = GetSections(filename);
        cellSections.RemoveAll(u => !u.Contains("Cell"));
        Form_ImageShaper.DGVCell[] cells = new Form_ImageShaper.DGVCell[cellSections.Count];
        for (int i = 0; i < cellSections.Count(); i++)
        {
            Form_ImageShaper.DGVCell cell = new Form_ImageShaper.DGVCell();
            int.TryParse(GetIniFileString(filename, cellSections[i], "ColumnIndex", ""), out cell.ColumnIndex);
            int.TryParse(GetIniFileString(filename, cellSections[i], "RowIndex", ""), out cell.RowIndex);

            string palfile = GetIniFileString(filename, cellSections[i], "FileName", "");
            int tmp = 0;
            int.TryParse(GetIniFileString(filename, cellSections[i], "CompressionFormat", ""), out tmp);
            SHP_TS_EncodingFormat CompressionFormat = (SHP_TS_EncodingFormat)tmp;
            tmp = 0;
            int.TryParse(GetIniFileString(filename, cellSections[i], "PaletteIndex", "0"), out tmp);
            CImageFile imf = new CImageFile(palfile, tmp, CompressionFormat);

            tmp = 0; int.TryParse(GetIniFileString(filename, cellSections[i], "BitFlags", "1"), out tmp); imf.BitFlags = (SHP_TS_BitFlags)tmp;
            bool.TryParse(GetIniFileString(filename, cellSections[i], "UseCustomBackgroundColor", ""), out imf.UseCustomBackgroundColor);
            imf.CustomBackgroundColor = String2Color(GetIniFileString(filename, cellSections[i], "CustomBackgroundColor", ""));
            bool.TryParse(GetIniFileString(filename, cellSections[i], "IsSHP", ""), out imf.IsSHP);
            tmp = -1; int.TryParse(GetIniFileString(filename, cellSections[i], "SHPFrameNr", "-1"), out tmp); imf.SHPFrameNr = tmp;
            imf.RadarColor = String2Color(GetIniFileString(filename, cellSections[i], "RadarColor", ""));
            bool.TryParse(GetIniFileString(filename, cellSections[i], "RadarColorAverage", ""), out imf.RadarColorAverage);
            bool.TryParse(GetIniFileString(filename, cellSections[i], "CombineTransparentPixel", ""), out imf.CombineTransparentPixel);

            cell.Value = imf;
            cells[i] = cell;
        }
        return cells;
    }

}//end of class Cinimanager
