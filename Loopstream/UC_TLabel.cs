using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Loopstream
{
    public class TLabel : Label
    {
        Pen pen;
        Image bg;
        Brush ctl;
        Color gra, grb;
        public TLabel()
            : base()
        {
            ctl = new SolidBrush(Color.Fuchsia);
            pen = new Pen(new SolidBrush(SystemColors.ControlDark));
            Color ca = SystemColors.ControlLightLight;
            Color cb = SystemColors.Control;
            try
            {
                grb = Color.FromArgb(
                    cb.R + (ca.R - cb.R) / 3,
                    cb.G + (ca.G - cb.G) / 3,
                    cb.B + (ca.B - cb.B) / 3);
                grb = cb;
                gra = Color.FromArgb(
                    grb.R + (ca.R - cb.R) / 2,
                    grb.G + (ca.G - cb.G) / 2,
                    grb.B + (ca.B - cb.B) / 2);
            }
            catch
            {
                gra = ca;
                grb = cb;
            }
            ShadeBack = false;
        }
        public bool ShadeBack;
        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                base.OnPaintBackground(pevent);
                return;
            }
            Graphics g = pevent.Graphics;
            Rectangle re = pevent.ClipRectangle;
            Rectangle fre = new Rectangle(0, 11, this.Width - 1, this.Height - 11);
            LinearGradientBrush lg = new LinearGradientBrush(fre, gra, grb, 90);
            ctl.Dispose(); // HOW AND WHY WAS THIS NECESSARY
            ctl = new SolidBrush(this.BackColor);
            g.FillRectangle(ctl, re);

            if (!ShadeBack) return;

            GraphicsPath gp = new GraphicsPath();
            gp.AddLines(new Point[]{
                new Point(0, fre.Height + fre.Top),
                new Point(fre.Top, fre.Top),
                new Point(fre.Width - fre.Top, fre.Top),
                new Point(fre.Width, fre.Height + fre.Top),
            });

            g.CompositingMode = CompositingMode.SourceOver;
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.InterpolationMode = InterpolationMode.High;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.FillPath(lg, gp);
            g.DrawPath(pen, gp);
        }
    }
}
