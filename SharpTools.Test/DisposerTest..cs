using System;
using System.Collections.Generic;
using Xunit;

namespace SharpTools
{
    public class DisposerTest
    {
        private class DisposeWithException : IDisposable
        {
            private Action action;

            public DisposeWithException(Action action = null)
            {
                this.action = action;
            }

            public void Dispose()
            {
                action?.Invoke();
                throw new Exception("Dispose Exception!");
            }
        }


        [Fact]
        public void StaticDisposeTest()
        {
            Disposer.Dispose(null);
            Disposer.Dispose(new DisposeWithException());
        }


        [Fact]
        public void InstanceDisposeTest()
        {
            var strings = new List<string>();

            var disposer = new Disposer(() => { strings.Add("First"); });
            disposer.Add(() => { strings.Add("Second"); });
            disposer.Add(null as Action);
            disposer.Add(null as IDisposable);
            disposer.Add(new DisposeWithException(() => { strings.Add("Third"); }));
            disposer.Dispose();

            Assert.Equal(3, strings.Count);
            Assert.Equal("Third", strings[0]);
            Assert.Equal("Second", strings[1]);
            Assert.Equal("First", strings[2]);
        }
    }
}