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
        WasapiLoopbackCapture recCap;
        WasapiCapture micCap;
        BufferedWaveProvider recIn, micIn;
        MediaFoundationResampler recRe, micRe;
        NPatch.VolumeSlider recVol, micVol, outVol;
        MixingSampleProvider mixer;
        NPatch.Mixa mixa;
        SampleToWaveProvider muxer;
        WasapiOut mixOut;
        NPatch.Fork fork;
        WaveFileWriter waver;
        public NPatch.Fork.Outlet lameOutlet;
        public string isLQ;

        public void Dispose(ref string tex)
        {
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

        public LSMixer(LSSettings settings)
        {
            Logger.mix.a("creating");
            this.settings = settings;
            isLQ = null;
            doMagic();
        }

        void doMagic()
        {
            Logger.mix.a("doMagic");
            string lq = "";
            recRe = micRe = null;
            ISampleProvider recProv, micProv;
            format = WaveFormat.CreateIeeeFloatWaveFormat(settings.samplerate, 2);
            //mixer = new MixingSampleProvider(format);
            mixa = new NPatch.Mixa(format);

            Logger.mix.a("create rec");
            recCap = new WasapiLoopbackCapture(settings.devRec.mm);
            recCap.DataAvailable += recDev_DataAvailable_03;
            recIn = new BufferedWaveProvider(recCap.WaveFormat);
            if (recCap.WaveFormat.SampleRate != settings.samplerate)
            {
                Logger.mix.a("create rec resampler");
                recRe = new MediaFoundationResampler(recIn, settings.samplerate);
                recRe.ResamplerQuality = 60;
                lq += "Incorrect samplerate on music device, resampling\n" +
                    settings.devRec.mm.DeviceFriendlyName + "\n" +
                    settings.devRec.mm.FriendlyName + "\n" +
                    settings.devRec.id + "\n" +
                    LSDevice.stringer(settings.devRec.wf) + "\n" +
                    LSDevice.stringer(recCap.WaveFormat) + "\n\n";
            }
            recProv = new WaveToSampleProvider((IWaveProvider)recRe ?? (IWaveProvider)recIn);
            recVol = new NPatch.VolumeSlider(recProv);
            mixa.AddMixerInput(recVol);
            Logger.mix.a("rec done");
            
            if (settings.devMic != null && settings.devMic.mm != null)
            {
                Logger.mix.a("create mic");
                micCap = new WasapiCapture(settings.devMic.mm);
                micCap.DataAvailable += micDev_DataAvailable_03;
                micIn = new BufferedWaveProvider(micCap.WaveFormat);
                if (micCap.WaveFormat.SampleRate != settings.samplerate)
                {
                    Logger.mix.a("create mic resampler");
                    micRe = new MediaFoundationResampler(micIn, settings.samplerate);
                    micRe.ResamplerQuality = 60;
                    lq += "Incorrect samplerate on microphone device, resampling\n" +
                        settings.devMic.mm.DeviceFriendlyName + "\n" +
                        settings.devMic.mm.FriendlyName + "\n" +
                        settings.devMic.id + "\n" +
                        LSDevice.stringer(settings.devMic.wf) + "\n" +
                        LSDevice.stringer(micCap.WaveFormat) + "\n\n";
                }
                micProv = new WaveToSampleProvider((IWaveProvider)micRe ?? (IWaveProvider)micIn);
                if (micCap.WaveFormat.Channels == 1)
                {
                    Logger.mix.a("mic mono2stereo");
                    micProv = new MonoToStereoSampleProvider(micProv);
                }
                else if (settings.micLeft != settings.micRight)
                {
                    Logger.mix.a("mic chanselector");
                    micProv = new NPatch.ChannelSelector(micProv, settings.micLeft ? 0 : 1);
                }
                micVol = new NPatch.VolumeSlider(micProv);
                mixa.AddMixerInput(micVol);
                Logger.mix.a("mic done");
            }
            else
            {
                Logger.mix.a("mic skipped");
                micVol = new NPatch.VolumeSlider();
            }

            //mixer.ReadFully = true;
            fork = new NPatch.Fork(mixa, 2);
            lameOutlet = fork.providers[1];
            outVol = new NPatch.VolumeSlider(fork.providers[0]);
            muxer = new SampleToWaveProvider(outVol);

            Logger.mix.a("init mixer vol");
            recVol.SetVolume((float)settings.mixer.vRec);
            micVol.SetVolume((float)settings.mixer.vMic);
            outVol.SetVolume((float)settings.mixer.vOut);
            recVol.muted = !settings.mixer.bRec;
            micVol.muted = !settings.mixer.bMic;
            outVol.muted = !settings.mixer.bOut;

            Logger.mix.a("create mixOut");
            mixOut = new WasapiOut(settings.devOut.mm,
                AudioClientShareMode.Shared, false, 100);

            Logger.mix.a("init mixOut"); mixOut.Init(outVol);
            Logger.mix.a("rec.startRec"); recCap.StartRecording();
            //System.Threading.Thread.Sleep(100);
            if (settings.devMic != null && settings.devMic.mm != null)
            {
                Logger.mix.a("mic.startRec"); micCap.StartRecording();
            }
            Logger.mix.a("mixOut.play (ready)"); mixOut.Play();

            if (!string.IsNullOrEmpty(lq)) isLQ = lq;

            /*byte[] buffer = new byte[outVol.WaveFormat.AverageBytesPerSecond * 10];
            while (true)
            {
                int i = wp16.Read(buffer, 0, fork.providers[1].avail());
                waver.Write(buffer, 0, i);
                System.Threading.Thread.Sleep(10);
                System.Windows.Forms.Application.DoEvents();
            }*/
        }

        public void FadeVolume(Slider slider, float vol, double seconds)
        {
            Logger.mix.a("fadeVol " + slider + " to " + vol + " over " + seconds);
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

        void recDev_DataAvailable_03(object sender, WaveInEventArgs e)
        {
            recIn.AddSamples(e.Buffer, 0, e.BytesRecorded);
        }

        void micDev_DataAvailable_03(object sender, WaveInEventArgs e)
        {
            micIn.AddSamples(e.Buffer, 0, e.BytesRecorded);
        }

        void doMagic02_WORKS()
        {
            WasapiLoopbackCapture wlc = new WasapiLoopbackCapture(settings.devRec.mm);
            WaveInProvider waveIn = new WaveInProvider(wlc);
            WasapiOut waveOut = new WasapiOut(settings.devOut.mm, AudioClientShareMode.Shared, false, 100);
            waveOut.Init(waveIn);
            wlc.StartRecording();
            waveOut.Play();
        }
    }
}
