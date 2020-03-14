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
        private ImageBox myImage = null;

        protected override void OnInitialize()
        {
            var rootStacker = new StackPanel();
            var aboutStacker = new StackPanel();
            var listStacker = new StackPanel();
            var checker = new CheckBox();

            checker.Content = new Label("Enable MonoGame Logo?");

            rootStacker.Orientation = Orientation.Horizontal;
            rootStacker.HorizontalAlignment = HorizontalAlignment.Center;
            rootStacker.VerticalAlignment = VerticalAlignment.Middle;

            rootStacker.Children.Add(aboutStacker);
            rootStacker.Children.Add(listStacker);

            var aboutTitle = new Label("Nucleus - Test UI");
            var aboutBody = new Label("This is a tiny test UI for Nucleus, my MonoGame-based virtual unix-like operating environment.  This UI is a test of the user interface system I'm developing for the graphical shell.  It is a light-weight container-based GUI system.");
            var textBox = new TextEntry();
            myImage = new ImageBox();
            var myButton = new Button();

            myImage.Image = ContentManager.Load<Texture2D>("MgBranding/HorizontalLogo_128px");

            myButton.Content = new Label("Click me!");

            aboutTitle.HorizontalAlignment = HorizontalAlignment.Center;
            aboutBody.WrapWidth = 384;
            textBox.MaxWidth = 384;

            aboutStacker.Children.Add(aboutTitle);
            aboutStacker.Children.Add(aboutBody);
            aboutStacker.Children.Add(myImage);
            aboutStacker.Children.Add(checker);
            aboutStacker.Children.Add(textBox);

            checker.Content.VerticalAlignment = VerticalAlignment.Middle;

            listStacker.Children.Add(new Label("Below is a list box."));

            var listBox = new ListBox();

            listBox.Items.Add("Item 1");
            listBox.Items.Add("Item 2");
            listBox.Items.Add("Item 3");

            listStacker.Children.Add(listBox);
            listStacker.Children.Add(myButton);

            myButton.Content.HorizontalAlignment = HorizontalAlignment.Center;
            myButton.Content.VerticalAlignment = VerticalAlignment.Middle;

            rootStacker.Spacing = 15;
            aboutStacker.Spacing = 15;
            listStacker.Spacing = 15;

            listStacker.Enabled = false;

            myImage.Collapsed = true;

            checker.CheckedChanged += HandleCheckerCheckedChanged;
            checker.Checked = !myImage.Collapsed;

            this.Content = rootStacker;
            base.OnInitialize();
        }

        private void HandleCheckerCheckedChanged(object sender, EventArgs e)
        {
            var checker = sender as CheckBox;

            myImage.Collapsed = !checker.Checked;
        }

        protected override void OnDraw(GameTime gameTime)
        {
            FillRectangle(Bounds, new Color(22, 22, 22, 255));
            base.OnDraw(gameTime);
        }
    }
}
