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
        readonly FunctionCache<string, Constraint> _constraintCache;
        readonly FunctionCache<string, CompountType> _compountTypeCache;

        public Information(IDataProvider provider)
        {
            _sqlSysViews = new SQLSysViews(provider);
            _constraintCache = new FunctionCache<string, Constraint>(GetConstraint);
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
                    .Where(o => o.type.In("IT", "S ", "U ", "V ") && o.Schema.name != "sys")
                    .Select(t => _compountTypeCache[t.name])
                    .ToArray();
            }
        }

        internal Constraint[] Constraints
        {
            get
            {
                return _sqlSysViews
                    .all_objects
                    .Where(o => o.type.In("PK", "UQ", "C ", "F "))
                    .Select(t => _constraintCache[t.name])
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

        Constraint GetConstraint(string name)
        {
            var constraint = _sqlSysViews
                .all_objects
                .SingleOrDefault(i => i.name == name);
            return constraint == null
                ? null
                : CreateConstraint(_compountTypeCache[constraint.Parent.name], constraint.name, constraint.Type);
        }

        Constraint CreateConstraint(CompountType compountType, string name, ConstraintType type)
        {

            if(type == ConstraintType.PrimaryKey || type == ConstraintType.UniqueIndex)
            {
                var ccu = _sqlSysViews
                    .key_constraints
                    .Single(i => i.name == name)
                    .Index
                    .Columns
                    .Select(c=>c.Column.name)
                    .ToArray();
         
                Tracer.Assert(ccu.Length > 0);
                return new KeyConstraint(compountType, name, type == ConstraintType.PrimaryKey, ccu);
            }
            
            if(type == ConstraintType.ForeignKey)
            {
                var fk = _sqlSysViews.foreign_keys.Single(o => o.name == name);
                var ccu = _sqlSysViews
                    .foreign_key_columns
                    .Where(o => o.Constraint.name == name)
                    .Select(c => c.ConstraintColumn.name)
                    .ToArray();
                Tracer.Assert(ccu.Length > 0);
                var constraint = _constraintCache[fk.name];
                if(constraint == null)
                    return null;
                return new ForeignKeyConstraint
                    (
                    compountType,
                    name,
                    constraint,
                    fk.delete_referential_action_desc,
                    fk.update_referential_action_desc,
                    ccu
                    );
            }
            if(type == ConstraintType.Check)
            {
                return null;
            }
            NotImplementedMethod(name, type);
            return null;
        }
    }

    sealed class ForeignKeyConstraint : Constraint
    {
        public readonly string DeleteRule;
        public readonly string UpdateRule;
        public readonly string[] ColumnNames;
        public readonly Constraint Target;

        public ForeignKeyConstraint(CompountType type, string name, Constraint target, string deleteRule, string updateRule, string[] columnNames)
            : base(name, type)
        {
            DeleteRule = deleteRule;
            UpdateRule = updateRule;
            ColumnNames = columnNames;
            Target = target;
        }
    }

    sealed class KeyConstraint : Constraint
    {
        public readonly bool IsPrimaryKey;
        public readonly string[] ColumnNames;

        public KeyConstraint(CompountType type, string name, bool isPrimaryKey, string[] columnNames)
            : base(name, type)
        {
            IsPrimaryKey = isPrimaryKey;
            ColumnNames = columnNames;
        }
    }

    sealed class CheckConstraint : Constraint
    {
        public readonly string[] ColumnNames;
        public readonly string Clause;

        public CheckConstraint(CompountType type, string name, string[] columnNames, string clause)
            : base(name, type)
        {
            ColumnNames = columnNames;
            Clause = clause;
        }
    }

    sealed class ConstraintType : EnumEx
    {
        public static readonly ConstraintType Check = new ConstraintType("C ");
        public static readonly ConstraintType ForeignKey = new ConstraintType("F ");
        public static readonly ConstraintType PrimaryKey = new ConstraintType("PK");
        public static readonly ConstraintType UniqueIndex = new ConstraintType("UQ");

        internal string Name;
        public ConstraintType(string name) { Name = name; }

        public static IEnumerable<ConstraintType> All { get { return AllInstances<ConstraintType>(); } }
        
    }
}