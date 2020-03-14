using AlkalineThunder.CodenameLadouceur.Gui;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlkalineThunder.CodenameLadouceur.Screens
{
    public sealed class AboutScreen : Screen
    {
        protected override void OnInitialize()
        {
            var rootStacker = new StackPanel();
            var aboutStacker = new StackPanel();
            var listStacker = new StackPanel();

            rootStacker.Orientation = Orientation.Horizontal;
            rootStacker.HorizontalAlignment = HorizontalAlignment.Center;
            rootStacker.VerticalAlignment = VerticalAlignment.Middle;

            rootStacker.Children.Add(aboutStacker);
            rootStacker.Children.Add(listStacker);

            var aboutTitle = new Label("Nucleus - Test UI");
            var aboutBody = new Label("This is a tiny test UI for Nucleus, my MonoGame-based virtual unix-like operating environment.  This UI is a test of the user interface system I'm developing for the graphical shell.  It is a light-weight container-based GUI system.");

            aboutTitle.HorizontalAlignment = HorizontalAlignment.Center;
            aboutBody.WrapWidth = 384;

            aboutStacker.Children.Add(aboutTitle);
            aboutStacker.Children.Add(aboutBody);

            listStacker.Children.Add(new Label("Below is a list box."));

            var listBox = new ListBox();

            listBox.Items.Add("Item 1");
            listBox.Items.Add("Item 2");
            listBox.Items.Add("Item 3");

            listStacker.Children.Add(listBox);

            rootStacker.Spacing = 15;
            aboutStacker.Spacing = 15;
            listStacker.Spacing = 15;

            this.Content = rootStacker;
            base.OnInitialize();
        }

        protected override void OnDraw(GameTime gameTime)
        {
            FillRectangle(Bounds, new Color(22, 22, 22, 255));
            base.OnDraw(gameTime);
        }
    }
}
