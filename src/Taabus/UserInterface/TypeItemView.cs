using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using hw.Helper;
using JetBrains.Annotations;
using MetroFramework.Controls;
using Taabus.Data;
using Taabus.External;

namespace Taabus.UserInterface
{
    [UsedImplicitly]
    public sealed class TypeItemView
        : MetroButton
            , IReferenceableItem
            , WorkspaceView.IControl
            , FilterView.IItem
    {
        readonly UserInteraction[] _itemFunctions;
        readonly IItem _item;
        readonly WorkspaceView _parent;

        internal TypeItemView(IItem item, WorkspaceView parent)
        {
            _item = item;
            _parent = parent;
            AutoSize = true;
            Text = _item.Title;
            _itemFunctions = new[]
            {
                new UserInteraction("Count", OnGetCount, text: "count"),
                new UserInteraction("Filter", OnFilter, text: "filter"),
                new UserInteraction("Table", OnShowTable, text: "table"),
            };
            ContextMenuStrip = CreateContextMenu();
        }

        IEnumerable<IDataColumn> IColumnsAndDataProvider.Columns { get { return _item.Columns; } }
        IEnumerable<DataRecord> IColumnsAndDataProvider.Data { get { return _item.Data; } }
        ItemData Internalizer.IItem.Convert(Externalizer externalizer)
        {
            var typeItem = _item.Convert(externalizer);
            return new External.TypeItemView
            {
                Data = typeItem
            };
        }
        Id IExternalizeable<Id>.Convert(Externalizer externalizer) { return new Id {Value = externalizer.Id(this)}; }
        Control DragDropController.ISource.Control { get { return this; } }
        DragDropController.IItem DragDropController.ISource.GetItemAt(Point point) { return this; }
        Size DragDropController.ISource.GetDisplacementAt(Point point) { return new Size(point); }
        string TableView.IItem.Title { get { return _item.Title; } }
        string FilterView.IItem.Title { get { return _item.Title; } }

        ContextMenuStrip CreateContextMenu()
        {
            var menu = new ContextMenuStrip();
            menu.Items.AddRange(_itemFunctions.Select(f => f.MenuItem).ToArray());
            return menu;
        }

        void OnFilter() { _parent.CallAddFilter(this, new Rectangle(Location, Size)); }
        void OnShowTable() { _parent.CallAddTable(this, new Rectangle(Location, Size)); }
        void OnGetCount() { Text = _item.Title + " " + _item.Count.Format3Digits(); }

        internal interface IItem : IColumnsAndDataProvider, IExternalizeable<External.TypeItem>
        {
            string Title { get; }
            long Count { get; }
        }

    }
}