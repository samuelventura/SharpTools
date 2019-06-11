using System;
using System.IO;
using System.Threading;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using NUnit.Framework;

namespace SharpTools
{
    [TestFixture]
    public class ThrowerTest
    {
        [Test]
        public void MakeTest()
        {
            var inner = Thrower.Make("Inner {0}", true);
            var outer = Thrower.Make(inner, "Outer {0}", false);

            Assert.AreEqual("Inner True", inner.Message);
            Assert.AreEqual("Outer False", outer.Message);
            Assert.NotNull(outer.InnerException);
            Assert.AreEqual("Inner True", outer.InnerException.Message);
        }

        [Test]
        public void ThrowTest()
        {
            var exceptions = new List<Exception>();

            try { Thrower.Throw("Inner {0}", true); } catch (Exception ex) { exceptions.Add(ex); }
            try { Thrower.Throw(new Exception("Inner"), "Outer {0}", true); } catch (Exception ex) { exceptions.Add(ex); }

            Assert.AreEqual(2, exceptions.Count);
            Assert.AreEqual("Inner True", exceptions[0].Message);
            Assert.AreEqual("Outer True", exceptions[1].Message);
            Assert.NotNull(exceptions[1].InnerException);
            Assert.AreEqual("Inner", exceptions[1].InnerException.Message);
        }

        [Test]
        public void DumpTest()
        {
            var folder = Executable.Relative("Exceptions");
            var folderInfo = new DirectoryInfo(folder);
            if (Directory.Exists(folder)) Directory.Delete(folder, true);
            //returns true sometimes when opened on Explorer
            while (Directory.Exists(folder)) Thread.Yield();
            Assert.IsFalse(Directory.Exists(folder));

            Thrower.Dump(new Exception("Exception at DumpTest!", new Exception("Inner")));
            Assert.IsTrue(Directory.Exists(folder));
            var files = Directory.GetFiles(folder);
            Assert.AreEqual(1, files.Length);
            var file = Path.GetFileName(files[0]);
            Assert.IsTrue(file.StartsWith("exception-"));
            Assert.IsTrue(file.EndsWith(".txt"));
            var regex = new Regex(@"exception-\d{8}_\d{6}_\d{3}\.txt");
            Assert.IsTrue(regex.IsMatch(file));
            var lines = File.ReadAllLines(files[0]);
            Assert.IsTrue(lines.Length > 0);
            Assert.AreEqual("System.Exception: Exception at DumpTest! ---> System.Exception: Inner", lines[0]);
        }
    }
}