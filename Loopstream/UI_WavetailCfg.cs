using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Loopstream
{
    public partial class UI_WavetailCfg : Form
    {
        public UI_WavetailCfg(LSWavetail wt)
        {
            this.wt = wt;
            InitializeComponent();
        }

        LSWavetail wt;

        private void UI_WavetailCfg_Load(object sender, EventArgs e)
        {
            this.Icon = Program.icon;

            // waveformat is float, use members
            gSampleRate.Text = wt.samplerate.ToString();
            gBitness.Text = wt.bitness.ToString();
            gChannels.Text = wt.chans.ToString();
            gBE.Checked = wt.big_endian;
            gLE.Checked = !wt.big_endian;
            
            gPath.Text = wt.srcdir;
            gDelet.Checked = wt.delete != 0;
        }

        int parse(string txt, string name, int[] legal)
        {
            int ret = 0;
            int.TryParse(txt, out ret);
            
            bool ok = false;
            string legals = "";
            foreach (int test in legal)
            {
                ok = ok || test == ret;
                legals += ", " + test;
            }

            if (!ok)
            {
                ret = 0;
                MessageBox.Show(name + " must be one of the following:\n  " + legals.Substring(2));
            }

            return ret;
        }
        
        private void gSave_Click(object sender, EventArgs e)
        {
            int rate = parse(gSampleRate.Text, "samplerate", new int[] { 
                8000, 11025, 16000, 22050, 32000,
                44100, 48000, 88200, 96000
            });
            int bits = parse(gBitness.Text, "bitness", new int[] { 16 });
            int ch = parse(gChannels.Text, "channels", new int[] { 2 });

            if (rate == 0 || bits == 0 || ch == 0)
                return;

            wt.srcdir = gPath.Text;
            wt.setFormat(rate, gBE.Checked, bits, ch);
            wt.delete = gDelet.Checked ? 10 : 0;

            this.Close();
        }

        private void gBrowse_Click(object sender, EventArgs e)
        {
            var b = new System.Windows.Forms.FolderBrowserDialog();
            b.SelectedPath = gPath.Text;
            b.ShowDialog();
            
            string str = b.SelectedPath;
            if (str != "")
                gPath.Text = str;
        }

        private void UI_WavetailCfg_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
        }

        private void gSampleRate_TextChanged(object sender, EventArgs e)
        {
            var tb = sender as TextBox;
            var ofs = tb.SelectionStart;
            tb.Text = new Regex(@"[^\d]").Replace(tb.Text, "");
            tb.SelectionStart = ofs;
        }

    }
}
