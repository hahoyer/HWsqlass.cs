using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using hw.Debug;
using hw.Forms;
using MetroFramework.Forms;

namespace Taabus.UserInterface
{
    abstract class MainView : DumpableObject
    {
        const int FrameSize = 10;
        const int AppBarSize = 25;

        readonly ToolStrip _toolbar = new ToolStrip();
        readonly ITaabusController _controller;
        readonly MetroForm _frame = new MetroForm();
        Control _client;
        UserInteraction[] _functions = new UserInteraction[0];

        protected MainView(string tag, ITaabusController controller)
        {
            _controller = controller;
            _toolbar.Stretch = true;
            _toolbar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            _frame.SuspendLayout();
            _frame.Name = "Taabus." + tag;
            _frame.ShadowType = MetroForm.MetroFormShadowType.DropShadow;
            _frame.Controls.Add(_toolbar);
            _frame.InstallPositionConfig();
            _frame.Closed += (s, e) => OnClosed();
            _frame.Load += (s, e) => OnLoad();
            _frame.ResumeLayout();
        }

        protected virtual void OnLoad() {  }

        internal void AddFunction(params UserInteraction[] functions)
        {
            _functions = _functions.Concat(functions).ToArray();
            _frame.SuspendLayout();
            _toolbar.Items.AddRange(functions.Select(f => f.Button).ToArray());
            RefreshLayout();
            _frame.ResumeLayout();
        }

        void RefreshLayout()
        {
            var cs = _frame.ClientSize;
            var ts = _toolbar.Size;
            _toolbar.Location = new Point(cs.Width - ts.Width - FrameSize, AppBarSize);

            if(_client == null)
                return;
            
            var tl = _toolbar.Location;
            var location = new Point(FrameSize, tl.Y + ts.Height);
            var size = new Size(cs.Width - 2 * FrameSize, cs.Height - FrameSize - tl.Y - ts.Height);
            _client.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            _client.Location = location;
            _client.Size = size;
        }

        internal IEnumerable<UserInteraction> Functions { get { return _functions; } }

        internal string Name { get { return _frame.Name; } }
        internal string Text { get { return _frame.Text; } set { _frame.Text = value; } }

        internal Control Client
        {
            get { return _client; }
            set
            {
                if(_client == value)
                    return;

                _frame.SuspendLayout();

                if(_client != null)
                    _frame.Controls.Remove(_client);

                _client = value;

                if(_client != null)
                {
                    RefreshLayout();
                    _frame.Controls.Add(_client);
                }

                _frame.ResumeLayout();
            }
        }

        internal void Show() { _frame.Show(); }

        protected virtual void OnClosed() { _controller.Exit(); }
    }
}