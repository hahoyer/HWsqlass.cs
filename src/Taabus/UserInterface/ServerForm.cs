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