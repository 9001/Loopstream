using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Loopstream
{
    public partial class UI_TagboxCfg : Form
    {
        public UI_TagboxCfg(LSSettings settings, UI_Tagbox tbox)
        {
            InitializeComponent();
            this.settings = settings;
            this.tbox = tbox;
        }

        LSSettings settings;
        UI_Tagbox tbox;

        private void UI_TagboxCfg_Load(object sender, EventArgs e)
        {
            gtFG.Font = new Font(FontFamily.GenericMonospace.ToString(), gtFG.Font.Size);
            gtBG.Font = gtFG.Font;

            if (settings.tboxBold)
                grBold.Checked = true;
            else if (settings.tboxItalic)
                grItalic.Checked = true;
            else
                grRegular.Checked = true;

            gtFont.Text = settings.tboxFont;
            gtSize.Text = settings.tboxSize.ToString();

            gtFG.Text = settings.tboxColorFront;
            gtBG.Text = settings.tboxColorBack;
        }

        public void ShowAt(Rectangle bounds)
        {
            var scr = Screen.FromPoint(bounds.Location);
            var mul = bounds.Top < scr.Bounds.Top + scr.Bounds.Height / 2 ? 1 : -1;
            
            this.Show();
            this.Location = new Point(
                bounds.Left + bounds.Width / 2 - this.Width / 2,
                bounds.Top + (mul > 0 ? bounds.Height : -1 * this.Height));
        }

        void relSize(double mul)
        {
            double sz;
            if (!double.TryParse(gtSize.Text.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out sz))
                sz = 12;

            sz = Math.Round(sz * mul, 2);
            settings.tboxSize = sz;
            gtSize.Text = sz.ToString();
            tbox.Reload();
        }

        private void gbSmol_Click(object sender, EventArgs e)
        {
            relSize(0.9);
        }

        private void gbHueg_Click(object sender, EventArgs e)
        {
            relSize(1.1);
        }

        private void gtSize_TextChanged(object sender, EventArgs e)
        {
            relSize(1);
        }

        void setStyle(bool bold, bool italic)
        {
            settings.tboxBold = bold;
            settings.tboxItalic = italic;
            tbox.Reload();
        }

        private void grRegular_CheckedChanged(object sender, EventArgs e)
        {
            if (grRegular.Checked)
                setStyle(false, false);
        }

        private void grBold_CheckedChanged(object sender, EventArgs e)
        {
            if (grBold.Checked)
                setStyle(true, false);
        }

        private void grItalic_CheckedChanged(object sender, EventArgs e)
        {
            if (grItalic.Checked)
                setStyle(false, true);
        }

        private void gtFG_TextChanged(object sender, EventArgs e)
        {
            string c = Z.shortcolor(gtFG.Text);
            if (!string.IsNullOrEmpty(c))
                settings.tboxColorFront = c;

            tbox.Reload();
        }

        private void gtBG_TextChanged(object sender, EventArgs e)
        {
            string c = Z.shortcolor(gtBG.Text);
            if (!string.IsNullOrEmpty(c))
                settings.tboxColorBack = c;

            tbox.Reload();
        }

        private void gtFont_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Font f = new Font(gtFont.Text, 1f);
                settings.tboxFont = gtFont.Text;
            }
            catch { }
        }

        string getColor(string orig)
        {
            var cd = new ColorDialog();
            cd.SolidColorOnly = true;
            cd.AllowFullOpen = true;
            cd.AnyColor = true;
            cd.Color = Z.hex2color(orig);
            cd.ShowDialog();
            return Z.shortcolor(cd.Color);
        }

        private void gbFG_Click(object sender, EventArgs e)
        {
            string c = getColor(gtFG.Text);
            if (string.IsNullOrEmpty(c))
                return;

            settings.tboxColorFront = c;
            gtFG.Text = c;
            tbox.Reload();
        }

        private void gbBG_Click(object sender, EventArgs e)
        {
            string c = getColor(gtBG.Text);
            if (string.IsNullOrEmpty(c))
                return;

            settings.tboxColorBack = c;
            gtBG.Text = c;
            tbox.Reload();
        }

        private void gbFont_Click(object sender, EventArgs e)
        {
            var fd = new FontDialog();
            fd.Font = new Font(settings.tboxFont, (float)settings.tboxSize,
                settings.tboxBold ? FontStyle.Bold :
                settings.tboxItalic ? FontStyle.Italic : FontStyle.Regular);

            fd.ShowDialog();

            settings.tboxFont = fd.Font.FontFamily.Name;
            settings.tboxSize = fd.Font.Size;
            settings.tboxBold = fd.Font.Style == FontStyle.Bold;
            settings.tboxItalic = fd.Font.Style == FontStyle.Italic;
            
            UI_TagboxCfg_Load(sender, e);
        }
    }
}
