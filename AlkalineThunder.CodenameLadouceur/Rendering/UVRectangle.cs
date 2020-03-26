﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlkalineThunder.Nucleus.Rendering
{
    public struct UVRectangle
    {
        public float U;
        public float V;
        public float Width;
        public float Height;

        public UVRectangle(float u, float v, float width, float height)
        {
            U = u;
            V = v;
            Width = width;
            Height = height;
        }

        public float Left => U;
        public float Top => V;
        public float Right => Left + Width;
        public float Bottom => Top + Height;

        public Vector2 Center => new Vector2(Left + (Width / 2), Top + (Height / 2));

        public Vector2 Location => new Vector2(U, V);

        public static UVRectangle Unit => new UVRectangle(0, 0, 1, 1);

        public static bool operator ==(UVRectangle a, UVRectangle b)
        {
            return a.U == b.U && a.V == b.V && a.Width == b.Width && a.Height == b.Height;
        }

        public static bool operator !=(UVRectangle a, UVRectangle b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            return obj is UVRectangle u && this == u;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(U, V, Width, Height);
        }

        public override string ToString()
        {
            return $"({U},{V},{Width},{Height})";
        }

        public static UVRectangle Map(Rectangle outerRect, Rectangle innerRect)
        {
            // this is what the outer rect is considered to be.
            var unit = Unit;

            // Size is inner rect / outer rect.
            unit.Width = (float)innerRect.Width / (float)outerRect.Width;
            unit.Height = (float)innerRect.Height / (float)outerRect.Height;

            // Get inner position relative to outer rect.
            var relX = innerRect.X - outerRect.Left;
            var relY = innerRect.Y - outerRect.Top;

            // UV coords are relative position / outer size.
            unit.U = (float)relX / (float)outerRect.Width;
            unit.V = (float)relY / (float)outerRect.Height;

            return unit;
        }
    }
}
