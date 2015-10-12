namespace Loopstream
{
    partial class UI_Status
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
            this.mp3 = new System.Windows.Forms.Label();
            this.ogg = new System.Windows.Forms.Label();
            this.tag = new System.Windows.Forms.Label();
            this.med = new System.Windows.Forms.Label();
            this.pcm = new System.Windows.Forms.Label();
            this.mix = new System.Windows.Forms.Label();
            this.app = new System.Windows.Forms.Label();
            this.now = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // mp3
            // 
            this.mp3.AutoSize = true;
            this.mp3.Location = new System.Drawing.Point(17, 69);
            this.mp3.Margin = new System.Windows.Forms.Padding(8, 8, 0, 0);
            this.mp3.Name = "mp3";
            this.mp3.Size = new System.Drawing.Size(424, 18);
            this.mp3.TabIndex = 1;
            this.mp3.Text = "mp3  2013-08-24 17:14:93  #71824  hello i am message";
            this.mp3.Click += new System.EventHandler(this.mp3_Click);
            // 
            // ogg
            // 
            this.ogg.AutoSize = true;
            this.ogg.Location = new System.Drawing.Point(17, 43);
            this.ogg.Margin = new System.Windows.Forms.Padding(8, 8, 0, 0);
            this.ogg.Name = "ogg";
            this.ogg.Size = new System.Drawing.Size(424, 18);
            this.ogg.TabIndex = 3;
            this.ogg.Text = "ogg  2013-08-24 17:14:93  #71824  hello i am message";
            this.ogg.Click += new System.EventHandler(this.ogg_Click);
            // 
            // tag
            // 
            this.tag.AutoSize = true;
            this.tag.Location = new System.Drawing.Point(17, 174);
            this.tag.Margin = new System.Windows.Forms.Padding(8, 8, 0, 0);
            this.tag.Name = "tag";
            this.tag.Size = new System.Drawing.Size(424, 18);
            this.tag.TabIndex = 6;
            this.tag.Text = "tag  2013-08-24 17:14:93  #71824  hello i am message";
            this.tag.Click += new System.EventHandler(this.tag_Click);
            // 
            // med
            // 
            this.med.AutoSize = true;
            this.med.Location = new System.Drawing.Point(17, 121);
            this.med.Margin = new System.Windows.Forms.Padding(8, 8, 0, 0);
            this.med.Name = "med";
            this.med.Size = new System.Drawing.Size(424, 18);
            this.med.TabIndex = 5;
            this.med.Text = "med  2013-08-24 17:14:93  #71824  hello i am message";
            this.med.Click += new System.EventHandler(this.med_Click);
            // 
            // pcm
            // 
            this.pcm.AutoSize = true;
            this.pcm.Location = new System.Drawing.Point(17, 95);
            this.pcm.Margin = new System.Windows.Forms.Padding(8, 8, 0, 0);
            this.pcm.Name = "pcm";
            this.pcm.Size = new System.Drawing.Size(424, 18);
            this.pcm.TabIndex = 4;
            this.pcm.Text = "pcm  2013-08-24 17:14:93  #71824  hello i am message";
            this.pcm.Click += new System.EventHandler(this.pcm_Click);
            // 
            // mix
            // 
            this.mix.AutoSize = true;
            this.mix.Location = new System.Drawing.Point(17, 148);
            this.mix.Margin = new System.Windows.Forms.Padding(8, 8, 0, 0);
            this.mix.Name = "mix";
            this.mix.Size = new System.Drawing.Size(424, 18);
            this.mix.TabIndex = 7;
            this.mix.Text = "mix  2013-08-24 17:14:93  #71824  hello i am message";
            this.mix.Click += new System.EventHandler(this.mix_Click);
            // 
            // app
            // 
            this.app.AutoSize = true;
            this.app.Location = new System.Drawing.Point(17, 17);
            this.app.Margin = new System.Windows.Forms.Padding(8, 8, 0, 0);
            this.app.Name = "app";
            this.app.Size = new System.Drawing.Size(424, 18);
            this.app.TabIndex = 8;
            this.app.Text = "app  2013-08-24 17:14:93  #71824  hello i am message";
            this.app.Click += new System.EventHandler(this.app_Click);
            // 
            // now
            // 
            this.now.AutoSize = true;
            this.now.Location = new System.Drawing.Point(17, 200);
            this.now.Margin = new System.Windows.Forms.Padding(8, 8, 0, 0);
            this.now.Name = "now";
            this.now.Size = new System.Drawing.Size(288, 18);
            this.now.TabIndex = 9;
            this.now.Text = "---  2013-08-24 17:14:93  ---------";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(17, 247);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(482, 32);
            this.button1.TabIndex = 10;
            this.button1.Text = "copy recent debug output to clipboard (for pastebin)";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // UI_Status
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(653, 291);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.now);
            this.Controls.Add(this.app);
            this.Controls.Add(this.mix);
            this.Controls.Add(this.tag);
            this.Controls.Add(this.med);
            this.Controls.Add(this.pcm);
            this.Controls.Add(this.ogg);
            this.Controls.Add(this.mp3);
            this.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.Name = "UI_Status";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "UI_Status";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.UI_Status_FormClosing);
            this.Load += new System.EventHandler(this.UI_Status_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label mp3;
        private System.Windows.Forms.Label ogg;
        private System.Windows.Forms.Label tag;
        private System.Windows.Forms.Label med;
        private System.Windows.Forms.Label pcm;
        private System.Windows.Forms.Label mix;
        private System.Windows.Forms.Label app;
        private System.Windows.Forms.Label now;
        private System.Windows.Forms.Button button1;
    }
}