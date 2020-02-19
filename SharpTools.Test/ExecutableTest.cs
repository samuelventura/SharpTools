using System;
using System.IO;
using Xunit;

namespace SharpTools
{
    public class ExecutableTest
    {
        [Fact]
        public void BasicTest()
        {
            //C:\Users\samuel\Documents\github\SharpTools\SharpTools.Test\bin\Debug\netcoreapp3.0\testhost.dll
            //C:\Users\samuel\Documents\github\SharpTools\SharpTools.Test\bin\Debug\netcoreapp3.0\SharpTools.Test.Console.exe

            var lines = Program.Run("Executable");

            Assert.Equal(7, lines.Length);
            Assert.Equal("1.0.0.0", lines[0]);
            Assert.Equal("1/1/2000 12:00:00 AM", lines[1]);
            Assert.Equal("SharpTools.Test.Console", lines[2]);
            var folder = lines[3];
            Assert.Equal(Path.Combine(folder, "SharpTools.Test.Console.dll"), lines[4]);
            Assert.Equal(Path.Combine(folder, "file"), lines[5]);
            Assert.Equal(Path.Combine(folder, "folder", "file"), lines[6]);
        }
    }
}