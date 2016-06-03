#region Copyright (C) 2013

//     Project hwsound
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

using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Collections.Generic;
using System;
using hw.DebugFormatter;
using hw.Helper;

namespace main
{
    static class ReaderBatchExtension
    {
        internal static Sql Query(this string text, int? columnCount = null, int? rowCount = null) { return new Query(text, columnCount, rowCount); }
        internal static Sql NonQuery(this string text, int? recordsAffected) { return new NonQuery(text, recordsAffected); }
    }

    sealed class ReaderBatch
    {
        readonly Sql[] _sqls;
        readonly ValueCache<DbDataReader> _readerCache;
        int _recordsAffected;
        readonly ValueCache<string> _sqlTextCache;

        public ReaderBatch(SqlConnection connection, Sql[] sqls)
        {
            _sqls = sqls;
            _readerCache = new ValueCache<DbDataReader>(() => ObtainDataReader(connection, SqlText));
            _sqlTextCache = new ValueCache<string>(GetSqlText);
        }
        string GetSqlText() { return Profiler.Measure(() => _sqls.Select(t => t.Text).Stringify("\n")); }
        string SqlText { get { return _sqlTextCache.Value; } }

        DbDataReader ObtainDataReader(SqlConnection connection, string text)
        {
            _recordsAffected = 0;
            if(_sqls.Length > 100)
                return Profiler.Measure(() => connection.ToDataReader(text));
            return Profiler.Measure(() => connection.ToDataReader(text));
        }

        public IEnumerable<ResultSet> Data
        {
            get
            {
                Profiler.Measure(() => _readerCache.IsValid = true);
                var index = 0;
                do
                {
                    index = Profiler.Measure(() => ReadNonQuery(index));
                    if(Reader.FieldCount != 0)
                        yield return ReadQuery(index++);
                } while(Reader.NextResult());
                index = Profiler.Measure(() => ReadNonQuery(index));
                Tracer.Assert(_sqls.Length == index);
            }
        }

        DbDataReader Reader { get { return _readerCache.Value; } }

        ResultSet ReadQuery(int index)
        {
            var reader = _readerCache.Value;
            var query = _sqls[index] as Query;
            Tracer.Assert(query != null);
            Tracer.Assert(!HasAffectedRecords);
            var columns = reader.GetColumns();
            var resultSet = reader.SelectFromReader(Converter);
            Tracer.Assert(query.ColumnCount == null || query.ColumnCount.Value == columns.Length);
            Tracer.Assert(query.RowCount == null || query.RowCount.Value == resultSet.Length);
            return new ResultSet(columns, resultSet);
        }

        static object[] Converter(DbDataRecord arg)
        {
            var result = new object[arg.FieldCount];
            for(var i = 0; i < arg.FieldCount; i++)
                result[i] = arg[i];
            return result;
        }

        int ReadNonQuery(int sqlIndex)
        {
            var reader = _readerCache.Value;
            int? expectedRecordsAffected = null;
            var isExactRecordsAffected = true;
            while(sqlIndex < _sqls.Length && _sqls[sqlIndex] is NonQuery)
            {
                if(expectedRecordsAffected == null)
                    expectedRecordsAffected = 0;
                var affected = ((NonQuery) _sqls[sqlIndex]).RecordsAffected;

                if(affected != null)
                    expectedRecordsAffected += affected.Value;
                else
                    isExactRecordsAffected = false;
                sqlIndex++;
            }

            if(expectedRecordsAffected != null)
            {
                var recordsAffected = reader.RecordsAffected - _recordsAffected;
                if(isExactRecordsAffected)
                    Tracer.Assert(recordsAffected == expectedRecordsAffected.Value);
                else
                    Tracer.Assert(recordsAffected >= expectedRecordsAffected.Value);
                _recordsAffected = reader.RecordsAffected;
            }

            return sqlIndex;
        }

        bool HasAffectedRecords
        {
            get
            {
                var recordsAffected = _readerCache.Value.RecordsAffected;
                return recordsAffected > -1 && _recordsAffected != recordsAffected;
            }
        }

        public int Length { get { return _sqls.Length; } }

        internal void Reset() { _readerCache.IsValid = false; }
    }

    abstract class Sql
    {
        public readonly string Text;
        protected Sql(string text) { Text = text; }
    }

    sealed class NonQuery : Sql
    {
        public NonQuery(string text, int? recordsAffected)
            : base(text) { RecordsAffected = recordsAffected; }
        public readonly int? RecordsAffected;
    }

    sealed class Query : Sql
    {
        public Query(string text, int? columnCount = null, int? rowCount = null)
            : base(text)
        {
            ColumnCount = columnCount;
            RowCount = rowCount;
        }
        internal readonly int? ColumnCount;
        internal readonly int? RowCount;
    }

    sealed class ResultSet
    {
        internal readonly string[] Columns;
        internal readonly object[][] Data;
        public ResultSet(string[] columns, object[][] data)
        {
            Columns = columns;
            Data = data;
        }
    }
}