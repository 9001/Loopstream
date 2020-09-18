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
            this.Icon = Program.icon;

            //var font = FontFamily.GenericMonospace.Name;
            var font = "Consolas";
            
            foreach (var ctl in new Control[] { gtFG, gtBG, gtSize })
            {
                ctl.Font = new Font(font, gtFont.Font.Size * 1.1f);
                ctl.Top -= 1;
            }

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

            setRadio("grAlign" + settings.tboxAlign);
            setRadio("grAlias" + settings.tboxAntialias);
            setRadio("grRender" + settings.tboxRendermode);
        }

        void setRadio(string name)
        {
            var ctls = this.Controls.Find(name, true);
            if (ctls.Length == 1)
                ((RadioButton)ctls[0]).Checked = true;
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
            tbox.Reload();
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

        private void grAlign_CheckedChanged(object sender, EventArgs e)
        {
            var c = (RadioButton)sender;
            if (!c.Checked)
                return;

            settings.tboxAlign = int.Parse(c.Name.Substring(c.Name.Length - 1));
            tbox.Reload();
        }

        private void grAlias_CheckedChanged(object sender, EventArgs e)
        {
            var c = (RadioButton)sender;
            if (!c.Checked)
                return;

            settings.tboxAntialias = c.Name.Substring(7);
            tbox.Reload();
        }

        private void grRender_CheckedChanged(object sender, EventArgs e)
        {
            var c = (RadioButton)sender;
            if (!c.Checked)
                return;

            settings.tboxRendermode = c.Name.Substring(8);
            tbox.Reload();
        }

        private void gbHelp_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "the main purpose of the tagbox is to provide an overlay in OBS or other streaming software\r\n\r\n" +
                "you will get the best results with a black or white background, and the opposite color for the text\r\n\r\n" +
                "in obs, you should add a ''Luma Key'' effect filter to the windowcapture, as that will remove the background and keep just the tags\r\n\r\n" +
                "if you need a shadow you can for example duplicate the windowcapture and invert the colors of the backmost one\r\n\r\n" +
                "when using a Luma Key to remove the background, these combinations of options are recommended (depending on font):\r\n\r\n" +
                "    1) antialiasing = cleartext   and   rendermode = bitmap\r\n\r\n" +
                "    2) antialiasing = grayscale   and   rendermode = bitmap\r\n\r\n" +
                "    3) antialiasing = grayscale   and   rendermode = label\r\n\r\n" +
                "other combinations may work but in particular cleartext+label is bad (will cause color bleeding)");
        }
    }
}
