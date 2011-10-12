using HWClassLibrary.Debug;
using System.Collections.Generic;
using System.Linq;
using System;

namespace sqlass
{
    partial class Modell
    {
        static partial class Objects
        {
            class OrganisationType
            {
                public int Id;
                public string Name;
            }
        }
    }

    partial class Modell
    {
        static partial class Objects
        {
            class Organisation
            {
                public int Id;
                public int TypeId;
                public string Name;
            }
        }

        static partial class Relations
        {
        }
    }
}