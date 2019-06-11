using System.Text;
using System.Collections.Generic;
using System.Globalization;

namespace SharpTools
{
    public static class Hexadecimal
    {
        public static string Encode(params byte[] bytes)
        {
            return Encode(bytes, 0, bytes.Length);
        }

        public static string Encode(byte[] bytes, int offset, int length)
        {
            var maximum = offset + length;
            var sb = new StringBuilder();
            for (var i = offset; i < maximum; i++)
                sb.Append(bytes[i].ToString("X2"));
            return sb.ToString();
        }

        public static string Encode(IEnumerable<byte> bytes)
        {
            var sb = new StringBuilder();
            foreach (var b in bytes)
                sb.Append(b.ToString("X2"));
            return sb.ToString();
        }

        public static byte[] Decode(string text)
        {
            var bytes = new byte[text.Length / 2];
            for (var i = 0; i < bytes.Length; i++)
            {
                var hex = text.Substring(i * 2, 2);
                bytes[i] = byte.Parse(hex, NumberStyles.HexNumber);
            }
            return bytes;
        }
    }
}
