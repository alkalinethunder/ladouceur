using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace AlkalineThunder.CodenameLadouceur
{
    public sealed class InputManager : GameComponent
    {
        private MouseState _lastMouse;
        private MouseState _mouse;


        public InputManager(Game game) : base(game)
        {

        }

        public override void Initialize()
        {
            Logger.Log("Hello world. I see dead people.");

            this.Game.Window.TextInput += HandleTextInput;
            this.Game.Window.KeyUp += HandleKeyUp;
            this.Game.Window.KeyDown += HandleKeyDown;

            _lastMouse = Mouse.GetState(this.Game.Window);

            base.Initialize();
        }

        private void HandleKeyDown(object sender, InputKeyEventArgs e)
        {
            Logger.Log($"{e.Key}", LogLevel.Debug);
        }

        private void HandleKeyUp(object sender, InputKeyEventArgs e)
        {
            Logger.Log($"{e.Key}", LogLevel.Debug);
        }

        private void HandleTextInput(object sender, TextInputEventArgs e)
        {
            Logger.Log(e.Character.ToString(), LogLevel.Debug);
        }

        private void HandleMouseMove(object sender, MouseMoveEventArgs e)
        {
            Logger.Log($"{e.MovementX}, {e.MovementY}", LogLevel.Debug);
        }

        private void HandleMouseDown(object sender, MouseButtonEventArgs e)
        {
            Logger.Log($"{e.Button}", LogLevel.Debug);
        }

        private void HandleMouseUp(object sender, MouseButtonEventArgs e)
        {
            Logger.Log($"{e.Button}", LogLevel.Debug);
        }


        private void DetermineButtonState(ButtonState lasts, ButtonState current, MouseButton button)
        {
            if(lasts != current)
            {
                bool pressed = current == ButtonState.Pressed;

                var e = new MouseButtonEventArgs(_mouse.X, _mouse.Y, button, pressed);

                if(pressed)
                {
                    HandleMouseDown(this, e);
                }
                else
                {
                    HandleMouseUp(this, e);
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (Game.IsActive)
            {
                _mouse = Mouse.GetState(this.Game.Window);

                if (_mouse.X != _lastMouse.X || _mouse.Y != _lastMouse.Y)
                {
                    HandleMouseMove(this, new MouseMoveEventArgs(_mouse.X, _mouse.Y, _mouse.X - _lastMouse.X, _mouse.Y - _lastMouse.Y));
                }

                DetermineButtonState(_lastMouse.LeftButton, _mouse.LeftButton, MouseButton.Left);
                DetermineButtonState(_lastMouse.MiddleButton, _mouse.MiddleButton, MouseButton.Middle);
                DetermineButtonState(_lastMouse.RightButton, _mouse.RightButton, MouseButton.Right);
                DetermineButtonState(_lastMouse.XButton1, _mouse.XButton1, MouseButton.XButton1);
                DetermineButtonState(_lastMouse.XButton2, _mouse.XButton2, MouseButton.XButton2);

                _lastMouse = _mouse;
            }

            base.Update(gameTime);
        }
    }
}
