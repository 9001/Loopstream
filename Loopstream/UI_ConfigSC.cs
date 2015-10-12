using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NAudio.CoreAudioApi;

namespace Loopstream
{
    public partial class ConfigSC : Form
    {
        public ConfigSC(LSSettings settings)
        {
            this.settings = settings;
            InitializeComponent();
        }

        Timer tPop;
        Control popTop;
        bool disregardEvents;
        public LSSettings settings; //, apply;

        private void button2_Click(object sender, EventArgs e)
        {
            apply(false);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*StringBuilder sb = new StringBuilder();
            sb.AppendLine(((LSDevice)gOutS.SelectedItem).id + "\t" + gOutS.SelectedItem);
            sb.AppendLine(((LSDevice)gOneS.SelectedItem).id + "\t" + gOneS.SelectedItem);
            sb.AppendLine(((LSDevice)gTwoS.SelectedItem).id + "\t" + gTwoS.SelectedItem);
            System.IO.File.WriteAllText("Loopstream.ini", sb.ToString());*/
            apply(true);
        }

        void apply(bool save)
        {
            unFX();
            if (settings.devRec == null ||
                settings.devMic == null ||
                settings.devOut == null)
            {
                MessageBox.Show("Sorry, you need to select all three audio devices.");
                return;
            }

            string smp = "";
            string fail = "";
            string dbg = "";
            try
            {
                if (settings.devRec.wf == null)
                {
                    settings.devRec.test();
                }
                NAudio.Wave.WaveFormat wf = settings.devRec.wf;
                if (wf.SampleRate != settings.samplerate)
                {
                    smp += "music=" + wf.SampleRate + ", ";
                }
                if (Program.debug)
                {
                    dbg += "music=" + LSDevice.stringer(wf) + "\r\n\r\n";
                }
            }
            catch
            {
                fail += "music, ";
            }
            if (settings.devMic.mm != null)
            {
                try
                {
                    if (settings.devMic.wf == null)
                    {
                        settings.devMic.test();
                    }
                    NAudio.Wave.WaveFormat wf = settings.devMic.wf;
                    if (wf.SampleRate != settings.samplerate)
                    {
                        smp += "mic=" + wf.SampleRate + ", ";
                    }
                    if (Program.debug)
                    {
                        dbg += "mic=" + LSDevice.stringer(wf) + "\r\n\r\n";
                    }
                }
                catch
                {
                    fail += "mic, ";
                }
            }
            if (!string.IsNullOrEmpty(fail))
            {
                MessageBox.Show("I am sorry to say that some devices (" + fail.Trim(',', ' ') + ") fail to open.\r\nThis will probably cause issues :(",
                    "Critical error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            if (!string.IsNullOrEmpty(smp))
            {
                if (DialogResult.Yes == MessageBox.Show("Hey, got some work for you!\r\n\r\n" +
                    "Some of your devices (" + smp.Trim(',', ' ') + ") have\r\na different samplerate than what you\r\nspecified in the settings (" + settings.samplerate + " Hz).\r\n\r\n" +
                    //"This will still work (I am applying a resampler), but there will be lower sound quality and more load on your computer.\r\n\r\n" +
                    //"If you want to keep things TopNotch™, please see the\n\"Soundcard Samplerate\" section in the readme.";
                    "You can't stream like this.\r\n\r\n" +
                    "Do you want to fix it now? I'll help you!",
                    "HEY! LISTEN!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
                {
                    samplerateTutorial();
                }
            }
            if (Program.debug)
            {
                if (DialogResult.No == MessageBox.Show("-!- DEBUG (no=clipboard)\r\n\r\n" + dbg, "DEBUG", MessageBoxButtons.YesNo, MessageBoxIcon.Information))
                {
                    Clipboard.Clear();
                    Clipboard.SetText(dbg);
                }
            }
            if (save)
            {
                settings.save();
            }
            this.Close();
        }

        string resolveBinary(string exe)
        {
            if (System.IO.File.Exists(exe))
            {
                return System.IO.Path.GetFullPath(exe);
            }
            var vals = Environment.GetEnvironmentVariable("PATH");
            foreach (string path in vals.Split(';'))
            {
                var fp = System.IO.Path.Combine(path, exe);
                if (System.IO.File.Exists(fp))
                {
                    return fp;
                }
            }
            return null;
        }

        void samplerateTutorial()
        {
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.StartInfo.Arguments = "shell32.dll,Control_RunDLL mmsys.cpl,,playback";
            proc.StartInfo.FileName = "rundll32.exe";
            proc.Start();
            MessageBox.Show("I have taken the liberty of opening\r\nyour soundcard control panel.\r\n\r\n" +
                "Here's what you'll do:\r\n\r\n" +
                "1. Rightclick the first playback device\r\n" +
                "2. Select «Properties»\r\n" +
                "3. Open the «Advanced» tab\r\n" +
                "4. Change «Default Format» to this:\r\n" +
                "        16 bit, " + settings.samplerate + " Hz (whatever)\r\n" +
                "5. Press OK\r\n\r\n" +
                "Now do that with all your playback devices,\r\nthen press OK on both the soundcard\r\ncontrol window and this message.");
            
            proc = new System.Diagnostics.Process();
            proc.StartInfo.Arguments = "shell32.dll,Control_RunDLL mmsys.cpl,,recording";
            proc.StartInfo.FileName = "rundll32.exe";
            proc.Start();
            MessageBox.Show("Sorry, but you're not finished just yet.\r\n" +
                "Don't worry, you're getting good at this!\r\n\r\n" +
                "1. Rightclick the first recording device\r\n" +
                "2. Select «Properties»\r\n" +
                "3. Open the «Advanced» tab\r\n" +
                "4. Change «Default Format» to this:\r\n" +
                "        2 channel, 16 bit, " + settings.samplerate + " Hz (whatever)\r\n" +
                "5. Press OK\r\n\r\n" +
                "==== if you can't see a «2 channel»\r\n" +
                "==== option, then «1 channel» is OK!\r\n\r\n" +
                "Same procedure like last year, do that on all\r\nof your recording devices, then hit OK.\r\n\r\n" +
                "Thanks, now you're ready to fly!");
        }

        private void ConfigSC_Load(object sender, EventArgs e)
        {
            this.Icon = Program.icon;
            disregardEvents = true;
            Timer t = new Timer();
            t.Tick += t_Tick;
            t.Interval = 100;
            t.Start();
        }
        void t_Tick(object sender, EventArgs e)
        {
            ((Timer)sender).Stop();
            populate(true, gOutS, gOneS);
            populate(false, gTwoS);
            LSDevice nil = new LSDevice();
            nil.name = "(disabled)";
            gTwoS.Items.Insert(0, nil);
            gOneS.SelectedItem = settings.devRec;
            gTwoS.SelectedItem = settings.devMic;
            gOutS.SelectedItem = settings.devOut;
            gLeft.Checked = settings.micLeft;
            gRight.Checked = settings.micRight;

            gMp3Enable.Checked = settings.mp3.enabled;
            gMp3Bitrate.Checked = settings.mp3.compression == LSSettings.LSCompression.cbr;
            gMp3Quality.Checked = settings.mp3.compression == LSSettings.LSCompression.q;
            gMp3BitrateV.Text = settings.mp3.bitrate.ToString();
            gMp3QualityV.Text = settings.mp3.quality.ToString();
            gMp3Mono.Checked = settings.mp3.channels == LSSettings.LSChannels.mono;
            gMp3Stereo.Checked = settings.mp3.channels == LSSettings.LSChannels.stereo;
            
            gOggEnable.Checked = settings.ogg.enabled;
            gOggBitrate.Checked = settings.ogg.compression == LSSettings.LSCompression.cbr;
            gOggQuality.Checked = settings.ogg.compression == LSSettings.LSCompression.q;
            gOggBitrateV.Text = settings.ogg.bitrate.ToString();
            gOggQualityV.Text = settings.ogg.quality.ToString();
            gOggMono.Checked = settings.ogg.channels == LSSettings.LSChannels.mono;
            gOggStereo.Checked = settings.ogg.channels == LSSettings.LSChannels.stereo;

            gHost.Text = settings.host + ":" + settings.port;
            gPass.Text = settings.pass;
            gMount.Text = settings.mount;
            gShout.Checked = settings.relay == LSSettings.LSRelay.shout;
            gIce.Checked = settings.relay == LSSettings.LSRelay.ice;
            gSiren.Checked = settings.relay == LSSettings.LSRelay.siren;
            gTestDevs.Checked = settings.testDevs;
            gUnavail.Checked = settings.showUnavail;
            gSplash.Checked = settings.splash;
            gRecPCM.Checked = settings.recPCM;
            gRecMP3.Checked = settings.recMp3;
            gRecOGG.Checked = settings.recOgg;
            gAutoconn.Checked = settings.autoconn;
            gAutohide.Checked = settings.autohide;

            disregardEvents = false;
            gMp3_Click(sender, e);

            tPop = new Timer();
            tPop.Interval = tt.InitialDelay;
            tPop.Tick += tPop_Tick;
        }

        void populate(bool playback, params ComboBox[] lb)
        {
            foreach (ComboBox l in lb)
            {
                l.Items.Clear();
            }
            foreach (LSDevice lsd in settings.devs)
            {
                if ((playback && lsd.isPlay) || (!playback && lsd.isRec))
                {
                    foreach (ComboBox l in lb)
                    {
                        if (lsd.wf != null || settings.showUnavail)
                        {
                            l.Items.Add(lsd);
                        }
                    }
                }
            }
        }

        int getValue(TextBox tb)
        {
            int v;
            if (Int32.TryParse(tb.Text, out v))
            {
                tb.BackColor = SystemColors.Window;
                tb.ForeColor = SystemColors.WindowText;
                return v;
            }
            else
            {
                tb.BackColor = Color.Red;
                tb.ForeColor = Color.Yellow;
                return -1;
            }
        }

        private void gHost_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string host = gHost.Text.Split(':')[0];
                int port = Convert.ToInt32(gHost.Text.Split(':')[1]);
                settings.host = host;
                settings.port = port;
                gHost.BackColor = SystemColors.Window;
                gHost.ForeColor = SystemColors.WindowText;
            }
            catch
            {
                gHost.BackColor = Color.Red;
                gHost.ForeColor = Color.Yellow;
            }
        }

        private void gPass_TextChanged(object sender, EventArgs e)
        {
            if (gPass.Text.Length > 0)
            {
                settings.pass = gPass.Text;
                gPass.BackColor = SystemColors.Window;
                gPass.ForeColor = SystemColors.WindowText;
            }
            else
            {
                gPass.BackColor = Color.Red;
                gPass.ForeColor = Color.Yellow;
            }
        }

        private void gMount_TextChanged(object sender, EventArgs e)
        {
            if (gMount.Text.Length > 0)
            {
                settings.mount = gMount.Text;
                gMount.BackColor = SystemColors.Window;
                gMount.ForeColor = SystemColors.WindowText;
            }
            else
            {
                gMount.BackColor = Color.Red;
                gMount.ForeColor = Color.Yellow;
            }
        }

        private void gShout_CheckedChanged(object sender, EventArgs e)
        {
            if (gShout.Checked) settings.relay = LSSettings.LSRelay.shout;
        }

        private void gIce_CheckedChanged(object sender, EventArgs e)
        {
            if (gIce.Checked) settings.relay = LSSettings.LSRelay.ice;
        }

        private void gSiren_CheckedChanged(object sender, EventArgs e)
        {
            if (gSiren.Checked) settings.relay = LSSettings.LSRelay.siren;
        }

        private void gOutS_SelectedIndexChanged(object sender, EventArgs e)
        {
            settings.devOut = (LSDevice)gOutS.SelectedItem;
            playFX(settings.devOut);
        }

        private void gOneS_SelectedIndexChanged(object sender, EventArgs e)
        {
            settings.devRec = (LSDevice)gOneS.SelectedItem;
            //if (Program.debug) MessageBox.Show(LSDevice.stringer(settings.devRec.wf));
            playFX(settings.devRec);
        }

        private void gTwoS_SelectedIndexChanged(object sender, EventArgs e)
        {
            settings.devMic = (LSDevice)gTwoS.SelectedItem;
            //if (Program.debug) MessageBox.Show(LSDevice.stringer(settings.devMic.wf));
        }

        private void gLeft_CheckedChanged(object sender, EventArgs e)
        {
            settings.micLeft = gLeft.Checked;
        }

        private void gRight_CheckedChanged(object sender, EventArgs e)
        {
            settings.micRight = gRight.Checked;
        }

        private void gTestDevs_CheckedChanged(object sender, EventArgs e)
        {
            settings.testDevs = gTestDevs.Checked;
        }
        
        private void gSplash_CheckedChanged(object sender, EventArgs e)
        {
            settings.splash = gSplash.Checked;
        }

        private void gRecPCM_CheckedChanged(object sender, EventArgs e)
        {
            settings.recPCM = gRecPCM.Checked;
        }

        private void gRecMP3_CheckedChanged(object sender, EventArgs e)
        {
            settings.recMp3 = gRecMP3.Checked;
        }

        private void gRecOGG_CheckedChanged(object sender, EventArgs e)
        {
            settings.recOgg = gRecOGG.Checked;
        }

        //NAudio.Wave.Mp3FileReader fx_mp3 = null;
        NAudio.Wave.WaveFileReader fx_wav = null;
        NAudio.Wave.WasapiOut fx_out = null;
        System.IO.Stream fx_stream = null;
        void playFX(LSDevice dev)
        {
            try
            {
                if (disregardEvents) return;
                unFX();
                fx_stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("Loopstream.res.sc.wav");
                //fx_mp3 = new NAudio.Wave.Mp3FileReader(fx_stream);
                fx_wav = new NAudio.Wave.WaveFileReader(fx_stream);
                fx_out = new NAudio.Wave.WasapiOut(dev.mm, NAudio.CoreAudioApi.AudioClientShareMode.Shared, false, 100);
                fx_out.Init(fx_wav);
                fx_out.Play();
            }
            catch { }
        }
        void unFX()
        {
            if (fx_wav != null)
            {
                fx_out.Stop();
                fx_out.Dispose();
                fx_wav.Dispose();
                fx_stream.Dispose();
            }
        }

        private void gMp3Enable_CheckedChanged(object sender, EventArgs e)
        {
            settings.mp3.enabled = gMp3Enable.Checked;
        }

        private void gMp3Bitrate_CheckedChanged(object sender, EventArgs e)
        {
            if (gMp3Bitrate.Checked) settings.mp3.compression = LSSettings.LSCompression.cbr;
        }

        private void gMp3Quality_CheckedChanged(object sender, EventArgs e)
        {
            if (gMp3Quality.Checked) settings.mp3.compression = LSSettings.LSCompression.q;
        }

        private void gMp3Mono_CheckedChanged(object sender, EventArgs e)
        {
            if (gMp3Mono.Checked) settings.mp3.channels = LSSettings.LSChannels.mono;
        }

        private void gMp3Stereo_CheckedChanged(object sender, EventArgs e)
        {
            if (gMp3Stereo.Checked) settings.mp3.channels = LSSettings.LSChannels.stereo;
        }

        private void gMp3BitrateV_TextChanged(object sender, EventArgs e)
        {
            int n = getValue(gMp3BitrateV);
            if (n >= 0) settings.mp3.bitrate = n;
        }

        private void gMp3QualityV_TextChanged(object sender, EventArgs e)
        {
            int n = getValue(gMp3QualityV);
            if (n >= 0) settings.mp3.quality = n;
        }

        private void gOggEnable_CheckedChanged(object sender, EventArgs e)
        {
            settings.ogg.enabled = gOggEnable.Checked;
        }

        private void gOggBitrate_CheckedChanged(object sender, EventArgs e)
        {
            if (gOggBitrate.Checked) settings.ogg.compression = LSSettings.LSCompression.cbr;
        }

        private void gOggQuality_CheckedChanged(object sender, EventArgs e)
        {
            if (gOggQuality.Checked) settings.ogg.compression = LSSettings.LSCompression.q;
        }

        private void gOggMono_CheckedChanged(object sender, EventArgs e)
        {
            if (gOggMono.Checked) settings.ogg.channels = LSSettings.LSChannels.mono;
        }

        private void gOggStereo_CheckedChanged(object sender, EventArgs e)
        {
            if (gOggStereo.Checked) settings.ogg.channels = LSSettings.LSChannels.stereo;
        }

        private void gOggBitrateV_TextChanged(object sender, EventArgs e)
        {
            int n = getValue(gOggBitrateV);
            if (n >= 0) settings.ogg.bitrate = n;
        }

        private void gOggQualityV_TextChanged(object sender, EventArgs e)
        {
            int n = getValue(gOggQualityV);
            if (n >= 0) settings.ogg.quality = n;
        }

        private void gMp3_Click(object sender, EventArgs e)
        {
            gOgg.ForeColor = SystemColors.ButtonShadow;
            gMp3.ForeColor = SystemColors.WindowText;
            pMp3.BringToFront();
        }

        private void gOgg_Click(object sender, EventArgs e)
        {
            gMp3.ForeColor = SystemColors.ButtonShadow;
            gOgg.ForeColor = SystemColors.WindowText;
            pOgg.BringToFront();
        }

        private void gUnavail_CheckedChanged(object sender, EventArgs e)
        {
            settings.showUnavail = gUnavail.Checked;
        }

        private void gAutoconn_CheckedChanged(object sender, EventArgs e)
        {
            settings.autoconn = gAutoconn.Checked;
        }

        private void gAutohide_CheckedChanged(object sender, EventArgs e)
        {
            settings.autohide = gAutohide.Checked;
        }

        private void gHost_MouseEnter(object sender, EventArgs e)
        {
            //tPop.Start();
            popTop = (Control)sender;
            tPop_Tick(sender, e);
        }

        private void gHost_MouseLeave(object sender, EventArgs e)
        {
            tt.Hide(gTwoS);
            tPop.Stop();
        }

        void tPop_Tick(object sender, EventArgs e)
        {
            tPop.Stop();
            // fucking hell microsoft how did you fuck THIS up
            tt.Show(tt.GetToolTip(popTop), gTwoS);
        }
    }
}
