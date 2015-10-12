using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Loopstream
{
    public partial class UI_Winpeck : Form
    {
        public UI_Winpeck()
        {
            InitializeComponent();
            target = IntPtr.Zero;
            me = IntPtr.Zero;
            starget = null;
            itarget = 0;
            tocker = 0;
        }

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        private int tocker;
        private IntPtr me;
        private IntPtr target;
        public string starget;
        public int itarget;

        private void UI_Winpeck_Load(object sender, EventArgs e)
        {
            int w = this.Width;
            int h = this.Height;
            this.Font = new Font(this.Font.FontFamily, this.Font.SizeInPoints * 1.3f);
            this.Show();
            Application.DoEvents();
            this.Left -= (this.Width - w) / 2;
            this.Top -= (this.Height - h) / 2;
            Application.DoEvents();
            label1.Text = "";
            label1_Click(sender, e);

            this.Icon = Program.icon;

            /*Timer t = new Timer();
            t.Interval = 10;
            t.Start();
            t.Tick += delegate(object oa, EventArgs ob)
            {
                t.Stop();
                label1_Click(sender, e);
            };*/
        }

        private void label1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(starget))
            {
                this.Hide();
                this.Dispose();
                return;
            }
            label1.Text = "Please click on target window :)";
            me = GetForegroundWindow();

            Timer t = new Timer();
            t.Interval = 10;
            t.Start();
            t.Tick += delegate(object oa, EventArgs ob)
            {
                IntPtr p = GetForegroundWindow();
                if (p != me && p != this.Handle)
                {
                    if (target == IntPtr.Zero)
                    {
                        target = p;
                    }
                    else
                    {
                        if (target != p)
                        {
                            tocker = 0;
                        }
                        if (++tocker > 5)
                        {
                            t.Stop();
                            doit();
                        }
                        //MessageBox.Show("You have selected procid(" + WinapiShit.getProcId(target).ToString("x") + ") with current title:\n\n<" + WinapiShit.getWinText(target) + ">");
                    }
                }
            };
        }

        void doit()
        {
            starget = "WINPECK_ERROR";
            try
            {
                if ((int)target <= 1)
                {
                    label1.Text = "Sorry, illegal window id :(";
                    throw new Exception();
                }
                uint iproc = WinapiShit.getProcId(target);
                if (iproc <= 1)
                {
                    label1.Text = "Sorry, process resolver failed :(";
                    throw new Exception();
                }
                System.Diagnostics.Process oproc = System.Diagnostics.Process.GetProcessById((int)iproc);
                if (oproc == null)
                {
                    label1.Text = "Sorry, module lookup failed :(";
                    throw new Exception();
                }
                string title = WinapiShit.getWinText(target);
                if (string.IsNullOrWhiteSpace(title))
                {
                    label1.Text = "Sorry, caption reader failed :(";
                    throw new Exception();
                }
                itarget = (int)target;
                starget = oproc.ProcessName;
                label1.Text =
                    "OK !\n\n" +
                    "Proc.ID " + iproc.ToString("X") + "\n" +
                    "Window.ID " + target.ToString("X") + "\n" +
                    "«" + starget + "»\n\n" +
                    "«" + title + "»";
            }
            catch { }
            //this.TopMost = true;
            //this.Focus();
            WinapiShit.topmost(this.Handle);
        }

        void fuckoff()
        {
            this.Hide();
            this.Dispose();
        }
    }
}
