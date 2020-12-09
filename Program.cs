using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace LambdaText
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (args.Length > 0)
            {
                Application.Run(new LambdaText(args[0]));
            }
            else
            {
                Application.Run(new LambdaText(String.Empty));
            }
        }

    }
}
