namespace Loopstream
{
    partial class UI_TagboxCfg
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
            this.gbFont = new System.Windows.Forms.Button();
            this.gtFont = new System.Windows.Forms.TextBox();
            this.gtSize = new System.Windows.Forms.TextBox();
            this.gbSmol = new System.Windows.Forms.Button();
            this.gbHueg = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.gbFG = new System.Windows.Forms.Button();
            this.gtFG = new System.Windows.Forms.TextBox();
            this.gtBG = new System.Windows.Forms.TextBox();
            this.gbBG = new System.Windows.Forms.Button();
            this.grRegular = new System.Windows.Forms.RadioButton();
            this.grBold = new System.Windows.Forms.RadioButton();
            this.grItalic = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // gbFont
            // 
            this.gbFont.Location = new System.Drawing.Point(12, 12);
            this.gbFont.Name = "gbFont";
            this.gbFont.Size = new System.Drawing.Size(75, 23);
            this.gbFont.TabIndex = 0;
            this.gbFont.Text = "Font";
            this.gbFont.UseVisualStyleBackColor = true;
            this.gbFont.Click += new System.EventHandler(this.gbFont_Click);
            // 
            // gtFont
            // 
            this.gtFont.Location = new System.Drawing.Point(93, 14);
            this.gtFont.Name = "gtFont";
            this.gtFont.Size = new System.Drawing.Size(179, 20);
            this.gtFont.TabIndex = 1;
            this.gtFont.Text = "Comic Sans MS";
            this.gtFont.TextChanged += new System.EventHandler(this.gtFont_TextChanged);
            // 
            // gtSize
            // 
            this.gtSize.Location = new System.Drawing.Point(93, 42);
            this.gtSize.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.gtSize.Name = "gtSize";
            this.gtSize.Size = new System.Drawing.Size(100, 20);
            this.gtSize.TabIndex = 2;
            this.gtSize.Text = "12";
            this.gtSize.TextChanged += new System.EventHandler(this.gtSize_TextChanged);
            // 
            // gbSmol
            // 
            this.gbSmol.Location = new System.Drawing.Point(199, 40);
            this.gbSmol.Name = "gbSmol";
            this.gbSmol.Size = new System.Drawing.Size(36, 23);
            this.gbSmol.TabIndex = 3;
            this.gbSmol.Text = "--";
            this.gbSmol.UseVisualStyleBackColor = true;
            this.gbSmol.Click += new System.EventHandler(this.gbSmol_Click);
            // 
            // gbHueg
            // 
            this.gbHueg.Location = new System.Drawing.Point(236, 40);
            this.gbHueg.Name = "gbHueg";
            this.gbHueg.Size = new System.Drawing.Size(36, 23);
            this.gbHueg.TabIndex = 4;
            this.gbHueg.Text = "+";
            this.gbHueg.UseVisualStyleBackColor = true;
            this.gbHueg.Click += new System.EventHandler(this.gbHueg_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(35, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(27, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Size";
            // 
            // gbFG
            // 
            this.gbFG.Location = new System.Drawing.Point(12, 139);
            this.gbFG.Name = "gbFG";
            this.gbFG.Size = new System.Drawing.Size(75, 23);
            this.gbFG.TabIndex = 6;
            this.gbFG.Text = "Text color";
            this.gbFG.UseVisualStyleBackColor = true;
            this.gbFG.Click += new System.EventHandler(this.gbFG_Click);
            // 
            // gtFG
            // 
            this.gtFG.Location = new System.Drawing.Point(93, 141);
            this.gtFG.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.gtFG.Name = "gtFG";
            this.gtFG.Size = new System.Drawing.Size(100, 20);
            this.gtFG.TabIndex = 7;
            this.gtFG.Text = "fff";
            this.gtFG.TextChanged += new System.EventHandler(this.gtFG_TextChanged);
            // 
            // gtBG
            // 
            this.gtBG.Location = new System.Drawing.Point(93, 170);
            this.gtBG.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.gtBG.Name = "gtBG";
            this.gtBG.Size = new System.Drawing.Size(100, 20);
            this.gtBG.TabIndex = 9;
            this.gtBG.Text = "000";
            this.gtBG.TextChanged += new System.EventHandler(this.gtBG_TextChanged);
            // 
            // gbBG
            // 
            this.gbBG.Location = new System.Drawing.Point(12, 168);
            this.gbBG.Name = "gbBG";
            this.gbBG.Size = new System.Drawing.Size(75, 23);
            this.gbBG.TabIndex = 8;
            this.gbBG.Text = "BG colour";
            this.gbBG.UseVisualStyleBackColor = true;
            this.gbBG.Click += new System.EventHandler(this.gbBG_Click);
            // 
            // grRegular
            // 
            this.grRegular.AutoSize = true;
            this.grRegular.Checked = true;
            this.grRegular.Location = new System.Drawing.Point(93, 70);
            this.grRegular.Name = "grRegular";
            this.grRegular.Size = new System.Drawing.Size(62, 17);
            this.grRegular.TabIndex = 10;
            this.grRegular.TabStop = true;
            this.grRegular.Text = "Regular";
            this.grRegular.UseVisualStyleBackColor = true;
            this.grRegular.CheckedChanged += new System.EventHandler(this.grRegular_CheckedChanged);
            // 
            // grBold
            // 
            this.grBold.AutoSize = true;
            this.grBold.Location = new System.Drawing.Point(93, 93);
            this.grBold.Name = "grBold";
            this.grBold.Size = new System.Drawing.Size(46, 17);
            this.grBold.TabIndex = 11;
            this.grBold.Text = "Bold";
            this.grBold.UseVisualStyleBackColor = true;
            this.grBold.CheckedChanged += new System.EventHandler(this.grBold_CheckedChanged);
            // 
            // grItalic
            // 
            this.grItalic.AutoSize = true;
            this.grItalic.Location = new System.Drawing.Point(93, 116);
            this.grItalic.Name = "grItalic";
            this.grItalic.Size = new System.Drawing.Size(56, 17);
            this.grItalic.TabIndex = 12;
            this.grItalic.Text = "愛talic";
            this.grItalic.UseVisualStyleBackColor = true;
            this.grItalic.CheckedChanged += new System.EventHandler(this.grItalic_CheckedChanged);
            // 
            // UI_TagboxCfg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 203);
            this.Controls.Add(this.grItalic);
            this.Controls.Add(this.grBold);
            this.Controls.Add(this.grRegular);
            this.Controls.Add(this.gtBG);
            this.Controls.Add(this.gbBG);
            this.Controls.Add(this.gtFG);
            this.Controls.Add(this.gbFG);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.gbHueg);
            this.Controls.Add(this.gbSmol);
            this.Controls.Add(this.gtSize);
            this.Controls.Add(this.gtFont);
            this.Controls.Add(this.gbFont);
            this.Name = "UI_TagboxCfg";
            this.Text = "UI_TagboxCfg";
            this.Load += new System.EventHandler(this.UI_TagboxCfg_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button gbFont;
        private System.Windows.Forms.TextBox gtFont;
        private System.Windows.Forms.TextBox gtSize;
        private System.Windows.Forms.Button gbSmol;
        private System.Windows.Forms.Button gbHueg;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button gbFG;
        private System.Windows.Forms.TextBox gtFG;
        private System.Windows.Forms.TextBox gtBG;
        private System.Windows.Forms.Button gbBG;
        private System.Windows.Forms.RadioButton grRegular;
        private System.Windows.Forms.RadioButton grBold;
        private System.Windows.Forms.RadioButton grItalic;
    }
}