using AlkalineThunder.CodenameLadouceur.Gui;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlkalineThunder.CodenameLadouceur.Screens
{
    public sealed class ShellScreen : Screen
    {
        private Texture2D _wallpaper = null;
        private Color _wallpaperColor = Color.Black;

        private Border _desktopPanel = new Border();
        private StackPanel _desktopPanelItems = new StackPanel();
        private Label _time = new Label();
        private Button _exitButton = new Button();
        private Button _appsButton = new Button();
        private StackPanel _panelButtons = new StackPanel();

        protected override void OnInitialize()
        {
            Content = _desktopPanel;

            _desktopPanelItems.Orientation = Orientation.Horizontal;

            _desktopPanel.VerticalAlignment = VerticalAlignment.Top;

            _desktopPanel.Content = _desktopPanelItems;

            _desktopPanelItems.Spacing = 2;

            _desktopPanelItems.Margin = 4;

            _desktopPanelItems.Children.Add(_exitButton);
            _desktopPanelItems.Children.Add(_appsButton);
            _desktopPanelItems.Children.Add(_panelButtons);
            _desktopPanelItems.Children.Add(_time);

            _panelButtons.VerticalAlignment = VerticalAlignment.Middle;

            _panelButtons.Children.Add(new Label("This is a test."));

            _desktopPanelItems.SetSizeMode(_panelButtons, SizeMode.Fill);
            _desktopPanelItems.SetSizeMode(_appsButton, SizeMode.Fill);

            _exitButton.Content = new Label("<< Back");
            _appsButton.Content = new Label("Applications");

            _exitButton.Click += ExitButtonClick;
            _appsButton.Click += AppsButtonClick;

            _time.VerticalAlignment = VerticalAlignment.Middle;

            base.OnInitialize();
        }

        private void AppsButtonClick(object sender, Input.MouseButtonEventArgs e)
        {
        }

        private void ExitButtonClick(object sender, Input.MouseButtonEventArgs e)
        {
            AddScreen<AboutScreen>();
            RemoveScreen();
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            _time.Text = DateTime.Now.ToShortTimeString();
            base.OnUpdate(gameTime);
        }

        protected override void OnDraw(GameTime gameTime)
        {
            FillRectangle(Bounds, _wallpaperColor);

            if(_wallpaper != null)
            {
                FillRectangle(Bounds, _wallpaper, Color.White);
            }

            base.OnDraw(gameTime);
        }
    }
}
