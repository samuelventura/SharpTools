using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;

namespace SharpTools
{
    [TestFixture]
    public class ThreadRunnerTest
    {
        [Test]
        public void IdleTest()
        {
            var errors = new List<Exception>();
            var flag = new AutoResetEvent(false);

            var runner = new ThreadRunner("TEST", (ex) =>
            {
                errors.Add(ex);
                flag.Set();
            }, () =>
            {
                throw new Exception("Idle");
            });
            flag.WaitOne();
            runner.Dispose();

            Assert.IsTrue(errors.Count > 0);
            Assert.AreEqual("Idle", errors[0].Message);
        }

        [Test]
        public void CatcherTest()
        {
            var errors = new List<Exception>();

            var runner = new ThreadRunner("TEST", (ex) =>
            {
                errors.Add(ex);
                throw new Exception("Catcher");
            });
            runner.Run(() => { throw new Exception("First"); });
            runner.Run(() => { throw new Exception("Second"); });
            runner.Dispose();

            Assert.AreEqual(2, errors.Count);
            Assert.AreEqual("First", errors[0].Message);
            Assert.AreEqual("Second", errors[1].Message);
        }

        [Test]
        public void DisposeUsingTest()
        {
            var errors = new List<Exception>();

            using (var runner = new ThreadRunner("TEST", (ex) => { errors.Add(ex); }))
            {
                runner.Run(() => { throw new Exception("First"); });
            }

            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("First", errors[0].Message);
        }

        [Test]
        public void DisposeExplicitTest()
        {
            var errors = new List<Exception>();

            var runner = new ThreadRunner("TEST", (ex) => { errors.Add(ex); });
            runner.Run(() => { throw new Exception("First"); });
            runner.Dispose();
            runner.Dispose(() => { throw new Exception("Already Disposed"); });

            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("First", errors[0].Message);
        }

        [Test]
        public void DisposeWithActionTest()
        {
            var errors = new List<Exception>();

            var runner = new ThreadRunner("TEST", (ex) => { errors.Add(ex); });
            runner.Run(() => { throw new Exception("First"); });
            runner.Dispose(() => { throw new Exception("Second"); });
            runner.Dispose(() => { throw new Exception("Already Disposed"); });

            Assert.AreEqual(2, errors.Count);
            Assert.AreEqual("First", errors[0].Message);
            Assert.AreEqual("Second", errors[1].Message);
        }
    }
}