using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using hw.DebugFormatter;

namespace Taabus.UserInterface
{
    sealed class DragDropController : DumpableObject
    {
        static int _nextObjectId;
        [EnableDump]
        readonly int _objectId;
        [EnableDump]
        readonly ISource _source;
        [EnableDump]
        readonly List<IDestination> _destinations = new List<IDestination>();

        [EnableDump]
        Point? _sourcePoint;

        public DragDropController(ISource source)
        {
            _objectId = _nextObjectId++;
            _source = source;
            ActivaterDrag();
            //Tracer.FlaggedLine("\n" + Tracer.Dump(this));
        }

        void ActivaterDrag()
        {
            var control = DragControl;
            //Tracer.FlaggedLine(_objectId + " " + control.Tag);
            control.MouseDown += (s, e) => LastMouseDown = e;
            control.MouseUp += (s, e) => LastMouseDown = null;
            control.QueryContinueDrag += (s, e) => OnSourceQueryContinueDrag(e);
            control.MouseMove += (s, e) => OnSourceMouseMove(e);
        }

        Control DragControl
        {
            get
            {
                var proxySource = _source as IProxySource;
                if(proxySource != null)
                    return proxySource.DragControl;
                return _source.Control;
            }
        }

        MouseEventArgs LastMouseDown
        {
            set
            {
                _sourcePoint = value == null ? (Point?) null : new Point(value.X, value.Y);
                //Tracer.FlaggedLine(_objectId + " " + _sourcePoint);
            }
        }

        internal void AddDestination(IDestination destination) { _destinations.Insert(0, destination); }

        void ActivateDrop(IDestination destination)
        {
            destination.Control.AllowDrop = true;
            destination.Control.DragOver += OnTargetDragOver;
            destination.Control.DragDrop += OnTargetDragDrop;
        }

        void DeActivateDrop(IDestination destination)
        {
            destination.Control.AllowDrop = false;
            destination.Control.DragOver -= OnTargetDragOver;
            destination.Control.DragDrop -= OnTargetDragDrop;
        }

        IItem Item { get { return _sourcePoint == null ? null : _source.GetItemAt(_sourcePoint.Value); } }
        Size Displacement { get { return _sourcePoint == null ? default(Size) : _source.GetDisplacementAt(_sourcePoint.Value); } }

        [DisableDump]
        Rectangle DragBox
        {
            get
            {
                Tracer.Assert(_sourcePoint != null);
                var dragSize = SystemInformation.DragSize;
                var location = _sourcePoint.Value - new Size(dragSize.Width / 2, dragSize.Height / 2);
                return new Rectangle(location, dragSize);
            }
        }

        internal bool IsMove { get; set; }
        internal bool IsCopy { get { return !IsMove; } set { IsMove = !value; } }
        internal bool HasMove { get; set; }
        internal bool HasCopy { get; set; }

        DragDropEffects AllowedEffects
        {
            get
            {
                var result = DefaultEffects;
                if(HasCopy)
                    result |= DragDropEffects.Copy;
                if(HasMove)
                    result |= DragDropEffects.Move;
                return result;
            }
        }
        DragDropEffects DefaultEffects { get { return IsMove ? DragDropEffects.Move : DragDropEffects.Copy; } }

        void OnSourceMouseMove(MouseEventArgs e)
        {
            //Tracer.FlaggedLine(_objectId + " " + e.Location);
            if((e.Button & MouseButtons.Left) != MouseButtons.Left)
                return;
            if(_sourcePoint == null)
                return;
            if(DragBox.Contains(e.X, e.Y))
                return;

            //Tracer.FlaggedLine(_objectId + " " + e.Location);
            foreach(var destination in _destinations)
                ActivateDrop(destination);

            _source.Control.DoDragDrop(_objectId, AllowedEffects);

            foreach(var destination in _destinations)
                DeActivateDrop(destination);
        }

        static void OnSourceQueryContinueDrag(QueryContinueDragEventArgs e)
        {
            if(e.EscapePressed)
                e.Action = DragAction.Cancel;
            //Tracer.FlaggedLine(_objectId.ToString() + " " + e.Action.ToString());
        }


        DestinationPoint FindDestinationPoint(object sender, DragEventArgs e)
        {
            SetEffect(e);
            if(e.IsNone())
                return null;

            if((AllowedEffects & e.Effect) == 0)
                return null;

            var target = _destinations.Single(t => t.Control == sender);
            return new DestinationPoint
            {
                Displacement = target.Control.PointToClient(new Point(e.X, e.Y)),
                Destination = target
            };
        }

        void OnTargetDragDrop(object sender, DragEventArgs e)
        {
            var targetPoint = FindDestinationPoint(sender, e);
            if(targetPoint == null)
                return;
            //Tracer.FlaggedLine(Tracer.Dump(targetPoint.Displacement));
            var location = targetPoint.Displacement - Displacement;
            switch(e.Effect)
            {
                case DragDropEffects.Copy:
                    targetPoint.Destination.Copy(Item, location);
                    return;
                case DragDropEffects.Move:
                    targetPoint.Destination.Move(Item, location);
                    return;
                case DragDropEffects.Link:
                    targetPoint.Destination.Link(Item, location);
                    return;
            }
        }

        void OnTargetDragOver(object sender, DragEventArgs e)
        {
            //Tracer.FlaggedLine(_objectId + " " + e.X + " : " + e.Y);
            SetEffect(e);
        }
        void SetEffect(DragEventArgs e) { e.SetEffect<int>(id => id == _objectId, DefaultEffects); }

        sealed class DestinationPoint
        {
            public Point Displacement;
            public IDestination Destination;
        }

        internal interface IItem
        {}

        internal interface IDestination
        {
            Control Control { get; }
            void Copy(IItem item, Point location);
            void Move(IItem item, Point location);
            void Link(IItem item, Point location);
        }

        internal interface ISource
        {
            Control Control { get; }
            IItem GetItemAt(Point point);
            Size GetDisplacementAt(Point point);
        }

        internal interface IProxySource
        {
            Control DragControl { get; }
        }
    }
}