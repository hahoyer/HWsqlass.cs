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
    public sealed class CardView : MetroButton, IControlledItem, IDataItemContainer 
    {
        readonly UserInteraction[] _itemFunctions;
        readonly IControlledItem _item;
        internal WorkspaceView Parent;

        internal CardView(IControlledItem item)
        {
            _item = item;
            AutoSize = true;
            Text = item.Title;
            _itemFunctions = new[]
            {
                new UserInteraction("Count", OnGetCount, text: "count"),
                new UserInteraction("Table", OnShowTable, text: "table"),
            };

            ContextMenuStrip = CreateContextMenu();
        }

        string IItem.Title { get { return _item.Title; } }
        long IItem.Count { get { return _item.Count; } }
        IEnumerable<IDataColumn> IColumnsAndDataProvider.Columns { get { return _item.Columns; } }
        IEnumerable<DataRecord> IColumnsAndDataProvider.Data { get { return _item.Data; } }
        External.DataItem IDataItemContainer.Externalize(IExternalIdProvider idProvider) { return new CardItem { Data = _item.Externalize(idProvider) }; }
        Link IControlledItem.Externalize(IExternalIdProvider idProvider) { return new Id {Value = idProvider.Id(this)}; }

        ContextMenuStrip CreateContextMenu()
        {
            var menu = new ContextMenuStrip();
            menu.Items.AddRange(_itemFunctions.Select(f => f.MenuItem).ToArray());
            return menu;
        }

        void OnShowTable() { Parent.CallAddTable(this, new Rectangle(Location, Size)); }
        void OnGetCount() { Text = _item.Title + " " + _item.Count.Format3Digits(); }

        internal interface IItem : IColumnsAndDataProvider
        {
            string Title { get; }
            long Count { get; }
        }
    }
}