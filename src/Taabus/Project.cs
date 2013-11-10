using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using hw.Debug;
using hw.Forms;
using hw.Helper;

namespace Taabus
{
    public sealed class Project : NamedObject, ITreeNodeSupport
    {
        internal string ProjectName;

        [DisableDump]
        public IEnumerable<Server> Servers { get; set; }

        IEnumerable<TreeNode> ITreeNodeSupport.CreateNodes() { return Servers.CreateNodes(); }

        internal override string Name { get { return ProjectName; } }

        internal static Project CreateFromCSharpFile(string fileName)
        {
            var result = (Project) fileName
                .CreateAssemblyFromFile()
                .GetType(typeof(TaabusProject).Name)
                .InvokeMember("Data", BindingFlags.InvokeMethod, null, null, new object[0]);
            result.ProjectName = fileName.FileHandle().Name;
            return result;
        }

        internal void SaveAsCSharpFile(string fileName)
        {
            var g = new Generator(this);
            fileName.FileHandle().String = g.TransformText();
        }
    }
}