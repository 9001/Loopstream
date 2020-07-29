using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;
using NAudio.CoreAudioApi;
using NAudio.Wave.SampleProviders;

namespace Loopstream
{
    class LSMixer
    {
        LSSettings settings;
        WaveFormat format;
        IWaveIn recCap;
        WasapiCapture micCap;
        BufferedWaveProvider recIn, micIn;
        MediaFoundationResampler recRe, micRe;
        NPatch.VolumeSlider recVol, micVol, outVol;
        NPatch.Mixa mixa;
        WasapiOut mixOut;
        List<Object> cage;
        public NPatch.Fork.Outlet lameOutlet;
        public string isLQ;
        System.Windows.Forms.Timer killmic, startReading;

        public void Dispose(ref string tex)
        {
            lock (mixOut)
            {
                if (startReading != null)
                {
                    startReading.Dispose();
                    startReading = null;
                }
            }
            Logger.mix.a("dispose recCap"); tex = "recCap"; if (recCap != null) recCap.StopRecording();
            Logger.mix.a("dispose micCap"); tex = "micCap"; if (micCap != null) micCap.StopRecording();
            Logger.mix.a("dispose mixOut"); tex = "mixOut"; if (mixOut != null) mixOut.Dispose();
            Logger.mix.a("dispose recRe"); tex = "recRe"; if (recRe != null) recRe.Dispose();
            Logger.mix.a("dispose micRe"); tex = "micRe"; if (micRe != null) micRe.Dispose();
            Logger.mix.a("disposed");
        }

        public enum Slider
        {
            Music,
            Mic,
            Out
        };

        LLabel[] bars;

        public LSMixer(LSSettings settings, LLabel[] bars)
        {
            Logger.mix.a("creating");
            this.settings = settings;
            this.bars = bars;
            isLQ = null;
            doMagic();
        }

        void doMagic()
        {
            Logger.mix.a("doMagic");
            cage = new List<Object>();
            string lq = "";
            recCap = null;
            micCap = null;
            recRe = micRe = null;
            ISampleProvider recProv;
            format = WaveFormat.CreateIeeeFloatWaveFormat(settings.samplerate, 2);
            mixa = new NPatch.Mixa(format);

            Logger.mix.a("create rec");
            if (settings.devRec is LSDevice)
                recCap = new WasapiLoopbackCapture(((LSDevice)settings.devRec).mm);
            else
                recCap = new LSWavetailDev((LSWavetail)settings.devRec);

            recCap.DataAvailable += recDev_DataAvailable_03;
            recIn = new BufferedWaveProvider(recCap.WaveFormat);
            
            //recIn.ReadFully = false;
            if (recCap.WaveFormat.SampleRate != settings.samplerate)
            {
                Logger.mix.a("create rec resampler");
                recRe = new MediaFoundationResampler(recIn, settings.samplerate);
                recRe.ResamplerQuality = 60;
                lq += "Incorrect samplerate on music device, resampling\n";

                if (settings.devRec is LSDevice)
                    lq +=
                        ((LSDevice)settings.devRec).mm.DeviceFriendlyName + "\n" +
                        ((LSDevice)settings.devRec).mm.FriendlyName + "\n";

                lq += settings.devRec.id + "\n" +
                    LSDevice.stringer(settings.devRec.wf) + "\n" +
                    LSDevice.stringer(recCap.WaveFormat) + "\n\n";
            }

            recProv = new WaveToSampleProvider((IWaveProvider)recRe ?? (IWaveProvider)recIn);
            if (recCap.WaveFormat.Channels != settings.chRec.Length)
            {
                cage.Add(recProv);
                Logger.mix.a("rec chanselector");
                recProv = new NPatch.ChannelSelectorIn(recProv, settings.chRec, 2);
            }
            cage.Add(recProv);
            recVol = new NPatch.VolumeSlider();
            recVol.SetSource(recProv);
            mixa.AddMixerInput(recVol);
            Logger.mix.a("rec done");

            killmic = new System.Windows.Forms.Timer();
            killmic.Interval = 1000;
            killmic.Tick += killmic_Tick;
            micVol = new NPatch.VolumeSlider();
            lq += micAdd();

            NPatch.Fork fork = new NPatch.Fork(mixa, 2);
            cage.Add(fork);
            lameOutlet = fork.providers[1];
            outVol = new NPatch.VolumeSlider();
            outVol.SetSource(fork.providers[0]);

            ISampleProvider outProv = outVol;
            if (settings.devOut.wf.Channels != settings.chOut.Length)
            {
                Logger.mix.a("create ChannelMapperOut " + settings.devOut.wf.Channels);
                outProv = new NPatch.ChannelMapperOut(outVol, settings.chOut, settings.devOut.wf.Channels);
                cage.Add(outProv);
            }
            SampleToWaveProvider muxer = new SampleToWaveProvider(outProv);
            cage.Add(muxer);

            Logger.mix.a("init mixer vol");
            recVol.SetVolume((float)settings.mixer.vRec);
            micVol.SetVolume((float)settings.mixer.vMic);
            outVol.SetVolume((float)settings.mixer.vOut);
            recVol.boostLock = (float)settings.mixer.yRec;
            micVol.boostLock = (float)settings.mixer.yMic;
            recVol.boost = (float)settings.mixer.xRec;
            micVol.boost = (float)settings.mixer.xMic;
            recVol.muted = !settings.mixer.bRec;
            micVol.muted = !settings.mixer.bMic;
            outVol.muted = !settings.mixer.bOut;

            Logger.mix.a("create mixOut");
            mixOut = new WasapiOut(((LSDevice)settings.devOut).mm,
                AudioClientShareMode.Shared, false, 100);

            Logger.mix.a("init mixOut");
            mixOut.Init(muxer);

            try
            {
                Logger.mix.a("rec.startRec");
                recCap.StartRecording();

                if (micCap != null)
                {
                    Logger.mix.a("mic.startRec");
                    micCap.StartRecording();
                }
                //throw new System.Runtime.InteropServices.COMException("fgsfds", 1234);
            }
            catch (System.Runtime.InteropServices.COMException ce)
            {
                string msg = WinapiShit.comExMsg((uint)ce.ErrorCode);
                System.Windows.Forms.MessageBox.Show(msg + "\r\n\r\ngonna crash now, bye");
                throw;
            }

            // give wasapicapture some time to fill the buffer
            startReading = new System.Windows.Forms.Timer();
            //startReading_Tick(null, null);
            startReading.Tick += startReading_Tick;
            startReading.Interval = 300;
            startReading.Start();

            if (settings.vu)
            {
                recVol.enVU = true;
                micVol.enVU = true;
                outVol.enVU = true;
                bars[0].src = recVol;
                bars[1].src = micVol;
                bars[2].src = outVol;
            }

            if (!string.IsNullOrEmpty(lq)) isLQ = lq;
        }

        void startReading_Tick(object sender, EventArgs e)
        {
            lock (mixOut)
            {
                if (startReading == null)
                    return;

                startReading.Dispose();
                startReading = null;
            }

            Logger.mix.a("mixOut.play (ready)");
            mixOut.Play();
        }

        string micAdd()
        {
            string ret = "";
            ISampleProvider micProv;
            if (micVol != null && micVol.OK())
                return "";

            var devMic = settings.devMic as LSDevice;
            if (devMic != null && devMic.mm != null)
            {
                Logger.mix.a("create mic");
                micCap = new WasapiCapture(devMic.mm);
                micCap.DataAvailable += micDev_DataAvailable_03;
                micIn = new BufferedWaveProvider(micCap.WaveFormat);
                //micIn.ReadFully = false;
                if (micCap.WaveFormat.SampleRate != settings.samplerate)
                {
                    Logger.mix.a("create mic resampler");
                    micRe = new MediaFoundationResampler(micIn, settings.samplerate);
                    micRe.ResamplerQuality = 60;
                    ret += "Incorrect samplerate on microphone device, resampling\n" +
                        devMic.mm.DeviceFriendlyName + "\n" +
                        devMic.mm.FriendlyName + "\n" +
                        devMic.id + "\n" +
                        LSDevice.stringer(devMic.wf) + "\n" +
                        LSDevice.stringer(micCap.WaveFormat) + "\n\n";
                }
                micProv = new WaveToSampleProvider((IWaveProvider)micRe ?? (IWaveProvider)micIn);
                cage.Add(micProv);
                if (micCap.WaveFormat.Channels == 1)
                {
                    Logger.mix.a("mic mono2stereo");
                    micProv = new MonoToStereoSampleProvider(micProv);
                    cage.Add(micProv);
                }
                else if (micCap.WaveFormat.Channels != settings.chMic.Length)
                {
                    Logger.mix.a("mic chanselector");
                    micProv = new NPatch.ChannelSelectorIn(micProv, settings.chMic, 2);
                    cage.Add(micProv);
                }
                if (settings.reverbP > 0)
                {
                    micProv = new NPatch.Reverb(micProv);
                    cage.Add(micProv);
                }

                micVol.SetSource(micProv);
                mixa.AddMixerInput(micVol);
                Logger.mix.a("mic done");
            }
            else
            {
                Logger.mix.a("mic skipped");
            }
            return ret;
        }

        void killmic_Tick(object sender, EventArgs e)
        {
            killmic.Stop();
            if (!settings.killmic)
                return;

            mixa.RemoveMixerInput(micVol);
            micVol.SetSource(null);
            if (micCap != null)
            {
                micCap.StopRecording();
                micCap.Dispose();
            }
            if (micRe != null)
                micRe.Dispose();

            Logger.mix.a("mic stopped");
        }

        public void FadeVolume(Slider slider, float vol, double seconds)
        {
            Logger.mix.a("fadeVol " + slider + " to " + vol + " over " + seconds);
            bool micOn = slider == Slider.Mic && micVol.GetVolume() < 0.1 && vol > 0.1;
            bool micOff = slider == Slider.Mic && micVol.GetVolume() > 0.1 && vol < 0.1;
            if (micOn || micOff)
            {
                if (micOn)
                {
                    killmic.Stop();
                    if (!micVol.OK())
                    {
                        micAdd();
                        if (micVol.OK())
                        {
                            Logger.mix.a("mic.startRec");
                            micCap.StartRecording();
                        }
                    }
                }
                else if (settings.killmic)
                {
                    killmic.Stop();
                    killmic.Interval = (int)(seconds * 1000) + 250;
                    killmic.Start();
                }
                try
                {
                    LSSettings.LSParams[] encs = { settings.mp3, settings.ogg, settings.opus };
                    foreach (LSSettings.LSParams enc in encs)
                    {
                        if (enc.enabled && !string.IsNullOrWhiteSpace(enc.i.filename))
                        {
                            System.IO.File.AppendAllText(
                                enc.i.filename + ".txt",
                                enc.i.timestamp() + " " + (micOn ? "@" : "-") + "\r\n",
                                Encoding.UTF8);
                        }
                    }
                }
                catch { }
            }
            if (slider == Slider.Music) recVol.SetVolume(vol, seconds);
            if (slider == Slider.Mic) micVol.SetVolume(vol, seconds);
            if (slider == Slider.Out) outVol.SetVolume(vol, seconds);
            //Console.WriteLine("VOLFADE " + vol);
        }

        public void MuteChannel(Slider slider, bool notMuted)
        {
            Logger.mix.a("mute " + slider + " " + !notMuted);
            //if (slider == Slider.Music) recVol.SetVolume(notMuted ? (float)settings.mixer.vRec : 0);
            //if (slider == Slider.Mic) micVol.SetVolume(notMuted ? (float)settings.mixer.vMic : 0);
            //if (slider == Slider.Out) outVol.SetVolume(notMuted ? (float)settings.mixer.vOut : 0);
            if (slider == Slider.Music) recVol.muted = !notMuted;
            if (slider == Slider.Mic) micVol.muted = !notMuted;
            if (slider == Slider.Out) outVol.muted = !notMuted;
        }

        public void BoostChannel(Slider slider, float boost)
        {
            Logger.mix.a("boost " + slider + " to " + boost);
            if (slider == Slider.Music) recVol.boost = boost;
            if (slider == Slider.Mic) micVol.boost = boost;
        }

        public void BoostLockChannel(Slider slider, float boostLock)
        {
            Logger.mix.a("boostLock " + slider + " to " + boostLock);
            if (slider == Slider.Music) recVol.boostLock = boostLock;
            if (slider == Slider.Mic) micVol.boostLock = boostLock;
        }

        void recDev_DataAvailable_03(object sender, WaveInEventArgs e)
        {
            recIn.AddSamples(e.Buffer, 0, e.BytesRecorded);

            if (recVol.attenuated)
            {
                recVol.attenuated = false;
                settings.mixer.xRec = recVol.boost;
            }
            if (micVol.attenuated)
            {
                micVol.attenuated = false;
                settings.mixer.xMic = micVol.boost;
            }
        }

        void micDev_DataAvailable_03(object sender, WaveInEventArgs e)
        {
            micIn.AddSamples(e.Buffer, 0, e.BytesRecorded);
        }
    }
}
