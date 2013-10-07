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

namespace Taabus.MetaData
{
    sealed class Information : DumpableObject
    {
        readonly SQLInformation _sqlInformation;

        public Information(SQLInformation.IDataProvider provider)
        {
            _sqlInformation = new SQLInformation(provider);
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
                var groupBy = Types.GroupBy(t => t.Name).FirstOrDefault(g => g.Count() > 1);
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

        internal CompountType[] Types
        {
            get
            {
                return _sqlInformation
                    .TABLES
                    .Select(t => new CompountType(t.TABLE_NAME, t.TABLE_SCHEMA, GetMembers(t), GetConstraints(t)))
                    .ToArray();
            }
        }

        Constraint[] GetConstraints(SQLInformation.TABLESClass table)
        {
            return _sqlInformation
                .TABLE_CONSTRAINTS
                .Where(constraint => constraint.TABLE_NAME == table.TABLE_NAME && constraint.TABLE_SCHEMA == table.TABLE_SCHEMA)
                .Select(CreateConstraint)
                .ToArray();
        }

        Member[] GetMembers(SQLInformation.TABLESClass table)
        {
            return _sqlInformation
                .COLUMNS
                .Where(column => column.TABLE_NAME == table.TABLE_NAME && column.TABLE_SCHEMA == table.TABLE_SCHEMA)
                .Select(CreateMember)
                .ToArray();
        }

        static Member CreateMember(SQLInformation.COLUMNSClass column)
        {
            return
                new Member(column.COLUMN_NAME, BasicType.GetInstance(column));
        }

        Constraint CreateConstraint(SQLInformation.TABLE_CONSTRAINTSClass constraint)
        {
            var cc = _sqlInformation
                .CHECK_CONSTRAINTS
                .SingleOrDefault(i => i.CONSTRAINT_NAME == constraint.CONSTRAINT_NAME);

            var rc = _sqlInformation
                .REFERENTIAL_CONSTRAINTS
                .SingleOrDefault(i => i.CONSTRAINT_NAME == constraint.CONSTRAINT_NAME);

            var ccu = _sqlInformation
                .CONSTRAINT_COLUMN_USAGE
                .Where(i => i.CONSTRAINT_NAME == constraint.CONSTRAINT_NAME)
                .Select(i => i.COLUMN_NAME)
                .ToArray();

            var ctu = _sqlInformation
                .CONSTRAINT_TABLE_USAGE
                .SingleOrDefault(i => i.CONSTRAINT_NAME == constraint.CONSTRAINT_NAME);

            Tracer.Assert(_sqlInformation.DOMAIN_CONSTRAINTS.All(i => i.CONSTRAINT_NAME != constraint.CONSTRAINT_NAME));

            var kcu = _sqlInformation
                .KEY_COLUMN_USAGE
                .Where(i => i.CONSTRAINT_NAME == constraint.CONSTRAINT_NAME)
                .Select(i => new {i.COLUMN_NAME, i.ORDINAL_POSITION})
                .OrderBy(i => i.ORDINAL_POSITION)
                .ToArray();

            switch(constraint.CONSTRAINT_TYPE)
            {
                case "PRIMARY KEY":
                case "UNIQUE":
                case "FOREIGN KEY":
                    Tracer.Assert(cc == null);
                    Tracer.Assert(ccu.Length > 0);
                    Tracer.Assert(ctu != null);
                    Tracer.Assert(ctu.TABLE_NAME == constraint.TABLE_NAME);
                    Tracer.Assert(kcu.Length == ccu.Length);
                    Tracer.Assert(kcu.All(kcui => ccu.Any(ccui => ccui == kcui.COLUMN_NAME)));
                    Tracer.Assert(!kcu.Where((kcui, i) => kcui.ORDINAL_POSITION != i + 1).Any());
                    break;
            }

            switch(constraint.CONSTRAINT_TYPE)
            {
                case "PRIMARY KEY":
                    Tracer.Assert(rc == null);
                    return new PrimaryKeyConstraint(constraint.CONSTRAINT_NAME, ccu);
                case "UNIQUE":
                    Tracer.Assert(rc == null);
                    return new UniqueConstraint(constraint.CONSTRAINT_NAME, ccu);
                case "FOREIGN KEY":
                    Tracer.Assert(rc != null);
                    return new ForeignKeyConstraint(constraint.CONSTRAINT_NAME, rc.DELETE_RULE, rc.MATCH_OPTION, rc.UPDATE_RULE, rc.UNIQUE_CONSTRAINT_NAME);
                case "CHECK":
                    Tracer.Assert(cc != null);
                    Tracer.Assert(rc == null);
                    Tracer.Assert(ccu.Length > 0);
                    Tracer.Assert(ctu != null);
                    Tracer.Assert(ctu.TABLE_NAME == constraint.TABLE_NAME);
                    Tracer.Assert(kcu.Length == 0);
                    return new CheckConstraint(constraint.CONSTRAINT_NAME,ccu, cc.CHECK_CLAUSE);
            }

            Tracer.Assert(rc == null);
            Tracer.Assert(ccu.Length == 0);
            Tracer.Assert(ctu == null);
            Tracer.Assert(kcu.Length == 0);
            NotImplementedMethod(constraint);
            return null;
        }
    }

    sealed class ForeignKeyConstraint : Constraint
    {
        readonly string _deleteRule;
        readonly string _matchOption;
        readonly string _updateRule;
        readonly string _uniqueConstraintName;
        public ForeignKeyConstraint(string name, string deleteRule, string matchOption, string updateRule, string uniqueConstraintName)
            : base(name)
        {
            _deleteRule = deleteRule;
            _matchOption = matchOption;
            _updateRule = updateRule;
            _uniqueConstraintName = uniqueConstraintName;
        }
    }

    sealed class UniqueConstraint : Constraint
    {
        public readonly string[] ColumnNames;
        public UniqueConstraint(string name, string[] columnNames)
            : base(name) { ColumnNames = columnNames; }
    }

    sealed class PrimaryKeyConstraint : Constraint
    {
        public readonly string[] ColumnNames;
        public PrimaryKeyConstraint(string name, string[] columnNames)
            : base(name) { ColumnNames = columnNames; }
    }

    sealed class CheckConstraint : Constraint
    {
        public readonly string[] ColumnNames;
        public readonly string Clause;
        public CheckConstraint(string name, string[] columnNames, string clause)
            : base(name)
        {
            ColumnNames = columnNames;
            Clause = clause;
        }
    }
}