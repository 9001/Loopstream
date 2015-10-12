using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Loopstream
{
    public partial class Splesh : Form
    {
        public Splesh()
        {
            InitializeComponent();
        }

        Random random;
        public bool fx;

        public Label pb { get { return label3; } set { } }

        private void Splesh_Load(object sender, EventArgs e)
        {
            this.Icon = Program.icon;
            random = new Random();
            fx = false;
            //this.Opacity = 0.999;
            //this.TopLevel = true;
        }

        public void vis()
        {
            /*MessageBox.Show(
                "Hello there!\r\n" +
                "\r\n" +
                "Since this is your first run, here's a\r\n" +
                "short list of things that don't work yet:\r\n" +
                "\r\n" +
                "    - Streaming to OGG/Vorbis\r\n" +
                "       (icecast disconnects when you stream silence)\r\n" +
                "\r\n" +
                "That's about it.\r\n" +
                "Enjoy streaming!");*/
            label1.Visible = label2.Visible = label3.Visible = true;
            Application.DoEvents();
        }

        public void prog(int cur, int max)
        {
            label3.Visible = true;
            label3.Width = (int)(this.Width * cur * 1.0 / max);
            Application.DoEvents();
        }

        private void Splesh_MouseClick(object sender, MouseEventArgs e)
        {
            gtfo();
        }

        private void Splesh_KeyPress(object sender, KeyPressEventArgs e)
        {
            gtfo();
        }

        public void gtfo()
        {
            label1.Visible = label2.Visible = label3.Visible = false;
            Application.DoEvents();
            if (fx)
            {
                int f = random.Next(2);
                switch (f)
                {
                    case 0: gtfo1(); break;
                    case 1: gtfo2(); break;
                }
            }
            this.Close();
            this.Dispose();
        }

        void gtfo1()
        {
            int sub = 1;
            for (double a = 0.9; a > 0; a -= 0.03)
            {
                this.Opacity = a;
                Application.DoEvents();
                System.Threading.Thread.Sleep(10);
                this.Top -= ++sub / 3;
            }
        }

        void gtfo2()
        {
            int w = this.Width;
            int h = this.Height;
            List<int> shreds = new List<int>();
            for (int a = 0; a < h; a++)
            {
                shreds.Add(a);
            }
            using (Graphics g = Graphics.FromHwnd(this.Handle))
            {
                Brush brush = new SolidBrush(Color.FromArgb(0,255,0));
                Pen pen = new Pen(brush);
                int ticker = 0;
                while (shreds.Count > 0)
                {
                    int i = random.Next(shreds.Count/8) + 7 * shreds.Count / 8;
                    int line = shreds[i];
                    shreds.RemoveAt(i);
                    
                    g.DrawLine(pen, 0, line, w, line);
                    Application.DoEvents();
                    if (++ticker % 20 == 0)
                    {
                        System.Threading.Thread.Sleep(10);
                    }
                }
            }
            this.Close();
            this.Dispose();
        }
    }
}
