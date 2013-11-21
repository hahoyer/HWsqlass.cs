using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using hw.Debug;

namespace Taabus
{
    sealed class TaabusController : DumpableObject
    {
        internal void Run()
        {
            Task.Factory.StartNew(() => ProjectExplorerView.Run(Closed));
            Task.Factory.StartNew(() => WorkspaceView.Run("main", Closed));
            Application.Run();
        }

        void Closed() { Application.Exit(); }
    }
}