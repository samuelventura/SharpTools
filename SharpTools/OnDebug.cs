using System;

namespace SharpTools
{
    public static class OnDebug
    {
        public static void Run(Action action)
        {
#if DEBUG
            action();
#endif
        }
    }
}
