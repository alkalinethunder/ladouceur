using AlkalineThunder.Nucleus.Screens;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlkalineThunder.Nucleus.Demo
{
    public sealed class DemoScreen : Screen
    {
        protected override void OnDraw(GameTime gameTime)
        {
            FillRectangle(Bounds, new Color(22, 22, 22));
        }
    }
}
