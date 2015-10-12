using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace LoopstreamTraktor
{
    static class Program
    {
        public static string tools;
        public static string[] args;
        public static bool SIGNMODE;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            SIGNMODE = false;
            Program.args = args;
            if (args.Length > 0)
            {
                if (args[0] == "sign")
                {
                    SIGNMODE = true;
                }
            }

            tools = System.Windows.Forms.Application.ExecutablePath;
            tools = tools.Substring(tools.Replace('\\', '/').LastIndexOf('/') + 1);
            tools = tools.Split('.')[0];
            tools = tools.Substring(0, tools.Length - 7).TrimEnd('-');
            tools += "Tools\\";

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        public static void kill()
        {
            //if (ni != null) ni.Dispose();
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
    }
}
