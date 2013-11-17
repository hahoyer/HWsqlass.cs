using System;
using System.Collections.Generic;
using System.Linq;
using hw.Debug;
using hw.Helper;
using Taabus.MetaData;

namespace Taabus
{
    abstract class Item : NamedObject
    {
        protected static MemberItem CreateMember(TypeItem parent, Member member) { return new MemberItem(parent, member); }

        [DisableDump]
        internal readonly DataBase Parent;
        readonly ValueCache<Item[]> _itemsCache;
        readonly string _name;

        protected Item(DataBase parent, string name)
        {
            Parent = parent;
            _name = name;
            _itemsCache = new ValueCache<Item[]>(GetItems);
        }

        [DisableDump]
        internal Item[] Items { get { return _itemsCache.Value; } }

        protected abstract Item[] GetItems();

        public override string Name { get { return _name; } }
    }
}