using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Text;

namespace AlkalineThunder.CodenameLadouceur.Gui
{
    public sealed class CanvasPanel : LayoutControl
    {
        private static readonly string WidthProp = "Width";
        private static readonly string HeightProp = "Height";
        private static readonly string AutoSizeProp = "AutoSize";
        private static readonly string AlignmentProp = "Alignment";
        private static readonly string LocationProp = "Location";

        public Vector2 GetLocation(Control control)
        {
            if(control.GetProperty<Vector2>(LocationProp, out Vector2 location))
            {
                return location;
            }
            else
            {
                return Vector2.Zero;
            }
        }

        public int GetWidth(Control control)
        {
            if(control.GetProperty(WidthProp, out int width))
            {
                return width;
            }
            else
            {
                return 0;
            }
        }

        public int GetHeight(Control control)
        {
            if(control.GetProperty(HeightProp, out int height))
            {
                return height;
            }
            else
            {
                return 0;
            }
        }

        public bool GetAutoSize(Control control)
        {
            if (control.GetProperty(AutoSizeProp, out bool autoSize))
            {
                return autoSize;
            }
            else
            {
                return false;
            }
        }

        public CanvasAlignment GetAlignment(Control control)
        {
            if(control.GetProperty(AlignmentProp, out CanvasAlignment alignment))
            {
                return alignment;
            }
            else
            {
                return CanvasAlignment.TopLeft;
            }
        }

        public void SetAutoSize(Control control, bool value)
        {
            control.SetAttachedProperty(AutoSizeProp, value);
        }

        public void SetWidth(Control control, int width)
        {
            control.SetAttachedProperty(WidthProp, width);
        }

        public void SetHeight(Control control, int height)
        {
            control.SetAttachedProperty(HeightProp, height);
        }

        public void SetAlignment(Control control, CanvasAlignment alignment)
        {
            control.SetAttachedProperty(AlignmentProp, alignment);
        }

        public void SetLocation(Control control, Vector2 location)
        {
            control.SetAttachedProperty(LocationProp, location);
        }

        protected override Vector2 MeasureOverride()
        {
            return Vector2.Zero;
        }

        protected override void ArrangeOverride()
        {
            foreach(var child in InternalChildren)
            {
                var autosize = GetAutoSize(child);
                var loc = GetLocation(child);
                var width = GetWidth(child);
                var height = GetHeight(child);
                var alignment = GetAlignment(child);

                var actualSize = autosize ? new Vector2(width, height) : child.DesiredSize;

                var alignOffset = new Vector2(actualSize.X * alignment.LeftValue, actualSize.Y * alignment.TopValue);

                var alignedLocation = loc - alignOffset;

                PlaceControl(child, new Rectangle(
                        ContentBounds.Left + (int)alignedLocation.X,
                        ContentBounds.Top + (int)alignedLocation.Y,
                        (int)actualSize.X,
                        (int)actualSize.Y
                    ));


            }
        }
    }
}
