using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ImageShaper
{
    public partial class Form_DockPreview : Form
    {
        public Form_DockPreview()
        {
            InitializeComponent();
            this.uC_ImageCanvas1.KeyDown += new KeyEventHandler(uC_ImageCanvas1_KeyDown);
        }

        void uC_ImageCanvas1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && (e.KeyCode == Keys.C))
                Clipboard.SetImage(this.uC_ImageCanvas1.Image);
        }

        internal void SetImage(Bitmap bitmap)
        {
            this.uC_ImageCanvas1.SetImage(bitmap);
        }

        internal void SetBackgroundImage(Image image)
        {
            this.uC_ImageCanvas1.SetBackgroundImage(image);
        }
    }
}
