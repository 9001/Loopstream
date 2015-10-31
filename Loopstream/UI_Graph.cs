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
        Color cmgrad, cograd;
        LinearGradientBrush mgrad, ograd;
        Pen red, orn, grn, bg;
        bool intimer, stopping;

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
            cmgrad = Color.FromArgb(
                SystemColors.Control.R - dr / 6,
                SystemColors.Control.G - dg / 4,
                SystemColors.Control.B - db / 3);
            cograd = Color.FromArgb(
                SystemColors.Control.R - dr / 4,
                SystemColors.Control.G - dg / 3,
                SystemColors.Control.B - db / 6);

            pictureBox1.BackgroundImage = null;
            penbrush = SystemBrushes.ControlText;
            brush = SystemBrushes.Control;
            pen = SystemPens.ControlText;
            bg = SystemPens.Control;
            intimer = false;
            stopping = false;
            Timer t = new Timer();
            t.Tick += t_Tick;
            t.Interval = 200;
            t.Start();
            this.Icon = Program.icon;
        }

        void paintshit(List<double> datta, int numPoints, double mulX, double mulY, int bw, int bh, Graphics g, LinearGradientBrush grad, Color cbase)
        {
            PointF[] points = new PointF[numPoints];
            lock (datta)
            {
                int s = 0;
                int samples = datta.Count;
                for (; s < (points.Length - 2) - samples; s++)
                {
                    points[s + 1] = new PointF((float)(s * mulX), bh);
                }
                int ofs = (points.Length - 2) - samples;
                s = Math.Max(0, samples - (points.Length - 2));
                for (; s < samples; s++)
                {
                    points[s + ofs + 1] = new PointF((float)((s + ofs) * mulX),
                        (float)(bh - datta[s] * mulY));
                }
                points[0] = new PointF(0f, bh);
                points[points.Length - 1] = new PointF(bw, bh);
            }
            GraphicsPath gp = new GraphicsPath();
            gp.AddLines(points);
            g.FillPath(grad, gp);
            g.DrawPath(new Pen(cbase, 2f), gp);
        }

        void t_Tick(object sender, EventArgs e)
        {
            if (pictureBox1.Width < 10 || pictureBox1.Height < 10)
                return;

            Bitmap b = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            mgrad = new LinearGradientBrush(Point.Empty, new Point(0, b.Height), cmgrad, SystemColors.Control);
            ograd = new LinearGradientBrush(Point.Empty, new Point(0, b.Height), cograd, SystemColors.Control);
            using (Graphics g = Graphics.FromImage(b))
            {
                g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Low;
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighSpeed;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
                int numPoints = 60 * 5 + 2; // 1 sample per 200msec
                
                /*double factOGG = 0;
                lock(Logger.bitrateo)
                    foreach (double d in Logger.bitrateo)
                        factOGG = Math.Max(factOGG, d);
                factOGG *= 1.1;
                double factMP3 = settings.mp3.bitrate * 1.1;*/
                long maxbitrate = Math.Max(
                    settings.mp3.bitrate, (
                    settings.ogg.compression == LSSettings.LSCompression.q ? (
                    settings.ogg.quality == 0 ? 80 :
                    settings.ogg.quality == 1 ? 96 :
                    settings.ogg.quality == 2 ? 112 :
                    settings.ogg.quality == 3 ? 128 :
                    settings.ogg.quality == 4 ? 160 :
                    settings.ogg.quality == 5 ? 192 :
                    settings.ogg.quality == 6 ? 224 :
                    settings.ogg.quality == 7 ? 256 :
                    settings.ogg.quality == 8 ? 320 :
                    settings.ogg.quality == 9 ? 500 :
                    settings.ogg.quality == 10 ? 1000 : 128) :
                    settings.ogg.bitrate));
                double fact = maxbitrate * 1.1;

                double mulY = b.Height * 1.0 / fact;
                double mulX = b.Width * 1.0 / (numPoints - 2);
                g.FillRectangle(brush, 0, 0, b.Width, b.Height);
                
                paintshit(Logger.bitratem, numPoints, mulX, mulY, b.Width, b.Height, g, mgrad, cmgrad);
                paintshit(Logger.bitrateo, numPoints, mulX, mulY, b.Width, b.Height, g, ograd, cograd);

                string str = maxbitrate + "kbps";
                int ty = (int)(b.Height - maxbitrate * mulY);
                SizeF sz = g.MeasureString(str, this.Font);
                g.DrawLine(grn, 0, ty, b.Width, ty);
                //g.DrawLine(bg, 0, ty - 1, b.Width, ty - 1);
                //g.DrawLine(bg, 0, ty + 1, b.Width, ty + 1);
                g.DrawString(str, this.Font, penbrush, 8f, (float)(ty - Math.Ceiling(sz.Height) + 1));

                str = "Connection Lost";
                ty = (int)(b.Height - settings.mp3.bitrate * mulY * settings.lim_drop_DEPRECATED);
                g.DrawLine(red, 0, ty, b.Width, ty);
                //g.DrawLine(bg, 0, ty - 1, b.Width, ty - 1);
                //g.DrawLine(bg, 0, ty + 1, b.Width, ty + 1);
                g.DrawString(str, this.Font, penbrush, 8f, (float)(ty + 1));

                str = "Poor Connection";
                ty = (int)(b.Height - settings.mp3.bitrate * mulY * settings.lim_poor_DEPRECATED);
                g.DrawLine(orn, 0, ty, b.Width, ty);
                //g.DrawLine(bg, 0, ty - 1, b.Width, ty - 1);
                //g.DrawLine(bg, 0, ty + 1, b.Width, ty + 1);
                g.DrawString(str, this.Font, penbrush, 8f, (float)(ty + 1));

                //g.DrawLines(pen, points);
            }
            if (pictureBox1.BackgroundImage != null)
                pictureBox1.BackgroundImage.Dispose();
            pictureBox1.BackgroundImage = b;

            if (stopping)
            {
                ((Timer)sender).Stop();
                ((Timer)sender).Dispose();
                if (pictureBox1.BackgroundImage != null)
                    pictureBox1.BackgroundImage.Dispose();

                mgrad.Dispose();
                ograd.Dispose();
                this.Close();
                this.Dispose();
            }
            else
                intimer = false;
        }

        private void UI_Graph_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.stopping = true;
        }
    }
}
