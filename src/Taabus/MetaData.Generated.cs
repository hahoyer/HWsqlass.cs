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
                CHECK_CONSTRAINTS = new ValueCache<CHECK_CONSTRAINTSClass[]>(() => dataBase.GetMetaData<CHECK_CONSTRAINTSClass>(r => new CHECK_CONSTRAINTSClass(r)));
                REFERENTIAL_CONSTRAINTS = new ValueCache<REFERENTIAL_CONSTRAINTSClass[]>(() => dataBase.GetMetaData<REFERENTIAL_CONSTRAINTSClass>(r => new REFERENTIAL_CONSTRAINTSClass(r)));
                COLUMN_DOMAIN_USAGE = new ValueCache<COLUMN_DOMAIN_USAGEClass[]>(() => dataBase.GetMetaData<COLUMN_DOMAIN_USAGEClass>(r => new COLUMN_DOMAIN_USAGEClass(r)));
                ROUTINES = new ValueCache<ROUTINESClass[]>(() => dataBase.GetMetaData<ROUTINESClass>(r => new ROUTINESClass(r)));
                COLUMN_PRIVILEGES = new ValueCache<COLUMN_PRIVILEGESClass[]>(() => dataBase.GetMetaData<COLUMN_PRIVILEGESClass>(r => new COLUMN_PRIVILEGESClass(r)));
                ROUTINE_COLUMNS = new ValueCache<ROUTINE_COLUMNSClass[]>(() => dataBase.GetMetaData<ROUTINE_COLUMNSClass>(r => new ROUTINE_COLUMNSClass(r)));
                COLUMNS = new ValueCache<COLUMNSClass[]>(() => dataBase.GetMetaData<COLUMNSClass>(r => new COLUMNSClass(r)));
                SCHEMATA = new ValueCache<SCHEMATAClass[]>(() => dataBase.GetMetaData<SCHEMATAClass>(r => new SCHEMATAClass(r)));
                CONSTRAINT_COLUMN_USAGE = new ValueCache<CONSTRAINT_COLUMN_USAGEClass[]>(() => dataBase.GetMetaData<CONSTRAINT_COLUMN_USAGEClass>(r => new CONSTRAINT_COLUMN_USAGEClass(r)));
                TABLE_CONSTRAINTS = new ValueCache<TABLE_CONSTRAINTSClass[]>(() => dataBase.GetMetaData<TABLE_CONSTRAINTSClass>(r => new TABLE_CONSTRAINTSClass(r)));
                CONSTRAINT_TABLE_USAGE = new ValueCache<CONSTRAINT_TABLE_USAGEClass[]>(() => dataBase.GetMetaData<CONSTRAINT_TABLE_USAGEClass>(r => new CONSTRAINT_TABLE_USAGEClass(r)));
                TABLE_PRIVILEGES = new ValueCache<TABLE_PRIVILEGESClass[]>(() => dataBase.GetMetaData<TABLE_PRIVILEGESClass>(r => new TABLE_PRIVILEGESClass(r)));
                DOMAIN_CONSTRAINTS = new ValueCache<DOMAIN_CONSTRAINTSClass[]>(() => dataBase.GetMetaData<DOMAIN_CONSTRAINTSClass>(r => new DOMAIN_CONSTRAINTSClass(r)));
                TABLES = new ValueCache<TABLESClass[]>(() => dataBase.GetMetaData<TABLESClass>(r => new TABLESClass(r)));
                DOMAINS = new ValueCache<DOMAINSClass[]>(() => dataBase.GetMetaData<DOMAINSClass>(r => new DOMAINSClass(r)));
                VIEW_COLUMN_USAGE = new ValueCache<VIEW_COLUMN_USAGEClass[]>(() => dataBase.GetMetaData<VIEW_COLUMN_USAGEClass>(r => new VIEW_COLUMN_USAGEClass(r)));
                KEY_COLUMN_USAGE = new ValueCache<KEY_COLUMN_USAGEClass[]>(() => dataBase.GetMetaData<KEY_COLUMN_USAGEClass>(r => new KEY_COLUMN_USAGEClass(r)));
                VIEW_TABLE_USAGE = new ValueCache<VIEW_TABLE_USAGEClass[]>(() => dataBase.GetMetaData<VIEW_TABLE_USAGEClass>(r => new VIEW_TABLE_USAGEClass(r)));
                PARAMETERS = new ValueCache<PARAMETERSClass[]>(() => dataBase.GetMetaData<PARAMETERSClass>(r => new PARAMETERSClass(r)));
                VIEWS = new ValueCache<VIEWSClass[]>(() => dataBase.GetMetaData<VIEWSClass>(r => new VIEWSClass(r)));
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
                CONSTRAINT_CATALOG = (string)record["CONSTRAINT_CATALOG"];
                CONSTRAINT_SCHEMA = (string)record["CONSTRAINT_SCHEMA"];
                CONSTRAINT_NAME = (string)record["CONSTRAINT_NAME"];
                CHECK_CLAUSE = (string)record["CHECK_CLAUSE"];
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
                CONSTRAINT_CATALOG = (string)record["CONSTRAINT_CATALOG"];
                CONSTRAINT_SCHEMA = (string)record["CONSTRAINT_SCHEMA"];
                CONSTRAINT_NAME = (string)record["CONSTRAINT_NAME"];
                UNIQUE_CONSTRAINT_CATALOG = (string)record["UNIQUE_CONSTRAINT_CATALOG"];
                UNIQUE_CONSTRAINT_SCHEMA = (string)record["UNIQUE_CONSTRAINT_SCHEMA"];
                UNIQUE_CONSTRAINT_NAME = (string)record["UNIQUE_CONSTRAINT_NAME"];
                MATCH_OPTION = (string)record["MATCH_OPTION"];
                UPDATE_RULE = (string)record["UPDATE_RULE"];
                DELETE_RULE = (string)record["DELETE_RULE"];
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
                DOMAIN_CATALOG = (string)record["DOMAIN_CATALOG"];
                DOMAIN_SCHEMA = (string)record["DOMAIN_SCHEMA"];
                DOMAIN_NAME = (string)record["DOMAIN_NAME"];
                TABLE_CATALOG = (string)record["TABLE_CATALOG"];
                TABLE_SCHEMA = (string)record["TABLE_SCHEMA"];
                TABLE_NAME = (string)record["TABLE_NAME"];
                COLUMN_NAME = (string)record["COLUMN_NAME"];
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
                SPECIFIC_CATALOG = (string)record["SPECIFIC_CATALOG"];
                SPECIFIC_SCHEMA = (string)record["SPECIFIC_SCHEMA"];
                SPECIFIC_NAME = (string)record["SPECIFIC_NAME"];
                ROUTINE_CATALOG = (string)record["ROUTINE_CATALOG"];
                ROUTINE_SCHEMA = (string)record["ROUTINE_SCHEMA"];
                ROUTINE_NAME = (string)record["ROUTINE_NAME"];
                ROUTINE_TYPE = (string)record["ROUTINE_TYPE"];
                MODULE_CATALOG = (string)record["MODULE_CATALOG"];
                MODULE_SCHEMA = (string)record["MODULE_SCHEMA"];
                MODULE_NAME = (string)record["MODULE_NAME"];
                UDT_CATALOG = (string)record["UDT_CATALOG"];
                UDT_SCHEMA = (string)record["UDT_SCHEMA"];
                UDT_NAME = (string)record["UDT_NAME"];
                DATA_TYPE = (string)record["DATA_TYPE"];
                CHARACTER_MAXIMUM_LENGTH = (int)record["CHARACTER_MAXIMUM_LENGTH"];
                CHARACTER_OCTET_LENGTH = (int)record["CHARACTER_OCTET_LENGTH"];
                COLLATION_CATALOG = (string)record["COLLATION_CATALOG"];
                COLLATION_SCHEMA = (string)record["COLLATION_SCHEMA"];
                COLLATION_NAME = (string)record["COLLATION_NAME"];
                CHARACTER_SET_CATALOG = (string)record["CHARACTER_SET_CATALOG"];
                CHARACTER_SET_SCHEMA = (string)record["CHARACTER_SET_SCHEMA"];
                CHARACTER_SET_NAME = (string)record["CHARACTER_SET_NAME"];
                NUMERIC_PRECISION = (Byte)record["NUMERIC_PRECISION"];
                NUMERIC_PRECISION_RADIX = (Int16)record["NUMERIC_PRECISION_RADIX"];
                NUMERIC_SCALE = (int)record["NUMERIC_SCALE"];
                DATETIME_PRECISION = (Int16)record["DATETIME_PRECISION"];
                INTERVAL_TYPE = (string)record["INTERVAL_TYPE"];
                INTERVAL_PRECISION = (Int16)record["INTERVAL_PRECISION"];
                TYPE_UDT_CATALOG = (string)record["TYPE_UDT_CATALOG"];
                TYPE_UDT_SCHEMA = (string)record["TYPE_UDT_SCHEMA"];
                TYPE_UDT_NAME = (string)record["TYPE_UDT_NAME"];
                SCOPE_CATALOG = (string)record["SCOPE_CATALOG"];
                SCOPE_SCHEMA = (string)record["SCOPE_SCHEMA"];
                SCOPE_NAME = (string)record["SCOPE_NAME"];
                MAXIMUM_CARDINALITY = (Int64)record["MAXIMUM_CARDINALITY"];
                DTD_IDENTIFIER = (string)record["DTD_IDENTIFIER"];
                ROUTINE_BODY = (string)record["ROUTINE_BODY"];
                ROUTINE_DEFINITION = (string)record["ROUTINE_DEFINITION"];
                EXTERNAL_NAME = (string)record["EXTERNAL_NAME"];
                EXTERNAL_LANGUAGE = (string)record["EXTERNAL_LANGUAGE"];
                PARAMETER_STYLE = (string)record["PARAMETER_STYLE"];
                IS_DETERMINISTIC = (string)record["IS_DETERMINISTIC"];
                SQL_DATA_ACCESS = (string)record["SQL_DATA_ACCESS"];
                IS_NULL_CALL = (string)record["IS_NULL_CALL"];
                SQL_PATH = (string)record["SQL_PATH"];
                SCHEMA_LEVEL_ROUTINE = (string)record["SCHEMA_LEVEL_ROUTINE"];
                MAX_DYNAMIC_RESULT_SETS = (Int16)record["MAX_DYNAMIC_RESULT_SETS"];
                IS_USER_DEFINED_CAST = (string)record["IS_USER_DEFINED_CAST"];
                IS_IMPLICITLY_INVOCABLE = (string)record["IS_IMPLICITLY_INVOCABLE"];
                CREATED = (DateTime)record["CREATED"];
                LAST_ALTERED = (DateTime)record["LAST_ALTERED"];
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
            public readonly int CHARACTER_MAXIMUM_LENGTH;
            public readonly int CHARACTER_OCTET_LENGTH;
            public readonly string COLLATION_CATALOG;
            public readonly string COLLATION_SCHEMA;
            public readonly string COLLATION_NAME;
            public readonly string CHARACTER_SET_CATALOG;
            public readonly string CHARACTER_SET_SCHEMA;
            public readonly string CHARACTER_SET_NAME;
            public readonly Byte NUMERIC_PRECISION;
            public readonly Int16 NUMERIC_PRECISION_RADIX;
            public readonly int NUMERIC_SCALE;
            public readonly Int16 DATETIME_PRECISION;
            public readonly string INTERVAL_TYPE;
            public readonly Int16 INTERVAL_PRECISION;
            public readonly string TYPE_UDT_CATALOG;
            public readonly string TYPE_UDT_SCHEMA;
            public readonly string TYPE_UDT_NAME;
            public readonly string SCOPE_CATALOG;
            public readonly string SCOPE_SCHEMA;
            public readonly string SCOPE_NAME;
            public readonly Int64 MAXIMUM_CARDINALITY;
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
            public readonly Int16 MAX_DYNAMIC_RESULT_SETS;
            public readonly string IS_USER_DEFINED_CAST;
            public readonly string IS_IMPLICITLY_INVOCABLE;
            public readonly DateTime CREATED;
            public readonly DateTime LAST_ALTERED;
        }
        public sealed class COLUMN_PRIVILEGESClass
        {
            internal COLUMN_PRIVILEGESClass(DbDataRecord record)
            {
                GRANTOR = (string)record["GRANTOR"];
                GRANTEE = (string)record["GRANTEE"];
                TABLE_CATALOG = (string)record["TABLE_CATALOG"];
                TABLE_SCHEMA = (string)record["TABLE_SCHEMA"];
                TABLE_NAME = (string)record["TABLE_NAME"];
                COLUMN_NAME = (string)record["COLUMN_NAME"];
                PRIVILEGE_TYPE = (string)record["PRIVILEGE_TYPE"];
                IS_GRANTABLE = (string)record["IS_GRANTABLE"];
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
                TABLE_CATALOG = (string)record["TABLE_CATALOG"];
                TABLE_SCHEMA = (string)record["TABLE_SCHEMA"];
                TABLE_NAME = (string)record["TABLE_NAME"];
                COLUMN_NAME = (string)record["COLUMN_NAME"];
                ORDINAL_POSITION = (int)record["ORDINAL_POSITION"];
                COLUMN_DEFAULT = (string)record["COLUMN_DEFAULT"];
                IS_NULLABLE = (string)record["IS_NULLABLE"];
                DATA_TYPE = (string)record["DATA_TYPE"];
                CHARACTER_MAXIMUM_LENGTH = (int)record["CHARACTER_MAXIMUM_LENGTH"];
                CHARACTER_OCTET_LENGTH = (int)record["CHARACTER_OCTET_LENGTH"];
                NUMERIC_PRECISION = (Byte)record["NUMERIC_PRECISION"];
                NUMERIC_PRECISION_RADIX = (Int16)record["NUMERIC_PRECISION_RADIX"];
                NUMERIC_SCALE = (int)record["NUMERIC_SCALE"];
                DATETIME_PRECISION = (Int16)record["DATETIME_PRECISION"];
                CHARACTER_SET_CATALOG = (string)record["CHARACTER_SET_CATALOG"];
                CHARACTER_SET_SCHEMA = (string)record["CHARACTER_SET_SCHEMA"];
                CHARACTER_SET_NAME = (string)record["CHARACTER_SET_NAME"];
                COLLATION_CATALOG = (string)record["COLLATION_CATALOG"];
                COLLATION_SCHEMA = (string)record["COLLATION_SCHEMA"];
                COLLATION_NAME = (string)record["COLLATION_NAME"];
                DOMAIN_CATALOG = (string)record["DOMAIN_CATALOG"];
                DOMAIN_SCHEMA = (string)record["DOMAIN_SCHEMA"];
                DOMAIN_NAME = (string)record["DOMAIN_NAME"];
            }

            public readonly string TABLE_CATALOG;
            public readonly string TABLE_SCHEMA;
            public readonly string TABLE_NAME;
            public readonly string COLUMN_NAME;
            public readonly int ORDINAL_POSITION;
            public readonly string COLUMN_DEFAULT;
            public readonly string IS_NULLABLE;
            public readonly string DATA_TYPE;
            public readonly int CHARACTER_MAXIMUM_LENGTH;
            public readonly int CHARACTER_OCTET_LENGTH;
            public readonly Byte NUMERIC_PRECISION;
            public readonly Int16 NUMERIC_PRECISION_RADIX;
            public readonly int NUMERIC_SCALE;
            public readonly Int16 DATETIME_PRECISION;
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
                TABLE_CATALOG = (string)record["TABLE_CATALOG"];
                TABLE_SCHEMA = (string)record["TABLE_SCHEMA"];
                TABLE_NAME = (string)record["TABLE_NAME"];
                COLUMN_NAME = (string)record["COLUMN_NAME"];
                ORDINAL_POSITION = (int)record["ORDINAL_POSITION"];
                COLUMN_DEFAULT = (string)record["COLUMN_DEFAULT"];
                IS_NULLABLE = (string)record["IS_NULLABLE"];
                DATA_TYPE = (string)record["DATA_TYPE"];
                CHARACTER_MAXIMUM_LENGTH = (int)record["CHARACTER_MAXIMUM_LENGTH"];
                CHARACTER_OCTET_LENGTH = (int)record["CHARACTER_OCTET_LENGTH"];
                NUMERIC_PRECISION = (Byte)record["NUMERIC_PRECISION"];
                NUMERIC_PRECISION_RADIX = (Int16)record["NUMERIC_PRECISION_RADIX"];
                NUMERIC_SCALE = (int)record["NUMERIC_SCALE"];
                DATETIME_PRECISION = (Int16)record["DATETIME_PRECISION"];
                CHARACTER_SET_CATALOG = (string)record["CHARACTER_SET_CATALOG"];
                CHARACTER_SET_SCHEMA = (string)record["CHARACTER_SET_SCHEMA"];
                CHARACTER_SET_NAME = (string)record["CHARACTER_SET_NAME"];
                COLLATION_CATALOG = (string)record["COLLATION_CATALOG"];
                COLLATION_SCHEMA = (string)record["COLLATION_SCHEMA"];
                COLLATION_NAME = (string)record["COLLATION_NAME"];
                DOMAIN_CATALOG = (string)record["DOMAIN_CATALOG"];
                DOMAIN_SCHEMA = (string)record["DOMAIN_SCHEMA"];
                DOMAIN_NAME = (string)record["DOMAIN_NAME"];
            }

            public readonly string TABLE_CATALOG;
            public readonly string TABLE_SCHEMA;
            public readonly string TABLE_NAME;
            public readonly string COLUMN_NAME;
            public readonly int ORDINAL_POSITION;
            public readonly string COLUMN_DEFAULT;
            public readonly string IS_NULLABLE;
            public readonly string DATA_TYPE;
            public readonly int CHARACTER_MAXIMUM_LENGTH;
            public readonly int CHARACTER_OCTET_LENGTH;
            public readonly Byte NUMERIC_PRECISION;
            public readonly Int16 NUMERIC_PRECISION_RADIX;
            public readonly int NUMERIC_SCALE;
            public readonly Int16 DATETIME_PRECISION;
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
                CATALOG_NAME = (string)record["CATALOG_NAME"];
                SCHEMA_NAME = (string)record["SCHEMA_NAME"];
                SCHEMA_OWNER = (string)record["SCHEMA_OWNER"];
                DEFAULT_CHARACTER_SET_CATALOG = (string)record["DEFAULT_CHARACTER_SET_CATALOG"];
                DEFAULT_CHARACTER_SET_SCHEMA = (string)record["DEFAULT_CHARACTER_SET_SCHEMA"];
                DEFAULT_CHARACTER_SET_NAME = (string)record["DEFAULT_CHARACTER_SET_NAME"];
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
                TABLE_CATALOG = (string)record["TABLE_CATALOG"];
                TABLE_SCHEMA = (string)record["TABLE_SCHEMA"];
                TABLE_NAME = (string)record["TABLE_NAME"];
                COLUMN_NAME = (string)record["COLUMN_NAME"];
                CONSTRAINT_CATALOG = (string)record["CONSTRAINT_CATALOG"];
                CONSTRAINT_SCHEMA = (string)record["CONSTRAINT_SCHEMA"];
                CONSTRAINT_NAME = (string)record["CONSTRAINT_NAME"];
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
                CONSTRAINT_CATALOG = (string)record["CONSTRAINT_CATALOG"];
                CONSTRAINT_SCHEMA = (string)record["CONSTRAINT_SCHEMA"];
                CONSTRAINT_NAME = (string)record["CONSTRAINT_NAME"];
                TABLE_CATALOG = (string)record["TABLE_CATALOG"];
                TABLE_SCHEMA = (string)record["TABLE_SCHEMA"];
                TABLE_NAME = (string)record["TABLE_NAME"];
                CONSTRAINT_TYPE = (string)record["CONSTRAINT_TYPE"];
                IS_DEFERRABLE = (string)record["IS_DEFERRABLE"];
                INITIALLY_DEFERRED = (string)record["INITIALLY_DEFERRED"];
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
                TABLE_CATALOG = (string)record["TABLE_CATALOG"];
                TABLE_SCHEMA = (string)record["TABLE_SCHEMA"];
                TABLE_NAME = (string)record["TABLE_NAME"];
                CONSTRAINT_CATALOG = (string)record["CONSTRAINT_CATALOG"];
                CONSTRAINT_SCHEMA = (string)record["CONSTRAINT_SCHEMA"];
                CONSTRAINT_NAME = (string)record["CONSTRAINT_NAME"];
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
                GRANTOR = (string)record["GRANTOR"];
                GRANTEE = (string)record["GRANTEE"];
                TABLE_CATALOG = (string)record["TABLE_CATALOG"];
                TABLE_SCHEMA = (string)record["TABLE_SCHEMA"];
                TABLE_NAME = (string)record["TABLE_NAME"];
                PRIVILEGE_TYPE = (string)record["PRIVILEGE_TYPE"];
                IS_GRANTABLE = (string)record["IS_GRANTABLE"];
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
                CONSTRAINT_CATALOG = (string)record["CONSTRAINT_CATALOG"];
                CONSTRAINT_SCHEMA = (string)record["CONSTRAINT_SCHEMA"];
                CONSTRAINT_NAME = (string)record["CONSTRAINT_NAME"];
                DOMAIN_CATALOG = (string)record["DOMAIN_CATALOG"];
                DOMAIN_SCHEMA = (string)record["DOMAIN_SCHEMA"];
                DOMAIN_NAME = (string)record["DOMAIN_NAME"];
                IS_DEFERRABLE = (string)record["IS_DEFERRABLE"];
                INITIALLY_DEFERRED = (string)record["INITIALLY_DEFERRED"];
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
                TABLE_CATALOG = (string)record["TABLE_CATALOG"];
                TABLE_SCHEMA = (string)record["TABLE_SCHEMA"];
                TABLE_NAME = (string)record["TABLE_NAME"];
                TABLE_TYPE = (string)record["TABLE_TYPE"];
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
                DOMAIN_CATALOG = (string)record["DOMAIN_CATALOG"];
                DOMAIN_SCHEMA = (string)record["DOMAIN_SCHEMA"];
                DOMAIN_NAME = (string)record["DOMAIN_NAME"];
                DATA_TYPE = (string)record["DATA_TYPE"];
                CHARACTER_MAXIMUM_LENGTH = (int)record["CHARACTER_MAXIMUM_LENGTH"];
                CHARACTER_OCTET_LENGTH = (int)record["CHARACTER_OCTET_LENGTH"];
                COLLATION_CATALOG = (string)record["COLLATION_CATALOG"];
                COLLATION_SCHEMA = (string)record["COLLATION_SCHEMA"];
                COLLATION_NAME = (string)record["COLLATION_NAME"];
                CHARACTER_SET_CATALOG = (string)record["CHARACTER_SET_CATALOG"];
                CHARACTER_SET_SCHEMA = (string)record["CHARACTER_SET_SCHEMA"];
                CHARACTER_SET_NAME = (string)record["CHARACTER_SET_NAME"];
                NUMERIC_PRECISION = (Byte)record["NUMERIC_PRECISION"];
                NUMERIC_PRECISION_RADIX = (Int16)record["NUMERIC_PRECISION_RADIX"];
                NUMERIC_SCALE = (int)record["NUMERIC_SCALE"];
                DATETIME_PRECISION = (Int16)record["DATETIME_PRECISION"];
                DOMAIN_DEFAULT = (string)record["DOMAIN_DEFAULT"];
            }

            public readonly string DOMAIN_CATALOG;
            public readonly string DOMAIN_SCHEMA;
            public readonly string DOMAIN_NAME;
            public readonly string DATA_TYPE;
            public readonly int CHARACTER_MAXIMUM_LENGTH;
            public readonly int CHARACTER_OCTET_LENGTH;
            public readonly string COLLATION_CATALOG;
            public readonly string COLLATION_SCHEMA;
            public readonly string COLLATION_NAME;
            public readonly string CHARACTER_SET_CATALOG;
            public readonly string CHARACTER_SET_SCHEMA;
            public readonly string CHARACTER_SET_NAME;
            public readonly Byte NUMERIC_PRECISION;
            public readonly Int16 NUMERIC_PRECISION_RADIX;
            public readonly int NUMERIC_SCALE;
            public readonly Int16 DATETIME_PRECISION;
            public readonly string DOMAIN_DEFAULT;
        }
        public sealed class VIEW_COLUMN_USAGEClass
        {
            internal VIEW_COLUMN_USAGEClass(DbDataRecord record)
            {
                VIEW_CATALOG = (string)record["VIEW_CATALOG"];
                VIEW_SCHEMA = (string)record["VIEW_SCHEMA"];
                VIEW_NAME = (string)record["VIEW_NAME"];
                TABLE_CATALOG = (string)record["TABLE_CATALOG"];
                TABLE_SCHEMA = (string)record["TABLE_SCHEMA"];
                TABLE_NAME = (string)record["TABLE_NAME"];
                COLUMN_NAME = (string)record["COLUMN_NAME"];
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
                CONSTRAINT_CATALOG = (string)record["CONSTRAINT_CATALOG"];
                CONSTRAINT_SCHEMA = (string)record["CONSTRAINT_SCHEMA"];
                CONSTRAINT_NAME = (string)record["CONSTRAINT_NAME"];
                TABLE_CATALOG = (string)record["TABLE_CATALOG"];
                TABLE_SCHEMA = (string)record["TABLE_SCHEMA"];
                TABLE_NAME = (string)record["TABLE_NAME"];
                COLUMN_NAME = (string)record["COLUMN_NAME"];
                ORDINAL_POSITION = (int)record["ORDINAL_POSITION"];
            }

            public readonly string CONSTRAINT_CATALOG;
            public readonly string CONSTRAINT_SCHEMA;
            public readonly string CONSTRAINT_NAME;
            public readonly string TABLE_CATALOG;
            public readonly string TABLE_SCHEMA;
            public readonly string TABLE_NAME;
            public readonly string COLUMN_NAME;
            public readonly int ORDINAL_POSITION;
        }
        public sealed class VIEW_TABLE_USAGEClass
        {
            internal VIEW_TABLE_USAGEClass(DbDataRecord record)
            {
                VIEW_CATALOG = (string)record["VIEW_CATALOG"];
                VIEW_SCHEMA = (string)record["VIEW_SCHEMA"];
                VIEW_NAME = (string)record["VIEW_NAME"];
                TABLE_CATALOG = (string)record["TABLE_CATALOG"];
                TABLE_SCHEMA = (string)record["TABLE_SCHEMA"];
                TABLE_NAME = (string)record["TABLE_NAME"];
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
                SPECIFIC_CATALOG = (string)record["SPECIFIC_CATALOG"];
                SPECIFIC_SCHEMA = (string)record["SPECIFIC_SCHEMA"];
                SPECIFIC_NAME = (string)record["SPECIFIC_NAME"];
                ORDINAL_POSITION = (int)record["ORDINAL_POSITION"];
                PARAMETER_MODE = (string)record["PARAMETER_MODE"];
                IS_RESULT = (string)record["IS_RESULT"];
                AS_LOCATOR = (string)record["AS_LOCATOR"];
                PARAMETER_NAME = (string)record["PARAMETER_NAME"];
                DATA_TYPE = (string)record["DATA_TYPE"];
                CHARACTER_MAXIMUM_LENGTH = (int)record["CHARACTER_MAXIMUM_LENGTH"];
                CHARACTER_OCTET_LENGTH = (int)record["CHARACTER_OCTET_LENGTH"];
                COLLATION_CATALOG = (string)record["COLLATION_CATALOG"];
                COLLATION_SCHEMA = (string)record["COLLATION_SCHEMA"];
                COLLATION_NAME = (string)record["COLLATION_NAME"];
                CHARACTER_SET_CATALOG = (string)record["CHARACTER_SET_CATALOG"];
                CHARACTER_SET_SCHEMA = (string)record["CHARACTER_SET_SCHEMA"];
                CHARACTER_SET_NAME = (string)record["CHARACTER_SET_NAME"];
                NUMERIC_PRECISION = (Byte)record["NUMERIC_PRECISION"];
                NUMERIC_PRECISION_RADIX = (Int16)record["NUMERIC_PRECISION_RADIX"];
                NUMERIC_SCALE = (int)record["NUMERIC_SCALE"];
                DATETIME_PRECISION = (Int16)record["DATETIME_PRECISION"];
                INTERVAL_TYPE = (string)record["INTERVAL_TYPE"];
                INTERVAL_PRECISION = (Int16)record["INTERVAL_PRECISION"];
                USER_DEFINED_TYPE_CATALOG = (string)record["USER_DEFINED_TYPE_CATALOG"];
                USER_DEFINED_TYPE_SCHEMA = (string)record["USER_DEFINED_TYPE_SCHEMA"];
                USER_DEFINED_TYPE_NAME = (string)record["USER_DEFINED_TYPE_NAME"];
                SCOPE_CATALOG = (string)record["SCOPE_CATALOG"];
                SCOPE_SCHEMA = (string)record["SCOPE_SCHEMA"];
                SCOPE_NAME = (string)record["SCOPE_NAME"];
            }

            public readonly string SPECIFIC_CATALOG;
            public readonly string SPECIFIC_SCHEMA;
            public readonly string SPECIFIC_NAME;
            public readonly int ORDINAL_POSITION;
            public readonly string PARAMETER_MODE;
            public readonly string IS_RESULT;
            public readonly string AS_LOCATOR;
            public readonly string PARAMETER_NAME;
            public readonly string DATA_TYPE;
            public readonly int CHARACTER_MAXIMUM_LENGTH;
            public readonly int CHARACTER_OCTET_LENGTH;
            public readonly string COLLATION_CATALOG;
            public readonly string COLLATION_SCHEMA;
            public readonly string COLLATION_NAME;
            public readonly string CHARACTER_SET_CATALOG;
            public readonly string CHARACTER_SET_SCHEMA;
            public readonly string CHARACTER_SET_NAME;
            public readonly Byte NUMERIC_PRECISION;
            public readonly Int16 NUMERIC_PRECISION_RADIX;
            public readonly int NUMERIC_SCALE;
            public readonly Int16 DATETIME_PRECISION;
            public readonly string INTERVAL_TYPE;
            public readonly Int16 INTERVAL_PRECISION;
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
                TABLE_CATALOG = (string)record["TABLE_CATALOG"];
                TABLE_SCHEMA = (string)record["TABLE_SCHEMA"];
                TABLE_NAME = (string)record["TABLE_NAME"];
                VIEW_DEFINITION = (string)record["VIEW_DEFINITION"];
                CHECK_OPTION = (string)record["CHECK_OPTION"];
                IS_UPDATABLE = (string)record["IS_UPDATABLE"];
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

