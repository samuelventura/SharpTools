using System;
using System.Threading;

namespace SharpTools
{
    public class GlobalMutex
    {
        private readonly string name;
        private readonly Action catcher;

        public GlobalMutex(string name, Action catcher)
        {
            this.name = name;
            this.catcher = catcher;
        }

        public void Run(Action action)
        {
            using (var mutex = new Mutex(false, @"Global\" + name))
            {
                if (mutex.WaitOne(0)) action();
                else catcher();
            }
        }
    }
}
