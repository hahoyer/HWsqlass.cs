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
    public sealed class Server : NamedObject
    {
        const string SelectDatabases = "select name from master.sys.databases";

        readonly ValueCache<DataBase[]> _dataBasesCache;

        public Server(string name)
            : base(name) { _dataBasesCache = new ValueCache<DataBase[]>(GetDataBases); }

        public DataBase[] DataBases { get { return _dataBasesCache.Value; } }

        DataBase[] GetDataBases() { return Select(SelectDatabases, record => DataBase.Create(record, this)); }

        public T[] Select<T>(string statement, Func<DbDataRecord, T> func) { return ToDataReader(statement).SelectFromReader(func); }
        public DbDataReader ToDataReader(string statement) { return Name.ToConnection().ToDataReader(statement); }
    }
}