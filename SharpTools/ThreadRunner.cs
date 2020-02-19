using System;
using System.Collections.Generic;
using System.Threading;

namespace SharpTools
{
    public class ThreadRunner : IDisposable
    {
        public class Args
        {
            public string ProcessName;
            public int IdleDelay;
            public Action IdleAction;
            public Action<Exception> ExceptionHandler;
        }

        private readonly Action<Exception> catcher;
        private readonly Queue<Action> queue;
        private readonly Thread thread;
        private readonly Action idle;
        private readonly int delay;
        private volatile bool disposed;

        public ThreadRunner(Args args = null)
        {
            args = args ?? new Args();
            this.catcher = args.ExceptionHandler;
            this.idle = args.IdleAction;
            this.delay = Math.Max(0, args.IdleDelay);
            if (this.idle == null) this.delay = -1;

            queue = new Queue<Action>();

            thread = new Thread(Loop);
            thread.IsBackground = true;
            thread.Name = args.ProcessName;
            thread.Start();
        }

        public void Dispose(Action action)
        {
            Run(() => { disposed = true; action(); });
            thread.Join();
        }

        public void Dispose()
        {
            Run(() => { disposed = true; });
            thread.Join();
        }

        public void Run(Action action)
        {
            lock (queue)
            {
                queue.Enqueue(action);
                Monitor.Pulse(queue);
            }
        }

        private void Loop()
        {
            while (!disposed)
            {
                var action = idle;

                lock (queue)
                {
                    if (queue.Count == 0)
                    {
                        Monitor.Wait(queue, delay);
                    }
                    if (queue.Count > 0)
                    {
                        action = queue.Dequeue();
                    }
                }

                if (action != null)
                {
                    Catcher.Try(action, catcher);
                }
            }
        }
    }
}