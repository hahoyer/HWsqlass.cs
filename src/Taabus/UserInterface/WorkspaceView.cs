using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Taabus.Properties;

namespace Taabus.UserInterface
{
    sealed class WorkspaceView : MainView, IDragDropTarget
    {
        readonly Panel _panel = new Panel();

        internal WorkspaceView(string tag, ITaabusController controller)
            : base("Workspace." + tag, controller)
        {
            AddFunction(new UserInteraction("Settings", OnConfiguration, Resources.appbar_settings));
            Client = _panel;
        }

        static void OnConfiguration() { }
        internal void Add(IControlledItem item) { _panel.ThreadCallGuard(() => AddCard(item)); }

        void AddCard(IControlledItem item)
        {
            var control = CreateCard(item);
            var rectangle = FindPosition(control.Size, _panel.Controls._().Select(c => new Rectangle(c.Location, c.Size)).ToArray());
            control.Location = rectangle.Location;
            _panel.Controls.Add(control);
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

        Control IDragDropTarget.Control { get { return _panel; } }
        void IDragDropTarget.Drop(IDragDropItem item)
        {
            var i = item as IControlledItem;
            Add(i);
        }
    }
}