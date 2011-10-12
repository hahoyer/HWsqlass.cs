using HWClassLibrary.Debug;
using System.Collections.Generic;
using System.Linq;
using System;

namespace sqlass
{
    class KeyToAttribute : Attribute
    {
        readonly Type _type;
        public KeyToAttribute(Type type) { _type = type; }
    }
}