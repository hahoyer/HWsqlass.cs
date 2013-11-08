using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using hw.Debug;

namespace Taabus.UserInterface
{
    sealed class UserInteraction : DumpableObject
    {
        readonly Func<bool> _getIsEnabled;
        public readonly ToolStripItem Button;

        internal UserInteraction(string text, Action handler, Bitmap bitmap, Func<bool> getIsEnabled = null)
        {
            _getIsEnabled = getIsEnabled?? (()=>true);
            Button = new ToolStripButton
            {
                DisplayStyle = ToolStripItemDisplayStyle.Image,
                Image = bitmap,
                ImageTransparentColor = Color.Magenta,
                Size = Size,
                Text = text
            };
            Button.Click += (s, e) => handler();
        }
        
        public static readonly Size Size = new Size(32, 32);

        public void Refresh() { Button.Enabled = _getIsEnabled(); }
    }
}