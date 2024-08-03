namespace ImageShaper
{
    partial class Form_LoadAndSplitImage
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2_OK = new System.Windows.Forms.TableLayoutPanel();
            this.button_OK = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.tableLayoutPanel2_Settings = new System.Windows.Forms.TableLayoutPanel();
            this.label_ColumnsCount = new System.Windows.Forms.Label();
            this.label_Width = new System.Windows.Forms.Label();
            this.label_Height = new System.Windows.Forms.Label();
            this.numericUpDown_Width = new NumericUpDownEx();
            this.numericUpDown_Height = new NumericUpDownEx();
            this.radioButton_RowWise = new System.Windows.Forms.RadioButton();
            this.radioButton_ColWise = new System.Windows.Forms.RadioButton();
            this.label_Columns = new System.Windows.Forms.Label();
            this.label_Rows = new System.Windows.Forms.Label();
            this.label_RowsCount = new System.Windows.Forms.Label();
            this.tableLayoutPanel3_Preview = new System.Windows.Forms.TableLayoutPanel();
            this.uC_ImageCanvas1_Original = new ImageShaper.UC_ImageCanvas();
            this.uC_ImageCanvas2_Preview = new ImageShaper.UC_ImageCanvas();
            this.tableLayoutPanel4_PreviewFrame = new System.Windows.Forms.TableLayoutPanel();
            this.numericUpDown_PreviewFrame = new NumericUpDownEx();
            this.label_Frame = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2_OK.SuspendLayout();
            this.tableLayoutPanel2_Settings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Width)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Height)).BeginInit();
            this.tableLayoutPanel3_Preview.SuspendLayout();
            this.tableLayoutPanel4_PreviewFrame.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_PreviewFrame)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2_OK, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2_Settings, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3_Preview, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(482, 333);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2_OK
            // 
            this.tableLayoutPanel2_OK.ColumnCount = 3;
            this.tableLayoutPanel2_OK.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel2_OK.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2_OK.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel2_OK.Controls.Add(this.button_OK, 2, 0);
            this.tableLayoutPanel2_OK.Controls.Add(this.button1, 0, 0);
            this.tableLayoutPanel2_OK.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2_OK.Location = new System.Drawing.Point(0, 303);
            this.tableLayoutPanel2_OK.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2_OK.Name = "tableLayoutPanel2_OK";
            this.tableLayoutPanel2_OK.RowCount = 1;
            this.tableLayoutPanel2_OK.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2_OK.Size = new System.Drawing.Size(482, 30);
            this.tableLayoutPanel2_OK.TabIndex = 0;
            // 
            // button_OK
            // 
            this.button_OK.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_OK.Location = new System.Drawing.Point(382, 0);
            this.button_OK.Margin = new System.Windows.Forms.Padding(0);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new System.Drawing.Size(100, 30);
            this.button_OK.TabIndex = 0;
            this.button_OK.Text = "OK";
            this.button_OK.UseVisualStyleBackColor = true;
            this.button_OK.Click += new System.EventHandler(this.button_OK_Click);
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button1.Location = new System.Drawing.Point(0, 0);
            this.button1.Margin = new System.Windows.Forms.Padding(0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 30);
            this.button1.TabIndex = 1;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tableLayoutPanel2_Settings
            // 
            this.tableLayoutPanel2_Settings.ColumnCount = 6;
            this.tableLayoutPanel2_Settings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2_Settings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel2_Settings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel2_Settings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel2_Settings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 130F));
            this.tableLayoutPanel2_Settings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel2_Settings.Controls.Add(this.label_ColumnsCount, 3, 0);
            this.tableLayoutPanel2_Settings.Controls.Add(this.label_Width, 1, 0);
            this.tableLayoutPanel2_Settings.Controls.Add(this.label_Height, 1, 1);
            this.tableLayoutPanel2_Settings.Controls.Add(this.numericUpDown_Width, 2, 0);
            this.tableLayoutPanel2_Settings.Controls.Add(this.numericUpDown_Height, 2, 1);
            this.tableLayoutPanel2_Settings.Controls.Add(this.radioButton_RowWise, 4, 0);
            this.tableLayoutPanel2_Settings.Controls.Add(this.radioButton_ColWise, 4, 1);
            this.tableLayoutPanel2_Settings.Controls.Add(this.label_Columns, 0, 0);
            this.tableLayoutPanel2_Settings.Controls.Add(this.label_Rows, 0, 1);
            this.tableLayoutPanel2_Settings.Controls.Add(this.label_RowsCount, 3, 1);
            this.tableLayoutPanel2_Settings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2_Settings.Location = new System.Drawing.Point(0, 233);
            this.tableLayoutPanel2_Settings.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2_Settings.Name = "tableLayoutPanel2_Settings";
            this.tableLayoutPanel2_Settings.RowCount = 3;
            this.tableLayoutPanel2_Settings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel2_Settings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel2_Settings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2_Settings.Size = new System.Drawing.Size(482, 70);
            this.tableLayoutPanel2_Settings.TabIndex = 2;
            // 
            // label_ColumnsCount
            // 
            this.label_ColumnsCount.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label_ColumnsCount.AutoSize = true;
            this.label_ColumnsCount.Location = new System.Drawing.Point(255, 6);
            this.label_ColumnsCount.Name = "label_ColumnsCount";
            this.label_ColumnsCount.Size = new System.Drawing.Size(20, 13);
            this.label_ColumnsCount.TabIndex = 8;
            this.label_ColumnsCount.Text = "#1";
            // 
            // label_Width
            // 
            this.label_Width.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label_Width.AutoSize = true;
            this.label_Width.Location = new System.Drawing.Point(125, 6);
            this.label_Width.Name = "label_Width";
            this.label_Width.Size = new System.Drawing.Size(38, 13);
            this.label_Width.TabIndex = 1;
            this.label_Width.Text = "Width:";
            // 
            // label_Height
            // 
            this.label_Height.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label_Height.AutoSize = true;
            this.label_Height.Location = new System.Drawing.Point(125, 31);
            this.label_Height.Name = "label_Height";
            this.label_Height.Size = new System.Drawing.Size(41, 13);
            this.label_Height.TabIndex = 0;
            this.label_Height.Text = "Height:";
            // 
            // numericUpDown_Width
            // 
            this.numericUpDown_Width.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numericUpDown_Width.Location = new System.Drawing.Point(175, 3);
            this.numericUpDown_Width.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_Width.Name = "numericUpDown_Width";
            this.numericUpDown_Width.Size = new System.Drawing.Size(74, 20);
            this.numericUpDown_Width.TabIndex = 2;
            this.numericUpDown_Width.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_Width.ValueChanged += new System.EventHandler(this.numericUpDown_Width_ValueChanged);
            // 
            // numericUpDown_Height
            // 
            this.numericUpDown_Height.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numericUpDown_Height.Location = new System.Drawing.Point(175, 28);
            this.numericUpDown_Height.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_Height.Name = "numericUpDown_Height";
            this.numericUpDown_Height.Size = new System.Drawing.Size(74, 20);
            this.numericUpDown_Height.TabIndex = 3;
            this.numericUpDown_Height.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_Height.ValueChanged += new System.EventHandler(this.numericUpDown_Height_ValueChanged);
            // 
            // radioButton_RowWise
            // 
            this.radioButton_RowWise.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.radioButton_RowWise.AutoSize = true;
            this.radioButton_RowWise.Location = new System.Drawing.Point(305, 4);
            this.radioButton_RowWise.Name = "radioButton_RowWise";
            this.radioButton_RowWise.Size = new System.Drawing.Size(96, 17);
            this.radioButton_RowWise.TabIndex = 4;
            this.radioButton_RowWise.TabStop = true;
            this.radioButton_RowWise.Text = "Row after Row";
            this.radioButton_RowWise.UseVisualStyleBackColor = true;
            this.radioButton_RowWise.CheckedChanged += new System.EventHandler(this.radioButton_RowWise_CheckedChanged);
            // 
            // radioButton_ColWise
            // 
            this.radioButton_ColWise.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.radioButton_ColWise.AutoSize = true;
            this.radioButton_ColWise.Location = new System.Drawing.Point(305, 29);
            this.radioButton_ColWise.Name = "radioButton_ColWise";
            this.radioButton_ColWise.Size = new System.Drawing.Size(122, 17);
            this.radioButton_ColWise.TabIndex = 5;
            this.radioButton_ColWise.TabStop = true;
            this.radioButton_ColWise.Text = "Column after Column";
            this.radioButton_ColWise.UseVisualStyleBackColor = true;
            this.radioButton_ColWise.CheckedChanged += new System.EventHandler(this.radioButton_ColWise_CheckedChanged);
            // 
            // label_Columns
            // 
            this.label_Columns.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label_Columns.AutoSize = true;
            this.label_Columns.Location = new System.Drawing.Point(72, 6);
            this.label_Columns.Name = "label_Columns";
            this.label_Columns.Size = new System.Drawing.Size(47, 13);
            this.label_Columns.TabIndex = 6;
            this.label_Columns.Text = "Columns";
            // 
            // label_Rows
            // 
            this.label_Rows.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label_Rows.AutoSize = true;
            this.label_Rows.Location = new System.Drawing.Point(85, 31);
            this.label_Rows.Name = "label_Rows";
            this.label_Rows.Size = new System.Drawing.Size(34, 13);
            this.label_Rows.TabIndex = 7;
            this.label_Rows.Text = "Rows";
            // 
            // label_RowsCount
            // 
            this.label_RowsCount.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label_RowsCount.AutoSize = true;
            this.label_RowsCount.Location = new System.Drawing.Point(255, 31);
            this.label_RowsCount.Name = "label_RowsCount";
            this.label_RowsCount.Size = new System.Drawing.Size(20, 13);
            this.label_RowsCount.TabIndex = 9;
            this.label_RowsCount.Text = "#1";
            // 
            // tableLayoutPanel3_Preview
            // 
            this.tableLayoutPanel3_Preview.ColumnCount = 3;
            this.tableLayoutPanel3_Preview.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3_Preview.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3_Preview.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel3_Preview.Controls.Add(this.uC_ImageCanvas1_Original, 0, 0);
            this.tableLayoutPanel3_Preview.Controls.Add(this.uC_ImageCanvas2_Preview, 1, 0);
            this.tableLayoutPanel3_Preview.Controls.Add(this.tableLayoutPanel4_PreviewFrame, 2, 0);
            this.tableLayoutPanel3_Preview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3_Preview.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel3_Preview.Name = "tableLayoutPanel3_Preview";
            this.tableLayoutPanel3_Preview.RowCount = 1;
            this.tableLayoutPanel3_Preview.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3_Preview.Size = new System.Drawing.Size(476, 227);
            this.tableLayoutPanel3_Preview.TabIndex = 3;
            // 
            // uC_ImageCanvas1_Original
            // 
            this.uC_ImageCanvas1_Original.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uC_ImageCanvas1_Original.Location = new System.Drawing.Point(3, 3);
            this.uC_ImageCanvas1_Original.MinimumSize = new System.Drawing.Size(200, 200);
            this.uC_ImageCanvas1_Original.Name = "uC_ImageCanvas1_Original";
            this.uC_ImageCanvas1_Original.Size = new System.Drawing.Size(200, 221);
            this.uC_ImageCanvas1_Original.TabIndex = 1;
            this.uC_ImageCanvas1_Original.ZoomLevel = 1;
            // 
            // uC_ImageCanvas2_Preview
            // 
            this.uC_ImageCanvas2_Preview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uC_ImageCanvas2_Preview.Location = new System.Drawing.Point(206, 3);
            this.uC_ImageCanvas2_Preview.MinimumSize = new System.Drawing.Size(200, 200);
            this.uC_ImageCanvas2_Preview.Name = "uC_ImageCanvas2_Preview";
            this.uC_ImageCanvas2_Preview.Size = new System.Drawing.Size(200, 221);
            this.uC_ImageCanvas2_Preview.TabIndex = 2;
            this.uC_ImageCanvas2_Preview.ZoomLevel = 1;
            // 
            // tableLayoutPanel4_PreviewFrame
            // 
            this.tableLayoutPanel4_PreviewFrame.ColumnCount = 1;
            this.tableLayoutPanel4_PreviewFrame.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4_PreviewFrame.Controls.Add(this.numericUpDown_PreviewFrame, 0, 1);
            this.tableLayoutPanel4_PreviewFrame.Controls.Add(this.label_Frame, 0, 0);
            this.tableLayoutPanel4_PreviewFrame.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4_PreviewFrame.Location = new System.Drawing.Point(409, 3);
            this.tableLayoutPanel4_PreviewFrame.Name = "tableLayoutPanel4_PreviewFrame";
            this.tableLayoutPanel4_PreviewFrame.RowCount = 3;
            this.tableLayoutPanel4_PreviewFrame.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4_PreviewFrame.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel4_PreviewFrame.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4_PreviewFrame.Size = new System.Drawing.Size(64, 221);
            this.tableLayoutPanel4_PreviewFrame.TabIndex = 3;
            // 
            // numericUpDown_PreviewFrame
            // 
            this.numericUpDown_PreviewFrame.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numericUpDown_PreviewFrame.Location = new System.Drawing.Point(3, 98);
            this.numericUpDown_PreviewFrame.Name = "numericUpDown_PreviewFrame";
            this.numericUpDown_PreviewFrame.Size = new System.Drawing.Size(58, 20);
            this.numericUpDown_PreviewFrame.TabIndex = 0;
            this.numericUpDown_PreviewFrame.ValueChanged += new System.EventHandler(this.numericUpDown_PreviewFrame_ValueChanged);
            // 
            // label_Frame
            // 
            this.label_Frame.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label_Frame.AutoSize = true;
            this.label_Frame.Location = new System.Drawing.Point(14, 82);
            this.label_Frame.Name = "label_Frame";
            this.label_Frame.Size = new System.Drawing.Size(36, 13);
            this.label_Frame.TabIndex = 1;
            this.label_Frame.Text = "Frame";
            // 
            // Form_LoadAndSplitImage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(482, 333);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MinimumSize = new System.Drawing.Size(490, 360);
            this.Name = "Form_LoadAndSplitImage";
            this.Text = "Form_LoadAndSplitImage";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2_OK.ResumeLayout(false);
            this.tableLayoutPanel2_Settings.ResumeLayout(false);
            this.tableLayoutPanel2_Settings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Width)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Height)).EndInit();
            this.tableLayoutPanel3_Preview.ResumeLayout(false);
            this.tableLayoutPanel4_PreviewFrame.ResumeLayout(false);
            this.tableLayoutPanel4_PreviewFrame.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_PreviewFrame)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2_OK;
        private System.Windows.Forms.Button button_OK;
        private System.Windows.Forms.Button button1;
        private UC_ImageCanvas uC_ImageCanvas1_Original;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2_Settings;
        private System.Windows.Forms.Label label_Height;
        private System.Windows.Forms.Label label_Width;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3_Preview;
        private UC_ImageCanvas uC_ImageCanvas2_Preview;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4_PreviewFrame;
        private NumericUpDownEx numericUpDown_PreviewFrame;
        private NumericUpDownEx numericUpDown_Width;
        private NumericUpDownEx numericUpDown_Height;
        private System.Windows.Forms.RadioButton radioButton_RowWise;
        private System.Windows.Forms.RadioButton radioButton_ColWise;
        private System.Windows.Forms.Label label_ColumnsCount;
        private System.Windows.Forms.Label label_Columns;
        private System.Windows.Forms.Label label_Rows;
        private System.Windows.Forms.Label label_RowsCount;
        private System.Windows.Forms.Label label_Frame;
    }
}