using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using hw.Helper;
using MetroFramework.Controls;

namespace Taabus.UserInterface
{
    sealed class CardView : MetroButton, IWorkspaceItem
    {
        readonly UserInteraction[] _itemFunctions;
        readonly IControlledItem _item;
        readonly WorkspaceView _parent;

        internal CardView(WorkspaceView parent, IControlledItem item)
        {
            _item = item;
            _parent = parent;
            AutoSize = true;
            Text = item.Title;
            _itemFunctions = new[]
            {
                new UserInteraction("Count", OnGetCount, text: "count"),
                new UserInteraction("Table", OnShowTable, text: "table"),
            };

            ContextMenuStrip = CreateContextMenu();
        }

        IControlledItem IWorkspaceItem.Item { get { return _item; } }

        ContextMenuStrip CreateContextMenu()
        {
            var menu = new ContextMenuStrip();
            menu.Items.AddRange(_itemFunctions.Select(f => f.MenuItem).ToArray());
            return menu;
        }

        void OnShowTable() { _parent.CallAddTable(_item, new Rectangle(Location, Size)); }
        void OnGetCount() { Text = _item.Title + " " + _item.Count.Format3Digits(); }
    }
}