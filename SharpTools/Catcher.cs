using System;

namespace SharpTools
{
    public class Catcher
    {
        private readonly Action<Exception> handler;

        public Catcher(Action<Exception> handler = null)
        {
            this.handler = handler;
        }

        public void Run(Action action)
        {
            Try(action, handler);
        }

        public static void Try(Action action, Action<Exception> handler = null)
        {
            try
            {
                action?.Invoke();
            }
            catch (Exception ex)
            {
                if (handler != null)
                {
                    Try(() => { handler(ex); });
                }
            }
        }
    }
}
