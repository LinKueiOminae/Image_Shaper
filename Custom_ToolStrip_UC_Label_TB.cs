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
    public partial class Custom_ToolStrip_UC_Label_TB : UserControl
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

        public Custom_ToolStrip_UC_Label_TB()
        {
            InitializeComponent();
        }
    }
}
