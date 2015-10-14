using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Loopstream
{
    public class LSFrame
    {

    }

    public class LSEncoder
    {
        object locker;
        protected bool dump;
        protected Process proc;
        protected Logger logger;
        protected LSPcmFeed pimp;
        protected LSSettings settings;
        public LSSettings.LSParams enc;
        public int rekick;

        public Stream stdin { get; set; }
        public Stream stdout { get; set; }
        protected Stream pstdin { get; set; }
        protected Stream pstdout { get; set; }
        public bool crashed { get; private set; }
        public bool aborted { get; private set; }

        public int iFrame;
        public LSFrame[] frames;

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
            rekick = 0;
        }

        string esc(string raw)
        {
            return raw;
        }

        protected void makeShouter()
        {
            logger.a("make shouter");
            proc.PriorityClass = System.Diagnostics.ProcessPriorityClass.AboveNormal;

            if (string.IsNullOrEmpty(settings.host))
            {
                s = null;
                rekick = 0;
                stampee = 0;
                stdin = pstdin;
                stdout = pstdout;
                stamps = new long[32];
                chunks = new long[32];
                long v = DateTime.UtcNow.Ticks / 10000;
                for (int a = 0; a < stamps.Length; a++) stamps[a] = v;

                System.Threading.Thread tr = new System.Threading.Thread(new System.Threading.ThreadStart(reader));
                tr.Name = "LSEnc_Reader";
                tr.Start();
                System.Threading.Thread tc = new System.Threading.Thread(new System.Threading.ThreadStart(counter));
                tc.Name = "LSEnc_Counter";
                tc.Start();
                return;
            }

            System.Net.Sockets.NetworkStream prepS;
            string ver = Application.ProductVersion;
            string auth = string.Format("{0}:{1}", settings.user, settings.pass);
            auth = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(auth));
            byte[] header;
            if (enc.ext == "mp3")
            {
                header = System.Text.Encoding.UTF8.GetBytes(string.Format(
                    "SOURCE /{1}.{2} HTTP/1.0{0}" +
                    "Authorization: Basic {3}{0}" +
                    "User-Agent: loopstream/{4}{0}" +
                    "Content-Type: audio/mpeg{0}" +
                    "ice-name: {5}{0}" +
                    "ice-public: {6}{0}" +
                    "ice-url: {7}{0}" +
                    "ice-genre: {8}{0}" +
                    "ice-audio-info: channels={9};samplerate={10};{11}={12}{0}" +
                    "ice-description: {13}{0}{0}",

                    "\r\n",
                    settings.mount,
                    enc.ext,
                    auth,
                    ver,

                    esc(settings.title),
                    settings.pubstream ? "1" : "0",
                    esc(settings.url),
                    esc(settings.genre),

                    enc.channels == LSSettings.LSChannels.stereo ? 2 : 1,
                    settings.samplerate,
                    enc.compression == LSSettings.LSCompression.cbr ? "bitrate" : "quality",
                    enc.compression == LSSettings.LSCompression.cbr ? "" + enc.bitrate : enc.quality + ".0",

                    esc(settings.description)
                ));
            }
            else
            {
                header = System.Text.Encoding.UTF8.GetBytes(string.Format(
                    "SOURCE /{1}.{2} ICE/1.0{0}" +
                    "Content-Type: application/ogg{0}" +
                    "Authorization: Basic {3}{0}" +
                    "User-Agent: loopstream/{4}{0}" +
                    "ice-name: {5}{0}" +
                    "ice-url: {6}{0}" +
                    "ice-genre: {7}{0}" +
                    "ice-bitrate: {8}{0}" +
                    "ice-private: {9}{0}" +
                    "ice-public: {10}{0}" +
                    "ice-description: {11}{0}" +
                    "ice-audio-info: ice-samplerate={12};ice-channels={13};ice-bitrate={8}{0}{0}",

                    "\r\n",
                    settings.mount,
                    enc.ext,
                    auth,
                    ver,
                    esc(settings.title),
                    esc(settings.url),
                    esc(settings.genre),
                    enc.compression == LSSettings.LSCompression.cbr ? enc.bitrate + "" : "Quality " + enc.quality,
                    settings.pubstream ? "0" : "1", // why
                    settings.pubstream ? "1" : "0",
                    esc(settings.description),
                    settings.samplerate,
                    enc.channels == LSSettings.LSChannels.stereo ? 2 : 1));
            }

            string str = "(no reply)";
            try
            {
                logger.a("making socket");
                tc = new System.Net.Sockets.TcpClient();
                tc.Connect(settings.host, settings.port);
                logger.a("socket connected");
                prepS = tc.GetStream();
                prepS.Write(header, 0, header.Length);
                logger.a("headers sent");
                int i = prepS.Read(header, 0, header.Length);
                str = System.Text.Encoding.UTF8.GetString(header, 0, i);
                logger.a("socket alive");
            }
            catch (Exception e)
            {
                Program.ni.ShowBalloonTip(9999, "Server connection error",
                    e.Message + " (" + e.Source + ")",
                    ToolTipIcon.Error);
                System.Threading.Thread.Sleep(200);
                crashed = true;
                return;
            }
            if (str.StartsWith("HTTP/1.0 401 Unauthorized"))
            {
                MessageBox.Show("Radio server error: Wrong password",
                    "Stream abort", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (str.StartsWith("HTTP/1.0 403 Mountpoint in use"))
            {
                if (--rekick > 0)
                {
                    crashed = true;
                }
                else if (DialogResult.Yes == MessageBox.Show(
                    "Someone are already connected to the server.\n\nTry to kick them?",
                    "Mountpoint in use", MessageBoxButtons.YesNo))
                {
                    byte[] kickrequest = System.Text.Encoding.UTF8.GetBytes(string.Format(
                        "GET /admin/killsource?mount=/{1}.{2} HTTP/1.0{0}" +
                        "Authorization: Basic {3}{0}" +
                        "User-Agent: loopstream/{4}{0}" +
                        "Content-Type: audio/mpeg{0}{0}",

                        "\r\n",
                        settings.mount,
                        enc.ext,
                        auth,
                        ver
                    ));
                    logger.a("kicker socket");
                    var kc = new System.Net.Sockets.TcpClient();
                    kc.Connect(settings.host, settings.port);
                    logger.a("kicker connected");
                    var ks = kc.GetStream();
                    ks.Write(kickrequest, 0, kickrequest.Length);
                    ks.Flush();
                    logger.a("kicker sent");
                    int i = ks.Read(kickrequest, 0, kickrequest.Length);
                    string kickresult = System.Text.Encoding.UTF8.GetString(kickrequest, 0, i);
                    logger.a("kicker done");
                    rekick = 5;
                    crashed = true;
                }
                else
                {
                    aborted = true;
                }
            }
            else if (str.StartsWith("HTTP/1.0 200 OK"))
            {
                logger.a("bootstrap");
                s = prepS;
                rekick = 0;
                stampee = 0;
                stdin = pstdin;
                stdout = pstdout;
                s.WriteTimeout = 1000;
                stamps = new long[32];
                chunks = new long[32];
                long v = DateTime.UtcNow.Ticks / 10000;
                for (int a = 0; a < stamps.Length; a++) stamps[a] = v;
                Program.ni.ShowBalloonTip(1000, "Loopstream Connected", "Streaming to " + settings.mount + "." + enc.ext, ToolTipIcon.Info);
                System.Threading.Thread tr = new System.Threading.Thread(new System.Threading.ThreadStart(reader));
                tr.Name = "LSEnc_Reader";
                tr.Start();
                System.Threading.Thread tc = new System.Threading.Thread(new System.Threading.ThreadStart(counter));
                tc.Name = "LSEnc_Counter";
                tc.Start();
            }
            else
            {
                MessageBox.Show("Unknown radio server error:\n\n" + str,
                    "Stream abort", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void eat(byte[] buffer, int c)
        {
            if (crashed || aborted) return;

            try
            {
                stdin.Write(buffer, 0, c);
                stdin.Flush();
            }
            catch
            {
                logger.a("encoder write failed");
                enc.FIXME_kbps = -1;
                crashed = true;
            }
        }

        int stampee;
        public long[] chunks;
        public long[] stamps;
        protected void reader()
        {
            logger.a("reader thread");
            System.IO.FileStream m = null;
            enc.i.begin = DateTime.UtcNow;
            enc.i.filename = "Loopstream-" +
                enc.i.begin.ToString("yyyy-MM-dd_HH.mm.ss.") +
                enc.ext;

            if (dump)
            {
                m = new System.IO.FileStream(
                    enc.i.filename,
                    System.IO.FileMode.Create);
            }
            long bufSize = settings.samplerate * 10;
            byte[] buffer = new byte[bufSize * 4];
            try
            {
                while (true)
                {
                    if (pimp.qt()) break;
                    //Console.Write('!');
                    logger.a("awaiting encoder output");
                    int i = stdout.Read(buffer, 0, 4096);
                    if (m != null)
                    {
                        logger.a("writing file");
                        m.Write(buffer, 0, i);
                    }
                    try
                    {
                        logger.a("writing socket");
                        
                        if (!string.IsNullOrEmpty(settings.host))
                            s.Write(buffer, 0, i);
                    }
                    catch
                    {
                        logger.a("socket write failed");
                        enc.FIXME_kbps = -1;
                        crashed = true;
                        break;
                    }

                    // speed measuring
                    logger.a("speed measure");
                    lock (locker)
                    {
                        chunks[stampee] = i;
                        stamps[stampee] = DateTime.UtcNow.Ticks / 10000;
                        stampee = ++stampee < stamps.Length ? stampee : 0;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.a("encoder readloop crash");
                //Program.ni.ShowBalloonTip("an encoder reading thread just crashed\r\n\r\nthought you might want to know");
            }
            string fn = proc.StartInfo.FileName;
            fn = fn.Substring(fn.Replace('\\', '/').LastIndexOf('/') + 1).Split('.')[0];
            Console.WriteLine("shutting down " + fn);
            if (m != null) m.Close();
            logger.a("mp3data readloop closed");
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
            logger.a("dispose called");
            enc.FIXME_kbps = -1;
            stdin = stdout = null;
            crashed = true;
            try
            {
                logger.a("proc dispose");
                proc.Kill();
            }
            catch
            {
                logger.a("proc dispose failure");
                // how can you kill that which is already dead
            }
            try
            {
                logger.a("socket dispose");
                tc.Close();
            }
            catch
            {
                logger.a("socket dispose failure");
                // you took a nuke to the head and you're worrying about a socket?
            }
        }
    }
}
