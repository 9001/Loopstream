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
        bool shuttingDown;
        LSSettings settings;
        List<LSEncoder> encoders;
        NPatch.Fork.Outlet outlet;
        NAudio.Wave.SampleProviders.SampleToWaveProvider16 wp16;
        public LSPcmFeed(LSSettings settings, NPatch.Fork.Outlet outlet)
        {
            Logger.pcm.a("pcm init");
            locker = new object();
            shuttingDown = false;
            quitting = 0;
            this.outlet = outlet;
            this.settings = settings;
            encoders = new List<LSEncoder>();
            wp16 = new NAudio.Wave.SampleProviders.SampleToWaveProvider16(outlet);
            System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ThreadStart(dicks));
            t.Name = "LSPcm_Prism";
            t.Start();
        }
        public void Dispose(ref string tex)
        {
            Logger.pcm.a("dispose called");
            shuttingDown = true;
            quitting = 2 + encoders.Count;
            System.Threading.Thread.Sleep(1000);

            /*for (int a = 0; a < 10; a++)
            {
                if (quitting <= 0) break;
                System.Threading.Thread.Sleep(1);
            }*/
            Logger.pcm.a("nuke encoders");
            foreach (LSEncoder enc in encoders)
            {
                Logger.pcm.a("nuke " + enc.enc.ext);
                enc.Dispose();
            }
            Logger.pcm.a("disposed");
            tex = "disconnected";
        }
        void dicks()
        {
            Logger.pcm.a("prism thread");
            long bufSize = settings.samplerate * 10;
            byte[] buffer = new byte[bufSize * 4];
            System.IO.FileStream w = null;
            if (settings.recPCM)
            {
                Logger.pcm.a("open dump target");
                w = new System.IO.FileStream(string.Format("Loopstream-{0}.pcm", DateTime.Now.ToString("yyyy-MM-dd_HH.mm.ss")), System.IO.FileMode.Create);
            }

            // Create encoders first, but do not feed data
            if (settings.mp3.enabled) encoders.Add(new LSLame(settings, this));
            if (settings.ogg.enabled) encoders.Add(new LSVorbis(settings, this));
            // Note that encoders handle creation of and connecting to shouters

            // start the thread watching encoders/shouters and restarting the crashed ones
            System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ThreadStart(medic));
            t.Name = "LSPcm_Medic";
            t.Start();

            // Finally, reposition PCM pointer to minimize latency
            // (and chance of lost packets because icecast a bitch)
            //outlet.setReadPtr(0.2);
            
            // PCM reader loop, passing on to encoders/shouters
            //List<string> toclip = new List<string>();
            try
            {
                while (true)
                {
                    if (qt()) break;
                    int avail = outlet.avail();
                    if (avail > 1024)
                    {
                        //Console.Write('.');
                        Logger.pcm.a("reading pcm data");
                        int i = wp16.Read(buffer, 0, (outlet.avail() / 4) * 4);
                        //toclip.Add((DateTime.UtcNow.Ticks / 10000) + ", " + i);
                        Logger.pcm.a("locking for write");
                        lock (locker)
                        {
                            for (int a = 0; a < encoders.Count; a++)
                            {
                                LSEncoder enc = encoders[a];
                                if (!enc.crashed && enc.stdin != null)
                                {
                                    Logger.pcm.a("writing to " + enc.enc.ext);
                                    enc.eat(buffer, i);
                                }
                            }
                        }
                        if (w != null)
                        {
                            Logger.pcm.a("writing to dump");
                            w.Write(buffer, 0, i);
                        }
                    }
                    Logger.pcm.a("waiting for pcm data");
                    System.Threading.Thread.Sleep(70); // value selected by fair dice roll

                    //if (toclip.Count > 1000) break;
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("pcm reader / enc prism just died\n\nthought you might want to know\n\n===========================\n" + ex.Message + "\n" + ex.StackTrace);
            }
            //StringBuilder sb = new StringBuilder();
            //foreach (string str in toclip) sb.AppendLine(str);
            //System.IO.File.WriteAllText("asdf", sb.ToString());

            Console.WriteLine("shutting down encoder prism");
            if (w != null) w.Close();
        }

        void medic()
        {
            Logger.med.a("active");
            while (true)
            {
                if (qt()) break;
                for (int a = 0; a < encoders.Count; a++)
                {
                    LSEncoder enc = encoders[a];
                    if (enc.crashed)
                    {
                        Logger.pcm.a("resurrecting " + enc.enc.ext);
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
                            Logger.pcm.a("resurrected " + enc.enc.ext);
                        }
                        catch
                        {
                            Logger.pcm.a("resurrect failure: " + enc.enc.ext);
                            Program.ni.ShowBalloonTip(1000, "Connection error", "Failed to restart " + enc.enc.ext, System.Windows.Forms.ToolTipIcon.Error);
                        }
                    }
                }
                System.Threading.Thread.Sleep(10);
            }
            Logger.med.a("disposed");
        }

        public bool qt()
        {
            return shuttingDown;
            lock (locker)
            {
                if (quitting > 0)
                {
                    Logger.pcm.a("qt()--");
                    quitting--;
                    return true;
                }
            }
            return false;
        }
    }
}