using System;
using System.Collections.Generic;
using System.Linq;
using hw.DebugFormatter;
using Taabus.MetaData;

namespace Taabus.Data
{
    sealed class Field : DumpableObject
    {
        [DisableDump]
        public DataBase DataBase;
        public string Name;
        public BasicType Type;
        [DisableDump]
        public CompountType Container;
    }
}