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
using Taabus.MetaData;

namespace Taabus
{
    sealed class ReferenceItem : Item
    {
        internal string[] Columns;
        internal string[] TargetColumns;
        readonly Func<CompountType, TypeItem> _getType;
        readonly CompountType _type;

        public ReferenceItem(DataBase parent, ForeignKeyConstraint constraint, Func<CompountType, TypeItem> getType)
            : base(parent, constraint.Name)
        {
            _getType = getType;
            Columns = constraint.ColumnNames;
            var target = constraint.Target as KeyConstraint;
            Tracer.Assert(target != null);
            Tracer.Assert(target.IsPrimaryKey);
            _type = target.Type;
            TargetColumns = target.ColumnNames;
            Tracer.Assert(Columns.Length == TargetColumns.Length);
        }

        public TypeItem TargetType { get { return _getType(_type); } }

        protected override Item[] GetItems() { return null; }
    }
}