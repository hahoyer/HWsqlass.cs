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

using hw.Debug;
using hw.Forms;
using Taabus.MetaData;

namespace Taabus.Data
{
    sealed class MemberItem : Item, IIconKeyProvider, IDataColumn
    {
        [Node]
        public readonly BasicType Type;
        public MemberItem(TypeItem parent, Member metaData)
            : base(parent.Parent, metaData.Name) { Type = metaData.Type; }

        [DisableDump]
        public Field Field { get { return new Field {Name = Name, Type = Type}; } }

        protected override Item[] GetItems() { return null; }
        string IIconKeyProvider.IconKey { get { return Type.GetIconKey(); } }
        string IDataColumn.Name{ get { return Name; } }
    }
}