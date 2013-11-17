using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using hw.Debug;
using hw.Forms;

namespace Taabus
{
    public sealed class Project : NamedObject, ITreeNodeSupport, ITreeNodeProbeSupport
    {
        internal string ProjectName;

        IEnumerable<TreeNode> ITreeNodeSupport.CreateNodes() { return Servers.CreateNodes(); }
        bool ITreeNodeProbeSupport.IsEmpty { get { return !Servers.Any(); } }

        [DisableDump]
        public IEnumerable<Server> Servers { get; set; }

        public override string Name { get { return ProjectName; } }
    }
}