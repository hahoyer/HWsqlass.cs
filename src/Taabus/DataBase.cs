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
    public sealed class DataBase : NamedObject
    {
        const string selectMetaDataStatement = "select * from [{0}].[INFORMATION_SCHEMA].[{1}]";

        const string SelectChildren = "select name = TABLE_NAME from [{0}].[INFORMATION_SCHEMA].[Tables] union select name= TABLE_NAME from [{0}].[INFORMATION_SCHEMA].[Views] ";
        internal static DataBase Create(DbDataRecord record, Server server) { return new DataBase(server, (string) record["name"]); }

        readonly ValueCache<Container[]> _containersCache;
        public readonly Server Server;

        DataBase(Server server, string name)
            : base(name)
        {
            Server = server;
            _containersCache = new ValueCache<Container[]>(GetContainers);
        }

        public Container[] Containers { get { return _containersCache.Value; } }

        Container[] GetContainers() { return Server.Select(SelectChildren.ReplaceArgs(Name), record => Container.Create(record, this)); }
        public T[] GetMetaData<T>(Func<DbDataRecord, T> func) { return Server.Select(SelectMetaDataStatement(typeof(T).Name), func); }
        public string SelectMetaDataStatement(string name) { return selectMetaDataStatement.ReplaceArgs(Name, name); }
    }
}