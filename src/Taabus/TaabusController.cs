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

        string _fileName;
        Project _project;

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
            _mainForm.InstallPositionConfig();
            _mainForm.Load += (s, e) => OnLoad();
            _mainForm.Closed += (s, e) => OnClosed();
            _mainForm.ResumeLayout();
        }

        File LastFileNameFileHandle { get { return LastFileNameFileName.FileHandle(); } }
        string LastFileNameFileName { get { return _mainForm.Name + ".lastFile"; } }
        NamedObject SelectedItem { get { return (NamedObject) _tree.SelectedNode.Tag; } }
        ExpansionDescription[] ExpandedNodes { get { return ScanNodes(_tree.Nodes); } set { ScanNodes(_tree.Nodes, value); } }

        string[] SelectedPath
        {
            get
            {
                return _tree
                    .SelectedNode
                    .Chain(n => n.Parent)
                    .Reverse()
                    .Select(n => n.Name)
                    .ToArray();
            }
            set { _tree.SelectedNode = PathWalk(value, _tree.Nodes, 0); }
        }

        string FileName
        {
            get { return _fileName; }
            set
            {
                if(_fileName == value)
                    return;
                _fileName = value;
                OnFileNameChanged();
            }
        }

        Server SelectedServer
        {
            get
            {
                var selectedNode = _tree.SelectedNode;
                if(selectedNode == null)
                    return null;
                return (Server) selectedNode.Chain(x => x.Parent).Last().Tag;
            }
            set { _tree.SelectedNode = _tree.Nodes._().First(n => n.Tag == value); }
        }

        int SelectedServerPosition
        {
            get
            {
                var current = SelectedServer;
                return _project.Servers.IndexOf(s => s == current) ?? _project.Servers.Count();
            }
        }

        Type ProjectInstanceFromFile
        {
            get
            {
                if (_fileName == null)
                    return null;
                return _fileName
                    .CreateAssemblyFromFile()
                    .GetType(typeof(TaabusProject).Name);
            }
        }

        void OnLoad()
        {
            FileName = LastFileNameFileName.FileHandle().String;

            if (FileName == null)
                OnOpen();

            if (FileName == null)
                _mainForm.Close();
        }

        void OnFileNameChanged()
        {
            var type = ProjectInstanceFromFile;
            
            if (type == null)
                _project = null;
            else
            {
                _project = type.Invoke<Project>("Project");
                _project.ProjectName = _fileName.FileHandle().Name;
            }

            _tree.Connect(_project);
            if(type != null)
            {
                ExpandedNodes = type.Invoke<ExpansionDescription[]>("ExpansionDescriptions");
                _tree.TopNode = Profiler.Measure(()=>_tree.Nodes._().FirstOrDefault());
                SelectedPath = type.Invoke<string[]>("Selection") ?? new string[0];
                if(_tree.SelectedNode != null)
                    _tree.SelectedNode.EnsureVisible();
            }
            
            SetLastFileNameConfig();
            _mainForm.Text = _project == null ? "<no file>" : _project.Name;
        }

        void OnChange(Server server)
        {
            var expandedNodes = ExpandedNodes;
            _tree.Connect(_project);
            ExpandedNodes = expandedNodes;
            SelectedServer = server;
            SaveFile();
        }

        void OnClosed() { SaveFile(); }

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
            var newFile = d.FileName.FileHandle();
            if(!newFile.Exists)
                newFile.String = new Generator().TransformText();
            FileName = d.FileName;
        }

        void OnNew() { EditServer(null); }
        void OnEdit() { EditServer(SelectedServer); }

        void OnRemove()
        {
            var server = SelectedServer;
            var position = SelectedServerPosition;
            _project.Servers = _project.Servers.Where(s => s != server).ToArray();
            OnChange(_project.Servers.Skip(position).FirstOrDefault());
        }

        static void OnConfiguration() { }

        internal void Run() { Application.Run(_mainForm); }

        static void OnAfterSelect(IEnumerable<UserInteraction> functions)
        {
            foreach(var function in functions)
                function.Refresh();
        }

        void SetLastFileNameConfig()
        {
            if (_fileName != null)
                LastFileNameFileHandle.String = _fileName;
            else if (LastFileNameFileHandle.Exists)
                LastFileNameFileHandle.Delete();
        }

        void InsertServer(Server server)
        {
            var position = SelectedServerPosition;
            _project.Servers = _project
                .Servers
                .Take(position)
                .Concat(new[] { server })
                .Concat(_project.Servers.Skip(position))
                .ToArray();
        }

        void SaveFile()
        {
            _fileName
                .FileHandle()
                .String
                = new Generator(_project, ExpandedNodes, SelectedPath)
                    .TransformText();
        }

        void EditServer(Server server)
        {
            var useServer = server ?? new Server();
            var dialog = new DataConnectionDialog();
            dialog.DataSources.Add(DataSource.SqlDataSource);
            dialog.ConnectionString = useServer.ConnectionString;
            if (DataConnectionDialog.Show(dialog) != DialogResult.OK)
                return;
            if (server == null)
                InsertServer(useServer);
            useServer.ConnectionString = dialog.ConnectionString;
            OnChange(useServer);
        }

        void ScanNodes(TreeNodeCollection treeNodes, ExpansionDescription[] value)
        {
            foreach (var description in value)
            {
                var node = treeNodes[description.Id];
                if (node != null)
                {
                    var nodes = node.Nodes;
                    if (description.IsExpanded)
                    {
                        nodes.BeforeExpand();
                        node.Expand();
                        ScanNodes(nodes, description.Nodes);
                    }
                    else
                        ScanClosedNodes(nodes, description.Nodes);
                }
            }
        }

        void ScanClosedNodes(TreeNodeCollection nodes, ExpansionDescription[] expansionDescriptions) { NotImplementedMethod(nodes, expansionDescriptions); }

        static ExpansionDescription[] ScanNodes(TreeNodeCollection treeNodes)
        {
            return
                treeNodes
                    .Cast<TreeNode>()
                    .Select(rootNode =>
                        new ExpansionDescription
                        {
                            Id = rootNode.Tag is LazyNode ? null : rootNode.Name,
                            IsExpanded = rootNode.IsExpanded,
                            Nodes = ScanNodes(rootNode.Nodes)
                        })
                    .Where(r => r.IsExpanded || r.Nodes.Any())
                    .ToArray();
        }

        static TreeNode PathWalk(string[] value, TreeNodeCollection nodes, int index)
        {
            if (index >= value.Length)
                return null;
            var node = nodes[value[index]];
            if (node == null)
                return null;
            return PathWalk(value, node.Nodes, index + 1) ?? node;
        }

    }
}