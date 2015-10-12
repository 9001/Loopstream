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
        public string tag;
        public string manual;
        LSSettings settings;
        bool haveFailed;
        bool quitting;

        public LSTag(LSSettings set)
        {
            Logger.tag.a("init");

            tag = "";
            settings = set;
            quitting = false;
            haveFailed = false;
            latin1 = Encoding.GetEncoding("ISO_8859-1");
            
            auth = "source:" + settings.pass;
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
            manual = str;
        }

        public static string get(LSSettings.LSMeta m, bool getRaw)
        {
            if (m.reader == LSSettings.LSMeta.Reader.WindowCaption)
            {
                Process[] proc = Process.GetProcessesByName(m.src);
                if (proc.Length < 1)
                {
                    return "(no such process)";
                }
                string raw = proc[0].MainWindowTitle;
                return getRaw ? raw : get(m, raw);
            }
            if (m.reader == LSSettings.LSMeta.Reader.File)
            {
                string ret = null;
                try
                {
                    ret = System.IO.File.ReadAllText(m.src, m.enc);
                }
                catch
                {
                    ret = "(file read failure)";
                }
                return getRaw ? ret : get(m, ret);
            }
            if (m.reader == LSSettings.LSMeta.Reader.Website)
            {
                string ret;
                byte[] b;
                try
                {
                    b = new System.Net.WebClient().DownloadData(m.src);
                    try
                    {
                        ret = m.enc.GetString(b);
                    }
                    catch
                    {
                        ret = "(web decode failure)";
                    }
                }
                catch
                {
                    ret = "(web request failure)";
                }
                return getRaw ? ret : get(m, ret);
            }
            if (m.reader == LSSettings.LSMeta.Reader.ProcessMemory)
            {
                // this is the fun one
                Process[] proc = Process.GetProcessesByName(m.src);
                if (proc.Length < 1)
                {
                    return "(no such process)";
                }
                LSMem mem;
                try
                {
                    mem = new LSMem(proc[0]);
                }
                catch
                {
                    return "(poke failure)";
                }
                try
                {
                    string ret = "";
                    string lpm = "";
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
                        if (len <= 0)
                        {
                            ret += "(read error)";
                        }
                        else
                        {
                            ret += m.enc.GetString(raw);
                            int i = ret.IndexOf('\0');
                            if (i > 0)
                            {
                                ret = ret.Substring(0, i);
                            }
                        }
                    }
                    return ret;
                }
                catch
                {
                    return "(peek failure)";
                }
            }
            return "(unexpected failure)";
        }

        public static string get(LSSettings.LSMeta m, string raw)
        {
            Logger.tag.a("get " + raw);
            if (m.reader == LSSettings.LSMeta.Reader.ProcessMemory)
            {
                throw new Exception("if you are seeing this, go whine to ed");
            }
            GroupCollection r;
            try
            {
                r = Regex.Match(raw, m.ptn, RegexOptions.Singleline).Groups;
            }
            catch
            {
                return "(bad regex)";
            }
            try
            {
                return r[m.grp].Value.Trim(' ', '\t', '\r', '\n'); // you can never be too sure
            }
            catch
            {
                return "(no match)";
            }
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
                if (!string.IsNullOrEmpty(tag))
                {
                    foreach (Est e in est)
                    {
                        if (e != null &&
                            e.tag != tag &&
                            e.enc.FIXME_kbps > 0)
                        {
                            e.tag = tag;
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
                tag = "Meta-fail " + est.enc.ext;
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
    }
}
