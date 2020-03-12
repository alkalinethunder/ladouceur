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
        public Control FocusedControl { get; private set; }
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

        private void HandleMouseUp(object sender, Input.MouseButtonEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void HandleMouseDown(object sender, Input.MouseButtonEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void HandleTextInput(object sender, TextInputEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void HandleKeyUp(object sender, InputKeyEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void HandleKeyDown(object sender, InputKeyEventArgs e)
        {
            throw new NotImplementedException();
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
