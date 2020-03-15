using AlkalineThunder.CodenameLadouceur.Gui;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlkalineThunder.CodenameLadouceur.Screens
{
    public sealed class AboutScreen : Screen
    {
        private StackPanel _infoStacker = new StackPanel();
        private Label _nucleusText = new Label("Nucleus\r\nUI Test Screen");
        
        protected override void OnInitialize()
        {
            var canvas = new CanvasPanel();

            canvas.Margin = 25;

            canvas.Children.Add(_infoStacker);

            _infoStacker.Children.Add(_nucleusText);

            canvas.SetAnchor(_infoStacker, CanvasAlignment.BottomLeft);
            canvas.SetAlignment(_infoStacker, CanvasAlignment.BottomLeft);

            Content = canvas;
            base.OnInitialize();
        }

        protected override void OnDraw(GameTime gameTime)
        {
            FillRectangle(Bounds, new Color(22, 22, 22, 255));
            base.OnDraw(gameTime);
        }
    }
}
