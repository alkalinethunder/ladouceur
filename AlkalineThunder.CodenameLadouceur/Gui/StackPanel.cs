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

        private IEnumerable<FilledLayoutGroup> GetFilledGroups(List<StackLayoutInfo> layoutInfo)
        {
            int groupStart = -1;
            for (int i = 0; i <= layoutInfo.Count; i++)
            {
                if (i < layoutInfo.Count)
                {
                    var layout = layoutInfo[i];

                    if (layout.SizeMode == SizeMode.Fill)
                    {
                        if (groupStart == -1)
                        {
                            groupStart = i;
                        }
                    }
                    else
                    {
                        if (groupStart > -1)
                        {
                            yield return new FilledLayoutGroup(groupStart, i - 1);
                            groupStart = -1;
                        }
                    }
                }
                else
                {
                    if (groupStart > -1)
                    {
                        yield return new FilledLayoutGroup(groupStart, i - 1);
                        groupStart = -1;
                    }
                }
            }
        }

        protected override void ArrangeOverride()
        {
            // Pass 1: Get layout information for each control.
            var layoutInfo = GetLayoutInfo().ToList();

            // Pass 2: Group all controls that fill the remainder of this widget's space.
            var fillGroups = GetFilledGroups(layoutInfo).ToList();

            // Pass 3: Figure out how big each fill group should be and where they go.
            if (fillGroups.Count > 0)
            {
                int fillGroupSize = ((Orientation == Orientation.Horizontal) ? ContentBounds.Width : ContentBounds.Height) / fillGroups.Count;
                for (int i = 0; i < fillGroups.Count; i++)
                {
                    int top = (Orientation == Orientation.Horizontal) ? ContentBounds.Left : ContentBounds.Top;
                    top += (fillGroupSize + Spacing) * i;

                    var group = fillGroups[i];
                    group.StartPos = top;
                    group.EndPos = top + fillGroupSize;
                    fillGroups[i] = group;
                }

                // Pass 4: Make room for auto-sized widgets.
                for (int i = 0; i < fillGroups.Count; i++)
                {
                    var group = fillGroups[i];

                    int beforeAccum = 0;
                    for (int j = group.StartIndex - 1; j >= 0; j--)
                    {
                        var layout = layoutInfo[j];
                        if (layout.SizeMode == SizeMode.Auto)
                        {
                            beforeAccum += (int)layout.Size + Spacing;
                        }
                        else break;
                    }

                    group.StartPos += beforeAccum;

                    // Special case: Last fill group gets shrunk at the end if there are children after it.
                    if (i == fillGroups.Count - 1)
                    {
                        int afterAccum = 0;
                        for (int j = group.EndIndex + 1; j < layoutInfo.Count; j++)
                        {
                            var layout = layoutInfo[j];

                            if (layout.SizeMode == SizeMode.Auto)
                            {
                                afterAccum += (int)layout.Size + Spacing;
                            }
                            else break;
                        }

                        group.EndPos -= afterAccum;
                    }

                    fillGroups[i] = group;
                }
            }

            // Pass 5: Calculate bounds for fill controls.
            foreach(var group in fillGroups)
            {
                int individualSize = ((group.TotalSize - (Spacing * group.Count))) / group.Count;
                
                for(int i = group.StartIndex; i <= group.EndIndex; i++)
                {
                    var layout = layoutInfo[i];

                    int loc = group.StartPos + ((individualSize + Spacing) * (i - group.StartIndex));

                    if(Orientation == Orientation.Horizontal)
                    {
                        layout.Bounds.X = loc;
                        layout.Bounds.Y = ContentBounds.Top;
                        layout.Bounds.Width = individualSize;
                        layout.Bounds.Height = ContentBounds.Height;
                    }
                    else
                    {
                        layout.Bounds.X = ContentBounds.Left;
                        layout.Bounds.Y = loc;
                        layout.Bounds.Width = ContentBounds.Width;
                        layout.Bounds.Height = individualSize;
                    }

                    layoutInfo[i] = layout;
                }
            }

            // Pass 5: Calculate bounds for all other controls.
            int cloc = (Orientation == Orientation.Horizontal) ? ContentBounds.Left : ContentBounds.Top;
            for(int i = 0; i < layoutInfo.Count; i++)
            {
                var layout = layoutInfo[i];

                if(layout.SizeMode == SizeMode.Auto)
                {
                    if(Orientation == Orientation.Horizontal)
                    {
                        layout.Bounds.X = cloc;
                        layout.Bounds.Y = ContentBounds.Top;
                        layout.Bounds.Width = (int)layout.Size;
                        layout.Bounds.Height = ContentBounds.Height;
                    }
                    else
                    {
                        layout.Bounds.X = ContentBounds.Left;
                        layout.Bounds.Y = cloc;
                        layout.Bounds.Width = ContentBounds.Width;
                        layout.Bounds.Height = (int)layout.Size;
                    }
                }

                if(Orientation == Orientation.Horizontal)
                {
                    cloc += layout.Bounds.Width + Spacing;
                }
                else
                {
                    cloc += layout.Bounds.Height + Spacing;
                }

                layoutInfo[i] = layout;
            }

            // Final pass: Place each control.
            foreach(var layout in layoutInfo)
            {
                PlaceControl(layout.Control, layout.Bounds);
            }
        }

        public struct FilledLayoutGroup
        {
            public int StartIndex;
            public int EndIndex;
            public int StartPos;
            public int EndPos;

            public int TotalSize => EndPos - StartPos;

            public int Count => (EndIndex - StartIndex) + 1;

            public FilledLayoutGroup(int start, int end)
            {
                StartIndex = start;
                EndIndex = end;
                StartPos = 0;
                EndPos = 0;
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
