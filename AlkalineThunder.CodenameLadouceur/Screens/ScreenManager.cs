using AlkalineThunder.CodenameLadouceur.Gui;
using AlkalineThunder.CodenameLadouceur.Rendering;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AlkalineThunder.CodenameLadouceur.Screens
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
            _screens.Add(new AboutScreen());

            var input = (Game as GameLoop).Input;

            input.KeyDown += HandleKeyDown;
            input.KeyUp += HandleKeyUp;
            input.TextInput += HandleTextInput;

            input.MouseDown += HandleMouseDown;
            input.MouseUp += HandleMouseUp;
            input.MouseMove += HandleMouseMove;

            base.Initialize();
        }

        private Control FindControl(int x, int y)
        {
            if(ActiveScreen != null)
            {
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
                        Propagate(HoveredControl, x => x.MouseLeave(e));
                    }
                }

                HoveredControl = hovered;

                if(HoveredControl != null)
                {
                    Propagate(HoveredControl, x => x.MouseEnter(e));
                }
            }

            if(HoveredControl != null)
            {
                Propagate(HoveredControl, x => x.MouseMove(e));
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
                    Propagate(HoveredControl, x => x.LostFocus(ev));
                }

                FocusedControl = control;

                if (FocusedControl != null)
                {
                    Propagate(HoveredControl, x => x.GainedFocus(ev));
                }
            }
        }

        private void HandleMouseUp(object sender, Input.MouseButtonEventArgs e)
        {
            if(HoveredControl != null)
            {
                Propagate(HoveredControl, x => x.MouseUp(e));

                // Focus is only handled on left-clicks
                if(e.Button == Input.MouseButton.Left)
                {
                    // Are we still hovering over the same control?
                    if(_preFocus == HoveredControl)
                    {
                        if (Propagate(_preFocus, x => x.Click(e)))
                        {
                            SetFocus(_preFocus);
                        }
                    }

                    _preFocus = null;
                }
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

                Propagate(HoveredControl, x => x.MouseDown(e));
            }
        }

        private void HandleTextInput(object sender, TextInputEventArgs e)
        {
            if(FocusedControl != null)
            {
                FocusedControl.TextInput(e);
            }
        }

        private void HandleKeyUp(object sender, InputKeyEventArgs e)
        {
            if (FocusedControl != null)
            {
                FocusedControl.KeyUp(e);
            }
        }

        private void HandleKeyDown(object sender, InputKeyEventArgs e)
        {
            if (FocusedControl != null)
            {
                FocusedControl.KeyDown(e);
            }
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var screen in _screens)
            {
                Control.PlaceControl(screen, new Rectangle(0, 0, Game.GraphicsDevice.PresentationParameters.BackBufferWidth, Game.GraphicsDevice.PresentationParameters.BackBufferHeight));

                screen.Update(gameTime);
            }
            base.Update(gameTime);
        }

        public void Draw(GameTime gameTime, Renderer renderer)
        {
            foreach (var screen in _screens) screen.Draw(gameTime, renderer);
        }
    }
}
