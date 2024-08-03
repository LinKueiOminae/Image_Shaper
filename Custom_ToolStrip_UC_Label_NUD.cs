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
    public partial class Custom_ToolStrip_UC_Label_NUD : UserControl
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

        public decimal Value
        {
            get { return this.numericUpDown1.Value; }
            set
            {
                decimal d = value;
                if (d > this.numericUpDown1.Maximum) d = this.numericUpDown1.Maximum;
                if (d < this.numericUpDown1.Minimum) d = this.numericUpDown1.Minimum;
                this.numericUpDown1.Value = d;
            }
        }

        public decimal Maximum
        {
            get { return this.numericUpDown1.Maximum; }
            set { this.numericUpDown1.Maximum = value; }
        }

        public decimal Minimum
        {
            get { return this.numericUpDown1.Minimum; }
            set { this.numericUpDown1.Minimum = value; }
        }

        public Custom_ToolStrip_UC_Label_NUD()
        {
            InitializeComponent();
        }
    }
}
