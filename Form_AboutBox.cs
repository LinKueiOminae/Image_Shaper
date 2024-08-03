using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

partial class Form_AboutBox : Form
{
    public Form_AboutBox()
    {
        InitializeComponent();
        this.Text = String.Format("About {0}", AssemblyTitle);
        this.textBox_Description.WordWrap = false;
        this.textBox_Description.Text = "";
        this.textBox_Description.LinkClicked += new LinkClickedEventHandler(textBox_Description_LinkClicked);

        this.pictureBox1.MouseDoubleClick += new MouseEventHandler(pictureBox1_MouseDoubleClick);
    }

    public void SetImage(string resourceStreamLocation)
    {
        try
        {
            Assembly myAssembly = Assembly.GetExecutingAssembly();
            Stream myStream = myAssembly.GetManifestResourceStream(resourceStreamLocation);

            this.pictureBox1.Image = Image.FromStream(myStream);
            this.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
        }
        catch { }
    }

    void pictureBox1_MouseDoubleClick(object sender, MouseEventArgs e)
    {
        //string target = "http://www.ppmsite.com";
        //try
        //{
        //    System.Diagnostics.Process.Start(target);
        //}
        //catch (System.ComponentModel.Win32Exception noBrowser)
        //{
        //    if (noBrowser.ErrorCode == -2147467259)
        //        MessageBox.Show(noBrowser.Message);
        //}
        //catch (System.Exception other)
        //{
        //    MessageBox.Show(other.Message);
        //}
    }

    void textBox_Description_LinkClicked(object sender, LinkClickedEventArgs e)
    {
        //this prevents that a click on a link makes the rtf scroll to the bottom
        //now the selection is set on the mouseclick location in the text
        RichTextBox rtb = sender as RichTextBox;
        int position = rtb.GetCharIndexFromPosition(this.textBox_Description.PointToClient(Control.MousePosition));
        this.textBox_Description.SelectionStart = position;

        if (e.LinkText.Contains("http://"))
        {
            string link = e.LinkText;
            if (link.Split('#').Length > 1) link = link.Split('#')[1];
            try
            {
                System.Diagnostics.Process.Start(link);
            }
            catch { }
        }
        else
        {
            if (System.IO.File.Exists(e.LinkText))
            {
                try
                {
                    System.Diagnostics.Process.Start(e.LinkText);
                }
                catch { }
            }
            else if (System.IO.Directory.Exists(e.LinkText))
            {
                System.Diagnostics.Process explorer = new System.Diagnostics.Process();
                explorer.StartInfo.FileName = "explorer.exe";
                explorer.StartInfo.Arguments = e.LinkText;
                explorer.StartInfo.UseShellExecute = false; //needs to be false to work with custom environment variables
                explorer.StartInfo.RedirectStandardOutput = false;
                try
                {
                    explorer.Start();
                }
                catch { }
            }
        }
    }

    public void AddEmptyLine()
    {
        this.textBox_Description.SelectedText = "\r\n\r\n";
    }
    public void AddEmptyLine(int newlines)
    {
        for (int i = 0; i < newlines; i++)
            this.textBox_Description.SelectedText = "\r\n";
    }

    public void AddUnderLine()
    {
        this.textBox_Description.SelectedText = "____________________________________\r\n\r\n";
    }
    public void AddUnderLine(Color color)
    {
        Color oldcolor = this.textBox_Description.SelectionColor;
        this.textBox_Description.SelectionColor = color;
        this.textBox_Description.SelectedText = "____________________________________\r\n\r\n";
        this.textBox_Description.SelectionColor = oldcolor;
    }

    /// <summary>
    /// adds the text as a new line
    /// </summary>
    public void AddTextLine(string text)
    {
        AddText("\r\n" + text);
    }
    public void AddTextLine(string text, Color color, Font font)
    {
        AddText("\r\n" + text, color, font);
    }

    public void AddText(string text)
    {
        this.textBox_Description.SelectedText = text;
    }
    public void AddText(string text, Color color)
    {
        Color oldcolor = this.textBox_Description.SelectionColor;
        this.textBox_Description.SelectionColor = color;
        this.textBox_Description.SelectedText = text;
        this.textBox_Description.SelectionColor = oldcolor;
    }
    public void AddText(string text, Color color, Font font)
    {
        Color oldcolor = this.textBox_Description.SelectionColor;
        System.Drawing.Font oldfont = this.textBox_Description.SelectionFont;
        this.textBox_Description.SelectionColor = color;
        this.textBox_Description.SelectionFont = font;

        this.textBox_Description.SelectedText = text;

        this.textBox_Description.SelectionColor = oldcolor;
        this.textBox_Description.SelectionFont = oldfont;
    }

    public void InsertLink(string text)
    {
        this.textBox_Description.InsertLink(text);
    }
    public void InsertLink(string text, int position)
    {
        this.textBox_Description.InsertLink(text, position);
    }
    public void InsertLink(string text, string hyperlink)
    {
        this.textBox_Description.InsertLink(text, hyperlink);
    }
    public void InsertLink(string text, string hyperlink, int position)
    {
        this.textBox_Description.InsertLink(text, hyperlink, position);
    }


    private void button_close_Click(object sender, EventArgs e)
    {
        this.Close();
    }

    #region Assembly Attribute Accessors

    public string AssemblyTitle
    {
        get
        {
            return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
        }
    }

    public string AssemblyVersion
    {
        get
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
    }

    public string AssemblyDescription
    {
        get
        {
            return "";
        }
    }

    public string AssemblyProduct
    {
        get
        {
            return "";
        }
    }

    public string AssemblyCopyright
    {
        get
        {
            return "";
        }
    }

    public string AssemblyCompany
    {
        get
        {
            return "";
        }
    }
    #endregion

}
