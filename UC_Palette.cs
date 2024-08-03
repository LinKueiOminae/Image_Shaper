using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace ImageShaper
{
    public partial class UC_Palette : UserControl
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

        public bool AllowImageShaperPaletteSetup = true;
        
        private bool _ShowPaletteOnly = false;
        public bool ShowPaletteOnly
        {
            get { return _ShowPaletteOnly; }
            set
            {
                _ShowPaletteOnly = value;
                UpdatePalettePanel();
            }
        }

        private CPalette _Palette;
        /// <summary>
        /// the palette of the control.
        /// </summary>
        public CPalette Palette
        {
            get { return _Palette; }
            set
            {
                _Palette = PaletteManager.GetPalette(value);
                UpdatePaletteManager();
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

        public UC_Palette()
        {
            InitializeComponent();
            _Palette = null;

            this.comboBox_ColorConversionMethod.Items.Clear();
            foreach (ColorConversionMethod ccm in Enum.GetValues(typeof(ColorConversionMethod)))
            {
                this.comboBox_ColorConversionMethod.Items.Add(ccm.ToString());
            }
            this.comboBox_ColorConversionMethod.SelectedIndex = 0;

            columnCount = 8;
            rowCount = (int)Math.Ceiling((double)256 / (double)columnCount);
            this.PaletteColorBox.SizeChanged += new EventHandler(PaletteColorBox_SizeChanged);
            this.PaletteColorBox.MouseClick += new MouseEventHandler(PaletteColorBox_MouseClick);
            this.PaletteColorBox.MouseDown += new MouseEventHandler(PaletteColorBox_MouseDown);
            this.PaletteColorBox.MouseMove += new MouseEventHandler(PaletteColorBox_MouseMove);
            this.PaletteColorBox.MouseUp += new MouseEventHandler(PaletteColorBox_MouseUp);

            ToolTip toolTip1 = new ToolTip();

            // Set up the delays for the ToolTip.
            toolTip1.AutoPopDelay = 1000000;
            toolTip1.InitialDelay = 100;
            toolTip1.ReshowDelay = 100;
            // Force the ToolTip text to be displayed whether or not the form is active.
            toolTip1.ShowAlways = true;

            // Set up the ToolTip text for the Button and Checkbox.
            toolTip1.SetToolTip(this.comboBox_Palette, "The list of different palette setups.");
            toolTip1.SetToolTip(this.button_NewCopy, "Creates a new clean palette setup.");
            toolTip1.SetToolTip(this.button_Clone, "Creates a copy of the current palette setup,\nincluding settings for ignored/transparent colors and the selected color conversion method.");
        }

        void PaletteColorBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                OnPaletteChanged(this, null);
        }

        public void SetPalettes(CPalette[] palettes)
        {
            PaletteManager.ClearAll();
            foreach (CPalette pal in palettes)
                PaletteManager.GetPalette(pal, true);

            _Palette = PaletteManager.GetPalette(0);
            UpdatePaletteManager();
            OnPaletteChanged(this, null);
        }

        public string initialDirectory = "";
        public void LoadPalette(string filename)
        {
            //_Palette = new CPalette(filename);
            _Palette = PaletteManager.GetPalette(new CPalette(filename));
            UpdatePaletteManager();
            OnPaletteChanged(this, null);
        }
        private void LoadPalette()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Open Palette File/Setup";
            ofd.InitialDirectory = initialDirectory;
            if ((_Palette != null) && (_Palette.PaletteFile != null) && (_Palette.PaletteFile != "")) ofd.InitialDirectory = System.IO.Path.GetDirectoryName(_Palette.PaletteFile);
            ofd.FileName = "";
            if (AllowImageShaperPaletteSetup)
                ofd.Filter = "Palette files|*.pal|ImageShaper Palette Setup|*.isps";
            else
                ofd.Filter = "Palette files|*.pal";
            if (ofd.ShowDialog() != DialogResult.Cancel)
            {
                LoadPalette(ofd.FileName);
            }
        }

        private void SavePalette()
        {
            if ((_Palette == null) || (_Palette.PaletteFile == null)) return;

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialDirectory = initialDirectory;
            if (_Palette.PaletteFile != "") sfd.InitialDirectory = System.IO.Path.GetDirectoryName(_Palette.PaletteFile);
            sfd.Title = "Save Palette File/Setup";
            if ((_Palette == null) || (_Palette.PaletteFile == null)) return;
            sfd.FileName = System.IO.Path.GetFileName(_Palette.PaletteFile);

            if (AllowImageShaperPaletteSetup)
                sfd.Filter = "TS/RA2 Palette|*.pal|JASC 8bit Palette|*.pal|ImageShaper Palette Setup|*.isps";
            else
                sfd.Filter = "TS/RA2 Palette|*.pal|JASC 8bit Palette|*.pal";
            if (sfd.ShowDialog() != DialogResult.Cancel)
            {
                _Palette.Save(sfd.FileName, sfd.FilterIndex);
            }
        }

        private void UnloadPaletteMenuItem_Click(object sender, EventArgs e)
        {
            this._Palette = null;
            this.comboBox_Palette.SelectedIndex = -1;
            UpdatePalettePanel();
        }

        private void MakeTransparentMenuItem_Click(object sender, EventArgs e)
        {
            if (_Palette != null)
            {
                _Palette.palette[PaletteSelectedColor].MakeTransparent = !_Palette.palette[PaletteSelectedColor].MakeTransparent;
                _Palette.palette[PaletteSelectedColor].IsUsed = true;
                UpdatePalettePanel();
                OnPaletteChanged(this, null);
            }
        }

        private void IgnoreColorMenuItem_Click(object sender, EventArgs e)
        {
            if (_Palette != null)
            {
                _Palette.palette[PaletteSelectedColor].IsUsed = !_Palette.palette[PaletteSelectedColor].IsUsed;
                _Palette.palette[PaletteSelectedColor].MakeTransparent = false;
                UpdatePalettePanel();
                OnPaletteChanged(this, null);
            }
        }
        
        private void loadPaletteMenuItem_Click(object sender, EventArgs e)
        {
            LoadPalette();
        }
        private void savePaletteMenuItem_Click(object sender, EventArgs e)
        {
            SavePalette();
        }
        void PaletteColorBox_MouseClick(object sender, MouseEventArgs e)
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

            if (e.Button == MouseButtons.Right)
            {
                if (contextMenuEnabled
                 && (contextMenuSaveEnabled || contextMenuLoadEnabled))
                {
                    ContextMenu contextMenu = new ContextMenu();

                    CPalette currentpalette = _Palette;
                    if (currentpalette != null)
                    {
                        string paletteinfo = currentpalette.PaletteFile;
                        if ((paletteinfo == null)||(paletteinfo == "")) paletteinfo = "temporary palette";

                        MenuItem info = new MenuItem(paletteinfo);
                        info.Enabled = false;
                        contextMenu.MenuItems.Add(info);

                        MenuItem UnloadPalette = new MenuItem("Unload Palette", new System.EventHandler(this.UnloadPaletteMenuItem_Click));
                        contextMenu.MenuItems.Add(UnloadPalette);

                        contextMenu.MenuItems.Add("-");

                        MenuItem MakeTransparent = new MenuItem("Make Transparent", new System.EventHandler(this.MakeTransparentMenuItem_Click));
                        contextMenu.MenuItems.Add(MakeTransparent);

                        MenuItem IgnoreColor = new MenuItem("Ignore Color", new System.EventHandler(this.IgnoreColorMenuItem_Click));
                        contextMenu.MenuItems.Add(IgnoreColor);

                        contextMenu.MenuItems.Add("-");
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
                OnPaletteSelectedColorChanged(sender, e);
            }
        }

        private int MouseDownIndex;
        private PaletteColor MouseDownValue = new PaletteColor(Color.FromArgb(0, 0, 0, 0));
        void PaletteColorBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
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

                int index = selectedRow + selectedColumn * rowCount;
                MouseDownIndex = index;
                if (_Palette != null)
                {
                    MouseDownValue.MakeTransparent = _Palette.palette[index].MakeTransparent;
                    MouseDownValue.IsUsed = _Palette.palette[index].IsUsed;
                    if (_Palette.palette[index].MakeTransparent) _Palette.palette[index].MakeTransparent = false;
                    if (!_Palette.palette[index].IsUsed) _Palette.palette[index].IsUsed = true;
                }
                UpdatePalettePanel();
            }
        }

        void PaletteColorBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
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

                int index = selectedRow + selectedColumn * rowCount;
                if (MouseDownIndex != index)
                {
                    int startindex = index;
                    int endindex = MouseDownIndex;
                    if (MouseDownIndex < index)
                    {
                        startindex = MouseDownIndex;
                        endindex = index;
                    }

                    if (startindex < 0) startindex = 0;
                    if (endindex < 0) endindex = 0;
                    if (startindex > 255) startindex = 255;
                    if (endindex > 255) endindex = 255;

                    if (_Palette != null)
                        for (int i = startindex; i <= endindex; i++)
                        {
                            _Palette.palette[i].IsUsed = MouseDownValue.IsUsed;
                            _Palette.palette[i].MakeTransparent = MouseDownValue.MakeTransparent;
                        }
                }
                UpdatePalettePanel();
            }
        }

        void PaletteColorBox_SizeChanged(object sender, EventArgs e)
        {
            UpdatePalettePanel();
        }

        private Image PaletteColorImage;

        private void UpdatePaletteManager()
        {
            this.comboBox_Palette.Items.Clear();
            for (int i = 0; i < PaletteManager.Palettes.Length; i++)
                if (PaletteManager.Palettes[i] != null) this.comboBox_Palette.Items.Add(PaletteManager.Palettes[i]);
            if (_Palette != null)
                this.comboBox_Palette.SelectedItem = _Palette;

            UpdatePalettePanel();
        }

        private void UpdatePalettePanel()
        {
            if (ShowPaletteOnly)
            {
                this.tableLayoutPanel1.RowStyles[0].Height = 0;
                this.tableLayoutPanel1.RowStyles[1].Height = 0;
                this.tableLayoutPanel1.RowStyles[3].Height = 0;
                this.comboBox_ColorConversionMethod.Visible = false;
                this.comboBox_Palette.Visible = false;
                this.label_ColorConversionMethod.Visible = false;
                this.button_Clone.Visible = false;
                this.button_NewCopy.Visible = false;
            }

            if (_Palette != null)
            {
                this.comboBox_ColorConversionMethod.SelectedIndex = (int)_Palette.ConversionMethod;
            }
            Size PaletteSpace = new Size(this.PaletteColorBox.Width, this.PaletteColorBox.Height);
            //8 columns, 32 rows

            PaletteColorImage = new Bitmap(PaletteSpace.Width, PaletteSpace.Height);
            Graphics PaletteColorImage_g = Graphics.FromImage(PaletteColorImage);

            bool alternate = false;
            for (int y = 0; y < PaletteSpace.Height / 5 + 1; y++)
            {
                alternate = false;
                if (y % 2 == 0) alternate = true;
                for (int x = 0; x < PaletteSpace.Width / 5 + 1; x++)
                {
                    alternate = !alternate;
                    Color checkboardcolor = Color.FromArgb(255, 0, 0, 0);
                    if (alternate)
                        checkboardcolor = Color.FromArgb(255, 255, 255, 255);
                    SolidBrush checkboardbrush = new SolidBrush(checkboardcolor);
                    PaletteColorImage_g.FillRectangle(checkboardbrush, x * 5, y * 5, 5, 5);
                }
            }

            Color c = Color.FromArgb(255, 0, 0, 0);
            //draw color palette
            double cellwidth = (double)(PaletteSpace.Width - 1) / (double)columnCount;
            double cellheight = (double)(PaletteSpace.Height - 1) / (double)rowCount;
            for (int i = 0; i < 256; i++)
            {
                int column = i / rowCount;
                int row = i % rowCount;
                if ((_Palette != null) && (_Palette.palette != null))
                {
                    c = _Palette.palette[i].Color;
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

            PaletteColorBox.Image = PaletteColorImage;
            SetPaletteUsageValues();
            SetSelectedColor();
        }
        private void SetPaletteUsageValues()
        {
            bool DrawTextInCenter = false;
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

                    int index = row + column * rowCount;

                    if (_Palette!=null)
                    {
                        int cw = p2.X - p1.X;
                        int ch = p2.Y - p1.Y;
                        if (!_Palette.palette[index].IsUsed)
                        {
                            tci_g.FillRectangle(Brushes.Black, (float)p1.X, (float)p1.Y, (float)cw, (float)ch);
                            tci_g.DrawLine(Pens.DarkRed, p1, p2);
                            tci_g.DrawLine(Pens.DarkRed, p1.X, p2.Y, p2.X, p1.Y);
                            tci_g.DrawRectangle(Pens.Red, p1.X, p1.Y, (float)cw, (float)ch);
                        }
                        if (_Palette.palette[index].MakeTransparent)
                        {
                            tci_g.DrawRectangle(Pens.Yellow, p1.X, p1.Y, (float)cw, (float)ch);
                            cw = (int)(cw / 4);
                            ch = (int)(ch / 4);
                            tci_g.DrawLine(Pens.Yellow, p1, new Point(p1.X + cw, p1.Y + ch));
                            tci_g.DrawLine(Pens.Yellow, p2, new Point(p2.X - cw, p2.Y - ch));
                            tci_g.DrawLine(Pens.Yellow, new Point(p1.X, p2.Y), new Point(p1.X + cw, p2.Y - ch));
                            tci_g.DrawLine(Pens.Yellow, new Point(p2.X, p1.Y), new Point(p2.X - cw, p1.Y + ch));
                        }


                        if (DrawTextInCenter)
                        {
                            Brush lineBrush = new System.Drawing.Drawing2D.LinearGradientBrush(
                            new Rectangle((int)(column * cellwidth - 1), (int)(row * cellheight - 1), (int)cellwidth + 3, (int)cellheight + 3),
                            Color.FromArgb(255, 0, 255, 0),
                            Color.FromArgb(255, 200, 255, 200),
                            System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal);

                            //start with a too big fontsize
                            float fontsize = 10;// (float)(cellheight * 7 / 10);
                            Font f = new System.Drawing.Font("Arial", fontsize, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));


                            string cross = "X";

                            SizeF stringsize = tci_g.MeasureString(cross, f);
                            while ((stringsize.Height < cellheight) && (stringsize.Width < cellwidth))
                            {
                                fontsize += 0.5f;
                                f = new System.Drawing.Font("Arial", fontsize, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                                stringsize = tci_g.MeasureString(cross, f);
                            }
                            while ((stringsize.Height > cellheight) || (stringsize.Width > cellwidth))
                            {
                                fontsize -= 0.5f;
                                f = new System.Drawing.Font("Arial", fontsize, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                                stringsize = tci_g.MeasureString(cross, f);
                            }

                            f = new System.Drawing.Font("Arial", fontsize, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                            stringsize = tci_g.MeasureString(cross, f);

                            int textx = (int)(p1.X + cellwidth / 2 - stringsize.Width / 2 + 1);
                            int texty = (int)(p1.Y + cellheight / 2 - stringsize.Height / 2 + 1);
                            tci_g.DrawString(cross,
                                f,
                                lineBrush,
                                new Point(textx, texty)
                                );
                        }
                    }//if value!=""
                }

            PaletteColorBox.Image = TextColorImage;
            PaletteColorImage = TextColorImage;
        }
        private void SetSelectedColor()
        {
            Image SelectedColorImage = new Bitmap(PaletteColorImage.Width, PaletteColorImage.Height);

            Graphics sci_g = Graphics.FromImage(SelectedColorImage);
            sci_g.DrawImage(PaletteColorImage, 0, 0, PaletteColorImage.Width, PaletteColorImage.Height);

            Size PaletteSpace = new Size(this.PaletteColorBox.Width, this.PaletteColorBox.Height);
            double cellwidth = (double)(PaletteSpace.Width - 1) / (double)columnCount;
            double cellheight = (double)(PaletteSpace.Height - 1) / (double)rowCount;

            if (cellheight == 0) return;

            int column = paletteSelectedColor / rowCount;
            int row = paletteSelectedColor % rowCount;


            Point p1 = new Point((int)(column * cellwidth), (int)(row * cellheight));
            Point p2 = new Point((int)((column + 1) * cellwidth), (int)((row + 1) * cellheight));

            Brush lineBrushDark;
            Brush lineBrush;
            if ((_Palette != null) && ((_Palette.palette[paletteSelectedColor].IsUsed) && (!_Palette.palette[paletteSelectedColor].MakeTransparent)))
            {
                lineBrushDark = new System.Drawing.Drawing2D.LinearGradientBrush(
                                        new Rectangle((int)(column * cellwidth - 1), (int)(row * cellheight - 1), (int)cellwidth + 3, (int)cellheight + 3),
                                        Color.FromArgb(255, 0, 50, 0),
                                        Color.FromArgb(255, 0, 200, 0),
                                        System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal);
                lineBrush = new System.Drawing.Drawing2D.LinearGradientBrush(
                                        new Rectangle((int)(column * cellwidth - 1), (int)(row * cellheight - 1), (int)cellwidth + 3, (int)cellheight + 3),
                                        Color.FromArgb(255, 0, 255, 0),
                                        Color.FromArgb(255, 200, 255, 200),
                                        System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal);
            }
            else
            {
                lineBrushDark = new System.Drawing.Drawing2D.LinearGradientBrush(
                                        new Rectangle((int)(column * cellwidth - 1), (int)(row * cellheight - 1), (int)cellwidth + 3, (int)cellheight + 3),
                                        Color.FromArgb(255, 50, 0, 0),
                                        Color.FromArgb(255, 200, 0, 0),
                                        System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal);
                lineBrush = new System.Drawing.Drawing2D.LinearGradientBrush(
                                        new Rectangle((int)(column * cellwidth - 1), (int)(row * cellheight - 1), (int)cellwidth + 3, (int)cellheight + 3),
                                        Color.FromArgb(255, 255, 0, 0),
                                        Color.FromArgb(255, 255, 200, 200),
                                        System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal);
            }

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
                if (fontsize <= 6) break;
                f = new System.Drawing.Font("Arial", fontsize, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                stringsize = sci_g.MeasureString(paletteSelectedColor.ToString(), f);
            }
            if (fontsize > 6)
            {
                f = new System.Drawing.Font("Arial", fontsize, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                stringsize = sci_g.MeasureString(paletteSelectedColor.ToString(), f);

                int textx = (int)(p1.X + cellwidth / 2 - stringsize.Width / 2 + 1);
                int texty = (int)(p1.Y + cellheight / 2 - stringsize.Height / 2 + 1);
                sci_g.DrawString(paletteSelectedColor.ToString(),
                    f,
                    lineBrush,
                    new Point(textx, texty)
                    );
            }

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

            PaletteColorBox.Image = SelectedColorImage;
            PaletteColorImage = SelectedColorImage;
        }

        private void comboBox_ColorConversionMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_Palette != null)
                _Palette.ConversionMethod = (ColorConversionMethod)this.comboBox_ColorConversionMethod.SelectedIndex;
        }

        private void button_NewCopy_Click(object sender, EventArgs e)
        {
            if (_Palette != null)
            {
                _Palette = PaletteManager.GetPalette(new CPalette(_Palette.PaletteFile));

                UpdatePaletteManager();
                OnPaletteChanged(this, null);
            }
        }

        private void button_Clone_Click(object sender, EventArgs e)
        {
            if (_Palette != null)
            {
                _Palette = PaletteManager.GetPalette(_Palette, true);
                UpdatePaletteManager();
                OnPaletteChanged(this, null);
            }
        }

        private void comboBox_Palette_SelectedIndexChanged(object sender, EventArgs e)
        {
            _Palette = PaletteManager.GetPalette((CPalette)this.comboBox_Palette.SelectedItem);
            UpdatePalettePanel();
        }

    }
}
