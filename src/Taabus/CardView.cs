using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using MetroFramework.Controls;
using Taabus.UserInterface;

namespace Taabus
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
            Click += (s, e) => OnClick((MouseEventArgs) e);
            _itemFunctions = new[]
            {
                new UserInteraction("Count", OnGetCount, text: "get count")
            };
        }

        void OnGetCount() { Text = _item.Title + "(" + _item.Count +")"; }


        void OnClick(MouseEventArgs args)
        {
            var menu = new ContextMenuStrip();
            menu.Items.AddRange(_itemFunctions.Select(f => f.MenuItem).ToArray());
            menu.Show(this, args.X, args.Y);
        }
    }
}