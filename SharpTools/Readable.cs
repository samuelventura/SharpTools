using System;
using System.Collections.Generic;
using System.Text;

namespace SharpTools
{
    public static class Readable
    {
        public static string Make(IEnumerable<byte> bytes)
        {
            var sb = new StringBuilder();
            foreach (var b in bytes)
                sb.Append(Make(b));
            return sb.ToString();
        }

        public static string Make(IEnumerable<char> chers)
        {
            var sb = new StringBuilder();
            foreach (var c in chers)
                sb.Append(Make(c));
            return sb.ToString();
        }

        public static string Make(byte b)
        {
            return Make((char)b);
        }

        public static string Make(char c)
        {
            if (Char.IsControl(c) || Char.IsWhiteSpace(c))
                return string.Format("[{0:X2}]", (int)c);
            return c.ToString();
        }
    }
}
