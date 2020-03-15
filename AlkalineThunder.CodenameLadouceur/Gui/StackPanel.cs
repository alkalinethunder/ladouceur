using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AlkalineThunder.CodenameLadouceur.Gui
{
    public sealed class StackPanel : LayoutControl
    {
        private static readonly string SizeModeProp = "SizeMode";

        public Orientation Orientation { get; set; } = Orientation.Vertical;
        public int Spacing { get; set; } = 0;

        public SizeMode GetSizeMode(Control control)
        {
            if(control.GetProperty(SizeModeProp, out SizeMode sizemode))
            {
                return sizemode;
            }
            else
            {
                return SizeMode.Auto;
            }
        }

        public void SetSizeMode(Control control, SizeMode sizemode)
        {
            control.SetAttachedProperty(SizeModeProp, sizemode);
        }

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

        private IEnumerable<StackLayoutInfo> GetLayoutInfo()
        {
            foreach(var control in InternalChildren)
            {
                var sizeMode = GetSizeMode(control);

                var desiredSize = control.CalculateSize();

                float size = (Orientation == Orientation.Horizontal) ? desiredSize.X : desiredSize.Y;

                yield return new StackLayoutInfo(control, sizeMode, size);
            }
        }

        protected override void ArrangeOverride()
        {
            // Pass 1: Figure out how big everything is and what their size mode is.
            var layoutInfo = GetLayoutInfo().ToList();

            // Pass two: Go through each layout IN REVERSE and start calculating the bounding rectangles.
            // Total space allowed for each element.
            int totalSpace = (Orientation == Orientation.Horizontal) ? ContentBounds.Width : ContentBounds.Height;
            for(int i = layoutInfo.Count - 1; i >= 0; i--)
            {
                var layout = layoutInfo[i];

                // x and y positions are easy as fuck.
                int x = ContentBounds.Left;
                int y = ContentBounds.Top;

                // Width and height depends on the orientation of the stack panel.
                int width = 0;
                int height = 0;

                switch(Orientation)
                {
                    case Orientation.Horizontal:
                        width = (int)layout.Size; // fine for auto-size elements.
                        height = ContentBounds.Height;
                        break;
                    case Orientation.Vertical:
                        width = ContentBounds.Width;
                        height = (int)layout.Size;
                        break;
                }

                // Does this child fill the available space?
                if(layout.SizeMode == SizeMode.Fill)
                {
                    // Reset the width/height depending on orientation.
                    switch(Orientation)
                    {
                        case Orientation.Horizontal:
                            width = totalSpace - Spacing;
                            break;
                        case Orientation.Vertical:
                            height = totalSpace - Spacing;
                            break;
                    }
                }

                // Set our child's bounds.
                layout.Bounds = new Rectangle(x, y, width, height);

                // Update the layout list.
                layoutInfo[i] = layout;

                // Now we must push everything over to insert the new element.
                float accum = 0;
                for(int j = i + 1; j < layoutInfo.Count; j++)
                {
                    var otherLayout = layoutInfo[j];

                    var otherRect = otherLayout.Bounds;

                    // Push the location based on the orientation.
                    switch(Orientation)
                    {
                        case Orientation.Horizontal:
                            otherRect = new Rectangle(
                                    otherRect.Left + Spacing + layout.Bounds.Width,
                                    otherRect.Top,
                                    otherRect.Width,
                                    otherRect.Height
                                );
                            break;
                        case Orientation.Vertical:
                            otherRect = new Rectangle(
                                    otherRect.Left,
                                    otherRect.Top + Spacing + layout.Bounds.Height,
                                    otherRect.Width,
                                    otherRect.Height
                                );
                            break;
                    }

                    // Accumulate auto-sized layouts.
                    if (otherLayout.SizeMode == SizeMode.Auto)
                    {
                        accum += (int)otherLayout.Size;
                    }
                    else
                    {
                        // We need to shrink the bounds.
                        switch (Orientation)
                        {
                            case Orientation.Horizontal:
                                otherRect = new Rectangle(
                                        otherRect.Left,
                                        otherRect.Top,
                                        otherRect.Width - (int)accum,
                                        otherRect.Height
                                    );
                                break;
                            case Orientation.Vertical:
                                otherRect = new Rectangle(
                                        otherRect.Left,
                                        otherRect.Top,
                                        otherRect.Width,
                                        otherRect.Height - (int)accum
                                    );
                                break;
                        }

                        accum = 0;
                    }

                    // Update the other rectangle.
                    otherLayout.Bounds = otherRect;

                    // Update the list.
                    layoutInfo[j] = otherLayout;

                    // Break when we've hit a filled element.
                    if(otherLayout.SizeMode == SizeMode.Fill)
                    {
                        break;
                    }
                }

                // Decrease the total available space if we are an auto-sized element.
                if(layout.SizeMode == SizeMode.Auto)
                {
                    totalSpace -= (int)layout.Size + Spacing;
                }
            }

            // Pass 3: Actually place the UI elements.
            foreach(var layout in layoutInfo)
            {
                PlaceControl(layout.Control, layout.Bounds);
            }
        }

        public struct StackLayoutInfo
        {
            public Control Control;
            public SizeMode SizeMode;
            public float Size;
            public Rectangle Bounds;

            public StackLayoutInfo(Control control, SizeMode sizeMode, float size)
            {
                Control = control;
                SizeMode = sizeMode;
                Size = size;
                Bounds = Rectangle.Empty;
            }
        }
    }

    public enum SizeMode
    {
        Auto,
        Fill
    }
}
