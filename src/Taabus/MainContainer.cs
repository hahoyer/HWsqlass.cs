using System;
using System.Collections.Generic;
using System.Diagnostics;
using hw.UnitTest;
using System.Linq;
using System.Reflection;

namespace Taabus
{
    static class MainContainer
    {
        [STAThread]
        public static void Main()
        {
            new Test().ForceFeedbackUserInterface();
            return;
            if(Debugger.IsAttached)
                TestRunner.IsModeErrorFocus = true;
            Assembly.GetExecutingAssembly().RunTests();
        }
    }
}