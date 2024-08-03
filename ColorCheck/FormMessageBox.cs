using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

public partial class EnhancedMessageBox : Form
{

    public event MyLinkClickedEventHandler LinkClicked;
    public delegate void MyLinkClickedEventHandler(object sender, LinkClickedEventArgs e);

    private bool manualSize = false;

    public EnhancedMessageBox()
    {
        InitializeComponent();
        this.StartPosition = FormStartPosition.Manual;
        this.pictureBox1.Image = Bitmap.FromHicon(SystemIcons.Warning.Handle);
        this.richTextBoxEx_Info.ReadOnly = true;
        this.richTextBoxEx_Info.BackColor = this.panel1.BackColor;

        this.richTextBoxEx_Info.LinkClicked += new LinkClickedEventHandler(richTextBoxEx_Info_LinkClicked);
        this.Shown += new EventHandler(EnhancedMessageBox_Shown);
    }

    void richTextBoxEx_Info_LinkClicked(object sender, LinkClickedEventArgs e)
    {
        LinkClicked(sender, e);
    }

    void EnhancedMessageBox_Shown(object sender, EventArgs e)
    {
        //stop the default cursor flashing in the richtextbox
        this.button_OK.Focus();
    }

    public EnhancedMessageBox(int xsize, int ysize)
    {
        InitializeComponent();
        this.pictureBox1.Image = Bitmap.FromHicon(SystemIcons.Warning.Handle);
        manualSize = true;
        this.Size = new Size(xsize, ysize);
    }

    public void SetLocation(int x, int y)
    {
        this.Left = x;
        this.Top = y;
    }

    public void SetLocation(Point p)
    {
        this.Left = p.X;
        this.Top = p.Y;
    }

    public static DialogResult ShowEnhancedMessageBox(string message, string caption)
    {
        using (EnhancedMessageBox mybox = new EnhancedMessageBox())
        {
            mybox.StartPosition = FormStartPosition.Manual;
            mybox.Text = caption;
            mybox.richTextBoxEx_Info.Text = message;
            return mybox.ShowDialog();
        }
    }

    public void SetIcon(IntPtr icon)
    {

        this.pictureBox1.Image = Bitmap.FromHicon(icon);
    }

    public void AddText(string text)
    {
        this.richTextBoxEx_Info.SelectedText = text;
        autoTextSize();
    }
    public void AddText(string text, Color color)
    {
        Color oldcolor = this.richTextBoxEx_Info.SelectionColor;
        this.richTextBoxEx_Info.SelectionColor = color;
        this.richTextBoxEx_Info.SelectedText = text;
        this.richTextBoxEx_Info.SelectionColor = oldcolor;
        autoTextSize();
    }
    public void AddText(string text, Color color, Font font)
    {
        Color oldcolor = this.richTextBoxEx_Info.SelectionColor;
        System.Drawing.Font oldfont = this.richTextBoxEx_Info.SelectionFont;
        this.richTextBoxEx_Info.SelectionColor = color;
        this.richTextBoxEx_Info.SelectionFont = font;

        this.richTextBoxEx_Info.SelectedText = text;

        this.richTextBoxEx_Info.SelectionColor = oldcolor;
        this.richTextBoxEx_Info.SelectionFont = oldfont;
        autoTextSize();
    }

    public void InsertLink(string text)
    {
        this.richTextBoxEx_Info.InsertLink(text);
        autoTextSize();
    }
    public void InsertLink(string text, int position)
    {
        this.richTextBoxEx_Info.InsertLink(text, position);
        autoTextSize();
    }
    public void InsertLink(string text, string hyperlink)
    {
        this.richTextBoxEx_Info.InsertLink(text, hyperlink);
        autoTextSize();
    }
    public void InsertLink(string text, string hyperlink, int position)
    {
        this.richTextBoxEx_Info.InsertLink(text, hyperlink, position);
        autoTextSize();
    }

    public void SetText(string text)
    {
        this.richTextBoxEx_Info.Text = "";
        this.richTextBoxEx_Info.SelectedText = text;
        autoTextSize();
    }

    public void SetCaption(string text)
    {
        this.Text = text;
        autoTextSize();
    }

    private void autoTextSize()
    {
        if (!manualSize)
        {
            Graphics graphics = this.CreateGraphics();
            SizeF textSize = graphics.MeasureString(this.richTextBoxEx_Info.Text, this.richTextBoxEx_Info.Font);
            int textwidth = (int)(this.richTextBoxEx_Info.Left + 20 + textSize.Width);
            int textheigth = (int)(this.MinimumSize.Height - 50 + textSize.Height);

            SizeF captionSize = graphics.MeasureString(this.Text, SystemFonts.CaptionFont);
            int captionwidth = (int)(captionSize.Width + 10);
            if (captionwidth > textwidth)
                this.Size = new Size(captionwidth, textheigth);
            else
                this.Size = new Size(textwidth, textheigth);
        }
    }

    public void ProgressBarSize(int size)
    {
        this.progressBar1.Maximum = size;
    }

    public void ProgressBarSize(long size)
    {
        this.progressBar1.Maximum = (int)size;
    }

    public void ProgressBarVisible(bool visible)
    {
        this.progressBar1.Visible = visible;
        this.label_ProgressBar.Visible = visible;
    }

    public void ProgressBarValue(int value)
    {
        this.label_ProgressBar.Text = string.Format("{0:0.0%}", (double)value / this.progressBar1.Maximum);
        if (value > this.progressBar1.Maximum) value = this.progressBar1.Maximum;
        if (value < this.progressBar1.Minimum) value = this.progressBar1.Minimum;
        this.progressBar1.Value = value;
    }

    public void button_OK_text(string text)
    {
        this.button_OK.Text = text;
    }

    private void button_OK_Click(object sender, EventArgs e)
    {
        this.DialogResult = DialogResult.OK;
        this.Close();
    }


    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        this.progressBar1.SendToBack();
        this.label_ProgressBar.BringToFront();
    }
}

public class RichTextBoxEx : RichTextBox
{
    #region Interop-Defines
    [StructLayout(LayoutKind.Sequential)]
    private struct CHARFORMAT2_STRUCT
    {
        public UInt32 cbSize;
        public UInt32 dwMask;
        public UInt32 dwEffects;
        public Int32 yHeight;
        public Int32 yOffset;
        public Int32 crTextColor;
        public byte bCharSet;
        public byte bPitchAndFamily;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public char[] szFaceName;
        public UInt16 wWeight;
        public UInt16 sSpacing;
        public int crBackColor; // Color.ToArgb() -> int
        public int lcid;
        public int dwReserved;
        public Int16 sStyle;
        public Int16 wKerning;
        public byte bUnderlineType;
        public byte bAnimation;
        public byte bRevAuthor;
        public byte bReserved1;
    }

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

    private const int WM_USER = 0x0400;
    private const int EM_GETCHARFORMAT = WM_USER + 58;
    private const int EM_SETCHARFORMAT = WM_USER + 68;

    private const int SCF_SELECTION = 0x0001;
    private const int SCF_WORD = 0x0002;
    private const int SCF_ALL = 0x0004;

    #region CHARFORMAT2 Flags
    private const UInt32 CFE_BOLD = 0x0001;
    private const UInt32 CFE_ITALIC = 0x0002;
    private const UInt32 CFE_UNDERLINE = 0x0004;
    private const UInt32 CFE_STRIKEOUT = 0x0008;
    private const UInt32 CFE_PROTECTED = 0x0010;
    private const UInt32 CFE_LINK = 0x0020;
    private const UInt32 CFE_AUTOCOLOR = 0x40000000;
    private const UInt32 CFE_SUBSCRIPT = 0x00010000;		/* Superscript and subscript are */
    private const UInt32 CFE_SUPERSCRIPT = 0x00020000;		/*  mutually exclusive			 */

    private const int CFM_SMALLCAPS = 0x0040;			/* (*)	*/
    private const int CFM_ALLCAPS = 0x0080;			/* Displayed by 3.0	*/
    private const int CFM_HIDDEN = 0x0100;			/* Hidden by 3.0 */
    private const int CFM_OUTLINE = 0x0200;			/* (*)	*/
    private const int CFM_SHADOW = 0x0400;			/* (*)	*/
    private const int CFM_EMBOSS = 0x0800;			/* (*)	*/
    private const int CFM_IMPRINT = 0x1000;			/* (*)	*/
    private const int CFM_DISABLED = 0x2000;
    private const int CFM_REVISED = 0x4000;

    private const int CFM_BACKCOLOR = 0x04000000;
    private const int CFM_LCID = 0x02000000;
    private const int CFM_UNDERLINETYPE = 0x00800000;		/* Many displayed by 3.0 */
    private const int CFM_WEIGHT = 0x00400000;
    private const int CFM_SPACING = 0x00200000;		/* Displayed by 3.0	*/
    private const int CFM_KERNING = 0x00100000;		/* (*)	*/
    private const int CFM_STYLE = 0x00080000;		/* (*)	*/
    private const int CFM_ANIMATION = 0x00040000;		/* (*)	*/
    private const int CFM_REVAUTHOR = 0x00008000;


    private const UInt32 CFM_BOLD = 0x00000001;
    private const UInt32 CFM_ITALIC = 0x00000002;
    private const UInt32 CFM_UNDERLINE = 0x00000004;
    private const UInt32 CFM_STRIKEOUT = 0x00000008;
    private const UInt32 CFM_PROTECTED = 0x00000010;
    private const UInt32 CFM_LINK = 0x00000020;
    private const UInt32 CFM_SIZE = 0x80000000;
    private const UInt32 CFM_COLOR = 0x40000000;
    private const UInt32 CFM_FACE = 0x20000000;
    private const UInt32 CFM_OFFSET = 0x10000000;
    private const UInt32 CFM_CHARSET = 0x08000000;
    private const UInt32 CFM_SUBSCRIPT = CFE_SUBSCRIPT | CFE_SUPERSCRIPT;
    private const UInt32 CFM_SUPERSCRIPT = CFM_SUBSCRIPT;

    private const byte CFU_UNDERLINENONE = 0x00000000;
    private const byte CFU_UNDERLINE = 0x00000001;
    private const byte CFU_UNDERLINEWORD = 0x00000002; /* (*) displayed as ordinary underline	*/
    private const byte CFU_UNDERLINEDOUBLE = 0x00000003; /* (*) displayed as ordinary underline	*/
    private const byte CFU_UNDERLINEDOTTED = 0x00000004;
    private const byte CFU_UNDERLINEDASH = 0x00000005;
    private const byte CFU_UNDERLINEDASHDOT = 0x00000006;
    private const byte CFU_UNDERLINEDASHDOTDOT = 0x00000007;
    private const byte CFU_UNDERLINEWAVE = 0x00000008;
    private const byte CFU_UNDERLINETHICK = 0x00000009;
    private const byte CFU_UNDERLINEHAIRLINE = 0x0000000A; /* (*) displayed as ordinary underline	*/

    #endregion

    #endregion

    public RichTextBoxEx()
    {
        // Otherwise, non-standard links get lost when user starts typing
        // next to a non-standard link
        this.DetectUrls = false;
    }

    [DefaultValue(false)]
    public new bool DetectUrls
    {
        get { return base.DetectUrls; }
        set { base.DetectUrls = value; }
    }

    /// <summary>
    /// Insert a given text as a link into the RichTextBox at the current insert position.
    /// </summary>
    /// <param name="text">Text to be inserted</param>
    public void InsertLink(string text)
    {
        InsertLink(text, this.SelectionStart);
    }

    /// <summary>
    /// Insert a given text at a given position as a link. 
    /// </summary>
    /// <param name="text">Text to be inserted</param>
    /// <param name="position">Insert position</param>
    public void InsertLink(string text, int position)
    {
        if (position < 0 || position > this.Text.Length)
            throw new ArgumentOutOfRangeException("position");
        if ((text != null) && (text != ""))
        {
            this.SelectionStart = position;
            this.SelectedText = text;
            this.Select(position, text.Length);
            this.SetSelectionLink(true);
            this.Select(position + text.Length, 0);
        }
    }

    /// <summary>
    /// Insert a given text at at the current input position as a link.
    /// The link text is followed by a hash (#) and the given hyperlink text, both of
    /// them invisible.
    /// When clicked on, the whole link text and hyperlink string are given in the
    /// LinkClickedEventArgs.
    /// </summary>
    /// <param name="text">Text to be inserted</param>
    /// <param name="hyperlink">Invisible hyperlink string to be inserted</param>
    public void InsertLink(string text, string hyperlink)
    {
        InsertLink(text, hyperlink, this.SelectionStart);
    }

    /// <summary>
    /// Insert a given text at a given position as a link. The link text is followed by
    /// a hash (#) and the given hyperlink text, both of them invisible.
    /// When clicked on, the whole link text and hyperlink string are given in the
    /// LinkClickedEventArgs.
    /// </summary>
    /// <param name="text">Text to be inserted</param>
    /// <param name="hyperlink">Invisible hyperlink string to be inserted</param>
    /// <param name="position">Insert position</param>
    public void InsertLink(string text, string hyperlink, int position)
    {
        if (position < 0 || position > this.Text.Length)
            throw new ArgumentOutOfRangeException("position");

        this.SelectionStart = position;
        this.SelectedRtf = @"{\rtf1\ansi " + text + @"\v #" + hyperlink + @"\v0}";
        this.Select(position, text.Length + hyperlink.Length + 1);
        this.SetSelectionLink(true);
        this.Select(position + text.Length + hyperlink.Length + 1, 0);
    }

    /// <summary>
    /// Set the current selection's link style
    /// </summary>
    /// <param name="link">true: set link style, false: clear link style</param>
    public void SetSelectionLink(bool link)
    {
        SetSelectionStyle(CFM_LINK, link ? CFE_LINK : 0);
    }
    /// <summary>
    /// Get the link style for the current selection
    /// </summary>
    /// <returns>0: link style not set, 1: link style set, -1: mixed</returns>
    public int GetSelectionLink()
    {
        return GetSelectionStyle(CFM_LINK, CFE_LINK);
    }


    private void SetSelectionStyle(UInt32 mask, UInt32 effect)
    {
        CHARFORMAT2_STRUCT cf = new CHARFORMAT2_STRUCT();
        cf.cbSize = (UInt32)Marshal.SizeOf(cf);
        cf.dwMask = mask;
        cf.dwEffects = effect;

        IntPtr wpar = new IntPtr(SCF_SELECTION);
        IntPtr lpar = Marshal.AllocCoTaskMem(Marshal.SizeOf(cf));
        Marshal.StructureToPtr(cf, lpar, false);

        IntPtr res = SendMessage(Handle, EM_SETCHARFORMAT, wpar, lpar);

        Marshal.FreeCoTaskMem(lpar);
    }

    private int GetSelectionStyle(UInt32 mask, UInt32 effect)
    {
        CHARFORMAT2_STRUCT cf = new CHARFORMAT2_STRUCT();
        cf.cbSize = (UInt32)Marshal.SizeOf(cf);
        cf.szFaceName = new char[32];

        IntPtr wpar = new IntPtr(SCF_SELECTION);
        IntPtr lpar = Marshal.AllocCoTaskMem(Marshal.SizeOf(cf));
        Marshal.StructureToPtr(cf, lpar, false);

        IntPtr res = SendMessage(Handle, EM_GETCHARFORMAT, wpar, lpar);

        cf = (CHARFORMAT2_STRUCT)Marshal.PtrToStructure(lpar, typeof(CHARFORMAT2_STRUCT));

        int state;
        // dwMask holds the information which properties are consistent throughout the selection:
        if ((cf.dwMask & mask) == mask)
        {
            if ((cf.dwEffects & effect) == effect)
                state = 1;
            else
                state = 0;
        }
        else
        {
            state = -1;
        }

        Marshal.FreeCoTaskMem(lpar);
        return state;
    }
}