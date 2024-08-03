using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ImageShaper
{
    public partial class Form_FireFLHFinder : Form
    {
        private CPalette _Palette;
        public CPalette Palette
        {
            get { return _Palette; }
            set
            {
                _Palette = value;
                if (_Palette != null)
                    this.button_LoadPalette.Text = "Palette: " + _Palette.PaletteName;
                UpdateImages();
            }
        }

        public string InitialDirectory;

        private string SHPFile;

        public Form_FireFLHFinder()
        {
            InitializeComponent();
            this.uC_ImageCanvas_Left.ShowColorInfo = false;
            this.uC_ImageCanvas_Right.ShowColorInfo = false;
            this.uC_ImageCanvas_Left.LocationFromCenter = true;
            this.uC_ImageCanvas_Right.LocationFromCenter = true;
            this.uC_ImageCanvas_Left.ShowCenterCross = true;
            this.uC_ImageCanvas_Right.ShowCenterCross = true;
            this.uC_ImageCanvas_Left.CaptureFocusOnMouseOver = false;
            this.uC_ImageCanvas_Right.CaptureFocusOnMouseOver = false;

            this.numericUpDown_Zoom.IncrementByMouseWheel = 1;
            this.numericUpDown_FrameLeft.Minimum = 0;
            this.numericUpDown_FrameRight.Minimum = 0;
            this.numericUpDown_FrameLeft.Maximum = 0;
            this.numericUpDown_FrameRight.Maximum = 0;
            this.radioButton_FireFLH.CheckedChanged += new EventHandler(radioButton_CheckedChanged);
            this.radioButton_FirePixelOffset.CheckedChanged += new EventHandler(radioButton_CheckedChanged);
            this.radioButton_DamageFireOffset.CheckedChanged += new EventHandler(radioButton_CheckedChanged);
            this.radioButton_MuzzleFlash.CheckedChanged += new EventHandler(radioButton_CheckedChanged);

            this.radioButton_FirePixelOffset.Checked = true;

            this.numericUpDown_FrameLeft.ValueChanged += new EventHandler(numericUpDown_FrameLeft_ValueChanged);
            this.numericUpDown_FrameRight.ValueChanged += new EventHandler(numericUpDown_FrameRight_ValueChanged);

            this.uC_ImageCanvas_Left.PixelClicked += new EventHandler<LocationEventArgs>(uC_ImageCanvas_Left_PixelClicked);
            this.uC_ImageCanvas_Right.PixelClicked += new EventHandler<LocationEventArgs>(uC_ImageCanvas_Right_PixelClicked);

            this.checkBox_IsLaser.CheckedChanged += new EventHandler(checkBox_IsLaser_CheckedChanged);

            ToolTip toolTip1 = new ToolTip();
            // Set up the delays for the ToolTip.
            toolTip1.AutoPopDelay = 1000000;
            toolTip1.InitialDelay = 100;
            toolTip1.ReshowDelay = 100;
            // Force the ToolTip text to be displayed whether or not the form is active.
            toolTip1.ShowAlways = true;

            // Set up the ToolTip text for the Button and Checkbox.
            toolTip1.SetToolTip(this.checkBox_IsLaser, "When using a Laser, the game (TS) renders it 1 pixel to the right.\nCheck this to get the correct FirePixelOffset when using a Laser.\nThis doesn't work for FireFLH. For that you have to nudge the entire unit 1 pixel to the right in the canvas.");
        }

        void checkBox_IsLaser_CheckedChanged(object sender, EventArgs e)
        {
            AdjustCheckBoxOptions();
        }
        private void AdjustCheckBoxOptions()
        {
            if (this.radioButton_FirePixelOffset.Checked)
            {
                this.uC_ImageCanvas_Left.CenterOffset = this.checkBox_IsLaser.Checked ? new Point(1, 0) : new Point(0, 0);
                this.uC_ImageCanvas_Right.CenterOffset = this.checkBox_IsLaser.Checked ? new Point(1, 0) : new Point(0, 0);
            }
            else
            {
                this.uC_ImageCanvas_Left.CenterOffset = new Point(0, 0);
                this.uC_ImageCanvas_Right.CenterOffset = new Point(0, 0);
            }
        }

        private struct FLH
        {
            private int _F;
            internal int F
            {
                get { return (int)Math.Round(_F / F_Factor); }
                set { _F = (int)(value * F_Factor); }
            }
            private int _L;
            internal int L
            {
                get { return (int)Math.Round(_L / L_Factor); }
                set { _L = (int)(value * L_Factor); }
            }

            private int _H;
            internal int H
            {
                get { return (int)Math.Round(_H / H_Factor); }
                set { _H = (int)(value * H_Factor); }
            }

            internal FLH(int F, int L, int H)
            {
                F_Factor = 7.5;
                L_Factor = 7.75;
                H_Factor = 8.5;

                _F = F;
                _L = L;
                _H = H;
            }
            public override string ToString()
            {
                return _F.ToString() + "," + _L.ToString() + "," + _H.ToString();
            }
            internal readonly double F_Factor;
            internal readonly double L_Factor;
            internal readonly double H_Factor;
        }
        private FLH FireFLH = new FLH(0, 0, 0);
        private List<Point> Points = new List<Point>();
        void uC_ImageCanvas_Left_PixelClicked(object sender, LocationEventArgs e)
        {
            //richtextbox converts \r\n (Environment.Newline) internally into \n
            int count = this.richTextBox1.Text.Split('\n').Length - 1;
            if (this.radioButton_FirePixelOffset.Checked)
            {
                this.richTextBox1.Clear();
                this.richTextBox1.SelectedText = "PrimaryFirePixelOffset=" + e.Location.X.ToString() + "," + e.Location.Y.ToString() + "\n";
                this.uC_ImageCanvas_Left.PreviewPixel = new Point[1] { e.Location };

            }
            if (this.radioButton_DamageFireOffset.Checked)
            {
                this.richTextBox1.SelectedText = "DamageFireOffset" + count + "=" + e.Location.X.ToString() + "," + e.Location.Y.ToString() + "\n";
                Points.Add(e.Location);
                this.uC_ImageCanvas_Left.PreviewPixel = Points.ToArray();
            }
            if (this.radioButton_MuzzleFlash.Checked)
            {
                this.richTextBox1.SelectedText = "MuzzleFlash" + count + "=" + e.Location.X.ToString() + "," + e.Location.Y.ToString() + "\n";
                Points.Add(e.Location);
                this.uC_ImageCanvas_Left.PreviewPixel = Points.ToArray();
            }

            if (this.radioButton_FireFLH.Checked)
            {
                FireFLH.F = (int)(-e.Location.X);
                FireFLH.H = (int)(-e.Location.Y);
                this.richTextBox1.Clear();
                this.richTextBox1.SelectedText = "FireFLH=" + FireFLH.ToString();
                this.uC_ImageCanvas_Left.PreviewPixel = new Point[1] { new Point(-FireFLH.F , (int)Math.Round(FireFLH.L / 1.9 - FireFLH.H)) };
                this.uC_ImageCanvas_Right.PreviewPixel = new Point[1] { new Point(-FireFLH.L, (int)Math.Round(-FireFLH.F / 1.9 - FireFLH.H)) };
            }
        }
        void uC_ImageCanvas_Right_PixelClicked(object sender, LocationEventArgs e)
        {
            if (this.radioButton_FireFLH.Checked)
            {
                FireFLH.L = (int)(-e.Location.X);
                this.richTextBox1.Clear();
                this.richTextBox1.SelectedText = "FireFLH=" + FireFLH.ToString();
                this.uC_ImageCanvas_Left.PreviewPixel = new Point[1] { new Point(-FireFLH.F, (int)Math.Round(FireFLH.L / 1.9 - FireFLH.H)) };
                this.uC_ImageCanvas_Right.PreviewPixel = new Point[1] { new Point(-FireFLH.L, (int)Math.Round(-FireFLH.F / 1.9 - FireFLH.H)) };
            }
        }

        void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            this.richTextBox1.Clear();
            Points.Clear();
            this.uC_ImageCanvas_Left.PreviewPixel = new Point[0];
            this.uC_ImageCanvas_Right.PreviewPixel = new Point[0];
            this.groupBox_Right.Enabled = this.radioButton_FireFLH.Checked;
            this.tableLayoutPanel3_Canvas.Visible = false;//without this, the left canvas is showing a scrollbar for some reason (it seems the canvas is still set to the old size and didn't get the new smaller size when switching to FireFLH)
            this.tableLayoutPanel3_Canvas.ColumnStyles[1].Width = this.radioButton_FireFLH.Checked ? 50 : 0;
            this.tableLayoutPanel3_Canvas.Visible = true;
            this.groupBox_Left.Text = this.radioButton_FireFLH.Checked ? "Unit facing west" : "Pixel Offset";
            this.groupBox_Right.Text = "Unit facing north";
            if (this.radioButton_FireFLH.Checked)
                FireFLH = new FLH(0, 0, 0);
            AdjustCheckBoxOptions();
        }


        void numericUpDown_FrameLeft_ValueChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(SHPFile)) return;
            if (this.numericUpDown_FrameLeft.Focused)
                this.uC_ImageCanvas_Left.SetImage(CSHaPer.GetFrame(SHPFile, (int)this.numericUpDown_FrameLeft.Value, _Palette));
        }

        void numericUpDown_FrameRight_ValueChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(SHPFile)) return;
            if (this.numericUpDown_FrameRight.Focused)
                this.uC_ImageCanvas_Right.SetImage(CSHaPer.GetFrame(SHPFile, (int)this.numericUpDown_FrameRight.Value, _Palette));
        }

        private void UpdateImages()
        {
            if (string.IsNullOrEmpty(SHPFile)) return;
            this.uC_ImageCanvas_Left.SetImage(CSHaPer.GetFrame(SHPFile, (int)this.numericUpDown_FrameLeft.Value, _Palette));
            this.uC_ImageCanvas_Right.SetImage(CSHaPer.GetFrame(SHPFile, (int)this.numericUpDown_FrameRight.Value, _Palette));
        }

        private void button_LoadSHP_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Load SHP";
            ofd.InitialDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            if (!string.IsNullOrEmpty(InitialDirectory)) ofd.InitialDirectory = this.InitialDirectory;
            if (Cinimanager.inisettings.LastFireFLHFinderDirectory != "")
                ofd.InitialDirectory = Cinimanager.inisettings.LastFireFLHFinderDirectory;
            ofd.FileName = "";
            ofd.Filter = "SHP(TS) files|*.shp";
            ofd.Multiselect = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (!CSHaPer.IsSHP(ofd.FileName))
                {
                    MessageBox.Show("No valid SHP file!");
                    return;
                }
                SHPFile = ofd.FileName;
                Cinimanager.inisettings.LastFireFLHFinderDirectory = System.IO.Path.GetDirectoryName(ofd.FileName);

                CImageFile[] _SHPFrames = CSHaPer.GetFrames(ofd.FileName, 0);

                int oldleft = (int)this.numericUpDown_FrameLeft.Value;
                int oldright = (int)this.numericUpDown_FrameRight.Value;
                this.numericUpDown_FrameLeft.Value = 0;
                this.numericUpDown_FrameRight.Value = 0;
                this.numericUpDown_FrameLeft.Maximum = _SHPFrames.Length - 1;
                this.numericUpDown_FrameRight.Maximum = _SHPFrames.Length - 1;
                if (oldleft < this.numericUpDown_FrameLeft.Maximum) this.numericUpDown_FrameLeft.Value = oldleft;
                if (oldright < this.numericUpDown_FrameRight.Maximum) this.numericUpDown_FrameRight.Value = oldright;

                UpdateImages();
            }
        }

        private void button_LoadPalette_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Load Palette";
            ofd.InitialDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            if ((_Palette != null) && (!string.IsNullOrEmpty(_Palette.PaletteFile))) ofd.InitialDirectory = System.IO.Path.GetDirectoryName(_Palette.PaletteFile);
            ofd.FileName = "";
            ofd.Filter = "Palette files|*.pal";
            ofd.Multiselect = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                this.Palette = new CPalette(ofd.FileName);
            }
        }

        private void numericUpDown_Zoom_ValueChanged(object sender, EventArgs e)
        {
            this.uC_ImageCanvas_Left.ZoomLevel = (double)this.numericUpDown_Zoom.Value;
            this.uC_ImageCanvas_Right.ZoomLevel = (double)this.numericUpDown_Zoom.Value;
        }

        private void button_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
