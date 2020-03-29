using NAudio.Wave;
using System;
using System.Collections.Generic;

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
            Dumper dumper_r, dumper_w;
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

                dumper_r = null;
                dumper_w = null;
                //dumper_r = new Dumper(wf, "fork_r");
                //dumper_w = new Dumper(wf, "fork_w");
            }
            public int Read(float[] fb, int offset, int count)
            {
                fork.sync(count, this);
                lock (locker)
                {
                    count = Math.Min(Math.Min(count, fb.Length - offset), avail());
                    
                    int ret = count;
                    while (--count >= 0)
                    {
                        fb[offset++] = buf[iRead++];
                        if (iRead >= buf.Length)
                            iRead = 0;
                    }
                    if (dumper_r != null)
                        dumper_r.samples(fb, offset - ret, ret);

                    return ret;
                }
            }
            /// <summary>
            /// Called by Fork (with lock)
            /// </summary>
            public void Write(float[] fb, int offset, int count)
            {
                if (dumper_w != null)
                    dumper_w.samples(fb, offset, count);

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
                    int align = this.WaveFormat.Channels * 4;
                    i -= (int)(((this.WaveFormat.SampleRate * this.WaveFormat.Channels) / latency) / align) * align;
                    if (i < 0 && iterated) i += (int)(buf.Length / align) * align; // ok
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

    public class ChannelSelectorIn : ISampleProvider
    {
        ISampleProvider source;
        float[] sourceBuffer;
        int[] chans2keep;

        public ChannelSelectorIn(ISampleProvider source, int[] chans2keep, int minChansOut)
        {
            if (source.WaveFormat.Channels < 2)
                throw new ArgumentException("Source must be stereo or more");

            // duplicate the input channel if 1 is selected but we want 2
            if (chans2keep.Length < minChansOut && chans2keep.Length == 1)
                chans2keep = new int[] { chans2keep[0], chans2keep[0] };

            this.source = source;
            this.chans2keep = chans2keep;
            this.WaveFormat = WaveFormat.CreateIeeeFloatWaveFormat(
                source.WaveFormat.SampleRate, chans2keep.Length);
        }

        public WaveFormat WaveFormat { get; private set; }

        public int Read(float[] buffer, int offset, int count)
        {
            int sourceChans = source.WaveFormat.Channels;
            count = (count / this.chans2keep.Length) * sourceChans;
            if (sourceBuffer == null || sourceBuffer.Length < count)
                sourceBuffer = new float[count];

            int outIndex = offset;
            int sourceSamplesRead = source.Read(sourceBuffer, 0, count);
            for (int n = 0; n < sourceSamplesRead; n += sourceChans)
                for (int c = 0; c < chans2keep.Length; c++)
                    buffer[outIndex++] = sourceBuffer[n + chans2keep[c]];

            return outIndex;
        }
    }

    // ********************************************************************************************************************************************************
    // ********************************************************************************************************************************************************
    // ********************************************************************************************************************************************************

    public class ChannelMapperOut : ISampleProvider
    {
        ISampleProvider source;
        float[] sourceBuffer;
        int[] chans2write;
        int numChansOut;

        public ChannelMapperOut(ISampleProvider source, int[] chans2write, int numChansOut)
        {
            if (source.WaveFormat.Channels < 1)
                throw new ArgumentException("Source must be mono or more");

            this.source = source;
            this.chans2write = chans2write;
            this.numChansOut = numChansOut;
            this.WaveFormat = WaveFormat.CreateIeeeFloatWaveFormat(
                source.WaveFormat.SampleRate, numChansOut);
        }

        public WaveFormat WaveFormat { get; private set; }

        public int Read(float[] buffer, int offset, int count0)
        {
            if (count0 == 0)
                return 0; // ??

            //return source.Read(buffer, offset, count0);

            int sourceChans = source.WaveFormat.Channels;
            int count = (count0 / numChansOut) * sourceChans;
            if (sourceBuffer == null || sourceBuffer.Length < count)
                sourceBuffer = new float[count];

            int outIndex = offset;
            int sourceSamplesRead = source.Read(sourceBuffer, 0, count);
            int numRet = (sourceSamplesRead / sourceChans) * numChansOut;
            /*System.Diagnostics.Debug.WriteLine(
                "buffer " + buffer.Length + ", offset " + offset + ", count0 " + count0 +
                ", ret " + numRet +
                ", sourceSamplesRead " + sourceSamplesRead + ", count " + count);*/

            // TODO blanking until buffer.length overwrites unrelated objects :thunk:
            for (int a = offset; a < offset + numRet; a++)
                buffer[a] = 0;

            for (int n = 0; n < sourceSamplesRead; n += sourceChans)
            {
                for (int c = 0; c < chans2write.Length; c++)
                    buffer[outIndex + chans2write[c]] = sourceBuffer[n + c];

                outIndex += numChansOut;

                //if (outIndex >= buffer.Length || outIndex > numRet + offset)
                //    throw new Exception("fug");
            }

            return outIndex - offset;
        }
    }

    // ********************************************************************************************************************************************************
    // ********************************************************************************************************************************************************
    // ********************************************************************************************************************************************************

    public class HelpfulSampleToWaveProvider16 : IWaveProvider
    {
        private readonly ISampleProvider sourceProvider;
        private volatile float volume;
        private float[] sourceBuffer;

        public WaveFormat WaveFormat { get; private set; }

        // unmodified naudio 1.10.0
        public HelpfulSampleToWaveProvider16(ISampleProvider sourceProvider)
        {
            if (sourceProvider.WaveFormat.Encoding != WaveFormatEncoding.IeeeFloat)
                throw new ArgumentException("Input source provider must be IEEE float");
            if (sourceProvider.WaveFormat.BitsPerSample != 32)
                throw new ArgumentException("Input source provider must be 32 bit");

            this.WaveFormat = new WaveFormat(sourceProvider.WaveFormat.SampleRate, 16, sourceProvider.WaveFormat.Channels);

            this.sourceProvider = sourceProvider;
        }

        // modified naudio 1.10.0
        public int Read(byte[] destBuffer, int offset, int numBytes)
        {
            // this is the big
            if (numBytes + offset > destBuffer.Length)
                throw new ArgumentOutOfRangeException("numBytes " + numBytes + "+" + offset + " exceeds " + destBuffer.Length);

            System.Diagnostics.Debug.WriteLine("wp16 read |" + destBuffer.Length + "|, " + offset + ", " + numBytes);
            int samplesRequired = numBytes / 2;
            sourceBuffer = NAudio.Utils.BufferHelpers.Ensure(sourceBuffer, samplesRequired);
            int sourceSamples = sourceProvider.Read(sourceBuffer, 0, samplesRequired);
            
            for (int sample = 0; sample < sourceSamples; sample++)
            {
                float sample32 = sourceBuffer[sample];
                if (sample32 > 1.0f)
                    sample32 = 1.0f;
                if (sample32 < -1.0f)
                    sample32 = -1.0f;

                short sample2 = (short)(sample32 * 32767);
                //System.Diagnostics.Debug.WriteLine("wp16 put |" + destBuffer.Length + "|, " + offset);
                destBuffer[offset++] = (byte)(sample2 / 256);
                destBuffer[offset++] = (byte)(sample2 % 256);
            }
            return sourceSamples * 2;
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
        Dumper dumper;

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
            this.dumper = null;
            if (source != null)
            {
                wf = source.WaveFormat;
                //this.dumper = new Dumper(wf, "vol");
            }
            else wf = null;
        }

        public bool OK()
        {
            return this.source != null;
        }

        public void ClearVu()
        {
            VU = 1;
            vuAge = 512;
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
                if (dumper != null)
                    dumper.samples(buffer, offset, sourceSamplesRead);

                double amp = 0;
                if (enVU)
                {
                    for (int a = 0; a < sourceSamplesRead; a++)
                    {
                        amp = Math.Max(amp, buffer[offset + a]);
                    }
                    amp *= boost;
                    if (amp > 0.001)
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

    // ********************************************************************************************************************************************************
    // ********************************************************************************************************************************************************
    // ********************************************************************************************************************************************************

    public class Dumper
    {
        static object lck = new object();
        static int ctr = 0;

        public Dumper(NAudio.Wave.WaveFormat wf, string tag)
        {
            string sheader = "";
            if (wf != null && wf.Channels == 1)
                sheader = "52 49 46 46 24 ff ff 7f 57 41 56 45 66 6D 74 20 10 00 00 00 01 00 01 00 44 AC 00 00 88 58 01 00 02 00 10 00 64 61 74 61 00 ff ff 7f";
            else if (wf != null && wf.Channels == 2)
                sheader = "52 49 46 46 24 ff ff 7f 57 41 56 45 66 6D 74 20 10 00 00 00 01 00 02 00 44 AC 00 00 10 B1 02 00 04 00 10 00 64 61 74 61 00 ff ff 7f";

            sheader = sheader.Replace(" ", "");
            byte[] header = new byte[sheader.Length / 2];
            if (header.Length > 0)
                for (int a = 0; a < header.Length; a++)
                    header[a] = Convert.ToByte(sheader.Substring(a * 2, 2), 16);

            string ext = "pcm";
            if (header.Length > 2)
                ext = "wav";

            string fn;
            lock (lck)
            {
                string ts = System.DateTime.UtcNow.ToString("yyyy-MM-dd_HH.mm.ss");
                fn = string.Format("Loopstream-{0}-{1}-{2}.{3}", ts, tag, ++ctr, ext);
            }
            fs = new System.IO.FileStream(fn, System.IO.FileMode.Create);
            if (header.Length > 0)
                fs.Write(header, 0, header.Length);
        }

        System.IO.FileStream fs;

        ~Dumper()
        {
            fs.Close();
            fs.Dispose();
        }

        public void wave(byte[] buf, int ofs, int len)
        {
            fs.Write(buf, ofs, len);
        }

        public void samples(float[] buf, int ofs, int len)
        {
            byte[] wbuf = new byte[len * 2];
            for (int a = ofs; a < ofs + len; a++)
            {
                short v = (short)(32767 * Math.Min(Math.Max(buf[a], -1), 1));
                wbuf[a * 2 + 0] = (byte)(v % 256);
                wbuf[a * 2 + 1] = (byte)(v / 256);
            }
            fs.Write(wbuf, 0, wbuf.Length);
        }
    }
}
