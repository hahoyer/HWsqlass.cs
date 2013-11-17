﻿using System;
using System.Collections.Generic;
using System.Linq;
using hw.Debug;
using hw.Helper;

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
        internal readonly SQLSysViews.all_objectsClass Object;
        readonly ValueCache<Member[]> _membersCache;

        public CompountType(SQLSysViews.all_objectsClass @object)
        {
            Object = @object;
            _membersCache = new ValueCache<Member[]>(()=>Object.Members);
        }
        protected override string GetName() { return Object.name; }
        internal override Member[] Members { get { return _membersCache.Value; } }
        internal string FullName { get { return Object.Schema.name + "." + Name; } }
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