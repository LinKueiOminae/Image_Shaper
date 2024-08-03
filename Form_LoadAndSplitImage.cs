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
    public partial class Form_LoadAndSplitImage : Form
    {
        public string[] images;

        private string tempPath;
        private string originalFileName;
        private string originalFileExtension;
        private Image originalImage;
        public Form_LoadAndSplitImage(string filename, string tempPath)
        {
            InitializeComponent();
            this.DialogResult = DialogResult.Abort;
            this.images = new string[0];

            this.radioButton_RowWise.Checked = true;

            this.Text = "Load and split image";

            this.tempPath = tempPath;
            originalImage = Image.FromFile(filename);
            originalFileName = System.IO.Path.GetFileNameWithoutExtension(filename);
            originalFileExtension = System.IO.Path.GetExtension(filename);

            this.uC_ImageCanvas1_Original.SetImage(originalImage);
            this.numericUpDown_Width.Maximum = this.uC_ImageCanvas1_Original.Image.Width;
            this.numericUpDown_Height.Maximum = this.uC_ImageCanvas1_Original.Image.Height;
            this.numericUpDown_Width.Value = this.uC_ImageCanvas1_Original.Image.Width;
            this.numericUpDown_Height.Value = this.uC_ImageCanvas1_Original.Image.Height;

            Size s = originalImage.Size;
            int t1 = s.Width % s.Height;
            int t2 = s.Height % s.Width;
            if ((t2 == 0) && (t1 > 0) && (t1 < this.numericUpDown_Height.Maximum)) this.numericUpDown_Height.Value = t1;
            if ((t1 == 0) && (t2 > 0) && (t2 < this.numericUpDown_Width.Maximum)) this.numericUpDown_Width.Value = t2;
            

            splitImages = new List<Image>();
            CreateSplitImages(false);
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            List<string> result = new List<string>();
            for (int i = 0; i < splitImages.Count; i++)
            {
                string framefilename = System.IO.Path.Combine(tempPath, originalFileName + "_" + i.ToString("D4") + ".png");
                splitImages[i].Save(framefilename);
                result.Add(framefilename);
            }
            images = result.ToArray();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private List<Image> splitImages;
        private void CreateSplitImages(bool ShowCurrentFrameOnly)
        {
            Bitmap org = (Bitmap)originalImage;
            Size s = new Size((int)this.numericUpDown_Width.Value, (int)this.numericUpDown_Height.Value);
            Size s2 = new Size(s.Width - 1, s.Height - 1);//drawn rectangle is 1 pixel smaller in width/height

            Bitmap org_rect = new Bitmap(org.Width, org.Height);
            Graphics g = Graphics.FromImage(org_rect);//crashes when loading an indexed image
            g.DrawImage(org, 0, 0);

            Pen p = new Pen(Color.FromArgb(50, 255, 255, 255));
            Pen py = new Pen(Color.FromArgb(255, 100, 255, 100));

            int rows = org.Height / s.Height;
            int cols = org.Width / s.Width;

            this.label_ColumnsCount.Text = "#" + cols.ToString();
            this.label_RowsCount.Text = "#" + rows.ToString();

            List<Image> result = new List<Image>();

            if (this.radioButton_RowWise.Checked)
                for (int r = 0; r < rows; r++)
                    for (int c = 0; c < cols; c++)
                    {
                        g.DrawLines(p, new Point[5]{
                            new Point(c * s.Width, r * s.Height),
                            new Point(c * s.Width+s2.Width, r * s.Height),
                            new Point(c * s.Width+s2.Width, r * s.Height + s2.Height),
                            new Point(c * s.Width, r * s.Height + s2.Height),
                            new Point(c * s.Width, r * s.Height)
                            });//Rectangle crashes with Width=0 or Height=0, thus using s2 for preview drawing and s for rect
                        Rectangle rect = new Rectangle(new Point(c * s.Width, r * s.Height), s);
                        result.Add(org.Clone(rect, org.PixelFormat));
                    }
            if (this.radioButton_ColWise.Checked)
                for (int c = 0; c < cols; c++)
                    for (int r = 0; r < rows; r++)
                    {
                        g.DrawLines(p, new Point[5]{
                            new Point(c * s.Width, r * s.Height),
                            new Point(c * s.Width+s2.Width, r * s.Height),
                            new Point(c * s.Width+s2.Width, r * s.Height + s2.Height),
                            new Point(c * s.Width, r * s.Height + s2.Height),
                            new Point(c * s.Width, r * s.Height)
                            });
                        Rectangle rect = new Rectangle(new Point(c * s.Width, r * s.Height), s);
                        result.Add(org.Clone(rect, org.PixelFormat));
                    }

            if (this.radioButton_RowWise.Checked)
                for (int r = 0; r < rows; r++)
                    for (int c = 0; c < cols; c++)
                    {
                        if (r * cols + c == this.numericUpDown_PreviewFrame.Value)
                            g.DrawLines(py, new Point[5]{
                            new Point(c * s.Width, r * s.Height),
                            new Point(c * s.Width+s2.Width, r * s.Height),
                            new Point(c * s.Width+s2.Width, r * s.Height + s2.Height),
                            new Point(c * s.Width, r * s.Height + s2.Height),
                            new Point(c * s.Width, r * s.Height)
                            });

                    }
            if (this.radioButton_ColWise.Checked)
                for (int c = 0; c < cols; c++)
                    for (int r = 0; r < rows; r++)
                    {
                        if (c * rows + r == this.numericUpDown_PreviewFrame.Value)
                            g.DrawLines(py, new Point[5]{
                            new Point(c * s.Width, r * s.Height),
                            new Point(c * s.Width+s2.Width, r * s.Height),
                            new Point(c * s.Width+s2.Width, r * s.Height + s2.Height),
                            new Point(c * s.Width, r * s.Height + s2.Height),
                            new Point(c * s.Width, r * s.Height)
                            });
                    }


            this.uC_ImageCanvas1_Original.SetImage(org_rect);
            g.Dispose();

            if (!ShowCurrentFrameOnly)
                splitImages = result;
            this.numericUpDown_PreviewFrame.Maximum = splitImages.Count - 1;
            UpdatePreview();
        }

        private void UpdatePreview()
        {
            if (splitImages.Count > 0)
            {
                this.uC_ImageCanvas2_Preview.SetImage(splitImages[(int)this.numericUpDown_PreviewFrame.Value]);
            }
        }

        private void numericUpDown_Width_ValueChanged(object sender, EventArgs e)
        {
            if (this.numericUpDown_Width.Focused)
                CreateSplitImages(false);
        }

        private void numericUpDown_Height_ValueChanged(object sender, EventArgs e)
        {
            if (this.numericUpDown_Height.Focused)
                CreateSplitImages(false);
        }

        private void numericUpDown_PreviewFrame_ValueChanged(object sender, EventArgs e)
        {
            if (this.numericUpDown_PreviewFrame.Focused)
            {
                CreateSplitImages(true);
            }
        }

        private void radioButton_RowWise_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton_RowWise.Focused)
                CreateSplitImages(false);
        }

        private void radioButton_ColWise_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton_ColWise.Focused)
                CreateSplitImages(false);
        }
    }
}
