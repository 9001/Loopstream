using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Loopstream
{
    public class LSVorbis : LSEncoder
    {
        public LSVorbis(LSSettings settings, LSPcmFeed pimp) : base()
        {
            this.pimp = pimp;
            this.settings = settings;
            proc = new System.Diagnostics.Process();
            proc.StartInfo.FileName = Program.tools + "oggenc2.exe";
            proc.StartInfo.WorkingDirectory = Program.tools.Trim('\\');
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardInput = true;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.Arguments = string.Format(
                "-Q -R 44100 {0} {1} {2} " + //.................target params
                "-r -F 1 -B 16 -C 2 --raw-endianness 0 -", //...source params
                (settings.ogg.compression == LSSettings.LSCompression.cbr ? "-b" : "-q"),
                (settings.ogg.compression == LSSettings.LSCompression.cbr ? settings.ogg.bitrate : settings.ogg.quality),
                (settings.ogg.channels == LSSettings.LSChannels.stereo ? "" : "--downmix"));

            proc.Start();
            while (true)
            {
                try
                {
                    proc.Refresh();
                    if (proc.Modules.Count > 1) break;
                }
                catch { }
            }
            foreach (System.Diagnostics.ProcessModule mod in proc.Modules)
            {
                Console.WriteLine(mod.ModuleName + " // " + mod.FileName);
            }
            pstdin = proc.StandardInput.BaseStream;
            pstdout = proc.StandardOutput.BaseStream;
            dump = settings.recOgg;
            enc = settings.ogg;
            makeShouter();
        }
    }
}
