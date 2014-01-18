using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using hw.Helper;
using Taabus.External;

namespace Taabus.UserInterface
{
    sealed class Internalizer
    {
        public readonly WorkspaceView WorkSpaceView;
        readonly IEnumerable<Item> _value;
        readonly FunctionCache<int, IReferenceableItem> _parents;

        internal Internalizer(IEnumerable<Item> value, WorkspaceView workSpaceView)
        {
            _value = value;
            WorkSpaceView = workSpaceView;
            _parents = new FunctionCache<int, IReferenceableItem>(CreateNamed);
        }

        internal IEnumerable<IItem> Execute() { return _value.Select(Execute); }
        internal TypeItemView.IItem Execute(TypeItem data) { return data.Internalize(this); }
        internal TableView.IItem Execute(Id data) { return data.Internalize(this); }

        IItem Execute(Item item)
        {
            if(item.Id == null)
                return Create(item);
            return _parents[item.Id.Value];
        }

        IReferenceableItem CreateNamed(int id) { return (IReferenceableItem) Create(_value.Single(v => v.Id == id)); }

        IItem Create(Item item)
        {
            var result = FindItem(item.Data);
            var control = (Control) result;
            control.Location = new Point(item.X, item.Y);
            control.Size = new Size(item.Width, item.Height);
            return result;
        }

        IItem FindItem(ItemData data) { return data.Internalize(this); }

        internal IReferenceableItem Id(int value) { return _parents[value]; }

        public interface IItem
        {
            ItemData Convert(Externalizer externalizer);
        }
    }

    interface IExternalizeable<out T>
    {
        T Convert(Externalizer provider);
    }

    interface IReferenceableItem : TableView.IItem, Internalizer.IItem
    {}
}