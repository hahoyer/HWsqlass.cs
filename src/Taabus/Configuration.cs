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
        public ExpansionDescription[] ExpansionDescriptions = new ExpansionDescription[0];
        public string[] Selection= new string[0];
        public Server[] Servers=new Server[0];
        public External.Item[] Items=new External.Item[0];
    }
}