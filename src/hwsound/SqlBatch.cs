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
using hw.Debug;
using hw.Helper;

namespace main
{
    sealed class SqlBatch
    {
        readonly SqlConnection _connection;

        SqlBatch(string server, string dataBase)
        {
            var connectionString = new DbConnectionStringBuilder();
            connectionString["Data Source"] = server;
            connectionString["Initial Catalog"] = dataBase;
            connectionString["Integrated Security"] = "SSPI";
            connectionString["MultipleActiveResultSets"] = true;
            _connection = new SqlConnection(connectionString.ConnectionString);
            _connection.Open();

            const string trunc = "truncate table test\n";
            const string insert = "insert into test (Id, Name) values ({0}, '{1}')";
            _connection
                .ToDataReader
                (
                    trunc
                    + 1.While(i => i <= 4)
                       .Select(i => insert.ReplaceArgs(i, "text" + i))
                       .Stringify("\n")
                );
        }

        void RunWithMeasure()
        {
            Profiler.Reset();
            var s2 = "update test set Name = 'updated' where Id = 1".NonQuery(1);
            var s0 = "select * from Test where Id = 1".Query(rowCount: 1, columnCount: 3);
            var sqls = new[] {s2, s0};
            const int count = 10000;
            Profiler.Measure(() => Check(count, ReaderBatch(sqls)));
            Profiler.Measure(() => Check(ReaderBatch(1.While(i => i < count).SelectMany(i => sqls).ToArray())));
            Tracer.Line(Profiler.Format(hidden: 0));
        }

        static void Check(int count, ReaderBatch readerBatch)
        {
            for(var i = 0; i < count; i++)
                Check(readerBatch);
        }

        void FlatRun()
        {
            var s0 = "select * from Test where Id = 1".Query(rowCount: 1, columnCount: 3);
            Check(s0);

            var s0a = "select * from Test where Id = -1".Query(rowCount: 0, columnCount: 3);
            Check(s0a);

            var s1 = "select * from Test".Query(rowCount: 4, columnCount: 3);
            Check(s1);

            var s2 = "update test set Name = 'updated' where Id = 1".NonQuery(1);
            Check(s2);

            var s2a = "update test set Name = 'updated' where Id = -1".NonQuery(0);
            Check(s2a);

            var s3 = "delete from test where Id = 1".NonQuery(1);
            Check(s3);

            var s4 = "insert into test (Id, Name) values (1, 'inserted')".NonQuery(1);
            Check(s4);

            Check(ReaderBatch(s0, s0));
            Check(ReaderBatch(s0, s0a));
            Check(ReaderBatch(s0a, s0));
            Check(ReaderBatch(s0a, s0a));
            Check(ReaderBatch(s0, s1));
            Check(ReaderBatch(s1, s0));

            Check(ReaderBatch(s0, s2));
            Check(ReaderBatch(s2, s0));

            Check(ReaderBatch(s2, s2));
            Check(ReaderBatch(s2, s2, s0));
            Check(ReaderBatch(s0, s2, s2));

            Check(ReaderBatch(s2, s0, s0));
            Check(ReaderBatch(s0, s0, s2));

            Check(ReaderBatch(s2, s0, s0, s2));
            Check(ReaderBatch(s0, s0, s2, s2));

            Check(ReaderBatch(s2, s2, s0, s0));
            Check(ReaderBatch(s2, s0, s0, s2));

            Check(ReaderBatch(s2, s1, s0));

            var s41 = "insert into test (Id, Name) values (41, 'inserted')".NonQuery(1);
            var s42 = "insert into test (Id, Name) values (42, 'inserted')".NonQuery(1);
            Check(ReaderBatch(s0, s0, s41, s42));
        }
        ReaderBatch ReaderBatch(params Sql[] text) { return new ReaderBatch(_connection, text); }

        void Check(params Sql[] text) { Check(ReaderBatch(text)); }
        static void Check(ReaderBatch readerBatch)
        {
            Profiler.Measure(readerBatch.Reset);
            if(readerBatch.Length > 100)
            {
                var result = Profiler.Measure(() => readerBatch.Data.ToArray());
            }
            else
            {
                var result2 = Profiler.Measure(() => readerBatch.Data.ToArray());
            }
        }
        internal static void Run()
        {
            var sqlBatch = new SqlBatch("ANNE\\OJB_NET", "unittest");
            sqlBatch.FlatRun();
            sqlBatch.RunWithMeasure();
        }
    }
    static class Extension
    {
        public static IEnumerable<int> While(this int start, Func<int, bool> @while)
        {
            for (var result = start; @while(result); result++)
                yield return result;
        }
    }
}