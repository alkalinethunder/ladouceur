using AlkalineThunder.CodenameLadouceur.Gui;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private StackPanel _screensStacker = new StackPanel();
        private Button _shellTest = new Button();
        private StackPanel _scrollerTest = new StackPanel();
        private ScrollPanel _scroller = new ScrollPanel();
        private StackPanel _scrollerContent = new StackPanel();
        private WrapBox _wrapperTest = new WrapBox();
        private Texture2D _cogWheel = null;

        protected override void OnInitialize()
        {
            var canvas = new CanvasPanel();

            _cogWheel = ContentManager.Load<Texture2D>("CogWheel");

            canvas.Margin = 25;

            canvas.Children.Add(_screensStacker);
            canvas.Children.Add(_infoStacker);
            canvas.Children.Add(_buttons);
            canvas.Children.Add(_scrollerTest);
            canvas.Children.Add(_wrapperTest);

            canvas.SetAnchor(_wrapperTest, CanvasAlignment.Center);
            canvas.SetAlignment(_wrapperTest, CanvasAlignment.Center);

            for(int i = 0; i < 25; i++)
            {
                var lbl = new Label("This is element number " + (i + 1) + " of the wrap box.");
                lbl.WrapWidth = 100;
                _wrapperTest.Children.Add(lbl);
            }

            _wrapperTest.HorizontalSpacing = 15;
            _wrapperTest.VerticalSpacing = 10;

            _scrollerTest.Children.Add(new Label("Scroll Panel Test:"));
            _scrollerTest.Children.Add(_scroller);

            _scroller.Content = _scrollerContent;

            _scrollerTest.SetSizeMode(_scroller, SizeMode.Fill);

            for(int i = 0; i < 100; i++)
            {
                _scrollerContent.Children.Add(new Label("Scroller content " + i));
            }

            canvas.SetAutoSize(_scrollerTest, false);
            canvas.SetWidth(_scrollerTest, 384);
            canvas.SetHeight(_scrollerTest, 500);
            canvas.SetAnchor(_scrollerTest, CanvasAlignment.TopRight);
            canvas.SetAlignment(_scrollerTest, CanvasAlignment.TopRight);

            _scrollerTest.Children.Add(new TextEntry
            {
                HintText = "Text Entry Test"
            });
            _scrollerTest.Children.Add(new TextEntry
            {
                HintText = "Password Entry Test",
                IsPassword = true
            });

            _screensStacker.Children.Add(new Label("Other screens: "));
            _screensStacker.Children.Add(_shellTest);

            _shellTest.Content = new Label("Shell test");

            _screensStacker.Spacing = 10;

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

            _github.Click += GitHubClick;
            _exit.Click += ExitClick;

            _shellTest.Click += ShellTestClick;

            var win = OpenWindow("Hello world!");
            win.Content = new Label("This is a window.");

            Content = canvas;
            base.OnInitialize();
        }

        private void ShellTestClick(object sender, Input.MouseButtonEventArgs e)
        {
            AddScreen<ShellScreen>();
            RemoveScreen();
        }

        private void ExitClick(object sender, Input.MouseButtonEventArgs e)
        {
            Exit();
        }

        private void GitHubClick(object sender, Input.MouseButtonEventArgs e)
        {
            var info = new ProcessStartInfo("https://github.com/alkalinethunder/ladouceur");
            info.UseShellExecute = true;
            Process.Start(info);
        }

        protected override void OnDraw(GameTime gameTime)
        {
            Clear(new Color(22, 22, 22));

            FillCircle(new Vector2(200, 200), 50, _cogWheel, Color.White);

            base.OnDraw(gameTime);
        }
    }
}
