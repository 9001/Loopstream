namespace Loopstream
{
    partial class UI_Winlist
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.gCancel = new System.Windows.Forms.Button();
            this.gSave = new System.Windows.Forms.Button();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.gList = new System.Windows.Forms.ListBox();
            this.gReload = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gReload);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 447);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.panel1.Size = new System.Drawing.Size(784, 55);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(784, 41);
            this.panel2.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.gSave);
            this.panel3.Controls.Add(this.gCancel);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel3.Location = new System.Drawing.Point(600, 4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(184, 51);
            this.panel3.TabIndex = 0;
            // 
            // gCancel
            // 
            this.gCancel.Location = new System.Drawing.Point(3, 3);
            this.gCancel.Name = "gCancel";
            this.gCancel.Size = new System.Drawing.Size(75, 23);
            this.gCancel.TabIndex = 0;
            this.gCancel.Text = "Cancel";
            this.gCancel.UseVisualStyleBackColor = true;
            this.gCancel.Click += new System.EventHandler(this.gCancel_Click);
            // 
            // gSave
            // 
            this.gSave.Location = new System.Drawing.Point(84, 3);
            this.gSave.Name = "gSave";
            this.gSave.Size = new System.Drawing.Size(75, 23);
            this.gSave.TabIndex = 1;
            this.gSave.Text = "Save";
            this.gSave.UseVisualStyleBackColor = true;
            this.gSave.Click += new System.EventHandler(this.gSave_Click);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.gList);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 41);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(784, 406);
            this.panel4.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label1.Location = new System.Drawing.Point(0, 6);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.label1.Size = new System.Drawing.Size(784, 35);
            this.label1.TabIndex = 0;
            this.label1.Text = "Right now,  the following windows are available:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // gList
            // 
            this.gList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gList.FormattingEnabled = true;
            this.gList.Location = new System.Drawing.Point(0, 0);
            this.gList.Name = "gList";
            this.gList.Size = new System.Drawing.Size(784, 406);
            this.gList.TabIndex = 0;
            // 
            // gReload
            // 
            this.gReload.Location = new System.Drawing.Point(23, 7);
            this.gReload.Name = "gReload";
            this.gReload.Size = new System.Drawing.Size(75, 23);
            this.gReload.TabIndex = 1;
            this.gReload.Text = "Reload";
            this.gReload.UseVisualStyleBackColor = true;
            this.gReload.Click += new System.EventHandler(this.gReload_Click);
            // 
            // UI_Winpick
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 502);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "UI_Winpick";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "UI_Winpick";
            this.Load += new System.EventHandler(this.UI_Winpick_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button gSave;
        private System.Windows.Forms.Button gCancel;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.ListBox gList;
        private System.Windows.Forms.Button gReload;
    }
}