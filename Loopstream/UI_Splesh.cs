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
        public Progress progress;
        Label betabadge;

        //public Label pb { get { return label3; } set { } }

        private void Splesh_Load(object sender, EventArgs e)
        {
            this.Icon = Program.icon;
            progress = new Progress(this, label3);
            random = new Random();
            fx = false;
            //this.Opacity = 0.999;
            //this.TopLevel = true;

            if (Program.beta > 0)
            {
                betabadge = new Label();
                betabadge.Text = "B e t a";
                betabadge.TextAlign = ContentAlignment.MiddleRight;
                betabadge.Font = new System.Drawing.Font(betabadge.Font.FontFamily, betabadge.Font.SizeInPoints * 4f);
                betabadge.ForeColor = Color.FromArgb(215, 215, 215);
                betabadge.ForeColor = Color.FromArgb(255, 192, 64);
                betabadge.BackColor = Color.FromArgb(65, 65, 65);
                betabadge.Bounds = new Rectangle(0, 384, 262, 112);
                this.Controls.Add(betabadge);
                betabadge.BringToFront();
            }
            else betabadge = null;
        }

        public void vis()
        {
            label1.Visible = label2.Visible = label3.Visible = true;
            Application.DoEvents();
        }

        public void unvis()
        {
            label1.Visible = label2.Visible = false;
            Application.DoEvents();
        }

        /*public void prog(int cur, int max)
        {
            label3.Visible = true;
            label3.Width = (int)(this.Width * cur * 1.0 / max);
            Application.DoEvents();
        }*/

        private void Splesh_MouseClick(object sender, MouseEventArgs e)
        {
            //gtfo();
        }

        private void Splesh_KeyPress(object sender, KeyPressEventArgs e)
        {
            //gtfo();
        }

        public void gtfo()
        {
            label1.Visible = label2.Visible = label3.Visible = false;
            
            if (betabadge != null)
                betabadge.Visible = false;

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
            try
            {
                using (Graphics g = Graphics.FromHwnd(this.Handle))
                {
                    Brush brush = new SolidBrush(Color.FromArgb(0, 255, 0));
                    Pen pen = new Pen(brush);
                    int ticker = 0;
                    while (shreds.Count > 0)
                    {
                        int i = random.Next(shreds.Count / 8) + 7 * shreds.Count / 8;
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
            }
            catch { }
            this.Close();
            this.Dispose();
        }
    }
}
