using AlkalineThunder.CodenameLadouceur.Gui;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlkalineThunder.CodenameLadouceur.Screens
{
    public sealed class AboutScreen : Screen
    {
        protected override void OnUpdate(GameTime gameTime)
        {
            if(Content == null)
            {
                Content = new Border();
                var b = Content as Border;

                b.BackgroundColor = Color.CornflowerBlue;

                this.Padding = 25;
                this.Margin = 25;
            }
            base.OnUpdate(gameTime);
        }

        protected override void OnDraw(GameTime gameTime)
        {
            FillRectangle(Bounds, new Color(22, 22, 22, 255));
            base.OnDraw(gameTime);
        }
    }
}
