
using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace SharpToolsUI
{
    public class Exporter
    {
        public static void CSV(ListView listView, string filename)
        {
            using (var writer = new StreamWriter(filename))
            {
                CSV(listView, writer);
            }
        }

        public static void CSV(ListView listView, TextWriter writer)
        {
            for (var i = 0; i < listView.Columns.Count; i++)
            {
                if (i > 0) writer.Write(",");
                writer.Write(Quote(listView.Columns[i].Text));
            }
            writer.WriteLine();
            for (var i = 0; i < listView.Items.Count; i++)
            {
                var item = listView.Items[i];
                for (int j = 0; j < item.SubItems.Count; j++)
                {
                    if (j > 0) writer.Write(",");
                    writer.Write(Quote(item.SubItems[j].Text));
                }
                writer.WriteLine();
            }
        }

        public static string Quote(string text)
        {
            return "\"" + text.Replace("\"", "\"\"") + "\"";
        }
    }
}
