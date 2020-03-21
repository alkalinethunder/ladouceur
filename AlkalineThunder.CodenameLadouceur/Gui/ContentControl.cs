using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlkalineThunder.Nucleus.Gui
{
    public abstract class ContentControl : Control, IContentControl
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
                    if (_content != null)
                    {
                        InternalChildren.Add(_content);
                        OnContentChanged(_content);
                    }
                }
            }
        }

        protected virtual void OnContentChanged(Control content)
        {

        }

        protected override Vector2 MeasureOverride()
        {
            if (_content != null) return _content.CalculateSize();
            return Vector2.Zero;
        }

        protected override void ArrangeOverride()
        {
            if (_content != null) Control.PlaceControl(_content, ContentBounds);
        }
    }
}
