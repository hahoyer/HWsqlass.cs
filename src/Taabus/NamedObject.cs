using System;
using System.Collections.Generic;
using System.Linq;
using hw.Debug;

namespace Taabus
{
    public abstract class NamedObject : DumpableObject
    {
        [DisableDump]
        internal abstract string Name { get; }

        protected override string GetNodeDump() { return Name; }
    }
}