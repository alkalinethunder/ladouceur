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
        public Brush Brush { get; set; } = Brush.Image.InColor(Color.White);

        protected override void OnDraw(GameTime gameTime)
        {
            DrawBrush(Bounds, Brush);
        }
    }
}
