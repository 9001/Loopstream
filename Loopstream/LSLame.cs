using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Loopstream
{
    public class LSLame : LSEncoder
    {
        public LSLame(LSSettings settings, LSPcmFeed pimp)
        {
            logger = Logger.mp3;

            this.pimp = pimp;
            this.settings = settings;
            logger.a("creating lame object");
            proc = new System.Diagnostics.Process();
            proc.StartInfo.FileName = Program.tools + "lame.exe";
            proc.StartInfo.WorkingDirectory = Program.tools.Trim('\\');
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardInput = true;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.Arguments = string.Format(
                "{0} {1} -h -m {2} --noreplaygain -S " + //..................target params
                "-r -s {3} --bitwidth 16 --signed --little-endian - -", //...source params
                (settings.mp3.compression == LSSettings.LSCompression.cbr ? "--preset cbr" : "-V"),
                (settings.mp3.compression == LSSettings.LSCompression.cbr ? settings.mp3.bitrate : settings.mp3.quality),
                (settings.mp3.channels == LSSettings.LSChannels.stereo ? "j" : "s -a"),
                settings.samplerate);

            if (!File.Exists(proc.StartInfo.FileName))
            {
                System.Windows.Forms.MessageBox.Show(
                    "Could not start streaming due to a missing required file:\r\n\r\n" + proc.StartInfo.FileName +
                    "\r\n\r\nThis is usually because whoever made your loopstream.exe fucked up",
                    "Shit wont fly", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                Program.kill();
            }

            logger.a("starting lame");
            proc.Start();
            while (true)
            {
                logger.a("waiting for lame");
                try
                {
                    proc.Refresh();
                    if (proc.Modules.Count > 1) break;

                    logger.a("modules: " + proc.Modules.Count);
                    System.Threading.Thread.Sleep(10);
                }
                catch { }
            }
            logger.a("lame running");
            pstdin = proc.StandardInput.BaseStream;
            pstdout = proc.StandardOutput.BaseStream;
            dump = settings.recMp3;
            enc = settings.mp3;
            makeShouter();
        }
    }
}
