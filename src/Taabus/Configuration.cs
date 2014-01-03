using System;
using System.Collections.Generic;
using System.Linq;
using Taabus.Data;
using Taabus.External;
using Taabus.UserInterface;

namespace Taabus
{
    [Serializer.Enable]
    public sealed class Configuration
    {
        public ExpansionDescription[] ExpansionDescriptions;
        public string[] Selection;
        public Server[] Servers;
        public External.Item[] Items;
    }
}