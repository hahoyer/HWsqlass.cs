using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Taabus.UserInterface;

namespace Taabus
{
    sealed class TaabusController : ApplicationContext, ITaabusController
    {
        ProjectExplorerView _explorer;
        WorkspaceView _workspace;
        DragDropController _dragDropController;

        void ITaabusController.Exit() { ExitThread(); }

        internal void Run()
        {
            Explorer = new ProjectExplorerView(this);
            Workspace = new WorkspaceView("Main", this);
            Explorer.Show();
            Workspace.Show();
            Application.Run(this);
        }

        internal static Type GetTypeFromFile(string fileName)
        {
            if(fileName == null)
                return null;
            return fileName
                .CreateAssemblyFromFile()
                .GetType(typeof(TaabusProject).Name);
        }

        ProjectExplorerView Explorer
        {
            get { return _explorer; }
            set
            {
                _explorer = value;
                EnsureDragDropController();
            }
        }
        WorkspaceView Workspace
        {
            get { return _workspace; }
            set
            {
                _workspace = value;
                EnsureDragDropController();
            }
        }

        void EnsureDragDropController()
        {
            if(_dragDropController != null)
                return;
            if(_explorer == null)
                return;
            if(_workspace == null)
                return;
            _dragDropController = new DragDropController(Explorer, Workspace);
        }
    }

    interface ITaabusController
    {
        void Exit();
    }

    interface IControlledItem : IDragDropItem
    {
        string Title { get; }
        long Count { get; }
    }
}