using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using hw.Forms;
using Taabus.Properties;

namespace Taabus.UserInterface
{
    public sealed class WorkspaceView : MainView, DragDropController.ITarget
    {
        readonly ITaabusController _controller;
        internal WorkspaceView(string tag, ITaabusController controller)
            : base("Workspace." + tag, controller)
        {
            _controller = controller;
            AddFunction(new UserInteraction("Settings", OnConfiguration, Resources.appbar_settings));
            Client = new Panel();
        }

        static void OnConfiguration() { }
        internal void CallAddCard(IControlledItem item, Point? location = null) { Client.ThreadCallGuard(() => AddCard(item, location)); }
        internal void CallAddTable(IControlledItem item, Rectangle itemRectangle) { Client.ThreadCallGuard(() => AddTable(item, itemRectangle)); }

        void AddCard(IControlledItem item, Point? location = null)
        {
            var control = new CardView(this, item);
            control.Location = location ?? DefaultLocation(control);
            Client.Controls.Add(control);
        }

        void AddTable(IControlledItem item, Rectangle itemRectangle)
        {
            var control = new TableView(this, item)
            {
                Location = new Point(itemRectangle.X, (int) (itemRectangle.Bottom + itemRectangle.Height * 0.5)),
                Size = new Size(itemRectangle.Width * 3, itemRectangle.Height * 10)
            };
            Client.Controls.Add(control);
            control.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);
        }

        Point DefaultLocation(Control control)
        {
            var regions = Items
                .Select(c => new Rectangle(c.Location, c.Size))
                .ToArray();
            return FindPosition(control.Size, regions).Location;
        }

        internal IEnumerable<Control> Items
        {
            get
            {
                return Client
                    .Controls
                    ._()
                    .Where(c => c is CardView || c is TableView);
            }
            set
            {
                var panel = new Panel();
                panel.Controls.AddRange(value.ToArray());
                Client = panel;
            }
        }

        static Rectangle FindPosition(Size size, Rectangle[] regions)
        {
            var result = new Rectangle {Size = size};
            var increment = (int) (size.Height * 1.5);
            while(regions.Any(r => r.IntersectsWith(result)))
                result.Y += increment;
            return result;
        }

        Control DragDropController.ITarget.Control { get { return Client; } }

        void DragDropController.ITarget.Drop(DragDropController.IItem item, Point location) { CallAddCard(item as IControlledItem, location); }

        bool Contains(Point point, Region region)
        {
            NotImplementedMethod(point, region);
            return true;
        }

        internal override void Reload() { Items = _controller.WorkspaceItems.Select(CreateItem); }
        Control CreateItem(WorkspaceItem item)
        {
            NotImplementedMethod(item);
            return null;
        }
    }
}