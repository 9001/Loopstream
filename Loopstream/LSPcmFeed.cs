using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Loopstream
{
    public class LSPcmFeed
    {
        int quitting;
        object locker;
        LSSettings settings;
        List<LSEncoder> encoders;
        NPatch.Fork.Outlet outlet;
        NAudio.Wave.SampleProviders.SampleToWaveProvider16 wp16;
        public LSPcmFeed(LSSettings settings, NPatch.Fork.Outlet outlet)
        {
            locker = new object();
            quitting = 0;
            this.outlet = outlet;
            this.settings = settings;
            encoders = new List<LSEncoder>();
            wp16 = new NAudio.Wave.SampleProviders.SampleToWaveProvider16(outlet);
            new System.Threading.Thread(new System.Threading.ThreadStart(dicks)).Start();
        }
        public void Dispose()
        {
            lock (locker)
            {
                quitting = 2 + encoders.Count;
            }
            for (int a = 0; a < 100; a++)
            {
                lock (locker)
                {
                    if (quitting <= 0) break;
                }
                System.Threading.Thread.Sleep(1);
            }
            foreach (LSEncoder enc in encoders)
            {
                enc.Dispose();
            }
        }
        void dicks()
        {
            long bufSize = settings.samplerate * 10;
            byte[] buffer = new byte[bufSize * 4];
            System.IO.FileStream w = null;
            if (settings.recPCM)
            {
                w = new System.IO.FileStream(string.Format("Loopstream-{0}.pcm", DateTime.Now.ToString("yyyy-MM-dd_HH.mm.ss")), System.IO.FileMode.Create);
            }

            // Create encoders first, but do not feed data
            if (settings.mp3.enabled) encoders.Add(new LSLame(settings, this));
            if (settings.ogg.enabled) encoders.Add(new LSVorbis(settings, this));
            // Note that encoders handle creation of and connecting to shouters

            // start the thread watching encoders/shouters and restarting the crashed ones
            new System.Threading.Thread(new System.Threading.ThreadStart(medic)).Start();

            // Finally, reposition PCM pointer to minimize latency
            // (and chance of lost packets because icecast a bitch)
            outlet.setReadPtr(0.2);
            
            // PCM reader loop, passing on to encoders/shouters
            while (true)
            {
                if (qt()) break;
                int avail = outlet.avail();
                if (avail > 1024)
                {
                    //Console.Write('.');
                    int i = wp16.Read(buffer, 0, (outlet.avail() / 4) * 4);
                    lock (locker)
                    {
                        for (int a = 0; a < encoders.Count; a++)
                        {
                            LSEncoder enc = encoders[a];
                            if (!enc.crashed && enc.stdin != null)
                            {
                                enc.stdin.Write(buffer, 0, i);
                                enc.stdin.Flush();
                            }
                        }
                    }
                    if (w != null)
                    {
                        w.Write(buffer, 0, i);
                    }
                }
                System.Threading.Thread.Sleep(10);
            }
            Console.WriteLine("shutting down encoder prism");
            if (w != null) w.Close();
        }

        void medic()
        {
            while (true)
            {
                if (qt()) break;
                for (int a = 0; a < encoders.Count; a++)
                {
                    LSEncoder enc = encoders[a];
                    if (enc.crashed)
                    {
                        enc.Dispose();
                        try
                        {
                            if (enc.enc.ext == "mp3")
                            {
                                enc = new LSLame(settings, this);
                            }
                            else if (enc.enc.ext == "ogg")
                            {
                                enc = new LSVorbis(settings, this);
                            }
                            else
                            {
                                System.Windows.Forms.MessageBox.Show("this shouldn't happen");
                                Program.kill();
                            }
                            lock (locker)
                            {
                                encoders[a] = enc;
                            }
                        }
                        catch
                        {
                            Program.ni.ShowBalloonTip(1000, "Connection error", "Failed to restart " + enc.enc.ext, System.Windows.Forms.ToolTipIcon.Error);
                        }
                    }
                }
                System.Threading.Thread.Sleep(10);
            }
        }

        public bool qt()
        {
            lock (locker)
            {
                if (quitting > 0)
                {
                    quitting--;
                    return true;
                }
            }
            return false;
        }
    }
}