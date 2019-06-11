using System;
using System.IO;
using System.Reflection;

namespace SharpTools
{
    public static class Executable
    {
        public static Assembly GetAssembly()
        {
            //GetExecutingAssembly returns SharpTools.dll
            //GetEntryAssembly returns the entry point assembly
            return Assembly.GetEntryAssembly();
        }

        public static DateTime BuildDateTime()
        {
            //http://stackoverflow.com/questions/1600962/displaying-the-build-date
            //does not consider daylight savings time
            var version = GetAssembly().GetName().Version;
            return new DateTime(2000, 1, 1).Add(new TimeSpan(
                TimeSpan.TicksPerDay * version.Build + // days since 1 January 2000
                TimeSpan.TicksPerSecond * 2 * version.Revision)); /* seconds since midnight,
                                                     (multiply by 2 to get original) */
        }

        public static string VersionString()
        {
            return GetAssembly().GetName().Version.ToString();
        }

        public static string FullPath()
        {
            return GetAssembly().Location;
        }

        public static string Filename()
        {
            var fullPath = FullPath();
            return Path.GetFileNameWithoutExtension(fullPath);
        }

        public static string DirectoryPath()
        {
            var fullPath = FullPath();
            return Path.GetDirectoryName(fullPath);
        }

        public static string Relative(string filename)
        {
            var folder = DirectoryPath();
            return Path.Combine(folder, filename);
        }

        public static string Relative(string folder, string filename)
        {
            folder = Relative(folder);
            return Path.Combine(folder, filename);
        }
    }
}