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
using hw.Helper;
using Taabus.MetaData;

namespace Taabus
{
    sealed class DataBase : NamedObject, SQLInformation.IDataProvider
    {
        const string MetaDataStatement = "select * from [{0}].[INFORMATION_SCHEMA].[{1}]";

        internal static DataBase Create(DbDataRecord record, Server server) { return new DataBase(server, (string) record["name"]); }

        readonly Information _information;
        readonly ValueCache<TypeItem[]> _typeItemsCache;
        [DisableDump]
        internal readonly Server Parent;

        internal TypeItem[] Types { get { return _typeItemsCache.Value; } }

        DataBase(Server parent, string name)
            : base(name)
        {
            Parent = parent;
            _information = new Information(this);
            _typeItemsCache = new ValueCache<TypeItem[]>(GetTypes);
        }

        TypeItem[] GetTypes()
        {
            return _information
                .CompountTypes
                .Select(type => Item.CreateType(this, type, References(type)))
                .ToArray();
        }

        ReferenceItem[] References(CompountType type)
        {
            return _information
                .Constraints
                .Where(c => c.Type == type)
                .OfType<ForeignKeyConstraint>()
                .Select(c => Item.CreateReference(this, c, FindType))
                .ToArray();
        }

        TypeItem FindType(CompountType compountType) { return GetTypes().Single(typeItem => typeItem.Type == compountType); }
        T[] SQLInformation.IDataProvider.Select<T>(string name, Func<DbDataRecord, T> func) { return Parent.Select(SelectMetaDataStatement(name), func); }
        internal string SelectMetaDataStatement(string name) { return MetaDataStatement.ReplaceArgs(Name, name); }
    }
}