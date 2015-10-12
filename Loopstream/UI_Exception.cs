using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Loopstream
{
    public partial class UI_Exception : Form
    {
        public UI_Exception(Exception ex, int type)
        {
            InitializeComponent();
            this.type = type;
            this.ex = ex;
        }

        int type;
        Exception ex;
        GeneralInfo gi;

        // 1 = AppDomain.CurrentDomain.UnhandledException
        // 2 = Application.ThreadException

        private void UI_Exception_Load(object sender, EventArgs e)
        {
            this.Icon = Program.icon;
            label1.Font = new Font(label1.Font.FontFamily, label1.Font.SizeInPoints * 2.18f);
            Timer t = new Timer();
            t.Interval = 100;
            t.Start();
            t.Tick += new EventHandler(t_Tick);
            gExit.Enabled = false;
            gSend.Enabled = false;
            gDesc.Enabled = false;
        }

        void t_Tick(object sender, EventArgs e)
        {
            ((Timer)sender).Stop();
            try
            {
                var se = new SerializableException(ex);
                gi = new GeneralInfo(se, ex);

                LSSettings s = LSSettings.singleton;
                try
                {
                    if (s == null)
                    {
                        s = new LSSettings();
                        s.host = "nullsettings";
                    }
                    s.serverPresets = new List<LSSettings.LSServerPreset>();
                    s.pass = "(redacted)";
                    GeneralInfo.ser(s);
                }
                catch { }
                try
                {
                    if (s.devs == null) s.devs = new LSDevice[0];
                    GeneralInfo.ser(s.devs);
                }
                catch
                {
                    try { s.devs = new LSDevice[0]; }
                    catch { }
                }

                StringBuilder sb = new StringBuilder();
                sb.AppendLine();
                bool ok1, ok2, ok3, oks;
                ok1 = ok2 = ok3 = oks = false;
                try
                {
                    ok1 = s.devRec != null && s.devRec.mm != null;
                    ok2 = s.devMic != null && s.devMic.mm != null;
                    ok3 = s.devOut != null && s.devOut.mm != null;
                    sb.AppendLine("dev state indicated: " + ok1 + ", " + ok2 + ", " + ok3);
                    s.runTests(null, true);
                    ok1 = ok1 == (s.devRec != null && s.devRec.mm != null);
                    ok2 = ok2 == (s.devMic != null && s.devMic.mm != null);
                    ok3 = ok3 == (s.devOut != null && s.devOut.mm != null);
                    sb.AppendLine("dev state correct: " + ok1 + ", " + ok2 + ", " + ok3);
                    oks = true;
                }
                catch { }
                try
                {
                    sb.AppendLine("\n\n\n\n\nCache for ogg"); sb.AppendLine(Logger.ogg.compile());
                    sb.AppendLine("\n\n\n\n\nCache for mp3"); sb.AppendLine(Logger.mp3.compile());
                    sb.AppendLine("\n\n\n\n\nCache for pcm"); sb.AppendLine(Logger.pcm.compile());
                    sb.AppendLine("\n\n\n\n\nCache for med"); sb.AppendLine(Logger.med.compile());
                    sb.AppendLine("\n\n\n\n\nCache for mix"); sb.AppendLine(Logger.mix.compile());
                    sb.AppendLine("\n\n\n\n\nCache for tag"); sb.AppendLine(Logger.tag.compile());
                    sb.AppendLine("\n\n\n\n\nCache for app"); sb.AppendLine(Logger.app.compile());
                }
                catch { }
                gi.moar = sb.ToString();
                string serialized = gi.ToString();

                gDesc.Enabled = true;
                gDesc.Text = "You can add a description here, if you'd like";
                gExit.Enabled = true;
                gSend.Enabled = true;
                gSend.Focus();

                /*string one = Z.gze(serialized);
                string two = Z.lze(serialized);
                gDesc.Text =
                    one.Length + "\n" +
                    two.Length + "\n\n" +
                    Convert.FromBase64String(one).Length + "\n" +
                    Convert.FromBase64String(two).Length + "\n\n" +
                    serialized;

                System.IO.File.WriteAllBytes("dump.txt", Encoding.UTF8.GetBytes(serialized));
                System.IO.File.WriteAllBytes("dump.gz", Convert.FromBase64String(one));
                System.IO.File.WriteAllBytes("dump.lzma", Convert.FromBase64String(two));*/

                if (oks && (!ok1 || !ok2 || !ok3))
                {
                    if (DialogResult.Yes == MessageBox.Show(
                        "The  bad  news:   Loopstream just crashed :(\n" + 
                        "The good news:   I think I know why!\n\n" +
                        "Did you disconnect or disable your speakers or microphone?\n" +
                        "'cause you need to restart Loopstream every time you do that.\n\n" +
                        "Wanna go for a restart?", "Dirty devices detected",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation))
                    {
                        Program.fixWorkingDirectory();
                    }
                }
            }
            catch (Exception exx)
            {
                gDesc.Enabled = true;
                try
                {
                    gDesc.Text = "Shit is properly fucked, can't send error information to devs\r\n" +
                        "This is the best I can do:\r\n\r\n" +
                        ex.Message + "\r\n\r\n" + ex.StackTrace;
                }
                catch
                {
                    gDesc.Text = "Shit is properly fucked, can't send error information to devs";
                }
                gExit.Enabled = true;
                gExit.Focus();
            }
        }

        private void gSend_Click(object sender, EventArgs e)
        {
            gExit.Enabled = gSend.Enabled = gDesc.Enabled = false;
            Application.DoEvents();
            try
            {
                gi.UserDescription = gDesc.Text;
                string serialized = gi.ToString();

                try
                {
                    serialized += "\r\n" + GeneralInfo.ser(LSSettings.singleton);
                }
                catch { serialized += "\r\n" + "LSSettings Serialization Error"; }

                try
                {
                    serialized += "\r\n" + GeneralInfo.ser(LSSettings.singleton.devs);
                }
                catch { serialized += "\r\n" + "LSDevices Serialization Error"; }



                byte[] payload = Z.lze(serialized, true);
                Int32 nix = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                System.IO.File.WriteAllText("crash-" + nix, serialized, Encoding.UTF8);
                
                string hash;
                using (var sha = System.Security.Cryptography.MD5.Create())
                {
                    hash = BitConverter.ToString(sha.ComputeHash(
                        payload)).Replace("-", "").ToLower();
                }
                var ms = new System.IO.MemoryStream();

                

                /*
POST /loopstream/report.php HTTP/1.1
Host: ocv.me
User-Agent: Mozilla/5.0 (Windows NT 6.1; WOW64; rv:27.0) Gecko/20100101 Firefox/27.0
Accept: text/html,application/xhtml+xml,application/xml;q=0.9,* /*;q=0.8
Accept-Language: en-US,en;q=0.5
Accept-Encoding: gzip, deflate
Referer: http://ocv.me/loopstream/report.html
Connection: keep-alive
Cache-Control: max-age=0
Content-Type: multipart/form-data; boundary=---------------------------145202957330368
Content-Length: 11090

-----------------------------145202957330368
Content-Disposition: form-data; name="md"

f25e056b844391cd9b6f787595c55768
-----------------------------145202957330368
Content-Disposition: form-data; name="se"; filename="dump.lzma"
Content-Type: application/octet-stream

FILEDATA...
-----------------------------145202957330368--
                */



                string boundary = DateTime.Now.Ticks.ToString("x").PadLeft(44, '-');
                string body = string.Format(
"{1}{0}" +
"Content-Disposition: form-data; name=\"md\"{0}" +
"{0}{2}{0}{1}{0}" +
"Content-Disposition: form-data; name=\"se\"; filename=\"deadbeef.bin\"{0}" +
"Content-Type: application/octet-stream{0}" +
"{0}",
                    "\r\n",
                    boundary,
                    hash);
                
                Encoding enc = new UTF8Encoding(false);
                byte[] buf = enc.GetBytes(body);
                ms.Write(buf, 0, buf.Length);
                ms.Write(payload, 0, payload.Length);
                buf = enc.GetBytes("\r\n" + boundary + "--\r\n");
                ms.Write(buf, 0, buf.Length);
                string header = string.Format(
"POST /loopstream/report.php HTTP/1.1{0}" +
"Host: ocv.me{0}" +
"User-Agent: Loopstream/{1}{0}" +
"Connection: close{0}" +
"Content-Type: multipart/form-data; boundary={2}{0}" +
"Content-Length: {3}{0}" +
"{0}",
                    "\r\n",
                    Application.ProductVersion,
                    boundary.Substring(2),
                    ms.Length);

                payload = ms.ToArray();
                buf = enc.GetBytes(header);
                ms = new System.IO.MemoryStream();
                ms.Write(buf, 0, buf.Length);
                ms.Write(payload, 0, payload.Length);
                ms.Seek(0, System.IO.SeekOrigin.Begin);

                System.Net.Sockets.TcpClient tc;
                System.Net.Sockets.NetworkStream s;
                tc = new System.Net.Sockets.TcpClient();
                tc.Connect("ocv.me", 80);
                s = tc.GetStream();
                ms.WriteTo(s);
                s.Flush();
                buf = new byte[8192];
                int i = s.Read(buf, 0, buf.Length);
                header = enc.GetString(buf, 0, i);
                string msg = "";
                if (!header.Contains("ls_ex_rep_ok"))
                {
                    msg = "FAILED to send error info to devs!\n\n(the crash reporter just crashed)";
                }
                else
                {
                    msg = "Thank you !";
                }
                if (DialogResult.Yes == MessageBox.Show(
                    msg + "\n\n" + "Restart Loopstream?", "now what",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    Program.fixWorkingDirectory();
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show("the crash reporter just crashed:\n\n" + ee.Message + "\n\n" + ee.StackTrace);
            }
            this.Close();
        }

        private void gExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void UI_Exception_FormClosing(object sender, FormClosingEventArgs e)
        {
            Program.kill();
        }
    }
}
