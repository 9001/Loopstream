using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LoopStream
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
        public long samplerate;
        public string host;
        public int port;
        public string pass;
        public string mount;
        public enum LSRelay { ice, shout, siren }
        public LSRelay relay;

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
            ogg.enabled = true;
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
            presets = new LSPreset[] {
                new LSPreset(1.00, 1, 0.6, 1, true, false, false),
                new LSPreset(0.25, 1, 0.6, 1, true, true, false),
                new LSPreset(1.00, 1, 0.6, 1, true, false, true),
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

            if (testDevs)
            {
                foreach (LSDevice dev in devs)
                {
                    dev.test();
                }
                /*using (System.IO.StreamWriter sw = new System.IO.StreamWriter("LoopStream.devs", false, Encoding.UTF8))
                {
                    foreach (LSDevice dev in devs)
                    {
                        try
                        {
                            dev.test();
                            sw.WriteLine(dev.mm.ID);
                            sw.WriteLine(dev.mm.DeviceFriendlyName);
                            sw.WriteLine(dev.mm.FriendlyName);
                            try
                            {
                                sw.WriteLine(LSDevice.stringer(dev.wf));
                            }
                            catch
                            {
                                sw.WriteLine("*** bad wf ***");
                            }
                        }
                        catch
                        {
                            sw.WriteLine("*!* bad dev *!*");
                        }
                        sw.WriteLine();
                        sw.WriteLine("---");
                        sw.WriteLine();
                    }
                }*/
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
            using (var s = System.IO.File.OpenWrite("LoopStream.ini"))
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
            if (System.IO.File.Exists("LoopStream.ini"))
            {
                try
                {
                    string str = System.IO.File.ReadAllText("LoopStream.ini", Encoding.UTF8);
                    string ver = str.Substring(0, str.IndexOf('\n'));
                    str = str.Substring(ver.Length + 1);
                    if (str.EndsWith(">>"))
                    {
                        str = str.Substring(0, str.Length - 1);
                    }
                    System.IO.MemoryStream s = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(str));

                    // FIXED LOL



                    int myVer = version();
                    int iniVer = Convert.ToInt32(ver, 16);
                    if (myVer != iniVer && false)
                    {
                        System.Windows.Forms.MessageBox.Show(
                            "Your configuration file is from version " + iniVer + ", which is too old.\r\n\r\n" +
                            "Starting with default config.",
                            "Incompatible config file",
                            System.Windows.Forms.MessageBoxButtons.OK,
                            System.Windows.Forms.MessageBoxIcon.Information);
                    }
                    //using (var s = System.IO.File.OpenRead("LoopStream.ini"))
                    {
                        ret = (LSSettings)x.Deserialize(s);
                    }
                }
                catch (Exception e)
                {
                    ret = new LSSettings();
                    System.Windows.Forms.MessageBox.Show(
                        "Failed to load settings:\r\n«LoopStream.ini» is probably from an old version of the program.\r\n\r\nDetailed information:\r\n" + e.Message + "\r\n" + e.StackTrace,
                        "Default settings",
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Information);
                }
                try
                {
                    ret.init();
                }
                catch (Exception e)
                {
                    System.Windows.Forms.MessageBox.Show(
                        "Failed to initialize settings object:\r\nPossibly from an outdated version of «LoopStream.ini», though more likely a developer fuckup. Go tell ed this:\r\n\r\n" +
                        e.Message + "\r\n\r\n" + e.Source + "\r\n\r\n" + e.InnerException + "\r\n\r\n" + e.StackTrace);
                }
            }
            else
            {
                ret = new LSSettings();
            }
            return ret;
        }
    }
}
