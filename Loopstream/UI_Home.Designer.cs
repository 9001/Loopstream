namespace Loopstream
{
    partial class Home
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
            this.box_sliders = new System.Windows.Forms.Panel();
            this.gMusic = new Loopstream.Verter();
            this.gOut = new Loopstream.Verter();
            this.gMic = new Loopstream.Verter();
            this.gSpeed = new Loopstream.Verter();
            this.box_top = new System.Windows.Forms.Panel();
            this.box_top_dark = new System.Windows.Forms.Label();
            this.box_top_light = new System.Windows.Forms.Label();
            this.box_top_graden = new Loopstream.Graden();
            this.box_bottom = new System.Windows.Forms.Panel();
            this.box_unused1 = new System.Windows.Forms.Panel();
            this.box_unused1_light = new System.Windows.Forms.Label();
            this.box_unused1_dark = new System.Windows.Forms.Label();
            this.box_bottom_dark = new System.Windows.Forms.Label();
            this.box_bottom_light = new System.Windows.Forms.Label();
            this.box_bottom_graden = new Loopstream.Graden();
            this.box_menu_outer = new System.Windows.Forms.Panel();
            this.box_menu = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.gGit = new System.Windows.Forms.Label();
            this.box_menu_light = new System.Windows.Forms.Label();
            this.box_menu_dark = new System.Windows.Forms.Label();
            this.gConnect = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.gSettings = new System.Windows.Forms.Button();
            this.gD = new System.Windows.Forms.Button();
            this.gExit = new System.Windows.Forms.Button();
            this.gC = new System.Windows.Forms.Button();
            this.gLoad = new System.Windows.Forms.Button();
            this.gB = new System.Windows.Forms.Button();
            this.gA = new System.Windows.Forms.Button();
            this.pMessage = new System.Windows.Forms.Panel();
            this.gLowQ = new System.Windows.Forms.Label();
            this.box_sliders.SuspendLayout();
            this.box_top.SuspendLayout();
            this.box_bottom.SuspendLayout();
            this.box_unused1.SuspendLayout();
            this.box_menu_outer.SuspendLayout();
            this.box_menu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.pMessage.SuspendLayout();
            this.SuspendLayout();
            // 
            // box_sliders
            // 
            this.box_sliders.Controls.Add(this.gMusic);
            this.box_sliders.Controls.Add(this.gOut);
            this.box_sliders.Controls.Add(this.gMic);
            this.box_sliders.Controls.Add(this.gSpeed);
            this.box_sliders.Controls.Add(this.box_top);
            this.box_sliders.Controls.Add(this.box_bottom);
            this.box_sliders.Dock = System.Windows.Forms.DockStyle.Left;
            this.box_sliders.Location = new System.Drawing.Point(0, 0);
            this.box_sliders.Margin = new System.Windows.Forms.Padding(3, 3, 9, 3);
            this.box_sliders.Name = "box_sliders";
            this.box_sliders.Size = new System.Drawing.Size(476, 412);
            this.box_sliders.TabIndex = 4;
            // 
            // gMusic
            // 
            this.gMusic.BackColor = System.Drawing.SystemColors.ControlLight;
            this.gMusic.canToggle = true;
            this.gMusic.enabled = true;
            this.gMusic.level = 0;
            this.gMusic.Location = new System.Drawing.Point(17, 17);
            this.gMusic.Margin = new System.Windows.Forms.Padding(8);
            this.gMusic.Name = "gMusic";
            this.gMusic.Size = new System.Drawing.Size(98, 382);
            this.gMusic.TabIndex = 9;
            this.gMusic.timeScale = false;
            this.gMusic.title = "Music";
            // 
            // gOut
            // 
            this.gOut.BackColor = System.Drawing.SystemColors.ControlLight;
            this.gOut.canToggle = true;
            this.gOut.enabled = true;
            this.gOut.level = 0;
            this.gOut.Location = new System.Drawing.Point(359, 17);
            this.gOut.Margin = new System.Windows.Forms.Padding(8);
            this.gOut.Name = "gOut";
            this.gOut.Size = new System.Drawing.Size(98, 382);
            this.gOut.TabIndex = 12;
            this.gOut.timeScale = false;
            this.gOut.title = "OUT";
            // 
            // gMic
            // 
            this.gMic.BackColor = System.Drawing.SystemColors.ControlLight;
            this.gMic.canToggle = true;
            this.gMic.enabled = true;
            this.gMic.level = 0;
            this.gMic.Location = new System.Drawing.Point(131, 17);
            this.gMic.Margin = new System.Windows.Forms.Padding(8);
            this.gMic.Name = "gMic";
            this.gMic.Size = new System.Drawing.Size(98, 382);
            this.gMic.TabIndex = 10;
            this.gMic.timeScale = false;
            this.gMic.title = "mic";
            // 
            // gSpeed
            // 
            this.gSpeed.BackColor = System.Drawing.SystemColors.ControlLight;
            this.gSpeed.canToggle = false;
            this.gSpeed.enabled = true;
            this.gSpeed.level = 0;
            this.gSpeed.Location = new System.Drawing.Point(245, 17);
            this.gSpeed.Margin = new System.Windows.Forms.Padding(8);
            this.gSpeed.Name = "gSpeed";
            this.gSpeed.Size = new System.Drawing.Size(98, 382);
            this.gSpeed.TabIndex = 11;
            this.gSpeed.timeScale = true;
            this.gSpeed.title = "Speed";
            // 
            // box_top
            // 
            this.box_top.BackColor = System.Drawing.SystemColors.ControlLight;
            this.box_top.Controls.Add(this.box_top_dark);
            this.box_top.Controls.Add(this.box_top_light);
            this.box_top.Controls.Add(this.box_top_graden);
            this.box_top.Dock = System.Windows.Forms.DockStyle.Top;
            this.box_top.Location = new System.Drawing.Point(0, 0);
            this.box_top.Name = "box_top";
            this.box_top.Size = new System.Drawing.Size(476, 59);
            this.box_top.TabIndex = 10;
            // 
            // box_top_dark
            // 
            this.box_top_dark.BackColor = System.Drawing.SystemColors.ControlDark;
            this.box_top_dark.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.box_top_dark.Location = new System.Drawing.Point(0, 57);
            this.box_top_dark.Name = "box_top_dark";
            this.box_top_dark.Size = new System.Drawing.Size(476, 1);
            this.box_top_dark.TabIndex = 9;
            // 
            // box_top_light
            // 
            this.box_top_light.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.box_top_light.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.box_top_light.Location = new System.Drawing.Point(0, 58);
            this.box_top_light.Name = "box_top_light";
            this.box_top_light.Size = new System.Drawing.Size(476, 1);
            this.box_top_light.TabIndex = 10;
            // 
            // box_top_graden
            // 
            this.box_top_graden.co = 1D;
            this.box_top_graden.colorA = System.Drawing.SystemColors.Control;
            this.box_top_graden.colorB = System.Drawing.SystemColors.ControlLight;
            this.box_top_graden.Direction = false;
            this.box_top_graden.Dock = System.Windows.Forms.DockStyle.Fill;
            this.box_top_graden.Location = new System.Drawing.Point(0, 0);
            this.box_top_graden.Name = "box_top_graden";
            this.box_top_graden.Size = new System.Drawing.Size(476, 59);
            this.box_top_graden.TabIndex = 13;
            // 
            // box_bottom
            // 
            this.box_bottom.BackColor = System.Drawing.SystemColors.ControlLight;
            this.box_bottom.Controls.Add(this.box_unused1);
            this.box_bottom.Controls.Add(this.box_bottom_dark);
            this.box_bottom.Controls.Add(this.box_bottom_light);
            this.box_bottom.Controls.Add(this.box_bottom_graden);
            this.box_bottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.box_bottom.Location = new System.Drawing.Point(0, 307);
            this.box_bottom.Name = "box_bottom";
            this.box_bottom.Size = new System.Drawing.Size(476, 105);
            this.box_bottom.TabIndex = 11;
            // 
            // box_unused1
            // 
            this.box_unused1.BackColor = System.Drawing.SystemColors.Control;
            this.box_unused1.Controls.Add(this.box_unused1_light);
            this.box_unused1.Controls.Add(this.box_unused1_dark);
            this.box_unused1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.box_unused1.Location = new System.Drawing.Point(0, 43);
            this.box_unused1.Name = "box_unused1";
            this.box_unused1.Size = new System.Drawing.Size(476, 62);
            this.box_unused1.TabIndex = 11;
            this.box_unused1.Visible = false;
            // 
            // box_unused1_light
            // 
            this.box_unused1_light.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.box_unused1_light.Dock = System.Windows.Forms.DockStyle.Top;
            this.box_unused1_light.Location = new System.Drawing.Point(0, 1);
            this.box_unused1_light.Name = "box_unused1_light";
            this.box_unused1_light.Size = new System.Drawing.Size(476, 1);
            this.box_unused1_light.TabIndex = 12;
            // 
            // box_unused1_dark
            // 
            this.box_unused1_dark.BackColor = System.Drawing.SystemColors.ControlDark;
            this.box_unused1_dark.Dock = System.Windows.Forms.DockStyle.Top;
            this.box_unused1_dark.Location = new System.Drawing.Point(0, 0);
            this.box_unused1_dark.Name = "box_unused1_dark";
            this.box_unused1_dark.Size = new System.Drawing.Size(476, 1);
            this.box_unused1_dark.TabIndex = 11;
            // 
            // box_bottom_dark
            // 
            this.box_bottom_dark.BackColor = System.Drawing.SystemColors.ControlDark;
            this.box_bottom_dark.Dock = System.Windows.Forms.DockStyle.Top;
            this.box_bottom_dark.Location = new System.Drawing.Point(0, 1);
            this.box_bottom_dark.Name = "box_bottom_dark";
            this.box_bottom_dark.Size = new System.Drawing.Size(476, 1);
            this.box_bottom_dark.TabIndex = 9;
            // 
            // box_bottom_light
            // 
            this.box_bottom_light.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.box_bottom_light.Dock = System.Windows.Forms.DockStyle.Top;
            this.box_bottom_light.Location = new System.Drawing.Point(0, 0);
            this.box_bottom_light.Name = "box_bottom_light";
            this.box_bottom_light.Size = new System.Drawing.Size(476, 1);
            this.box_bottom_light.TabIndex = 10;
            // 
            // box_bottom_graden
            // 
            this.box_bottom_graden.co = 1D;
            this.box_bottom_graden.colorA = System.Drawing.SystemColors.Control;
            this.box_bottom_graden.colorB = System.Drawing.SystemColors.ControlLight;
            this.box_bottom_graden.Direction = true;
            this.box_bottom_graden.Dock = System.Windows.Forms.DockStyle.Fill;
            this.box_bottom_graden.Location = new System.Drawing.Point(0, 0);
            this.box_bottom_graden.Name = "box_bottom_graden";
            this.box_bottom_graden.Size = new System.Drawing.Size(476, 105);
            this.box_bottom_graden.TabIndex = 12;
            // 
            // box_menu_outer
            // 
            this.box_menu_outer.BackColor = System.Drawing.SystemColors.ControlLight;
            this.box_menu_outer.Controls.Add(this.box_menu);
            this.box_menu_outer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.box_menu_outer.Location = new System.Drawing.Point(476, 0);
            this.box_menu_outer.Margin = new System.Windows.Forms.Padding(9, 3, 3, 3);
            this.box_menu_outer.Name = "box_menu_outer";
            this.box_menu_outer.Size = new System.Drawing.Size(177, 412);
            this.box_menu_outer.TabIndex = 5;
            // 
            // box_menu
            // 
            this.box_menu.BackColor = System.Drawing.SystemColors.ControlLight;
            this.box_menu.Controls.Add(this.pictureBox1);
            this.box_menu.Controls.Add(this.gGit);
            this.box_menu.Controls.Add(this.box_menu_light);
            this.box_menu.Controls.Add(this.box_menu_dark);
            this.box_menu.Controls.Add(this.gConnect);
            this.box_menu.Controls.Add(this.label3);
            this.box_menu.Controls.Add(this.gSettings);
            this.box_menu.Controls.Add(this.gD);
            this.box_menu.Controls.Add(this.gExit);
            this.box_menu.Controls.Add(this.gC);
            this.box_menu.Controls.Add(this.gLoad);
            this.box_menu.Controls.Add(this.gB);
            this.box_menu.Controls.Add(this.gA);
            this.box_menu.Dock = System.Windows.Forms.DockStyle.Right;
            this.box_menu.Location = new System.Drawing.Point(0, 0);
            this.box_menu.Name = "box_menu";
            this.box_menu.Size = new System.Drawing.Size(177, 412);
            this.box_menu.TabIndex = 10;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::Loopstream.Properties.Resources.logo;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pictureBox1.Location = new System.Drawing.Point(2, 177);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(174, 95);
            this.pictureBox1.TabIndex = 16;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseClick);
            // 
            // gGit
            // 
            this.gGit.AutoSize = true;
            this.gGit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.gGit.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.gGit.Location = new System.Drawing.Point(17, 161);
            this.gGit.Name = "gGit";
            this.gGit.Size = new System.Drawing.Size(40, 13);
            this.gGit.TabIndex = 15;
            this.gGit.Text = "GitHub";
            this.gGit.Click += new System.EventHandler(this.gGit_Click);
            // 
            // box_menu_light
            // 
            this.box_menu_light.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.box_menu_light.Dock = System.Windows.Forms.DockStyle.Left;
            this.box_menu_light.Location = new System.Drawing.Point(1, 0);
            this.box_menu_light.Margin = new System.Windows.Forms.Padding(3, 0, 9, 0);
            this.box_menu_light.Name = "box_menu_light";
            this.box_menu_light.Size = new System.Drawing.Size(1, 412);
            this.box_menu_light.TabIndex = 14;
            // 
            // box_menu_dark
            // 
            this.box_menu_dark.BackColor = System.Drawing.SystemColors.ControlDark;
            this.box_menu_dark.Dock = System.Windows.Forms.DockStyle.Left;
            this.box_menu_dark.Location = new System.Drawing.Point(0, 0);
            this.box_menu_dark.Name = "box_menu_dark";
            this.box_menu_dark.Size = new System.Drawing.Size(1, 412);
            this.box_menu_dark.TabIndex = 13;
            // 
            // gConnect
            // 
            this.gConnect.Location = new System.Drawing.Point(19, 17);
            this.gConnect.Margin = new System.Windows.Forms.Padding(8, 8, 8, 3);
            this.gConnect.Name = "gConnect";
            this.gConnect.Size = new System.Drawing.Size(141, 43);
            this.gConnect.TabIndex = 1;
            this.gConnect.Text = "Connect";
            this.gConnect.UseVisualStyleBackColor = true;
            this.gConnect.Click += new System.EventHandler(this.gConnect_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.label3.Location = new System.Drawing.Point(103, 161);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "test wasapi";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // gSettings
            // 
            this.gSettings.Location = new System.Drawing.Point(19, 66);
            this.gSettings.Margin = new System.Windows.Forms.Padding(8, 3, 8, 3);
            this.gSettings.Name = "gSettings";
            this.gSettings.Size = new System.Drawing.Size(141, 43);
            this.gSettings.TabIndex = 2;
            this.gSettings.Text = "Settings";
            this.gSettings.UseVisualStyleBackColor = true;
            this.gSettings.Click += new System.EventHandler(this.gSettings_Click);
            // 
            // gD
            // 
            this.gD.Location = new System.Drawing.Point(91, 317);
            this.gD.Margin = new System.Windows.Forms.Padding(3, 0, 8, 0);
            this.gD.Name = "gD";
            this.gD.Size = new System.Drawing.Size(69, 39);
            this.gD.TabIndex = 7;
            this.gD.Text = "D";
            this.gD.UseVisualStyleBackColor = true;
            this.gD.Click += new System.EventHandler(this.gPreset_Click);
            // 
            // gExit
            // 
            this.gExit.Location = new System.Drawing.Point(19, 115);
            this.gExit.Margin = new System.Windows.Forms.Padding(8, 3, 8, 3);
            this.gExit.Name = "gExit";
            this.gExit.Size = new System.Drawing.Size(141, 43);
            this.gExit.TabIndex = 3;
            this.gExit.Text = "Exit";
            this.gExit.UseVisualStyleBackColor = true;
            this.gExit.Click += new System.EventHandler(this.gExit_Click);
            // 
            // gC
            // 
            this.gC.Location = new System.Drawing.Point(19, 317);
            this.gC.Margin = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.gC.Name = "gC";
            this.gC.Size = new System.Drawing.Size(69, 39);
            this.gC.TabIndex = 6;
            this.gC.Text = "C";
            this.gC.UseVisualStyleBackColor = true;
            this.gC.Click += new System.EventHandler(this.gPreset_Click);
            // 
            // gLoad
            // 
            this.gLoad.Location = new System.Drawing.Point(19, 359);
            this.gLoad.Margin = new System.Windows.Forms.Padding(8, 3, 8, 8);
            this.gLoad.Name = "gLoad";
            this.gLoad.Size = new System.Drawing.Size(141, 40);
            this.gLoad.TabIndex = 8;
            this.gLoad.Text = "Load preset";
            this.gLoad.UseVisualStyleBackColor = true;
            this.gLoad.Click += new System.EventHandler(this.gLoad_Click);
            // 
            // gB
            // 
            this.gB.Location = new System.Drawing.Point(91, 275);
            this.gB.Margin = new System.Windows.Forms.Padding(3, 0, 8, 3);
            this.gB.Name = "gB";
            this.gB.Size = new System.Drawing.Size(69, 39);
            this.gB.TabIndex = 5;
            this.gB.Text = "B";
            this.gB.UseVisualStyleBackColor = true;
            this.gB.Click += new System.EventHandler(this.gPreset_Click);
            // 
            // gA
            // 
            this.gA.Location = new System.Drawing.Point(19, 275);
            this.gA.Margin = new System.Windows.Forms.Padding(8, 0, 0, 3);
            this.gA.Name = "gA";
            this.gA.Size = new System.Drawing.Size(69, 39);
            this.gA.TabIndex = 4;
            this.gA.Text = "A";
            this.gA.UseVisualStyleBackColor = true;
            this.gA.Click += new System.EventHandler(this.gPreset_Click);
            // 
            // pMessage
            // 
            this.pMessage.Controls.Add(this.gLowQ);
            this.pMessage.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pMessage.Location = new System.Drawing.Point(0, 412);
            this.pMessage.Name = "pMessage";
            this.pMessage.Size = new System.Drawing.Size(653, 4);
            this.pMessage.TabIndex = 17;
            this.pMessage.Visible = false;
            // 
            // gLowQ
            // 
            this.gLowQ.BackColor = System.Drawing.Color.Maroon;
            this.gLowQ.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gLowQ.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gLowQ.ForeColor = System.Drawing.Color.Gold;
            this.gLowQ.Location = new System.Drawing.Point(0, 0);
            this.gLowQ.Name = "gLowQ";
            this.gLowQ.Size = new System.Drawing.Size(653, 4);
            this.gLowQ.TabIndex = 0;
            this.gLowQ.Text = "Resampler  enabled :     STREAMING  IN  LOW  QUALITY  MODE";
            this.gLowQ.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.gLowQ.Click += new System.EventHandler(this.gLowQ_Click);
            // 
            // Home
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(653, 416);
            this.Controls.Add(this.box_menu_outer);
            this.Controls.Add(this.box_sliders);
            this.Controls.Add(this.pMessage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Home";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Loopstream";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Move += new System.EventHandler(this.Home_Move);
            this.box_sliders.ResumeLayout(false);
            this.box_top.ResumeLayout(false);
            this.box_bottom.ResumeLayout(false);
            this.box_unused1.ResumeLayout(false);
            this.box_menu_outer.ResumeLayout(false);
            this.box_menu.ResumeLayout(false);
            this.box_menu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.pMessage.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Verter gMusic;
        private Verter gMic;
        private Verter gSpeed;
        private Verter gOut;
        private System.Windows.Forms.Panel box_sliders;
        private System.Windows.Forms.Panel box_menu_outer;
        private System.Windows.Forms.Button gExit;
        private System.Windows.Forms.Button gSettings;
        private System.Windows.Forms.Button gConnect;
        private System.Windows.Forms.Button gD;
        private System.Windows.Forms.Button gC;
        private System.Windows.Forms.Button gB;
        private System.Windows.Forms.Button gA;
        private System.Windows.Forms.Button gLoad;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel box_top;
        private System.Windows.Forms.Label box_top_dark;
        private System.Windows.Forms.Label box_top_light;
        private System.Windows.Forms.Panel box_bottom;
        private System.Windows.Forms.Label box_bottom_light;
        private System.Windows.Forms.Label box_bottom_dark;
        private System.Windows.Forms.Panel box_unused1;
        private System.Windows.Forms.Label box_unused1_light;
        private System.Windows.Forms.Label box_unused1_dark;
        private System.Windows.Forms.Panel box_menu;
        private System.Windows.Forms.Label box_menu_light;
        private System.Windows.Forms.Label box_menu_dark;
        private Graden box_top_graden;
        private Graden box_bottom_graden;
        private System.Windows.Forms.Label gGit;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel pMessage;
        private System.Windows.Forms.Label gLowQ;

    }
}

