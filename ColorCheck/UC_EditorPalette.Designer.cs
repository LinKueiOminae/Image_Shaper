namespace TMPEditorNamespace
{
    partial class UC_EditorPalette
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
            this.radioButton_Color = new System.Windows.Forms.RadioButton();
            this.radioButton_ZData = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.PaletteColorBox)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // PaletteColorBox
            // 
            this.PaletteColorBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PaletteColorBox.Location = new System.Drawing.Point(0, 20);
            this.PaletteColorBox.Margin = new System.Windows.Forms.Padding(0);
            this.PaletteColorBox.Name = "PaletteColorBox";
            this.PaletteColorBox.Size = new System.Drawing.Size(150, 180);
            this.PaletteColorBox.TabIndex = 0;
            this.PaletteColorBox.TabStop = false;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.PaletteColorBox, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(150, 200);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.radioButton_Color, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.radioButton_ZData, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(150, 20);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // radioButton_Color
            // 
            this.radioButton_Color.Appearance = System.Windows.Forms.Appearance.Button;
            this.radioButton_Color.AutoSize = true;
            this.radioButton_Color.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radioButton_Color.Location = new System.Drawing.Point(0, 0);
            this.radioButton_Color.Margin = new System.Windows.Forms.Padding(0);
            this.radioButton_Color.Name = "radioButton_Color";
            this.radioButton_Color.Size = new System.Drawing.Size(75, 20);
            this.radioButton_Color.TabIndex = 0;
            this.radioButton_Color.TabStop = true;
            this.radioButton_Color.Text = "Color";
            this.radioButton_Color.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.radioButton_Color.UseVisualStyleBackColor = true;
            // 
            // radioButton_ZData
            // 
            this.radioButton_ZData.Appearance = System.Windows.Forms.Appearance.Button;
            this.radioButton_ZData.AutoSize = true;
            this.radioButton_ZData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radioButton_ZData.Location = new System.Drawing.Point(75, 0);
            this.radioButton_ZData.Margin = new System.Windows.Forms.Padding(0);
            this.radioButton_ZData.Name = "radioButton_ZData";
            this.radioButton_ZData.Size = new System.Drawing.Size(75, 20);
            this.radioButton_ZData.TabIndex = 1;
            this.radioButton_ZData.TabStop = true;
            this.radioButton_ZData.Text = "Z-Data";
            this.radioButton_ZData.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.radioButton_ZData.UseVisualStyleBackColor = true;
            // 
            // UC_EditorPalette
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "UC_EditorPalette";
            this.Size = new System.Drawing.Size(150, 200);
            ((System.ComponentModel.ISupportInitialize)(this.PaletteColorBox)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox PaletteColorBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.RadioButton radioButton_Color;
        private System.Windows.Forms.RadioButton radioButton_ZData;
    }
}
