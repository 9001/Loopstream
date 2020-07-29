namespace Loopstream
{
    partial class UI_WavetailCfg
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
            this.label1 = new System.Windows.Forms.Label();
            this.gPath = new System.Windows.Forms.TextBox();
            this.gBrowse = new System.Windows.Forms.Button();
            this.gDelet = new System.Windows.Forms.CheckBox();
            this.gSave = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.gSampleRate = new System.Windows.Forms.TextBox();
            this.gChannels = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.gBitness = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Source directory";
            // 
            // gPath
            // 
            this.gPath.Location = new System.Drawing.Point(12, 25);
            this.gPath.Name = "gPath";
            this.gPath.Size = new System.Drawing.Size(498, 20);
            this.gPath.TabIndex = 1;
            // 
            // gBrowse
            // 
            this.gBrowse.Location = new System.Drawing.Point(516, 23);
            this.gBrowse.Name = "gBrowse";
            this.gBrowse.Size = new System.Drawing.Size(75, 23);
            this.gBrowse.TabIndex = 2;
            this.gBrowse.Text = "Browse";
            this.gBrowse.UseVisualStyleBackColor = true;
            this.gBrowse.Click += new System.EventHandler(this.gBrowse_Click);
            // 
            // gDelet
            // 
            this.gDelet.AutoSize = true;
            this.gDelet.Location = new System.Drawing.Point(232, 78);
            this.gDelet.Margin = new System.Windows.Forms.Padding(3, 12, 3, 3);
            this.gDelet.Name = "gDelet";
            this.gDelet.Size = new System.Drawing.Size(95, 17);
            this.gDelet.TabIndex = 6;
            this.gDelet.Text = "Delete old files";
            this.gDelet.UseVisualStyleBackColor = true;
            // 
            // gSave
            // 
            this.gSave.Location = new System.Drawing.Point(498, 71);
            this.gSave.Name = "gSave";
            this.gSave.Size = new System.Drawing.Size(93, 29);
            this.gSave.TabIndex = 7;
            this.gSave.Text = "S a v e";
            this.gSave.UseVisualStyleBackColor = true;
            this.gSave.Click += new System.EventHandler(this.gSave_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 60);
            this.label2.Margin = new System.Windows.Forms.Padding(3, 12, 3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Samplerate";
            // 
            // gSampleRate
            // 
            this.gSampleRate.Location = new System.Drawing.Point(12, 76);
            this.gSampleRate.Name = "gSampleRate";
            this.gSampleRate.Size = new System.Drawing.Size(60, 20);
            this.gSampleRate.TabIndex = 3;
            this.gSampleRate.Text = "44100";
            // 
            // gChannels
            // 
            this.gChannels.Location = new System.Drawing.Point(144, 76);
            this.gChannels.Name = "gChannels";
            this.gChannels.Size = new System.Drawing.Size(60, 20);
            this.gChannels.TabIndex = 5;
            this.gChannels.Text = "2";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(144, 60);
            this.label3.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Channels";
            // 
            // gBitness
            // 
            this.gBitness.Enabled = false;
            this.gBitness.Location = new System.Drawing.Point(78, 76);
            this.gBitness.Name = "gBitness";
            this.gBitness.Size = new System.Drawing.Size(60, 20);
            this.gBitness.TabIndex = 4;
            this.gBitness.Text = "16";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(78, 60);
            this.label4.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Bitness";
            // 
            // UI_WavetailCfg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(603, 112);
            this.Controls.Add(this.gBitness);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.gChannels);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.gSampleRate);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.gSave);
            this.Controls.Add(this.gDelet);
            this.Controls.Add(this.gBrowse);
            this.Controls.Add(this.gPath);
            this.Controls.Add(this.label1);
            this.Name = "UI_WavetailCfg";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "UI_WavetailCfg";
            this.Load += new System.EventHandler(this.UI_WavetailCfg_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.UI_WavetailCfg_KeyUp);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox gPath;
        private System.Windows.Forms.Button gBrowse;
        private System.Windows.Forms.CheckBox gDelet;
        private System.Windows.Forms.Button gSave;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox gSampleRate;
        private System.Windows.Forms.TextBox gChannels;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox gBitness;
        private System.Windows.Forms.Label label4;
    }
}