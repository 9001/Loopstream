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
    public partial class UI_Status : Form
    {
        public UI_Status(LSSettings settings)
        {
            this.settings = settings;
            InitializeComponent();
        }

        Timer t;
        LSSettings settings;

        private void UI_Status_Load(object sender, EventArgs e)
        {
            t = new Timer();
            t.Interval = 10;
            t.Tick += t_Tick;
            t.Start();
            this.Icon = Program.icon;
        }
        private void UI_Status_FormClosing(object sender, FormClosingEventArgs e)
        {
            t.Stop();
        }

        void t_Tick(object sender, EventArgs e)
        {
            mp3.Text = "mp3  " + Logger.mp3;
            ogg.Text = "ogg  " + Logger.ogg;
            opus.Text = "opus " + Logger.opus;
            pcm.Text = "pcm  " + Logger.pcm;
            med.Text = "med  " + Logger.med;
            mix.Text = "mix  " + Logger.mix;
            tag.Text = "tag  " + Logger.tag;
            app.Text = "app  " + Logger.app;
            wtail.Text = "wt   " + Logger.wt;
            now.Text = "---  " + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.ffff") + "  ------ |";
        }

        void pop(Logger log)
        {
            string p = log.compile(96);
            
            Clipboard.Clear();
            if (p != "")
                Clipboard.SetText(p);
            
            MessageBox.Show(p);
        }

        private void mp3_Click(object sender, EventArgs e)
        {
            pop(Logger.mp3);
        }

        private void ogg_Click(object sender, EventArgs e)
        {
            pop(Logger.ogg);
        }

        private void opus_Click(object sender, EventArgs e)
        {
            pop(Logger.opus);
        }

        private void pcm_Click(object sender, EventArgs e)
        {
            pop(Logger.pcm);
        }

        private void med_Click(object sender, EventArgs e)
        {
            pop(Logger.med);
        }

        private void mix_Click(object sender, EventArgs e)
        {
            pop(Logger.mix);
        }

        private void tag_Click(object sender, EventArgs e)
        {
            pop(Logger.tag);
        }

        private void app_Click(object sender, EventArgs e)
        {
            pop(Logger.app);
        }

        private void wtail_Click(object sender, EventArgs e)
        {
            pop(Logger.wt);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            // it's time to think of a way to optimize this
            sb.AppendLine("Loopstream debug cache");
            sb.AppendLine("\n\n\n\n\nCache for ogg"); sb.AppendLine(Logger.ogg.compile());
            sb.AppendLine("\n\n\n\n\nCache for opus"); sb.AppendLine(Logger.opus.compile());
            sb.AppendLine("\n\n\n\n\nCache for mp3"); sb.AppendLine(Logger.mp3.compile());
            sb.AppendLine("\n\n\n\n\nCache for pcm"); sb.AppendLine(Logger.pcm.compile());
            sb.AppendLine("\n\n\n\n\nCache for med"); sb.AppendLine(Logger.med.compile());
            sb.AppendLine("\n\n\n\n\nCache for mix"); sb.AppendLine(Logger.mix.compile());
            sb.AppendLine("\n\n\n\n\nCache for tag"); sb.AppendLine(Logger.tag.compile());
            sb.AppendLine("\n\n\n\n\nCache for wt"); sb.AppendLine(Logger.wt.compile());
            sb.AppendLine("\n\n\n\n\nCache for app"); sb.AppendLine(Logger.app.compile());
            Clipboard.Clear();
            Clipboard.SetText(sb.ToString());
            MessageBox.Show("ok");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new UI_Graph(settings).Show();
            this.Close();
        }
    }
}
