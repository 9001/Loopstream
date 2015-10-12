using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Loopstream
{
    public class LSEncoder
    {
        object locker;
        protected bool dump;
        protected Process proc;
        protected LSPcmFeed pimp;
        protected LSSettings settings;
        public LSSettings.LSParams enc;

        public Stream stdin { get; set; }
        public Stream stdout { get; set; }
        protected Stream pstdin { get; set; }
        protected Stream pstdout { get; set; }
        public bool crashed { get; private set; }

        System.Net.Sockets.TcpClient tc;
        System.Net.Sockets.NetworkStream s;

        public LSEncoder()
        {
            stdin = stdout = pstdin = pstdout = null;
            s = null;
            tc = null;
            enc = null;
            proc = null;
            pimp = null;
            dump = false;
            settings = null;
            crashed = false;
            locker = new object();
        }

        protected void makeShouter()
        {
            System.Net.Sockets.NetworkStream prepS;
            string ver = Application.ProductVersion;
            string auth = "source:" + settings.pass;
            auth = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(auth));
            byte[] header;
            if (enc.ext == "mp3")
            {
                header = System.Text.Encoding.UTF8.GetBytes(string.Format(
                    "SOURCE /{1}.{2} HTTP/1.0{0}" +
                    "Authorization: Basic {3}{0}" +
                    "User-Agent: loopstream/{4}{0}" +
                    "Content-Type: audio/mpeg{0}" +
                    "ice-name: Loopstream{0}" +
                    "ice-public: 0{0}" +
                    "ice-url: https://github.com/9001/loopstream{0}" +
                    "ice-genre: Post-Avant Jazzcore{0}" +
                    "ice-audio-info: channels={5};samplerate={6};{7}={8}{0}" +
                    "ice-description: Wasapi Capture{0}{0}",

                    "\r\n",
                    settings.mount,
                    enc.ext,
                    auth,
                    ver,
                    enc.channels == LSSettings.LSChannels.stereo ? 2 : 1,
                    settings.samplerate,
                    enc.compression == LSSettings.LSCompression.cbr ? "bitrate" : "quality",
                    enc.compression == LSSettings.LSCompression.cbr ? "" + enc.bitrate : enc.quality + ".0"));
            }
            else
            {
                header = System.Text.Encoding.UTF8.GetBytes(string.Format(
                    "SOURCE /{1}.{2} ICE/1.0{0}" +
                    "Content-Type: application/ogg{0}" +
                    "Authorization: Basic {3}{0}" +
                    "User-Agent: loopstream/{4}{0}" +
                    "ice-name: Loopstream{0}" +
                    "ice-url: https://github.com/9001/loopstream{0}" +
                    "ice-genre: Post-Avant Jazzcore{0}" +
                    "ice-bitrate: {5}{0}" +
                    "ice-private: 0{0}" +
                    "ice-public: 0{0}" +
                    "ice-description: Wasapi Capture{0}" +
                    "ice-audio-info: ice-samplerate={6};ice-channels={5};ice-bitrate={5}{0}{0}",

                    "\r\n",
                    settings.mount,
                    enc.ext,
                    auth,
                    ver,
                    enc.compression == LSSettings.LSCompression.cbr ? enc.bitrate + "" : "Quality " + enc.quality,
                    settings.samplerate,
                    enc.channels == LSSettings.LSChannels.stereo ? 2 : 1));
            }

            string str = "(no reply)";
            try
            {
                tc = new System.Net.Sockets.TcpClient();
                tc.Connect(settings.host, settings.port);
                prepS = tc.GetStream();
                prepS.Write(header, 0, header.Length);
                int i = prepS.Read(header, 0, header.Length);
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
                s = prepS;
                stampee = 0;
                stdin = pstdin;
                stdout = pstdout;
                stamps = new long[32];
                chunks = new long[32];
                long v = DateTime.UtcNow.Ticks / 10000;
                for (int a = 0; a < stamps.Length; a++) stamps[a] = v;
                Program.ni.ShowBalloonTip(1000, "Loopstream Connected", "Streaming to " + settings.mount + "." + enc.ext, ToolTipIcon.Info);
                new System.Threading.Thread(new System.Threading.ThreadStart(reader)).Start();
                new System.Threading.Thread(new System.Threading.ThreadStart(counter)).Start();
            }
            else
            {
                MessageBox.Show("Unknown radio server error:\r\n\r\n" + str,
                    "Stream abort", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        int stampee;
        public long[] chunks;
        public long[] stamps;
        protected void reader()
        {
            System.IO.FileStream m = null;
            if (dump)
            {
                m = new System.IO.FileStream(string.Format("Loopstream-{0}.{1}",
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
                try
                {
                    s.Write(buffer, 0, i);
                }
                catch
                {
                    enc.FIXME_kbps = -1;
                    crashed = true;
                    break;
                }
                
                // speed measuring
                lock (locker)
                {
                    chunks[stampee] = i;
                    stamps[stampee] = DateTime.UtcNow.Ticks / 10000;
                    stampee = ++stampee < stamps.Length ? stampee : 0;
                }
            }
            string fn = proc.StartInfo.FileName;
            fn = fn.Substring(fn.Replace('\\', '/').LastIndexOf('/') + 1).Split('.')[0];
            Console.WriteLine("shutting down " + fn);
            if (m != null) m.Close();
        }

        void counter()
        {
            while (!crashed)
            {
                lock (locker)
                {
                    long time = DateTime.UtcNow.Ticks / 10000;
                    int i = stampee + 1;
                    i = i >= stamps.Length ? 0 : i;
                    time -= stamps[i];
                    enc.FIXME_kbps = Math.Max(1, ((8000.0 * (chunks.Sum() - 4096)) / time) / 1024);
                }
                System.Threading.Thread.Sleep(10);
            }
            enc.FIXME_kbps = -1;
        }

        public void Dispose()
        {
            enc.FIXME_kbps = -1;
            stdin = stdout = null;
            crashed = true;
            proc.Kill();
            try
            {
                tc.Close();
            }
            catch
            {
                // you took a nuke to the head and you're worrying about a socket?
            }
        }
    }
}
