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
    public partial class UI_Winlist : Form
    {
        public UI_Winlist()
        {
            InitializeComponent();
            target = null;
        }

        bool fuck, shit;
        public string target;

        private void UI_Winpick_Load(object sender, EventArgs e) { }
        private void gReload_Click(object sender, EventArgs e) { }
        private void gSave_Click(object sender, EventArgs e) { }
        private void gCancel_Click(object sender, EventArgs e) { }
        /*private void UI_Winpick_Load(object sender, EventArgs e)
        {
            label1.Font = new Font(label1.Font.FontFamily, label1.Font.SizeInPoints * 1.5f);
            gList.Font = new Font(FontFamily.GenericMonospace, gList.Font.SizeInPoints * 1.1f);
            Timer t = new Timer();
            t.Interval = 100;
            t.Start();
            t.Tick += delegate(object oa, EventArgs ob)
            {
                t.Stop();
                gReload_Click(oa, ob);
            };
            fuck = true;
            shit = false;
        }

        public class EnumHandles
        {
            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);
            delegate bool EnumWindowsProc(int hwnd, int lParam);

            static bool blocker = false;
            static List<int> list = null;
            public static bool Hadd(int hwnd, int lParam)
            {
                list.Add(hwnd);
                return true;
            }
            public static int[] Run()
            {
                if (blocker) return new int[0];
                blocker = true;
                if (list != null) list.Clear();
                else list = new List<int>();
                EnumWindowsProc enumWindowsProc = new EnumWindowsProc(EnumHandles.Hadd);
                EnumWindows(enumWindowsProc, IntPtr.Zero);
                int[] ret = list.ToArray();
                list.Clear();
                blocker = false;
                return ret;
            }
        }

        private void gReload_Click(object sender, EventArgs e)
        {
            if (shit) return;
            shit = true;
            if (fuck)
            {
                fuck = false;
                int i = gList.Height;
                while (i == gList.Height)
                {
                    this.Height += 1;
                    Application.DoEvents();
                }
            }
            gList.Items.Clear();

            int myId = System.Diagnostics.Process.GetCurrentProcess().Id;
            int[] handles = EnumHandles.Run();
            foreach (int hWnd in handles)
            {
                IntPtr ptr = new IntPtr(hWnd);
                uint proc = WinapiShit.getProcId(ptr);
                if (proc <= 1) continue;
                if (proc == myId) continue;
                string text = WinapiShit.getWinText(ptr);
                if (string.IsNullOrEmpty(text)) continue;

                gList.Items.Add("<" + hWnd + "> // <" + proc + "> // <" + text + ">");
            }
            
            /*System.Diagnostics.Process[] procs = System.Diagnostics.Process.GetProcesses();
            foreach (System.Diagnostics.Process proc in procs)
            {
                if (proc.MainWindowHandle != IntPtr.Zero)
                {
                    string str = proc.MainWindowTitle;
                    if (!string.IsNullOrWhiteSpace(str))
                    {
                        gList.Items.Add(proc.MainWindowHandle.ToString("x").PadLeft(8) + "  " + str);
                    }
                }
            }*/


            /*
            shit = false;
        }

        private void gSave_Click(object sender, EventArgs e)
        {
            try
            {
                target = gList.SelectedItem.ToString();
                target = target.Trim();
                
            }
            catch { }
            this.Hide();
            this.Dispose();
        }

        private void gCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
            this.Dispose();
        }*/
    }
}
