using System;
using System.Collections.Generic;
using System.Linq;
using hw.Debug;

namespace Taabus.MetaData
{
    sealed class Member : DumpableObject
    {
        [DisableDump]
        public readonly string Name;
        public readonly BasicType Type;

        public Member(string name, BasicType type)
        {
            Name = name;
            Type = type;
        }
        protected override string GetNodeDump() { return Name; }
    }

    sealed class CompountType : Type
    {
        readonly string _name;
        [DisableDump]
        internal readonly string Schema;
        readonly Member[] _members;

        public CompountType(string name, string schema, Member[] members)
        {
            _name = name;
            Schema = schema;
            _members = members;
        }
        protected override string GetName() { return _name; }
        internal override Member[] Members { get { return _members; } }
        internal string FullName { get { return Schema + "." + Name; } }
        internal TypeItem CreateType(DataBase dataBase) { return new TypeItem(dataBase, this); }
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

        [DisableDump]
        public string Name { get { return GetName(); } }

        [DisableDump]
        internal virtual Member[] Members { get { return new Member[0]; } }

        protected override string GetNodeDump() { return Name; }
    }
}