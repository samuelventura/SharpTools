using System;
using System.Collections.Generic;
using System.Threading;

namespace SharpTools
{
    public class ThreadRunner : IDisposable
    {
        private readonly Action<Exception> catcher;
        private readonly Queue<Action> queue;
        private readonly Thread thread;
        private readonly Action idle;
        private volatile bool disposed;

        public ThreadRunner(string name = null, Action<Exception> catcher = null, Action idle = null)
        {
            this.catcher = catcher;
            this.idle = idle;

            queue = new Queue<Action>();

            thread = new Thread(Loop);
            thread.IsBackground = true;
            thread.Name = name;
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
                if (idle == null)
                {
                    var action = idle;

                    lock (queue)
                    {
                        if (queue.Count == 0)
                        {
                            Monitor.Wait(queue);
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
                else
                {
                    var action = idle;

                    lock (queue)
                    {
                        if (queue.Count > 0)
                        {
                            action = queue.Dequeue();
                        }
                    }

                    Catcher.Try(action, catcher);
                }
            }
        }
    }
}