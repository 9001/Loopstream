using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoopStream
{
    public class LSDevice
    {
        public string id, name;
        public bool isRec, isPlay;
        public NAudio.CoreAudioApi.MMDevice mm;
        public NAudio.Wave.WaveFormat wf;
        bool tested;

        public LSDevice()
        {
            id = name = null;
            isRec = isPlay = false;
            mm = null;
            wf = null;
        }

        public override string ToString()
        {
            return string.Format("{0} - {1}", !tested ? "?" : tested && wf == null ? "FUCKED" : "OK", name);
        }

        public static string stringer(NAudio.Wave.IWaveIn wp)
        {
            return stringer(wp.WaveFormat);
        }

        public static string stringer(NAudio.Wave.WaveFormat wf)
        {
            return "ABPS:" + wf.AverageBytesPerSecond +
                "  BPS:" + wf.BitsPerSample +
                "  BA:" + wf.BlockAlign +
                "  CH:" + wf.Channels +
                "  ENC:" + wf.Encoding +
                "  ES:" + wf.ExtraSize +
                "  SR:" + wf.SampleRate;
        }

        public bool test()
        {
            tested = true;
            wf = null;
            try
            {
                if (mm == null) return false;
                if (mm.DataFlow == NAudio.CoreAudioApi.DataFlow.All ||
                    mm.DataFlow == NAudio.CoreAudioApi.DataFlow.Render)
                {
                    NAudio.Wave.WasapiLoopbackCapture dev = new NAudio.Wave.WasapiLoopbackCapture(mm);
                    wf = dev.WaveFormat;
                    dev.Dispose();
                    return true;
                }
                else
                {
                    NAudio.CoreAudioApi.WasapiCapture dev = new NAudio.CoreAudioApi.WasapiCapture(mm);
                    wf = dev.WaveFormat;
                    dev.Dispose();
                    return true;
                }
            }
            catch { return false; }
        }
    }
}
