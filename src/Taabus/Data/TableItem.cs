using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Forms;
using hw.DebugFormatter;
using hw.Forms;
using hw.Helper;
using JetBrains.Annotations;
using Taabus.MetaData;
using Taabus.UserInterface;

namespace Taabus.Data
{
    sealed class TypeItem : Item
        , ITreeNodeSupport
        , ITreeNodeProbeSupport
        , IIconKeyProvider
        , IDataItem
        , DragDropController.IItem
        , TypeItemView.IItem
    {
        readonly DataBase _parent;
        [DisableDump]
        internal readonly CompountType Type;
        [DisableDump]
        internal readonly TypeQuery Data;

        readonly ValueCache<int?> _keyIndexCache;
        readonly ValueCache<int[][]> _uniquesCache;

        readonly ValueCache<MemberItem[]> _membersCache;
        readonly ValueCache<string[]> _foreignKeysCache;
        readonly ValueCache<ReferenceItem[]> _referencesCache;

        public TypeItem(DataBase parent, CompountType type)
            : base(parent, type.Name)
        {
            _parent = parent;
            Type = type;
            _foreignKeysCache = new ValueCache<string[]>(GetForeignKeys);
            _membersCache = new ValueCache<MemberItem[]>(GetMembers);
            _referencesCache = new ValueCache<ReferenceItem[]>(() => parent.GetReferences(type));
            _keyIndexCache = new ValueCache<int?>(() => parent.GetPrimaryKeyIndex(type));
            _uniquesCache = new ValueCache<int[][]>(() => parent.GetUniques(type));
            Data = new TypeQuery(parent.Parent, "["+Parent.Name + "]." + type.FullName);
        }

        string TypeItemView.IItem.Title { get { return Name; } }
        long TypeItemView.IItem.Count { get { return Data.Count(); } }
        IEnumerable<IDataColumn> IColumnsAndDataProvider.Columns { get { return Members; } }
        IEnumerable<DataRecord> IColumnsAndDataProvider.Data { get { return Data; } }

        External.TypeItem IExternalizeable<External.TypeItem>.Convert(Externalizer provider)
        {
            return new External.TypeItem
            {
                TypeId = Name,
                DataBaseId = _parent.Name,
                ServerId = _parent.Parent.Name
            };
        }

        IEnumerable<TreeNode> ITreeNodeSupport.CreateNodes() { return CreateNodesYield(); }
        bool ITreeNodeProbeSupport.IsEmpty { get { return false; } }
        string IIconKeyProvider.IconKey { get { return "Table"; } }

        [EnableDumpExcept(null)]
        internal MemberItem[] Members { get { return _membersCache.Value; } }

        [DisableDump]
        internal string[] ForeignKeys { get { return _foreignKeysCache.Value; } }

        [Node]
        [DisableDump]
        internal ReferenceItem[] References { get { return _referencesCache.Value; } }

        string[] GetForeignKeys()
        {
            return References
                .SelectMany(r => r.Columns)
                .Distinct()
                .ToArray();
        }

        [Node]
        [DisableDump]
        internal MemberItem[] Attributes
        {
            get
            {
                var memberItems = Profiler.Measure(() => Members);
                return Profiler.Measure(() => memberItems
                    .Where(item => !ForeignKeys.Contains(item.Name))
                    .ToArray());
            }
        }

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

        public int? KeyIndex { get { return _keyIndexCache.Value; } }

        public int[][] Uniques { get { return _uniquesCache.Value; } }

        IEnumerable<TreeNode> CreateNodes() { return GetReferenceNodes().Concat(GetAttributeNodes()); }

        IEnumerable<TreeNode> CreateNodesYield()
        {
            foreach(var item in References)
                yield return item.CreateNode();
            foreach(var item in Attributes)
                yield return item.CreateNode();
        }

        IEnumerable<TreeNode> GetAttributeNodes()
        {
            return Attributes
                .Select(item => item.CreateNode())
                .ToArray();
        }

        IEnumerable<TreeNode> GetReferenceNodes()
        {
            return References
                .Select(item => item.CreateNode())
                .ToArray();
        }

        protected override Item[] GetItems()
        {
            return GetMembers()
                .Cast<Item>()
                .ToArray();
        }

        MemberItem[] GetMembers()
        {
            return Type
                .Members
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

    interface IDataItem
    {}

    sealed class TypeQuery : QueryBase
    {
        [NotNull]
        readonly string _tableName;
        public TypeQuery(Server server, string tableName)
            : base(new QueryProvider(server)) { _tableName = tableName; }

        protected override string ExecutableStatement { get { return "select * from " + base.ExecutableStatement; } }
        internal override string SubStatement { get { return _tableName; } }
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