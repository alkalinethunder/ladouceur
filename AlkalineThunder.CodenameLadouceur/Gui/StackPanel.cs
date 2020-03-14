using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlkalineThunder.CodenameLadouceur.Gui
{
    public sealed class StackPanel : LayoutControl
    {
        public Orientation Orientation { get; set; } = Orientation.Vertical;
        public int Spacing { get; set; } = 0;


        protected override Vector2 MeasureOverride()
        {
            float x = 0;
            float y = 0;

            if(Orientation == Orientation.Vertical)
            {
                foreach(var child in InternalChildren)
                {
                    var measure = child.CalculateSize();

                    x = Math.Max(x, measure.X);
                    y += measure.Y;
                }

                y += Spacing * (InternalChildren.Count - 1);
            }
            else
            {
                foreach(var child in InternalChildren)
                {
                    var measure = child.CalculateSize();

                    y = Math.Max(y, measure.Y);
                    x += measure.X;
                }

                x += Spacing * (InternalChildren.Count - 1);
            }

            return new Vector2(x, y);
        }

        protected override void ArrangeOverride()
        {
            if(Orientation == Orientation.Vertical)
            {
                float y = 0;

                foreach(var child in InternalChildren)
                {
                    PlaceControl(child, new Rectangle(Bounds.Left, Bounds.Top + (int)y, Bounds.Width, (int)child.DesiredSize.Y));
                    y += (int)child.DesiredSize.Y + Spacing;
                }
            }
            else
            {
                float x = 0;

                foreach (var child in InternalChildren)
                {
                    PlaceControl(child, new Rectangle(Bounds.Left + (int)x, Bounds.Top, (int)child.DesiredSize.X, Bounds.Height));
                    x += (int)child.DesiredSize.X + Spacing;
                }
            }
        }
    }
}
