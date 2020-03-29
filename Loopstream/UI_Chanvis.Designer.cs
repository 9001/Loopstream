namespace Loopstream
{
    partial class UI_Chanvis
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
            this.gNDev = new System.Windows.Forms.Label();
            this.gDevAdd = new System.Windows.Forms.Button();
            this.gDevSub = new System.Windows.Forms.Button();
            this.gPreviewOn = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gNDev);
            this.panel1.Controls.Add(this.gDevAdd);
            this.panel1.Controls.Add(this.gDevSub);
            this.panel1.Controls.Add(this.gPreviewOn);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(277, 38);
            this.panel1.TabIndex = 0;
            // 
            // gNDev
            // 
            this.gNDev.AutoSize = true;
            this.gNDev.Location = new System.Drawing.Point(112, 13);
            this.gNDev.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.gNDev.Name = "gNDev";
            this.gNDev.Size = new System.Drawing.Size(83, 13);
            this.gNDev.TabIndex = 3;
            this.gNDev.Text = "windows-default";
            this.gNDev.Click += new System.EventHandler(this.gNDev_Click);
            // 
            // gDevAdd
            // 
            this.gDevAdd.Location = new System.Drawing.Point(236, 8);
            this.gDevAdd.Name = "gDevAdd";
            this.gDevAdd.Size = new System.Drawing.Size(29, 23);
            this.gDevAdd.TabIndex = 2;
            this.gDevAdd.Text = "+";
            this.gDevAdd.UseVisualStyleBackColor = true;
            this.gDevAdd.Click += new System.EventHandler(this.gDevAdd_Click);
            // 
            // gDevSub
            // 
            this.gDevSub.Location = new System.Drawing.Point(201, 8);
            this.gDevSub.Name = "gDevSub";
            this.gDevSub.Size = new System.Drawing.Size(29, 23);
            this.gDevSub.TabIndex = 1;
            this.gDevSub.Text = "--";
            this.gDevSub.UseVisualStyleBackColor = true;
            this.gDevSub.Click += new System.EventHandler(this.gDevSub_Click);
            // 
            // gPreviewOn
            // 
            this.gPreviewOn.AutoSize = true;
            this.gPreviewOn.Checked = true;
            this.gPreviewOn.CheckState = System.Windows.Forms.CheckState.Checked;
            this.gPreviewOn.Location = new System.Drawing.Point(12, 12);
            this.gPreviewOn.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.gPreviewOn.Name = "gPreviewOn";
            this.gPreviewOn.Size = new System.Drawing.Size(106, 17);
            this.gPreviewOn.TabIndex = 0;
            this.gPreviewOn.Text = "play to speakers:";
            this.gPreviewOn.UseVisualStyleBackColor = true;
            this.gPreviewOn.CheckedChanged += new System.EventHandler(this.gPreviewOn_CheckedChanged);
            // 
            // UI_Chanvis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(277, 460);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "UI_Chanvis";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "UI_Chanvis";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.UI_Chanvis_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.UI_Chanvis_FormClosed);
            this.Load += new System.EventHandler(this.UI_Chanvis_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox gPreviewOn;
        private System.Windows.Forms.Label gNDev;
        private System.Windows.Forms.Button gDevAdd;
        private System.Windows.Forms.Button gDevSub;
    }
}