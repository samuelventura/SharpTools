using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
using NUnit.Framework;
using SharpTools;

namespace SharpToolsUI
{
    [TestFixture]
    public class ControlRunnerTest
    {
        [Test]
        public void BasicTest()
        {
            var lines = Program.Run("ControlRunner");

            Assert.AreEqual(8, lines.Length);

            Assert.AreEqual("Thread Main", lines[0]);
            Assert.AreEqual("InvokeRequired False", lines[1]);
            Assert.AreEqual("InvokeRequired True", lines[2]);
            Assert.AreEqual("Thread Runner", lines[3]);
            Assert.AreEqual("Should be catched", lines[4]);
            Assert.AreEqual("InvokeRequired False", lines[5]);
            Assert.AreEqual("Thread Main", lines[6]);
            Assert.AreEqual("form.Text MainForm - nunit", lines[7]);
        }
    }
}