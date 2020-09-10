using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Loopstream
{
    public partial class UI_Tagbox : Form
    {
        public UI_Tagbox(LSSettings settings, LSTag tag)
        {
            InitializeComponent();

            this.settings = settings;
            this.tag = tag;
            cui = null;
            Reload();

            var t = new Timer();
            t.Interval = 100;
            t.Start();
            t.Tick += t_Tick;
        }

        LSTag tag;
        LSSettings settings;
        UI_TagboxCfg cui;

        private void UI_Tagbox_Load(object sender, EventArgs e)
        {

        }

        public void ShowAt(Rectangle bounds)
        {
            var w = settings.tboxWidth > 0 ? settings.tboxWidth : this.Width;
            var h = settings.tboxHeight > 0 ? settings.tboxHeight : this.Height;
            this.Show();
            this.Bounds = new Rectangle(new Point(
                bounds.Left + bounds.Width / 2 - w / 2,
                bounds.Top + bounds.Height / 2 - h / 2),
                new Size(w, h));
        }

        public void Reload()
        {
            var style = FontStyle.Regular;
            if (settings.tboxBold)
                style = FontStyle.Bold;
            else if (settings.tboxItalic)
                style = FontStyle.Italic;

            gTags.Font = new Font(settings.tboxFont, (float)settings.tboxSize, style);
            
            this.BackColor = Z.hex2color(settings.tboxColorBack);
            gTags.ForeColor = Z.hex2color(settings.tboxColorFront);
        }

        void t_Tick(object sender, EventArgs e)
        {
            string s = "";
            if (tag != null)
                s = tag.tag.tag;

            gTags.Text = s;
        }

        private void gTags_Click(object sender, EventArgs e)
        {
            if (cui == null || cui.IsDisposed)
                cui = new UI_TagboxCfg(settings, this);

            cui.ShowAt(this.Bounds);
        }

        private void UI_Tagbox_Resize(object sender, EventArgs e)
        {
            settings.tboxWidth = this.Width;
            settings.tboxHeight = this.Height;
        }
    }
}
