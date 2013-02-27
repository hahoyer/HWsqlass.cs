using System.Windows.Forms;

namespace Clock2
{
    sealed partial class Clock
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Clock));
            this._date = new System.Windows.Forms.Label();
            this._time = new System.Windows.Forms.Label();
            this._timer = new System.Windows.Forms.Timer(this.components);
            this._menuAnchor = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // _date
            // 
            this._date.AutoSize = true;
            this._date.Font = new System.Drawing.Font("Arial", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._date.Location = new System.Drawing.Point(0, 0);
            this._date.Name = "_date";
            this._date.Size = new System.Drawing.Size(121, 32);
            this._date.TabIndex = 1;
            this._date.Text = "26.02.13";
            // 
            // _time
            // 
            this._time.AutoSize = true;
            this._time.Font = new System.Drawing.Font("Arial", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._time.Location = new System.Drawing.Point(0, 32);
            this._time.Name = "_time";
            this._time.Size = new System.Drawing.Size(141, 36);
            this._time.TabIndex = 2;
            this._time.Text = "18:01:34";
            // 
            // _timer
            // 
            this._timer.Enabled = true;
            this._timer.Interval = 50;
            this._timer.Tick += new System.EventHandler(this.TimerTick);
            // 
            // _menuAnchor
            // 
            this._menuAnchor.BackColor = System.Drawing.Color.Gold;
            this._menuAnchor.Location = new System.Drawing.Point(125, 1);
            this._menuAnchor.Name = "_menuAnchor";
            this._menuAnchor.Size = new System.Drawing.Size(8, 8);
            this._menuAnchor.TabIndex = 3;
            // 
            // Clock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(134, 69);
            this.Controls.Add(this._menuAnchor);
            this.Controls.Add(this._time);
            this.Controls.Add(this._date);
            this.ForeColor = System.Drawing.Color.Gold;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Clock";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Clock";
            this.TransparencyKey = System.Drawing.Color.Blue;
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.HandleMouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.HandleMouseUp);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label _date;
        private System.Windows.Forms.Label _time;
        private System.Windows.Forms.Timer _timer;
        private Panel _menuAnchor;
    }
}

