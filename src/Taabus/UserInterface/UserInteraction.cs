using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using hw.Debug;
using hw.Helper;

namespace Taabus.UserInterface
{
    sealed class UserInteraction : DumpableObject
    {
        readonly string _name;
        readonly Action _handler;
        readonly Bitmap _bitmap;
        readonly string _text;
        readonly Func<bool> _getIsEnabled;
        readonly ValueCache<ToolStripButton> _buttonCache;
        readonly ValueCache<ToolStripMenuItem> _menuItemCache;

        internal UserInteraction(string name, Action handler,
            Bitmap bitmap = null,
            Func<bool> getIsEnabled = null,
            string text = null)
        {
            _name = name;
            _handler = handler;
            _bitmap = bitmap;
            _text = text;
            _getIsEnabled = getIsEnabled ?? (() => true);
            _buttonCache = new ValueCache<ToolStripButton>(GetButton);
            _menuItemCache = new ValueCache<ToolStripMenuItem>(GetMenuItem);
        }

        internal ToolStripItem Button { get { return _buttonCache.Value; } }

        ToolStripButton GetButton()
        {
            var result = new ToolStripButton
            {
                Name = _name,
                DisplayStyle = ToolStripItemDisplayStyle,
                Image = _bitmap,
                ImageTransparentColor = Color.Magenta,
                AutoSize = true,
                Text = _text
            };
            result.Click += (s, e) => _handler();
            return result;
        }

        ToolStripItemDisplayStyle ToolStripItemDisplayStyle { get
        {
            return _bitmap == null
                ? (_text == null ? ToolStripItemDisplayStyle.None : ToolStripItemDisplayStyle.Text)
                : (_text == null ? ToolStripItemDisplayStyle.Image : ToolStripItemDisplayStyle.ImageAndText);
        }
        }

        ToolStripMenuItem GetMenuItem()
        {
            var result = new ToolStripMenuItem
            {
                Name = _name,
                DisplayStyle = ToolStripItemDisplayStyle,
                Image = _bitmap,
                ImageTransparentColor = Color.Magenta,
                AutoSize = true,
                Text = _text
            };
            result.Click += (s, e) => _handler();
            return result;
        }

        public static readonly Size Size = new Size(32, 32);

        public ToolStripItem MenuItem { get { return _menuItemCache.Value; } }

        public void Refresh()
        {
            if(_buttonCache.IsValid)
                _buttonCache.Value.Enabled = _getIsEnabled();
            if(_menuItemCache.IsValid)
                _menuItemCache.Value.Enabled = _getIsEnabled();
        }
    }
}