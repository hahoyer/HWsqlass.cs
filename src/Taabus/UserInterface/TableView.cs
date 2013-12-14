using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using hw.Debug;
using hw.Forms;
using MetroFramework;
using MetroFramework.Controls;
using MetroFramework.Drawing;

namespace Taabus.UserInterface
{
    sealed class TableView : MetroPanel
    {
        static readonly Size _gripSize = new Size(5,5);
        readonly WorkspaceView _parent;
        readonly IControlledItem _item;

        readonly TableGridView _grid;

        internal TableView(WorkspaceView parent, IControlledItem item)
        {
            _parent = parent;
            _item = item;

            _grid = new TableGridView(_item);
            _grid.Anchor = AnchorStyles.Top | AnchorStyles.Left ;

            var p = new MetroPanel();
            p.Location = new Point(_gripSize);
            p.Size = Size - _gripSize - _gripSize;
            p.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            p.BorderStyle = MetroBorderStyle.FixedSingle;

            _grid.Dock = DockStyle.Fill;
            p.Controls.Add(_grid);
            p.Name = "TableContainer";

            Controls.Add(p);
            Name = "TableView";
            BorderStyle = MetroBorderStyle.FixedSingle;
        }
    }
}