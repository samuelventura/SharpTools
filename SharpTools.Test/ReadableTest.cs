using System;
using System.Collections.Generic;
using Xunit;

namespace SharpTools
{
    public class ReadableTest
    {
        [Fact]
        public void BasicTest()
        {
            Assert.Equal("1", Readable.Make('1'));
            Assert.Equal("[09]", Readable.Make('\t'));
            Assert.Equal("[01]", Readable.Make(1));
            Assert.Equal("[09][20][0D][20][0A][20][00]", Readable.Make("\t \r \n \0"));
            Assert.Equal("[09][20][0D][20][0A][20][00]", Readable.Make(new byte[] { 9, 32, 13, 32, 10, 32, 0 }));
        }
    }
}