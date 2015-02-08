using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using JetBrains.Annotations;
using MetroFramework.Controls;

namespace Taabus.UserInterface
{
    [UsedImplicitly]
    public sealed class FilterView
        : MetroPanel
            , DragDropController.ISource
    {
        readonly WorkspaceView _parent;
        readonly IItem _item;

        readonly MetroButton _header;
        readonly  _condition;


        internal FilterView(IItem item, WorkspaceView parent)
        {
            _item = item;
            _parent = parent;

            _header = new MetroButton
            {
                AutoSize = true,
                Text = _item.Title,
                Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top
            };
            Controls.Add(_header);
        }

        Control DragDropController.ISource.Control { get { throw new NotImplementedException(); } }
        DragDropController.IItem DragDropController.ISource.GetItemAt(Point point) { throw new NotImplementedException(); }
        Size DragDropController.ISource.GetDisplacementAt(Point point) { throw new NotImplementedException(); }

        internal interface IItem
        {
            string Title { get; }
        }

        public void LoadData() { throw new NotImplementedException(); }
        public void OnAdded() { throw new NotImplementedException(); }
    }
}