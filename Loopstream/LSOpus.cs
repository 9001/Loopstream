using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Loopstream
{
    public class LSOpus : LSEncoder
    {
        public LSOpus(LSSettings settings, LSPcmFeed pimp) : base()
        {
            logger = Logger.opus;

            this.pimp = pimp;
            this.settings = settings;
            logger.a("creating opusenc object");
            proc = new System.Diagnostics.Process();
            proc.StartInfo.FileName = Program.tools + "opusenc.exe";
            proc.StartInfo.WorkingDirectory = Program.tools.Trim('\\');
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardInput = true;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.Arguments = string.Format(
                "--quiet --bitrate {1} --raw --raw-rate {0} {2} - -",
                settings.samplerate,
                settings.opus.quality,
                (settings.opus.channels == LSSettings.LSChannels.stereo ? "--downmix-stereo" : "--downmix-mono"));

            if (!File.Exists(proc.StartInfo.FileName))
            {
                System.Windows.Forms.MessageBox.Show(
                    "Could not start streaming due to a missing required file:\r\n\r\n" + proc.StartInfo.FileName +
                    "\r\n\r\nThis is usually because whoever made your loopstream.exe fucked up",
                    "Shit wont fly", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                Program.kill();
            }

            logger.a("starting opusenc");
            proc.Start();
            while (true)
            {
                logger.a("waiting for opusenc");
                try
                {
                    proc.Refresh();
                    if (proc.Modules.Count > 1) break;

                    logger.a("modules: " + proc.Modules.Count);
                    System.Threading.Thread.Sleep(10);
                }
                catch { }
            }
            logger.a("opusenc running");
            pstdin = proc.StandardInput.BaseStream;
            pstdout = proc.StandardOutput.BaseStream;
            dump = settings.recOpus;
            enc = settings.opus;
            makeShouter();
        }
    }
}