using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using hw.Helper;
using Taabus.Data;
using Taabus.UserInterface;

namespace Taabus
{
    sealed class Controller : ApplicationContext, ITaabusController, IFileOpenController
    {
        const string LastFileNameFileName = "Taabus.lastFile";

        ProjectExplorerView _explorer;
        WorkspaceView _workspace;
        string _fileName;

        public Controller() { _configurationCache = new ValueCache<PersistentConfiguration>(GetConfiguration); }

        readonly ValueCache<PersistentConfiguration> _configurationCache;
        PersistentConfiguration Configuration { get { return _configurationCache.Value; } }
        PersistentConfiguration GetConfiguration() { return PersistentConfiguration.Load(FileName); }

        ExpansionDescription[] ITaabusController.ExpansionDescriptions { get { return Configuration.Data.ExpansionDescriptions; } set { Configuration.Data.ExpansionDescriptions = value; } }
        string[] ITaabusController.Selection { get { return Configuration.Data.Selection; } set { Configuration.Data.Selection = value; } }
        Server[] ITaabusController.Servers { get { return Configuration.Data.Servers; } set { Configuration.Data.Servers = value; } }
        WorkspaceItem[] ITaabusController.WorkspaceItems { get { return Configuration.Data.WorkspaceItems; } }

        string ITaabusController.ProjectName { get { return _fileName.FileHandle().Name; } }
        void ITaabusController.OnOpen() { OnOpen(); }

        void ITaabusController.Exit()
        {
            CheckForSave();
            ExitThread();
        }

        string IFileOpenController.FileName { get { return FileName; } set { FileName = value; } }
        string IFileOpenController.CreateEmptyFile { get { return PersistentConfiguration.CreateEmpty(); } }

        void OnOpen() { this.OnFileOpen("Taabus project file", "Taabus files|*.taabus|All files|*.*"); }

        internal void Run()
        {
            Explorer = new ProjectExplorerView(this);
            Workspace = new WorkspaceView("Main", this);
            Explorer.Show();
            Workspace.Show();

            FileName = LastFileNameFileHandle.String;
            if(FileName == null)
                OnOpen();
            if(FileName == null)
                return;

            Application.Run(this);
        }

        string FileName
        {
            get { return _fileName; }
            set
            {
                if(_fileName == value)
                    return;

                CheckForSave();
                _configurationCache.IsValid = false;
                _fileName = value;
                _explorer.Reload();
                _workspace.Reload();
                SetLastFileNameConfig();
            }
        }
        void CheckForSave()
        {
            if(_configurationCache.IsValid)
                Configuration.CheckedSave();
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
            if(_explorer == null)
                return;
            if(_workspace == null)
                return;
            _explorer.AddDropSite(Workspace);
        }

        static File LastFileNameFileHandle { get { return LastFileNameFileName.FileHandle(); } }

        void SetLastFileNameConfig()
        {
            if(FileName != null)
                LastFileNameFileHandle.String = FileName;
            else if(LastFileNameFileHandle.Exists)
                LastFileNameFileHandle.Delete();
        }
    }
}