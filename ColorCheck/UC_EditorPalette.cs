using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using ImageShaper;

namespace TMPEditorNamespace
{
    public partial class UC_EditorPalette : UserControl
    {
        #region Events
        public event EventHandler<MouseEventArgs> PaletteSelectedColorChanged;
        // The method which fires the Event
        protected void OnPaletteSelectedColorChanged(object sender, MouseEventArgs e)
        {
            var target = PaletteSelectedColorChanged;
            if (target != null)
            {
                target(this, e);
            }
        }

        public event EventHandler<EventArgs> PaletteChanged;
        // The method which fires the Event
        protected void OnPaletteChanged(object sender, EventArgs e)
        {
            var target = PaletteChanged;
            if (target != null)
            {
                target(this, new EventArgs());
            }
        }
        #endregion

        #region special properties to customize view and functionality of the palette (used for TMPShop palette checker)
        private bool contextMenuEnabled = true;
        /// <summary>
        /// allows to enable/disable the contextMenu
        /// </summary>
        public bool ContextMenuEnabled
        {
            get { return contextMenuEnabled; }
            set
            {
                contextMenuEnabled = value;
            }
        }
        private bool contextMenuLoadEnabled = true;
        /// <summary>
        /// allows to enable/disable the Load option in the contextMenu
        /// </summary>
        public bool ContextMenuLoadEnabled
        {
            get { return contextMenuLoadEnabled; }
            set
            {
                contextMenuLoadEnabled = value;
            }
        }
        private bool contextMenuSaveEnabled = true;
        /// <summary>
        /// allows to enable/disable the Save option in the contextMenu
        /// </summary>
        public bool ContextMenuSaveEnabled
        {
            get { return contextMenuSaveEnabled; }
            set
            {
                contextMenuSaveEnabled = value;
            }
        }

        private CPalette useFixedPalette;
        /// <summary>
        /// use a certain palette when showing the control. Not for the editor which can constantly switch between palettes.
        /// </summary>
        public CPalette UseFixedPalette
        {
            get { return useFixedPalette; }
            set
            {
                useFixedPalette = value;
                UpdatePalettePanel();
            }
        }

        private bool showPaletteModes = true;
        /// <summary>
        /// Show Color/Z-Data button options yes/no?
        /// </summary>
        public bool ShowPaletteModes
        {
            get { return showPaletteModes; }
            set
            {
                showPaletteModes = value;
                UpdatePalettePanel();
            }
        }

        private long[] paletteUsage;
        /// <summary>
        /// List that has a numeric value for each palette color entry
        /// </summary>
        public long[] PaletteUsage
        {
            get { return paletteUsage; }
            set
            {
                paletteUsage = value;
                UpdatePalettePanel();
            }
        }

        private bool allowColorSelection = true;
        /// <summary>
        /// Specifies if selecting a color and the whole selection logic is enabled
        /// </summary>
        public bool AllowColorSelection
        {
            get { return allowColorSelection; }
            set
            {
                allowColorSelection = value;
                UpdatePalettePanel();
            }
        }
        #endregion

        private int columnCount = 1;
        private int rowCount = 1;

        private int paletteSelectedColor = 0;
        public int PaletteSelectedColor
        {
            get { return paletteSelectedColor; }
            set
            {
                paletteSelectedColor = value;
                UpdatePalettePanel();
            }
        }
        private int paletteMode = 0;
        /// <summary>
        /// Used for either color (0) or z-data mode (1) or fixedPalette (2)
        /// </summary>
        public int PaletteMode
        {
            get { return paletteMode; }
            set
            {
                paletteMode = value;
                UpdatePalettePanel();
            }
        }

        public UC_EditorPalette()
        {
            InitializeComponent();
            useFixedPalette = null;

            columnCount = 8;
            rowCount = (int)Math.Ceiling((double)256 / (double)columnCount);
            this.PaletteColorBox.SizeChanged += new EventHandler(PaletteColorBox_SizeChanged);
            this.PaletteColorBox.MouseClick += new MouseEventHandler(PaletteColorBox_MouseClick);

            this.radioButton_Color.CheckedChanged += new EventHandler(radioButton_Color_CheckedChanged);
        }

        void radioButton_Color_CheckedChanged(object sender, EventArgs e)
        {
            //react only if the user clicks on them
            if ((this.radioButton_Color.Focused) || (this.radioButton_ZData.Focused))
            {
                if (radioButton_Color.Checked)
                    PaletteMode = 0;
                if (radioButton_ZData.Checked)
                    PaletteMode = 1;
                OnPaletteChanged(sender, e);
            }
        }

        public void LoadPalette(string defaultPaletteFolder)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if ((useFixedPalette != null) && (useFixedPalette.PaletteFile != "")) ofd.InitialDirectory = useFixedPalette.PaletteFile;
            else ofd.InitialDirectory = defaultPaletteFolder;

            ofd.Title = "Open Palette File";
            ofd.FileName = "";
            ofd.Filter = "Palette files|*.pal";
            if (ofd.ShowDialog() != DialogResult.Cancel)
            {
                useFixedPalette = new CPalette();
                useFixedPalette.Load(ofd.FileName);
                UpdatePalettePanel();
                OnPaletteChanged(this, null);
            }
        }

        public void SavePalette(string defaultPaletteFolder)
        {
        }

        private void loadPaletteMenuItem_Click(object sender, EventArgs e)
        {
            LoadPalette("");
        }
        private void savePaletteMenuItem_Click(object sender, EventArgs e)
        {
            SavePalette("");
        }
        void PaletteColorBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (contextMenuEnabled
                 && (contextMenuSaveEnabled || contextMenuLoadEnabled)
                    )
                {
                    ContextMenu contextMenu = new ContextMenu();

                    CPalette currentpalette = new CPalette();
                    currentpalette = useFixedPalette;

                    if (currentpalette != null)
                    {
                        string paletteinfo = currentpalette.PaletteFile;
                        if ((paletteinfo == null)||(paletteinfo == "")) paletteinfo = "temporary palette";

                        MenuItem info = new MenuItem(paletteinfo);
                        info.Enabled = false;
                        contextMenu.MenuItems.Add(info);
                    }
                    else
                    {
                        MenuItem info = new MenuItem("no palette loaded");
                        info.Enabled = false;
                        contextMenu.MenuItems.Add(info);
                    }
                    
                    if (contextMenuLoadEnabled)
                    {
                        MenuItem loadPalette = new MenuItem("Load Palette", new System.EventHandler(this.loadPaletteMenuItem_Click));
                        contextMenu.MenuItems.Add(loadPalette);
                    }
                    if (contextMenuSaveEnabled)
                    {
                        MenuItem savePalette = new MenuItem("Save Palette", new System.EventHandler(this.savePaletteMenuItem_Click));
                        contextMenu.MenuItems.Add(savePalette);
                    }
                    contextMenu.Show(this, new Point(e.X, e.Y));
                }
            }
            if (e.Button == MouseButtons.Left)
            {
                if (allowColorSelection)
                {
                    Size PaletteSpace = this.PaletteColorBox.Size;
                    int columnwidth = (PaletteSpace.Width - 1) / columnCount;
                    int rowheight = (PaletteSpace.Height - 1) / rowCount;
                    double cellwidth = (double)PaletteColorImage.Width / (double)columnCount;
                    double cellheight = (double)PaletteColorImage.Height / (double)rowCount;

                    int selectedColumn = (int)(e.X / cellwidth);
                    int selectedRow = (int)(e.Y / cellheight);

                    //the property PaletteSelectedColor launches UpdatePalettePanel when changed
                    int selcol = selectedColumn * rowCount + selectedRow;
                    if (selcol > 255) selcol = 255; //used when palette shows more than 256 colors, due to uneven columncount
                    if (selcol < 0) selcol = 0;
                    PaletteSelectedColor = selcol;
                    OnPaletteSelectedColorChanged(sender, e);
                }
            }
        }

        void PaletteColorBox_SizeChanged(object sender, EventArgs e)
        {
            UpdatePalettePanel();
        }

        private Image PaletteColorImage;
        private Image PaletteZDataImage;

        private void UpdatePalettePanel()
        {
            if (paletteMode == 0) this.radioButton_Color.Checked = true;
            else this.radioButton_ZData.Checked = true;

            if (!showPaletteModes)
            {
                this.tableLayoutPanel1.RowStyles[0].Height = 0;
                this.radioButton_Color.Enabled = false;
                this.radioButton_ZData.Enabled = false;
            }
            else
            {
                this.tableLayoutPanel1.RowStyles[0].Height = 20;
                this.radioButton_Color.Enabled = true;
                this.radioButton_ZData.Enabled = true;
            }
            Size PaletteSpace = new Size(this.PaletteColorBox.Width, this.PaletteColorBox.Height);
            //8 columns, 32 rows

            PaletteColorImage = new Bitmap(PaletteSpace.Width, PaletteSpace.Height);
            PaletteZDataImage = new Bitmap(PaletteSpace.Width, PaletteSpace.Height);


            Graphics PaletteColorImage_g = Graphics.FromImage(PaletteColorImage);
            Graphics PaletteZDataImage_g = Graphics.FromImage(PaletteZDataImage);

            Bitmap PaletteBitmap = new Bitmap(columnCount, rowCount);
            Color c = Color.FromArgb(255, 0, 0, 0);
            //draw color palette
            double cellwidth = (double)(PaletteSpace.Width - 1) / (double)columnCount;
            double cellheight = (double)(PaletteSpace.Height - 1) / (double)rowCount;
            for (int i = 0; i < 256; i++)
            {
                int column = i / rowCount;
                int row = i % rowCount;
                    if ((useFixedPalette != null) && (useFixedPalette.palette != null))
                    {
                        c = useFixedPalette.palette[i].Color;
                    }
                    else
                        c = Color.FromArgb(255, i, i, i);
                Rectangle cell = new Rectangle((int)(column * cellwidth), (int)(row * cellheight), (int)cellwidth + 1, (int)cellheight + 1);
                PaletteColorImage_g.FillRectangle(new SolidBrush(c), cell);
            }

            Brush lineBrush = new System.Drawing.Drawing2D.LinearGradientBrush(
                                    new Rectangle(0, 0, this.PaletteColorBox.Width, this.PaletteColorBox.Height),
                                    Color.FromArgb(255, 0, 200, 0),
                                    Color.FromArgb(255, 0, 50, 0),
                                    System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal);

            Pen linePen = new Pen(lineBrush);
            for (int i = 0; i < columnCount + 1; i++)
                PaletteColorImage_g.DrawLine(linePen, new Point((int)(i * cellwidth), 0), new Point((int)(i * cellwidth), PaletteSpace.Height - 1));
            for (int i = 0; i < rowCount + 1; i++)
                PaletteColorImage_g.DrawLine(linePen, new Point(0, (int)(i * cellheight)), new Point(PaletteSpace.Width - 1, (int)(i * cellheight)));

            //draw ZData palette
            for (int i = 0; i < 256; i++)
            {
                int column = i / rowCount;
                int row = i % rowCount;
                c = Color.FromArgb(255, (i * 8) % 256, (i / 2) % 256, (i * 1) % 256);
                Rectangle cell = new Rectangle((int)(column * cellwidth), (int)(row * cellheight), (int)cellwidth + 1, (int)cellheight + 1);
                PaletteZDataImage_g.FillRectangle(new SolidBrush(c), cell);
            }
            for (int i = 0; i < columnCount+1; i++)
                PaletteZDataImage_g.DrawLine(linePen, new Point((int)(i * cellwidth), 0), new Point((int)(i * cellwidth), PaletteSpace.Height - 1));
            for (int i = 0; i < rowCount+1; i++)
                PaletteZDataImage_g.DrawLine(linePen, new Point(0, (int)(i * cellheight)), new Point(PaletteSpace.Width - 1, (int)(i * cellheight)));

            if (allowColorSelection)
                SetSelectedColor();
            else
            {
                if ((paletteMode == 0) || (paletteMode == 2))
                    PaletteColorBox.Image = PaletteColorImage;
                if (paletteMode == 1)
                    PaletteColorBox.Image = PaletteZDataImage;

                if (paletteUsage != null)
                {
                    SetPaletteUsageValues();
                }
            }
        }
        private void SetPaletteUsageValues()
        {
            Image TextColorImage = new Bitmap(PaletteColorImage.Width, PaletteColorImage.Height);
            Graphics tci_g = Graphics.FromImage(TextColorImage);
            tci_g.DrawImage(PaletteColorImage, 0, 0, PaletteColorImage.Width, PaletteColorImage.Height);

            Size PaletteSpace = new Size(this.PaletteColorBox.Width, this.PaletteColorBox.Height);
            double cellwidth = (double)(PaletteSpace.Width - 1) / (double)columnCount;
            double cellheight = (double)(PaletteSpace.Height - 1) / (double)rowCount;

            for (int column = 0; column < columnCount; column++)
                for (int row = 0; row < rowCount; row++)
                {
                    Point p1 = new Point((int)(column * cellwidth), (int)(row * cellheight));
                    Point p2 = new Point((int)((column + 1) * cellwidth), (int)((row + 1) * cellheight));

                    Brush lineBrush = new System.Drawing.Drawing2D.LinearGradientBrush(
                                            new Rectangle((int)(column * cellwidth - 1), (int)(row * cellheight - 1), (int)cellwidth + 3, (int)cellheight + 3),
                                            Color.FromArgb(255, 0, 255, 0),
                                            Color.FromArgb(255, 200, 255, 200),
                                            System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal);

                    //start with a too big fontsize
                    float fontsize = 10;// (float)(cellheight * 7 / 10);
                    Font f = new System.Drawing.Font("Arial", fontsize, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

                    int index = row + column * rowCount;
                    string value = "";
                    if ((paletteUsage != null) && (index < paletteUsage.Length)) value = paletteUsage[index].ToString();

                    if (value != "")
                    {
                        SizeF stringsize = tci_g.MeasureString(value, f);
                        while ((stringsize.Height < cellheight) && (stringsize.Width < cellwidth))
                        {
                            fontsize += 0.5f;
                            f = new System.Drawing.Font("Arial", fontsize, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                            stringsize = tci_g.MeasureString(value, f);
                        }
                        while ((stringsize.Height > cellheight) || (stringsize.Width > cellwidth))
                        {
                            fontsize -= 0.5f;
                            f = new System.Drawing.Font("Arial", fontsize, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                            stringsize = tci_g.MeasureString(value, f);
                        }

                        f = new System.Drawing.Font("Arial", fontsize, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                        stringsize = tci_g.MeasureString(value, f);

                        int textx = (int)(p1.X + cellwidth / 2 - stringsize.Width / 2 + 1);
                        int texty = (int)(p1.Y + cellheight / 2 - stringsize.Height / 2 + 1);
                        tci_g.DrawString(value,
                            f,
                            lineBrush,
                            new Point(textx, texty)
                            );
                    }//if value!=""
                }

            PaletteColorBox.Image = TextColorImage;
        }
        private void SetSelectedColor()
        {
            Image SelectedColorImage = new Bitmap(PaletteColorImage.Width, PaletteColorImage.Height);
            Image SelectedZDataImage = new Bitmap(PaletteZDataImage.Width, PaletteZDataImage.Height);

            Graphics sci_g = Graphics.FromImage(SelectedColorImage);
            Graphics szi_g = Graphics.FromImage(SelectedZDataImage);
            sci_g.DrawImage(PaletteColorImage, 0, 0, PaletteColorImage.Width, PaletteColorImage.Height);
            szi_g.DrawImage(PaletteZDataImage, 0, 0, PaletteZDataImage.Width, PaletteZDataImage.Height);

            Size PaletteSpace = new Size(this.PaletteColorBox.Width, this.PaletteColorBox.Height);
            double cellwidth = (double)(PaletteSpace.Width - 1) / (double)columnCount;
            double cellheight = (double)(PaletteSpace.Height - 1) / (double)rowCount;

            int column = paletteSelectedColor / rowCount;
            int row = paletteSelectedColor % rowCount;


            Point p1 = new Point((int)(column * cellwidth), (int)(row * cellheight));
            Point p2 = new Point((int)((column + 1) * cellwidth), (int)((row + 1) * cellheight));


            Brush lineBrushDark = new System.Drawing.Drawing2D.LinearGradientBrush(
                                    new Rectangle((int)(column * cellwidth - 1), (int)(row * cellheight - 1), (int)cellwidth + 3, (int)cellheight + 3),
                                    Color.FromArgb(255, 0, 50, 0),
                                    Color.FromArgb(255, 0, 200, 0),
                                    System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal);
            Brush lineBrush = new System.Drawing.Drawing2D.LinearGradientBrush(
                                    new Rectangle((int)(column * cellwidth - 1), (int)(row * cellheight - 1), (int)cellwidth + 3, (int)cellheight + 3),
                                    Color.FromArgb(255, 0, 255, 0),
                                    Color.FromArgb(255, 200, 255, 200),
                                    System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal);


            //start with a too big fontsize
            float fontsize = 10;// (float)(cellheight * 7 / 10);
            Font f = new System.Drawing.Font("Arial", fontsize, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            SizeF stringsize = sci_g.MeasureString(paletteSelectedColor.ToString(), f);
            while ((stringsize.Height < cellheight) && (stringsize.Width < cellwidth))
            {
                fontsize += 0.1f;
                f = new System.Drawing.Font("Arial", fontsize, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                stringsize = sci_g.MeasureString(paletteSelectedColor.ToString(), f);
            }
            while ((stringsize.Height > cellheight) || (stringsize.Width > cellwidth))
            {
                fontsize -= 0.1f;
                f = new System.Drawing.Font("Arial", fontsize, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                stringsize = sci_g.MeasureString(paletteSelectedColor.ToString(), f);
            }

            f = new System.Drawing.Font("Arial", fontsize, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            stringsize = sci_g.MeasureString(paletteSelectedColor.ToString(), f);

            int textx = (int)(p1.X + cellwidth / 2 - stringsize.Width / 2 + 1);
            int texty = (int)(p1.Y + cellheight / 2 - stringsize.Height / 2 + 1);
            sci_g.DrawString(paletteSelectedColor.ToString(),
                f,
                lineBrush,
                new Point(textx, texty)
                );
            szi_g.DrawString(paletteSelectedColor.ToString(),
                f,
                lineBrush,
                new Point(textx, texty)
                );

            Pen linePen = new Pen(lineBrushDark);
            sci_g.DrawLine(linePen,
                new Point(p1.X + 1, p1.Y + 1),
                new Point(p2.X - 1, p1.Y + 1)
                );
            sci_g.DrawLine(linePen,
                new Point(p1.X + 1, p2.Y - 1),
                new Point(p2.X - 1, p2.Y - 1)
                );
            sci_g.DrawLine(linePen,
                new Point(p1.X + 1, p1.Y + 1),
                new Point(p1.X + 1, p2.Y - 1)
                );
            sci_g.DrawLine(linePen,
                new Point(p2.X - 1, p1.Y + 1),
                new Point(p2.X - 1, p2.Y - 1)
                );
            //same for ZData palette
            szi_g.DrawLine(linePen,
                new Point(p1.X + 1, p1.Y + 1),
                new Point(p2.X - 1, p1.Y + 1)
                );
            szi_g.DrawLine(linePen,
                new Point(p1.X + 1, p2.Y - 1),
                new Point(p2.X - 1, p2.Y - 1)
                );
            szi_g.DrawLine(linePen,
                new Point(p1.X + 1, p1.Y + 1),
                new Point(p1.X + 1, p2.Y - 1)
                );
            szi_g.DrawLine(linePen,
                new Point(p2.X - 1, p1.Y + 1),
                new Point(p2.X - 1, p2.Y - 1)
                );

            linePen = new Pen(lineBrush);
            sci_g.DrawLine(linePen,
                p1,
                new Point(p2.X, p1.Y)
                );
            sci_g.DrawLine(linePen,
                new Point(p1.X, p2.Y),
                p2
                );
            sci_g.DrawLine(linePen,
                p1,
                new Point(p1.X, p2.Y)
                );
            sci_g.DrawLine(linePen,
                new Point(p2.X, p1.Y),
                p2
                );

            szi_g.DrawLine(linePen,
                p1,
                new Point(p2.X, p1.Y)
                );
            szi_g.DrawLine(linePen,
                new Point(p1.X, p2.Y),
                p2
                );
            szi_g.DrawLine(linePen,
                p1,
                new Point(p1.X, p2.Y)
                );
            szi_g.DrawLine(linePen,
                new Point(p2.X, p1.Y),
                p2
                );

            if ((paletteMode == 0) || (paletteMode == 2))
                PaletteColorBox.Image = SelectedColorImage;
            if (paletteMode == 1)
                PaletteColorBox.Image = SelectedZDataImage;
        }

    }
}
