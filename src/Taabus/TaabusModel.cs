using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using hw.Debug;
using hw.Forms;
using hw.Helper;
using MetroFramework.Forms;
using Microsoft.Data.ConnectionUI;
using Taabus.Properties;
using Taabus.UserInterface;

namespace Taabus
{
    sealed class TaabusController : DumpableObject
    {
        const int FrameSize = 10;
        const int AppBarSize = 25;

        readonly MetroForm _mainForm = new MetroForm();
        readonly ToolStrip _toolbar = new ToolStrip();
        readonly TreeView _tree = new TreeView();
        readonly UserInteraction[] _functions;

        Project _data;

        public TaabusController()
        {
            _functions = new[]
            {
                new UserInteraction("Open", OnOpen, Resources.appbar_folder_open),
                new UserInteraction("New", OnNew, Resources.appbar_add),
                new UserInteraction("Edit", OnEdit, Resources.appbar_edit, () => SelectedItem is Server),
                new UserInteraction("Remove", OnRemove, Resources.appbar_delete, () => SelectedItem is Server),
                new UserInteraction("Settings", OnConfiguration, Resources.appbar_settings)
            };

            _toolbar.Items.AddRange(_functions.Select(f => f.Button).ToArray());
            _toolbar.Stretch = true;

            _tree.ImageList = Images.Instance;
            _tree.AfterSelect += (s, e) => OnAfterSelect(_functions);

            var cs = _mainForm.ClientSize;
            var ts = _toolbar.Size;
            _toolbar.Location = new Point(cs.Width - ts.Width - FrameSize, AppBarSize);
            var tl = _toolbar.Location;
            _tree.Location = new Point(FrameSize, tl.Y + ts.Height);
            _tree.Size = new Size(cs.Width - 2 * FrameSize, cs.Height - FrameSize - tl.Y - ts.Height);

            _toolbar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            _tree.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            _mainForm.SuspendLayout();
            _mainForm.Name = "Taabus.MainForm";
            _mainForm.Controls.Add(_toolbar);
            _mainForm.Controls.Add(_tree);
            _mainForm.ResumeLayout();
            _mainForm.InstallPositionConfig();

            FileName = LastFileNameFileName.FileHandle().String;
        }

        string FileName
        {
            get { return _data == null ? null : _data.FileName; }
            set
            {
                var fileHandle = LastFileNameFileName.FileHandle();
                _data = value == null ? null : new Project(value);
                ReConnect();
                if(value != null)
                    fileHandle.String = value;
                else if(fileHandle.Exists)
                    fileHandle.Delete();
                _mainForm.Text = Title;
            }
        }

        string LastFileNameFileName { get { return _mainForm.Name + ".lastFile"; } }

        [DisableDump]
        NamedObject SelectedItem { get { return (NamedObject) _tree.SelectedNode.Tag; } }

        internal string Title { get { return _data == null ? "<no file>" : _data.Name; } }

        void OnOpen()
        {
            var d = new OpenFileDialog
            {
                Title = "Taabus project file",
                RestoreDirectory = true,
                InitialDirectory = FileName == null ? "." : FileName.FileHandle().DirectoryName,
                FileName = FileName == null ? null : FileName.FileHandle().Name,
                Filter = "Taabus files|*.taabus|All files|*.*",
                CheckFileExists = false,
                FilterIndex = 2
            };

            if(d.ShowDialog() != DialogResult.OK)
                return;
            FileName = d.FileName;
        }

        void OnNew()
        {
            var server = new Server();
            var selectedNode = _tree.SelectedNode;
            var current = selectedNode == null ? null : (Server) selectedNode.Chain(x => x.Parent).Last().Tag;
            var position = _data.Servers.IndexOf(s => s == current) ?? _data.Servers.Count();
            _data.Servers = _data
                .Servers
                .Take(position)
                .Concat(new[] {server})
                .Concat(_data.Servers.Skip(position))
                .ToArray();
            ReConnect();
            _tree.SelectedNode = _tree.Nodes[position];
            OnEdit();
        }
        void ReConnect() { _tree.Connect(_data); }

        void OnEdit()
        {
            var server = (Server) _tree.SelectedNode.Chain(x => x.Parent).Last().Tag;
            var dcd = new DataConnectionDialog();
            dcd.DataSources.Add(DataSource.SqlDataSource);
            dcd.ConnectionString = server.ConnectionString;
            DataConnectionDialog.Show(dcd);
            server.ConnectionString = dcd.ConnectionString;
            ReConnect();
        }
        static void OnRemove() { }
        static void OnConfiguration() { }

        internal void Run()
        {
            if(FileName == null)
                OnOpen();
            if(FileName != null)
                Application.Run(_mainForm);
        }

        static void OnAfterSelect(IEnumerable<UserInteraction> functions)
        {
            foreach(var function in functions)
                function.Refresh();
        }
    }
}