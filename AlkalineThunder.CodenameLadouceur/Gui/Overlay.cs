using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlkalineThunder.Nucleus.Gui
{
    public sealed class Overlay : LayoutControl
    {
        protected override Vector2 MeasureOverride()
        {
            float x = 0;
            float y = 0;

            foreach(var child in InternalChildren)
            {
                var measure = child.CalculateSize();

                x = Math.Max(x, measure.X);
                y = Math.Max(y, measure.Y);
            }

            return new Vector2(x, y);
        }

        protected override void ArrangeOverride()
        {
            foreach(var child in InternalChildren)
            {
                PlaceControl(child, ContentBounds);
            }
        }
    }
}
