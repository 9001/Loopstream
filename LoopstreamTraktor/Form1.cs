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

            if (!Directory.Exists(Program.tools + "ice"))
            {
                splash.vis();
                new DFC().extract(splash.pb);

                DialogResult dr = MessageBox.Show(
                    "Would you like me to perform the necessary\n" +
                    "changes to the Traktor configuration files?",
                    "Automatic Install Prompt",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (dr == System.Windows.Forms.DialogResult.Yes)
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
                        Program.kill();
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
                            Program.kill();
                        }
                    }
                    MessageBox.Show("Traktor configuration successful.");
                }
            }
            splash.unvis();
            splash.fx = false;
            splash.gtfo();

            System.Diagnostics.Process proc;
            proc = new System.Diagnostics.Process();
            proc.StartInfo = new System.Diagnostics.ProcessStartInfo(
                "traktor2loopstream.exe",
                "-c icecast.xml");
            proc.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            proc.StartInfo.WorkingDirectory = Program.tools + "ice";
            proc.Start();
            while (true)
            {
                try
                {
                    string str = new System.Net.WebClient().DownloadString("http://127.0.0.1:42069/status2.xsl");
                    if (str.Contains("<pre>"))
                    {
                        break;
                    }
                }
                catch { }
                Application.DoEvents();
                System.Threading.Thread.Sleep(500);
            }
            Application.DoEvents();
            Program.kill();
        }
    }
}
