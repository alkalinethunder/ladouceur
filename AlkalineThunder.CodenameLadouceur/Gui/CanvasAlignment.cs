using Microsoft.Xna.Framework;
using System;

namespace AlkalineThunder.Nucleus.Gui
{
    public struct CanvasAlignment
    {
        public float LeftValue;
        public float TopValue;

        public CanvasAlignment(float left, float top)
        {
            LeftValue = left;
            TopValue = top;
        }

        public static implicit operator CanvasAlignment(Vector2 vector)
        {
            return new CanvasAlignment(vector.X, vector.Y);
        }

        public static bool operator ==(CanvasAlignment a, CanvasAlignment b)
        {
            return (a.LeftValue == b.LeftValue) && (a.TopValue == b.TopValue);
        }

        public static bool operator !=(CanvasAlignment a, CanvasAlignment b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            return obj is CanvasAlignment ca && ca == this;
        }

        public override string ToString()
        {
            return $"(Left={LeftValue}, Top={TopValue})";
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(LeftValue, TopValue);
        }

        public static CanvasAlignment TopLeft => new CanvasAlignment(0, 0);
        public static CanvasAlignment Top => new CanvasAlignment(0.5f, 0);
        public static CanvasAlignment TopRight => new CanvasAlignment(1, 0);
        public static CanvasAlignment Left => new CanvasAlignment(0, 0.5f);
        public static CanvasAlignment Center => new CanvasAlignment(0.5f, 0.5f);
        public static CanvasAlignment Right => new CanvasAlignment(1, 0.5f);
        public static CanvasAlignment BottomLeft => new CanvasAlignment(0, 1);
        public static CanvasAlignment Bottom => new CanvasAlignment(0.5f, 1);
        public static CanvasAlignment BottomRight => new CanvasAlignment(1, 1);
    }
}
