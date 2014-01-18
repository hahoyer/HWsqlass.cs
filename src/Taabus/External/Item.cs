using System;
using System.Collections.Generic;
using System.Linq;

namespace Taabus.External
{
    [Serializer.Enable]
    public sealed class Item
    {
        [Serializer.Except(null)]
        public int? Id;
        public ItemData Data;
        public int X;
        public int Y;
        public int Width;
        public int Height;
    }
}