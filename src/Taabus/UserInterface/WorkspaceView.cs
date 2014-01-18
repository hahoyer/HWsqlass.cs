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
        void DragDropController.IDestination.Move(DragDropController.IItem item, Point location) { ((Control) item).Location = location; }

        void DragDropController.IDestination.Link(DragDropController.IItem item, Point location) { NotImplementedMethod(item, location); }

        internal void CallAddCard(TypeItemView.IItem item, Point? location = null) { Client.ThreadCallGuard(() => AddCard(item, location)); }
        internal void CallAddTable(TableView.IItem item, Rectangle itemRectangle) { Client.ThreadCallGuard(() => AddTable(item, itemRectangle)); }

        void AddCard(TypeItemView.IItem item, Point? location = null)
        {
            var control = new TypeItemView(item, this);
            control.Location = location ?? DefaultLocation(control);
            AddItem(control);
        }

        void AddTable(TableView.IItem item, Rectangle itemRectangle)
        {
            var control = new TableView(item)
            {
                Location = new Point(itemRectangle.X, (int) (itemRectangle.Bottom + itemRectangle.Height * 0.5)),
                Size = new Size(itemRectangle.Width * 3, itemRectangle.Height * 10)
            };
            AddItem(control);
            control.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);
        }

        Point DefaultLocation(Control control)
        {
            var regions = ItemControls
                .Select(c => new Rectangle(c.Location, c.Size))
                .ToArray();
            return FindPosition(control.Size, regions).Location;
        }

        IEnumerable<Control> ItemControls
        {
            get
            {
                return Client
                    .Controls
                    ._()
                    .Where(c => c is Internalizer.IItem);
            }
            set
            {
                var panel = new Panel();
                panel.Controls.AddRange(value.ToArray());
                Client = panel;
            }
        }

        IEnumerable<Internalizer.IItem> Items { get { return ItemControls.Cast<Internalizer.IItem>(); } set { ItemControls = value.Cast<Control>(); } }
        IEnumerable<Item> ExternalItems { get { return new Externalizer(Items).Execute(); } set { Items = new Internalizer(value, this).Execute(); } }

        void AddItem(Control control)
        {
            Client.Controls.Add(control);
            Controller.Items = ExternalItems.ToArray();
        }

        internal override void Reload() { ExternalItems = Controller.Items; }

        static Rectangle FindPosition(Size size, Rectangle[] regions)
        {
            var result = new Rectangle {Size = size};
            var increment = (int) (size.Height * 1.5);
            while(regions.Any(r => r.IntersectsWith(result)))
                result.Y += increment;
            return result;
        }
    }
}