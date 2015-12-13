using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Loopstream
{
    public delegate void TagsChanged(string tags);
    public class LSTag
    {
        public event TagsChanged changedTags;
        string auth;
        Encoding latin1;
        public LSTD tag;
        public LSTD manual;
        LSSettings settings;
        bool socket_fallback;
        bool haveFailed;
        bool quitting;

        public LSTag(LSSettings set)
        {
            Logger.tag.a("init");

            settings = set;
            quitting = false;
            haveFailed = false;
            socket_fallback = false;
            latin1 = Encoding.GetEncoding("ISO_8859-1");
            manual = new LSTD(false, "", "STILL_UNUSED");
            tag = new LSTD(false, "", "STILL_UNUSED");
            
            auth = string.Format("{0}:{1}", settings.user, settings.pass);
            auth = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(auth)); 
            System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ThreadStart(feeder));
            t.Name = "LSTag_Feeder";
            t.Start();
        }

        public void Dispose()
        {
            quitting = true;
        }

        public void set(string str)
        {
            Logger.tag.a("set " + str);
            manual = new LSTD(true, str, "MANUALLY_APPLIED");
        }

        public static LSTD get(LSSettings.LSMeta m, bool getRaw)
        {
            if (m.reader == LSSettings.LSMeta.Reader.WindowCaption)
            {
                if (m.src.Trim() == "*")
                {
                    int myId = System.Diagnostics.Process.GetCurrentProcess().Id;
                    int[] handles = EnumHandles.Run();
                    foreach (int hWnd in handles)
                    {
                        IntPtr ptr = new IntPtr(hWnd);
                        uint proc = WinapiShit.getProcId(ptr);
                        if (proc <= 1) continue;
                        if (proc == myId) continue;
                        string text = WinapiShit.getWinText(ptr);
                        if (string.IsNullOrEmpty(text)) continue;

                        //gList.Items.Add("<" + hWnd + "> // <" + proc + "> // <" + text + ">");
                        LSTD td = get(m, text);
                        if (td.ok)
                        {
                            return td;
                        }
                    }
                    return new LSTD(false, "(no hits)",
                        "The pattern in this profile did not match any of your windows.\n" +
                        "\n" +
                        "If you are using an in-browser media player (cloud service),\n" +
                        "make sure the media player is in a dedicated browser window.\n" +
                        "\n" +
                        "(you must construct additional windows)");
                }


                string raw = null;
                if (m.src.Contains('*'))
                {
                    raw = WinapiShit.getWinText(new IntPtr(Convert.ToInt32(m.src.Split('*')[1], 16)));
                }
                if (string.IsNullOrEmpty(raw))
                {
                    Process[] proc = Process.GetProcessesByName(m.src.Split('*')[0]);
                    if (proc.Length < 1)
                    {
                        return new LSTD(false, "(no such process)", "The media player  «" + m.src + "»  could not be found!\n\nAre you sure that it is running?");
                    }
                    raw = proc[0].MainWindowTitle;
                }
                if (string.IsNullOrEmpty(raw))
                {
                    return new LSTD(false, "(no such target)", "The media player  «" + m.src + "»  could not be found!\n\nAre you sure that it is running?");
                }
                return getRaw ? new LSTD(true, raw, "SUCCESS") : get(m, raw);
            }
            if (m.reader == LSSettings.LSMeta.Reader.File)
            {
                try
                {
                    string ret = System.IO.File.ReadAllText(m.src, m.enc);
                    return getRaw ? new LSTD(true, ret, "SUCCESS") : get(m, ret);
                }
                catch
                {
                    return new LSTD(false, "(file read failure)", "Something went wrong while reading the provided file.\nAre you sure it exists?\n\nPath: " + m.src);
                }
            }
            if (m.reader == LSSettings.LSMeta.Reader.Website)
            {
                byte[] b;
                try
                {
                    b = new System.Net.WebClient().DownloadData(m.src);
                    try
                    {
                        string ret = m.enc.GetString(b);
                        return getRaw ? new LSTD(true, ret, "SUCCESS") : get(m, ret);
                    }
                    catch
                    {
                        return new LSTD(false, "(web decode failure)",  "I failed to unpack the data from the web server.\nMaybe incorrect address?\n\nLink: " + m.src);
                    }
                }
                catch
                {
                    return new LSTD(false, "(web request failure)", "I failed to download the data from the web server.\nMaybe it is down?\n\nLink: " + m.src);
                }
            }
            if (m.reader == LSSettings.LSMeta.Reader.ProcessMemory)
            {
                // this is the fun one
                Process[] proc = Process.GetProcessesByName(m.src);
                if (proc.Length < 1)
                {
                    return new LSTD(false, "(no such process)", "The media player  «" + m.src + "»  could not be found!\n\nAre you sure that it is running?");
                }
                LSMem mem;
                try
                {
                    mem = new LSMem(proc[0]);
                }
                catch
                {
                    return new LSTD(false, "(poke failure)", "I failed to harvest the metadata from inside  «" + m.src + "»...\n\nMaybe system permissions blocked the request?\nTry running Loopstream as administrator.");
                }
                try
                {
                    string ret = "";
                    string lpm = "";
                    bool hadError = false;
                    ProcessModule pm = null;
                    byte[] raw = new byte[1024];
                    string[] ad = m.ptn.Split(' ');
                    for (int a = 0; a < ad.Length; a++)
                    {
                        string arg = ad[a].Trim(',', ' ');
                        IntPtr ofs = IntPtr.Zero;
                        if (arg.Contains('+'))
                        {
                            string[] args = arg.Split('+');
                            if (args[0] != lpm)
                            {
                                pm = null;
                                lpm = null;
                                foreach (ProcessModule mod in proc[0].Modules)
                                {
                                    if (mod.ModuleName == args[0])
                                    //if (mod.FileName.EndsWith("\\iTunes.dll"))
                                    {
                                        pm = mod;
                                        lpm = pm.ModuleName;
                                        ofs = pm.BaseAddress;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                ofs = pm.BaseAddress;
                            }
                            arg = args[1];
                        }
                        int[] steps = new int[0];
                        if (arg.Contains('*'))
                        {
                            string[] args = arg.Split('*');
                            steps = new int[args.Length-1];
                            for (int b = 1; b < args.Length; b++)
                            {
                                steps[b - 1] = Convert.ToInt32(args[b], 16);
                            }
                            arg = args[0];
                        }
                        ofs += Convert.ToInt32(arg, 16);
                        ret += a == 0 || ret.Length == 0 ? "" : " - ";
                        int len = mem.read(ofs, raw, steps);
                        if (len < 0)
                        {
                            ret += "(read error)";
                            hadError = true;
                        }
                        else if (len > 0)
                        {
                            ret += m.enc.GetString(raw);
                            int i = ret.IndexOf('\0');
                            if (i >= 0)
                            {
                                ret = ret.Substring(0, i);
                            }
                        }
                    }
                    return new LSTD(!hadError, ret, hadError ? "Could not peek into the target application.\n\nThe media player is likely a 32bit process, while\nLoopstream is running in 64bit mode\n(or the other way around)." : "SUCCESS");
                }
                catch
                {
                    return new LSTD(false, "(peek failure)", "I failed to harvest the metadata from inside  «" + m.src + "»...\n\nThis is probably a bug in Loopstream,  but it could\nalso be system permissions getting in the way.\n\nYou could try running Loopstream as Administrator.");
                }
            }
            try
            {
                return new LSTD(false, "(unexpected metadata reader)", "You somehow managed to select a\nMetaReader which does not exist:\n\n      «" + m.reader.ToString() + "»");
            }
            catch
            {
                return new LSTD(false, "(unexpected metadata reader)", "You somehow managed to select a\nMetaReader which does not exist. How?");
            }
        }

        public static LSTD get(LSSettings.LSMeta m, string raw)
        {
            Logger.tag.a("get " + raw);
            if (m.reader == LSSettings.LSMeta.Reader.ProcessMemory)
            {
                return new LSTD(false, "if you are seeing this, go whine to ed", "(this really should not happen)");
            }
            GroupCollection r;
            try
            {
                r = Regex.Match(raw, m.ptn, RegexOptions.Singleline).Groups;
            }
            catch
            {
                return new LSTD(false, "(bad regex)", "The Pattern in this profile has a typo,\nor is otherwise broken. Call techsupport.\n\nPattern: " + m.ptn);
            }
            if (r.Count > m.yi.max)
            {
                try
                {
                    //string ret = r[m.grp].Value.Trim(' ', '\t', '\r', '\n'); // you can never be too sure
                    string ret = m.yi.format(r);
                    if (m.urldecode)
                    {
                        string[] sanitize = {
                            "&quot;", "\"",
                            "&apos;", "'",
                            "&#039;", "'",
                            "&lt;",   "<",
                            "&gt;",   ">",
                            "&amp;",  "&"
                        };
                        for (int a = 0; a < sanitize.Length; a += 2)
                        {
                            ret = ret.Replace(sanitize[a], sanitize[a + 1]);
                        }
                    }
                    return new LSTD(true, ret, "SUCCESS");
                }
                catch
                {
                }
            }
            return new LSTD(false, "(no match)",
            (
                m.reader == LSSettings.LSMeta.Reader.WindowCaption ? "The window title of  «" + m.src + "» " :
                m.reader == LSSettings.LSMeta.Reader.Website ? "The contents of the provided website" :
                m.reader == LSSettings.LSMeta.Reader.File ? "The contents in the provided file" :
                "(something is seriously wrong)"
            ) +
            " was not possible to\nunderstand using the Pattern specified in this profile." +
            (
                m.reader != LSSettings.LSMeta.Reader.WindowCaption ?
                "" : ("\n\nI tried to read from this:   " + m.src)
            ));
        }

        long bouncer;
        string lastTag;
        void feeder()
        {
            Est[] est = {
                settings.mp3.enabled ? new Est("", settings.mp3) : null,
                settings.ogg.enabled ? new Est("", settings.ogg) : null,
            };
            Logger.tag.a("active");
            bouncer = settings.meta.bnc;
            lastTag = null;
            while (!quitting)
            {
                LSSettings.LSMeta m = settings.meta;
                tag = settings.tagAuto ? get(m, false) : manual;
                manual = tag;
                if (!string.IsNullOrEmpty(tag.tag) && tag.ok)
                {
                    long now = DateTime.UtcNow.Ticks / 10000;
                    if (lastTag != tag.tag)
                    {
                        lastTag = tag.tag;
                        bouncer = settings.meta.bnc;
                    }
                    if (--bouncer < 0)
                    {
                        foreach (Est e in est)
                        {
                            if (e != null &&
                                e.tag != tag.tag &&
                                e.enc.FIXME_kbps > 0)
                            {
                                e.tag = tag.tag;
                                if (e.enc.ext.Contains("ogg"))
                                {
                                    if ((e.enc.tagMethod & LSSettings.LSTagMethod.inband) == LSSettings.LSTagMethod.inband)
                                        changedTags(e.tag);
                                    if ((e.enc.tagMethod & LSSettings.LSTagMethod.outband) == LSSettings.LSTagMethod.outband)
                                        sendTags(e);
                                }
                                else
                                    sendTags(e);
                            }
                        }
                    }
                }
                System.Threading.Thread.Sleep(settings.meta.freq);
            }
            Logger.tag.a("disposed");
        }

        void sendTags(Est est)
        {
            try
            {
                System.IO.File.AppendAllText(
                    est.enc.i.filename + ".txt",
                    est.enc.i.timestamp() + " " + est.tag + "\r\n",
                    Encoding.UTF8);
            }
            catch { }
            string meta = est.tag;
            //meta = new string(meta.Reverse().ToArray());
            if (settings.latin)
            {
                meta = Uri.EscapeUriString(latin1.GetString(Encoding.UTF8.GetBytes(meta))).Replace("+", "%2B");
            }
            else
            {
                //meta = Uri.EscapeUriString(est.tag).Replace("+", "%2B");
                meta = Chencode.HonkHonk(meta);
                //meta = est.tag;
            }
            if (!socket_fallback && !settings.tagsock)
            {
                try
                {
                    Logger.tag.a("wc_send: " + est.enc.ext + " " + est.tag);
                    string url = string.Format(
                        "http://{0}:{1}/admin/metadata?mode=updinfo&mount=/{2}.{3}&charset=UTF-8&song={4}",
                        settings.host,
                        settings.port,
                        settings.mount,
                        est.enc.ext,
                        meta);

                    Logger.tag.a("url: " + url);
                    using (System.Net.WebClient wc = new System.Net.WebClient())
                    {
                        //throw new ExecutionEngineException();
                        Logger.tag.a("made webclient");
                        wc.Headers.Add("Authorization: Basic " + auth);
                        string msg = wc.DownloadString(url);

                        Logger.tag.a(est.enc.ext + " socket ok: " + msg);
                        if (!haveFailed && !msg.Contains("<return>1</return>"))
                        {
                            haveFailed = true;
                            msg = msg.Contains("<message>") ? msg.Substring(msg.IndexOf("<message>") + 9) : msg;
                            msg = msg.Contains("</message>") ? msg.Substring(0, msg.LastIndexOf("</message>")) : msg;
                            System.Windows.Forms.MessageBox.Show("Metadata broadcast failure:\r\n\r\n" + msg, "Tags failed",
                                System.Windows.Forms.MessageBoxButtons.OK,
                                System.Windows.Forms.MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception e)
                {
                    socket_fallback = true;
                    Logger.tag.a(est.enc.ext + " wc_send fail: " + e.Message);
                    //tag = new LSTD(false, "Meta-fail " + est.enc.ext, "(unknown error)");
                }
            }
            if (socket_fallback || settings.tagsock)
            {
                try
                {
                    Logger.tag.a("sck_send: " + est.enc.ext + " " + est.tag);
                    string header = string.Format(
                        "GET /admin/metadata?mode=updinfo&mount=/{1}.{2}&charset=UTF-8&song={3} HTTP/1.1{0}" +
                        "Authorization: Basic {4}{0}" +
                        "Host: {5}:{6}{0}" +
                        "Connection: Close{0}{0}",
                        "\r\n",
                        settings.mount,
                        est.enc.ext,
                        meta,
                        auth,
                        settings.host,
                        settings.port);

                    Logger.tag.a(header.Replace(auth, "BASE_64_AUTH"));
                    byte[] buffer = System.Text.Encoding.UTF8.GetBytes(header);
                    var kc = new System.Net.Sockets.TcpClient();
                    kc.Connect(settings.host, settings.port);
                    Logger.tag.a("sck_connected");
                    var ks = kc.GetStream();
                    ks.Write(buffer, 0, buffer.Length);
                    ks.Flush();
                    Logger.tag.a("sck_sent");
                    buffer = new byte[8192];

                    /*var response = new List<byte>();
                    byte[][] find1 = {
                        System.Text.Encoding.UTF8.GetBytes("\nContent-Length:"),
                        System.Text.Encoding.UTF8.GetBytes("\ncontent-length:"),
                        System.Text.Encoding.UTF8.GetBytes("\ncontent-Length:"),
                        System.Text.Encoding.UTF8.GetBytes("\nContent-length:"),
                        System.Text.Encoding.UTF8.GetBytes("\nCONTENT-LENGTH:")
                    };
                    byte[] find2 = System.Text.Encoding.UTF8.GetBytes("\r\n\r\n");
                    byte[] find3 = System.Text.Encoding.UTF8.GetBytes("\n\n");
                    while (kc.Connected)
                    {
                        int i = ks.Read(buffer, 0, buffer.Length);
                        if (i <= 0) break;
                        for (int a = 0; a < i; a++) response.Add(buffer[a]);
                        string tmp = latin1.GetString(response.ToArray());
                        if (tmp.Length == i) System.Windows.Forms.MessageBox.Show(tmp);
                        i = -1;
                        for (int n = 0; n < find1.Length; n++)
                        {
                            i = find(response, find1[n], 0);
                            if (i > 0) break;
                        }
                        if (i < 0) continue;
                        i += find1[0].Length;
                        
                        int nl = find(response, find2, i);
                        if (nl < 0)
                            nl = find(response, find3, i);
                        if (nl < 0) continue;

                        while (response[i] == ' ') i++;
                        List<byte> asdf = new List<byte>();
                        while (
                            response[i] != '\r' &&
                            response[i] != '\n')
                            asdf.Add(response[i]);

                        int clen = Convert.ToInt32(System.Text.Encoding.UTF8.GetString(asdf.ToArray()));
                        int red = response.Count - nl;
                        if (clen - red < 5) break;
                    }
                    string msg = System.Text.Encoding.UTF8.GetString(response.ToArray());*/
                    string msg = "";
                    while (kc.Connected)
                    {
                        int i = ks.Read(buffer, 0, buffer.Length);
                        if (i <= 0) break;
                        msg += latin1.GetString(buffer, 0, i);

                        int clen = msg.IndexOf("\ncontent-length:", StringComparison.OrdinalIgnoreCase);
                        if (clen < 0) continue;
                        clen += 16;
                        int nl = msg.IndexOf("\r\n\r\n", clen);
                        if (nl <= 0)
                            nl = msg.IndexOf("\n\n", clen);
                        if (nl <= 0) continue;

                        while (msg[clen] == ' ' && clen < nl) clen++;
                        clen = Convert.ToInt32(msg.Substring(clen, msg.IndexOf("\n", clen) - clen).Trim('\r', '\n', ' '));
                        clen = (msg.Length - nl) - clen;
                        if (clen < 8)
                        {
                            Logger.tag.a("got eof");
                            break;
                        }
                    }
                    Logger.tag.a("sck_ok: " + msg);
                    if (!haveFailed && !msg.Contains("<return>1</return>"))
                    {
                        haveFailed = true;
                        msg = msg.Contains("<message>") ? msg.Substring(msg.IndexOf("<message>") + 9) : msg;
                        msg = msg.Contains("</message>") ? msg.Substring(0, msg.LastIndexOf("</message>")) : msg;
                        System.Windows.Forms.MessageBox.Show("Metadata broadcast failure:\r\n\r\n" + msg, "Tags failed",
                            System.Windows.Forms.MessageBoxButtons.OK,
                            System.Windows.Forms.MessageBoxIcon.Error);
                    }
                }
                catch (Exception e)
                {
                    Logger.tag.a(est.enc.ext + " sck_send fail: " + e.Message);
                    tag = new LSTD(false, "Meta-fail " + est.enc.ext, "(unknown error)");
                }
            }
        }

        int find(List<byte> hs, byte[] ne, int o)
        {
            if (ne.Length > hs.Count - o)
            {
                return -1;
            }
            for (int i = o; i < hs.Count - ne.Length; i++)
            {
                bool yes = true;
                for (int j = 0; j < ne.Length; j++)
                {
                    if (hs[i + j] != ne[j])
                    {
                        yes = false;
                        break;
                    }
                }
                if (yes)
                    return i;
            }
            return -1;
        }

        private class Est
        {
            public string tag;
            public LSSettings.LSParams enc;
            public Est(string s, LSSettings.LSParams e)
            {
                tag = s;
                enc = e;
            }
        }

        public class LSTD
        {
            public bool ok;
            public string tag;
            public string error;
            public LSTD(bool o, string t, string e)
            {
                error = e;
                tag = t;
                ok = o;
            }
        }
    }
}
