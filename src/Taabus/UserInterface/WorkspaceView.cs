using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using hw.Forms;
using Taabus.External;
using Taabus.Properties;

namespace Taabus.UserInterface
{
    sealed class WorkspaceView : MainView, DragDropController.IDestination
    {
        internal readonly ITaabusController Controller;
        internal WorkspaceView(string tag, ITaabusController controller)
            : base("Workspace." + tag, controller)
        {
            Controller = controller;
            AddFunction(new UserInteraction("Settings", OnConfiguration, Resources.appbar_settings));
            Client = new Panel();
        }

        static void OnConfiguration() { }

        Control DragDropController.IDestination.Control { get { return Client; } }
        void DragDropController.IDestination.Copy(DragDropController.IItem item, Point location) { CallAddCard((TypeItemView.IItem) item, location); }
        void DragDropController.IDestination.Move(DragDropController.IItem item, Point location)
        {
            var control = ((Control) item);
            control.Location = location;
            control.BringToFront();
            Save();
        }

        void DragDropController.IDestination.Link(DragDropController.IItem item, Point location) { NotImplementedMethod(item, location); }

        void CallAddCard(TypeItemView.IItem item, Point? location = null) { Client.ThreadCallGuard(() => AddCard(item, location)); }
        internal void CallAddTable(TableView.IItem item, Rectangle itemRectangle) { Client.ThreadCallGuard(() => AddTable(item, itemRectangle)); }

        void AddCard(TypeItemView.IItem item, Point? location = null)
        {
            var control = new TypeItemView(item, this);
            control.Location = location ?? DefaultLocation(control);
            AddItem(control);
        }

        void AddTable(TableView.IItem item, Rectangle itemRectangle)
        {
            var control = new TableView(item, this)
            {
                Location = new Point(itemRectangle.X, (int) (itemRectangle.Bottom + itemRectangle.Height * 0.5)),
                Size = new Size(itemRectangle.Width * 3, itemRectangle.Height * 10)
            };
            control.LoadData();
            AddItem(control);
            control.OnAdded();
        }

        Point DefaultLocation(Control control)
        {
            var regions = ItemControls
                .Select(c => new Rectangle(c.Control.Location, c.Control.Size))
                .ToArray();
            return FindPosition(control.Size, regions).Location;
        }

        IEnumerable<IControl> ItemControls
        {
            get
            {
                return Client
                    .Controls
                    ._()
                    .Cast<IControl>();
            }
            set
            {

                var panel = new Panel();
                var controls = value.Select(c => c.Control).ToArray();
                panel.Controls.AddRange(controls);
                Client = panel;
                foreach (var control in value)
                    InstallDragDropController(control);
            }
        }

        IEnumerable<Item> ExternalItems
        {
            get
            {
                var items = ItemControls
                    .Cast<Internalizer.IItem>();
                return new Externalizer()
                    .Execute(items);
            }
            set
            {
                ItemControls = new Internalizer(value, this)
                    .Execute()
                    .Cast<IControl>()
                    .ToArray();
            }
        }

        void AddItem(DragDropController.ISource control)
        {
            Client.Controls.Add(control.Control);
            InstallDragDropController(control);
            Save();
        }

        internal void Save() { Controller.Items = ExternalItems.ToArray(); }

        internal override void Reload() { ExternalItems = Controller.Items; }

        static Rectangle FindPosition(Size size, Rectangle[] regions)
        {
            var result = new Rectangle {Size = size};
            var increment = (int) (size.Height * 1.5);
            while(regions.Any(r => r.IntersectsWith(result)))
                result.Y += increment;
            return result;
        }

        internal interface IControl : DragDropController.ISource, DragDropController.IItem
        {}

        internal void InstallDragDropController(DragDropController.ISource source)
        {
            var dragDropController = new DragDropController(source)
            {
                IsMove = true,
                HasCopy = false,
            };
            dragDropController.AddDestination(this);
        }
    }
}