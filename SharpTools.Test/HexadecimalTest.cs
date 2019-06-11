using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace SharpTools
{
    [TestFixture]
    public class HexadecimalTest
    {
        [Test]
        public void BasicTest()
        {
            Assert.AreEqual("010203", Hexadecimal.Encode(1, 2, 3));
            Assert.AreEqual("010203", Hexadecimal.Encode(new byte[] { 1, 2, 3 }));
            Assert.AreEqual("010203", Hexadecimal.Encode(new byte[] { 1, 2, 3 }, 0, 3));
            Assert.AreEqual("010203", Hexadecimal.Encode(new List<byte>(new byte[] { 1, 2, 3 })));
            Assert.AreEqual(new byte[] { 1, 2, 3 }, Hexadecimal.Decode("010203"));
            Assert.AreEqual(new byte[] { 1, 2, 3 }, Hexadecimal.Decode("010203x"));
        }
    }
}