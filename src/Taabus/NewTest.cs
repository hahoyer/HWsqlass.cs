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
using hw.UnitTest;

namespace Taabus
{
    [TestFixture]
    public sealed class NewTest
    {
        const string ColumnInfoStatement = @"
SELECT 
	Name = s.name + '.' + o.name + '.' + c.name, 
	SchemeName = s.name + '', 
	ObjectName = o.name + '', 
	ColumnName = c.name + '', 
	ObjectType = o.type , 
	ColumnType = t.name +
	case
	when t.name = 'decimal' then '(' + convert(varchar, c.precision) + ',' + convert(varchar, c.scale) + ')'
	when t.name = 'char' then '(' + convert(varchar, c.max_length) + ')'
	when t.name = 'nvarchar' and c.max_length = -1 then '(max)'
	when t.name = 'nvarchar' then '(' + convert(varchar, c.max_length/2) + ')'
	when t.name = 'nchar' then '(' + convert(varchar, c.max_length/2) + ')'
	else ''
	end, 
	IsNull = c.is_nullable
FROM sys.columns AS c  
JOIN sys.objects AS o ON o.object_id = c.object_id 
JOIN sys.schemas AS s ON o.schema_id = s.schema_id 
JOIN sys.types AS t ON c.user_type_id = t.user_type_id
";
        [Test]
        public void KeySearch()
        {
            try
            {
                var server = new Server("ANNE\\OJB_NET");
                var dataBase = server
                    .DataBases
                    .Single(db => db.Name == "cwg_adsalesng_devtest");

                var x = server.Select(ColumnInfoStatement, ColumnInfo.Create);

                var query = dataBase
                    .Types
                    .SelectMany(t => t.FindKeyCandiadtes().Select(r => new TableRecord {Name = t.Name, Record = r}));
                var result = query.ToArray();
                Tracer.FlaggedLine(result.Length + " records found.");
                var f = result.FirstOrDefault();
                Tracer.Assert(f != null);
                Tracer.Line(f.Dump());
                Tracer.AssertionFailed("");
            }
            catch(Exception exception)
            {
                Tracer.AssertionFailed("");
                throw;
            }
        }
    }

    sealed class ColumnInfo
    {
        public string Name;
        public string SchemeName;
        public string ObjectName;
        public string ColumnName;
        public string ObjectType;
        public string ColumnType;
        public bool IsNull;

        internal static ColumnInfo Create(DbDataRecord arg)
        {
            return new ColumnInfo()
            {
                Name = arg.GetString(0),
                SchemeName = arg.GetString(1),
                ObjectName = arg.GetString(2),
                ColumnName = arg.GetString(3),
                ObjectType = arg.GetString(4),
                ColumnType = arg.GetString(5),
                IsNull = arg.GetBoolean(6)
            };
        }
    }
}