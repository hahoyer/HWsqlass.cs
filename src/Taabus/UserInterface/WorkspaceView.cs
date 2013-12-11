using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
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
        internal void Add(IControlledItem item, Point location) { _panel.ThreadCallGuard(() => AddCard(item, location)); }

        void AddCard(IControlledItem item, Point? location = null)
        {
            var control = CreateCard(item);
            control.Location = location ?? DefaultLocation(control);
            _panel.Controls.Add(control);
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

        Control CreateCard(IControlledItem item) { return new CardView(this, item); }

        Control DragDropController.ITarget.Control { get { return _panel; } }

        void DragDropController.ITarget.Drop(DragDropController.IItem item, Point location) { Add(item as IControlledItem, location); }

        bool Contains(Point point, Region region)
        {
            NotImplementedMethod(point, region);
            return true;
        }
    }
}