namespace Loopstream
{
    partial class UI_Exception
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.gRestart = new System.Windows.Forms.Button();
            this.gDesc = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.gIgnAlways = new System.Windows.Forms.Button();
            this.gIgnOnce = new System.Windows.Forms.Button();
            this.gcSend = new System.Windows.Forms.CheckBox();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.label3 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Loopstream.Properties.Resources.logo;
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(156, 85);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(448, 109);
            this.panel1.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(180, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(268, 109);
            this.panel2.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(268, 109);
            this.label1.TabIndex = 1;
            this.label1.Text = "Y o u   b r o k e   i t";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // gRestart
            // 
            this.gRestart.Enabled = false;
            this.gRestart.Location = new System.Drawing.Point(240, 12);
            this.gRestart.Margin = new System.Windows.Forms.Padding(3, 9, 3, 3);
            this.gRestart.Name = "gRestart";
            this.gRestart.Size = new System.Drawing.Size(196, 38);
            this.gRestart.TabIndex = 2;
            this.gRestart.Text = "R e s t a r t   ( recommended )";
            this.gRestart.UseVisualStyleBackColor = true;
            this.gRestart.Click += new System.EventHandler(this.gSend_Click);
            // 
            // gDesc
            // 
            this.gDesc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gDesc.Enabled = false;
            this.gDesc.Location = new System.Drawing.Point(12, 0);
            this.gDesc.Multiline = true;
            this.gDesc.Name = "gDesc";
            this.gDesc.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.gDesc.Size = new System.Drawing.Size(424, 209);
            this.gDesc.TabIndex = 4;
            this.gDesc.Text = "Please wait, gathering error information ...";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label2.Location = new System.Drawing.Point(12, 98);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(403, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Crash  information  is  fairly  anonymous,  and  your  password  will  not  be  i" +
    "ncluded.";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.gIgnAlways);
            this.panel3.Controls.Add(this.gIgnOnce);
            this.panel3.Controls.Add(this.gRestart);
            this.panel3.Controls.Add(this.gcSend);
            this.panel3.Controls.Add(this.linkLabel1);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 109);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(448, 164);
            this.panel3.TabIndex = 6;
            // 
            // gIgnAlways
            // 
            this.gIgnAlways.Enabled = false;
            this.gIgnAlways.Location = new System.Drawing.Point(111, 12);
            this.gIgnAlways.Margin = new System.Windows.Forms.Padding(3, 9, 3, 3);
            this.gIgnAlways.Name = "gIgnAlways";
            this.gIgnAlways.Size = new System.Drawing.Size(123, 38);
            this.gIgnAlways.TabIndex = 10;
            this.gIgnAlways.Text = "ignore until restart";
            this.gIgnAlways.UseVisualStyleBackColor = true;
            this.gIgnAlways.Click += new System.EventHandler(this.gIgnAlways_Click);
            // 
            // gIgnOnce
            // 
            this.gIgnOnce.Enabled = false;
            this.gIgnOnce.Location = new System.Drawing.Point(12, 12);
            this.gIgnOnce.Margin = new System.Windows.Forms.Padding(3, 9, 3, 3);
            this.gIgnOnce.Name = "gIgnOnce";
            this.gIgnOnce.Size = new System.Drawing.Size(93, 38);
            this.gIgnOnce.TabIndex = 9;
            this.gIgnOnce.Text = "ignore once";
            this.gIgnOnce.UseVisualStyleBackColor = true;
            this.gIgnOnce.Click += new System.EventHandler(this.gIgnOnce_Click);
            // 
            // gcSend
            // 
            this.gcSend.AutoSize = true;
            this.gcSend.Checked = true;
            this.gcSend.CheckState = System.Windows.Forms.CheckState.Checked;
            this.gcSend.Location = new System.Drawing.Point(13, 78);
            this.gcSend.Margin = new System.Windows.Forms.Padding(4, 6, 3, 3);
            this.gcSend.Name = "gcSend";
            this.gcSend.Size = new System.Drawing.Size(244, 17);
            this.gcSend.TabIndex = 8;
            this.gcSend.Text = "Also send crash information to ed   ( big thank)";
            this.gcSend.UseVisualStyleBackColor = true;
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(80, 114);
            this.linkLabel1.Margin = new System.Windows.Forms.Padding(0, 3, 3, 0);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(220, 13);
            this.linkLabel1.TabIndex = 7;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "<PATH TO CRASH REPORT GOES HERE>";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label3.Location = new System.Drawing.Point(12, 114);
            this.label3.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Read it here:";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.gDesc);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 273);
            this.panel4.Name = "panel4";
            this.panel4.Padding = new System.Windows.Forms.Padding(12, 0, 12, 8);
            this.panel4.Size = new System.Drawing.Size(448, 217);
            this.panel4.TabIndex = 7;
            // 
            // UI_Exception
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(448, 490);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "UI_Exception";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Crash";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.UI_Exception_FormClosing);
            this.Load += new System.EventHandler(this.UI_Exception_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button gRestart;
        private System.Windows.Forms.TextBox gDesc;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox gcSend;
        private System.Windows.Forms.Button gIgnAlways;
        private System.Windows.Forms.Button gIgnOnce;
    }
}