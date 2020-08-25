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
        // 1 = AppDomain.CurrentDomain.UnhandledException
        // 2 = Application.ThreadException

        static bool bail = false;
        public UI_Exception(Exception ex, int type)
        {
            if (ex != null && ex.Message != null)
                System.Console.WriteLine(bail + " " + ex.Message);

            if (bail)
                return;
            bail = true;

            this.ex = ex;
            this.type = type;
            defaultmsg = "You can add a description here, if you'd like\r\n\r\n(or maybe something like an email address even)";
            genExceptionData();

            new System.Threading.Thread(new System.Threading.ThreadStart(() => // ok fine i give up
            {
                System.Threading.Thread.Sleep(50);
                this.Invoke((MethodInvoker)delegate
                {
                    this.BringToFront();
                    Application.DoEvents();
                    t_Tick(null, null);
                });
            })).Start();
            this.ShowDialog();
        }

        private void UI_Exception_Load(object sender, EventArgs e)
        {
            this.Icon = Program.icon;
            label1.Font = new Font(label1.Font.FontFamily, label1.Font.SizeInPoints * 2.18f);
        }

        int type;
        Exception ex;
        GeneralInfo gi;
        string serText;
        string serPath;
        string defaultmsg;

        void t_Tick(object sender, EventArgs e)
        {
            if (serText == null)
                return;

            var sb = new StringBuilder(serText);
            sb.AppendLine();
            bool ok1, ok2, ok3, oks;
            ok1 = ok2 = ok3 = oks = false;
            try
            {
                LSSettings s = LSSettings.singleton;
                ok1 = s.devRec != null && (!(s.devRec is LSDevice)) || ((LSDevice)s.devRec).mm != null;
                ok2 = s.devMic != null && ((LSDevice)s.devMic).mm != null;
                ok3 = s.devOut != null && ((LSDevice)s.devOut).mm != null;
                sb.AppendLine("dev state indicated: " + ok1 + ", " + ok2 + ", " + ok3);
                LSSettings.singleton.init();
                s.runTests(null, true);
                ok1 = ok1 == (s.devRec != null && (!(s.devRec is LSDevice)) || ((LSDevice)s.devRec).mm != null);
                ok2 = ok2 == (s.devMic != null && ((LSDevice)s.devMic).mm != null);
                ok3 = ok3 == (s.devOut != null && ((LSDevice)s.devOut).mm != null);
                sb.AppendLine("dev state correct: " + ok1 + ", " + ok2 + ", " + ok3);
                oks = true;
            }
            catch { }
            serText = gi.ToString() + "\r\n" + sb.ToString();
            System.IO.File.WriteAllText(serPath + "2.txt", serText, Encoding.UTF8);
            System.IO.File.Delete(serPath + "1.txt");
            linkLabel1.Text = serPath + "2.txt";

            gDesc.Enabled = true;
            gDesc.Text = defaultmsg;
            gExit.Enabled = true;
            gSend.Enabled = true;
            gSend.Focus();

            if (oks && (!ok1 || !ok2 || !ok3))
            {
                if (DialogResult.Yes == MessageBox.Show(
                    "The  bad  news:   Loopstream just crashed :(\n" +
                    "The good news:   I think I know why!\n\n" +
                    "Did you disconnect or disable your speakers or microphone?\n" +
                    "'cause you need to restart Loopstream when you do that.\n\n" +
                    "Wanna go for a restart?", "Dirty devices detected",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation))
                {
                    Program.fixWorkingDirectory();
                }
            }
        }

        void genExceptionData()
        {
            this.gSend = null;
            try
            {
                Int32 nix = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                serPath = Program.tools + "loopstream-crash-" + nix + ".";

                var se = new SerializableException(ex);
                StringBuilder sb = new StringBuilder();
                gi = new GeneralInfo(se, ex);

                LSSettings s = LSSettings.singleton;
                try
                {
                    if (s == null)
                    {
                        LSSettings.singleton = new LSSettings();
                        s = LSSettings.singleton;
                        s.host = "nullsettings";
                    }
                    s.serverPresets = new List<LSSettings.LSServerPreset>();
                    s.metas = new List<LSSettings.LSMeta>();
                    s.host = "(redacted)";
                    s.pass = "(redacted)";
                    sb.Append("\r\n// cfg\r\n" + GeneralInfo.ser(s));
                }
                catch (Exception exx) { }

                try
                {
                    var devs = new LSDevice[s.devs.Length];
                    for (var a = 0; a < devs.Length; a++)
                        devs[a] = s.devs[a] as LSDevice;

                    sb.Append("\r\n// devs\r\n" + GeneralInfo.ser(devs));
                }
                catch (Exception exx) { }

                sb.AppendLine();
                try
                {
                    sb.AppendLine("\n\n\n\n\nCache for opus"); sb.AppendLine(Logger.opus.compile());
                    sb.AppendLine("\n\n\n\n\nCache for ogg"); sb.AppendLine(Logger.ogg.compile());
                    sb.AppendLine("\n\n\n\n\nCache for mp3"); sb.AppendLine(Logger.mp3.compile());
                    sb.AppendLine("\n\n\n\n\nCache for pcm"); sb.AppendLine(Logger.pcm.compile());
                    sb.AppendLine("\n\n\n\n\nCache for med"); sb.AppendLine(Logger.med.compile());
                    sb.AppendLine("\n\n\n\n\nCache for mix"); sb.AppendLine(Logger.mix.compile());
                    sb.AppendLine("\n\n\n\n\nCache for tag"); sb.AppendLine(Logger.tag.compile());
                    sb.AppendLine("\n\n\n\n\nCache for app"); sb.AppendLine(Logger.app.compile());
                }
                catch { }
                serText = gi.ToString() + "\r\n" + sb.ToString();
                System.IO.File.WriteAllText(serPath + "1.txt", serText, Encoding.UTF8);

                InitializeComponent();
                gDesc.Text = "L o a d i n g    d e t a i l s . . .";
                linkLabel1.Text = serPath + "1.txt";
                gExit.Enabled = false;
                gSend.Enabled = false;
                gDesc.Enabled = false;
            }
            catch (Exception exx)
            {
                serText = null;
                string desc = "";
                try
                {
                    var msg = ex.Message + "\r\n\r\n" + ex.StackTrace;

                    desc = "Shit is properly fucked, can't send error information to devs\r\n" +
                        "This is the best I can do:\r\n\r\n" + msg;
                    
                    System.IO.File.WriteAllText(serPath + "0.txt", msg, Encoding.UTF8);
                    linkLabel1.Text = serPath + "0.txt";
                }
                catch
                {
                    desc = "Shit is properly fucked, can't send error information to devs";
                }

                if (this.gSend == null)
                    InitializeComponent();

                gDesc.Text = desc;
                gDesc.Enabled = true;
                gExit.Enabled = true;
                gExit.Focus();
            }
        }

        private void gSend_Click(object sender, EventArgs e)
        {
            if (gDesc.Text.Contains(defaultmsg) && gDesc.Text.Length - defaultmsg.Length < 3)
            {
                if (DialogResult.Yes == MessageBox.Show(
                    "You didn't provide a way for me to contact you!\n\n" +
                    "Wanna go back and add an email address or something?",
                    "No contact info ", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation))
                {
                    return;
                }
            }
            gExit.Enabled = gSend.Enabled = gDesc.Enabled = false;
            Application.DoEvents();
            try
            {
                gi.UserDescription = gDesc.Text;
                byte[] payload = Z.lze(serText, true);
                
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

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(((Control)sender).Text));
        }
    }
}
