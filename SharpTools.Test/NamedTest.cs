using System;
using NUnit.Framework;

namespace SharpTools
{
    [TestFixture]
    public class NamedTest
    {
        [Test]
        public void BasicTest()
        {
            var named = new Named("Name", "Payload");

            Assert.AreEqual("Name", named.Name);
            Assert.AreEqual("Payload", named.Payload);
        }
    }
}