﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using hw.DebugFormatter;
using hw.Helper;
using Taabus.Data;
using Taabus.External;
using Taabus.UserInterface;

namespace Taabus
{
    sealed class Controller : ApplicationContext, ITaabusController, IFileOpenController
    {
        const string LastFileNameFileName = "Taabus.lastFile";

        ProjectExplorerView _explorer;
        WorkspaceView _workspace;
        string _fileName;

        public Controller()
        {
            Tracer.Dumper.Configuration.Handlers.Force(typeof(Control), methodCheck: IsDumpable);
            Tracer.Dumper.Configuration.Handlers.Force(typeof(Component), methodCheck: IsDumpable);
            _configurationCache = new ValueCache<PersistentConfiguration>(GetConfiguration);
        }

        static bool IsDumpable(MemberInfo memberInfo, object data)
        {
            return memberInfo.Name.In("Name", "Title", "Text");
        }

        readonly ValueCache<PersistentConfiguration> _configurationCache;
        PersistentConfiguration Configuration { get { return _configurationCache.Value; } }
        PersistentConfiguration GetConfiguration() { return PersistentConfiguration.Load(FileName); }

        ExpansionDescription[] ITaabusController.ExpansionDescriptions { get { return Configuration.Data.ExpansionDescriptions; } set { Configuration.Data.ExpansionDescriptions = value; } }
        string[] ITaabusController.Selection { get { return Configuration.Data.Selection; } set { Configuration.Data.Selection = value; } }
        Server[] ITaabusController.Servers { get { return Configuration.Data.Servers; } set { Configuration.Data.Servers = value; } }
        External.Item[] ITaabusController.Items { get { return Configuration.Data.Items; } set { Configuration.Data.Items = value; } }

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
            _explorer = new ProjectExplorerView(this);
            _workspace = new WorkspaceView("Main", this);
            _explorer.Show();
            _workspace.Show();

            FileName = LastFileNameFileHandle.String;
            if(FileName == null)
                OnOpen();
            if(FileName == null)
                return;

            var t = new System.Timers.Timer(TimeSpan.FromSeconds(2).TotalMilliseconds) {AutoReset = true};
            t.Elapsed += (s, e) => CheckForSave();
            t.Enabled = true;

            EnsureDragDropController();
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
            lock(_configurationCache)
                if(_configurationCache.IsValid)
                    Configuration.CheckedSave();
        }

        void EnsureDragDropController()
        {
            if(_explorer == null)
                return;
            if(_workspace == null)
                return;
            _explorer.AddDropSite(_workspace);
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