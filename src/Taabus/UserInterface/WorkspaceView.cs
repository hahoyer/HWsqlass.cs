using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using hw.Forms;
using hw.Helper;
using Taabus.External;
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
            AddItem(control);
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

        IEnumerable<Control> Items
        {
            get
            {
                return Client
                    .Controls
                    ._()
                    .Where(c => c is IWorkspaceItem);
            }
            set
            {
                var panel = new Panel();
                panel.Controls.AddRange(value.ToArray());
                Client = panel;
            }
        }

        IEnumerable<Item> ExternalItems
        {
            get
            {
                return Items
                    .Select(i => CreateExternalItem(i, ((IWorkspaceItem) i).Item));
            }
            set { Items = value.Select(CreateWorkSpaceItem); }
        }

        void AddItem(Control control)
        {
            Client.Controls.Add(control);
            _controller.Items = ExternalItems.ToArray();
        }

        internal override void Reload() { ExternalItems = _controller.Items; }

        Control CreateWorkSpaceItem(Item item)
        {
            var constructor = item
                .Type
                .ResolveUniqueType()
                .GetConstructor(BindingFlags.NonPublic|BindingFlags.Public|BindingFlags.Instance,null, new []{GetType(), typeof(IControlledItem)},null);

            var result = ((Control) constructor.Invoke(new object[]
            {
                this,
                item.Data.ToControlledItem(_controller)
            }));
            result.Location = new Point(item.X, item.Y);
            result.Size = new Size(item.Width, item.Height);
            return result;
        }

        static Item CreateExternalItem(Control control, IControlledItem controlledItem)
        {
            return new Item
            {
                Type = control.GetType().CompleteName(),
                X = control.Location.X,
                Y = control.Location.Y,
                Width = control.Size.Width,
                Height = control.Size.Height,
                Data = controlledItem.ToDataItem
            };
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
    }
}