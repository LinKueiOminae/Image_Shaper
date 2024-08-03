using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;

using System.IO;
using System.Diagnostics;

namespace ImageShaper
{
    public partial class Form_ImageShaper : Form
    {
        public const string ProgramName = "Image Shaper";
        private ContextMenuStrip dataGrid_CM;

        [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "ShowWindow")]
        private static extern int ShowWindow(IntPtr hWnd, uint Msg);

        [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        private static extern int SetWindowPos(int hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        static void ShowInactiveTopmost(Form frm)
        {
            if ((frm == null) || (frm.IsDisposed))
                return;
            try
            {
                ShowWindow(frm.Handle, 4);
                SetWindowPos(frm.Handle.ToInt32(), 0, frm.Left, frm.Top, frm.Width, frm.Height, 0x0010);
            }
            catch { }
        }

        private string GetProgramPath
        {
            get { return Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location); }
        }
        private string GetProgramTempPath
        {
            get { return Path.Combine(GetProgramPath, "Temp"); }
        }

        /// <summary>
        /// show index number in row header
        /// </summary>
        void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var grid = sender as DataGridView;
            var rowIdx = e.RowIndex.ToString("00000");

            var centerFormat = new StringFormat()
            {
                // use right alignment for numbers
                Alignment = StringAlignment.Far,
                LineAlignment = StringAlignment.Center
            };

            var headerBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, grid.RowHeadersWidth - 5, e.RowBounds.Height);
            if (e.RowIndex < this.dataGridView_Files.RowCount - 1)
                e.Graphics.DrawString(rowIdx, this.dataGridView_Files.Font, SystemBrushes.ControlText, headerBounds, centerFormat);
        }

        void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridViewColumn column = this.dataGridView_Files.Columns[e.ColumnIndex];
            if (this.dataGridView_Files.Rows.Count > 0)
            {
                bool selected = !this.dataGridView_Files.Rows[0].Cells[column.Index].Selected;
                foreach (DataGridViewRow row in this.dataGridView_Files.Rows)
                {
                    row.Cells[column.Index].Selected = selected;
                }
            }
            UpdatePreview();
        }

        void Form_ImageShaper_WindowChange(object sender, EventArgs e)
        {
            if (form_Preview != null)
                form_Preview.Location = new Point(this.Location.X + this.Width, this.Location.Y);
        }

        private ImageFormat getImageFormat(string extension)
        {
            switch (extension.ToLower())
            {
                case "png": return ImageFormat.Png;
                case "bmp": return ImageFormat.Bmp;
                case "gif": return ImageFormat.Gif;
                case "tiff": return ImageFormat.Tiff;
                default: return null;
            }
        }

        public SHP_TS_EncodingFormat GetDefaultCompression
        {
            get
            {
                //the 0. item is the "Undefined" compression, which is not present in the global compression combobox
                return (SHP_TS_EncodingFormat)(this.toolStripComboBox_DefaultCompression.SelectedIndex + 1);
            }
        }

        void Form_ImageShaper_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (RunAsCommand) return;//don't save anything when run as console command

            if ((this.Location.X >= 0) && (this.Location.Y >= 0))
            {
                Cinimanager.inisettings.StartPosition = this.Location;
                Cinimanager.inisettings.StartSize = this.Size;
            }
            Cinimanager.inisettings.PreventTSWobbleBug = this.checkBox_PreventWobbleBug.Checked;

            Cinimanager.inisettings.CreateImages = this.checkBox_FrameFiles.Checked;
            Cinimanager.inisettings.CreateImages_FileName = this.textBox_CreateFiles.Text;
            Cinimanager.inisettings.CreateImages_Format = this.comboBox_CreateFilesFormat.SelectedItem.ToString();
            if (this.uC_Palette1.Palette != null)
                Cinimanager.inisettings.LastPalette = this.uC_Palette1.Palette.PaletteFile;
            else Cinimanager.inisettings.LastPalette = "";
            Cinimanager.inisettings.ShowPreview = ShowPreview;

            Cinimanager.inisettings.RadarColor = (Color)this.button_RadarColor.Tag;
            Cinimanager.inisettings.AverageRadarColor = this.checkBox_RadarColorAverage.Checked;

            Cinimanager.inisettings.UseCustomBackgroundColor = this.checkBox_UseCustomBackgroundColor.Checked;
            Cinimanager.inisettings.CustomBackgroundColor = (Color)this.button_CustomBackgroundColor.Tag;
            Cinimanager.inisettings.CombineTransparentPixel = this.checkBox_CombineTransparency.Checked;

            Cinimanager.inisettings.OptimizeCanvas = this.checkBox_OptimizeCanvas.Checked;
            Cinimanager.inisettings.KeepCentered = this.checkBox_KeepCentered.Checked;

            Cinimanager.inisettings.OutputFolder = this.toolStripMenuItem_Outputfolder.ToolStrip_UC_FolderSelector.Value;
            Cinimanager.inisettings.PreviewBackgroundImage = this.toolStripMenuItem_previewBackgroundImage.ToolStrip_UC_FolderSelector.Value;

            Cinimanager.SaveIniSettings();
        }

        public Form_ImageShaper()
        {
            InitializeComponent();
            System.Reflection.Assembly thisAssembly = System.Reflection.Assembly.GetExecutingAssembly();
            string version = thisAssembly.GetName().Version.Major.ToString("D2") + "." +
                                     thisAssembly.GetName().Version.Minor.ToString("D2") + "." +
                                     thisAssembly.GetName().Version.Build.ToString("D2") + "." +
                                     thisAssembly.GetName().Version.Revision.ToString("D2");
            this.FormClosing += new FormClosingEventHandler(Form_ImageShaper_FormClosing);
            this.Text = ProgramName + " v" + version;
            this.StartPosition = FormStartPosition.Manual;


            this.dataGridView_BitFields.Rows.Add();
            foreach (DataGridViewCell c in this.dataGridView_BitFields.Rows[0].Cells)
                c.Value = false;
            SetBitField(SHP_TS_BitFlags.UnknownBit1, SHP_TS_EncodingFormat.Undefined);
            this.dataGridView_BitFields.CellPainting += new DataGridViewCellPaintingEventHandler(dataGridView_BitFields_CellPainting);
            this.dataGridView_BitFields.CellClick += new DataGridViewCellEventHandler(dataGridView_BitFields_CellClick);


            this.comboBox_Compression.Items.Clear();
            foreach (SHP_TS_EncodingFormat ef in Enum.GetValues(typeof(SHP_TS_EncodingFormat)))
            {
                this.comboBox_Compression.Items.Add(ef.ToString());
            }
            this.comboBox_Compression.SelectedIndex = 0;


            this.toolStripComboBox_DefaultCompression.Items.Clear();
            foreach (SHP_TS_EncodingFormat ef in Enum.GetValues(typeof(SHP_TS_EncodingFormat)))
            {
                if (ef != SHP_TS_EncodingFormat.Undefined)
                    this.toolStripComboBox_DefaultCompression.Items.Add(ef);
            }
            this.toolStripComboBox_DefaultCompression.SelectedIndex = 1;

            this.comboBox_CreateFilesFormat.Items.Clear();
            this.comboBox_CreateFilesFormat.Items.Add("PNG");
            this.comboBox_CreateFilesFormat.Items.Add("BMP");
            this.comboBox_CreateFilesFormat.Items.Add("GIF");
            this.comboBox_CreateFilesFormat.Items.Add("TIFF");
            this.comboBox_CreateFilesFormat.Items.Add("SHP(TS)");
            this.comboBox_CreateFilesFormat.SelectedIndex = 0;


            ///Load the INI
            Cinimanager.inifilename = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "ImageShaper.ini");
            Cinimanager.LoadIniSettings();
            if ((Cinimanager.inisettings.StartPosition.X >= 0) && (Cinimanager.inisettings.StartPosition.Y >= 0))
                this.Location = Cinimanager.inisettings.StartPosition;

            Boolean isWindowOnScreen = false;
            for (int i = 0; i < System.Windows.Forms.Screen.AllScreens.Length; i++)
            {
                if (
                    (this.Location.X < System.Windows.Forms.Screen.AllScreens[i].WorkingArea.Right - 10) &&
                    (this.Location.Y < System.Windows.Forms.Screen.AllScreens[i].WorkingArea.Bottom - 10) &&
                    (this.Location.X >= System.Windows.Forms.Screen.AllScreens[i].WorkingArea.Left) &&
                    (this.Location.Y >= System.Windows.Forms.Screen.AllScreens[i].WorkingArea.Top - 18)
                    )
                {
                    isWindowOnScreen = true;
                    break;
                }
            }
            if (!isWindowOnScreen)
                for (int i = 0; i < System.Windows.Forms.Screen.AllScreens.Length; i++)
                    if (System.Windows.Forms.Screen.AllScreens[i].Primary)
                        this.Location = System.Windows.Forms.Screen.AllScreens[i].Bounds.Location;


            this.Size = Cinimanager.inisettings.StartSize;
            this.checkBox_PreventWobbleBug.Checked = Cinimanager.inisettings.PreventTSWobbleBug;

            for (int i = 0; i < this.toolStripComboBox_DefaultCompression.Items.Count; i++)
            {
                if (Cinimanager.inisettings.DefaultCompression == (SHP_TS_EncodingFormat)this.toolStripComboBox_DefaultCompression.Items[i])
                    this.toolStripComboBox_DefaultCompression.SelectedIndex = i;
            }

            this.checkBox_FrameFiles.Checked = Cinimanager.inisettings.CreateImages;
            this.textBox_CreateFiles.Text = Cinimanager.inisettings.CreateImages_FileName;
            for (int i = 0; i < this.comboBox_CreateFilesFormat.Items.Count; i++)
            {
                if (this.comboBox_CreateFilesFormat.Items[i].ToString() == Cinimanager.inisettings.CreateImages_Format)
                    this.comboBox_CreateFilesFormat.SelectedIndex = i;
            }

            this.checkBox_OptimizeCanvas.Checked = Cinimanager.inisettings.OptimizeCanvas;
            this.checkBox_KeepCentered.Checked = Cinimanager.inisettings.KeepCentered;

            if (Cinimanager.inisettings.LastPalette != "")
                this.uC_Palette1.LoadPalette(Cinimanager.inisettings.LastPalette);
            this.ShowPreview = Cinimanager.inisettings.ShowPreview;


            this.button_RadarColor.Tag = Cinimanager.inisettings.RadarColor;
            this.button_RadarColor.Text = Cinimanager.ColorToStr(Cinimanager.inisettings.RadarColor, true);
            this.checkBox_RadarColorAverage.Checked = Cinimanager.inisettings.AverageRadarColor;

            this.checkBox_UseCustomBackgroundColor.Checked = true;
            this.checkBox_UseCustomBackgroundColor.Checked = Cinimanager.inisettings.UseCustomBackgroundColor;
            this.button_CustomBackgroundColor.Tag = Cinimanager.inisettings.CustomBackgroundColor;
            this.button_CustomBackgroundColor.Text = Cinimanager.ColorToStr(Cinimanager.inisettings.CustomBackgroundColor, true);
            this.checkBox_CombineTransparency.Checked = Cinimanager.inisettings.CombineTransparentPixel;

            this.toolStripMenuItem_Outputfolder.ToolStrip_UC_FolderSelector.Value = Cinimanager.inisettings.OutputFolder;
            this.toolStripMenuItem_previewBackgroundImage.ToolStrip_UC_FolderSelector.Value = Cinimanager.inisettings.PreviewBackgroundImage;

            ///end of ini loading


            this.dataGridView_Files.RowPostPaint += new DataGridViewRowPostPaintEventHandler(dataGridView1_RowPostPaint);//show index nr in row header
            this.Resize += new EventHandler(Form_ImageShaper_WindowChange);
            this.Move += new EventHandler(Form_ImageShaper_WindowChange);

            this.dataGridView_Files.AllowDrop = true;
            this.dataGridView_Files.DragDrop += new DragEventHandler(dataGridView1_DragDrop);
            this.dataGridView_Files.DragEnter += new DragEventHandler(dataGridView1_DragEnter);
            this.dataGridView_Files.KeyUp += new KeyEventHandler(dataGridView1_KeyUp);

            this.dataGridView_Files.MouseDown += new MouseEventHandler(dataGridView_Files_MouseDown);
            this.dataGridView_Files.MouseMove += new MouseEventHandler(dataGridView_Files_MouseMove);
            this.dataGridView_Files.DragOver += new DragEventHandler(dataGridView_Files_DragOver);

            this.dataGridView_Files.MouseClick += new MouseEventHandler(dataGridView1_MouseClick);
            this.dataGridView_Files.ColumnHeaderMouseClick += new DataGridViewCellMouseEventHandler(dataGridView1_ColumnHeaderMouseClick);

            dataGrid_CM = new ContextMenuStrip();
            dataGrid_CM.Items.Add("Set Palette");
            dataGrid_CM.Items.Add("Set Bits/Compression");
            dataGrid_CM.Items.Add("-");
            dataGrid_CM.Items.Add("Load from Clipboard");
            dataGrid_CM.Items.Add("-");
            dataGrid_CM.Items.Add("Load Images / SHPs");
            dataGrid_CM.Items.Add("Load and Split Image");
            dataGrid_CM.Items.Add("-");
            dataGrid_CM.Items.Add("Reverse order of selected cells");
            dataGrid_CM.Items.Add("-");
            dataGrid_CM.Items.Add("Copy");
            dataGrid_CM.Items.Add("Cut");
            dataGrid_CM.Items.Add("Paste");
            dataGrid_CM.Items[0].Click += new EventHandler(DataGridCell_SetPalette);
            dataGrid_CM.Items[1].Click += new EventHandler(DataGridCell_SetCompression);
            dataGrid_CM.Items[3].Click += new EventHandler(DataGridCell_LoadFromClipboard);
            dataGrid_CM.Items[5].Click += new EventHandler(DataGridCell_LoadImages);
            dataGrid_CM.Items[6].Click += new EventHandler(DataGridCell_LoadSplitImage);
            dataGrid_CM.Items[8].Click += new EventHandler(DataGridCell_ReverseOrder);
            dataGrid_CM.Items[10].Click += new EventHandler(DataGridCell_Copy);
            dataGrid_CM.Items[11].Click += new EventHandler(DataGridCell_Cut);
            dataGrid_CM.Items[12].Click += new EventHandler(DataGridCell_Paste);

            //changes are now instantly applied to the selected cells
            dataGrid_CM.Items[1].Visible = false;

            this.uC_Palette1.initialDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            if (Directory.Exists(Path.Combine(this.uC_Palette1.initialDirectory, "Palettes")))
                this.uC_Palette1.initialDirectory = Path.Combine(this.uC_Palette1.initialDirectory, "Palettes");

            this.uC_Palette1.PaletteChanged += new EventHandler<EventArgs>(uC_Palette1_PaletteChanged);

            ToolTip toolTip1 = new ToolTip();
            // Set up the delays for the ToolTip.
            toolTip1.AutoPopDelay = 1000000;
            toolTip1.InitialDelay = 100;
            toolTip1.ReshowDelay = 100;
            // Force the ToolTip text to be displayed whether or not the form is active.
            toolTip1.ShowAlways = true;

            // Set up the ToolTip text for the Button and Checkbox.
            toolTip1.SetToolTip(this.toolStripComboBox_DefaultCompression.Control, "The default compression to use for all images that have \"Undefined\" compression set.\nUncompressed is SHP Builders Compression 1,\nRLE_Zero is SHP Builders Compression 3.\nDetect_best_size is first trying RLE_Zero. If that turns out bigger than uncompressed, uncompressed is used." +
                "\nUncompressed_Full_Frame writes the SHP frame uncompressed and in the full canvas size");
            //toolTip1.SetToolTip(this.toolStripMenuItem_Label_NUD_NrWorker.Control, "The number of threads to use for the color conversion of the imagelist. Default is number of processors.");
            toolTip1.SetToolTip(this.comboBox_Compression, "The compression method of the selected image/frame.\n\"Undefined\" means, this frame will use the global setting from the menustrip.");
            this.dataGridView_BitFields.ShowCellToolTips = false;
            toolTip1.SetToolTip(this.dataGridView_BitFields, "The bit flags with the compression as the second bit. The compression bit can only be changed via the combobox above.\nBy default the first bit is set, since nearly all TS/RA2 SHPs have that bit set.");

            toolTip1.SetToolTip(this.button_RadarColor, "The frame's Radarcolor. Works only on tiberium/ore Overlays.\nTS and RA2 ignore it for all other objecttypes.");
            toolTip1.SetToolTip(this.checkBox_RadarColorAverage, "Ignore the set color and instead calculate for this frame the average color of all colored pixel.");
            toolTip1.SetToolTip(this.checkBox_FrameFiles, "Create for each frame an image file in the \\Temp subfolder.");
            toolTip1.SetToolTip(this.textBox_CreateFiles, "The filename for the frame images. This is followed by a 5 digit framenumber.\nIf * is set, the original filename will be used.");
            toolTip1.SetToolTip(this.comboBox_CreateFilesFormat, "The format for the single frame images.");
            toolTip1.SetToolTip(this.checkBox_PreventWobbleBug, "Prevent the wobble bug in TS for turreted units. When set, Image Shaper makes sure the CX/CY and OffsetX/OffsetY values for each frame are all even.");

            toolTip1.SetToolTip(this.checkBox_UseCustomBackgroundColor, "When enabled, the color conversion uses the specified background color as the transparent palette color #0.\nWhen disabled, the color conversion uses the color of the pixel in the images top left corner for the transparent color #0.");
            toolTip1.SetToolTip(this.button_CustomBackgroundColor, "The fixed background color used for the color conversion into the transparent palette color #0.");
            toolTip1.SetToolTip(this.checkBox_CombineTransparency, "When combining this image with another, only the transparent pixel of this image are copied over.\nThis allows to remove parts of the base image with the transparency mask provided by this image.\nThis works only for images in the 2nd or 3rd ImageList!");

            toolTip1.SetToolTip(this.checkBox_OptimizeCanvas, "Reduces the main SHP canvas to the smallest possible size, by removing all pure transparent borders.");
            toolTip1.SetToolTip(this.checkBox_KeepCentered, "While reducing the main SHP canvas, the object is kept centered by shrinking the sides evenly for left/right and top/bottom.\nThe games work fine without, but SHP Builder is not showing the offset of each frame, thus the visible center in SHP Builder could be different from the real object's centerpoint.");

            toolTip1.SetToolTip(this.numericUpDown_SplitResult, "Splits the frames up into the specified number of files. #frames div value = frames per SHP. Leftover frames from a division with remainders are skipped!");

            UpdatePreview();

            toolStripMenuItem_Label_NUD_NrWorker.ToolStrip_UC_Label_NUD.Minimum = 1;
            toolStripMenuItem_Label_NUD_NrWorker.ToolStrip_UC_Label_NUD.Maximum = 16;
            if (Environment.ProcessorCount > (this.toolStripMenuItem_Label_NUD_NrWorker.ToolStrip_UC_Label_NUD.Maximum))
                this.toolStripMenuItem_Label_NUD_NrWorker.ToolStrip_UC_Label_NUD.Maximum = Environment.ProcessorCount;
            this.toolStripMenuItem_Label_NUD_NrWorker.ToolStrip_UC_Label_NUD.Value = Environment.ProcessorCount;


            if (ShowPreview) this.showHidePreviewToolStripMenuItem.Text = "Hide Preview";
            else this.showHidePreviewToolStripMenuItem.Text = "Show Preview";
            ShowHidePreview();
            this.Activated += new EventHandler(Form_ImageShaper_Activated);
        }

        void uC_Palette1_PaletteChanged(object sender, EventArgs e)
        {
            UpdatePreview();
        }

        void Form_ImageShaper_Activated(object sender, EventArgs e)
        {
            if (form_Preview != null)
                ShowInactiveTopmost(form_Preview);
        }

        #region bitfield
        private void comboBox_Compression_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetBitField(GetBitField(), (SHP_TS_EncodingFormat)this.comboBox_Compression.SelectedIndex);
            if (this.comboBox_Compression.Focused)
            {
                if (this.dataGridView_Files.SelectedCells.Count > 0)
                {
                    SHP_TS_EncodingFormat f = (SHP_TS_EncodingFormat)this.comboBox_Compression.SelectedIndex;
                    SHP_TS_BitFlags b = GetBitField();
                    foreach (DataGridViewCell cell in this.dataGridView_Files.SelectedCells)
                        if (cell.Value != null)
                            ((CImageFile)cell.Value).CompressionFormat = f;
                    this.dataGridView_Files.Invalidate();
                }
            }
        }

        private void SetBitField(SHP_TS_BitFlags bits, SHP_TS_EncodingFormat compression)
        {
            for (byte i = 0; i < this.dataGridView_BitFields.Rows[0].Cells.Count; i++)
            {
                this.dataGridView_BitFields.Rows[0].Cells[i].Value = BitHelper.GetBit((int)bits, i);
                if (i == 1)
                {
                    switch (compression)
                    {
                        case SHP_TS_EncodingFormat.Undefined: this.dataGridView_BitFields.Rows[0].Cells[i].Value = "?"; break;
                        case SHP_TS_EncodingFormat.Uncompressed: this.dataGridView_BitFields.Rows[0].Cells[i].Value = "U"; break;
                        case SHP_TS_EncodingFormat.RLE_Zero: this.dataGridView_BitFields.Rows[0].Cells[i].Value = "R"; break;
                        case SHP_TS_EncodingFormat.Detect_best_size: this.dataGridView_BitFields.Rows[0].Cells[i].Value = "!"; break;
                        case SHP_TS_EncodingFormat.Uncompressed_Full_Frame: this.dataGridView_BitFields.Rows[0].Cells[i].Value = "UF"; break;
                        default: this.dataGridView_BitFields.Rows[0].Cells[i].Value = "?"; break;
                    }
                }
            }
            SHP_TS_BitFlags f = GetBitField();
        }

        private SHP_TS_BitFlags GetBitField()
        {
            int t = 0;
            for (byte i = 0; i < this.dataGridView_BitFields.Rows[0].Cells.Count; i++)
            {
                if ((i != 1) && ((bool)this.dataGridView_BitFields.Rows[0].Cells[i].Value))
                    t += (int)Math.Pow((double)2, (double)i);
            }
            return (SHP_TS_BitFlags)t;
        }

        void dataGridView_BitFields_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewCell c = dataGridView_BitFields[e.ColumnIndex, e.RowIndex];
            if (!dataGridView_BitFields.Columns[e.ColumnIndex].ReadOnly)
            {
                if (c.Value == null)
                    c.Value = true;
                else
                    c.Value = !(bool)c.Value;
                dataGridView_BitFields.EndEdit();
            }
            if (this.dataGridView_BitFields.Focused)
            {
                if (this.dataGridView_Files.SelectedCells.Count > 0)
                {
                    SHP_TS_BitFlags b = GetBitField();
                    foreach (DataGridViewCell cell in this.dataGridView_Files.SelectedCells)
                        if (cell.Value != null)
                            ((CImageFile)cell.Value).BitFlags = b;
                    this.dataGridView_Files.Invalidate();
                }
            }
        }

        void dataGridView_BitFields_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            //e.PaintBackground(e.CellBounds, true);
            if (e.ColumnIndex != 1)
                e.Graphics.FillRectangle(Brushes.White, e.CellBounds);
            else
                e.Graphics.FillRectangle(Brushes.Gray, e.CellBounds);
            e.Graphics.DrawRectangle(Pens.Black, new Rectangle(new Point(e.CellBounds.X, e.CellBounds.Y), new Size(e.CellBounds.Width - 1, e.CellBounds.Height - 1)));
            Point center = new Point(e.CellBounds.X + e.CellBounds.Width / 2 - 1, e.CellBounds.Y + e.CellBounds.Height / 2 + 1);
            if (e.ColumnIndex == 1)
            {

                switch (e.Value.ToString())
                {
                    case "!":
                        {
                            for (int i = 0; i < 5; i++)
                                e.Graphics.DrawLines(Pens.Black, new Point[] { 
                                    new Point(center.X + 3+i, center.Y - 6), 
                                    new Point(center.X - 3+i, center.Y), 
                                    new Point(center.X + 3+i, center.Y + 6) });
                            break;
                        }
                    case "R":
                        {
                            //check mark
                            for (int i = 0; i < 5; i++)
                                e.Graphics.DrawLines(Pens.Black, new Point[] { new Point(center.X - 3, center.Y - 3 + i), new Point(center.X, center.Y + i), new Point(center.X + 6, center.Y - 6 + i) });
                            break;
                        }
                    case "?":
                        {
                            Font f = new Font(this.dataGridView_BitFields.Font.FontFamily, 14, FontStyle.Bold);
                            SizeF s = e.Graphics.MeasureString("?", f);
                            e.Graphics.DrawString("?", f, Brushes.Black, new Point(center.X - (int)s.Width / 2, center.Y - 1 - (int)s.Height / 2));
                            break;
                        }
                    case "UF":
                        {
                            e.Graphics.DrawRectangle(Pens.Black, new Rectangle(new Point(e.CellBounds.X + 2, e.CellBounds.Y + 2), new Size(e.CellBounds.Width - 5, e.CellBounds.Height - 5)));
                            e.Graphics.DrawRectangle(Pens.Black, new Rectangle(new Point(e.CellBounds.X + 3, e.CellBounds.Y + 3), new Size(e.CellBounds.Width - 7, e.CellBounds.Height - 7)));
                            break;
                        }
                    default: break;
                }
            }
            else
                if ((bool)e.FormattedValue)
                    for (int i = 0; i < 5; i++)
                        e.Graphics.DrawLines(Pens.Black, new Point[] { new Point(center.X - 3, center.Y - 3 + i), new Point(center.X, center.Y + i), new Point(center.X + 6, center.Y - 6 + i) });
            //ControlPaint.DrawCheckBox(e.Graphics, e.CellBounds.X - 1, e.CellBounds.Y - 1, e.CellBounds.Width + 2, e.CellBounds.Height + 2, (bool)e.FormattedValue ? ButtonState.Checked | ButtonState.Flat : ButtonState.Normal | ButtonState.Flat);
            e.Handled = true;
        }
        #endregion

        #region instantly applied changes to selected cells
        private void checkBox_RadarColorAverage_CheckedChanged(object sender, EventArgs e)
        {
            this.button_RadarColor.Enabled = !this.checkBox_RadarColorAverage.Checked;
            if (this.checkBox_RadarColorAverage.Focused)
            {
                if (this.dataGridView_Files.SelectedCells.Count > 0)
                {
                    foreach (DataGridViewCell cell in this.dataGridView_Files.SelectedCells)
                        if (cell.Value != null)
                            ((CImageFile)cell.Value).RadarColorAverage = this.checkBox_RadarColorAverage.Checked;
                }
            }
        }

        private void button_RadarColor_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            cd.Color = (Color)this.button_RadarColor.Tag;
            cd.AllowFullOpen = true;
            cd.AnyColor = true;
            cd.FullOpen = true;
            if (cd.ShowDialog() == DialogResult.OK)
            {
                this.button_RadarColor.Tag = cd.Color;
                this.button_RadarColor.Text = Cinimanager.ColorToStr(cd.Color, true);

                if (this.dataGridView_Files.SelectedCells.Count > 0)
                {
                    foreach (DataGridViewCell cell in this.dataGridView_Files.SelectedCells)
                        if (cell.Value != null)
                            ((CImageFile)cell.Value).RadarColor = cd.Color;
                }
            }
        }

        private void checkBox_OptimizeCanvas_CheckedChanged(object sender, EventArgs e)
        {
            this.checkBox_KeepCentered.Enabled = this.checkBox_OptimizeCanvas.Checked;
            if (this.checkBox_OptimizeCanvas.Focused)
                this.checkBox_KeepCentered.Checked = this.checkBox_OptimizeCanvas.Checked;
        }

        private void button_CustomBackgroundColor_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            cd.Color = (Color)this.button_CustomBackgroundColor.Tag;
            cd.AllowFullOpen = true;
            cd.AnyColor = true;
            cd.FullOpen = true;
            if (cd.ShowDialog() == DialogResult.OK)
            {
                this.button_CustomBackgroundColor.Text = Cinimanager.ColorToStr(cd.Color, true);
                this.button_CustomBackgroundColor.Tag = cd.Color;

                if (this.dataGridView_Files.SelectedCells.Count > 0)
                {
                    foreach (DataGridViewCell cell in this.dataGridView_Files.SelectedCells)
                        if (cell.Value != null)
                            ((CImageFile)cell.Value).CustomBackgroundColor = cd.Color;
                }
            }
        }

        private void checkBox_UseCustomBackgroundColor_CheckedChanged(object sender, EventArgs e)
        {
            this.button_CustomBackgroundColor.Enabled = this.checkBox_UseCustomBackgroundColor.Checked;
            if (this.checkBox_UseCustomBackgroundColor.Focused)
            {
                if (this.dataGridView_Files.SelectedCells.Count > 0)
                {
                    foreach (DataGridViewCell cell in this.dataGridView_Files.SelectedCells)
                        if (cell.Value != null)
                            ((CImageFile)cell.Value).UseCustomBackgroundColor = this.checkBox_UseCustomBackgroundColor.Checked;
                }
            }
        }

        private void checkBox_CombineTransparency_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox_CombineTransparency.Focused)
            {
                if (this.dataGridView_Files.SelectedCells.Count > 0)
                {
                    foreach (DataGridViewCell cell in this.dataGridView_Files.SelectedCells)
                        if (cell.Value != null)
                            ((CImageFile)cell.Value).CombineTransparentPixel = this.checkBox_CombineTransparency.Checked;
                }
            }
        }

        #endregion


        bool ShowPreview = false;
        Form_DockPreview form_Preview;
        private void showHidePreviewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowPreview = !ShowPreview;
            ShowHidePreview();
        }

        private void ShowHidePreview()
        {
            if (ShowPreview)
            {
                this.showHidePreviewToolStripMenuItem.Text = "Hide Preview";
                form_Preview = new Form_DockPreview();
                form_Preview.StartPosition = FormStartPosition.Manual;
                form_Preview.Location = new Point(this.Location.X + this.Width, this.Location.Y);
                form_Preview.uC_ImageCanvas1.PixelColorChanged += new EventHandler<UC_ImageCanvas.ImageCanvasDataEventArgs>(uC_ImageCanvas1_PixelColorChanged);

                UpdatePreview();
                form_Preview.Show();
            }
            else
            {
                this.showHidePreviewToolStripMenuItem.Text = "Show Preview";
                if (form_Preview != null)
                {
                    form_Preview.Close();
                    form_Preview.uC_ImageCanvas1.PixelColorChanged -= uC_ImageCanvas1_PixelColorChanged;
                    form_Preview = null;
                }
            }
        }

        void uC_ImageCanvas1_PixelColorChanged(object sender, UC_ImageCanvas.ImageCanvasDataEventArgs e)
        {
            this.uC_Palette1.PaletteSelectedColor = e.Color;
        }

        #region datagrid control
        void dataGridView1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                foreach (DataGridViewCell cell in this.dataGridView_Files.SelectedCells)
                {
                    if ((cell.ColumnIndex < this.dataGridView_Files.ColumnCount) && (cell.ColumnIndex >= 0) &&
                        (cell.RowIndex < this.dataGridView_Files.RowCount) && (cell.RowIndex >= 0))
                    {
                        cell.Value = null;
                        if (IsEmptyRow(this.dataGridView_Files.Rows[cell.RowIndex])) this.dataGridView_Files.Rows.RemoveAt(cell.RowIndex);
                    }
                }
                AddLastEmptyRow();
            }
            if ((e.Control) && (e.KeyCode == Keys.V))
            {
                LoadFromClipboard();
            }
            UpdatePreview();
        }

        private bool IsEmptyRow(DataGridViewRow row)
        {
            if (row == null) return true;
            for (int i = 0; i < row.Cells.Count; i++)
                if (row.Cells[i].Value != null) return false;
            return true;
        }

        void dataGridView1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        void dataGridView1_DragDrop(object sender, DragEventArgs e)
        {
            Point clientPoint = dataGridView_Files.PointToClient(new Point(e.X, e.Y));
            int rowindex = this.dataGridView_Files.HitTest(clientPoint.X, clientPoint.Y).RowIndex;
            int columnindex = this.dataGridView_Files.HitTest(clientPoint.X, this.dataGridView_Files.ColumnHeadersHeight + 5).ColumnIndex;

            if (columnindex == -1) columnindex = 0;
            if (columnindex >= this.dataGridView_Files.ColumnCount) return;

            string[] formats = e.Data.GetFormats();
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files != null)
                AddFilesToDataGrid(files, columnindex, rowindex);
            else
            {
                RemoveLastEmptyRow();
                //d&d selected cells
                //only when target cell is empty, the value is pasted, otherwise a new row inserted/added
                List<DGVCell> dgvscc = e.Data.GetData(typeof(List<DGVCell>)) as List<DGVCell>;

                Point cursorLocation = this.PointToClient(new Point(e.X, e.Y));

                int rowdelta = int.MaxValue;
                int coldelta = int.MaxValue;
                for (int i = 0; i < dgvscc.Count; i++)
                {
                    if (dgvscc[i].RowIndex < rowdelta) rowdelta = dgvscc[i].RowIndex;
                    if (dgvscc[i].ColumnIndex < coldelta) coldelta = dgvscc[i].ColumnIndex;
                }

                if (rowindex == -1) rowindex = this.dataGridView_Files.RowCount;
                for (int i = 0; i < dgvscc.Count; i++)
                {
                    int cellcolindex = columnindex + (dgvscc[i].ColumnIndex - coldelta);
                    int cellrowindex = rowindex + (dgvscc[i].RowIndex - rowdelta);

                    if (cellcolindex >= this.dataGridView_Files.ColumnCount) continue;

                    if ((cellrowindex < this.dataGridView_Files.RowCount) && (this.dataGridView_Files[cellcolindex, cellrowindex].Value == null))
                        this.dataGridView_Files[cellcolindex, cellrowindex].Value = dgvscc[i].Value;
                    else
                    {
                        DataGridViewRow row = (DataGridViewRow)this.dataGridView_Files.RowTemplate.Clone();
                        object[] values = new object[this.dataGridView_Files.ColumnCount];
                        values[cellcolindex] = dgvscc[i].Value;
                        row.CreateCells(this.dataGridView_Files, values);
                        if (cellrowindex < this.dataGridView_Files.RowCount)
                            this.dataGridView_Files.Rows.Insert(cellrowindex, row);
                        else
                            this.dataGridView_Files.Rows.Add(row);
                    }

                }
                AddLastEmptyRow();
            }
        }

        void dataGridView_Files_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }
        private Rectangle dragBoxFromMouseDown;
        //necessary, since the DataGridViewCell changes its properties as soon as something is changed in the DGV
        //e.g. the first pasted cell will change the rowindex of all other selected cells and thus mess up the pasting operation
        internal struct DGVCell
        {
            public DGVCell(DataGridViewCell cell)
            {
                this.Value = cell.Value;
                this.ColumnIndex = cell.ColumnIndex;
                this.RowIndex = cell.RowIndex;
            }
            public int ColumnIndex;
            public int RowIndex;
            public object Value;
        }
        private void dataGridView_Files_MouseDown(object sender, MouseEventArgs e)
        {
            // Get the index of the item the mouse is below.
            int rowindex = this.dataGridView_Files.HitTest(e.X, e.Y).RowIndex;
            int colindex = this.dataGridView_Files.HitTest(e.X, e.Y).ColumnIndex;

            if (rowindex != -1)
            {
                //only by moving outside the cell, the d&d operation starts
                //the default SystemInformation.DragSize is way too tiny and errorprone. just 4 pixel sucks for fast working people.
                dragBoxFromMouseDown = this.dataGridView_Files.GetCellDisplayRectangle(colindex, rowindex, true);
            }
            else
                // Reset the rectangle if the mouse is not over an item in the ListBox.
                dragBoxFromMouseDown = Rectangle.Empty;
        }
        void dataGridView_Files_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (dragBoxFromMouseDown != Rectangle.Empty && !dragBoxFromMouseDown.Contains(e.X, e.Y))
                {
                    List<DGVCell> cells = new List<DGVCell>();
                    for (int c = this.dataGridView_Files.ColumnCount - 1; c >= 0; c--)
                        for (int i = 0; i < this.dataGridView_Files.SelectedCells.Count; i++)
                        {
                            DGVCell cell = new DGVCell(this.dataGridView_Files.SelectedCells[i]);
                            if ((cell.Value != null) && (cell.ColumnIndex == c))
                                cells.Insert(0, cell);
                        }
                    this.dataGridView_Files.DoDragDrop(cells, DragDropEffects.Copy);
                }
            }
        }

        List<DGVCell> Cells2Copy;
        void DataGridCell_Copy(object sender, EventArgs e)
        {
            Cells2Copy = new List<DGVCell>();
            for (int c = this.dataGridView_Files.ColumnCount - 1; c >= 0; c--)
                for (int i = 0; i < this.dataGridView_Files.SelectedCells.Count; i++)
                {
                    DGVCell cell = new DGVCell(this.dataGridView_Files.SelectedCells[i]);
                    if ((cell.Value != null) && (cell.ColumnIndex == c))
                        Cells2Copy.Insert(0, cell);
                }
        }

        void DataGridCell_Cut(object sender, EventArgs e)
        {
            Cells2Copy = new List<DGVCell>();
            for (int c = this.dataGridView_Files.ColumnCount - 1; c >= 0; c--)
                for (int i = 0; i < this.dataGridView_Files.SelectedCells.Count; i++)
                {
                    DGVCell cell = new DGVCell(this.dataGridView_Files.SelectedCells[i]);
                    if ((cell.Value != null) && (cell.ColumnIndex == c))
                        Cells2Copy.Insert(0, cell);
                }

            foreach (DataGridViewCell cell in this.dataGridView_Files.SelectedCells)
                cell.Value = null;
        }

        void DataGridCell_Paste(object sender, EventArgs e)
        {
            if ((targetCell.X >= 0) && (targetCell.Y >= 0))
            {
                int rowindex = targetCell.Y;
                int columnindex = targetCell.X;

                int rowdelta = int.MaxValue;
                int coldelta = int.MaxValue;
                for (int i = 0; i < Cells2Copy.Count; i++)
                {
                    if (Cells2Copy[i].RowIndex < rowdelta) rowdelta = Cells2Copy[i].RowIndex;
                    if (Cells2Copy[i].ColumnIndex < coldelta) coldelta = Cells2Copy[i].ColumnIndex;
                }

                if (rowindex == -1) rowindex = this.dataGridView_Files.RowCount;
                for (int i = 0; i < Cells2Copy.Count; i++)
                {
                    int cellcolindex = columnindex + (Cells2Copy[i].ColumnIndex - coldelta);
                    int cellrowindex = rowindex + (Cells2Copy[i].RowIndex - rowdelta);

                    if (cellcolindex >= this.dataGridView_Files.ColumnCount) continue;

                    if ((cellrowindex < this.dataGridView_Files.RowCount) && (this.dataGridView_Files[cellcolindex, cellrowindex].Value == null))
                        this.dataGridView_Files[cellcolindex, cellrowindex].Value = Cells2Copy[i].Value;
                    else
                    {
                        DataGridViewRow row = (DataGridViewRow)this.dataGridView_Files.RowTemplate.Clone();
                        object[] values = new object[this.dataGridView_Files.ColumnCount];
                        values[cellcolindex] = Cells2Copy[i].Value;
                        row.CreateCells(this.dataGridView_Files, values);
                        if (cellrowindex < this.dataGridView_Files.RowCount)
                            this.dataGridView_Files.Rows.Insert(cellrowindex, row);
                        else
                            this.dataGridView_Files.Rows.Add(row);
                    }

                }
                AddLastEmptyRow();
            }
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);
        private const int WM_SETREDRAW = 11;
        BackgroundWorker LoadFilesBW;
        private void AddFilesToDataGrid(string[] filenames, int columnindex, int rowindex)
        {
            if ((LoadFilesBW != null) && (LoadFilesBW.IsBusy))
            {
                if (MessageBox.Show("File loading in progress!\nDo you want to abort that process and instead add the new files?", "File loading in progress", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    LoadFilesBW.CancelAsync();
                else
                    return;
            }

            if (this.dataGridView_Files.SelectedCells.Count > 1)
            {
                int SelectedCellsInOneColumn = 0;
                int columnnr = -1;
                int toprowindex = int.MaxValue;
                foreach (DataGridViewCell c in this.dataGridView_Files.SelectedCells)
                {
                    SelectedCellsInOneColumn++;
                    if (c.RowIndex < toprowindex) toprowindex = c.RowIndex;
                    if (columnnr == -1)
                        columnnr = c.ColumnIndex;
                    else
                        if (columnnr != c.ColumnIndex)
                        {
                            SelectedCellsInOneColumn = -1;
                            break;
                        }
                }
                //don't do anything if the user selected cells across multiple columns
                //this works for cells in a single column only!
                if ((SelectedCellsInOneColumn > 0) && (SelectedCellsInOneColumn > filenames.Length))
                {
                    if (MessageBox.Show("Do you wish to duplicate the " + filenames.Length.ToString() + " file(s) to fill the " + SelectedCellsInOneColumn.ToString() + " selected cells?", "Duplicate files?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        string[] dupfilenames = new string[SelectedCellsInOneColumn];
                        for (int i = 0; i < dupfilenames.Length; i++)
                            dupfilenames[i] = filenames[i % filenames.Length];
                        filenames = dupfilenames;
                        rowindex = toprowindex;
                    }
                }
            }


            this.richTextBox_Reports.Clear();
            int palindex = -1;
            palindex = PaletteManager.GetPaletteIndex(this.uC_Palette1.Palette, false);

            this.progressBar1.Minimum = 0;
            this.progressBar1.Maximum = filenames.Length;

            this.richTextBox_Reports.SelectedText = "adding " + filenames.Length + " files...";


            SHP_TS_EncodingFormat format = (SHP_TS_EncodingFormat)this.comboBox_Compression.SelectedIndex;
            SHP_TS_BitFlags bitflags = GetBitField();
            bool checkBox_RadarColorAverage = this.checkBox_RadarColorAverage.Checked;
            bool checkBox_UseCustomBackgroundColor = this.checkBox_UseCustomBackgroundColor.Checked;
            Color button_CustomBackgroundColor = (Color)this.button_CustomBackgroundColor.Tag;
            bool checkBox_CombineTransparency = this.checkBox_CombineTransparency.Checked;

            LoadFilesBW = new BackgroundWorker();
            LoadFilesBW.DoWork += (w_s, w_e) =>
            {
                BackgroundWorker worker = (BackgroundWorker)w_s;
                CFiles2Load job = w_e.Argument as CFiles2Load;
                DataGridView tmpdgv = job.dgv;
                w_e.Result = "Loading-Files-Worker stopped unfinished";
                int SHPFrameCount = 0;

                for (int f = 0; f < job.files.Length; f++)
                {
                    //System.Threading.Thread.Sleep(1000);
                    string file = job.files[f];
                    if (!worker.CancellationPending)
                        worker.ReportProgress(f, "");
                    else
                    {
                        w_e.Cancel = true;
                        return;
                    }

                    bool IsSHP = false;
                    //ignore unsupported files by testing each file if we can use it as Bitmap
                    try
                    {
                        Bitmap test = (Bitmap)Image.FromFile(file);
                    }
                    catch
                    {
                        IsSHP = CSHaPer.IsSHP(file);
                        //if its no image and no SHP, skip this
                        if (!IsSHP)
                            continue;
                    }

                    if (IsSHP)
                    {
                        CImageFile[] SHPFrames = CSHaPer.GetFrames(file, palindex);
                        DataGridViewRow row;
                        for (int shp_i = 0; shp_i < SHPFrames.Length; shp_i++)
                        {
                            CImageFile SHPFrame = SHPFrames[shp_i];
                            SHPFrame.RadarColorAverage = checkBox_RadarColorAverage;
                            if ((rowindex == -1) || ((rowindex >= 0) && (rowindex + (f + SHPFrameCount) + shp_i >= tmpdgv.Rows.Count)))
                            {
                                row = (DataGridViewRow)tmpdgv.RowTemplate.Clone();
                                object[] values = new object[3];
                                for (int i = 0; i < values.Length; i++)
                                    if (i == columnindex) values[i] = SHPFrame;
                                    else values[i] = null;

                                row.CreateCells(tmpdgv, values);
                                tmpdgv.Rows.Add(row);
                            }
                            else
                            {
                                row = tmpdgv.Rows[rowindex + (f + SHPFrameCount) + shp_i];
                                row.Cells[columnindex].Value = SHPFrame;
                            }
                        }
                        SHPFrameCount += SHPFrames.Length - 1;//f for the files counter already is 1 for the SHP itself. e.g. for an SHP with only 1 frame, SHPFrameCount doesn't need to be raised
                    }
                    else
                    {
                        DataGridViewRow row;
                        CImageFile cif = new CImageFile(file, palindex, format);
                        cif.UseCustomBackgroundColor = checkBox_UseCustomBackgroundColor;
                        cif.CustomBackgroundColor = button_CustomBackgroundColor;
                        cif.RadarColorAverage = checkBox_RadarColorAverage;
                        cif.CombineTransparentPixel = checkBox_CombineTransparency;
                        cif.BitFlags = bitflags;
                        if ((rowindex == -1) || ((rowindex >= 0) && (rowindex + (f + SHPFrameCount) >= tmpdgv.Rows.Count)))
                        {
                            row = (DataGridViewRow)tmpdgv.RowTemplate.Clone();
                            object[] values = new object[3];
                            for (int i = 0; i < values.Length; i++)
                                if (i == columnindex) values[i] = cif;
                                else values[i] = null;

                            row.CreateCells(tmpdgv, values);
                            tmpdgv.Rows.Add(row);
                        }
                        else
                        {
                            row = tmpdgv.Rows[rowindex + (f + SHPFrameCount)];
                            row.Cells[columnindex].Value = cif;
                        }
                    }
                }

                w_e.Result = tmpdgv;
            };

            LoadFilesBW.ProgressChanged += (w_s, w_e) =>
            {
                if (w_e.ProgressPercentage != -1)
                {
                    this.progressBar1.Value = w_e.ProgressPercentage;
                    if ((w_e.UserState != null) && (w_e.UserState.ToString() != ""))
                        this.richTextBox_Reports.SelectedText = w_e.UserState.ToString();
                }
            };

            //throw new Exception("this.dataGridView_Files.Rows.Clear(); is shit. add only changes so the selected cells and the current scrollbar location are kept");
            LoadFilesBW.RunWorkerCompleted += (w_s, w_e) =>
            {
                if ((!w_e.Cancelled) && (w_e.Error == null))
                {
                    SendMessage(this.dataGridView_Files.Handle, WM_SETREDRAW, false, 0);
                    //this.dataGridView_Files.Rows.Clear();
                    DataGridView wdgv = (DataGridView)w_e.Result;
                    DataGridViewRow row = new DataGridViewRow();
                    for (int i = 0; i < wdgv.Rows.Count; i++)
                    {
                        if (i < this.dataGridView_Files.Rows.Count)
                        {
                            for (int c = 0; c < wdgv.Rows[i].Cells.Count; c++)
                                this.dataGridView_Files.Rows[i].Cells[c].Value = wdgv.Rows[i].Cells[c].Value;
                        }
                        else
                        {
                            row = (System.Windows.Forms.DataGridViewRow)wdgv.Rows[i].Clone();
                            for (int c = 0; c < wdgv.Rows[i].Cells.Count; c++)
                                row.Cells[c].Value = wdgv.Rows[i].Cells[c].Value;

                            this.dataGridView_Files.Rows.Add(row);
                        }
                    }
                    AddLastEmptyRow();

                    SendMessage(this.dataGridView_Files.Handle, WM_SETREDRAW, true, 0);
                    this.dataGridView_Files.Refresh();

                    this.progressBar1.Value = 0;
                    this.richTextBox_Reports.SelectedText = " done." + Environment.NewLine;
                }
                else
                {
                    if (w_e.Error != null)
                    {
                        this.richTextBox_Reports.SelectionColor = Color.Red;
                        this.richTextBox_Reports.SelectedText = "PreviewWorkerError:" + w_e.Error.Message + Environment.NewLine;
                    }
                }
            };

            LoadFilesBW.WorkerReportsProgress = true;
            LoadFilesBW.WorkerSupportsCancellation = true;

            RemoveLastEmptyRow();
            LoadFilesBW.RunWorkerAsync(new CFiles2Load(filenames, this.dataGridView_Files));
        }


        /// <summary>
        /// synchronous loading for command line file load
        /// </summary>
        private void AddFilesToDataGridSync(string[] filenames, int columnindex, int rowindex, bool setSHPBits, bool setSHPCompression)
        {
            int palindex = -1;
            palindex = PaletteManager.GetPaletteIndex(this.uC_Palette1.Palette, false);

            this.progressBar1.Minimum = 0;
            this.progressBar1.Maximum = filenames.Length;

            Console.Write("adding " + filenames.Length + " files...");


            SHP_TS_EncodingFormat compressionformat = (SHP_TS_EncodingFormat)this.comboBox_Compression.SelectedIndex;
            SHP_TS_BitFlags bitflags = GetBitField();
            bool checkBox_RadarColorAverage = this.checkBox_RadarColorAverage.Checked;
            bool checkBox_UseCustomBackgroundColor = this.checkBox_UseCustomBackgroundColor.Checked;
            Color button_CustomBackgroundColor = (Color)this.button_CustomBackgroundColor.Tag;
            bool checkBox_CombineTransparency = this.checkBox_CombineTransparency.Checked;


            RemoveLastEmptyRow();
            int SHPFrameCount = 0;

            SendMessage(this.dataGridView_Files.Handle, WM_SETREDRAW, false, 0);

            for (int f = 0; f < filenames.Length; f++)
            {
                string file = filenames[f];

                bool IsSHP = false;
                //ignore unsupported files by testing each file if we can use it as Bitmap
                try
                {
                    Bitmap test = (Bitmap)Image.FromFile(file);
                }
                catch
                {
                    IsSHP = CSHaPer.IsSHP(file);
                    //if its no image and no SHP, skip this
                    if (!IsSHP)
                        continue;
                }

                if (IsSHP)
                {
                    CImageFile[] SHPFrames = CSHaPer.GetFrames(file, palindex);
                    DataGridViewRow row;
                    for (int shp_i = 0; shp_i < SHPFrames.Length; shp_i++)
                    {
                        CImageFile SHPFrame = SHPFrames[shp_i];
                        if (setSHPBits) SHPFrame.BitFlags = bitflags;
                        if (setSHPCompression) SHPFrame.CompressionFormat = compressionformat;
                        SHPFrame.RadarColorAverage = checkBox_RadarColorAverage;
                        if ((rowindex == -1) || ((rowindex >= 0) && (rowindex + (f + SHPFrameCount) + shp_i >= this.dataGridView_Files.Rows.Count)))
                        {
                            row = (DataGridViewRow)this.dataGridView_Files.RowTemplate.Clone();
                            object[] values = new object[3];
                            for (int i = 0; i < values.Length; i++)
                                if (i == columnindex) values[i] = SHPFrame;
                                else values[i] = null;

                            row.CreateCells(this.dataGridView_Files, values);
                            this.dataGridView_Files.Rows.Add(row);
                        }
                        else
                        {
                            row = this.dataGridView_Files.Rows[rowindex + (f + SHPFrameCount) + shp_i];
                            row.Cells[columnindex].Value = SHPFrame;
                        }
                    }
                    SHPFrameCount += SHPFrames.Length - 1;//f for the files counter already is 1 for the SHP itself. e.g. for an SHP with only 1 frame, SHPFrameCount doesn't need to be raised
                }
                else
                {
                    DataGridViewRow row;
                    CImageFile cif = new CImageFile(file, palindex, compressionformat);
                    cif.UseCustomBackgroundColor = checkBox_UseCustomBackgroundColor;
                    cif.CustomBackgroundColor = button_CustomBackgroundColor;
                    cif.RadarColorAverage = checkBox_RadarColorAverage;
                    cif.CombineTransparentPixel = checkBox_CombineTransparency;
                    cif.BitFlags = bitflags;
                    if ((rowindex == -1) || ((rowindex >= 0) && (rowindex + (f + SHPFrameCount) >= this.dataGridView_Files.Rows.Count)))
                    {
                        row = (DataGridViewRow)this.dataGridView_Files.RowTemplate.Clone();
                        object[] values = new object[3];
                        for (int i = 0; i < values.Length; i++)
                            if (i == columnindex) values[i] = cif;
                            else values[i] = null;

                        row.CreateCells(this.dataGridView_Files, values);
                        this.dataGridView_Files.Rows.Add(row);
                    }
                    else
                    {
                        row = this.dataGridView_Files.Rows[rowindex + (f + SHPFrameCount)];
                        row.Cells[columnindex].Value = cif;
                    }
                }
            }

            AddLastEmptyRow();

            SendMessage(this.dataGridView_Files.Handle, WM_SETREDRAW, true, 0);
            this.dataGridView_Files.Refresh();

            this.progressBar1.Value = 0;
            Console.WriteLine(" done.");
        }

        private Point targetCell;
        void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {
            targetCell = new Point(-1, -1);

            if (e.Button == MouseButtons.Right)
            {
                //Point clientPoint = dataGridView1.PointToClient(new Point(e.X, e.Y));
                int rowindex = this.dataGridView_Files.HitTest(e.X, e.Y).RowIndex;
                int columnindex = this.dataGridView_Files.HitTest(e.X, e.Y).ColumnIndex;
                targetCell = new Point(columnindex, rowindex);
                dataGrid_CM.Show(dataGridView_Files, new Point(e.X, e.Y));
            }

            if (e.Button == MouseButtons.Left)
                UpdatePreview();
        }

        void DataGridCell_LoadImages(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Load Images";
            ofd.InitialDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            ofd.FileName = "";
            ofd.Filter = "Image files|*.png;*.bmp;*.shp";
            ofd.Multiselect = true;
            if (ofd.ShowDialog() != DialogResult.Cancel)
            {
                int rowindex = targetCell.Y;
                int columnindex = targetCell.X;
                if (columnindex == -1) columnindex = 0;
                AddFilesToDataGrid(ofd.FileNames, columnindex, rowindex);
            }
        }

        void DataGridCell_LoadSplitImage(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Load Image";
            ofd.InitialDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            ofd.FileName = "";
            ofd.Filter = "Image file|*.png;*.bmp";
            ofd.Multiselect = false;
            if (ofd.ShowDialog() != DialogResult.Cancel)
            {
                int rowindex = targetCell.Y;
                int columnindex = targetCell.X;
                if (columnindex == -1) columnindex = 0;

                Form_LoadAndSplitImage lasi = new Form_LoadAndSplitImage(ofd.FileName, GetProgramTempPath);
                lasi.Icon = this.Icon;
                lasi.StartPosition = FormStartPosition.Manual;
                lasi.Location = this.PointToScreen(this.customMenuStrip1.Location);
                if (lasi.ShowDialog() == DialogResult.OK)
                    AddFilesToDataGrid(lasi.images, columnindex, rowindex);
            }
        }

        void DataGridCell_ReverseOrder(object sender, EventArgs e)
        {
            List<DGVCell> Cells2Reverse = new List<DGVCell>();
            for (int c = this.dataGridView_Files.ColumnCount - 1; c >= 0; c--)
                for (int i = 0; i < this.dataGridView_Files.SelectedCells.Count; i++)
                {
                    DGVCell cell = new DGVCell(this.dataGridView_Files.SelectedCells[i]);
                    if ((cell.Value != null) && (cell.ColumnIndex == c))
                        Cells2Reverse.Insert(0, cell);
                }

            for (int i = 0; i < Cells2Reverse.Count; i++)
            {
                int ii = Cells2Reverse.Count - 1 - i;
                this.dataGridView_Files[Cells2Reverse[ii].ColumnIndex, Cells2Reverse[ii].RowIndex].Value = Cells2Reverse[i].Value;
            }
        }

        void DataGridCell_SetPalette(object sender, EventArgs e)
        {
            if (this.dataGridView_Files.SelectedCells.Count > 0)
            {
                int palindex = -1;
                palindex = PaletteManager.GetPaletteIndex(this.uC_Palette1.Palette, false);


                foreach (DataGridViewCell cell in this.dataGridView_Files.SelectedCells)
                    if (cell.Value != null)
                        ((CImageFile)cell.Value).PaletteIndex = palindex;
                this.dataGridView_Files.Invalidate();
                UpdatePreview();
            }
        }
        void DataGridCell_SetCompression(object sender, EventArgs e)
        {
            if (this.dataGridView_Files.SelectedCells.Count > 0)
            {
                SHP_TS_EncodingFormat f = (SHP_TS_EncodingFormat)this.comboBox_Compression.SelectedIndex;
                SHP_TS_BitFlags b = GetBitField();
                foreach (DataGridViewCell cell in this.dataGridView_Files.SelectedCells)
                    if (cell.Value != null)
                    {
                        ((CImageFile)cell.Value).CompressionFormat = f;
                        ((CImageFile)cell.Value).BitFlags = b;
                    }
                this.dataGridView_Files.Invalidate();
            }
        }
        void DataGridCell_LoadFromClipboard(object sender, EventArgs e)
        {
            LoadFromClipboard();
        }
        private void LoadFromClipboard()
        {
            try
            {
                Image img = Clipboard.GetImage();
                if (img == null) return;
                string clipboardfilename = "tmpclipb" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png";
                string filename = System.IO.Path.Combine(GetProgramTempPath, clipboardfilename);
                img.Save(filename);

                int rowindex = targetCell.Y;
                int columnindex = targetCell.X;
                if (columnindex == -1) columnindex = 0;
                AddFilesToDataGrid(new string[] { filename }, columnindex, rowindex);
            }
            catch (Exception ex)
            {
                this.richTextBox_Reports.SelectionColor = Color.Red;
                this.richTextBox_Reports.SelectedText = ex.Message;
            }
        }

        #endregion

        private void button_Start_Click(object sender, EventArgs e)
        {
            CreatSHP(false);
        }

        private void CreatSHP(bool CloseWhenFinished)
        {
            bool PreventWobbleBug = this.checkBox_PreventWobbleBug.Checked;
            bool optimizeCanvas = this.checkBox_OptimizeCanvas.Checked;
            bool keepCentered = this.checkBox_KeepCentered.Checked;
            bool CreateFrameFiles = this.checkBox_FrameFiles.Checked;
            int max_Worker = (int)this.toolStripMenuItem_Label_NUD_NrWorker.ToolStrip_UC_Label_NUD.Value;
            SHP_TS_EncodingFormat DefaultCompression = GetDefaultCompression;
            string tmpfilename = this.textBox_CreateFiles.Text;
            string tmpfileformat = this.comboBox_CreateFilesFormat.SelectedItem.ToString();

            int SplitResultCount = (int)this.numericUpDown_SplitResult.Value;

            this.richTextBox_Reports.Clear();
            this.richTextBox_Reports.Focus();

            Stopwatch duration = Stopwatch.StartNew();

            string outputfolder = GetProgramPath; // Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string ouputfolder_Temp = GetProgramTempPath; // Path.Combine(outputfolder, "Temp");
            if (this.toolStripMenuItem_Outputfolder.ToolStrip_UC_FolderSelector.Value != "")
                outputfolder = this.toolStripMenuItem_Outputfolder.ToolStrip_UC_FolderSelector.Value;

            try
            {
                if (!Directory.Exists(ouputfolder_Temp))
                {
                    Directory.CreateDirectory(ouputfolder_Temp);
                }

                //no longer necessary to delete the files, since they aren't used anymore for conversion
                //DirectoryInfo di = new DirectoryInfo(targetpath);

                //string extension = "";
                //switch (tmpfileformat.ToLower())
                //{
                //    case "png": extension = ".png"; break;
                //    case "bmp": extension = ".bmp"; break;
                //    case "gif": extension = ".gif"; break;
                //    case "tiff": extension = ".tiff"; break;
                //    case "shp(ts)": extension = ".shp"; break;
                //    default: extension = ".shp"; break;
                //}

                //foreach (FileInfo file in di.GetFiles("*" + extension))
                //    file.Delete();
            }
            catch (Exception ex)
            {
                this.richTextBox_Reports.SelectionColor = Color.Red;
                this.richTextBox_Reports.SelectedText = ex.Message;
                return;
            }


            string SHPFilename = "result";
            string SHPFileExtension = ".shp"; //default is .shp, but when a .tem file in SHP format is loaded, keep that extension instead
            bool DoTrim = true;
            for (int r = 0; r < this.dataGridView_Files.Rows.Count; r++)
            {
                if (this.dataGridView_Files.Rows[r].Cells[0].Value != null)
                {
                    SHPFilename = ((CImageFile)this.dataGridView_Files.Rows[r].Cells[0].Value).FileName;
                    if (((CImageFile)this.dataGridView_Files.Rows[r].Cells[0].Value).IsSHP)
                    {
                        DoTrim = false;
                        //remember for SHPs the extension, because they can be also called .tem etc
                        SHPFileExtension = Path.GetExtension(SHPFilename);
                    }
                }
                if (SHPFilename != "") break;
            }
            SHPFilename = Path.GetFileNameWithoutExtension(SHPFilename);
            //trim trailing frame numbers only if the first image is not an SHP
            //for SHPs keep the filename exactly the same
            if (DoTrim)
            {
                //first remove the trailing frame numbers
                SHPFilename = SHPFilename.TrimEnd(new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' });
                //then remove trailing spaces or underlines (this way allowing "myfile01_00001.png" to result in the SHP name "myfile01.shp" with a trailing number)
                SHPFilename = SHPFilename.TrimEnd(new char[] { ' ', '_' });
            }

            //collect the interface data into a package that can be send to the worker thread
            List<CImageJob> jobs = new List<CImageJob>();
            int frameNr = 0;
            for (int r = 0; r < this.dataGridView_Files.Rows.Count; r++)
            {

                List<CImageFile> files = new List<CImageFile>();
                for (int i = 0; i < this.dataGridView_Files.Rows[r].Cells.Count; i++)
                    if (this.dataGridView_Files.Rows[r].Cells[i].Value != null)
                    {
                        CImageFile cell = (CImageFile)this.dataGridView_Files.Rows[r].Cells[i].Value;
                        files.Add(cell);
                        if (cell.PaletteIndex == -1)
                        {
                            this.richTextBox_Reports.SelectionColor = Color.Red;
                            this.richTextBox_Reports.SelectedText = "Image from Row[" + r.ToString() + "] Column[" + i.ToString() + "] has no palette assigned!";
                            return;
                        }
                    }
                if (files.Count > 0)
                {
                    jobs.Add(new CImageJob(ouputfolder_Temp, frameNr, tmpfilename, tmpfileformat, files, CreateFrameFiles, DefaultCompression));
                    frameNr++;
                }
            }
            if (jobs.Count == 0)
            {
                if (CloseWhenFinished)
                    this.Close();
                return;
            }
            if (jobs.Count > ushort.MaxValue)
            {
                this.richTextBox_Reports.SelectionColor = Color.Red;
                this.richTextBox_Reports.SelectedText = jobs.Count.ToString() + " frames detected! SHP format supports only max " + ushort.MaxValue.ToString() + " frames";
                return;
            }

            this.button_Start.Enabled = false;
            this.progressBar1.Minimum = 0;
            this.progressBar1.Maximum = this.dataGridView_Files.Rows.Count;

            if (!CreateFrameFiles) this.richTextBox_Reports.SelectedText = "Creating frames" + Environment.NewLine;
            else this.richTextBox_Reports.SelectedText = "Creating palette indexed files in folder:" + Environment.NewLine + ouputfolder_Temp + Environment.NewLine;

            //create the worker and divide the imagejobs in small parts that get assigned to each worker
            List<BackgroundWorker> bw = new List<BackgroundWorker>();
            int jobsize = jobs.Count / max_Worker + jobs.Count % max_Worker;
            int finished_Worker = 0;

            //initialized with same length as jobs, so we can insert the in random order returned images at the right place, without having to sort them afterwards
            CImageResult[] convertedframes = new CImageResult[jobs.Count];

            for (int i = 0; i < max_Worker; i++)
            {
                List<CImageJob> jobspart = new List<CImageJob>();
                for (int j = 0; j < jobsize; j++)
                    if (jobs.Count > i * jobsize + j)
                        jobspart.Add(jobs[i * jobsize + j]);

                CWorkerJob workerJob = new CWorkerJob(jobspart, i);

                bw.Add(new BackgroundWorker());
                bw[i].DoWork += (w_s, w_e) =>
                {
                    BackgroundWorker worker = (BackgroundWorker)w_s;
                    CWorkerJob wJ = w_e.Argument as CWorkerJob;

                    w_e.Result = "Worker " + wJ.workerID.ToString() + " stopped unfinished";
                    CConverter c = new CConverter();
                    for (int j = 0; j < wJ.imagejobs.Count; j++)
                    {
                        try
                        {
                            CImageResult cac = c.CombineAndConvert(wJ.imagejobs[j].files);
                            Bitmap img = cac.bmp;
                            if (wJ.imagejobs[j].files[0].RadarColorAverage) wJ.imagejobs[j].files[0].RadarColor = cac.RadarColor;

                            if (wJ.imagejobs[j].CreateImageFile)
                            {
                                ImageFormat imfo = getImageFormat(wJ.imagejobs[j].tmpfileformat);
                                if ((imfo != null) && (wJ.imagejobs[j].tmpfileformat != "SHP(TS)"))
                                    img.Save(wJ.imagejobs[j].outputfilename + "." + wJ.imagejobs[j].tmpfileformat, imfo);
                                else
                                    CSHaPer.CreateSHP(wJ.imagejobs[j].outputfilename + ".SHP", new CImageResult[] { cac }, PreventWobbleBug, optimizeCanvas, keepCentered);
                            }
                            //the first image file sets the encoding format (previous checks make sure, that at this point is always a first file present)
                            SHP_TS_EncodingFormat format = wJ.imagejobs[j].files[0].CompressionFormat;
                            if (format == SHP_TS_EncodingFormat.Undefined) format = wJ.imagejobs[j].DefaultCompression;
                            string report = " created";
                            if (!wJ.imagejobs[j].CreateImageFile) report = " processed";
                            worker.ReportProgress(j, new CImageResult(img,
                                wJ.imagejobs[j].frameNr, format,
                                wJ.imagejobs[j].files[0].BitFlags,
                                wJ.imagejobs[j].files[0].RadarColor,
                                wJ.imagejobs[j].tmpfilename + report + Environment.NewLine));
                        }
                        catch (Exception ex)
                        {
                            worker.ReportProgress(-1, "Job#" + j.ToString() + " failed!\n" + ex.Message);
                            return;
                        }
                    }
                    w_e.Result = "Worker " + wJ.workerID.ToString() + " finished";
                };

                bw[i].ProgressChanged += (w_s, w_e) =>
                {
                    if (w_e.ProgressPercentage != -1)
                    {
                        CImageResult wr = (CImageResult)w_e.UserState;

                        if (wr.frameNr < convertedframes.Length)
                            convertedframes[wr.frameNr] = wr;

                        this.richTextBox_Reports.SelectedText = wr.message;
                        if (this.progressBar1.Value + 1 < this.progressBar1.Maximum)
                            this.progressBar1.Value += 1;
                    }
                    else
                    {
                        this.richTextBox_Reports.SelectionColor = Color.Red;
                        this.richTextBox_Reports.SelectedText = "Worker Error:" + w_e.UserState.ToString();
                    }
                };

                bw[i].RunWorkerCompleted += (w_s, w_e) =>
                {
                    if ((!w_e.Cancelled) && (w_e.Error == null))
                    {
                        this.richTextBox_Reports.SelectedText = w_e.Result.ToString() + Environment.NewLine;
                        finished_Worker++;
                        if (finished_Worker >= max_Worker)
                        {
                            this.progressBar1.Value = 0;
                            this.richTextBox_Reports.SelectedText = "___ creating SHP ___" + Environment.NewLine;

                            //lets take here the convertedframes and create the SHP
                            try
                            {
                                int splitFramesCount = convertedframes.Length / SplitResultCount;
                                int maxdigits = (int)Math.Floor(Math.Log10(SplitResultCount) + 1);

                                if (SplitResultCount <= convertedframes.Length)
                                    for (int r = 0; r < SplitResultCount; r++)
                                    {
                                        string SHPFilenameResult = SHPFilename + SHPFileExtension;
                                        if (SplitResultCount > 1) SHPFilenameResult = SHPFilename + "_" + r.ToString().PadLeft(maxdigits, '0') + SHPFileExtension;

                                        CImageResult[] SplitFrames = new CImageResult[splitFramesCount];
                                        Array.Copy(convertedframes, r * splitFramesCount, SplitFrames, 0, splitFramesCount);

                                        CSHaPer.CreateSHP(Path.Combine(outputfolder, SHPFilenameResult), SplitFrames, PreventWobbleBug, optimizeCanvas, keepCentered);
                                        this.richTextBox_Reports.SelectedText = "SHP File [" + SHPFilenameResult + "] created in output folder:" + Environment.NewLine;
                                        this.richTextBox_Reports.SelectedText = "\t" + outputfolder + Environment.NewLine;
                                        if (RunAsCommand)
                                            Console.WriteLine("SHP File [" + SHPFilenameResult + "] created in output folder:" + outputfolder);
                                        PlaySound();
                                    }
                                else
                                {
                                    this.richTextBox_Reports.SelectionColor = Color.Red;
                                    this.richTextBox_Reports.SelectedText = "Can't split " + convertedframes.Length.ToString() + " frames into " + SplitResultCount.ToString() + " files." + Environment.NewLine;
                                }
                                //CSHaPer.CreateSHP(Path.Combine(programpath, SHPFilename), convertedframes, PreventWobbleBug, optimizeCanvas, keepCentered);
                                //this.richTextBox_Reports.SelectedText = "SHP File [" + SHPFilename + "] created" + Environment.NewLine;
                            }
                            catch (Exception ex)
                            {
                                this.richTextBox_Reports.SelectionColor = Color.Red;
                                this.richTextBox_Reports.SelectedText = "CSHaPer.CreateSHP Error:" + ex.Message + Environment.NewLine;
                                if (RunAsCommand)
                                    Console.WriteLine("CSHaPer.CreateSHP Error:" + ex.Message);
                            }

                            duration.Stop();
                            this.richTextBox_Reports.SelectedText = "Duration: " + duration.Elapsed.TotalSeconds.ToString("0.0sec") + Environment.NewLine;
                            try
                            {
                                using (TextWriter w = File.CreateText(Path.Combine(ouputfolder_Temp, "#conversionlog.txt")))
                                {
                                    w.Write(this.richTextBox_Reports.Text);
                                }
                            }
                            catch (Exception ex)
                            {
                                this.richTextBox_Reports.SelectionColor = Color.Red;
                                this.richTextBox_Reports.SelectedText = "Log Error:" + ex.Message + Environment.NewLine;
                            }
                            this.button_Start.Enabled = true;
                            this.dataGridView_Files.Focus();

                            if (CloseWhenFinished)
                            {
                                System.Threading.Thread.Sleep(1000);
                                this.Close();
                            }
                        }
                    }
                    else
                    {
                        if (w_e.Error != null)
                        {
                            this.richTextBox_Reports.SelectionColor = Color.Red;
                            this.richTextBox_Reports.SelectedText = w_e.Error.Message + Environment.NewLine;
                        }
                    }
                };

                bw[i].WorkerReportsProgress = true;

                bw[i].RunWorkerAsync(workerJob);
            }
        }

        BackgroundWorker PreviewBW;
        private void UpdatePreview()
        {
            List<CImageFile> files = new List<CImageFile>();
            if (this.dataGridView_Files.SelectedRows.Count > 0)
            {
                for (int i = 0; i < this.dataGridView_Files.SelectedRows[0].Cells.Count; i++)
                    if (this.dataGridView_Files.SelectedRows[0].Cells[i].Value != null)
                    {
                        files.Add((CImageFile)this.dataGridView_Files.SelectedRows[0].Cells[i].Value);
                    }
            }
            else
                if (this.dataGridView_Files.SelectedCells.Count > 0)
                    if (this.dataGridView_Files.SelectedCells[0].Value != null)
                        files.Add((CImageFile)this.dataGridView_Files.SelectedCells[0].Value);

            if (files.Count > 0)
            {
                if (files[0].PaletteIndex != -1)
                    this.uC_Palette1.Palette = PaletteManager.GetPalette(files[0].PaletteIndex);

                this.button_RadarColor.Tag = files[0].RadarColor;
                this.button_RadarColor.Text = Cinimanager.ColorToStr(files[0].RadarColor, true);
                this.checkBox_RadarColorAverage.Checked = files[0].RadarColorAverage;

                this.checkBox_CombineTransparency.Checked = files[0].CombineTransparentPixel;
                this.checkBox_UseCustomBackgroundColor.Checked = files[0].UseCustomBackgroundColor;
                this.button_CustomBackgroundColor.Tag = files[0].CustomBackgroundColor;
                this.button_CustomBackgroundColor.Text = Cinimanager.ColorToStr(files[0].CustomBackgroundColor, true);

                this.comboBox_Compression.SelectedIndex = (int)files[0].CompressionFormat;
                SetBitField(files[0].BitFlags, files[0].CompressionFormat);

                if ((form_Preview != null) && (ShowPreview))
                {
                    if ((this.toolStripMenuItem_previewBackgroundImage.ToolStrip_UC_FolderSelector.Value != "")
                     && (System.IO.File.Exists(this.toolStripMenuItem_previewBackgroundImage.ToolStrip_UC_FolderSelector.Value)))
                    {
                        try
                        {
                            form_Preview.SetBackgroundImage(Image.FromFile(this.toolStripMenuItem_previewBackgroundImage.ToolStrip_UC_FolderSelector.Value));
                        }
                        catch (Exception ex)
                        {
                            form_Preview.SetBackgroundImage(null);
                            this.richTextBox_Reports.SelectionColor = Color.Red;
                            this.richTextBox_Reports.SelectedText = "Background Image Error:" + ex.Message + Environment.NewLine;
                        }
                    }
                    else
                        form_Preview.SetBackgroundImage(null);

                    if ((PreviewBW != null) && (PreviewBW.IsBusy))
                        PreviewBW.CancelAsync();
                    PreviewBW = new BackgroundWorker();
                    PreviewBW.DoWork += (w_s, w_e) =>
                    {
                        BackgroundWorker worker = (BackgroundWorker)w_s;
                        CImageJob job = w_e.Argument as CImageJob;

                        w_e.Result = "Preview Worker stopped unfinished";
                        CConverter c = new CConverter();
                        CImageResult cac;
                        try
                        {
                            cac = c.CombineAndConvert(job.files);
                        }
                        catch (Exception ex)
                        {
                            worker.ReportProgress(-1, ex.Message);
                            return;
                        }
                        Bitmap img = cac.bmp;
                        if (job.files[0].RadarColorAverage) job.files[0].RadarColor = cac.RadarColor;
                        //do not update the preview if this was cancelled
                        if (!worker.CancellationPending)
                            worker.ReportProgress(100, new CImageResult(img,
                                                            job.frameNr,
                                                            job.files[0].CompressionFormat,
                                                            job.files[0].BitFlags,
                                                            job.files[0].RadarColor,
                                                            job.tmpfilename + " created" + Environment.NewLine));
                        w_e.Result = "Preview Worker finished";
                    };

                    PreviewBW.ProgressChanged += (w_s, w_e) =>
                    {
                        if (w_e.ProgressPercentage != -1)
                        {
                            CImageResult wr = (CImageResult)w_e.UserState;
                            if (form_Preview != null)
                                form_Preview.SetImage(wr.bmp);
                        }
                        else
                        {
                            this.richTextBox_Reports.SelectionColor = Color.Red;
                            this.richTextBox_Reports.SelectedText = "Worker Error:" + w_e.UserState.ToString() + Environment.NewLine;
                        }

                    };

                    PreviewBW.RunWorkerCompleted += (w_s, w_e) =>
                    {
                        if ((!w_e.Cancelled) && (w_e.Error == null))
                        {
                        }
                        else
                        {
                            this.richTextBox_Reports.SelectionColor = Color.Red;
                            this.richTextBox_Reports.SelectedText = "PreviewWorkerError:" + w_e.Error.Message + Environment.NewLine;
                        }
                    };

                    PreviewBW.WorkerReportsProgress = true;
                    PreviewBW.WorkerSupportsCancellation = true;

                    PreviewBW.RunWorkerAsync(new CImageJob("", -1, "", "", files, false, GetDefaultCompression));

                }
            }
        }

        private void infoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_AboutBox ab = new Form_AboutBox();
            ab.Icon = this.Icon;
            ab.StartPosition = FormStartPosition.Manual;
            ab.Location = new Point(this.Location.X, PointToScreen(this.customMenuStrip1.Location).Y);
            ab.Height = this.Height - this.customMenuStrip1.Height - 20;
            ab.Width = this.Width;
            ab.Text = "About " + ProgramName;

            Font f = new Font("Microsoft Sans Serif", 14.0f, FontStyle.Bold, GraphicsUnit.Pixel);
            ab.AddText("Image Combine", Color.Black, f);
            ab.AddEmptyLine(1);
            System.Reflection.Assembly thisAssembly = System.Reflection.Assembly.GetExecutingAssembly();
            string version = thisAssembly.GetName().Version.Major.ToString("D2") + "." +
                                     thisAssembly.GetName().Version.Minor.ToString("D2") + "." +
                                     thisAssembly.GetName().Version.Build.ToString("D2") + "." +
                                     thisAssembly.GetName().Version.Revision.ToString("D2");

            ab.AddText("Version: \t\t" + version + "\r\n" +
                       "Copyright: \t" + "LKO Software Solutions\r\n" +
                       "Company: \t" + "LKO Industries\r\n");

            ab.AddEmptyLine(1);
            ab.AddText("Support: \t\t"); ab.InsertLink("www.ppmsite.com", "http://www.ppmforums.com/viewtopic.php?p=563643");
            ab.AddEmptyLine();
            ab.AddUnderLine();
            ab.AddText("This program allows to combine images and convert the sequence of images into an SHP file.");
            ab.AddEmptyLine(1);
            ab.AddText("Drag and Drop files on the datagrid or use its right-click context menu to load image files.");
            ab.AddEmptyLine(2);
            ab.AddText("The 3 columns can hold different images which are combined from right to left.\nThe image from column 3 is copied onto the column 2 image which in turn gets copied onto the column 1 image.");
            ab.AddEmptyLine(1);
            ab.AddText("Thus each row represents a frame of the then generated SHP file.");
            ab.AddEmptyLine(1);
            ab.AddText("This allows to create an SHP in one step from images of different render passes. (e.g. normal images + glowing colors images)");

            ab.AddEmptyLine(2);
            ab.AddText("To avoid dozens of different palettes for the color conversion, colors can be ignored.");
            ab.AddEmptyLine(1);
            ab.AddText("Right-click on the palette to load a palette. If a palette is loaded, the right-click contextmenu allows certain colors to be \"ignored\" or");
            ab.AddEmptyLine(1);
            ab.AddText("marked as \"make transparent\", which means the pixel that would be converted into this color are instead set to color #0, which is transparent.");
            ab.AddEmptyLine(1);
            ab.AddText("Note: Do not forget to right-click on the images and set \"Set Palette\" in the image context menu, so the modified palette is assigned to image.");
            ab.AddEmptyLine(1);
            ab.AddText("Best is to first select all images than should get the same palette, then do the palette adjustments and finally assign the palette via \"Set Palette\" to the imgaes.");
            ab.AddEmptyLine(2);
            ab.AddText("");
            ab.AddEmptyLine(1);
            ab.AddUnderLine();
            f = new Font("Microsoft Sans Serif", 12.0f, FontStyle.Bold, GraphicsUnit.Pixel);
            ab.AddText("Changelog:", Color.Black, f);
            ab.AddEmptyLine();
            ab.AddText("Version 01.00.00.01", Color.Black, f);
            ab.AddEmptyLine(1);
            ab.AddText("\tFirst public version");

            ab.AddEmptyLine();
            ab.AddText("Version 01.00.00.02", Color.Black, f);
            ab.AddEmptyLine(1);
            ab.AddText("\t-(bugfix) Compiled as x86 to match the x86 MagickLibrary and prevent mismatch on x64 systems.");

            ab.AddEmptyLine();
            ab.AddText("Version 01.01.00.00", Color.Black, f);
            ab.AddEmptyLine(1);
            ab.AddText("\t-(update) SHP creating routine implemented, which allowed to get rid of the big ImageMagick library and the limited ShapeSet tool.");
            ab.AddEmptyLine(1);
            ab.AddText("\t-(update) SHP reading routine implemented, which allows to load SHPs and have the frames listed for further resaving and modification.");
            ab.AddEmptyLine(1);
            ab.AddText("\t\tloaded SHPs are not color converted again and read/shown as indexed palette image. Color Conversion methods are not applied on these.");
            ab.AddEmptyLine(1);
            ab.AddText("\t-(update) Radar color setting added, allowing to modify/set each frames radar color. Used by TS/RA2 only for Overlay SHPs like tiberium/ore.");
            ab.AddEmptyLine(1);
            ab.AddText("\t-(update) Added option to create an image for each processed frame.");
            ab.AddEmptyLine(1);
            ab.AddText("\t-(update) Added option to prevent wobble bug in TS. This fixes turreted SHP units and prevents them from wobbling 1 pixel up down when rotating the turret or when using a moving animation.");
            ab.AddEmptyLine(1);
            ab.AddText("\t-(update) Options to set the compression for each frame.");
            ab.AddEmptyLine(1);
            ab.AddText("\t-(update) Hovering the mouse over palette indexed pixel in the preview image, highlights the used color of that pixel in the palette");

            ab.AddEmptyLine();
            ab.AddText("Version 01.01.00.01", Color.Black, f);
            ab.AddEmptyLine(1);
            ab.AddText("\t-compiled as target platform \"Any CPU\"");

            ab.AddEmptyLine();
            ab.AddText("Version 01.01.00.02", Color.Black, f);
            ab.AddEmptyLine(1);
            ab.AddText("\t-focus is set back to the datagrid after SHP conversion process is finished");
            ab.AddEmptyLine(1);
            ab.AddText("\t-SHPs with wrong compression bit set are loaded anyway");
            ab.AddEmptyLine(1);
            ab.AddText("\t-last used settings stored/loaded from ImageShaper.ini");

            ab.AddEmptyLine();
            ab.AddText("Version 01.01.00.03", Color.Black, f);
            ab.AddEmptyLine(1);
            ab.AddText("\t-(bugfix) bad ini values for position and size don't crash anymore");
            ab.AddEmptyLine(1);
            ab.AddText("\t-(update) simple palette manager added, which keeps track of different palette settings");
            ab.AddEmptyLine(1);
            ab.AddText("\t-(update) image files only point to a palette from the manager and don't keep a full copy of it, thus hopefully reducing memory usage");
            ab.AddEmptyLine(1);
            ab.AddText("\t-(update) palette edits are instantly applied to all files sharing the currently selected palette");
            ab.AddEmptyLine(1);
            ab.AddText("\t-(update) performance of deleting files/frames improved");

            ab.AddEmptyLine();
            ab.AddText("Version 01.01.00.04", Color.Black, f);
            ab.AddEmptyLine(1);
            ab.AddText("\t-(update) images/frames store only an int index to the used palette from the palettemanager, thus reducing memory usage per image/frame");

            ab.AddEmptyLine();
            ab.AddText("Version 01.01.00.05", Color.Black, f);
            ab.AddEmptyLine(1);
            ab.AddText("\t-(bugfix) palettemanager crashed on images with unassigned palettes");

            ab.AddEmptyLine();
            ab.AddText("Version 01.01.00.06", Color.Black, f);
            ab.AddEmptyLine(1);
            ab.AddText("\t-a few adjustments on the class for the internal stored images/frames");
            ab.AddEmptyLine(1);
            ab.AddText("\t-progressbar is showing the current progress of the imported files");

            ab.AddEmptyLine();
            ab.AddText("Version 01.01.00.07", Color.Black, f);
            ab.AddEmptyLine(1);
            ab.AddText("\t-(bugfix) loading palette indexed images was not supported. Now loaded directly without any color conversion.");
            ab.AddEmptyLine(1);
            ab.AddText("\t-(bugfix) SHP frames loaded in the 2nd or 3rd imagelist caused an exception");
            ab.AddEmptyLine(1);
            ab.AddText("\t-(update) images with 32bpp ARGB, 24bpp RGB and 8bpp palette indexed color formats are directly supported. All other image color formats are converted to 32bpp ARGB before processing image in color conversion.");
            ab.AddEmptyLine(1);
            ab.AddText("\t-(update) added options to optimize canvas size");
            ab.AddEmptyLine(1);
            ab.AddText("\t-(update) BitFlags added");
            ab.AddEmptyLine(1);
            ab.AddText("\t-(update) option to calculate average radar color added (ignoring all transparent background pixel)");
            ab.AddEmptyLine(1);
            ab.AddText("\t-(update) Routine to identify SHPs changed to be less strict. Previously anything with a BitFlag value of >3 was discarded as invalid SHP.");


            ab.AddEmptyLine();
            ab.AddText("Version 01.01.00.08", Color.Black, f);
            ab.AddEmptyLine(1);
            ab.AddText("\t-(bugfix) optimize canvas failed when empty frames were included");
            ab.AddEmptyLine(1);
            ab.AddText("\t-(bugfix) when using \"RLE_Zero\" compression, the bitflag wasn't set in the SHP");
            ab.AddEmptyLine(1);
            ab.AddText("\t-(update) last used average color checkbox value stored in ini");
            ab.AddEmptyLine(1);
            ab.AddText("\t-(update) when enabling \"optimize canvas\", \"keep centered\" is enabled by default as well");
            ab.AddEmptyLine(1);
            ab.AddText("\t-(update) option added to use a custom color as transparent background color during color conversion");
            ab.AddEmptyLine(1);
            ab.AddText("\t-(update) option added to copy only the transparent pixel when combining this image with a base image");
            ab.AddEmptyLine(1);
            ab.AddText("\t-(update) changes in the \"Image/Frame Settings\" are instantly applied to the selected images/frames");

            ab.AddEmptyLine();
            ab.AddText("Version 01.01.00.09", Color.Black, f);
            ab.AddEmptyLine(1);
            ab.AddText("\t-(update) create images expanded with SHP(TS) file format, allowing each frame to be saved as SHP");
            ab.AddEmptyLine(1);
            ab.AddText("\t-(update) create images filename can now use an asterisk *, to keep the original filename for the single frame");

            ab.AddEmptyLine();
            ab.AddText("Version 01.01.00.10", Color.Black, f);
            ab.AddEmptyLine(1);
            ab.AddText("\t-(bugfix) RLE-Zero encoding algorithm crashed when the encoded result data was bigger than 2 times the uncompressed data (RLE encoded worst-case is 3 times the size as uncompressed)");
            ab.AddEmptyLine(1);
            ab.AddText("\t-(bugfix) imported files ignored the \"fixed Backcolor\" setting");
            ab.AddEmptyLine(1);
            ab.AddText("\t-(update) faster file import in datagridview (now refreshes only once after import is complete)");

            ab.AddEmptyLine();
            ab.AddText("Version 01.01.00.11", Color.Black, f);
            ab.AddEmptyLine(1);
            ab.AddText("\t-(update) JASC Palette format supported");
            ab.AddEmptyLine(1);
            ab.AddText("\t-(update) loading files now done in a separate thread to prevent/reduce freezing interface");
            ab.AddEmptyLine(1);
            ab.AddText("\t-(bugfix) when loading multiple SHPs, an empty cell was added after each SHPs last frame");

            ab.AddEmptyLine();
            ab.AddText("Version 01.01.00.12", Color.Black, f);
            ab.AddEmptyLine(1);
            ab.AddText("\t-(update) if preview window is focused, ctrl+c copies the image into the clipboard");
            ab.AddEmptyLine(1);
            ab.AddText("\t-(update) Split result added, which allows to split the frames evenly into multiple SHPs");

            ab.AddEmptyLine();
            ab.AddText("Version 01.01.00.13", Color.Black, f);
            ab.AddEmptyLine(1);
            ab.AddText("\t-(bugfix) RLE Encoder fixed (again). Now resizes array during encoding if encoded data exceeds the preallocated array size.");

            ab.AddEmptyLine();
            ab.AddText("Version 01.01.00.14", Color.Black, f);
            ab.AddEmptyLine(1);
            ab.AddText("\t-(bugfix) RLE Encoder fixed (again)");

            ab.AddEmptyLine();
            ab.AddText("Version 01.01.00.15", Color.Black, f);
            ab.AddEmptyLine(1);
            ab.AddText("\t-(update) internal Drag & Drop added to the datagrid cells. If a cell is empty, the d&d value is set, otherwise a new row inserted. Existing values are not replaced!");
            ab.AddEmptyLine(1);
            ab.AddText("\t-(update) [File] menu added to menustrip, which offers functions to save and load a project. A project includes the data from the datagrid and the complete palette setup.");

            ab.AddEmptyLine();
            ab.AddText("Version 01.01.00.16", Color.Black, f);
            ab.AddEmptyLine(1);
            ab.AddText("\t-(update) \"Load from Clipboard\" added to the Datagrid right-click menu. The clipboard image is saved in the Temp subfolder.");
            ab.AddEmptyLine(1);
            ab.AddText("\t-(bugfix) Copy, Cut, Paste in Datagrid right-click menu fixed");
            ab.AddEmptyLine(1);
            ab.AddText("\t-(update) datagrid ensures a single empty row at the end");
            ab.AddEmptyLine(1);
            ab.AddText("\t-(update) preview window moves to front as well, when program gets focus");

            ab.AddEmptyLine();
            ab.AddText("Version 01.01.00.17", Color.Black, f);
            ab.AddEmptyLine(1);
            ab.AddText("\t-(bugfix) annoying windows warning sound removed when adding files");
            ab.AddEmptyLine(1);
            ab.AddText("\t-(bugfix) \"combine transparent pixel\" is applied only to the image in the column left of it");
            ab.AddEmptyLine(1);
            ab.AddText("\t-(update) if multiple cells are selected when adding files, the user is asked if the files should be duplicated into the cells");
            ab.AddEmptyLine(1);
            ab.AddText("\t-(update) the audio file SHPfinished.wav is played when SHP conversion is done");

            ab.AddEmptyLine();
            ab.AddText("Version 01.01.00.18", Color.Black, f);
            ab.AddEmptyLine(1);
            ab.AddText("\t-(update) support for RGBA JASC palettes");
            ab.AddEmptyLine(1);
            ab.AddText("\t-(update) Euclidean color conversion with Alpha-channel support");
            ab.AddEmptyLine(1);
            ab.AddText("\t-(update) [Options] menu added");
            ab.AddEmptyLine(1);
            ab.AddText("\t-(update) \"#Worker Threads\" setting moved into [Options]");
            ab.AddEmptyLine(1);
            ab.AddText("\t-(update) \"Output Folder\" setting added to [Options] to define output folder for SHPs");
            ab.AddEmptyLine(1);
            ab.AddText("\t-(update) support to save/load a palette setup. Select \"ImageShaper Palette Setup\" as fileformat in the Load/Save Palette Dialog.");
            ab.AddEmptyLine(1);
            ab.AddText("\t-(update) Console command line functionality added. Run ImageShaper.exe ? for help.");

            ab.AddEmptyLine();
            ab.AddText("Version 01.01.00.19", Color.Black, f);
            ab.AddEmptyLine(1);
            ab.AddText("\t-(bugfix) Preview shows alpha channel correct");
            ab.AddEmptyLine(1);
            ab.AddText("\t-(update) \"Preview Background Image\" setting added to [Options]");

            ab.AddEmptyLine();
            ab.AddText("Version 01.01.00.20", Color.Black, f);
            ab.AddEmptyLine(1);
            ab.AddText("\t-(update) Preview updates for the selected image automatically when changes to the palette are made (e.g. colors set to \"ignore\")");
            ab.AddEmptyLine(1);
            ab.AddText("\t-(update) with the DataGrid focused, CTRL+V copies the Clipboard image into it");

            ab.AddEmptyLine();
            ab.AddText("Version 01.01.00.21", Color.Black, f);
            ab.AddEmptyLine(1);
            ab.AddText("\t-(update) the Preview isn't locking the access of the shown file anymore. Image files can now be replaced/changed while Image Shaper is open.");
            ab.AddEmptyLine(1);
            ab.AddText("\t-(update) new option \"Reverse order of selected cells\" added to the DataGrid context menu.");

            ab.AddEmptyLine();
            ab.AddText("Version 01.01.00.22", Color.Black, f);
            ab.AddEmptyLine(1);
            ab.AddText("\t-(update) The resuling SHP filename has trailing numbers only removed when operating on image files. When loading an SHP file, the resulting filename is exactly the same.");
            ab.AddEmptyLine(1);
            ab.AddText("\t-(bugfix) in rare cases \"opt. canvas\" caused an index of bounds error when creating a frame");

            ab.AddEmptyLine();
            ab.AddText("Version 01.01.00.23", Color.Black, f);
            ab.AddEmptyLine(1);
            ab.AddText("\t-(update) new format \"Uncompressed_Full_Frame\" added, which stores each SHP frame in its full size. \"optimize canvas\" doesn't work for this case");
            ab.AddEmptyLine(1);
            ab.AddText("\t-(update) command line option added to support setting \"Split result\"");

            ab.AddEmptyLine();
            ab.AddText("Version 01.01.00.24", Color.Black, f);
            ab.AddEmptyLine(1);
            ab.AddText("\t-(update) new function \"Load and Split Image\" added to right click contextmenu, which allows to load an image with frames as panels inside the image.");
            ab.AddEmptyLine(1);
            ab.AddText("\t-(update) new tool \"FireFLH Finder\" added to menubar \"Tools\" , which makes it easy to find the correct values for FireFLH, PrimaryFirePixelOffset, DamageFireOffset# and MuzzeFlash#.");
            ab.AddEmptyLine(1);
            ab.AddText("\t-(update) the custom image control (e.g. Preview window) doesn't reset the view/scrollbars anymore whenever the image is changed.");

            //TODO add auto Shadow generator
            ab.AddEmptyLine(1);
            ab.AddText("\t-");

            ab.AddEmptyLine(1);
            ab.Show();
        }

        private void saveProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.DefaultExt = "isp";
            sfd.InitialDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            sfd.Filter = "Image Shaper Project|*.isp";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists(sfd.FileName)) File.Delete(sfd.FileName);

                List<DGVCell> cells = new List<DGVCell>();
                foreach (DataGridViewRow row in this.dataGridView_Files.Rows)
                    foreach (DataGridViewCell cell in row.Cells)
                        if (cell.Value != null)
                            cells.Add(new DGVCell(cell));
                Cinimanager.SaveProject(sfd.FileName, cells.ToArray(), PaletteManager.Palettes);
            }
        }

        private void loadProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            ofd.Filter = "Image Shaper Project|*.isp";
            ofd.Multiselect = false;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                this.uC_Palette1.SetPalettes(Cinimanager.LoadPaletteSetup(ofd.FileName));
                DGVCell[] cells = Cinimanager.LoadProject(ofd.FileName);
                this.dataGridView_Files.Rows.Clear();
                foreach (DGVCell cell in cells)
                {
                    if (cell.ColumnIndex == 0)
                    {
                        DataGridViewRow row = (DataGridViewRow)this.dataGridView_Files.RowTemplate.Clone();
                        object[] values = new object[this.dataGridView_Files.ColumnCount];
                        values[cell.ColumnIndex] = cell.Value;
                        row.CreateCells(this.dataGridView_Files, values);
                        this.dataGridView_Files.Rows.Add(row);
                    }
                    else
                    {
                        if (cell.RowIndex < this.dataGridView_Files.RowCount)
                            this.dataGridView_Files[cell.ColumnIndex, cell.RowIndex].Value = cell.Value;
                    }
                }
                AddLastEmptyRow();
            }
        }

        private void AddLastEmptyRow()
        {
            if ((this.dataGridView_Files.RowCount > 0) && (!IsEmptyRow(this.dataGridView_Files.Rows[this.dataGridView_Files.RowCount - 1])))
            {
                DataGridViewRow row = (DataGridViewRow)this.dataGridView_Files.RowTemplate.Clone();
                this.dataGridView_Files.Rows.Add(row);
            }
        }
        private void RemoveLastEmptyRow()
        {
            if ((this.dataGridView_Files.RowCount > 0) && (IsEmptyRow(this.dataGridView_Files.Rows[this.dataGridView_Files.RowCount - 1])))
                this.dataGridView_Files.Rows.RemoveAt(this.dataGridView_Files.RowCount - 1);
        }

        private void PlaySound()
        {
            string sound = "SHPfinished.wav";

            string soundfile = Path.Combine(GetProgramPath, sound);
            try
            {
                using (System.Media.SoundPlayer sp = new System.Media.SoundPlayer(soundfile))
                {
                    sp.Play();
                }
            }
            catch { }
        }


        bool RunAsCommand = false;
        internal void RunCommand(string[] args)
        {
            RunAsCommand = true;
            bool setbits = false;
            bool setCompression = false;
            bool closewhenfinished = true;
            for (int i = 0; i < args.Length; i++)
            {
                string argvalue = "";
                if (args[i].Contains('=')) argvalue = args[i].Split('=')[1];

                if (args[i].StartsWith("-o="))
                    this.toolStripMenuItem_Outputfolder.ToolStrip_UC_FolderSelector.Value = argvalue;

                if (args[i].StartsWith("-p="))
                {
                    Console.WriteLine("loading palette [" + argvalue + "]");
                    this.uC_Palette1.LoadPalette(argvalue);
                }

                if (args[i].StartsWith("-c="))
                {
                    setCompression = true;
                    switch (argvalue.ToLower())
                    {
                        case "0":
                        case "undefined": this.comboBox_Compression.SelectedIndex = 0; break;
                        case "1":
                        case "uncompressed": this.comboBox_Compression.SelectedIndex = 1; break;
                        case "2":
                        case "rle_zero": this.comboBox_Compression.SelectedIndex = 2; break;
                        case "3":
                        case "detect_best_size": this.comboBox_Compression.SelectedIndex = 3; break;
                        case "4":
                        case "uncompressed_full_frame": this.comboBox_Compression.SelectedIndex = 4; break;
                    }
                }

                if (args[i].StartsWith("-i="))
                {
                    AddFilesToDataGridSync(GetCommandFiles(argvalue), 0, -1, setbits, setCompression);
                }

                //general settings that don't affect file loading order/settings
                if (args[i].StartsWith("-z"))
                    closewhenfinished = false;

                if (args[i].StartsWith("-optcan="))
                {
                    switch (argvalue)
                    {
                        case "0":
                        case "off":
                        case "no": this.checkBox_OptimizeCanvas.Checked = false; break;
                        case "1":
                        case "on":
                        case "yes": this.checkBox_OptimizeCanvas.Checked = true; break;
                    }
                }

                if (args[i].StartsWith("-centered="))
                {
                    switch (argvalue)
                    {
                        case "0":
                        case "off":
                        case "no": this.checkBox_KeepCentered.Checked = false; break;
                        case "1":
                        case "on":
                        case "yes": this.checkBox_KeepCentered.Checked = true; break;
                    }
                }

                if (args[i].StartsWith("-stopwobblebug="))
                {
                    switch (argvalue)
                    {
                        case "0":
                        case "off":
                        case "no": this.checkBox_PreventWobbleBug.Checked = false; break;
                        case "1":
                        case "on":
                        case "yes": this.checkBox_PreventWobbleBug.Checked = true; break;
                    }
                }

                if (args[i].StartsWith("-split="))
                {
                    int splitvalue = 1;
                    if ((int.TryParse(argvalue, out splitvalue)) && (splitvalue > 0) && (splitvalue <= this.numericUpDown_SplitResult.Maximum))
                        this.numericUpDown_SplitResult.Value = splitvalue;
                }
            }
            CreatSHP(closewhenfinished);
            if (!this.IsDisposed)
                this.ShowDialog();
        }
        private static string[] GetCommandFiles(string p)
        {
            List<string> files = new List<string>();
            string path = System.IO.Path.GetDirectoryName(p);
            string filename = System.IO.Path.GetFileName(p);

            try
            {
                files.AddRange(System.IO.Directory.GetFiles(path, filename));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return files.ToArray();
        }

        private void FireFLHFinderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_FireFLHFinder flhfinder = new Form_FireFLHFinder();
            flhfinder.Icon = this.Icon;
            flhfinder.StartPosition = FormStartPosition.Manual;
            flhfinder.Location = this.PointToScreen(this.customMenuStrip1.Location);
            flhfinder.Palette = PaletteManager.GetPalette(0);
            flhfinder.InitialDirectory = this.toolStripMenuItem_Outputfolder.ToolStrip_UC_FolderSelector.Value;
            flhfinder.Show();
        }

    }
}
