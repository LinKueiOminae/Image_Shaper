partial class Form_AboutBox
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_AboutBox));
        this.pictureBox1 = new System.Windows.Forms.PictureBox();
        this.textBox_Description = new RichTextBoxEx();
        this.button_close = new System.Windows.Forms.Button();
        this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
        this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
        ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
        this.tableLayoutPanel1.SuspendLayout();
        this.tableLayoutPanel2.SuspendLayout();
        this.SuspendLayout();
        // 
        // pictureBox1
        // 
        this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
        this.pictureBox1.Location = new System.Drawing.Point(194, 0);
        this.pictureBox1.Margin = new System.Windows.Forms.Padding(0);
        this.pictureBox1.Name = "pictureBox1";
        this.pictureBox1.Size = new System.Drawing.Size(180, 80);
        this.pictureBox1.TabIndex = 0;
        this.pictureBox1.TabStop = false;
        // 
        // textBox_Description
        // 
        this.textBox_Description.BackColor = System.Drawing.SystemColors.Control;
        this.textBox_Description.Cursor = System.Windows.Forms.Cursors.Arrow;
        this.textBox_Description.Dock = System.Windows.Forms.DockStyle.Fill;
        this.textBox_Description.Location = new System.Drawing.Point(3, 83);
        this.textBox_Description.Name = "textBox_Description";
        this.textBox_Description.ReadOnly = true;
        this.textBox_Description.Size = new System.Drawing.Size(368, 168);
        this.textBox_Description.TabIndex = 1;
        this.textBox_Description.TabStop = false;
        this.textBox_Description.Text = "Description";
        this.textBox_Description.WordWrap = false;
        // 
        // button_close
        // 
        this.button_close.Location = new System.Drawing.Point(3, 3);
        this.button_close.Name = "button_close";
        this.button_close.Size = new System.Drawing.Size(134, 23);
        this.button_close.TabIndex = 2;
        this.button_close.Text = "Close";
        this.button_close.UseVisualStyleBackColor = true;
        this.button_close.Click += new System.EventHandler(this.button_close_Click);
        // 
        // tableLayoutPanel1
        // 
        this.tableLayoutPanel1.ColumnCount = 1;
        this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
        this.tableLayoutPanel1.Controls.Add(this.textBox_Description, 0, 1);
        this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
        this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
        this.tableLayoutPanel1.Location = new System.Drawing.Point(9, 9);
        this.tableLayoutPanel1.Name = "tableLayoutPanel1";
        this.tableLayoutPanel1.RowCount = 2;
        this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
        this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
        this.tableLayoutPanel1.Size = new System.Drawing.Size(374, 254);
        this.tableLayoutPanel1.TabIndex = 3;
        // 
        // tableLayoutPanel2
        // 
        this.tableLayoutPanel2.ColumnCount = 2;
        this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 140F));
        this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
        this.tableLayoutPanel2.Controls.Add(this.pictureBox1, 1, 0);
        this.tableLayoutPanel2.Controls.Add(this.button_close, 0, 0);
        this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
        this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
        this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
        this.tableLayoutPanel2.Name = "tableLayoutPanel2";
        this.tableLayoutPanel2.RowCount = 1;
        this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
        this.tableLayoutPanel2.Size = new System.Drawing.Size(374, 80);
        this.tableLayoutPanel2.TabIndex = 2;
        // 
        // Form_AboutBox
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(392, 272);
        this.Controls.Add(this.tableLayoutPanel1);
        this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.MinimumSize = new System.Drawing.Size(400, 300);
        this.Name = "Form_AboutBox";
        this.Padding = new System.Windows.Forms.Padding(9);
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "AboutBox1";
        ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
        this.tableLayoutPanel1.ResumeLayout(false);
        this.tableLayoutPanel2.ResumeLayout(false);
        this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.PictureBox pictureBox1;
    private RichTextBoxEx textBox_Description;
    private System.Windows.Forms.Button button_close;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;

}
