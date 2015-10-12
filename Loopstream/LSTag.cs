using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Loopstream
{
    class LSTag
    {
        string auth;
        Encoding latin1;
        public LSTD tag;
        public LSTD manual;
        LSSettings settings;
        bool haveFailed;
        bool quitting;

        public LSTag(LSSettings set)
        {
            Logger.tag.a("init");

            settings = set;
            quitting = false;
            haveFailed = false;
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
                        ret += a == 0 ? "" : " - ";
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
                            if (i > 0)
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

        void feeder()
        {
            Est[] est = {
                settings.mp3.enabled ? new Est("", settings.mp3) : null,
                settings.ogg.enabled ? new Est("", settings.ogg) : null,
            };
            Logger.tag.a("active");
            while (!quitting)
            {
                LSSettings.LSMeta m = settings.meta;
                tag = settings.tagAuto ? get(m, false) : manual;
                manual = tag;
                if (!string.IsNullOrEmpty(tag.tag) && tag.ok)
                {
                    foreach (Est e in est)
                    {
                        if (e != null &&
                            e.tag != tag.tag &&
                            e.enc.FIXME_kbps > 0)
                        {
                            e.tag = tag.tag;
                            sendTags(e);
                        }
                    }
                }
                System.Threading.Thread.Sleep(settings.meta.freq);
            }
            Logger.tag.a("disposed");
        }

        void sendTags(Est est)
        {
            string meta;
            if (settings.latin)
            {
                meta = Uri.EscapeUriString(latin1.GetString(Encoding.UTF8.GetBytes(est.tag))).Replace("+", "%2B");
            }
            else
            {
                //meta = Uri.EscapeUriString(est.tag).Replace("+", "%2B");
                meta = Chencode.HonkHonk(est.tag);
                //meta = est.tag;
            }
            try
            {
                Logger.tag.a("send " + est.enc.ext + " " + est.tag);
                using (System.Net.WebClient wc = new System.Net.WebClient())
                {
                    wc.Headers.Add("Authorization: Basic " + auth);
                    string msg = wc.DownloadString(string.Format(
                        "http://{0}:{1}/admin/metadata?mode=updinfo&mount=/{2}.{3}&charset=UTF-8&song={4}",
                        settings.host,
                        settings.port,
                        settings.mount,
                        est.enc.ext,
                        meta));

                    Logger.tag.a(est.enc.ext + " socket ok");
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
            catch
            {
                Logger.tag.a(est.enc.ext + " send fail");
                tag = new LSTD(false, "Meta-fail " + est.enc.ext, "(unknown error)");
            }
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
