using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Loopstream
{
    public partial class UI_Graph : Form
    {
        public UI_Graph(LSSettings settings)
        {
            this.settings = settings;
            InitializeComponent();
        }

        Pen pen;
        Brush brush;
        Brush penbrush;
        LSSettings settings;
        Color cgrad;
        Brush grad;
        Pen red, orn, grn, bg;

        private void UI_Graph_Load(object sender, EventArgs e)
        {
            bool inv = SystemColors.Control.R < 128;
            grn = new Pen(inv ?
                Color.FromArgb(192, 255, 160) :
                Color.FromArgb(128, 255, 96));
            orn = new Pen(inv ?
                Color.FromArgb(255, 224, 96) :
                Color.FromArgb(224, 192, 64));
            red = new Pen(inv ?
                Color.FromArgb(255, 96, 64) :
                Color.FromArgb(255, 128, 96));

            int dr = SystemColors.Control.R - SystemColors.ControlText.R;
            int dg = SystemColors.Control.G - SystemColors.ControlText.G;
            int db = SystemColors.Control.B - SystemColors.ControlText.B;
            cgrad = Color.FromArgb(
                SystemColors.Control.R - dr / 3,
                SystemColors.Control.G - dg / 3,
                SystemColors.Control.B - db / 3);

            pictureBox1.BackgroundImage = null;
            penbrush = SystemBrushes.ControlText;
            brush = SystemBrushes.Control;
            pen = SystemPens.ControlText;
            bg = SystemPens.Control;
            Timer t = new Timer();
            t.Tick += t_Tick;
            t.Interval = 200;
            t.Start();
        }

        void t_Tick(object sender, EventArgs e)
        {
            Bitmap b = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            grad = new LinearGradientBrush(Point.Empty, new Point(0, b.Height), cgrad, SystemColors.Control);
            using (Graphics g = Graphics.FromImage(b))
            {
                g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Low;
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighSpeed;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
                PointF[] points = new PointF[60 * 5 + 2]; // 1 sample per 200msec
                double mulX = b.Width * 1.0 / (points.Length - 2);
                double mulY = b.Height * 1.0 / (settings.mp3.bitrate * 1.1);
                g.FillRectangle(brush, 0, 0, b.Width, b.Height);
                lock (Logger.bitrate)
                {
                    int s = 0;
                    int samples = Logger.bitrate.Count;
                    for (; s < (points.Length - 2) - samples; s++)
                    {
                        points[s + 1] = new PointF((float)(s * mulX), b.Height);
                    }
                    int ofs = (points.Length - 2) - samples;
                    s = Math.Max(0, samples - (points.Length - 2));
                    for (; s < samples; s++)
                    {
                        points[s + ofs + 1] = new PointF((float)((s + ofs) * mulX),
                            (float)(b.Height - Logger.bitrate[s] * mulY));
                    }
                    points[0] = new PointF(0f, b.Height);
                    points[points.Length - 1] = new PointF(b.Width, b.Height);
                }
                GraphicsPath gp = new GraphicsPath();
                gp.AddLines(points);
                g.FillPath(grad, gp);

                string str = settings.mp3.bitrate + "kbps";
                int ty = (int)(b.Height - settings.mp3.bitrate * mulY);
                SizeF sz = g.MeasureString(str, this.Font);
                g.DrawLine(grn, 0, ty, b.Width, ty);
                //g.DrawLine(bg, 0, ty - 1, b.Width, ty - 1);
                //g.DrawLine(bg, 0, ty + 1, b.Width, ty + 1);
                g.DrawString(str, this.Font, penbrush, 8f, (float)(ty - Math.Ceiling(sz.Height) + 1));

                str = "Connection Lost";
                ty = (int)(b.Height - settings.mp3.bitrate * mulY * settings.lim_drop);
                g.DrawLine(red, 0, ty, b.Width, ty);
                //g.DrawLine(bg, 0, ty - 1, b.Width, ty - 1);
                //g.DrawLine(bg, 0, ty + 1, b.Width, ty + 1);
                g.DrawString(str, this.Font, penbrush, 8f, (float)(ty + 1));

                str = "Poor Connection";
                ty = (int)(b.Height - settings.mp3.bitrate * mulY * settings.lim_poor);
                g.DrawLine(orn, 0, ty, b.Width, ty);
                //g.DrawLine(bg, 0, ty - 1, b.Width, ty - 1);
                //g.DrawLine(bg, 0, ty + 1, b.Width, ty + 1);
                g.DrawString(str, this.Font, penbrush, 8f, (float)(ty + 1));

                //g.DrawLines(pen, points);
            }
            if (pictureBox1.BackgroundImage != null)
                pictureBox1.BackgroundImage.Dispose();
            pictureBox1.BackgroundImage = b;
        }
    }
}
