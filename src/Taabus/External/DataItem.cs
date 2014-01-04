using System;
using System.Collections.Generic;
using System.Linq;
using hw.Debug;
using Taabus.Data;

namespace Taabus.External
{
    [Serializer.Enable]
    public sealed class DataItem : DumpableObject
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
    }
}