using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace Loopstream
{
    public partial class UC_Troggle : UserControl
    {
        Bitmap bLoad, bSave;
        public enum Modes { Load, Save };
        Modes mode = Modes.Load;
        public Modes Mode
        {
            get { return mode; }
            set { mode = value; pb.BackgroundImage = mode == Modes.Load ? bLoad : bSave; this.Invalidate(); }
        }

        public UC_Troggle()
        {
            InitializeComponent();
            bLoad = bSave = null;
            init();
            Resize += UC_Troggle_Resize;
        }
        void UC_Troggle_Resize(object sender, EventArgs e)
        {
            init();
        }
        void init()
        {
            int w = pb.Width;
            int h = pb.Height;
            if (bLoad != null) bLoad.Dispose();
            if (bSave != null) bSave.Dispose();
            bLoad = new Bitmap(w, h);
            bSave = new Bitmap(w, h);

            using (Graphics g = Graphics.FromImage(bLoad))
            {
                radGrad(g, w, h, true);
            }
            using (Graphics g = Graphics.FromImage(bSave))
            {
                radGrad(g, w, h, false);
            }
            pb.BackgroundImage = bLoad;
        }
        void radGrad(Graphics g, int w, int h, bool load)
        {
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;

            Color cbg = this.BackColor;
            Brush bg = new SolidBrush(cbg);
            g.FillRectangle(bg, 0, 0, w, h);
            bool isDark = this.BackColor.R < 128;

            /*double dR = (SystemColors.ControlLightLight.R - cbg.R) / (w / 2.0);
            double dG = (SystemColors.ControlLightLight.G - cbg.G) / (w / 2.0);
            double dB = (SystemColors.ControlLightLight.B - cbg.B) / (w / 2.0);
            double mul = (w / 2.0) / Math.Pow(w / 2.0, Math.E);
            for (int x = 0; x < w / 2; x++)
            {
                double log = x == 0 ? 0 : Math.Pow(x, Math.E) * mul;
                Brush b = new SolidBrush(Color.FromArgb(
                    cbg.R + (int)(dR * log),
                    cbg.G + (int)(dG * log),
                    cbg.B + (int)(dB * log)));

                double ofs = x * 1.0 / w;
                double dy = ofs * h;
                int rx = (int)ofs;
                int ry = (int)dy;
                g.FillEllipse(b, rx, ry, w - rx * 2, h - ry * 2);
            }*/

            SizeF sfLoad = g.MeasureString("Load", this.Font);
            SizeF sfSave = g.MeasureString("Save", this.Font);
            
            Brush brLoad = new SolidBrush(isDark ?
                Color.FromArgb(192, 255, 64) :
                Color.FromArgb(96, 128, 0));
            
            Brush brSave = new SolidBrush(isDark ?
                Color.FromArgb(255, 128, 64) :
                Color.FromArgb(160, 96, 0));

            PointF pSave = new PointF((w / 2) + 8, (h - sfLoad.Height) / 2);
            PointF pLoad = new PointF(((w / 2) - sfLoad.Width) - 8, (h - sfLoad.Height) / 2);
            //LinearGradientBrush lgb;
            //Rectangle rect;

            if (load)
            {
                //MessageBox.Show(w + "x" + h);
                //g.FillRectangle(bg, w / 2, 0, w, h);
                
                //rect = new Rectangle(0, 0, w / 2, h);
                //lgb = new LinearGradientBrush(rect, SystemColors.ControlLight, SystemColors.Control, 0f);
                brSave = SystemBrushes.ControlDark;
            }
            else
            {
                //g.FillRectangle(bg, 0, 0, w / 2, h);
                
                //rect = new Rectangle(w / 2, 0, w / 2, h);
                //lgb = new LinearGradientBrush(rect, SystemColors.ControlLight, SystemColors.Control, 180f);
                brLoad = SystemBrushes.ControlDark;
            }
            //g.FillRectangle(lgb, rect);
            g.DrawString("Save", this.Font, brSave, pSave.X, pSave.Y);
            g.DrawString("Load", this.Font, brLoad, pLoad.X, pLoad.Y);
            if (load)
            {
                g.DrawLine(new Pen(brLoad),
                    pLoad.X + 2, pLoad.Y + sfLoad.Height + 0f,
                    pLoad.X + sfLoad.Width - 2, pLoad.Y + sfLoad.Height + 0f);
            }
            else
            {
                g.DrawLine(new Pen(brSave),
                    pSave.X + 2, pSave.Y + sfSave.Height + 0f,
                    pSave.X + sfSave.Width - 2, pSave.Y + sfSave.Height + 0f);
            }
            //g.DrawLine(SystemPens.ControlDark, w / 2, 0, w / 2, h);
        }

        private void pb_Click(object sender, EventArgs e)
        {
            InvokeOnClick(this, null);
        }
        
    }
}
