using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using hw.Debug;
using hw.Forms;

namespace Taabus
{
    public sealed class Project : NamedObject, ITreeNodeSupport, ITreeNodeProbeSupport,IIconKeyProvider
    {
        internal string ProjectName;

        IEnumerable<TreeNode> ITreeNodeSupport.CreateNodes() { return Profiler.Measure(()=>Servers.CreateNodes()); }
        bool ITreeNodeProbeSupport.IsEmpty { get { return Profiler.Measure(()=>!Servers.Any()); } }
        string IIconKeyProvider.IconKey { get { return "Folder"; } }

        [DisableDump]
        public IEnumerable<Server> Servers = new Server[0];

        public override string Name { get { return ProjectName; } }
    }
}