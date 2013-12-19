using System;
using System.Collections.Generic;
using System.Linq;
using Taabus.UserInterface;

namespace Taabus
{
    partial class Generator
    {
        readonly TaabusProject _project;
        internal Generator(TaabusProject taabusProject) { _project = taabusProject; }

        string Serialize() { return new Serializer().Serialize(_project); }

        internal static string CreateEmptyProject() { return new Generator(TaabusProject.Create()).TransformText(); }
    }

    [Serializer.Class]
    public sealed class TaabusProject
    {
        internal static TaabusProject Create()
        {
            return new TaabusProject
            {
                Project = new Project(),
                ExpansionDescriptions = new ExpansionDescription[0],
                Selection = new string[0]
            };
        }

        [Serializer.Member]
        public Project Project;
        [Serializer.Member]
        public ExpansionDescription[] ExpansionDescriptions;
        [Serializer.Member]
        public string[] Selection;
    }
}