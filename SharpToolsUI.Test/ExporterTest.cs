using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
using NUnit.Framework;
using SharpTools;

namespace SharpToolsUI
{
    [TestFixture]
    public class ExporterTest
    {
        [Test]
        public void StringTest()
        {
            var listView = new ListView();
            listView.Columns.Add("Name");
            listView.Columns.Add("Desc");
            listView.Items.Add(new ListViewItem(new string[] { "Item1", "Desc1" }));
            listView.Items.Add(new ListViewItem(new string[] { "Item2", "Desc2" }));

            var writer = new StringWriter();
            Exporter.CSV(listView, writer);
            var lines = writer.ToString().Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            Assert.AreEqual(3, lines.Length);
            Assert.AreEqual("\"Name\",\"Desc\"", lines[0]);
            Assert.AreEqual("\"Item1\",\"Desc1\"", lines[1]);
            Assert.AreEqual("\"Item2\",\"Desc2\"", lines[2]);
        }

        [Test]
        public void FileTest()
        {
            var listView = new ListView();
            listView.Columns.Add("Name");
            listView.Columns.Add("Desc");
            listView.Items.Add(new ListViewItem(new string[] { "Item1", "Desc1" }));
            listView.Items.Add(new ListViewItem(new string[] { "Item2", "Desc2" }));

            var file = Executable.Relative("Exports", "nunit-export.csv");
            Directory.CreateDirectory(Executable.Relative("Exports"));
            using (var writer = new StreamWriter(file)) Exporter.CSV(listView, writer);
            var lines = File.ReadAllLines(file);

            Assert.AreEqual(3, lines.Length);
            Assert.AreEqual("\"Name\",\"Desc\"", lines[0]);
            Assert.AreEqual("\"Item1\",\"Desc1\"", lines[1]);
            Assert.AreEqual("\"Item2\",\"Desc2\"", lines[2]);
        }

        [Test]
        public void QuoteTest()
        {
            Assert.AreEqual("\"Hello\"", Exporter.Quote("Hello"));
            Assert.AreEqual("\"He\"\"llo\"", Exporter.Quote("He\"llo"));
        }
    }
}