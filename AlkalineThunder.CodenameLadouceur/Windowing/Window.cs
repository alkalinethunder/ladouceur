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
            if(!_isContentLoaded)
            {
                if(Screen.ContentManager != null)
                {
                    LoadContent();
                    _isContentLoaded = true;
                }
            }

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

        private void LoadContent()
        {
            _closeButton.Image = Screen.ContentManager.Load<Texture2D>("Textures/WindowBorder/Close");
            _maxButton.Image = Screen.ContentManager.Load<Texture2D>("Textures/WindowBorder/Max");
            _minButton.Image = Screen.ContentManager.Load<Texture2D>("Textures/WindowBorder/Min");

            _closeButton.VerticalAlignment = VerticalAlignment.Middle;
            _maxButton.VerticalAlignment = VerticalAlignment.Middle;
            _minButton.VerticalAlignment = VerticalAlignment.Middle;
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
