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
using hw.Helper;

namespace Taabus.MetaData
{
    public sealed partial class SQLSysViews
    {
        public class CacheClass
        {
            public readonly ValueCache<columnsClass[]> columns;
            public readonly ValueCache<objectsClass[]> objects;
            public readonly ValueCache<schemasClass[]> schemas;
            public readonly ValueCache<typesClass[]> types;

            public CacheClass(IDataProvider provider, SQLSysViews parent)
            {
                columns = new ValueCache<columnsClass[]>(() => provider.Select<columnsClass>("sys", "columns", r => new columnsClass(r, parent)));
                objects = new ValueCache<objectsClass[]>(() => provider.Select<objectsClass>("sys", "objects", r => new objectsClass(r, parent)));
                schemas = new ValueCache<schemasClass[]>(() => provider.Select<schemasClass>("sys", "schemas", r => new schemasClass(r, parent)));
                types = new ValueCache<typesClass[]>(() => provider.Select<typesClass>("sys", "types", r => new typesClass(r, parent)));
            }
        }

        public readonly CacheClass Cache;

        public columnsClass[] columns { get { return Cache.columns.Value; } }
        public objectsClass[] objects { get { return Cache.objects.Value; } }
        public schemasClass[] schemas { get { return Cache.schemas.Value; } }
        public typesClass[] types { get { return Cache.types.Value; } }

        public SQLSysViews(IDataProvider provider) { Cache = new CacheClass(provider, this); }

        public sealed partial class columnsClass
        {
            readonly SQLSysViews _parent;
            internal columnsClass(DbDataRecord record, SQLSysViews parent)
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

            objectsClass _ObjectCache;
            public objectsClass Object { get { return _ObjectCache ?? (_ObjectCache = _parent.objects.Single(t => t.object_id == object_id)); } }

            typesClass _TypeCache;
            public typesClass Type { get { return _TypeCache ?? (_TypeCache = _parent.types.Single(t => t.user_type_id == user_type_id)); } }


        }
        public sealed partial class objectsClass
        {
            readonly SQLSysViews _parent;
            internal objectsClass(DbDataRecord record, SQLSysViews parent)
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
            public schemasClass Schema { get { return _SchemaCache ?? (_SchemaCache = _parent.schemas.Single(t => t.schema_id == schema_id)); } }


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

