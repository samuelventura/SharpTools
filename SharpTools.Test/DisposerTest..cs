using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace SharpTools
{
    [TestFixture]
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


        [Test]
        public void StaticDisposeTest()
        {
            Disposer.Dispose(null);
            Disposer.Dispose(new DisposeWithException());
        }


        [Test]
        public void InstanceDisposeTest()
        {
            var strings = new List<string>();

            var disposer = new Disposer(() => { strings.Add("First"); });
            disposer.Add(() => { strings.Add("Second"); });
            disposer.Add(null as Action);
            disposer.Add(null as IDisposable);
            disposer.Add(new DisposeWithException(() => { strings.Add("Third"); }));
            disposer.Dispose();

            Assert.AreEqual(3, strings.Count);
            Assert.AreEqual("Third", strings[0]);
            Assert.AreEqual("Second", strings[1]);
            Assert.AreEqual("First", strings[2]);
        }
    }
}