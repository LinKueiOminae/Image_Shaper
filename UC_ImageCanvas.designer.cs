namespace ImageShaper
{
    partial class UC_ImageCanvas
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
            this.label_MouseCoord = new System.Windows.Forms.Label();
            this.label_Color = new System.Windows.Forms.Label();
            this.panel_Canvas = new ImageShaper.EnhPanel();
            this.pictureBox_Canvas = new System.Windows.Forms.PictureBox();
            this.control_PanelBorderDummy = new System.Windows.Forms.Control();
            this.panel_Canvas.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Canvas)).BeginInit();
            this.SuspendLayout();
            // 
            // label_MouseCoord
            // 
            this.label_MouseCoord.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label_MouseCoord.BackColor = System.Drawing.Color.White;
            this.label_MouseCoord.Location = new System.Drawing.Point(3, 175);
            this.label_MouseCoord.Name = "label_MouseCoord";
            this.label_MouseCoord.Size = new System.Drawing.Size(120, 20);
            this.label_MouseCoord.TabIndex = 1;
            this.label_MouseCoord.Text = "[x000 , y000] Zoom=10";
            this.label_MouseCoord.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label_Color
            // 
            this.label_Color.BackColor = System.Drawing.Color.White;
            this.label_Color.Location = new System.Drawing.Point(3, 5);
            this.label_Color.Name = "label_Color";
            this.label_Color.Size = new System.Drawing.Size(110, 20);
            this.label_Color.TabIndex = 3;
            this.label_Color.Text = "RGB=[255,255,255]";
            this.label_Color.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel_Canvas
            // 
            this.panel_Canvas.AutoScroll = true;
            this.panel_Canvas.BackColor = System.Drawing.Color.White;
            this.panel_Canvas.Controls.Add(this.pictureBox_Canvas);
            this.panel_Canvas.Controls.Add(this.control_PanelBorderDummy);
            this.panel_Canvas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_Canvas.Location = new System.Drawing.Point(0, 0);
            this.panel_Canvas.Name = "panel_Canvas";
            this.panel_Canvas.Padding = new System.Windows.Forms.Padding(20);
            this.panel_Canvas.Size = new System.Drawing.Size(200, 200);
            this.panel_Canvas.TabIndex = 1;
            // 
            // pictureBox_Canvas
            // 
            this.pictureBox_Canvas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox_Canvas.Location = new System.Drawing.Point(20, 20);
            this.pictureBox_Canvas.Name = "pictureBox_Canvas";
            this.pictureBox_Canvas.Size = new System.Drawing.Size(160, 160);
            this.pictureBox_Canvas.TabIndex = 0;
            this.pictureBox_Canvas.TabStop = false;
            // 
            // control_PanelBorderDummy
            // 
            this.control_PanelBorderDummy.Location = new System.Drawing.Point(171, 167);
            this.control_PanelBorderDummy.Name = "control_PanelBorderDummy";
            this.control_PanelBorderDummy.Size = new System.Drawing.Size(26, 28);
            this.control_PanelBorderDummy.TabIndex = 2;
            this.control_PanelBorderDummy.TabStop = false;
            // 
            // UC_ImageCanvas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label_MouseCoord);
            this.Controls.Add(this.label_Color);
            this.Controls.Add(this.panel_Canvas);
            this.MinimumSize = new System.Drawing.Size(200, 200);
            this.Name = "UC_ImageCanvas";
            this.Size = new System.Drawing.Size(200, 200);
            this.panel_Canvas.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Canvas)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox_Canvas;
        private EnhPanel panel_Canvas;
        private System.Windows.Forms.Label label_MouseCoord;
        private System.Windows.Forms.Control control_PanelBorderDummy;
        private System.Windows.Forms.Label label_Color;
    }
}
