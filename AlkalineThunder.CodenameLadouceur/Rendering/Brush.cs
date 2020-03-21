using System;
using System.Collections.Generic;
using System.Text;
using AlkalineThunder.Nucleus.Gui;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AlkalineThunder.Nucleus.Rendering
{
    public struct Brush
    {
        public Color BrushColor;
        public Padding Margin;
        public Texture2D Texture;
        public BrushType BrushType;

        public Brush(Texture2D texture, Color color, Padding padding, BrushType type)
        {
            BrushColor = color;
            Texture = texture;
            Margin = padding;
            BrushType = type;
        }

        public Brush(Color color) : this(null, color, 0, BrushType.Image)
        {

        }

        public Brush(Texture2D texture, Color color, Padding padding) : this(texture,color,padding, BrushType.Box)
        {

        }

        public Brush(Texture2D texture, Color color) : this(texture, color, 0, BrushType.Image)
        {

        }

        public Brush(Texture2D texture, Padding padding, BrushType type) : this(texture, Color.White, padding, type)
        {

        }

        public Brush(Texture2D texture, Padding padding) : this(texture, padding, BrushType.Box)
        {

        }

        public Brush(Color color, Padding padding) : this(null, color, padding, BrushType.Box)
        {

        }

        public Brush(Texture2D texture) : this(texture, 0, BrushType.Image)
        {

        }

        public static Brush None => new Brush(null, Color.White, 0, BrushType.None);
        public static Brush Border => new Brush(null, Color.White, 1, BrushType.Border);
        public static Brush Box => new Brush(null, Color.White, 1, BrushType.Box);
        public static Brush Image => new Brush(null, Color.White, 0, BrushType.Image);


        public static implicit operator Texture2D(Brush brush)
        {
            return brush.Texture;
        }

        public static implicit operator Brush(Texture2D texture)
        {
            return new Brush(texture);
        }

        public static implicit operator Color(Brush brush)
        {
            return brush.BrushColor;
        }

        public static implicit operator Brush(Color color)
        {
            return new Brush(color);
        }

        public static bool operator ==(Brush a, Brush b)
        {
            return a.BrushColor == b.BrushColor && a.Texture == b.Texture && a.Margin == b.Margin && a.BrushType == b.BrushType;
        }

        public static bool operator !=(Brush a, Brush b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            return obj is Brush b && this == b;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(BrushColor, Texture, Margin, BrushType);
        }

        public override string ToString()
        {
            return $"(BrushColor={BrushColor}, Texture={Texture}, Margin={Margin}, BrushType={BrushType})";
        }

        public Brush InColor(Color color)
        {
            return new Brush(this.Texture, color, this.Margin, this.BrushType);
        }

        public Brush Pad(Padding padding)
        {
            return new Brush(Texture, BrushColor, padding, BrushType);
        }

        public Brush WithTexture(Texture2D texture)
        {
            return new Brush(texture, BrushColor, Margin, BrushType);
        }

        public Brush AsType(BrushType type)
        {
            return new Brush(Texture, BrushColor, Margin, type);
        }
    }
}
