using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlkalineThunder.CodenameLadouceur.Gui
{
    public sealed class Switcher : LayoutControl
    {
        private int _activeIndex = 0;

        public int ActiveIndex
        {
            get => _activeIndex;
            set => _activeIndex = MathHelper.Clamp(value, 0, InternalChildren.Count);
        }

        public Control ActiveControl => InternalChildren.Count > 0 ? InternalChildren[_activeIndex] : null;

        protected override Vector2 MeasureOverride()
        {
            if(ActiveControl != null)
            {
                return ActiveControl.CalculateSize();
            }
            else
            {
                return Vector2.Zero;
            }
        }

        protected override void ArrangeOverride()
        {
            foreach(var child in InternalChildren)
            {
                if(child == ActiveControl)
                {
                    PlaceControl(child, ContentBounds);
                }
                else
                {
                    PlaceControl(child, Rectangle.Empty);
                }
            }
        }
    }
}
