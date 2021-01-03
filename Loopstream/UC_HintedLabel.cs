using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Text;

namespace Loopstream
{
    public partial class HintedLabel : Label
    {
        public TextRenderingHint Hinting { get; set; }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.TextRenderingHint = Hinting;
            base.OnPaint(e);
        }
    }
}
