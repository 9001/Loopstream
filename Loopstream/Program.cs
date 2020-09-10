using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Loopstream
{
    static class Program
    {
        public const int beta = 1;
        public const bool debug = false;
        public const string toolsVer = "ls.tools.v1.txt";
        
        public static string[] args;
        public static bool VERIFY_CHECKSUM;
        public static bool CRASH_REPORTER;
        public static bool SIGN_BINARY;
        public static bool BALLOONS;
        public static bool ASK_DFC;
        public static string DBGLOG;
        public static string tools;
        public static System.Drawing.Icon icon;
        public static NotifyIcon ni;
        public static Random rnd;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        [System.Runtime.ExceptionServices.HandleProcessCorruptedStateExceptions]
        static void Main(string[] args)
        {
            DBGLOG = "";
            SIGN_BINARY = false;
            CRASH_REPORTER = true;
            VERIFY_CHECKSUM = true;
            BALLOONS = true;
            ASK_DFC = true;

            Program.args = args;
            foreach (string str in args)
            {
                if (str == "sign")
                    SIGN_BINARY = true;

                if (str == "exceptions")
                    CRASH_REPORTER = false;

                if (str == "unsigned")
                    VERIFY_CHECKSUM = false;

                if (str == "no_dfc")
                    ASK_DFC = false;
            }

            if (CRASH_REPORTER)
            {
                AppDomain.CurrentDomain.UnhandledException += (ueSender, ueArgs) =>
                    new UI_Exception(ueArgs.ExceptionObject as Exception, 1);

                Application.ThreadException += (ueSender, ueArgs) =>
                    new UI_Exception(ueArgs.Exception, 2);

                Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            }

            if (!debug)
            {
                AppDomain.CurrentDomain.AssemblyResolve += (sender, dargs) =>
                {
                    // workaround for win7 custom themes
                    if (dargs.Name.Contains("PresentationFramework")) return null;
                    
                    // vs2019
                    if (dargs.Name.StartsWith("Loopstream.XmlSerializers")) return null;

                    String resourceName = "Loopstream.lib." +
                        dargs.Name.Substring(0, dargs.Name.IndexOf(", ")) + ".dll";
                    
                    using (var stream = Assembly.GetExecutingAssembly().
                                GetManifestResourceStream(resourceName))
                    {
                        if (stream == null)
                            return null;

                        Byte[] assemblyData = new Byte[stream.Length];
                        stream.Read(assemblyData, 0, assemblyData.Length);
                        return Assembly.Load(assemblyData);
                    }
                };
            }

            if (!debug)
            {
                IconExtractor ie = new IconExtractor(Application.ExecutablePath);
                icon = ie.GetIcon(0);
                ie.Dispose();
            }
            else icon = new System.Drawing.Icon(@"..\..\res\loopstream.ico");

            tools = System.Windows.Forms.Application.ExecutablePath;
            tools = tools.Substring(tools.Replace('\\', '/').LastIndexOf('/') + 1);
            tools = tools.Split('.')[0];
            tools += "Tools\\";

            System.Diagnostics.Process.GetCurrentProcess().PriorityClass =
                System.Diagnostics.ProcessPriorityClass.AboveNormal;

            Logger.init();
            Skinner.init();
            rnd = new Random();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Home());
        }

        public static void kill()
        {
            if (ni != null)
                ni.Dispose();

            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        public static void fixWorkingDirectory()
        {
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            string path = Application.ExecutablePath;
            int i = path.LastIndexOf('\\');
            proc.StartInfo.FileName = path.Substring(i + 1);
            proc.StartInfo.WorkingDirectory = path.Substring(0, i);
            proc.StartInfo.Arguments = "wdfix";
            proc.Start();

            while (true)
            {
                try
                {
                    proc.Refresh();
                    if (proc.Modules.Count > 1) break;
                    System.Threading.Thread.Sleep(10);
                }
                catch { }
            }
            Application.DoEvents();
            System.Threading.Thread.Sleep(1000);
            kill();
        }

        public static void popception(int lv = 1)
        {
            bool cu = false;
            try
            {
                if (lv == 6)
                {
                    throw new Exception("hit lv");
                }
                popception(lv + 1);
            }
            finally
            {
                cu = true;
            }
        }
    }
}
