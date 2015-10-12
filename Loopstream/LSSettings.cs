using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Loopstream
{
    public class LSSettings
    {
        public class LSServerPreset
        {
            public int port;
            public LSRelay relay;
            public string host, user, pass, mount;
            public string title, description, genre, url;
            public bool pubstream;
            public string presetName;

            public LSServerPreset()
            {
                presetName = "<no name>";
                host = user = pass = mount = "";
                relay = LSRelay.ice;
                port = 0;
            }
            public void SaveToProfile(LSSettings settings)
            {
                host = settings.host;
                port = settings.port;
                user = settings.user;
                pass = settings.pass;
                mount = settings.mount;
                relay = settings.relay;

                title = settings.title;
                description = settings.description;
                genre = settings.genre;
                url = settings.url;
                pubstream = settings.pubstream;
            }
            public void LoadFromProfile(LSSettings settings)
            {
                settings.host = host;
                settings.port = port;
                settings.user = user;
                settings.pass = pass;
                settings.mount = mount;
                settings.relay = relay;

                settings.title = title;
                settings.description = description;
                settings.genre = genre;
                settings.url = url;
                settings.pubstream = pubstream;
            }
            public bool Matches(LSSettings settings)
            {
                return (
                    settings.host == host &&
                    settings.port == port &&
                    settings.user == user &&
                    settings.pass == pass &&
                    settings.mount == mount &&
                    settings.relay == relay &&

                    settings.title == title &&
                    settings.description == description &&
                    settings.genre == genre &&
                    settings.url == url &&
                    settings.pubstream == pubstream
                );
            }
            public override string ToString()
            {
                return presetName;
            }
        }

        public class LSPreset
        {
            public double vRec, vMic, vSpd, vOut;
            public bool bRec, bMic, bOut;
            public double xRec, xMic;

            public LSPreset()
            {
            }
            public LSPreset(double vrec, double vmic, double vspd, double vout, bool brec, bool bmic, bool bout, double xrec, double xmic)
            {
                vRec = vrec;
                vMic = vmic;
                vSpd = vspd;
                vOut = vout;
                bRec = brec;
                bMic = bmic;
                bOut = bout;
                xRec = xrec;
                xMic = xmic;
            }
            public void apply(LSPreset preset)
            {
                vRec = preset.vRec;
                vMic = preset.vMic;
                vOut = preset.vOut;
                vSpd = preset.vSpd;
                bRec = preset.bRec;
                bMic = preset.bMic;
                bOut = preset.bOut;
                xRec = preset.xRec;
                xMic = preset.xMic;
            }
        }

        public class LSMeta
        {
            public enum Reader { WindowCaption, File, Website, ProcessMemory };
            public string src, ptn, tit;
            [XmlIgnore]
            public Encoding enc;
            public string encoding { get { return enc.WebName; } set { enc = Encoding.GetEncoding(value); } }
            public Reader reader;
            public int freq, grp;
            public bool urldecode;

            public LSMeta()
            {
                src = ptn = tit = "";
                encoding = "utf-8";
                reader = Reader.WindowCaption;
                freq = 500;
                grp = 1;
            }
            public LSMeta(
                Reader r,
                string profileTitle,
                string dataSource,
                int pollingFrequency,
                string parserPattern,
                int keepGroup = 1,
                string textEncoding = "utf-8",
                bool urlDecode = false)
            {
                reader = r;
                tit = profileTitle;
                src = dataSource;
                ptn = parserPattern;
                freq = pollingFrequency;
                encoding = textEncoding;
                grp = keepGroup;
                urldecode = urlDecode;
            }
            public override string ToString()
            {
                return tit;
            }
            public void apply(LSMeta meta)
            {
                LSMeta.apply(meta, this);
            }
            public static LSMeta copy(LSMeta meta)
            {
                LSMeta ret = new LSMeta();
                apply(meta, ret);
                return ret;
            }
            public static void apply(LSMeta src, LSMeta target)
            {
                target.src = src.src;
                target.ptn = src.ptn;
                target.tit = src.tit;
                target.encoding = src.encoding;
                target.reader = src.reader;
                target.freq = src.freq;
                target.grp = src.grp;
                target.urldecode = src.urldecode;
            }
            public bool eq(LSMeta meta)
            {
                return
                    src == meta.src &&
                    ptn == meta.ptn &&
                    tit == meta.tit &&
                    encoding == meta.encoding &&
                    reader == meta.reader &&
                    freq == meta.freq &&
                    grp == meta.grp &&
                    urldecode == meta.urldecode;
            }
        }
        public List<LSMeta> metas;
        public LSMeta meta;
        public bool latin;
        public bool tagAuto;

        public enum LSCompression { cbr, q };
        public enum LSChannels { mono, stereo }
        public class LSParams
        {
            public bool enabled;
            public LSCompression compression;
            public long bitrate;
            public long quality;
            public LSChannels channels;
            public string ext;
            public double FIXME_kbps;
        }

        LSDevice _devRec, _devMic, _devOut;
        public string s_devRec, s_devMic, s_devOut;
        public bool micLeft, micRight;

        [XmlIgnore]
        public LSDevice[] devs;
        [XmlIgnore]
        public LSDevice devRec { get { return _devRec; } set { _devRec = value; s_devRec = value == null? "" : value.id; } }
        [XmlIgnore]
        public LSDevice devMic { get { return _devMic; } set { _devMic = value; s_devMic = value == null ? "" : value.id; } }
        [XmlIgnore]
        public LSDevice devOut { get { return _devOut; } set { _devOut = value; s_devOut = value == null ? "" : value.id; } }

        public LSPreset mixer;
        public LSPreset[] presets;
        public LSParams mp3, ogg;
        public int samplerate;

        public List<LSServerPreset> serverPresets;
        public enum LSRelay { ice, shout, siren }
        public int port;
        public LSRelay relay;
        public string host, user, pass, mount;
        public string title, description, genre, url;
        public bool pubstream;

        public bool testDevs;
        public bool splash;
        public bool vu;
        public bool recPCM;
        public bool recMp3;
        public bool recOgg;
        public bool showUnavail;
        public bool autoconn;
        public bool autohide;

        public bool warn_poor, warn_drop;
        public double lim_poor, lim_drop;

        public LSSettings()
        {
            s_devRec = s_devMic = s_devOut = null;
            micLeft = true;
            micRight = false;
            mp3 = new LSParams();
            mp3.enabled = true;
            mp3.compression = LSCompression.cbr;
            mp3.bitrate = 192;
            mp3.quality = 2;
            mp3.channels = LSChannels.stereo;
            mp3.ext = "mp3";
            ogg = new LSParams();
            ogg.enabled = false;
            ogg.compression = LSCompression.q;
            ogg.bitrate = 192;
            ogg.quality = 5;
            ogg.channels = LSChannels.stereo;
            ogg.ext = "ogg";
            samplerate = 44100;

            serverPresets = new List<LSServerPreset>();
            host = "become.stream.r-a-d.io";
            port = 1337;
            user = "source";
            pass = "hackme";
            mount = "main";
            relay = LSRelay.ice;
            title = "Loopstream";
            description = "Cave Explorer Committee";
            genre = "Post-Avant Jazzcore";
            url = "https://github.com/9001/loopstream";
            pubstream = false;

            latin = false;
            tagAuto = true;
            meta = new LSMeta();
            metas = new List<LSMeta>();

            resetPresets();
            //presets[0] = new LSPreset(1, 1, 0.32, 1, true, true, true); //DEBUG
            //presets[0] = new LSPreset(0.5, 0.875, 0.32, 0.75, true, true, true); //DEBUG
            mixer = new LSPreset();
            mixer.apply(presets[0]);
            testDevs = true;
            splash = true;
            vu = true;
            recMp3 = true;
            recOgg = true;
            recPCM = false;
            showUnavail = false;
            autoconn = false;
            autohide = false;

            warn_poor = warn_drop = true;
            lim_poor = 0.92;
            lim_drop = 0.78;
            init();
        }

        public void init()
        {
            List<LSDevice> ldev = new List<LSDevice>();
            NAudio.CoreAudioApi.MMDeviceEnumerator mde = new NAudio.CoreAudioApi.MMDeviceEnumerator();
            foreach (
                NAudio.CoreAudioApi.MMDevice device
                 in mde.EnumerateAudioEndPoints(
                    NAudio.CoreAudioApi.DataFlow.All,
                    NAudio.CoreAudioApi.DeviceState.All))
            {
                LSDevice add = new LSDevice();
                add.mm = device;
                add.isRec = device.DataFlow == NAudio.CoreAudioApi.DataFlow.Capture;
                add.isPlay = device.DataFlow == NAudio.CoreAudioApi.DataFlow.Render;
                if (device.DataFlow == NAudio.CoreAudioApi.DataFlow.All)
                {
                    add.isRec = add.isPlay = true;
                }
                add.name = device.ToString();
                add.id = device.ID;
                ldev.Add(add);
            }
            devs = ldev.ToArray();
            if (!string.IsNullOrEmpty(s_devRec)) devRec = getDevByID(s_devRec); // ?? devs.First(x => x.isPlay);
            if (!string.IsNullOrEmpty(s_devMic)) devMic = getDevByID(s_devMic);
            if (!string.IsNullOrEmpty(s_devOut)) devOut = getDevByID(s_devOut);
        }
        public void initWhenDeserializationFails()
        {
            if (metas.Count == 0)
            {
                metas.AddRange(new LSMeta[] {
                    new LSMeta(LSMeta.Reader.WindowCaption, "Foobar 2000", "foobar2000", 500,
                        @" *(.*[^ ]) *( - foobar2000$|\[foobar2000 v([0-9\.]*)\]$)"),
                    new LSMeta(LSMeta.Reader.ProcessMemory, "iTunes 64bit 11.0.4.4", "itunes", 500,
                        "iTunes.dll+15C4D52, iTunes.dll+15C4952", 1, "utf-16"),
                    new LSMeta(LSMeta.Reader.WindowCaption, "Mieda Player Classic HomeCinema", "mpc-hc", 500,
                        @" *(.*[^ ]) *"),
                    new LSMeta(LSMeta.Reader.WindowCaption, "VLC", "vlc", 500,
                        @" *(.*[^ ]) * - VLC media player"),
                    new LSMeta(LSMeta.Reader.WindowCaption, "Winamp", "winamp", 500,
                        @"([0-9]*\. )? *(.*[^ ]) * - Winamp$", 2),
                    new LSMeta(LSMeta.Reader.File, "------------------------------------------", "tag.txt", 1000, ""),

                    new LSMeta(LSMeta.Reader.ProcessMemory, "FamiTracker v0.4.2", "ft042", 1000,
                        "ft042.exe+156b98*8c*680, ft042.exe+156b98*8c*480", 1, "utf-16"),
                    //new LSMeta(LSMeta.Reader.ProcessMemory, "FamiTracker v0.4.2 (slimeball mod)", "ft042", 1000,
                        //"sunsoft.exe+156b98*8c*680, sunsoft.exe+156b98*8c*480", 1, "utf-16"),
                        //"c7400*44*200, c7400*44*0", 1, "utf-16"),
                    new LSMeta(LSMeta.Reader.WindowCaption, "Firefox - Soundcloud", "firefox", 500,
                        @"^▶  *(.*[^ ]) *- Mozilla Firefox"),
                    new LSMeta(LSMeta.Reader.WindowCaption, "Firefox - YouTube", "firefox", 500,
                        @"^▶  *(.*[^ ]) *- YouTube - Mozilla Firefox"),
                    new LSMeta(LSMeta.Reader.Website, "other icecast mount", "http://wessie.info:1130/", 2000,
                        "<tr>\\n<td><h3>Mount Point /main.mp3</h3></td>.*?<td>Current Song:</td>\\n<td class=\"streamdata\">(.*?)</td>"),
                    new LSMeta(LSMeta.Reader.Website, "NI Traktor  (requires Loopstream Plugin)", "http://localhost:42069/status2.xsl", 1000,
                        "\\n<pre>(.*)</pre>\\n", 1, "utf-8", true)
                });
            }
        }
        public void resetPresets()
        {
            presets = new LSPreset[] {
                new LSPreset(1.00, 0, 0.6, 1, true, true, false, 1, 1),
                new LSPreset(0.15, 1, 0.6, 1, true, true, false, 1, 4),
                new LSPreset(1.00, 0, 0.6, 1, true, true, true, 1, 1),
                new LSPreset(0.15, 1, 0.6, 1, true, true, true, 1, 4),
            };
        }
        public void runTests(Splesh splesh, bool forceTest)
        {
            Program.DBGLOG = "";
            if (testDevs || forceTest)
            {
                StringBuilder sw = new StringBuilder();
                for (int a = 0; a < devs.Length; a++)
                {
                    splesh.prog(a + 1, devs.Length);
                    //devs[a].test();
                    try
                    {
                        devs[a].test();
                        sw.AppendLine(devs[a].mm.ID);
                        sw.AppendLine(devs[a].mm.DeviceFriendlyName);
                        sw.AppendLine(devs[a].mm.FriendlyName);
                        try
                        {
                            sw.AppendLine(LSDevice.stringer(devs[a].wf));
                        }
                        catch
                        {
                            sw.AppendLine("*** bad wf ***");
                        }
                    }
                    catch
                    {
                        sw.AppendLine("*!* bad dev *!*");
                    }
                    sw.AppendLine();
                    sw.AppendLine("---");
                    sw.AppendLine();
                }
                Program.DBGLOG += sw.ToString();
            }
        }

        public LSDevice getDevByID(string id)
        {
            foreach (LSDevice dev in devs)
            {
                if (dev.id == id)
                {
                    return dev;
                }
            }
            return null;
        }

        static int version()
        {
            string[] str = System.Windows.Forms.Application.ProductVersion.Split('.');
            return
                (Convert.ToInt32(str[0]) << 0x18) +
                (Convert.ToInt32(str[1]) << 0x10) +
                (Convert.ToInt32(str[2]) << 0x8) +
                (Convert.ToInt32(str[3]));
        }
        public void save()
        {
            XmlSerializer x = new XmlSerializer(this.GetType());
            using (var s = new System.IO.FileStream("Loopstream.ini", System.IO.FileMode.Create))
            {
                byte[] ver = System.Text.Encoding.UTF8.GetBytes(version().ToString("x") + "\n");
                s.Write(ver, 0, ver.Length);
                x.Serialize(s, this);
            }
        }
        public static LSSettings load()
        {
            LSSettings ret;
            XmlSerializer x = new XmlSerializer(typeof(LSSettings));
            if (System.IO.File.Exists("Loopstream.ini"))
            {
                try
                {
                    string str = System.IO.File.ReadAllText("Loopstream.ini", Encoding.UTF8);
                    string ver = str.Substring(0, str.IndexOf('\n'));
                    str = str.Substring(ver.Length + 1);
                    System.IO.MemoryStream s = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(str));

                    int myVer = version();
                    int iniVer = Convert.ToInt32(ver, 16);
                    //if (myVer != iniVer)
                    if (iniVer < 0x01020600)
                    {
                        byte[] bver = BitConverter.GetBytes(iniVer);
                        Array.Reverse(bver);
                        System.Windows.Forms.MessageBox.Show(
                            "Your configuration file is from version " + BitConverter.ToString(bver) + ", which is rather old.\n\n" +
                            "Don't get your hopes up but I'll try to load it...",
                            "Incompatible config file",
                            System.Windows.Forms.MessageBoxButtons.OK,
                            System.Windows.Forms.MessageBoxIcon.Information);
                    }
                    //using (var s = System.IO.File.OpenRead("Loopstream.ini"))
                    {
                        ret = (LSSettings)x.Deserialize(s);

                        //
                        //  Upgrade from v1.2.8.0
                        //
                        if (ret.mixer.xRec < 1) ret.mixer.xRec = 1;
                        if (ret.mixer.xMic < 1) ret.mixer.xMic = 1;
                        foreach (LSSettings.LSPreset pre in ret.presets)
                        {
                            if (pre.xRec < 1) pre.xRec = 1;
                            if (pre.xMic < 1) pre.xMic = 1;
                        }
                        if (ret.lim_drop <= 0 || ret.lim_poor <= 0)
                        {
                            ret.warn_poor = ret.warn_drop = true;
                            ret.lim_poor = 0.9;
                            ret.lim_drop = 0.6;
                        }
                    }
                }
                catch (Exception e)
                {
                    ret = new LSSettings();
                    System.Windows.Forms.MessageBox.Show(
                        "Failed to load settings:\n«Loopstream.ini» is probably from an old version of the program.\n\nDetailed information:\n" + e.Message + "\n" + e.StackTrace,
                        "Default settings",
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Information);
                }
                try
                {
                    ret.init();
                    ret.mp3.FIXME_kbps =
                    ret.ogg.FIXME_kbps = -1;
                    return ret;
                }
                catch (Exception e)
                {
                    System.Windows.Forms.MessageBox.Show(
                        "Failed to initialize settings object:\nPossibly from an outdated version of «Loopstream.ini», though more likely a developer fuckup. Go tell ed this:\n\n" +
                        e.Message + "\n\n" + e.Source + "\n\n" + e.InnerException + "\n\n" + e.StackTrace);
                }
            }
            ret = new LSSettings();
            ret.initWhenDeserializationFails(); // it is 06:20 am, what are you looking at
            return ret;
        }
    }
}
