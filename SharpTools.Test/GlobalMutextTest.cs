using System;
using NUnit.Framework;
using System.Threading;

namespace SharpTools
{
    [TestFixture]
    public class GlobalMutexText
    {
        [Test]
        public void FreeTest()
        {
            var lines = Program.Run("GlobalMutex testhost");

            Assert.AreEqual(1, lines.Length);
            Assert.AreEqual("Mutex Free", lines[0]);
        }

        [Test]
        public void BusyTest()
        {
            using (new Mutex(true, @"Global\testhost"))
            {
                var lines = Program.Run("GlobalMutex testhost");

                Assert.AreEqual(1, lines.Length);
                Assert.AreEqual("Mutex Busy", lines[0]);
            }
        }
    }
}