using System;
using Xunit;

namespace SharpTools
{
    public class NamedTest
    {
        [Fact]
        public void BasicTest()
        {
            var named = new Named("Name", "Payload");

            Assert.Equal("Name", named.Name);
            Assert.Equal("Payload", named.Payload);
        }
    }
}