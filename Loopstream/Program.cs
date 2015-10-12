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
        public static string DBGLOG;
        public static bool debug = false;
        public static NotifyIcon ni;
        public static string tools;
        public static string[] args;
        public static bool SIGNMODE;
        public static bool DUMBCEPTIONS;
        public static Random rnd;
        //public static System.IO.StreamWriter log;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        [System.Runtime.ExceptionServices.HandleProcessCorruptedStateExceptions]
        static void Main(string[] args)
        {
            //new UI_Msg("poor", "").ShowDialog();
            //Console.WriteLine(LSSettings.version().ToString("x"));
            //Program.kill();

            DBGLOG = "";
            SIGNMODE = false;
            DUMBCEPTIONS = false;

            Program.args = args;
            foreach (string str in args)
            {
                if (str == "sign") SIGNMODE = true;
                if (str == "exceptions") DUMBCEPTIONS = true;
            }

            if (!DUMBCEPTIONS)
            {
                AppDomain.CurrentDomain.UnhandledException += (ueSender, ueArgs) =>
                    new UI_Exception(ueArgs.ExceptionObject as Exception, 1).ShowDialog();

                Application.ThreadException += (ueSender, ueArgs) =>
                    new UI_Exception(ueArgs.Exception, 2).ShowDialog();

                Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            }

            //AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            if (!debug)
            {
                //log = new System.IO.StreamWriter("resolve.log", false, System.Text.Encoding.UTF8);
                //log.AutoFlush = true;
                AppDomain.CurrentDomain.AssemblyResolve += (sender, dargs) =>
                {
                    //log.WriteLine(DateTime.UtcNow.Ticks + "  " + dargs.Name + " // " + dargs.RequestingAssembly);
                    String resourceName = "Loopstream.lib." +
                        //new AssemblyName(dargs.Name).Name + ".dll";
                        dargs.Name.Substring(0, dargs.Name.IndexOf(", ")) + ".dll";
                    
                    using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
                    {
                        if (stream == null) return null;
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

            System.Diagnostics.Process.GetCurrentProcess().PriorityClass = System.Diagnostics.ProcessPriorityClass.AboveNormal;
            
            Logger.init();
            Skinner.init();
            rnd = new Random();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //throw new Exception("asdf");
            //Application.Run(new UI_Winlist());
            Application.Run(new Home());
        }

        public static System.Drawing.Icon icon;

        [Obsolete()]
        static System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            string ns = "Loopstream";
            // Project -> Loopstream Properties -> Resources -> Add existing file -> *.dll
            // No further actions required. Also next line is workaround for win7 custom themes
            if (args.Name.Contains("PresentationFramework")) return null;
            string dllname = args.Name.Contains(',')
                ? args.Name.Substring(0, args.Name.IndexOf(','))
                : args.Name.Replace(".dll", "");

            dllname = dllname.Replace(".", "_");
            if (dllname.EndsWith("_resources"))return null;
            System.Resources.ResourceManager rm = new System.Resources.ResourceManager(ns + ".Properties.Resources",
                                                                                       System.Reflection.Assembly.GetExecutingAssembly());
            byte[] bytes = (byte[])rm.GetObject(dllname);
            return System.Reflection.Assembly.Load(bytes);
        }

        public static void kill()
        {
            if (ni != null) ni.Dispose();
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
