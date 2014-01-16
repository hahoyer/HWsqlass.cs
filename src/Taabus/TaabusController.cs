using System;
using System.Collections.Generic;
using System.Linq;
using Taabus.Data;
using Taabus.External;
using Taabus.UserInterface;

namespace Taabus
{
    interface ITaabusController
    {
        ExpansionDescription[] ExpansionDescriptions { get; set; }
        string[] Selection { get; set; }
        Server[] Servers { get; set; }
        External.Item[] Items { get; set; }
        string ProjectName { get; }
        void OnOpen();
        void Exit();
    }

    interface IControlledItem : DragDropController.IItem, TableView.IItem, CardView.IItem
    {
        Link Externalize(IExternalIdProvider idProvider);
    }

    interface IDataItemContainer
    {
        External.DataItem Externalize(IExternalIdProvider idProvider);
    }

    interface IExternalIdProvider
    {
        int Id(IDataItemContainer parent);
    }

    interface IDataColumn
    {
        string Name { get; }
    }

    internal interface IColumnsAndDataProvider
    {
        IEnumerable<IDataColumn> Columns { get; }
        IEnumerable<DataRecord> Data { get; }
    }
}