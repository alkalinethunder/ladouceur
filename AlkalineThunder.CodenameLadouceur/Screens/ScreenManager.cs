using AlkalineThunder.Nucleus.Gui;
using AlkalineThunder.Nucleus.Rendering;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AlkalineThunder.Nucleus.Screens
{
    public sealed class ScreenManager : GameComponent
    {
        private List<Screen> _screens = new List<Screen>();
        private Control _preFocus = null;
        
        public Control HoveredControl { get; private set; }
        public static Control FocusedControl { get; private set; }
        public Screen ActiveScreen => _screens.LastOrDefault();

        public ScreenManager(GameLoop game) : base(game)
        {
        }

        public override void Initialize()
        {
            var input = (Game as GameLoop).Input;

            input.KeyDown += HandleKeyDown;
            input.KeyUp += HandleKeyUp;
            input.TextInput += HandleTextInput;

            input.MouseDown += HandleMouseDown;
            input.MouseUp += HandleMouseUp;
            input.MouseMove += HandleMouseMove;
            input.MouseScroll += HandleMouseScroll;

            base.Initialize();
        }

        private void HandleMouseScroll(object sender, Input.MouseScrollEventArgs e)
        {
            if(HoveredControl != null)
            {
                Propagate(HoveredControl, x => x.FireMouseScroll(e));
            }
        }

        public T AddScreen<T>() where T : Screen, new()
        {
            var newScreen = new T();
            _screens.Add(newScreen);

            newScreen.LoadContent(this.Game.Content);
            newScreen.Initialize(this);

            return newScreen;
        }

        public void RemoveScreen(Screen screen)
        {
            if(this._screens.Contains(screen))
            {
                this._screens.Remove(screen);
            }
        }

        private Control FindControl(int x, int y)
        {
            if(ActiveScreen != null)
            {
                for(int i = ActiveScreen.Windows.Count - 1; i >= 0; i--)
                {
                    var win = ActiveScreen.Windows[i];

                    var winControl = win.FindControl(x, y);

                    if(winControl != null)
                    {
                        return winControl;
                    }
                }

                return ActiveScreen.FindControl(x, y);
            }
            else
            {
                return null;
            }
        }

        private void HandleMouseMove(object sender, Input.MouseMoveEventArgs e)
        {
            var hovered = FindControl(e.X, e.Y);

            if(hovered != HoveredControl)
            {
                // I need to fucking piss before I do this.
                if(HoveredControl != null)
                {
                    if(hovered == null || !hovered.HasParent(HoveredControl))
                    {
                        Propagate(HoveredControl, x => x.FireMouseLeave(e));
                    }
                }

                HoveredControl = hovered;

                if(HoveredControl != null)
                {
                    Propagate(HoveredControl, x => x.FireMouseEnter(e));
                }
            }

            if(HoveredControl != null)
            {
                Propagate(HoveredControl, x => x.FireMouseMove(e));
            }
        }

        private bool Propagate(Control control, Func<Control, bool> handler)
        {
            while(control != null)
            {
                if (handler(control)) return true;
                control = control.Parent;
            }

            return false;
        }

        public void SetFocus(Control control)
        {
            if(FocusedControl != control)
            {
                var ev = new FocusEventArgs(FocusedControl, control);

                if(FocusedControl != null)
                {
                    Propagate(FocusedControl, x => x.FireLostFocus(ev));
                }

                FocusedControl = control;

                if (FocusedControl != null)
                {
                    Propagate(FocusedControl, x => x.FireGainedFocus(ev));
                }
            }
        }

        private void HandleMouseUp(object sender, Input.MouseButtonEventArgs e)
        {
            if(HoveredControl != null)
            {
                Propagate(HoveredControl, x => x.FireMouseUp(e));

                // Focus is only handled on left-clicks
                if(e.Button == Input.MouseButton.Left)
                {
                    // Are we still hovering over the same control?
                    if(_preFocus == HoveredControl)
                    {
                        Propagate(_preFocus, x => x.FireClick(e));
                        SetFocus(_preFocus);
                    }

                    _preFocus = null;
                }
            }
            else
            {
                SetFocus(null);
            }
        }

        private void HandleMouseDown(object sender, Input.MouseButtonEventArgs e)
        {
            if (HoveredControl != null)
            {
                // Focus is only handled on left-clicks
                if (e.Button == Input.MouseButton.Left)
                {
                    _preFocus = HoveredControl;
                }

                Propagate(HoveredControl, x => x.FireMouseDown(e));
            }
        }

        private void HandleTextInput(object sender, TextInputEventArgs e)
        {
            if(FocusedControl != null)
            {
                FocusedControl.FireTextInput(e);
            }
        }

        private void HandleKeyUp(object sender, InputKeyEventArgs e)
        {
            if (FocusedControl != null)
            {
                FocusedControl.FireKeyUp(e);
            }
        }



        private void HandleKeyDown(object sender, InputKeyEventArgs e)
        {
            if (FocusedControl != null)
            {
                FocusedControl.FireKeyDown(e);
            }
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var screen in _screens)
            {
                Control.PlaceControl(screen, new Rectangle(0, 0, Game.GraphicsDevice.PresentationParameters.BackBufferWidth, Game.GraphicsDevice.PresentationParameters.BackBufferHeight));

                screen.Update(gameTime);

                foreach(var win in screen.Windows.ToArray())
                {
                    win.Update(screen, gameTime);
                }
            }
            base.Update(gameTime);
        }

        public void Draw(GameTime gameTime, Renderer renderer)
        {
            foreach (var screen in _screens)
            {
                screen.Draw(gameTime, renderer);

                foreach(var win in screen.Windows)
                {
                    win.Draw(gameTime, renderer);
                }
            }
        }
    }
}
