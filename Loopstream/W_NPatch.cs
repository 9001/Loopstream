using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;
using System.IO;

namespace NPatch
{
    public class Fork
    {
        public class Outlet : ISampleProvider
        {
            System.IO.FileStream fs;
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
                    if (iWrite < 0 && iterated) iWrite += buf.Length; // ok
                    else iWrite = 0; // latency lower than requested
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
            }
        }
    }

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

    public class VolumeSlider : ISampleProvider
    {
        readonly object lockObject = new object();
        readonly ISampleProvider source;
        float currentVolume;
        float targetVolume;
        float volumeDelta;
        float absDelta;
        public bool muted;

        public float curVol { get { return currentVolume; } set { } }

        public VolumeSlider() { }
        
        public VolumeSlider(ISampleProvider source, bool initiallySilent = false)
        {
            this.source = source;
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
                volumeDelta = (float)(volumeDelta / (seconds * source.WaveFormat.SampleRate));
                absDelta = Math.Abs(volumeDelta);
            }
        }

        public int Read(float[] buffer, int offset, int count)
        {
            int sourceSamplesRead = source.Read(buffer, offset, count);
            lock (lockObject)
            {
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

                        for (int ch = 0; ch < source.WaveFormat.Channels; ch++)
                        {
                            buffer[offset + sample++] *= currentVolume;
                        }
                    }
                }
            }
            //Console.WriteLine("{0:000000} {1:000000}", count, sourceSamplesRead);
            return sourceSamplesRead;
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
            get { return source.WaveFormat; }
        }
    }
}
