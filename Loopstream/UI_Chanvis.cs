using NAudio.CoreAudioApi;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using NPatch;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Loopstream
{
    public partial class UI_Chanvis : Form
    {
        public UI_Chanvis(LSDevice src, int[] chans)
        {
            this.src = src;
            this.chans = chans;
            InitializeComponent();
        }

        LSDevice src;
        public int[] chans;
        NPatch.VolumeSlider[] Vu;
        Verter[] gVu;
        int outDev;
        Testfeed feed;
        bool discard_events;

        Timer unFxTimer = null;
        NAudio.Wave.WaveFileReader fx_wav = null;
        NAudio.Wave.WasapiOut fx_out = null;
        System.IO.Stream fx_stream = null;

        private void UI_Chanvis_Load(object sender, EventArgs e)
        {
            discard_events = false;
            this.Icon = Program.icon;
            var center = new Point(
                this.Location.X + this.Width / 2,
                this.Location.Y + this.Height / 2);

            var x = 17;
            Vu = new NPatch.VolumeSlider[src.wf.Channels];
            gVu = new Verter[Vu.Length];
            for (var a = 0; a < gVu.Length; a++)
            {
                var c = new Verter();
                c.Tag = a;
                c.A_GRAD_1 = System.Drawing.Color.FromArgb(220, 0, 255);
                c.A_GRAD_2 = System.Drawing.Color.Blue;
                c.title = "Channel " + (a + 1);
                c.level = 255;
                c.enabled = chans.Contains(a);
                c.valueChanged += c_valueChanged;
                if (!src.isPlay)
                    c.disableOutput();

                Vu[a] = new NPatch.VolumeSlider();
                c.giSlider.src = Vu[a];
                Vu[a].enVU = true;

                this.Controls.Add(c);
                c.Location = new Point(x, -13);
                x += c.Width + 16;
                gVu[a] = c;
            }
            
            x += 17;
            if (x > this.Width)
                this.Width = x;

            panel1.BringToFront();
            this.Location = new Point(
                center.X - this.Width / 2,
                center.Y - this.Height / 2);

            feed = null;
            outDev = -1;
            chdev(0);
        }

        private void UI_Chanvis_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (feed != null)
                feed.Dispose();
        }

        private void gNDev_Click(object sender, EventArgs e)
        {
            gPreviewOn.Checked = !gPreviewOn.Checked;
            chdev(0);
        }

        private void gPreviewOn_CheckedChanged(object sender, EventArgs e)
        {
            chdev(0);
        }

        private void gDevSub_Click(object sender, EventArgs e)
        {
            chdev(-1);
        }

        private void gDevAdd_Click(object sender, EventArgs e)
        {
            chdev(1);
        }

        void c_valueChanged(object sender, EventArgs e)
        {
            if (discard_events)
                return;

            var verter = (Verter)sender;
            var ev = verter.eventType;

            if (ev == Verter.EventType.mute)
            {
                chdev(0);
                return;
            }

            if (ev == Verter.EventType.solo)
            {
                discard_events = true;
                for (var a = 0; a < gVu.Length; a++)
                    gVu[a].enabled = false;

                ((Verter)sender).enabled = true;
                discard_events = false;
                chdev(0);
                return;
            }

            if (ev == Verter.EventType.airhorn)
            {
                gPreviewOn.Checked = false;
                chdev(0);

                if (unFxTimer == null)
                {
                    unFxTimer = new Timer();
                    unFxTimer.Interval = 3000;
                    unFxTimer.Tick += delegate(object oa, EventArgs ob)
                    {
                        unFX();
                    };
                }
                unFX();
                fx_stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("Loopstream.res.sc.wav");
                fx_wav = new NAudio.Wave.WaveFileReader(fx_stream);
                var prov2 = new NPatch.ChannelMapperOut(fx_wav.ToSampleProvider(), new int[] { (int)verter.Tag }, src.wf.Channels);
                fx_out = new NAudio.Wave.WasapiOut(src.mm, NAudio.CoreAudioApi.AudioClientShareMode.Shared, false, 100);
                fx_out.Init(prov2);
                fx_out.Play();
                unFxTimer.Start();
            }
        }

        void unFX()
        {
            try
            {
                if (fx_out != null) fx_out.Stop();
                if (fx_out != null) fx_out.Dispose();
                if (fx_wav != null) fx_wav.Dispose();
                if (fx_stream != null) fx_stream.Dispose();
                unFxTimer.Stop();
            }
            catch { }
        }

        void chdev(int steps)
        {
            gPreviewOn.Enabled = false;
            gDevSub.Enabled = false;
            gDevAdd.Enabled = false;
            Application.DoEvents();

            outDev += steps;
            if (outDev < -1)
                outDev = WaveOut.DeviceCount - 1;
            if (outDev >= WaveOut.DeviceCount)
                outDev = -1;

            gNDev.Text = outDev == -1 ? "windows-default" : "output " + (outDev + 1);

            if (feed != null)
                feed.Dispose();

            var chans = new List<int>();
            for (int a = 0; a < gVu.Length; a++)
                if (gVu[a].enabled)
                    chans.Add(a);

            this.chans = chans.ToArray();
            feed = new Testfeed(src, this.chans, Vu, gPreviewOn.Checked ? outDev : -2);
            gPreviewOn.Enabled = true;
            gDevSub.Enabled = true;
            gDevAdd.Enabled = true;
            if (!feed.ok)
                this.Close();
        }

        private void UI_Chanvis_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (chans.Length < 1 || chans.Length > 2)
                if (System.Windows.Forms.DialogResult.Retry == MessageBox.Show("please select 1 or 2 channels", "invalid selection", MessageBoxButtons.RetryCancel))
                    e.Cancel = true;
        }
    }

    class Testfeed : IDisposable
    {
        public Testfeed(LSDevice src, int[] chans, VolumeSlider[] vus, int outDev)
        {
            this.src = src;
            this.chans = chans;
            this.vus = vus;
            this.ok = false;
            
            dummies = new List<DummySink>();

            try
            {
                cap = null;
                if (src.isPlay)
                    cap = new WasapiLoopbackCapture(src.mm);
                else
                    cap = new WasapiCapture(src.mm);
            }
            catch (System.Runtime.InteropServices.COMException ce)
            {
                string errmsg = WinapiShit.comEx((uint)ce.ErrorCode);
                if (errmsg == "")
                    errmsg = "(i don't know what this means but please report it)";

                MessageBox.Show("could not access audio device; error code " + ce.ErrorCode.ToString("x") + "\r\n\r\n" + errmsg);
                return;
            }

            cap.DataAvailable += input_DataAvailable;
            wi = new BufferedWaveProvider(cap.WaveFormat);
            var cap_samples = wi.ToSampleProvider();
            var chan_splitter = new ChannelSplitter(cap_samples, chans);
            for (var a = 0; a < vus.Length; a++)
            {
                vus[a].SetSource(chan_splitter.output[a]);
                dummies.Add(new DummySink(vus[a]));
            }

            if (outDev >= -1)
            {
                wo = new WaveOut();
                wo.DeviceNumber = outDev;
                wo.Init(chan_splitter);
                wo.Play();
            }
            else
            {
                dummies.Add(new DummySink(chan_splitter));
                wo = null;
            }

            try
            {
                cap.StartRecording();
            }
            catch (System.Runtime.InteropServices.COMException ce)
            {
                MessageBox.Show(WinapiShit.comExMsg((uint)ce.ErrorCode));
                return;
            }
            this.ok = true;
        }

        ~Testfeed()
        {
            Dispose();
        }

        public void Dispose()
        {
            foreach (var ds in dummies)
                ds.Dispose();

            if (cap != null)
                cap.Dispose();

            if (wo != null)
                wo.Dispose();
        }

        void input_DataAvailable(object sender, WaveInEventArgs e)
        {
            wi.AddSamples(e.Buffer, 0, e.BytesRecorded);
        }

        LSDevice src;
        int[] chans;
        VolumeSlider[] vus;
        WasapiCapture cap;
        BufferedWaveProvider wi;
        WaveOut wo;
        List<DummySink> dummies;
        public bool ok;
    }

    class ChannelSplitter : ISampleProvider
    {
        public ChannelSplitter(ISampleProvider src, int[] chans)
        {
            this.src = src;
            this.chans = chans;
            this.WaveFormat = WaveFormat.CreateIeeeFloatWaveFormat(src.WaveFormat.SampleRate, 1);
            this.output = new SampleCache[src.WaveFormat.Channels];
            for (int a = 0; a < this.output.Length; a++)
                this.output[a] = new SampleCache(this.WaveFormat);
        }

        public WaveFormat WaveFormat { get; private set; }

        ISampleProvider src;
        int[] chans;
        public SampleCache[] output;

        public int Read(float[] buffer, int offset, int count)
        {
            int src_chans = src.WaveFormat.Channels;
            int src_count = count * src_chans;
            float[] retbuf = new float[count];
            float[] srcbuf = new float[src_count];
            float[][] splits = new float[src_chans][];
            int sourceSamplesRead = src.Read(srcbuf, 0, src_count);
            for (int a = 0; a < src_chans; a++)
                splits[a] = new float[sourceSamplesRead / src_chans];

            for (int a = 0; a < srcbuf.Length; a++)
                splits[a % src_chans][a / src_chans] = srcbuf[a];

            for (int a = offset; a < offset + count; a++)
                buffer[a] = 0;

            for (int ich = 0; ich < chans.Length; ich++)
            { 
                int ch = chans[ich];
                for (int ptr = 0; ptr < sourceSamplesRead; ptr += src_chans)
                    buffer[offset + ptr / src_chans] += srcbuf[ptr + ch] / chans.Length;
            }
            for (int a = 0; a < output.Length; a++)
                output[a].put(splits[a]);

            return sourceSamplesRead / src.WaveFormat.Channels;
        }
    }

    class SampleCache : ISampleProvider
    {
        public SampleCache(WaveFormat wf)
        {
            this.WaveFormat = wf;
            cache = new List<float[]>();
        }

        public WaveFormat WaveFormat { get; private set; }
        List<float[]> cache;

        public void put(float[] floats)
        {
            cache.Add(floats);
        }

        public int Read(float[] buffer, int offset, int count)
        {
            while (cache.Count > 0 && cache[0].Length == 0)
                cache.RemoveAt(0);

            if (cache.Count == 0)
                return 0;

            float[] leftovers;
            float[] first = cache[0];
            if (count < first.Length)
            {
                leftovers = new float[first.Length - count];
                Array.Copy(first, 0, buffer, offset, count);
                Array.Copy(first, count, leftovers, 0, leftovers.Length);
                cache[0] = leftovers;
                return count;
            }
            Array.Copy(first, 0, buffer, offset, first.Length);
            cache.RemoveAt(0);
            return first.Length;
        }
    }

    class DummySink : IDisposable
    {
        public DummySink(ISampleProvider src)
        {
            t = new Timer();
            t.Interval = 25;
            t.Tick += t_Tick;

            this.src = src;
            buf = new float[src.WaveFormat.AverageBytesPerSecond / (1000 / t.Interval)];
            t.Start();
        }

        public void Dispose()
        {
            lock (t)
                t.Dispose();
        }

        ~DummySink()
        {
            Dispose();
        }

        ISampleProvider src;
        float[] buf;
        Timer t;

        void t_Tick(object sender, EventArgs e)
        {
            int bytes_read = 0;
            lock (sender)
            {
                for (int a = 0; a < 64; a++)
                {
                    int nread = src.Read(buf, 0, buf.Length);
                    if (nread <= 0)
                        break;

                    bytes_read += nread;
                    if (bytes_read >= buf.Length)
                        break;
                }
            }
        }
    }
}
