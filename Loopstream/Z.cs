using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Loopstream
{
    public class EnumHandles
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);
        delegate bool EnumWindowsProc(int hwnd, int lParam);

        static bool blocker = false;
        static List<int> list = null;
        public static bool Hadd(int hwnd, int lParam)
        {
            list.Add(hwnd);
            return true;
        }
        public static int[] Run()
        {
            if (blocker) return new int[0];
            blocker = true;
            if (list != null) list.Clear();
            else list = new List<int>();
            EnumWindowsProc enumWindowsProc = new EnumWindowsProc(EnumHandles.Hadd);
            EnumWindows(enumWindowsProc, IntPtr.Zero);
            int[] ret = list.ToArray();
            list.Clear();
            blocker = false;
            return ret;
        }
    }

    public class Z
    {
        public static string lze(string plain)
        {
            return Convert.ToBase64String(lze(plain, true));
        }

        public static byte[] lze(string plain, bool fjdsuioafuweabnfowa)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(plain);
            MemoryStream msi = new MemoryStream(bytes);
            MemoryStream mso = new MemoryStream();
            SevenZip.Compression.LZMA.Encoder enc = new SevenZip.Compression.LZMA.Encoder();
            enc.WriteCoderProperties(mso);
            mso.Write(BitConverter.GetBytes(msi.Length), 0, 8);
            enc.Code(msi, mso, msi.Length, -1, null);
            return mso.ToArray();
        }

        public static string lzd(string b64)
        {
            return lzd(Convert.FromBase64String(b64));
        }

        public static string lzd(byte[] bytes)
        {
            MemoryStream msi = new MemoryStream(bytes);
            MemoryStream mso = new MemoryStream();
            SevenZip.Compression.LZMA.Decoder dec = new SevenZip.Compression.LZMA.Decoder();
            byte[] props = new byte[5]; msi.Read(props, 0, 5);
            byte[] length = new byte[8]; msi.Read(length, 0, 8);
            long len = BitConverter.ToInt64(length, 0);
            dec.SetDecoderProperties(props);
            dec.Code(msi, mso, msi.Length, len, null);
            bytes = mso.ToArray();
            return Encoding.UTF8.GetString(bytes);
        }

        public static string gze(string plain)
        {
            return Convert.ToBase64String(gze(plain, true));
        }

        public static byte[] gze(string plain, bool jfiodsajfiwoabnwfe)
        {
            using (var msi = new MemoryStream(Encoding.UTF8.GetBytes(plain)))
            {
                using (var mso = new MemoryStream())
                {
                    using (var gz = new System.IO.Compression.GZipStream(mso, System.IO.Compression.CompressionMode.Compress))
                    {
                        msi.CopyTo(gz);
                        gz.Close();
                        return mso.ToArray();
                    }
                }
            }
        }

        public static string gzd(string b64)
        {
            return gzd(Convert.FromBase64String(b64));
        }
        
        public static string gzd(byte[] input)
        {
            using (var msi = new MemoryStream(input))
            {
                using (var gz = new System.IO.Compression.GZipStream(msi, System.IO.Compression.CompressionMode.Decompress))
                {
                    using (var mso = new MemoryStream())
                    {
                        gz.CopyTo(mso);
                        gz.Close();
                        byte[] bytes = mso.ToArray();
                        return Encoding.UTF8.GetString(bytes);
                    }
                }
            }
        }
    }

    public class Skinner
    {
        public static List<Control> controls = new List<Control>();
        public static void init()
        {
            //controls = new List<Control>();
        }
        public static void add(Control c)
        {
            lock (controls)
            {
                controls.Add(c);
            }
        }
        public static void rem(Control c)
        {
            lock (controls)
            {
                controls.Remove(c);
            }
        }
    }

    public class Logger
    {
        public static Logger mp3, ogg, pcm, med, mix, tag, app;
        public static List<double> bitratem, bitrateo;
        public static void init()
        {
            bitratem = new List<double>();
            bitrateo = new List<double>();
            pcm = new Logger();
            med = new Logger();
            mix = new Logger();
            mp3 = new Logger();
            ogg = new Logger();
            tag = new Logger();
            app = new Logger();
        }
        public long i, msg;
        
        Entry[] buf;
        public class Entry
        {
            public long tick;
            public long num;
            public string msg;
            public Entry()
            {
                tick = num = -1;
                msg = "";
            }
        }
        
        object locker;
        public Logger()
        {
            locker = new object();
            
            i = msg = 0;
            buf = new Entry[1024];
            for (int a = 0; a < buf.Length; a++)
            {
                buf[a] = new Entry();
            }
        }
        public override string ToString()
        {
            Entry e;
            lock (locker)
            {
                e = buf[i];
            }
            if (e.tick <= 0) return "not active";
            return new DateTime(e.tick, DateTimeKind.Utc)
                .ToString("yyyy-MM-dd HH:mm:ss.ffff") +
                " " + e.num.ToString().PadLeft(7) +
                " | " + e.msg;
        }
        public void a(string text)
        {
            lock (locker)
            {
                if (text == buf[i].msg)
                {
                    buf[i].num = ++msg;
                    return;
                }
                if (++i >= buf.Length) i = 0;
                Entry e = buf[i];
                e.msg = text;
                e.num = ++msg;
                e.tick = DateTime.UtcNow.Ticks;
            }
        }
        public string compile()
        {
            StringBuilder ret = new StringBuilder();
            lock (locker)
            {
                long o = i;
                while (true)
                {
                    if (buf[i].num >= 0)
                    {
                        ret.AppendLine(ToString());
                    }
                    if (--i < 0) i = buf.Length - 1;
                    if (i == o) break;
                }
            }
            return ret.ToString();
        }
    }
    public class LSMem
    {
        [Flags]
        public enum ProcessAccessFlags : uint
        {
            All = 0x001f0fff,
            Terminate = 0x00000001,
            CreateThread = 0x00000002,
            VMOperation = 0x00000008,
            VMRead = 0x00000010,
            VMWrite = 0x00000020,
            DupHandle = 0x00000040,
            SetInformation = 0x00000200,
            QueryInformation = 0x00000400,
            Synchronize = 0x00100000,
        };

        [DllImport("kernel32.dll")]
        private static extern IntPtr OpenProcess(ProcessAccessFlags dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, int dwProcessId);
        [DllImport("kernel32.dll")]
        private static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, out int lpNumberOfBytesWritten);
        [DllImport("kernel32.dll")]
        private static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [Out] byte[] lpBuffer, int dwSize, out int lpNumberOfBytesWritten);
        [DllImport("kernel32.dll")]
        private static extern IntPtr CloseHandle(IntPtr hProcess);

        IntPtr handle;

        public LSMem(Process proc)
        {
            handle = OpenProcess(ProcessAccessFlags.VMRead, false, proc.Id);
        }

        public void Dispose()
        {
            CloseHandle(handle);
        }

        public int read(IntPtr adr, byte[] buf)
        {
            int ret = -1;
            ReadProcessMemory(handle, adr, buf, sizeof(byte) * buf.Length, out ret);
            return ret;
        }

        public int read(IntPtr adr, byte[] buf, int[] ofs)
        {
            byte[] u32 = new byte[4];
            foreach (int o in ofs)
            {
                int i = read(adr, u32);
                if (i < u32.Length) return -1;
                adr = new IntPtr(BitConverter.ToUInt32(u32, 0) + o);
            }
            return read(adr, buf);
        }
    }

    public class WinapiShit
    {
        [DllImport("user32.dll")]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
        public static uint getProcId(IntPtr windowHandle)
        {
            uint ret = 0;
            GetWindowThreadProcessId(windowHandle, out ret);
            return ret;
        }

        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        static extern int GetWindowTextW(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        static extern int GetWindowTextLengthW(IntPtr hWnd);
        public static string getWinText(IntPtr windowHandle)
        {
            int len = GetWindowTextLengthW(windowHandle);
            if (len <= 1 || len > 1020) return null;
            StringBuilder ret = new StringBuilder(len + 1);
            GetWindowTextW(windowHandle, ret, ret.Capacity);
            return ret.ToString();
        }

        [DllImport("user32.dll")]
        static extern int SetWindowPos(IntPtr hWnd, IntPtr hWndAfter, int x, int y, int cx, int cy, uint uFags);
        public static void topmost(IntPtr hWnd)
        {
            UInt32 nosize = 0x0001;
            UInt32 nomove = 0x0002;
            UInt32 doshow = 0x0040;
            IntPtr topmost = new IntPtr(-1);
            SetWindowPos(hWnd, topmost, 0, 0, 0, 0, nomove | nosize | doshow);
        }
    }
}
