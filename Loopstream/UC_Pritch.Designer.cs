namespace Loopstream
{
    partial class Pritch
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
            this.pV = new System.Windows.Forms.Panel();
            this.pH = new System.Windows.Forms.Panel();
            this.bV = new System.Windows.Forms.Label();
            this.bH = new System.Windows.Forms.Label();
            this.bp = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // pV
            // 
            this.pV.Dock = System.Windows.Forms.DockStyle.Left;
            this.pV.Location = new System.Drawing.Point(1, 0);
            this.pV.Name = "pV";
            this.pV.Size = new System.Drawing.Size(4, 364);
            this.pV.TabIndex = 3;
            // 
            // pH
            // 
            this.pH.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pH.Location = new System.Drawing.Point(1, 364);
            this.pH.Name = "pH";
            this.pH.Size = new System.Drawing.Size(106, 4);
            this.pH.TabIndex = 4;
            // 
            // bV
            // 
            this.bV.BackColor = System.Drawing.SystemColors.ControlDark;
            this.bV.Dock = System.Windows.Forms.DockStyle.Left;
            this.bV.ForeColor = System.Drawing.SystemColors.Control;
            this.bV.Location = new System.Drawing.Point(0, 0);
            this.bV.Margin = new System.Windows.Forms.Padding(6);
            this.bV.Name = "bV";
            this.bV.Size = new System.Drawing.Size(1, 369);
            this.bV.TabIndex = 5;
            // 
            // bH
            // 
            this.bH.BackColor = System.Drawing.SystemColors.ControlDark;
            this.bH.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bH.ForeColor = System.Drawing.SystemColors.Control;
            this.bH.Location = new System.Drawing.Point(1, 368);
            this.bH.Margin = new System.Windows.Forms.Padding(6);
            this.bH.Name = "bH";
            this.bH.Size = new System.Drawing.Size(106, 1);
            this.bH.TabIndex = 6;
            // 
            // bp
            // 
            this.bp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bp.Location = new System.Drawing.Point(5, 0);
            this.bp.Name = "bp";
            this.bp.Size = new System.Drawing.Size(102, 364);
            this.bp.TabIndex = 7;
            this.bp.UseVisualStyleBackColor = true;
            this.bp.Click += new System.EventHandler(this.pb_Click);
            // 
            // Pritch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.bp);
            this.Controls.Add(this.pV);
            this.Controls.Add(this.pH);
            this.Controls.Add(this.bH);
            this.Controls.Add(this.bV);
            this.Name = "Pritch";
            this.Size = new System.Drawing.Size(107, 369);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pV;
        private System.Windows.Forms.Panel pH;
        private System.Windows.Forms.Label bV;
        private System.Windows.Forms.Label bH;
        private System.Windows.Forms.Button bp;

    }
}
