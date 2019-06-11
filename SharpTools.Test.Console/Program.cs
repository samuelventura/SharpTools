using System;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using SharpTools;

namespace SharpTools
{
    public static class Program
    {
        public static string[] Run(string arguments)
        {
            //A fatal error was encountered. The library 'hostpolicy.dll' required to execute the application was not found in 'C:\Program Files\dotnet'.
            //Problem is SharpTools.Test.Console.runtimeconfig.json is not being copied to output folder
            //It gets generated in SharpTools.Test.Console/bin/Debug/netcoreapp3.0 but carried to SharpTools.Test/bin/Debug/netcoreapp3.0
            var rtcExecuting = Path.ChangeExtension(Assembly.GetExecutingAssembly().Location, ".runtimeconfig.json"); //SharpTools.Test.Console
            var rtcCalling = Path.ChangeExtension(Assembly.GetCallingAssembly().Location, ".runtimeconfig.json"); //SharpTools.Test
            if (!File.Exists(rtcExecuting)) File.Copy(rtcCalling, rtcExecuting);

            var p = new Process();
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.Arguments = arguments;
            //Get DLL extension!
            //C:\Users\samuel\Documents\github\SharpTools\SharpTools.Test\bin\Debug\netcoreapp3.0\SharpTools.Test.Console.dll
            p.StartInfo.FileName = Path.ChangeExtension(Assembly.GetExecutingAssembly().Location, ".exe");
            p.Start();
            p.WaitForExit();
            var output = p.StandardOutput.ReadToEnd();
            return output.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
        }

        [STAThread]
        private static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("No arguments...");
                Console.ReadKey();
            }
            else
            {
                switch (args[0])
                {
                    case "Executable":
                        RunExecutable(args);
                        break;
                    case "GlobalMutex":
                        RunGlobalMutext(args);
                        break;
                    case "Logger":
                        RunLogger(args);
                        break;
                }
            }
            Console.Out.Flush();
        }

        private static void RunExecutable(string[] args)
        {
            Console.WriteLine(Executable.VersionString());
            Console.WriteLine(Executable.BuildDateTime().ToString());
            Console.WriteLine(Executable.Filename());
            Console.WriteLine(Executable.DirectoryPath());
            Console.WriteLine(Executable.FullPath());
            Console.WriteLine(Executable.Relative("file"));
            Console.WriteLine(Executable.Relative("folder", "file"));
        }

        private static void RunGlobalMutext(string[] args)
        {
            var mutex = new GlobalMutex(args[1], () => { Console.WriteLine("Mutex Busy"); });
            mutex.Run(() => { Console.WriteLine("Mutex Free"); });
        }

        private static void RunLogger(string[] args)
        {
            using (var runner = new LogRunner(new PatternLogFormatter("{LEVEL} {NAME} {MESSAGE}")))
            {
                runner.AddAppender(new ConsoleLogAppender());

                var logger = new Logger(runner, "N$AME");

                logger.Debug("Message {0}", 1);
                logger.Info("Message {0}", 2);
                logger.Warn("Message {0}", 3);
                logger.Error("Message {0}", 4);
                logger.Success("Message {0}", 5);
            }
        }
    }
}