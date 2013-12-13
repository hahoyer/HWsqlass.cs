using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using MetroFramework.Controls;

namespace Taabus.UserInterface
{
    sealed class TableView : MetroPanel
    {
        readonly WorkspaceView _parent;
        readonly IControlledItem _item;

        readonly DataGrid _grid;

        internal TableView(WorkspaceView parent, IControlledItem item)
        {
            _parent = parent;
            _item = item;

            _grid = new DataGrid();
            _grid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            Controls.Add(_grid);
        }

    }
}