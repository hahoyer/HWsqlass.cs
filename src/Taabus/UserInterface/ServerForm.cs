#region Copyright (C) 2013

//     Project Taabus
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

namespace Taabus.UserInterface

{
    public sealed class ServerForm : Form
    {
        readonly TreeView _treeView;

        public ServerForm(DataBase[] data)
        {
            SuspendLayout();
            _treeView = new TreeView
            {
                Dock = DockStyle.Fill,
                Font = new Font("Microsoft Sans Serif", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0),
                ImageIndex = 0, 
                ImageList = Images.Instance,
                Location = new Point(0, 0),
                Name = "treeView1",
                SelectedImageIndex = 0,
                Size = new Size(554, 565),
                TabIndex = 0
            };
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(554, 565);
            Controls.Add(_treeView);
            Name = "TreeForm";
            Text = "Main";
            ResumeLayout(false);
        }
    }
}