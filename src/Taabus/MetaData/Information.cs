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
using System.Linq;
using hw.Debug;
using hw.Helper;

namespace Taabus.MetaData
{
    sealed class Information : DumpableObject
    {
        readonly SQLSysViews _sqlSysViews;
        readonly FunctionCache<string, CompountType> _compountTypeCache;

        public Information(IDataProvider provider)
        {
            _sqlSysViews = new SQLSysViews(provider);
            _compountTypeCache = new FunctionCache<string, CompountType>(GetCompountType);

            Tracer.Assert(IsSimpleScheme);
        }

        /// <summary>
        ///     Check if scheme is not required to identify an object, i. e. there are not objects with the same name under
        ///     different schemes
        /// </summary>
        bool IsSimpleScheme
        {
            get
            {
                if(_sqlSysViews.schemas.Count() == 1)
                    return true;
                var groupBy = CompountTypes.GroupBy(t => t.Name).FirstOrDefault(g => g.Count() > 1);
                if(groupBy == null)
                    return true;
                var d = groupBy.ToArray();
                return false;
            }
        }

        internal CompountType[] CompountTypes
        {
            get
            {
                return _sqlSysViews
                    .all_objects
                    .Where(o => o.Type.IsCompountType && o.Schema.name != "sys")
                    .Select(t => _compountTypeCache[t.name])
                    .ToArray();
            }
        }

        internal SQLSysViews.all_columnsClass[] SysColumns { get { return _sqlSysViews.all_columns; } }
        internal SQLSysViews.all_objectsClass[] SysObjects { get { return _sqlSysViews.all_objects; } }

        CompountType GetCompountType(string name)
        {
            var type = _sqlSysViews
                .all_objects
                .Single(t => t.name == name);
            return new CompountType
                (
                type.name,
                type.Schema.name,
                GetMembers(type)
                );
        }

        Member[] GetMembers(SQLSysViews.all_objectsClass table)
        {
            return _sqlSysViews
                .all_columns
                .Where(column => column.Object == table && column.Object.Schema == table.Schema)
                .Select(CreateMember)
                .ToArray();
        }

        static Member CreateMember(SQLSysViews.all_columnsClass column) { return new Member(column.name, BasicType.GetInstance(column)); }
    }

    sealed class ObjectType : EnumEx
    {
        public static readonly ObjectType Check = new ObjectType("C ");
        public static readonly ObjectType ForeignKey = new ObjectType("F ");
        public static readonly ObjectType PrimaryKey = new ObjectType("PK");
        public static readonly ObjectType UniqueIndex = new ObjectType("UQ");
        public static readonly ObjectType InternalTable = new ObjectType("IT",isCompountType:true);
        public static readonly ObjectType Table = new ObjectType("U ", isCompountType: true);
        public static readonly ObjectType View = new ObjectType("V ", isCompountType: true);
        public static readonly ObjectType SystemBaseTable = new ObjectType("S ", isCompountType: true);
        public static readonly ObjectType ServiceQueue = new ObjectType("SQ");
        public static readonly ObjectType Default = new ObjectType("D ");
        public static readonly ObjectType SQLStoredProcedure = new ObjectType("P ");
        public static readonly ObjectType ExtendedStoredProcedure = new ObjectType("X ");
        public static readonly ObjectType SQLScalarFunction = new ObjectType("FN");
        public static readonly ObjectType SQLInlineTableValuedFunction = new ObjectType("IF");
        public static readonly ObjectType SQLTableValuedFunction = new ObjectType("TF");
        public static readonly ObjectType AggregateFunction = new ObjectType("AF");
        public static readonly ObjectType CLRScalarFunction = new ObjectType("FS");
        public static readonly ObjectType CLRStoredProcedure = new ObjectType("PC");
        public static readonly ObjectType Trigger = new ObjectType("TR");
        public static readonly ObjectType TableType = new ObjectType("TT");
        public static readonly ObjectType Synonym = new ObjectType("SN");

        internal string Name;
        internal readonly bool IsCompountType;

        public ObjectType(string name, bool isCompountType = false)
        {
            Name = name;
            IsCompountType = isCompountType;
        }

        public static IEnumerable<ObjectType> All { get { return AllInstances<ObjectType>(); } }
    }
}