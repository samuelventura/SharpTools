using System;
using System.Collections.Generic;
using Xunit;

namespace SharpTools
{
    public class HexadecimalTest
    {
        [Fact]
        public void BasicTest()
        {
            Assert.Equal("010203", Hexadecimal.Encode(1, 2, 3));
            Assert.Equal("010203", Hexadecimal.Encode(new byte[] { 1, 2, 3 }));
            Assert.Equal("010203", Hexadecimal.Encode(new byte[] { 1, 2, 3 }, 0, 3));
            Assert.Equal("010203", Hexadecimal.Encode(new List<byte>(new byte[] { 1, 2, 3 })));
            Assert.Equal(new byte[] { 1, 2, 3 }, Hexadecimal.Decode("010203"));
            Assert.Equal(new byte[] { 1, 2, 3 }, Hexadecimal.Decode("010203x"));
        }
    }
}