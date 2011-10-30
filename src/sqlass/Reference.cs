using HWClassLibrary.Debug;
using System.Collections.Generic;
using System.Linq;
using System;

namespace sqlass
{
    public sealed class Reference<T>
    {
        readonly T _target;
        public Reference(T target) { _target = target; }
    }
}