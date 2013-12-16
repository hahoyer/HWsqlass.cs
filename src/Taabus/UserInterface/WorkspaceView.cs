using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using hw.Forms;
using Taabus.Properties;

namespace Taabus.UserInterface
{
    sealed class WorkspaceView : MainView, DragDropController.ITarget
    {
        readonly Panel _panel = new Panel();

        internal WorkspaceView(string tag, ITaabusController controller)
            : base("Workspace." + tag, controller)
        {
            AddFunction(new UserInteraction("Settings", OnConfiguration, Resources.appbar_settings));
            Client = _panel;
        }

        static void OnConfiguration() { }
        internal void CallAddCard(IControlledItem item, Point? location = null) { _panel.ThreadCallGuard(() => AddCard(item, location)); }
        internal void CallAddTable(IControlledItem item, Rectangle itemRectangle) { _panel.ThreadCallGuard(() => AddTable(item, itemRectangle)); }

        void AddCard(IControlledItem item, Point? location = null)
        {
            var control = new CardView(this, item);
            control.Location = location ?? DefaultLocation(control);
            _panel.Controls.Add(control);
        }

        void AddTable(IControlledItem item, Rectangle itemRectangle)
        {
            var control = new TableView(this, item)
            {
                Location = new Point(itemRectangle.X, (int) (itemRectangle.Bottom + itemRectangle.Height * 0.5)), 
                Size = new Size(itemRectangle.Width * 3, itemRectangle.Height * 10)
            };
            _panel.Controls.Add(control);
            control.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);
        }

        Point DefaultLocation(Control control)
        {
            var regions = _panel
                .Controls
                ._()
                .Select(c => new Rectangle(c.Location, c.Size))
                .ToArray();
            return FindPosition(control.Size, regions).Location;
        }

        static Rectangle FindPosition(Size size, Rectangle[] regions)
        {
            var result = new Rectangle {Size = size};
            var increment = (int) (size.Height * 1.5);
            while(regions.Any(r => r.IntersectsWith(result)))
                result.Y += increment;
            return result;
        }

        Control DragDropController.ITarget.Control { get { return _panel; } }

        void DragDropController.ITarget.Drop(DragDropController.IItem item, Point location) { CallAddCard(item as IControlledItem, location); }

        bool Contains(Point point, Region region)
        {
            NotImplementedMethod(point, region);
            return true;
        }

    }

}