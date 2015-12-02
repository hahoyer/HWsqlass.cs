using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using hw.Forms;

namespace Clock2
{
    abstract class Form : System.Windows.Forms.Form
    {
        PositionConfig PositionConfig;

        protected Form()
        {
            SuspendLayout();
            AutoScaleMode = AutoScaleMode.Font;
            PositionConfig = new PositionConfig(GetFileName)
            {
                Target = this
            };
            ResumeLayout(true);
        }

        string GetFileName()
        {
            return
                Environment.GetEnvironmentVariable("APPDATA")
                    + "\\HoyerWare.Clock2." + ConfigFileName + ".config";
        }

        protected abstract string ConfigFileName { get; }
    }
}