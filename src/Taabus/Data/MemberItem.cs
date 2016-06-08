using System;
using System.Collections.Generic;
using System.Linq;
using hw.DebugFormatter;
using hw.Forms;
using Taabus.MetaData;

namespace Taabus.Data
{
    sealed class MemberItem : Item, IIconKeyProvider, IDataColumn
    {
        [Node]
        public readonly BasicType Type;
        public MemberItem(TypeItem parent, Member metaData)
            : base(parent.Parent, metaData.Name) { Type = metaData.Type; }

        [DisableDump]
        public Field Field { get { return new Field {Name = Name, Type = Type}; } }

        protected override Item[] GetItems() => null;
        string IIconKeyProvider.IconKey => Type.GetIconKey();
        string IDataColumn.Name => Name;
    }
}