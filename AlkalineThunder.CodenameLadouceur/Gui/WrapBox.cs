using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlkalineThunder.CodenameLadouceur.Gui
{
    public sealed class WrapBox :LayoutControl
    {
        public Orientation Orientation { get; set; } = Orientation.Horizontal;

        public int MaxItems { get; set; } = 6;
        public int HorizontalSpacing { get; set; } = 0;
        public int VerticalSpacing { get; set; } = 0;

        protected override Vector2 MeasureOverride()
        {
            if(MaxItems > 0)
            {
                float x = 0;
                float y = 0;
                float lw = 0;
                float lm = 0;
                                
                for(int i = 0; i < InternalChildren.Count; i++)
                {
                    var size = InternalChildren[i].CalculateSize();

                    if(i % MaxItems == 0)
                    {
                        if(Orientation == Orientation.Horizontal)
                        {
                            x = Math.Max(lw, x);
                            lw = 0;
                            y += lm + VerticalSpacing;
                            lm = 0;
                        }
                        else
                        {
                            y = Math.Max(lw, y);
                            lw = 0;
                            x += lm + HorizontalSpacing;
                            lm = 0;
                        }
                    }

                    if(Orientation == Orientation.Horizontal)
                    {
                        lw += size.X + HorizontalSpacing;
                        lm = Math.Max(lm, size.Y);
                    }
                    else
                    {
                        lw += size.Y + VerticalSpacing;
                        lm = Math.Max(lm, size.X);
                    }
                }

                if(Orientation == Orientation.Horizontal)
                {
                    x = Math.Max(lw, x);
                    y += lm;
                }
                else
                {
                    y = Math.Max(lw, y);
                    x += lm;
                }

                return new Vector2(x, y);
            }
            else
            {
                return Vector2.Zero;
            }
        }

        protected override void ArrangeOverride()
        {
            int x = ContentBounds.Left;
            int y = ContentBounds.Top;
            int lineMax = 0;

            foreach(var child in InternalChildren)
            {
                var size = child.DesiredSize;

                if(Orientation == Orientation.Horizontal)
                {
                    if(x + (int)size.X > ContentBounds.Right)
                    {
                        x = ContentBounds.Left;
                        y += lineMax + VerticalSpacing;
                        lineMax = 0;
                    }

                    PlaceControl(child, new Microsoft.Xna.Framework.Rectangle(
                            x,
                            y,
                            (int)size.X,
                            (int)size.Y
                        ));

                    lineMax = Math.Max(lineMax, (int)size.Y);
                    x += (int)size.X + HorizontalSpacing;
                }
                else
                {
                    if(y + (int)size.Y > ContentBounds.Bottom)
                    {
                        y = ContentBounds.Top;
                        x += lineMax + HorizontalSpacing;
                        lineMax = 0;
                    }

                    PlaceControl(child, new Microsoft.Xna.Framework.Rectangle(
                            x,
                            y,
                            (int)size.X,
                            (int)size.Y
                        ));

                    y += (int)size.Y + VerticalSpacing;
                }
            }
        }
    }
}
