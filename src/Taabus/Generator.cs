using System;
using System.Collections.Generic;
using System.Linq;

namespace Taabus
{
    partial class Generator
    {
        readonly Project _project;
        readonly ExpansionDescription[] _expansionDescriptions;
        readonly string[] _selection;

        internal Generator(Project project, ExpansionDescription[] expansionDescriptions, string[] selection)
        {
            _project = project;
            _expansionDescriptions = expansionDescriptions;
            _selection = selection;
        }
    }
}