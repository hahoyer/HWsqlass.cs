using System;
using System.Collections.Generic;
using System.Linq;
using Taabus.Data;
using Taabus.External;
using Taabus.UserInterface;

namespace Taabus
{
    public interface ITaabusController
    {
        ExpansionDescription[] ExpansionDescriptions { get; set; }
        string[] Selection { get; set; }
        Server[] Servers { get; set; }
        External.Item[] Items { get; set; }
        string ProjectName { get; }
        void OnOpen();
        void Exit();
    }

    interface IControlledItem : DragDropController.IItem
    {
        string Title { get; }
        long Count { get; }
        IEnumerable<IDataColumn> Columns { get; }
        IEnumerable<DataRecord> Data { get; }
        External.DataItem ToDataItem { get; }
    }

    interface IDataColumn
    {
        string Name { get; }
    }
}