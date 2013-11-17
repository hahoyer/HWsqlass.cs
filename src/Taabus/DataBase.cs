using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Windows.Forms;
using hw.Debug;
using hw.Forms;
using hw.Helper;
using Taabus.MetaData;

namespace Taabus
{
    public sealed class DataBase : NamedObject, IDataProvider, ITreeNodeSupport, ITreeNodeProbeSupport
    {
        const string MetaDataStatement = "select * from [{0}].[{1}].[{2}]";

        internal static DataBase Create(DbDataRecord record, Server server) { return new DataBase(server, (string) record["name"]); }

        readonly Information _information;
        readonly ValueCache<TypeItem[]> _typeItemsCache;
        [DisableDump]
        internal readonly Server Parent;
        readonly string _name;

        DataBase(Server parent, string name)
        {
            Parent = parent;
            _name = name;
            _information = new Information(this);
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

        internal SQLSysViews.all_columnsClass[] SysColumns { get { return _information.SysColumns; } }
        internal SQLSysViews.all_objectsClass[] SysObjects { get { return _information.SysObjects; } }

        internal string SelectMetaDataStatement(string schema, string name)
        {
            return MetaDataStatement
                .ReplaceArgs(Name, schema, name);
        }

        TypeItem[] GetTypes()
        {
            return _information
                .CompountTypes
                .Select(type1 => type1.CreateType(this))
                .OrderBy(type => type.Name)
                .ToArray();
        }

        internal int[][] GetUniques(CompountType type)
        {
            if(IsInDump)
                return null;

            var keyConstraint = SysObject(type)
                .Indexes
                .Where(c => c.is_unique == true)
                .Select
                (kc => kc
                    .Columns
                    .Select(kccn => type.Members.IndexOf(m => m.Name == kccn.Column.name).AssertValue())
                    .ToArray()
                )
                .ToArray();
            return keyConstraint;
        }

        internal int? GetPrimaryKeyIndex(CompountType type)
        {
            if(IsInDump)
                return null;

            var keyConstraint = SysObject(type)
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

        SQLSysViews.all_objectsClass SysObject(CompountType type)
        {
            return _information
                .SysObjects
                .Single(o => o.name == type.Name && o.Schema.name == type.Schema);
        }

        internal ReferenceItem[] GetReferences(CompountType type)
        {
            return SysObject(type)
                .ForeignKeys
                .Select(CreateReference)
                .ToArray();
        }

        TypeItem FindType(SQLSysViews.all_objectsClass target)
        {
            return GetTypes()
                .Single(typeItem => SysObject(typeItem.Type) == target);
        }

        T[] IDataProvider.Select<T>(string schema, string name, Func<DbDataRecord, T> func) { return Parent.Select(SelectMetaDataStatement(schema, name), func); }
        IEnumerable<TreeNode> ITreeNodeSupport.CreateNodes() { return Types.CreateNodes(); }
 
        bool ITreeNodeProbeSupport.IsEmpty
        {
            get
            {
                if(_typeItemsCache.IsValid)
                    return !Types.Any();
                return !_information.HasCompountTypes;
            }
        }

        ReferenceItem CreateReference(SQLSysViews.foreign_keysClass constraint)
        {
            return
                new ReferenceItem(this, constraint, FindType);
        }
    }
}