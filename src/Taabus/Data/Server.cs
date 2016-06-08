using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using hw.DebugFormatter;
using hw.Forms;
using hw.Helper;

namespace Taabus.Data
{
    [Serializer.Enable]
    public sealed class Server 
        : NamedObject
        , ITreeNodeSupport
        , ITreeNodeProbeSupport
        , INodeNameProvider 
        , IIconKeyProvider
        , IEquatable<Server>
    {
        const string SelectDatabases = "select name from master.sys.databases";

        readonly ValueCache<DataBase[]> _dataBasesCache;
        readonly ValueCache<SqlConnectionStringBuilder> _sqlConnectionStringBuilderCache;

        string _connectionString;

        public Server()
        {
            _sqlConnectionStringBuilderCache = new ValueCache<SqlConnectionStringBuilder>(() => new SqlConnectionStringBuilder(_connectionString));
            _dataBasesCache = new ValueCache<DataBase[]>(GetDataBases);
        }

        [Serializer.Disable]
        internal DataBase[] DataBases { get { return _dataBasesCache.Value; } }

        public string ConnectionString
        {
            get { return _connectionString; }
            set
            {
                _connectionString = value;
                _sqlConnectionStringBuilderCache.IsValid = false;
                _dataBasesCache.IsValid = false;
            }
        }

        bool IEquatable<Server>.Equals(Server other) { return ConnectionString == other.ConnectionString; }

        IEnumerable<TreeNode> ITreeNodeSupport.CreateNodes() { return DataBases.CreateNodes(); }
        bool ITreeNodeProbeSupport.IsEmpty { get { return DataBases == null || !DataBases.Any(); } }
        string INodeNameProvider.Value(string name) { return "ConnectionString=" + _connectionString; }
        string IIconKeyProvider.IconKey { get { return "Server"; } }

        DataBase[] GetDataBases()
        {
            if(string.IsNullOrEmpty(_connectionString))
                return null;
            return Select(SelectDatabases, record => DataBase.Create(record, this));
        }

        internal T[] Select<T>(string statement, Func<DbDataRecord, T> func)
        {
            return ToDataReader(statement)
                .SelectFromReader(func);
        }

        internal DbDataReader ToDataReader(string statement)
        {
            return _sqlConnectionStringBuilderCache
                .Value
                .ToConnection()
                .ToDataReader(statement);
        }

        [DisableDump,Serializer.Disable]
        internal string DataSource { get { return _sqlConnectionStringBuilderCache.Value.DataSource; } set { _sqlConnectionStringBuilderCache.Value.DataSource = value; } }

        [Serializer.Disable]
        public override string Name
        {
            get
            {
                if(ConnectionString == null)
                    return null;
                var builder = _sqlConnectionStringBuilderCache.Value;
                var result = builder.DataSource;
                if(builder.InitialCatalog != "")
                    result += "(" + builder.InitialCatalog + ")";
                if(builder.IntegratedSecurity)
                    return result;
                return builder.UserID + "@" + result;
            }
        }
        
    }
}