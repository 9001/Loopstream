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
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.gD = new System.Windows.Forms.Button();
            this.gC = new System.Windows.Forms.Button();
            this.gB = new System.Windows.Forms.Button();
            this.gA = new System.Windows.Forms.Button();
            this.gLoad = new System.Windows.Forms.Button();
            this.gExit = new System.Windows.Forms.Button();
            this.gSettings = new System.Windows.Forms.Button();
            this.gConnect = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.gMusic = new LoopStream.Verter();
            this.gOut = new LoopStream.Verter();
            this.gMic = new LoopStream.Verter();
            this.gSpeed = new LoopStream.Verter();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel6.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gMusic);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.gOut);
            this.panel1.Controls.Add(this.gMic);
            this.panel1.Controls.Add(this.gSpeed);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 3, 9, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(497, 416);
            this.panel1.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label2.Dock = System.Windows.Forms.DockStyle.Right;
            this.label2.Location = new System.Drawing.Point(495, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(1, 253);
            this.label2.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.label1.Dock = System.Windows.Forms.DockStyle.Right;
            this.label1.Location = new System.Drawing.Point(496, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(1, 253);
            this.label1.TabIndex = 4;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panel2.Controls.Add(this.panel6);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(497, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(9, 3, 3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(217, 416);
            this.panel2.TabIndex = 5;
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
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panel3.Controls.Add(this.label4);
            this.panel3.Controls.Add(this.label6);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(497, 59);
            this.panel3.TabIndex = 10;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.label6.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label6.Location = new System.Drawing.Point(0, 58);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(497, 1);
            this.label6.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.SystemColors.ControlDark;
            this.label4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label4.Location = new System.Drawing.Point(0, 57);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(497, 1);
            this.label4.TabIndex = 9;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panel4.Controls.Add(this.panel5);
            this.panel4.Controls.Add(this.label9);
            this.panel4.Controls.Add(this.label8);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(0, 312);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(497, 104);
            this.panel4.TabIndex = 11;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.SystemColors.ControlDark;
            this.label8.Dock = System.Windows.Forms.DockStyle.Top;
            this.label8.Location = new System.Drawing.Point(0, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(497, 1);
            this.label8.TabIndex = 9;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.label9.Dock = System.Windows.Forms.DockStyle.Top;
            this.label9.Location = new System.Drawing.Point(0, 1);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(497, 1);
            this.label9.TabIndex = 10;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.SystemColors.Control;
            this.panel5.Controls.Add(this.label5);
            this.panel5.Controls.Add(this.label7);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel5.Location = new System.Drawing.Point(0, 42);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(497, 62);
            this.panel5.TabIndex = 11;
            this.panel5.Visible = false;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.label5.Dock = System.Windows.Forms.DockStyle.Top;
            this.label5.Location = new System.Drawing.Point(0, 1);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(497, 1);
            this.label5.TabIndex = 12;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.SystemColors.ControlDark;
            this.label7.Dock = System.Windows.Forms.DockStyle.Top;
            this.label7.Location = new System.Drawing.Point(0, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(497, 1);
            this.label7.TabIndex = 11;
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.SystemColors.Control;
            this.panel6.Controls.Add(this.label10);
            this.panel6.Controls.Add(this.label11);
            this.panel6.Controls.Add(this.gConnect);
            this.panel6.Controls.Add(this.label3);
            this.panel6.Controls.Add(this.gSettings);
            this.panel6.Controls.Add(this.gD);
            this.panel6.Controls.Add(this.gExit);
            this.panel6.Controls.Add(this.gC);
            this.panel6.Controls.Add(this.gLoad);
            this.panel6.Controls.Add(this.gB);
            this.panel6.Controls.Add(this.gA);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel6.Location = new System.Drawing.Point(40, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(177, 416);
            this.panel6.TabIndex = 10;
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.label10.Dock = System.Windows.Forms.DockStyle.Left;
            this.label10.Location = new System.Drawing.Point(1, 0);
            this.label10.Margin = new System.Windows.Forms.Padding(3, 0, 9, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(1, 416);
            this.label10.TabIndex = 14;
            // 
            // label11
            // 
            this.label11.BackColor = System.Drawing.SystemColors.ControlDark;
            this.label11.Dock = System.Windows.Forms.DockStyle.Left;
            this.label11.Location = new System.Drawing.Point(0, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(1, 416);
            this.label11.TabIndex = 13;
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
            // Home
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(714, 416);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "Home";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LoopStream";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Verter gMusic;
        private Verter gMic;
        private Verter gSpeed;
        private Verter gOut;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button gExit;
        private System.Windows.Forms.Button gSettings;
        private System.Windows.Forms.Button gConnect;
        private System.Windows.Forms.Button gD;
        private System.Windows.Forms.Button gC;
        private System.Windows.Forms.Button gB;
        private System.Windows.Forms.Button gA;
        private System.Windows.Forms.Button gLoad;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;

    }
}

