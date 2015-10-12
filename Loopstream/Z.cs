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
    public class Logger
    {
        public static Logger mp3, ogg, pcm, med, mix, tag, app;
        public static List<double> bitrate;
        public static void init()
        {
            bitrate = new List<double>();
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
}
