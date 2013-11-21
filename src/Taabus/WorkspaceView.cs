using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using hw.Debug;
using hw.Forms;
using MetroFramework.Forms;
using Taabus.Properties;
using Taabus.UserInterface;

namespace Taabus
{
    class MainView : DumpableObject
    {}

    sealed class WorkspaceView : MainView
    {
        const int FrameSize = 10;
        const int AppBarSize = 25;

        readonly MetroForm _frame = new MetroForm();
        readonly ToolStrip _toolbar = new ToolStrip();
        readonly Panel _panel = new Panel();
        readonly UserInteraction[] _functions;
        readonly Action _closeHandler;

        WorkspaceView(string tag, Action closeHandler)
        {
            _closeHandler = closeHandler;
            _functions = new[]
            {
                new UserInteraction("Settings", OnConfiguration, Resources.appbar_settings)
            };

            _toolbar.Items.AddRange(_functions.Select(f => f.Button).ToArray());
            _toolbar.Stretch = true;

            var cs = _frame.ClientSize;
            var ts = _toolbar.Size;
            _toolbar.Location = new Point(cs.Width - ts.Width - FrameSize, AppBarSize);
            var tl = _toolbar.Location;
            _panel.Location = new Point(FrameSize, tl.Y + ts.Height);
            _panel.Size = new Size(cs.Width - 2 * FrameSize, cs.Height - FrameSize - tl.Y - ts.Height);

            _toolbar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            _panel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            _frame.SuspendLayout();
            _frame.Name = "Taabus.Workspace." + tag;
            _frame.ShadowType = MetroForm.MetroFormShadowType.DropShadow;
            _frame.Controls.Add(_toolbar);
            _frame.Controls.Add(_panel);
            _frame.InstallPositionConfig();
            _frame.Closed += (s, e) => OnClosed();
            _frame.ResumeLayout();
        }

        void OnClosed() { _closeHandler(); }

        static internal void Run(string tag, Action closeHandler)
        {
            var form = new WorkspaceView(tag, closeHandler);
            Application.Run(form._frame);
        }

        static void OnConfiguration() { }
    }
}