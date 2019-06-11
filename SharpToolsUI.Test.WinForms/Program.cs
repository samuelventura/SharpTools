using System;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;
using SharpTools;

namespace SharpToolsUI
{

    public static class Program
    {
        public static string[] Run(string arguments)
        {
            //A fatal error was encountered. The library 'hostpolicy.dll' required to execute the application was not found in 'C:\Program Files\dotnet'.
            //Problem is SharpToolsUI.Test.WinForms.runtimeconfig.json is not being copied to output folder
            //It gets generated in SharpToolsUI.Test.WinForms/bin/Debug/netcoreapp3.0 but not carried to SharpToolsUI.Test/bin/Debug/netcoreapp3.0
            var rtcExecuting = Path.ChangeExtension(Assembly.GetExecutingAssembly().Location, ".runtimeconfig.json"); //SharpToolsUI.Test.WinForms
            var rtcCalling = Path.ChangeExtension(Assembly.GetCallingAssembly().Location, ".runtimeconfig.json"); //SharpToolsUI.Test
            if (!File.Exists(rtcExecuting)) File.Copy(rtcCalling, rtcExecuting);

            var p = new Process();
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.Arguments = arguments;
            //Gets DLL extension!
            //C:\Users\samuel\Documents\github\SharpTools\SharpToolsUI.Test\bin\Debug\netcoreapp3.0\SharpToolsUI.Test.WinForms.dll
            p.StartInfo.FileName = Path.ChangeExtension(Assembly.GetExecutingAssembly().Location, ".exe");
            p.Start();
            p.WaitForExit();
            var output = p.StandardOutput.ReadToEnd();
            return output.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
        }

        private static void OnException(Exception ex)
        {
            Console.WriteLine(string.Format("{0} {1}", ex.GetType(), ex.Message));
            Thrower.Dump(ex);
            Application.Exit();
        }

        [STAThread]
        static void Main(string[] args)
        {
            Thread.CurrentThread.Name = "Main";

            CatchAll.Setup(OnException);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var form = new MainForm();
            form.WindowState = FormWindowState.Minimized;
            form.ShowInTaskbar = false;
            form.Load += (s, e) =>
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
                        case "ControlRunner":
                            RunControlRunner(form, args);
                            break;
                        case "CatchAll":
                            RunCatchAll(form, args);
                            break;
                    }
                }
            };
            //runs loop on current thread
            Application.Run(form);
        }

        private static void RunCatchAll(MainForm form, string[] args)
        {
            var thread = new Thread(() =>
            {
                switch (args[1])
                {
                    case "Thread":
                        throw new Exception("In Thread");
                    case "UI":
                        Action action = () => { throw new Exception("In UI"); };
                        form.Invoke(action);
                        break;
                }
            });
            thread.IsBackground = false;
            thread.Start();
        }

        private static void RunControlRunner(MainForm form, string[] args)
        {
            Console.WriteLine("Thread {0}", Thread.CurrentThread.Name);
            Console.WriteLine("InvokeRequired {0}", form.InvokeRequired);
            var uir = new ControlRunner(form, (ex) =>
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Should be ignored");
            });
            var ior = new ThreadRunner("Runner");
            ior.Run(() =>
            {
                Console.WriteLine("InvokeRequired {0}", form.InvokeRequired);
                Console.WriteLine("Thread {0}", Thread.CurrentThread.Name);
                uir.Run(() => { throw new Exception("Should be catched"); });
                uir.Run(() =>
                {
                    Console.WriteLine("InvokeRequired {0}", form.InvokeRequired);
                    Console.WriteLine("Thread {0}", Thread.CurrentThread.Name);
                    Console.WriteLine("form.Text {0}", form.Text);
                    form.Close();
                });
            });
        }
    }
}
