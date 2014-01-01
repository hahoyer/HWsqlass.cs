using System;
using System.Collections.Generic;
using System.Linq;
using Taabus.Data;
using Taabus.UserInterface;

namespace Taabus
{
    public interface ITaabusController
    {
        ExpansionDescription[] ExpansionDescriptions { get; set; }
        string[] Selection { get; set; }
        Server[] Servers { get; set; }
        WorkspaceItem[] WorkspaceItems { get; }
        string ProjectName { get; }
        void OnOpen();
        void Exit();
    }

    public class WorkspaceItem : IEquatable<WorkspaceItem>
    {
        bool IEquatable<WorkspaceItem>.Equals(WorkspaceItem other) { return false; }
    }

    interface IControlledItem : DragDropController.IItem
    {
        string Title { get; }
        long Count { get; }
        IEnumerable<IDataColumn> Columns { get; }
        IEnumerable<DataRecord> Data { get; }
    }

    interface IDataColumn
    {
        string Name { get; }
    }
}