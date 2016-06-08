using System;
using System.Collections.Generic;
using System.Linq;
using hw.DebugFormatter;
using hw.Helper;

namespace Taabus.MetaData
{
    partial class SQLSysViews
    {
        internal readonly FunctionCache<all_objectsClass, all_columnsClass[]> Columns;
        readonly ValueCache<Dictionary<string, all_objectsClass>> _objectsCache;
        readonly ValueCache<all_objectsClass[]> _compountTypesCache;

        SQLSysViews()
        {
            _compountTypesCache = new ValueCache<all_objectsClass[]>(GetCompountTypes);
            _objectsCache = new ValueCache<Dictionary<string, all_objectsClass>>(CreateObjects);
            Columns = new FunctionCache<all_objectsClass, all_columnsClass[]>(CreateColumns);
        }
        Dictionary<string, all_objectsClass> CreateObjects() { return all_objects.ToDictionary(o => o.name); }

        all_columnsClass[] CreateColumns(all_objectsClass key) { return Profiler.Measure(()=>all_columns.Where(c => c.object_id == key.object_id).ToArray()); }

        all_objectsClass[] GetCompountTypes()
        {
            return all_objects
                .Where(o => o.Type.IsCompountType && o.Schema.name != "sys")
                .ToArray();
        }

        internal all_objectsClass[] CompountTypes { get { return _compountTypesCache.Value; } }

        // ReSharper disable once InconsistentNaming
        partial class all_columnsClass : DumpableObject
        {
            protected override string GetNodeDump() { return "[" + object_id + ":" + column_id + "]" + name + "(" + TypeName + ")"; }

            string TypeName
            {
                get
                {
                    var result = Type.name;
                    switch(Type.name)
                    {
                        case "decimal":
                            return result + "(" + precision + "," + scale + ")";
                        case "char":
                            return result + "(" + max_length + ")";
                        case "nvarchar":
                            return result + "(" + (max_length == -1 ? "max" : (max_length / 2).ToString()) + ")";
                        case "nchar":
                            return result + "(" + max_length / 2 + ")";

                        default:
                            return result;
                    }
                }
            }

            internal Member CreateMember() { return new Member(name, BasicType.GetInstance(this)); }
        }

        // ReSharper disable once InconsistentNaming
        partial class all_objectsClass : DumpableObject
        {
            protected override string GetNodeDump() { return "[" + object_id + "]" + name; }

            [DisableDump]
            internal all_columnsClass[] Columns { get { return _parent.Columns[this].ToArray(); } }

            internal ObjectType Type { get { return ObjectType.All.Single(c => c.Name == type); } }

            [DisableDump]
            internal key_constraintsClass AsKeyConstraint { get { return _parent.key_constraints.Single(k => k.object_id == object_id); } }

            [DisableDump]
            internal foreign_keysClass AsForeignKey { get { return _parent.foreign_keys.Single(k => k.object_id == object_id); } }

            [DisableDump]
            internal indexesClass[] Indexes { get { return _parent.indexes.Where(k => k.object_id == object_id).ToArray(); } }

            [DisableDump]
            internal key_constraintsClass[] KeyConstraints { get { return _parent.key_constraints.Where(k => k.Parent == this).ToArray(); } }

            [DisableDump]
            internal foreign_keysClass[] ForeignKeys { get { return _parent.foreign_keys.Where(k => k.Parent == this).ToArray(); } }

            [DisableDump]
            internal Member[] Members
            {
                get
                {
                    return _parent
                        .Columns[this]
                        .Select(column => column.CreateMember())
                        .ToArray();
                }
            }
        }

        // ReSharper disable once InconsistentNaming
        partial class schemasClass : DumpableObject
        {
            protected override string GetNodeDump() { return "[" + schema_id + "]" + name; }
        }

        // ReSharper disable once InconsistentNaming
        partial class indexesClass : DumpableObject
        {
            protected override string GetNodeDump() { return "[" + object_id + "." + index_id + "]" + name; }

            internal index_columnsClass[] Columns
            {
                get
                {
                    return _parent
                        .index_columns
                        .Where(c => c.index_id == index_id && c.Object == Object)
                        .ToArray();
                }
            }
        }

        // ReSharper disable once InconsistentNaming
        sealed partial class key_constraintsClass : DumpableObject
        {
            protected override string GetNodeDump() { return "[" + object_id + "]" + name; }
        }

        // ReSharper disable once InconsistentNaming
        partial class foreign_keysClass : DumpableObject
        {
            protected override string GetNodeDump() { return "[" + object_id + "]" + name; }

            internal foreign_key_columnsClass[] Columns
            {
                get
                {
                    return _parent
                        .foreign_key_columns
                        .Where(c => c.Constraint == this)
                        .ToArray();
                }
            }
        }
    }
}