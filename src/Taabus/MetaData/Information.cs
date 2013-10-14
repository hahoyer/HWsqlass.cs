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
        readonly SQLInformation _sqlInformation;
        readonly FunctionCache<string, Constraint> _constraintCache;
        readonly FunctionCache<string, CompountType> _compountTypeCache;

        public Information(SQLInformation.IDataProvider provider)
        {
            _sqlInformation = new SQLInformation(provider);
            _constraintCache = new FunctionCache<string, Constraint>(GetConstraint);
            _compountTypeCache = new FunctionCache<string, CompountType>(GetCompountType);

            Tracer.Assert(IsSimpleScheme);
            Tracer.Assert(IsSimpleConstraintUsage);
        }

        /// <summary>
        ///     Check if scheme is not required to identify an object, i. e. there are not objects with the same name under
        ///     different schemes
        /// </summary>
        bool IsSimpleScheme
        {
            get
            {
                if(_sqlInformation.SCHEMATA.Count() == 1)
                    return true;
                var groupBy = CompountTypes.GroupBy(t => t.Name).FirstOrDefault(g => g.Count() > 1);
                if(groupBy == null)
                    return true;
                var d = groupBy.ToArray();
                return false;
            }
        }

        /// <summary>
        ///     Check if scheme is not required to identify an object, i. e. there are not objects with the same name under
        ///     different schemes
        /// </summary>
        bool IsSimpleConstraintUsage
        {
            get
            {
                var cc = _sqlInformation.TABLE_CONSTRAINTS.GroupBy(c => c.CONSTRAINT_CATALOG).ToArray();
                switch(cc.Length)
                {
                    case 0:
                        return true;
                    case 1:
                        break;
                    default:
                        return false;
                }

                var cs = cc[0].GroupBy(c => c.CONSTRAINT_SCHEMA).ToArray();
                if(cs.Length > 1)
                    return false;
                var tc = cs[0].GroupBy(c => c.TABLE_CATALOG).ToArray();
                if(tc.Length > 1)
                    return false;
                var ts = tc[0].GroupBy(c => c.TABLE_SCHEMA).ToArray();
                if(ts.Length > 1)
                    return false;
                var id = ts[0].GroupBy(c => c.INITIALLY_DEFERRED).ToArray();
                if(id.Length > 1)
                    return false;
                var d = id[0].GroupBy(c => c.IS_DEFERRABLE).ToArray();
                if(d.Length > 1)
                    return false;
                return true;
            }
        }

        internal CompountType[] CompountTypes
        {
            get
            {
                return _sqlInformation
                    .TABLES
                    .Select(t => _compountTypeCache[t.TABLE_NAME])
                    .ToArray();
            }
        }

        internal Constraint[] Constraints
        {
            get
            {
                return _sqlInformation
                    .TABLE_CONSTRAINTS
                    .Select(t => _constraintCache[t.CONSTRAINT_NAME])
                    .ToArray();
            }
        }

        CompountType GetCompountType(string name)
        {
            var type = _sqlInformation
                .TABLES
                .Single(t => t.TABLE_NAME == name);
            return new CompountType(type.TABLE_NAME, type.TABLE_SCHEMA, GetMembers(type));
        }

        Member[] GetMembers(SQLInformation.TABLESClass table)
        {
            return _sqlInformation
                .COLUMNS
                .Where(column => column.TABLE_NAME == table.TABLE_NAME && column.TABLE_SCHEMA == table.TABLE_SCHEMA)
                .Select(CreateMember)
                .ToArray();
        }

        static Member CreateMember(SQLInformation.COLUMNSClass column) { return new Member(column.COLUMN_NAME, BasicType.GetInstance(column)); }

        Constraint GetConstraint(string name)
        {
            var constraint = _sqlInformation
                .TABLE_CONSTRAINTS
                .SingleOrDefault(i => i.CONSTRAINT_NAME == name);
            return constraint == null 
                ? null 
                : CreateConstraint(_compountTypeCache[constraint.TABLE_NAME], constraint.CONSTRAINT_NAME, constraint.CONSTRAINT_TYPE);
        }

        Constraint CreateConstraint(CompountType compountType, string name, string type)
        {
            var cc = CheckConstraints(name);
            var rc = ReferentialConstraints(name);
            var ccu = ConstraintColumns(name, type == "CHECK");

            CheckConstraintTableUsage(name, compountType.Name);
            CheckDomainConstraints(name);

            switch(type)
            {
                case "CHECK":
                    Tracer.Assert(cc != null);
                    return new CheckConstraint(compountType, name, ccu, cc.CHECK_CLAUSE);
            }

            Tracer.Assert(cc == null);
            Tracer.Assert(ccu.Length > 0);

            switch(type)
            {
                case "PRIMARY KEY":
                case "UNIQUE":
                    Tracer.Assert(rc == null);
                    return new KeyConstraint(compountType, name, type == "PRIMARY KEY", ccu);
                case "FOREIGN KEY":
                    Tracer.Assert(rc != null);
                    var constraint = _constraintCache[rc.UNIQUE_CONSTRAINT_NAME];
                    if(constraint == null)
                        return null;
                    return new ForeignKeyConstraint
                        (
                        compountType,
                        name,
                        constraint,
                        rc.DELETE_RULE,
                        rc.MATCH_OPTION,
                        rc.UPDATE_RULE,
                        ccu
                        );
                default:
                    NotImplementedMethod(name, type);
                    return null;
            }
        }

        void CheckDomainConstraints(string name)
        {
            var any = _sqlInformation.DOMAIN_CONSTRAINTS.Any(i => i.CONSTRAINT_NAME == name);
            Tracer.Assert(!any);
        }

        void CheckConstraintTableUsage(string name, string tableName)
        {
            Tracer.Assert(_sqlInformation
                .CONSTRAINT_TABLE_USAGE
                .Count(i => i.CONSTRAINT_NAME == name && i.TABLE_NAME == tableName)
                == 1);
        }

        string[] ConstraintColumns(string name, bool isCheck)
        {
            var result = _sqlInformation
                .CONSTRAINT_COLUMN_USAGE
                .Where(i => i.CONSTRAINT_NAME == name)
                .Select(i => i.COLUMN_NAME)
                .ToArray();

            var keyColumns = _sqlInformation
                .KEY_COLUMN_USAGE
                .Where(i => i.CONSTRAINT_NAME == name)
                .Select(i => new {i.COLUMN_NAME, i.ORDINAL_POSITION})
                .OrderBy(i => i.ORDINAL_POSITION)
                .ToArray();

            if(isCheck)
                Tracer.Assert(keyColumns.Length == 0);
            else
                Tracer.Assert(keyColumns.Length == result.Length);
            Tracer.Assert(!keyColumns.Where((kcui, i) => kcui.ORDINAL_POSITION != i + 1).Any());
            Tracer.Assert(keyColumns.All(kcui => result.Any(ccui => ccui == kcui.COLUMN_NAME)));

            return result;
        }

        SQLInformation.REFERENTIAL_CONSTRAINTSClass ReferentialConstraints(string name)
        {
            return _sqlInformation
                .REFERENTIAL_CONSTRAINTS
                .SingleOrDefault(i => i.CONSTRAINT_NAME == name);
        }

        SQLInformation.CHECK_CONSTRAINTSClass CheckConstraints(string name)
        {
            return _sqlInformation
                .CHECK_CONSTRAINTS
                .SingleOrDefault(i => i.CONSTRAINT_NAME == name);
        }
    }

    sealed class ForeignKeyConstraint : Constraint
    {
        public readonly string DeleteRule;
        public readonly string MatchOption;
        public readonly string UpdateRule;
        public readonly string[] ColumnNames;
        public readonly Constraint Target;

        public ForeignKeyConstraint(CompountType type, string name, Constraint target, string deleteRule, string matchOption, string updateRule, string[] columnNames)
            : base(name, type)
        {
            DeleteRule = deleteRule;
            MatchOption = matchOption;
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
}