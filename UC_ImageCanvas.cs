using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace ImageShaper
{
    public partial class UC_ImageCanvas : UserControl
    {
        public class ImageCanvasDataEventArgs : EventArgs
        {
            public int Color;
        }
        public event EventHandler<ImageCanvasDataEventArgs> PixelColorChanged;
        // The method which fires the Event
        protected void OnPixelColorChanged(object sender, ImageCanvasDataEventArgs e)
        {
            var target = PixelColorChanged;
            if (target != null)
            {
                target(this, e);
            }
        }

        public event EventHandler<EventArgs> ZoomLevelChanged;
        // The method which fires the Event
        protected void OnZoomLevelChanged(object sender, EventArgs e)
        {
            var target = ZoomLevelChanged;
            if (target != null)
            {
                target(this, e);
            }
        }

        public event EventHandler<LocationEventArgs> PixelClicked;
        // The method which fires the Event
        protected void OnPixelClicked(object sender, LocationEventArgs e)
        {
            var target = PixelClicked;
            if (target != null)
            {
                target(this, e);
            }
        }

        #region jitter free control redrawing (not used currently)
        private const int WM_SETREDRAW      = 0x000B;
        private const int WM_USER           = 0x400;
        private const int EM_GETEVENTMASK   = (WM_USER + 59);
        private const int EM_SETEVENTMASK   = (WM_USER + 69);

        [System.Runtime.InteropServices.DllImport("user32", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        private extern static IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, IntPtr lParam);

        IntPtr eventMask = IntPtr.Zero;

        private int drawStopCount = 0;
        public void StopDrawing()
        {
            if (drawStopCount == 0)
            {
                // Stop redrawing:
                SendMessage(this.Handle, WM_SETREDRAW, 0, IntPtr.Zero);
                // Stop sending of events:
                eventMask = SendMessage(this.Handle, EM_GETEVENTMASK, 0, IntPtr.Zero);
            }
            drawStopCount++;
        }

        public void StartDrawing()
        {
            drawStopCount--;
            if (drawStopCount == 0)
            {
                // turn on events
                SendMessage(this.Handle, EM_SETEVENTMASK, 0, eventMask);

                // turn on redrawing
                SendMessage(this.Handle, WM_SETREDRAW, 1, IntPtr.Zero);

                Invalidate();
                Refresh();
            }
        }
        #endregion

        private const int panel_border_size = 10;

        private const double MinimumZoom = 1;
        private const double MaximumZoom = 10;
        private double _zoomLevel = 1;
        public double ZoomLevel
        {
            get { return _zoomLevel; }
            set
            {
                _zoomLevel = value;
                UpdateZoomedPicture(true);
            }
        }

        private bool _LocationFromCenter = false;
        public bool LocationFromCenter
        {
            get { return _LocationFromCenter; }
            set { _LocationFromCenter = value; }
        }

        private bool _ShowColorInfo = true;
        public bool ShowColorInfo
        {
            get { return _ShowColorInfo; }
            set { _ShowColorInfo = value; }
        }

        private bool _ShowCenterCross = false;
        public bool ShowCenterCross
        {
            get { return _ShowCenterCross; }
            set { _ShowCenterCross = value; }
        }

        private Color _PreviewPixelColor = Color.FromArgb(255, 255, 255, 255);

        private Point[] _PreviewPixel;
        public Point[] PreviewPixel
        {
            get { return _PreviewPixel; }
            set
            {
                _PreviewPixel = value;
                UpdateZoomedPicture(true);
            }
        }

        private bool _CaptureFocusOnMouseOver = true;
        public bool CaptureFocusOnMouseOver
        {
            get { return _CaptureFocusOnMouseOver; }
            set
            {
                _CaptureFocusOnMouseOver = value;
                this.label_MouseCoord.Text = "[x" + (0).ToString("000") + " , y" + (0).ToString("000") + "]";
                if (_CaptureFocusOnMouseOver)
                    this.label_MouseCoord.Text += " Zoom=" + ZoomLevel.ToString();
            }
        }

        public UC_ImageCanvas()
        {
            InitializeComponent();
            this.panel_Canvas.Padding = new Padding(panel_border_size);
            this.pictureBox_Canvas.SizeChanged += new EventHandler(Canvas_SizeChanged);
            this.panel_Canvas.SizeChanged += new EventHandler(Canvas_SizeChanged);
            this.pictureBox_Canvas.MouseMove += new MouseEventHandler(pictureBox_Canvas_MouseMove);

            this.panel_Canvas.MouseEnter += new EventHandler(Canvas_MouseEnter);
            this.pictureBox_Canvas.MouseEnter += new EventHandler(Canvas_MouseEnter);
            this.pictureBox_Canvas.MouseClick += new MouseEventHandler(pictureBox_Canvas_MouseClick);
            this.MouseEnter += new EventHandler(Canvas_MouseEnter);
            this.MouseWheel += new MouseEventHandler(Canvas_MouseWheel);

            this.label_MouseCoord.Text = "[x" + (0).ToString("000") + " , y" + (0).ToString("000") + "]";
            if (_CaptureFocusOnMouseOver)
                this.label_MouseCoord.Text += " Zoom=" + ZoomLevel.ToString();
        }

        void pictureBox_Canvas_MouseClick(object sender, MouseEventArgs e)
        {
            Point p = new Point((int)Math.Truncate(e.X / _zoomLevel), (int)Math.Truncate(e.Y / _zoomLevel));
            if ((_LocationFromCenter) && (normalImage != null))
                p = new Point(p.X - Center.X, p.Y - Center.Y);
            OnPixelClicked(sender, new LocationEventArgs() { Location = p });
        }

        void Canvas_MouseEnter(object sender, EventArgs e)
        {
            if (_CaptureFocusOnMouseOver)
                this.Focus();
        }

        public Image Image
        {
            get
            {
                return this.pictureBox_Canvas.Image;
            }
        }


        public void SetImage(Image image)
        {
            this.normalImage = image;
            this.pictureBox_Canvas.Image = null;
            UpdateZoomedPicture(true);
        }

        public void SetBackgroundImage(Image image)
        {
            this.BackgroundImage = image;
            UpdateZoomedPicture(true);
        }

        private Point _CenterOffset = new Point(0, 0);
        public Point CenterOffset
        {
            get { return _CenterOffset; }
            set
            {
                _CenterOffset = value;
                UpdateZoomedPicture(true);
            }
        }

        /// <summary>
        /// the center point of the normalImage
        /// </summary>
        private Point Center
        {
            get
            {
                //SHPBuilder shows center with Point((normalImage.Width -1) / 2, (normalImage.Height -1) / 2);

                //the game however works like this
                if (normalImage != null)
                    return new Point(normalImage.Width / 2 + CenterOffset.X, normalImage.Height / 2 + CenterOffset.Y);
                return new Point(-1, -1);
            }
        }

        private Image normalImage;
        void Canvas_MouseWheel(object sender, MouseEventArgs e)
        {
            if ((Cursor.Position.X > PointToScreen(this.panel_Canvas.Location).X)
             && (Cursor.Position.Y > PointToScreen(this.panel_Canvas.Location).Y)
             && (Cursor.Position.X < PointToScreen(this.panel_Canvas.Location).X + this.panel_Canvas.Width)
             && (Cursor.Position.Y < PointToScreen(this.panel_Canvas.Location).Y + this.panel_Canvas.Height))
            {
                if (e.Delta > 0) _zoomLevel += 1;
                if (e.Delta < 0) _zoomLevel -= 1;
                if (_zoomLevel < MinimumZoom) _zoomLevel = MinimumZoom;
                if (_zoomLevel > MaximumZoom) _zoomLevel = MaximumZoom;
                ZoomLevel = _zoomLevel;
                OnZoomLevelChanged(this, new EventArgs());
                this.label_MouseCoord.Text = "Zoom=" + ZoomLevel.ToString();
            }
        }

        void Canvas_SizeChanged(object sender, EventArgs e)
        {
            UpdateZoomedPicture();
        }

        public void UpdateZoomedPicture()
        {
            UpdateZoomedPicture(false);
        }

        public void UpdateZoomedPicture(bool ForceUpdate)
        {
            this.label_Color.Visible = _ShowColorInfo;

            if (normalImage != null)
            {
                Point loc = new Point(0, 0);
                Size sz;
                if ((this.pictureBox_Canvas.Image == null)
                  ||(normalImage.Width * _zoomLevel != this.pictureBox_Canvas.Image.Width)
                  || ForceUpdate)
                {
                    if (_zoomLevel != 1)
                    {
                        sz = new Size((int)(normalImage.Width * _zoomLevel), (int)(normalImage.Height * _zoomLevel));
                    }
                    else
                    {
                        sz = normalImage.Size;
                    }
                    Rectangle rectSrc = new Rectangle(loc, sz);
                    // now draw the rect of the source picture in the entire client rect of MyPictureBox
                    Bitmap zoomedImage = new Bitmap(sz.Width, sz.Height, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
                    Graphics zoomedImage_g = Graphics.FromImage(zoomedImage);
                    zoomedImage_g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                    zoomedImage_g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
                    zoomedImage_g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;

                    //stupid DrawImage can't handle Alpha channel colors in the palette of an 8bppIndexed Image
                    //so first change the image back into a 32bit ARGB bitmap and then draw this thing
                    Bitmap correctAlpha = new Bitmap(normalImage.Width, normalImage.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                    if (this.BackgroundImage != null)
                    {
                        zoomedImage_g.DrawImage(this.BackgroundImage, new Rectangle(0, 0, (int)(this.BackgroundImage.Width * _zoomLevel), (int)(this.BackgroundImage.Height * _zoomLevel)));
                        //clone the normalImage and create a version where color #0 is transparent, so the BackgroundImage is visible through these pixel
                        Image normalImage_noBack = (Image)normalImage.Clone();
                        System.Drawing.Imaging.ColorPalette cp = normalImage_noBack.Palette;
                        cp.Entries[0] = Color.FromArgb(0, 0, 0, 0);
                        normalImage_noBack.Palette = cp;
                        using (Graphics gr = Graphics.FromImage(correctAlpha))
                            gr.DrawImage(normalImage_noBack, new Point(0, 0));
                    }
                    else
                        using (Graphics gr = Graphics.FromImage(correctAlpha))
                            gr.DrawImage(normalImage, new Point(0, 0));

                    if (ShowCenterCross)
                    {
                        Pen crosslinepen = new Pen(Color.FromArgb(100, 255, 255, 255));
                        using (Graphics gr = Graphics.FromImage(correctAlpha))
                        {
                            gr.DrawLine(crosslinepen, new Point(Center.X, 0), new Point(Center.X, correctAlpha.Height));
                            gr.DrawLine(crosslinepen, new Point(0, Center.Y), new Point(correctAlpha.Width, Center.Y));
                        }
                    }
                    if (_PreviewPixel != null)
                        foreach (Point p in _PreviewPixel)
                            using (Graphics gr = Graphics.FromImage(correctAlpha))
                                gr.FillRectangle(new SolidBrush(_PreviewPixelColor), p.X + (ShowCenterCross ? Center.X : 0), p.Y + (ShowCenterCross ? Center.Y : 0), 1, 1);

                    zoomedImage_g.DrawImage(correctAlpha, new Rectangle(0, 0, sz.Width, sz.Height));
                    
                    zoomedImage_g.Dispose();
                    this.pictureBox_Canvas.Image = zoomedImage;
                    this.pictureBox_Canvas.Size = zoomedImage.Size;
                }
            }
            AdjustPosition();
        }

        /// <summary>
        /// this adjusts the position of the picturebox in the panel and the info label
        /// it also adds a small nonfunctional border (panel_border_size) around the image, so the image isn't going directly to the usercontrol-bounds
        /// </summary>
        private void AdjustPosition()
        {
            if (this.pictureBox_Canvas.Image != null)
            {
                this.pictureBox_Canvas.MinimumSize = this.pictureBox_Canvas.Image.Size;
                this.pictureBox_Canvas.MaximumSize = this.pictureBox_Canvas.Image.Size;
                int w = (this.panel_Canvas.Width + 2 * panel_border_size - this.pictureBox_Canvas.Width) / 2;
                int h = (this.panel_Canvas.Height + 2 * panel_border_size - this.pictureBox_Canvas.Height) / 2;
                if (w < panel_border_size) w = panel_border_size;
                if (h < panel_border_size) h = panel_border_size;
                this.panel_Canvas.Padding = new Padding(w, h, w, h);
            }
            else
            {
                this.pictureBox_Canvas.MinimumSize = new Size(1, 1);
                this.pictureBox_Canvas.MaximumSize = new Size(1, 1);
            }

            //this adds a border to the bottom right and lets the scrollbars appear on the panel_canvas
            this.control_PanelBorderDummy.Location = new Point(this.pictureBox_Canvas.Location.X + this.pictureBox_Canvas.Width + panel_border_size,
                                                             this.pictureBox_Canvas.Location.Y + this.pictureBox_Canvas.Height + panel_border_size);
            this.control_PanelBorderDummy.Width = 0;
            this.control_PanelBorderDummy.Height = 0;

            int my = 0;
            if (panel_Canvas.HorizontalScroll.Visible) my = this.panel_Canvas.Height - 16 - this.label_MouseCoord.Height;
            else my = this.panel_Canvas.Height - this.label_MouseCoord.Height;
            this.label_MouseCoord.Location = new Point(5, my);

            //refresh the special canvas borders
            this.panel_Canvas.Refresh();
        }

        int PaletteColorUnderMouse = 0;
        void pictureBox_Canvas_MouseMove(object sender, MouseEventArgs e)
        {            
            Point p = new Point((int)Math.Truncate(e.X / _zoomLevel), (int)Math.Truncate(e.Y / _zoomLevel));

            if ((_LocationFromCenter) && (normalImage != null))
                this.label_MouseCoord.Text = "[x" + (p.X - Center.X).ToString("000") + " , y" + (p.Y - Center.Y).ToString("000") + "]";
            else
                this.label_MouseCoord.Text = "[x" + (p.X).ToString("000") + " , y" + (p.Y).ToString("000") + "]";

            //Zoom via MouseWheel works only when the control has focus (which panels and pictureboxes usualy can't have). It only gets focus when CaptureFocusOnMouseOver is true
            if (_CaptureFocusOnMouseOver)
                this.label_MouseCoord.Text += " Zoom=" + ZoomLevel.ToString();


            this.label_Color.Visible = _ShowColorInfo;

            if (normalImage != null) 
            {
                if ((p.X < normalImage.Width) && (p.Y < normalImage.Height))
                    if (normalImage.PixelFormat != System.Drawing.Imaging.PixelFormat.Format8bppIndexed)
                    {
                        Bitmap tmp = (Bitmap)normalImage;
                        Color c = tmp.GetPixel(p.X, p.Y);
                        this.label_Color.Text = "RGB=[" + c.R.ToString("000") + "," + c.G.ToString("000") + "," + c.B.ToString("000") + "]";
                    }
                    else
                    {
                        int c = CConverter.GetPixel(p.X, p.Y, normalImage);
                        if (c != PaletteColorUnderMouse)
                        {
                            ImageCanvasDataEventArgs ne = new ImageCanvasDataEventArgs();
                            ne.Color = c;
                            PaletteColorUnderMouse = c;
                            OnPixelColorChanged(this, ne);
                        }
                        this.label_Color.Text = "Color=" + c;
                    }
            }
        }

    }

    /// <summary>
    /// this adds a small border around the main drawing canvas (pictureBox_canvas)
    /// </summary>
    class EnhPanel : Panel
    {
        //if this is enabled, drawline is not always done
        //stop the stupid flicker of the OnPaint stuff when resizing the control
        //protected override CreateParams CreateParams
        //{
        //    get
        //    {
        //        CreateParams cp = base.CreateParams;
        //        cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED
        //        return cp;
        //    }
        //} 
        
        public int drawBorderAroundControlID = 0;
        public EnhPanel()
            : base()
        {
            this.DoubleBuffered = true; //works better than CreateParams
            this.ResizeRedraw = true;
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if ((this.Controls != null) && (drawBorderAroundControlID >= 0) && (drawBorderAroundControlID < this.Controls.Count))
            {
                if (((PictureBox)this.Controls[drawBorderAroundControlID]).Image != null)
                {
                    Rectangle controlbounds = new Rectangle();
                    controlbounds.X = this.Controls[drawBorderAroundControlID].Location.X - 1;
                    controlbounds.Y = this.Controls[drawBorderAroundControlID].Location.Y - 1;
                    controlbounds.Width = this.Controls[drawBorderAroundControlID].Width + 1;
                    controlbounds.Height = this.Controls[drawBorderAroundControlID].Height + 1;
                    Point topleft = new Point(controlbounds.X, controlbounds.Y);
                    Point topright = new Point(controlbounds.X + controlbounds.Width, controlbounds.Y);
                    Point bottomleft = new Point(controlbounds.X, controlbounds.Y + controlbounds.Height);
                    Point bottomright = new Point(controlbounds.X + controlbounds.Width, controlbounds.Y + controlbounds.Height);

                    int linelength = controlbounds.Width / 10;
                    e.Graphics.DrawLine(Pens.Black, topleft, new Point(topleft.X + linelength, topleft.Y));
                    e.Graphics.DrawLine(Pens.Black, topleft, new Point(topleft.X, topleft.Y + linelength));
                    e.Graphics.DrawLine(Pens.Black, topright, new Point(topright.X - linelength, topright.Y));
                    e.Graphics.DrawLine(Pens.Black, topright, new Point(topright.X, topright.Y + linelength));

                    e.Graphics.DrawLine(Pens.Black, bottomleft, new Point(bottomleft.X + linelength, bottomleft.Y));
                    e.Graphics.DrawLine(Pens.Black, bottomleft, new Point(bottomleft.X, bottomleft.Y - linelength));
                    e.Graphics.DrawLine(Pens.Black, bottomright, new Point(bottomright.X - linelength, bottomright.Y));
                    e.Graphics.DrawLine(Pens.Black, bottomright, new Point(bottomright.X, bottomright.Y - linelength));
                    //e.Graphics.DrawRectangle(Pens.Black, controlbounds);
                }
            }
        }
    }
}
