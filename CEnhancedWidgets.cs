using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

//sets the background of the menustrip to have the custom red gradient
public class CustomMenuStrip : MenuStrip
{
    private Color highMenuColor = Color.FromArgb(255, 0, 200, 0);
    private Color lowMenuColor = SystemColors.Control;
    private Color dropDownItemBackColor = Color.FromArgb(255, 75, 75, 75);
    public CustomMenuStrip()
        : base()
    {
        ToolStripRenderer myrenderer = new CustomMenuStripRenderer(highMenuColor, dropDownItemBackColor);
        this.Renderer = myrenderer;
    }

    private ToolStripMenuItem ColorAll(ToolStripMenuItem menuItem)
    {
        for (int i = 0; i < menuItem.DropDownItems.Count; i++)
        {
            if (menuItem.DropDownItems[i].GetType().Name == "ToolStripMenuItem")
            {
                ToolStripMenuItem submenuItem = (ToolStripMenuItem)menuItem.DropDownItems[i];

                ((ToolStripDropDownMenu)submenuItem.DropDown).ShowImageMargin = true;
                ((ToolStripDropDownMenu)submenuItem.DropDown).BackColor = dropDownItemBackColor;
                submenuItem.ForeColor = Color.White;
                submenuItem = ColorAll(submenuItem);

                ToolStripItemCollection tsic = submenuItem.DropDownItems;
                for (int j = 0; j < tsic.Count; j++)
                {
                    if (tsic[j].GetType().Name == "ToolStripMenuItem")
                        tsic[j].ForeColor = Color.White;
                }
            }
        }
        return menuItem;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        for (int i = 0; i < this.Items.Count; i++)
        {
            if (this.Items[i].GetType().Name == "ToolStripMenuItem")
            {
                ToolStripMenuItem menuItem = (ToolStripMenuItem)this.Items[i];

                ((ToolStripDropDownMenu)menuItem.DropDown).ShowImageMargin = true;
                ((ToolStripDropDownMenu)menuItem.DropDown).BackColor = dropDownItemBackColor;
                menuItem.ForeColor = Color.White;
                menuItem = ColorAll(menuItem);

                ToolStripItemCollection tsic = menuItem.DropDownItems;
                for (int j = 0; j < tsic.Count; j++)
                {
                    if (tsic[j].GetType().Name == "ToolStripMenuItem")
                        tsic[j].ForeColor = Color.White;
                }
            }
        }
    }

    protected override void OnPaintBackground(PaintEventArgs e)
    {
        base.OnPaintBackground(e);
        Graphics g = e.Graphics;
        Rectangle bounds = new Rectangle(Point.Empty, this.Size);

        if (bounds.Width > 0 && bounds.Height > 0)
        {
            Brush b = new System.Drawing.Drawing2D.LinearGradientBrush(
                                    bounds,
                                    highMenuColor,
                                    lowMenuColor,
                                    System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal);
            g.FillRectangle(b, bounds);
        }
    }
}
//color the menuitems
public class CustomMenuStripRenderer : ToolStripProfessionalRenderer
{
    private Color highMenuColor = Color.FromArgb(255, 0, 255, 0);
    private Color lowMenuColor = Color.FromArgb(255, 75, 75, 75);
    private Color borderColor = Color.FromArgb(255, 255, 0, 0);

    public CustomMenuStripRenderer(Color highMenuColor, Color lowMenuColor)
        : base()
    {
        this.highMenuColor = highMenuColor;
        this.lowMenuColor = lowMenuColor;
    }

    protected override void OnRenderImageMargin(ToolStripRenderEventArgs myMenu)
    {
        //base.OnRenderImageMargin(myMenu);

        Rectangle itemBounds = myMenu.AffectedBounds;
        Brush backgroundbrush = new System.Drawing.Drawing2D.LinearGradientBrush(
                                            itemBounds,
                                            Color.FromArgb(0, 0, 0, 0),
                                            highMenuColor,
                                            System.Drawing.Drawing2D.LinearGradientMode.BackwardDiagonal);

        myMenu.Graphics.FillRectangle(backgroundbrush, itemBounds);
    }

    protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs myMenu)
    {
        //base.OnRenderItemText(myMenu);

        //only draw the menustrip itemtext with shadow, not the items in the dropdownmenu
        if ((myMenu.Item.GetType().Name == "ToolStripMenuItem")
         && (myMenu.Item.Owner != null)
         && (myMenu.Item.Owner.GetType().Name == "CustomMenuStrip"))
        {
            myMenu.Graphics.DrawString(myMenu.Text, myMenu.TextFont, new SolidBrush(Color.Black), new Point(myMenu.TextRectangle.X + 1, myMenu.TextRectangle.Y + 1));
            myMenu.Graphics.DrawString(myMenu.Text, myMenu.TextFont, new SolidBrush(myMenu.TextColor), new Point(myMenu.TextRectangle.X, myMenu.TextRectangle.Y));
        }
        else
        {
            if (myMenu.Item.GetType().Name == "ToolStripLabel")
            {
                SizeF textsize = myMenu.Graphics.MeasureString(myMenu.Text, myMenu.TextFont);
                Rectangle itemBounds = new Rectangle(0, 0, myMenu.Item.Owner.Width, myMenu.Item.Height);
                //draw the fancy background only when the label is in a submenu
                if (myMenu.Item.OwnerItem != null)
                {
                    Brush backgroundbrush = new System.Drawing.Drawing2D.LinearGradientBrush(
                                                        itemBounds,
                                                        Color.FromArgb(0, highMenuColor.R, highMenuColor.G, highMenuColor.B),
                                                        Color.FromArgb(100, highMenuColor.R, highMenuColor.G, highMenuColor.B),
                                                        System.Drawing.Drawing2D.LinearGradientMode.Vertical);

                    System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
                    gp = Tools.CnCRectangle.Create(itemBounds, 8, 4, 0);
                    myMenu.Graphics.FillPath(backgroundbrush, gp);
                    //myMenu.Graphics.FillRectangle(backgroundbrush, itembounds);
                    myMenu.Graphics.DrawLine(new Pen(highMenuColor), new Point(0, myMenu.Item.Height - 1), new Point(myMenu.Item.Width, myMenu.Item.Height - 1));
                }

                myMenu.Graphics.DrawString(myMenu.Text, myMenu.TextFont,
                    new SolidBrush(Color.FromArgb(255, 0, 0, 0)),
                    new Point(2, (int)(myMenu.Item.Height - textsize.Height)));
                //draw white text in menustrip, but green when the label is in a submenu
                if (myMenu.Item.OwnerItem == null)
                    myMenu.Graphics.DrawString(myMenu.Text, myMenu.TextFont,
                        new SolidBrush(myMenu.TextColor),
                        new Point(1, (int)(myMenu.Item.Height - textsize.Height - 1)));
                else
                    myMenu.Graphics.DrawString(myMenu.Text, myMenu.TextFont,
                        new SolidBrush(Color.FromArgb(255, 0, 255, 0)),
                        new Point(1, (int)(myMenu.Item.Height - textsize.Height - 1)));
            }
            else
            {
                //items in the dropdownmenu use the default renderer
                base.OnRenderItemText(myMenu);
            }
        }
    }

    protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs myMenu)
    {
        if ((!myMenu.Item.Selected) && (!myMenu.Item.Pressed))
            base.OnRenderMenuItemBackground(myMenu);

        else
        {
            Rectangle itemBounds = new Rectangle(Point.Empty, new Size(myMenu.Item.Size.Width, myMenu.Item.Size.Height));

            System.Drawing.Drawing2D.LinearGradientBrush myBrush = new System.Drawing.Drawing2D.LinearGradientBrush(itemBounds, highMenuColor, Color.FromArgb(255, 0, 0, 0), System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal);

            //Fill Color
            System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
            gp = Tools.CnCRectangle.Create(itemBounds, 8, 4,0);
            myMenu.Graphics.FillPath(myBrush, gp);
            //myMenu.Graphics.FillRectangle(myBrush, menuRectangle);
            // Border Color

            Rectangle linebounds = new Rectangle(itemBounds.X , itemBounds.Y, itemBounds.Width - 1, itemBounds.Height - 1);
            myMenu.Graphics.DrawLine(
                Pens.LightGray,
                new Point(linebounds.X, linebounds.Y + 1),
                new Point(linebounds.Width - 5, linebounds.Y + 1));
            myMenu.Graphics.DrawLine(
                Pens.LightGray,
                new Point(linebounds.Width - 5, linebounds.Y + 1),
                new Point(linebounds.Width, linebounds.Y + 6));

            myMenu.Graphics.DrawLine(
                Pens.LightGray,
                new Point(linebounds.Width, linebounds.Height - 1),
                new Point(linebounds.X + 4, linebounds.Height - 1));
            myMenu.Graphics.DrawLine(
                Pens.LightGray,
                new Point(linebounds.X + 4, linebounds.Height - 1),
                new Point(linebounds.X, linebounds.Height - 5));
            //myMenu.Graphics.DrawRectangle(Pens.Gray, 1, 1, menuRectangle.Width - 3, menuRectangle.Height - 3);

            if (myMenu.Item.Pressed)
            {
                myBrush = new System.Drawing.Drawing2D.LinearGradientBrush(itemBounds, highMenuColor, Color.FromArgb(0, highMenuColor.R, highMenuColor.G, highMenuColor.B), System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal);

                myMenu.Graphics.FillPath(myBrush, gp);
                myMenu.Graphics.DrawLine(
                    Pens.White,
                    new Point(linebounds.X, linebounds.Y + 1),
                    new Point(linebounds.Width - 5, linebounds.Y + 1));
                myMenu.Graphics.DrawLine(
                    Pens.White,
                    new Point(linebounds.Width - 5, linebounds.Y + 1),
                    new Point(linebounds.Width, linebounds.Y + 6));

                myMenu.Graphics.DrawLine(
                    Pens.White,
                    new Point(linebounds.Width, linebounds.Height - 1),
                    new Point(linebounds.X + 4, linebounds.Height - 1));
                myMenu.Graphics.DrawLine(
                    Pens.White,
                    new Point(linebounds.X + 4, linebounds.Height - 1),
                    new Point(linebounds.X, linebounds.Height - 5));
                //myMenu.Graphics.FillRectangle(myBrush, itemBounds);
                //myMenu.Graphics.DrawRectangle(Pens.White, 1, 1, itemBounds.Width - 3, itemBounds.Height - 3);
            }
        }
    }
}

public class CustomToolStripNumericUpDown : ToolStripControlHost
{
    public CustomToolStripNumericUpDown() : base(new NumericUpDown()) { }

    public NumericUpDown NumericUpDown
    {
        get
        {
            return Control as NumericUpDown;
        }
    }
}

/// <summary>
/// needs Refresh() called, every time the background behind the transparent label changes
/// </summary>
public class TransparentLabel : Control
{
    private bool drawShadow = true;
    /// <summary>
    /// Creates a new <see cref="TransparentLabel"/> instance.
    /// </summary>
    public TransparentLabel()
    {
        TabStop = false;
    }

    /// <summary>
    /// Gets the creation parameters.
    /// </summary>
    protected override CreateParams CreateParams
    {
        get
        {
            CreateParams cp = base.CreateParams;
            cp.ExStyle |= 0x20;
            return cp;
        }
    }

    /// <summary>
    /// Paints the background.
    /// </summary>
    /// <param name="e">E.</param>
    protected override void OnPaintBackground(PaintEventArgs e)
    {
        // do nothing
    }

    /// <summary>
    /// Paints the control.
    /// </summary>
    /// <param name="e">E.</param>
    protected override void OnPaint(PaintEventArgs e)
    {
        DrawText();
    }

    protected override void WndProc(ref Message m)
    {
        base.WndProc(ref m);
        if (m.Msg == 0x000F)
        {
            DrawText();
        }
    }

    private void DrawText()
    {
        using (Graphics graphics = CreateGraphics())
        using (SolidBrush brush = new SolidBrush(ForeColor))
        {
            SizeF size = graphics.MeasureString(Text, Font);

            // first figure out the top
            float top = 0;
            switch (textAlign)
            {
                case ContentAlignment.MiddleLeft:
                case ContentAlignment.MiddleCenter:
                case ContentAlignment.MiddleRight:
                    top = (Height - size.Height) / 2;
                    break;
                case ContentAlignment.BottomLeft:
                case ContentAlignment.BottomCenter:
                case ContentAlignment.BottomRight:
                    top = Height - size.Height;
                    break;
            }

            float left = -1;
            switch (textAlign)
            {
                case ContentAlignment.TopLeft:
                case ContentAlignment.MiddleLeft:
                case ContentAlignment.BottomLeft:
                    if (RightToLeft == RightToLeft.Yes)
                        left = Width - size.Width;
                    else
                        left = -1;
                    break;
                case ContentAlignment.TopCenter:
                case ContentAlignment.MiddleCenter:
                case ContentAlignment.BottomCenter:
                    left = (Width - size.Width) / 2;
                    break;
                case ContentAlignment.TopRight:
                case ContentAlignment.MiddleRight:
                case ContentAlignment.BottomRight:
                    if (RightToLeft == RightToLeft.Yes)
                        left = -1;
                    else
                        left = Width - size.Width;
                    break;
            }
            //draw the shadow first which is placed a tad to the southeast
            if (drawShadow) graphics.DrawString(Text, Font, new SolidBrush(Color.Black), left + 1, top + 1);
            graphics.DrawString(Text, Font, brush, left, top);
        }
    }

    /// <summary>
    /// Gets or sets the text associated with this control.
    /// </summary>
    /// <returns>
    /// The text associated with this control.
    /// </returns>
    public override string Text
    {
        get
        {
            return base.Text;
        }
        set
        {
            base.Text = value;
            RecreateHandle();
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether control's elements are aligned to support locales using right-to-left fonts.
    /// </summary>
    /// <value></value>
    /// <returns>
    /// One of the <see cref="T:System.Windows.Forms.RightToLeft"/> values. The default is <see cref="F:System.Windows.Forms.RightToLeft.Inherit"/>.
    /// </returns>
    /// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">
    /// The assigned value is not one of the <see cref="T:System.Windows.Forms.RightToLeft"/> values.
    /// </exception>
    public override RightToLeft RightToLeft
    {
        get
        {
            return base.RightToLeft;
        }
        set
        {
            base.RightToLeft = value;
            RecreateHandle();
        }
    }

    /// <summary>
    /// Gets or sets the font of the text displayed by the control.
    /// </summary>
    /// <value></value>
    /// <returns>
    /// The <see cref="T:System.Drawing.Font"/> to apply to the text displayed by the control. The default is the value of the <see cref="P:System.Windows.Forms.Control.DefaultFont"/> property.
    /// </returns>
    public override Font Font
    {
        get
        {
            return base.Font;
        }
        set
        {
            base.Font = value;
            RecreateHandle();
        }
    }

    private ContentAlignment textAlign = ContentAlignment.TopLeft;
    /// <summary>
    /// Gets or sets the text alignment.
    /// </summary>
    public ContentAlignment TextAlign
    {
        get { return textAlign; }
        set
        {
            textAlign = value;
            RecreateHandle();
        }
    }

    public bool DrawShadow
    {
        get { return drawShadow; }
        set
        {
            drawShadow = value;
            RecreateHandle();
        }
    }
}

/// <summary>
/// NUD that also validates and fires the ValueChanged when the Text is edited directly
/// </summary>
public class NumericUpDownEx : NumericUpDown
{
    private decimal _IncrementByMouseWheel = SystemInformation.MouseWheelScrollLines;
    public decimal IncrementByMouseWheel
    {
        get { return _IncrementByMouseWheel; }
        set { _IncrementByMouseWheel = value; }
    }

    protected override void OnTextChanged(EventArgs e)
    {
        base.OnTextChanged(e);
        decimal d = this.Value;
        if (decimal.TryParse(this.Text, out d))
        {
            if (d > this.Maximum) d = this.Maximum;
            if (d < this.Minimum) d = this.Minimum;
            this.Value = d;
        }
    }

    protected override void OnMouseWheel(MouseEventArgs e)
    {
        HandledMouseEventArgs hme = e as HandledMouseEventArgs;
        if (hme != null)
            hme.Handled = true;
        if (e.Delta > 0)
            if (this.Value + _IncrementByMouseWheel > this.Maximum) this.Value = this.Maximum;
            else
                this.Value += _IncrementByMouseWheel;
        if (e.Delta < 0)
            if (this.Value - _IncrementByMouseWheel < this.Minimum) this.Value = this.Minimum;
            else
                this.Value -= _IncrementByMouseWheel;
    }
}

public class Tools
{
    public abstract class CnCRectangle
    {
        public static System.Drawing.Drawing2D.GraphicsPath Create(Rectangle bounds,
                                          int topoffset, int bottomoffset, int shrink)
        {
            System.Drawing.Drawing2D.GraphicsPath p = new System.Drawing.Drawing2D.GraphicsPath();
            p.StartFigure();

            //____
            p.AddLine(bounds.X + topoffset + shrink, bounds.Y + shrink, bounds.Width - bottomoffset - shrink, bounds.Y + shrink);
            // \
            p.AddLine(bounds.Width - bottomoffset - shrink, bounds.Y + shrink, bounds.Width - shrink, bounds.Y + bottomoffset + shrink);
            // |
            p.AddLine(bounds.Width - shrink, bounds.Y + bottomoffset + shrink, bounds.Width - topoffset - shrink, bounds.Height - shrink);
            // /
            p.AddLine(bounds.Width - topoffset - shrink, bounds.Height - shrink, bounds.X + bottomoffset + shrink, bounds.Height - shrink);
            //____
            p.AddLine(bounds.X + bottomoffset + shrink, bounds.Height - shrink, bounds.X + shrink, bounds.Height - bottomoffset - shrink);
            // \
            p.AddLine(bounds.X + shrink, bounds.Height - bottomoffset - shrink, bounds.X + topoffset + shrink, bounds.Y + shrink);

            p.CloseFigure();
            return p;
        }
    }

    // r,g,b values are from 0 to 1
    // h = [0,360], s = [0,1], v = [0,1]
    //		if s == 0, then h = -1 (undefined)
    public static void RGBtoHSV(Color c, out float h, out float s, out float v)
    {
        RGBtoHSV(c.R / 255, c.G / 255, c.B / 255, out h, out s, out v);
    }
    public static void RGBtoHSV(float r, float g, float b, out float h, out float s, out float v)
    {
        float min, max, delta;
        min = Math.Min(Math.Min(r, g), Math.Min(g, b));
        max = Math.Min(Math.Min(r, g), Math.Min(g, b));
        v = max;				// v
        delta = max - min;
        if (max != 0)
            s = delta / max;		// s
        else
        {
            // r = g = b = 0		// s = 0, v is undefined
            s = 0;
            h = -1;
            return;
        }
        if (r == max)
            h = (g - b) / delta;		// between yellow & magenta
        else if (g == max)
            h = 2 + (b - r) / delta;	// between cyan & yellow
        else
            h = 4 + (r - g) / delta;	// between magenta & cyan
        h *= 60;				// degrees
        if (h < 0)
            h += 360;
    }
    public static void HSVtoRGB(out Color c, float h, float s, float v)
    {
        float r, g, b;
        HSVtoRGB(out r, out g, out b, h, s, v);
        c = Color.FromArgb((int)r * 255, (int)g * 255, (int)b * 255);
    }
    public static void HSVtoRGB(out float r, out float g, out float b, float h, float s, float v)
    {
        int i;
        float f, p, q, t;
        if (s == 0)
        {
            // achromatic (grey)
            r = g = b = v;
            return;
        }
        h /= 60;			// sector 0 to 5
        i = (int)Math.Floor(h);
        f = h - i;			// factorial part of h
        p = v * (1 - s);
        q = v * (1 - s * f);
        t = v * (1 - s * (1 - f));
        switch (i)
        {
            case 0:
                r = v;
                g = t;
                b = p;
                break;
            case 1:
                r = q;
                g = v;
                b = p;
                break;
            case 2:
                r = p;
                g = v;
                b = t;
                break;
            case 3:
                r = p;
                g = q;
                b = v;
                break;
            case 4:
                r = t;
                g = p;
                b = v;
                break;
            default:		// case 5:
                r = v;
                g = p;
                b = q;
                break;
        }
    }

    public static void RGBtoXYZ(Color RGB, out double X, out double Y, out double Z)
    {
        double R = (double)RGB.R / (double)255;
        double G = (double)RGB.G / (double)255;
        double B = (double)RGB.B / (double)255;

        if (R > 0.04045) R = Math.Pow(((R + 0.055) / 1.055), 2.4);
        else R = R / 12.92;
        if (G > 0.04045) G = Math.Pow(((G + 0.055) / 1.055), 2.4);
        else G = G / 12.92;
        if (B > 0.04045) B = Math.Pow(((B + 0.055) / 1.055), 2.4);
        else B = B / 12.92;

        R = R * 100;
        G = G * 100;
        B = B * 100;

        //Observer. = 2°, Illuminant = D65
        X = R * 0.4124 + G * 0.3576 + B * 0.1805;
        Y = R * 0.2126 + G * 0.7152 + B * 0.0722;
        Z = R * 0.0193 + G * 0.1192 + B * 0.9505;
    }

    public struct Color_CIELab
    {
        public double L;
        public double a;
        public double b;
    }

    public static void XYZtoCIELab(double X, double Y, double Z, out Color_CIELab CIELab)
    {
        double ref_X = 95.047;
        double ref_Y = 100.000;
        double ref_Z = 108.883;

        double var_X = X / ref_X;//ref_X =  95.047   Observer= 2°, Illuminant= D65
        double var_Y = Y / ref_Y;//ref_Y = 100.000
        double var_Z = Z / ref_Z;//ref_Z = 108.883

        if (var_X > 0.008856) var_X = Math.Pow(var_X, (double)1 / (double)3);
        else var_X = (7.787 * var_X) + ((double)16 / (double)116);
        if (var_Y > 0.008856) var_Y = Math.Pow(var_Y, (double)1 / (double)3);
        else var_Y = (7.787 * var_Y) + ((double)16 / (double)116);
        if (var_Z > 0.008856) var_Z = Math.Pow(var_Z, (double)1 / (double)3);
        else var_Z = (7.787 * var_Z) + ((double)16 / (double)116);

        CIELab = new Color_CIELab();
        CIELab.L = (116 * var_Y) - 16;
        CIELab.a = 500 * (var_X - var_Y);
        CIELab.b = 200 * (var_Y - var_Z);
    }

    public static void CIELabtoXYZ(Color_CIELab CIELab, out double X, out double Y, out double Z)
    {
        double ref_X = 95.047;
        double ref_Y = 100.000;
        double ref_Z = 108.883;

        double var_Y = (CIELab.L + 16) / 116;
        double var_X = CIELab.a / 500 + var_Y;
        double var_Z = var_Y - CIELab.b / 200;

        if (Math.Pow(var_Y, (double)3) > 0.008856) var_Y = Math.Pow(var_Y, (double)3);
        else var_Y = (var_Y - (double)16 / (double)116) / 7.787;
        if (Math.Pow(var_X, (double)3) > 0.008856) var_X = Math.Pow(var_X, (double)3);
        else var_X = (var_X - (double)16 / (double)116) / 7.787;
        if (Math.Pow(var_Z, (double)3) > 0.008856) var_Z = Math.Pow(var_Z, (double)3);
        else var_Z = (var_Z - (double)16 / (double)116) / 7.787;

        X = ref_X * var_X;     //ref_X =  95.047     Observer= 2°, Illuminant= D65
        Y = ref_Y * var_Y;     //ref_Y = 100.000
        Z = ref_Z * var_Z;     //ref_Z = 108.883
    }

    public static void XYZtoRGB(double X, double Y, double Z, out Color RGB)
    {
        double var_X = X / (double)100;        //X from 0 to  95.047      (Observer = 2°, Illuminant = D65)
        double var_Y = Y / (double)100;        //Y from 0 to 100.000
        double var_Z = Z / (double)100;        //Z from 0 to 108.883

        double var_R = var_X * 3.2406 + var_Y * -1.5372 + var_Z * -0.4986;
        double var_G = var_X * -0.9689 + var_Y * 1.8758 + var_Z * 0.0415;
        double var_B = var_X * 0.0557 + var_Y * -0.2040 + var_Z * 1.0570;

        if (var_R > 0.0031308) var_R = 1.055 * Math.Pow(var_R, (double)1 / (double)2.4) - 0.055;
        else var_R = 12.92 * var_R;
        if (var_G > 0.0031308) var_G = 1.055 * Math.Pow(var_G, (double)1 / (double)2.4) - 0.055;
        else var_G = 12.92 * var_G;
        if (var_B > 0.0031308) var_B = 1.055 * Math.Pow(var_B, (double)1 / (double)2.4) - 0.055;
        else var_B = 12.92 * var_B;

        RGB = Color.FromArgb((int)(var_R * 255), (int)(var_G * 255), (int)(var_B * 255));
    }

    public static void RGBtoCIELab(Color RGB, out Color_CIELab CIELab)
    {
        double X=0;
        double Y=0;
        double Z=0;
        RGBtoXYZ(RGB, out X, out Y, out Z);
        XYZtoCIELab(X, Y, Z, out CIELab);
    }

    public static void CIELabtoRGB(Color_CIELab CIELab, out Color RGB)
    {
        double X=0;
        double Y=0;
        double Z=0;
        CIELabtoXYZ(CIELab, out X, out Y, out Z);
        XYZtoRGB(X, Y, Z, out RGB);
    }

    public static double getDeltaC_viaCIELab(Color RGB1, Color RGB2)
    {
        double DeltaC = 0;
        Color_CIELab CIELab1;
        Color_CIELab CIELab2;
        RGBtoCIELab(RGB1, out CIELab1);
        RGBtoCIELab(RGB2, out CIELab2);

        DeltaC = Math.Sqrt(Math.Pow(CIELab2.a, 2) + Math.Pow(CIELab2.b, 2))
              - Math.Sqrt(Math.Pow(CIELab1.a, 2) + Math.Pow(CIELab1.b, 2));

        return DeltaC;
    }
    public static double getDeltaH_viaCIELab(Color RGB1, Color RGB2)
    {
        double DeltaH = 0;
        Color_CIELab CIELab1;
        Color_CIELab CIELab2;
        RGBtoCIELab(RGB1, out CIELab1);
        RGBtoCIELab(RGB2, out CIELab2);

        double xDE = Math.Sqrt(Math.Pow(CIELab2.a, 2) + Math.Pow(CIELab2.b, 2))
                   - Math.Sqrt(Math.Pow(CIELab1.a, 2) + Math.Pow(CIELab1.b, 2));

        DeltaH = Math.Sqrt(Math.Pow(CIELab2.a - CIELab1.a, 2)
                         + Math.Pow(CIELab2.b - CIELab1.b, 2)
                         - Math.Pow(xDE, 2));
        return DeltaH;
    }
    public static double getDeltaE_viaCIELab(Color RGB1, Color RGB2)
    {
        double DeltaE = 0;
        Color_CIELab CIELab1;
        Color_CIELab CIELab2;
        RGBtoCIELab(RGB1, out CIELab1);
        RGBtoCIELab(RGB2, out CIELab2);

        DeltaE = Math.Sqrt(Math.Pow((CIELab1.L - CIELab2.L), 2)
                         + Math.Pow((CIELab1.a - CIELab2.a), 2)
                         + Math.Pow((CIELab1.b - CIELab2.b), 2));
        return DeltaE;
    }

    public static double getDeltaE94_viaCIELab(Color RGB1, Color RGB2)
    {
        double DeltaE94 = 0;
        Color_CIELab CIELab1;
        Color_CIELab CIELab2;
        RGBtoCIELab(RGB1, out CIELab1);
        RGBtoCIELab(RGB2, out CIELab2);

        double WHT_L = 1.0;
        double WHT_C = 1.0;
        double WHT_H = 1.0;

        double xC1 = Math.Sqrt(Math.Pow(CIELab1.a, (double)2) + Math.Pow(CIELab1.b, (double)2));
        double xC2 = Math.Sqrt(Math.Pow(CIELab2.a, (double)2) + Math.Pow(CIELab2.b, (double)2));
        double xDL = CIELab2.L - CIELab1.L;
        double xDC = xC2 - xC1;
        double xDE = Math.Sqrt(((CIELab1.L - CIELab2.L) * (CIELab1.L - CIELab2.L))
          + ((CIELab1.a - CIELab2.a) * (CIELab1.a - CIELab2.a))
          + ((CIELab1.b - CIELab2.b) * (CIELab1.b - CIELab2.b)));
        double xDH = 0.0;
        if (Math.Sqrt(xDE) > (Math.Sqrt(Math.Abs(xDL)) + Math.Sqrt(Math.Abs(xDC))))
        {
            xDH = Math.Sqrt((xDE * xDE) - (xDL * xDL) - (xDC * xDC));
        }
        double xSC = 1 + (0.045 * xC1);
        double xSH = 1 + (0.015 * xC1);
        xDL /= WHT_L;
        xDC /= WHT_C * xSC;
        xDH /= WHT_H * xSH;
        DeltaE94 = Math.Sqrt(Math.Pow(xDL, (double)2) + Math.Pow(xDC, (double)2) + Math.Pow(xDH, (double)2));
        return DeltaE94;
    }

    public static double getDeltaE2000_viaCIELab(Color RGB1, Color RGB2)
    {
        //see www.easyrgb.com/index.php?X=DELT&H=05#text5
        double DeltaE2000 = 0;
        Color_CIELab CIELab1;
        Color_CIELab CIELab2;
        RGBtoCIELab(RGB1, out CIELab1);
        RGBtoCIELab(RGB2, out CIELab2);

        double WHT_L = 1.0;
        double WHT_C = 1.0;
        double WHT_H = 1.0;

        double xC1 = Math.Sqrt(Math.Pow(CIELab1.a, (double)2) + Math.Pow(CIELab1.b, (double)2));
        double xC2 = Math.Sqrt(Math.Pow(CIELab2.a, (double)2) + Math.Pow(CIELab2.b, (double)2));

        double xCX = (xC1 + xC2) / (double)2;
        double xGX = 0.5 * (1 - Math.Sqrt(Math.Pow(xCX, 7) / (Math.Pow(xCX, 7) + Math.Pow(25, 7))));
        double xNN = (1 + xGX) * CIELab1.a;
        xC1 = Math.Sqrt(xNN * xNN + CIELab1.b * CIELab1.b);
        double xH1 = CieLab2Hue(xNN, CIELab1.b);
        xNN = (1 + xGX) * CIELab2.a;
        xC2 = Math.Sqrt(xNN * xNN + CIELab2.b * CIELab2.b);
        double xH2 = CieLab2Hue(xNN, CIELab2.b);
        double xDL = CIELab2.L - CIELab1.L;
        double xDC = xC2 - xC1;
        double xDH = 0;
        if ((xC1 * xC2) == 0)
        {
            xDH = 0;
        }
        else
        {
            xNN = Math.Round(xH2 - xH1, 12);
            if (Math.Abs(xNN) <= 180)
            {
                xDH = xH2 - xH1;
            }
            else
            {
                if (xNN > 180) xDH = xH2 - xH1 - 360;
                else xDH = xH2 - xH1 + 360;
            }
        }
        xDH = 2 * Math.Sqrt(xC1 * xC2) * Math.Sin(Deg2Rad(xDH / 2));
        double xLX = (CIELab1.L + CIELab2.L) / 2;
        double xCY = (xC1 + xC2) / 2;
        double xHX = 0;
        if ((xC1 * xC2) == 0)
        {
            xHX = xH1 + xH2;
        }
        else
        {
            xNN = Math.Abs(Math.Round(xH1 - xH2, 12));
            if (xNN > 180)
            {
                if ((xH2 + xH1) < 360) xHX = xH1 + xH2 + 360;
                else xHX = xH1 + xH2 - 360;
            }
            else
            {
                xHX = xH1 + xH2;
            }
            xHX /= 2;
        }
        double xTX = 1 - 0.17 * Math.Cos(Deg2Rad(xHX - 30)) + 0.24
                       * Math.Cos(Deg2Rad(2 * xHX)) + 0.32
                       * Math.Cos(Deg2Rad(3 * xHX + 6)) - 0.20
                       * Math.Cos(Deg2Rad(4 * xHX - 63));
        double xPH = 30 * Math.Exp(-((xHX - 275) / 25) * ((xHX - 275) / 25));
        double xRC = 2 * Math.Sqrt(Math.Pow(xCY, 7) / (Math.Pow(xCY, 7) + Math.Pow(25, 7)));
        double xSL = 1 + ((0.015 * ((xLX - 50) * (xLX - 50)))
                / Math.Sqrt(20 + ((xLX - 50) * (xLX - 50))));
        double xSC = 1 + 0.045 * xCY;
        double xSH = 1 + 0.015 * xCY * xTX;
        double xRT = -Math.Sin(Deg2Rad(2 * xPH)) * xRC;
        xDL = xDL / (WHT_L * xSL);
        xDC = xDC / (WHT_C * xSC);
        xDH = xDH / (WHT_H * xSH);
        DeltaE2000 = Math.Sqrt(Math.Pow(xDL, 2) + Math.Pow(xDC, 2) + Math.Pow(xDH, 2) + xRT * xDC * xDH);
        return DeltaE2000;
    }

    //Function returns CIE-H° value
    public static double CieLab2Hue(double var_a, double var_b)
    {
        double var_bias = 0;
        if (var_a >= 0 && var_b == 0) return 0;
        if (var_a < 0 && var_b == 0) return 180;
        if (var_a == 0 && var_b > 0) return 90;
        if (var_a == 0 && var_b < 0) return 270;
        if (var_a > 0 && var_b > 0) var_bias = 0;
        if (var_a < 0) var_bias = 180;
        if (var_a > 0 && var_b < 0) var_bias = 360;
        return (Rad2Deg(Math.Atan(var_b / var_a)) + var_bias);
    }

    public static double Deg2Rad(double angle)
    {
        return Math.PI * angle / 180.0;
    }
    public static double Rad2Deg(double angle)
    {
        return angle * (180.0 / Math.PI);
    }


}
