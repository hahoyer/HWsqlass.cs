using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using hw.Debug;
using hw.Helper;
using Taabus.External;

namespace Taabus.UserInterface
{
    sealed class Externalizer : DumpableObject
    {
        readonly IEnumerable<Internalizer.IItem> _value;
        readonly FunctionCache<Internalizer.IItem, Item> _map;
        readonly FunctionCache<Item, int> _ids;
        int _nextId;

        public Externalizer(IEnumerable<Internalizer.IItem> value)
        {
            _map = new FunctionCache<Internalizer.IItem, Item>(Create);
            _ids = new FunctionCache<Item, int>(Id);
            _value = value;
        }

        internal int Id(Internalizer.IItem parent) { return _ids[_map[parent]]; }

        internal IEnumerable<Item> Execute()
        {
            return _value
                .Select(item => _map[item])
                .ToArray()
                .Select(SetId);
        }

        int Id(Item arg) { return _nextId++; }
        ItemData GetItemData(Internalizer.IItem item) { return item.Convert(this); }

        Item SetId(Item item)
        {
            if(_ids.IsValid(item))
                item.Id = _ids[item];
            return item;
        }

        Item Create(Internalizer.IItem item)
        {
            var control = (Control) item;
            return new Item
            {
                X = control.Location.X,
                Y = control.Location.Y,
                Width = control.Size.Width,
                Height = control.Size.Height,
                Data = GetItemData(item)
            };
        }

    }
}