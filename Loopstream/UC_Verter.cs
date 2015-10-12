using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Loopstream
{
    public partial class Verter : UserControl
    {
        public Verter()
        {
            InitializeComponent();
            _enabled = true;
            defaultFG = gLabel.ForeColor;
            defaultBG = gLabel.BackColor;
            w8fuckOn = new Padding(0, 4, 0, 0);
            w8fuckOff = new Padding(1, 5, 1, 0);
            giSlider.Font = new Font(giSlider.Font.FontFamily, giSlider.Font.Size * 2);
        }

        public event EventHandler valueChanged;
        public bool timeScale { get; set; }
        Color defaultFG;
        Color defaultBG;
        Padding w8fuckOn;
        Padding w8fuckOff;
        public enum EventType { set, slide, mute };
        public EventType eventType;
        bool _enabled;
        public bool enabled
        {
            get
            {
                return _enabled;
            }
            set
            {
                _enabled = value;
                gLabel.ForeColor = value ? defaultFG : Color.White;
                gLabel.BackColor = value ? defaultBG : Color.DarkRed;
                gLabel.FlatStyle = value ? FlatStyle.Standard : FlatStyle.Popup;
                
                //gButton.Padding.Left = value ? 0 : 1;
                //gButton.Padding.Right = value ? 0 : 1;
                gButton.Padding = value ? w8fuckOn : w8fuckOff;
                gLabel.Height = value ? 40 : 38;
            }
        }
        public string title
        {
            get
            {
                return gLabel.Text;
            }
            set
            {
                gLabel.Text = value;
            }
        }
        public bool canToggle
        {
            get
            {
                return gLabel.Enabled;
            }
            set
            {
                gLabel.Enabled = value;
            }
        }
        int _level;
        public int level
        {
            get
            {
                return Math.Min(Math.Max(_level - 40, 0), 255);
            }
            set
            {
                bool inval = _level <= 40 || _level >= 255 + 40;
                _level = Math.Min(Math.Max(value, -40), 295) + 40;
                int top = gOSlider.Height - _level;
                giSlider.Bounds = new Rectangle(0, top - 2, gOSlider.Width, _level + 2);
                giSlider.Text = timeScale ?
                    (Math.Round(level / 200.0,2) + " s\n") :
                    (Math.Round(level/2.55,0) + " %\n");
                //gLabel.Text = level.ToString();

                inval = inval || _level <= 40 || _level >= 255 + 40;
                if (inval)
                {
                    if (_level > 100) graden1.Invalidate();
                    if (_level < 100) graden2.Invalidate();
                }
            }
        }

        private void gLabel_Click(object sender, EventArgs e)
        {
            enabled = !enabled;
            eventType = EventType.mute;
            emit();
        }
        void emit()
        {
            if (valueChanged != null)
            {
                valueChanged(this, null);
            }
        }

        int slideDelta = -1;
        long clickTime = -1;
        int clickOffset = -1;
        Point clickPosition = Point.Empty;
        private void giSlider_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                clickTime = DateTime.UtcNow.Ticks / 10000;
                clickOffset = ((Control)sender).Top + 40;
                clickPosition = Cursor.Position;
                slideDelta = level - (gOSlider.Height - e.Y - clickOffset);
                //gLabel.Text = slideDelta.ToString();
            }
        }

        bool fuckoff = false;
        object fucker = new object();
        private void giSlider_MouseMove(object sender, MouseEventArgs e)
        {
            lock (fucker)
            {
                if (fuckoff) return;
                fuckoff = true;
            }
            if (e.Button != System.Windows.Forms.MouseButtons.Left)
            {
                fuckoff = false;
                return;
            }
            long now = DateTime.UtcNow.Ticks / 10000;
            Point mouse = Cursor.Position;
            int dx = Math.Abs(mouse.X - clickPosition.X);
            int dy = Math.Abs(mouse.Y - clickPosition.Y);
            int d = dx + dy;
            if (now - clickTime < 150 && d < 8)
            {
                fuckoff = false;
                return;
            }
            level = slideDelta + gOSlider.Height - e.Y - clickOffset;
            clickTime -= 1000;
            eventType = EventType.set;
            emit();
            Application.DoEvents();
            if (sender == giSlider)
            {
                clickOffset = giSlider.Top + 40;
            }
            fuckoff = false;
        }

        private void gOSlider_MouseUp(object sender, MouseEventArgs e)
        {
            long now = DateTime.UtcNow.Ticks / 10000;
            if (now - clickTime < 150)
            {
                level = gOSlider.Height - e.Y - clickOffset;
                if (enabled) eventType = EventType.slide;
                else eventType = EventType.set;
                emit();
            }
        }
    }
}
