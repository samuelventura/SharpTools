using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
using NUnit.Framework;
using SharpTools;

namespace SharpToolsUI
{
    [TestFixture]
    public class CatchAllTest
    {
        [Test]
        public void ThreadTest()
        {
            var lines = Program.Run("CatchAll Thread");

            Assert.AreEqual(1, lines.Length);

            Assert.AreEqual("System.Exception In Thread", lines[0]);
        }

        [Test]
        public void UITest()
        {
            var lines = Program.Run("CatchAll UI");

            Assert.AreEqual(1, lines.Length);

            Assert.AreEqual("System.Exception In UI", lines[0]);
        }
    }
}