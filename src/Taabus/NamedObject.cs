﻿using System;
using System.Collections.Generic;
using System.Linq;
using hw.Debug;
using hw.Forms;

namespace Taabus
{
    public abstract class NamedObject : DumpableObject, INodeNameProvider, IAdditionalNodeInfoProvider
    {
        [DisableDump]
        public abstract string Name { get; }

        string INodeNameProvider.Value(string name) { return Name; }
        string IAdditionalNodeInfoProvider.AdditionalNodeInfo { get { return Name; } }
    }
}