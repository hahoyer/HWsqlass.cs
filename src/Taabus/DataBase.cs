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
using System.Windows.Forms;
using hw.Debug;
using hw.Forms;
using hw.Helper;
using Taabus.MetaData;

namespace Taabus
{
    public sealed class DataBase : NamedObject, IDataProvider, ITreeNodeSupport
    {
        const string MetaDataStatement = "select * from [{0}].[{1}].[{2}]";

        internal static DataBase Create(DbDataRecord record, Server server) { return new DataBase(server, (string) record["name"]); }

        readonly Information _information;
        readonly ValueCache<TypeItem[]> _typeItemsCache;
        [DisableDump]
        internal readonly Server Parent;

        DataBase(Server parent, string name)
            : base(name)
        {
            Parent = parent;
            _information = new Information(this);
            _typeItemsCache = new ValueCache<TypeItem[]>(GetTypes);
        }

        internal TypeItem[] Types
        {
            get
            {
                if(_typeItemsCache.IsBusy)
                    return null;
                return _typeItemsCache.Value;
            }
        }

        internal IEnumerable<Field> Fields
        {
            get
            {
                if(_typeItemsCache.IsBusy)
                    return null;

                return Types.SelectMany(t => t.Fields);
            }
        }

        internal string SelectMetaDataStatement(string schema, string name)
        {
            return MetaDataStatement
                .ReplaceArgs(Name, schema, name);
        }

        TypeItem[] GetTypes()
        {
            return _information
                .CompountTypes
                .Select
                (
                    type
                        =>
                        Item
                            .CreateType
                            (
                                this,
                                type,
                                GetReferences(type),
                                GetPrimaryKeyIndex(type),
                                GetUniques(type)
                            )
                )
                .OrderBy(type => type.Name)
                .ToArray();
        }

        int[][] GetUniques(CompountType type)
        {
            if(IsInDump)
                return null;
            var keyConstraint = _information
                .Constraints
                .Where(c => c != null && c.Type == type)
                .OfType<KeyConstraint>()
                .Select
                (
                    kc
                        =>
                        kc
                            .ColumnNames
                            .Select(kccn => type.Members.IndexOf(m => m.Name == kccn).AssertValue())
                            .ToArray()
                )
                .ToArray();
            return keyConstraint;
        }

        int? GetPrimaryKeyIndex(CompountType type)
        {
            if(IsInDump)
                return null;
            var constraints = _information
                .Constraints
                .Where(c => c != null && c.Type == type);
            var keyConstraint = constraints
                .OfType<KeyConstraint>()
                .Single(kc => kc.IsPrimaryKey);

            if(keyConstraint.ColumnNames.Length != 1)
                return null;

            var result = type
                .Members
                .IndexOf(m => m.Name == keyConstraint.ColumnNames[0])
                .AssertValue();

            return result;
        }

        ReferenceItem[] GetReferences(CompountType type)
        {
            return _information
                .Constraints
                .Where(c => c != null && c.Type == type)
                .OfType<ForeignKeyConstraint>()
                .Select(c => Item.CreateReference(this, c, FindType))
                .ToArray();
        }

        TypeItem FindType(CompountType compountType)
        {
            return GetTypes()
                .Single(typeItem => typeItem.Type == compountType);
        }

        T[] IDataProvider.Select<T>(string schema, string name, Func<DbDataRecord, T> func) { return Parent.Select(SelectMetaDataStatement(schema, name), func); }
        IEnumerable<TreeNode> ITreeNodeSupport.CreateNodes() { return Types.CreateNodes(); }
    }

    partial class MetaDataGenerator
    {
        readonly string _className;
        readonly string _schema;
        readonly string[] _objectNames;
        readonly DataBase _dataBase;
        readonly Dictionary<string, Relation[]> _relations;
        internal MetaDataGenerator(string schema, DataBase dataBase, string[] objectNames, string className, Dictionary<string, Relation[]> relations = null)
        {
            _dataBase = dataBase;
            _objectNames = objectNames;
            _className = className;
            if(relations == null)
                relations = new Dictionary<string, Relation[]>();
            _relations =
                _objectNames
                    .Select(on => new {key = on, value = relations.ContainsKey(on) ? relations[on] : new Relation[0]})
                    .ToDictionary(o => o.key, o => o.value);
            _schema = schema;
        }
    }
}