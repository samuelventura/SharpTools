using System;

namespace SharpTools
{
    public class Atomic
    {
        private readonly object locker = new Object();
        private readonly Action<Exception> handler;

        public Atomic(Action<Exception> handler = null)
        {
            this.handler = handler;
        }

        public void Run(Action action)
        {
            Run(locker, action, handler);
        }

        public static void Run(
            object locker,
            Action action,
            Action<Exception> handler = null
            )
        {
            lock (locker) Catcher.Try(action, handler);
        }
    }
}
