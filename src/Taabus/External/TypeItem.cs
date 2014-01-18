using System;
using System.Collections.Generic;
using System.Linq;
using Taabus.UserInterface;

namespace Taabus.External
{
    [Serializer.Enable]
    public sealed class TypeItem
    {
        public string ServerId;
        public string DataBaseId;
        public string TypeId;

        internal Data.TypeItem Internalize(Internalizer internalizer)
        {
            return internalizer
                .WorkSpaceView
                .Controller
                .Servers
                .Single(s => s.Name == ServerId)
                .DataBases
                .Single(d => d.Name == DataBaseId)
                .Types
                .Single(t => t.Name == TypeId);
        }
    }
}