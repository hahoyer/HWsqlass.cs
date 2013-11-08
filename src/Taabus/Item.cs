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
        public static TypeItem CreateType(DataBase parent, CompountType type, ReferenceItem[] references, int? keyIndex, int[][] uniques)
        {
            return new TypeItem
                (
                parent,
                type,
                references,
                keyIndex,
                uniques
                );
        }
        public static ReferenceItem CreateReference(DataBase parent, SQLSysViews.foreign_keysClass constraint, Func<SQLSysViews.all_objectsClass, TypeItem> getType) { return new ReferenceItem(parent, constraint, getType); }
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

        internal override string Name { get { return _name; } }
    }
}