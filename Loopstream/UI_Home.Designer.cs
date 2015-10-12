namespace LoopStream
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
            this.box_top = new System.Windows.Forms.Panel();
            this.box_top_dark = new System.Windows.Forms.Label();
            this.box_top_light = new System.Windows.Forms.Label();
            this.box_bottom = new System.Windows.Forms.Panel();
            this.box_unused1 = new System.Windows.Forms.Panel();
            this.box_unused1_light = new System.Windows.Forms.Label();
            this.box_unused1_dark = new System.Windows.Forms.Label();
            this.box_bottom_light = new System.Windows.Forms.Label();
            this.box_bottom_dark = new System.Windows.Forms.Label();
            this.box_menu_outer = new System.Windows.Forms.Panel();
            this.box_menu = new System.Windows.Forms.Panel();
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
            this.gMusic = new LoopStream.Verter();
            this.gOut = new LoopStream.Verter();
            this.gMic = new LoopStream.Verter();
            this.gSpeed = new LoopStream.Verter();
            this.box_top_graden = new LoopStream.Graden();
            this.box_bottom_graden = new LoopStream.Graden();
            this.box_sliders.SuspendLayout();
            this.box_top.SuspendLayout();
            this.box_bottom.SuspendLayout();
            this.box_unused1.SuspendLayout();
            this.box_menu_outer.SuspendLayout();
            this.box_menu.SuspendLayout();
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
            this.box_sliders.Size = new System.Drawing.Size(474, 416);
            this.box_sliders.TabIndex = 4;
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
            this.box_top.Size = new System.Drawing.Size(474, 59);
            this.box_top.TabIndex = 10;
            // 
            // box_top_dark
            // 
            this.box_top_dark.BackColor = System.Drawing.SystemColors.ControlDark;
            this.box_top_dark.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.box_top_dark.Location = new System.Drawing.Point(0, 57);
            this.box_top_dark.Name = "box_top_dark";
            this.box_top_dark.Size = new System.Drawing.Size(474, 1);
            this.box_top_dark.TabIndex = 9;
            // 
            // box_top_light
            // 
            this.box_top_light.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.box_top_light.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.box_top_light.Location = new System.Drawing.Point(0, 58);
            this.box_top_light.Name = "box_top_light";
            this.box_top_light.Size = new System.Drawing.Size(474, 1);
            this.box_top_light.TabIndex = 10;
            // 
            // box_bottom
            // 
            this.box_bottom.BackColor = System.Drawing.SystemColors.ControlLight;
            this.box_bottom.Controls.Add(this.box_unused1);
            this.box_bottom.Controls.Add(this.box_bottom_dark);
            this.box_bottom.Controls.Add(this.box_bottom_light);
            this.box_bottom.Controls.Add(this.box_bottom_graden);
            this.box_bottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.box_bottom.Location = new System.Drawing.Point(0, 311);
            this.box_bottom.Name = "box_bottom";
            this.box_bottom.Size = new System.Drawing.Size(474, 105);
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
            this.box_unused1.Size = new System.Drawing.Size(474, 62);
            this.box_unused1.TabIndex = 11;
            this.box_unused1.Visible = false;
            // 
            // box_unused1_light
            // 
            this.box_unused1_light.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.box_unused1_light.Dock = System.Windows.Forms.DockStyle.Top;
            this.box_unused1_light.Location = new System.Drawing.Point(0, 1);
            this.box_unused1_light.Name = "box_unused1_light";
            this.box_unused1_light.Size = new System.Drawing.Size(474, 1);
            this.box_unused1_light.TabIndex = 12;
            // 
            // box_unused1_dark
            // 
            this.box_unused1_dark.BackColor = System.Drawing.SystemColors.ControlDark;
            this.box_unused1_dark.Dock = System.Windows.Forms.DockStyle.Top;
            this.box_unused1_dark.Location = new System.Drawing.Point(0, 0);
            this.box_unused1_dark.Name = "box_unused1_dark";
            this.box_unused1_dark.Size = new System.Drawing.Size(474, 1);
            this.box_unused1_dark.TabIndex = 11;
            // 
            // box_bottom_light
            // 
            this.box_bottom_light.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.box_bottom_light.Dock = System.Windows.Forms.DockStyle.Top;
            this.box_bottom_light.Location = new System.Drawing.Point(0, 0);
            this.box_bottom_light.Name = "box_bottom_light";
            this.box_bottom_light.Size = new System.Drawing.Size(474, 1);
            this.box_bottom_light.TabIndex = 10;
            // 
            // box_bottom_dark
            // 
            this.box_bottom_dark.BackColor = System.Drawing.SystemColors.ControlDark;
            this.box_bottom_dark.Dock = System.Windows.Forms.DockStyle.Top;
            this.box_bottom_dark.Location = new System.Drawing.Point(0, 1);
            this.box_bottom_dark.Name = "box_bottom_dark";
            this.box_bottom_dark.Size = new System.Drawing.Size(474, 1);
            this.box_bottom_dark.TabIndex = 9;
            // 
            // box_menu_outer
            // 
            this.box_menu_outer.BackColor = System.Drawing.SystemColors.ControlLight;
            this.box_menu_outer.Controls.Add(this.box_menu);
            this.box_menu_outer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.box_menu_outer.Location = new System.Drawing.Point(474, 0);
            this.box_menu_outer.Margin = new System.Windows.Forms.Padding(9, 3, 3, 3);
            this.box_menu_outer.Name = "box_menu_outer";
            this.box_menu_outer.Size = new System.Drawing.Size(177, 416);
            this.box_menu_outer.TabIndex = 5;
            // 
            // box_menu
            // 
            this.box_menu.BackColor = System.Drawing.SystemColors.ControlLight;
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
            this.box_menu.Size = new System.Drawing.Size(177, 416);
            this.box_menu.TabIndex = 10;
            // 
            // box_menu_light
            // 
            this.box_menu_light.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.box_menu_light.Dock = System.Windows.Forms.DockStyle.Left;
            this.box_menu_light.Location = new System.Drawing.Point(1, 0);
            this.box_menu_light.Margin = new System.Windows.Forms.Padding(3, 0, 9, 0);
            this.box_menu_light.Name = "box_menu_light";
            this.box_menu_light.Size = new System.Drawing.Size(1, 416);
            this.box_menu_light.TabIndex = 14;
            // 
            // box_menu_dark
            // 
            this.box_menu_dark.BackColor = System.Drawing.SystemColors.ControlDark;
            this.box_menu_dark.Dock = System.Windows.Forms.DockStyle.Left;
            this.box_menu_dark.Location = new System.Drawing.Point(0, 0);
            this.box_menu_dark.Name = "box_menu_dark";
            this.box_menu_dark.Size = new System.Drawing.Size(1, 416);
            this.box_menu_dark.TabIndex = 13;
            // 
            // gConnect
            // 
            this.gConnect.Location = new System.Drawing.Point(19, 17);
            this.gConnect.Margin = new System.Windows.Forms.Padding(8, 8, 8, 3);
            this.gConnect.Name = "gConnect";
            this.gConnect.Size = new System.Drawing.Size(141, 43);
            this.gConnect.TabIndex = 0;
            this.gConnect.Text = "Connect";
            this.gConnect.UseVisualStyleBackColor = true;
            this.gConnect.Click += new System.EventHandler(this.gConnect_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.label3.Location = new System.Drawing.Point(71, 161);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "wasapiDumpTest";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // gSettings
            // 
            this.gSettings.Location = new System.Drawing.Point(19, 66);
            this.gSettings.Margin = new System.Windows.Forms.Padding(8, 3, 8, 3);
            this.gSettings.Name = "gSettings";
            this.gSettings.Size = new System.Drawing.Size(141, 43);
            this.gSettings.TabIndex = 1;
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
            this.gD.TabIndex = 8;
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
            this.gExit.TabIndex = 2;
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
            this.gC.TabIndex = 7;
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
            this.gLoad.TabIndex = 3;
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
            this.gB.TabIndex = 6;
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
            this.gA.TabIndex = 5;
            this.gA.Text = "A";
            this.gA.UseVisualStyleBackColor = true;
            this.gA.Click += new System.EventHandler(this.gPreset_Click);
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
            this.gMusic.TabIndex = 0;
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
            this.gOut.TabIndex = 3;
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
            this.gMic.TabIndex = 1;
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
            this.gSpeed.TabIndex = 2;
            this.gSpeed.timeScale = true;
            this.gSpeed.title = "Speed";
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
            this.box_top_graden.Size = new System.Drawing.Size(474, 59);
            this.box_top_graden.TabIndex = 13;
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
            this.box_bottom_graden.Size = new System.Drawing.Size(474, 105);
            this.box_bottom_graden.TabIndex = 12;
            // 
            // Home
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(651, 416);
            this.Controls.Add(this.box_menu_outer);
            this.Controls.Add(this.box_sliders);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Home";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LoopStream";
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

    }
}

