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
    public partial class UI_Graph2 : Form
    {
        public UI_Graph2(LSBuffers.Buf bv)
        {
            InitializeComponent();
            this.bv = bv;
        }
        LSBuffers.Buf bv;
        double mi, mo, md;
        List<double> lr, lw, ld;
        LinearGradientBrush mgrad, ograd;
        Color cmgrad, cograd;
        Brush penbrush;
        Brush brush;
        Pen pen, bg;
        bool intimer;
        bool stopping;

        private void UI_Graph2_Load(object sender, EventArgs e)
        {
            lr = new List<double>();
            lw = new List<double>();
            ld = new List<double>();
            mi = mo = md = 1;

            bool inv = SystemColors.Control.R < 128;
            pen = new Pen(inv ?
                Color.FromArgb(192, 255, 160) :
                Color.FromArgb(128, 255, 96));

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

            pb.BackgroundImage = null;
            penbrush = SystemBrushes.ControlText;
            brush = SystemBrushes.Control;
            pen.Dispose();
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

        void t_Tick(object sender, EventArgs e)
        {
            if (intimer)
                return;
            intimer = true;

            Bitmap b = new Bitmap(pb.Width, pb.Height);
            mgrad = new LinearGradientBrush(Point.Empty, new Point(0, b.Height), cmgrad, SystemColors.Control);
            ograd = new LinearGradientBrush(Point.Empty, new Point(0, b.Height), cograd, SystemColors.Control);
            double vi = bv.i;
            double vo = bv.o;
            //double vd = vi > vo ? vi - vo : vo - vi;
            double vd = Math.Abs(bv.d);
            lr.Add(vi);
            lw.Add(vo);
            ld.Add(vd);
            if (vi > mi) mi = vi;
            if (vo > mo) mo = vo;
            if (vd > md) md = vd;
            //if (vo > md) md = vo;
            //if (vi > md) md = vi;
            using (Graphics g = Graphics.FromImage(b))
            {
                g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Low;
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighSpeed;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
                int numPoints = 60 * 30 + 2; // 1 sample per 200msec

                double fact = bv.s > 0 ? bv.s : md;
                double mulY = b.Height * 1.0 / fact;
                double mulX = b.Width * 1.0 / (numPoints - 2);
                g.FillRectangle(brush, 0, 0, b.Width, b.Height);

                if (bv.s <= 0 || true)
                {
                    paintshit(ld, numPoints, mulX, mulY, b.Width, b.Height, g, mgrad, cmgrad, +1);
                    paintshit(ld, numPoints, mulX, mulY, b.Width, b.Height, g, ograd, cmgrad, -1);
                    paintshit(lr, numPoints, mulX, mulY, b.Width, b.Height, g, Pens.Red);
                    paintshit(lw, numPoints, mulX, mulY, b.Width, b.Height, g, Pens.Blue);
                }
                else
                {
                    paintshit(lr, numPoints, mulX, mulY, b.Width, b.Height, g, mgrad, cmgrad, +1);
                    paintshit(lw, numPoints, mulX, mulY, b.Width, b.Height, g, ograd, cograd, +1);
                }
                this.Text = bv.name + ",      cd) " + th(vd) + "      in) " + th(vi) + "      out) " + th(vo) + "      md) " + th(bv.s);
            }
            if (pb.BackgroundImage != null)
                pb.BackgroundImage.Dispose();
            pb.BackgroundImage = b;

            if (stopping)
            {
                ((Timer)sender).Stop();
                ((Timer)sender).Dispose();
                if (pb.BackgroundImage != null)
                    pb.BackgroundImage.Dispose();

                mgrad.Dispose();
                ograd.Dispose();
                this.Close();
                this.Dispose();
            }
            else
                intimer = false;
        }

        void paintshit(List<double> datta, int numPoints, double mulX, double mulY, int bw, int bh, Graphics g, Pen pen)
        {
            PointF[] points = new PointF[numPoints];
            double lastsample = 0;
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
                    double smp = datta[s] / 5;
                    double sample = (smp - lastsample) + 500;
                    lastsample = smp;
                    if (sample < 0) sample = 0;

                    points[s + ofs + 1] = new PointF((float)((s + ofs) * mulX),
                        (float)(bh - sample * mulY));
                }
                points[0] = new PointF(0f, bh);
                points[points.Length - 1] = new PointF(bw, bh);
            }
            GraphicsPath gp = new GraphicsPath();
            gp.AddLines(points);
            g.DrawPath(pen, gp);
            gp.Dispose();
        }

        void paintshit(List<double> datta, int numPoints, double mulX, double mulY, int bw, int bh, Graphics g, LinearGradientBrush grad, Color cbase, double sign)
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
                    double sample = datta[s] * sign;
                    if (sample < 0) sample = 0;

                    points[s + ofs + 1] = new PointF((float)((s + ofs) * mulX),
                        (float)(bh - sample * mulY));
                }
                points[0] = new PointF(0f, bh);
                points[points.Length - 1] = new PointF(bw, bh);
            }
            GraphicsPath gp = new GraphicsPath();
            gp.AddLines(points);
            g.FillPath(grad, gp);
            g.DrawPath(new Pen(cbase, 2f), gp);
            gp.Dispose();
        }

        private void UI_Graph2_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.stopping = true;
        }

        string th(double v)
        {
            return th(((long)v).ToString());
        }

        string th(string v)
        {
            string ret = "";
            while (v.Length > 0)
            {
                int ofs = v.Length - 3;
                if (ofs < 0)
                    ofs = 0;

                ret = v.Substring(ofs) + " " + ret;
                v = v.Substring(0, ofs);
            }
            return ret.Trim();
        }
    }
}