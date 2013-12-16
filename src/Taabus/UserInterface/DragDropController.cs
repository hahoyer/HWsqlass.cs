using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using hw.Debug;
using hw.Forms;
using hw.Helper;

namespace Taabus.UserInterface
{
    sealed class DragDropController : DumpableObject
    {
        static int _nextObjectId;
        readonly int _objectId;
        readonly ISource _source;
        ITarget[] _targets = new ITarget[0];

        Point? _sourcePoint;

        public DragDropController(ISource source)
        {
            _objectId = _nextObjectId++;
            _source = source;
            _source.Control.MouseDown += (s, e) => Point(e);
            _source.Control.MouseUp += (s, e) => Point();
            _source.Control.QueryContinueDrag += (s, e) => OnSourceQueryContinueDrag(e);
            _source.Control.MouseMove += (s, e) => OnSourceMouseMove(e);
        }

        void Point(MouseEventArgs e) { _sourcePoint = new Point(e.X, e.Y); }
        void Point() { _sourcePoint = null; }

        internal void Add(ITarget target)
        {
            target.Control.AllowDrop = true;
            target.Control.DragOver += OnTargetDragOver;
            target.Control.DragDrop += OnTargetDragDrop;
            _targets = _targets.Concat(new[] {target}).ToArray();
        }

        [DisableDump]
        IItem Item
        {
            get
            {
                if(_sourcePoint == null)
                    return null;
                return _source.GetItemAt(_sourcePoint.Value);
            }
        }

        [DisableDump]
        Size Displacement
        {
            get
            {
                if(_sourcePoint == null)
                    return default(Size);
                return _source.GetDisplacementAt(_sourcePoint.Value);
            }
        }


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
            if((e.Button & MouseButtons.Left) != MouseButtons.Left)
                return;
            if(_sourcePoint == null)
                return;
            if(DragBox.Contains(e.X, e.Y))
                return;

            var dropEffect = _source.Control.DoDragDrop(_objectId, AllowedEffects);

            // If the drag operation was a move then remove the item. 
            if(dropEffect == DragDropEffects.Move)
                NotImplementedMethod(e);
        }

        static void OnSourceQueryContinueDrag(QueryContinueDragEventArgs e)
        {
            if(e.EscapePressed)
                e.Action = DragAction.Cancel;
        }


        TargetPoint FindTargetPoint(object sender, DragEventArgs e)
        {
            if(!e.SetEffect<int>(id => id == _objectId, DefaultEffects))
                return null;

            if((AllowedEffects & e.Effect) == 0)
                return null;

            var target = _targets.Single(t => t.Control == sender);
            return new TargetPoint
            {
                Displacement = target.Control.PointToClient(new Point(e.X, e.Y)),
                Target = target
            };
        }

        void OnTargetDragDrop(object sender, DragEventArgs e)
        {
            var targetPoint = FindTargetPoint(sender, e);
            if(targetPoint == null)
                return;
            Tracer.FlaggedLine(Tracer.Dump(targetPoint.Displacement));
            targetPoint.Target.Drop(Item, targetPoint.Displacement - Displacement);
        }

        void OnTargetDragOver(object sender, DragEventArgs e) { FindTargetPoint(sender, e); }
        static void OnTargetDragEnter(DragEventArgs e) { Tracer.FlaggedLine(e.X + " : " + e.Y); }
        static void OnTargetDragLeave(EventArgs e) { Tracer.FlaggedLine(e.GetType().PrettyName()); }

        sealed class TargetPoint
        {
            public Point Displacement;
            public ITarget Target;
        }

        internal interface IItem
        {}

        internal interface ITarget
        {
            Control Control { get; }
            void Drop(IItem item, Point location);
        }

        internal interface ISource
        {
            Control Control { get; }
            IItem GetItemAt(Point point);
            Size GetDisplacementAt(Point point);
        }
    }
}