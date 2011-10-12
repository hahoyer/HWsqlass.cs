using HWClassLibrary.Debug;
using System.Collections.Generic;
using System.Linq;
using System;

namespace sqlass
{
    sealed class Association<TDependant, TPrincipal, TType>
    {
        readonly TType _property;
        readonly TType _key;
        public Association(ref TType property, ref TType key)
        {
            _property = property;
            _key = key;
        }
    }
}