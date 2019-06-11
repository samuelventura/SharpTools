using System;
using System.IO;
using NUnit.Framework;

namespace SharpTools
{
    [TestFixture]
    public class ExecutableTest
    {
        [Test]
        public void BasicTest()
        {
            //C:\Users\samuel\Documents\github\SharpTools\SharpTools.Test\bin\Debug\netcoreapp3.0\testhost.dll
            //C:\Users\samuel\Documents\github\SharpTools\SharpTools.Test\bin\Debug\netcoreapp3.0\SharpTools.Test.Console.exe

            var lines = Program.Run("Executable");

            Assert.AreEqual(7, lines.Length);
            Assert.AreEqual("1.0.0.0", lines[0]);
            Assert.AreEqual("1/1/2000 12:00:00 AM", lines[1]);
            Assert.AreEqual("SharpTools.Test.Console", lines[2]);
            var folder = lines[3];
            Assert.AreEqual(Path.Combine(folder, "SharpTools.Test.Console.dll"), lines[4]);
            Assert.AreEqual(Path.Combine(folder, "file"), lines[5]);
            Assert.AreEqual(Path.Combine(folder, "folder", "file"), lines[6]);
        }
    }
}