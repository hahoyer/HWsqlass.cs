using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using hw.Debug;
using hw.Helper;
using Taabus.External;

namespace Taabus.UserInterface
{
    sealed class Externalizer : DumpableObject, IExternalIdProvider
    {
        readonly IEnumerable<IDataItemContainer> _value;
        readonly FunctionCache<IDataItemContainer, Item> _map;
        readonly FunctionCache<Item, int> _ids;
        int _nextId;

        public Externalizer(IEnumerable<IDataItemContainer> value)
        {
            _map = new FunctionCache<IDataItemContainer, Item>(Create);
            _ids = new FunctionCache<Item, int>(Id);
            _value = value;
        }

        int IExternalIdProvider.Id(IDataItemContainer parent) { return _ids[_map[parent]]; }

        internal IEnumerable<Item> Execute() { return _value.Select(item => _map[item]).Select(SetId); }

        int Id(Item arg) { return _nextId++; }
        DataItem GetDataItem(IDataItemContainer item) { return item.Externalize(this); }

        Item SetId(Item item)
        {
            item.Id = _ids[item];
            return item;
        }

        Item Create(IDataItemContainer item)
        {
            var control = (Control) item;
            return new Item
            {
                Type = item.GetType().CompleteName(),
                X = control.Location.X,
                Y = control.Location.Y,
                Width = control.Size.Width,
                Height = control.Size.Height,
                Data = GetDataItem(item)
            };
        }
    }

    sealed class Internalizer
    {
        readonly IEnumerable<Item> _value;
        readonly FunctionCache<int, IReferenceableItem> _parents;
        public Internalizer(IEnumerable<Item> value, WorkspaceView workSpaceView)
        {
            _value = value;
            WorkSpaceView = workSpaceView;
            _parents = new FunctionCache<int, IReferenceableItem>(CreateNamed);
        }

        public readonly WorkspaceView WorkSpaceView;

        public IEnumerable<IDataItemContainer> Execute() { return _value.Select(Execute); }
        internal IControlledItem Execute(Link data) { return data.Internalize(this); }

        IDataItemContainer Execute(Item item)
        {
            if(item.Id == null)
                return Create(item);
            return _parents[item.Id.Value];
        }

        IReferenceableItem CreateNamed(int id) { return (IReferenceableItem) Create(_value.Single(v => v.Id == id)); }

        IDataItemContainer Create(Item item)
        {
            var constructor = item
                .Type
                .ResolveUniqueType()
                .GetConstructor<WorkspaceView, IControlledItem>();

            var result = ((IDataItemContainer) constructor.Invoke(new object[]
            {
                this,
                FindItem(item.Data)
            }));

            var control = (Control) result;
            control.Location = new Point(item.X, item.Y);
            control.Size = new Size(item.Width, item.Height);
            return result;
        }

        IControlledItem FindItem(DataItem data) { return data.Internalize(this); }

        internal IControlledItem Id(int value) { return _parents[value]; }
    }
}