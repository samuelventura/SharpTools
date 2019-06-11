
using System;
using System.Windows.Forms;

namespace SharpTools
{
    public class ControlRunner
    {
        private readonly Control control;
        private readonly Catcher catcher;

        public ControlRunner(Control control, Action<Exception> catcher = null)
        {
            this.control = control;
            this.catcher = new Catcher(catcher);
        }

        public void Run(Action action)
        {
            Action wrapper = () => { catcher.Run(action); };
            //Invoke re-throws nested exception in caller thread
            //BeginInvoke throws nested exception in UI thread
            control.Invoke(wrapper);
        }
    }
}