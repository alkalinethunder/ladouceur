using System;
using System.Collections.Generic;
using System.Text;

namespace AlkalineThunder.Nucleus.Gui
{
    public abstract class LayoutControl : Control
    {
        protected override bool SupportsChildren => true;

        public ControlCollection Children => InternalChildren;
    }
}
