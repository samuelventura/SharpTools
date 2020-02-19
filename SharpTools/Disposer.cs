using System;
using System.Collections.Generic;

namespace SharpTools
{
    public class Disposer : IDisposable
    {
        private readonly Action<Exception> handler;
        private readonly Stack<Action> actions;

        public Disposer(Action<Exception> handler = null)
        {
            this.handler = handler;
            this.actions = new Stack<Action>();
        }

        public Disposer Clear()
        {
            var array = actions.ToArray();
            actions.Clear();
            Array.Reverse(array);
            var disposer = new Disposer();
            foreach (var action in array)
            {
                disposer.actions.Push(action);
            }
            return disposer;
        }

        public void Add(IDisposable disposable)
        {
            actions.Push(() => { disposable?.Dispose(); });
        }

        public void Add(Action action)
        {
            actions.Push(action);
        }

        public void Dispose()
        {
            while (actions.Count > 0)
            {
                Catcher.Try(actions.Pop(), handler);
            }
        }

        public static void Dispose(IDisposable disposable, Action<Exception> handler = null)
        {
            Catcher.Try(() => { disposable?.Dispose(); }, handler);
        }
    }
}
