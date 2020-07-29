using Microsoft.Win32.SafeHandles;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Xml.Serialization;

namespace Loopstream
{
    public class LSWavetail : LSAudioSrc
    {
        public string id { get; set; }
        public string name { get; set; }
        
        [XmlIgnore]
        public NAudio.Wave.WaveFormat wf { get; set; }

        public int samplerate;
        public int bitness;
        public int chans;

        public string srcdir;
        public int delete;
        
        public LSWavetail()
        {
            srcdir = "";
            id = "wavetailer";
            samplerate = 44100;
            bitness = 16;
            chans = 2;
            delete = 10;
            wf = null;
        }

        public void setFormat(int samplerate, int bitness, int chans)
        {
            this.samplerate = samplerate;
            this.bitness = bitness;
            this.chans = chans;
        }

        public void setFormat()
        {
            //wf = new WaveFormat(samplerate, bitness, chans);
            wf = WaveFormat.CreateIeeeFloatWaveFormat(samplerate, chans);
        }
        
        public override string ToString()
        {
            return string.Format("Wavetailer: " + (this.srcdir == "" ? "(unconfigured)" : this.srcdir));
        }
    }

    public class LSWavetailDev : IWaveIn, IDisposable
    {
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern SafeFileHandle CreateFile(
           string lpFileName,
           EFileAccess dwDesiredAccess,
           EFileShare dwShareMode,
           IntPtr lpSecurityAttributes,
           ECreationDisposition dwCreationDisposition,
           EFileAttributes dwFlagsAndAttributes,
           IntPtr hTemplateFile);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool CloseHandle(SafeFileHandle hObject);

        [Flags]
        enum EFileAccess : uint
        {
            AccessSystemSecurity = 0x1000000,   // AccessSystemAcl access type
            MaximumAllowed = 0x2000000,         // MaximumAllowed access type

            Delete = 0x10000,
            ReadControl = 0x20000,
            WriteDAC = 0x40000,
            WriteOwner = 0x80000,
            Synchronize = 0x100000,

            StandardRightsRequired = 0xF0000,
            StandardRightsRead = ReadControl,
            StandardRightsWrite = ReadControl,
            StandardRightsExecute = ReadControl,
            StandardRightsAll = 0x1F0000,
            SpecificRightsAll = 0xFFFF,

            FILE_READ_DATA = 0x0001,            // file & pipe
            FILE_LIST_DIRECTORY = 0x0001,       // directory
            FILE_WRITE_DATA = 0x0002,           // file & pipe
            FILE_ADD_FILE = 0x0002,             // directory
            FILE_APPEND_DATA = 0x0004,          // file
            FILE_ADD_SUBDIRECTORY = 0x0004,     // directory
            FILE_CREATE_PIPE_INSTANCE = 0x0004, // named pipe
            FILE_READ_EA = 0x0008,              // file & directory
            FILE_WRITE_EA = 0x0010,             // file & directory
            FILE_EXECUTE = 0x0020,              // file
            FILE_TRAVERSE = 0x0020,             // directory
            FILE_DELETE_CHILD = 0x0040,         // directory
            FILE_READ_ATTRIBUTES = 0x0080,      // all
            FILE_WRITE_ATTRIBUTES = 0x0100,     // all

            GenericRead = 0x80000000,
            GenericWrite = 0x40000000,
            GenericExecute = 0x20000000,
            GenericAll = 0x10000000,

            SPECIFIC_RIGHTS_ALL = 0x00FFFF,
            FILE_ALL_ACCESS = StandardRightsRequired | Synchronize | 0x1FF,
            FILE_GENERIC_READ = StandardRightsRead | FILE_READ_DATA | FILE_READ_ATTRIBUTES | FILE_READ_EA | Synchronize,
            FILE_GENERIC_WRITE = StandardRightsWrite | FILE_WRITE_DATA | FILE_WRITE_ATTRIBUTES | FILE_WRITE_EA | FILE_APPEND_DATA | Synchronize,
            FILE_GENERIC_EXECUTE = StandardRightsExecute | FILE_READ_ATTRIBUTES | FILE_EXECUTE | Synchronize
        }

        [Flags]
        public enum EFileShare : uint
        {
            /// <summary>
            ///
            /// </summary>
            None = 0x00000000,
            /// <summary>
            /// Enables subsequent open operations on an object to request read access.
            /// Otherwise, other processes cannot open the object if they request read access.
            /// If this flag is not specified, but the object has been opened for read access, the function fails.
            /// </summary>
            Read = 0x00000001,
            /// <summary>
            /// Enables subsequent open operations on an object to request write access.
            /// Otherwise, other processes cannot open the object if they request write access.
            /// If this flag is not specified, but the object has been opened for write access, the function fails.
            /// </summary>
            Write = 0x00000002,
            /// <summary>
            /// Enables subsequent open operations on an object to request delete access.
            /// Otherwise, other processes cannot open the object if they request delete access.
            /// If this flag is not specified, but the object has been opened for delete access, the function fails.
            /// </summary>
            Delete = 0x00000004
        }

        public enum ECreationDisposition : uint
        {
            /// <summary>
            /// Creates a new file. The function fails if a specified file exists.
            /// </summary>
            New = 1,
            /// <summary>
            /// Creates a new file, always.
            /// If a file exists, the function overwrites the file, clears the existing attributes, combines the specified file attributes,
            /// and flags with FILE_ATTRIBUTE_ARCHIVE, but does not set the security descriptor that the SECURITY_ATTRIBUTES structure specifies.
            /// </summary>
            CreateAlways = 2,
            /// <summary>
            /// Opens a file. The function fails if the file does not exist.
            /// </summary>
            OpenExisting = 3,
            /// <summary>
            /// Opens a file, always.
            /// If a file does not exist, the function creates a file as if dwCreationDisposition is CREATE_NEW.
            /// </summary>
            OpenAlways = 4,
            /// <summary>
            /// Opens a file and truncates it so that its size is 0 (zero) bytes. The function fails if the file does not exist.
            /// The calling process must open the file with the GENERIC_WRITE access right.
            /// </summary>
            TruncateExisting = 5
        }

        [Flags]
        public enum EFileAttributes : uint
        {
            Readonly = 0x00000001,
            Hidden = 0x00000002,
            System = 0x00000004,
            Directory = 0x00000010,
            Archive = 0x00000020,
            Device = 0x00000040,
            Normal = 0x00000080,
            Temporary = 0x00000100,
            SparseFile = 0x00000200,
            ReparsePoint = 0x00000400,
            Compressed = 0x00000800,
            Offline = 0x00001000,
            NotContentIndexed = 0x00002000,
            Encrypted = 0x00004000,
            Write_Through = 0x80000000,
            Overlapped = 0x40000000,
            NoBuffering = 0x20000000,
            RandomAccess = 0x10000000,
            SequentialScan = 0x08000000,
            DeleteOnClose = 0x04000000,
            BackupSemantics = 0x02000000,
            PosixSemantics = 0x01000000,
            OpenReparsePoint = 0x00200000,
            OpenNoRecall = 0x00100000,
            FirstPipeInstance = 0x00080000
        }

        public WaveFormat WaveFormat { get; set; }
        public LSWavetail cfg;

        public event EventHandler<WaveInEventArgs> DataAvailable;
        public event EventHandler<StoppedEventArgs> RecordingStopped;

        bool stopping;
        Thread thr;

        public LSWavetailDev(LSWavetail cfg)
        {
            this.cfg = cfg;
            this.WaveFormat = cfg.wf;
            stopping = false;
            thr = null;
        }

        public void Dispose()
        {
            StopRecording();
        }

        public void StopRecording()
        {
            stopping = true;
            if (thr != null)
                thr.Join();
        }

        public void StartRecording()
        {
            if (thr != null)
                return;

            thr = new Thread(new ThreadStart(work));
            thr.Start();
        }

        System.IO.FileInfo[] getwavs()
        {
            var ret = new List<System.IO.FileInfo>();
            foreach (var file in System.IO.Directory.GetFiles(cfg.srcdir))
            {
                if (!file.EndsWith(".wav") && !file.EndsWith(".rbrec"))
                    continue;

                var fi = new System.IO.FileInfo(file);
                if (fi.Length > 44)
                    ret.Add(fi);
            }
            return ret.OrderByDescending(x => x.LastWriteTimeUtc).ToArray();
        }

        void work()
        {
            string last_path = null;
            long ofs = 0;
            string prev_path = null;
            long prev_depart_at = 0;
            var first_open = false;
            
            int samplerate = cfg.samplerate;
            int quant = (cfg.bitness * cfg.chans) / 8;

            while (!stopping)
            {
                var files = getwavs();
                if (files.Length == 0)
                {
                    Logger.wt.a("input folder empty");
                    Thread.Sleep(300);
                    continue;
                }

                var now = DateTime.UtcNow.Ticks / 10000000;
                string path = "";
                long sz = -1;
                
                bool resume = false;
                foreach (var file in files)
                {
                    if (file.Name == last_path && file.Length > ofs)
                    {
                        path = file.FullName;
                        sz = file.Length;
                        resume = true;
                        break;
                    }
                }

                if (!resume)
                {
                    for (int a = 2; a < files.Length; a++)
                    {
                        Logger.wt.a("deleting " + files[a].Name);
                        try
                        {
                            System.IO.File.Delete(files[a].FullName);
                        }
                        catch { }
                        prev_path = null;
                    }
                    path = files[0].FullName;
                    sz = files[0].Length;
                }

                if (last_path != path)
                {
                    Logger.wt.a("open " + path);
                    prev_depart_at = now;
                    prev_path = last_path;
                }

                if (prev_path != null && now - prev_depart_at > cfg.delete)
                {
                    Logger.wt.a("deleting " + prev_path);
                    try
                    {
                        System.IO.File.Delete(prev_path);
                    }
                    catch { }
                    prev_path = null;
                }

                if (path == last_path)
                {
                    if (sz > ofs)
                        Thread.Sleep(50);
                    else
                        Thread.Sleep(100);
                }
                else
                {
                    last_path = path;
                    var latency = 10;  // 1/latency seconds
                    ofs = 44 + (long)(Math.Max(0, sz - (long)(samplerate * quant / latency)) / quant) * quant;
                    var bufsz_seek = ofs / (samplerate * 1.0 * quant);
                    var bufsz_start = sz / (samplerate * 1.0 * quant);

                    string msg = "SKIPPING!";

                    // do not skip start of file after a split
                    // unless we are more than 3sec late
                    if (!first_open && ofs < samplerate * quant * 3)
                    {
                        msg = "playing from start";
                        ofs = 44;
                    }
                    Logger.wt.a(string.Format("bufsz {0:#.##} / {1:#.##}, {2}", bufsz_start, bufsz_seek, msg));
                }

                //var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
                var fh = CreateFile(path, EFileAccess.GenericRead, EFileShare.Read | EFileShare.Write, IntPtr.Zero, ECreationDisposition.OpenExisting, EFileAttributes.SequentialScan, IntPtr.Zero);
                if (fh.IsInvalid)
                {
                    Logger.wt.a("failed to open " + path + ", ofs " + ofs + ", sz " + sz);
                    Thread.Sleep(50);
                    continue;
                }
                var fs = new FileStream(fh, FileAccess.Read);

                int nreads_sec = 8;
                var rbuf = new byte[((samplerate * quant / nreads_sec) / quant) * quant];
                var fbuf = new float[samplerate * cfg.chans / nreads_sec];
                var bbuf = new byte[fbuf.Length * sizeof(float)];
                while (!stopping)
                {
                    //sz = new System.IO.FileInfo(path).Length;
                    //if (sz < ofs + 1)
                    //    break;
                    //
                    //int nr;
                    //using (var mmf = MemoryMappedFile.CreateFromFile(path, FileMode.Open, "thx_ms", 0, MemoryMappedFileAccess.Read))
                    //    using (var mvs = mmf.CreateViewStream(ofs, sz, MemoryMappedFileAccess.Read))
                    //        nr = mvs.Read(rbuf, 0, (int)Math.Min(rbuf.Length, sz - ofs));

                    fs.Position = ofs;
                    var nr = fs.Read(rbuf, 0, rbuf.Length);
                    nr = (nr / quant) * quant;
                    if (nr <= 0)
                        break;

                    ofs += nr;
                    if (cfg.bitness == 16)
                    {
                        for (int a = 0; a < nr; a += 2)
                        {
                            //fbuf[a / 2] = rbuf[a] | rbuf[a + 1] << 8;
                            var b = rbuf[a + 1];
                            fbuf[a / 2] = (rbuf[a] | (b < 128 ? b : b - 256) << 8) / (256f * 256);
                        }
                        nr *= sizeof(float) / (cfg.bitness / 8);
                        Buffer.BlockCopy(fbuf, 0, bbuf, 0, nr);
                    }
                    else throw new Exception("just 16-bit for now sorry");

                    try
                    {
                        Logger.wt.a("send " + nr + " bytes");
                        DataAvailable(this, new WaveInEventArgs(bbuf, nr));
                    }
                    catch
                    {
                        Logger.wt.a("we too fast");
                    }
                }
                fs.Dispose();
                //CloseHandle(fh);  // seems to be handled by FileStream
            }
        }
    }
}
