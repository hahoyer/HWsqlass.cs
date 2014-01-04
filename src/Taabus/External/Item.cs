using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Taabus.External
{
    [Serializer.Enable]
    sealed public class Item
    {
        public Type Type;
        public Rectangle Rectangle;
        public DataItem Data;
    }
}