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
            this.gTags = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // gTags
            // 
            this.gTags.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gTags.ForeColor = System.Drawing.Color.White;
            this.gTags.Location = new System.Drawing.Point(0, 0);
            this.gTags.Name = "gTags";
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
            this.Controls.Add(this.gTags);
            this.Name = "UI_Tagbox";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "tags (click them for settings)";
            this.Load += new System.EventHandler(this.UI_Tagbox_Load);
            this.Resize += new System.EventHandler(this.UI_Tagbox_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label gTags;
    }
}