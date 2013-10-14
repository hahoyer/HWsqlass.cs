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
    sealed class Member
    {
        public readonly string Name;
        public readonly Type Type;
        public Member(string name, Type type)
        {
            Name = name;
            Type = type;
        }
    }

    sealed class CompountType : Type
    {
        readonly string _name;
        public readonly string Schema;
        readonly Member[] _members;

        public CompountType(string name, string schema, Member[] members)
        {
            _name = name;
            Schema = schema;
            _members = members;
        }
        protected override string GetName() { return _name; }
        internal override Member[] Members { get { return _members; } }
    }

    abstract class Constraint : DumpableObject
    {
        public readonly string Name;
        public readonly CompountType Type;
        protected Constraint(string name, CompountType type)
        {
            Name = name;
            Type = type;
        }
    }

    public abstract class Type : DumpableObject
    {
        protected abstract string GetName();
        public string Name { get { return GetName(); } }
        internal virtual Member[] Members { get { return new Member[0]; } }
        protected override string GetNodeDump() { return Name; }
    }
}