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
        [DisableDump]
        internal readonly ReferenceItem[] References;

        internal readonly int? KeyIndex;
        internal readonly int[][] Uniques;

        readonly ValueCache<MemberItem[]> _membersCache;
        readonly ValueCache<string[]> _foreignKeysCache;

        public TypeItem(DataBase parent, CompountType type, ReferenceItem[] references, int? keyIndex, int[][] uniques)
            : base(parent, type.Name)
        {
            _foreignKeysCache = new ValueCache<string[]>(GetForeignKeys);
            _membersCache = new ValueCache<MemberItem[]>(GetMembers);
            Type = type;
            References = references;
            KeyIndex = keyIndex;
            Uniques = uniques;
            Data = new TypeQuery(parent.Parent, Parent.Name + "." + Type.FullName);
        }

        [EnableDumpExcept(null)]
        internal MemberItem[] Members { get { return _membersCache.Value; } }

        [DisableDump]
        internal string[] ForeignKeys { get { return _foreignKeysCache.Value; } }

        string[] GetForeignKeys()
        {
            return References
                .SelectMany(r => r.Columns)
                .Distinct()
                .ToArray();
        }

        [Node]
        [DisableDump]
        internal MemberItem[] Attributes { get { return Members.Where(item => !ForeignKeys.Contains(item.Name)).ToArray(); } }

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

        internal IEnumerable<DataRecord> FindAllText(string value)
        {
            var fields = Fields.Where(f => f.Type.IsText).ToArray();
            if(fields.Any())
                return Data.Where(r => r.Contains(fields, value));
            return new DataRecord[0];
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
                var value = ("%" + args[1].Eval<string>() + "%").SQLFormat();
                return "(" + field.Select(f => f + " like " + value).Stringify(" or ") + ")";
            }
        }
    }
}