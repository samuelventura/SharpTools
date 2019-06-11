
using System;
using System.Windows.Forms;

namespace SharpToolsUI
{
    public class CatchAll
    {
        public static void Setup(Action<Exception> handler)
        {
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += (s, e) => { handler(e.Exception); };
            AppDomain.CurrentDomain.UnhandledException += (s, e) => { handler(e.ExceptionObject as Exception); };
        }
    }
}
