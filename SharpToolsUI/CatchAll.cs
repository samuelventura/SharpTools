
using System;
using System.Windows.Forms;

namespace SharpToolsUI
{
    public class CatchAll
    {
        public static void Setup(Action<Exception> handler = null)
        {
            handler = handler ?? Handler;
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += (s, e) => { handler(e.Exception); };
            AppDomain.CurrentDomain.UnhandledException += (s, e) => { handler(e.ExceptionObject as Exception); };
        }

        public static void Handler(Exception ex)
        {
            Catcher.Try(() => Process.Start(Thrower.Dump(ex)));
            MessageBox.Show(ex.Message, "Unhandled Exception");
            Environment.Exit(1);
        }
    }
}
