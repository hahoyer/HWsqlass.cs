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
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using hw.Debug;
using hw.Forms;
using hw.Helper;
using JetBrains.Annotations;
using Taabus.MetaData;

namespace Taabus
{
    sealed class TypeItem : Item
    {
        [DisableDump]
        internal readonly CompountType Type;
        [DisableDump]
        internal readonly TypeQuery Data;
        [Node]
        internal readonly ReferenceItem[] References;
        readonly ValueCache<MemberItem[]> _membersCache;

        public TypeItem(DataBase parent, CompountType type, ReferenceItem[] references)
            : base(parent, type.Name)
        {
            _membersCache = new ValueCache<MemberItem[]>(GetMembers);
            Type = type;
            References = references;
            Data = new TypeQuery(parent.Parent, Parent.Name + "." + Type.FullName);
        }

        [Node]
        [EnableDumpExcept(null)]
        internal MemberItem[] Members { get { return _membersCache.Value; } }

        [DisableDump]
        public IEnumerable<Field> Fields
        {
            get
            {
                return Members.Select(m =>
                {
                    var field = m.Field;
                    field.DataBase = Parent;
                    field.Container = Type;
                    return field;
                });
            }
        }

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

    sealed class TypeQuery : QueryBase
    {
        [NotNull]
        readonly string _tableName;
        public TypeQuery(Server server, string tableName)
            : base(new QueryProvider(server)) { _tableName = tableName; }

        internal override string CreateSQL() { return "select * from " + _tableName; }
    }

    sealed class DataItem
    {
        public string Name;
        public object Value;
    }

    sealed class TableRecord : DumpableObject
    {
        public string Name;
        public DataRecord Record;
    }

    sealed class DataRecord : DumpableObject
    {
        public readonly DataItem[] Values;

        public DataRecord(DbDataRecord reader)
        {
            Values = reader
                .FieldCount
                .Select(i => new DataItem
                {
                    Name = reader.GetName(i),
                    Value = reader[i]
                })
                .ToArray();
        }

        [ContainsConverter]
        internal bool Contains(IEnumerable<Field> fields, string value)
        {
            NotImplementedMethod(fields, value);
            return false;
        }

        sealed class ContainsConverter : SQLConverter
        {
            internal override string Convert(QueryProvider provider, Expression objectExpression, Expression[] args)
            {
                var parameterExpression = objectExpression as ParameterExpression;
                Tracer.Assert(parameterExpression != null);
                var name = parameterExpression.Name;
                Tracer.Assert(args.Length == 2);
                var field = QueryProvider.CreateFieldNames(name, args[0]).ToArray();
                if(field.Length == 0)
                    return "1=0";
                var value = ("%"+args[1].Eval<string>() +"%").SQLFormat();
                return "("+ field.Select(f => f + " like " + value).Stringify(" or ") +")";
            }
        }
    }
}