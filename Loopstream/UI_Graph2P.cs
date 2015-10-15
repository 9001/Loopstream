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
    public partial class UI_Graph2P : Form
    {
        public UI_Graph2P()
        {
            InitializeComponent();
        }

        private void UI_Buffers_Load(object sender, EventArgs e)
        {
            int l = 6;
            foreach (var b in LSBuffers.bufs)
            {
                var c = new Button();
                this.Controls.Add(c);
                c.Width = this.Width - 36;
                c.Click += c_Click;
                c.Text = b.name;
                c.Left = 12;
                c.Top = l;
                c.Tag = b;
                l += c.Height + 6;
            }
            this.Height += l;
        }

        void c_Click(object sender, EventArgs e)
        {
            new UI_Graph2(((LSBuffers.Buf)((Button)sender).Tag)).Show();
            this.Show();
            this.Focus();
        }
    }
}
