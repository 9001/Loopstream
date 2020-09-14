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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.grAlign9 = new System.Windows.Forms.RadioButton();
            this.grAlign8 = new System.Windows.Forms.RadioButton();
            this.grAlign7 = new System.Windows.Forms.RadioButton();
            this.grAlign6 = new System.Windows.Forms.RadioButton();
            this.grAlign5 = new System.Windows.Forms.RadioButton();
            this.grAlign4 = new System.Windows.Forms.RadioButton();
            this.grAlign3 = new System.Windows.Forms.RadioButton();
            this.grAlign2 = new System.Windows.Forms.RadioButton();
            this.grAlign1 = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.grAliasNone = new System.Windows.Forms.RadioButton();
            this.grAliasGray = new System.Windows.Forms.RadioButton();
            this.grAliasClear = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.grRenderBitmap = new System.Windows.Forms.RadioButton();
            this.grRenderLabel = new System.Windows.Forms.RadioButton();
            this.gbHelp = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
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
            this.gbFG.TabIndex = 8;
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
            this.gtFG.TabIndex = 9;
            this.gtFG.Text = "fff";
            this.gtFG.TextChanged += new System.EventHandler(this.gtFG_TextChanged);
            // 
            // gtBG
            // 
            this.gtBG.Location = new System.Drawing.Point(93, 170);
            this.gtBG.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.gtBG.Name = "gtBG";
            this.gtBG.Size = new System.Drawing.Size(100, 20);
            this.gtBG.TabIndex = 11;
            this.gtBG.Text = "000";
            this.gtBG.TextChanged += new System.EventHandler(this.gtBG_TextChanged);
            // 
            // gbBG
            // 
            this.gbBG.Location = new System.Drawing.Point(12, 168);
            this.gbBG.Name = "gbBG";
            this.gbBG.Size = new System.Drawing.Size(75, 23);
            this.gbBG.TabIndex = 10;
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
            this.grRegular.TabIndex = 5;
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
            this.grBold.TabIndex = 6;
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
            this.grItalic.TabIndex = 7;
            this.grItalic.Text = "愛talic";
            this.grItalic.UseVisualStyleBackColor = true;
            this.grItalic.CheckedChanged += new System.EventHandler(this.grItalic_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.grAlign9);
            this.groupBox1.Controls.Add(this.grAlign8);
            this.groupBox1.Controls.Add(this.grAlign7);
            this.groupBox1.Controls.Add(this.grAlign6);
            this.groupBox1.Controls.Add(this.grAlign5);
            this.groupBox1.Controls.Add(this.grAlign4);
            this.groupBox1.Controls.Add(this.grAlign3);
            this.groupBox1.Controls.Add(this.grAlign2);
            this.groupBox1.Controls.Add(this.grAlign1);
            this.groupBox1.Location = new System.Drawing.Point(225, 108);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(82, 82);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Alignment";
            // 
            // grAlign9
            // 
            this.grAlign9.AutoSize = true;
            this.grAlign9.Location = new System.Drawing.Point(53, 57);
            this.grAlign9.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.grAlign9.Name = "grAlign9";
            this.grAlign9.Size = new System.Drawing.Size(28, 17);
            this.grAlign9.TabIndex = 20;
            this.grAlign9.TabStop = true;
            this.grAlign9.Text = " ";
            this.grAlign9.UseVisualStyleBackColor = true;
            this.grAlign9.CheckedChanged += new System.EventHandler(this.grAlign_CheckedChanged);
            // 
            // grAlign8
            // 
            this.grAlign8.AutoSize = true;
            this.grAlign8.Location = new System.Drawing.Point(31, 57);
            this.grAlign8.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.grAlign8.Name = "grAlign8";
            this.grAlign8.Size = new System.Drawing.Size(28, 17);
            this.grAlign8.TabIndex = 19;
            this.grAlign8.TabStop = true;
            this.grAlign8.Text = " ";
            this.grAlign8.UseVisualStyleBackColor = true;
            this.grAlign8.CheckedChanged += new System.EventHandler(this.grAlign_CheckedChanged);
            // 
            // grAlign7
            // 
            this.grAlign7.AutoSize = true;
            this.grAlign7.Location = new System.Drawing.Point(9, 57);
            this.grAlign7.Margin = new System.Windows.Forms.Padding(6, 3, 0, 3);
            this.grAlign7.Name = "grAlign7";
            this.grAlign7.Size = new System.Drawing.Size(28, 17);
            this.grAlign7.TabIndex = 18;
            this.grAlign7.TabStop = true;
            this.grAlign7.Text = " ";
            this.grAlign7.UseVisualStyleBackColor = true;
            this.grAlign7.CheckedChanged += new System.EventHandler(this.grAlign_CheckedChanged);
            // 
            // grAlign6
            // 
            this.grAlign6.AutoSize = true;
            this.grAlign6.Location = new System.Drawing.Point(53, 38);
            this.grAlign6.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.grAlign6.Name = "grAlign6";
            this.grAlign6.Size = new System.Drawing.Size(28, 17);
            this.grAlign6.TabIndex = 17;
            this.grAlign6.TabStop = true;
            this.grAlign6.Text = " ";
            this.grAlign6.UseVisualStyleBackColor = true;
            this.grAlign6.CheckedChanged += new System.EventHandler(this.grAlign_CheckedChanged);
            // 
            // grAlign5
            // 
            this.grAlign5.AutoSize = true;
            this.grAlign5.Location = new System.Drawing.Point(31, 38);
            this.grAlign5.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.grAlign5.Name = "grAlign5";
            this.grAlign5.Size = new System.Drawing.Size(28, 17);
            this.grAlign5.TabIndex = 16;
            this.grAlign5.TabStop = true;
            this.grAlign5.Text = " ";
            this.grAlign5.UseVisualStyleBackColor = true;
            this.grAlign5.CheckedChanged += new System.EventHandler(this.grAlign_CheckedChanged);
            // 
            // grAlign4
            // 
            this.grAlign4.AutoSize = true;
            this.grAlign4.Location = new System.Drawing.Point(9, 38);
            this.grAlign4.Margin = new System.Windows.Forms.Padding(6, 3, 0, 3);
            this.grAlign4.Name = "grAlign4";
            this.grAlign4.Size = new System.Drawing.Size(28, 17);
            this.grAlign4.TabIndex = 15;
            this.grAlign4.TabStop = true;
            this.grAlign4.Text = " ";
            this.grAlign4.UseVisualStyleBackColor = true;
            this.grAlign4.CheckedChanged += new System.EventHandler(this.grAlign_CheckedChanged);
            // 
            // grAlign3
            // 
            this.grAlign3.AutoSize = true;
            this.grAlign3.Location = new System.Drawing.Point(53, 19);
            this.grAlign3.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.grAlign3.Name = "grAlign3";
            this.grAlign3.Size = new System.Drawing.Size(28, 17);
            this.grAlign3.TabIndex = 14;
            this.grAlign3.TabStop = true;
            this.grAlign3.Text = " ";
            this.grAlign3.UseVisualStyleBackColor = true;
            this.grAlign3.CheckedChanged += new System.EventHandler(this.grAlign_CheckedChanged);
            // 
            // grAlign2
            // 
            this.grAlign2.AutoSize = true;
            this.grAlign2.Location = new System.Drawing.Point(31, 19);
            this.grAlign2.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.grAlign2.Name = "grAlign2";
            this.grAlign2.Size = new System.Drawing.Size(28, 17);
            this.grAlign2.TabIndex = 13;
            this.grAlign2.TabStop = true;
            this.grAlign2.Text = " ";
            this.grAlign2.UseVisualStyleBackColor = true;
            this.grAlign2.CheckedChanged += new System.EventHandler(this.grAlign_CheckedChanged);
            // 
            // grAlign1
            // 
            this.grAlign1.AutoSize = true;
            this.grAlign1.Location = new System.Drawing.Point(9, 19);
            this.grAlign1.Margin = new System.Windows.Forms.Padding(6, 3, 0, 3);
            this.grAlign1.Name = "grAlign1";
            this.grAlign1.Size = new System.Drawing.Size(28, 17);
            this.grAlign1.TabIndex = 12;
            this.grAlign1.TabStop = true;
            this.grAlign1.Text = " ";
            this.grAlign1.UseVisualStyleBackColor = true;
            this.grAlign1.CheckedChanged += new System.EventHandler(this.grAlign_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 324);
            this.label2.Margin = new System.Windows.Forms.Padding(3, 12, 3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(211, 39);
            this.label2.TabIndex = 15;
            this.label2.Text = "You can make these settings permanent by\r\nopening up the main settings window and" +
    "\r\nhitting the save button there (sorry)";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.grAliasClear);
            this.groupBox2.Controls.Add(this.grAliasGray);
            this.groupBox2.Controls.Add(this.grAliasNone);
            this.groupBox2.Location = new System.Drawing.Point(93, 207);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 12, 3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(214, 44);
            this.groupBox2.TabIndex = 16;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Antialiasing";
            // 
            // grAliasNone
            // 
            this.grAliasNone.AutoSize = true;
            this.grAliasNone.Location = new System.Drawing.Point(6, 19);
            this.grAliasNone.Name = "grAliasNone";
            this.grAliasNone.Size = new System.Drawing.Size(51, 17);
            this.grAliasNone.TabIndex = 0;
            this.grAliasNone.TabStop = true;
            this.grAliasNone.Text = "None";
            this.grAliasNone.UseVisualStyleBackColor = true;
            this.grAliasNone.CheckedChanged += new System.EventHandler(this.grAlias_CheckedChanged);
            // 
            // grAliasGray
            // 
            this.grAliasGray.AutoSize = true;
            this.grAliasGray.Location = new System.Drawing.Point(63, 19);
            this.grAliasGray.Name = "grAliasGray";
            this.grAliasGray.Size = new System.Drawing.Size(72, 17);
            this.grAliasGray.TabIndex = 1;
            this.grAliasGray.TabStop = true;
            this.grAliasGray.Text = "Grayscale";
            this.grAliasGray.UseVisualStyleBackColor = true;
            this.grAliasGray.CheckedChanged += new System.EventHandler(this.grAlias_CheckedChanged);
            // 
            // grAliasClear
            // 
            this.grAliasClear.AutoSize = true;
            this.grAliasClear.Location = new System.Drawing.Point(141, 19);
            this.grAliasClear.Name = "grAliasClear";
            this.grAliasClear.Size = new System.Drawing.Size(66, 17);
            this.grAliasClear.TabIndex = 2;
            this.grAliasClear.TabStop = true;
            this.grAliasClear.Text = "Cleartext";
            this.grAliasClear.UseVisualStyleBackColor = true;
            this.grAliasClear.CheckedChanged += new System.EventHandler(this.grAlias_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.gbHelp);
            this.groupBox3.Controls.Add(this.grRenderBitmap);
            this.groupBox3.Controls.Add(this.grRenderLabel);
            this.groupBox3.Location = new System.Drawing.Point(93, 266);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(3, 12, 3, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(214, 43);
            this.groupBox3.TabIndex = 17;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Render mode";
            // 
            // grRenderBitmap
            // 
            this.grRenderBitmap.AutoSize = true;
            this.grRenderBitmap.Location = new System.Drawing.Point(63, 19);
            this.grRenderBitmap.Name = "grRenderBitmap";
            this.grRenderBitmap.Size = new System.Drawing.Size(57, 17);
            this.grRenderBitmap.TabIndex = 3;
            this.grRenderBitmap.TabStop = true;
            this.grRenderBitmap.Text = "Bitmap";
            this.grRenderBitmap.UseVisualStyleBackColor = true;
            this.grRenderBitmap.CheckedChanged += new System.EventHandler(this.grRender_CheckedChanged);
            // 
            // grRenderLabel
            // 
            this.grRenderLabel.AutoSize = true;
            this.grRenderLabel.Location = new System.Drawing.Point(6, 19);
            this.grRenderLabel.Name = "grRenderLabel";
            this.grRenderLabel.Size = new System.Drawing.Size(51, 17);
            this.grRenderLabel.TabIndex = 2;
            this.grRenderLabel.TabStop = true;
            this.grRenderLabel.Text = "Label";
            this.grRenderLabel.UseVisualStyleBackColor = true;
            this.grRenderLabel.CheckedChanged += new System.EventHandler(this.grRender_CheckedChanged);
            // 
            // gbHelp
            // 
            this.gbHelp.Location = new System.Drawing.Point(126, 14);
            this.gbHelp.Name = "gbHelp";
            this.gbHelp.Size = new System.Drawing.Size(82, 23);
            this.gbHelp.TabIndex = 11;
            this.gbHelp.Text = "help";
            this.gbHelp.UseVisualStyleBackColor = true;
            this.gbHelp.Click += new System.EventHandler(this.gbHelp_Click);
            // 
            // UI_TagboxCfg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(322, 375);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.groupBox1);
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
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
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
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton grAlign9;
        private System.Windows.Forms.RadioButton grAlign8;
        private System.Windows.Forms.RadioButton grAlign7;
        private System.Windows.Forms.RadioButton grAlign6;
        private System.Windows.Forms.RadioButton grAlign5;
        private System.Windows.Forms.RadioButton grAlign4;
        private System.Windows.Forms.RadioButton grAlign3;
        private System.Windows.Forms.RadioButton grAlign2;
        private System.Windows.Forms.RadioButton grAlign1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton grAliasClear;
        private System.Windows.Forms.RadioButton grAliasGray;
        private System.Windows.Forms.RadioButton grAliasNone;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton grRenderBitmap;
        private System.Windows.Forms.RadioButton grRenderLabel;
        private System.Windows.Forms.Button gbHelp;
    }
}