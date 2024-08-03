using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

public partial class CheckGroupBox : GroupBox
{
    private CheckBox checkBox_Main;
    public bool Checked
    {
        get { return this.checkBox_Main.Checked; }
        set
        {
            this.checkBox_Main.Checked = value;
        }
    }

    public override string Text
    {
        get { return this.checkBox_Main.Text; }
        set { this.checkBox_Main.Text = value;}
    }

    public CheckGroupBox()
    {
        checkBox_Main = new CheckBox();
        checkBox_Main.Name = "#checkBox_Main#";
        checkBox_Main.Location = new Point(5, -1);
        checkBox_Main.CheckAlign= ContentAlignment.MiddleLeft;
        checkBox_Main.AutoSize = true;
        checkBox_Main.CheckedChanged += new EventHandler(checkBox_Main_CheckedChanged);
        this.Controls.Add(checkBox_Main);
    }

    void checkBox_Main_CheckedChanged(object sender, EventArgs e)
    {
        foreach (Control c in this.Controls)
        {
            if (c.Name != checkBox_Main.Name)
                c.Enabled = this.checkBox_Main.Checked;
        }
    }
}
