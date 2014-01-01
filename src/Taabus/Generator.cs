using System;
using System.Collections.Generic;
using System.Linq;

namespace Taabus
{
    partial class Generator
    {
        readonly Configuration _configuration;
        internal Generator(Configuration configuration) { _configuration = configuration; }
        string Serialize() { return Serializer.Serialize(_configuration); }
    }
}