using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace LoopstreamTraktor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Splesh splash;
        Rectangle myBounds;
        private void Form1_Load(object sender, EventArgs e)
        {
            /*System.Collections.IDictionary dict = System.Environment.GetEnvironmentVariables();
            foreach (System.Collections.DictionaryEntry env in dict)
            {
                MessageBox.Show(env.Key + ": " + env.Value);
            }*/

            myBounds = this.Bounds;
            this.Bounds = new Rectangle(0, -100, 0, 0);
            splash = new Splesh();
            splash.Show();

            Timer t = new Timer();
            t.Tick += t_Tick;
            t.Interval = 100;
            t.Start();
        }

        string setvalue(string row, string val)
        {
            string s = "\" Value=\"";
            int i = row.IndexOf(s);
            int j = row.IndexOf('"', i + s.Length);
            return row.Substring(0, i + s.Length) + val + row.Substring(j);
        }

        void configure_traktor()
        {
            string path = System.Environment.GetEnvironmentVariable("USERPROFILE") + "\\";
            string try1 = path + "Documents\\Native Instruments";
            string try2 = path + "Native Instruments";
            //MessageBox.Show(path);
            if (Directory.Exists(try1)) path = try1;
            else if (Directory.Exists(try2)) path = try2;
            else
            {
                MessageBox.Show("Could not find the location of Traktor on your system.");
                return;
            }
            bool messaged = false;
            foreach (string str in Directory.GetDirectories(path))
            {
                string fpath = str + "\\Traktor Settings.tsi";
                if (!File.Exists(fpath)) continue;
                if (!messaged)
                {
                    MessageBox.Show("About to modify the following config file:\n\n" + fpath + "\n\nPlease make sure that Traktor is completely\nshut down before proceeding.");
                }
                var enc = new System.Text.UTF8Encoding(false);
                string[] data = System.IO.File.ReadAllText(fpath, enc).Split('\n');

                int matches = 0;
                for (int a = 0; a < data.Length; a++)
                {
                    string s = data[a] + 's';
                    if (s.StartsWith("<Entry Name=\"Broadcast.IcecastServer.Address\"")) s = setvalue(data[a], "127.0.0.1");
                    if (s.StartsWith("<Entry Name=\"Broadcast.IcecastServer.MountPath\"")) s = setvalue(data[a], "/why.ogg");
                    if (s.StartsWith("<Entry Name=\"Broadcast.IcecastServer.Password\"")) s = setvalue(data[a], "loopstream");
                    if (s.StartsWith("<Entry Name=\"Broadcast.IcecastServer.Port\"")) s = setvalue(data[a], "42069");
                    if (s != data[a] + 's')
                    {
                        matches++;
                        data[a] = s;
                    }
                }
                if (matches == 4)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (string ss in data)
                    {
                        sb.Append(ss);
                        sb.Append('\n');
                    }
                    System.IO.File.WriteAllText(fpath, sb.ToString(), enc);
                }
                else
                {
                    MessageBox.Show("Failed to configure Traktor:\n\nUnable to parse config file");
                    return;
                }
            }
            MessageBox.Show("Traktor configuration successful.");
        }

        void configure_virtualdj()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            path = Path.Combine(path, "VirtualDJ", "settings.xml");
            if (!File.Exists(path))
            {
                MessageBox.Show("Could not find the location of Virtualdj settings.xml on your system.");
                return;
            }
            MessageBox.Show("About to modify the following config file:\n\n" + path + "\n\nPlease make sure that Virtualdj is completely\nshut down before proceeding.");
            
            string str = File.ReadAllText(path, Encoding.UTF8);
            string f1 = "<broadcastServers />";
            string f2 = "</broadcastServers>";
            string v1 = "<server name=\"Loopstream\" format=\"1\" bitrate=\"192\" url=\"127.0.0.1\" port=\"42069\" mount=\"utf.mp3\" login=\"source\" password=\"loopstream\" />";

            if (str.Contains("password=\"loopstream\""))
            {
                MessageBox.Show("Virtualdj configuration appears to be correct already; will not change anything");
                return;
            }
            else if (str.Contains(f1))
            {
                str = str.Replace(f1, "<broadcastServers>" + v1 + f2);
            }
            else if (str.Contains(f2))
            {
                str = str.Replace(f2, v1 + f2);
            }
            else
            {
                MessageBox.Show("Failed to configure Virtualdj:\n\nUnable to parse config file");
                return;
            }
            System.IO.File.WriteAllText(path, str, Encoding.UTF8);
            MessageBox.Show("Virtualdj configuration successful.");
        }

        void t_Tick(object sender, EventArgs e)
        {
            ((Timer)sender).Stop();
            this.Text += " v" + Application.ProductVersion;

            DFC.coreTest();
            if (Directory.Exists(@"..\..\tools\"))
            {
                if (DialogResult.Yes == MessageBox.Show(
                    "make .dfc (decent file container) ?\r\n\r\nhint: rename the tools folder\r\n         if you don't wanna see this",
                    "new embedded archive",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question))
                {
                    splash.vis();
                    new DFC().make(splash.pb);
                    Program.kill();
                }
            }
            if (!Program.DO_IT)
            {
                MessageBox.Show("This program should be run by Loopstream!\n\nStop doing that.");
                Program.kill();
            }
            if (!Directory.Exists(Program.tools))
            {
                //MessageBox.Show(Program.tools);
                MessageBox.Show("Could not find LoopstreamTools directory.\n\nWhat the fuck.");
                Program.kill();
            }
            System.Diagnostics.Process[] procs = System.Diagnostics.Process.GetProcessesByName("traktor2loopstream");
            if (procs.Length > 0) Program.kill();

            string basedir = Program.tools + "ice";
            if (Directory.Exists(basedir))
            {
                bool ok = false;
                try
                {
                    var txt = File.ReadAllText(Path.Combine(basedir, "web", "statuls.xsl"), Encoding.UTF8);
                    ok = txt.Contains("artist\" /> - </xsl:if><xsl");
                }
                catch { }

                if (!ok)
                    for (int a = 0; a < 3; a++)
                        try
                        {
                            Directory.Delete(basedir, true);
                            Application.DoEvents();
                            System.Threading.Thread.Sleep(500); // mkdir fails otherwise, ok yes good
                            break;
                        }
                        catch
                        {
                            Application.DoEvents();
                            System.Threading.Thread.Sleep(250);
                        }
            }

            if (!Directory.Exists(basedir))
            {
                splash.vis();
                new DFC().extract(splash.pb);

                if (DialogResult.Yes == MessageBox.Show(
                    "Would you like me to perform the necessary\n" +
                    "changes to the Traktor configuration files?",
                    "Automatic Install Prompt",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question))
                    configure_traktor();

                if (DialogResult.Yes == MessageBox.Show(
                    "Would you like me to perform the necessary\n" +
                    "changes to the Virtualdj configuration files?",
                    "Automatic Install Prompt",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question))
                    configure_virtualdj();
            }
            splash.unvis();
            splash.fx = false;
            splash.gtfo();

            System.Diagnostics.Process proc;
            proc = new System.Diagnostics.Process();
            proc.StartInfo = new System.Diagnostics.ProcessStartInfo(
                "bin\\traktor2loopstream.exe",
                "-c icecast.xml");
            proc.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            proc.StartInfo.WorkingDirectory = Program.tools + "ice";
            proc.Start();
            for (int a = 0; a < 20; a++)
            {
                try
                {
                    string str = new System.Net.WebClient().DownloadString("http://127.0.0.1:42069/statuls.xsl");
                    if (str.Contains("<pre>"))
                        break;
                }
                catch { }
                if (a == 19)
                {
                    MessageBox.Show("the Traktor plugin failed to start ;_;");
                    break;
                }
                Application.DoEvents();
                System.Threading.Thread.Sleep(500);
            }
            Application.DoEvents();
            Program.kill();
        }
    }
}
