using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Taabus.Data;

namespace Taabus.UserInterface
{
    public sealed class ServerForm : Form
    {
        private Panel panel1;
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

        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Location = new System.Drawing.Point(36, 47);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 100);
            this.panel1.TabIndex = 0;
            // 
            // ServerForm
            // 
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.panel1);
            this.Name = "ServerForm";
            this.ResumeLayout(false);

        }
    }
}