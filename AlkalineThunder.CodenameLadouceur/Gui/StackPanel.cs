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

            // Pass 2: Calculate sizes and locations of auto-sized elements.
            int totalSpace = (Orientation == Orientation.Horizontal) ? ContentBounds.Width : ContentBounds.Height;
            int loc = (Orientation == Orientation.Horizontal) ? ContentBounds.Left : ContentBounds.Top;
            for(int i = 0; i < layoutInfo.Count; i++)
            {
                var layout = layoutInfo[i];

                if(layout.SizeMode == SizeMode.Auto)
                {
                    int x = 0;
                    int y = 0;
                    int width = 0;
                    int height = 0;

                    switch(Orientation)
                    {
                        case Orientation.Horizontal:
                            x = loc;
                            y = ContentBounds.Top;
                            width = (int)layout.Size;
                            height = ContentBounds.Height;

                            loc += width + Spacing;
                            totalSpace -= width + Spacing;
                            break;
                        case Orientation.Vertical:
                            x = ContentBounds.Left;
                            y = loc;
                            width = ContentBounds.Width;
                            height = (int)layout.Size;

                            loc += height + Spacing;
                            totalSpace -= height + Spacing;
                            break;
                    }

                    layout.Bounds = new Rectangle(x, y, width, height);
                    layoutInfo[i] = layout;
                }
            }

            // Pass 3: Find and group all filled widgets.
            var filledGroups = new List<FilledLayoutGroup>();
            int fillStart = -1;
            for(int i = 0; i <= layoutInfo.Count; i++)
            {
                if (i == layoutInfo.Count)
                {
                    if(fillStart > -1)
                    {
                        filledGroups.Add(new FilledLayoutGroup(fillStart, i - 1));
                        fillStart = -1;
                    }
                }
                else
                {
                    var layout = layoutInfo[i];

                    if (layout.SizeMode == SizeMode.Auto)
                    {
                        if (fillStart > -1)
                        {
                            filledGroups.Add(new FilledLayoutGroup(fillStart, i - 1));
                            fillStart = -1;
                        }
                    }
                    else
                    {
                        if (fillStart == -1) fillStart = i;
                    }
                }
            }
            
            // Pass 4: 
            for(int i = 0; i < filledGroups.Count; i++)
            {
                // Group information.
                var group = filledGroups[i];

                // Start position of the fill group.
                int startPos = (Orientation == Orientation.Horizontal) ? ContentBounds.Left : ContentBounds.Top;

                // End position of the fill group.
                int endPos = (Orientation == Orientation.Horizontal) ? ContentBounds.Right : ContentBounds.Bottom;

                // Calculate the start position of the fill - it's the total size of each auto-sized widget before the fill group.
                int prevAccum = 0;
                if (group.StartIndex > 0)
                {
                    for(int j = 0; j < group.StartIndex; j++)
                    {
                        var layout = layoutInfo[j];

                        if(layout.SizeMode == SizeMode.Auto)
                        {
                            if(Orientation == Orientation.Horizontal)
                            {
                                prevAccum += layout.Bounds.Width + Spacing;
                            }
                            else
                            {
                                prevAccum += layout.Bounds.Height + Spacing;
                            }
                        }
                    }

                    startPos += prevAccum;
                }

                // The end position is trickier - first we need to know the size of everything "below" the fill group...
                int accum = 0;
                for(int j = group.EndIndex + 1; j < layoutInfo.Count; j++)
                {
                    var layout = layoutInfo[j];

                    if(Orientation == Orientation.Horizontal)
                    {
                        accum += layout.Bounds.Width + Spacing;
                    }
                    else
                    {
                        accum += layout.Bounds.Height + Spacing;
                    }
                }

                // End position gets decreased by that accumulated value.
                endPos -= accum;

                // Move over previous controls to make some more room for this widget.
                int gainedArea = 0;
                int lostArea = 0;
                for(int j = i - 1; j >= 0; j--)
                {
                    var prevGroup = filledGroups[j];

                    // Calculate size of widgets between the fill group and the next.
                    int afterSize = 0;
                    for(int k = prevGroup.EndIndex + 1; k < layoutInfo.Count; k++)
                    {
                        var afLayout = layoutInfo[k];

                        if(afLayout.SizeMode == SizeMode.Fill)
                        {
                            break;
                        }
                        else
                        {
                            afterSize += (int)afLayout.Size;
                        }
                    }

                    int fillDecrease = prevAccum / prevGroup.Count;

                    for(int k = prevGroup.StartIndex; k <= prevGroup.EndIndex; k++)
                    {
                        var kLayout = layoutInfo[k];

                        if(Orientation == Orientation.Horizontal)
                        {
                            kLayout.Bounds.Width -= fillDecrease;
                            if(k > prevGroup.StartIndex)
                            {
                                kLayout.Bounds.X -= fillDecrease;
                            }

                            lostArea += kLayout.Bounds.Width + Spacing;
                        }
                        else
                        {
                            kLayout.Bounds.Height -= fillDecrease;
                            if (k > prevGroup.StartIndex)
                            {
                                kLayout.Bounds.Y -= fillDecrease;
                            }

                            lostArea += kLayout.Bounds.Height + Spacing;
                        }

                        layoutInfo[k] = kLayout;
                    }

                    gainedArea += fillDecrease + (Spacing * prevGroup.Count);

                    // Move over widgets that come after this fill group.
                    for(int k = prevGroup.EndIndex + 1; k < layoutInfo.Count; k++)
                    {
                        var kLayout = layoutInfo[k];

                        if (kLayout.SizeMode == SizeMode.Auto)
                        {
                            if(Orientation == Orientation.Horizontal)
                            {
                                kLayout.Bounds.X -= gainedArea;
                            }
                            else
                            {
                                kLayout.Bounds.Y -= gainedArea;
                            }
                        }
                        else break;

                        layoutInfo[k] = kLayout;
                    }
                }

                startPos -= gainedArea;
                startPos += lostArea;

                // And now we know our total width.
                int totalFillWidth = endPos - startPos;

                // Make room for the fill group by pushing auto-sized widgets...
                for(int j = group.EndIndex + 1; j < layoutInfo.Count; j++)
                {
                    var layout = layoutInfo[j];

                    if(layout.SizeMode == SizeMode.Auto)
                    {
                        if(Orientation == Orientation.Horizontal)
                        {
                            layout.Bounds.X += totalFillWidth;
                        }
                        else
                        {
                            layout.Bounds.Y += totalFillWidth;
                        }

                        layoutInfo[j] = layout;
                    }
                }

                // Now that there's room, we need to figure out how large each widget in the group should be.
                int individualSize = (totalFillWidth + (Spacing * group.Count)) / group.Count;

                // Go through each layout in the group.
                for(int j = group.StartIndex; j <= group.EndIndex; j++)
                {
                    var layout = layoutInfo[j];

                    if(Orientation == Orientation.Horizontal)
                    {
                        layout.Bounds.X = startPos;
                        layout.Bounds.Y = ContentBounds.Top;
                        layout.Bounds.Width = individualSize;
                        layout.Bounds.Height = ContentBounds.Height;
                    }
                    else
                    {
                        layout.Bounds.X = ContentBounds.Left;
                        layout.Bounds.Y = startPos;
                        layout.Bounds.Width = ContentBounds.Width;
                        layout.Bounds.Height = individualSize;
                    }

                    startPos += individualSize + Spacing;

                    layoutInfo[j] = layout;
                }
            }

            // Pass 5: Actually place the UI elements.
            foreach (var layout in layoutInfo)
            {
                PlaceControl(layout.Control, layout.Bounds);
            }
        }

        public struct FilledLayoutGroup
        {
            public int StartIndex;
            public int EndIndex;

            public int Count => (EndIndex - StartIndex) + 1;

            public FilledLayoutGroup(int start, int end)
            {
                StartIndex = start;
                EndIndex = end;
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
