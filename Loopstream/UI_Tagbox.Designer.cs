namespace Loopstream
{
    partial class UI_Tagbox
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
            this.gPic = new System.Windows.Forms.PictureBox();
            this.gTags = new Loopstream.HintedLabel();
            ((System.ComponentModel.ISupportInitialize)(this.gPic)).BeginInit();
            this.SuspendLayout();
            // 
            // gPic
            // 
            this.gPic.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gPic.Location = new System.Drawing.Point(0, 0);
            this.gPic.Name = "gPic";
            this.gPic.Size = new System.Drawing.Size(667, 204);
            this.gPic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.gPic.TabIndex = 1;
            this.gPic.TabStop = false;
            this.gPic.Click += new System.EventHandler(this.gTags_Click);
            // 
            // gTags
            // 
            this.gTags.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gTags.ForeColor = System.Drawing.Color.White;
            this.gTags.Hinting = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            this.gTags.Location = new System.Drawing.Point(0, 0);
            this.gTags.Name = "gTags";
            this.gTags.Padding = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.gTags.Size = new System.Drawing.Size(667, 204);
            this.gTags.TabIndex = 0;
            this.gTags.Text = "label1";
            this.gTags.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.gTags.UseMnemonic = false;
            this.gTags.Click += new System.EventHandler(this.gTags_Click);
            // 
            // UI_Tagbox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(667, 204);
            this.Controls.Add(this.gPic);
            this.Controls.Add(this.gTags);
            this.DoubleBuffered = true;
            this.Name = "UI_Tagbox";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "tags (click them for settings)";
            this.Load += new System.EventHandler(this.UI_Tagbox_Load);
            this.Resize += new System.EventHandler(this.UI_Tagbox_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.gPic)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private HintedLabel gTags;
        private System.Windows.Forms.PictureBox gPic;
    }
}
