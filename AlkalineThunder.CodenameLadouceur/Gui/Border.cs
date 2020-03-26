using AlkalineThunder.Nucleus.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlkalineThunder.Nucleus.Gui
{
    public sealed class Border : ContentControl
    {
        public Color Color { get; set; } = Color.White;
        public Texture2D Image { get; set; } = null;
        public BrushType BorderType { get; set; } = BrushType.Image;
        public Padding BrushPadding { get; set; } = 0;

        protected override void OnDraw(GameTime gameTime)
        {
            DrawBrush(Bounds, new Brush(Image, Color, BrushPadding, BorderType));
        }
    }
}
