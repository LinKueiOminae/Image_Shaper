using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ImageShaper;

namespace ColorCheck
{
    public partial class Form_PaletteChecker : Form
    {
        private CPalette palette;
        public CPalette Palette
        {
            get { return palette; }
            set
            {
                palette = value;
                UpdateInterface();
            }
        }

        private string defaultPath = "";
        public string DefaultPath
        {
            get { return defaultPath; }
            set
            {
                defaultPath = value;
                if ((defaultPath != null) && (!defaultPath.EndsWith("\\"))) defaultPath += "\\";
                UpdateInterface();
            }
        }

        private long[] paletteUsage;
        private EnhancedMessageBox enhmb;

        void Form_PaletteChecker_Resize(object sender, EventArgs e)
        {
            this.listView_MainFileList_Column_Name.Width = (int)(this.listView_MainFileList.Width - 20);
        }

        public Form_PaletteChecker()
        {
            InitializeComponent();
            this.uC_EditorPalette1.PaletteMode = 2;

            this.Text = "Palette Analyzer";

            this.Resize += new EventHandler(Form_PaletteChecker_Resize);
            paletteUsage = new long[256];
            this.listView_MainFileList.MultiSelect = true;

            this.listView_MainFileList.SelectedIndexChanged += new EventHandler(listView_MainFileList_SelectedIndexChanged);
            this.listView_MainFileList.KeyDown += new KeyEventHandler(listView_MainFileList_KeyDown);

            this.radioButton_AnalyzePaletteUsage.Checked = true;
            this.radioButton_ConvertPalette.CheckedChanged += new EventHandler(radioButton_ConvertPalette_CheckedChanged);
            this.radioButton_ListFilesUsingColor.CheckedChanged += new EventHandler(radioButton_ListFilesUsingColor_CheckedChanged);
            UpdateInterface();
        }

        void listView_MainFileList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A && e.Control)
            {
                foreach (ListViewItem item in this.listView_MainFileList.Items)
                {
                    item.Selected = true;
                }
            }
        }

        void listView_MainFileList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((this.listView_MainFileList.SelectedItems.Count == 1) && (this.radioButton_AnalyzePaletteUsage.Checked))
            {
                AnalyzePaletteUsage(false);
            }
        }

        void radioButton_ListFilesUsingColor_CheckedChanged(object sender, EventArgs e)
        {
            UpdateInterface();
        }

        void radioButton_ConvertPalette_CheckedChanged(object sender, EventArgs e)
        {
            UpdateInterface();
        }

        private void UpdateInterface()
        {
            this.listView_MainFileList_Column_Name.Width = (int)(this.listView_MainFileList.Width - 20);
            this.uC_EditorPalette1.PaletteUsage = paletteUsage;
            this.uC_EditorPalette1.UseFixedPalette = this.palette;
            this.uC_EditorPalette1.ShowPaletteModes = false;

            this.button_PaletteConversionScheme.Enabled = false;
            this.textBox_PaletteConversionScheme.Enabled = false;

            if (this.radioButton_ConvertPalette.Checked)
            {
                this.button_PaletteConversionScheme.Enabled = true;
                this.textBox_PaletteConversionScheme.Enabled = true;
                this.uC_EditorPalette1.PaletteUsage = null;
            }
            
            if (this.radioButton_AnalyzePaletteUsage.Checked)
                this.uC_EditorPalette1.PaletteUsage = this.paletteUsage;

            if (this.radioButton_ListFilesUsingColor.Checked)
                this.uC_EditorPalette1.AllowColorSelection = true;
            else 
                this.uC_EditorPalette1.AllowColorSelection = false;
        }

        private void button_SelectSHPs_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = defaultPath;
            ofd.Filter = "SHPs|*.shp";
            ofd.Multiselect = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                foreach (string file in ofd.FileNames)
                {
                    ListViewItem item = new ListViewItem();
                    item.Text = System.IO.Path.GetFileName(file);
                    item.Tag = file;
                    this.listView_MainFileList.Items.Add(item);
                }
            }
        }

        private void button_PaletteConversionScheme_Click(object sender, EventArgs e)
        {
        }

        private void button_Start_Click(object sender, EventArgs e)
        {
            if (this.radioButton_AnalyzePaletteUsage.Checked)
                AnalyzePaletteUsage(true);
            if (this.radioButton_ListFilesUsingColor.Checked)
                ListFilesUsingColor();
            if (this.radioButton_ConvertPalette.Checked)
                ConvertPalette();
        }

        private void AnalyzePaletteUsage(bool writeResultFile)
        {
            int filescount = this.listView_MainFileList.SelectedItems.Count;

            if (filescount > 1)
            {
                enhmb = new EnhancedMessageBox();
                enhmb.StartPosition = FormStartPosition.Manual;
                enhmb.Location = new Point(this.Location.X, this.Location.Y);
                enhmb.SetText("Processing files.\nThis may take a few seconds.");
                enhmb.SetCaption("Processing files.");
                enhmb.ProgressBarVisible(true);
                enhmb.button_OK_text("Abort");
                enhmb.Show();


                Cursor.Current = Cursors.WaitCursor;
                Application.DoEvents(); //finish showing the dialog before starting the cpu-intensive operation

                enhmb.ProgressBarSize(filescount);
            }
            int pb = 0;

            paletteUsage = new long[256];

            foreach (ListViewItem item in this.listView_MainFileList.SelectedItems)
            {
                pb++;
                if (filescount > 1)
                {
                    enhmb.SetText("Processing files.\nThis may take a few seconds.\n\nFile: " + item.Text);
                    enhmb.ProgressBarValue((int)pb);
                    Application.DoEvents();
                    //accept user input and when the messagebox is closed, stop working
                    if (enhmb.Visible == false)
                        break;
                }

                int[] shpPaletteUsage = CSHaPer.GetSHPPaletteUsage(item.Tag.ToString());
                for (int i = 0; i < shpPaletteUsage.Length; i++)
                    paletteUsage[i] += shpPaletteUsage[i];


            }//foreach file
            if (filescount > 1)
            {
                enhmb.Close();
                Application.DoEvents(); //finish showing the dialog before starting the cpu-intensive operation
                Cursor.Current = Cursors.Default;
            }
            this.uC_EditorPalette1.PaletteUsage = paletteUsage;
            //create result file
            if (writeResultFile)
            {
                string result = "";
                result = "Palette Usage Result\r\n";
                result += "Colorindex = Number of pixel using that color\r\n\r\n";
                for (int i = 0; i < paletteUsage.Length; i++) result += string.Format("{0:000}", i) + " = " + paletteUsage[i].ToString() + "\r\n";
                System.IO.File.WriteAllText(defaultPath + "#PaletteUsage.txt", result);
                System.Diagnostics.Process.Start(defaultPath + "#PaletteUsage.txt");
            }
        }

        private void ListFilesUsingColor()
        {
            enhmb = new EnhancedMessageBox();
            enhmb.StartPosition = FormStartPosition.Manual;
            enhmb.Location = new Point(this.Location.X, this.Location.Y);
            enhmb.SetText("Processing files.\nThis may take a few seconds.");
            enhmb.SetCaption("Processing files.");
            enhmb.ProgressBarVisible(true);
            enhmb.button_OK_text("Abort");
            enhmb.Show();

            Cursor.Current = Cursors.WaitCursor;
            Application.DoEvents(); //finish showing the dialog before starting the cpu-intensive operation

            int filescount = this.listView_MainFileList.SelectedItems.Count;
            enhmb.ProgressBarSize(filescount);
            int pb = 0;

            foreach (ListViewItem item in this.listView_MainFileList.SelectedItems)
            {
                pb++;
                enhmb.SetText("Processing files.\nThis may take a few seconds.\n\nFile: " + item.Text);
                enhmb.ProgressBarValue((int)pb);
                Application.DoEvents();
                //accept user input and when the messagebox is closed, stop working
                if (enhmb.Visible == false)
                    break;

                item.Selected = CSHaPer.IsColorUsed(item.Tag.ToString(), (byte)this.uC_EditorPalette1.PaletteSelectedColor);

            }//foreach file
            enhmb.Close();
            Application.DoEvents(); //finish showing the dialog before starting the cpu-intensive operation
            Cursor.Current = Cursors.Default;
            //create result file
            string result = "";
            result = "Files using color result\r\n";
            result += this.listView_MainFileList.SelectedItems.Count.ToString() + " Files found, using the color [" + this.uC_EditorPalette1.PaletteSelectedColor.ToString() + "]\r\n\r\n";
            for (int i = 0; i < this.listView_MainFileList.SelectedItems.Count; i++) result += this.listView_MainFileList.SelectedItems[i].Text + "\r\n";
            System.IO.File.WriteAllText(defaultPath + "#ColorUsage.txt", result);
            System.Diagnostics.Process.Start(defaultPath + "#ColorUsage.txt");
        }

        private void ConvertPalette()
        {
            string schemefile = this.textBox_PaletteConversionScheme.Text;
            if (System.IO.File.Exists(schemefile))
            {
                List<KeyValuePair<byte, byte>> conversions = LoadCSchemeFile(schemefile);
                if (conversions.Count > 0)
                {
                    int filescount = this.listView_MainFileList.SelectedItems.Count;

                    if (filescount > 1)
                    {
                        enhmb = new EnhancedMessageBox();
                        enhmb.StartPosition = FormStartPosition.Manual;
                        enhmb.Location = new Point(this.Location.X, this.Location.Y);
                        enhmb.SetText("Processing files.\nThis may take a few seconds.");
                        enhmb.SetCaption("Processing files.");
                        enhmb.ProgressBarVisible(true);
                        enhmb.button_OK_text("Abort");
                        enhmb.Show();


                        Cursor.Current = Cursors.WaitCursor;
                        Application.DoEvents(); //finish showing the dialog before starting the cpu-intensive operation

                        enhmb.ProgressBarSize(filescount);
                    }
                    int pb = 0;

                    paletteUsage = new long[256];

                    foreach (ListViewItem item in this.listView_MainFileList.SelectedItems)
                    {
                        pb++;
                        if (filescount > 1)
                        {
                            enhmb.SetText("Processing files.\nThis may take a few seconds.\n\nFile: " + item.Text);
                            enhmb.ProgressBarValue((int)pb);
                            Application.DoEvents();
                            //accept user input and when the messagebox is closed, stop working
                            if (enhmb.Visible == false)
                                break;
                        }

                        //CTaMPer tmpfile = new CTaMPer();
                        //tmpfile.LoadTMP(defaultPath + item.Text);
                        //tmpfile.ReplaceColors(conversions);
                        //tmpfile.SaveTMP();
                    }//foreach file
                    if (filescount > 1)
                    {
                        enhmb.Close();
                        Application.DoEvents(); //finish showing the dialog before starting the cpu-intensive operation
                        Cursor.Current = Cursors.Default;
                    }
                }
                else
                {
                    MessageBox.Show("Empty or no [Data] section found in color scheme file.\n\nFile: " + schemefile, "Data not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else MessageBox.Show("Color scheme file not found.\n\nFile: " + this.textBox_PaletteConversionScheme.Text, "File not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private List<KeyValuePair<byte, byte>> LoadCSchemeFile(string file)
        {
            List<KeyValuePair<byte, byte>> conversions = new List<KeyValuePair<byte, byte>>();
            if (System.IO.File.Exists(file))
            {
                using (System.IO.StreamReader r = new System.IO.StreamReader(file, System.Text.Encoding.Default))
                {
                    string line;
                    bool datasectionfound = false;
                    while ((line = r.ReadLine()) != null)
                    {
                        if ((line.Contains('['))
                           || (line.Contains(']'))
                           || (!line.Contains('='))) datasectionfound = false;

                        if (datasectionfound)
                        {
                            
                            byte oldcolor = 0;
                            byte newcolor = 0;
                            if (line.Contains('='))
                            {

                                byte.TryParse(line.Split('=')[0], out oldcolor);
                                byte.TryParse(line.Split('=')[1], out newcolor);
                                conversions.Add(new KeyValuePair<byte, byte>(oldcolor, newcolor));
                            }
                        }
                        if (line.ToUpper().Contains("[DATA]"))
                        {
                            datasectionfound = true;
                        }
                    }
                }//using
            }
            return conversions;
        }//LoadCSchemeFile
    }
}
