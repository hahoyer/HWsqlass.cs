using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using hw.Debug;
using hw.Forms;
using hw.Helper;
using JetBrains.Annotations;

namespace Taabus
{
    sealed class Project : NamedObject, ITreeNodeSupport
    {
        internal readonly string FileName;
        readonly ValueCache<Assembly> _assemblyCache;
        IEnumerable<Server> _servers;

        public Project([NotNull] string fileName)
        {
            FileName = fileName;
            EnsureFileExists();
            _assemblyCache = new ValueCache<Assembly>(() => FileName.CreateAssemblyFromFile());
        }
        void EnsureFileExists()
        {
            var file = FileName.FileHandle();
            if(file.Exists)
                return;
            file.String = @"
using System;
using System.Collections.Generic;
using System.Linq;
using Taabus;

public static class TaabusProject
{
    public static IEnumerable<Server> Servers() { yield break;}
}
";
        }

        static string ClassName { get { return "TaabusProject"; } }

        [DisableDump]
        internal IEnumerable<Server> Servers
        {
            get
            {
                return _servers
                    ?? (
                        _servers =
                            (IEnumerable<Server>) _assemblyCache
                                .Value
                                .GetType(ClassName)
                                .GetMethod("Servers")
                                .Invoke(null, new object[0])
                        );
            }
            set { _servers = value; }
        }

        IEnumerable<TreeNode> ITreeNodeSupport.CreateNodes() { return Servers.CreateNodes(); }
        internal override string Name { get { return FileName.FileHandle().Name; } }
    }
}