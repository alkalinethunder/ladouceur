using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlkalineThunder.Nucleus.Gui
{
    public sealed class Border : ContentControl
    {
        public Color BackgroundColor { get; set; } = Color.Gray;

        protected override void OnDraw(GameTime gameTime)
        {
            FillRectangle(Bounds, BackgroundColor);
        }
    }
}
