using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loopstream
{
    class LSMixer
    {
        LSSettings settings;
        NAudio.Wave.WaveFormat format;
        NAudio.Wave.WasapiLoopbackCapture recCap;
        NAudio.CoreAudioApi.WasapiCapture micCap;
        NAudio.Wave.BufferedWaveProvider recIn, micIn;
        NAudio.Dmo.Resampler recRe, micRe;
        NPatch.VolumeSlider recVol, micVol, outVol;
        NAudio.Wave.SampleProviders.MixingSampleProvider mixer;
        NAudio.Wave.SampleProviders.SampleToWaveProvider muxer;
        NAudio.Wave.WasapiOut mixOut;
        NPatch.Fork fork;
        NAudio.Wave.WaveFileWriter waver;
        public NPatch.Fork.Outlet lameOutlet;

        public void Dispose()
        {
            recCap.StopRecording();
            micCap.StopRecording();
            mixOut.Dispose();
            if (recRe != null) recRe.Dispose();
            if (micRe != null) micRe.Dispose();
        }

        public enum Slider
        {
            Music,
            Mic,
            Out
        };

        public LSMixer(LSSettings settings)
        {
            this.settings = settings;
            doMagic();
        }

        void doMagic()
        {
            recRe = micRe = null;
            NAudio.Wave.ISampleProvider recProv, micProv;
            //format = new NAudio.Wave.WaveFormat(44100, 32, 2);
            format = NAudio.Wave.WaveFormat.CreateIeeeFloatWaveFormat(44100, 2);
            mixer = new NAudio.Wave.SampleProviders.MixingSampleProvider(format);
            // music=ABPS:384000  BPS:32  BA:8  CH:2  ENC:IeeeFloat  ES:0  SR:48000
            // mic=ABPS:352800  BPS:32  BA:8  CH:2  ENC:IeeeFloat  ES:0  SR:44100
            //settings.mixer.valueChanged += mixer_valueChanged;
            
            recCap = new NAudio.Wave.WasapiLoopbackCapture(settings.devRec.mm);
            /*if (recCap.WaveFormat.SampleRate != settings.samplerate)
            {
                recRe = new NAudio.Dmo.Resampler();
                recRe.MediaObject.SetInputWaveFormat(0, recCap.WaveFormat);%
                recRe.MediaObject.SetOutputWaveFormat(0, format);
                recRe.MediaObject.AllocateStreamingResources();
            }*/
            if (recCap.WaveFormat.SampleRate != settings.samplerate)
            {
                System.Windows.Forms.MessageBox.Show("Incorrect samplerate on music device, aborting");
                Program.kill();
            }
            recCap.DataAvailable += recDev_DataAvailable_03;
            recIn = new NAudio.Wave.BufferedWaveProvider(format); //recCap.WaveFormat);
            recProv = new NAudio.Wave.SampleProviders.WaveToSampleProvider(recIn);
            //recProv = new NPatch.ChannelSelector(recProv, settings.micLeft ? 0 : 1);
            recVol = new NPatch.VolumeSlider(recProv);
            mixer.AddMixerInput(recVol);

            if (settings.devMic.mm != null)
            {
                micCap = new NAudio.CoreAudioApi.WasapiCapture(settings.devMic.mm);
                if (micCap.WaveFormat.SampleRate != settings.samplerate)
                {
                    System.Windows.Forms.MessageBox.Show("Incorrect samplerate on microphone device, aborting");
                    Program.kill();
                }
                micIn = new NAudio.Wave.BufferedWaveProvider(micCap.WaveFormat);
                micCap.DataAvailable += micDev_DataAvailable_03;
                micProv = new NAudio.Wave.SampleProviders.WaveToSampleProvider(micIn);
                if (micCap.WaveFormat.Channels == 1)
                {
                    micProv = new NAudio.Wave.SampleProviders.MonoToStereoSampleProvider(micProv);
                }
                else if (settings.micLeft != settings.micRight)
                {
                    micProv = new NPatch.ChannelSelector(micProv, settings.micLeft ? 0 : 1);
                }
                micVol = new NPatch.VolumeSlider(micProv);
                mixer.AddMixerInput(micVol);
            }
            else
            {
                micVol = new NPatch.VolumeSlider();
            }

            fork = new NPatch.Fork(mixer, 2);
            lameOutlet = fork.providers[1];
            outVol = new NPatch.VolumeSlider(fork.providers[0]);
            muxer = new NAudio.Wave.SampleProviders.SampleToWaveProvider(outVol);
            //mixer_valueChanged(null, null);
            recVol.SetVolume((float)settings.mixer.vRec);
            micVol.SetVolume((float)settings.mixer.vMic);
            outVol.SetVolume((float)settings.mixer.vOut);
            recVol.muted = !settings.mixer.bRec;
            micVol.muted = !settings.mixer.bMic;
            outVol.muted = !settings.mixer.bOut;
            
            mixOut = new NAudio.Wave.WasapiOut(settings.devOut.mm,
                NAudio.CoreAudioApi.AudioClientShareMode.Shared, false, 100);

            mixOut.Init(muxer);
            recCap.StartRecording();
            if (settings.devMic.mm != null)
            {
                micCap.StartRecording();
            }
            mixOut.Play();

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
            if (slider == Slider.Music) recVol.SetVolume(vol, seconds);
            if (slider == Slider.Mic) micVol.SetVolume(vol, seconds);
            if (slider == Slider.Out) outVol.SetVolume(vol, seconds);
        }

        public void MuteChannel(Slider slider, bool notMuted)
        {
            //if (slider == Slider.Music) recVol.SetVolume(notMuted ? (float)settings.mixer.vRec : 0);
            //if (slider == Slider.Mic) micVol.SetVolume(notMuted ? (float)settings.mixer.vMic : 0);
            //if (slider == Slider.Out) outVol.SetVolume(notMuted ? (float)settings.mixer.vOut : 0);
            if (slider == Slider.Music) recVol.muted = !notMuted;
            if (slider == Slider.Mic) micVol.muted = !notMuted;
            if (slider == Slider.Out) outVol.muted = !notMuted;
        }

        void recDev_DataAvailable_03(object sender, NAudio.Wave.WaveInEventArgs e)
        {
            if (recRe == null)
            {
                recIn.AddSamples(e.Buffer, 0, e.BytesRecorded);
            }
            else
            {
                resampler(e, recRe, recIn);
            }
        }

        void micDev_DataAvailable_03(object sender, NAudio.Wave.WaveInEventArgs e)
        {
            if (micRe == null)
            {
                micIn.AddSamples(e.Buffer, 0, e.BytesRecorded);
            }
            else
            {
                resampler(e, micRe, micIn);
            }
        }

        void resampler(NAudio.Wave.WaveInEventArgs e, NAudio.Dmo.Resampler re, NAudio.Wave.BufferedWaveProvider wp)
        {
            Console.Write('.');
            NAudio.Dmo.MediaBuffer b = new NAudio.Dmo.MediaBuffer(e.Buffer.Length);
            b.LoadData(e.Buffer, e.BytesRecorded);
            re.MediaObject.ProcessInput(0, b, NAudio.Dmo.DmoInputDataBufferFlags.None, 0, 0);
            using (NAudio.Dmo.DmoOutputDataBuffer outputBuffer = new
                   NAudio.Dmo.DmoOutputDataBuffer(format.AverageBytesPerSecond))
            {
                re.MediaObject.ProcessOutput(
                    NAudio.Dmo.DmoProcessOutputFlags.None, 1,
                    new NAudio.Dmo.DmoOutputDataBuffer[] { outputBuffer });
                byte[] oBytes = new byte[outputBuffer.Length];
                outputBuffer.RetrieveData(oBytes, 0);
                wp.AddSamples(oBytes, 0, oBytes.Length);
            }
        }

        void doMagic02_WORKS()
        {
            NAudio.Wave.WasapiLoopbackCapture wlc = new NAudio.Wave.WasapiLoopbackCapture(settings.devRec.mm);
            NAudio.Wave.WaveInProvider waveIn = new NAudio.Wave.WaveInProvider(wlc);
            NAudio.Wave.WasapiOut waveOut = new NAudio.Wave.WasapiOut(settings.devOut.mm, NAudio.CoreAudioApi.AudioClientShareMode.Shared, false, 100);
            waveOut.Init(waveIn);
            wlc.StartRecording();
            waveOut.Play();
        }
    }
}
