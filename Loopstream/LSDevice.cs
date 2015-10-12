using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Loopstream
{
    public class LSDevice
    {
        bool tested;
        public string id, name;
        public bool isRec, isPlay;
        public string capt1, capt2;
        public string serializationData;

        [XmlIgnore]
        public NAudio.CoreAudioApi.MMDevice mm;

        [XmlIgnore]
        public NAudio.Wave.WaveFormat wf;

        public LSDevice()
        {
            id = name = null;
            isRec = isPlay = false;
            serializationData = "undef";
            capt1 = "undef";
            capt2 = "undef";
            mm = null;
            wf = null;
        }

        void makeSerializationData()
        {
            serializationData = LSDevice.stringer(wf);
        }

        public override string ToString()
        {
            return string.Format("{0} - {1}", !tested ? "?" : tested && wf == null ? "FUCKED" : "OK", name);
        }

        public static string stringer(NAudio.Wave.IWaveIn wp)
        {
            if (wp == null) return "FUCKED";
            return stringer(wp.WaveFormat);
        }

        public static string stringer(NAudio.Wave.WaveFormat wf)
        {
            if (wf == null) return "FUCKED";
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
                NAudio.Wave.IWaveIn dev = null;

                capt1 = mm.FriendlyName; // windows name
                capt2 = mm.DeviceFriendlyName; // just device
                if (capt1.EndsWith(capt2 + ")"))
                {
                    capt1 = capt1.Substring(0, capt1.Length - (capt2.Length + 3));
                }
                isRec = mm.DataFlow == NAudio.CoreAudioApi.DataFlow.All || mm.DataFlow == NAudio.CoreAudioApi.DataFlow.Capture;
                isPlay = mm.DataFlow == NAudio.CoreAudioApi.DataFlow.All || mm.DataFlow == NAudio.CoreAudioApi.DataFlow.Render;

                dev = isPlay
                    ? dev = new NAudio.Wave.WasapiLoopbackCapture(mm)
                    : dev = new NAudio.CoreAudioApi.WasapiCapture(mm);

                if (dev != null)
                {
                    wf = dev.WaveFormat;
                    makeSerializationData();
                    dev.Dispose();
                    return true;
                }
            }
            catch
            {
                mm = null;
            }
            return false;
        }
    }
}
