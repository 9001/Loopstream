using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LoopStream
{
    public class LSEncoder
    {
        protected bool dump;
        protected Process proc;
        protected LSPcmFeed pimp;
        protected LSSettings settings;
        protected LSSettings.LSParams enc;

        public virtual Stream stdin { get; set; }
        public virtual Stream stdout { get; set; }
        public void Kill() { }

        System.Net.Sockets.TcpClient tc;
        System.Net.Sockets.NetworkStream s;

        protected void makeShouter()
        {
            string ver = Application.ProductVersion;
            string auth = "source:" + settings.pass;
            auth = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(auth));
            byte[] header = System.Text.Encoding.UTF8.GetBytes(string.Format(
                "SOURCE /{1}.{2} HTTP/1.0{0}" +
                "Authorization: Basic {3}{0}" +
                "User-Agent: loopstream/{4}{0}" +
                "Content-Type: {5}{0}" +
                "ice-name: LoopStream{0}" +
                "ice-public: 0{0}" +
                "ice-url: https://github.com/9001/loopstream{0}" +
                "ice-genre: Post-Avant Jazzcore{0}" +
                "ice-audio-info: channels={6};samplerate={7};{8}={9}{0}" +
                "ice-description: Wasapi Capture{0}{0}",
                
                "\r\n",
                settings.mount,
                enc.ext,
                auth,
                ver,
                enc.ext == "mp3" ? "audio/mpeg" :
                enc.ext == "ogg" ? "application/ogg" :
                                   "idk/wtf",
                enc.channels == LSSettings.LSChannels.stereo ? 2 : 1,
                settings.samplerate,
                enc.compression == LSSettings.LSCompression.cbr ? "bitrate" : "quality",
                enc.compression == LSSettings.LSCompression.cbr ? "" + enc.bitrate : enc.quality + ".0"));

            string str = "(no reply)";
            try
            {
                tc = new System.Net.Sockets.TcpClient();
                tc.Connect(settings.host, settings.port);
                s = tc.GetStream();
                s.Write(header, 0, header.Length);
                int i = s.Read(header, 0, header.Length);
                str = System.Text.Encoding.UTF8.GetString(header, 0, i);
            }
            catch (Exception e)
            {
                MessageBox.Show("Server connection error:\r\n\r\n" + e.Message + " (" + e.Source + ")",
                    "Stream abort", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (str.StartsWith("HTTP/1.0 401 Unauthorized"))
            {
                MessageBox.Show("Radio server error: Wrong password",
                    "Stream abort", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (str.StartsWith("HTTP/1.0 200 OK"))
            {
                Program.ni.ShowBalloonTip(1000, "LoopStream Connected", "Streaming to " + settings.mount + "." + enc.ext, ToolTipIcon.Info);
                new System.Threading.Thread(new System.Threading.ThreadStart(reader)).Start();
            }
            else
            {
                MessageBox.Show("Unknown radio server error:\r\n\r\n" + str,
                    "Stream abort", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected void reader()
        {
            System.IO.FileStream m = null;
            if (dump)
            {
                m = new System.IO.FileStream(string.Format("LoopStream-{0}.{1}",
                    DateTime.Now.ToString("yyyy-MM-dd_HH.mm.ss"), enc.ext),
                    System.IO.FileMode.Create);
            }
            long bufSize = settings.samplerate * 10;
            byte[] buffer = new byte[bufSize * 4];
            while (true)
            {
                if (pimp.qt()) break;
                //Console.Write('!');
                int i = stdout.Read(buffer, 0, 4096);
                if (m != null)
                {
                    m.Write(buffer, 0, i);
                }
                s.Write(buffer, 0, i);
            }
            string fn = proc.StartInfo.FileName;
            fn = fn.Substring(fn.Replace('\\', '/').LastIndexOf('/') + 1).Split('.')[0];
            Console.WriteLine("shutting down " + fn);
            if (m != null) m.Close();
        }
    }
}
