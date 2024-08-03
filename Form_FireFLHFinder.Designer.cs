namespace ImageShaper
{
    partial class Form_FireFLHFinder
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_FireFLHFinder));
            this.tableLayoutPanel2_Left = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox_Mode = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel4_GroupMode = new System.Windows.Forms.TableLayoutPanel();
            this.radioButton_FireFLH = new System.Windows.Forms.RadioButton();
            this.radioButton_FirePixelOffset = new System.Windows.Forms.RadioButton();
            this.radioButton_DamageFireOffset = new System.Windows.Forms.RadioButton();
            this.radioButton_MuzzleFlash = new System.Windows.Forms.RadioButton();
            this.checkBox_IsLaser = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel3_Canvas = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox_Left = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel3_GroupLeft = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel4_FrameLeft = new System.Windows.Forms.TableLayoutPanel();
            this.numericUpDown_FrameLeft = new NumericUpDownEx();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDown_Zoom = new NumericUpDownEx();
            this.label_FrameLeft = new System.Windows.Forms.Label();
            this.uC_ImageCanvas_Left = new ImageShaper.UC_ImageCanvas();
            this.groupBox_Right = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel3_GroupRight = new System.Windows.Forms.TableLayoutPanel();
            this.uC_ImageCanvas_Right = new ImageShaper.UC_ImageCanvas();
            this.tableLayoutPanel4_FrameRight = new System.Windows.Forms.TableLayoutPanel();
            this.numericUpDown_FrameRight = new NumericUpDownEx();
            this.label_FrameRight = new System.Windows.Forms.Label();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.tableLayoutPanel1_Main = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2_Right = new System.Windows.Forms.TableLayoutPanel();
            this.button_Close = new System.Windows.Forms.Button();
            this.button_LoadSHP = new System.Windows.Forms.Button();
            this.button_LoadPalette = new System.Windows.Forms.Button();
            this.tableLayoutPanel2_Left.SuspendLayout();
            this.groupBox_Mode.SuspendLayout();
            this.tableLayoutPanel4_GroupMode.SuspendLayout();
            this.tableLayoutPanel3_Canvas.SuspendLayout();
            this.groupBox_Left.SuspendLayout();
            this.tableLayoutPanel3_GroupLeft.SuspendLayout();
            this.tableLayoutPanel4_FrameLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_FrameLeft)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Zoom)).BeginInit();
            this.groupBox_Right.SuspendLayout();
            this.tableLayoutPanel3_GroupRight.SuspendLayout();
            this.tableLayoutPanel4_FrameRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_FrameRight)).BeginInit();
            this.tableLayoutPanel1_Main.SuspendLayout();
            this.tableLayoutPanel2_Right.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel2_Left
            // 
            this.tableLayoutPanel2_Left.ColumnCount = 1;
            this.tableLayoutPanel2_Left.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2_Left.Controls.Add(this.groupBox_Mode, 0, 1);
            this.tableLayoutPanel2_Left.Controls.Add(this.tableLayoutPanel3_Canvas, 0, 0);
            this.tableLayoutPanel2_Left.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2_Left.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2_Left.Name = "tableLayoutPanel2_Left";
            this.tableLayoutPanel2_Left.RowCount = 2;
            this.tableLayoutPanel2_Left.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2_Left.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 110F));
            this.tableLayoutPanel2_Left.Size = new System.Drawing.Size(453, 377);
            this.tableLayoutPanel2_Left.TabIndex = 0;
            // 
            // groupBox_Mode
            // 
            this.groupBox_Mode.Controls.Add(this.tableLayoutPanel4_GroupMode);
            this.groupBox_Mode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_Mode.Location = new System.Drawing.Point(3, 270);
            this.groupBox_Mode.Name = "groupBox_Mode";
            this.groupBox_Mode.Size = new System.Drawing.Size(447, 104);
            this.groupBox_Mode.TabIndex = 2;
            this.groupBox_Mode.TabStop = false;
            this.groupBox_Mode.Text = "Mode";
            // 
            // tableLayoutPanel4_GroupMode
            // 
            this.tableLayoutPanel4_GroupMode.ColumnCount = 2;
            this.tableLayoutPanel4_GroupMode.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4_GroupMode.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4_GroupMode.Controls.Add(this.radioButton_FireFLH, 1, 0);
            this.tableLayoutPanel4_GroupMode.Controls.Add(this.radioButton_FirePixelOffset, 0, 0);
            this.tableLayoutPanel4_GroupMode.Controls.Add(this.radioButton_DamageFireOffset, 0, 1);
            this.tableLayoutPanel4_GroupMode.Controls.Add(this.radioButton_MuzzleFlash, 0, 2);
            this.tableLayoutPanel4_GroupMode.Controls.Add(this.checkBox_IsLaser, 1, 2);
            this.tableLayoutPanel4_GroupMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4_GroupMode.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel4_GroupMode.Name = "tableLayoutPanel4_GroupMode";
            this.tableLayoutPanel4_GroupMode.RowCount = 4;
            this.tableLayoutPanel4_GroupMode.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel4_GroupMode.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel4_GroupMode.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel4_GroupMode.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4_GroupMode.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel4_GroupMode.Size = new System.Drawing.Size(441, 85);
            this.tableLayoutPanel4_GroupMode.TabIndex = 0;
            // 
            // radioButton_FireFLH
            // 
            this.radioButton_FireFLH.AutoSize = true;
            this.radioButton_FireFLH.Checked = true;
            this.radioButton_FireFLH.Location = new System.Drawing.Point(223, 3);
            this.radioButton_FireFLH.Name = "radioButton_FireFLH";
            this.radioButton_FireFLH.Size = new System.Drawing.Size(62, 17);
            this.radioButton_FireFLH.TabIndex = 1;
            this.radioButton_FireFLH.TabStop = true;
            this.radioButton_FireFLH.Text = "FireFLH";
            this.radioButton_FireFLH.UseVisualStyleBackColor = true;
            // 
            // radioButton_FirePixelOffset
            // 
            this.radioButton_FirePixelOffset.AutoSize = true;
            this.radioButton_FirePixelOffset.Location = new System.Drawing.Point(3, 3);
            this.radioButton_FirePixelOffset.Name = "radioButton_FirePixelOffset";
            this.radioButton_FirePixelOffset.Size = new System.Drawing.Size(126, 17);
            this.radioButton_FirePixelOffset.TabIndex = 0;
            this.radioButton_FirePixelOffset.Text = "PrimaryFirePixelOffset";
            this.radioButton_FirePixelOffset.UseVisualStyleBackColor = true;
            // 
            // radioButton_DamageFireOffset
            // 
            this.radioButton_DamageFireOffset.AutoSize = true;
            this.radioButton_DamageFireOffset.Location = new System.Drawing.Point(3, 28);
            this.radioButton_DamageFireOffset.Name = "radioButton_DamageFireOffset";
            this.radioButton_DamageFireOffset.Size = new System.Drawing.Size(117, 17);
            this.radioButton_DamageFireOffset.TabIndex = 1;
            this.radioButton_DamageFireOffset.Text = "DamageFireOffset#";
            this.radioButton_DamageFireOffset.UseVisualStyleBackColor = true;
            // 
            // radioButton_MuzzleFlash
            // 
            this.radioButton_MuzzleFlash.AutoSize = true;
            this.radioButton_MuzzleFlash.Location = new System.Drawing.Point(3, 53);
            this.radioButton_MuzzleFlash.Name = "radioButton_MuzzleFlash";
            this.radioButton_MuzzleFlash.Size = new System.Drawing.Size(90, 17);
            this.radioButton_MuzzleFlash.TabIndex = 2;
            this.radioButton_MuzzleFlash.Text = "MuzzleFlash#";
            this.radioButton_MuzzleFlash.UseVisualStyleBackColor = true;
            // 
            // checkBox_IsLaser
            // 
            this.checkBox_IsLaser.AutoSize = true;
            this.checkBox_IsLaser.Location = new System.Drawing.Point(223, 53);
            this.checkBox_IsLaser.Name = "checkBox_IsLaser";
            this.checkBox_IsLaser.Size = new System.Drawing.Size(60, 17);
            this.checkBox_IsLaser.TabIndex = 3;
            this.checkBox_IsLaser.Text = "IsLaser";
            this.checkBox_IsLaser.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel3_Canvas
            // 
            this.tableLayoutPanel3_Canvas.ColumnCount = 2;
            this.tableLayoutPanel3_Canvas.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3_Canvas.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3_Canvas.Controls.Add(this.groupBox_Left, 0, 0);
            this.tableLayoutPanel3_Canvas.Controls.Add(this.groupBox_Right, 1, 0);
            this.tableLayoutPanel3_Canvas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3_Canvas.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel3_Canvas.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3_Canvas.Name = "tableLayoutPanel3_Canvas";
            this.tableLayoutPanel3_Canvas.RowCount = 1;
            this.tableLayoutPanel3_Canvas.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3_Canvas.Size = new System.Drawing.Size(453, 267);
            this.tableLayoutPanel3_Canvas.TabIndex = 3;
            // 
            // groupBox_Left
            // 
            this.groupBox_Left.Controls.Add(this.tableLayoutPanel3_GroupLeft);
            this.groupBox_Left.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_Left.Location = new System.Drawing.Point(3, 3);
            this.groupBox_Left.Name = "groupBox_Left";
            this.groupBox_Left.Size = new System.Drawing.Size(220, 261);
            this.groupBox_Left.TabIndex = 6;
            this.groupBox_Left.TabStop = false;
            this.groupBox_Left.Text = "groupBox1";
            // 
            // tableLayoutPanel3_GroupLeft
            // 
            this.tableLayoutPanel3_GroupLeft.ColumnCount = 1;
            this.tableLayoutPanel3_GroupLeft.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3_GroupLeft.Controls.Add(this.tableLayoutPanel4_FrameLeft, 0, 1);
            this.tableLayoutPanel3_GroupLeft.Controls.Add(this.uC_ImageCanvas_Left, 0, 0);
            this.tableLayoutPanel3_GroupLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3_GroupLeft.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel3_GroupLeft.Name = "tableLayoutPanel3_GroupLeft";
            this.tableLayoutPanel3_GroupLeft.RowCount = 2;
            this.tableLayoutPanel3_GroupLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3_GroupLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel3_GroupLeft.Size = new System.Drawing.Size(214, 242);
            this.tableLayoutPanel3_GroupLeft.TabIndex = 0;
            // 
            // tableLayoutPanel4_FrameLeft
            // 
            this.tableLayoutPanel4_FrameLeft.ColumnCount = 4;
            this.tableLayoutPanel4_FrameLeft.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel4_FrameLeft.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel4_FrameLeft.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4_FrameLeft.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel4_FrameLeft.Controls.Add(this.numericUpDown_FrameLeft, 3, 0);
            this.tableLayoutPanel4_FrameLeft.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel4_FrameLeft.Controls.Add(this.numericUpDown_Zoom, 1, 0);
            this.tableLayoutPanel4_FrameLeft.Controls.Add(this.label_FrameLeft, 2, 0);
            this.tableLayoutPanel4_FrameLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4_FrameLeft.Location = new System.Drawing.Point(0, 212);
            this.tableLayoutPanel4_FrameLeft.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel4_FrameLeft.Name = "tableLayoutPanel4_FrameLeft";
            this.tableLayoutPanel4_FrameLeft.RowCount = 1;
            this.tableLayoutPanel4_FrameLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4_FrameLeft.Size = new System.Drawing.Size(214, 30);
            this.tableLayoutPanel4_FrameLeft.TabIndex = 4;
            // 
            // numericUpDown_FrameLeft
            // 
            this.numericUpDown_FrameLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numericUpDown_FrameLeft.IncrementByMouseWheel = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.numericUpDown_FrameLeft.Location = new System.Drawing.Point(157, 3);
            this.numericUpDown_FrameLeft.Name = "numericUpDown_FrameLeft";
            this.numericUpDown_FrameLeft.Size = new System.Drawing.Size(54, 20);
            this.numericUpDown_FrameLeft.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Zoom:";
            // 
            // numericUpDown_Zoom
            // 
            this.numericUpDown_Zoom.IncrementByMouseWheel = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.numericUpDown_Zoom.Location = new System.Drawing.Point(53, 3);
            this.numericUpDown_Zoom.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDown_Zoom.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_Zoom.Name = "numericUpDown_Zoom";
            this.numericUpDown_Zoom.Size = new System.Drawing.Size(39, 20);
            this.numericUpDown_Zoom.TabIndex = 3;
            this.numericUpDown_Zoom.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_Zoom.ValueChanged += new System.EventHandler(this.numericUpDown_Zoom_ValueChanged);
            // 
            // label_FrameLeft
            // 
            this.label_FrameLeft.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label_FrameLeft.AutoSize = true;
            this.label_FrameLeft.Location = new System.Drawing.Point(112, 8);
            this.label_FrameLeft.Name = "label_FrameLeft";
            this.label_FrameLeft.Size = new System.Drawing.Size(39, 13);
            this.label_FrameLeft.TabIndex = 1;
            this.label_FrameLeft.Text = "Frame:";
            // 
            // uC_ImageCanvas_Left
            // 
            this.uC_ImageCanvas_Left.CaptureFocusOnMouseOver = true;
            this.uC_ImageCanvas_Left.CenterOffset = new System.Drawing.Point(0, 0);
            this.uC_ImageCanvas_Left.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uC_ImageCanvas_Left.Location = new System.Drawing.Point(3, 3);
            this.uC_ImageCanvas_Left.LocationFromCenter = false;
            this.uC_ImageCanvas_Left.MinimumSize = new System.Drawing.Size(200, 200);
            this.uC_ImageCanvas_Left.Name = "uC_ImageCanvas_Left";
            this.uC_ImageCanvas_Left.PreviewPixel = null;
            this.uC_ImageCanvas_Left.ShowCenterCross = false;
            this.uC_ImageCanvas_Left.ShowColorInfo = true;
            this.uC_ImageCanvas_Left.Size = new System.Drawing.Size(208, 206);
            this.uC_ImageCanvas_Left.TabIndex = 5;
            this.uC_ImageCanvas_Left.ZoomLevel = 1;
            // 
            // groupBox_Right
            // 
            this.groupBox_Right.Controls.Add(this.tableLayoutPanel3_GroupRight);
            this.groupBox_Right.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_Right.Location = new System.Drawing.Point(229, 3);
            this.groupBox_Right.Name = "groupBox_Right";
            this.groupBox_Right.Size = new System.Drawing.Size(221, 261);
            this.groupBox_Right.TabIndex = 7;
            this.groupBox_Right.TabStop = false;
            this.groupBox_Right.Text = "groupBox1";
            // 
            // tableLayoutPanel3_GroupRight
            // 
            this.tableLayoutPanel3_GroupRight.ColumnCount = 1;
            this.tableLayoutPanel3_GroupRight.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3_GroupRight.Controls.Add(this.uC_ImageCanvas_Right, 0, 0);
            this.tableLayoutPanel3_GroupRight.Controls.Add(this.tableLayoutPanel4_FrameRight, 0, 1);
            this.tableLayoutPanel3_GroupRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3_GroupRight.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel3_GroupRight.Name = "tableLayoutPanel3_GroupRight";
            this.tableLayoutPanel3_GroupRight.RowCount = 2;
            this.tableLayoutPanel3_GroupRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3_GroupRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel3_GroupRight.Size = new System.Drawing.Size(215, 242);
            this.tableLayoutPanel3_GroupRight.TabIndex = 0;
            // 
            // uC_ImageCanvas_Right
            // 
            this.uC_ImageCanvas_Right.AutoScroll = true;
            this.uC_ImageCanvas_Right.CaptureFocusOnMouseOver = true;
            this.uC_ImageCanvas_Right.CenterOffset = new System.Drawing.Point(0, 0);
            this.uC_ImageCanvas_Right.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uC_ImageCanvas_Right.Location = new System.Drawing.Point(3, 3);
            this.uC_ImageCanvas_Right.LocationFromCenter = false;
            this.uC_ImageCanvas_Right.MinimumSize = new System.Drawing.Size(200, 200);
            this.uC_ImageCanvas_Right.Name = "uC_ImageCanvas_Right";
            this.uC_ImageCanvas_Right.PreviewPixel = null;
            this.uC_ImageCanvas_Right.ShowCenterCross = false;
            this.uC_ImageCanvas_Right.ShowColorInfo = true;
            this.uC_ImageCanvas_Right.Size = new System.Drawing.Size(209, 206);
            this.uC_ImageCanvas_Right.TabIndex = 1;
            this.uC_ImageCanvas_Right.ZoomLevel = 1;
            // 
            // tableLayoutPanel4_FrameRight
            // 
            this.tableLayoutPanel4_FrameRight.ColumnCount = 2;
            this.tableLayoutPanel4_FrameRight.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4_FrameRight.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel4_FrameRight.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel4_FrameRight.Controls.Add(this.numericUpDown_FrameRight, 1, 0);
            this.tableLayoutPanel4_FrameRight.Controls.Add(this.label_FrameRight, 0, 0);
            this.tableLayoutPanel4_FrameRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4_FrameRight.Location = new System.Drawing.Point(0, 212);
            this.tableLayoutPanel4_FrameRight.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel4_FrameRight.Name = "tableLayoutPanel4_FrameRight";
            this.tableLayoutPanel4_FrameRight.RowCount = 1;
            this.tableLayoutPanel4_FrameRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4_FrameRight.Size = new System.Drawing.Size(215, 30);
            this.tableLayoutPanel4_FrameRight.TabIndex = 5;
            // 
            // numericUpDown_FrameRight
            // 
            this.numericUpDown_FrameRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numericUpDown_FrameRight.IncrementByMouseWheel = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.numericUpDown_FrameRight.Location = new System.Drawing.Point(158, 3);
            this.numericUpDown_FrameRight.Name = "numericUpDown_FrameRight";
            this.numericUpDown_FrameRight.Size = new System.Drawing.Size(54, 20);
            this.numericUpDown_FrameRight.TabIndex = 0;
            // 
            // label_FrameRight
            // 
            this.label_FrameRight.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label_FrameRight.AutoSize = true;
            this.label_FrameRight.Location = new System.Drawing.Point(113, 8);
            this.label_FrameRight.Name = "label_FrameRight";
            this.label_FrameRight.Size = new System.Drawing.Size(39, 13);
            this.label_FrameRight.TabIndex = 1;
            this.label_FrameRight.Text = "Frame:";
            // 
            // richTextBox1
            // 
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Location = new System.Drawing.Point(3, 63);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(171, 281);
            this.richTextBox1.TabIndex = 2;
            this.richTextBox1.Text = "";
            // 
            // tableLayoutPanel1_Main
            // 
            this.tableLayoutPanel1_Main.ColumnCount = 2;
            this.tableLayoutPanel1_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 71.5F));
            this.tableLayoutPanel1_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 28.5F));
            this.tableLayoutPanel1_Main.Controls.Add(this.tableLayoutPanel2_Left, 0, 0);
            this.tableLayoutPanel1_Main.Controls.Add(this.tableLayoutPanel2_Right, 1, 0);
            this.tableLayoutPanel1_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1_Main.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1_Main.Name = "tableLayoutPanel1_Main";
            this.tableLayoutPanel1_Main.RowCount = 1;
            this.tableLayoutPanel1_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1_Main.Size = new System.Drawing.Size(642, 383);
            this.tableLayoutPanel1_Main.TabIndex = 1;
            // 
            // tableLayoutPanel2_Right
            // 
            this.tableLayoutPanel2_Right.ColumnCount = 1;
            this.tableLayoutPanel2_Right.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2_Right.Controls.Add(this.richTextBox1, 0, 2);
            this.tableLayoutPanel2_Right.Controls.Add(this.button_Close, 0, 3);
            this.tableLayoutPanel2_Right.Controls.Add(this.button_LoadSHP, 0, 0);
            this.tableLayoutPanel2_Right.Controls.Add(this.button_LoadPalette, 0, 1);
            this.tableLayoutPanel2_Right.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2_Right.Location = new System.Drawing.Point(462, 3);
            this.tableLayoutPanel2_Right.Name = "tableLayoutPanel2_Right";
            this.tableLayoutPanel2_Right.RowCount = 4;
            this.tableLayoutPanel2_Right.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2_Right.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2_Right.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2_Right.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2_Right.Size = new System.Drawing.Size(177, 377);
            this.tableLayoutPanel2_Right.TabIndex = 1;
            // 
            // button_Close
            // 
            this.button_Close.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_Close.Location = new System.Drawing.Point(3, 350);
            this.button_Close.Name = "button_Close";
            this.button_Close.Size = new System.Drawing.Size(171, 24);
            this.button_Close.TabIndex = 3;
            this.button_Close.Text = "Close";
            this.button_Close.UseVisualStyleBackColor = true;
            this.button_Close.Click += new System.EventHandler(this.button_Close_Click);
            // 
            // button_LoadSHP
            // 
            this.button_LoadSHP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_LoadSHP.Location = new System.Drawing.Point(3, 3);
            this.button_LoadSHP.Name = "button_LoadSHP";
            this.button_LoadSHP.Size = new System.Drawing.Size(171, 24);
            this.button_LoadSHP.TabIndex = 4;
            this.button_LoadSHP.Text = "Load SHP";
            this.button_LoadSHP.UseVisualStyleBackColor = true;
            this.button_LoadSHP.Click += new System.EventHandler(this.button_LoadSHP_Click);
            // 
            // button_LoadPalette
            // 
            this.button_LoadPalette.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_LoadPalette.Location = new System.Drawing.Point(3, 33);
            this.button_LoadPalette.Name = "button_LoadPalette";
            this.button_LoadPalette.Size = new System.Drawing.Size(171, 24);
            this.button_LoadPalette.TabIndex = 5;
            this.button_LoadPalette.Text = "Palette:";
            this.button_LoadPalette.UseVisualStyleBackColor = true;
            this.button_LoadPalette.Click += new System.EventHandler(this.button_LoadPalette_Click);
            // 
            // Form_FireFLHFinder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(642, 383);
            this.Controls.Add(this.tableLayoutPanel1_Main);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(650, 410);
            this.Name = "Form_FireFLHFinder";
            this.Text = "PixelOffset and FireFLH Finder";
            this.tableLayoutPanel2_Left.ResumeLayout(false);
            this.groupBox_Mode.ResumeLayout(false);
            this.tableLayoutPanel4_GroupMode.ResumeLayout(false);
            this.tableLayoutPanel4_GroupMode.PerformLayout();
            this.tableLayoutPanel3_Canvas.ResumeLayout(false);
            this.groupBox_Left.ResumeLayout(false);
            this.tableLayoutPanel3_GroupLeft.ResumeLayout(false);
            this.tableLayoutPanel4_FrameLeft.ResumeLayout(false);
            this.tableLayoutPanel4_FrameLeft.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_FrameLeft)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Zoom)).EndInit();
            this.groupBox_Right.ResumeLayout(false);
            this.tableLayoutPanel3_GroupRight.ResumeLayout(false);
            this.tableLayoutPanel4_FrameRight.ResumeLayout(false);
            this.tableLayoutPanel4_FrameRight.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_FrameRight)).EndInit();
            this.tableLayoutPanel1_Main.ResumeLayout(false);
            this.tableLayoutPanel2_Right.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2_Left;
        private System.Windows.Forms.RadioButton radioButton_FireFLH;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1_Main;
        private System.Windows.Forms.GroupBox groupBox_Mode;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4_GroupMode;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2_Right;
        private System.Windows.Forms.Button button_Close;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4_FrameLeft;
        private NumericUpDownEx numericUpDown_FrameLeft;
        private System.Windows.Forms.Label label_FrameLeft;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3_Canvas;
        private UC_ImageCanvas uC_ImageCanvas_Right;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4_FrameRight;
        private NumericUpDownEx numericUpDown_FrameRight;
        private System.Windows.Forms.Label label_FrameRight;
        private System.Windows.Forms.Button button_LoadSHP;
        private System.Windows.Forms.GroupBox groupBox_Left;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3_GroupLeft;
        private System.Windows.Forms.GroupBox groupBox_Right;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3_GroupRight;
        private UC_ImageCanvas uC_ImageCanvas_Left;
        private System.Windows.Forms.Button button_LoadPalette;
        private System.Windows.Forms.RadioButton radioButton_FirePixelOffset;
        private System.Windows.Forms.RadioButton radioButton_DamageFireOffset;
        private System.Windows.Forms.RadioButton radioButton_MuzzleFlash;
        private System.Windows.Forms.CheckBox checkBox_IsLaser;
        private System.Windows.Forms.Label label1;
        private NumericUpDownEx numericUpDown_Zoom;
    }
}