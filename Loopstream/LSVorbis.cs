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
            logger = Logger.ogg;
            pimp.tagger.changedTags += new TagsChanged(ForcefulInsertion);

            this.pimp = pimp;
            this.settings = settings;
            logger.a("creating oggenc object");
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

            if (!File.Exists(proc.StartInfo.FileName))
            {
                System.Windows.Forms.MessageBox.Show(
                    "Could not start streaming due to a missing required file:\r\n\r\n" + proc.StartInfo.FileName +
                    "\r\n\r\nThis is usually because whoever made your loopstream.exe fucked up",
                    "Shit wont fly", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                Program.kill();
            }

            logger.a("starting oggenc");
            proc.Start();
            while (true)
            {
                logger.a("waiting for oggenc");
                try
                {
                    proc.Refresh();
                    if (proc.Modules.Count > 1) break;

                    logger.a("modules: " + proc.Modules.Count);
                    System.Threading.Thread.Sleep(10);
                }
                catch { }
            }
            /*foreach (System.Diagnostics.ProcessModule mod in proc.Modules)
            {
                Console.WriteLine(mod.ModuleName + " // " + mod.FileName);
            }*/
            logger.a("oggenc running");
            pstdin = proc.StandardInput.BaseStream;
            pstdout = proc.StandardOutput.BaseStream;
            dump = settings.recOgg;
            enc = settings.ogg;
            makeShouter();
        }

        public void ForcefulInsertion(string newtag)
        {
            newTags = true;
            tags = newtag;
        }
    }
}
