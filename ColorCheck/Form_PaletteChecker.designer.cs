namespace ColorCheck
{
    partial class Form_PaletteChecker
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_PaletteChecker));
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.listView_MainFileList = new System.Windows.Forms.ListView();
            this.listView_MainFileList_Column_Name = new System.Windows.Forms.ColumnHeader();
            this.button_SelectFiles = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.uC_EditorPalette1 = new TMPEditorNamespace.UC_EditorPalette();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button_Start = new System.Windows.Forms.Button();
            this.groupBox_Settings = new System.Windows.Forms.GroupBox();
            this.radioButton_ConvertPalette = new System.Windows.Forms.RadioButton();
            this.radioButton_AnalyzePaletteUsage = new System.Windows.Forms.RadioButton();
            this.radioButton_ListFilesUsingColor = new System.Windows.Forms.RadioButton();
            this.button_PaletteConversionScheme = new System.Windows.Forms.Button();
            this.textBox_PaletteConversionScheme = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox_Settings.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 1;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Controls.Add(this.listView_MainFileList, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.button_SelectFiles, 0, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 2;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(156, 473);
            this.tableLayoutPanel4.TabIndex = 5;
            // 
            // listView_MainFileList
            // 
            this.listView_MainFileList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.listView_MainFileList_Column_Name});
            this.listView_MainFileList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView_MainFileList.FullRowSelect = true;
            this.listView_MainFileList.HideSelection = false;
            this.listView_MainFileList.Location = new System.Drawing.Point(3, 33);
            this.listView_MainFileList.Name = "listView_MainFileList";
            this.listView_MainFileList.Size = new System.Drawing.Size(150, 437);
            this.listView_MainFileList.TabIndex = 2;
            this.listView_MainFileList.UseCompatibleStateImageBehavior = false;
            this.listView_MainFileList.View = System.Windows.Forms.View.Details;
            // 
            // listView_MainFileList_Column_Name
            // 
            this.listView_MainFileList_Column_Name.Text = "Name";
            // 
            // button_SelectFiles
            // 
            this.button_SelectFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_SelectFiles.Location = new System.Drawing.Point(3, 3);
            this.button_SelectFiles.Name = "button_SelectFiles";
            this.button_SelectFiles.Size = new System.Drawing.Size(150, 24);
            this.button_SelectFiles.TabIndex = 1;
            this.button_SelectFiles.Text = "Select SHPs";
            this.button_SelectFiles.UseVisualStyleBackColor = true;
            this.button_SelectFiles.Click += new System.EventHandler(this.button_SelectSHPs_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel4, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(392, 473);
            this.tableLayoutPanel1.TabIndex = 6;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.uC_EditorPalette1, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(156, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(236, 473);
            this.tableLayoutPanel2.TabIndex = 6;
            // 
            // uC_EditorPalette1
            // 
            this.uC_EditorPalette1.AllowColorSelection = true;
            this.uC_EditorPalette1.ContextMenuEnabled = true;
            this.uC_EditorPalette1.ContextMenuLoadEnabled = true;
            this.uC_EditorPalette1.ContextMenuSaveEnabled = true;
            this.uC_EditorPalette1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uC_EditorPalette1.Location = new System.Drawing.Point(3, 153);
            this.uC_EditorPalette1.Name = "uC_EditorPalette1";
            this.uC_EditorPalette1.PaletteMode = 0;
            this.uC_EditorPalette1.PaletteSelectedColor = 0;
            this.uC_EditorPalette1.PaletteUsage = null;
            this.uC_EditorPalette1.ShowPaletteModes = false;
            this.uC_EditorPalette1.Size = new System.Drawing.Size(230, 317);
            this.uC_EditorPalette1.TabIndex = 0;
            this.uC_EditorPalette1.UseFixedPalette = null;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button_Start);
            this.panel1.Controls.Add(this.groupBox_Settings);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(236, 150);
            this.panel1.TabIndex = 1;
            // 
            // button_Start
            // 
            this.button_Start.Location = new System.Drawing.Point(10, 5);
            this.button_Start.Name = "button_Start";
            this.button_Start.Size = new System.Drawing.Size(75, 30);
            this.button_Start.TabIndex = 3;
            this.button_Start.Text = "Start";
            this.button_Start.UseVisualStyleBackColor = true;
            this.button_Start.Click += new System.EventHandler(this.button_Start_Click);
            // 
            // groupBox_Settings
            // 
            this.groupBox_Settings.Controls.Add(this.radioButton_ConvertPalette);
            this.groupBox_Settings.Controls.Add(this.radioButton_AnalyzePaletteUsage);
            this.groupBox_Settings.Controls.Add(this.radioButton_ListFilesUsingColor);
            this.groupBox_Settings.Controls.Add(this.button_PaletteConversionScheme);
            this.groupBox_Settings.Controls.Add(this.textBox_PaletteConversionScheme);
            this.groupBox_Settings.Location = new System.Drawing.Point(10, 40);
            this.groupBox_Settings.Name = "groupBox_Settings";
            this.groupBox_Settings.Size = new System.Drawing.Size(220, 100);
            this.groupBox_Settings.TabIndex = 3;
            this.groupBox_Settings.TabStop = false;
            this.groupBox_Settings.Text = "Settings";
            // 
            // radioButton_ConvertPalette
            // 
            this.radioButton_ConvertPalette.AutoSize = true;
            this.radioButton_ConvertPalette.Enabled = false;
            this.radioButton_ConvertPalette.Location = new System.Drawing.Point(10, 55);
            this.radioButton_ConvertPalette.Name = "radioButton_ConvertPalette";
            this.radioButton_ConvertPalette.Size = new System.Drawing.Size(98, 17);
            this.radioButton_ConvertPalette.TabIndex = 6;
            this.radioButton_ConvertPalette.TabStop = true;
            this.radioButton_ConvertPalette.Text = "Convert Palette";
            this.radioButton_ConvertPalette.UseVisualStyleBackColor = true;
            // 
            // radioButton_AnalyzePaletteUsage
            // 
            this.radioButton_AnalyzePaletteUsage.AutoSize = true;
            this.radioButton_AnalyzePaletteUsage.Location = new System.Drawing.Point(10, 15);
            this.radioButton_AnalyzePaletteUsage.Name = "radioButton_AnalyzePaletteUsage";
            this.radioButton_AnalyzePaletteUsage.Size = new System.Drawing.Size(132, 17);
            this.radioButton_AnalyzePaletteUsage.TabIndex = 4;
            this.radioButton_AnalyzePaletteUsage.TabStop = true;
            this.radioButton_AnalyzePaletteUsage.Text = "Analyze Palette Usage";
            this.radioButton_AnalyzePaletteUsage.UseVisualStyleBackColor = true;
            // 
            // radioButton_ListFilesUsingColor
            // 
            this.radioButton_ListFilesUsingColor.AutoSize = true;
            this.radioButton_ListFilesUsingColor.Location = new System.Drawing.Point(10, 35);
            this.radioButton_ListFilesUsingColor.Name = "radioButton_ListFilesUsingColor";
            this.radioButton_ListFilesUsingColor.Size = new System.Drawing.Size(169, 17);
            this.radioButton_ListFilesUsingColor.TabIndex = 5;
            this.radioButton_ListFilesUsingColor.TabStop = true;
            this.radioButton_ListFilesUsingColor.Text = "List files using a selected Color";
            this.radioButton_ListFilesUsingColor.UseVisualStyleBackColor = true;
            // 
            // button_PaletteConversionScheme
            // 
            this.button_PaletteConversionScheme.Enabled = false;
            this.button_PaletteConversionScheme.Location = new System.Drawing.Point(10, 74);
            this.button_PaletteConversionScheme.Name = "button_PaletteConversionScheme";
            this.button_PaletteConversionScheme.Size = new System.Drawing.Size(50, 20);
            this.button_PaletteConversionScheme.TabIndex = 7;
            this.button_PaletteConversionScheme.Text = "select";
            this.button_PaletteConversionScheme.UseVisualStyleBackColor = true;
            this.button_PaletteConversionScheme.Click += new System.EventHandler(this.button_PaletteConversionScheme_Click);
            // 
            // textBox_PaletteConversionScheme
            // 
            this.textBox_PaletteConversionScheme.Enabled = false;
            this.textBox_PaletteConversionScheme.Location = new System.Drawing.Point(70, 74);
            this.textBox_PaletteConversionScheme.Name = "textBox_PaletteConversionScheme";
            this.textBox_PaletteConversionScheme.Size = new System.Drawing.Size(145, 20);
            this.textBox_PaletteConversionScheme.TabIndex = 8;
            this.textBox_PaletteConversionScheme.Text = "Palette conversion scheme file";
            // 
            // Form_PaletteChecker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(392, 473);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(400, 500);
            this.Name = "Form_PaletteChecker";
            this.Text = "Form_PaletteChecker";
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.groupBox_Settings.ResumeLayout(false);
            this.groupBox_Settings.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.ListView listView_MainFileList;
        private System.Windows.Forms.ColumnHeader listView_MainFileList_Column_Name;
        private System.Windows.Forms.Button button_SelectFiles;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private TMPEditorNamespace.UC_EditorPalette uC_EditorPalette1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button_Start;
        private System.Windows.Forms.GroupBox groupBox_Settings;
        private System.Windows.Forms.Button button_PaletteConversionScheme;
        private System.Windows.Forms.TextBox textBox_PaletteConversionScheme;
        private System.Windows.Forms.RadioButton radioButton_ListFilesUsingColor;
        private System.Windows.Forms.RadioButton radioButton_ConvertPalette;
        private System.Windows.Forms.RadioButton radioButton_AnalyzePaletteUsage;
    }
}