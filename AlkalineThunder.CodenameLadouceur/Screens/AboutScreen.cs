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
                var stackPanel = new StackPanel();

                stackPanel.HorizontalAlignment = HorizontalAlignment.Center;
                stackPanel.VerticalAlignment = VerticalAlignment.Middle;

                stackPanel.Spacing = 15;

                var titleLabel = new Label();
                var bodyLabel = new Label();

                titleLabel.HorizontalAlignment = HorizontalAlignment.Center;
                bodyLabel.WrapWidth = 480;

                titleLabel.Text = "Welcome to Project: Ladouceur.";
                bodyLabel.Text = "Project: Ladouceur is a .NET Core and MonoGame-based user interface and game development framework written by Alkaline Thunder.";

                stackPanel.Children.Add(titleLabel);
                stackPanel.Children.Add(bodyLabel);

                this.Content = stackPanel;
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
