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

        public static WindowDecorator ActiveDecorator { get; private set; }

        public static void LoadDecorator<T>() where T : WindowDecorator, new()
        {
            ActiveDecorator = new T();
            ActiveDecorator.LoadContent(GameLoop.Instance.Content);
        }

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

                item.BindEvents();

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
                    item.UnbindEvents();
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
        private Border _closeButton = new Border();
        private Border _minButton = new Border();
        private Border _maxButton = new Border();
        private Border _clientArea = new Border();
        private bool _dragging = false;

        private Overlay _titleOverlay = new Overlay();
        private StackPanel _horizBorderStacker = new StackPanel();

        private Border _leftBorder = new Border();
        private Border _rightBorder = new Border();

        private Border _blBorder = new Border();
        private Border _brBorder = new Border();
        private Border _bottomBorder = new Border();

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

        private bool _closeDown = false;
        private bool _closeOver = false;
        private bool _maxDown = false;
        private bool _maxOver = false;
        private bool _minDown = false;
        private bool _minOver = false;

        public Window()
        {
            if (ActiveDecorator == null)
                LoadDecorator<ThemeWindowDecorator>();

            _rootBorder.Content = _mainStacker;
            _mainStacker.Children.Add(_titleOverlay);
            _mainStacker.Children.Add(_horizBorderStacker);
            _mainStacker.Children.Add(_bottomStacker);

            _bottomStacker.Orientation = Orientation.Horizontal;

            _bottomStacker.Children.Add(_blBorder);
            _bottomStacker.Children.Add(_bottomBorder);
            _bottomStacker.Children.Add(_brBorder);

            _bottomStacker.SetSizeMode(_bottomBorder, SizeMode.Fill);

            _clientArea.MinWidth = 100;
            _clientArea.MinHeight = 100;

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

            _titleText.VerticalAlignment = VerticalAlignment.Middle;

            _titleStacker.VerticalAlignment = VerticalAlignment.Middle;
        }

        private void UnbindEvents()
        {
            _maxButton.Click -= MaximizeHandler;
            _closeButton.Click -= HandleClose;
            _minButton.Click -= HGandleMinimize;

            _closeButton.MouseEnter -= HandleTitleButtonEnter;
            _closeButton.MouseLeave -= HandleTitleButtonLeave;
            _closeButton.MouseDown -= HandleTitleButtonDown;
            _closeButton.MouseUp -= HandleTitleButtonUp;

            _maxButton.MouseEnter -= HandleTitleButtonEnter;
            _maxButton.MouseLeave -= HandleTitleButtonLeave;
            _maxButton.MouseDown -= HandleTitleButtonDown;
            _maxButton.MouseUp -= HandleTitleButtonUp;

            _minButton.MouseEnter -= HandleTitleButtonEnter;
            _minButton.MouseLeave -= HandleTitleButtonLeave;
            _minButton.MouseDown -= HandleTitleButtonDown;
            _minButton.MouseUp -= HandleTitleButtonUp;

            _titleOverlay.MouseDown -= StartDrag;
            _titleOverlay.MouseUp -= EndDrag;

            // GUI system can't do this due to reasons.  Why? BEcause reasons.  Is it your fault?
            // No, chill.  Otherwise I'll have to cancle you.  Fuck I need to stop quoting that guy...
            GameLoop.Instance.Input.MouseMove -= Drag;
        }

        private void BindEvents()
        {
            _maxButton.Click += MaximizeHandler;
            _closeButton.Click += HandleClose;
            _minButton.Click += HGandleMinimize;

            _closeButton.MouseEnter += HandleTitleButtonEnter;
            _closeButton.MouseLeave += HandleTitleButtonLeave;
            _closeButton.MouseDown += HandleTitleButtonDown;
            _closeButton.MouseUp += HandleTitleButtonUp;

            _maxButton.MouseEnter += HandleTitleButtonEnter;
            _maxButton.MouseLeave += HandleTitleButtonLeave;
            _maxButton.MouseDown += HandleTitleButtonDown;
            _maxButton.MouseUp += HandleTitleButtonUp;

            _minButton.MouseEnter += HandleTitleButtonEnter;
            _minButton.MouseLeave += HandleTitleButtonLeave;
            _minButton.MouseDown += HandleTitleButtonDown;
            _minButton.MouseUp += HandleTitleButtonUp;

            _titleOverlay.MouseDown += StartDrag;
            _titleOverlay.MouseUp += EndDrag;

            // GUI system can't do this due to reasons.  Why? BEcause reasons.  Is it your fault?
            // No, chill.  Otherwise I'll have to cancle you.  Fuck I need to stop quoting that guy...
            GameLoop.Instance.Input.MouseMove += Drag;
        }

        private void HandleTitleButtonUp(object sender, Input.MouseButtonEventArgs e)
        {
            if (sender == _closeButton) _closeDown = false;
            if (sender == _maxButton) _maxDown = false;
            if (sender == _minButton) _minDown = false;
        }

        private void HandleTitleButtonDown(object sender, Input.MouseButtonEventArgs e)
        {
            if (sender == _closeButton) _closeDown = true;
            if (sender == _maxButton) _maxDown = true;
            if (sender == _minButton) _minDown = true;
        }

        private void HandleTitleButtonLeave(object sender, Input.MouseMoveEventArgs e)
        {
            if (sender == _closeButton) _closeOver = false;
            if (sender == _maxButton) _maxOver = false;
            if (sender == _minButton) _minOver = false;
        }

        private void HandleTitleButtonEnter(object sender, Input.MouseMoveEventArgs e)
        {
            if (sender == _closeButton) _closeOver = true;
            if (sender == _maxButton) _maxOver = true;
            if (sender == _minButton) _minOver = true;
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
                GameLoop.Instance.ScreenManager.SetFocus(this.WindowBorder);
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

                SetFrameSizes();
                SetColors(IsActive ? ActiveDecorator.ActiveFrameColor : ActiveDecorator.InactiveFrameColor);

                _rootBorder.Brush = Brush.None;
            }
        }

        private void SetFrameSizes()
        {
            _leftBorder.Width = ActiveDecorator.FrameSize.Left;
            _rightBorder.Width = ActiveDecorator.FrameSize.Right;
            _bottomBorder.Height = ActiveDecorator.FrameSize.Bottom;

            _blBorder.Width = _leftBorder.Width;
            _brBorder.Width = _rightBorder.Width;
            _blBorder.Height = _bottomBorder.Height;
            _brBorder.Height = _bottomBorder.Height;

            _titleBackground.Height = ActiveDecorator.FrameSize.Height;
        }

        public Control FindControl(int x, int y)
        {
            return WindowState == WindowState.Minimized ? null : WindowBorder.FindControl(x, y);
        }

        private void LoadContent()
        {
            _closeButton.VerticalAlignment = VerticalAlignment.Middle;
            _maxButton.VerticalAlignment = VerticalAlignment.Middle;
            _minButton.VerticalAlignment = VerticalAlignment.Middle;

            _horizBorderStacker.Orientation = Orientation.Horizontal;
        }

        private void SetColors(Color color)
        {
            _trCorner.Tint = color;
            _tlCorner.Tint = color;
            _titleLeft.Tint = color;
            _titleRight.Tint = color;

            _leftBorder.Brush = ActiveDecorator.LeftBrush.TintWith(color);
            _rightBorder.Brush = ActiveDecorator.RightBrush.TintWith(color);
            _bottomBorder.Brush = ActiveDecorator.BottomtBrush.TintWith(color);
            _blBorder.Brush = ActiveDecorator.BottomLeftBrush.TintWith(color);
            _brBorder.Brush = ActiveDecorator.BottomRightBrush.TintWith(color);

            _clientArea.Brush = ActiveDecorator.ClientBackground;

            _titleBackground.Brush = ActiveDecorator.TitleBrush.TintWith(color);

            _titleText.Font = ActiveDecorator.TitleFont;

            switch(ActiveDecorator.TitleTextLocation)
            {
                case TitleTextLocation.Left:
                    _titleText.HorizontalAlignment = HorizontalAlignment.Left;
                    break;
                case TitleTextLocation.Center:
                    _titleText.HorizontalAlignment = HorizontalAlignment.Center;
                    break;
                case TitleTextLocation.Right:
                    _titleText.HorizontalAlignment = HorizontalAlignment.Right;
                    break;
            }

            _titleButtons.Spacing = ActiveDecorator.TitleButtonSpacing;

            _closeButton.Width = (int)ActiveDecorator.CloseButtonSize.X;
            _closeButton.Height = (int)ActiveDecorator.CloseButtonSize.Y;

            _maxButton.Width = (int)ActiveDecorator.MaximizeButtonSize.X;
            _maxButton.Height = (int)ActiveDecorator.MaximizeButtonSize.Y;

            _minButton.Width = (int)ActiveDecorator.MinimizeButtonSize.X;
            _minButton.Height = (int)ActiveDecorator.MinimizeButtonSize.Y;

            if (_closeDown)
                _closeButton.Brush = ActiveDecorator.ClosePressedBrush;
            else if (_closeOver)
                _closeButton.Brush = ActiveDecorator.CloseHoverBrush;
            else
                _closeButton.Brush = ActiveDecorator.CloseButtonBrush;

            if (WindowState == WindowState.Maximized)
            {
                if (_maxDown)
                    _maxButton.Brush = ActiveDecorator.RestorePressedBrush;
                else if (_maxOver)
                    _maxButton.Brush = ActiveDecorator.RestoreHoverBrush;
                else
                    _maxButton.Brush = ActiveDecorator.RestoreButtonBrush;
            }
            else
            {
                if (_maxDown)
                    _maxButton.Brush = ActiveDecorator.MaximizePressedBrush;
                else if (_maxOver)
                    _maxButton.Brush = ActiveDecorator.MaximizeHoverBrush;
                else
                    _maxButton.Brush = ActiveDecorator.MaximizeButtonBrush;
            }

            if (_minDown)
                _minButton.Brush = ActiveDecorator.MinimizePressedBrush;
            else if (_minOver)
                _minButton.Brush = ActiveDecorator.MinimizeHoverBrush;
            else
                _minButton.Brush = ActiveDecorator.MinimizeButtonBrush;

            if(IsActive)
            {
                _titleText.TextColor = ActiveDecorator.ActiveTitleTextColor;
            }
            else
            {
                _titleText.TextColor = ActiveDecorator.InactiveTitleTextColor;
            }
        }

        public void Draw(GameTime gameTime, Renderer renderer)
        {
            if (WindowState != WindowState.Minimized)
            {
                WindowBorder.Draw(gameTime, renderer);
            }
        }
    }
}
