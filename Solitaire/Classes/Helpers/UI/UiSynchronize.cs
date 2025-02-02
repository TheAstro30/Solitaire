/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System;
using System.ComponentModel;

namespace Solitaire.Classes.Helpers.UI
{
    public class UiSynchronize
    {
        /* This class eliminates the need for delegates when calling InvokeRequired/BeginInvoke invocation on UI objects */
        private readonly ISynchronizeInvoke _sync;

        public UiSynchronize(ISynchronizeInvoke sync)
        {
            _sync = sync;
        }

        public void Execute(Action action)
        {
            if (_sync == null)
            {
                /* It shouldn't be null, as the constructor forces us to use a synchronous object */
                return;
            }
            _sync.BeginInvoke(action, null);
        }
    }
}
