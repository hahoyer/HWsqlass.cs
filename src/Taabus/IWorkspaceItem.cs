using System;
using System.Collections.Generic;
using System.Linq;

namespace Taabus
{
    interface IWorkspaceItem
    {
        IControlledItem Item { get; }
    }
}