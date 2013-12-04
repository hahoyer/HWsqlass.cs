using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Windows.Forms;
using hw.Debug;
using hw.Forms;
using hw.Helper;
using Taabus.MetaData;

namespace Taabus.Data
{
    public sealed class DataBase : NamedObject, IDataProvider, ITreeNodeSupport, ITreeNodeProbeSupport, IIconKeyProvider
    {
        const string MetaDataStatement = "select * from [{0}].[{1}].[{2}]";

        internal static DataBase Create(DbDataRecord record, Server server)
        {
            var name = Profiler.Measure(() => (string) record["name"]);
            return Profiler.Measure(() => new DataBase(server, name));
        }

        readonly Information _information;
        readonly ValueCache<TypeItem[]> _typeItemsCache;
        [DisableDump]
        internal readonly Server Parent;
        readonly string _name;

        DataBase(Server parent, string name)
        {
            Parent = parent;
            _name = name;
            _information = Profiler.Measure(() => new Information(this));
            _typeItemsCache = new ValueCache<TypeItem[]>(GetTypes);
        }

        public override string Name { get { return _name; } }

        internal TypeItem[] Types
        {
            get
            {
                if(_typeItemsCache.IsBusy)
                    return null;
                return _typeItemsCache.Value;
            }
        }

        internal IEnumerable<Field> Fields
        {
            get
            {
                if(_typeItemsCache.IsBusy)
                    return null;

                return Types.SelectMany(t => t.Fields);
            }
        }

        internal string SelectMetaDataStatement(string schema, string name)
        {
            return MetaDataStatement
                .ReplaceArgs(Name, schema, name);
        }

        TypeItem[] GetTypes()
        {
            var compountTypes = _information.CompountTypes;
            var typeItems = Profiler.Measure(() => compountTypes.Select(CreateType));
            return Profiler.Measure(() => typeItems.OrderBy(type => type.Name).ToArray());
        }

        internal int[][] GetUniques(CompountType type) { return IsInDump ? null : type.Uniques; }

        internal int? GetPrimaryKeyIndex(CompountType type)
        {
            if(IsInDump)
                return null;

            var keyConstraint = type.Object
                .KeyConstraints
                .SingleOrDefault(k => k.Object.Type == ObjectType.PrimaryKey);
            if(keyConstraint == null)
                return null;
            if(keyConstraint.Index.Columns.Length != 1)
                return null;

            return type
                .Members
                .IndexOf(m => m.Name == keyConstraint.Index.Columns[0].Column.name)
                .AssertValue();
        }

        internal ReferenceItem[] GetReferences(CompountType type)
        {
            return type.Object
                .ForeignKeys
                .Select(CreateReference)
                .ToArray();
        }

        TypeItem FindType(SQLSysViews.all_objectsClass target)
        {
            return GetTypes()
                .Single(typeItem => typeItem.Type.Object == target);
        }

        IEnumerable<T> IDataProvider.Select<T>(string schema, string name, Func<DbDataRecord, T> func)
        {
            return Parent
                .Select(SelectMetaDataStatement(schema, name), func);
        }

        IEnumerable<TreeNode> ITreeNodeSupport.CreateNodes() { return Types.CreateNodes(); }
        string IIconKeyProvider.IconKey { get { return "Database"; } }

        bool ITreeNodeProbeSupport.IsEmpty { get { return _typeItemsCache.IsValid && !Types.Any(); } }

        ReferenceItem CreateReference(SQLSysViews.foreign_keysClass constraint)
        {
            return
                new ReferenceItem(this, constraint, FindType);
        }

        TypeItem CreateType(CompountType compountType) { return Profiler.Measure(() => new TypeItem(this, compountType)); }
    }
}