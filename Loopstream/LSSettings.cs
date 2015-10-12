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
        public class LSPreset
        {
            public double vRec, vMic, vSpd, vOut;
            public bool bRec, bMic, bOut;

            public LSPreset()
            {
            }
            public LSPreset(double srec, double smic, double sspd, double sout, bool erec, bool emic, bool eout)
            {
                vRec = srec;
                vMic = smic;
                vSpd = sspd;
                vOut = sout;
                bRec = erec;
                bMic = emic;
                bOut = eout;
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

            public LSMeta()
            {
                src = ptn = tit = "";
                encoding = "utf-8";
                reader = Reader.WindowCaption;
                freq = 500;
                grp = 1;
            }
            public LSMeta(Reader r, string ti, string sr, int fr, string pt, int grp = 1, string enc = "utf-8")
            {
                reader = r;
                tit = ti;
                src = sr;
                ptn = pt;
                freq = fr;
                encoding = enc;
                this.grp = grp;
            }
            public override string ToString()
            {
                return tit;
            }
            public void apply(LSMeta meta)
            {
                src = meta.src;
                ptn = meta.ptn;
                tit = meta.tit;
                encoding = meta.encoding;
                reader = meta.reader;
                freq = meta.freq;
                grp = meta.grp;
            }
        }
        public List<LSMeta> metas;
        public LSMeta meta;
        public bool latin;

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
        public LSDevice devRec { get { return _devRec; } set { _devRec = value; s_devRec = value.id; } }
        [XmlIgnore]
        public LSDevice devMic { get { return _devMic; } set { _devMic = value; s_devMic = value.id; } }
        [XmlIgnore]
        public LSDevice devOut { get { return _devOut; } set { _devOut = value; s_devOut = value.id; } }

        public LSPreset mixer;
        public LSPreset[] presets;

        public LSParams mp3, ogg;
        public int samplerate;
        public string host;
        public int port;
        public string pass;
        public string mount;
        public enum LSRelay { ice, shout, siren }
        public LSRelay relay;

        public string title, description, genre, url;
        public bool pubstream;

        public bool testDevs;
        public bool splash;
        public bool recPCM;
        public bool recMp3;
        public bool recOgg;
        public bool showUnavail;
        public bool autoconn;
        public bool autohide;

        public LSSettings()
        {
            s_devRec = s_devMic = s_devOut = null;
            micLeft = false;
            micRight = true;
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
            
            host = "stream0.r-a-d.io";
            port = 1337;
            pass = "user|assword";
            mount = "main";
            relay = LSRelay.ice;
            title = "Loopstream";
            description = "Cave Explorer Committee";
            genre = "Post-Avant Jazzcore";
            url = "https://github.com/9001/loopstream";
            pubstream = false;

            latin = false;
            meta = new LSMeta();
            metas = new List<LSMeta>();

            presets = new LSPreset[] {
                new LSPreset(1.00, 0, 0.6, 1, true, true, false),
                new LSPreset(0.25, 1, 0.6, 1, true, true, false),
                new LSPreset(1.00, 0, 0.6, 1, true, true, true),
                new LSPreset(0.25, 1, 0.6, 1, true, true, true),
            };
            //presets[0] = new LSPreset(1, 1, 0.32, 1, true, true, true); //DEBUG
            //presets[0] = new LSPreset(0.5, 0.875, 0.32, 0.75, true, true, true); //DEBUG
            mixer = new LSPreset();
            mixer.apply(presets[0]);
            testDevs = true;
            splash = true;
            recMp3 = true;
            recOgg = true;
            recPCM = false;
            showUnavail = false;
            autoconn = false;
            autohide = false;
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
                add.name = device.ToString();
                add.id = device.ID;
                ldev.Add(add);
            }
            devs = ldev.ToArray();
            if (!string.IsNullOrEmpty(s_devRec)) devRec = getDevByID(s_devRec);
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
                    new LSMeta(LSMeta.Reader.WindowCaption, "Winamp", "winamp", 500,
                        @"([0-9]*\. )? *(.*[^ ]) * - Winamp$", 2),
                    new LSMeta(LSMeta.Reader.WindowCaption, "VLC", "vlc", 500,
                        @" *(.*[^ ]) * - VLC media player"),
                    new LSMeta(LSMeta.Reader.ProcessMemory, "iTunes 64bit 11.0.4.4", "itunes", 500,
                        "iTunes.dll+15C4D52, iTunes.dll+15C4952", 1, "utf-16"),
                    new LSMeta(LSMeta.Reader.Website, "other icecast mount", "http://stream0.r-a-d.io:8000/", 2000,
                        "<tr>\\n<td><h3>Mount Point /main.mp3</h3></td>.*?<td>Current Song:</td>\\n<td class=\"streamdata\">(.*?)</td>"),
                });
            }
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
                    if (myVer != iniVer)
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
