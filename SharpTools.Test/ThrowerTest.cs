using System;
using System.IO;
using System.Threading;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Xunit;

namespace SharpTools
{
    public class ThrowerTest
    {
        [Fact]
        public void MakeTest()
        {
            var inner = Thrower.Make("Inner {0}", true);
            var outer = Thrower.Make(inner, "Outer {0}", false);

            Assert.Equal("Inner True", inner.Message);
            Assert.Equal("Outer False", outer.Message);
            Assert.NotNull(outer.InnerException);
            Assert.Equal("Inner True", outer.InnerException.Message);
        }

        [Fact]
        public void ThrowTest()
        {
            var exceptions = new List<Exception>();

            try { Thrower.Throw("Inner {0}", true); } catch (Exception ex) { exceptions.Add(ex); }
            try { Thrower.Throw(new Exception("Inner"), "Outer {0}", true); } catch (Exception ex) { exceptions.Add(ex); }

            Assert.Equal(2, exceptions.Count);
            Assert.Equal("Inner True", exceptions[0].Message);
            Assert.Equal("Outer True", exceptions[1].Message);
            Assert.NotNull(exceptions[1].InnerException);
            Assert.Equal("Inner", exceptions[1].InnerException.Message);
        }

        [Fact]
        public void DumpTest()
        {
            var folder = Executable.Relative("Exceptions");
            var folderInfo = new DirectoryInfo(folder);
            if (Directory.Exists(folder)) Directory.Delete(folder, true);
            //returns true sometimes when opened on Explorer
            while (Directory.Exists(folder)) Thread.Yield();
            Assert.Equal(false, Directory.Exists(folder));

            Thrower.Dump(new Exception("Exception at DumpTest!", new Exception("Inner")));
            Assert.Equal(true, Directory.Exists(folder));
            var files = Directory.GetFiles(folder);
            Assert.Equal(1, files.Length);
            var file = Path.GetFileName(files[0]);
            Assert.Equal(true, file.StartsWith("exception-"));
            Assert.Equal(true, file.EndsWith(".txt"));
            var regex = new Regex(@"exception-\d{8}_\d{6}_\d{3}\.txt");
            Assert.Equal(true, regex.IsMatch(file));
            var lines = File.ReadAllLines(files[0]);
            Assert.Equal(true, lines.Length > 0);
            Assert.Equal("System.Exception: Exception at DumpTest! ---> System.Exception: Inner", lines[0]);
        }
    }
}