using System;
using System.Collections.Generic;
using System.Text;

namespace AlkalineThunder.Nucleus.Gui
{
    public sealed class FocusEventArgs : EventArgs
    {
        public Control LostFocus { get; private set; }
        public Control GainedFocus { get; private set; }

        public FocusEventArgs(Control lost, Control gained)
        {
            LostFocus = lost;
            GainedFocus = gained;
        }
    }
}
