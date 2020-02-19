using System;
using System.Threading;
using Xunit;

namespace SharpTools
{
    public class GlobalMutexText
    {
        [Fact]
        public void FreeTest()
        {
            var lines = Program.Run("GlobalMutex testhost");

            Assert.Equal(1, lines.Length);
            Assert.Equal("Mutex Free", lines[0]);
        }

        [Fact]
        public void BusyTest()
        {
            using (new Mutex(true, @"Global\testhost"))
            {
                var lines = Program.Run("GlobalMutex testhost");

                Assert.Equal(1, lines.Length);
                Assert.Equal("Mutex Busy", lines[0]);
            }
        }
    }
}