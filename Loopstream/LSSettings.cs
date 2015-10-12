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
        [XmlIgnore]
        public static LSSettings singleton;

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
            public class Yield
            {
                public class YieldMember
                {
                    public int bref;
                    public string text;
                    public YieldMember(int i)
                    {
                        bref = i;
                        text = null;
                    }
                    public YieldMember(string s)
                    {
                        text = s;
                        bref = -1;
                    }
                }
                string _base;
                public int max;
                List<YieldMember> mset;
                public Yield(string asdf)
                {
                    max = -1;
                    _base = asdf;
                    mset = new List<YieldMember>();
                    string strText = "";
                    string strBref = "";
                    foreach (char c in asdf)
                    {
                        strText += c;
                        if (c == '{')
                        {
                            strBref += c;
                        }
                        else if (strBref.Length != 0)
                        {
                            if (c >= '0' && c <= '9')
                            {
                                strBref += c;
                            }
                            else if (c == '}')
                            {
                                strBref += '}';
                                if (strBref.Length == 2)
                                {
                                    strBref = "";
                                }
                                else
                                {
                                    strText = strText.Substring(0, strText.Length - strBref.Length);
                                    if (!string.IsNullOrEmpty(strText))
                                    {
                                        mset.Add(new YieldMember(strText));
                                        strText = "";
                                    }
                                    int v = Convert.ToInt32(strBref.Trim('{', '}'));
                                    mset.Add(new YieldMember(v));
                                    max = Math.Max(max, v);
                                    strBref = "";
                                }
                            }
                            else
                            {
                                strBref = "";
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(strText))
                    {
                        mset.Add(new YieldMember(strText));
                        strText = "";
                    }
                }
                public override string ToString()
                {
                    return _base;
                }
                public string format(System.Text.RegularExpressions.GroupCollection r)
                {
                    //r[m.grp].Value.Trim(' ', '\t', '\r', '\n');
                    if (r.Count <= max) return null;
                    StringBuilder ret = new StringBuilder();
                    foreach (YieldMember ym in mset)
                    {
                        if (ym.bref < 0)
                        {
                            ret.Append(ym.text);
                        }
                        else
                        {
                            ret.Append(r[ym.bref].Value.Trim(' ', '\t', '\r', '\n'));
                        }
                    }
                    return ret.ToString();
                }
                public string assert()
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (YieldMember ym in mset)
                    {
                        sb.Append(ym.bref > -1 ? "[" + ym.bref + "]" : "(" + ym.text + ")");
                    }
                    return sb.ToString();
                }
            }
            public enum Reader { WindowCaption, File, Website, ProcessMemory };
            public string src, ptn, tit, desc;
            [XmlIgnore]
            public Encoding enc;
            public string encoding { get { return enc.WebName; } set { enc = Encoding.GetEncoding(value); } }
            [XmlIgnore]
            public Yield yi;
            public string yield { get { return yi.ToString(); } set { yi = new Yield(value); } }
            public Reader reader;
            public int freq, grp;
            public bool urldecode;

            public LSMeta()
            {
                reader = Reader.WindowCaption;
                src = ptn = tit = desc = "";
                encoding = "utf-8";
                urldecode = false;
                yield = "{1}";
                freq = 500;
                grp = 1;
            }
            public LSMeta(
                Reader r,
                string profileTitle,
                string dataSource,
                int pollingFrequency,
                string parserPattern,
                string parserYield = "{1}",
                string textEncoding = "utf-8",
                bool urlDecode = false,
                int keepGroup = 1,
                string description = "")
            {
                reader = r;
                tit = profileTitle;
                src = dataSource;
                ptn = parserPattern;
                yield = parserYield;
                freq = pollingFrequency;
                encoding = textEncoding;
                grp = keepGroup;
                urldecode = urlDecode;
                desc = description;
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
                target.yield = src.yield;
                target.encoding = src.encoding;
                target.reader = src.reader;
                target.freq = src.freq;
                target.grp = src.grp;
                target.urldecode = src.urldecode;
                target.desc = src.desc;
            }
            public bool eq(LSMeta meta)
            {
                return
                    src == meta.src &&
                    ptn == meta.ptn &&
                    tit == meta.tit &&
                    yield == meta.yield &&
                    encoding == meta.encoding &&
                    reader == meta.reader &&
                    freq == meta.freq &&
                    grp == meta.grp &&
                    urldecode == meta.urldecode;
                // description left out intentionally
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
            [XmlIgnore]
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
                resetMetas();
            }
        }
        public void resetMetas()
        {
            metas.Clear();
            metas.AddRange(new LSMeta[] {
                new LSMeta(
                    LSMeta.Reader.WindowCaption,
                    "Foobar 2000",
                    "foobar2000",
                    500,
                    @" *(.*[^ ]) *( - foobar2000$|\[foobar2000 v([0-9\.]*)\]$)",
                    "{1}",
                    "utf-8",
                    false,
                    1,
                    "H4sIAAAAAAAEAHWRMU8DMQyF9/sVbzmJSlBVjGyoLVKHAoIyVR3Mna+JmksqJ9fj/j1OQAgGtsj2e++zszM24iwcOWG0zkGYWkxhEDyE8E5yu1gstOPbMCLZ5PgaGI1tDIY4kHMTogljRDOIsE+4l2RjAvkWuzw+r6pNlw1Bwqqx/gjS6ZhCj2S4z4ZBYL+GGkP+yN8Ev2PRBekpJdWrwCb0NMEHpQ5y0pCd0R1KhpriZb182m7Xj6v16o9LzDZ4TYpH0r5tcIXOivLOUJCXwQ29j6URuQlams2rZ8ek5j2dGHHQiEwq3IcLgy8sUzJ5LetLdlfuBhV39qj27Fol1o7X2nkqOWeKSbUf1CQ33VXVvqZytxo3OOzrwlof/qnnd/fzOZ+am6aNwgEAAA=="
                ),
                new LSMeta(
                    LSMeta.Reader.ProcessMemory,
                    "iTunes 64bit 11.0.4.4  (BROKEN (PROBABLY))",
                    "itunes",
                    500,
                    "iTunes.dll+15C4D52, iTunes.dll+15C4952",
                    "{1}",
                    "utf-16",
                    false,
                    1,
                    "H4sIAAAAAAAEAE1RzU7DMAy+9ym+G0yaKpAm7pyA+3gAt3Mba1lcGpeyt8fuJkRPaeLv18ckFY/TrB11+bqDlnzFqvO5YhVLsMTgH+oN3zxX0YLn5/apPbQH6AA5LoXrHlRON2SMvxw6+Rtvm+bD+ZZ8Kg+Gc9F1D4g9VBQ1UIUM+IAUY6cw9VM1yvnODHYa0EhSWryrC9Qk5pz350TB404G6YUyVrqGrZHNpIx4nU2qbe6OYpn3qOpqiTapmavOFidCr5cps7Er0DosGRe2pKe2+SyDDy2FtreIN87sP07ba6n8tXDpGV6iJTJPhmo61a3CsOB3AapZxnRDJSqjA8o9ood5U3SZLozXyU3ABdFxgCPsPvAlIrnNzoN5IVOmK89Ng3/f49b/mXkKpMVafauDOGGVzWKUThhVT7HRyBtd+VDP1YeZw/DuFzHtZE8RAgAA"
                ),
                new LSMeta(
                    LSMeta.Reader.WindowCaption,
                    "Mieda Player Classic HomeCinema",
                    "mpc-hc",
                    500,
                    @" *(.*[^ ]) *",
                    "{1}",
                    "utf-8",
                    false,
                    1,
                    "H4sIAAAAAAAEAE2RvW7DMAyEdz3FIbOT7l2zdClQIH0BxqJjtfoxSDmO376U0wTxIvhE3n2kTvPEAg1pioxJyhDsrAXC5FFHBucahLGE7MuCyreKMuDz67j/OHbuZ9ZqtalcQ76A8gqdqGcF1a1bK0k13ZuPb41NrKFGPjjnvsegz9CeMq4sK5g0xBVnBvnmz74BLUV+jaKOoBhRzEcwRVpZtAOpzskIXLO3xGBUb/ccPFL7WcSGgRYjtVwfdOv3CHkrSOwD/Xs+520W787h/h1HyhfeqnenMkvPO2AIHD0GKQm7NPX7sTfRiFuVDWfbUGRK3D1cimx3TXvAHfjGaGvo7Nd2t5YZCxnt61O8Mlnj4Q8Cgn3ovAEAAA=="
                ),
                new LSMeta(
                    LSMeta.Reader.WindowCaption,
                    "Spotify",
                    "spotify",
                    500,
                    @"^Spotify - *(.*[^ ]) *– *(.*[^ ]) *$",
                    "{1} - {2}",
                    "utf-8",
                    false,
                    1,
                    "H4sIAAAAAAAEAGWQTWrDMBCF9z7FKy39MakgWRWyyqYXSHalgYk1tkRlyUjjuCYEeofesCepXBIaWm1Gw0hvvvfWXRBbj+gTJxDq3rnBajEwY2fYw/okTBqhztPITe8onmc7loFzXUWxSUBeY2PFsSo2xiZUwdcnWRdkUoikbUDiuOeYZqgik1jfQKhJcPaNcUttt7x+WsznSwBKqRTgWO4SavsOMSRXRYHT2a5P8I8o71X5ssXrA8qvj8/L9qbINIyORDhmPwna7q1mna1JgAwTUEeRhNGSVGYCamLou0wYPIOpMqhDvLS5cru+nRXnjxq7McPxv/gU8Gw9OTfOMGR9jg3/PJzWSmg432POftr5N171a/QwP2aPh8XxGwsP5XauAQAA"
                ),
                new LSMeta(
                    LSMeta.Reader.WindowCaption,
                    "VLC",
                    "vlc",
                    500,
                    @" *(.*[^ ]) * - VLC media player",
                    "{1}",
                    "utf-8",
                    false,
                    1,
                    "H4sIAAAAAAAEAGWRwWrDMAyG73mKn55ayPIOJZcdOtih7O7WciJw7CCrzfr2k2MGg/lkydKvT7/PKetMgsLLGgmr5MCRejxJXjXJ0Qk04+NzfHsfe2ysM3Ii3HiC5xBIKN1pGIbuOhOUvhWHr8uIhTw7rNG9SA7WFiNuBKElP8kjSF5gg0HJI4f9qqw2ucN+CpsqWMEFhghOhT3tdUfgBEvswepUSdLQdecCZ7qcJoOjRIG1t7KmXYk2Tj5vbQx8piZcAfal/lP3nbUmXHJeiwq5pa1RuyzyDdpNBcdsBlYhyyzVrfpSSMzEk5FdZ1vi7lI14FEoPCK2KrwJq+H+ml4QTKd9x1+OMvwAo4V1mqUBAAA="
                ),
                new LSMeta(
                    LSMeta.Reader.WindowCaption,
                    "Winamp",
                    "winamp",
                    500,
                    @"([0-9]*\. )? *(.*[^ ]) * - Winamp$",
                    "{1}",
                    "utf-8",
                    false,
                    2,
                    "H4sIAAAAAAAEAF2QzWrDMBCE736KweckUHoo5FboA5RSKD2u7VW8VD9GWtf123cth5ZEFyFm95sZfbKCYtKRM6acnHjGMko/IjMNBSYg8CCEydNqQ4vEIS1QUc8HoJttH44koxNFSJnRpzB56Ul5ODXNCzuaveJDIoXJxOjkMmdSSRFiDgll3IjmtHl4KQqOmlfEOXRmSVrFopQVydXH1d/LF5+bBvt5eHw64TnrRjjifRuxezdumuZ9nMuhbr+SKue4wyzDX3Px/lxpxyPeOKRvvoYocMn7tPCAbrXG7akFXE6h8jq+SIwSLzfx7kAUV5SJei4277aPojiAnCWpO+1t8haT9b1jVDb/KNq9VfufgQ124/4Lus7FPNsBAAA="
                ),
                new LSMeta(
                    LSMeta.Reader.File,
                    "---------------------------------------------------",
                    "tag.txt",
                    1000,
                    ""
                ),
                new LSMeta(
                    LSMeta.Reader.ProcessMemory,
                    "FamiTracker v0.4.2   [32bit]",
                    "ft042",
                    1000,
                    "ft042.exe+156b98*8c*680, ft042.exe+156b98*8c*480",
                    "{1}",
                    "utf-16",
                    false,
                    1,
                    "H4sIAAAAAAAEAJ1Sy26DMBC8+ytWOaYE0SqN6JFDI0VqpSrhBwxehCVjW7ZpyN93jUNaRWoP5Qaz89hhKw1cfHLdogCc+GAVgtHQmzMEA6NHCD3ChzMtev+Og3EXcMgFupzVEeEhoNPQSVQCpI8UAdwDh4YTmwvhiJlBZ5QyZ8KaC2HWSE08CI5LFZ0SnOc5Y5CeLhTbpxwnfHh83jUv5bps17uyyH6BtmXB2F46H2KKq21Uro714VRntJ1VXOqYAKMV6ZywNVrczdeH+u01n1NsNrCXNBArWCZMN78ORoxU1eoWZQUgNUEkZlNbi0RlLV5FeqpYYCsHrkCPQ0MNFFPaIVrP7GS0kI/U9c/vPnV77S+LqvQHk0MxlW2S+Y47Z8Jl/n+q1Pq9bDwB6JwZ/lBHOgpNi94sSMxhGOlayK1yQfrAvgADvUSJgAIAAA=="
                ),
                new LSMeta(
                    LSMeta.Reader.Website,
                    "other icecast mount",
                    "http://wessie.info:1130/",
                    2000,
                    "<tr>\\n<td><h3>Mount Point /[^<]*</h3></td>.*?<td>Current Song:</td>\\n<td class=\"streamdata\">(.*?)</td>",
                    "{1}",
                    "utf-8",
                    true,
                    1,
                    "H4sIAAAAAAAEAE2QsW7DMAxEd38FtyRA0A/o1rFApyIdMtIWbQuWRYOiYPjve0pSoIQmkXx3x2/hQD4LfZjH4sQ50C16EhpNV2IKMnJNTnGQgTFQnL0W2niSt677KTLWRKMamSQ+Yp6AUAANkya8Am5ap5m+VLfnF/buUuiTlqw7+uy08oKfrFQkF7l2ffVH41SIU9GHw164+kE6/kO9d3etNHCmoDTouiVxSQfMOFuQQGWO8D7+wfYmdmBl5+zwQa3O6MZCeAgbc3wQEALpDWMUM3hlUzgjV5ph+ma8OEJjp5dDcTQXs9gnubygrzoj5iqcr9RkT+3WMS80ia2c4UdrCkAQY7XBd4M8fAwC4c10Ml7L9em7HaHlufwCFqKqebUBAAA="
                ),
                new LSMeta(
                    LSMeta.Reader.Website,
                    "NI Traktor  (requires Loopstream Plugin)",
                    "http://localhost:42069/status2.xsl",
                    1000,
                    "\\n<pre>(.*)</pre>\\n",
                    "{1}",
                    "utf-8",
                    true,
                    1,
                    "H4sIAAAAAAAEAMtUSE7MK1FISs3JTC1LVajML1UoSczJTk1RyE1VyMwryVcoycgsBgCrdlFBJgAAAA=="
                ),
                new LSMeta(
                    LSMeta.Reader.ProcessMemory,
                    "NSFPlay v2.3.b3 - SynthesiaMOD v0.53   [32bit]",
                    "nsfplay",
                    1000,
                    "in_yansf.dll+47cc4*376, in_yansf.dll+47cc4*356",
                    "{1}",
                    "shift_jis",
                    false,
                    1,
                    "H4sIAAAAAAAEAG1SwW7CMAy99yusHaFUSGMg7cZhSEybNEHvk9u4YJEmVZxq7d/PFIoGmk+J/fye/ZK18xyPFOAr+JJEPqn2oYdAaCikAJZPBBusOQ9YnhRXtBHE8uEYbQ/CdWMpZMn6gOxSaDBGCg6kdxG7V9AoUAhqb1pLMAV2HBkt+KoSijCBxrPTFmhsK9dsksAl2H336KTKjLXTxaosF5Pn1TL9N/+yTJINB4nAAmhM0F0geljv8u0+T4G6xuqIZKAg638ypd9T6Z15wOfb/OMtGyaYzWDDClB7bghfDdfrPk9/J3nSubSodM3FypHkva2bM/VQuxKlZxoH6EYnlHjeDbtA0Ue6NedncVGHWE4qYLhErWo3xjvCYY3RzUfl0eR78Xmnbl7UoGrD8AvwqA8/tu/0DBgiS0x+AZrbiA0oAgAA"
                ),
                new LSMeta(
                    LSMeta.Reader.File,
                    "---------------------------------------------------",
                    "tag.txt",
                    1000,
                    ""
                ),
                new LSMeta(
                    LSMeta.Reader.WindowCaption,
                    "SoundCloud, YouTube, Spotify    ( Firefox )",
                    "*",
                    1000,
                    @"^▶  *(.*?[^ ]) *(- YouTube|- Spotify)? - (Mozilla Firefox|Nightly|Google Chrome)$",
                    "{1}",
                    "utf-8",
                    false,
                    1,
                    "H4sIAAAAAAAEADWQQU7EMAxF9znFPwAqe5YgsYPNDEKzdBpPY00aV4nTUk5PyoB39n+2nvxcdKtc6pPDUW/6LSkRXqXwVb/wiHeZoqXduU/2VYz/yZO2HF6StnDvL9rOzfNfuKjJtS85d45SUaO2FLBpuUEyeOWyY5Qytrka5ZEfAModEOukIdIqeYIpjMrEhplyo5T2AfjIXcI4DO5k3RWa0w7yiQ+8MAVYZNBosvYR+X66Km7My28wcxDCkmjncrgQKi9UyBj+/otukYNuww9HedJjGwEAAA=="
                ),
                new LSMeta(
                    LSMeta.Reader.WindowCaption,
                    "SoundCloud, YouTube, Spotify    ( Chrome )",
                    "*",
                    1000,
                    @"^ *(.*?[^ ]) *(- YouTube|- Spotify)? - (Google Chrome)$",
                    "{1}",
                    "utf-8",
                    false,
                    1,
                    "H4sIAAAAAAAEADWQsW7FMAhFd38FH1Ale4cOfUOnTn1S1ZHEJEF1jGVwokj9+JKXlsmAL5zLa5VdqepzgDPeROZEcFuqrBTCJw3KRv/dD2k53pK0eOVf0u5toL9mEePpCCFcathRodJI2dIBrUQ0ik9eKQlHzjPYQmAVIxtLxgQ/L6DHOkiCnW0BBC2E31SBR8lduC+ssHpBgQ14LaLKg8OawMTJ/KM0g4JNKUIPalKKv1TyrIA5wsaRRGFyOhjoRKiEsQvvPhS01ccoyU674EYPvrHVehlw6uOUHL7EPfd63mI8b9Hvi1vbfL8bxtH41OLQ/QJGhKpcWwEAAA=="
                ),
                new LSMeta(
                    LSMeta.Reader.WindowCaption,
                    "Google Music                               ( Chrome + Firefox )",
                    "*",
                    1000,
                    //@"^ *(.*?[^ ])  *- Google Play Music - (Mozilla Firefox|Nightly|Google Chrome)$",
                    @"^ *(.*?[^ ])( - (.*?[^ ]))?  *- Google Play Music - (Mozilla Firefox|Nightly|Google Chrome)$",
                    "{3} - {1}",
                    "utf-8",
                    false,
                    1,
                    "H4sIAAAAAAAEADWOsW7DMAxEd3/FbV0Cd++WFGinFBkCdJZlxiasiAZJw3W+vpKbHjjd3SN5UlmN1N4aVJ3lwSkFfLDSTX7wii8eRk/bX/wpMiTC+6hyp6b5ps7Y6R9+pufFODZVT2Mimg3LDB8JrqFnZ8mQGzriPKDnOBm6DfPiXo0re8G68oESjupsDs47vnLuZYXXRtscYxTtC5K2A/b8EtxJc63HMehA9UwpKcV9tY9sKBNwJ7O2bYFLomCEXvKLI4lMxRN9/ALIywW3GgEAAA=="
                ),
            });
            metaDec();
        }
        void metaEnc()
        {
            foreach (LSMeta meta in metas)
            {
                try
                {
                    meta.desc = Z.gze(meta.desc.Replace("\r", ""));
                }
                catch
                {
                    meta.desc = "(encoding failed)";
                }
            }
        }
        void metaDec()
        {
            foreach (LSMeta meta in metas)
            {
                try
                {
                    meta.desc = Z.gzd(meta.desc).Replace("\r", "").Replace("\n", "\r\n");
                }
                catch
                {
                    meta.desc = "(encoding failed)";
                }
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
                    if (splesh != null)
                    {
                        splesh.prog(a + 1, devs.Length);
                    }
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
            metaEnc();
            XmlSerializer x = new XmlSerializer(this.GetType());
            using (var s = new System.IO.FileStream("Loopstream.ini", System.IO.FileMode.Create))
            {
                byte[] ver = System.Text.Encoding.UTF8.GetBytes(version().ToString("x") + "\n");
                s.Write(ver, 0, ver.Length);
                x.Serialize(s, this);
            }
            metaDec();
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
                    foreach (var v in ret.metas)
                    {
                        if (v.grp != 1)
                        {
                            v.yield = "{" + v.grp + "}";
                            v.grp = 1;
                        }
                    }
                    try
                    {
                        ret.metaDec();
                    }
                    catch { }
                    LSSettings.singleton = ret;
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
            LSSettings.singleton = ret; // for exception handler
            return ret;
        }
    }
}
