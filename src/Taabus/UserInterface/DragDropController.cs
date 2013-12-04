using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using hw.Debug;
using hw.Helper;

namespace Taabus.UserInterface
{
    sealed class DragDropController : DumpableObject
    {
        readonly IDragDropSource _source;
        readonly IDragDropTarget[] _targets;

        Point? _sourcePoint;

        public DragDropController(IDragDropSource source, params IDragDropTarget[] targets)
        {
            _source = source;
            _targets = targets;

            _source.Control.MouseDown += (s, e) => { _sourcePoint = new Point(e.X, e.Y); };
            _source.Control.MouseUp += (s, e) => { _sourcePoint = null; };
            _source.Control.QueryContinueDrag += (s, e) => OnSourceQueryContinueDrag(e);
            _source.Control.MouseMove += (s, e) => OnSourceMouseMove(e);
            //_source.Control.GiveFeedback += (s, e) => OnSourceGiveFeedback(e);

            foreach(var target in _targets)
            {
                target.Control.AllowDrop = true;
                target.Control.DragOver += OnTargetDragOver;
                target.Control.DragDrop += OnTargetDragDrop;
                //target .Control.DragEnter += (s, e) => OnTargetDragEnter(e);
                //target .Control.DragLeave += (s, e) => OnTargetDragLeave(e);
                
            }
        }

        IDragDropItem Item { get { return _sourcePoint == null ? null : _source.GetItemAt(_sourcePoint.Value); } }

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

        void OnSourceMouseMove(MouseEventArgs e)
        {
            if((e.Button & MouseButtons.Left) != MouseButtons.Left)
                return;
            if(_sourcePoint == null)
                return;
            if(DragBox.Contains(e.X, e.Y))
                return;

            var dropEffect = _source.Control.DoDragDrop(GetType().Name, DragDropEffects.Copy);

            // If the drag operation was a move then remove the item. 
            if(dropEffect == DragDropEffects.Move)
                NotImplementedMethod(e);
        }


        static void OnSourceQueryContinueDrag(QueryContinueDragEventArgs e)
        {
            if(e.EscapePressed)
                e.Action = DragAction.Cancel;
        }


        void OnTargetDragOver(object sender, DragEventArgs e)
        {
            if(!e.SetEffect(e.Data.GetDataPresent(typeof(string)), DragDropEffects.Copy))
                return;

            var targetPoint = _targets.Single(t=>t.Control == sender).Control.PointToClient(new Point(e.X, e.Y));
            Tracer.FlaggedLine(Tracer.Dump(targetPoint));
        }

        void OnTargetDragDrop(object sender, DragEventArgs e)
        {
            if(!e.Data.GetDataPresent(typeof(string)))
                return;
            if((string)e.Data.GetData(typeof(string)) != GetType().Name)
                return;
            if(e.Effect != DragDropEffects.Copy && e.Effect != DragDropEffects.Move)
                return;

            _targets.Single(t => t.Control == sender).Drop(Item);
        }

        static void OnSourceGiveFeedback(GiveFeedbackEventArgs e) { Tracer.FlaggedLine(e.Effect + " " + e.UseDefaultCursors); }
        static void OnTargetDragEnter(DragEventArgs e) { Tracer.FlaggedLine(e.X + " : " + e.Y); }
        static void OnTargetDragLeave(EventArgs e) { Tracer.FlaggedLine(e.GetType().PrettyName()); }
    }

    interface IDragDropItem
    {}

    interface IDragDropTarget
    {
        Control Control { get; }
        void Drop(IDragDropItem item);
    }

    interface IDragDropSource
    {
        Control Control { get; }
        IDragDropItem GetItemAt(Point point);
    }
}