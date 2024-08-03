using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace ImageShaper
{
    class ToolStrip_Item_Label_NUD : ToolStripControlHost
    {
        public float LabelWidth
        {
            get { return (Control as Custom_ToolStrip_UC_Label_NUD).LabelWidth; }
            set { (Control as Custom_ToolStrip_UC_Label_NUD).LabelWidth = value; }
        }

        public Size MenuItem_Size
        {
            get { return (Control as Custom_ToolStrip_UC_Label_NUD).MinimumSize; }
            set { (Control as Custom_ToolStrip_UC_Label_NUD).MinimumSize = value; }
        }

        public ToolStrip_Item_Label_NUD() : base(new Custom_ToolStrip_UC_Label_NUD()) { }

        public Custom_ToolStrip_UC_Label_NUD ToolStrip_UC_Label_NUD
        {
            get
            {
                return Control as Custom_ToolStrip_UC_Label_NUD;
            }
        }

    }

    class ToolStrip_Item_Label_TB : ToolStripControlHost
    {
        public float LabelWidth
        {
            get { return (Control as Custom_ToolStrip_UC_Label_TB).LabelWidth; }
            set { (Control as Custom_ToolStrip_UC_Label_TB).LabelWidth = value; }
        }

        public Size MenuItem_Size
        {
            get { return (Control as Custom_ToolStrip_UC_Label_TB).MinimumSize; }
            set { (Control as Custom_ToolStrip_UC_Label_TB).MinimumSize = value; }
        }

        public ToolStrip_Item_Label_TB() : base(new Custom_ToolStrip_UC_Label_TB()) { }

        public Custom_ToolStrip_UC_Label_TB ToolStrip_UC_Label_TB
        {
            get
            {
                return Control as Custom_ToolStrip_UC_Label_TB;
            }
        }

    }

    class ToolStrip_Item_FolderSelector : ToolStripControlHost
    {
        public float LabelWidth
        {
            get { return (Control as Custom_ToolStrip_UC_FolderSelector).LabelWidth; }
            set { (Control as Custom_ToolStrip_UC_FolderSelector).LabelWidth = value; }
        }

        public Size MenuItem_Size
        {
            get { return (Control as Custom_ToolStrip_UC_FolderSelector).MinimumSize; }
            set { (Control as Custom_ToolStrip_UC_FolderSelector).MinimumSize = value; }
        }

        public bool FileSelector
        {
            get { return (Control as Custom_ToolStrip_UC_FolderSelector).FileSelector; }
            set { (Control as Custom_ToolStrip_UC_FolderSelector).FileSelector = value; }
        }

        public ToolStrip_Item_FolderSelector() : base(new Custom_ToolStrip_UC_FolderSelector()) { }

        public Custom_ToolStrip_UC_FolderSelector ToolStrip_UC_FolderSelector
        {
            get
            {
                return Control as Custom_ToolStrip_UC_FolderSelector;
            }
        }

    }

}
