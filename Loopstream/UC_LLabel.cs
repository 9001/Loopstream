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
    public class LLabel : Label
    {
        int w, h;
        Image bg;
        Image sha;
        Pen dark;
        Pen light;
        Color lightF, light0;
        LinearGradientBrush volGrad;
        Timer tinval;
        //int debounce;
        
        public LLabel()
            : base()
        {
            //debounce = 0;
            src = null;
            tinval = null;
            agrad1 = SystemColors.ScrollBar;
            agrad2 = SystemColors.ScrollBar;
            //olevel = LicenseManager.UsageMode == LicenseUsageMode.Designtime ? 1 : 0;
            olevel = 0;
            applyGradient();
            //applyLevel();
            
            //bg = Bitmap.FromFile(@"C:\Users\ed\Documents\Visual Studio 2012\Projects\Loopstream\Loopstream\res\volbar.png");
            bg = new Bitmap(16, 16);
            sha = new Bitmap(96, 96);
            w = bg.Width;
            h = bg.Height;
            dark = new Pen(new SolidBrush(SystemColors.ControlDark));
            light = new Pen(new SolidBrush(SystemColors.ControlLightLight));
            Color ca = SystemColors.ControlLight;
            Color cb = SystemColors.ControlLightLight;
            Color one, two;
            try
            {
                one = Color.FromArgb(
                    ca.R + (cb.R - ca.R) / 3,
                    ca.G + (cb.G - ca.G) / 3,
                    ca.B + (cb.B - ca.B) / 3);
                two = Color.FromArgb(
                    one.R + (cb.R - ca.R) / 3,
                    one.G + (cb.G - ca.G) / 3,
                    one.B + (cb.B - ca.B) / 3);
            }
            catch
            {
                // TODO: Check order
                one = ca;
                two = cb;
            }
            one = Color.FromArgb(64,
                SystemColors.ControlDark.R,
                SystemColors.ControlDark.G,
                SystemColors.ControlDark.B);
            two = Color.FromArgb(64,
                SystemColors.ControlLightLight.R,
                SystemColors.ControlLightLight.G,
                SystemColors.ControlLightLight.B);
            
            lightF = SystemColors.ControlLightLight;
            light0 = Color.FromArgb(0,
                lightF.R,
                lightF.G,
                lightF.B);

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
            Image bbg = new Bitmap(bg.Width * 6, bg.Height * 6);
            using (Graphics g = Graphics.FromImage(bbg))
            {
                for (int x = 0; x < bbg.Width; x += w)
                {
                    for (int y = 0; y < bbg.Height; y += h)
                    {
                        g.DrawImageUnscaled(bg, x, y);
                    }
                }
            }
            bg.Dispose();
            bg = bbg;
            w = bg.Width;
            h = bg.Height;

            using (Graphics g = Graphics.FromImage(sha))
            {
                int zh = sha.Height;
                for (int y = 0; y < zh; y++)
                {
                    int ih = zh - y;
                    double opac = ih * 1.0 / zh;
                    opac = 0.9 * opac * opac;
                    g.DrawLine(new Pen(Color.FromArgb(
                        (int)(opac * 255), lightF.R, lightF.G, lightF.B)),
                        0, y, sha.Width, y);
                }
            }

            Skinner.add(this);
        }
        ~LLabel()
        {
            Skinner.rem(this);
        }

        double olevel;
        Color agrad1, agrad2;
        NPatch.VolumeSlider vs;
        public NPatch.VolumeSlider src { get { return vs; } set { vs = value; applyLevel(); } }
        //public double A_LEVEL { get { return alevel; } set { alevel = value; applyLevel(); } }
        public Color A_GRAD_1 { get { return agrad1; } set { agrad1 = value; applyGradient(); } }
        public Color A_GRAD_2 { get { return agrad2; } set { agrad2 = value; applyGradient(); } }
        void applyGradient()
        {
            Rectangle re = new Rectangle(0, 0, 96, 256 + 40 + 40);
            volGrad = new LinearGradientBrush(re, agrad1, agrad2, 270);
        }
        void applyLevel()
        {
            if (tinval == null &&
                LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            {
                tinval = new Timer();
                tinval.Interval = 25;
                tinval.Start();
                tinval.Tick += tinval_Tick;
            }
        }

        void tinval_Tick(object sender, EventArgs e)
        {
            double alevel = (vs != null && vs.vuAge < 16) ? vs.VU : 0;
            olevel =
                alevel > olevel ?
                alevel * 0.30 + olevel * 0.70 :
                alevel * 0.06 + olevel * 0.94;
            //this.Text = alevel.ToString();
            this.Invalidate();
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
            //g.FillRectangle(new SolidBrush(gra), re);
            
            int ofsTop = this.Height > 256 + 40 ? this.Height - (256 + 40) : 0;
            int ofsHei = this.Height - 40;
            ofsHei = ofsHei > 255 ? 255 : ofsHei < 0 ? 0 : ofsHei;
            ofsTop += (int)(ofsHei * (1 - olevel));
            g.FillRectangle(volGrad, re);
            //g.FillRectangle(new SolidBrush(gra), 0, ofsTop, 96, this.Height - ofsTop);
            g.FillRectangle(new SolidBrush(lightF), 0, 0, 96, ofsTop);
            for (int x = 0; x < re.Width; x += w)
            {
                for (int y = 0; y < re.Height; y += h)
                {
                    g.DrawImageUnscaled(bg, x, y);
                }
            }
            //g.DrawImageUnscaledAndClipped(sha, re);
            g.DrawImageUnscaled(sha, re.Location);
            
            //int ofs = (MAH_HEIGHT * 32) / 326;
            /*g.DrawString(
                ((int)(alevel * 256)).ToString(),
                System.Drawing.SystemFonts.CaptionFont,
                Brushes.Black,
                10.0f, 10.0f);*/
            //MAH_HEIGHT = Height < 1 ? 1 : Height;

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
