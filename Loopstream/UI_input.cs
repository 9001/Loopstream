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
    public partial class UI_input : Form
    {
        public UI_input(LSSettings.LSMeta meta, List<LSSettings.LSMeta> metas)
        {
            this.metas = metas;
            this.meta = meta;
            InitializeComponent();
            textBox1.Text = meta.tit;
            wasAdded = true;
        }

        public bool wasAdded;
        LSSettings.LSMeta meta;
        List<LSSettings.LSMeta> metas;
        private void UI_input_Load(object sender, EventArgs e)
        {
            this.Icon = Program.icon;
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                Application.DoEvents();
                button2_Click(sender, e);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            wasAdded = false;
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string yea = textBox1.Text.Replace("\r", "").Replace("\n", "").Trim();
            meta.tit = yea;
            foreach (var v in metas)
            {
                if (v.tit.ToLower() == yea.ToLower())
                {
                    if (DialogResult.Yes == MessageBox.Show(
                        "This profile already exists!\n\n" +
                        "Do you want to overwrite it?",
                        "Confirm OVERWRITE",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning))
                    {
                        v.apply(meta);
                        this.Hide();
                        return;
                    }
                    else
                    {
                        return;
                    }
                }
            }
            metas.Add(meta);
            this.Hide();
        }
    }
}
