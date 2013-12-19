using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using hw.Debug;
using hw.Forms;
using hw.Helper;
using Taabus.Data;
using Taabus.UserInterface;

namespace Taabus
{
    [Serializer.Class]
    public sealed class Project : NamedObject, ITreeNodeSupport, ITreeNodeProbeSupport, IIconKeyProvider
    {
        internal string FileName;

        IEnumerable<TreeNode> ITreeNodeSupport.CreateNodes() { return Profiler.Measure(() => Servers.CreateNodes()); }
        bool ITreeNodeProbeSupport.IsEmpty { get { return Profiler.Measure(() => !Servers.Any()); } }
        string IIconKeyProvider.IconKey { get { return "Folder"; } }

        [DisableDump]
        [Serializer.Member]
        public IEnumerable<Server> Servers = new Server[0];

        public override string Name { get { return ProjectName; } }
        internal string ProjectName { get { return FileName.FileHandle().Name; } }

        internal void Save(ExpansionDescription[] expansionDescriptions, string[] selectedPath)
        {
            var generator = new Generator(new TaabusProject
            {
                Project = this, 
                ExpansionDescriptions = expansionDescriptions, 
                Selection = selectedPath
            });
            var transformText = generator.TransformText();
            FileName.FileHandle().String = transformText;
        }

        internal void InsertServer(Server server, int position)
        {
            Servers = Servers
                .Take(position)
                .Concat(new[] {server})
                .Concat(Servers.Skip(position))
                .ToArray();
        }
    }
}