using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlkalineThunder.CodenameLadouceur.Gui
{
    public struct Padding
    {
        public int Left;
        public int Top;
        public int Bottom;
        public int Right;

        public Padding(int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        public Padding(int h, int v) : this(h, v, h, v) { }

        public Padding(int all) : this(all, all) { }

        public int Width => Left + Right;
        public int Height => Top + Bottom;

        public static implicit operator Point(Padding p)
        {
            return new Point(p.Width, p.Height);
        }

        public static implicit operator Vector2(Padding p)
        {
            return new Vector2(p.Width, p.Height);
        }

        public static implicit operator Padding(int all)
        {
            return new Padding(all);
        }

        public static implicit operator Padding(Point pt)
        {
            return new Padding(pt.X, pt.Y);
        }

        public static implicit operator Padding(Vector2 vector)
        {
            return new Padding((int)vector.X, (int)vector.Y);
        }

        public static bool operator ==(Padding a, Padding b)
        {
            return a.Left == b.Left && a.Right == b.Right && a.Top == b.Top && a.Bottom == b.Bottom;
        }

        public static bool operator !=(Padding a, Padding b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            return obj is Padding p && p == this;
        }

        public override string ToString()
        {
            return $"(Left={Left}, Top={Top}, Right={Right}, Bottom={Bottom})";
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Left, Top, Bottom, Right);
        }

        public Rectangle Affect(Rectangle rect)
        {
            return new Rectangle(
                rect.Left + Left,
                rect.Top + Top,
                rect.Width - Width,
                rect.Height - Height
                );
        }

    }
}
