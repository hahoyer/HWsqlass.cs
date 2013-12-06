using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using hw.Helper;
using MetroFramework.Controls;

namespace Taabus.UserInterface
{
    sealed class CardView : MetroButton
    {
        readonly UserInteraction[] _itemFunctions;
        readonly IControlledItem _item;
        readonly WorkspaceView _parent;
        public CardView(WorkspaceView parent, IControlledItem item)
        {
            _item = item;
            _parent = parent;
            AutoSize = true;
            Text = item.Title;
            _itemFunctions = new[]
            {
                new UserInteraction("Count", OnGetCount, text: "get count")
            };
            ContextMenuStrip = CreateContextMenu();
        }

        void OnGetCount() { Text = _item.Title + " " + _item.Count.Format3Digits(); }

        ContextMenuStrip CreateContextMenu()
        {
            var menu = new ContextMenuStrip();
            menu.Items.AddRange(_itemFunctions.Select(f => f.MenuItem).ToArray());
            return menu;
        }
    }
}