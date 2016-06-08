using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using hw.DebugFormatter;
using hw.Forms;
using Taabus.Data;

namespace Taabus
{
    [Serializer.Enable]
    public sealed class Project : NamedObject, ITreeNodeSupport, ITreeNodeProbeSupport, IIconKeyProvider
    {
        internal string ProjectName;

        IEnumerable<TreeNode> ITreeNodeSupport.CreateNodes() { return Profiler.Measure(() => Servers.CreateNodes()); }
        bool ITreeNodeProbeSupport.IsEmpty { get { return Profiler.Measure(() => !Servers.Any()); } }
        string IIconKeyProvider.IconKey { get { return "Folder"; } }

        [DisableDump]
        public IEnumerable<Server> Servers = new Server[0];

        [Serializer.Disable]
        public override string Name { get { return ProjectName; } }

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