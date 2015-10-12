namespace LoopStream
{
    partial class ConfigSC
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.gOutS = new System.Windows.Forms.ComboBox();
            this.gTwoS = new System.Windows.Forms.ComboBox();
            this.gOneS = new System.Windows.Forms.ComboBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.gSiren = new System.Windows.Forms.RadioButton();
            this.gIce = new System.Windows.Forms.RadioButton();
            this.gShout = new System.Windows.Forms.RadioButton();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.gPass = new System.Windows.Forms.TextBox();
            this.gMount = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.gHost = new System.Windows.Forms.TextBox();
            this.gSave = new System.Windows.Forms.Button();
            this.gCancel = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.gLeft = new System.Windows.Forms.CheckBox();
            this.gRight = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.gOgg = new System.Windows.Forms.Label();
            this.gMp3 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.gRate = new System.Windows.Forms.TextBox();
            this.pMp3 = new System.Windows.Forms.Panel();
            this.gMp3Stereo = new System.Windows.Forms.RadioButton();
            this.gMp3Enable = new System.Windows.Forms.CheckBox();
            this.gMp3Mono = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.gMp3BitrateV = new System.Windows.Forms.TextBox();
            this.gMp3Quality = new System.Windows.Forms.RadioButton();
            this.gMp3QualityV = new System.Windows.Forms.TextBox();
            this.gMp3Bitrate = new System.Windows.Forms.RadioButton();
            this.pOgg = new System.Windows.Forms.Panel();
            this.gOggStereo = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.gOggBitrateV = new System.Windows.Forms.TextBox();
            this.gOggQuality = new System.Windows.Forms.RadioButton();
            this.gOggQualityV = new System.Windows.Forms.TextBox();
            this.gOggBitrate = new System.Windows.Forms.RadioButton();
            this.gOggMono = new System.Windows.Forms.RadioButton();
            this.gOggEnable = new System.Windows.Forms.CheckBox();
            this.gTestDevs = new System.Windows.Forms.CheckBox();
            this.gSplash = new System.Windows.Forms.CheckBox();
            this.gRecMP3 = new System.Windows.Forms.CheckBox();
            this.gRecPCM = new System.Windows.Forms.CheckBox();
            this.gUnavail = new System.Windows.Forms.CheckBox();
            this.gRecOGG = new System.Windows.Forms.CheckBox();
            this.gAutoconn = new System.Windows.Forms.CheckBox();
            this.gAutohide = new System.Windows.Forms.CheckBox();
            this.tt = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox4.SuspendLayout();
            this.panel1.SuspendLayout();
            this.pMp3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.pOgg.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // gOutS
            // 
            this.gOutS.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.gOutS.FormattingEnabled = true;
            this.gOutS.Location = new System.Drawing.Point(94, 66);
            this.gOutS.Margin = new System.Windows.Forms.Padding(3, 3, 3, 8);
            this.gOutS.Name = "gOutS";
            this.gOutS.Size = new System.Drawing.Size(418, 21);
            this.gOutS.TabIndex = 15;
            this.tt.SetToolTip(this.gOutS, "A set of speakers to play the final mix on\r\n    (can be the same as input Music)\r" +
        "\n    (can be muted on the mixer board)");
            this.gOutS.SelectedIndexChanged += new System.EventHandler(this.gOutS_SelectedIndexChanged);
            // 
            // gTwoS
            // 
            this.gTwoS.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.gTwoS.FormattingEnabled = true;
            this.gTwoS.Location = new System.Drawing.Point(94, 12);
            this.gTwoS.Name = "gTwoS";
            this.gTwoS.Size = new System.Drawing.Size(305, 21);
            this.gTwoS.TabIndex = 12;
            this.tt.SetToolTip(this.gTwoS, "The microphone you will use to voiceover on the stream");
            this.gTwoS.SelectedIndexChanged += new System.EventHandler(this.gTwoS_SelectedIndexChanged);
            // 
            // gOneS
            // 
            this.gOneS.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.gOneS.FormattingEnabled = true;
            this.gOneS.Location = new System.Drawing.Point(94, 39);
            this.gOneS.Name = "gOneS";
            this.gOneS.Size = new System.Drawing.Size(418, 21);
            this.gOneS.TabIndex = 9;
            this.tt.SetToolTip(this.gOneS, "The set of speakers you will be playing music on\r\n    (either your regular speake" +
        "rs or a separate soundcard)");
            this.gOneS.SelectedIndexChanged += new System.EventHandler(this.gOneS_SelectedIndexChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.gSiren);
            this.groupBox4.Controls.Add(this.gIce);
            this.groupBox4.Controls.Add(this.gShout);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Controls.Add(this.gPass);
            this.groupBox4.Controls.Add(this.gMount);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.gHost);
            this.groupBox4.Location = new System.Drawing.Point(261, 107);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(14);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(251, 120);
            this.groupBox4.TabIndex = 27;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Radio server";
            // 
            // gSiren
            // 
            this.gSiren.AutoSize = true;
            this.gSiren.Enabled = false;
            this.gSiren.Location = new System.Drawing.Point(195, 97);
            this.gSiren.Margin = new System.Windows.Forms.Padding(3, 3, 7, 3);
            this.gSiren.Name = "gSiren";
            this.gSiren.Size = new System.Drawing.Size(49, 17);
            this.gSiren.TabIndex = 37;
            this.gSiren.Text = "Siren";
            this.tt.SetToolTip(this.gSiren, "This is a siren server (in-house protocol)");
            this.gSiren.UseVisualStyleBackColor = true;
            this.gSiren.CheckedChanged += new System.EventHandler(this.gSiren_CheckedChanged);
            // 
            // gIce
            // 
            this.gIce.AutoSize = true;
            this.gIce.Checked = true;
            this.gIce.Location = new System.Drawing.Point(145, 97);
            this.gIce.Margin = new System.Windows.Forms.Padding(3, 3, 7, 3);
            this.gIce.Name = "gIce";
            this.gIce.Size = new System.Drawing.Size(40, 17);
            this.gIce.TabIndex = 36;
            this.gIce.TabStop = true;
            this.gIce.Text = "Ice";
            this.tt.SetToolTip(this.gIce, "This is an icecast server");
            this.gIce.UseVisualStyleBackColor = true;
            this.gIce.CheckedChanged += new System.EventHandler(this.gIce_CheckedChanged);
            // 
            // gShout
            // 
            this.gShout.AutoSize = true;
            this.gShout.Enabled = false;
            this.gShout.Location = new System.Drawing.Point(82, 97);
            this.gShout.Margin = new System.Windows.Forms.Padding(3, 3, 7, 3);
            this.gShout.Name = "gShout";
            this.gShout.Size = new System.Drawing.Size(53, 17);
            this.gShout.TabIndex = 35;
            this.gShout.Text = "Shout";
            this.tt.SetToolTip(this.gShout, "This is a ShoutCAST server");
            this.gShout.UseVisualStyleBackColor = true;
            this.gShout.CheckedChanged += new System.EventHandler(this.gShout_CheckedChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 74);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(60, 13);
            this.label6.TabIndex = 34;
            this.label6.Text = "Mountpoint";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 48);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 13);
            this.label7.TabIndex = 33;
            this.label7.Text = "Password";
            // 
            // gPass
            // 
            this.gPass.Location = new System.Drawing.Point(82, 45);
            this.gPass.Name = "gPass";
            this.gPass.Size = new System.Drawing.Size(163, 20);
            this.gPass.TabIndex = 31;
            this.gPass.Text = "user|assword";
            this.tt.SetToolTip(this.gPass, "Username \"source\" is assumed, this is your source password.");
            this.gPass.TextChanged += new System.EventHandler(this.gPass_TextChanged);
            this.gPass.MouseEnter += new System.EventHandler(this.gHost_MouseEnter);
            this.gPass.MouseLeave += new System.EventHandler(this.gHost_MouseLeave);
            // 
            // gMount
            // 
            this.gMount.Location = new System.Drawing.Point(82, 71);
            this.gMount.Name = "gMount";
            this.gMount.Size = new System.Drawing.Size(163, 20);
            this.gMount.TabIndex = 32;
            this.gMount.Text = "main";
            this.tt.SetToolTip(this.gMount, "The mountpoint to stream to:\r\n    NOT including the preceding /\r\n    NOT includin" +
        "g .extension");
            this.gMount.TextChanged += new System.EventHandler(this.gMount_TextChanged);
            this.gMount.MouseEnter += new System.EventHandler(this.gHost_MouseEnter);
            this.gMount.MouseLeave += new System.EventHandler(this.gHost_MouseLeave);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 13);
            this.label4.TabIndex = 29;
            this.label4.Text = "Address";
            // 
            // gHost
            // 
            this.gHost.Location = new System.Drawing.Point(82, 19);
            this.gHost.Name = "gHost";
            this.gHost.Size = new System.Drawing.Size(163, 20);
            this.gHost.TabIndex = 21;
            this.gHost.Text = "stream0.r-a-d.io:1337";
            this.tt.SetToolTip(this.gHost, "The server you will stream to.\r\nUse one of either:\r\n    DOMAIN:PORT\r\n    IPADDRES" +
        "S:PORT\r\n    HOSTNAME:PORT\r\n");
            this.gHost.TextChanged += new System.EventHandler(this.gHost_TextChanged);
            this.gHost.MouseEnter += new System.EventHandler(this.gHost_MouseEnter);
            this.gHost.MouseLeave += new System.EventHandler(this.gHost_MouseLeave);
            // 
            // gSave
            // 
            this.gSave.Location = new System.Drawing.Point(260, 245);
            this.gSave.Name = "gSave";
            this.gSave.Size = new System.Drawing.Size(121, 50);
            this.gSave.TabIndex = 29;
            this.gSave.Text = "S A V E   4 E V A";
            this.tt.SetToolTip(this.gSave, "Save and apply this configuration");
            this.gSave.UseVisualStyleBackColor = true;
            this.gSave.Click += new System.EventHandler(this.button1_Click);
            // 
            // gCancel
            // 
            this.gCancel.Location = new System.Drawing.Point(391, 245);
            this.gCancel.Name = "gCancel";
            this.gCancel.Size = new System.Drawing.Size(121, 50);
            this.gCancel.TabIndex = 30;
            this.gCancel.Text = "A p p l y   o n l y";
            this.tt.SetToolTip(this.gCancel, "Apply this configuration only for this session");
            this.gCancel.UseVisualStyleBackColor = true;
            this.gCancel.Click += new System.EventHandler(this.button2_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label5.ForeColor = System.Drawing.SystemColors.ButtonShadow;
            this.label5.Location = new System.Drawing.Point(13, 111);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 170);
            this.label5.TabIndex = 31;
            this.label5.Text = "L\r\n    o\r\n        o\r\n            p\r\nS\r\n  t\r\n    r\r\n      e\r\n        a\r\n          " +
    "m";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 69);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(68, 13);
            this.label8.TabIndex = 32;
            this.label8.Text = "Local Output";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(13, 42);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(61, 13);
            this.label9.TabIndex = 33;
            this.label9.Text = "input Music";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(13, 15);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(50, 13);
            this.label10.TabIndex = 34;
            this.label10.Text = "input Mic";
            // 
            // gLeft
            // 
            this.gLeft.AutoSize = true;
            this.gLeft.Location = new System.Drawing.Point(411, 14);
            this.gLeft.Margin = new System.Windows.Forms.Padding(9, 3, 3, 3);
            this.gLeft.Name = "gLeft";
            this.gLeft.Size = new System.Drawing.Size(44, 17);
            this.gLeft.TabIndex = 35;
            this.gLeft.Text = "Left";
            this.tt.SetToolTip(this.gLeft, "Use the left audio channel of the microphone?");
            this.gLeft.UseVisualStyleBackColor = true;
            this.gLeft.CheckedChanged += new System.EventHandler(this.gLeft_CheckedChanged);
            // 
            // gRight
            // 
            this.gRight.AutoSize = true;
            this.gRight.Checked = true;
            this.gRight.CheckState = System.Windows.Forms.CheckState.Checked;
            this.gRight.Location = new System.Drawing.Point(461, 14);
            this.gRight.Name = "gRight";
            this.gRight.Size = new System.Drawing.Size(51, 17);
            this.gRight.TabIndex = 36;
            this.gRight.Text = "Right";
            this.tt.SetToolTip(this.gRight, "Use the right audio channel of the microphone?");
            this.gRight.UseVisualStyleBackColor = true;
            this.gRight.CheckedChanged += new System.EventHandler(this.gRight_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.gOgg);
            this.panel1.Controls.Add(this.gMp3);
            this.panel1.Controls.Add(this.label13);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.gRate);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.gRight);
            this.panel1.Controls.Add(this.gCancel);
            this.panel1.Controls.Add(this.groupBox4);
            this.panel1.Controls.Add(this.gSave);
            this.panel1.Controls.Add(this.gOneS);
            this.panel1.Controls.Add(this.gLeft);
            this.panel1.Controls.Add(this.gTwoS);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.gOutS);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.pMp3);
            this.panel1.Controls.Add(this.pOgg);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(542, 319);
            this.panel1.TabIndex = 37;
            // 
            // gOgg
            // 
            this.gOgg.AutoSize = true;
            this.gOgg.Location = new System.Drawing.Point(165, 107);
            this.gOgg.Name = "gOgg";
            this.gOgg.Size = new System.Drawing.Size(65, 13);
            this.gOgg.TabIndex = 45;
            this.gOgg.Text = "OGG/Vorbis";
            this.gOgg.Click += new System.EventHandler(this.gOgg_Click);
            // 
            // gMp3
            // 
            this.gMp3.AutoSize = true;
            this.gMp3.Location = new System.Drawing.Point(98, 107);
            this.gMp3.Name = "gMp3";
            this.gMp3.Size = new System.Drawing.Size(60, 13);
            this.gMp3.TabIndex = 44;
            this.gMp3.Text = "MP3/Lame";
            this.gMp3.Click += new System.EventHandler(this.gMp3_Click);
            // 
            // label13
            // 
            this.label13.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label13.Location = new System.Drawing.Point(94, 121);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(139, 1);
            this.label13.TabIndex = 43;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label2.Dock = System.Windows.Forms.DockStyle.Right;
            this.label2.Location = new System.Drawing.Point(540, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(1, 319);
            this.label2.TabIndex = 38;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(96, 277);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 13);
            this.label3.TabIndex = 24;
            this.label3.Text = "SampleRate";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.label1.Dock = System.Windows.Forms.DockStyle.Right;
            this.label1.Location = new System.Drawing.Point(541, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(1, 319);
            this.label1.TabIndex = 37;
            // 
            // gRate
            // 
            this.gRate.Location = new System.Drawing.Point(168, 274);
            this.gRate.Name = "gRate";
            this.gRate.Size = new System.Drawing.Size(59, 20);
            this.gRate.TabIndex = 21;
            this.gRate.Text = "44100";
            this.tt.SetToolTip(this.gRate, "The sample rate to stream at (same for both MP3 and OGG)\r\n    (internet and CD st" +
        "andard is 44100)");
            this.gRate.TextChanged += new System.EventHandler(this.gOggQualityV_TextChanged);
            this.gRate.MouseEnter += new System.EventHandler(this.gHost_MouseEnter);
            this.gRate.MouseLeave += new System.EventHandler(this.gHost_MouseLeave);
            // 
            // pMp3
            // 
            this.pMp3.Controls.Add(this.gMp3Stereo);
            this.pMp3.Controls.Add(this.gMp3Enable);
            this.pMp3.Controls.Add(this.gMp3Mono);
            this.pMp3.Controls.Add(this.groupBox1);
            this.pMp3.Location = new System.Drawing.Point(94, 124);
            this.pMp3.Name = "pMp3";
            this.pMp3.Size = new System.Drawing.Size(139, 140);
            this.pMp3.TabIndex = 42;
            // 
            // gMp3Stereo
            // 
            this.gMp3Stereo.AutoSize = true;
            this.gMp3Stereo.Checked = true;
            this.gMp3Stereo.Location = new System.Drawing.Point(74, 118);
            this.gMp3Stereo.Name = "gMp3Stereo";
            this.gMp3Stereo.Size = new System.Drawing.Size(56, 17);
            this.gMp3Stereo.TabIndex = 26;
            this.gMp3Stereo.TabStop = true;
            this.gMp3Stereo.Text = "Stereo";
            this.tt.SetToolTip(this.gMp3Stereo, "Stream using 2 audio channels (stereo)");
            this.gMp3Stereo.UseVisualStyleBackColor = true;
            this.gMp3Stereo.CheckedChanged += new System.EventHandler(this.gMp3Stereo_CheckedChanged);
            // 
            // gMp3Enable
            // 
            this.gMp3Enable.AutoSize = true;
            this.gMp3Enable.Location = new System.Drawing.Point(7, 3);
            this.gMp3Enable.Name = "gMp3Enable";
            this.gMp3Enable.Size = new System.Drawing.Size(65, 17);
            this.gMp3Enable.TabIndex = 40;
            this.gMp3Enable.Text = "Enabled";
            this.tt.SetToolTip(this.gMp3Enable, "Transmit an MP3-encoded stream");
            this.gMp3Enable.UseVisualStyleBackColor = true;
            this.gMp3Enable.CheckedChanged += new System.EventHandler(this.gMp3Enable_CheckedChanged);
            // 
            // gMp3Mono
            // 
            this.gMp3Mono.AutoSize = true;
            this.gMp3Mono.Location = new System.Drawing.Point(6, 118);
            this.gMp3Mono.Name = "gMp3Mono";
            this.gMp3Mono.Size = new System.Drawing.Size(52, 17);
            this.gMp3Mono.TabIndex = 25;
            this.gMp3Mono.Text = "Mono";
            this.tt.SetToolTip(this.gMp3Mono, "Stream using 1 audio channel (mono)\r\n    (there is really no need to choose this)" +
        "");
            this.gMp3Mono.UseVisualStyleBackColor = true;
            this.gMp3Mono.CheckedChanged += new System.EventHandler(this.gMp3Mono_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.gMp3BitrateV);
            this.groupBox1.Controls.Add(this.gMp3Quality);
            this.groupBox1.Controls.Add(this.gMp3QualityV);
            this.groupBox1.Controls.Add(this.gMp3Bitrate);
            this.groupBox1.Location = new System.Drawing.Point(0, 32);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(0, 3, 0, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(139, 71);
            this.groupBox1.TabIndex = 26;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Compression";
            // 
            // gMp3BitrateV
            // 
            this.gMp3BitrateV.Location = new System.Drawing.Point(74, 19);
            this.gMp3BitrateV.Name = "gMp3BitrateV";
            this.gMp3BitrateV.Size = new System.Drawing.Size(59, 20);
            this.gMp3BitrateV.TabIndex = 21;
            this.gMp3BitrateV.Text = "192";
            this.tt.SetToolTip(this.gMp3BitrateV, "The bitrate to stream at (128 or 192 recommended)");
            this.gMp3BitrateV.TextChanged += new System.EventHandler(this.gMp3BitrateV_TextChanged);
            this.gMp3BitrateV.MouseEnter += new System.EventHandler(this.gHost_MouseEnter);
            this.gMp3BitrateV.MouseLeave += new System.EventHandler(this.gHost_MouseLeave);
            // 
            // gMp3Quality
            // 
            this.gMp3Quality.AutoSize = true;
            this.gMp3Quality.Location = new System.Drawing.Point(6, 46);
            this.gMp3Quality.Name = "gMp3Quality";
            this.gMp3Quality.Size = new System.Drawing.Size(57, 17);
            this.gMp3Quality.TabIndex = 25;
            this.gMp3Quality.Text = "Quality";
            this.tt.SetToolTip(this.gMp3Quality, "Stream MP3 using constant bitrate (NOT recommended)\r\n    (some players will have " +
        "problems with speedup/down)");
            this.gMp3Quality.UseVisualStyleBackColor = true;
            this.gMp3Quality.CheckedChanged += new System.EventHandler(this.gMp3Quality_CheckedChanged);
            // 
            // gMp3QualityV
            // 
            this.gMp3QualityV.Location = new System.Drawing.Point(74, 45);
            this.gMp3QualityV.Name = "gMp3QualityV";
            this.gMp3QualityV.Size = new System.Drawing.Size(59, 20);
            this.gMp3QualityV.TabIndex = 23;
            this.gMp3QualityV.Text = "5";
            this.tt.SetToolTip(this.gMp3QualityV, "The quality to stream at (LOWER is better/bigger)");
            this.gMp3QualityV.TextChanged += new System.EventHandler(this.gMp3QualityV_TextChanged);
            this.gMp3QualityV.MouseEnter += new System.EventHandler(this.gHost_MouseEnter);
            this.gMp3QualityV.MouseLeave += new System.EventHandler(this.gHost_MouseLeave);
            // 
            // gMp3Bitrate
            // 
            this.gMp3Bitrate.AutoSize = true;
            this.gMp3Bitrate.Checked = true;
            this.gMp3Bitrate.Location = new System.Drawing.Point(6, 20);
            this.gMp3Bitrate.Name = "gMp3Bitrate";
            this.gMp3Bitrate.Size = new System.Drawing.Size(55, 17);
            this.gMp3Bitrate.TabIndex = 24;
            this.gMp3Bitrate.TabStop = true;
            this.gMp3Bitrate.Text = "Bitrate";
            this.tt.SetToolTip(this.gMp3Bitrate, "Stream MP3 using constant bitrate (recommended)\r\n    (works in every single media" +
        " player)");
            this.gMp3Bitrate.UseVisualStyleBackColor = true;
            this.gMp3Bitrate.CheckedChanged += new System.EventHandler(this.gMp3Bitrate_CheckedChanged);
            // 
            // pOgg
            // 
            this.pOgg.Controls.Add(this.gOggStereo);
            this.pOgg.Controls.Add(this.groupBox2);
            this.pOgg.Controls.Add(this.gOggMono);
            this.pOgg.Controls.Add(this.gOggEnable);
            this.pOgg.Location = new System.Drawing.Point(94, 124);
            this.pOgg.Name = "pOgg";
            this.pOgg.Size = new System.Drawing.Size(139, 140);
            this.pOgg.TabIndex = 42;
            // 
            // gOggStereo
            // 
            this.gOggStereo.AutoSize = true;
            this.gOggStereo.Checked = true;
            this.gOggStereo.Location = new System.Drawing.Point(74, 118);
            this.gOggStereo.Name = "gOggStereo";
            this.gOggStereo.Size = new System.Drawing.Size(56, 17);
            this.gOggStereo.TabIndex = 45;
            this.gOggStereo.TabStop = true;
            this.gOggStereo.Text = "Stereo";
            this.tt.SetToolTip(this.gOggStereo, "Stream using 2 audio channels (stereo)");
            this.gOggStereo.UseVisualStyleBackColor = true;
            this.gOggStereo.CheckedChanged += new System.EventHandler(this.gOggStereo_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.gOggBitrateV);
            this.groupBox2.Controls.Add(this.gOggQuality);
            this.groupBox2.Controls.Add(this.gOggQualityV);
            this.groupBox2.Controls.Add(this.gOggBitrate);
            this.groupBox2.Location = new System.Drawing.Point(0, 32);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(0, 3, 0, 8);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(139, 71);
            this.groupBox2.TabIndex = 41;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Compression";
            // 
            // gOggBitrateV
            // 
            this.gOggBitrateV.Location = new System.Drawing.Point(74, 19);
            this.gOggBitrateV.Name = "gOggBitrateV";
            this.gOggBitrateV.Size = new System.Drawing.Size(59, 20);
            this.gOggBitrateV.TabIndex = 21;
            this.gOggBitrateV.Text = "192";
            this.tt.SetToolTip(this.gOggBitrateV, "The bitrate to stream at (128, 160 or 192 recommended)");
            this.gOggBitrateV.TextChanged += new System.EventHandler(this.gOggBitrateV_TextChanged);
            this.gOggBitrateV.MouseEnter += new System.EventHandler(this.gHost_MouseEnter);
            this.gOggBitrateV.MouseLeave += new System.EventHandler(this.gHost_MouseLeave);
            // 
            // gOggQuality
            // 
            this.gOggQuality.AutoSize = true;
            this.gOggQuality.Location = new System.Drawing.Point(6, 46);
            this.gOggQuality.Name = "gOggQuality";
            this.gOggQuality.Size = new System.Drawing.Size(57, 17);
            this.gOggQuality.TabIndex = 25;
            this.gOggQuality.Text = "Quality";
            this.tt.SetToolTip(this.gOggQuality, "Stream OGG using constant bitrate (recommended)\r\n    (superb dynamic sound qualit" +
        "y at small sizes)");
            this.gOggQuality.UseVisualStyleBackColor = true;
            this.gOggQuality.CheckedChanged += new System.EventHandler(this.gOggQuality_CheckedChanged);
            // 
            // gOggQualityV
            // 
            this.gOggQualityV.Location = new System.Drawing.Point(74, 45);
            this.gOggQualityV.Name = "gOggQualityV";
            this.gOggQualityV.Size = new System.Drawing.Size(59, 20);
            this.gOggQualityV.TabIndex = 23;
            this.gOggQualityV.Text = "5";
            this.tt.SetToolTip(this.gOggQualityV, "The quality to stream at (HIGHER is better/bigger)");
            this.gOggQualityV.TextChanged += new System.EventHandler(this.gOggQualityV_TextChanged);
            this.gOggQualityV.MouseEnter += new System.EventHandler(this.gHost_MouseEnter);
            this.gOggQualityV.MouseLeave += new System.EventHandler(this.gHost_MouseLeave);
            // 
            // gOggBitrate
            // 
            this.gOggBitrate.AutoSize = true;
            this.gOggBitrate.Checked = true;
            this.gOggBitrate.Location = new System.Drawing.Point(6, 20);
            this.gOggBitrate.Name = "gOggBitrate";
            this.gOggBitrate.Size = new System.Drawing.Size(55, 17);
            this.gOggBitrate.TabIndex = 24;
            this.gOggBitrate.TabStop = true;
            this.gOggBitrate.Text = "Bitrate";
            this.tt.SetToolTip(this.gOggBitrate, "Stream OGG using constant bitrate (NOT recommended)\r\n    (slow AND poor sound qua" +
        "lity, just terrible)");
            this.gOggBitrate.UseVisualStyleBackColor = true;
            this.gOggBitrate.CheckedChanged += new System.EventHandler(this.gOggBitrate_CheckedChanged);
            // 
            // gOggMono
            // 
            this.gOggMono.AutoSize = true;
            this.gOggMono.Location = new System.Drawing.Point(6, 118);
            this.gOggMono.Name = "gOggMono";
            this.gOggMono.Size = new System.Drawing.Size(52, 17);
            this.gOggMono.TabIndex = 44;
            this.gOggMono.Text = "Mono";
            this.tt.SetToolTip(this.gOggMono, "Stream using 1 audio channel (mono)\r\n    (there is really no need to choose this)" +
        "");
            this.gOggMono.UseVisualStyleBackColor = true;
            this.gOggMono.CheckedChanged += new System.EventHandler(this.gOggMono_CheckedChanged);
            // 
            // gOggEnable
            // 
            this.gOggEnable.AutoSize = true;
            this.gOggEnable.Location = new System.Drawing.Point(74, 3);
            this.gOggEnable.Name = "gOggEnable";
            this.gOggEnable.Size = new System.Drawing.Size(65, 17);
            this.gOggEnable.TabIndex = 43;
            this.gOggEnable.Text = "Enabled";
            this.tt.SetToolTip(this.gOggEnable, "Transmit an OGG/Vorbis-encoded stream");
            this.gOggEnable.UseVisualStyleBackColor = true;
            this.gOggEnable.CheckedChanged += new System.EventHandler(this.gOggEnable_CheckedChanged);
            // 
            // gTestDevs
            // 
            this.gTestDevs.AutoSize = true;
            this.gTestDevs.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.gTestDevs.Checked = true;
            this.gTestDevs.CheckState = System.Windows.Forms.CheckState.Checked;
            this.gTestDevs.Location = new System.Drawing.Point(557, 12);
            this.gTestDevs.Margin = new System.Windows.Forms.Padding(12, 3, 3, 3);
            this.gTestDevs.Name = "gTestDevs";
            this.gTestDevs.Size = new System.Drawing.Size(205, 30);
            this.gTestDevs.TabIndex = 38;
            this.gTestDevs.Text = "Run all sound device tests on startup\r\n(theoretical crash on some computers)";
            this.tt.SetToolTip(this.gTestDevs, "Check which audio devices that are usable on startup.\r\nSome soundcards could caus" +
        "e unhandled exceptions.");
            this.gTestDevs.UseVisualStyleBackColor = true;
            this.gTestDevs.CheckedChanged += new System.EventHandler(this.gTestDevs_CheckedChanged);
            // 
            // gSplash
            // 
            this.gSplash.AutoSize = true;
            this.gSplash.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.gSplash.Checked = true;
            this.gSplash.CheckState = System.Windows.Forms.CheckState.Checked;
            this.gSplash.Location = new System.Drawing.Point(557, 71);
            this.gSplash.Margin = new System.Windows.Forms.Padding(12, 3, 3, 3);
            this.gSplash.Name = "gSplash";
            this.gSplash.Size = new System.Drawing.Size(167, 17);
            this.gSplash.TabIndex = 39;
            this.gSplash.Text = "Splash screen fadeout effects";
            this.tt.SetToolTip(this.gSplash, "420 lensflare bling");
            this.gSplash.UseVisualStyleBackColor = true;
            this.gSplash.CheckedChanged += new System.EventHandler(this.gSplash_CheckedChanged);
            // 
            // gRecMP3
            // 
            this.gRecMP3.AutoSize = true;
            this.gRecMP3.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.gRecMP3.Checked = true;
            this.gRecMP3.CheckState = System.Windows.Forms.CheckState.Checked;
            this.gRecMP3.Location = new System.Drawing.Point(557, 94);
            this.gRecMP3.Margin = new System.Windows.Forms.Padding(12, 3, 3, 3);
            this.gRecMP3.Name = "gRecMP3";
            this.gRecMP3.Size = new System.Drawing.Size(137, 17);
            this.gRecMP3.TabIndex = 40;
            this.gRecMP3.Text = "Record streams to MP3";
            this.tt.SetToolTip(this.gRecMP3, "Record a copy of your streams as MP3");
            this.gRecMP3.UseVisualStyleBackColor = true;
            this.gRecMP3.CheckedChanged += new System.EventHandler(this.gRecMP3_CheckedChanged);
            // 
            // gRecPCM
            // 
            this.gRecPCM.AutoSize = true;
            this.gRecPCM.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.gRecPCM.Location = new System.Drawing.Point(557, 140);
            this.gRecPCM.Margin = new System.Windows.Forms.Padding(12, 3, 3, 3);
            this.gRecPCM.Name = "gRecPCM";
            this.gRecPCM.Size = new System.Drawing.Size(138, 17);
            this.gRecPCM.TabIndex = 41;
            this.gRecPCM.Text = "Record streams to PCM";
            this.tt.SetToolTip(this.gRecPCM, "Record a copy of your streams as raw PCM audio");
            this.gRecPCM.UseVisualStyleBackColor = true;
            this.gRecPCM.CheckedChanged += new System.EventHandler(this.gRecPCM_CheckedChanged);
            // 
            // gUnavail
            // 
            this.gUnavail.AutoSize = true;
            this.gUnavail.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.gUnavail.Location = new System.Drawing.Point(557, 48);
            this.gUnavail.Margin = new System.Windows.Forms.Padding(12, 3, 3, 3);
            this.gUnavail.Name = "gUnavail";
            this.gUnavail.Size = new System.Drawing.Size(150, 17);
            this.gUnavail.TabIndex = 42;
            this.gUnavail.Text = "Show unavailable devices";
            this.tt.SetToolTip(this.gUnavail, "When listing devices in dropdowns,\r\nalso include those that are disabled.");
            this.gUnavail.UseVisualStyleBackColor = true;
            this.gUnavail.CheckedChanged += new System.EventHandler(this.gUnavail_CheckedChanged);
            // 
            // gRecOGG
            // 
            this.gRecOGG.AutoSize = true;
            this.gRecOGG.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.gRecOGG.Location = new System.Drawing.Point(557, 117);
            this.gRecOGG.Margin = new System.Windows.Forms.Padding(12, 3, 3, 3);
            this.gRecOGG.Name = "gRecOGG";
            this.gRecOGG.Size = new System.Drawing.Size(139, 17);
            this.gRecOGG.TabIndex = 43;
            this.gRecOGG.Text = "Record streams to OGG";
            this.tt.SetToolTip(this.gRecOGG, "Record a copy of your streams as OGG/Vorbis");
            this.gRecOGG.UseVisualStyleBackColor = true;
            this.gRecOGG.CheckedChanged += new System.EventHandler(this.gRecOGG_CheckedChanged);
            // 
            // gAutoconn
            // 
            this.gAutoconn.AutoSize = true;
            this.gAutoconn.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.gAutoconn.Location = new System.Drawing.Point(557, 163);
            this.gAutoconn.Margin = new System.Windows.Forms.Padding(12, 3, 3, 3);
            this.gAutoconn.Name = "gAutoconn";
            this.gAutoconn.Size = new System.Drawing.Size(141, 17);
            this.gAutoconn.TabIndex = 44;
            this.gAutoconn.Text = "On startup, autoconnect";
            this.tt.SetToolTip(this.gAutoconn, "Connect to radio server on startup");
            this.gAutoconn.UseVisualStyleBackColor = true;
            this.gAutoconn.CheckedChanged += new System.EventHandler(this.gAutoconn_CheckedChanged);
            // 
            // gAutohide
            // 
            this.gAutohide.AutoSize = true;
            this.gAutohide.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.gAutohide.Location = new System.Drawing.Point(557, 186);
            this.gAutohide.Margin = new System.Windows.Forms.Padding(12, 3, 3, 3);
            this.gAutohide.Name = "gAutohide";
            this.gAutohide.Size = new System.Drawing.Size(122, 17);
            this.gAutohide.TabIndex = 45;
            this.gAutohide.Text = "On startup, autohide";
            this.tt.SetToolTip(this.gAutohide, "Hide after startup sequence");
            this.gAutohide.UseVisualStyleBackColor = true;
            this.gAutohide.CheckedChanged += new System.EventHandler(this.gAutohide_CheckedChanged);
            // 
            // tt
            // 
            this.tt.ShowAlways = true;
            this.tt.UseAnimation = false;
            this.tt.UseFading = false;
            // 
            // ConfigSC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ClientSize = new System.Drawing.Size(777, 319);
            this.Controls.Add(this.gAutohide);
            this.Controls.Add(this.gAutoconn);
            this.Controls.Add(this.gRecOGG);
            this.Controls.Add(this.gUnavail);
            this.Controls.Add(this.gRecPCM);
            this.Controls.Add(this.gRecMP3);
            this.Controls.Add(this.gSplash);
            this.Controls.Add(this.gTestDevs);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "ConfigSC";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LoopStream Config";
            this.Load += new System.EventHandler(this.ConfigSC_Load);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pMp3.ResumeLayout(false);
            this.pMp3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.pOgg.ResumeLayout(false);
            this.pOgg.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox gOutS;
        private System.Windows.Forms.ComboBox gTwoS;
        private System.Windows.Forms.ComboBox gOneS;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RadioButton gSiren;
        private System.Windows.Forms.RadioButton gIce;
        private System.Windows.Forms.RadioButton gShout;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox gPass;
        private System.Windows.Forms.TextBox gMount;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox gHost;
        private System.Windows.Forms.Button gSave;
        private System.Windows.Forms.Button gCancel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox gLeft;
        private System.Windows.Forms.CheckBox gRight;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox gTestDevs;
        private System.Windows.Forms.CheckBox gSplash;
        private System.Windows.Forms.CheckBox gRecMP3;
        private System.Windows.Forms.CheckBox gRecPCM;
        private System.Windows.Forms.CheckBox gMp3Enable;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox gMp3BitrateV;
        private System.Windows.Forms.RadioButton gMp3Quality;
        private System.Windows.Forms.TextBox gMp3QualityV;
        private System.Windows.Forms.RadioButton gMp3Bitrate;
        private System.Windows.Forms.RadioButton gMp3Stereo;
        private System.Windows.Forms.RadioButton gMp3Mono;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox gRate;
        private System.Windows.Forms.RadioButton gOggStereo;
        private System.Windows.Forms.RadioButton gOggMono;
        private System.Windows.Forms.CheckBox gOggEnable;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox gOggBitrateV;
        private System.Windows.Forms.RadioButton gOggQuality;
        private System.Windows.Forms.TextBox gOggQualityV;
        private System.Windows.Forms.RadioButton gOggBitrate;
        private System.Windows.Forms.Panel pMp3;
        private System.Windows.Forms.Label gOgg;
        private System.Windows.Forms.Label gMp3;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Panel pOgg;
        private System.Windows.Forms.CheckBox gUnavail;
        private System.Windows.Forms.CheckBox gRecOGG;
        private System.Windows.Forms.CheckBox gAutoconn;
        private System.Windows.Forms.CheckBox gAutohide;
        private System.Windows.Forms.ToolTip tt;
    }
}