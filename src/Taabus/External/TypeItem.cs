using System;
using System.Collections.Generic;
using System.Linq;
using Taabus.UserInterface;

namespace Taabus.External
{
    [Serializer.Enable]
    public sealed class TypeItem: Link
    {
        public string ServerId;
        public string DataBaseId;
        public string TypeId;

        internal IControlledItem ToControlledItem(ITaabusController parent)
        {
            return parent
                .Servers
                .Single(s => s.Name == ServerId)
                .DataBases
                .Single(d => d.Name == DataBaseId)
                .Types
                .Single(t => t.Name == TypeId);
        }
        internal override IControlledItem Internalize(Internalizer internalizer) { throw new NotImplementedException(); }
    }
}