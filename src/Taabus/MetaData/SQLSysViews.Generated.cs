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
using hw.Debug;
using hw.Helper;

namespace Taabus.MetaData
{
    public sealed partial class SQLSysViews
    {
        public class CacheClass
        {
            public readonly ValueCache<IEnumerable<all_columnsClass>> all_columns;
            public readonly ValueCache<IEnumerable<all_objectsClass>> all_objects;
            public readonly ValueCache<IEnumerable<key_constraintsClass>> key_constraints;
            public readonly ValueCache<IEnumerable<foreign_keysClass>> foreign_keys;
            public readonly ValueCache<IEnumerable<foreign_key_columnsClass>> foreign_key_columns;
            public readonly ValueCache<IEnumerable<indexesClass>> indexes;
            public readonly ValueCache<IEnumerable<index_columnsClass>> index_columns;
            public readonly ValueCache<IEnumerable<schemasClass>> schemas;
            public readonly ValueCache<IEnumerable<typesClass>> types;

            public CacheClass(IDataProvider provider, SQLSysViews parent)
            {
                all_columns = new ValueCache<IEnumerable<all_columnsClass>>(() => provider.Select<all_columnsClass>("sys", "all_columns", r => new all_columnsClass(r, parent)));
                all_objects = new ValueCache<IEnumerable<all_objectsClass>>(() => provider.Select<all_objectsClass>("sys", "all_objects", r => new all_objectsClass(r, parent)));
                key_constraints = new ValueCache<IEnumerable<key_constraintsClass>>(() => provider.Select<key_constraintsClass>("sys", "key_constraints", r => new key_constraintsClass(r, parent)));
                foreign_keys = new ValueCache<IEnumerable<foreign_keysClass>>(() => provider.Select<foreign_keysClass>("sys", "foreign_keys", r => new foreign_keysClass(r, parent)));
                foreign_key_columns = new ValueCache<IEnumerable<foreign_key_columnsClass>>(() => provider.Select<foreign_key_columnsClass>("sys", "foreign_key_columns", r => new foreign_key_columnsClass(r, parent)));
                indexes = new ValueCache<IEnumerable<indexesClass>>(() => provider.Select<indexesClass>("sys", "indexes", r => new indexesClass(r, parent)));
                index_columns = new ValueCache<IEnumerable<index_columnsClass>>(() => provider.Select<index_columnsClass>("sys", "index_columns", r => new index_columnsClass(r, parent)));
                schemas = new ValueCache<IEnumerable<schemasClass>>(() => provider.Select<schemasClass>("sys", "schemas", r => new schemasClass(r, parent)));
                types = new ValueCache<IEnumerable<typesClass>>(() => provider.Select<typesClass>("sys", "types", r => new typesClass(r, parent)));
            }
        }

        public readonly CacheClass Cache;

        public IEnumerable<all_columnsClass> all_columns { get { return Profiler.Measure(()=>Cache.all_columns.Value); } }
        public IEnumerable<all_objectsClass> all_objects { get { return Cache.all_objects.Value; } }
        public IEnumerable<key_constraintsClass> key_constraints { get { return Cache.key_constraints.Value; } }
        public IEnumerable<foreign_keysClass> foreign_keys { get { return Cache.foreign_keys.Value; } }
        public IEnumerable<foreign_key_columnsClass> foreign_key_columns { get { return Cache.foreign_key_columns.Value; } }
        public IEnumerable<indexesClass> indexes { get { return Cache.indexes.Value; } }
        public IEnumerable<index_columnsClass> index_columns { get { return Cache.index_columns.Value; } }
        public IEnumerable<schemasClass> schemas { get { return Cache.schemas.Value; } }
        public IEnumerable<typesClass> types { get { return Cache.types.Value; } }

        public SQLSysViews(IDataProvider provider) :this()
        {
            Cache = new CacheClass(provider, this);
        }

        public sealed partial class all_columnsClass
        {
            readonly SQLSysViews _parent;
            internal all_columnsClass(DbDataRecord record, SQLSysViews parent)
            {
                _parent = parent;
                object_id = record["object_id"].Convert<int?>();
                name = record["name"].Convert<string>();
                column_id = record["column_id"].Convert<int?>();
                system_type_id = record["system_type_id"].Convert<Byte?>();
                user_type_id = record["user_type_id"].Convert<int?>();
                max_length = record["max_length"].Convert<Int16?>();
                precision = record["precision"].Convert<Byte?>();
                scale = record["scale"].Convert<Byte?>();
                collation_name = record["collation_name"].Convert<string>();
                is_nullable = record["is_nullable"].Convert<Boolean?>();
                is_ansi_padded = record["is_ansi_padded"].Convert<Boolean?>();
                is_rowguidcol = record["is_rowguidcol"].Convert<Boolean?>();
                is_identity = record["is_identity"].Convert<Boolean?>();
                is_computed = record["is_computed"].Convert<Boolean?>();
                is_filestream = record["is_filestream"].Convert<Boolean?>();
                is_replicated = record["is_replicated"].Convert<Boolean?>();
                is_non_sql_subscribed = record["is_non_sql_subscribed"].Convert<Boolean?>();
                is_merge_published = record["is_merge_published"].Convert<Boolean?>();
                is_dts_replicated = record["is_dts_replicated"].Convert<Boolean?>();
                is_xml_document = record["is_xml_document"].Convert<Boolean?>();
                xml_collection_id = record["xml_collection_id"].Convert<int?>();
                default_object_id = record["default_object_id"].Convert<int?>();
                rule_object_id = record["rule_object_id"].Convert<int?>();
                is_sparse = record["is_sparse"].Convert<Boolean?>();
                is_column_set = record["is_column_set"].Convert<Boolean?>();
            }

            public readonly int? object_id;
            public readonly string name;
            public readonly int? column_id;
            public readonly Byte? system_type_id;
            public readonly int? user_type_id;
            public readonly Int16? max_length;
            public readonly Byte? precision;
            public readonly Byte? scale;
            public readonly string collation_name;
            public readonly Boolean? is_nullable;
            public readonly Boolean? is_ansi_padded;
            public readonly Boolean? is_rowguidcol;
            public readonly Boolean? is_identity;
            public readonly Boolean? is_computed;
            public readonly Boolean? is_filestream;
            public readonly Boolean? is_replicated;
            public readonly Boolean? is_non_sql_subscribed;
            public readonly Boolean? is_merge_published;
            public readonly Boolean? is_dts_replicated;
            public readonly Boolean? is_xml_document;
            public readonly int? xml_collection_id;
            public readonly int? default_object_id;
            public readonly int? rule_object_id;
            public readonly Boolean? is_sparse;
            public readonly Boolean? is_column_set;

            all_objectsClass _ObjectCache;
            public all_objectsClass Object
            {
                get
                {
                    return _ObjectCache
                        ?? (_ObjectCache = _parent.all_objects.Single(t => object_id == t.object_id));
                }
            }

            typesClass _TypeCache;
            public typesClass Type
            {
                get
                {
                    return _TypeCache
                        ?? (_TypeCache = _parent.types.Single(t => user_type_id == t.user_type_id));
                }
            }


        }
        public sealed partial class all_objectsClass
        {
            readonly SQLSysViews _parent;
            internal all_objectsClass(DbDataRecord record, SQLSysViews parent)
            {
                _parent = parent;
                name = record["name"].Convert<string>();
                object_id = record["object_id"].Convert<int?>();
                principal_id = record["principal_id"].Convert<int?>();
                schema_id = record["schema_id"].Convert<int?>();
                parent_object_id = record["parent_object_id"].Convert<int?>();
                type = record["type"].Convert<string>();
                type_desc = record["type_desc"].Convert<string>();
                create_date = record["create_date"].Convert<DateTime?>();
                modify_date = record["modify_date"].Convert<DateTime?>();
                is_ms_shipped = record["is_ms_shipped"].Convert<Boolean?>();
                is_published = record["is_published"].Convert<Boolean?>();
                is_schema_published = record["is_schema_published"].Convert<Boolean?>();
            }

            public readonly string name;
            public readonly int? object_id;
            public readonly int? principal_id;
            public readonly int? schema_id;
            public readonly int? parent_object_id;
            public readonly string type;
            public readonly string type_desc;
            public readonly DateTime? create_date;
            public readonly DateTime? modify_date;
            public readonly Boolean? is_ms_shipped;
            public readonly Boolean? is_published;
            public readonly Boolean? is_schema_published;

            schemasClass _SchemaCache;
            public schemasClass Schema
            {
                get
                {
                    return _SchemaCache
                        ?? (_SchemaCache = _parent.schemas.Single(t => schema_id == t.schema_id));
                }
            }

            all_objectsClass _ParentCache;
            public all_objectsClass Parent
            {
                get
                {
                    if(parent_object_id == 0)
                        return null;
                    return _ParentCache
                        ?? (_ParentCache = _parent.all_objects.Single(t => parent_object_id == t.object_id));
                }
            }


        }
        public sealed partial class key_constraintsClass
        {
            readonly SQLSysViews _parent;
            internal key_constraintsClass(DbDataRecord record, SQLSysViews parent)
            {
                _parent = parent;
                name = record["name"].Convert<string>();
                object_id = record["object_id"].Convert<int?>();
                principal_id = record["principal_id"].Convert<int?>();
                schema_id = record["schema_id"].Convert<int?>();
                parent_object_id = record["parent_object_id"].Convert<int?>();
                type = record["type"].Convert<string>();
                type_desc = record["type_desc"].Convert<string>();
                create_date = record["create_date"].Convert<DateTime?>();
                modify_date = record["modify_date"].Convert<DateTime?>();
                is_ms_shipped = record["is_ms_shipped"].Convert<Boolean?>();
                is_published = record["is_published"].Convert<Boolean?>();
                is_schema_published = record["is_schema_published"].Convert<Boolean?>();
                unique_index_id = record["unique_index_id"].Convert<int?>();
                is_system_named = record["is_system_named"].Convert<Boolean?>();
            }

            public readonly string name;
            public readonly int? object_id;
            public readonly int? principal_id;
            public readonly int? schema_id;
            public readonly int? parent_object_id;
            public readonly string type;
            public readonly string type_desc;
            public readonly DateTime? create_date;
            public readonly DateTime? modify_date;
            public readonly Boolean? is_ms_shipped;
            public readonly Boolean? is_published;
            public readonly Boolean? is_schema_published;
            public readonly int? unique_index_id;
            public readonly Boolean? is_system_named;

            all_objectsClass _ObjectCache;
            public all_objectsClass Object
            {
                get
                {
                    return _ObjectCache
                        ?? (_ObjectCache = _parent.all_objects.Single(t => object_id == t.object_id));
                }
            }

            all_objectsClass _ParentCache;
            public all_objectsClass Parent
            {
                get
                {
                    return _ParentCache
                        ?? (_ParentCache = _parent.all_objects.Single(t => parent_object_id == t.object_id));
                }
            }

            schemasClass _SchemaCache;
            public schemasClass Schema
            {
                get
                {
                    return _SchemaCache
                        ?? (_SchemaCache = _parent.schemas.Single(t => schema_id == t.schema_id));
                }
            }

            indexesClass _IndexCache;
            public indexesClass Index
            {
                get
                {
                    return _IndexCache
                        ?? (_IndexCache = _parent.indexes.Single(t => parent_object_id == t.object_id && unique_index_id == t.index_id));
                }
            }


        }
        public sealed partial class foreign_keysClass
        {
            readonly SQLSysViews _parent;
            internal foreign_keysClass(DbDataRecord record, SQLSysViews parent)
            {
                _parent = parent;
                name = record["name"].Convert<string>();
                object_id = record["object_id"].Convert<int?>();
                principal_id = record["principal_id"].Convert<int?>();
                schema_id = record["schema_id"].Convert<int?>();
                parent_object_id = record["parent_object_id"].Convert<int?>();
                type = record["type"].Convert<string>();
                type_desc = record["type_desc"].Convert<string>();
                create_date = record["create_date"].Convert<DateTime?>();
                modify_date = record["modify_date"].Convert<DateTime?>();
                is_ms_shipped = record["is_ms_shipped"].Convert<Boolean?>();
                is_published = record["is_published"].Convert<Boolean?>();
                is_schema_published = record["is_schema_published"].Convert<Boolean?>();
                referenced_object_id = record["referenced_object_id"].Convert<int?>();
                key_index_id = record["key_index_id"].Convert<int?>();
                is_disabled = record["is_disabled"].Convert<Boolean?>();
                is_not_for_replication = record["is_not_for_replication"].Convert<Boolean?>();
                is_not_trusted = record["is_not_trusted"].Convert<Boolean?>();
                delete_referential_action = record["delete_referential_action"].Convert<Byte?>();
                delete_referential_action_desc = record["delete_referential_action_desc"].Convert<string>();
                update_referential_action = record["update_referential_action"].Convert<Byte?>();
                update_referential_action_desc = record["update_referential_action_desc"].Convert<string>();
                is_system_named = record["is_system_named"].Convert<Boolean?>();
            }

            public readonly string name;
            public readonly int? object_id;
            public readonly int? principal_id;
            public readonly int? schema_id;
            public readonly int? parent_object_id;
            public readonly string type;
            public readonly string type_desc;
            public readonly DateTime? create_date;
            public readonly DateTime? modify_date;
            public readonly Boolean? is_ms_shipped;
            public readonly Boolean? is_published;
            public readonly Boolean? is_schema_published;
            public readonly int? referenced_object_id;
            public readonly int? key_index_id;
            public readonly Boolean? is_disabled;
            public readonly Boolean? is_not_for_replication;
            public readonly Boolean? is_not_trusted;
            public readonly Byte? delete_referential_action;
            public readonly string delete_referential_action_desc;
            public readonly Byte? update_referential_action;
            public readonly string update_referential_action_desc;
            public readonly Boolean? is_system_named;

            all_objectsClass _ObjectCache;
            public all_objectsClass Object
            {
                get
                {
                    return _ObjectCache
                        ?? (_ObjectCache = _parent.all_objects.Single(t => object_id == t.object_id));
                }
            }

            all_objectsClass _ParentCache;
            public all_objectsClass Parent
            {
                get
                {
                    return _ParentCache
                        ?? (_ParentCache = _parent.all_objects.Single(t => parent_object_id == t.object_id));
                }
            }

            all_objectsClass _ReferenceCache;
            public all_objectsClass Reference
            {
                get
                {
                    return _ReferenceCache
                        ?? (_ReferenceCache = _parent.all_objects.Single(t => referenced_object_id == t.object_id));
                }
            }

            schemasClass _SchemaCache;
            public schemasClass Schema
            {
                get
                {
                    return _SchemaCache
                        ?? (_SchemaCache = _parent.schemas.Single(t => schema_id == t.schema_id));
                }
            }

            indexesClass _IndexCache;
            public indexesClass Index
            {
                get
                {
                    return _IndexCache
                        ?? (_IndexCache = _parent.indexes.Single(t => referenced_object_id == t.object_id && key_index_id == t.index_id));
                }
            }


        }
        public sealed partial class foreign_key_columnsClass
        {
            readonly SQLSysViews _parent;
            internal foreign_key_columnsClass(DbDataRecord record, SQLSysViews parent)
            {
                _parent = parent;
                constraint_object_id = record["constraint_object_id"].Convert<int?>();
                constraint_column_id = record["constraint_column_id"].Convert<int?>();
                parent_object_id = record["parent_object_id"].Convert<int?>();
                parent_column_id = record["parent_column_id"].Convert<int?>();
                referenced_object_id = record["referenced_object_id"].Convert<int?>();
                referenced_column_id = record["referenced_column_id"].Convert<int?>();
            }

            public readonly int? constraint_object_id;
            public readonly int? constraint_column_id;
            public readonly int? parent_object_id;
            public readonly int? parent_column_id;
            public readonly int? referenced_object_id;
            public readonly int? referenced_column_id;

            foreign_keysClass _ConstraintCache;
            public foreign_keysClass Constraint
            {
                get
                {
                    return _ConstraintCache
                        ?? (_ConstraintCache = _parent.foreign_keys.Single(t => constraint_object_id == t.object_id));
                }
            }

            all_columnsClass _ConstraintColumnCache;
            public all_columnsClass ConstraintColumn
            {
                get
                {
                    return _ConstraintColumnCache
                        ?? (_ConstraintColumnCache = _parent.all_columns.Single(t => Constraint.parent_object_id == t.object_id && constraint_column_id == t.column_id));
                }
            }

            all_objectsClass _ParentCache;
            public all_objectsClass Parent
            {
                get
                {
                    return _ParentCache
                        ?? (_ParentCache = _parent.all_objects.Single(t => parent_object_id == t.object_id));
                }
            }

            all_columnsClass _ParentColumnCache;
            public all_columnsClass ParentColumn
            {
                get
                {
                    return _ParentColumnCache
                        ?? (_ParentColumnCache = _parent.all_columns.Single(t => parent_object_id == t.object_id && parent_column_id == t.column_id));
                }
            }

            all_objectsClass _ReferenceCache;
            public all_objectsClass Reference
            {
                get
                {
                    return _ReferenceCache
                        ?? (_ReferenceCache = _parent.all_objects.Single(t => referenced_object_id == t.object_id));
                }
            }

            all_columnsClass _ReferenceColumnCache;
            public all_columnsClass ReferenceColumn
            {
                get
                {
                    return _ReferenceColumnCache
                        ?? (_ReferenceColumnCache = _parent.all_columns.Single(t => referenced_object_id == t.object_id && referenced_column_id == t.column_id));
                }
            }


        }
        public sealed partial class indexesClass
        {
            readonly SQLSysViews _parent;
            internal indexesClass(DbDataRecord record, SQLSysViews parent)
            {
                _parent = parent;
                object_id = record["object_id"].Convert<int?>();
                name = record["name"].Convert<string>();
                index_id = record["index_id"].Convert<int?>();
                type = record["type"].Convert<Byte?>();
                type_desc = record["type_desc"].Convert<string>();
                is_unique = record["is_unique"].Convert<Boolean?>();
                data_space_id = record["data_space_id"].Convert<int?>();
                ignore_dup_key = record["ignore_dup_key"].Convert<Boolean?>();
                is_primary_key = record["is_primary_key"].Convert<Boolean?>();
                is_unique_constraint = record["is_unique_constraint"].Convert<Boolean?>();
                fill_factor = record["fill_factor"].Convert<Byte?>();
                is_padded = record["is_padded"].Convert<Boolean?>();
                is_disabled = record["is_disabled"].Convert<Boolean?>();
                is_hypothetical = record["is_hypothetical"].Convert<Boolean?>();
                allow_row_locks = record["allow_row_locks"].Convert<Boolean?>();
                allow_page_locks = record["allow_page_locks"].Convert<Boolean?>();
                has_filter = record["has_filter"].Convert<Boolean?>();
                filter_definition = record["filter_definition"].Convert<string>();
            }

            public readonly int? object_id;
            public readonly string name;
            public readonly int? index_id;
            public readonly Byte? type;
            public readonly string type_desc;
            public readonly Boolean? is_unique;
            public readonly int? data_space_id;
            public readonly Boolean? ignore_dup_key;
            public readonly Boolean? is_primary_key;
            public readonly Boolean? is_unique_constraint;
            public readonly Byte? fill_factor;
            public readonly Boolean? is_padded;
            public readonly Boolean? is_disabled;
            public readonly Boolean? is_hypothetical;
            public readonly Boolean? allow_row_locks;
            public readonly Boolean? allow_page_locks;
            public readonly Boolean? has_filter;
            public readonly string filter_definition;

            all_objectsClass _ObjectCache;
            public all_objectsClass Object
            {
                get
                {
                    return _ObjectCache
                        ?? (_ObjectCache = _parent.all_objects.Single(t => object_id == t.object_id));
                }
            }


        }
        public sealed partial class index_columnsClass
        {
            readonly SQLSysViews _parent;
            internal index_columnsClass(DbDataRecord record, SQLSysViews parent)
            {
                _parent = parent;
                object_id = record["object_id"].Convert<int?>();
                index_id = record["index_id"].Convert<int?>();
                index_column_id = record["index_column_id"].Convert<int?>();
                column_id = record["column_id"].Convert<int?>();
                key_ordinal = record["key_ordinal"].Convert<Byte?>();
                partition_ordinal = record["partition_ordinal"].Convert<Byte?>();
                is_descending_key = record["is_descending_key"].Convert<Boolean?>();
                is_included_column = record["is_included_column"].Convert<Boolean?>();
            }

            public readonly int? object_id;
            public readonly int? index_id;
            public readonly int? index_column_id;
            public readonly int? column_id;
            public readonly Byte? key_ordinal;
            public readonly Byte? partition_ordinal;
            public readonly Boolean? is_descending_key;
            public readonly Boolean? is_included_column;

            all_objectsClass _ObjectCache;
            public all_objectsClass Object
            {
                get
                {
                    return _ObjectCache
                        ?? (_ObjectCache = _parent.all_objects.Single(t => object_id == t.object_id));
                }
            }

            indexesClass _IndexCache;
            public indexesClass Index
            {
                get
                {
                    return _IndexCache
                        ?? (_IndexCache = _parent.indexes.Single(t => object_id == t.object_id && index_id == t.index_id));
                }
            }

            all_columnsClass _ColumnCache;
            public all_columnsClass Column
            {
                get
                {
                    return _ColumnCache
                        ?? (_ColumnCache = _parent.all_columns.Single(t => object_id == t.object_id && column_id == t.column_id));
                }
            }


        }
        public sealed partial class schemasClass
        {
            readonly SQLSysViews _parent;
            internal schemasClass(DbDataRecord record, SQLSysViews parent)
            {
                _parent = parent;
                name = record["name"].Convert<string>();
                schema_id = record["schema_id"].Convert<int?>();
                principal_id = record["principal_id"].Convert<int?>();
            }

            public readonly string name;
            public readonly int? schema_id;
            public readonly int? principal_id;


        }
        public sealed partial class typesClass
        {
            readonly SQLSysViews _parent;
            internal typesClass(DbDataRecord record, SQLSysViews parent)
            {
                _parent = parent;
                name = record["name"].Convert<string>();
                system_type_id = record["system_type_id"].Convert<Byte?>();
                user_type_id = record["user_type_id"].Convert<int?>();
                schema_id = record["schema_id"].Convert<int?>();
                principal_id = record["principal_id"].Convert<int?>();
                max_length = record["max_length"].Convert<Int16?>();
                precision = record["precision"].Convert<Byte?>();
                scale = record["scale"].Convert<Byte?>();
                collation_name = record["collation_name"].Convert<string>();
                is_nullable = record["is_nullable"].Convert<Boolean?>();
                is_user_defined = record["is_user_defined"].Convert<Boolean?>();
                is_assembly_type = record["is_assembly_type"].Convert<Boolean?>();
                default_object_id = record["default_object_id"].Convert<int?>();
                rule_object_id = record["rule_object_id"].Convert<int?>();
                is_table_type = record["is_table_type"].Convert<Boolean?>();
            }

            public readonly string name;
            public readonly Byte? system_type_id;
            public readonly int? user_type_id;
            public readonly int? schema_id;
            public readonly int? principal_id;
            public readonly Int16? max_length;
            public readonly Byte? precision;
            public readonly Byte? scale;
            public readonly string collation_name;
            public readonly Boolean? is_nullable;
            public readonly Boolean? is_user_defined;
            public readonly Boolean? is_assembly_type;
            public readonly int? default_object_id;
            public readonly int? rule_object_id;
            public readonly Boolean? is_table_type;


        }
    }
}

