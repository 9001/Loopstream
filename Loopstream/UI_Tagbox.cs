using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Loopstream
{
    public partial class UI_Tagbox : Form
    {
        public UI_Tagbox(LSSettings settings, LSTag tag)
        {
            InitializeComponent();

            this.settings = settings;
            this.tag = tag;

            lastTag = null;
            lastSize = Size.Empty;
            gPic.Image = null;
            cui = null;
        }

        LSTag tag;
        LSSettings settings;
        UI_TagboxCfg cui;
        string lastTag;
        Size lastSize;

        private void UI_Tagbox_Load(object sender, EventArgs e)
        {
            Reload();

            var t = new Timer();
            t.Interval = 100;
            t.Start();
            t.Tick += t_Tick;
        }

        public void ShowAt(Rectangle bounds)
        {
            var w = settings.tboxWidth > 0 ? settings.tboxWidth : this.Width;
            var h = settings.tboxHeight > 0 ? settings.tboxHeight : this.Height;
            this.Show();
            this.Bounds = new Rectangle(new Point(
                bounds.Left + bounds.Width / 2 - w / 2,
                bounds.Top + bounds.Height / 2 - h / 2),
                new Size(w, h));
        }

        public void Reload()
        {
            var style = FontStyle.Regular;
            if (settings.tboxBold)
                style = FontStyle.Bold;
            else if (settings.tboxItalic)
                style = FontStyle.Italic;

            gTags.Font = new Font(settings.tboxFont, (float)settings.tboxSize, style);
            
            this.BackColor = gTags.BackColor = Z.hex2color(settings.tboxColorBack);
            this.ForeColor = gTags.ForeColor = Z.hex2color(settings.tboxColorFront);

            gTags.TextAlign = Z.int2alignment(settings.tboxAlign, ContentAlignment.MiddleCenter);
            
            gTags.Hinting =
                settings.tboxAntialias == "None" ? TextRenderingHint.SingleBitPerPixelGridFit :
                settings.tboxAntialias == "Gray" ? TextRenderingHint.AntiAliasGridFit :
                TextRenderingHint.ClearTypeGridFit;

            gPic.Visible = settings.tboxRendermode != "Label";
            paint(true);
            gTags.Invalidate();
        }

        void t_Tick(object sender, EventArgs e)
        {
            paint(false);
        }

        void paint(bool force)
        {
            string s = "(no tags yet)";
            if (tag != null)
                s = tag.tag.tag;

            if (!force && s == lastTag && this.Size == lastSize)
                return;

            lastTag = s;
            lastSize = this.Size;

            gTags.Text = s;

            if (!gPic.Visible)
                return;

            if (gPic.Image != null)
            {
                var old = gPic.Image;
                gPic.Image = null;
                old.Dispose();
            }

            // label kerning is busted if non-cleartype,
            // graphics hinting is busted if non-cleartype,
            // win by grayscaling cleartype (orz)

            var brushBG = new SolidBrush(gTags.BackColor);
            var bmFG = new Bitmap(gPic.Width, gPic.Height, PixelFormat.Format24bppRgb);
            using (var g = Graphics.FromImage(bmFG))
            {
                setGraphicOptions(g);
                g.TextRenderingHint = gTags.Hinting;
                g.TextContrast = 0; // 0..12

                var sz = g.MeasureString(s, gTags.Font, gPic.Width);
                var al = settings.tboxAlign;

                int margin = 4;
                
                var x = -1;
                if (al == 1 || al == 4 || al == 7)
                    x = margin;
                else if (al == 3 || al == 6 || al == 9)
                    x = (int)((gPic.Width - margin) - sz.Width);
                else
                    x = (int)((gPic.Width - sz.Width) / 2);

                var y = -1;
                if (al == 1 || al == 2 || al == 3)
                    y = margin;
                else if (al == 7 || al == 8 || al == 9)
                    y = (int)((gPic.Height - margin) - sz.Height);
                else
                    y = (int)((gPic.Height - sz.Height) / 2);

                g.FillRectangle(Brushes.Black, 0, 0, gPic.Width, gPic.Height);

                g.DrawString(s, gTags.Font, Brushes.White, new RectangleF(
                    x, y, gPic.Width - margin * 2, gPic.Height - margin * 2));
            }

            var bmBG = new Bitmap(gPic.Width, gPic.Height);
            using (Graphics g = Graphics.FromImage(bmBG))
            {
                setGraphicOptions(g);
                g.FillRectangle(brushBG, 0, 0, gPic.Width, gPic.Height);

                float cr = gTags.ForeColor.R / 256f;
                float cg = gTags.ForeColor.G / 256f;
                float cb = gTags.ForeColor.B / 256f;
                var tab = new float[][] {
                    new float[] { 0, 0, 0, .30f, 0 },
                    new float[] { 0, 0, 0, .59f, 0 },
                    new float[] { 0, 0, 0, .11f, 0 },
                    new float[] { 0, 0, 0, 0, 0 },
                    new float[] { cr, cg, cb, 0, 1 }
                };
                var cm = new ColorMatrix(tab);
                using (var iattr = new ImageAttributes())
                {
                    iattr.SetColorMatrix(cm);
                    g.DrawImage(bmFG, new Rectangle(Point.Empty, gPic.Size),
                        0, 0, gPic.Width, gPic.Height, GraphicsUnit.Pixel, iattr);
                }
            }
            bmFG.Dispose();
            brushBG.Dispose();
            gPic.Image = bmBG;
        }

        void setGraphicOptions(Graphics g)
        {
            g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
        }

        private void gTags_Click(object sender, EventArgs e)
        {
            if (cui == null || cui.IsDisposed)
                cui = new UI_TagboxCfg(settings, this);

            cui.ShowAt(this.Bounds);
        }

        private void UI_Tagbox_Resize(object sender, EventArgs e)
        {
            settings.tboxWidth = this.Width;
            settings.tboxHeight = this.Height;
        }
    }
}
