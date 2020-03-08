using System;
using System.Collections.Generic;
using System.Text;

namespace AlkalineThunder.CodenameLadouceur.Gui
{
    public abstract class ContentControl : Control
    {
        private Control _content = null;

        protected override bool SupportsChildren => true;

        public Control Content
        {
            get => _content;
            set
            {
                if(_content != value)
                {
                    if (_content != null) InternalChildren.Remove(_content);
                    _content = value;
                    if (_content != null) InternalChildren.Add(_content);
                }
            }
        }
    }
}
