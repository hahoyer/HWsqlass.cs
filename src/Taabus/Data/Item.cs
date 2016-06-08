using System;
using System.Collections.Generic;
using System.Linq;
using hw.DebugFormatter;
using hw.Helper;
using Taabus.MetaData;

namespace Taabus.Data
{
    abstract class Item : NamedObject
    {
        protected static MemberItem CreateMember(TypeItem parent, Member member)
        {
            return new MemberItem(parent, member);
        }

        [DisableDump]
        internal readonly DataBase Parent;
        readonly ValueCache<Item[]> _itemsCache;

        protected Item(DataBase parent, string name)
        {
            Parent = parent;
            Name = name;
            _itemsCache = new ValueCache<Item[]>(GetItems);
        }

        [DisableDump]
        internal Item[] Items => _itemsCache.Value;

        protected abstract Item[] GetItems();

        public override string Name { get; }
    }
}