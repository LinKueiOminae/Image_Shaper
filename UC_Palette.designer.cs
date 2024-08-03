namespace ImageShaper
{
    partial class UC_Palette
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.PaletteColorBox = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.comboBox_ColorConversionMethod = new System.Windows.Forms.ComboBox();
            this.label_ColorConversionMethod = new System.Windows.Forms.Label();
            this.tableLayoutPanel_3PaletteNameNew = new System.Windows.Forms.TableLayoutPanel();
            this.comboBox_Palette = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.button_NewCopy = new System.Windows.Forms.Button();
            this.button_Clone = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.PaletteColorBox)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel_3PaletteNameNew.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // PaletteColorBox
            // 
            this.PaletteColorBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PaletteColorBox.Location = new System.Drawing.Point(0, 50);
            this.PaletteColorBox.Margin = new System.Windows.Forms.Padding(0);
            this.PaletteColorBox.Name = "PaletteColorBox";
            this.PaletteColorBox.Size = new System.Drawing.Size(150, 125);
            this.PaletteColorBox.TabIndex = 0;
            this.PaletteColorBox.TabStop = false;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.PaletteColorBox, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel_3PaletteNameNew, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel4, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(150, 200);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.comboBox_ColorConversionMethod, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.label_ColorConversionMethod, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 175);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(150, 25);
            this.tableLayoutPanel2.TabIndex = 2;
            // 
            // comboBox_ColorConversionMethod
            // 
            this.comboBox_ColorConversionMethod.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBox_ColorConversionMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_ColorConversionMethod.FormattingEnabled = true;
            this.comboBox_ColorConversionMethod.Location = new System.Drawing.Point(100, 2);
            this.comboBox_ColorConversionMethod.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.comboBox_ColorConversionMethod.Name = "comboBox_ColorConversionMethod";
            this.comboBox_ColorConversionMethod.Size = new System.Drawing.Size(50, 21);
            this.comboBox_ColorConversionMethod.TabIndex = 0;
            this.comboBox_ColorConversionMethod.SelectedIndexChanged += new System.EventHandler(this.comboBox_ColorConversionMethod_SelectedIndexChanged);
            // 
            // label_ColorConversionMethod
            // 
            this.label_ColorConversionMethod.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label_ColorConversionMethod.AutoSize = true;
            this.label_ColorConversionMethod.Location = new System.Drawing.Point(3, 6);
            this.label_ColorConversionMethod.Name = "label_ColorConversionMethod";
            this.label_ColorConversionMethod.Size = new System.Drawing.Size(87, 13);
            this.label_ColorConversionMethod.TabIndex = 1;
            this.label_ColorConversionMethod.Text = "Color Conversion";
            // 
            // tableLayoutPanel_3PaletteNameNew
            // 
            this.tableLayoutPanel_3PaletteNameNew.ColumnCount = 1;
            this.tableLayoutPanel_3PaletteNameNew.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel_3PaletteNameNew.Controls.Add(this.comboBox_Palette, 0, 0);
            this.tableLayoutPanel_3PaletteNameNew.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel_3PaletteNameNew.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel_3PaletteNameNew.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel_3PaletteNameNew.Name = "tableLayoutPanel_3PaletteNameNew";
            this.tableLayoutPanel_3PaletteNameNew.RowCount = 1;
            this.tableLayoutPanel_3PaletteNameNew.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel_3PaletteNameNew.Size = new System.Drawing.Size(150, 25);
            this.tableLayoutPanel_3PaletteNameNew.TabIndex = 3;
            // 
            // comboBox_Palette
            // 
            this.comboBox_Palette.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBox_Palette.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_Palette.FormattingEnabled = true;
            this.comboBox_Palette.Location = new System.Drawing.Point(3, 3);
            this.comboBox_Palette.Name = "comboBox_Palette";
            this.comboBox_Palette.Size = new System.Drawing.Size(144, 21);
            this.comboBox_Palette.TabIndex = 3;
            this.comboBox_Palette.SelectedIndexChanged += new System.EventHandler(this.comboBox_Palette_SelectedIndexChanged);
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 3;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tableLayoutPanel4.Controls.Add(this.button_NewCopy, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.button_Clone, 2, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 25);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(150, 25);
            this.tableLayoutPanel4.TabIndex = 4;
            // 
            // button_NewCopy
            // 
            this.button_NewCopy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_NewCopy.Location = new System.Drawing.Point(0, 0);
            this.button_NewCopy.Margin = new System.Windows.Forms.Padding(0);
            this.button_NewCopy.Name = "button_NewCopy";
            this.button_NewCopy.Size = new System.Drawing.Size(75, 25);
            this.button_NewCopy.TabIndex = 2;
            this.button_NewCopy.Text = "New Copy";
            this.button_NewCopy.UseVisualStyleBackColor = true;
            this.button_NewCopy.Click += new System.EventHandler(this.button_NewCopy_Click);
            // 
            // button_Clone
            // 
            this.button_Clone.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_Clone.Location = new System.Drawing.Point(75, 0);
            this.button_Clone.Margin = new System.Windows.Forms.Padding(0);
            this.button_Clone.Name = "button_Clone";
            this.button_Clone.Size = new System.Drawing.Size(75, 25);
            this.button_Clone.TabIndex = 0;
            this.button_Clone.Text = "Clone";
            this.button_Clone.UseVisualStyleBackColor = true;
            this.button_Clone.Click += new System.EventHandler(this.button_Clone_Click);
            // 
            // UC_Palette
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "UC_Palette";
            this.Size = new System.Drawing.Size(150, 200);
            ((System.ComponentModel.ISupportInitialize)(this.PaletteColorBox)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel_3PaletteNameNew.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox PaletteColorBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.ComboBox comboBox_ColorConversionMethod;
        private System.Windows.Forms.Label label_ColorConversionMethod;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel_3PaletteNameNew;
        private System.Windows.Forms.Button button_NewCopy;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.ComboBox comboBox_Palette;
        private System.Windows.Forms.Button button_Clone;
    }
}
