using System;
using System.Collections.Generic;
using System.Linq;
using Taabus.UserInterface;

namespace Taabus
{
    partial class Generator
    {
        readonly Project _project;
        readonly ExpansionDescription[] _expansionDescriptions;
        readonly string[] _selection;

        internal Generator(Project project = null, ExpansionDescription[] expansionDescriptions = null, string[] selection = null)
        {
            _project = project ?? new Project();
            _expansionDescriptions = expansionDescriptions ?? new ExpansionDescription[0];
            _selection = selection ?? new string[0];
        }
    }
}