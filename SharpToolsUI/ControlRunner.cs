
using System;
using System.Windows.Forms;

namespace SharpTools
{
    public class ControlRunner
    {
        private readonly Control control;
        private readonly Action<Exception> catcher;

        public ControlRunner(Control control, Action<Exception> catcher = null)
        {
            this.control = control;
            this.catcher = catcher;
        }

        public void Run(Action action)
        {
            Action wrapper = () => Catcher.Try(action, catcher);
            //catcher should run on the ui thread
            //Invoke re-throws nested exception in caller thread
            //BeginInvoke throws nested exception in UI thread generating the .net error dialog
            //Confirmed Invoke can be called from UI thread without locking (docs say nothing)
            //https://ikriv.com/dev/dotnet/MysteriousHang
            if (control.InvokeRequired)
            {
                control.Invoke(wrapper);
            }
            else wrapper();
        }
    }
}