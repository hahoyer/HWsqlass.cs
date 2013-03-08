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

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Clock2
{
    sealed partial class Clock : Form
    {
        public Clock()
        {
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
            var dateTime = DateTime.Now;
            var date = dateTime.ToString("ddd dd.MM");
            _date.Text = date;
            var time = dateTime.ToString("HH:mm:ss");
            _time.Text = time;
            Text = date + " " + time;
            TopMost = true;
        }

        Size? _clickedPoint;

        void HandleMouseUp(object sender, MouseEventArgs e) { _clickedPoint = null; }
        void HandleMouseDown(object sender, MouseEventArgs e) { _clickedPoint = new Size(e.X, e.Y); }

        void HandleMouseMove(object sender, MouseEventArgs e)
        {
            if(_clickedPoint != null)
                Location = PointToScreen(e.Location) - _clickedPoint.Value;
        }
        protected override string ConfigFileName { get { return "Clock"; } }

        void MenuAnchorMouseClick(object sender, MouseEventArgs e) { _mainMenu.Show(PointToScreen(e.Location + new Size(_menuAnchor.Location))); }

        void CloseClick(object sender, EventArgs e) { Close(); }
    }
}