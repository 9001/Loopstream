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
 * Program.beta = 0
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
        bool assumeConnected, popFilt;
        UI_Msg popPoor, popDrop, popSign, popQuit;
        Control[] invals; //sorry
        string lqMessage; //sorry
        string daText = "Connect";
        string wincap;

        int iKonami = 0;
        Timer tKonami = null;
        Keys[] cKonami;
        List<SFXNode> sfxes;

        private void Form1_Load(object sender, EventArgs e)
        {
            pMessage.Height = 32;
            myBounds = this.Bounds;
            this.Bounds = new Rectangle(0, -100, 0, 0);
            this.Icon = Program.icon;

            Timer t = new Timer();
            t.Tick += t_Tick;
            t.Interval = 100;
            t.Start();
        }

        int loads = 0;
        string z(string str) { Logger.app.a(str); return str; }
        void t_Tick(object sender, EventArgs e)
        {
            ((Timer)sender).Stop();
            
            wincap = this.Text + " v" + Application.ProductVersion;
            if (Program.beta > 0)
                wincap += "  beta-" + Program.beta;

            this.Text = wincap;
            z("set window title");

            DFC.coreTest();
            z("dfc core ok");
            string toolsBase = @"..\..\tools\";
            string[] requiredFiles = {
                "opusenc.exe",
                "oggenc2.exe",
                "lame.exe",
                "lame_enc.dll"
            };
            if (Directory.Exists(toolsBase) &&
                File.Exists(toolsBase + Program.toolsVer))
            {
                if (DialogResult.Yes == MessageBox.Show(
                    "make .dfc (decent file container) ?\r\n\r\nhint: rename the tools folder\r\n         if you don't wanna see this",
                    "new embedded archive",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question))
                {
                    splash.vis();
                    z("dfc maker");
                    foreach (string filename in requiredFiles)
                    {
                        if (!File.Exists(toolsBase + filename))
                        {
                            DialogResult reply = MessageBox.Show(
                                "The following file is required in order to build a\r\n" +
                                "working DFC. Would you like to browse for it?\r\n\r\n" + filename,
                                "Missing dependency", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                            if (reply == System.Windows.Forms.DialogResult.Cancel)
                                Program.kill();

                            if (reply == System.Windows.Forms.DialogResult.No)
                                continue;

                            string ext = filename.Substring(filename.LastIndexOf(".") + 1);

                            OpenFileDialog ofd = new OpenFileDialog();
                            ofd.CheckFileExists = true;
                            ofd.Title = "I am hungry for " + filename;
                            ofd.Filter = ext + " files|*." + ext;
                            ofd.FileName =
                                filename.Contains("lame") ? "*lame*" :
                                filename.Contains("ogg") ? "*ogg*" :
                                filename.Contains("opus") ? "*opus*" : "";

                            if (DialogResult.OK == ofd.ShowDialog())
                                File.Copy(ofd.FileName, toolsBase + toolsBase);

                            else
                                Program.kill();
                        }
                    }
                    new DFC().make(splash.progress);
                    Program.kill();
                }
            }
            
            string deleteReason = "";

            if (Directory.Exists(Program.tools))
            {
                if (!File.Exists(Program.tools + Program.toolsVer))
                    deleteReason = "outdated.\r\n\r\nIf you'd like to keep something in there (for example your SFX folder) then create a backup now :)";

                foreach (string filename in requiredFiles)
                    if (!File.Exists(Program.tools + filename))
                        deleteReason =
                            "missing the following required file:\r\n\r\n" + 
                            Program.tools + filename + "\r\n\r\n" +
                            "Whoever made your Loopstream.exe probably messed up.";
            }

            if (!string.IsNullOrWhiteSpace(deleteReason))
            {
                z("deleting cause its " + deleteReason);
                if (DialogResult.OK != MessageBox.Show(
                    "I'm about to delete your " + Program.tools.Trim('\\') + " directory since it's " + deleteReason +
                    "\r\n\r\nIf there's something you want to keep (sfx folder)\r\ntake a backup now",
                    "Housekeeping", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning))
                    Program.kill();

                for (int a = 0; a < 3; a++)
                {
                    try
                    {
                        Directory.Delete(Program.tools, true);
                        break;
                    }
                    catch (Exception ex)
                    {
                        z("thank you microsoft");
                        System.Threading.Thread.Sleep(100);
                    }
                }
                while (Directory.Exists(Program.tools))
                    Application.DoEvents();
            }

            if (!Directory.Exists(Program.tools))
            {
                splash.vis();
                z("extracting tools");
                new DFC().extract(splash.progress);
            }
            z("extract sequence done");

            foreach (string filename in requiredFiles)
                if (!File.Exists(Program.tools + filename))
                {
                    MessageBox.Show(
                        "The following required file was not found:\r\n\r\n" +
                        Program.tools + filename + "\r\n\r\n" +
                        "Whoever made your copy of Loopstream.exe fucked up.",
                        "pokker", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Program.kill();
                }

            plowTheFields(); z("traktor test ok");
            splash.unvis(); z("splash hidden");

            if (++loads > 1)
            {
                MessageBox.Show("Critical semantical error, load sequencye fucskdjflks");
                return;
            }
            z("Settings #1 - LOAD"); settings = LSSettings.load();
            z("Settings #2 - TEST"); settings.runTests(splash.progress, false);
            z("Settings #3 - DONE"); isPresetLoad = true;

            z("Binding sliders");
            foreach (var c in new Verter[] { gMus, gMic, gSpd, gOut })
            {
                c.makeInteractive();
                c.valueChanged += gSlider_valueChanged;
            }
            mixerPresetChanged(sender, e);

            z("Creating ni");
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

            z("Binding ni");
            gA.preset = settings.presets[0];
            gB.preset = settings.presets[1];
            gC.preset = settings.presets[2];
            gD.preset = settings.presets[3];

            popPoor = popDrop = popSign = popQuit = null;
            assumeConnected = popFilt = false;

            z("Layout gManualTags");
            gManualTags.Font = new System.Drawing.Font(gManualTags.Font.FontFamily, gManualTags.Font.SizeInPoints * 0.8f);
            gManualTags.Text = gManualTags.Text.ToUpper().Replace(" ", "  ");
            gManualTags.Location = new Point(
                (int)(pictureBox1.Left + (pictureBox1.Width - gManualTags.Width) / 1.85),
                (int)(pictureBox1.Top + (pictureBox1.Height - gManualTags.Height) / 1.9));

            // sfx
            psfx_MouseClick(null, null);

            z("Position form");
            this.Bounds = myBounds;

            splash.Focus();
            //splash.BringToFront();
            splash.fx = settings.splash;
            splash.gtfo();

            if (settings.autohide)
            {
                z("Autohide was true");
                this.Visible = false;
            }
            if (settings.autoconn)
            {
                z("Autoconn was true");
                connect();
            }

            // please don't look
            invals = new Control[] {
                box_top_graden,
                box_bottom_graden,
                gMus.giSlider,
                gMus.graden1,
                gMus.graden2,
                gMic.giSlider,
                gMic.graden1,
                gMic.graden2,
                gSpd.giSlider,
                gSpd.graden1,
                gSpd.graden2,
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

            Timer manMachineInterfaceManager = new Timer();
            manMachineInterfaceManager.Tick += manMachineInterfaceManager_manageWidgets;
            manMachineInterfaceManager.Interval = 200;
            manMachineInterfaceManager.Start();
            
            z("showhide"); showhide();
            z("skinner"); hookskinner(this.Controls);
            //Program.popception();
        }

        void b_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
                lock (sfxes)
                    foreach (var n in sfxes)
                        if (n.title == ((Control)sender).Text)
                            n.fx_out_PlaybackStopped(null, null);
        }

        class SFXNode
        {
            public bool done { get; private set; }
            public string title { get; private set; }
            
            NAudio.Wave.WasapiOut fx_out;
            NAudio.Wave.WaveStream fx_in;

            public SFXNode(string title)
            {
                done = false;
                fx_in = null;
                fx_out = null;
                this.title = title;
                string path = Program.tools + "sfx\\" + title;

                if (File.Exists(path + ".mp3"))
                {
                    fx_in = new NAudio.Wave.Mp3FileReader(path + ".mp3");
                }
                else
                {
                    fx_in = new NVorbis.NAudioSupport.VorbisWaveReader(path + ".ogg");
                }

                var devRec = LSSettings.singleton.devRec as LSDevice;
                if (devRec == null)
                    return;

                fx_out = new NAudio.Wave.WasapiOut(devRec.mm, NAudio.CoreAudioApi.AudioClientShareMode.Shared, false, 100);
                fx_out.PlaybackStopped += new EventHandler<NAudio.Wave.StoppedEventArgs>(fx_out_PlaybackStopped);
                fx_out.Init(fx_in);
                fx_out.Play();
            }
            public void fx_out_PlaybackStopped(object sender, NAudio.Wave.StoppedEventArgs e)
            {
                try
                {
                    if (fx_out != null) fx_out.Dispose();
                    if (fx_in != null) fx_in.Dispose();
                }
                catch { }
                done = true;
            }
        }
        void b_Click(object sender, EventArgs e)
        {
            lock (sfxes)
                try
                {
                    sfxes.Add(new SFXNode(((Button)sender).Text));
                    while (sfxes.Count > 0 && sfxes[0].done)
                    {
                        sfxes.RemoveAt(0);
                    }
                }
                catch { }
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
            z("ni toggle");
            this.Visible = !this.Visible;
            //Program.ni.ContextMenuStrip.Items[0].Text = this.Visible ? "Hide" : "Show";
            Program.ni.ContextMenu.MenuItems[0].Text = this.Visible ? "Hide" : "Show";
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F1))
            {
                System.Diagnostics.Process.Start("http://ocv.me/loopstream");
                return true;
            }
            if (!gTag.Focused)
            {
                int nPreset = -1;
                if (keyData == Keys.D1) nPreset = 0;
                if (keyData == Keys.D2) nPreset = 1;
                if (keyData == Keys.D3) nPreset = 2;
                if (keyData == Keys.D4) nPreset = 3;
                if (nPreset >= 0)
                {
                    settings.mixer.apply(settings.presets[nPreset]);
                    mixerPresetChanged(null, null);
                }
                if (keyData == Keys.Q && mixer != null) mixer.MuteChannel(LSMixer.Slider.Music, settings.mixer.bRec);
                if (keyData == Keys.W && mixer != null) mixer.MuteChannel(LSMixer.Slider.Mic, settings.mixer.bMic);
                if (keyData == Keys.R && mixer != null) mixer.MuteChannel(LSMixer.Slider.Out, settings.mixer.bOut);
                if (keyData == Keys.C) gConnect_Click(null, null);
                if (keyData == Keys.S) gSettings_Click(null, null);
            }
            //this.Text = keyData.ToString();
            if (tKonami != null)
            {
                if (iKonami < cKonami.Length)
                {
                    if (keyData != cKonami[iKonami++])
                    {
                        if (iKonami > 1 &&
                            keyData == cKonami[iKonami - 2])
                        {
                            --iKonami;
                        }
                        else
                        {
                            iKonami = 0;
                        }
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
            z("Mixer slider changed");
            settings.mixer.vRec = gMus.level / 255.0;
            settings.mixer.vMic = gMic.level / 255.0;
            settings.mixer.vSpd = gSpd.level / 200.0;
            settings.mixer.vOut = gOut.level / 255.0;
            settings.mixer.bRec = gMus.enabled;
            settings.mixer.bMic = gMic.enabled;
            settings.mixer.bOut = gOut.enabled;
            settings.mixer.yRec = gMus.boostLock;
            settings.mixer.yMic = gMic.boostLock;
            settings.mixer.xRec = gMus.boost;
            settings.mixer.xMic = gMic.boost;

            if (mixer != null)
            {
                mixer.MuteChannel(LSMixer.Slider.Music, settings.mixer.bRec);
                mixer.MuteChannel(LSMixer.Slider.Mic, settings.mixer.bMic);
                mixer.MuteChannel(LSMixer.Slider.Out, settings.mixer.bOut);

                LSMixer.Slider sl = LSMixer.Slider.Out;
                if (sender == gSpd) return;
                if (sender == gMus) sl = LSMixer.Slider.Music;
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
                else if (et == Verter.EventType.boostLock)
                {
                    mixer.BoostLockChannel(sl, (float)((Verter)sender).boostLock);
                }
                else
                {
                    mixer.FadeVolume(sl, (float)(((Verter)sender).level / 255.0), dur);
                }
            }
        }

        void mixerPresetChanged(object sender, EventArgs e)
        {
            z("Mixer preset changed");
            gMus.level = (int)(255 * settings.mixer.vRec);
            gMic.level = (int)(255 * settings.mixer.vMic);
            gSpd.level = (int)(200 * settings.mixer.vSpd);
            gOut.level = (int)(255 * settings.mixer.vOut);
            gMus.enabled = settings.mixer.bRec;
            gMic.enabled = settings.mixer.bMic;
            gOut.enabled = settings.mixer.bOut;
            gMus.boostLock = settings.mixer.yRec;
            gMic.boostLock = settings.mixer.yMic;
            gMus.boost = settings.mixer.xRec;
            gMic.boost = settings.mixer.xMic;

            // you should probably fix this
            if (mixer != null)
            {
                gMus.eventType = Verter.EventType.slide;
                gMic.eventType = Verter.EventType.slide;
                gOut.eventType = Verter.EventType.slide;
                gSlider_valueChanged(gMus, null);
                gSlider_valueChanged(gMic, null);
                gSlider_valueChanged(gOut, null);

                gMus.eventType = Verter.EventType.boost;
                gMic.eventType = Verter.EventType.boost;
                gSlider_valueChanged(gMus, null);
                gSlider_valueChanged(gMic, null);

                // in case this is necessary
                gMus.eventType = Verter.EventType.boostLock;
                gMic.eventType = Verter.EventType.boostLock;
                gSlider_valueChanged(gMus, null);
                gSlider_valueChanged(gMic, null);
            }
        }

        void connect()
        {
            if (settings.devRec == null || settings.devOut == null)
            {
                if (DialogResult.OK == MessageBox.Show(
                    "Please take a minute to adjust your settings\n\n(soundcard and radio server)",
                    "Audio endpoint is null", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning))
                {
                    gSettings_Click(null, null);

                    if (settings.devRec == null || settings.devOut == null)
                    {
                        MessageBox.Show("Config is still invalid.\n\nGiving up.", "Crit", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else return;
            }

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
            mixer = new LSMixer(settings, new LLabel[] { gMus.giSlider, gMic.giSlider, gOut.giSlider });
            pcm = new LSPcmFeed(settings, mixer.lameOutlet);
            assumeConnected = true;
        }

        void disconnect()
        {
            Program.ni.ContextMenu.MenuItems[1].Text = "Connect";
            if (pMessage.Visible) gLowQ_Click(null, null);
            assumeConnected = false;

            daText = "disconnecting...";
            gConnect.Enabled = false;
            gConnect.Text = daText;
            Application.DoEvents();

            System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ThreadStart(discthread));
            //t.Priority = System.Threading.ThreadPriority.Highest;
            t.Name = "LS_DISC";
            t.Start();
        }

        private void gConnect_Click(object sender, EventArgs e)
        {
            Logger.app.a("begin " + gConnect.Text);

            if (gConnect.Text == "Connect")
            {
                connect();
            }
            else
            {
                disconnect();
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

            this.Invoke((MethodInvoker)delegate
            {
                if (gMus != null && gMus.giSlider != null && gMus.giSlider.src != null) gMus.giSlider.src.ClearVu();
                if (gMic != null && gMic.giSlider != null && gMic.giSlider.src != null) gMic.giSlider.src.ClearVu();
                if (gOut != null && gOut.giSlider != null && gOut.giSlider.src != null) gOut.giSlider.src.ClearVu();
            });
        }

        private void gSettings_Click(object sender, EventArgs e)
        {
            z("Show settings form");
            this.Hide();
            new ConfigSC(settings, this.Bounds).ShowDialog();
            showhide();
            this.Show();
        }

        private void gExit_Click(object sender, EventArgs e)
        {
            Program.kill();
        }

        private void gLoad_Click(object sender, EventArgs e)
        {
            if (gLoad.rightclick)
            {
                if (DialogResult.Yes == MessageBox.Show("Reset mixer preset?", "hello", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    z("mixer-preset reset");
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
                z("mixer-preset mode: load");
                gLoad.Mode = UC_Troggle.Modes.Load;
            }
            else
            {
                z("mixer-preset mode: save");
                gLoad.Mode = UC_Troggle.Modes.Save;
            }
        }

        private void gPreset_Click(object sender, EventArgs e)
        {
            z("mixer-preset interaction");
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
            z("Displaying status form");
            new UI_Status(settings).Show();
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

        Point lastMousePosition = Point.Empty;
        void manMachineInterfaceManager_manageWidgets(object sender, EventArgs e)
        {
            if (mixer != null && mixer.isLQ != null)
            {
                lqMessage = mixer.isLQ;
                mixer.isLQ = null;
                this.Height += pMessage.Height;
                pMessage.Visible = true;
            }

            bool connected = assumeConnected && tag != null;
            long now = DateTime.UtcNow.Ticks / 10000;
            if (!connected)
            {
                foreach (LSSettings.LSTrigger ev in LSSettings.singleton.triggers)
                {
                    ev.lastAudio = now;
                    ev.lastMouse = now;
                }
            }
            else
            {
                Point mousepos = Cursor.Position;
                if (mousepos.X != lastMousePosition.X ||
                    mousepos.Y != lastMousePosition.Y)
                {
                    lastMousePosition = mousepos;
                    foreach (LSSettings.LSTrigger ev in LSSettings.singleton.triggers)
                    {
                        ev.lastMouse = now;
                    }
                }
                if (gOut != null &&
                    gOut.giSlider != null &&
                    gOut.giSlider.src != null)
                {
                    if (gOut.giSlider.src.vuAge < 16)
                    {
                        foreach (LSSettings.LSTrigger ev in LSSettings.singleton.triggers)
                        {
                            if (gOut.giSlider.src.VU >= ev.pAudio)
                            {
                                ev.lastAudio = now;
                            }
                        }
                    }
                }
            }
            if (tag == null) return;





            if (settings.mp3.FIXME_kbps <= 1 && settings.ogg.FIXME_kbps <= 1 && settings.opus.FIXME_kbps <= 1)
            {
                if (daText == "Connect")
                {
                    this.Text = wincap;
                    if (popPoor != null && popPoor.sactive) popPoor.Dispose(); popPoor = null;
                    if (popDrop != null && popDrop.sactive) popDrop.Dispose(); popDrop = null;
                    if (popSign != null && popSign.sactive) popSign.Dispose(); popSign = null;
                    if (popQuit != null && popQuit.sactive) popQuit.Dispose(); popQuit = null;
                }
                else if (daText == "D I S C O N N E C T")
                    this.Text = "- C O N N E C T I N G -";
                else
                    this.Text = "- D I S C O N N E C T I N G -";
            }
            else
            {
                this.Text = string.Format("{0:0.00} // {1:0.00} // {2:0.00} // {3}",
                    Math.Round(settings.mp3.FIXME_kbps, 2),
                    Math.Round(settings.ogg.FIXME_kbps, 2),
                    Math.Round(settings.opus.FIXME_kbps, 2),
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

            if (settings.mixer.xRec < gMus.boost) gMus.boost = settings.mixer.xRec;
            if (settings.mixer.xMic < gMic.boost) gMic.boost = settings.mixer.xMic;

            double f, f_mp3, f_ogg, f_opus;
            f = f_mp3 = f_ogg = f_opus = -1;
            if (settings.mp3.FIXME_kbps >= 0)
            {
                f_mp3 = settings.mp3.FIXME_kbps * 1.0 / settings.mp3.bitrate;
            }
            if (settings.ogg.FIXME_kbps >= 0)
            {
                f_ogg = settings.ogg.FIXME_kbps * 1.0 / 24;
            }
            if (settings.opus.FIXME_kbps >= 0)
            {
                f_opus = settings.opus.FIXME_kbps * 1.0 / 24;
            }
            
            // TODO: ogg
            f = f_mp3 >= 0 ? f_mp3 : f_ogg >= 0 ? f_ogg : f_opus;
            
            lock (Logger.bitrate_mp3)
                Logger.bitrate_mp3.Add(Math.Max(settings.mp3.FIXME_kbps, 0));

            lock (Logger.bitrate_ogg)
                Logger.bitrate_ogg.Add(Math.Max(settings.ogg.FIXME_kbps, 0));
            
            lock (Logger.bitrate_opus)
                Logger.bitrate_opus.Add(Math.Max(settings.opus.FIXME_kbps, 0));
            
            if (connected && popFilt)
            {
                foreach (LSSettings.LSTrigger ev in LSSettings.singleton.triggers)
                {
                    ev.lastNet = f;
                }
                LSSettings.LSTrigger.Until prev = new LSSettings.LSTrigger.Until();
                prev.required = prev.msec = int.MaxValue;
                
                LSSettings.LSTrigger show = null;
                foreach (LSSettings.LSTrigger ev in LSSettings.singleton.triggers)
                {
                    LSSettings.LSTrigger.Until until = ev.until(now);
                    if (until.msec <= prev.msec ||
                        until.msec < until.required / 2)
                    {
                        show = ev;
                        prev = until;
                    }
                }
                if (popPoor != null && popPoor.sactive && (show == null || show.eType != LSSettings.LSTrigger.EventType.WARN_CONN_POOR || show.until(now).msec > 0)) { popPoor.Dispose(); popPoor = null; }
                if (popDrop != null && popDrop.sactive && (show == null || show.eType != LSSettings.LSTrigger.EventType.WARN_CONN_DROP || show.until(now).msec > 0)) { popDrop.Dispose(); popDrop = null; }
                if (popSign != null && popSign.sactive && (show == null || show.eType != LSSettings.LSTrigger.EventType.WARN_NO_AUDIO || show.until(now).msec > 100)) { popSign.Dispose(); popSign = null; }
                if (popQuit != null && popQuit.sactive)
                {
                    if (show == null || show.eType != LSSettings.LSTrigger.EventType.DISCONNECT)
                    {
                        popQuit.Dispose(); popQuit = null;
                    }
                    else
                    {
                        LSSettings.LSTrigger.Until remain = show.until(now);
                        if (remain.msec > remain.required / 2)
                        {
                            popQuit.Dispose(); popQuit = null;
                        }
                    }
                }
                if (show != null && show.until(now).msec <= 0)
                {
                    if (show.eType == LSSettings.LSTrigger.EventType.WARN_CONN_POOR && (popPoor == null || !popPoor.sactive))
                    {
                        popPoor = new UI_Msg("poor", "");
                        popPoor.Show();
                    }
                    if (show.eType == LSSettings.LSTrigger.EventType.WARN_CONN_DROP && (popDrop == null || !popDrop.sactive))
                    {
                        popDrop = new UI_Msg("drop", "");
                        popDrop.Show();
                    }
                    if (show.eType == LSSettings.LSTrigger.EventType.WARN_NO_AUDIO && popSign == null)
                    {
                        popSign = new UI_Msg("audio", "");
                        popSign.Show();
                    }
                    if (show.eType != LSSettings.LSTrigger.EventType.WARN_NO_AUDIO && popSign != null)
                    {
                        popSign.Dispose();
                        popSign = null;
                    }
                    if (show.eType == LSSettings.LSTrigger.EventType.DISCONNECT)
                    {
                        if (popQuit != null && popQuit.sactive)
                        {
                            popQuit.setMsg("0");
                        }
                        disconnect();
                    }
                }
                else if (show != null && show.eType == LSSettings.LSTrigger.EventType.DISCONNECT)
                {
                    LSSettings.LSTrigger.Until remain = show.until(now);
                    if (remain.msec <= remain.required / 2)
                    {
                        string msg = Math.Ceiling(remain.msec / 1000.0).ToString();

                        if (popQuit == null || !popQuit.sactive)
                        {
                            popQuit = new UI_Msg("quit", msg);
                            popQuit.Show();
                        }
                        else
                        {
                            popQuit.setMsg(msg);
                        }

                        if (popSign != null && !popSign.sactive)
                        {
                            popSign.Dispose();
                            popSign = null;
                        }
                    }
                }
            }
            else
            {
                double max = 0;
                foreach (LSSettings.LSTrigger ev in LSSettings.singleton.triggers)
                {
                    if (ev.eType == LSSettings.LSTrigger.EventType.WARN_CONN_POOR ||
                        ev.eType == LSSettings.LSTrigger.EventType.WARN_CONN_DROP)
                    {
                        max = Math.Max(max, ev.pUpload);
                    }
                }
                if (f > max) popFilt = assumeConnected;
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
                    z("Faking crash");
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
            this.Height -= pMessage.Height;
            pMessage.Visible = false;
            if (sender == gLowQ) MessageBox.Show(lqMessage);
            lqMessage = null;
        }

        private void gTag_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                z("Sending tags by ENTER");
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
                z("displaying pTag");
                this.SuspendLayout();
                tagvis = pTag.Visible = true;
                this.Height += pTag.Height;
                this.ResumeLayout();
            }
            else if (settings.tagAuto && tagvis)
            {
                z("concealing pTag");
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
                z("gTagRead");
                gTag.Text = LSTag.get(settings.meta, false).tag;
            }
            catch (Exception ex)
            {
                z("failed, no stream");
                System.Windows.Forms.MessageBox.Show("Can't let you do that, Dave.\n(please start streaming first)\n\n" + ex.Message + "\n" + ex.StackTrace);
            }
        }

        private void gTagSend_Click(object sender, EventArgs e)
        {
            try
            {
                z("gTagSend");
                tag.set(gTag.Text);
            }
            catch (Exception ex)
            {
                z("failed, no stream");
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
                z("Found ice, launching");
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
            z("Toggle manualTags checkbox");
            settings.tagAuto = !gManualTags.Checked;
            showhide();
        }

        private void psfx_MouseClick(object sender, MouseEventArgs e)
        {
            if (e != null && e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                this.Width -= psfx.Width;
                psfx.Visible = false;
                while (psfx.Controls.Count > 0)
                {
                    psfx.Controls.RemoveAt(0);
                }
                return;
            }

            z("DJ Effects");
            sfxes = new List<SFXNode>();
            int widen = 0;
            try
            {
                while (psfx.Controls.Count > 2)
                {
                    var c = psfx.Controls[2];
                    psfx.Controls.RemoveAt(2);
                    c.Dispose();
                }
            }
            catch { }
            try
            {
                Size szb = new Size(68, 25);
                Point pto = new Point(16, 14);
                string[] files = Directory.GetFiles(Program.tools + "sfx");
                Array.Sort(files);
                int n = 0;
                foreach (string f in files)
                {
                    string title = f.Substring(f.Replace("\\", "/").LastIndexOf('/') + 1);
                    int dot = title.LastIndexOf('.');
                    if (dot < 0) continue;
                    title = title.Substring(0, dot);
                    
                    if (++n > 14)
                    {
                        n = 0;
                        pto.Y = 14;
                        pto.X += szb.Width + 10;
                        widen += szb.Width + 10;
                    }
                    Button b = new Button();
                    b.Text = title;
                    b.Size = szb;
                    b.Location = pto;
                    psfx.Controls.Add(b);
                    pto.Y += b.Height + b.Margin.Bottom;
                    b.Click += new EventHandler(b_Click);
                    b.MouseDown += new MouseEventHandler(b_MouseDown);
                }
            }
            catch { }
            //this.Width += widen;
            myBounds.Width += widen;
        }
    }
}