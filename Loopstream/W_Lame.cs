using System;
using System.Runtime.InteropServices;

namespace Loopstream
{
    public class LibMp3LameException : Exception
    {
        public LibMp3LameException(string message)
            : base(message)
        {
        }

        public LibMp3LameException(string message, LibMp3LameException innerException)
            : base(message, innerException)
        {
        }
    }

    public class LibMp3Lame : IDisposable
    {
        #region DllImport

        /* MPEG modes */
        public enum MPEG_mode
        {
            STEREO = 0,
            JOINT_STEREO,
            DUAL_CHANNEL, /* LAME doesn't supports this! */
            MONO,
            NOT_SET,
            MAX_INDICATOR /* Don't use this! It's used for sanity checks. */
        }

        // lame_global_flags * CDECL lame_init(void);
        [DllImport("lame_enc.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr lame_init();

        // int CDECL lame_init_params(lame_global_flags *);
        [DllImport("lame_enc.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern int lame_init_params(IntPtr lame_global_flags);

        // void lame_close(lame_global_flags *);
        [DllImport("lame_enc.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern void lame_close(IntPtr lame_global_flags);

        // int CDECL lame_set_mode(lame_global_flags *, MPEG_mode);
        //[DllImport("lame_enc.dll", CallingConvention = CallingConvention.Cdecl)]
        //static extern int lame_set_mode(IntPtr lame_global_flags, MPEG_mode mode);

        // int CDECL lame_set_in_samplerate(lame_global_flags *, int);
        [DllImport("lame_enc.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern int lame_set_in_samplerate(IntPtr lame_global_flags, int rateInHz);

        // int CDECL lame_set_num_channels(lame_global_flags *, int);
        [DllImport("lame_enc.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern int lame_set_num_channels(IntPtr lame_global_flags, int channels);

        /*
        * input pcm data, output (maybe) mp3 frames.
        * This routine handles all buffering, resampling and filtering for you.
        *
        * return code number of bytes output in mp3buf. Can be 0
        * -1: mp3buf was too small
        * -2: malloc() problem
        * -3: lame_init_params() not called
        * -4: psycho acoustic problems
        *
        * The required mp3buf_size can be computed from num_samples,
        * samplerate and encoding rate, but here is a worst case estimate:
        *
        * mp3buf_size in bytes = 1.25*num_samples + 7200
        *
        * I think a tighter bound could be: (mt, March 2000)
        * MPEG1:
        * num_samples*(bitrate/8)/samplerate + 4*1152*(bitrate/8)/samplerate + 512
        * MPEG2:
        * num_samples*(bitrate/8)/samplerate + 4*576*(bitrate/8)/samplerate + 256
        *
        * but test first if you use that!
        *
        * set mp3buf_size = 0 and LAME will not check if mp3buf_size is
        * large enough.
        *
        * NOTE:
        * if gfp->num_channels=2, but gfp->mode = 3 (mono), the L & R channels
        * will be averaged into the L channel before encoding only the L channel
        * This will overwrite the data in buffer_l[] and buffer_r[].
        *
        */
        // int CDECL lame_encode_buffer (
        // lame_global_flags* gfp, /* global context handle */
        // const short int buffer_l [], /* PCM data for left channel */
        // const short int buffer_r [], /* PCM data for right channel */
        // const int nsamples, /* number of samples per channel */
        // unsigned char* mp3buf, /* pointer to encoded MP3 stream */
        // const int mp3buf_size ); /* number of valid octets in this stream */
        [DllImport("lame_enc.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern int lame_encode_buffer_interleaved(IntPtr lame_global_flags,
        short[] pcm,
        int nsamples,
        IntPtr mp3buf,
        int mp3buf_size);

        // int CDECL lame_encode_flush(
        // lame_global_flags * gfp, /* global context handle */
        // unsigned char* mp3buf, /* pointer to encoded MP3 stream */
        // int size); /* number of valid octets in this stream */
        [DllImport("lame_enc.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern int lame_encode_flush(IntPtr lame_global_flags,
        IntPtr mp3buf,
        int mp3buf_size);

        #endregion

        #region Private

        IntPtr lame_global_flags;

        #endregion

        #region Public

        public void LameInit()
        {
            IntPtr ret = lame_init();
            if (ret == default(IntPtr))
                throw new LibMp3LameException(
                "Unable to create the lame struct possibly due to no memory being left");
            lame_global_flags = ret;
        }

        /*public void LameSetMode(MPEG_mode mode)
        {
            if (lame_set_mode(lame_global_flags, mode) != 0)
                throw new LibMp3LameException("lame_set_mode returned an error");
        }*/

        public void LameSetInSampleRate(int rateInHz)
        {
            if (lame_set_in_samplerate(lame_global_flags, rateInHz) != 0)
                throw new LibMp3LameException("lame_set_in_samplerate returned an error");
        }

        public void LameSetNumChannels(int numChannels)
        {
            if (lame_set_num_channels(lame_global_flags, numChannels) != 0)
                throw new LibMp3LameException("lame_set_num_channels returned an error");
        }

        public void LameInitParams()
        {
            if (lame_init_params(lame_global_flags) < 0)
                throw new LibMp3LameException("lame_init_params returned an error");
        }

        public int LameEncodeBuffer(short[] pcm,
        int nsamples, byte[] mp3Buffer)
        {
            GCHandle pinnedArray = GCHandle.Alloc(mp3Buffer, GCHandleType.Pinned);
            IntPtr p = pinnedArray.AddrOfPinnedObject();
            int ret = lame_encode_buffer_interleaved(
                lame_global_flags,
                pcm,
                nsamples,
                p,
                mp3Buffer.Length);
            
            pinnedArray.Free();
            if (ret < 0)
                throw new LibMp3LameException("lame_encode_buffer returned an error (" + ret + ")");
            return ret;
        }

        public int LameEncodeFlush(byte[] mp3Buffer)
        {
            GCHandle pinnedArray = GCHandle.Alloc(mp3Buffer, GCHandleType.Pinned);
            IntPtr p = pinnedArray.AddrOfPinnedObject();
            int ret = lame_encode_flush(lame_global_flags, p, mp3Buffer.Length);
            pinnedArray.Free();
            if (ret < 0)
                throw new LibMp3LameException("lame_encode_flush returned an error (" + ret + ")");
            return ret;
        }

        /*public int LameGetLameTagFrame(byte[] buffer)
        {
            GCHandle pinnedArray = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            IntPtr p = pinnedArray.AddrOfPinnedObject();
            int ret = lame_get_lametag_frame(lame_global_flags, p, buffer.Length);
            pinnedArray.Free();
            if (ret < 0)
                throw new LibMp3LameException("lame_encode_flush returned an error (" + ret + ")");
            if (ret > buffer.Length)
                throw new LibMp3LameException("lame_encode_flush failed due to buffer being to small"
                + " as it should have been at least " + ret
                + " bytes rather than " + buffer.Length);
            return ret;
        }*/

        public void LameClose()
        {
            lame_close(lame_global_flags);
            lame_global_flags = default(IntPtr);
        }

        public void Dispose()
        {
            if (lame_global_flags != default(IntPtr))
                LameClose();
        }

        #endregion
    }
}
