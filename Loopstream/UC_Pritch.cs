using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Loopstream
{
    public partial class Pritch : UserControl
    {
        int pad;
        public Pritch()
        {
            pre = null;
            Text = "asdf";
            InitializeComponent();
            this.Resize += UC_Pritch_Resize;
            pre = null;

            pad = 8;
            pH.Height = pV.Width = 5;
            bH.Visible = bV.Visible = false;

            magic();
        }

        void UC_Pritch_Resize(object sender, EventArgs e)
        {
            magic();
        }

        [Browsable(true)] //why
        [EditorBrowsable(EditorBrowsableState.Always)] //is
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)] //this
        [Bindable(true)] //necessary
        public override string Text { get; set; }
        
        public LSSettings settings;
        public bool AlignTop
        {
            get { return pH.Dock == DockStyle.Top; }
            set
            {
                pH.Dock = bH.Dock = value ? DockStyle.Top : DockStyle.Bottom;
                bH.BackColor = value ? SystemColors.ControlLightLight : SystemColors.ControlDark;
            }
        }
        public bool AlignLeft
        {
            get { return pV.Dock == DockStyle.Left; }
            set
            {
                pV.Dock = bV.Dock = value ? DockStyle.Left : DockStyle.Right;
                bV.BackColor = value ? SystemColors.ControlLightLight : SystemColors.ControlDark;
            }
        }
        LSSettings.LSPreset pre;
        public LSSettings.LSPreset preset
        {
            get { return pre; }
            set {
                pre = value;
                magic();
            }
        }
        void magic()
        {
            Bitmap bm = new Bitmap(bp.Width - (pad - 2), bp.Height - pad);
            //int c = this.BackColor.R < 128 ? 255 : 0;
            //Brush b = new SolidBrush(Color.FromArgb(96, c, c, c));
            //Brush d = new SolidBrush(Color.FromArgb(32, c, c, c));
            Brush[] b = {
                new SolidBrush(Color.FromArgb(96, 0, 80, 112)),
                new SolidBrush(Color.FromArgb(96, 48, 96, 0)),
                new SolidBrush(Color.FromArgb(96, 96, 48, 0)),
                new SolidBrush(Color.FromArgb(96, 96, 0, 48))
            };
            Brush[] d = {
                new SolidBrush(Color.FromArgb(24, 0, 0, 0)),
                new SolidBrush(Color.FromArgb(24, 0, 0, 0)),
                new SolidBrush(Color.FromArgb(48, 96, 48, 0)),
                new SolidBrush(Color.FromArgb(24, 0, 0, 0))
            };

            using (Graphics g = Graphics.FromImage(bm))
            {
                //g.FillRectangle(t, 0, 0, bm.Width, bm.Height);
                if (pre != null)
                {
                    bool[] e = new bool[] { pre.bRec, pre.bMic, false, pre.bOut };
                    double[] h = new double[] { pre.vRec, pre.vMic, pre.vSpd, pre.vOut };
                    for (int a = 0; a < h.Length; a++)
                    {
                        h[a] *= a == 2 ? 200 : 255;
                        h[a] *= bm.Height / 255.0;
                        h[a] = Math.Max(h[a], 1);
                        h[a] = bm.Height - h[a];
                    }
                    int w = bm.Width / h.Length;
                    for (int a = 0; a < h.Length; a++)
                    {
                        g.FillRectangle(e[a] ? b[a] : d[a], w * a, (int)h[a], w - 1, bm.Height - (int)h[a]);
                        //g.DrawLine(new Pen(e[a] ? b[a] : d[a]), w * a, (int)h[a], w * a, (int)h[a] + w - 1);
                        continue;

                        /*int x = w * a;
                        int y = (int)h[a];
                        int bw = w - 1;
                        int bh = bm.Height - y;
                        g.FillRectangle(b, x, y, bw, bh);
                        if (!e[a])
                        {
                            y += 1;
                            bw += x;
                            bh += y;
                            for (; y < bh; y+=2)
                            {
                                g.DrawLine(pt, x, y, bw, y);
                            }
                        }*/
                    }
                }
            }
            if (bp.Image != null) bp.Image.Dispose();
            bp.Image = bm;
        }

        private void pb_Click(object sender, EventArgs e)
        {
            InvokeOnClick(this, e);
        }
    }
}
