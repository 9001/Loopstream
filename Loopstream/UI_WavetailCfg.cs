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
            // waveformat is float, use members
            gSampleRate.Text = wt.samplerate.ToString();
            gBitness.Text = wt.bitness.ToString();
            gChannels.Text = wt.chans.ToString();
            
            gPath.Text = wt.srcdir;
            gDelet.Checked = wt.delete != 0;
        }
        
        private void gSave_Click(object sender, EventArgs e)
        {
            var rate = int.Parse(gSampleRate.Text);
            var bits = int.Parse(gBitness.Text);
            var ch = int.Parse(gChannels.Text);

            wt.srcdir = gPath.Text;
            wt.setFormat(rate, bits, ch);
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

    }
}
