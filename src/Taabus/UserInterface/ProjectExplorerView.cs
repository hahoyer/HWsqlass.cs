using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using hw.Forms;
using hw.Helper;
using Microsoft.Data.ConnectionUI;
using Taabus.Data;
using Taabus.External;
using Taabus.Properties;

namespace Taabus.UserInterface
{
    sealed class ProjectExplorerView : MainView, DragDropController.ISource
    {
        readonly ITaabusController _controller;
        readonly TreeView _tree = new TreeView();
        readonly DragDropController _dragDropController;
        Project _project;

        internal ProjectExplorerView(ITaabusController controller)
            : base("Explorer", controller)
        {
            _controller = controller;
            AddFunction(
                new UserInteraction("Open", OnOpen, Resources.appbar_folder_open),
                new UserInteraction("New", OnNew, Resources.appbar_add),
                new UserInteraction("Edit", OnEdit, Resources.appbar_edit, () => SelectedItem is Server),
                new UserInteraction("Remove", OnRemove, Resources.appbar_delete, () => SelectedItem is Server),
                new UserInteraction("Settings", OnConfiguration, Resources.appbar_settings)
                );

            _tree.ImageList = Images.Instance;
            _tree.AfterSelect += (s, e) => OnAfterSelect();
            _tree.DragDrop += OnDragDrop;
            _tree.AfterCollapse += (s, e) => OnCollapse();
            _tree.AfterExpand += (s, e) => OnExpand();
            Client = _tree;
            _dragDropController = new DragDropController(this);
        }
        Control DragDropController.ISource.Control { get { return _tree; } }
        DragDropController.IItem DragDropController.ISource.GetItemAt(Point point) { return _tree.GetNodeAt(point).Tag as DragDropController.IItem; }
        Size DragDropController.ISource.GetDisplacementAt(Point point) { return new Size(0, 0); }

        void OnDragDrop(object sender, DragEventArgs e) { NotImplementedMethod(sender, e); }

        NamedObject SelectedItem { get { return (NamedObject) _tree.SelectedNode.Tag; } }
        ExpansionDescription[] ExpandedNodes { get { return ScanNodes(_tree.Nodes); } set { ScanNodes(_tree.Nodes, value); } }

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
                return _project.Servers.IndexWhere(s => s == current) ?? _project.Servers.Count();
            }
        }

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

        internal TreeView DragSource { get { return _tree; } }

        void OnOpen() { _controller.OnOpen(); }
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

        void OnAfterSelect()
        {
            foreach(var function in Functions)
                function.Refresh();
            _controller.Selection = SelectedPath;
        }

        void OnExpand() { SaveExpansionDescriptions(); }
        void OnCollapse() { SaveExpansionDescriptions(); }

        void OnChange(Server server)
        {
            var expandedNodes = ExpandedNodes;
            _tree.Connect(_project);
            ExpandedNodes = expandedNodes;
            SelectedServer = server;
            _controller.Servers = _project.Servers.ToArray();
        }

        void EditServer(Server server)
        {
            var useServer = server ?? new Server();
            var dialog = new DataConnectionDialog();
            dialog.DataSources.Add(DataSource.SqlDataSource);
            dialog.ConnectionString = useServer.ConnectionString;
            if(DataConnectionDialog.Show(dialog) != DialogResult.OK)
                return;
            if(server == null)
                _project.InsertServer(useServer, SelectedServerPosition);
            useServer.ConnectionString = dialog.ConnectionString;
            OnChange(useServer);
        }

        static void ScanNodes(TreeNodeCollection treeNodes, ExpansionDescription[] value)
        {
            foreach(var description in value)
            {
                var node = treeNodes[description.Id];
                if(node != null)
                {
                    var nodes = node.Nodes;
                    nodes.BeforeExpand();
                    if(description.IsExpanded)
                        node.Expand();
                    ScanNodes(nodes, description.Nodes);
                }
            }
        }

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
            if(index >= value.Length)
                return null;
            var node = nodes[value[index]];
            if(node == null)
                return null;
            return PathWalk(value, node.Nodes, index + 1) ?? node;
        }

        public void AddDropSite(DragDropController.ITarget target) { _dragDropController.Add(target); }

        internal override void Reload() { _tree.ThreadCallGuard(InternalReload); }

        void InternalReload()
        {
            _project = new Project {Servers = _controller.Servers, ProjectName = _controller.ProjectName};
            _tree.Connect(_project);
            ExpandedNodes = _controller.ExpansionDescriptions;
            _tree.TopNode = _tree.Nodes._().FirstOrDefault();
            SelectedPath = _controller.Selection ?? new string[0];
            var selectedNode = _tree.SelectedNode;
            if(selectedNode != null)
                selectedNode.EnsureVisible();
        }

        void SaveExpansionDescriptions() { _controller.ExpansionDescriptions = ExpandedNodes; }
    }
}