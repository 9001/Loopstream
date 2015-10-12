using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoopStream
{
    public class LLabel : Label
    {
        int w, h;
        Image bg;
        Color gra, grb, grc;
        Pen dark;
        Pen light;
        public LLabel()
            : base()
        {
            //bg = Bitmap.FromFile(@"C:\Users\ed\Documents\Visual Studio 2012\Projects\LoopStream\LoopStream\res\volbar.png");
            bg = new Bitmap(16, 16);
            w = bg.Width;
            h = bg.Height;
            dark = new Pen(new SolidBrush(SystemColors.ControlDark));
            light = new Pen(new SolidBrush(SystemColors.ControlLightLight));
            Color ca = SystemColors.ControlLight;
            Color cb = SystemColors.ControlLightLight;
            Color one = Color.FromArgb(
                ca.R + (cb.R - ca.R) / 3,
                ca.G + (cb.G - ca.G) / 3,
                ca.B + (cb.B - ca.B) / 3);
            Color two = Color.FromArgb(
                one.R + (cb.R - ca.R) / 3,
                one.G + (cb.G - ca.G) / 3,
                one.B + (cb.B - ca.B) / 3);
            gra = Color.FromArgb(255, cb.R, cb.G, cb.B);
            grb = Color.FromArgb(96, ca.R, ca.G, ca.B);
            grc = Color.FromArgb(16, 0, 0, 0);
            using (Graphics g = Graphics.FromImage(bg))
            {
                g.FillRectangle(new SolidBrush(one), 0, 0, w, h);
                Pen pen = new Pen(new SolidBrush(two));
                for (int a = -16; a < w; a++)
                {
                    if (a > -8 && a < 0 || a > 8) continue;
                    g.DrawLine(pen, new Point(a, 16), new Point(a + 16, 0));
                }
            }
        }
        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                base.OnPaintBackground(pevent);
                return;
            }
            Graphics g = pevent.Graphics;
            Rectangle re = pevent.ClipRectangle;
            for (int x = 0; x < re.Width; x += w)
            {
                for (int y = 0; y < re.Height; y += h)
                {
                    g.DrawImage(bg, x, y);
                }
            }
            //re = new Rectangle(re.X, re.Y, re.Width, re.Height / 2);
            LinearGradientBrush lg = new LinearGradientBrush(re, gra, grb, 270);
            g.FillRectangle(lg, re);

            int top = this.Top;
            if (top >= 0)
            {
                g.DrawLine(dark, re.X, re.Y, re.X + re.Width, re.Y);
                g.DrawLine(light, re.X, re.Y + 1, re.X + re.Width, re.Y + 1);
            }
            else if (top == -1)
            {
                g.DrawLine(light, re.X, re.Y, re.X + re.Width, re.Y);
            }

            /*re = new Rectangle(re.X, re.Y, re.Width / 2, re.Height);
            lg = new LinearGradientBrush(re, grb, gra, 180);
            g.FillRectangle(lg, re);
            re = new Rectangle(re.X + re.Width + 1, re.Y, re.Width, re.Height);
            lg = new LinearGradientBrush(re, gra, grb, 180);
            g.FillRectangle(lg, re);*/
        }
    }
}
