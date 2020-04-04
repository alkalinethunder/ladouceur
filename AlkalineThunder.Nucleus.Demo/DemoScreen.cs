using AlkalineThunder.Nucleus.Gui;
using AlkalineThunder.Nucleus.Screens;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlkalineThunder.Nucleus.Demo
{
    public sealed class DemoScreen : Screen
    {
        protected override void OnInitialize()
        {
            var demoWindow = OpenWindow("Demo Window");

            var stacker = new StackPanel();

            stacker.Children.Add(new Label("Welcome to Nucleus!", ActiveTheme.TitleFont)
            {
                HorizontalAlignment = HorizontalAlignment.Center
            });
            stacker.Children.Add(new Label("If you are seeing this, then Nucleus is working correctly.  This is a demo of Nucleus' windowing system. Feel free to drag the window around, maximize it, minimize it, or do what you want with it.")
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                WrapWidth = 425
            });

            stacker.Spacing = 10;
            stacker.Padding = 15;

            demoWindow.Content = stacker;

            base.OnInitialize();
        }

        protected override void OnDraw(GameTime gameTime)
        {
            FillRectangle(Bounds, new Color(22, 22, 22));
        }
    }
}
