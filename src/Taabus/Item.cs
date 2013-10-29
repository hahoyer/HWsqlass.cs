#region Copyright (C) 2013

//     Project Taabus
//     Copyright (C) 2013 - 2013 Harald Hoyer
// 
//     This program is free software: you can redistribute it and/or modify
//     it under the terms of the GNU General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     This program is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU General Public License for more details.
// 
//     You should have received a copy of the GNU General Public License
//     along with this program.  If not, see <http://www.gnu.org/licenses/>.
//     
//     Comments, bugs and suggestions to hahoyer at yahoo.de

#endregion

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
        public static ReferenceItem CreateReference(DataBase parent, SQLSysViews.foreign_keysClass constraint, Func<SQLSysViews.all_objectsClass, TypeItem> getType)
        {
            return new ReferenceItem(parent, constraint, getType);
        }
        protected static MemberItem CreateMember(TypeItem parent, Member member) { return new MemberItem(parent, member); }

        [DisableDump]
        internal readonly DataBase Parent;
        readonly ValueCache<Item[]> _itemsCache;

        protected Item(DataBase parent, string name)
            : base(name)
        {
            Parent = parent;
            _itemsCache = new ValueCache<Item[]>(GetItems);
        }

        [DisableDump]
        internal Item[] Items { get { return _itemsCache.Value; } }

        protected abstract Item[] GetItems();
    }
}