using AlkalineThunder.Nucleus.Gui;
using AlkalineThunder.Nucleus.Rendering;
using AlkalineThunder.Nucleus.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace AlkalineThunder.Nucleus.Windowing
{
    public class Window : IContentControl
    {
        private bool _isContentLoaded = false;

        public class WindowCollection : IReorderable<Window>
        {
            private List<Window> _windows = new List<Window>();
            private Screen _owner = null;

            public WindowCollection(Screen screen)
            {
                _owner = screen ?? throw new ArgumentNullException(nameof(screen));
            }

            public int Count => _windows.Count;

            public bool IsReadOnly => false;

            public void BringToFront(Window item)
            {
                if(Contains(item))
                {
                    _windows.Remove(item);
                    _windows.Add(item);
                }
            }

            public void SendToBack(Window item)
            {
                if(Contains(item))
                {
                    _windows.Remove(item);
                    _windows.Insert(0, item);
                }
            }

            public void Add(Window item)
            {
                if (item == null) throw new ArgumentNullException(nameof(item));
                if (item.Screen != null) throw new InvalidOperationException("Window already belongs to this screen.");

                _windows.Add(item);
                item.Screen = this._owner;

                if(_owner.ContentManager != null && !item._isContentLoaded)
                {
                    item.LoadContent();
                    item._isContentLoaded = true;
                }
            }

            public void Clear()
            {
                while(_windows.Count > 0)
                {
                    Remove(_windows[0]);
                }
            }

            public bool Contains(Window item)
            {
                return item != null && item.Screen == _owner;
            }

            public void CopyTo(Window[] array, int arrayIndex)
            {
                _windows.CopyTo(array, arrayIndex);
            }

            public IEnumerator<Window> GetEnumerator()
            {
                return _windows.GetEnumerator();
            }

            public Window this[int index]
            {
                get => this._windows[index];
            }

            public bool Remove(Window item)
            {
                if(Contains(item))
                {
                    item.Screen = null;
                    _windows.Remove(item);
                    return true;
                }
                else
                {
                    return false;
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return _windows.GetEnumerator();
            }
        }

        private Border _rootBorder = new Border();
        private Border _titleBackground = new Border();
        private StackPanel _mainStacker = new StackPanel();
        private StackPanel _titleStacker = new StackPanel();
        private ImageBox _titleIcon = new ImageBox();
        private Label _titleText = new Label();
        private StackPanel _titleButtons = new StackPanel();
        private ImageBox _closeButton = new ImageBox();
        private ImageBox _minButton = new ImageBox();
        private ImageBox _maxButton = new ImageBox();
        private Border _clientArea = new Border();
        private bool _dragging = false;

        private Overlay _titleOverlay = new Overlay();
        private StackPanel _horizBorderStacker = new StackPanel();

        private ImageBox _leftBorder = new ImageBox();
        private ImageBox _rightBorder = new ImageBox();

        private ImageBox _blBorder = new ImageBox();
        private ImageBox _brBorder = new ImageBox();
        private ImageBox _bottomBorder = new ImageBox();

        private ImageBox _trCorner = new ImageBox();
        private ImageBox _tlCorner = new ImageBox();
        private ImageBox _titleLeft = new ImageBox();
        private ImageBox _titleRight = new ImageBox();

        private StackPanel _bottomStacker = new StackPanel();

        public Screen Screen { get; private set; }

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

        public bool IsActive => Screen.Windows[Screen.Windows.Count - 1] == this && WindowBorder.HasAnyFocus;

        public Window()
        {
            _rootBorder.Content = _mainStacker;
            _mainStacker.Children.Add(_titleOverlay);
            _mainStacker.Children.Add(_horizBorderStacker);
            _mainStacker.Children.Add(_bottomStacker);

            _bottomStacker.Orientation = Orientation.Horizontal;

            _bottomStacker.Children.Add(_blBorder);
            _bottomStacker.Children.Add(_bottomBorder);
            _bottomStacker.Children.Add(_brBorder);

            _bottomStacker.SetSizeMode(_bottomBorder, SizeMode.Fill);

            _clientArea.MinWidth = 300;
            _clientArea.MinHeight = 300;

            _mainStacker.SetSizeMode(_horizBorderStacker, SizeMode.Fill);

            _horizBorderStacker.Children.Add(_leftBorder);
            _horizBorderStacker.Children.Add(_clientArea);
            _horizBorderStacker.Children.Add(_rightBorder);

            _horizBorderStacker.SetSizeMode(_clientArea, SizeMode.Fill);

            _titleOverlay.Children.Add(_titleBackground);
            _titleOverlay.Children.Add(_titleLeft);
            _titleOverlay.Children.Add(_tlCorner);
            _titleOverlay.Children.Add(_titleRight);
            _titleOverlay.Children.Add(_trCorner);
            _titleOverlay.Children.Add(_titleStacker);

            _titleLeft.HorizontalAlignment = HorizontalAlignment.Left;
            _titleRight.HorizontalAlignment = HorizontalAlignment.Right;
            _tlCorner.VerticalAlignment = VerticalAlignment.Top;
            _trCorner.VerticalAlignment = VerticalAlignment.Top;
            _tlCorner.HorizontalAlignment = HorizontalAlignment.Left;
            _trCorner.HorizontalAlignment = HorizontalAlignment.Right;

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

            _maxButton.Click += MaximizeHandler;

            _titleOverlay.MouseDown += StartDrag;
            _titleOverlay.MouseUp += EndDrag;

            _titleStacker.VerticalAlignment = VerticalAlignment.Middle;
            _titleStacker.Padding = new Padding(8, 0);

            // GUI system can't do this due to reasons.  Why? BEcause reasons.  Is it your fault?
            // No, chill.  Otherwise I'll have to cancle you.  Fuck I need to stop quoting that guy...
            GameLoop.Instance.Input.MouseMove += Drag;

            _closeButton.Click += HandleClose;
            _minButton.Click += HGandleMinimize;
        }

        private void HGandleMinimize(object sender, Input.MouseButtonEventArgs e)
        {
            WindowState = WindowState.Minimized;
        } 

        private void HandleClose(object sender, Input.MouseButtonEventArgs e)
        {
            Screen.Windows.Remove(this);
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
                Screen.Windows.BringToFront(this);
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
            if (WindowState != WindowState.Minimized)
            {
                if (!_isContentLoaded)
                {
                    if (Screen.ContentManager != null)
                    {
                        LoadContent();
                        _isContentLoaded = true;
                    }
                }

                if (WindowState == WindowState.Maximized)
                {
                    Control.PlaceControl(WindowBorder, owningScreen.WindowBounds);
                }
                else if (WindowState == WindowState.Normal)
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

                _clientArea.Color = Screen.ActiveTheme.WindowBackground;

                SetColors(IsActive ? Screen.ActiveTheme.WindowActiveColor : Screen.ActiveTheme.WindowInactiveColor);

                _rootBorder.Color = Color.Transparent;
            }
        }

        public Control FindControl(int x, int y)
        {
            return WindowState == WindowState.Minimized ? null : WindowBorder.FindControl(x, y);
        }

        private void LoadContent()
        {
            _closeButton.Image = Screen.ContentManager.Load<Texture2D>("Textures/WindowBorder/Close");
            _maxButton.Image = Screen.ContentManager.Load<Texture2D>("Textures/WindowBorder/Max");
            _minButton.Image = Screen.ContentManager.Load<Texture2D>("Textures/WindowBorder/Min");

            _closeButton.VerticalAlignment = VerticalAlignment.Middle;
            _maxButton.VerticalAlignment = VerticalAlignment.Middle;
            _minButton.VerticalAlignment = VerticalAlignment.Middle;

            _leftBorder.Image = Screen.ContentManager.Load<Texture2D>("Textures/WindowBorder/LSide");
            _rightBorder.Image = Screen.ContentManager.Load<Texture2D>("Textures/WindowBorder/RSide");

            _horizBorderStacker.Orientation = Orientation.Horizontal;

            _bottomBorder.Image = Screen.ContentManager.Load<Texture2D>("Textures/WindowBorder/WinBottom");

            _blBorder.Image = Screen.ContentManager.Load<Texture2D>("Textures/WindowBorder/BLCorner");
            _brBorder.Image = Screen.ContentManager.Load<Texture2D>("Textures/WindowBorder/BRCorner");

            _titleBackground.Image = Screen.ContentManager.Load<Texture2D>("Textures/WindowBorder/WinTop");
            _titleBackground.BorderType = BrushType.Box;
            _titleBackground.BrushPadding = 6;

            _titleLeft.Image = _leftBorder.Image;
            _titleRight.Image = _rightBorder.Image;

            _tlCorner.Image = Screen.ContentManager.Load<Texture2D>("Textures/WindowBorder/TLCorner");
            _trCorner.Image = Screen.ContentManager.Load<Texture2D>("Textures/WindowBorder/TRCorner");
        }

        private void SetColors(Color color)
        {
            _trCorner.Tint = color;
            _tlCorner.Tint = color;
            _titleLeft.Tint = color;
            _titleRight.Tint = color;

            _titleBackground.Color = color;

            _leftBorder.Tint = color;
            _rightBorder.Tint = color;
            _bottomBorder.Tint = color;

            _brBorder.Tint = color;
            _blBorder.Tint = color;
        }

        public void Draw(GameTime gameTime, Renderer renderer)
        {
            if (WindowState != WindowState.Minimized)
            {
                WindowBorder.Draw(gameTime, renderer);
            }
        }
    }

    public enum WindowState
    {
        Normal,
        Maximized,
        Minimized
    }
}
