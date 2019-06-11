using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
using NUnit.Framework;
using SharpTools;

namespace SharpToolsUI
{
    [TestFixture]
    public class LogViewerTester
    {
        [Test]
        public void AppendTest()
        {
            var log = new Log
            {
                Name = "NAM$",
                Level = LogLevel.INFO,
                Timestamp = DateTime.Parse("2019-01-02 23:24:25.876"),
                ThreadName = "THREA$",
                Verbosity = 1,
                Format = "Message {0}",
                Args = new object[] { 2 }
            };

            var formatter = new PatternLogFormatter("{LEVEL} {MESSAGE}");
            var viewer = new LogViewer();

            formatter.Format(log);
            viewer.Append(log);

            log.Level = LogLevel.DEBUG;

            log.Args = new object[] { 3 };
            formatter.Format(log);
            viewer.ShowDebug = false;
            viewer.Append(log);

            log.Args = new object[] { 4 };
            formatter.Format(log);
            viewer.ShowDebug = true;
            viewer.Append(log);


            var lines = viewer.AllText().Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            Assert.AreEqual(2, lines.Length);
            Assert.AreEqual("INFO Message 2", lines[0]);
            Assert.AreEqual("DEBUG Message 4", lines[1]);
        }
    }
}