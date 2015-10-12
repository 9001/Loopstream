using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Loopstream
{
    public class Chencode
    {
        /// <summary>
        /// Perform RFC 3986 Percent-encoding on a string.
        /// </summary>
        /// <param name="input">The input string</param>
        /// <returns>The RFC 3986 Percent-encoded string</returns>
        public static string HonkHonk(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            return Encoding.ASCII.GetString(EncodeToBytes(input, Encoding.UTF8));
        }

        private static byte[] EncodeToBytes(string input, Encoding enc)
        {
            if (string.IsNullOrEmpty(input))
                return new byte[0];

            byte[] inbytes = enc.GetBytes(input);

            // Count unsafe characters
            int unsafeChars = 0;
            char c;
            foreach (byte b in inbytes)
            {
                c = (char)b;

                if (NeedsEscaping(c))
                    unsafeChars++;
            }

            // Check if we need to do any encoding
            if (unsafeChars == 0)
                return inbytes;

            byte[] outbytes = new byte[inbytes.Length + (unsafeChars * 2)];
            int pos = 0;

            for (int i = 0; i < inbytes.Length; i++)
            {
                byte b = inbytes[i];

                if (NeedsEscaping((char)b))
                {
                    outbytes[pos++] = (byte)'%';
                    outbytes[pos++] = (byte)IntToHex((b >> 4) & 0xf);
                    outbytes[pos++] = (byte)IntToHex(b & 0x0f);
                }
                else
                    outbytes[pos++] = b;
            }

            return outbytes;
        }

        private static bool NeedsEscaping(char c)
        {
            return !((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || (c >= '0' && c <= '9')
                     || c == '-' || c == '_' || c == '.' || c == '~');
        }

        private static char IntToHex(int n)
        {
            if (n < 0 || n >= 16)
                throw new ArgumentOutOfRangeException("n");

            if (n <= 9)
                return (char)(n + (int)'0');
            else
                return (char)(n - 10 + (int)'A');
        }

    }
}
