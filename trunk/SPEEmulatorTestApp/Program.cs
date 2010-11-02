using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SPEEmulatorTestApp
{
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Simulator(args));
        }
    }
}
