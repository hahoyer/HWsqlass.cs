using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Linq.Expressions;

namespace hw.Helper
{
    public static class AdoExtension
    {
        public static DataTable GetSchemaTable(this DbConnection connection, string text)
        {
            return connection
                .ToDataReader(text)
                .GetSchemaTable();
        }

        public static DbDataReader ToDataReader(this DbConnection connection, string text)
        {
            return connection
                .ToCommand(text)
                .ExecuteReader();
        }

        public static T[] ToArray<T>(this DbConnection connection, string text)
            where T : IReaderInitialize, new()
        {
            return connection
                .ToDataReader(text)
                .ToArray<T>();
        }

        public static DbCommand ToCommand(this DbConnection connection, string text)
        {
            var command = connection.CreateCommand();
            command.CommandText = text;
            return command;
        }

        public static T[] ToArray<T>(this DbDataReader reader) where T : IReaderInitialize, new()
        {
            var result = new List<T>();
            while(reader.Read())
            {
                var t = new T();
                t.Initialize(reader);
                result.Add(t);
            }
            return result.ToArray();
        }

        public static T[] SelectFromReader<T>(this DbDataReader reader, Func<DbDataRecord, T> converter)
        {
            var result = new List<T>();
            var e = reader.GetEnumerator();
            while(e.MoveNext())
                result.Add(converter((DbDataRecord) e.Current));
            return result.ToArray();
        }

        public static string[] GetColumns(this DbDataReader reader)
        {
            var result = new string[reader.FieldCount];
            for(var i = 0; i < reader.FieldCount; i++)
                result[i] = reader.GetName(i);
            return result;
        }

        public static DateTime EnsureSqlDateTime(this DateTime dateTime) { return new SqlDateTime(dateTime).Value; }

        public static SqlConnection ToConnection(this string serverName, string dataBase = null)
        {
            return ConnectionString(serverName, dataBase)
                .ToConnection();
        }

        internal static SqlConnection ToConnection(this SqlConnectionStringBuilder connectionString)
        {
            var connection = new SqlConnection(connectionString.ConnectionString);
            connection.Open();
            return connection;
        }

        static SqlConnectionStringBuilder ConnectionString(string serverName, string dataBase)
        {
            return new SqlConnectionStringBuilder
            {
                DataSource = serverName,
                IntegratedSecurity = true,
                MultipleActiveResultSets = true,
                InitialCatalog = dataBase
            };
        }

        public static string SQLFormat(this string data) { return "'" + data.Replace("'", "''") + "'"; }
    }
}