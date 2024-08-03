partial class EnhancedMessageBox
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
        this.button_OK = new System.Windows.Forms.Button();
        this.progressBar1 = new System.Windows.Forms.ProgressBar();
        this.label_ProgressBar = new TransparentLabel();
        this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
        this.richTextBoxEx_Info = new RichTextBoxEx();
        this.pictureBox1 = new System.Windows.Forms.PictureBox();
        this.panel1 = new System.Windows.Forms.Panel();
        this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
        this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
        this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
        this.panel2 = new System.Windows.Forms.Panel();
        ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
        this.panel1.SuspendLayout();
        this.tableLayoutPanel3.SuspendLayout();
        this.tableLayoutPanel1.SuspendLayout();
        this.tableLayoutPanel2.SuspendLayout();
        this.panel2.SuspendLayout();
        this.SuspendLayout();
        // 
        // button_OK
        // 
        this.button_OK.Anchor = System.Windows.Forms.AnchorStyles.Left;
        this.button_OK.Location = new System.Drawing.Point(35, 3);
        this.button_OK.Name = "button_OK";
        this.button_OK.Size = new System.Drawing.Size(74, 23);
        this.button_OK.TabIndex = 0;
        this.button_OK.Text = "OK";
        this.button_OK.UseVisualStyleBackColor = true;
        this.button_OK.Click += new System.EventHandler(this.button_OK_Click);
        // 
        // progressBar1
        // 
        this.progressBar1.Dock = System.Windows.Forms.DockStyle.Fill;
        this.progressBar1.Location = new System.Drawing.Point(0, 0);
        this.progressBar1.Name = "progressBar1";
        this.progressBar1.Size = new System.Drawing.Size(26, 23);
        this.progressBar1.TabIndex = 2;
        this.progressBar1.Visible = false;
        // 
        // label_ProgressBar
        // 
        this.label_ProgressBar.Dock = System.Windows.Forms.DockStyle.Fill;
        this.label_ProgressBar.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.label_ProgressBar.ForeColor = System.Drawing.Color.White;
        this.label_ProgressBar.Location = new System.Drawing.Point(0, 0);
        this.label_ProgressBar.Name = "label_ProgressBar";
        this.label_ProgressBar.Size = new System.Drawing.Size(26, 23);
        this.label_ProgressBar.TabIndex = 3;
        this.label_ProgressBar.Text = "%";
        this.label_ProgressBar.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        this.label_ProgressBar.Visible = false;
        // 
        // flowLayoutPanel1
        // 
        this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
        this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
        this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 150);
        this.flowLayoutPanel1.Name = "flowLayoutPanel1";
        this.flowLayoutPanel1.Size = new System.Drawing.Size(294, 30);
        this.flowLayoutPanel1.TabIndex = 0;
        // 
        // richTextBoxEx_Info
        // 
        this.richTextBoxEx_Info.BorderStyle = System.Windows.Forms.BorderStyle.None;
        this.richTextBoxEx_Info.Dock = System.Windows.Forms.DockStyle.Fill;
        this.richTextBoxEx_Info.Location = new System.Drawing.Point(73, 3);
        this.richTextBoxEx_Info.Name = "richTextBoxEx_Info";
        this.richTextBoxEx_Info.Size = new System.Drawing.Size(36, 91);
        this.richTextBoxEx_Info.TabIndex = 100;
        this.richTextBoxEx_Info.Text = "";
        // 
        // pictureBox1
        // 
        this.pictureBox1.Anchor = System.Windows.Forms.AnchorStyles.Left;
        this.pictureBox1.Location = new System.Drawing.Point(3, 16);
        this.pictureBox1.Name = "pictureBox1";
        this.pictureBox1.Size = new System.Drawing.Size(64, 64);
        this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
        this.pictureBox1.TabIndex = 1;
        this.pictureBox1.TabStop = false;
        // 
        // panel1
        // 
        this.panel1.BackColor = System.Drawing.SystemColors.Info;
        this.panel1.Controls.Add(this.tableLayoutPanel3);
        this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
        this.panel1.Location = new System.Drawing.Point(3, 3);
        this.panel1.Name = "panel1";
        this.panel1.Size = new System.Drawing.Size(112, 97);
        this.panel1.TabIndex = 1;
        // 
        // tableLayoutPanel3
        // 
        this.tableLayoutPanel3.ColumnCount = 2;
        this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
        this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
        this.tableLayoutPanel3.Controls.Add(this.pictureBox1, 0, 0);
        this.tableLayoutPanel3.Controls.Add(this.richTextBoxEx_Info, 1, 0);
        this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
        this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
        this.tableLayoutPanel3.Name = "tableLayoutPanel3";
        this.tableLayoutPanel3.RowCount = 1;
        this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
        this.tableLayoutPanel3.Size = new System.Drawing.Size(112, 97);
        this.tableLayoutPanel3.TabIndex = 2;
        // 
        // tableLayoutPanel1
        // 
        this.tableLayoutPanel1.ColumnCount = 1;
        this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
        this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
        this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
        this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
        this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
        this.tableLayoutPanel1.Name = "tableLayoutPanel1";
        this.tableLayoutPanel1.RowCount = 2;
        this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
        this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
        this.tableLayoutPanel1.Size = new System.Drawing.Size(118, 138);
        this.tableLayoutPanel1.TabIndex = 4;
        // 
        // tableLayoutPanel2
        // 
        this.tableLayoutPanel2.ColumnCount = 2;
        this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
        this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
        this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
        this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
        this.tableLayoutPanel2.Controls.Add(this.panel2, 0, 0);
        this.tableLayoutPanel2.Controls.Add(this.button_OK, 1, 0);
        this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
        this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 106);
        this.tableLayoutPanel2.Name = "tableLayoutPanel2";
        this.tableLayoutPanel2.RowCount = 1;
        this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
        this.tableLayoutPanel2.Size = new System.Drawing.Size(112, 29);
        this.tableLayoutPanel2.TabIndex = 2;
        // 
        // panel2
        // 
        this.panel2.Controls.Add(this.label_ProgressBar);
        this.panel2.Controls.Add(this.progressBar1);
        this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
        this.panel2.Location = new System.Drawing.Point(3, 3);
        this.panel2.Name = "panel2";
        this.panel2.Size = new System.Drawing.Size(26, 23);
        this.panel2.TabIndex = 1;
        // 
        // EnhancedMessageBox
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(118, 138);
        this.ControlBox = false;
        this.Controls.Add(this.tableLayoutPanel1);
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
        this.MaximizeBox = false;
        this.MinimumSize = new System.Drawing.Size(120, 140);
        this.Name = "EnhancedMessageBox";
        this.Text = "Form_MessageBox";
        ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
        this.panel1.ResumeLayout(false);
        this.tableLayoutPanel3.ResumeLayout(false);
        this.tableLayoutPanel1.ResumeLayout(false);
        this.tableLayoutPanel2.ResumeLayout(false);
        this.panel2.ResumeLayout(false);
        this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    private System.Windows.Forms.Button button_OK;
    private System.Windows.Forms.ProgressBar progressBar1;
    private TransparentLabel label_ProgressBar;
    private RichTextBoxEx richTextBoxEx_Info;
    private System.Windows.Forms.PictureBox pictureBox1;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
    private System.Windows.Forms.Panel panel2;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
}
