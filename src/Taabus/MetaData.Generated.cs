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

namespace Taabus
{
    public class MetaData
    {
        public class CacheClass
        {
            public readonly ValueCache<CHECK_CONSTRAINTSClass[]> CHECK_CONSTRAINTS;
            public readonly ValueCache<REFERENTIAL_CONSTRAINTSClass[]> REFERENTIAL_CONSTRAINTS;
            public readonly ValueCache<COLUMN_DOMAIN_USAGEClass[]> COLUMN_DOMAIN_USAGE;
            public readonly ValueCache<ROUTINESClass[]> ROUTINES;
            public readonly ValueCache<COLUMN_PRIVILEGESClass[]> COLUMN_PRIVILEGES;
            public readonly ValueCache<ROUTINE_COLUMNSClass[]> ROUTINE_COLUMNS;
            public readonly ValueCache<COLUMNSClass[]> COLUMNS;
            public readonly ValueCache<SCHEMATAClass[]> SCHEMATA;
            public readonly ValueCache<CONSTRAINT_COLUMN_USAGEClass[]> CONSTRAINT_COLUMN_USAGE;
            public readonly ValueCache<TABLE_CONSTRAINTSClass[]> TABLE_CONSTRAINTS;
            public readonly ValueCache<CONSTRAINT_TABLE_USAGEClass[]> CONSTRAINT_TABLE_USAGE;
            public readonly ValueCache<TABLE_PRIVILEGESClass[]> TABLE_PRIVILEGES;
            public readonly ValueCache<DOMAIN_CONSTRAINTSClass[]> DOMAIN_CONSTRAINTS;
            public readonly ValueCache<TABLESClass[]> TABLES;
            public readonly ValueCache<DOMAINSClass[]> DOMAINS;
            public readonly ValueCache<VIEW_COLUMN_USAGEClass[]> VIEW_COLUMN_USAGE;
            public readonly ValueCache<KEY_COLUMN_USAGEClass[]> KEY_COLUMN_USAGE;
            public readonly ValueCache<VIEW_TABLE_USAGEClass[]> VIEW_TABLE_USAGE;
            public readonly ValueCache<PARAMETERSClass[]> PARAMETERS;
            public readonly ValueCache<VIEWSClass[]> VIEWS;

            public CacheClass(DataBase dataBase)
            {
                CHECK_CONSTRAINTS = new ValueCache<CHECK_CONSTRAINTSClass[]>(() => dataBase.GetMetaData<CHECK_CONSTRAINTSClass>("CHECK_CONSTRAINTS", r => new CHECK_CONSTRAINTSClass(r)));
                REFERENTIAL_CONSTRAINTS = new ValueCache<REFERENTIAL_CONSTRAINTSClass[]>(() => dataBase.GetMetaData<REFERENTIAL_CONSTRAINTSClass>("REFERENTIAL_CONSTRAINTS", r => new REFERENTIAL_CONSTRAINTSClass(r)));
                COLUMN_DOMAIN_USAGE = new ValueCache<COLUMN_DOMAIN_USAGEClass[]>(() => dataBase.GetMetaData<COLUMN_DOMAIN_USAGEClass>("COLUMN_DOMAIN_USAGE", r => new COLUMN_DOMAIN_USAGEClass(r)));
                ROUTINES = new ValueCache<ROUTINESClass[]>(() => dataBase.GetMetaData<ROUTINESClass>("ROUTINES", r => new ROUTINESClass(r)));
                COLUMN_PRIVILEGES = new ValueCache<COLUMN_PRIVILEGESClass[]>(() => dataBase.GetMetaData<COLUMN_PRIVILEGESClass>("COLUMN_PRIVILEGES", r => new COLUMN_PRIVILEGESClass(r)));
                ROUTINE_COLUMNS = new ValueCache<ROUTINE_COLUMNSClass[]>(() => dataBase.GetMetaData<ROUTINE_COLUMNSClass>("ROUTINE_COLUMNS", r => new ROUTINE_COLUMNSClass(r)));
                COLUMNS = new ValueCache<COLUMNSClass[]>(() => dataBase.GetMetaData<COLUMNSClass>("COLUMNS", r => new COLUMNSClass(r)));
                SCHEMATA = new ValueCache<SCHEMATAClass[]>(() => dataBase.GetMetaData<SCHEMATAClass>("SCHEMATA", r => new SCHEMATAClass(r)));
                CONSTRAINT_COLUMN_USAGE = new ValueCache<CONSTRAINT_COLUMN_USAGEClass[]>(() => dataBase.GetMetaData<CONSTRAINT_COLUMN_USAGEClass>("CONSTRAINT_COLUMN_USAGE", r => new CONSTRAINT_COLUMN_USAGEClass(r)));
                TABLE_CONSTRAINTS = new ValueCache<TABLE_CONSTRAINTSClass[]>(() => dataBase.GetMetaData<TABLE_CONSTRAINTSClass>("TABLE_CONSTRAINTS", r => new TABLE_CONSTRAINTSClass(r)));
                CONSTRAINT_TABLE_USAGE = new ValueCache<CONSTRAINT_TABLE_USAGEClass[]>(() => dataBase.GetMetaData<CONSTRAINT_TABLE_USAGEClass>("CONSTRAINT_TABLE_USAGE", r => new CONSTRAINT_TABLE_USAGEClass(r)));
                TABLE_PRIVILEGES = new ValueCache<TABLE_PRIVILEGESClass[]>(() => dataBase.GetMetaData<TABLE_PRIVILEGESClass>("TABLE_PRIVILEGES", r => new TABLE_PRIVILEGESClass(r)));
                DOMAIN_CONSTRAINTS = new ValueCache<DOMAIN_CONSTRAINTSClass[]>(() => dataBase.GetMetaData<DOMAIN_CONSTRAINTSClass>("DOMAIN_CONSTRAINTS", r => new DOMAIN_CONSTRAINTSClass(r)));
                TABLES = new ValueCache<TABLESClass[]>(() => dataBase.GetMetaData<TABLESClass>("TABLES", r => new TABLESClass(r)));
                DOMAINS = new ValueCache<DOMAINSClass[]>(() => dataBase.GetMetaData<DOMAINSClass>("DOMAINS", r => new DOMAINSClass(r)));
                VIEW_COLUMN_USAGE = new ValueCache<VIEW_COLUMN_USAGEClass[]>(() => dataBase.GetMetaData<VIEW_COLUMN_USAGEClass>("VIEW_COLUMN_USAGE", r => new VIEW_COLUMN_USAGEClass(r)));
                KEY_COLUMN_USAGE = new ValueCache<KEY_COLUMN_USAGEClass[]>(() => dataBase.GetMetaData<KEY_COLUMN_USAGEClass>("KEY_COLUMN_USAGE", r => new KEY_COLUMN_USAGEClass(r)));
                VIEW_TABLE_USAGE = new ValueCache<VIEW_TABLE_USAGEClass[]>(() => dataBase.GetMetaData<VIEW_TABLE_USAGEClass>("VIEW_TABLE_USAGE", r => new VIEW_TABLE_USAGEClass(r)));
                PARAMETERS = new ValueCache<PARAMETERSClass[]>(() => dataBase.GetMetaData<PARAMETERSClass>("PARAMETERS", r => new PARAMETERSClass(r)));
                VIEWS = new ValueCache<VIEWSClass[]>(() => dataBase.GetMetaData<VIEWSClass>("VIEWS", r => new VIEWSClass(r)));
            }

        }

        public readonly CacheClass Cache;

        public CHECK_CONSTRAINTSClass[] CHECK_CONSTRAINTS { get { return Cache.CHECK_CONSTRAINTS.Value; } }
        public REFERENTIAL_CONSTRAINTSClass[] REFERENTIAL_CONSTRAINTS { get { return Cache.REFERENTIAL_CONSTRAINTS.Value; } }
        public COLUMN_DOMAIN_USAGEClass[] COLUMN_DOMAIN_USAGE { get { return Cache.COLUMN_DOMAIN_USAGE.Value; } }
        public ROUTINESClass[] ROUTINES { get { return Cache.ROUTINES.Value; } }
        public COLUMN_PRIVILEGESClass[] COLUMN_PRIVILEGES { get { return Cache.COLUMN_PRIVILEGES.Value; } }
        public ROUTINE_COLUMNSClass[] ROUTINE_COLUMNS { get { return Cache.ROUTINE_COLUMNS.Value; } }
        public COLUMNSClass[] COLUMNS { get { return Cache.COLUMNS.Value; } }
        public SCHEMATAClass[] SCHEMATA { get { return Cache.SCHEMATA.Value; } }
        public CONSTRAINT_COLUMN_USAGEClass[] CONSTRAINT_COLUMN_USAGE { get { return Cache.CONSTRAINT_COLUMN_USAGE.Value; } }
        public TABLE_CONSTRAINTSClass[] TABLE_CONSTRAINTS { get { return Cache.TABLE_CONSTRAINTS.Value; } }
        public CONSTRAINT_TABLE_USAGEClass[] CONSTRAINT_TABLE_USAGE { get { return Cache.CONSTRAINT_TABLE_USAGE.Value; } }
        public TABLE_PRIVILEGESClass[] TABLE_PRIVILEGES { get { return Cache.TABLE_PRIVILEGES.Value; } }
        public DOMAIN_CONSTRAINTSClass[] DOMAIN_CONSTRAINTS { get { return Cache.DOMAIN_CONSTRAINTS.Value; } }
        public TABLESClass[] TABLES { get { return Cache.TABLES.Value; } }
        public DOMAINSClass[] DOMAINS { get { return Cache.DOMAINS.Value; } }
        public VIEW_COLUMN_USAGEClass[] VIEW_COLUMN_USAGE { get { return Cache.VIEW_COLUMN_USAGE.Value; } }
        public KEY_COLUMN_USAGEClass[] KEY_COLUMN_USAGE { get { return Cache.KEY_COLUMN_USAGE.Value; } }
        public VIEW_TABLE_USAGEClass[] VIEW_TABLE_USAGE { get { return Cache.VIEW_TABLE_USAGE.Value; } }
        public PARAMETERSClass[] PARAMETERS { get { return Cache.PARAMETERS.Value; } }
        public VIEWSClass[] VIEWS { get { return Cache.VIEWS.Value; } }

        public MetaData(DataBase dataBase) { Cache = new CacheClass(dataBase); }

        public sealed class CHECK_CONSTRAINTSClass
        {
            internal CHECK_CONSTRAINTSClass(DbDataRecord record)
            {
                CONSTRAINT_CATALOG = record["CONSTRAINT_CATALOG"].Convert<string>();
                CONSTRAINT_SCHEMA = record["CONSTRAINT_SCHEMA"].Convert<string>();
                CONSTRAINT_NAME = record["CONSTRAINT_NAME"].Convert<string>();
                CHECK_CLAUSE = record["CHECK_CLAUSE"].Convert<string>();
            }

            public readonly string CONSTRAINT_CATALOG;
            public readonly string CONSTRAINT_SCHEMA;
            public readonly string CONSTRAINT_NAME;
            public readonly string CHECK_CLAUSE;
        }
        public sealed class REFERENTIAL_CONSTRAINTSClass
        {
            internal REFERENTIAL_CONSTRAINTSClass(DbDataRecord record)
            {
                CONSTRAINT_CATALOG = record["CONSTRAINT_CATALOG"].Convert<string>();
                CONSTRAINT_SCHEMA = record["CONSTRAINT_SCHEMA"].Convert<string>();
                CONSTRAINT_NAME = record["CONSTRAINT_NAME"].Convert<string>();
                UNIQUE_CONSTRAINT_CATALOG = record["UNIQUE_CONSTRAINT_CATALOG"].Convert<string>();
                UNIQUE_CONSTRAINT_SCHEMA = record["UNIQUE_CONSTRAINT_SCHEMA"].Convert<string>();
                UNIQUE_CONSTRAINT_NAME = record["UNIQUE_CONSTRAINT_NAME"].Convert<string>();
                MATCH_OPTION = record["MATCH_OPTION"].Convert<string>();
                UPDATE_RULE = record["UPDATE_RULE"].Convert<string>();
                DELETE_RULE = record["DELETE_RULE"].Convert<string>();
            }

            public readonly string CONSTRAINT_CATALOG;
            public readonly string CONSTRAINT_SCHEMA;
            public readonly string CONSTRAINT_NAME;
            public readonly string UNIQUE_CONSTRAINT_CATALOG;
            public readonly string UNIQUE_CONSTRAINT_SCHEMA;
            public readonly string UNIQUE_CONSTRAINT_NAME;
            public readonly string MATCH_OPTION;
            public readonly string UPDATE_RULE;
            public readonly string DELETE_RULE;
        }
        public sealed class COLUMN_DOMAIN_USAGEClass
        {
            internal COLUMN_DOMAIN_USAGEClass(DbDataRecord record)
            {
                DOMAIN_CATALOG = record["DOMAIN_CATALOG"].Convert<string>();
                DOMAIN_SCHEMA = record["DOMAIN_SCHEMA"].Convert<string>();
                DOMAIN_NAME = record["DOMAIN_NAME"].Convert<string>();
                TABLE_CATALOG = record["TABLE_CATALOG"].Convert<string>();
                TABLE_SCHEMA = record["TABLE_SCHEMA"].Convert<string>();
                TABLE_NAME = record["TABLE_NAME"].Convert<string>();
                COLUMN_NAME = record["COLUMN_NAME"].Convert<string>();
            }

            public readonly string DOMAIN_CATALOG;
            public readonly string DOMAIN_SCHEMA;
            public readonly string DOMAIN_NAME;
            public readonly string TABLE_CATALOG;
            public readonly string TABLE_SCHEMA;
            public readonly string TABLE_NAME;
            public readonly string COLUMN_NAME;
        }
        public sealed class ROUTINESClass
        {
            internal ROUTINESClass(DbDataRecord record)
            {
                SPECIFIC_CATALOG = record["SPECIFIC_CATALOG"].Convert<string>();
                SPECIFIC_SCHEMA = record["SPECIFIC_SCHEMA"].Convert<string>();
                SPECIFIC_NAME = record["SPECIFIC_NAME"].Convert<string>();
                ROUTINE_CATALOG = record["ROUTINE_CATALOG"].Convert<string>();
                ROUTINE_SCHEMA = record["ROUTINE_SCHEMA"].Convert<string>();
                ROUTINE_NAME = record["ROUTINE_NAME"].Convert<string>();
                ROUTINE_TYPE = record["ROUTINE_TYPE"].Convert<string>();
                MODULE_CATALOG = record["MODULE_CATALOG"].Convert<string>();
                MODULE_SCHEMA = record["MODULE_SCHEMA"].Convert<string>();
                MODULE_NAME = record["MODULE_NAME"].Convert<string>();
                UDT_CATALOG = record["UDT_CATALOG"].Convert<string>();
                UDT_SCHEMA = record["UDT_SCHEMA"].Convert<string>();
                UDT_NAME = record["UDT_NAME"].Convert<string>();
                DATA_TYPE = record["DATA_TYPE"].Convert<string>();
                CHARACTER_MAXIMUM_LENGTH = record["CHARACTER_MAXIMUM_LENGTH"].Convert<int?>();
                CHARACTER_OCTET_LENGTH = record["CHARACTER_OCTET_LENGTH"].Convert<int?>();
                COLLATION_CATALOG = record["COLLATION_CATALOG"].Convert<string>();
                COLLATION_SCHEMA = record["COLLATION_SCHEMA"].Convert<string>();
                COLLATION_NAME = record["COLLATION_NAME"].Convert<string>();
                CHARACTER_SET_CATALOG = record["CHARACTER_SET_CATALOG"].Convert<string>();
                CHARACTER_SET_SCHEMA = record["CHARACTER_SET_SCHEMA"].Convert<string>();
                CHARACTER_SET_NAME = record["CHARACTER_SET_NAME"].Convert<string>();
                NUMERIC_PRECISION = record["NUMERIC_PRECISION"].Convert<Byte?>();
                NUMERIC_PRECISION_RADIX = record["NUMERIC_PRECISION_RADIX"].Convert<Int16?>();
                NUMERIC_SCALE = record["NUMERIC_SCALE"].Convert<int?>();
                DATETIME_PRECISION = record["DATETIME_PRECISION"].Convert<Int16?>();
                INTERVAL_TYPE = record["INTERVAL_TYPE"].Convert<string>();
                INTERVAL_PRECISION = record["INTERVAL_PRECISION"].Convert<Int16?>();
                TYPE_UDT_CATALOG = record["TYPE_UDT_CATALOG"].Convert<string>();
                TYPE_UDT_SCHEMA = record["TYPE_UDT_SCHEMA"].Convert<string>();
                TYPE_UDT_NAME = record["TYPE_UDT_NAME"].Convert<string>();
                SCOPE_CATALOG = record["SCOPE_CATALOG"].Convert<string>();
                SCOPE_SCHEMA = record["SCOPE_SCHEMA"].Convert<string>();
                SCOPE_NAME = record["SCOPE_NAME"].Convert<string>();
                MAXIMUM_CARDINALITY = record["MAXIMUM_CARDINALITY"].Convert<Int64?>();
                DTD_IDENTIFIER = record["DTD_IDENTIFIER"].Convert<string>();
                ROUTINE_BODY = record["ROUTINE_BODY"].Convert<string>();
                ROUTINE_DEFINITION = record["ROUTINE_DEFINITION"].Convert<string>();
                EXTERNAL_NAME = record["EXTERNAL_NAME"].Convert<string>();
                EXTERNAL_LANGUAGE = record["EXTERNAL_LANGUAGE"].Convert<string>();
                PARAMETER_STYLE = record["PARAMETER_STYLE"].Convert<string>();
                IS_DETERMINISTIC = record["IS_DETERMINISTIC"].Convert<string>();
                SQL_DATA_ACCESS = record["SQL_DATA_ACCESS"].Convert<string>();
                IS_NULL_CALL = record["IS_NULL_CALL"].Convert<string>();
                SQL_PATH = record["SQL_PATH"].Convert<string>();
                SCHEMA_LEVEL_ROUTINE = record["SCHEMA_LEVEL_ROUTINE"].Convert<string>();
                MAX_DYNAMIC_RESULT_SETS = record["MAX_DYNAMIC_RESULT_SETS"].Convert<Int16?>();
                IS_USER_DEFINED_CAST = record["IS_USER_DEFINED_CAST"].Convert<string>();
                IS_IMPLICITLY_INVOCABLE = record["IS_IMPLICITLY_INVOCABLE"].Convert<string>();
                CREATED = record["CREATED"].Convert<DateTime?>();
                LAST_ALTERED = record["LAST_ALTERED"].Convert<DateTime?>();
            }

            public readonly string SPECIFIC_CATALOG;
            public readonly string SPECIFIC_SCHEMA;
            public readonly string SPECIFIC_NAME;
            public readonly string ROUTINE_CATALOG;
            public readonly string ROUTINE_SCHEMA;
            public readonly string ROUTINE_NAME;
            public readonly string ROUTINE_TYPE;
            public readonly string MODULE_CATALOG;
            public readonly string MODULE_SCHEMA;
            public readonly string MODULE_NAME;
            public readonly string UDT_CATALOG;
            public readonly string UDT_SCHEMA;
            public readonly string UDT_NAME;
            public readonly string DATA_TYPE;
            public readonly int? CHARACTER_MAXIMUM_LENGTH;
            public readonly int? CHARACTER_OCTET_LENGTH;
            public readonly string COLLATION_CATALOG;
            public readonly string COLLATION_SCHEMA;
            public readonly string COLLATION_NAME;
            public readonly string CHARACTER_SET_CATALOG;
            public readonly string CHARACTER_SET_SCHEMA;
            public readonly string CHARACTER_SET_NAME;
            public readonly Byte? NUMERIC_PRECISION;
            public readonly Int16? NUMERIC_PRECISION_RADIX;
            public readonly int? NUMERIC_SCALE;
            public readonly Int16? DATETIME_PRECISION;
            public readonly string INTERVAL_TYPE;
            public readonly Int16? INTERVAL_PRECISION;
            public readonly string TYPE_UDT_CATALOG;
            public readonly string TYPE_UDT_SCHEMA;
            public readonly string TYPE_UDT_NAME;
            public readonly string SCOPE_CATALOG;
            public readonly string SCOPE_SCHEMA;
            public readonly string SCOPE_NAME;
            public readonly Int64? MAXIMUM_CARDINALITY;
            public readonly string DTD_IDENTIFIER;
            public readonly string ROUTINE_BODY;
            public readonly string ROUTINE_DEFINITION;
            public readonly string EXTERNAL_NAME;
            public readonly string EXTERNAL_LANGUAGE;
            public readonly string PARAMETER_STYLE;
            public readonly string IS_DETERMINISTIC;
            public readonly string SQL_DATA_ACCESS;
            public readonly string IS_NULL_CALL;
            public readonly string SQL_PATH;
            public readonly string SCHEMA_LEVEL_ROUTINE;
            public readonly Int16? MAX_DYNAMIC_RESULT_SETS;
            public readonly string IS_USER_DEFINED_CAST;
            public readonly string IS_IMPLICITLY_INVOCABLE;
            public readonly DateTime? CREATED;
            public readonly DateTime? LAST_ALTERED;
        }
        public sealed class COLUMN_PRIVILEGESClass
        {
            internal COLUMN_PRIVILEGESClass(DbDataRecord record)
            {
                GRANTOR = record["GRANTOR"].Convert<string>();
                GRANTEE = record["GRANTEE"].Convert<string>();
                TABLE_CATALOG = record["TABLE_CATALOG"].Convert<string>();
                TABLE_SCHEMA = record["TABLE_SCHEMA"].Convert<string>();
                TABLE_NAME = record["TABLE_NAME"].Convert<string>();
                COLUMN_NAME = record["COLUMN_NAME"].Convert<string>();
                PRIVILEGE_TYPE = record["PRIVILEGE_TYPE"].Convert<string>();
                IS_GRANTABLE = record["IS_GRANTABLE"].Convert<string>();
            }

            public readonly string GRANTOR;
            public readonly string GRANTEE;
            public readonly string TABLE_CATALOG;
            public readonly string TABLE_SCHEMA;
            public readonly string TABLE_NAME;
            public readonly string COLUMN_NAME;
            public readonly string PRIVILEGE_TYPE;
            public readonly string IS_GRANTABLE;
        }
        public sealed class ROUTINE_COLUMNSClass
        {
            internal ROUTINE_COLUMNSClass(DbDataRecord record)
            {
                TABLE_CATALOG = record["TABLE_CATALOG"].Convert<string>();
                TABLE_SCHEMA = record["TABLE_SCHEMA"].Convert<string>();
                TABLE_NAME = record["TABLE_NAME"].Convert<string>();
                COLUMN_NAME = record["COLUMN_NAME"].Convert<string>();
                ORDINAL_POSITION = record["ORDINAL_POSITION"].Convert<int?>();
                COLUMN_DEFAULT = record["COLUMN_DEFAULT"].Convert<string>();
                IS_NULLABLE = record["IS_NULLABLE"].Convert<string>();
                DATA_TYPE = record["DATA_TYPE"].Convert<string>();
                CHARACTER_MAXIMUM_LENGTH = record["CHARACTER_MAXIMUM_LENGTH"].Convert<int?>();
                CHARACTER_OCTET_LENGTH = record["CHARACTER_OCTET_LENGTH"].Convert<int?>();
                NUMERIC_PRECISION = record["NUMERIC_PRECISION"].Convert<Byte?>();
                NUMERIC_PRECISION_RADIX = record["NUMERIC_PRECISION_RADIX"].Convert<Int16?>();
                NUMERIC_SCALE = record["NUMERIC_SCALE"].Convert<int?>();
                DATETIME_PRECISION = record["DATETIME_PRECISION"].Convert<Int16?>();
                CHARACTER_SET_CATALOG = record["CHARACTER_SET_CATALOG"].Convert<string>();
                CHARACTER_SET_SCHEMA = record["CHARACTER_SET_SCHEMA"].Convert<string>();
                CHARACTER_SET_NAME = record["CHARACTER_SET_NAME"].Convert<string>();
                COLLATION_CATALOG = record["COLLATION_CATALOG"].Convert<string>();
                COLLATION_SCHEMA = record["COLLATION_SCHEMA"].Convert<string>();
                COLLATION_NAME = record["COLLATION_NAME"].Convert<string>();
                DOMAIN_CATALOG = record["DOMAIN_CATALOG"].Convert<string>();
                DOMAIN_SCHEMA = record["DOMAIN_SCHEMA"].Convert<string>();
                DOMAIN_NAME = record["DOMAIN_NAME"].Convert<string>();
            }

            public readonly string TABLE_CATALOG;
            public readonly string TABLE_SCHEMA;
            public readonly string TABLE_NAME;
            public readonly string COLUMN_NAME;
            public readonly int? ORDINAL_POSITION;
            public readonly string COLUMN_DEFAULT;
            public readonly string IS_NULLABLE;
            public readonly string DATA_TYPE;
            public readonly int? CHARACTER_MAXIMUM_LENGTH;
            public readonly int? CHARACTER_OCTET_LENGTH;
            public readonly Byte? NUMERIC_PRECISION;
            public readonly Int16? NUMERIC_PRECISION_RADIX;
            public readonly int? NUMERIC_SCALE;
            public readonly Int16? DATETIME_PRECISION;
            public readonly string CHARACTER_SET_CATALOG;
            public readonly string CHARACTER_SET_SCHEMA;
            public readonly string CHARACTER_SET_NAME;
            public readonly string COLLATION_CATALOG;
            public readonly string COLLATION_SCHEMA;
            public readonly string COLLATION_NAME;
            public readonly string DOMAIN_CATALOG;
            public readonly string DOMAIN_SCHEMA;
            public readonly string DOMAIN_NAME;
        }
        public sealed class COLUMNSClass
        {
            internal COLUMNSClass(DbDataRecord record)
            {
                TABLE_CATALOG = record["TABLE_CATALOG"].Convert<string>();
                TABLE_SCHEMA = record["TABLE_SCHEMA"].Convert<string>();
                TABLE_NAME = record["TABLE_NAME"].Convert<string>();
                COLUMN_NAME = record["COLUMN_NAME"].Convert<string>();
                ORDINAL_POSITION = record["ORDINAL_POSITION"].Convert<int?>();
                COLUMN_DEFAULT = record["COLUMN_DEFAULT"].Convert<string>();
                IS_NULLABLE = record["IS_NULLABLE"].Convert<string>();
                DATA_TYPE = record["DATA_TYPE"].Convert<string>();
                CHARACTER_MAXIMUM_LENGTH = record["CHARACTER_MAXIMUM_LENGTH"].Convert<int?>();
                CHARACTER_OCTET_LENGTH = record["CHARACTER_OCTET_LENGTH"].Convert<int?>();
                NUMERIC_PRECISION = record["NUMERIC_PRECISION"].Convert<Byte?>();
                NUMERIC_PRECISION_RADIX = record["NUMERIC_PRECISION_RADIX"].Convert<Int16?>();
                NUMERIC_SCALE = record["NUMERIC_SCALE"].Convert<int?>();
                DATETIME_PRECISION = record["DATETIME_PRECISION"].Convert<Int16?>();
                CHARACTER_SET_CATALOG = record["CHARACTER_SET_CATALOG"].Convert<string>();
                CHARACTER_SET_SCHEMA = record["CHARACTER_SET_SCHEMA"].Convert<string>();
                CHARACTER_SET_NAME = record["CHARACTER_SET_NAME"].Convert<string>();
                COLLATION_CATALOG = record["COLLATION_CATALOG"].Convert<string>();
                COLLATION_SCHEMA = record["COLLATION_SCHEMA"].Convert<string>();
                COLLATION_NAME = record["COLLATION_NAME"].Convert<string>();
                DOMAIN_CATALOG = record["DOMAIN_CATALOG"].Convert<string>();
                DOMAIN_SCHEMA = record["DOMAIN_SCHEMA"].Convert<string>();
                DOMAIN_NAME = record["DOMAIN_NAME"].Convert<string>();
            }

            public readonly string TABLE_CATALOG;
            public readonly string TABLE_SCHEMA;
            public readonly string TABLE_NAME;
            public readonly string COLUMN_NAME;
            public readonly int? ORDINAL_POSITION;
            public readonly string COLUMN_DEFAULT;
            public readonly string IS_NULLABLE;
            public readonly string DATA_TYPE;
            public readonly int? CHARACTER_MAXIMUM_LENGTH;
            public readonly int? CHARACTER_OCTET_LENGTH;
            public readonly Byte? NUMERIC_PRECISION;
            public readonly Int16? NUMERIC_PRECISION_RADIX;
            public readonly int? NUMERIC_SCALE;
            public readonly Int16? DATETIME_PRECISION;
            public readonly string CHARACTER_SET_CATALOG;
            public readonly string CHARACTER_SET_SCHEMA;
            public readonly string CHARACTER_SET_NAME;
            public readonly string COLLATION_CATALOG;
            public readonly string COLLATION_SCHEMA;
            public readonly string COLLATION_NAME;
            public readonly string DOMAIN_CATALOG;
            public readonly string DOMAIN_SCHEMA;
            public readonly string DOMAIN_NAME;
        }
        public sealed class SCHEMATAClass
        {
            internal SCHEMATAClass(DbDataRecord record)
            {
                CATALOG_NAME = record["CATALOG_NAME"].Convert<string>();
                SCHEMA_NAME = record["SCHEMA_NAME"].Convert<string>();
                SCHEMA_OWNER = record["SCHEMA_OWNER"].Convert<string>();
                DEFAULT_CHARACTER_SET_CATALOG = record["DEFAULT_CHARACTER_SET_CATALOG"].Convert<string>();
                DEFAULT_CHARACTER_SET_SCHEMA = record["DEFAULT_CHARACTER_SET_SCHEMA"].Convert<string>();
                DEFAULT_CHARACTER_SET_NAME = record["DEFAULT_CHARACTER_SET_NAME"].Convert<string>();
            }

            public readonly string CATALOG_NAME;
            public readonly string SCHEMA_NAME;
            public readonly string SCHEMA_OWNER;
            public readonly string DEFAULT_CHARACTER_SET_CATALOG;
            public readonly string DEFAULT_CHARACTER_SET_SCHEMA;
            public readonly string DEFAULT_CHARACTER_SET_NAME;
        }
        public sealed class CONSTRAINT_COLUMN_USAGEClass
        {
            internal CONSTRAINT_COLUMN_USAGEClass(DbDataRecord record)
            {
                TABLE_CATALOG = record["TABLE_CATALOG"].Convert<string>();
                TABLE_SCHEMA = record["TABLE_SCHEMA"].Convert<string>();
                TABLE_NAME = record["TABLE_NAME"].Convert<string>();
                COLUMN_NAME = record["COLUMN_NAME"].Convert<string>();
                CONSTRAINT_CATALOG = record["CONSTRAINT_CATALOG"].Convert<string>();
                CONSTRAINT_SCHEMA = record["CONSTRAINT_SCHEMA"].Convert<string>();
                CONSTRAINT_NAME = record["CONSTRAINT_NAME"].Convert<string>();
            }

            public readonly string TABLE_CATALOG;
            public readonly string TABLE_SCHEMA;
            public readonly string TABLE_NAME;
            public readonly string COLUMN_NAME;
            public readonly string CONSTRAINT_CATALOG;
            public readonly string CONSTRAINT_SCHEMA;
            public readonly string CONSTRAINT_NAME;
        }
        public sealed class TABLE_CONSTRAINTSClass
        {
            internal TABLE_CONSTRAINTSClass(DbDataRecord record)
            {
                CONSTRAINT_CATALOG = record["CONSTRAINT_CATALOG"].Convert<string>();
                CONSTRAINT_SCHEMA = record["CONSTRAINT_SCHEMA"].Convert<string>();
                CONSTRAINT_NAME = record["CONSTRAINT_NAME"].Convert<string>();
                TABLE_CATALOG = record["TABLE_CATALOG"].Convert<string>();
                TABLE_SCHEMA = record["TABLE_SCHEMA"].Convert<string>();
                TABLE_NAME = record["TABLE_NAME"].Convert<string>();
                CONSTRAINT_TYPE = record["CONSTRAINT_TYPE"].Convert<string>();
                IS_DEFERRABLE = record["IS_DEFERRABLE"].Convert<string>();
                INITIALLY_DEFERRED = record["INITIALLY_DEFERRED"].Convert<string>();
            }

            public readonly string CONSTRAINT_CATALOG;
            public readonly string CONSTRAINT_SCHEMA;
            public readonly string CONSTRAINT_NAME;
            public readonly string TABLE_CATALOG;
            public readonly string TABLE_SCHEMA;
            public readonly string TABLE_NAME;
            public readonly string CONSTRAINT_TYPE;
            public readonly string IS_DEFERRABLE;
            public readonly string INITIALLY_DEFERRED;
        }
        public sealed class CONSTRAINT_TABLE_USAGEClass
        {
            internal CONSTRAINT_TABLE_USAGEClass(DbDataRecord record)
            {
                TABLE_CATALOG = record["TABLE_CATALOG"].Convert<string>();
                TABLE_SCHEMA = record["TABLE_SCHEMA"].Convert<string>();
                TABLE_NAME = record["TABLE_NAME"].Convert<string>();
                CONSTRAINT_CATALOG = record["CONSTRAINT_CATALOG"].Convert<string>();
                CONSTRAINT_SCHEMA = record["CONSTRAINT_SCHEMA"].Convert<string>();
                CONSTRAINT_NAME = record["CONSTRAINT_NAME"].Convert<string>();
            }

            public readonly string TABLE_CATALOG;
            public readonly string TABLE_SCHEMA;
            public readonly string TABLE_NAME;
            public readonly string CONSTRAINT_CATALOG;
            public readonly string CONSTRAINT_SCHEMA;
            public readonly string CONSTRAINT_NAME;
        }
        public sealed class TABLE_PRIVILEGESClass
        {
            internal TABLE_PRIVILEGESClass(DbDataRecord record)
            {
                GRANTOR = record["GRANTOR"].Convert<string>();
                GRANTEE = record["GRANTEE"].Convert<string>();
                TABLE_CATALOG = record["TABLE_CATALOG"].Convert<string>();
                TABLE_SCHEMA = record["TABLE_SCHEMA"].Convert<string>();
                TABLE_NAME = record["TABLE_NAME"].Convert<string>();
                PRIVILEGE_TYPE = record["PRIVILEGE_TYPE"].Convert<string>();
                IS_GRANTABLE = record["IS_GRANTABLE"].Convert<string>();
            }

            public readonly string GRANTOR;
            public readonly string GRANTEE;
            public readonly string TABLE_CATALOG;
            public readonly string TABLE_SCHEMA;
            public readonly string TABLE_NAME;
            public readonly string PRIVILEGE_TYPE;
            public readonly string IS_GRANTABLE;
        }
        public sealed class DOMAIN_CONSTRAINTSClass
        {
            internal DOMAIN_CONSTRAINTSClass(DbDataRecord record)
            {
                CONSTRAINT_CATALOG = record["CONSTRAINT_CATALOG"].Convert<string>();
                CONSTRAINT_SCHEMA = record["CONSTRAINT_SCHEMA"].Convert<string>();
                CONSTRAINT_NAME = record["CONSTRAINT_NAME"].Convert<string>();
                DOMAIN_CATALOG = record["DOMAIN_CATALOG"].Convert<string>();
                DOMAIN_SCHEMA = record["DOMAIN_SCHEMA"].Convert<string>();
                DOMAIN_NAME = record["DOMAIN_NAME"].Convert<string>();
                IS_DEFERRABLE = record["IS_DEFERRABLE"].Convert<string>();
                INITIALLY_DEFERRED = record["INITIALLY_DEFERRED"].Convert<string>();
            }

            public readonly string CONSTRAINT_CATALOG;
            public readonly string CONSTRAINT_SCHEMA;
            public readonly string CONSTRAINT_NAME;
            public readonly string DOMAIN_CATALOG;
            public readonly string DOMAIN_SCHEMA;
            public readonly string DOMAIN_NAME;
            public readonly string IS_DEFERRABLE;
            public readonly string INITIALLY_DEFERRED;
        }
        public sealed class TABLESClass
        {
            internal TABLESClass(DbDataRecord record)
            {
                TABLE_CATALOG = record["TABLE_CATALOG"].Convert<string>();
                TABLE_SCHEMA = record["TABLE_SCHEMA"].Convert<string>();
                TABLE_NAME = record["TABLE_NAME"].Convert<string>();
                TABLE_TYPE = record["TABLE_TYPE"].Convert<string>();
            }

            public readonly string TABLE_CATALOG;
            public readonly string TABLE_SCHEMA;
            public readonly string TABLE_NAME;
            public readonly string TABLE_TYPE;
        }
        public sealed class DOMAINSClass
        {
            internal DOMAINSClass(DbDataRecord record)
            {
                DOMAIN_CATALOG = record["DOMAIN_CATALOG"].Convert<string>();
                DOMAIN_SCHEMA = record["DOMAIN_SCHEMA"].Convert<string>();
                DOMAIN_NAME = record["DOMAIN_NAME"].Convert<string>();
                DATA_TYPE = record["DATA_TYPE"].Convert<string>();
                CHARACTER_MAXIMUM_LENGTH = record["CHARACTER_MAXIMUM_LENGTH"].Convert<int?>();
                CHARACTER_OCTET_LENGTH = record["CHARACTER_OCTET_LENGTH"].Convert<int?>();
                COLLATION_CATALOG = record["COLLATION_CATALOG"].Convert<string>();
                COLLATION_SCHEMA = record["COLLATION_SCHEMA"].Convert<string>();
                COLLATION_NAME = record["COLLATION_NAME"].Convert<string>();
                CHARACTER_SET_CATALOG = record["CHARACTER_SET_CATALOG"].Convert<string>();
                CHARACTER_SET_SCHEMA = record["CHARACTER_SET_SCHEMA"].Convert<string>();
                CHARACTER_SET_NAME = record["CHARACTER_SET_NAME"].Convert<string>();
                NUMERIC_PRECISION = record["NUMERIC_PRECISION"].Convert<Byte?>();
                NUMERIC_PRECISION_RADIX = record["NUMERIC_PRECISION_RADIX"].Convert<Int16?>();
                NUMERIC_SCALE = record["NUMERIC_SCALE"].Convert<int?>();
                DATETIME_PRECISION = record["DATETIME_PRECISION"].Convert<Int16?>();
                DOMAIN_DEFAULT = record["DOMAIN_DEFAULT"].Convert<string>();
            }

            public readonly string DOMAIN_CATALOG;
            public readonly string DOMAIN_SCHEMA;
            public readonly string DOMAIN_NAME;
            public readonly string DATA_TYPE;
            public readonly int? CHARACTER_MAXIMUM_LENGTH;
            public readonly int? CHARACTER_OCTET_LENGTH;
            public readonly string COLLATION_CATALOG;
            public readonly string COLLATION_SCHEMA;
            public readonly string COLLATION_NAME;
            public readonly string CHARACTER_SET_CATALOG;
            public readonly string CHARACTER_SET_SCHEMA;
            public readonly string CHARACTER_SET_NAME;
            public readonly Byte? NUMERIC_PRECISION;
            public readonly Int16? NUMERIC_PRECISION_RADIX;
            public readonly int? NUMERIC_SCALE;
            public readonly Int16? DATETIME_PRECISION;
            public readonly string DOMAIN_DEFAULT;
        }
        public sealed class VIEW_COLUMN_USAGEClass
        {
            internal VIEW_COLUMN_USAGEClass(DbDataRecord record)
            {
                VIEW_CATALOG = record["VIEW_CATALOG"].Convert<string>();
                VIEW_SCHEMA = record["VIEW_SCHEMA"].Convert<string>();
                VIEW_NAME = record["VIEW_NAME"].Convert<string>();
                TABLE_CATALOG = record["TABLE_CATALOG"].Convert<string>();
                TABLE_SCHEMA = record["TABLE_SCHEMA"].Convert<string>();
                TABLE_NAME = record["TABLE_NAME"].Convert<string>();
                COLUMN_NAME = record["COLUMN_NAME"].Convert<string>();
            }

            public readonly string VIEW_CATALOG;
            public readonly string VIEW_SCHEMA;
            public readonly string VIEW_NAME;
            public readonly string TABLE_CATALOG;
            public readonly string TABLE_SCHEMA;
            public readonly string TABLE_NAME;
            public readonly string COLUMN_NAME;
        }
        public sealed class KEY_COLUMN_USAGEClass
        {
            internal KEY_COLUMN_USAGEClass(DbDataRecord record)
            {
                CONSTRAINT_CATALOG = record["CONSTRAINT_CATALOG"].Convert<string>();
                CONSTRAINT_SCHEMA = record["CONSTRAINT_SCHEMA"].Convert<string>();
                CONSTRAINT_NAME = record["CONSTRAINT_NAME"].Convert<string>();
                TABLE_CATALOG = record["TABLE_CATALOG"].Convert<string>();
                TABLE_SCHEMA = record["TABLE_SCHEMA"].Convert<string>();
                TABLE_NAME = record["TABLE_NAME"].Convert<string>();
                COLUMN_NAME = record["COLUMN_NAME"].Convert<string>();
                ORDINAL_POSITION = record["ORDINAL_POSITION"].Convert<int?>();
            }

            public readonly string CONSTRAINT_CATALOG;
            public readonly string CONSTRAINT_SCHEMA;
            public readonly string CONSTRAINT_NAME;
            public readonly string TABLE_CATALOG;
            public readonly string TABLE_SCHEMA;
            public readonly string TABLE_NAME;
            public readonly string COLUMN_NAME;
            public readonly int? ORDINAL_POSITION;
        }
        public sealed class VIEW_TABLE_USAGEClass
        {
            internal VIEW_TABLE_USAGEClass(DbDataRecord record)
            {
                VIEW_CATALOG = record["VIEW_CATALOG"].Convert<string>();
                VIEW_SCHEMA = record["VIEW_SCHEMA"].Convert<string>();
                VIEW_NAME = record["VIEW_NAME"].Convert<string>();
                TABLE_CATALOG = record["TABLE_CATALOG"].Convert<string>();
                TABLE_SCHEMA = record["TABLE_SCHEMA"].Convert<string>();
                TABLE_NAME = record["TABLE_NAME"].Convert<string>();
            }

            public readonly string VIEW_CATALOG;
            public readonly string VIEW_SCHEMA;
            public readonly string VIEW_NAME;
            public readonly string TABLE_CATALOG;
            public readonly string TABLE_SCHEMA;
            public readonly string TABLE_NAME;
        }
        public sealed class PARAMETERSClass
        {
            internal PARAMETERSClass(DbDataRecord record)
            {
                SPECIFIC_CATALOG = record["SPECIFIC_CATALOG"].Convert<string>();
                SPECIFIC_SCHEMA = record["SPECIFIC_SCHEMA"].Convert<string>();
                SPECIFIC_NAME = record["SPECIFIC_NAME"].Convert<string>();
                ORDINAL_POSITION = record["ORDINAL_POSITION"].Convert<int?>();
                PARAMETER_MODE = record["PARAMETER_MODE"].Convert<string>();
                IS_RESULT = record["IS_RESULT"].Convert<string>();
                AS_LOCATOR = record["AS_LOCATOR"].Convert<string>();
                PARAMETER_NAME = record["PARAMETER_NAME"].Convert<string>();
                DATA_TYPE = record["DATA_TYPE"].Convert<string>();
                CHARACTER_MAXIMUM_LENGTH = record["CHARACTER_MAXIMUM_LENGTH"].Convert<int?>();
                CHARACTER_OCTET_LENGTH = record["CHARACTER_OCTET_LENGTH"].Convert<int?>();
                COLLATION_CATALOG = record["COLLATION_CATALOG"].Convert<string>();
                COLLATION_SCHEMA = record["COLLATION_SCHEMA"].Convert<string>();
                COLLATION_NAME = record["COLLATION_NAME"].Convert<string>();
                CHARACTER_SET_CATALOG = record["CHARACTER_SET_CATALOG"].Convert<string>();
                CHARACTER_SET_SCHEMA = record["CHARACTER_SET_SCHEMA"].Convert<string>();
                CHARACTER_SET_NAME = record["CHARACTER_SET_NAME"].Convert<string>();
                NUMERIC_PRECISION = record["NUMERIC_PRECISION"].Convert<Byte?>();
                NUMERIC_PRECISION_RADIX = record["NUMERIC_PRECISION_RADIX"].Convert<Int16?>();
                NUMERIC_SCALE = record["NUMERIC_SCALE"].Convert<int?>();
                DATETIME_PRECISION = record["DATETIME_PRECISION"].Convert<Int16?>();
                INTERVAL_TYPE = record["INTERVAL_TYPE"].Convert<string>();
                INTERVAL_PRECISION = record["INTERVAL_PRECISION"].Convert<Int16?>();
                USER_DEFINED_TYPE_CATALOG = record["USER_DEFINED_TYPE_CATALOG"].Convert<string>();
                USER_DEFINED_TYPE_SCHEMA = record["USER_DEFINED_TYPE_SCHEMA"].Convert<string>();
                USER_DEFINED_TYPE_NAME = record["USER_DEFINED_TYPE_NAME"].Convert<string>();
                SCOPE_CATALOG = record["SCOPE_CATALOG"].Convert<string>();
                SCOPE_SCHEMA = record["SCOPE_SCHEMA"].Convert<string>();
                SCOPE_NAME = record["SCOPE_NAME"].Convert<string>();
            }

            public readonly string SPECIFIC_CATALOG;
            public readonly string SPECIFIC_SCHEMA;
            public readonly string SPECIFIC_NAME;
            public readonly int? ORDINAL_POSITION;
            public readonly string PARAMETER_MODE;
            public readonly string IS_RESULT;
            public readonly string AS_LOCATOR;
            public readonly string PARAMETER_NAME;
            public readonly string DATA_TYPE;
            public readonly int? CHARACTER_MAXIMUM_LENGTH;
            public readonly int? CHARACTER_OCTET_LENGTH;
            public readonly string COLLATION_CATALOG;
            public readonly string COLLATION_SCHEMA;
            public readonly string COLLATION_NAME;
            public readonly string CHARACTER_SET_CATALOG;
            public readonly string CHARACTER_SET_SCHEMA;
            public readonly string CHARACTER_SET_NAME;
            public readonly Byte? NUMERIC_PRECISION;
            public readonly Int16? NUMERIC_PRECISION_RADIX;
            public readonly int? NUMERIC_SCALE;
            public readonly Int16? DATETIME_PRECISION;
            public readonly string INTERVAL_TYPE;
            public readonly Int16? INTERVAL_PRECISION;
            public readonly string USER_DEFINED_TYPE_CATALOG;
            public readonly string USER_DEFINED_TYPE_SCHEMA;
            public readonly string USER_DEFINED_TYPE_NAME;
            public readonly string SCOPE_CATALOG;
            public readonly string SCOPE_SCHEMA;
            public readonly string SCOPE_NAME;
        }
        public sealed class VIEWSClass
        {
            internal VIEWSClass(DbDataRecord record)
            {
                TABLE_CATALOG = record["TABLE_CATALOG"].Convert<string>();
                TABLE_SCHEMA = record["TABLE_SCHEMA"].Convert<string>();
                TABLE_NAME = record["TABLE_NAME"].Convert<string>();
                VIEW_DEFINITION = record["VIEW_DEFINITION"].Convert<string>();
                CHECK_OPTION = record["CHECK_OPTION"].Convert<string>();
                IS_UPDATABLE = record["IS_UPDATABLE"].Convert<string>();
            }

            public readonly string TABLE_CATALOG;
            public readonly string TABLE_SCHEMA;
            public readonly string TABLE_NAME;
            public readonly string VIEW_DEFINITION;
            public readonly string CHECK_OPTION;
            public readonly string IS_UPDATABLE;
        }

    }
}

