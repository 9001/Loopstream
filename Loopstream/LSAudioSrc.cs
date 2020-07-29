using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Loopstream
{
    public interface LSAudioSrc
    {
        string id {get; set; }
        string name { get; set; }
        NAudio.Wave.WaveFormat wf { get; set; }
    }
}
