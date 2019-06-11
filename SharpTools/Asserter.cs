
using System;

namespace SharpTools
{
    public static class Asserter
    {
        public static void IsTrue(bool condition, string format, params object[] args)
        {
            if (!condition) Thrower.Throw(format, args);
        }

        public static void IsFalse(bool condition, string format, params object[] args)
        {
            if (condition) Thrower.Throw(format, args);
        }
    }
}
