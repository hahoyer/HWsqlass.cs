using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Taabus.External
{
    [Serializer.Enable]
    sealed public class Item
    {
        public DataItem Data;
        public int X;
        public int Y;
        public int Width;
        public int Height;
        public string Type;
    }
}