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
        public static bool SIGN_BINARY;
        public static bool VERIFY_CHECKSUM;
        public static bool DO_IT;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            SIGN_BINARY = false;
            VERIFY_CHECKSUM = true;
            Program.args = args;
            foreach (string arg in args)
            {
                if (arg == "doit")
                    DO_IT = true;

                if (arg == "sign")
                    SIGN_BINARY = true;

                if (arg == "unsigned")
                    VERIFY_CHECKSUM = false;
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
