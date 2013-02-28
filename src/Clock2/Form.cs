#region Copyright (C) 2013

//     Project Clock2
//     Copyright (C) 2013 - 2013 Harald Hoyer
// 
//     This program is free software: you can redistribute it and/or modify
//     it under the terms of the GNU General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     This program is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU General Public License for more details.
// 
//     You should have received a copy of the GNU General Public License
//     along with this program.  If not, see <http://www.gnu.org/licenses/>.
//     
//     Comments, bugs and suggestions to hahoyer at yahoo.de

#endregion

using System.Drawing;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Clock2
{
    class Form : System.Windows.Forms.Form
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
            var rectangle = LoadWindowLocation(File);
            if(rectangle != null)
            {
                StartPosition = FormStartPosition.Manual;
                Bounds = rectangle.Value;
            }

            ResumeLayout(true);
            _initialLocationSet = true;
        }

        protected virtual string ConfigFileName { get { return "Form"; } }

        protected override void OnLocationChanged(EventArgs e)
        {
            if(_initialLocationSet)
                SaveWindowLocation(Bounds, File);
            base.OnLocationChanged(e);
        }

        File File
        {
            get
            {
                var fileName =
                    Environment.GetEnvironmentVariable("APPDATA")
                        + "\\HoyerWare.Clock2." + ConfigFileName + ".config";
                return File.Create(fileName);
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            if(_initialLocationSet)
                SaveWindowLocation(Bounds, File);
            base.OnSizeChanged(e);
        }

        static void SaveWindowLocation(Rectangle rectangle, File file) { file.String = new RectangleConverter().ConvertToString(rectangle); }

        static Rectangle? LoadWindowLocation(File file)
        {
            var context = file.String;
            if(context == null)
                return null;
            return (Rectangle) new RectangleConverter().ConvertFromString(context);
        }
    }
}