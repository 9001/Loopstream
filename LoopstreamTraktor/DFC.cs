using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LoopstreamTraktor
{
    class DFC
    {
        public DFC()
        {
        }

        public void extract(Label pb)
        {
            icp = new ICP();
            System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ThreadStart(exthread));
            t.Name = "DFC_Extracter";
            t.Start();
            while (!extracting)
            {
                System.Threading.Thread.Sleep(1);
                Application.DoEvents();
            }
            while (extracting)
            {
                double progress = icp.i * 1.0 / toExtract;
                pb.Width = (int)(progress * 600);
                Application.DoEvents();
                System.Threading.Thread.Sleep(10);
            }
        }
        
        long toExtract = 0;
        bool extracting = false;
        void exthread()
        {
            using (Stream stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("LoopstreamTraktor.res.tools.dfc"))
            {
                if (stream == null)
                {
                    MessageBox.Show("dfc not found\n\n(ed fucked up)");
                    Program.kill();
                }
                if (!System.IO.Path.GetFullPath("asdf").StartsWith(Application.StartupPath))
                {
                    MessageBox.Show(
                        "Warning: The environment you gave me is fucked up\n\n" +
                        "===[ what I expected ]=================\n" +
                        Application.StartupPath + "\n\n" +
                        "===[ what I got ]=====================\n" +
                        System.IO.Path.GetFullPath("what") + "\n\n" +
                        "================================\n" +
                        "This makes no sense and now I'm gonna exit",
                        "How the fuck did that happen");
                    Program.kill();
                }
                byte[] tmp = new byte[8];
                stream.Read(tmp, 0, 4);
                byte[] bheader = new byte[BitConverter.ToInt32(tmp, 0) - 4];
                stream.Read(bheader, 0, bheader.Length);
                MemoryStream msh = new MemoryStream(bheader);
                msh.Read(tmp, 0, 4);
                byte[][] filename = new byte[BitConverter.ToInt32(tmp, 0)][];
                string[] files = new string[filename.Length];
                long[] cLen = new long[filename.Length];
                long[] eLen = new long[filename.Length];
                int bgmPtr = 0;
                toExtract = 0;

                for (int a = 0; a < filename.Length; a++)
                {
                    msh.Read(tmp, 0, 4);
                    filename[a] = new byte[BitConverter.ToInt32(tmp, 0)];
                }
                for (int a = 0; a < filename.Length; a++)
                {
                    msh.Read(tmp, 0, 8);
                    cLen[a] = BitConverter.ToInt64(tmp, 0);
                    toExtract += cLen[a];
                }
                for (int a = 0; a < filename.Length; a++)
                {
                    msh.Read(tmp, 0, 8);
                    eLen[a] = BitConverter.ToInt64(tmp, 0);
                }
                for (int a = 0; a < filename.Length; a++)
                {
                    msh.Read(filename[a], 0, filename[a].Length);
                }
                for (int a = 0; a < filename.Length; a++)
                {
                    files[a] = Encoding.UTF8.GetString(filename[a]).Replace('/', '\\');
                    int ofs = files[a].LastIndexOf('\\');
                    if (ofs > 0)
                    {
                        //Directory.CreateDirectory(tempdir + files[a].Substring(0, ofs));
                    }
                    if (files[a].StartsWith("z\\"))
                    {
                        //upgradeMode = true;
                    }
                }
                for (int a = 0; a < filename.Length; a++)
                {
                    if (!files[a].StartsWith("nsf\\"))
                    {
                        bgmPtr = a;
                        break;
                    }
                }
                extracting = true;
                for (int a = 0; a < filename.Length; a++)
                {
                    if (a == bgmPtr)
                    {
                        //bgmReady = true;
                    }
                    string fil = Program.tools + files[a];
                    string dir = fil.Substring(0, fil.Replace('/', '\\').LastIndexOf('\\'));
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }
                    extract(stream, cLen[a], Program.tools + files[a], icp);
                }
                extracting = false;
            }
        }

        bool extract(Stream si, long end, string filename, ICP icp)
        {
            SevenZip.Compression.LZMA.Decoder dec = new SevenZip.Compression.LZMA.Decoder();
            //using (MemoryStream msi = new MemoryStream(GamePatcher.Properties.Resources.kengeki1))
            using (FileStream fso = new FileStream(filename, FileMode.Create))
            {
                byte[] props = new byte[5];
                si.Read(props, 0, 5);

                byte[] length = new byte[8];
                si.Read(length, 0, 8);

                long len = BitConverter.ToInt64(length, 0);

                dec.SetDecoderProperties(props);
                //dec.Code(si, fso, si.Length, len, null);
                dec.Code(si, fso, end, len, icp);
            }
            //si.Close();
            return true;
        }

        public void make(Label pb)
        {
            icp = new ICP();
            new System.Threading.Thread(new System.Threading.ThreadStart(makeDFC)).Start();
            while (!encoding)
            {
                System.Threading.Thread.Sleep(1);
                Application.DoEvents();
            }
            while (encoding)
            {
                double progress = (icp.i + bytesCompressed) * 1.0 / bytesToCompress;
                //this.Text = string.Format("{0:0.00}%", Math.Round(progress * 100, 2)).Replace(".", " . ");
                pb.Width = (int)(progress * 600);
                Application.DoEvents();
                System.Threading.Thread.Sleep(10);
            }
            //this.Text = "done";
            MessageBox.Show("new dfc ok");
        }

        ICP icp;
        bool encoding = false;
        long bytesCompressed = 0;
        long bytesToCompress = 1;
        List<string> archivequeue;

        void recurse(string dir, List<string> archivequeue)
        {
            foreach (string str in Directory.GetDirectories(dir))
            {
                recurse(str, archivequeue);
            }
            archivequeue.AddRange(Directory.GetFiles(dir));
        }

        void makeDFC()
        {
            archivequeue = new List<string>();
            string src = Path.GetFullPath(@"..\..\tools\");
            recurse(src, archivequeue);
            string[] files = archivequeue.ToArray();
            for (int a = 0; a < files.Length; a++)
            {
                files[a] = files[a].Substring(src.Length).Replace('\\', '/');
            }

            int filenameslength = 0;
            long[] cLen = new long[files.Length];
            long[] eLen = new long[files.Length];
            byte[][] filename = new byte[files.Length][];
            for (int a = 0; a < files.Length; a++)
            {
                eLen[a] = new FileInfo(src + files[a]).Length;
                filename[a] = Encoding.UTF8.GetBytes(files[a]);
                filenameslength += filename[a].Length;
                bytesToCompress += eLen[a];
            }

            byte[] buffer = new byte[8192];
            byte[] header = new byte[
                            4 + // header     length
                            4 + // file       count
               files.Length * 4 + // filename   length
               files.Length * 8 + // compressed length
               files.Length * 8 + // extracted  length
                filenameslength
            ];
            for (int a = 0; a < header.Length; a++)
            {
                header[a] = 0xFF;
            }

            using (FileStream fso = new FileStream(@"..\..\tools.dfc", FileMode.Create))
            {
                encoding = true;
                fso.Write(header, 0, header.Length);
                for (int a = 0; a < files.Length; a++)
                {
                    using (FileStream fsi = new FileStream(src + files[a], FileMode.Open, FileAccess.Read))
                    {
                        long pre = fso.Position;
                        SevenZip.Compression.LZMA.Encoder enc = new SevenZip.Compression.LZMA.Encoder();
                        enc.WriteCoderProperties(fso);
                        fso.Write(BitConverter.GetBytes(fsi.Length), 0, 8);
                        enc.Code(fsi, fso, fsi.Length, -1, icp);
                        cLen[a] = fso.Position - pre;
                        bytesCompressed += icp.i;
                        icp = new ICP();
                        
                    }
                }
                using (MemoryStream msh = new MemoryStream(header.Length))
                {
                    msh.Write(BitConverter.GetBytes((Int32)header.Length), 0, 4);
                    msh.Write(BitConverter.GetBytes((Int32)files.Length), 0, 4);
                    foreach (byte[] fn in filename)
                    {
                        msh.Write(BitConverter.GetBytes((Int32)fn.Length), 0, 4);
                    }
                    foreach (long cLn in cLen)
                    {
                        msh.Write(BitConverter.GetBytes((Int64)cLn), 0, 8);
                    }
                    foreach (long eLn in eLen)
                    {
                        msh.Write(BitConverter.GetBytes((Int64)eLn), 0, 8);
                    }
                    foreach (byte[] fn in filename)
                    {
                        msh.Write(fn, 0, fn.Length);
                    }
                    fso.Seek(0, SeekOrigin.Begin);
                    msh.Seek(0, SeekOrigin.Begin);
                    msh.WriteTo(fso);
                }
            }
            encoding = false;
        }






        static byte[] md5sum(string file, int trim)
        {
            using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
            {
                Stream storm = null;
                if (trim != 0)
                {
                    //fs.SetLength(fs.Length - trim);
                    storm = new StreamTruncate(fs, trim);
                }
                else
                {
                    storm = fs;
                }
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                return md5.ComputeHash(storm);
            }
        }
        public static void coreTest()
        {
            if (Program.SIGN_BINARY)
            {
                byte[] myMD5 = md5sum(Application.ExecutablePath, 0);
                using (FileStream fso = new FileStream(Application.ExecutablePath + ".exe", FileMode.Create))
                {
                    using (FileStream fsi = new FileStream(Application.ExecutablePath, FileMode.Open, FileAccess.Read))
                    {
                        byte[] buffer = new byte[8192];
                        while (true)
                        {
                            int i = fsi.Read(buffer, 0, buffer.Length);
                            if (i <= 0) break;
                            fso.Write(buffer, 0, i);
                        }
                    }
                    fso.Write(myMD5, 0, myMD5.Length);
                }
                Program.kill();
            }
            if (Program.VERIFY_CHECKSUM)
            {
                byte[] myMD5 = md5sum(Application.ExecutablePath, 16);
                byte[] chMD5 = new byte[myMD5.Length];
                using (FileStream fs = new FileStream(Application.ExecutablePath, FileMode.Open, FileAccess.Read))
                {
                    //fs.Seek(chMD5.Length, SeekOrigin.End);
                    fs.Seek(-chMD5.Length, SeekOrigin.End);
                    fs.Read(chMD5, 0, chMD5.Length);
                }
                bool matchMD5 = true;
                for (int a = 0; a < myMD5.Length; a++)
                {
                    if (myMD5[a] != chMD5[a])
                    {
                        matchMD5 = false;
                        break;
                    }
                }
                if (!matchMD5)
                {
                    if (DialogResult.Cancel == MessageBox.Show(
                        "The application seems to be corrupted!\n\n" +
                        "You should redownload it.\n\n" +
                        "Ignore and continune?", "Bad binary",
                        MessageBoxButtons.OKCancel, MessageBoxIcon.Warning))
                    {
                        Program.kill();
                    }
                }
            }
        }
    }

    public class ICP : SevenZip.ICodeProgress
    {
        public Int64 i;
        public ICP()
        {
            i = 0;
        }
        public void SetProgress(Int64 inSize, Int64 outSize)
        {
            i = inSize;
        }
    }

    public class StreamTruncate : Stream
    {
        long len;
        Stream stream;
        public StreamTruncate(Stream stream, int trim)
        {
            len = stream.Length - trim;
            this.stream = stream;
        }
        public override int Read(byte[] buffer, int offset, int count)
        {
            long i = stream.Read(buffer, offset, count);
            if (stream.Position >= len)
            {
                i -= stream.Position - len;
            }
            return (int)Math.Max(0, i);
        }
        public override bool CanRead { get { return stream.CanRead; } }
        public override bool CanSeek { get { return stream.CanSeek; } }
        public override bool CanWrite { get { return stream.CanWrite; } }
        public override void Flush()
        {
            stream.Flush();
        }
        public override long Length { get { return len; } }
        public override long Position
        {
            get
            {
                return stream.Position;
            }
            set
            {
                stream.Position = value;
            }
        }
        public override long Seek(long offset, System.IO.SeekOrigin origin)
        {
            return stream.Seek(offset, origin);
        }
        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }
        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }
    }
}
