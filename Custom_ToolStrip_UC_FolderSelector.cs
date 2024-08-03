using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ImageShaper
{
    public partial class Custom_ToolStrip_UC_FolderSelector : UserControl
    {
        public float LabelWidth
        {
            get { return this.tableLayoutPanel1.ColumnStyles[0].Width; }
            set { this.tableLayoutPanel1.ColumnStyles[0].Width = value; }
        }

        public override string Text
        {
            get { return this.label1.Text; }
            set { this.label1.Text = value; }
        }

        public string Value
        {
            get { return this.textBox1.Text; }
            set { this.textBox1.Text = value; }
        }

        private bool _FileSelector=false;
        public bool FileSelector
        {
            get { return _FileSelector; }
            set { _FileSelector = value; }
        }

        public Custom_ToolStrip_UC_FolderSelector()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!_FileSelector)
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.SelectedPath = this.textBox1.Text;
                if (fbd.ShowDialog() == DialogResult.OK)
                    this.textBox1.Text = fbd.SelectedPath;
            }
            else
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.InitialDirectory = this.textBox1.Text;
                ofd.Multiselect = false;
                if (ofd.ShowDialog() == DialogResult.OK)
                    this.textBox1.Text = ofd.FileName;
            }
        }
    }
}
