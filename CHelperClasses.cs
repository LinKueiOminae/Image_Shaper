using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace ImageShaper
{
    public static class BitHelper
    {
        public static bool IsSet<T>(T flags, T flag) where T : struct
        {
            int flagsvalue = (int)(object)flags;
            int flagvalue = (int)(object)flag;
            return (flagsvalue & flagvalue) != 0;
        }

        public static void Set<T>(ref T flags, T flag) where T : struct
        {
            int flagsvalue = (int)(object)flags;
            int flagvalue = (int)(object)flag;
            flags = (T)(object)(flagsvalue | flagvalue);
        }

        public static void Unset<T>(ref T flags, T flag) where T : struct
        {
            int flagsvalue = (int)(object)flags;
            int flagvalue = (int)(object)flag;
            flags = (T)(object)(flagsvalue & (~flagvalue));
        }

        public static bool GetBit(int i, byte pos)
        {
            return (i & (1 << pos)) != 0;
        }
    }

    /// <summary>
    /// a single Image/Frame in the datagrid
    /// </summary>
    class CImageFile
    {
        public CImageFile(string file, int paletteIndex, SHP_TS_EncodingFormat CompressionFormat)
        {
            this.ImageName = System.IO.Path.GetFileName(file);
            this.FileName = file;
            this.PaletteIndex = paletteIndex;
            this.UseCustomBackgroundColor = false;
            this.CustomBackgroundColor = Color.FromArgb(0, 0, 0);
            this.CombineTransparentPixel = false;
            this.CompressionFormat = CompressionFormat;
            this.IsSHP = false;
            this.SHPFrameNr = -1;
            this.RadarColor = Color.FromArgb(255, 255, 255);
            this.RadarColorAverage = false;
            this.BitFlags = SHP_TS_BitFlags.UnknownBit1;
        }
        public string ImageName;
        public string FileName;

        private int _PaletteIndex;
        /// <summary>
        /// the index of the used palette in the PaletteManager
        /// </summary>
        public int PaletteIndex
        {
            get { return _PaletteIndex; }
            set
            {
                _PaletteIndex = value;
                this._PaletteName = "";
                if ((PaletteIndex != -1) && (PaletteIndex < PaletteManager.Palettes.Length))
                    _PaletteName = "(" + PaletteManager.GetPalette(_PaletteIndex).PaletteName + ")";
                else
                    _PaletteName = "(Palette not loaded)";
            }
        }

        private string _PaletteName;
        public string PaletteName
        {
            get { return _PaletteName; }
        }

        public SHP_TS_EncodingFormat CompressionFormat;
        public SHP_TS_BitFlags BitFlags;

        public Color CustomBackgroundColor;
        public bool UseCustomBackgroundColor;
        public bool CombineTransparentPixel;

        public override string ToString()
        {
            string compression = " " + CompressionFormat.ToString();

            string SHPFrame = "";
            if (IsSHP) SHPFrame = "[" + SHPFrameNr.ToString("00000") + "]";
            return ImageName + SHPFrame + " " + _PaletteName + compression;
        }

        public bool IsSHP;
        public int SHPFrameNr;

        public Color RadarColor;
        public bool RadarColorAverage;
    }

    class CFiles2Load
    {
        public string[] files;
        public System.Windows.Forms.DataGridView dgv;
        public CFiles2Load(string[] files, System.Windows.Forms.DataGridView DGV)
        {
            this.files = files;
            this.dgv = new System.Windows.Forms.DataGridView();
            this.dgv.AllowUserToAddRows = false;
            foreach (System.Windows.Forms.DataGridViewColumn col in DGV.Columns)
                this.dgv.Columns.Add((System.Windows.Forms.DataGridViewColumn)col.Clone());

            System.Windows.Forms.DataGridViewRow row = new System.Windows.Forms.DataGridViewRow();
            for (int i = 0; i < DGV.Rows.Count; i++)
            {
                row = (System.Windows.Forms.DataGridViewRow)DGV.Rows[i].Clone();
                for (int c = 0; c < DGV.Rows[i].Cells.Count; c++)
                    row.Cells[c].Value = DGV.Rows[i].Cells[c].Value;

                this.dgv.Rows.Add(row);
            }
        }
    }

    /// <summary>
    /// one row in the datagrid, containing one ore more (up to 3) imageFiles
    /// </summary>
    class CImageJob
    {
        public CImageFile[] files;
        public int frameNr;
        /// <summary>
        /// only the filename without path
        /// </summary>
        public string tmpfilename;
        public string tmpfileformat;
        /// <summary>
        /// the path/filename of the file to write
        /// </summary>
        public string outputfilename;
        public bool CreateImageFile;
        public SHP_TS_EncodingFormat DefaultCompression;
        public CImageJob(string targetpath, int row_index, string framefilename, string framefileformat, List<CImageFile> imagefiles, bool CreateFrameFile, SHP_TS_EncodingFormat DefaultCompression)
        {
            this.CreateImageFile = CreateFrameFile;
            this.frameNr = row_index;
            if (framefilename == "*")
                this.tmpfilename = Path.GetFileNameWithoutExtension(imagefiles[0].FileName);
            else
                this.tmpfilename = framefilename + row_index.ToString("00000");
            this.tmpfileformat = framefileformat;
            if (!CreateFrameFile)
                this.tmpfilename = "frame" + row_index.ToString("00000");
            this.outputfilename = Path.Combine(targetpath, tmpfilename);
            this.files = imagefiles.ToArray();
            this.DefaultCompression = DefaultCompression;
        }
    }

    /// <summary>
    /// collection of imagejobs for a workerthread
    /// </summary>
    class CWorkerJob
    {
        public List<CImageJob> imagejobs;
        public int workerID;
        public CWorkerJob(List<CImageJob> jobs, int ID)
        {
            this.imagejobs = jobs;
            this.workerID = ID;
        }
    }

    /// <summary>
    /// the result from the worker of the processed and combined images of a single imagejob
    /// </summary>
    class CImageResult
    {
        public Bitmap bmp;
        public SHP_TS_EncodingFormat format;
        public SHP_TS_BitFlags bitflags;
        public int frameNr;
        public string message;
        public Color RadarColor;
        public CImageResult(Bitmap bmp, int frameNr, SHP_TS_EncodingFormat format, SHP_TS_BitFlags bitflags, Color RadarColor, string message)
        {
            this.bmp = bmp;
            this.frameNr = frameNr;
            this.message = message;
            this.format = format;
            this.RadarColor = RadarColor;
            this.bitflags = bitflags;
        }
    }

}