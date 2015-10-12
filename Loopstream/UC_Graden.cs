using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LoopStream
{
    public class Graden : Label
    {
        public Graden()
            : base()
        {
            co = 1;
            Direction = false;
            colorA = SystemColors.Control;
            colorB = SystemColors.ControlLight;
            renderedOpacity = 1;
            ca = _ca = colorA;
            cb = _cb = colorB;
        }

        Color ca, cb;
        Color _ca, _cb;
        double renderedOpacity;
        public bool Direction { get; set; }
        public Color colorA { get { return _ca; } set { renderedOpacity = 1; _ca = ca = value; } }
        public Color colorB { get { return _cb; } set { renderedOpacity = 1; _cb = cb = value; } }
        public double co { get; set; }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                base.OnPaintBackground(pevent);
                return;
            }
            if (renderedOpacity != co)
            {
                renderedOpacity = co;
                ca = Color.FromArgb((int)(co * 255), _ca.R, _ca.G, _ca.B);
                cb = Color.FromArgb((int)(co * 255), _cb.R, _cb.G, _cb.B);
            }
            Graphics g = pevent.Graphics;
            Rectangle re = pevent.ClipRectangle;
            LinearGradientBrush lg = new LinearGradientBrush(re, ca, cb, Direction ? 90 : 270);
            g.FillRectangle(lg, re);
        }
    }
}
