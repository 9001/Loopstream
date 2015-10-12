using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/*
 * TODO before release:
 * 
 * Program.debug = false
 * DFC.CHECK_MD5 = true
 * LSSettings ver.check
 * assembly version
 * Loopstream.exe sign
 */

namespace Loopstream
{
    public partial class Home : Form
    {
        public Home()
        {
            invals = new Control[0];
            InitializeComponent();
            splash = new Splesh();
            splash.Show();
        }

        bool isPresetLoad;
        LSSettings settings;
        Splesh splash;
        Rectangle myBounds;
        LSMixer mixer;
        LSPcmFeed pcm;
        LSTag tag;
        bool popEn, popFilt;
        UI_Msg popPoor, popDrop;
        Control[] invals; //sorry
        string lqMessage; //sorry
        string daText = "Connect";
        string wincap;

        int iKonami = 0;
        Timer tKonami = null;
        Keys[] cKonami;

        private void Form1_Load(object sender, EventArgs e)
        {
            pMessage.Height = 64;
            myBounds = this.Bounds;
            this.Bounds = new Rectangle(0, -100, 0, 0);
            this.Icon = Program.icon;

            Timer t = new Timer();
            t.Tick += t_Tick;
            t.Interval = 100;
            t.Start();
        }

        int loads = 0;
        void t_Tick(object sender, EventArgs e)
        {
            ((Timer)sender).Stop();                                                                         /**/ Logger.app.a("timer triggered");
            wincap = this.Text + " v" + Application.ProductVersion;
            this.Text = wincap;                                                                             /**/ Logger.app.a("set window title");

            DFC.coreTest();                                                                                 /**/ Logger.app.a("dfc core ok");
            if (Directory.Exists(@"..\..\tools\"))
            {
                splash.vis();                                                                               /**/ Logger.app.a("dfc maker");
                new DFC().make(splash.pb);
                Program.kill();
            }
            if (Directory.Exists(Program.tools) &&
                !File.Exists(Program.tools + @"web\png\win95.png"))
            {
                Logger.app.a("outdated tools; deleting");
                Directory.Delete(Program.tools, true);
            }
            if (!Directory.Exists(Program.tools))
            {
                Logger.app.a("outdated tools; deleting");
                splash.vis();
                new DFC().extract(splash.pb);
            }
            Logger.app.a("extract sequence done");
            plowTheFields();                                                                                /**/ Logger.app.a("traktor test ok");
            splash.unvis();                                                                                 /**/ Logger.app.a("splash hidden");

            if (++loads > 1)
            {
                MessageBox.Show("Critical semantical error, load sequencye fucskdjflks");
                return;
            }
            settings = LSSettings.load();
            settings.runTests(splash, false);
            isPresetLoad = true;

            gMusic.valueChanged += gSlider_valueChanged;
            gMic.valueChanged += gSlider_valueChanged;
            gSpeed.valueChanged += gSlider_valueChanged;
            gOut.valueChanged += gSlider_valueChanged;
            mixerPresetChanged(sender, e);

            Program.ni = new NotifyIcon();
            NotifyIcon ni = Program.ni;
            ni.Icon = this.Icon;
            ni.Visible = true;
            ni.DoubleClick += ni_DoubleClick;
            MenuItem[] items = {
                new MenuItem("Hide", ni_DoubleClick),
                new MenuItem("Connect", gConnect_Click),
                new MenuItem("---"),
                new MenuItem("A (1st preset)", gPreset_Click),
                new MenuItem("B (2nd preset)", gPreset_Click),
                new MenuItem("C (3nd impact)", gPreset_Click),
                new MenuItem("D (4th preset)", gPreset_Click),
                new MenuItem("---"),
                new MenuItem("Exit", gExit_Click)
            };
            ni.ContextMenu = new ContextMenu(items);

            /*
             *  Not using ContextMenuStrip because
             *  it's an unresponsive little shit
             * 
            *ContextMenuStrip cm = new ContextMenuStrip();
            ni.ContextMenuStrip = cm;
            ToolStripItem iShow = new ToolStripLabel("Hide");
            ToolStripItem iConn = new ToolStripLabel("Connect");
            ToolStripItem iA = new ToolStripLabel("A (1st preset)");
            ToolStripItem iB = new ToolStripLabel("B (2nd preset)");
            ToolStripItem iC = new ToolStripLabel("C (3nd impact)");
            ToolStripItem iD = new ToolStripLabel("D (4th preset)");
            ToolStripItem iExit = new ToolStripLabel("Exit");
            iShow.Click += ni_DoubleClick;
            iConn.Click += gConnect_Click;
            iExit.Click += gExit_Click;
            iA.Click += gPreset_Click;
            iB.Click += gPreset_Click;
            iC.Click += gPreset_Click;
            iD.Click += gPreset_Click;
            cm.Items.Add(iShow);
            cm.Items.Add(iConn);
            cm.Items.Add(new ToolStripSeparator());
            cm.Items.Add(iA);
            cm.Items.Add(iB);
            cm.Items.Add(iC);
            cm.Items.Add(iD);
            cm.Items.Add(new ToolStripSeparator());
            cm.Items.Add(iExit);*/

            gA.preset = settings.presets[0];
            gB.preset = settings.presets[1];
            gC.preset = settings.presets[2];
            gD.preset = settings.presets[3];

            popPoor = popDrop = null;
            popEn = popFilt = false;

            gManualTags.Font = new System.Drawing.Font(gManualTags.Font.FontFamily, gManualTags.Font.SizeInPoints * 0.8f);
            gManualTags.Text = gManualTags.Text.ToUpper().Replace(" ", "  ");
            gManualTags.Location = new Point(
                (int)(pictureBox1.Left + (pictureBox1.Width - gManualTags.Width) / 1.85),
                (int)(pictureBox1.Top + (pictureBox1.Height - gManualTags.Height) / 1.9));

            this.Bounds = myBounds;
            splash.Focus();
            //splash.BringToFront();
            splash.fx = settings.splash;
            splash.gtfo();

            if (settings.autohide)
            {
                this.Visible = false;
            }
            if (settings.autoconn)
            {
                gConnect_Click(sender, e);
            }

            // please don't look
            invals = new Control[] {
                box_top_graden,
                box_bottom_graden,
                gMusic.giSlider,
                gMusic.graden1,
                gMusic.graden2,
                gMic.giSlider,
                gMic.graden1,
                gMic.graden2,
                gSpeed.giSlider,
                gSpeed.graden1,
                gSpeed.graden2,
                gOut.giSlider,
                gOut.graden1,
                gOut.graden2,
            };
            invalOnNext = false;
            lqMessage = null;

            iKonami = 0;
            tKonami = new Timer();
            cKonami = new Keys[] { Keys.Up, Keys.Up, Keys.Down, Keys.Down, Keys.Left, Keys.Right, Keys.Left, Keys.Right, Keys.B, Keys.A, Keys.Enter };
            tKonami.Tick += new EventHandler(tKonami_Tick);
            tKonami.Interval = 10;
            //tKonami.Start();

            //gSpeed.giSlider.A_LEVEL = 1;

            Timer tTitle = new Timer();
            tTitle.Tick += tTitle_Tick;
            tTitle.Interval = 200;
            tTitle.Start();
            showhide();
            
            hookskinner(this.Controls);
            //Program.popception();
        }

        double shake = 1;
        int dW, dH;
        double dX, dY;
        double dGravity = 1.05;
        double dBounce = 0.8;
        double dSpeedY = 0;
        double dSpeedX = 10;
        Rectangle dRect = Rectangle.Empty;
        bool inKonami = false;
        void tKonami_Tick(object sender, EventArgs e)
        {
            if (inKonami) return;
            inKonami = true;

            if (dRect.Width == 0)
            {
                dW = this.Width;
                dH = this.Height;
                dX = this.Left;
                dY = this.Top;
                dRect = Screen.FromControl(this).WorkingArea;
            }
            if (shake < 10)
            {
                dX += Program.rnd.Next((int)shake * 2 + 1) - (int)(shake+0.5);
                dY += Program.rnd.Next((int)shake * 2 + 1) - (int)(shake+0.5);
                this.Location = new Point((int)dX, (int)dY);
                shake += 0.05;
                if (shake >= 10)
                {
                    tKonami.Interval = 500;
                }
                inKonami = false;
                return;
            }
            tKonami.Interval = 10;
            //this.Left += Program.rnd.Next(5) - 2;
            //this.Top += Program.rnd.Next(5) - 2;
            
            //if (dSpeedX < 2) dSpeedX *= dGravity;
            //else dSpeedX *= dGravity * 0.93;
            if (dSpeedX > 5) dSpeedX -= dGravity / 30;

            dSpeedY += dGravity;
            dX += dSpeedX;
            dY += dSpeedY;
            if (dY + dH > dRect.Bottom)
            {
                dY = dRect.Bottom - dH;
                dSpeedY *= -dBounce;
            }
            if (dX > dRect.Right + dRect.Width * 0.1)
            {
                tKonami.Stop();
                dX = dRect.Left + (dRect.Width - dW) / 2;
                dY = dRect.Top + (dRect.Height - dH) / 2;

                dGravity = 1.05;
                dBounce = 0.8;
                dSpeedY = 0;
                dSpeedX = 10;
                shake = 1;
            }
            this.Location = new Point((int)dX, (int)dY);
            
            inKonami = false;
        }

        void hookskinner(System.Windows.Forms.Control.ControlCollection cc)
        {
            foreach (Control c in cc)
            {
                Type t = c.GetType();
                if (t == typeof(Panel) ||
                    t == typeof(Button) ||
                    t == typeof(Label))
                {
                    Skinner.add(c);
                }
                hookskinner(c.Controls);
            }
        }

        bool invalOnNext;
        void inval()
        {
            foreach (Control c in invals)
            {
                c.Invalidate();
            }
            /*foreach (Control c in controls)
            {
                if (c.GetType() == typeof(LLabel) ||
                    c.GetType() == typeof(Graden))
                {
                    c.Invalidate();
                }
                //this.InvokePaintBackground(c, new PaintEventArgs(c.CreateGraphics(), c.Bounds));
                //this.InvokePaint(c, new PaintEventArgs(c.CreateGraphics(), c.Bounds));
                inval(c.Controls);
            }*/
        }

        void ni_DoubleClick(object sender, EventArgs e)
        {
            this.Visible = !this.Visible;
            //Program.ni.ContextMenuStrip.Items[0].Text = this.Visible ? "Hide" : "Show";
            Program.ni.ContextMenu.MenuItems[0].Text = this.Visible ? "Hide" : "Show";
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F1))
            {
                System.Diagnostics.Process.Start("http://r-a-d.io/ed/loopstream");
                return true;
            }
            //this.Text = keyData.ToString();
            if (tKonami != null)
            {
                if (iKonami < cKonami.Length)
                {
                    if (keyData != cKonami[iKonami++])
                    {
                        iKonami = 0;
                    }
                    if (iKonami >= cKonami.Length)
                    {
                        tKonami.Start();
                        iKonami = 0;
                        return true;
                    }
                }
                if (keyData == Keys.Escape)
                {
                    //tKonami.Stop();
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        void gSlider_valueChanged(object sender, EventArgs e)
        {
            settings.mixer.vRec = gMusic.level / 255.0;
            settings.mixer.vMic = gMic.level / 255.0;
            settings.mixer.vSpd = gSpeed.level / 200.0;
            settings.mixer.vOut = gOut.level / 255.0;
            settings.mixer.bRec = gMusic.enabled;
            settings.mixer.bMic = gMic.enabled;
            settings.mixer.bOut = gOut.enabled;
            settings.mixer.xRec = gMusic.boost;
            settings.mixer.xMic = gMic.boost;

            if (mixer != null)
            {
                mixer.MuteChannel(LSMixer.Slider.Music, settings.mixer.bRec);
                mixer.MuteChannel(LSMixer.Slider.Mic, settings.mixer.bMic);
                mixer.MuteChannel(LSMixer.Slider.Out, settings.mixer.bOut);

                LSMixer.Slider sl = LSMixer.Slider.Out;
                if (sender == gSpeed) return;
                if (sender == gMusic) sl = LSMixer.Slider.Music;
                if (sender == gMic) sl = LSMixer.Slider.Mic;
                double dur = 0;

                Verter.EventType et = ((Verter)sender).eventType;

                if (et == Verter.EventType.slide)
                {
                    //dur = (float)(settings.mixer.vSpd);
                    dur = settings.mixer.vSpd;
                }
                if (et == Verter.EventType.mute)
                {
                    mixer.MuteChannel(sl, ((Verter)sender).enabled);
                }
                else if (et == Verter.EventType.boost)
                {
                    mixer.BoostChannel(sl, (float)((Verter)sender).boost);
                }
                else
                {
                    mixer.FadeVolume(sl, (float)(((Verter)sender).level / 255.0), dur);
                }
            }
        }

        void mixerPresetChanged(object sender, EventArgs e)
        {
            gMusic.level = (int)(255 * settings.mixer.vRec);
            gMic.level = (int)(255 * settings.mixer.vMic);
            gSpeed.level = (int)(200 * settings.mixer.vSpd);
            gOut.level = (int)(255 * settings.mixer.vOut);
            gMusic.enabled = settings.mixer.bRec;
            gMic.enabled = settings.mixer.bMic;
            gOut.enabled = settings.mixer.bOut;
            gMusic.boost = settings.mixer.xRec;
            gMic.boost = settings.mixer.xMic;

            // you should probably fix this
            if (mixer != null)
            {
                gMusic.eventType = Verter.EventType.slide;
                gMic.eventType = Verter.EventType.slide;
                gOut.eventType = Verter.EventType.slide;
                gSlider_valueChanged(gMusic, null);
                gSlider_valueChanged(gMic, null);
                gSlider_valueChanged(gOut, null);

                gMusic.eventType = Verter.EventType.boost;
                gMic.eventType = Verter.EventType.boost;
                gSlider_valueChanged(gMusic, null);
                gSlider_valueChanged(gMic, null);
            }
        }

        private void gConnect_Click(object sender, EventArgs e)
        {
            if (settings.devRec == null || settings.devOut == null)
            {
                if (DialogResult.OK == MessageBox.Show(
                    "Please take a minute to adjust your settings\n\n(soundcard and radio server)",
                    "Audio endpoint is null", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning))
                {
                    gSettings_Click(sender, e);

                    if (settings.devRec == null || settings.devOut == null)
                    {
                        MessageBox.Show("Config is still invalid.\n\nGiving up.", "Crit", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else return;
            }

            if (gConnect.Text == "Connect")
            {
                if (settings.testDevs && (
                    settings.devOut == null ||
                    settings.devRec == null ||
                    settings.devOut.wf == null ||
                    settings.devRec.wf == null || (
                    settings.devMic != null &&
                    settings.devMic.id != null &&
                    settings.devMic.wf == null)))
                {
                    // TODO: Fix devMic != null when disabled (deserializing bug?)

                    MessageBox.Show("The soundcard devices you selected have been disabled or removed." +
                        "\r\n\r\nPlease check your privilege...uh, settings before connecting.",
                        "oh snap nigga", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                Program.ni.ContextMenu.MenuItems[1].Text = "Disconnect";
                daText = "D I S C O N N E C T";
                gConnect.Text = daText;
                tag = new LSTag(settings);
                mixer = new LSMixer(settings, new LLabel[] { gMusic.giSlider, gMic.giSlider, gOut.giSlider });
                pcm = new LSPcmFeed(settings, mixer.lameOutlet);
                popEn = true;
            }
            else
            {
                Program.ni.ContextMenu.MenuItems[1].Text = "Connect";
                if (pMessage.Visible) gLowQ_Click(sender, e);
                popEn = false;

                daText = "disconnecting...";
                gConnect.Enabled = false;
                gConnect.Text = daText;
                Application.DoEvents();

                System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ThreadStart(discthread));
                //t.Priority = System.Threading.ThreadPriority.Highest;
                t.Name = "LS_DISC";
                t.Start();
            }
        }

        void discthread()
        {
            daText = "kill_tag";
            tag.Dispose();
            daText = "kill_pcm";
            pcm.Dispose(ref daText);
            daText = "kill_mixer";
            mixer.Dispose(ref daText);
            daText = "Connect";
        }

        private void gSettings_Click(object sender, EventArgs e)
        {
            this.Hide();
            new ConfigSC(settings).ShowDialog();
            showhide();
            this.Show();
        }

        private void gExit_Click(object sender, EventArgs e)
        {
            Program.kill();
        }

        private void gLoad_Click(object sender, EventArgs e)
        {
            //this.Text = gLoad.rightclick ? "right" : "left";
            if (gLoad.rightclick)
            {
                if (DialogResult.Yes == MessageBox.Show("Reset presets?", "hello", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    settings.resetPresets();
                    Pritch[] pritches = { gA, gB, gC, gD };
                    for (int a = 0; a < pritches.Length; a++)
                    {
                        pritches[a].preset = settings.presets[a];
                    }
                }
                return;
            }
            isPresetLoad = !isPresetLoad;
            if (isPresetLoad)
            {
                //MessageBox.Show("set gload load");
                gLoad.Mode = UC_Troggle.Modes.Load;
            }
            else
            {
                //MessageBox.Show("set gload save");
                gLoad.Mode = UC_Troggle.Modes.Save;
            }
        }

        private void gPreset_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(sender.ToString());

            int preset = sender.GetType() == typeof(Pritch) ?
                ((Pritch)sender).Text[0] - 'A' :
                ((MenuItem)sender).Text[0] - 'A';

            if (isPresetLoad)
            {
                settings.mixer.apply(settings.presets[preset]);
                mixerPresetChanged(sender, e);
            }
            else
            {
                Pritch[] pritches = { gA, gB, gC, gD };
                settings.presets[preset].apply(settings.mixer);
                pritches[preset].preset = settings.presets[preset];
                gLoad_Click(sender, e);
            }
        }
        private void gPreset_MouseClick(object sender, MouseEventArgs e)
        {
            gPreset_Click(sender, null);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            gExit_Click(sender, e);
        }

        private void label3_Click(object sender, EventArgs e)
        {
            new UI_Status(settings).Show();
        }
        void helloworld(object sender, EventArgs e)
        {
            if (settings.devRec == null || settings.devRec.mm == null)
            {
                MessageBox.Show("I'm about to open the settings window.\n\n" +
                    "In the second dropdown (input Music), please select the speakers output you use when listening to music.\n\nPress apply when done.");
                gSettings_Click(sender, e);

                if (settings.devRec == null || settings.devRec.mm == null)
                {
                    MessageBox.Show("Sorry, but the settings are still invalid. Please try again or something.");
                    return;
                }
            }
            string app, str = "";
            app = "CAPTURE_ENDPOINT = " + settings.devRec.mm.ToString() + "\r\n";
            File.AppendAllText("Loopstream.log", app);
            str += app;

            wdt_waveIn = new NAudio.Wave.WasapiLoopbackCapture(settings.devRec.mm);

            app = "WASAPI_FMT = " + LSDevice.stringer(wdt_waveIn) + "\r\n";
            File.AppendAllText("Loopstream.log", app);
            str += app;

            Clipboard.Clear();
            Clipboard.SetText(str);
            MessageBox.Show(
                "Capture will begin when you press OK; please open a media player and start listening to some music.\n\n" +
                "While you wait, the following text is on your clipboard... Paste it in irc (Ctrl-V)\n\n" + str);

            wdt_v = File.OpenWrite("Loopstream.raw");
            wdt_writer = new NAudio.Wave.WaveFileWriter("Loopstream.wav", wdt_waveIn.WaveFormat);
            wdt_waveIn.DataAvailable += wdt_OnDataAvailable;
            wdt_waveIn.StartRecording();
            while (true)
            {
                if (val < 0) break;
                gMic.level = val;
                Application.DoEvents();
                System.Threading.Thread.Sleep(110);
                val += 2;
            }
            gMic.level = 0;
            if (DialogResult.Yes == MessageBox.Show("Test finished! The soundclip has been recorded to Loopstream.wav in the " +
                    "same folder as this .exe, more specifically here:\n\n" + Application.StartupPath + "\n\n" +
                    "Could you uploading this to pomf.se? Thanks!", "Open browser?", MessageBoxButtons.YesNo))
            {
                System.Diagnostics.Process.Start("http://pomf.se/");
            }
        }
        int val = 0;
        NAudio.Wave.IWaveIn wdt_waveIn;
        NAudio.Wave.WaveFileWriter wdt_writer;
        Stream wdt_v;

        void wdt_OnDataAvailable(object sender, NAudio.Wave.WaveInEventArgs e)
        {
            //Console.Write('.');
            wdt_v.Write(e.Buffer, 0, e.BytesRecorded);
            wdt_writer.Write(e.Buffer, 0, e.BytesRecorded);
            double secondsRecorded = wdt_writer.Length / wdt_writer.WaveFormat.AverageBytesPerSecond;
            int nval = (int)(secondsRecorded * 25.5);
            if (nval > val) val = nval;
            if (secondsRecorded >= 10)
            {
                val = -100;
                wdt_waveIn.StopRecording();
            }
        }

        private void Home_Move(object sender, EventArgs e)
        {
            if (!Screen.AllScreens.Any(s => s.WorkingArea.Contains(this.Bounds)))
            {
                invalOnNext = true;
            }
            else if (invalOnNext)
            {
                invalOnNext = false;
                inval();
            }
            if (invalOnNext)
            {
                inval();
            }
        }

        private void gGit_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://github.com/9001/loopstream");
        }

        void tTitle_Tick(object sender, EventArgs e)
        {
            if (mixer != null && mixer.isLQ != null)
            {
                lqMessage = mixer.isLQ;
                mixer.isLQ = null;
                this.Height += 64;
                pMessage.Visible = true;
            }
            if (tag == null) return;

            if (settings.mp3.FIXME_kbps <= 1 && settings.ogg.FIXME_kbps <= 1)
            {
                if (daText == "Connect")
                {
                    this.Text = wincap;
                }
                else
                {
                    this.Text = "- C O N N E C T I N G -";
                }
            }
            else
            {
                this.Text = string.Format("{0:0.00} // {1:0.00} // {2}",
                    Math.Round(settings.mp3.FIXME_kbps, 2),
                    Math.Round(settings.ogg.FIXME_kbps, 2),
                    tag.tag.tag);
            }
            
            if (settings.tagAuto)
            {
                gTag.Text = tag.tag.tag;
            }
            
            gConnect.Text = daText;
            if (daText == "Connect")
            {
                gConnect.Enabled = true;
            }

            if (settings.mixer.xRec < gMusic.boost) gMusic.boost = settings.mixer.xRec;
            if (settings.mixer.xMic < gMic.boost) gMic.boost = settings.mixer.xMic;

            double f, f_mp3, f_ogg;
            f = f_mp3 = f_ogg = -1;
            if (settings.mp3.FIXME_kbps >= 0)
            {
                f_mp3 = settings.mp3.FIXME_kbps * 1.0 / settings.mp3.bitrate;
            }
            if (settings.ogg.FIXME_kbps >= 0)
            {
                f_ogg = settings.ogg.FIXME_kbps * 1.0 / 24;
            }
            // TODO: ogg
            f = f_mp3 >= 0 ? f_mp3 : f_ogg;
            lock (Logger.bitratem)
            {
                Logger.bitratem.Add(Math.Max(settings.mp3.FIXME_kbps, 0));
            }
            lock (Logger.bitrateo)
            {
                Logger.bitrateo.Add(Math.Max(settings.ogg.FIXME_kbps, 0));
            }
            if (popEn && popFilt)
            {
                if (settings.warn_drop && settings.lim_drop > f)
                {
                    if (popPoor != null && popPoor.sactive) { popPoor.Dispose(); popPoor = null; }
                    if (popDrop == null || !popDrop.sactive)
                    {
                        popDrop = new UI_Msg("drop", "");
                        popDrop.Show();
                    }
                }
                else if (settings.warn_poor && settings.lim_poor > f)
                {
                    if (popDrop != null && popDrop.sactive) { popDrop.Dispose(); popDrop = null; }
                    if (popPoor == null || !popPoor.sactive)
                    {
                        popPoor = new UI_Msg("poor", "");
                        popPoor.Show();
                    }
                }
                else
                {
                    if (popPoor != null && popPoor.sactive) { popPoor.Dispose(); popPoor = null; }
                    if (popDrop != null && popDrop.sactive) { popDrop.Dispose(); popDrop = null; }
                }
            }
            else
            {
                if (f >= Math.Max(settings.lim_poor, settings.lim_drop))
                {
                    popFilt = popEn;
                }
                if (popPoor != null && popPoor.sactive) { popPoor.Dispose(); popPoor = null; }
                if (popDrop != null && popDrop.sactive) { popDrop.Dispose(); popDrop = null; }
            }
        }

        long lastclick = 0;
        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                long time = DateTime.UtcNow.Ticks / 10000;
                if (time < lastclick + 1000)
                {
                    try
                    {
                        Program.popception();
                    }
                    catch (Exception ex)
                    {
                        throw ex; // UNCOMMENT for production
                        SerializableException se = new SerializableException(ex);
                        GeneralInfo gi = new GeneralInfo(se, ex);
                        string serialized = gi.ToString();
                    }

                    Clipboard.Clear();
                    Application.DoEvents();
                    Clipboard.SetText(Program.DBGLOG);
                    Application.DoEvents();
                    MessageBox.Show("Debug information placed on clipboard\n\nGo to pastebin and Ctrl-V");
                }
                lastclick = time;
            }
        }

        private void gLowQ_Click(object sender, EventArgs e)
        {
            pMessage.Visible = false;
            this.Height -= 64;
            if (sender == gLowQ) MessageBox.Show(lqMessage);
            lqMessage = null;
        }

        private void gTag_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                Application.DoEvents();
                gTag.Text = gTag.Text.Replace("\r", "").Replace("\n", "");
                gTag.SelectionStart = gTag.Text.Length;
                gTag.SelectionLength = 0;
                if (tag != null) tag.set(gTag.Text);
            }
        }

        bool tagvis = false;
        void showhide()
        {
            if (!settings.tagAuto && !tagvis)
            {
                this.SuspendLayout();
                tagvis = pTag.Visible = true;
                this.Height += pTag.Height;
                this.ResumeLayout();
            }
            else if (settings.tagAuto && tagvis)
            {
                this.SuspendLayout();
                this.Height -= pTag.Height;
                tagvis = pTag.Visible = false;
                this.ResumeLayout();
            }
            gManualTags.Checked = !settings.tagAuto;
        }

        private void gTagRead_Click(object sender, EventArgs e)
        {
            try
            {
                //gTag.Text = tag.tag.tag;
                gTag.Text = LSTag.get(settings.meta, false).tag;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Can't let you do that, Dave.\n(please start streaming first)\n\n" + ex.Message + "\n" + ex.StackTrace);
            }
        }

        private void gTagSend_Click(object sender, EventArgs e)
        {
            try
            {
                tag.set(gTag.Text);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Can't let you do that, Dave.\n(please start streaming first)\n\n" + ex.Message + "\n" + ex.StackTrace);
            }
        }

        void plowTheFields()
        {
            string wd = Application.ExecutablePath;
            string ice = Application.ExecutablePath;
            wd = wd.Substring(0, wd.Replace('\\', '/').LastIndexOf('/') + 1);
            ice = ice.Substring(0, ice.LastIndexOf('.')) + "Traktor.exe";
            if (System.IO.File.Exists(ice))
            {
                ice = ice.Substring(wd.Length);
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo = new System.Diagnostics.ProcessStartInfo(ice, "doit");
                proc.StartInfo.WorkingDirectory = wd;
                proc.Start();
                proc.WaitForExit();
            }
        }

        private void gManualTags_CheckedChanged(object sender, EventArgs e)
        {
            settings.tagAuto = !gManualTags.Checked;
            showhide();
        }
    }
}