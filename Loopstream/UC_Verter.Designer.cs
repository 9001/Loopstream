namespace LoopStream
{
    partial class Verter
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.gOSlider = new System.Windows.Forms.Panel();
            this.gBar = new System.Windows.Forms.Label();
            this.gLabel = new System.Windows.Forms.Button();
            this.gButton = new System.Windows.Forms.Panel();
            this.gBorder = new System.Windows.Forms.Panel();
            this.gW8bugfix = new System.Windows.Forms.Panel();
            this.giSlider = new LoopStream.LLabel();
            this.gOSlider.SuspendLayout();
            this.gButton.SuspendLayout();
            this.gBorder.SuspendLayout();
            this.gW8bugfix.SuspendLayout();
            this.SuspendLayout();
            // 
            // gOSlider
            // 
            this.gOSlider.BackColor = System.Drawing.SystemColors.ControlLight;
            this.gOSlider.Controls.Add(this.gBar);
            this.gOSlider.Controls.Add(this.giSlider);
            this.gOSlider.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gOSlider.Location = new System.Drawing.Point(1, 1);
            this.gOSlider.Name = "gOSlider";
            this.gOSlider.Size = new System.Drawing.Size(94, 336);
            this.gOSlider.TabIndex = 1;
            this.gOSlider.MouseDown += new System.Windows.Forms.MouseEventHandler(this.giSlider_MouseDown);
            this.gOSlider.MouseMove += new System.Windows.Forms.MouseEventHandler(this.giSlider_MouseMove);
            this.gOSlider.MouseUp += new System.Windows.Forms.MouseEventHandler(this.gOSlider_MouseUp);
            // 
            // gBar
            // 
            this.gBar.BackColor = System.Drawing.SystemColors.ControlDark;
            this.gBar.Location = new System.Drawing.Point(48, 0);
            this.gBar.Name = "gBar";
            this.gBar.Size = new System.Drawing.Size(48, 14);
            this.gBar.TabIndex = 2;
            this.gBar.Visible = false;
            // 
            // gLabel
            // 
            this.gLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.gLabel.Location = new System.Drawing.Point(0, 4);
            this.gLabel.Name = "gLabel";
            this.gLabel.Size = new System.Drawing.Size(98, 40);
            this.gLabel.TabIndex = 2;
            this.gLabel.Text = "button1";
            this.gLabel.UseVisualStyleBackColor = true;
            this.gLabel.Click += new System.EventHandler(this.gLabel_Click);
            // 
            // gButton
            // 
            this.gButton.Controls.Add(this.gLabel);
            this.gButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.gButton.Location = new System.Drawing.Point(0, 338);
            this.gButton.Name = "gButton";
            this.gButton.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.gButton.Size = new System.Drawing.Size(98, 96);
            this.gButton.TabIndex = 3;
            // 
            // gBorder
            // 
            this.gBorder.BackColor = System.Drawing.SystemColors.ControlDark;
            this.gBorder.Controls.Add(this.gOSlider);
            this.gBorder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gBorder.Location = new System.Drawing.Point(1, 0);
            this.gBorder.Name = "gBorder";
            this.gBorder.Padding = new System.Windows.Forms.Padding(1);
            this.gBorder.Size = new System.Drawing.Size(96, 338);
            this.gBorder.TabIndex = 4;
            // 
            // gW8bugfix
            // 
            this.gW8bugfix.Controls.Add(this.gBorder);
            this.gW8bugfix.Dock = System.Windows.Forms.DockStyle.Top;
            this.gW8bugfix.Location = new System.Drawing.Point(0, 0);
            this.gW8bugfix.Name = "gW8bugfix";
            this.gW8bugfix.Padding = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.gW8bugfix.Size = new System.Drawing.Size(98, 338);
            this.gW8bugfix.TabIndex = 5;
            // 
            // giSlider
            // 
            this.giSlider.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.giSlider.ForeColor = System.Drawing.SystemColors.ButtonShadow;
            this.giSlider.Location = new System.Drawing.Point(0, 0);
            this.giSlider.Name = "giSlider";
            this.giSlider.Size = new System.Drawing.Size(48, 40);
            this.giSlider.TabIndex = 1;
            this.giSlider.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.giSlider.MouseDown += new System.Windows.Forms.MouseEventHandler(this.giSlider_MouseDown);
            this.giSlider.MouseMove += new System.Windows.Forms.MouseEventHandler(this.giSlider_MouseMove);
            this.giSlider.MouseUp += new System.Windows.Forms.MouseEventHandler(this.gOSlider_MouseUp);
            // 
            // Verter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gButton);
            this.Controls.Add(this.gW8bugfix);
            this.Name = "Verter";
            this.Size = new System.Drawing.Size(98, 420);
            this.gOSlider.ResumeLayout(false);
            this.gButton.ResumeLayout(false);
            this.gBorder.ResumeLayout(false);
            this.gW8bugfix.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel gOSlider;
        private LoopStream.LLabel giSlider;
        private System.Windows.Forms.Button gLabel;
        private System.Windows.Forms.Panel gButton;
        private System.Windows.Forms.Panel gBorder;
        private System.Windows.Forms.Label gBar;
        private System.Windows.Forms.Panel gW8bugfix;

    }
}
