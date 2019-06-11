using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace SharpTools
{
    public class Disposer : IDisposable
    {
        private readonly Stack<Action> actions;

        public Disposer(params Action[] actions)
        {
            this.actions = new Stack<Action>(actions);
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
                Catcher.Try(actions.Pop());
            }
        }

        public static void Dispose(IDisposable disposable)
        {
            Catcher.Try(() => { disposable?.Dispose(); });
        }
    }
}
