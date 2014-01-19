using System;
using System.Collections.Generic;
using System.Linq;
using Taabus.UserInterface;

namespace Taabus.External
{
    public abstract class ItemData
    {
        internal abstract Internalizer.IItem Internalize(Internalizer internalizer);
    }

    [Serializer.Enable]
    public sealed class TypeItemView : ItemData
    {
        public TypeItem Data;
        internal override Internalizer.IItem Internalize(Internalizer controller)
        {
            return new UserInterface.TypeItemView(controller.Execute(Data), controller.WorkSpaceView)
            {};
        }
    }

    [Serializer.Enable]
    public sealed class TableView : ItemData
    {
        public Id Data;
        public ColumnConfig[] ColumnConfig;
        internal override Internalizer.IItem Internalize(Internalizer controller)
        {
            return new UserInterface.TableView(controller.Execute(Data), controller.WorkSpaceView)
            {
                ExternalColumnConfig = ColumnConfig
            };
        }
    }

    [Serializer.Enable]
    public sealed class ColumnConfig
    {
        public string Name;
        public int Width;
    }

    [Serializer.Enable]
    public sealed class Id
    {
        public int Value;
        internal IReferenceableItem Internalize(Internalizer internalizer) { return internalizer.Id(Value); }
    }
}