using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using hw.Helper;
using Taabus.External;

namespace Taabus.UserInterface
{
    sealed class Externalizer
    {
        readonly IEnumerable<IDataItemContainer> _value;
        readonly FunctionCache<IDataItemContainer, Item> _parents;
        int _nextId;

        public Externalizer(IEnumerable<IDataItemContainer> value)
        {
            _parents = new FunctionCache<IDataItemContainer, Item>(Create);
            _value = value;
        }
        public IEnumerable<Item> Execute() { return _value.Select(item => _parents[item]); }

        Item Create(IDataItemContainer item)
        {
            var control = (Control)item;
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

        DataItem GetDataItem(IDataItemContainer item)
        {
            IChildItem c = item.Child;
            var tableItemOrDefault = c.ToTableItemOrDefault;
            if(tableItemOrDefault != null)
                return new DataItem {TableItem = tableItemOrDefault};

            var result = _parents[(IDataItemContainer)c];
            result.Id = _nextId++;
            return new DataItem {Id = result.Id};
        }
    }

    sealed class Internalizer
    {
        readonly IEnumerable<Item> _value;
        readonly FunctionCache<int, IReferenceableItem> _parents;
        readonly ITaabusController _controller;
        public Internalizer(IEnumerable<Item> value, ITaabusController controller)
        {
            _value = value;
            _controller = controller;
            _parents = new FunctionCache<int, IReferenceableItem>(CreateNamed);
        }

        public IEnumerable<IDataItemContainer> Execute() { return _value.Select(Execute); }

        IDataItemContainer Execute(Item item)
        {
            if(item.Id == null)
                return Create(item);
            return _parents[item.Id.Value];
        }

        IReferenceableItem CreateNamed(int id) { return (IReferenceableItem)Create(_value.Single(v => v.Id == id)); }

        IDataItemContainer Create(Item item)
        {
            var constructor = item
                .Type
                .ResolveUniqueType()
                .GetConstructor<WorkspaceView, IControlledItem>();

            var result = ((IDataItemContainer)constructor.Invoke(new object[]
            {
                this,
                FindItem(item.Data)
            }));

            var control = (Control)result;
            control.Location = new Point(item.X, item.Y);
            control.Size = new Size(item.Width, item.Height);
            return result;
        }

        IControlledItem FindItem(DataItem data)
        {
            if(data.Id == null)
                return data.TableItem.ToControlledItem(_controller);
            return _parents[data.Id.Value];
        }

    }
}