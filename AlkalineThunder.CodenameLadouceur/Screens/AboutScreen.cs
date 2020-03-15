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
        private StackPanel _buttons = new StackPanel();
        private Button _github = new Button();
        private Button _exit = new Button();


        protected override void OnInitialize()
        {
            var canvas = new CanvasPanel();

            canvas.Margin = 25;

            canvas.Children.Add(_infoStacker);
            canvas.Children.Add(_buttons);

            _buttons.Orientation = Orientation.Horizontal;
            _buttons.Spacing = 15;

            _buttons.Children.Add(_github);
            _buttons.Children.Add(_exit);

            _github.Content = new Label("Source code");
            _exit.Content = new Label("Exit");

            _github.Content.HorizontalAlignment = HorizontalAlignment.Center;
            _github.Content.VerticalAlignment = VerticalAlignment.Middle;

            _exit.Content.HorizontalAlignment = HorizontalAlignment.Center;
            _exit.Content.VerticalAlignment = VerticalAlignment.Middle;

            _infoStacker.Children.Add(_nucleusText);

            canvas.SetAnchor(_infoStacker, CanvasAlignment.BottomLeft);
            canvas.SetAlignment(_infoStacker, CanvasAlignment.BottomLeft);

            canvas.SetAnchor(_buttons, CanvasAlignment.BottomRight);
            canvas.SetAlignment(_buttons, CanvasAlignment.BottomRight);

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
