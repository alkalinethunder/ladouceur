using AlkalineThunder.Nucleus.Gui;
using AlkalineThunder.Nucleus.Rendering;
using AlkalineThunder.Nucleus.Screens;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlkalineThunder.Nucleus.Windowing
{
    public class Window : IContentControl
    {
        private Border _rootBorder = new Border();
        private StackPanel _mainStacker = new StackPanel();
        private StackPanel _titleStacker = new StackPanel();
        private ImageBox _titleIcon = new ImageBox();
        private Label _titleText = new Label();
        private StackPanel _titleButtons = new StackPanel();
        private Button _closeButton = new Button();
        private Button _minButton = new Button();
        private Button _maxButton = new Button();
        private Border _clientArea = new Border();
        private bool _dragging = false;

        public Control Content
        {
            get => _clientArea.Content;
            set => _clientArea.Content = value;
        }

        private Control WindowBorder => _rootBorder;

        public int X { get; set; } = 0;
        public int Y { get; set; } = 0;
        
        public WindowState WindowState { get; set; } = WindowState.Normal;
        public string Title
        {
            get => _titleText.Text;
            set => _titleText.Text = value;
        }

        public Window()
        {
            _rootBorder.Content = _mainStacker;
            _mainStacker.Children.Add(_titleStacker);
            _mainStacker.Children.Add(_clientArea);

            _clientArea.MinWidth = 300;
            _clientArea.MinHeight = 300;

            _mainStacker.SetSizeMode(_clientArea, SizeMode.Fill);

            _titleStacker.Children.Add(_titleIcon);
            _titleStacker.Children.Add(_titleText);
            _titleStacker.Children.Add(_titleButtons);

            _titleStacker.SetSizeMode(_titleText, SizeMode.Fill);

            _titleButtons.Children.Add(_minButton);
            _titleButtons.Children.Add(_maxButton);
            _titleButtons.Children.Add(_closeButton);

            _titleStacker.Orientation = Orientation.Horizontal;

            _titleButtons.Orientation = Orientation.Horizontal;

            _titleStacker.Spacing = 4;

            _titleButtons.Spacing = 2;

            _titleText.VerticalAlignment = VerticalAlignment.Middle;

            _closeButton.Content = new Label("X");
            _maxButton.Content = new Label("[]");
            _minButton.Content = new Label("_");

            _maxButton.Click += MaximizeHandler;

            _titleStacker.MouseDown += StartDrag;
            _titleStacker.MouseUp += EndDrag;

            // GUI system can't do this due to reasons.  Why? BEcause reasons.  Is it your fault?
            // No, chill.  Otherwise I'll have to cancle you.  Fuck I need to stop quoting that guy...
            GameLoop.Instance.Input.MouseMove += Drag;
        }

        private void Drag(object sender, Input.MouseMoveEventArgs e)
        {
            if(_dragging)
            {
                X += e.MovementX;
                Y += e.MovementY;
            }
        }

        private void EndDrag(object sender, Input.MouseButtonEventArgs e)
        {
            _dragging = false;
        }

        private void StartDrag(object sender, Input.MouseButtonEventArgs e)
        {
            if(e.Button == Input.MouseButton.Left && WindowState == WindowState.Normal)
            {
                _dragging = true;
            }
        }

        private void MaximizeHandler(object sender, Input.MouseButtonEventArgs e)
        {
            if(WindowState == WindowState.Normal)
            {
                WindowState = WindowState.Maximized;
            }
            else
            {
                WindowState = WindowState.Normal;
            }
        }

        public void Update(Screen owningScreen, GameTime gameTime)
        {
            if(WindowState == WindowState.Maximized)
            {
                Control.PlaceControl(WindowBorder, owningScreen.WindowBounds);
            }
            else if(WindowState == WindowState.Normal)
            {
                var center = owningScreen.WindowBounds.Center;
                var winPos = center + new Point(X, Y);

                var winSize = WindowBorder.CalculateSize();

                var winPosActual = winPos.ToVector2() - new Vector2(winSize.X / 2, winSize.Y / 2);

                Control.PlaceControl(WindowBorder, new Rectangle(
                        (int)winPosActual.X,
                        (int)winPosActual.Y,
                        (int)winSize.X,
                        (int)winSize.Y
                    ));
            }
            WindowBorder.Update(gameTime);
        }

        public Control FindControl(int x, int y)
        {
            return WindowBorder.FindControl(x, y);
        }

        public void Draw(GameTime gameTime, Renderer renderer)
        {
            WindowBorder.Draw(gameTime, renderer);
        }
    }

    public enum WindowState
    {
        Normal,
        Maximized,
        Minimized
    }
}
