using System;
using System.Linq;
using hw.Forms;
using hw.Helper;
using Taabus.MetaData;

namespace Taabus.Data
{
    sealed class ReferenceItem : Item, IIconKeyProvider, IAdditionalNodeInfoProvider
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

        string IIconKeyProvider.IconKey { get { return "Dictionary"; } }
        string IAdditionalNodeInfoProvider.AdditionalNodeInfo { get { return GetColumnList() + " => " + TargetType.Name; } }
        string GetColumnList()
        {
            if(Columns.Length == 1)
                return Columns[0];
            return "(" + Columns.Stringify(",") + ")";
        }

        [Node]
        public TypeItem TargetType { get { return _targetTypeCache.Value; } }

        protected override Item[] GetItems() { return null; }
    }
}