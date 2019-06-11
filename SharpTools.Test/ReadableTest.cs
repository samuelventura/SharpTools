using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace SharpTools
{
    [TestFixture]
    public class ReadableTest
    {
        [Test]
        public void BasicTest()
        {
            Assert.AreEqual("1", Readable.Make('1'));
            Assert.AreEqual("[09]", Readable.Make('\t'));
            Assert.AreEqual("[01]", Readable.Make(1));
            Assert.AreEqual("[09][20][0D][20][0A][20][00]", Readable.Make("\t \r \n \0"));
            Assert.AreEqual("[09][20][0D][20][0A][20][00]", Readable.Make(new byte[] { 9, 32, 13, 32, 10, 32, 0 }));
        }
    }
}