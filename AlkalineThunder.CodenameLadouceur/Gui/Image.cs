using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlkalineThunder.CodenameLadouceur.Gui
{
    public sealed class ImageBox : Control
    {
        public Texture2D Image { get; set; } = null;
        public Color Tint { get; set; } = Color.White;
        public float Scale { get; set; } = 1;

        protected override Vector2 MeasureOverride()
        {
            if(Image != null)
            {
                return new Vector2(Image.Width * Math.Abs(Scale), Image.Height * Math.Abs(Scale));
            }
            else
            {
                return Vector2.Zero;
            }
        }

        protected override void OnDraw(GameTime gameTime)
        {
            FillRectangle(Bounds, Image, Tint);
        }
    }
}
