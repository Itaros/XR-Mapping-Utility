using CommonUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XRMappingUtilityWin
{
    
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {

            var platform = new Eto.Wpf.Platform();
            var application = new Eto.Forms.Application(platform);
            var form = new MainForm();
            application.Run(form);

        }
    }
}
