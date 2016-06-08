using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using hw.Helper;
using log4net;
using log4net.Config;

namespace Clock2
{
    sealed partial class Clock : Form
    {
        static readonly ILog Logger = LogManager.GetLogger(typeof(Clock));

        public Clock()
        {
            XmlConfigurator.Configure(new FileInfo("log4net.xml"));
            InitializeComponent();
            InitializeChildren(_date, _time);
        }

        void InitializeChildren(params Control[] controls)
        {
            foreach(var label in controls)
            {
                label.MouseDown += HandleMouseDown;
                label.MouseMove += HandleMouseMove;
                label.MouseUp += HandleMouseUp;
            }
        }

        void TimerTick(object sender, EventArgs e)
        {
            UpdateUserInterface();
            LogBounds();
            LogScreenLayout();
        }

        void LogScreenLayout()
        {
            var text = GetScreenLayout();
            if(LastScreenLayoutText == text)
                return;

            LastScreenLayoutText = text;
            Logger.Info(text);
        }

        void LogBounds()
        {
            var text = Format(Bounds);
            if (LastBoundsText == text)
                return;

            LastBoundsText = text;
            Logger.Info(text);
        }

        static string GetScreenLayout()
            => Screen
                .AllScreens
                .Select(Format)
                .Stringify("; ");

        static string Format(Screen value)
            => (value.Primary ? "*" : "") + value.DeviceName + "(" + Format(value.Bounds) + ")";

        static string Format(Rectangle value)
            => Format(value.Left, value.Right) + "/" + Format(value.Top, value.Bottom);

        static string Format(int low, int high) => low + "[" + (high - low) + "]" + high;

        void UpdateUserInterface()
        {
            var dateTime = DateTime.Now;
            var date = dateTime.ToString("ddd dd.MM");
            _date.Text = date;
            var time = dateTime.ToString("HH:mm:ss");
            _time.Text = time;
            Text = date + " " + time;
            TopMost = true;
        }

        Size? _clickedPoint;
        string LastScreenLayoutText;
        string LastBoundsText;

        void HandleMouseUp(object sender, MouseEventArgs e) { _clickedPoint = null; }

        void HandleMouseDown(object sender, MouseEventArgs e)
        {
            _clickedPoint = new Size(e.X, e.Y);
        }

        void HandleMouseMove(object sender, MouseEventArgs e)
        {
            if(_clickedPoint != null)
                Location = PointToScreen(e.Location) - _clickedPoint.Value;
        }

        void MenuAnchorMouseClick(object sender, MouseEventArgs e)
            => _mainMenu.Show(PointToScreen(e.Location + new Size(_menuAnchor.Location)));

        void CloseClick(object sender, EventArgs e) => Close();
        protected override string ConfigFileName => "Clock";
    }
}