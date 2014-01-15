using System;
using System.Collections.Generic;
using System.Linq;
using hw.Debug;

namespace Taabus.External
{
    [Serializer.Enable]
    public sealed class DataItem : DumpableObject
    {
        public TableItem TableItem;
        public int? Id;
    }
}