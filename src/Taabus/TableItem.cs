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
using hw.Forms;
using hw.Helper;

namespace Taabus
{
    sealed class TypeItem : Item
    {
        internal readonly MetaData.Type Type;
        [Node]
        internal readonly ReferenceItem[] References;
        readonly ValueCache<MemberItem[]> _membersCache;

        public TypeItem(DataBase parent, MetaData.Type type, ReferenceItem[] references)
            : base(parent, type.Name)
        {
            _membersCache = new ValueCache<MemberItem[]>(GetMembers);
            Type = type;
            References = references;
        }

        [Node]
        [EnableDumpExcept(null)]
        internal MemberItem[] Members { get { return _membersCache.Value; } }

        protected override Item[] GetItems() { return GetMembers().Cast<Item>().ToArray(); }

        MemberItem[] GetMembers()
        {
            var members = Type
                .Members
                .ToArray();
            return members
                .Select(metaData => CreateMember(this, metaData))
                .ToArray();
        }
    }
}