using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using hw.Debug;

namespace Taabus
{
    sealed class TaabusController : DumpableObject, ITaabusController
    {
        ProjectExplorerView _explorer;
        WorkspaceView _workspace;

        void ITaabusController.OnClosed() { Application.ExitThread(); }
        void ITaabusController.OnActivate(IControlledItem item) { _workspace.Add(item); }

        internal void Run()
        {
            Task.Factory.StartNew(() =>
            {
                _explorer = new ProjectExplorerView(this);
                _explorer.Run();
            });
            Task.Factory.StartNew(() =>
            {
                _workspace = new WorkspaceView("Main", this);
                _workspace.Run();
            });
            Application.Run();
        }

        internal static Type GetTypeFromFile(string fileName)
        {
            if(fileName == null)
                return null;
            return fileName
                .CreateAssemblyFromFile()
                .GetType(typeof(TaabusProject).Name);
        }
    }

    interface ITaabusController
    {
        void OnClosed();
        void OnActivate(IControlledItem controlledItem);
    }

    interface IControlledItem
    {
        string Title { get; }
    }
}