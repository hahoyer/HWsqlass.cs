#region Copyright (C) 2013

//     Project Taabus
//     Copyright (C) 2013 - 2013 Harald Hoyer
// 
//     Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE
//     
//     Comments, bugs and suggestions to hahoyer at yahoo.de

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using hw.Forms;
using hw.Helper;
using Taabus.MetaData;

namespace Taabus
{
    sealed class ReferenceItem : Item
    {
        [Node]
        internal readonly string[] Columns;
        [Node]
        internal readonly string[] TargetColumns;
        readonly ValueCache<TypeItem> _targetTypeCache;

        public ReferenceItem(DataBase parent, SQLSysViews.foreign_keysClass constraint, Func<SQLSysViews.all_objectsClass, TypeItem> getType)
            : base(parent, constraint.name)
        {
            var columns = constraint.Columns.OrderBy(c => c.constraint_column_id).ToArray();
            Columns = columns.Select(c => c.ParentColumn.name).ToArray();
            TargetColumns = columns.Select(c => c.ReferenceColumn.name).ToArray();
            _targetTypeCache = new ValueCache<TypeItem>(() => getType(constraint.Reference));
        }

        [Node]
        public TypeItem TargetType { get { return _targetTypeCache.Value; } }

        protected override Item[] GetItems() { return null; }
        protected override string GetNodeDump() { return "->" + TargetType.Name; }
    }
}