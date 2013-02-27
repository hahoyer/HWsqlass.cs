using System.Drawing;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Windows.Forms;

namespace Clock2
{
    abstract class Form : System.Windows.Forms.Form
    {
        bool _initialLocationSet;

        protected Form()
        {
            SuspendLayout();
            AutoScaleMode = AutoScaleMode.Font;
            ResumeLayout(true);
        }

        protected override void OnLoad(EventArgs e)
        {
            SuspendLayout();
            var rectangle = LoadWindowLocation(ConfigFileName);
            if(rectangle != null)
            {
                StartPosition = FormStartPosition.Manual;
                Bounds = rectangle.Value;
            }

            ResumeLayout(true);
            _initialLocationSet = true;
        }

        protected abstract string ConfigFileName { get; }

        protected override void OnLocationChanged(EventArgs e)
        {
            if(_initialLocationSet)
                SaveWindowLocation(ConfigFileName, Bounds);
            base.OnLocationChanged(e);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            if (_initialLocationSet)
                SaveWindowLocation(ConfigFileName, Bounds);
            base.OnSizeChanged(e);
        }

        static void SaveWindowLocation(string name, Rectangle rectangle) { File.Create(name).String = new RectangleConverter().ConvertToString(rectangle); }

        static Rectangle? LoadWindowLocation(string name)
        {
            var context = File.Create(name).String;
            if(context == null)
                return null;
            return (Rectangle) new RectangleConverter().ConvertFromString(context);
        }
    }
}