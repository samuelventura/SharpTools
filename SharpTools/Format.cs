using System;

namespace SharpTools
{
    public static class Format
    {
        public static string Apply(string format, params object[] args)
        {
            var text = format;
            if (args.Length > 0)
            {
                text = string.Format(format, args);
            }
            return text;
        }
    }
}
