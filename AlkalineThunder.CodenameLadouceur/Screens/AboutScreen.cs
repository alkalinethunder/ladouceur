using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlkalineThunder.CodenameLadouceur.Screens
{
    public sealed class AboutScreen : Screen
    {
        protected override void OnDraw(GameTime gameTime)
        {
            FillRectangle(new Rectangle(20, 20, 40, 40), Color.CornflowerBlue);
            base.OnDraw(gameTime);
        }
    }
}
