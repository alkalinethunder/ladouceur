using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlkalineThunder.CodenameLadouceur.Gui
{
    public sealed class Button : ContentControl
    {
        public Button()
        {
            Content = new Label("Button Text");
            Margin = new Padding(70, 40);
        }

        public int BorderThickness { get; set; } = 2;

        protected override Vector2 MeasureOverride()
        {
            if(Content != null)
            {
                var measure = Content.CalculateSize();
                return new Vector2(measure.X + (BorderThickness * 2), measure.Y + (BorderThickness * 2));
            }
            else
            {
                return new Vector2(BorderThickness * 2, BorderThickness * 2);
            }
        }

        protected override void OnDraw(GameTime gameTime)
        {
            DrawRectangle(Bounds, Color.White, BorderThickness);
        }
    }
}
