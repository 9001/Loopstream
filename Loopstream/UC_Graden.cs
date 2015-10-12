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
    class Graden : Label
    {
        Color ca, cb;

        public Graden()
            : base()
        {
            ca = SystemColors.Control;
            cb = SystemColors.ControlLightLight;
            direction = false;
        }

        public bool direction;

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                base.OnPaintBackground(pevent);
                return;
            }
            Graphics g = pevent.Graphics;
            Rectangle re = pevent.ClipRectangle;
            LinearGradientBrush lg = new LinearGradientBrush(re, ca, cb, direction ? 90 : 270);
            g.FillRectangle(lg, re);
        }
    }
}
