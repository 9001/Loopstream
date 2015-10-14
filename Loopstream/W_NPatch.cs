using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;
using System.IO;

namespace NPatch
{
    public class Mixa : ISampleProvider
    {
        float[] buf;
        List<ISampleProvider> sources;
        public WaveFormat WaveFormat { get; set; }
        public Mixa(WaveFormat wf)
        {
            if (wf.Encoding != WaveFormatEncoding.IeeeFloat)
            {
                throw new ArgumentException("Mixer wave format must be IEEE float");
            }
            this.sources = new List<ISampleProvider>();
            this.WaveFormat = wf;
        }
        public void AddMixerInput(ISampleProvider mixerInput)
        {
            lock (sources)
            {
                sources.Add(mixerInput);
            }
            if (this.WaveFormat.SampleRate != mixerInput.WaveFormat.SampleRate ||
                this.WaveFormat.Channels != mixerInput.WaveFormat.Channels)
            {
                throw new ArgumentException("All mixer inputs must have the same WaveFormat");
            }
        }
        public void RemoveMixerInput(ISampleProvider mixerInput)
        {
            lock (sources)
            {
                sources.Remove(mixerInput);
            }
        }
        public int Read(float[] buffer, int offset, int count)
        {
            int ret = 0;
            buf = NAudio.Utils.BufferHelpers.Ensure(buf, count);
            lock (sources)
            {
                for (int s = 0; s < sources.Count; s++)
                {
                    int i = sources[s].Read(buf, 0, count);
                    if (s == 0)
                    {
                        ret = i;
                        for (int a = 0; a < i; a++)
                        {
                            buffer[a] = buf[a];
                        }
                    }
                    else
                    {
                        for (int a = 0; a < i; a++)
                        {
                            buffer[a] += buf[a];
                        }
                    }
                }
            }
            //Console.WriteLine(ret);
            return ret;
        }
        public int oRead(float[] buffer, int offset, int count)
        {
            int outputSamples = 0;
            float[] sourceBuffer = new float[count];// NAudio.Utils.BufferHelpers.Ensure(sourceBuffer, count);
            lock (sources)
            {
                int index = sources.Count - 1;
                while (index >= 0)
                {
                    var source = sources[index];
                    int samplesRead = source.Read(sourceBuffer, 0, count);
                    int outIndex = offset;
                    for (int n = 0; n < samplesRead; n++)
                    {
                        if (n >= outputSamples)
                        {
                            buffer[outIndex++] = sourceBuffer[n];
                        }
                        else
                        {
                            buffer[outIndex++] += sourceBuffer[n];
                        }
                    }
                    outputSamples = Math.Max(samplesRead, outputSamples);
                    if (samplesRead == 0)
                    {
                        sources.RemoveAt(index);
                    }
                    index--;
                }
            }
            Console.WriteLine(outputSamples);
            return outputSamples;
        }
    }

    // ********************************************************************************************************************************************************
    // ********************************************************************************************************************************************************
    // ********************************************************************************************************************************************************

    public class Fork
    {
        public class Outlet : ISampleProvider
        {
            Fork fork;
            float[] buf;
            bool iterated;
            object locker;
            int iRead, iWrite;
            public WaveFormat WaveFormat { get; set; }
            /// <summary>
            /// Forked output from a common source. Not compatible with WasapiOut
            /// </summary>
            public Outlet(Fork fork, WaveFormat wf, object locker, int i)
            {
                iterated = false;
                this.WaveFormat = wf;
                this.fork = fork;
                this.locker = locker;
                buf = new float[wf.SampleRate * wf.Channels * 5];
                iRead = iWrite = 0;
            }
            public int Read(float[] fb, int offset, int count)
            {
                fork.sync(count, this);
                lock (locker)
                {
                    //Console.WriteLine("{3} {0:000000} {1:000000} {2:000000}", buf.Length, iRead, iWrite, DateTime.UtcNow.ToLongTimeString());

                    count = Math.Min(Math.Min(count, fb.Length - offset), avail());
                    
                    // apparently the incoming array is a BYTE array, not a FUCKING FLOAT
                    // thank you for invalidating my code naudio

                    /*int len = buf.Length - iRead;
                    if (count >= len)
                    {
                        Array.Copy(buf, iRead, fb, offset, count);
                        offset += len;
                        count -= len;
                        Array.Copy(buf, 0, fb, offset, count);
                        iRead = count;
                    }
                    else
                    {
                        Array.Copy(buf, iRead, fb, offset, count);
                        iRead += count;
                    }*/


                    int ret = count;
                    while (--count >= 0)
                    {
                        fb[offset++] = buf[iRead++];
                        if (iRead >= buf.Length)
                            iRead = 0;
                    }
                    return ret;
                }
            }
            /// <summary>
            /// Called by Fork (with lock)
            /// </summary>
            public void Write(float[] fb, int offset, int count)
            {
                // TODO: Check overrun/underrun
                int len = buf.Length - iWrite;
                if (count >= len)
                {
                    Array.Copy(fb, offset, buf, iWrite, len);
                    offset += len;
                    count -= len;
                    Array.Copy(fb, offset, buf, 0, count);
                    iWrite = count;
                    iterated = true;
                }
                else
                {
                    Array.Copy(fb, offset, buf, iWrite, count);
                    iWrite += count;
                }
            }
            /// <summary>
            /// Available samples for reading
            /// </summary>
            public int avail()
            {
                return iWrite >= iRead ? iWrite - iRead : iWrite + buf.Length - iRead;
            }
            /// <summary>
            /// Position read pointer «latency» seconds behind write pointer
            /// </summary>
            public void setReadPtr(double latency)
            {
                lock (locker)
                {
                    int i = iWrite;
                    i -= (int)((this.WaveFormat.SampleRate * this.WaveFormat.Channels) / latency);
                    if (i < 0 && iterated) i += buf.Length; // ok
                    else i = 0; // latency lower than requested
                    iRead = i;
                }
            }
        }

        float[] buf;
        object locker;
        ISampleProvider source;
        public Outlet[] providers;
        /// <summary>
        /// Forks a single source into multiple sources with separate buffers.
        /// Does NOT work when hooked up to a WasapiOut (TODO: figure out why)
        /// </summary>
        public Fork(ISampleProvider source, int outlets)
        {
            buf = new float[source.WaveFormat.SampleRate * source.WaveFormat.Channels * 5];
            this.source = source;
            locker = new object();
            providers = new Outlet[outlets];
            for (int a = 0; a < outlets; a++)
            {
                providers[a] = new Outlet(this, source.WaveFormat, locker, a);
            }
        }

        /// <summary>
        /// Performs the read from the source stream.
        /// Called by all providers, only executed for first.
        /// First provider MUST be a stable sink, such as a WasapiOut.
        /// </summary>
        public void sync(int smp, Outlet outlet)
        {
            if (outlet != providers[0]) return;
            lock (locker)
            {
                int i = source.Read(buf, 0, smp);
                foreach (Outlet o in providers)
                {
                    o.Write(buf, 0, i);
                }
                //Console.WriteLine(i);
            }
        }
    }

    // ********************************************************************************************************************************************************
    // ********************************************************************************************************************************************************
    // ********************************************************************************************************************************************************

    public class Reverb : ISampleProvider
    {
        ISampleProvider source;
        WaveFormat waveFormat;
        float[] sourceBuffer;
        float[] reverbBuffer;
        int channels;
        int[][] rev;
        int revw;

        public Reverb(ISampleProvider source)
        {
            if (source.WaveFormat.Channels < 2)
            {
                throw new ArgumentException("Source must be stereo or more");
            }
            this.source = source;
            this.channels = source.WaveFormat.Channels;
            this.waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(source.WaveFormat.SampleRate, channels);
            reverbBuffer = new float[44100];
            rev = new int[][]
            {
                // Centered heptagonal primes
                // Of the form (7n2 − 7n + 2) / 2
                new int[]
                {
                    71, 197, 547, 953, 1471, 1933, 2647, 2843, 3697, 4663, 5741, 8233, 9283, 10781, 11173, 12391, 14561, 18397, 20483, 29303, 29947, 34651, 37493, 41203
                },
                new int[]
                {
                    43, 463, 547, 953, 1471, 1933, 2647, 2843, 3697, 4663, 5741, 8233, 9283, 10781, 11173, 12391, 14561, 18397, 20483, 29303, 29947, 34651, 37493, 41203
                }
            };
            for (int a = 0; a < rev.Length; a++)
                for (int b = 0; b < rev[a].Length; b++)
                    rev[a][b] = reverbBuffer.Length - (1 + rev[a][b]);
            revw = 0;
        }

        public WaveFormat WaveFormat
        {
            get { return this.waveFormat; }
        }

        public int Read(float[] buffer, int offset, int count)
        {
            int sourceSamplesRequired = count;
            int outIndex = offset;
            EnsureSourceBuffer(sourceSamplesRequired);
            int sourceSamplesRead = source.Read(sourceBuffer, 0, sourceSamplesRequired);

            int sourceSamplesWritten = Math.Min(sourceSamplesRead, (reverbBuffer.Length - revw));
            Array.Copy(sourceBuffer, 0, reverbBuffer, revw, sourceSamplesWritten);
            revw += sourceSamplesWritten;
            if (revw >= reverbBuffer.Length)
            {
                revw = sourceSamplesRead - sourceSamplesWritten;
                Array.Copy(sourceBuffer, sourceSamplesWritten, reverbBuffer, 0, sourceSamplesRead - sourceSamplesWritten);
            }
            for (int n = 0; n < sourceSamplesRead; n += channels)
            {
                for (int c = 0; c < channels; c++)
                {
                    buffer[outIndex++] = sourceBuffer[n + c];
                }
            }
            for (int ch = 0; ch < rev.Length; ch++)
            {
                double boost = (Loopstream.LSSettings.singleton.reverbP / 100.0);
                for (int ri = 0; ri < rev[ch].Length; ri++)
                {
                    outIndex -= sourceSamplesRead;
                    for (int n = 0; n < sourceSamplesRead; n += channels)
                    {
                        try
                        {
                            if (rev[ch][ri] >= reverbBuffer.Length - ch)
                                rev[ch][ri] -= reverbBuffer.Length;
                            buffer[outIndex + ch] += (float)(reverbBuffer[rev[ch][ri] + ch] * boost);
                            outIndex += channels;
                            rev[ch][ri] += channels;
                        }
                        catch (Exception ex)
                        {
                            System.Windows.Forms.MessageBox.Show("buffer " + (outIndex + ch) + " of " + buffer.Length + ", reverb " + (rev[ch][ri] + ch) + " of " + reverbBuffer.Length + " (" + ch + "/" + ri + ")");
                        }
                    }
                    boost *= (Loopstream.LSSettings.singleton.reverbS / 100.0);
                }
            }
            return sourceSamplesRead;
        }

        private void EnsureSourceBuffer(int count)
        {
            if (this.sourceBuffer == null || this.sourceBuffer.Length < count)
            {
                this.sourceBuffer = new float[count];
            }
        }
    }

    // ********************************************************************************************************************************************************
    // ********************************************************************************************************************************************************
    // ********************************************************************************************************************************************************

    public class ChannelSelector : ISampleProvider
    {
        ISampleProvider source;
        WaveFormat waveFormat;
        float[] sourceBuffer;
        int ch;
        int channels;

        //int frac;
        //bool mul;

        public ChannelSelector(ISampleProvider source, int keepChannel)
        {
            if (source.WaveFormat.Channels < 2)
            {
                throw new ArgumentException("Source must be stereo or more");
            }
            this.ch = keepChannel;
            this.source = source;
            this.channels = source.WaveFormat.Channels;
            this.waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(source.WaveFormat.SampleRate, channels);
            /*if (waveFormat.Channels > source.WaveFormat.Channels)
            {
                frac = waveFormat.Channels / source.WaveFormat.Channels;
                mul = false;
            }
            else
            {
                frac = source.WaveFormat.Channels / waveFormat.Channels;
                mul = true;
            }*/
        }

        public WaveFormat WaveFormat
        {
            get { return this.waveFormat; }
        }

        public int Read(float[] buffer, int offset, int count)
        {
            //int sourceSamplesRequired = mul ? count * frac : count / frac;
            int sourceSamplesRequired = count;
            int outIndex = offset;
            EnsureSourceBuffer(sourceSamplesRequired);
            int sourceSamplesRead = source.Read(sourceBuffer, 0, sourceSamplesRequired);
            for (int n = 0; n < sourceSamplesRead; n += channels)// ++)
            {
                for (int c = 0; c < channels; c++)
                {
                    buffer[outIndex++] = sourceBuffer[n + ch];
                }
            }
            return sourceSamplesRead;
        }

        private void EnsureSourceBuffer(int count)
        {
            if (this.sourceBuffer == null || this.sourceBuffer.Length < count)
            {
                this.sourceBuffer = new float[count];
            }
        }
    }

    // ********************************************************************************************************************************************************
    // ********************************************************************************************************************************************************
    // ********************************************************************************************************************************************************

    public class VolumeSlider : ISampleProvider
    {
        readonly object lockObject = new object();
        ISampleProvider source;
        WaveFormat wf;
        float currentVolume;
        float targetVolume;
        float volumeDelta;
        float absDelta;
        public bool muted;

        public float boost;
        public float boostLock;
        public bool attenuated;

        //public event EventHandler vuc;
        public bool enVU;
        public double VU { get; private set; }
        public long vuAge { get; private set; }
        public float curVol { get { return currentVolume; } set { } }

        public VolumeSlider()
        {
            this.source = null;
            this.wf = null;
            currentVolume = 1;
            targetVolume = 1;
            enVU = false;
            VU = 1;

            attenuated = false;
            boost = 1;
        }

        public void SetSource(ISampleProvider source)
        {
            this.source = source;
            if (source != null)
                wf = source.WaveFormat;
            else wf = null;
        }

        public bool OK()
        {
            return this.source != null;
        }

        public float GetVolume()
        {
            return targetVolume;
        }

        public void SetVolume(float volume)
        {
            SetVolume(volume, 0);
        }

        public void SetVolume(float volume, double seconds)
        {
            targetVolume = volume;
            volumeDelta = volume - currentVolume;
            if (seconds > 0)
            {
                int samplerate = 44100;
                if (wf != null)
                    samplerate = wf.SampleRate;

                volumeDelta = (float)(volumeDelta / (seconds * samplerate));
                absDelta = Math.Abs(volumeDelta);
            }
            else
            {
                absDelta = 0;
                currentVolume = volume;
            }
        }

        public int Read(float[] buffer, int offset, int count)
        {
            try
            {
                int sourceSamplesRead = source.Read(buffer, offset, count);

                double amp = 0;
                if (enVU)
                {
                    for (int a = 0; a < sourceSamplesRead; a++)
                    {
                        amp = Math.Max(amp, buffer[offset + a]);
                    }
                    amp *= boost;
                    if (amp > 0)
                    {
                        // local playback on higher sample rate
                        // than inputs cause empty buffers here
                        VU = amp > 1 ? 1 : amp;
                        vuAge = 0;
                    }
                    else vuAge++;
                }
                lock (lockObject)
                {
                    float nboost = boost;
                    if (currentVolume == targetVolume && targetVolume == 0 || muted)
                    {
                        ClearBuffer(buffer, offset, count);
                    }
                    else
                    {
                        int sample = 0;
                        while (sample < sourceSamplesRead)
                        {
                            if (Math.Abs(targetVolume - currentVolume) > absDelta * 1.5)
                            {
                                currentVolume += volumeDelta;
                            }
                            else currentVolume = targetVolume;

                            for (int ch = 0; ch < wf.Channels; ch++)
                            {
                                float v = buffer[offset + sample] * currentVolume * nboost;
                                if (v > 1)
                                {
                                    nboost = Math.Max(boostLock, Math.Max(1f, nboost / v));
                                    v = 1;
                                }
                                buffer[offset + sample++] = v;
                            }
                        }
                    }
                    if (nboost < boost && boost > boostLock)
                    {
                        boost = Math.Max(nboost, boostLock);
                        attenuated = true;
                    }
                }
                //Console.WriteLine("{0:000000} {1:000000}", count, sourceSamplesRead);
                return sourceSamplesRead;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("VolumeSlider::Read>\n\n" + ex.ToString());
                return -1;
            }
        }

        private static void ClearBuffer(float[] buffer, int offset, int count)
        {
            for (int n = 0; n < count; n++)
            {
                buffer[n + offset] = 0;
            }
        }

        public WaveFormat WaveFormat
        {
            get { return wf; }
        }
    }
}
